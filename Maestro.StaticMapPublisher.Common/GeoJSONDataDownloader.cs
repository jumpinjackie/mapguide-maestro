using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.IO;
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

        private async Task<DownloadedFeaturesRef> DownloadFeatureDataAsync(int layerNumber, string name, Stream stream)
        {
            var filePath = Path.GetFullPath(Path.Combine(_options.OutputDirectory, "vector_overlays", $"{GetFileName(layerNumber)}.js"));
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var sw = new StreamWriter(filePath))
            {
                await sw.WriteLineAsync($"//Downloaded features for: {name}");
                await sw.WriteAsync($"var {GetVariableName(layerNumber)} = ");
            }

            using (var fw = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                await stream.CopyToAsync(fw);
            }

            return new DownloadedFeaturesRef(name, $"vector_overlays/{GetFileName(layerNumber)}.js", GetVariableName(layerNumber));
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
                return await DownloadFeatureDataAsync(layerNumber, name, stream);
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
