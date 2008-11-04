#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    public class LocalNativeCoordinateSystem : ICoordinateSystem
    {
        private LocalNativeConnection m_con;
        private HttpCoordinateSystem.Category[] m_categories;
        private string m_coordLib = null;
        internal OSGeo.MapGuide.MgCoordinateSystemFactory m_cf;

        internal LocalNativeCoordinateSystem(LocalNativeConnection con)
        {
            m_con = con;
            m_cf = new MgCoordinateSystemFactory();
        }

        #region ICoordinateSystem Members

        public HttpCoordinateSystem.Category[] Categories
        {
            get
            {
                if (m_categories == null)
                {
                    MgStringCollection c = m_cf.EnumerateCategories();
                    HttpCoordinateSystem.Category[] data = new HttpCoordinateSystem.Category[c.GetCount()];

                    for (int i = 0; i < c.GetCount(); i++)
                        data[i] = new HttpCoordinateSystem.Category(this, c.GetItem(i));
                    m_categories = data;
                }

                return m_categories;
            }
        }

        public string ConvertCoordinateSystemCodeToWkt(string coordcode)
        {
            return m_cf.ConvertCoordinateSystemCodeToWkt(coordcode);
        }

        public string ConvertEpsgCodeToWkt(string epsg)
        {
            return m_cf.ConvertEpsgCodeToWkt(int.Parse(epsg));
        }

        public string ConvertWktToCoordinateSystemCode(string wkt)
        {
            return m_cf.ConvertWktToCoordinateSystemCode(wkt);
        }

        public string ConvertWktToEpsgCode(string wkt)
        {
            return m_cf.ConvertWktToEpsgCode(wkt).ToString();
        }

        public HttpCoordinateSystem.CoordSys[] Coordsys
        {
            get
            {
                ArrayList items = new ArrayList();
                foreach (OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category cat in this.Categories)
                    foreach (OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys coord in cat.Items)
                        items.Add(coord);

                return (HttpCoordinateSystem.CoordSys[])items.ToArray(typeof(HttpCoordinateSystem.CoordSys));
            }
        }

        public HttpCoordinateSystem.CoordSys FindCoordSys(string coordcode)
        {
            try
            {
                foreach (OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category cat in this.Categories)
                    foreach (OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys coord in cat.Items)
                        if (coord.Code == coordcode)
                            return coord;
            }
            catch
            {
            }

            return null;
        }

        public bool IsValid(string wkt)
        {
            return m_cf.IsValid(wkt);
        }

        public string LibraryName
        {
            get
            {
                if (m_coordLib == null)
                    m_coordLib = m_cf.GetBaseLibrary();
                return m_coordLib;
            }
        }

        #endregion
    }
}
