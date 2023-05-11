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

using CommandLine;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.ObjectPool;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MgTileSeeder
{
    class BaseOptions
    {
        [Option("max-parallelism", HelpText = "The maximum degree of parallelism")]
        public int? MaxDegreeOfParallelism { get; set; }

        [Option("wait", Default = false)]
        public bool Wait { get; set; }

        public virtual void Validate() { }
    }

    class CommonSeederOptions : BaseOptions
    {
        // Making these virtual so that we can override and tack on [Option] in subclasses

        public virtual double MinX { get; set; }

        public virtual double MinY { get; set; }

        public virtual double MaxX { get; set; }

        public virtual double MaxY { get; set; }

        [Option("failed-requests", HelpText = "Path to file where failed requests will be logged to")]
        public string FailedRequestsFile { get; set; }
    }

    class BaseReplayOptions : BaseOptions
    {
        [Option("tile-list", Required = true, HelpText = "Path to file containing logged tiles")]
        public string TileListFile { get; set; }

        [Option("failed-requests", HelpText = "Path to file where failed requests will be logged to")]
        public string FailedRequestsFile { get; set; }
    }

    [Verb("xyz_replay", HelpText = "XYZ replay options")]
    class XYZReplayOptions : BaseReplayOptions
    {
        [Option("url", Required = true, HelpText = "The URL of the XYZ tile source. It must have {x}, {y} and {z} placeholders")]
        public string UrlTemplate { get; set; }
    }

    [Verb("mapguide_replay", HelpText = "MapGuide replay options")]
    class MgReplayOptions : BaseReplayOptions
    {
        [Option('m', "mapagent", Required = true, HelpText = "The mapagent endpoint URL")]
        public string MapAgentUri { get; set; }

        [Option("map", Required = true, HelpText = "The resource id of the tiled map definition or tile set definition to seed")]
        public string ResourceID { get; set; }

        [Option('u', "username", Default = "Anonymous", HelpText = "The MapGuide username")]
        public string Username { get; set; }

        [Option("password", Default = "", HelpText = "The password of the specified MapGuide user")]
        public string Password { get; set; }
    }

    enum XYZOutputType
    {
        Directory,
        MBTiles
    }

    [Verb("xyz", HelpText = "XYZ tiling options")]
    class XYZSeederOptions : CommonSeederOptions
    {
        [Option("url", Required = true, HelpText = "The URL of the XYZ tile source. It must have {x}, {y} and {z} placeholders")]
        public string UrlTemplate { get; set; }

        [Option("minx", SetName = "bbox", Required = true)]
        public override double MinX { get; set; }

        [Option("miny", SetName = "bbox", Required = true)]
        public override double MinY { get; set; }

        [Option("maxx", SetName = "bbox", Required = true)]
        public override double MaxX { get; set; }

        [Option("maxy", SetName = "bbox", Required = true)]
        public override double MaxY { get; set; }

        [Option("specific-zoom-levels", Required = false)]
        public IEnumerable<int> SpecificZoomLevels { get; set; }

        [Option("max-zoom-level", HelpText = "The custom maximum zoom level. The default is 19")]
        public int? MaxZoomLevel { get; set; }

        [Option("output-path", HelpText = "The path to output generated tiles to. Can be either a directory or a path to a MBTiles database to be created")]
        public string OutputPath { get; set; }

        [Option("output-type", HelpText = "The type of the output path")]
        public XYZOutputType OutputType { get; set; }

        public override void Validate()
        {
            // Snap to within domain
            if (!Utility.InRange(this.MinX, -180, 180))
                this.MinX = Math.Max(this.MinX, -180);
            if (!Utility.InRange(this.MaxX, -180, 180))
                this.MaxX = Math.Min(this.MaxX, 180);
            if (!Utility.InRange(this.MinY, -90, 90))
                this.MinY = Math.Max(this.MinY, -90);
            if (!Utility.InRange(this.MaxY, -90, 90))
                this.MaxY = Math.Min(this.MaxY, 90);

            if (this.MinX > this.MaxX)
                throw new Exception("Invalid BBOX: minx > maxx");
            if (this.MinY > this.MaxY)
                throw new Exception("Invalid BBOX: miny > maxy");
        }
    }

    [Verb("mapguide", HelpText = "MapGuide tiling options")]
    class MgTileSeederOptions : CommonSeederOptions
    {
        [Option('m', "mapagent", Required = true, HelpText = "The mapagent endpoint URL")]
        public string MapAgentUri { get; set; }

        [Option("map", Required = true, HelpText = "The resource id of the tiled map definition or tile set definition to seed")]
        public string ResourceID { get; set; }

        [Option("groups", HelpText = "A list of base layer groups to seed. If not specified, all base layer groups of the specified map are seeded")]
        public IEnumerable<string> Groups { get; set; }

        [Option('u', "username", Default = "Anonymous", HelpText = "The MapGuide username")]
        public string Username { get; set; }

        [Option("password", Default = "", HelpText = "The password of the specified MapGuide user")]
        public string Password { get; set; }

        [Option("meters-per-unit", HelpText = "The meters per unit value. If connecting to a 2.6 or higher server and tiling a map definition, this value does not have to be specified as we can infer it from a CREATERUNTIMEMAP operation")]
        public double MetersPerUnit { get; set; }

        [Option("minx", SetName = "bbox")]
        public override double MinX { get; set; }

        [Option("miny", SetName = "bbox")]
        public override double MinY { get; set; }

        [Option("maxx", SetName = "bbox")]
        public override double MaxX { get; set; }

        [Option("maxy", SetName = "bbox")]
        public override double MaxY { get; set; }
    }

    class TileListWalker : ITileWalker
    {
        readonly string _file;

        public TileListWalker(string file)
        {
            _file = file;
        }

        public string ResourceID { get; set; }

        public TileRef[] GetTileList()
        {
            var tiles = new List<TileRef>();
            using (var sr = new StreamReader(_file))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    var t = TileRef.Parse(line);
                    if (t.HasValue)
                    {
                        tiles.Add(t.Value);
                    }
                    line = sr.ReadLine();
                }
            }
            return tiles.ToArray();
        }
    }

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var result = Parser.Default
                  .ParseArguments<MgTileSeederOptions, XYZSeederOptions, MgReplayOptions, XYZReplayOptions>(args)
                  .MapResult(opts => RunAsync(opts), _ => Task.FromResult(1));

            var retCode = await result;
            return retCode;
        }

        class ConsoleProgress : IProgress<TileProgress>
        {
            public void Report(TileProgress value)
            {
                var processed = value.Rendered + value.Failed;
                Console.WriteLine($"Processed ({processed}/{value.Total}) tiles [{((double)processed / (double)value.Total):P}] - {value.Rendered} rendered, {value.Failed} failed");
            }
        }

        static readonly object _errorLoggerLock = new object();

        static StreamWriter _output;

        static void ErrorLogger(TileRef tile, Exception ex)
        {
            if (_output != null)
            {
                lock (_errorLoggerLock)
                {
                    _output.WriteLine(tile.Serialize());
                }
            }
        }

        static async Task<int> RunAsync(object arg)
        {
            try
            {
                switch (arg)
                {
                    case MgReplayOptions mgReplay:
                        {
                            if (!string.IsNullOrEmpty(mgReplay.FailedRequestsFile))
                                _output = new StreamWriter(mgReplay.FailedRequestsFile);
                            int ret = await ReplayMapGuideAsync(mgReplay);
                            Environment.ExitCode = ret;
                            return ret;
                        }
                    case MgTileSeederOptions mgOpts:
                        {
                            if (!string.IsNullOrEmpty(mgOpts.FailedRequestsFile))
                                _output = new StreamWriter(mgOpts.FailedRequestsFile);
                            int ret = await RunMapGuideAsync(mgOpts);
                            Environment.ExitCode = ret;
                            return ret;
                        }
                    case XYZReplayOptions xyzReplay:
                        {
                            if (!string.IsNullOrEmpty(xyzReplay.FailedRequestsFile))
                                _output = new StreamWriter(xyzReplay.FailedRequestsFile);
                            int ret = await ReplayXYZAsync(xyzReplay);
                            Environment.ExitCode = ret;
                            return ret;
                        }
                    case XYZSeederOptions xyzOpts:
                        {
                            if (!string.IsNullOrEmpty(xyzOpts.FailedRequestsFile))
                                _output = new StreamWriter(xyzOpts.FailedRequestsFile);
                            int ret = await RunXYZAsync(xyzOpts);
                            Environment.ExitCode = ret;
                            return ret;
                        }
                    default:
                        throw new ArgumentException();
                }
            }
            finally
            {
                _output?.Close();
                _output?.Dispose();
            }
        }

        static async Task<int> ReplayXYZAsync(XYZReplayOptions options)
        {
            int ret = 0;
            try
            {
                options.Validate();

                var xyz = new XYZTileService(options.UrlTemplate);
                var walker = new TileListWalker(options.TileListFile);

                var seederOptions = new TileSeederOptions();
                seederOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;
                seederOptions.ErrorLogger = ErrorLogger;
                var seeder = new TileSeeder(xyz, walker, seederOptions);

                var progress = new ConsoleProgress();
                var stats = await seeder.RunAsync(progress);

                Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ret = 1;
            }
            finally
            {
                if (options.Wait)
                {
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
            return ret;
        }

        static async Task<int> RunXYZAsync(XYZSeederOptions options)
        {
            int ret = 0;
            try
            {
                options.Validate();

                var xyz = new XYZTileService(options.UrlTemplate);
                var walker = new XYZTileWalker(options.MinX, options.MinY, options.MaxX, options.MaxY, options.MaxZoomLevel ?? XYZTileWalker.DEFAULT_MAX_ZOOM_LEVEL);

                if (options.SpecificZoomLevels != null)
                    walker.SetSpecificZoomLevels(options.SpecificZoomLevels);

                var seederOptions = new TileSeederOptions();
                seederOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;
                seederOptions.ErrorLogger = ErrorLogger;

                if (!string.IsNullOrEmpty(options.OutputPath))
                {
                    switch (options.OutputType)
                    {
                        case XYZOutputType.Directory:
                            {
                                var imgDir = options.OutputPath;
                                if (!Directory.Exists(imgDir))
                                {
                                    Directory.CreateDirectory(imgDir);
                                }
                                seederOptions.SaveTile = (tr, stream) =>
                                {
                                    var tilePath = Path.Combine(imgDir, tr.GroupName, $"{tr.Scale}" /* z */, $"{tr.Row}"  /* x */, $"{tr.Col}.png" /* y */);
                                    var parentDir = Path.GetDirectoryName(tilePath);
                                    if (!Directory.Exists(parentDir))
                                        Directory.CreateDirectory(parentDir);
                                    using (var fw = File.OpenWrite(tilePath))
                                    {
                                        Utility.CopyStream(stream, fw);
                                    }
                                };
                            }
                            break;
                        case XYZOutputType.MBTiles:
                            {
                                await SetupMBTilesAsync(options);
                                seederOptions.SaveTile = (tr, stream) =>
                                {
                                    WriteMBTile(options, tr, stream);
                                };
                            }
                            break;
                    }
                }

                var seeder = new TileSeeder(xyz, walker, seederOptions);

                var progress = new ConsoleProgress();
                var stats = await seeder.RunAsync(progress);

                Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ret = 1;
            }
            finally
            {
                if (options.Wait)
                {
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
            return ret;
        }

        class SqliteConnectionPolicy : PooledObjectPolicy<SqliteConnection>
        {
            public override SqliteConnection Create()
            {
                return new SqliteConnection();
            }

            public override bool Return(SqliteConnection obj)
            {
                obj.Close();
                return true;
            }
        }

        static ObjectPool<SqliteConnection> smConnectionPool = new DefaultObjectPool<SqliteConnection>(new SqliteConnectionPolicy());

        static void WriteMBTile(XYZSeederOptions options, TileRef tr, Stream stream)
        {
            var conn = smConnectionPool.Get();
            conn.ConnectionString = $"Data Source={options.OutputPath}";
            conn.Open();

            try
            {
                using var ms = MemoryStreamPool.GetStream();
                stream.CopyTo(ms);
                ms.Position = 0L;
                conn.Execute($"INSERT OR REPLACE INTO tiles (zoom_level, tile_column, tile_row, tile_data) VALUES (@z, @y, @x, @img)", param: new
                {
                    z = tr.Scale,
                    x = tr.Row,
                    y = tr.Col,
                    img = ms.GetBuffer()
                });
            }
            finally
            {
                smConnectionPool.Return(conn);
            }
        }

        static async Task SetupMBTilesAsync(XYZSeederOptions options)
        {
            var tsName = "MBTileSet";
            var imgFormat = "png";
            using (var conn = new SqliteConnection())
            {
                conn.ConnectionString = $"Data Source={options.OutputPath}";
                conn.Open();
                /*
                https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md#schema

                The database MUST contain a table or view named metadata.

                This table or view MUST yield exactly two columns of type text, named name and value. A typical create statement for the metadata table:
                 */
                await conn.ExecuteAsync("CREATE TABLE metadata (name text, value text);");
                /*
                https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md#content

                The metadata table is used as a key/value store for settings. It MUST contain these two rows:

                name (string): The human-readable name of the tileset.
                format (string): The file format of the tile data: pbf, jpg, png, webp, or an IETF media type for other formats.
                 */
                await conn.ExecuteAsync($"INSERT INTO metadata (name, value) VALUES ('name', '{tsName}');");
                await conn.ExecuteAsync($"INSERT INTO metadata (name, value) VALUES ('format', '{imgFormat}');");
                /*
                The metadata table SHOULD contain these four rows:

                bounds (string of comma-separated numbers): The maximum extent of the rendered map area. Bounds must define an area covered by all zoom levels. The bounds are represented as WGS 84 latitude and longitude values, in the OpenLayers Bounds format (left, bottom, right, top). For example, the bounds of the full Earth, minus the poles, would be: -180.0,-85,180,85.
                center (string of comma-separated numbers): The longitude, latitude, and zoom level of the default view of the map. Example: -122.1906,37.7599,11
                minzoom (number): The lowest zoom level for which the tileset provides data
                maxzoom (number): The highest zoom level for which the tileset provides data
                 */
                await conn.ExecuteAsync($"INSERT INTO metadata (name, value) VALUES ('bounds', '{options.MinX},{options.MinY},{options.MaxX},{options.MaxY}');");

                /*
                https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md#schema-1

                The database MUST contain a table named tiles.

                The table MUST contain three columns of type integer, named zoom_level, tile_column, tile_row, and one of type blob, named tile_data. A typical create statement for the tiles table:
                 */
                await conn.ExecuteAsync($"CREATE TABLE tiles (zoom_level integer, tile_column integer, tile_row integer, tile_data blob);");
                //The database MAY contain an index for efficient access to this table:
                await conn.ExecuteAsync($"CREATE UNIQUE INDEX tile_index on tiles (zoom_level, tile_column, tile_row);");
            }
        }

        static async Task<int> ReplayMapGuideAsync(MgReplayOptions options)
        {
            int ret = 0;
            try
            {
                options.Validate();

                var conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                    "Url", options.MapAgentUri,
                    "Username", options.Username,
                    "Password", options.Password);

                var tileSvc = (ITileService)conn.GetService((int)ServiceType.Tile);

                var walker = new TileListWalker(options.TileListFile);
                walker.ResourceID = options.ResourceID;

                var seederOptions = new TileSeederOptions();
                seederOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;
                seederOptions.ErrorLogger = ErrorLogger;
                var seeder = new TileSeeder(tileSvc, walker, seederOptions);

                var progress = new ConsoleProgress();
                var stats = await seeder.RunAsync(progress);

                Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ret = 1;
            }
            finally
            {
                if (options.Wait)
                {
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
            return ret;
        }

        static async Task<int> RunMapGuideAsync(MgTileSeederOptions options)
        {
            int ret = 0;
            try
            { 
                options.Validate();

                var conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", 
                    "Url", options.MapAgentUri, 
                    "Username", options.Username, 
                    "Password", options.Password);

                var tileSvc = (ITileService)conn.GetService((int)ServiceType.Tile);
                var res = conn.ResourceService.GetResource(options.ResourceID);
                DefaultTileWalkOptions walkOptions = null;
                switch (res)
                {
                    case IMapDefinition mdf:
                        walkOptions = new DefaultTileWalkOptions(mdf, options.Groups.ToArray());
                        //If meters-per-unit not specified and this is >= 2.6 or higher, we can use
                        //CREATERUNTIMEMAP to get this value
                        if (options.MetersPerUnit == default(double))
                        {
                            if (conn.SiteVersion >= new Version(2, 6))
                            {
                                Console.WriteLine("Using CREATERUNTIMEMAP to obtain required meters-per-unit value");
                                var createRt = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
                                createRt.MapDefinition = options.ResourceID;
                                createRt.RequestedFeatures = (int)RuntimeMapRequestedFeatures.None;
                                var rtMapInfo = createRt.Execute();
                                options.MetersPerUnit = rtMapInfo.CoordinateSystem.MetersPerUnit;
                                Console.WriteLine($"Using meters-per-unit value of: {options.MetersPerUnit}");
                            }
                        }
                        break;
                    case ITileSetDefinition tsd:
                        walkOptions = new DefaultTileWalkOptions(tsd, options.Groups.ToArray());

                        //Wrong options. Fortunately we have enough information here to tell them what the *correct*
                        //arguments are
                        if (tsd.TileStoreParameters.TileProvider == "XYZ")
                        {
                            var bbox = tsd.Extents;
                            var urls = new List<string>();
                            foreach (var grp in tsd.BaseMapLayerGroups)
                            {
                                urls.Add(options.MapAgentUri + "?OPERATION=GETTILEIMAGE&VERSION=1.2.0&USERNAME=Anonymous&MAPDEFINITION=" + tsd.ResourceID + "&BASEMAPLAYERGROUPNAME=" + grp.Name + "&TILECOL={y}&TILEROW={x}&SCALEINDEX={z}");
                            }

                            Console.WriteLine("[ERROR]: Cannot use mapguide tiling mode for seeding XYZ tile sets. Use xyz tiling mode instead. Example(s):");
                            foreach (var url in urls)
                            {
                                Console.WriteLine($"  MgTileSeeder xyz --url \"{url}\" --minx {bbox.MinX} --miny {bbox.MinY} --maxx {bbox.MaxX} --maxy {bbox.MaxY}");
                            }
                            if (options.Wait)
                            {
                                Console.WriteLine("Press any key to continue");
                                Console.Read();
                            }
                            return 1;
                        }

                        //If meters-per-unit not specified and the tile set is using the "Default" provider, we can create
                        //a Map Definition linked to the tile set, save it to a temp resource and call CREATERUNTIMEMAP
                        //from it to obtain the reuqired meters-per-unit value
                        if (options.MetersPerUnit == default(double) && tsd.TileStoreParameters.TileProvider == "Default")
                        {
                            IMapDefinition3 mdf3 = (IMapDefinition3)ObjectFactory.CreateMapDefinition(new Version(3, 0, 0), "LinkedTileSet");
                            string tmpId = $"Session:{conn.SessionID}//{mdf3.Name}.MapDefinition";
                            var text = tsd.Extents;
                            mdf3.SetExtents(text.MinX, text.MinY, text.MaxX, text.MaxY);
                            mdf3.CoordinateSystem = tsd.GetDefaultCoordinateSystem();
                            mdf3.TileSetDefinitionID = tsd.ResourceID;
                            conn.ResourceService.SaveResourceAs(mdf3, tmpId);

                            Console.WriteLine("Using CREATERUNTIMEMAP to obtain required meters-per-unit value");
                            var createRt = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
                            createRt.MapDefinition = tmpId;
                            createRt.RequestedFeatures = (int)RuntimeMapRequestedFeatures.None;
                            var rtMapInfo = createRt.Execute();
                            options.MetersPerUnit = rtMapInfo.CoordinateSystem.MetersPerUnit;
                            Console.WriteLine($"Using meters-per-unit value of: {options.MetersPerUnit}");
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid resource type");
                }

                walkOptions.MetersPerUnit = options.MetersPerUnit;
                var walker = new DefaultTileWalker(walkOptions);

                var seederOptions = new TileSeederOptions();
                seederOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;
                seederOptions.ErrorLogger = ErrorLogger;
                var seeder = new TileSeeder(tileSvc, walker, seederOptions);

                var progress = new ConsoleProgress();
                var stats = await seeder.RunAsync(progress);

                Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ret = 1;
            }
            finally
            {
                if (options.Wait)
                {
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
            return ret;
        }
    }
}
