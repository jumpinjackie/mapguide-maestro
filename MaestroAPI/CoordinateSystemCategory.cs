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
    public abstract class CoordinateSystemCategory
    {
        private ICoordinateSystemCatalog _parent;
		private string m_name;
        private CoordinateSystem[] m_items;

        internal CoordinateSystemCategory(ICoordinateSystemCatalog parent, string name)
		{
			m_name = name;
			_parent = parent;
		}

		public string Name { get { return m_name; } }

        internal ICoordinateSystemCatalog Parent { get { return _parent; } }

		public CoordinateSystem[] Items
		{
			get
			{
				if (m_items == null)
				{
                    if (_parent != null)
                    {
                        return _parent.EnumerateCoordinateSystems(m_name);
                    }
                    /*
                    if (_parent != null)
                    {
                        string req = m_httpParent.RequestBuilder.EnumerateCoordinateSystems(m_name);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(m_httpParent.Connection.WebClient.OpenRead(req));
                        XmlNodeList lst = doc.SelectNodes("BatchPropertyCollection/PropertyCollection");
                        CoordSys[] data = new CoordSys[lst.Count];
                        for (int i = 0; i < lst.Count; i++)
                            data[i] = new CoordSys(this, lst[i]);
                        m_items = data;
                    }
                    else
                    {
                        MgBatchPropertyCollection bp = m_localParent.m_cf.EnumerateCoordinateSystems(m_name);
                        List<CoordSys> lst = new List<CoordSys>();
                        for(int i = 0; i < bp.Count; i++)
                            lst.Add(new CoordSys(this, bp[i]));

                    }*/
				}
				return m_items;
			}
		}

		public override string ToString()
		{
			return m_name;
		}
    }
}
