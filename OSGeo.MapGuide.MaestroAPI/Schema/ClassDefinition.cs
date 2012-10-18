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

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public ClassDefinition(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the base class
        /// </summary>
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

        /// <summary>
        /// Gets the ordinal of the specified property name
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns></returns>
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

            throw new ArgumentException(string.Format(MaestroAPI.Strings.ErrorPropertyNotFound, name));
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
        /// Removes the property definition of the specified name. If it is a data property
        /// it is also removed from the identity properties (if it is specified as one)
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveProperty(string propertyName)
        {
            int idx = -1;
            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Name == propertyName)
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0)
                RemovePropertyAt(idx);
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

        /// <summary>
        /// Gets the parent schema
        /// </summary>
        public FeatureSchema Parent { get; internal set; }

        /// <summary>
        /// Gets a Property Definition by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The matching property definition. null if none found</returns>
        public PropertyDefinition FindProperty(string name)
        {
            foreach (var prop in _properties)
            {
                if (prop.Name.Equals(name))
                    return prop;
            }
            return null;
        }

        /// <summary>
        /// Gets the qualified name of this class. The qualified name takes the form [Schema Name]:[Class Name]
        /// </summary>
        public string QualifiedName { get { return this.Parent != null ? this.Parent.Name + ":" + this.Name : this.Name; } } //NOXLATE

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(XmlDocument doc, XmlNode currentNode)
        {
            XmlElement id = null;

            var en = Utility.EncodeFDOName(this.Name);
            if (_identity.Count > 0)
            {
                id = doc.CreateElement("xs", "element", XmlNamespaces.XS); //NOXLATE

                //TODO: May need encoding
                id.SetAttribute("name", en); //NOXLATE
                id.SetAttribute("type", this.Parent.Name + ":" + en + "Type"); //NOXLATE
                id.SetAttribute("abstract", this.IsAbstract.ToString().ToLower()); //NOXLATE
                id.SetAttribute("substitutionGroup", "gml:_Feature"); //NOXLATE

                var key = doc.CreateElement("xs", "key", XmlNamespaces.XS); //NOXLATE
                key.SetAttribute("name", en + "Key"); //NOXLATE

                var selector = doc.CreateElement("xs", "selector", XmlNamespaces.XS); //NOXLATE
                selector.SetAttribute("xpath", ".//" + en); //NOXLATE

                key.AppendChild(selector);

                foreach (var prop in _identity)
                {
                    var field = doc.CreateElement("xs", "field", XmlNamespaces.XS); //NOXLATE
                    field.SetAttribute("xpath", prop.Name); //NOXLATE

                    key.AppendChild(field);
                }
                id.AppendChild(key);
            }

            //Now write class body
            var ctype = doc.CreateElement("xs", "complexType", XmlNamespaces.XS); //NOXLATE
            //TODO: This may have been decoded. Should it be re-encoded?
            ctype.SetAttribute("name", en + "Type"); //NOXLATE
            ctype.SetAttribute("abstract", this.IsAbstract.ToString().ToLower()); //NOXLATE
            if (!string.IsNullOrEmpty(this.DefaultGeometryPropertyName))
            {
                var geom = FindProperty(this.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
                if (geom != null)
                {
                    ctype.SetAttribute("geometryName", XmlNamespaces.FDO, geom.Name); //NOXLATE
                }
            }
            else
            {
                ctype.SetAttribute("hasGeometry", XmlNamespaces.FDO, "false"); //NOXLATE
            }

            //Write description node
            var anno = doc.CreateElement("xs", "annotation", XmlNamespaces.XS); //NOXLATE
            var docN = doc.CreateElement("xs", "documentation", XmlNamespaces.XS); //NOXLATE
            docN.InnerText = this.Description;
            ctype.AppendChild(anno);
            anno.AppendChild(docN);

            var cnt = doc.CreateElement("xs", "complexContent", XmlNamespaces.XS); //NOXLATE
            ctype.AppendChild(cnt);

            var ext = doc.CreateElement("xs", "extension", XmlNamespaces.XS); //NOXLATE
            if (this.BaseClass != null)
                ext.SetAttribute("base", this.BaseClass.QualifiedName); //NOXLATE
            else
                ext.SetAttribute("base", "gml:AbstractFeatureType"); //NOXLATE
            cnt.AppendChild(ext);

            var seq = doc.CreateElement("xs", "sequence", XmlNamespaces.XS); //NOXLATE
            ext.AppendChild(seq);

            foreach (var prop in _properties)
            {
                prop.WriteXml(doc, seq);
            }

            if (id != null)
                currentNode.AppendChild(id);

            currentNode.AppendChild(ctype);
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(XmlNode node, XmlNamespaceManager mgr)
        {
            var en = Utility.EncodeFDOName(this.Name);
            var abn = node.Attributes["abstract"]; //NOXLATE
            if (abn != null)
                this.IsAbstract = Convert.ToBoolean(abn.Value);

            //Description
            var docNode = node.SelectSingleNode("xs:annotation/xs:documentation", mgr); //NOXLATE
            if (docNode != null)
                this.Description = docNode.InnerText;

            //Process properties
            XmlNodeList propNodes = node.SelectNodes("xs:complexContent/xs:extension/xs:sequence/xs:element", mgr); //NOXLATE
            if (propNodes.Count == 0)
                propNodes = node.SelectNodes("xs:sequence/xs:element", mgr); //NOXLATE
            foreach (XmlNode propNode in propNodes)
            {
                var prop = PropertyDefinition.Parse(propNode, mgr);
                this.AddProperty(prop);
            }

            //Set designated geometry property
            var geom = Utility.GetFdoAttribute(node, "geometryName"); //NOXLATE

            if (geom != null)
                this.DefaultGeometryPropertyName = geom.Value;

            //TODO: Base class

            //Process identity properties
            var parent = node.ParentNode;
            //This is a lower-case coerced xpath query as our encoded name for querying may not be of the correct case
            var xpath = "xs:element[translate(@name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"" + en.ToLower() + "\"]/xs:key"; //NOXLATE
            var key = parent.SelectSingleNode(xpath, mgr);
            if (key != null)
            {
                var fields = key.SelectNodes("xs:field", mgr); //NOXLATE
                foreach (XmlNode f in fields)
                {
                    var idpropa = f.Attributes["xpath"]; //NOXLATE
                    if (idpropa == null)
                        throw new Exception(string.Format(MaestroAPI.Strings.ErrorBadDocumentExpectedAttribute, "xpath"));

                    var prop = FindProperty(idpropa.Value);
                    if (prop != null && prop.Type == PropertyDefinitionType.Data)
                        _identity.Add((DataPropertyDefinition)prop);
                }
            }
        }

        /// <summary>
        /// Creates a clone of the specified instance
        /// </summary>
        /// <param name="source">The instance to clone.</param>
        /// <returns></returns>
        public static ClassDefinition Clone(ClassDefinition source)
        {
            var clone = new ClassDefinition(source.Name, source.Description);
            foreach (var prop in source.Properties)
            {
                var clonedProp = PropertyDefinition.Clone(prop);
                if (clonedProp.Type == PropertyDefinitionType.Data &&
                    source.IdentityProperties.Contains((DataPropertyDefinition)prop))
                {
                    clone.AddProperty((DataPropertyDefinition)clonedProp, true);
                }
                else
                {
                    clone.AddProperty(clonedProp);
                }
            }
            clone.DefaultGeometryPropertyName = source.DefaultGeometryPropertyName;
            clone.IsAbstract = source.IsAbstract;
            clone.IsComputed = source.IsComputed;
            if (source.Parent != null)
                clone.Parent = new FeatureSchema(source.Parent.Name, source.Parent.Description);
            //TODO: Base Class
            return clone;
        }
    }
}
