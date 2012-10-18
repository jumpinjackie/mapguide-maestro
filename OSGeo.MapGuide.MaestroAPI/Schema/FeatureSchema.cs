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
using System.Collections.ObjectModel;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    /// <summary>
    /// Contains all of the classes and relationships that make up a particular data model. This class is used to 
    /// represent the internal logical structure of a Feature Source
    /// </summary>
    public class FeatureSchema : SchemaElement, IFdoSerializable
    {
        private List<ClassDefinition> _classes;

        internal FeatureSchema() { _classes = new List<ClassDefinition>(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSchema"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public FeatureSchema(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets the class definitions
        /// </summary>
        public ReadOnlyCollection<ClassDefinition> Classes
        {
            get { return _classes.AsReadOnly(); }
        }

        /// <summary>
        /// Adds the specified class definition
        /// </summary>
        /// <param name="cls"></param>
        public void AddClass(ClassDefinition cls) 
        { 
            _classes.Add(cls);
            cls.Parent = this;
        }

        /// <summary>
        /// Removes the specified class definition
        /// </summary>
        /// <param name="cls"></param>
        /// <returns></returns>
        public bool RemoveClass(ClassDefinition cls) 
        {
            if (_classes.Remove(cls))
            {
                cls.Parent = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the Class Definition by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The matching Class Definition. null if it doesn't exist</returns>
        public ClassDefinition GetClass(string name)
        {
            foreach (var cls in _classes)
            {
                if (cls.Name.Equals(name))
                    return cls;
            }
            return null;
        }

        /// <summary>
        /// Gets the index of the specified class definition
        /// </summary>
        /// <param name="cls"></param>
        /// <returns></returns>
        public int IndexOf(ClassDefinition cls) { return _classes.IndexOf(cls); }

        /// <summary>
        /// Gets the class definition at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ClassDefinition this[int index]
        {
            get { return GetItem(index); }
        }

        /// <summary>
        /// Gets the class definition at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ClassDefinition GetItem(int index) { return _classes[index]; }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var schema = doc.CreateElement("xs", "schema", XmlNamespaces.XS); //NOXLATE
            schema.SetAttribute("xmlns:xs", XmlNamespaces.XS); //NOXLATE
            schema.SetAttribute("targetNamespace", XmlNamespaces.FDO + "/feature/" + this.Name); //NOXLATE
            schema.SetAttribute("xmlns:fdo", XmlNamespaces.FDO); //NOXLATE
            schema.SetAttribute("xmlns:" + this.Name, XmlNamespaces.FDO + "/feature/" + this.Name); //NOXLATE
            schema.SetAttribute("elementFormDefault", "qualified"); //NOXLATE
            schema.SetAttribute("attributeFormDefault", "unqualified"); //NOXLATE

            //Write description node
            var anno = doc.CreateElement("xs", "annotation", XmlNamespaces.XS); //NOXLATE
            var docN = doc.CreateElement("xs", "documentation", XmlNamespaces.XS); //NOXLATE
            docN.InnerText = this.Description;
            schema.AppendChild(anno);
            anno.AppendChild(docN);

            foreach (var cls in this.Classes)
            {
                cls.WriteXml(doc, schema);
            }

            currentNode.AppendChild(schema);
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("xs:schema")) //NOXLATE
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "xs:schema"));

            //Description
            var docNode = node.SelectSingleNode("xs:annotation/xs:documentation", mgr); //NOXLATE
            if (docNode != null)
                this.Description = docNode.InnerText;

            var tns = node.Attributes["targetNamespace"];
            if (tns == null)
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "targetNamespace"));

            int lidx = tns.Value.LastIndexOf("/") + 1; //NOXLATE
            this.Name = tns.Value.Substring(lidx);

            //TODO: Description

            //Now handle classes
            if (node.ChildNodes.Count > 0)
            {
                XmlNodeList clsNodes = node.SelectNodes("xs:complexType", mgr); //NOXLATE
                foreach (XmlNode clsNode in clsNodes)
                {
                    var nn = clsNode.Attributes["name"]; //NOXLATE
                    if (nn == null)
                        throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name")); //NOXLATE

                    string name = Utility.DecodeFDOName(nn.Value.Substring(0, nn.Value.Length - "Type".Length)); //NOXLATE
                    ClassDefinition cls = new ClassDefinition(name, string.Empty); //TODO: Description
                    cls.ReadXml(clsNode, mgr);
                    this.AddClass(cls);
                }
            }
        }

        /// <summary>
        /// Removes a class definition by its name
        /// </summary>
        /// <param name="className"></param>
        public void RemoveClass(string className)
        {
            var cls = GetClass(className);
            if (cls != null)
                RemoveClass(cls);
        }

        /// <summary>
        /// Creates a clone of the specified instance
        /// </summary>
        /// <param name="fs">The instance to clone</param>
        /// <returns></returns>
        public static FeatureSchema Clone(FeatureSchema fs)
        {
            var clone = new FeatureSchema(fs.Name, fs.Description);
            foreach (var cls in fs.Classes)
            {
                clone.AddClass(ClassDefinition.Clone(cls));
            }
            return clone;
        }
    }
}
