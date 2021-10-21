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
using Maestro.MapPublisher.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Json;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.MapPublisher
{
    [Verb("generate-schema")]
    public class GenerateSchemaOptions
    {
        [Option("output-dir")]
        public string OutputDir { get; set; }
    }

    [Verb("publish")]
    public class PublishOptions
    {
        [Option("wait", Default = false)]
        public bool Wait { get; set; }

        [Option("publish-profile-path", Required = true, HelpText = "The path of the publish profile")]
        public string PublishProfilePath { get; set; }

        public IStaticMapPublishingOptions PublishingOptions { get; set; }

        public void Validate(TextWriter stdout)
        {
            if (!File.Exists(this.PublishProfilePath))
                throw new Exception("Specified publish profile not found");

            stdout.WriteLine($"Loading publishing profile from: {this.PublishProfilePath}");

            var content = File.ReadAllText(this.PublishProfilePath);
            var pp = JsonConvert.DeserializeObject<PublishProfile>(content);

            pp.Validate(stdout);
            this.PublishingOptions = pp;
        }
    }


    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var result = Parser
                .Default
                .ParseArguments<PublishOptions, GenerateSchemaOptions>(args)
                .MapResult(opts => Run(opts), _ => Task.FromResult(1));
            var retCode = await result;
            return retCode;
        }

        static async Task<int> Run(object arg)
        {
            var stdout = Console.Out;
            try
            {
                switch (arg)
                {
                    case GenerateSchemaOptions go:
                        {
                            var schemaGen = new JSchemaGenerator();
                            var schema = schemaGen.Generate(typeof(PublishProfile));
                            await File.WriteAllTextAsync(Path.Combine(go.OutputDir, "PublishProfile.schema.json"), schema.ToString());
                            return 0;
                        }
                    case PublishOptions po:
                        {
                            po.Validate(stdout);

                            var pubOpts = po.PublishingOptions;
                            var pub = new Maestro.MapPublisher.Common.StaticMapPublisher(stdout);
                            var ret = await pub.PublishAsync(pubOpts);
                            var bounds = pubOpts.Bounds;

                            int counter = 0;
                            // Download any GeoJSON sources
                            foreach (var source in pubOpts.OverlayLayers)
                            {
                                if (source.Type == OverlayLayerType.GeoJSON_FromMapGuide)
                                {
                                    var mgSource = ((GeoJSONFromMapGuideOverlayLayer)source);
                                    await stdout.WriteLineAsync($"Start downloading GeoJSON data for: {source.Name}");
                                    var downloader = new GeoJSONDataDownloader(pubOpts);
                                    mgSource.Downloaded = await downloader.DownloadAsync(counter, mgSource);
                                    await stdout.WriteLineAsync($"GeoJSON data for ({source.Name}) downloaded to: {mgSource.Downloaded.DataScriptRelPath}");
                                }
                                counter++;
                            }

                            // Generate index.html
                            var vm = new MapViewerModel
                            {
                                Title = pubOpts.Title,
                                MapAgent = pubOpts.MapAgent.Endpoint,
                                ViewerOptions = pubOpts.ViewerOptions,
                                LatLngBounds = new [] { bounds.MinX, bounds.MinY, bounds.MaxX, bounds.MaxY },
                                ExternalBaseLayers = pubOpts.ExternalBaseLayers,
                                OverlayLayers = pubOpts.OverlayLayers
                            };

                            var agent = pubOpts.Title;
                            if (pubOpts.UTFGridTileSet != null && !string.IsNullOrEmpty(pubOpts.UTFGridTileSet.ResourceID))
                            {
                                if (pubOpts.UTFGridTileSet.Mode == TileSetRefMode.Local)
                                    vm.UTFGridUrl = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.UTFGridTileSet?.ResourceID) + "/{z}/{x}/{y}.json";
                                else if (pubOpts.UTFGridTileSet.Mode == TileSetRefMode.Remote)
                                    vm.UTFGridUrl = $"{pubOpts.MapAgent.Endpoint}?OPERATION=GETTILEIMAGE&VERSION=1.2.0&USERNAME=Anonymous&CLIENTAGENT={agent}&MAPDEFINITION={pubOpts.UTFGridTileSet.ResourceID}&BASEMAPLAYERGROUPNAME={pubOpts.UTFGridTileSet.GroupName}&TILECOL={{y}}&TILEROW={{x}}&SCALEINDEX={{z}}";
                            }
                            if (pubOpts.ImageTileSet != null && !string.IsNullOrEmpty(pubOpts.ImageTileSet.ResourceID))
                            {
                                if (pubOpts.ImageTileSet.Mode == TileSetRefMode.Local)
                                    vm.XYZImageUrl = Common.StaticMapPublisher.GetResourceRelPath(pubOpts, o => o.ImageTileSet?.ResourceID) + "/{z}/{x}/{y}.png";
                                else if (pubOpts.ImageTileSet.Mode == TileSetRefMode.Remote)
                                    vm.XYZImageUrl = $"{pubOpts.MapAgent.Endpoint}?OPERATION=GETTILEIMAGE&VERSION=1.2.0&USERNAME=Anonymous&CLIENTAGENT={agent}&MAPDEFINITION={pubOpts.ImageTileSet.ResourceID}&BASEMAPLAYERGROUPNAME={pubOpts.ImageTileSet.GroupName}&TILECOL={{y}}&TILEROW={{x}}&SCALEINDEX={{z}}";
                            }
                            

                            string result;
                            switch (pubOpts.ViewerOptions.Type)
                            {
                                case ViewerType.OpenLayers:
                                    {
                                        string template = File.ReadAllText("viewer_content/viewer_ol.cshtml");
                                        result = Engine.Razor.RunCompile(template, "templateKey", null, vm);
                                    }
                                    break;
                                case ViewerType.Leaflet:
                                    {
                                        string template = File.ReadAllText("viewer_content/viewer_leaflet.cshtml");
                                        result = Engine.Razor.RunCompile(template, "templateKey", null, vm);
                                    }
                                    break;
                                case ViewerType.MapGuideReactLayout:
                                    {
                                        var appDef = Utility.CreateFlexibleLayout(pubOpts.Connection, ((MapGuideReactLayoutViewerOptions)pubOpts.ViewerOptions).TemplateName);
                                        InitAppDef(appDef, pubOpts);
                                        var json = AppDefJsonSerializer.Serialize(appDef);
                                        string template = File.ReadAllText("viewer_content/viewer_mrl.cshtml");
                                        result = Engine.Razor.RunCompile(template, "templateKey", null, vm);
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("Unknown or unsupported viewer type");
                            }

                            var outputHtmlPath = Path.Combine(pubOpts.OutputDirectory, pubOpts.OutputPageFileName ?? "index.html");
                            File.WriteAllText(outputHtmlPath, result);
                            await stdout.WriteLineAsync($"Written: {outputHtmlPath}");

                            // Copy assets
                            if (pubOpts.ViewerOptions.Type == ViewerType.MapGuideReactLayout)
                            {
                                var assetsDir = pubOpts.OutputDirectory; // Path.Combine(pubOpts.OutputDirectory, "mrl_assets");
                                if (!Directory.Exists(assetsDir))
                                {
                                    Directory.CreateDirectory(assetsDir);
                                }
                                var files = Directory.GetFiles("viewer_content/mrl_assets", "*", SearchOption.AllDirectories);
                                foreach (var f in files)
                                {
                                    var fileName = f.Substring("viewer_content/mrl_assets".Length).Trim('\\', '/'); //Path.GetFileName(f);
                                    var targetFileName = Path.GetFullPath(Path.Combine(assetsDir, fileName));
                                    var targetParentDir = Path.GetDirectoryName(targetFileName);
                                    if (!Directory.Exists(targetParentDir))
                                        Directory.CreateDirectory(targetParentDir);
                                    File.Copy(f, targetFileName, true);
                                    await stdout.WriteLineAsync($"Copied to assets: {targetFileName}");
                                }
                            }
                            else
                            {
                                var assetsDir = Path.Combine(pubOpts.OutputDirectory, "assets");
                                if (!Directory.Exists(assetsDir))
                                {
                                    Directory.CreateDirectory(assetsDir);
                                }
                                var files = Directory.GetFiles("viewer_content/assets", "*", SearchOption.AllDirectories);
                                foreach (var f in files)
                                {
                                    var fileName = f.Substring("viewer_content/assets".Length).Trim('\\', '/'); //Path.GetFileName(f);
                                    var targetFileName = Path.GetFullPath(Path.Combine(assetsDir, fileName));
                                    var targetParentDir = Path.GetDirectoryName(targetFileName);
                                    if (!Directory.Exists(targetParentDir))
                                        Directory.CreateDirectory(targetParentDir);
                                    File.Copy(f, targetFileName, true);
                                    await stdout.WriteLineAsync($"Copied to assets: {targetFileName}");
                                }
                            }

                            if (po.Wait)
                            {
                                await stdout.WriteLineAsync("Press any key to continue");
                                Console.Read();
                            }

                            return ret;
                        }
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                await stdout.WriteLineAsync($"ERROR: {ex}");
                return 1;
            }
        }

        static void InitAppDef(IApplicationDefinition appDef, IStaticMapPublishingOptions pubOpts)
        {
            appDef.Title = pubOpts.Title;
            //Clear groups
            var toRemove = appDef.MapSet.MapGroups.ToList();
            foreach (var rem in toRemove)
            {
                appDef.MapSet.RemoveGroup(rem);
            }
            //Make our MapGroup
            var mg = appDef.AddMapGroup("MainMap");

            //Add base layers
            foreach (var bl in pubOpts.ExternalBaseLayers)
            {
                var ble = mg.CreateCmsMapEntry(bl.Type.ToString(), false, bl.Name, bl.Type.ToString());
                if (bl.Type == ExternalBaseLayerType.XYZ)
                {
                    ble.SetValue("Foo", "Bar");
                }
                else if (bl.Type == ExternalBaseLayerType.BingMaps)
                {

                }
            }

            //Add UTFGrid layer (if set)
            if (pubOpts.UTFGridTileSet != null)
            {
                
            }

            //Add XYZ layer (if set)
            if (pubOpts.ImageTileSet != null)
            {

            }
        }
    }
}
