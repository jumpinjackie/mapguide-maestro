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

using Newtonsoft.Json;
using System;

namespace Maestro.StaticMapPublisher.Common.Serialization
{
    public class ExternalBaseLayerConverter : BaseJsonConverter<ExternalBaseLayer>
    {
        protected override string DiscriminatorPropertyName => nameof(ExternalBaseLayer.Type);

        protected override object ReadJsonForDiscriminatedValue(string discriminator, string json)
        {
            switch (discriminator)
            {
                case nameof(ExternalBaseLayerType.BingMaps):
                    return JsonConvert.DeserializeObject<BingMapsBaseLayer>(json, SpecifiedSubclassConversion);
                case nameof(ExternalBaseLayerType.OSM):
                    return JsonConvert.DeserializeObject<OSMBaseLayer>(json, SpecifiedSubclassConversion);
                case nameof(ExternalBaseLayerType.Stamen):
                    return JsonConvert.DeserializeObject<StamenBaseLayer>(json, SpecifiedSubclassConversion);
                case nameof(ExternalBaseLayerType.XYZ):
                    return JsonConvert.DeserializeObject<XYZBaseLayer>(json, SpecifiedSubclassConversion);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new NotImplementedException();
        }
    }
}
