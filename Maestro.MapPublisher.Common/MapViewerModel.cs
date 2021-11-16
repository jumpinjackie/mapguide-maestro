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

using System.Collections.Generic;
using System.Linq;

namespace Maestro.MapPublisher.Common
{
    public class MapViewerModel
    {
        public string Title { get; set; }

        public string XYZImageUrl { get; set; }

        public string UTFGridUrl { get; set; }

        public double[] LatLngBounds { get; set; }

        public string MapAgent { get; set; }

        public ViewerOptionsBase ViewerOptions { get; set; }

        public dynamic Meta { get; set; }

        public bool HasExternalBaseLayer(ExternalBaseLayerType type)
            => ExternalBaseLayers.Any(ebl => ebl.Type == type);

        public IEnumerable<ExternalBaseLayer> ExternalBaseLayers { get; set; }

        public bool HasOverlayLayer(OverlayLayerType type)
            => OverlayLayers.Any(ovl => ovl.Type == type);

        public IEnumerable<OverlayLayer> OverlayLayers { get; set; }
    }
}