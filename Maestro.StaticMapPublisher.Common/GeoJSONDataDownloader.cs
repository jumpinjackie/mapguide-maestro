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


using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.StaticMapPublisher.Common
{
    public readonly struct DownloadedFeaturesRef
    {
        public DownloadedFeaturesRef(string name, string relPath, string varName)
        {
            this.Name = name;
            this.ScriptRelPath = relPath;
            this.GlobalVar = varName;
        }

        public string Name { get; }

        public string ScriptRelPath { get; }

        public string GlobalVar { get; }
    }

    public class GeoJSONDataDownloader
    {
        static HttpClient httpClient = new HttpClient();

        readonly IStaticMapPublishingOptions _options;
        readonly string _outputCsCode;

        public GeoJSONDataDownloader(IStaticMapPublishingOptions options, string outputCsCode = "LL84")
        {
            _options = options;
            _outputCsCode = outputCsCode;
        }

        public Task<DownloadedFeaturesRef> DownloadAsync(int layerNumber, GeoJSONFromMapGuideOverlayLayer layer)
        {
            switch (layer.Source.Origin)
            {
                //case GeoJSONFromMapGuideOrigin.FeatureSource:
                //    return DownloadFromFeatureSourceAsync(layerNumber, layer.Name, (GeoJSONFromFeatureSource)layer.Source);
                case GeoJSONFromMapGuideOrigin.LayerDefinition:
                    return DownloadFromLayerDefinitionAsync(layerNumber, layer.Name, (GeoJSONFromLayerDefinition)layer.Source);
            }
            throw new ArgumentOutOfRangeException("Unknown origin");
        }

        public static string GetFileName(int layerNumber)
        {
            return $"vector_layer_{layerNumber}";
        }

        public static string GetVariableName(int layerNumber)
        {
            return $"vector_features_{layerNumber}";
        }

        private async Task<DownloadedFeaturesRef> DownloadFeatureDataAsync(IVectorLayerDefinition vl, int layerNumber, string name, Stream stream)
        {
            var filePath = Path.GetFullPath(Path.Combine(_options.OutputDirectory, "vector_overlays", $"{GetFileName(layerNumber)}.js"));
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var sw = new StreamWriter(filePath))
            {
                await sw.WriteLineAsync($"//Vector overlay configuration for: {name}");
                if (vl.PropertyMapping.Any())
                {
                    await WritePopupConfigurationAsync(vl, layerNumber, name, sw);
                }
                await WriteStyleConfigurationAsync(vl, layerNumber, name, sw);
                await sw.WriteLineAsync($"//Downloaded features for: {name}");
                await sw.WriteAsync($"var {GetVariableName(layerNumber)} = ");
            }

            using (var fw = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                await stream.CopyToAsync(fw);
            }

            return new DownloadedFeaturesRef(name, $"vector_overlays/{GetFileName(layerNumber)}.js", GetVariableName(layerNumber));
        }

        

        private async Task WritePopupConfigurationAsync(IVectorLayerDefinition vl, int layerNumber, string name, StreamWriter sw)
        {
            await sw.WriteLineAsync($"//{_options.Viewer} popup template configuration for: {name}");
            await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_popup_template = {{");
            var lines = new List<string>();
            switch (_options.Viewer)
            {
                case ViewerType.Leaflet:
                    {

                    }
                    break;
                case ViewerType.OpenLayers:
                    {
                        foreach (var pm in vl.PropertyMapping)
                        {
                            lines.Add($"'{pm.Name}': {{ title: '{pm.Value}' }}");
                        }
                    }
                    break;
            }
            await sw.WriteLineAsync("    " + string.Join(",\n    ", lines));
            await sw.WriteLineAsync("}");
        }

        private async Task WriteStyleConfigurationAsync(IVectorLayerDefinition vl, int layerNumber, string name, StreamWriter sw)
        {
            await sw.WriteLineAsync($"//Style configuration for: {name}");
            var vsr = vl.VectorScaleRange.FirstOrDefault();

            //No vector scale range = nothing to translate.
            if (vsr == null)
            {
                await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style = null; //Could not determine or translate from source style. Use OL default");
                return;
            }

            //Can't translate from advanced stylization
            if (vsr is IVectorScaleRange2 vsr2 && vsr2.CompositeStyle.Any())
            {
                await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style = null; //Could not determine or translate from source style. Use OL default");
                return;
            }

            if (_options.Viewer == ViewerType.OpenLayers)
            {
                var olx = new OLStyleTranslator("feature");
                await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style = function ({olx.FeatureVariableName}, resolution) {{");
                if (vsr.AreaStyle != null)
                {
                    await olx.WritePolygonStyleFunctionAsync(sw, vl, vsr.AreaStyle);
                }
                else if (vsr.LineStyle != null)
                {
                    await olx.WriteLineStyleFunctionAsync(sw, vl, vsr.LineStyle);
                }
                else if (vsr.PointStyle != null)
                {
                    await olx.WritePointStyleFunctionAsync(sw, vl, vsr.PointStyle);
                }
                await sw.WriteLineAsync("}");
            }  
            else if (_options.Viewer == ViewerType.Leaflet)
            {
                var lst = new LeafletStyleTranslator("feature");
                await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style = function ({lst.FeatureVariableName}) {{");
                if (vsr.AreaStyle != null)
                {
                    await lst.WritePolygonStyleFunctionAsync(sw, vl, vsr.AreaStyle);
                }
                else if (vsr.LineStyle != null)
                {
                    await lst.WriteLineStyleFunctionAsync(sw, vl, vsr.LineStyle);
                }
                else if (vsr.PointStyle != null)
                {
                    await lst.WritePointStyleFunctionAsync(sw, vl, vsr.PointStyle);
                }
                await sw.WriteLineAsync("}");
                //TODO: This should be offloaded to LeafletStyleTranslator
                await sw.WriteLineAsync("//Point feature function");
                if (vsr.PointStyle != null && vsr.PointStyle.RuleCount > 0)
                {
                    var pr = vsr.PointStyle.Rules.First();
                    if (pr.PointSymbolization2D != null && pr.PointSymbolization2D.Symbol is IMarkSymbol msm)
                    {
                        await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style_point = function(point, latlng) {{;");
                        switch (msm.Shape)
                        {
                            case ShapeType.Circle:
                                await sw.WriteLineAsync("    return L.circleMarker(latlng);");
                                break;
                            default:
                                await sw.WriteLineAsync("    return L.marker(latlng);");
                                break;
                        }
                        await sw.WriteLineAsync("};");
                    }
                    else
                    {
                        await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style_point = null;");
                    }
                }
                else
                {
                    await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_style_point = null;");
                }
            }
        }

        private string BuildSelectFeaturesUrl(string featureSource, string className, string filter = null)
        {
            var reqUrl = $"{_options.MapAgent}?OPERATION=SELECTFEATURES&VERSION=4.0.0&FORMAT=application/json&CLEAN=1";
            reqUrl += "&CLIENTAGENT=Maestro.StaticMapPublisher";
            reqUrl += $"&RESOURCEID={featureSource}&CLASSNAME={className}";
            reqUrl += $"&TRANSFORMTO={_outputCsCode}";
            reqUrl += $"&USERNAME={_options.Username ?? "Anonymous"}";
            if (!string.IsNullOrEmpty(_options.Password))
                reqUrl += $"&PASSWORD={_options.Password}";
            if (!string.IsNullOrEmpty(filter))
                reqUrl += $"&FILTER={filter}";

            return reqUrl;
        }

        private string GetResourceContentUrl(string resourceId)
        {
            var reqUrl = $"{_options.MapAgent}?OPERATION=GETRESOURCECONTENT&VERSION=1.0.0&FORMAT=text/xml";
            reqUrl += "&CLIENTAGENT=Maestro.StaticMapPublisher";
            reqUrl += $"&RESOURCEID={resourceId}";
            reqUrl += $"&USERNAME={_options.Username ?? "Anonymous"}";
            if (!string.IsNullOrEmpty(_options.Password))
                reqUrl += $"&PASSWORD={_options.Password}";

            return reqUrl;
        }

        private async Task<DownloadedFeaturesRef> DownloadFromLayerDefinitionAsync(int layerNumber, string name, GeoJSONFromLayerDefinition source)
        {
            var grcUrl = GetResourceContentUrl(source.LayerDefinition);
            var resp = await httpClient.GetAsync(grcUrl);
            if (!resp.IsSuccessStatusCode)
            {
                var respContent = await resp.Content.ReadAsStringAsync();
                resp.EnsureSuccessStatusCode();
            }
            var grcStream = await resp.Content.ReadAsStreamAsync();

            var ldf = ObjectFactory.Deserialize(nameof(ResourceTypes.LayerDefinition), grcStream) as ILayerDefinition;
            if (ldf == null)
                throw new Exception("Not a layer definition");

            if (ldf.SubLayer.LayerType != LayerType.Vector)
                throw new Exception("Not a vector layer definition");

            var vl = ldf.SubLayer as IVectorLayerDefinition;
            if (vl == null)
                throw new Exception("Not a vector layer definition");

            var url = BuildSelectFeaturesUrl(vl.ResourceId, vl.FeatureName, vl.Filter);
            resp = await httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
            {
                var respContent = await resp.Content.ReadAsStringAsync();
                resp.EnsureSuccessStatusCode();
            }

            using (var stream = await resp.Content.ReadAsStreamAsync())
            {
                return await DownloadFeatureDataAsync(vl, layerNumber, name, stream);
            }
        }
        /*
        private async Task<DownloadedFeaturesRef> DownloadFromFeatureSourceAsync(int layerNumber, string name, GeoJSONFromFeatureSource source)
        {
            var url = BuildSelectFeaturesUrl(source.FeatureSource, source.ClassName, source.Filter);
            var resp = await httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            using (var stream = await resp.Content.ReadAsStreamAsync())
            {
                return await DownloadFeatureDataAsync(layerNumber, name, stream);
            }
        }
        */
    }
}
