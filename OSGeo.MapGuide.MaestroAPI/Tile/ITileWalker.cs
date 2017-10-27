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

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// Defines a request for a single tile
    /// </summary>
    public struct TileRef
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="scale"></param>
        public TileRef(string groupName, int row, int col, int scale)
        {
            this.GroupName = groupName;
            this.Row = row;
            this.Col = col;
            this.Scale = scale;
        }

        /// <summary>
        /// The base layer group
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// The row
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// The column
        /// </summary>
        public int Col { get; }
        
        /// <summary>
        /// The scale index
        /// </summary>
        public int Scale { get; }
    }

    /// <summary>
    /// Defines a strategy for walking through all the possible tiles for a given tiled map or tile set
    /// </summary>
    public interface ITileWalker
    {
        /// <summary>
        /// The resource id of the map definition or tile set
        /// </summary>
        string ResourceID { get; }

        /// <summary>
        /// Computes the list of all possible tiles to request
        /// </summary>
        /// <returns></returns>
        TileRef[] GetTileList();
    }
}
