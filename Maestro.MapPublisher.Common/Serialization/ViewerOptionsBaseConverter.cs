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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace Maestro.MapPublisher.Common.Serialization
{
    public class ViewerOptionsBaseConverter : BaseJsonConverter<ViewerOptionsBase>
    {
        protected override string DiscriminatorPropertyName => nameof(ViewerOptionsBase.Type);

        protected override object ReadJsonForDiscriminatedValue(string discriminator, string json)
        {
            switch (discriminator)
            {
                case nameof(ViewerType.Leaflet):
                    return JsonConvert.DeserializeObject<LeafletViewerOptions>(json, SpecifiedSubclassConversion);
                case nameof(ViewerType.OpenLayers):
                    return JsonConvert.DeserializeObject<OpenLayersViewerOptions>(json, SpecifiedSubclassConversion);
                case nameof(ViewerType.MapGuideReactLayout):
                    return JsonConvert.DeserializeObject<MapGuideReactLayoutViewerOptions>(json, SpecifiedSubclassConversion);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new NotImplementedException();
        }
    }
}
