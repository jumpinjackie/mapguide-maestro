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
using System.Threading.Tasks;

namespace Maestro.MapPublisher.Common
{
    public readonly struct DownloadedFeaturesRef
    {
        public DownloadedFeaturesRef(string name, string dataRelPath, string configRelPath, string varName)
        {
            this.Name = name;
            this.DataScriptRelPath = dataRelPath;
            this.ConfigScriptRelPath = configRelPath;
            this.GlobalVar = varName;
        }

        public string Name { get; }

        public string DataScriptRelPath { get; }

        public string ConfigScriptRelPath { get; }

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

        private async Task<DownloadedFeaturesRef> DownloadFeatureDataAsync(IVectorLayerDefinition vl, int layerNumber, string name, Stream stream, bool generateVtIndex)
        {
            var dataFilePath = Path.GetFullPath(Path.Combine(_options.OutputDirectory, "vector_overlays", $"{GetFileName(layerNumber)}_data.js"));
            var configFilePath = Path.GetFullPath(Path.Combine(_options.OutputDirectory, "vector_overlays", $"{GetFileName(layerNumber)}_config.js"));
            var dir = Path.GetDirectoryName(dataFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var sw = new StreamWriter(configFilePath))
            {
                await sw.WriteLineAsync($"//Vector overlay configuration for: {name}");
                await WritePopupConfigurationAsync(vl, layerNumber, name, sw);
                await WriteStyleConfigurationAsync(vl, layerNumber, name, sw);

                if (generateVtIndex)
                {
                    await sw.WriteLineAsync($"\n\n//GeoJSON Vector Tile index for: {name}");
                    await sw.WriteLineAsync($@"
var {GetVariableName(layerNumber)}_vtindex = geojsonvt({GetVariableName(layerNumber)}, {{
    extent: 4096,
    maxZoom: 19,
    debug: 1
}});");
                }
            }

            using (var sw = new StreamWriter(dataFilePath))
            {
                await sw.WriteLineAsync($"//GeoJSON vector data for: {name}");
                await sw.WriteLineAsync($"//Downloaded features for: {name}");
                await sw.WriteAsync($"var {GetVariableName(layerNumber)} = ");
            }

            //Download the GeoJSON and append to the JS
            using (var fw = new FileStream(dataFilePath, FileMode.Append, FileAccess.Write))
            {
                await stream.CopyToAsync(fw);
            }

            return new DownloadedFeaturesRef(name, 
                $"vector_overlays/{GetFileName(layerNumber)}_data.js",
                $"vector_overlays/{GetFileName(layerNumber)}_config.js",
                GetVariableName(layerNumber));
        }

        

        private async Task WritePopupConfigurationAsync(IVectorLayerDefinition vl, int layerNumber, string name, StreamWriter sw)
        {
            await sw.WriteLineAsync($"//{_options.ViewerOptions.Type} popup template configuration for: {name}");
            if (vl.PropertyMapping.Any())
            {
                switch (_options.ViewerOptions.Type)
                {
                    case ViewerType.Leaflet:
                        {
                            await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_popup_template = function(feature, layer) {{");
                            await sw.WriteLineAsync("    var html = '<h3>" + name + "</h3><table>';");
                            await sw.WriteLineAsync("    html += '<tbody>';");
                            foreach (var pm in vl.PropertyMapping)
                            {
                                await sw.WriteLineAsync("    html += '<tr>';");
                                await sw.WriteLineAsync("    html += '<td>';");
                                await sw.WriteLineAsync("    html += '" + pm.Value + "';");
                                await sw.WriteLineAsync("    html += '</td>';");
                                await sw.WriteLineAsync("    html += '<td>';");
                                await sw.WriteLineAsync("    html += feature.properties." + pm.Name + " || '';");
                                await sw.WriteLineAsync("    html += '</td>';");
                                await sw.WriteLineAsync("    html += '</tr>';");
                            }
                            await sw.WriteLineAsync("    html += '</tbody>';");
                            await sw.WriteLineAsync("    html += '</table>';");
                            if (!string.IsNullOrEmpty(vl.Url))
                            {
                                try
                                {
                                    //Only supports property references in the URL
                                    var expr = FdoExpression.Parse(vl.Url);
                                    if (expr is FdoIdentifier ident)
                                    {
                                        await sw.WriteLineAsync($"    if (feature.properties.{ident.Name}) {{");
                                        await sw.WriteLineAsync($"        html += \"<hr /><a href='\" + feature.properties.{ident.Name} + \"' target='_blank'>Open Link</a>\"");
                                        await sw.WriteLineAsync ("    }");
                                    }
                                }
                                catch { }
                            }
                            await sw.WriteLineAsync("    layer.bindPopup(html);");
                            await sw.WriteLineAsync("}");
                        }
                        break;
                    case ViewerType.OpenLayers:
                        {
                            await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_popup_template = function (feature) {{");
                            await sw.WriteLineAsync("    var html = '<h3>" + name + "</h3><table>';");
                            await sw.WriteLineAsync("    html += '<tbody>';");
                            foreach (var pm in vl.PropertyMapping)
                            {
                                await sw.WriteLineAsync("    html += '<tr>';");
                                await sw.WriteLineAsync("    html += '<td>';");
                                await sw.WriteLineAsync("    html += '" + pm.Value + "';");
                                await sw.WriteLineAsync("    html += '</td>';");
                                await sw.WriteLineAsync("    html += '<td>';");
                                await sw.WriteLineAsync("    html += feature.get('" + pm.Name + "') || '';");
                                await sw.WriteLineAsync("    html += '</td>';");
                                await sw.WriteLineAsync("    html += '</tr>';");
                            }
                            await sw.WriteLineAsync("    html += '</tbody>';");
                            await sw.WriteLineAsync("    html += '</table>';");
                            if (!string.IsNullOrEmpty(vl.Url))
                            {
                                try
                                {
                                    //Only supports property references in the URL
                                    var expr = FdoExpression.Parse(vl.Url);
                                    if (expr is FdoIdentifier ident)
                                    {
                                        await sw.WriteLineAsync($"    if (feature.get('{ident.Name}')) {{");
                                        await sw.WriteLineAsync($"        html += \"<hr /><a href='\" + feature.get('{ident.Name}') + \"' target='_blank'>Open Link</a>\"");
                                        await sw.WriteLineAsync ("    }");
                                    }
                                }
                                catch { }
                            }
                            await sw.WriteLineAsync("    return html;");
                            await sw.WriteLineAsync("}");
                        }
                        break;
                }
            }
            else
            {
                switch (_options.ViewerOptions.Type)
                {
                    case ViewerType.Leaflet:
                        {
                            await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_popup_template = function(feature, layer) {{ }}");
                        }
                        break;
                    case ViewerType.OpenLayers:
                        {
                            await sw.WriteLineAsync($"var {GetVariableName(layerNumber)}_popup_template = null;");
                        }
                        break;
                }
            }
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

            if (_options.ViewerOptions.Type == ViewerType.OpenLayers)
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
            else if (_options.ViewerOptions.Type == ViewerType.Leaflet)
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
                await lst.WritePointMarker(GetVariableName(layerNumber), vsr, sw);
            }
        }

        private string BuildSelectFeaturesUrl(string featureSource, string className, int? precision = null, string filter = null)
        {
            var reqUrl = $"{_options.MapAgent}?OPERATION=SELECTFEATURES&VERSION=4.0.0&FORMAT=application/json&CLEAN=1";
            reqUrl += "&CLIENTAGENT=Maestro.MapPublisher";
            reqUrl += $"&RESOURCEID={featureSource}&CLASSNAME={className}";
            reqUrl += $"&TRANSFORMTO={_outputCsCode}";
            reqUrl += $"&USERNAME={_options.Username ?? "Anonymous"}";
            if (precision.HasValue)
                reqUrl += $"&PRECISION={precision.Value}";
            if (!string.IsNullOrEmpty(_options.Password))
                reqUrl += $"&PASSWORD={_options.Password}";
            if (!string.IsNullOrEmpty(filter))
                reqUrl += $"&FILTER={filter}";

            return reqUrl;
        }

        private string GetResourceContentUrl(string resourceId)
        {
            var reqUrl = $"{_options.MapAgent}?OPERATION=GETRESOURCECONTENT&VERSION=1.0.0&FORMAT=text/xml";
            reqUrl += "&CLIENTAGENT=Maestro.MapPublisher";
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

            var url = BuildSelectFeaturesUrl(vl.ResourceId, vl.FeatureName, source.Precision, vl.Filter);
            resp = await httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
            {
                var respContent = await resp.Content.ReadAsStringAsync();
                resp.EnsureSuccessStatusCode();
            }

            using (var stream = await resp.Content.ReadAsStreamAsync())
            {
                return await DownloadFeatureDataAsync(vl, layerNumber, name, stream, source.LoadAsVectorTiles);
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
