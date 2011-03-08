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
    //TODO: Expand on documentation as this is an important class

    public class FeatureSchema : SchemaElement, IFdoSerializable
    {
        private List<ClassDefinition> _classes;

        internal FeatureSchema() { _classes = new List<ClassDefinition>(); }

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

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var schema = doc.CreateElement("xs", "schema", XmlNamespaces.XS);
            schema.SetAttribute("xmlns:xs", XmlNamespaces.XS);
            schema.SetAttribute("targetNamespace", XmlNamespaces.FDO  + "/feature/" + this.Name);
            schema.SetAttribute("xmlns:fdo", XmlNamespaces.FDO);
            schema.SetAttribute("xmlns:" + this.Name, XmlNamespaces.FDO + "/feature/" + this.Name);
            schema.SetAttribute("elementFormDefault", "qualified");
            schema.SetAttribute("attributeFormDefault", "unqualified");

            foreach (var cls in this.Classes)
            {
                cls.WriteXml(doc, schema);
            }

            currentNode.AppendChild(schema);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("xs:schema"))
                throw new Exception("Bad document. Expected element: xs:schema"); //LOCALIZEME

            var tns = node.Attributes["targetNamespace"];
            if (tns == null)
                throw new Exception("Bad document. Expected attribute: targetNamespace"); //LOCALIZEME

            int lidx = tns.Value.LastIndexOf("/") + 1;
            this.Name = tns.Value.Substring(lidx);

            //TODO: Description

            //Now handle classes
            if (node.ChildNodes.Count > 0)
            {
                XmlNodeList clsNodes = node.SelectNodes("xs:complexType", mgr);
                foreach (XmlNode clsNode in clsNodes)
                {
                    var nn = clsNode.Attributes["name"];
                    if (nn == null)
                        throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

                    string name = Utility.DecodeFDOName(nn.Value.Substring(0, nn.Value.Length - "Type".Length));
                    ClassDefinition cls = new ClassDefinition(name, string.Empty); //TODO: Description
                    cls.ReadXml(clsNode, mgr);
                    this.AddClass(cls);
                }
            }
        }
    }
}
