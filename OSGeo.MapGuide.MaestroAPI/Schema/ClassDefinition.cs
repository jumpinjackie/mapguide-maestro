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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    //TODO: Expand on documentation as this is an important class

    /// <summary>
    /// Represents a FDO class definition
    /// </summary>
    public class ClassDefinition : SchemaElement, IFdoSerializable
    {
        private List<DataPropertyDefinition> _identity;

        private List<PropertyDefinition> _properties;

        private Dictionary<string, int> _ordinalMap;

        private ClassDefinition()
        {
            _ordinalMap = new Dictionary<string, int>();
            _identity = new List<DataPropertyDefinition>();
            _properties = new List<PropertyDefinition>();
        }

        public ClassDefinition(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public ClassDefinition BaseClass { get; set; }

        /// <summary>
        /// Gets the property definition at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PropertyDefinition this[int index]
        {
            get { return _properties[index]; }
        }

        public int GetOrdinal(string name)
        {
            if (_ordinalMap.ContainsKey(name))
                return _ordinalMap[name];

            for (int i = 0; i < this.Properties.Count; i++)
            {
                if (this[i].Name.Equals(name))
                {
                    _ordinalMap[name] = i;
                    return i;
                }
            }

            throw new ArgumentException("Property not found: " + name); //LOCALIZEME
        }

        /// <summary>
        /// Gets or sets whether this is abstract
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets whether this is computed. Computed classes should have its properties
        /// checked out (and possibly modified) before serving as a basis for a new class definition
        /// </summary>
        public bool IsComputed { get; set; }

        /// <summary>
        /// Gets or sets the name of the default geometry property.
        /// </summary>
        public string DefaultGeometryPropertyName { get; set; }

        /// <summary>
        /// Gets the identity properties
        /// </summary>
        public ReadOnlyCollection<DataPropertyDefinition> IdentityProperties
        {
            get { return _identity.AsReadOnly(); }
        }

        /// <summary>
        /// Removes the assigned identity properties
        /// </summary>
        public void ClearIdentityProperties() { _identity.Clear(); }

        /// <summary>
        /// Gets the properties
        /// </summary>
        public ReadOnlyCollection<PropertyDefinition> Properties
        {
            get { return _properties.AsReadOnly(); }
        }

        /// <summary>
        /// Adds the specified data property, with an option to include it as an identity property
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="identity"></param>
        public void AddProperty(DataPropertyDefinition prop, bool identity)
        {
            if (!_properties.Contains(prop))
                _properties.Add(prop);

            if (identity && !_identity.Contains(prop))
                _identity.Add(prop);

            prop.Parent = this;
        }

        /// <summary>
        /// Adds the specified property definition
        /// </summary>
        /// <param name="prop"></param>
        public void AddProperty(PropertyDefinition prop)
        {
            if (!_properties.Contains(prop)) 
                _properties.Add(prop);
            prop.Parent = this;
        }

        /// <summary>
        /// Gets the index of the specified property
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public int IndexOfProperty(PropertyDefinition prop)
        {
            return _properties.IndexOf(prop);
        }
        
        /// <summary>
        /// Removes the property definition at the specified index. If it is a data property
        /// is is also removed from the identity properties (if it is specified as one)
        /// </summary>
        /// <param name="index"></param>
        public void RemovePropertyAt(int index)
        {
            if (index < _properties.Count)
            {
                var prop = _properties[index];
                _properties.RemoveAt(index);
                if (prop.Type == PropertyDefinitionType.Data)
                {
                    _identity.Remove((DataPropertyDefinition)prop);
                    prop.Parent = null;
                }
            }
        }

        /// <summary>
        /// Removes the specified property from the properties collection. If it is a data property definition, it is also
        /// removed from the identity properties collection
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public bool RemoveProperty(PropertyDefinition prop)
        {
            bool removed = _properties.Remove(prop);

            if (removed && prop.Type == PropertyDefinitionType.Data)
            {
                _identity.Remove((DataPropertyDefinition)prop);
                prop.Parent = null;
            }

            return removed;
        }

        #region old impl
        /*
        private string m_name;
        private string m_schema;
        private FeatureSetColumn[] m_columns;

        internal ClassDefinition(XmlNode node, XmlNamespaceManager mgr)
        {
            XmlNode root = node.ParentNode;
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
            for (int i = 0; i < lst.Count; i++)
                m_columns[i] = new ClassPropertyColumn(lst[i]);

            XmlNode extension = node.SelectSingleNode("xs:complexContent/xs:extension", mgr);
            if (extension != null && extension.Attributes["base"] != null)
            {
                string extTypeName = extension.Attributes["base"].Value;
                extTypeName = extTypeName.Substring(extTypeName.IndexOf(":") + 1);

                XmlNode baseEl = node.ParentNode.SelectSingleNode("xs:complexType[@name='" + extTypeName + "']", mgr);
                if (baseEl != null)
                {
                    ClassDefinition tmpScm = new ClassDefinition(baseEl, mgr);
                    FeatureSetColumn[] tmpCol = new FeatureSetColumn[m_columns.Length + tmpScm.m_columns.Length];
                    Array.Copy(m_columns, tmpCol, m_columns.Length);
                    Array.Copy(tmpScm.m_columns, 0, tmpCol, m_columns.Length, tmpScm.m_columns.Length);
                    m_columns = tmpCol;
                }
            }
        }

        /// <summary>
        /// Gets the name of this class definition
        /// </summary>
        public string Name { get { return m_name; } }

        /// <summary>
        /// Gets the name of the schema which this class definition belongs to
        /// </summary>
        public string SchemaName { get { return m_schema; } }

        /// <summary>
        /// Gets the fully qualified name of this class definition ([schema_name]:[name])
        /// </summary>
        public string QualifiedName { get { return m_schema == null ? m_name : m_schema + ":" + m_name; } }

        /// <summary>
        /// Gets the decoded fully qualified name of this class definition ([schema_name]:[name])
        /// </summary>
        public string QualifiedNameDecoded { get { return Utility.DecodeFDOName(this.QualifiedName); } }

        /// <summary>
        /// Gets an array of columns defining the properties in this class definition
        /// </summary>
        public FeatureSetColumn[] Columns { get { return m_columns; } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.QualifiedName;
        }

        internal void MarkIdentityProperties(IEnumerable<string> keyFieldNames)
        {
            foreach (var name in keyFieldNames)
            {
                foreach (var col in m_columns)
                {
                    if (col.Name.Equals(name))
                    {
                        col.IsIdentity = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an array of names of the identity properties
        /// </summary>
        /// <returns></returns>
        public string[] GetIdentityProperties()
        {
            List<string> keys = new List<string>();
            foreach (var col in m_columns)
            {
                if (col.IsIdentity)
                    keys.Add(col.Name);
            }
            return keys.ToArray();
        }
         */
        #endregion

        public FeatureSchema Parent { get; internal set; }

        public PropertyDefinition FindProperty(string name)
        {
            foreach (var prop in _properties)
            {
                if (prop.Name.Equals(name))
                    return prop;
            }
            return null;
        }

        public string QualifiedName { get { return this.Parent != null ? this.Parent.Name + ":" + this.Name : this.Name; } }

        public void WriteXml(XmlDocument doc, XmlNode currentNode)
        {
            XmlElement id = null;

            if (_identity.Count > 0)
            {
                id = doc.CreateElement("xs", "element", XmlNamespaces.XS);
                id.SetAttribute("name", this.Name);
                id.SetAttribute("type", this.Parent.Name + ":" + this.Name + "Type");
                id.SetAttribute("abstract", this.IsAbstract.ToString().ToLower());
                id.SetAttribute("substitutionGroup", "gml:_Feature");

                var key = doc.CreateElement("xs", "key", XmlNamespaces.XS);
                key.SetAttribute("name", this.Name + "Key");

                var selector = doc.CreateElement("xs", "selector", XmlNamespaces.XS);
                selector.SetAttribute("xpath", ".//" + this.Name);

                key.AppendChild(selector);

                foreach (var prop in _identity)
                {
                    var field = doc.CreateElement("xs", "field", XmlNamespaces.XS);
                    field.SetAttribute("xpath", prop.Name);

                    key.AppendChild(field);
                }
                id.AppendChild(key);
            }

            //Now write class body
            var ctype = doc.CreateElement("xs", "complexType", XmlNamespaces.XS);
            ctype.SetAttribute("name", this.Name + "Type"); //TODO: This may have been decoded. Should it be re-encoded?
            ctype.SetAttribute("abstract", this.IsAbstract.ToString().ToLower());
            if (!string.IsNullOrEmpty(this.DefaultGeometryPropertyName))
            {
                var geom = FindProperty(this.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
                if (geom != null)
                {
                    ctype.SetAttribute("geometryName", XmlNamespaces.FDO, geom.Name);
                }
            }

            var cnt = doc.CreateElement("xs", "complexContent", XmlNamespaces.XS);
            ctype.AppendChild(cnt);

            var ext = doc.CreateElement("xs", "extension", XmlNamespaces.XS);
            if (this.BaseClass != null)
                ext.SetAttribute("base", this.BaseClass.QualifiedName);
            else
                ext.SetAttribute("base", "gml:AbstractFeatureType");
            cnt.AppendChild(ext);

            var seq = doc.CreateElement("xs", "sequence", XmlNamespaces.XS);
            ext.AppendChild(seq);

            foreach (var prop in _properties)
            {
                prop.WriteXml(doc, seq);
            }

            if (id != null)
                currentNode.AppendChild(id);

            currentNode.AppendChild(ctype);
        }

        public void ReadXml(XmlNode node, XmlNamespaceManager mgr)
        {
 	        var abn = node.Attributes["abstract"];
            if (abn != null)
                this.IsAbstract = Convert.ToBoolean(abn.Value);

            //Process properties
            XmlNodeList propNodes = node.SelectNodes("xs:complexContent/xs:extension/xs:sequence/xs:element", mgr);
            if (propNodes.Count == 0)
                propNodes = node.SelectNodes("xs:sequence/xs:element", mgr);
            foreach (XmlNode propNode in propNodes)
            {
                var prop = PropertyDefinition.Parse(propNode, mgr);
                this.AddProperty(prop);
            }

            //Set designated geometry property
            var geom = node.Attributes["fdo:geometryName"];
            if (geom != null)
                this.DefaultGeometryPropertyName = geom.Value;

            //TODO: Base class

            //Process identity properties
            var parent = node.ParentNode;
            var key = parent.SelectSingleNode("xs:element[@name='" + this.Name + "']/xs:key", mgr);
            if (key != null)
            {
                var fields = key.SelectNodes("xs:field", mgr);
                foreach (XmlNode f in fields)
                {
                    var idpropa = f.Attributes["xpath"];
                    if (idpropa == null)
                        throw new Exception("Bad document. Expected attribute: xpath"); //LOCALIZEME

                    var prop = FindProperty(idpropa.Value);
                    if (prop != null && prop.Type == PropertyDefinitionType.Data)
                        _identity.Add((DataPropertyDefinition)prop);
                }
            }
        }
    }
}
