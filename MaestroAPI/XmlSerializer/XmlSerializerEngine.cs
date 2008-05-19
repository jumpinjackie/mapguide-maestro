#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
using System.Reflection;
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI.XmlSerializer
{
	/// <summary>
	/// An engine that can serialize and deserialize objects using Attributes
	/// </summary>
	public class XmlSerializerEngine
	{
		private Hashtable m_serializers;
		private Hashtable m_deserializers;
		private Hashtable m_namespaces;

		public delegate string SerializeItem(object value, object parent, XmlSerializerAttribute attribute);
		public delegate string DeserializeItem(object value, object target, XmlSerializerAttribute attribute);

		public XmlSerializerEngine()
		{
			m_serializers = new Hashtable();
			m_deserializers = new Hashtable();
			m_namespaces = new Hashtable();
			m_namespaces.Add("http://www.w3.org/2001/XMLSchema-instance", "xsi");
			m_namespaces.Add("http://www.w3.org/2001/XMLSchema", "xsd");
		}

		public void AddSerialize(Type type, SerializeItem serializer, DeserializeItem deserializer)
		{
			//TODO: Validate
			m_serializers.Add(type, serializer);
			m_deserializers.Add(type, deserializer);
		}
	
		public XmlDocument Serialize(object item, PropertyInfo versionProperty)
		{
			return Serialize(item, (Version)Convert.ChangeType(versionProperty.GetValue(item, null), typeof(Version)));
		}

		public XmlDocument Serialize(object item, Version version)
		{
			Type roottype = item.GetType();

			XmlSerializerAttribute rootattr = GetAttribute(roottype, version);
			if (rootattr == null)
				throw new Exception("Supplied object did not have an XmlSerializerAttribute: " + roottype.FullName);			

			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
			XmlNamespaceManager nms = new XmlNamespaceManager(doc.NameTable);
			foreach(System.Collections.DictionaryEntry de in m_namespaces)
				nms.AddNamespace((string)de.Key, (string)de.Value);
			XmlNode rootnode = doc.AppendChild(doc.CreateElement(rootattr.XmlMembername, rootattr.XmlNamespace));
			FixupNamespace(rootnode);

			foreach(PropertyInfo pi in roottype.GetProperties())
				SerializeElement(item, rootnode, pi, version);

			return doc;
		}

		private XmlNode FixupNamespace(XmlNode n)
		{
			if (n.Prefix == "" && n.NamespaceURI != "")
				n.Prefix = (string)m_namespaces[n.NamespaceURI];
			return n;
		}

		private XmlAttribute FixupNamespace(XmlAttribute n)
		{
			if (n.Prefix == "" && n.NamespaceURI != "")
				n.Prefix = (string)m_namespaces[n.NamespaceURI];
			return n;
		}

		private XmlSerializerAttribute GetAttribute(MemberInfo pi, Version version)
		{
			object[] attrs = pi.GetCustomAttributes(typeof(XmlSerializerAttribute), true);
			if (attrs == null || attrs.Length == 0)
				return null;

			XmlSerializerAttribute best_attr = null;
			foreach(XmlSerializerAttribute attr in attrs)
				if ((attr.MinVersion == null || (attr.MinVersion != null && version >= attr.MinVersion)) && (attr.MaxVersion == null || (attr.MaxVersion != null && version <= attr.MaxVersion)))
					if (best_attr == null)
						best_attr = attr;
					else
						throw new Exception("Multiple attributes were defined, but no unique match could be made: " + pi.DeclaringType.FullName + "." + pi.Name );



			if (best_attr != null && best_attr.XmlMembername == null)
				best_attr.XmlMembername = pi.Name;
			return best_attr;
		}

		private void SerializeElement(object item, XmlNode parent, PropertyInfo pi, Version version)
		{
			XmlSerializerAttribute xmlattr = GetAttribute(pi, version);
			if (xmlattr == null)
				return;

			if (xmlattr.ExcludeProperty != null)
			{
				PropertyInfo pix = pi.DeclaringType.GetProperty(xmlattr.ExcludeProperty);
				if (pix == null)
					throw new Exception("An exclude property was declared: " + xmlattr.ExcludeProperty + ", but type '" + pi.DeclaringType.FullName + "' had no such property");
				else if (pix.PropertyType != typeof(bool))
					throw new Exception("An exclude property was declared: " + xmlattr.ExcludeProperty + " for type '" + pi.DeclaringType.FullName + "' but the return type was: " + pi.PropertyType.FullName + " and boolean was expected");
				else if ((bool)pi.DeclaringType.GetProperty(xmlattr.ExcludeProperty).GetValue(item, null))
				{
					if (xmlattr.Required)
						throw new Exception("Attribute " + pi.Name + " is required");
					else
						return;
				}
			}

			object val = pi.GetValue(item, null);

            if (val == null)
				if (xmlattr.Required)
					throw new Exception("Attribute " + pi.Name + " is required");
				else
					return;


			string nodevalue = null;

			if (m_serializers.ContainsKey(pi.PropertyType))
				nodevalue = ((SerializeItem)m_serializers[pi.PropertyType])(val, item, xmlattr);
			else if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string))
				nodevalue = Convert.ToString(val);
			else if (pi.PropertyType.IsArray || pi.PropertyType.GetInterface(typeof(System.Collections.ICollection).FullName) != null)
			{
				if (xmlattr.Attribute)
					throw new Exception("Collection types cannot be attributes");

				foreach(object o in (IEnumerable)val)
				{
					XmlNode n = parent.AppendChild(parent.OwnerDocument.CreateElement(xmlattr.GetNameOfType(o.GetType())));
					foreach(PropertyInfo pix in o.GetType().GetProperties())
						SerializeElement(o, n, pix, version);
				}
				
			}
			else if (pi.PropertyType.GetProperties().Length > 0)
			{
				if (xmlattr.Attribute)
					throw new Exception("Complex types cannot be attributes");

				XmlNode n = parent.AppendChild(parent.OwnerDocument.CreateElement(xmlattr.XmlMembername));
				object subvalue = pi.GetValue(item, null);
				foreach(PropertyInfo pix in pi.PropertyType.GetProperties())
					SerializeElement(subvalue, n, pix, version);
			}
			else
				nodevalue = val.ToString();

			if (nodevalue != null)
			{
				if (xmlattr.ExcludeIfThisValue != null && object.Equals(nodevalue, xmlattr.ExcludeIfThisValue))
					if (xmlattr.Required)
						throw new Exception("Attribute " + pi.Name + " is required");
					else
						return;

				if (xmlattr.Attribute)
					parent.Attributes.Append(FixupNamespace(parent.OwnerDocument.CreateAttribute(xmlattr.XmlMembername, xmlattr.XmlNamespace))).Value = nodevalue;
				else if(xmlattr.Text)
					//TODO: There can be only one Text attribute per level
					parent.InnerText = nodevalue;
				else
					parent.AppendChild(FixupNamespace(parent.OwnerDocument.CreateElement(xmlattr.XmlMembername, xmlattr.XmlNamespace))).InnerText = nodevalue;
			}
		}

		public object Deserialize(XmlDocument doc, Type targetType, Version version)
		{
			return Deserialize(doc, targetType, null, version);
		}

		public object Deserialize(XmlDocument doc, Type targetType, object[] constructorparams, Version version)
		{
			XmlSerializerAttribute rootattr = GetAttribute(targetType, version);
			if (rootattr == null)
				throw new Exception("Supplied object did not have an XmlSerializerAttribute: " + targetType.FullName);			

			XmlNode rootnode = doc[rootattr.XmlMembername];
			if (rootnode == null)
				throw new Exception("Document root node was not named: " + rootattr.XmlMembername);

			object item = Activator.CreateInstance(targetType, constructorparams);

			foreach(PropertyInfo pi in targetType.GetProperties())
				DeserializeElement(rootnode, item, targetType, pi, version);

			return item;
		}

		public void DeserializeElement(XmlNode parent, object targetItem, Type targetType, PropertyInfo pi, Version version)
		{
			XmlSerializerAttribute xmlattr = GetAttribute(pi, version);
			if (xmlattr == null)
				return;

			object itemvalue = null;

			object nodevalue = null;
			if (xmlattr.Attribute)
			{
				if (parent.Attributes[xmlattr.XmlMembername] != null)
					nodevalue = parent.Attributes[xmlattr.XmlMembername].Value;
			}
			else if(xmlattr.Text)
			{
				nodevalue = parent;
			}
			else
			{
				if (parent[xmlattr.XmlMembername] != null)
					nodevalue = parent[xmlattr.XmlMembername];
			}
				

			if (m_deserializers.ContainsKey(pi.PropertyType))
				itemvalue = ((DeserializeItem)m_deserializers[pi.PropertyType])(nodevalue, targetItem, xmlattr);
			else if (pi.PropertyType.IsArray || pi.PropertyType.GetInterface(typeof(System.Collections.ICollection).FullName) != null)
			{
				if (xmlattr.Attribute)
					throw new Exception("Collection types cannot be attributes");

				ArrayList items = new ArrayList();

				foreach(XmlNode child in parent.ChildNodes)
					if (xmlattr.GetTypeOfName(child.Name) != null)
					{
						Type childType = xmlattr.GetTypeOfName(child.Name);
					
						if (childType.GetConstructor(new Type[0]) == null)
							throw new Exception("An object of type " + childType.FullName + " must be created for property " + pi.Name + " but no default constructor exists.\nEither create a default constructor, or prepare the value in initialization of the owner class");
						object targetChild = Activator.CreateInstance(childType);

						foreach(PropertyInfo pix in childType.GetProperties())
							DeserializeElement(child, targetChild, childType, pix, version);

						items.Add(targetChild);
					}


				if (pi.PropertyType.IsArray)
					pi.SetValue(targetItem, items.ToArray(pi.PropertyType.GetElementType()), null);
				else
				{
					object targetCollection = pi.GetValue(targetItem, null);
					if (targetCollection == null)
					{
						if (pi.PropertyType.GetConstructor(new Type[0]) == null)
							throw new Exception("An object of type " + pi.PropertyType.FullName + " must be created for property " + pi.Name + " but no default constructor exists.\nEither create a default constructor, or prepare the value in initialization of the owner class");
						targetCollection = Activator.CreateInstance(pi.PropertyType);
						pi.SetValue(targetItem, targetCollection, null);
					}

					MethodInfo mi = pi.PropertyType.GetMethod("AddRange");
					if (mi != null)
						mi.Invoke(targetCollection, new object[] { items });
					else
					{
						mi = pi.PropertyType.GetMethod("Add");
						if (mi != null)
							foreach(object o in items)
								mi.Invoke(targetCollection, new object[] { o });
						else
							throw new Exception("Collection type must have either the \"Add\" or the \"AddRange\" method exposed. Neither method was found for: " + targetItem.GetType().FullName);
					}
				}

			}
			else if (!pi.PropertyType.IsPrimitive && pi.PropertyType != typeof(string))
			{
				if (xmlattr.Attribute)
					throw new Exception("Complex types cannot be attributes");

				object targetChild = pi.GetValue(targetItem, null);
				if (targetChild == null)
				{
					if (pi.PropertyType.GetConstructor(new Type[0]) == null)
						throw new Exception("An object of type " + pi.PropertyType.FullName + " must be created for property " + pi.Name + " but no default constructor exists.\nEither create a default constructor, or prepare the value in initialization of the owner class");
					targetChild = Activator.CreateInstance(pi.PropertyType);
					pi.SetValue(targetItem, targetChild, null);
				}

				XmlNode child = parent[xmlattr.XmlMembername];
				foreach(PropertyInfo pix in pi.PropertyType.GetProperties())
					DeserializeElement(child, targetChild, pi.PropertyType, pix, version);

			}
			else
			{
				//TODO: Is there a better default?
				if (nodevalue != null)
				{
					if (nodevalue.GetType() == typeof(XmlElement))
						nodevalue = ((XmlElement)nodevalue).InnerText;
					itemvalue = Convert.ChangeType(Convert.ToString(nodevalue), pi.PropertyType);
					pi.SetValue(targetItem, itemvalue, null);
				}
			}

		}
	}
}
