using System;
using System.Collections.Generic;
using System.Text;

namespace Maestro.StaticMapPublisher.Common
{
    public enum OverlayLayerType
    {
        WMS,
        WFS,
        GeoJSON_External,
        GeoJSON_FromMapGuide
    }

    public abstract class OverlayLayer
    {
        public abstract OverlayLayerType Type { get; }
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

        public string Layer { get; set; }
    }

    public class GeoJSONExternalOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.GeoJSON_External;

        public string Url { get; set; }
    }

    public enum GeoJSONFromMapGuideOrigin
    {
        FeatureSource,
        LayerDefinition
    }

    public abstract class GeoJSONFromMapGuide
    {
        public abstract GeoJSONFromMapGuideOrigin Origin { get; }
    }

    public class GeoJSONFromLayerDefinition : GeoJSONFromMapGuide
    {
        public override GeoJSONFromMapGuideOrigin Origin => GeoJSONFromMapGuideOrigin.LayerDefinition;

        public string LayerDefinition { get; set; }
    }

    public class GeoJSONFromFeatureSource : GeoJSONFromMapGuide
    {
        public override GeoJSONFromMapGuideOrigin Origin => GeoJSONFromMapGuideOrigin.FeatureSource;

        public string FeatureSource { get; set; }

        public string ClassName { get; set; }

        public string Filter { get; set; }
    }

    public class GeoJSONFromMapGuideOverlayLayer : OverlayLayer
    {
        public override OverlayLayerType Type => OverlayLayerType.GeoJSON_FromMapGuide;

        public GeoJSONFromMapGuide Source { get; set; }
    }
}
