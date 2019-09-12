#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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
using Maestro.StaticMapPublisher.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;

namespace Maestro.StaticMapPublisher
{
    class BaseOptions
    {
        [Option("max-parallelism", HelpText = "The maximum degree of parallelism")]
        public int? MaxDegreeOfParallelism { get; set; }

        [Option("wait", Default = false)]
        public bool Wait { get; set; }

        public virtual void Validate(TextWriter stdout) { }
    }

    class PublishOptions : BaseOptions, IStaticMapPublishingOptions
    {
        [Option("mapagent", HelpText = "The mapagent endpoint")]
        public string MapAgent { get; set; }

        [Option("output-directory", HelpText = "The path where published content will be output to. Tiles will be output relative to this directory")]
        public string OutputDirectory { get; set; }

        [Option("external-base-layers", HelpText = "Any external base layers to include (in addition to the primary tile sets)")]
        public IEnumerable<ExternalBaseLayer> ExternalBaseLayers { get; set; }

        [Option("viewer-library", Default = ViewerType.OpenLayers, HelpText = "The viewer library to use")]
        public ViewerType Viewer { get; set; }

        [Option("image-tileset", HelpText = "The image tile set definition id. This tile set must be one using the XYZ tile access scheme")]
        public string ImageTileSetDefinition { get; set; }

        [Option("image-tileset-group", HelpText = "The group within the specified image tile set definition to fetch tiles for. If not specified, the first group in the tile set is used")]
        public string ImageTileSetGroup { get; set; }

        [Option("utfgrid-tileset", HelpText = "The utfgird tile set definition id")]
        public string UTFGridTileSetDefinition { get; set; }

        [Option("utfgrid-tileset-group", HelpText = "The group within the specified utfgrid tile set definition to fetch tiles for. If not specified, the first group in the tile set is used")]
        public string UTFGridTileSetGroup { get; set; }

        public OSGeo.MapGuide.ObjectModels.Common.IEnvelope Bounds => ObjectFactory.CreateEnvelope(MinX, MinY, MaxX, MaxY);

        [Option("minx", SetName = "bbox", Required = true)]
        public double MinX { get; set; }

        [Option("miny", SetName = "bbox", Required = true)]
        public double MinY { get; set; }

        [Option("maxx", SetName = "bbox", Required = true)]
        public double MaxX { get; set; }

        [Option("maxy", SetName = "bbox", Required = true)]
        public double MaxY { get; set; }

        [Option("username", Default = "Anonymous")]
        public string Username { get; set; }

        [Option("password")]
        public string Password { get; set; }

        public IServerConnection Connection { get; private set; }

        public override void Validate(TextWriter stdout)
        {
            if (!Utility.InRange(this.MinX, -180, 180))
                throw new Exception("minx not in range of [-180, 180]");
            if (!Utility.InRange(this.MaxX, -180, 180))
                throw new Exception("maxx not in range of [-180, 180]");
            if (!Utility.InRange(this.MinY, -90, 90))
                throw new Exception("miny not in range of [-90, 90]");
            if (!Utility.InRange(this.MaxY, -90, 90))
                throw new Exception("maxy not in range of [-90, 90]");

            if (this.MinX > this.MaxX)
                throw new Exception("Invalid BBOX: minx > maxx");
            if (this.MinY > this.MaxY)
                throw new Exception("Invalid BBOX: miny > maxy");

            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Url"] = this.MapAgent; //NOXLATE
            builder["Username"] = this.Username; //NOXLATE
            builder["Password"] = this.Password; //NOXLATE
            builder["Locale"] = "en"; //NOXLATE
            builder["AllowUntestedVersion"] = true; //NOXLATE

            string agent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //NOXLATE

            var conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString()); //NOXLATE
            conn.SetCustomProperty("UserAgent", agent); //NOXLATE

            // Must be MapGuide Open Source 4.0
            if (conn.SiteVersion < new Version(4, 0))
            {
                throw new Exception($"This tool requires capabilities present in MapGuide Open Source 4.0 and newer. Version {conn.SiteVersion} is not supported");
            }

            if (!string.IsNullOrEmpty(this.ImageTileSetDefinition))
            {
                var resId = new ResourceIdentifier(this.ImageTileSetDefinition);
                if (resId.ResourceType != ResourceTypes.TileSetDefinition.ToString())
                {
                    throw new Exception("Image tile set definiiton is not a tile set definition resource id");
                }

                var tsd = conn.ResourceService.GetResource(this.ImageTileSetDefinition) as ITileSetDefinition;
                if (tsd == null)
                    throw new Exception("Not an image tile set definition resource");

                if (tsd.TileStoreParameters.TileProvider != "XYZ")
                {
                    throw new Exception("Not a XYZ image tile set definition resource");
                }

                if (string.IsNullOrEmpty(this.ImageTileSetGroup))
                {
                    this.ImageTileSetGroup = tsd.BaseMapLayerGroups.First().Name;
                    stdout.WriteLine($"Defaulting to layer group ({this.ImageTileSetGroup}) for image tileset");
                }
                else
                {
                    if (!tsd.GroupExists(this.ImageTileSetGroup))
                        throw new Exception($"The specified group ({this.ImageTileSetGroup}) does not exist in the specified image tile set definition");
                }
            }
            if (!string.IsNullOrEmpty(this.UTFGridTileSetDefinition))
            {
                var resId = new ResourceIdentifier(this.UTFGridTileSetDefinition);
                if (resId.ResourceType != ResourceTypes.TileSetDefinition.ToString())
                {
                    throw new Exception("UTFGrid tile set definiiton is not a tile set definition resource id");
                }

                var tsd = conn.ResourceService.GetResource(this.UTFGridTileSetDefinition) as ITileSetDefinition;
                if (tsd == null)
                    throw new Exception("Not an UTFGrid tile set definition resource");

                if (tsd.TileStoreParameters.TileProvider != "XYZ")
                {
                    throw new Exception("Not a XYZ UTFGrid tile set definition resource");
                }

                if (string.IsNullOrEmpty(this.UTFGridTileSetGroup))
                {
                    this.UTFGridTileSetGroup = tsd.BaseMapLayerGroups.First().Name;
                    stdout.WriteLine($"Defaulting to layer group ({this.UTFGridTileSetGroup}) for utfgrid tileset");
                }
                else
                {
                    if (!tsd.GroupExists(this.UTFGridTileSetGroup))
                        throw new Exception($"The specified group ({this.UTFGridTileSetGroup}) does not exist in the specified utfgrid tile set definition");
                }
            }

            Connection = conn;
        }
    }


    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var result = Parser
                .Default
                .ParseArguments<PublishOptions>(args)
                .MapResult(opts => Run(opts), _ => Task.FromResult(1));
            var retCode = await result;
            return retCode;
        }

        static async Task<int> Run(object arg)
        {
            try
            {
                switch (arg)
                {
                    case PublishOptions pubOpts:
                        {
                            var stdout = Console.Out;
                            pubOpts.Validate(stdout);

                            var pub = new Maestro.StaticMapPublisher.Common.StaticMapPublisher(stdout);
                            //var ret = await pub.PublishAsync(pubOpts);
                            var ret = 0;

                            // Generate index.html
                            var vm = new MapViewerModel
                            {
                                Title = "Published Map",
                                UTFGridRelPath = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.UTFGridTileSetDefinition),
                                XYZImageRelPath = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.ImageTileSetDefinition)
                            };
                            string template = File.ReadAllText("viewer_content/viewer.cshtml");
                            var result =
                                Engine.Razor.RunCompile(template, "templateKey", null, vm);

                            var outputHtmlPath = Path.Combine(pubOpts.OutputDirectory, "index.html");
                            File.WriteAllText(outputHtmlPath, result);
                            stdout.WriteLine($"Written: {outputHtmlPath}");

                            // Copy assets
                            var assetsDir = Path.Combine(pubOpts.OutputDirectory, "assets");
                            if (!Directory.Exists(assetsDir))
                            {
                                Directory.CreateDirectory(assetsDir);
                            }
                            var files = Directory.GetFiles("viewer_content/assets", "*");
                            foreach (var f in files)
                            {
                                var fileName = Path.GetFileName(f);
                                var targetFileName = Path.Combine(assetsDir, fileName);
                                File.Copy(f, targetFileName, true);
                                stdout.WriteLine($"Copied to assets: {targetFileName}");
                            }

                            return ret;
                        }
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
    }
}
