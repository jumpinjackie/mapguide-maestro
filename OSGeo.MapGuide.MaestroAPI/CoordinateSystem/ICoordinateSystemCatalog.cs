#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// Defines a coordinate system catalog
    /// </summary>
    /// <example>
    /// This example uses the <see cref="T:OSGeo.MapGuide.MaestroAPI.CoordinateSystem.ICoordindateSystemCatalog"/>
    /// to create a simple coordinate system transformation to transform points.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// ICoordinateSystemCatalog csCatalog = conn.CoordinateSystemCatalog;
    /// //Find the source and target coordinate system definitions
    /// CoordinateSystemDefinitionBase srcCs = csCatalog.FindCoordSys("LL84");
    /// CoordinateSystemDefinitionBase dstCs = csCatalog.FindCoordSys("WGS84.PseudoMercator");
    /// //Create the transform using the WKTs of the source and target coordinate system definitions
    /// ISimpleTransform trans = csCatalog.CreateTransform(srcCs.WKT, dstCs.WKT);
    /// 
    /// double x = -71.061342;
    /// double y = 42.355892;
    /// double tx;
    /// double ty;
    /// 
    /// trans.Transform(x, y, out tx, out ty);
    /// //tx and ty will contain the transformed coordinates
    /// ]]>
    /// </code>
    /// </example>
    public interface ICoordinateSystemCatalog
    {
        /// <summary>
        /// Gets an array of coordinate system categories
        /// </summary>
        CoordinateSystemCategory[] Categories { get; }

        /// <summary>
        /// Convers the specified coordinate system code to WKT
        /// </summary>
        /// <param name="coordcode"></param>
        /// <returns></returns>
        string ConvertCoordinateSystemCodeToWkt(string coordcode);

        /// <summary>
        /// Converts the specified epsg code to WKT
        /// </summary>
        /// <param name="epsg"></param>
        /// <returns></returns>
        string ConvertEpsgCodeToWkt(string epsg);

        /// <summary>
        /// Converts the specified WKT into a coordinate system code
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        string ConvertWktToCoordinateSystemCode(string wkt);

        /// <summary>
        /// Converts the specified WKT into an EPSG code
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        string ConvertWktToEpsgCode(string wkt);

        /// <summary>
        /// Gets an array of all coordinate systems in this catalog
        /// </summary>
        CoordinateSystemDefinitionBase[] Coordsys { get; }
        
        /// <summary>
        /// Gets an array of all coordinate systems in the specified category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        CoordinateSystemDefinitionBase[] EnumerateCoordinateSystems(string category);

        /// <summary>
        /// Gets the coordinate system that matches the specified code
        /// </summary>
        /// <param name="coordcode"></param>
        /// <returns></returns>
        CoordinateSystemDefinitionBase FindCoordSys(string coordcode);
        
        /// <summary>
        /// Gets an empty coordinate system
        /// </summary>
        /// <returns></returns>
        CoordinateSystemDefinitionBase CreateEmptyCoordinateSystem();

        /// <summary>
        /// Checks if the specified WKT is valid
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        bool IsValid(string wkt);
        
        /// <summary>
        /// Gets the name of the coordinate system library
        /// </summary>
        string LibraryName { get; }
        
        /// <summary>
        /// Gets whether the coordinate system catalog has been loaded
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Creates a simple coordinate system transformation from the source and target WKT strings
        /// </summary>
        /// <param name="sourceWkt"></param>
        /// <param name="targetWkt"></param>
        /// <returns></returns>
        ISimpleTransform CreateTransform(string sourceWkt, string targetWkt);
    }
}
