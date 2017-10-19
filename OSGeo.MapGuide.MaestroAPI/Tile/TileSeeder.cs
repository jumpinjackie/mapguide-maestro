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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// Defines options for controlling the tile seeding process
    /// </summary>
    public class TileSeederOptions
    {

    }

    public struct TileProgress
    {
        public TileProgress(int rendered, int total)
        {
            this.Rendered = rendered;
            this.Total = total;
        }

        public int Rendered { get; }

        public int Total { get; }
    }

    /// <summary>
    /// Populates a tile cache by requesting all possible tiles for a given in map in a multi-threaded manner
    /// 
    /// This is the successor to the <see cref="TilingRunCollection"/> with a simpler API design and implementation
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
    public class TileSeeder
    {
        readonly ITileService _tileSvc;
        readonly ITileWalker _walker;
        readonly TileSeederOptions _options;

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

        /// <summary>
        /// Populates the tile cache for the configured tiled map definition or tile set
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public TileSeedStats Run(IProgress<TileProgress> progress = null)
        {
            var rendered = 0;
            var sw = new Stopwatch();
            sw.Start();

            var resId = _walker.ResourceID;
            var tiles = _walker.GetTileList();
            var total = tiles.Length;
            var interval = 1000; //Every second

            using (new Timer(_ => progress?.Report(new TileProgress(rendered, total)), null, interval, interval))
            {
                // And here's our multi-threaded tile seeding. One big Parallel.ForEach loop!
                // Simple isn't it?
                Parallel.ForEach(tiles, tile =>
                {
                    using (_tileSvc.GetTile(resId, tile.GroupName, tile.Col, tile.Row, tile.Scale))
                    {
                        Interlocked.Increment(ref rendered);
                    }
                });
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
