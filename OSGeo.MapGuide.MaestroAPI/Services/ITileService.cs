#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for map tile generation
    /// </summary>
    /// <example>
    /// This example shows how to obtain a tile service instance. Note that you should check if this service type is
    /// supported through its capabilities.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// ITileService tileSvc = (ITileService)conn.GetService((int)ServiceType.Tile);
    /// ]]>
    /// </code>
    /// </example>
    public interface ITileService : IService
    {
        /// <summary>
        /// Reads a tile from MapGuide
        /// </summary>
        /// <param name="mapDefinition">A map with baselayergroups</param>
        /// <param name="baseLayerGroup">The name of a baselayergroup</param>
        /// <param name="column">The column index of the tile</param>
        /// <param name="row">The row index of the tile</param>
        /// <param name="scaleIndex">The scale index for the tile set</param>
        /// <param name="format">The format to return the tile in, either JPG, GIF, PNG or PNG8</param>
        /// <returns>An image stream</returns>
        System.IO.Stream GetTile(string mapDefinition, string baseLayerGroup, int column, int row, int scaleIndex, string format);
    }
}
