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
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Interface to MapGuide coordinate system functions.
	/// Only works with server > 1.2, since the coordinate mapping is not avalible through Http on older versions
	/// </summary>
	public class HttpCoordinateSystem : OSGeo.MapGuide.MaestroAPI.ICoordinateSystem
	{
		private HttpServerConnection m_con;
		private RequestBuilder m_req;
		private Category[] m_categories;
		private string m_coordLib = null;

		internal HttpCoordinateSystem(HttpServerConnection con, RequestBuilder req)
		{
			m_con = con;
			m_req = req;
		}

		internal HttpServerConnection Connection { get { return m_con; } }
		internal RequestBuilder RequestBuilder { get { return m_req; } }

		public Category[] Categories
		{
			get
			{
				if (m_categories == null)
				{
					string req = m_req.EnumerateCategories();
					XmlDocument doc = new XmlDocument();
					doc.Load(m_con.WebClient.OpenRead(req));
					XmlNodeList lst = doc.SelectNodes("StringCollection/Item");
					Category[] data = new Category[lst.Count];
					for(int i = 0; i < lst.Count; i++)
						data[i] = new Category(this, lst[i].InnerText);
					m_categories = data;
				}

				return m_categories;
			}
		}

		public string LibraryName
		{
			get 
			{
				if (m_coordLib == null)
				{
					string req = m_req.GetBaseLibrary();
					m_coordLib = System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req));
				}
				return m_coordLib;
			}
		}

		public bool IsValid(string wkt)
		{
			string req = m_req.IsValidCoordSys(wkt);
			return System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req)).ToLower().Trim().Equals("true");
		}

		public string ConvertWktToCoordinateSystemCode(string wkt)
		{
			string req = m_req.ConvertWktToCoordinateSystemCode(wkt);
			return System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req));
		}

		public string ConvertCoordinateSystemCodeToWkt(string coordcode)
		{
			string req = m_req.ConvertCoordinateSystemCodeToWkt(coordcode);
			return System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req));
		}

		public string ConvertWktToEpsgCode(string wkt)
		{
			string req = m_req.ConvertWktToEpsgCode(wkt);
			return System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req));
		}

		public string ConvertEpsgCodeToWkt(string epsg)
		{
			string req = m_req.ConvertEpsgCodeToWkt(epsg);
			return System.Text.Encoding.UTF8.GetString(m_con.WebClient.DownloadData(req));
		}

		public class Category
		{
			private HttpCoordinateSystem m_httpParent;
            private LocalNativeCoordinateSystem m_localParent;
			private string m_name;
			private CoordSys[] m_items;

			internal Category(HttpCoordinateSystem parent, string name)
			{
				m_name = name;
				m_httpParent = parent;
			}

            internal Category(LocalNativeCoordinateSystem parent, string name)
            {
                m_name = name;
                m_localParent = parent;
            }

			public string Name { get { return m_name; } }
			internal HttpCoordinateSystem HttpParent { get { return m_httpParent; } }
            internal LocalNativeCoordinateSystem LocalParent { get { return m_localParent; } }
            internal ICoordinateSystem Parent { get { return m_httpParent == null ? (ICoordinateSystem)m_localParent : (ICoordinateSystem)m_httpParent; } }

			public CoordSys[] Items
			{
				get
				{
					if (m_items == null)
					{
                        if (m_httpParent != null)
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

                        }
					}
					return m_items;
				}
			}

			public override string ToString()
			{
				return m_name;
			}

		}

		public class CoordSys
		{
			private Category m_parent;
			private string m_code;
			private string m_description;
			private string m_projection;
			private string m_projectionDescription;
			private string m_datum;
			private string m_datumDescription;
			private string m_ellipsoid;
			private string m_ellipsoidDescription;

			private string m_wkt = null;
			private string m_epsg = null;

			public CoordSys()
			{
			}

            internal CoordSys(Category parent, MgPropertyCollection props)
            {
                m_parent = parent;

                for (int i = 0; i < props.Count; i++)
                    switch (props[i].Name.ToLower())
                    {
                        case "code":
                            m_code = (props[i] as MgStringProperty).Value;
                            break;
                        case "description":
                            m_description = (props[i] as MgStringProperty).Value;
                            break;
                        case "projection":
                            m_projection = (props[i] as MgStringProperty).Value;
                            break;
                        case "projection description":
                            m_projectionDescription = (props[i] as MgStringProperty).Value;
                            break;
                        case "Datum":
                            m_datum = (props[i] as MgStringProperty).Value;
                            break;
                        case "datum description":
                            m_datumDescription = (props[i] as MgStringProperty).Value;
                            break;
                        case "ellipsoid":
                            m_ellipsoid = (props[i] as MgStringProperty).Value;
                            break;
                        case "ellipsoid description":
                            m_ellipsoidDescription = (props[i] as MgStringProperty).Value;
                            break;
                    }
            }

			internal CoordSys(Category parent, XmlNode topnode)
			{
				m_parent = parent;

				foreach(XmlNode node in topnode.ChildNodes)
					switch(node["Name"].InnerText.ToLower())
					{
						case "code":
							m_code = node["Value"].InnerText;
							break;
						case "description":
							m_description = node["Value"].InnerText;
							break;
						case "projection":
							m_projection = node["Value"].InnerText;
							break;
						case "projection description":
							m_projectionDescription = node["Value"].InnerText;
							break;
						case "Datum":
							m_datum = node["Value"].InnerText;
							break;
						case "datum description":
							m_datumDescription = node["Value"].InnerText;
							break;
						case "ellipsoid":
							m_ellipsoid = node["Value"].InnerText;
							break;
						case "ellipsoid description":
							m_ellipsoidDescription = node["Value"].InnerText;
							break;
					}
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

		public CoordSys[] Coordsys
		{
			get 
			{
				ArrayList items = new ArrayList();
				foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category cat in this.Categories)
						foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys coord in cat.Items)
							items.Add(coord);

				return (CoordSys[])items.ToArray(typeof(CoordSys));
			}
		}

		public CoordSys FindCoordSys(string coordcode)
		{
			try
			{
				foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.Category cat in this.Categories)
						foreach(OSGeo.MapGuide.MaestroAPI.HttpCoordinateSystem.CoordSys coord in cat.Items)
							if (coord.Code == coordcode)
								return coord;
			}
			catch
			{
			}

			return null;
		}

        public bool IsLoaded { get { return m_categories != null; } }

	}
}
