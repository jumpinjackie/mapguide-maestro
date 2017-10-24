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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MgTileSeeder
{
    class CommonSeederOptions
    {
        // Making these virtual so that we can override and tack on [Option] in subclasses

        public virtual double MinX { get; set; }

        public virtual double MinY { get; set; }

        public virtual double MaxX { get; set; }

        public virtual double MaxY { get; set; }

        [Option("wait", Default = false)]
        public bool Wait { get; set; }

        public virtual void Validate() { }
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

        public override void Validate()
        {
            
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

        [Option("meters-per-unit", HelpText = "The meters per unit value. If connecting to a 2.6 or higher server and tiling a map definition, this value can be inferred automatically")]
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

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default
                  .ParseArguments<MgTileSeederOptions, XYZSeederOptions>(args)
                  .MapResult(opts => Run(opts), _ => 1);
        }

        class ConsoleProgress : IProgress<TileProgress>
        {
            public void Report(TileProgress value)
            {
                Console.WriteLine($"Rendered {value.Rendered} of {value.Total} tiles [{((double)value.Rendered / (double)value.Total):P}]");
            }
        }

        static int Run(object arg)
        {
            switch (arg)
            {
                case MgTileSeederOptions mgOpts:
                    return RunMapGuide(mgOpts);
                case XYZSeederOptions xyzOpts:
                    return RunXYZ(xyzOpts);
                default:
                    throw new ArgumentException();
            }
        }

        static int RunXYZ(XYZSeederOptions options)
        {
            options.Validate();

            var xyz = new XYZTileService(options.UrlTemplate);
            var walker = new XYZTileWalker(options.MinX, options.MinY, options.MaxX, options.MaxY);

            var seederOptions = new TileSeederOptions();
            var seeder = new TileSeeder(xyz, walker, seederOptions);

            var progress = new ConsoleProgress();
            var stats = seeder.Run(progress);

            Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            if (options.Wait)
            {
                Console.WriteLine("Press any key to continue");
                Console.Read();
            }

            return 0;
        }

        static int RunMapGuide(MgTileSeederOptions options)
        {
            options.Validate();

            var conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", 
                "Url", options.MapAgentUri, 
                "Username", options.Username, 
                "Password", options.Password);

            var tileSvc = (ITileService)conn.GetService((int)ServiceType.Tile);
            var res = conn.ResourceService.GetResource(options.ResourceID);
            TileWalkOptions walkOptions = null;
            switch (res)
            {
                case IMapDefinition mdf:
                    walkOptions = new TileWalkOptions(mdf, options.Groups.ToArray());
                    //TODO: If meters-per-unit not specified and this is >= 2.6 or higher, use
                    //CREATERUNTIMEMAP to get this value
                    break;
                case ITileSetDefinition tsd:
                    walkOptions = new TileWalkOptions(tsd, options.Groups.ToArray());
                    break;
                default:
                    throw new ArgumentException("Invalid resource type");
            }

            walkOptions.MetersPerUnit = options.MetersPerUnit;
            var walker = new DefaultTileWalker(walkOptions);

            var seederOptions = new TileSeederOptions();
            var seeder = new TileSeeder(tileSvc, walker, seederOptions);

            var progress = new ConsoleProgress();
            var stats = seeder.Run(progress);

            Console.WriteLine($"Rendered {stats.TilesRendered} tiles in {stats.Duration}");
            if (options.Wait)
            {
                Console.WriteLine("Press any key to continue");
                Console.Read();
            }

            return 0;
        }
    }
}
