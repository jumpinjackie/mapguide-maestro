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

using Maestro.MapPublisher.Common.Serialization;
using Newtonsoft.Json;

namespace Maestro.MapPublisher.Common
{
    [JsonConverter(typeof(ExternalBaseLayerConverter))]
    public abstract class ExternalBaseLayer : INamedLayer
    {
        public abstract ExternalBaseLayerType Type { get; }

        public string Name { get; set; }

        public bool Visible { get; set; }
    }

    public class OSMBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.OSM;
    }

    public enum StamenLayerType
    {
        Toner,
        Terrain,
        WaterColor
    }

    public class StamenBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.Stamen;

        public StamenLayerType LayerType { get; set; }
    }

    public class BingMapsBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.BingMaps;

        public string LayerType { get; set; }

        public string ApiKey { get; set; }
    }

    public class XYZBaseLayer : ExternalBaseLayer
    {
        public override ExternalBaseLayerType Type => ExternalBaseLayerType.XYZ;

        public string UrlTemplate { get; set; }
    }
}
