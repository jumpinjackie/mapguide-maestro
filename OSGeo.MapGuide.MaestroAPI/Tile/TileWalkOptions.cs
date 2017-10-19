#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System.Linq;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// Defines options that control how the tile set is to be walked to obtain the full list of tiles to be requested
    /// </summary>
    public class TileWalkOptions
    {
        /// <summary>
        /// Constructs a new instance from a Map Definition
        /// </summary>
        /// <param name="mdf"></param>
        /// <param name="groupNames"></param>
        public TileWalkOptions(IMapDefinition mdf, string[] groupNames = null)
        {
            this.ResourceID = mdf.ResourceID;
            this.TileSet = mdf.BaseMap;
            this.GroupNames = groupNames ?? mdf.BaseMap.BaseMapLayerGroups.Select(g => g.Name).ToArray();
            this.Extents = ObjectFactory.CreateEnvelope(mdf.Extents.MinX, mdf.Extents.MinY, mdf.Extents.MaxX, mdf.Extents.MaxY);
            this.DPI = 96;
            this.TileWidth = 300;
            this.TileHeight = 300;
        }

        /// <summary>
        /// Constructs a new instance from a Tile Set Definition
        /// </summary>
        /// <param name="tsd"></param>
        /// <param name="groupNames"></param>
        public TileWalkOptions(ITileSetDefinition tsd, string[] groupNames = null)
        {
            this.ResourceID = tsd.ResourceID;
            this.TileSet = tsd;
            this.GroupNames = groupNames ?? tsd.BaseMapLayerGroups.Select(g => g.Name).ToArray();
            this.Extents = ObjectFactory.CreateEnvelope(tsd.Extents.MinX, tsd.Extents.MinY, tsd.Extents.MaxX, tsd.Extents.MaxY);

            this.DPI = 96;
            this.TileWidth = tsd.GetDefaultTileWidth() ?? 300;
            this.TileHeight = tsd.GetDefaultTileHeight() ?? 300;
        }

        public string ResourceID { get; }

        public ITileSetAbstract TileSet { get; }

        public string[] GroupNames { get; }

        public IEnvelope Extents { get; }

        public IEnvelope OverrideExtents { get; set; }

        public double MetersPerUnit { get; set; }

        public int DPI { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }
    }
}
