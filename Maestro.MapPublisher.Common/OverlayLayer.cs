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

using Maestro.StaticMapPublisher.Common.Serialization;
using Newtonsoft.Json;

namespace Maestro.StaticMapPublisher.Common
{
    public enum OverlayLayerType
    {
        WMS,
        WFS,
        GeoJSON_External,
        GeoJSON_FromMapGuide
    }

    [JsonConverter(typeof(OverlayLayerConverter))]
    public abstract class OverlayLayer : INamedLayer
    {
        public abstract OverlayLayerType Type { get; }

        public string Name { get; set; }

        public bool InitiallyVisible { get; set; }
    }

    public class WMSOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.WMS;

        public string Service { get; set; }

        public string Layer { get; set; }

        public bool Tiled { get; set; }
    }

    public class WFSOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.WFS;

        public string Service { get; set; }

        public string FeatureName { get; set; }
    }

    public class GeoJSONExternalOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.GeoJSON_External;

        public string Url { get; set; }
    }

    public enum GeoJSONFromMapGuideOrigin
    {
        //FeatureSource,
        LayerDefinition
    }

    [JsonConverter(typeof(GeoJSONFromMapGuideConverter))]
    public abstract class GeoJSONFromMapGuide
    {
        public abstract GeoJSONFromMapGuideOrigin Origin { get; }
    }

    public class GeoJSONFromLayerDefinition : GeoJSONFromMapGuide
    {
        public override GeoJSONFromMapGuideOrigin Origin => GeoJSONFromMapGuideOrigin.LayerDefinition;

        public string LayerDefinition { get; set; }
    }
    /*
    public class GeoJSONFromFeatureSource : GeoJSONFromMapGuide
    {
        public override GeoJSONFromMapGuideOrigin Origin => GeoJSONFromMapGuideOrigin.FeatureSource;

        public string FeatureSource { get; set; }

        public string ClassName { get; set; }

        public string Filter { get; set; }
    }
    */
    public class GeoJSONFromMapGuideOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.GeoJSON_FromMapGuide;

        public GeoJSONFromMapGuide Source { get; set; }

        /// <summary>
        /// Will be set by the publisher before outputting the HTML template
        /// </summary>
        [JsonIgnore]
        public DownloadedFeaturesRef Downloaded { get; set; }
    }
}
