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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maestro.StaticMapPublisher.Common
{
    public interface IStaticMapPublishingOptions
    {
        string Title { get; }

        int? MaxDegreeOfParallelism { get; }

        string MapAgent { get; }

        string OutputDirectory { get; }

        IEnumerable<ExternalBaseLayer> ExternalBaseLayers { get; }

        IEnumerable<OverlayLayer> OverlayLayers { get; }

        ViewerType Viewer { get; }

        TileSetRef ImageTileSet { get; }

        TileSetRef UTFGridTileSet { get; }

        IEnvelope Bounds { get; }

        string Username { get; }

        string Password { get; }

        IServerConnection Connection { get; }

        bool RandomizeRequests { get; }
    }

    public enum ExternalBaseLayerType
    {
        OSM,
        Stamen,
        BingMaps,
        XYZ
    }

    public enum ViewerType
    {
        OpenLayers,
        Leaflet
    }
}
