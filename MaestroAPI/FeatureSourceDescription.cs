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

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Class that represents a the layout of a datasource
	/// </summary>
	public class FeatureSourceDescription
	{
		private FeatureSourceSchema[] m_schemas;

		public FeatureSourceDescription(System.IO.Stream stream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(stream);

			if (doc.FirstChild.Name != "xml")
				throw new Exception("Bad document");

			if (doc.ChildNodes.Count != 2 || doc.ChildNodes[1].Name != "xs:schema")
				throw new Exception("Bad document");

			XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
			mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			mgr.AddNamespace("gml", "http://www.opengis.net/gml");
			mgr.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");

			XmlNodeList lst = doc.SelectNodes("xs:schema/xs:complexType[@abstract='false']", mgr);
			m_schemas = new FeatureSourceSchema[lst.Count];
			for(int i = 0;i<lst.Count;i++)
				m_schemas[i] = new FeatureSourceSchema(lst[i], doc, mgr);
		}

		public FeatureSourceSchema[] Schemas { get { return m_schemas; } }

		public FeatureSourceSchema this[int index] { get { return m_schemas[index]; } }
		public FeatureSourceSchema this[string index] 
		{
			get 
			{
				for(int i =0 ;i<m_schemas.Length; i++)
					if (m_schemas[i].Name == index)
						return m_schemas[i];

				throw new OverflowException("No such item found: " + index);
			}
		}

		public class FeatureSourceSchema
		{
			private string m_name;
			private string m_schema;
			private FeatureSetColumn[] m_columns;

			public FeatureSourceSchema(XmlNode node, XmlDocument doc, XmlNamespaceManager mgr)
			{
				XmlNode root = doc.FirstChild;
				if (root.NodeType == XmlNodeType.XmlDeclaration)
					root = root.NextSibling;
				m_schema = root.Attributes["targetNamespace"] == null ? null : root.Attributes["targetNamespace"].Value;
				if (m_schema != null && m_schema.IndexOf("/") > 0)
					m_schema = m_schema.Substring(m_schema.LastIndexOf("/") + 1);
				m_name = node.Attributes["name"].Value;
				if (m_name.EndsWith("Type"))
					m_name = m_name.Substring(0, m_name.Length - "Type".Length);
				
				XmlNodeList lst;
				if (node.ChildNodes.Count == 0)
				{
					m_columns = new FeatureSetColumn[0];
					return;
				}
				else if (node.FirstChild.Name == "xs:sequence")
					lst = node.SelectNodes("xs:sequence/xs:element", mgr);
				else
					lst = node.SelectNodes("xs:complexContent/xs:extension/xs:sequence/xs:element", mgr);
				
				m_columns = new FeatureSetColumn[lst.Count];
				for(int i = 0;i<lst.Count;i++)
					m_columns[i] = new FeatureSetColumn(lst[i]);
			}

			public string Name { get { return m_name; } }
			public string Schema { get { return m_schema; } }
			public string Fullname { get { return m_schema == null ? m_name : m_schema + ":" + m_name; } }
            public string FullnameDecoded { get { return Utility.DecodeFDOName(this.Fullname); } }
			public FeatureSetColumn[] Columns { get { return m_columns; } }
		}
	}
}
