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

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// Base coordinate system catalog class
    /// </summary>
    public abstract class CoordinateSystemCatalog : ICoordinateSystemCatalog
    {
        /// <summary>
        /// Gets an array of all coordinate systems in this catalog
        /// </summary>
        /// <value></value>
        public virtual CoordinateSystemDefinitionBase[] Coordsys
        {
            get
            {
                List<CoordinateSystemDefinitionBase> items = new List<CoordinateSystemDefinitionBase>();
                foreach (CoordinateSystemCategory cat in this.Categories)
                {
                    foreach (CoordinateSystemDefinitionBase coord in cat.Items)
                    {
                        items.Add(coord);
                    }
                }
                return items.ToArray();
            }
        }

        /// <summary>
        /// Gets the coordinate system that matches the specified code
        /// </summary>
        /// <param name="coordcode"></param>
        /// <returns></returns>
        public virtual CoordinateSystemDefinitionBase FindCoordSys(string coordcode)
        {
            try
            {
                foreach (CoordinateSystemCategory cat in this.Categories)
                {
                    foreach (CoordinateSystemDefinitionBase coord in cat.Items)
                    {
                        if (coord.Code == coordcode)
                            return coord;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Gets an empty coordinate system
        /// </summary>
        /// <returns></returns>
        public abstract CoordinateSystemDefinitionBase CreateEmptyCoordinateSystem();

        /// <summary>
        /// Gets an array of coordinate system categories
        /// </summary>
        /// <value></value>
        public abstract CoordinateSystemCategory[] Categories { get; }

        /// <summary>
        /// Gets the name of the coordinate system library
        /// </summary>
        /// <value></value>
        public abstract string LibraryName { get; }

        /// <summary>
        /// Convers the specified coordinate system code to WKT
        /// </summary>
        /// <param name="coordcode"></param>
        /// <returns></returns>
        public abstract string ConvertCoordinateSystemCodeToWkt(string coordcode);

        /// <summary>
        /// Converts the specified epsg code to WKT
        /// </summary>
        /// <param name="epsg"></param>
        /// <returns></returns>
        public abstract string ConvertEpsgCodeToWkt(string epsg);

        /// <summary>
        /// Converts the specified WKT into a coordinate system code
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public abstract string ConvertWktToCoordinateSystemCode(string wkt);

        /// <summary>
        /// Converts the specified WKT into an EPSG code
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public abstract string ConvertWktToEpsgCode(string wkt);

        /// <summary>
        /// Gets an array of all coordinate systems in the specified category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public abstract CoordinateSystemDefinitionBase[] EnumerateCoordinateSystems(string category);

        /// <summary>
        /// Checks if the specified WKT is valid
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public abstract bool IsValid(string wkt);

        /// <summary>
        /// Gets whether the coordinate system catalog has been loaded
        /// </summary>
        /// <value></value>
        public abstract bool IsLoaded { get; }

        /// <summary>
        /// Creates a simple coordinate system transformation from the source and target WKT strings
        /// </summary>
        /// <param name="sourceWkt"></param>
        /// <param name="targetWkt"></param>
        /// <returns></returns>
        public virtual ISimpleTransform CreateTransform(string sourceWkt, string targetWkt)
        {
            return new DefaultSimpleTransform(sourceWkt, targetWkt);
        }
    }
}
