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
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
                                OverlayLayers = pubOpts.OverlayLayers,
                                Meta = new ExpandoObject()
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
                                        var appDef = Utility.CreateFlexibleLayout(pubOpts.Connection, ((MapGuideReactLayoutViewerOptions)pubOpts.ViewerOptions).TemplateName, true);
                                        InitAppDef(pubOpts.Connection, appDef, pubOpts, vm);
                                        vm.Meta.AppDefJson = AppDefJsonSerializer.Serialize(appDef);
                                        string template = File.ReadAllText("viewer_content/viewer_mrl.cshtml");
                                        var config = new TemplateServiceConfiguration();
                                        config.EncodedStringFactory = new RawStringFactory();
                                        var service = RazorEngineService.Create(config);
                                        result = service.RunCompile(template, "templateKey", null, vm);
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

        static void InitAppDef(IServerConnection conn, IApplicationDefinition appDef, IStaticMapPublishingOptions pubOpts, MapViewerModel vm)
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
                IMap ble;
                if (bl is StamenBaseLayer sbl)
                {
                    ble = mg.CreateCmsMapEntry(sbl.Type.ToString(), false, sbl.Name, sbl.LayerType.ToString().ToLower());
                }
                else
                {
                    ble = mg.CreateCmsMapEntry(bl.Type.ToString(), false, bl.Name, bl.Type.ToString());
                }
                if (bl.Type == ExternalBaseLayerType.XYZ)
                {
                    ble.SetXYZUrls(((XYZBaseLayer)bl).UrlTemplate);
                }
                else if (bl.Type == ExternalBaseLayerType.BingMaps)
                {
                    var bs = (BingMapsBaseLayer)bl;
                    appDef.SetValue("BingMapsKey", bs.ApiKey);
                }
                mg.AddMap(ble);
            }

            //Add UTFGrid layer (if set)
            if (!string.IsNullOrEmpty(vm.UTFGridUrl))
            {
                var ug = mg.CreateUTFGridEntry(vm.UTFGridUrl);
                mg.AddMap(ug);
            }

            //Add XYZ layer (if set)
            if (!string.IsNullOrEmpty(vm.XYZImageUrl))
            {
                var sub = mg.CreateSubjectLayerEntry("XYZ Tile Set" /* TODO: Configurable layer name */, "XYZ");
                dynamic props = new ExpandoObject();
                props.layer_name = "Sheboygan XYZ";
                props.source_type = "XYZ";
                props.source_param_urls = new[]
                {
                    vm.XYZImageUrl
                };

                var llCs = conn.CoordinateSystemCatalog.FindCoordSys("LL84");
                var wmCs = conn.CoordinateSystemCatalog.FindCoordSys("WGS84.PseudoMercator");

                var xform = conn.CoordinateSystemCatalog.CreateTransform(llCs.WKT, wmCs.WKT);
                double llx, lly, urx, ury;
                xform.Transform(pubOpts.Bounds.MinX, pubOpts.Bounds.MinY, out llx, out lly);
                xform.Transform(pubOpts.Bounds.MaxX, pubOpts.Bounds.MaxY, out urx, out ury);

                props.meta_extents = new[] { llx, lly, urx, ury };
                props.meta_projection = "EPSG:3857";

                sub.SetSubjectOrExternalLayerProperties((IDictionary<string, object>)props);
                mg.AddMap(sub);
            }

            foreach (var ov in vm.OverlayLayers)
            {
                string sType = null;
                switch (ov.Type)
                {
                    case OverlayLayerType.GeoJSON_External:
                    case OverlayLayerType.GeoJSON_FromMapGuide:
                        sType = "GeoJSON";
                        break;
                    case OverlayLayerType.WFS:
                        sType = "WFS";
                        break;
                    case OverlayLayerType.WMS:
                        sType = "TileWMS";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var ext = mg.CreateExternalLayerEntry(ov.Name, sType);
                dynamic props = new ExpandoObject();
                props.layer_name = ov.Name;
                props.source_type = sType;
                props.initially_visible = ov.InitiallyVisible;

                switch (ov.Type)
                {
                    case OverlayLayerType.GeoJSON_External:
                        {
                            var gov = ((GeoJSONExternalOverlayLayer)ov);
                            props.source_param_url = gov.Url;
                        }
                        break;
                    case OverlayLayerType.GeoJSON_FromMapGuide:
                        {
                            var gov = ((GeoJSONFromMapGuideOverlayLayer)ov);
                            if (!string.IsNullOrEmpty(gov.Downloaded.GlobalVar))
                            {
                                props.source_param_url = new ExpandoObject();
                                props.source_param_url.var_source = gov.Downloaded.GlobalVar;
                            }
                            else
                            {
                                props.source_param_url = $"";
                            }
                        }
                        break;
                    case OverlayLayerType.WFS:
                        {
                            var wov = ((WFSOverlayLayer)ov);
                            props.source_param_url = $"{wov.Service}?service=WFS&version={wov.WfsVersion ?? "2.0.0"}&request=GetFeature&typenames={wov.FeatureName}&outputFormat=application/json&srsName=EPSG:3857";
                        }
                        break;
                    case OverlayLayerType.WMS:
                        {
                            var wov = ((WMSOverlayLayer)ov);
                            props.source_param_url = wov.Service;
                            props.source_param_params = new ExpandoObject();
                            props.source_param_params.LAYERS = wov.Layer;
                            props.source_param_params.TILED = wov.Tiled ? "1" : "0";
                        }
                        break;
                }

                ext.SetSubjectOrExternalLayerProperties((IDictionary<string, object>)props);

                mg.AddMap(ext);
            }
        }
    }
}
