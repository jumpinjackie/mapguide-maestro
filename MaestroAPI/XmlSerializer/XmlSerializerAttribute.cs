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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI.XmlSerializer
{
	/// <summary>
	/// A decoration attribute for an Xml Serializeable element
	/// </summary>
	public class XmlSerializerAttribute
		: Attribute
	{	
		private string m_xmlMemberName;
		private Type m_xmlType;
		private string m_xmlNamespace;

		private bool m_required;
		private bool m_attribute;
		private bool m_text;
		private Version m_minVersion;
		private Version m_maxVersion;
		private string m_excludeValue;
		private string[] m_namedTypes;
		private string m_excludeProperty;

		private Hashtable m_nameToType;
		private Hashtable m_typeToName;

		public XmlSerializerAttribute()
			: this(null, typeof(string), false, false, null, null)
		{
		}

		public XmlSerializerAttribute(string XmlMembername, Type XmlType, bool Required, bool Attribute, Version MinVersion, Version MaxVersion)
			: base()
		{
			m_xmlMemberName = XmlMembername;
			m_xmlType = XmlType;
			m_required = Required;
			m_attribute = Attribute;
			m_minVersion = MinVersion;
			m_maxVersion = MaxVersion;
		}

		public string XmlMembername	
		{ 
			get { return m_xmlMemberName; } 
			set { m_xmlMemberName = value; }
		}
		public Type XmlType 
		{ 
			get { return m_xmlType; } 
			set { m_xmlType = value; }
		}
		public bool Text
		{
			get { return m_text; }
			set { m_text = value; }
		}
		public bool Attribute 
		{ 
			get { return m_attribute; } 
			set { m_attribute = value; }
		}
		public Version MinVersion 
		{ 
			get { return m_minVersion; } 
			set { m_minVersion = value; }
		}
		public Version MaxVersion 
		{
			get { return m_maxVersion; } 
			set { m_maxVersion = value; }
		}
		public string ExcludeIfThisValue
		{
			get { return m_excludeValue; }
			set { m_excludeValue = value; }
		}
		public string ExcludeProperty
		{
			get { return m_excludeProperty; }
			set { m_excludeProperty = value; }
		}
		public bool Required
		{
			get { return m_required; }
			set { m_required = value; }
		}
		public string XmlNamespace
		{
			get { return m_xmlNamespace; }
			set { m_xmlNamespace = value; }
		}
		public string[] NamedTypes
		{
			get { return m_namedTypes; }
			set 
			{ 
				m_typeToName = null;
				m_nameToType = null;
				m_namedTypes = value; 
			}
		}
		public string GetNameOfType(Type type)
		{
			if (m_typeToName == null)
			{
				m_typeToName = new Hashtable();
				for(int i = 0; i<m_namedTypes.Length - 1; i+=2)
					m_typeToName.Add(Type.GetType(m_namedTypes[i+1]), m_namedTypes[i]);
			}
			return (string)m_typeToName[type];
		}
		public Type GetTypeOfName(string name)
		{
			if (m_nameToType == null)
			{
				m_nameToType = new Hashtable();
				for(int i = 0; i<m_namedTypes.Length - 1; i+=2)
					m_nameToType.Add(m_namedTypes[i], Type.GetType(m_namedTypes[i+1]));
			}
			return (Type)m_nameToType[name];
		}
	}
}
