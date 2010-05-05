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
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI
{
    public abstract class CoordinateSystem
    {
        protected CoordinateSystemCategory m_parent;
        protected string m_code;
        protected string m_description;
        protected string m_projection;
        protected string m_projectionDescription;
        protected string m_datum;
        protected string m_datumDescription;
        protected string m_ellipsoid;
        protected string m_ellipsoidDescription;

        protected string m_wkt = null;
        protected string m_epsg = null;

        protected CoordinateSystem() { } 

        protected CoordinateSystem(CoordinateSystemCategory parent)
        {
            m_parent = parent;
        }

		public string Code 
		{ 
			get { return m_code; } 
			set { m_code = value; }
		}
		public string Description 
		{ 
			get { return m_description; } 
			set { m_description = value; }
		}
		public string Projection { get { return m_projection; } }
		public string ProjectionDescription { get { return m_projectionDescription; } }
		public string Datum { get { return m_datum; } }
		public string DatumDescription { get { return m_datumDescription; } }
		public string Ellipsoid { get { return m_ellipsoid; } }
		public string EllipsoidDescription { get { return m_ellipsoidDescription; } }

		public string WKT 
		{
			get 
			{
				if (m_wkt == null)
					m_wkt = m_parent.Parent.ConvertCoordinateSystemCodeToWkt(m_code);
				return m_wkt;
			}
			set
			{
				m_wkt = value;
			}
		}

		public string EPSG 
		{
			get 
			{
				if (m_epsg == null)
					if (m_code.StartsWith("EPSG:"))
						m_epsg = m_code.Substring(5);
					else
						m_epsg = m_parent.Parent.ConvertWktToEpsgCode(m_parent.Parent.ConvertCoordinateSystemCodeToWkt(m_code));

				return m_epsg;
			}
		}

		public override string ToString()
		{
			if (m_description == null && m_code == null)
				return m_wkt == null ? "<null>" : m_wkt;
			else if (m_description == null)
				return m_code;
			else if (m_code == null)
				return m_description;
			else
				return m_description + " (" + m_code + ")";
		}
    }
}
