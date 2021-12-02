#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// Defines options for controlling the tile seeding process
    /// </summary>
    public class TileSeederOptions
    {
        /// <summary>
        /// The maximum degree of parallelism
        /// </summary>
        public int? MaxDegreeOfParallelism { get; set; }

        /// <summary>
        /// A custom tile fetcher that is invoked when a tile is about to be fetch for the given <see cref="TileRef"/>
        /// </summary>
        public Action<Action<TileRef>, TileRef> Executor { get; set; }

        /// <summary>
        /// An error handler that is invoked if an exception is thrown fetching the given <see cref="TileRef"/>
        /// </summary>
        public Action<TileRef, Exception> ErrorLogger { get; set; }

        /// <summary>
        /// An optional action to save the specified tile image stream. By default
        /// (if this action isn't specified) the request for the given tile is discarded
        /// as the seeder's purpose is to ensure the tile is generated and cached on
        /// the server side.
        /// </summary>
        public Action<TileRef, Stream> SaveTile { get; set; }
    }

    /// <summary>
    /// A tile seeding progress event
    /// </summary>
    public struct TileProgress
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="rendered"></param>
        /// <param name="total"></param>
        /// <param name="failed"></param>
        public TileProgress(int rendered, int total, int failed)
        {
            this.Rendered = rendered;
            this.Total = total;
            this.Failed = failed;
        }

        /// <summary>
        /// The number of rendered tiles
        /// </summary>
        public int Rendered { get; }

        /// <summary>
        /// The total number of tiles to be rendered
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// The number of failed tile requests
        /// </summary>
        public int Failed { get; }
    }

    /// <summary>
    /// Populates a tile cache by requesting all possible tiles for a given in map in a multi-threaded manner
    /// </summary>
    /// <example>
    /// This example shows how to seed a tile cache
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// ITileService tileSvc = (ITileService)conn.GetService((int)ServiceType.Tile);
    /// IMapDefinition mapDef = (IMapDefinition)conn.ResourceService.GetResource("Library://Path/To/MyTiled.MapDefinition");
    /// TileSeederOptions options = new TileSeederOptions();
    /// TileWalkOptions walkOptions = new TileWalkOptions(mapDef);
    /// ITileWalker walker = new DefaultTileWalker(walkOptions);
    /// TileSeeder seeder = new TileSeeder(tileSvc, walker, options);
    /// TileSeedStats stats = seeder.Run();
    /// ]]>
    /// </code>
    /// </example>
    public class TileSeeder
    {
        readonly ITileService _tileSvc;
        readonly ITileWalker _walker;
        readonly TileSeederOptions _options;

        public bool RandomizeRequests { get; set; }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="tileSvc"></param>
        /// <param name="walker"></param>
        /// <param name="options"></param>
        public TileSeeder(ITileService tileSvc, ITileWalker walker, TileSeederOptions options)
        {
            _tileSvc = tileSvc;
            _walker = walker;
            _options = options;
        }

        static void DefaultExecutor(Action<TileRef> fetcher, TileRef tile) => fetcher(tile);

        static Random rng = new Random();

        static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public async Task<TileSeedStats> RunAsync(IProgress<TileProgress> progress = null)
        {
            var failed = 0;
            var rendered = 0;
            var sw = new Stopwatch();
            sw.Start();

            Action<Action<TileRef>, TileRef> executor = _options.Executor ?? DefaultExecutor;
            var maxThreads = _options.MaxDegreeOfParallelism;
            var resId = _walker.ResourceID;
            var tiles = _walker.GetTileList();
            var total = tiles.Length;
            var interval = 1000; //Every second

            using (new Timer(_ => progress?.Report(new TileProgress(rendered, total, failed)), null, interval, interval))
            {
                Func<TileRef, Task> fetcher = async tile =>
                {
                    try
                    {
                        using (var tileStream = await _tileSvc.GetTileAsync(resId, tile.GroupName, tile.Col, tile.Row, tile.Scale))
                        {
                            _options.SaveTile?.Invoke(tile, tileStream);
                            Interlocked.Increment(ref rendered);
                        }
                    }
                    catch (Exception ex)
                    {
                        _options.ErrorLogger?.Invoke(tile, ex);
                        Interlocked.Increment(ref failed);
                    }
                };

                var parallelism = Environment.ProcessorCount * 2;
                if (maxThreads.HasValue)
                {
                    parallelism = maxThreads.Value;
                }

                var fetchTasks = new List<Task>(parallelism);
                if (this.RandomizeRequests)
                    Shuffle(tiles);

                //For the async version, we will loop through all the tiles
                //we need to fetch and load up an intermediate list of async tasks 
                //(up to the specified parallelism limit) and then await the whole 
                //lot once we hit that limit. Once we await that lot, we clear the
                //intermediate list and fill it up with the next batch. Rinse
                //and repeat until we've gone through all the tiles.
                foreach (var tr in tiles)
                {
                    fetchTasks.Add(fetcher(tr));
                    if (fetchTasks.Count == parallelism)
                    {
                        await Task.WhenAll(fetchTasks);
                        fetchTasks.Clear();
                    }
                }

                if (fetchTasks.Count > 0)
                {
                    await Task.WhenAll(fetchTasks);
                    fetchTasks.Clear();
                }
            }

            sw.Stop();
            return new TileSeedStats
            {
                ResourceID = resId,
                TilesRendered = rendered,
                Duration = sw.Elapsed
            };
        }

        /// <summary>
        /// Populates the tile cache for the configured tiled map definition or tile set
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public TileSeedStats Run(IProgress<TileProgress> progress = null)
        {
            var failed = 0;
            var rendered = 0;
            var sw = new Stopwatch();
            sw.Start();

            Action<Action<TileRef>, TileRef> executor = _options.Executor ?? DefaultExecutor;
            var maxThreads = _options.MaxDegreeOfParallelism;
            var resId = _walker.ResourceID;
            var tiles = _walker.GetTileList();
            var total = tiles.Length;
            var interval = 1000; //Every second

            using (new Timer(_ => progress?.Report(new TileProgress(rendered, total, failed)), null, interval, interval))
            {
                Action<TileRef> fetcher = tile =>
                {
                    try
                    {
                        using (var tileStream = _tileSvc.GetTile(resId, tile.GroupName, tile.Col, tile.Row, tile.Scale))
                        {
                            _options.SaveTile?.Invoke(tile, tileStream);
                            Interlocked.Increment(ref rendered);
                        }
                    }
                    catch (Exception ex)
                    {
                        _options.ErrorLogger?.Invoke(tile, ex);
                        Interlocked.Increment(ref failed);
                    }
                };

                if (this.RandomizeRequests)
                    Shuffle(tiles);

                if (maxThreads.HasValue)
                {
                    var popts = new ParallelOptions();
                    popts.MaxDegreeOfParallelism = maxThreads.Value;
                    Parallel.ForEach(tiles, popts, t => executor(fetcher, t));
                }
                else
                {
                    Parallel.ForEach(tiles, t => executor(fetcher, t));
                }
            }

            // And this method blocks! So if we get to this point, everything has been iterated through and
            // this the tiling run has finished.

            sw.Stop();
            return new TileSeedStats
            {
                ResourceID = resId,
                TilesRendered = rendered,
                Duration = sw.Elapsed
            };
        }
    }

    /// <summary>
    /// Defines the statistics of a tile seeding run
    /// </summary>
    public struct TileSeedStats
    {
        /// <summary>
        /// The resource id of the map definition or tile set being seeded
        /// </summary>
        public string ResourceID { get; set; }

        /// <summary>
        /// The number of tiles rendered
        /// </summary>
        public int TilesRendered { get; set; }

        /// <summary>
        /// The duration of the whole operation
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
