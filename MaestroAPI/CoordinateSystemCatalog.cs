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

namespace OSGeo.MapGuide.MaestroAPI
{
    public abstract class CoordinateSystemCatalog : ICoordinateSystemCatalog
    {
        public virtual CoordinateSystem[] Coordsys
        {
            get
            {
                List<CoordinateSystem> items = new List<CoordinateSystem>();
                foreach (CoordinateSystemCategory cat in this.Categories)
                {
                    foreach (CoordinateSystem coord in cat.Items)
                    {
                        items.Add(coord);
                    }
                }
                return items.ToArray();
            }
        }

        public virtual CoordinateSystem FindCoordSys(string coordcode)
        {
            try
            {
                foreach (CoordinateSystemCategory cat in this.Categories)
                {
                    foreach (CoordinateSystem coord in cat.Items)
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

        public abstract CoordinateSystem CreateEmptyCoordinateSystem();

        public abstract CoordinateSystemCategory[] Categories { get; }

        public abstract string LibraryName { get; }

        public abstract string ConvertCoordinateSystemCodeToWkt(string coordcode);

        public abstract string ConvertEpsgCodeToWkt(string epsg);

        public abstract string ConvertWktToCoordinateSystemCode(string wkt);

        public abstract string ConvertWktToEpsgCode(string wkt);

        public abstract CoordinateSystem[] EnumerateCoordinateSystems(string category);

        public abstract bool IsValid(string wkt);

        public abstract bool IsLoaded { get; }
    }
}
