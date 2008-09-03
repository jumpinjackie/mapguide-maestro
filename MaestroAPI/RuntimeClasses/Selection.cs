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

namespace OSGeo.MapGuide.MaestroAPI.RuntimeClasses
{
	/// <summary>
	/// Summary description for Selection.
	/// </summary>
	public class Selection
	{
		private XmlDocument m_selection;

		public Selection()
		{
			m_selection = new XmlDocument();
		}

		public Selection(string xml)
			: this()
		{
			if (xml != null && xml.Trim() != "")
				m_selection.LoadXml(xml);
		}

		internal void Serialize(BinarySerializer.MgBinarySerializer s)
		{
			if (m_selection["FeatureSet"] == null)
			{
				s.Write((int)0);
				return;
			}

			XmlNodeList lst = m_selection["FeatureSet"].SelectNodes("Layer");
			s.Write(lst.Count);
			foreach(XmlNode n in lst)
			{
				if (n.Attributes["id"] == null)
					throw new Exception("A layer in selection had no id");
				s.Write(n.Attributes["id"].Value);
	
				XmlNodeList cls = n.SelectNodes("Class");
				s.Write(cls.Count);

				foreach(XmlNode c in cls)
				{
					s.Write(c.Attributes["id"].Value);
					XmlNodeList ids = c.SelectNodes("ID");
					s.Write(ids.Count);

					foreach(XmlNode id in ids)
						s.Write(id.InnerText);
				}
			}
		}

		internal void Deserialize(BinarySerializer.MgBinaryDeserializer d)
		{
			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.AppendChild(doc.CreateElement("FeatureSet"));
			int layerCount = d.ReadInt32();
			for(int i = 0; i < layerCount; i++)
			{
				XmlNode layer = root.AppendChild(doc.CreateElement("Layer"));
				layer.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString();

				int classCount = d.ReadInt32();
				for(int j = 0; j < classCount; j++)
				{
					XmlNode @class = layer.AppendChild(doc.CreateElement("Class"));
					@class.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString();

					int idCount = d.ReadInt32();
					for(int k = 0; k < idCount; k++)
						@class.AppendChild(doc.CreateElement("ID")).InnerText = d.ReadString();
				}
			}

			m_selection = doc;
		}

		public string SelectionXml 
		{
			get { return m_selection.OuterXml; }
			set { m_selection.LoadXml(value); }
		}


	}
}
