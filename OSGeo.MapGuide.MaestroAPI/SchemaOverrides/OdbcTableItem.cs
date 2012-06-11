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
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    /// <summary>
    /// Represents a table override. A table override allows a table to be configured as a point feature class
    /// </summary>
    public class OdbcTableItem : IFdoSerializable
    {
        internal OdbcConfigurationDocument Parent { get; set; }

        /// <summary>
        /// Gets or sets the name of the feature schema
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the name of the feature class this override is applicable to
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the spatial context the point geometries will be applicable to
        /// </summary>
        public string SpatialContextName { get; set; }

        /// <summary>
        /// Gets or sets the name of the column which contains the X coordinates of the point features
        /// </summary>
        public string XColumn { get; set; }

        /// <summary>
        /// Gets or sets the name of the column which contains the Y coordinates of the point features
        /// </summary>
        public string YColumn { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the column which contains the Z coordinates of the point features
        /// </summary>
        public string ZColumn { get; set; }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var cls = this.Parent.GetClass(this.SchemaName, this.ClassName);
            if (cls != null)
            {
                var ctype = doc.CreateElement("complexType");
                ctype.SetAttribute("name", Utility.EncodeFDOName(this.ClassName) + "Type");
                {
                    var table = doc.CreateElement("Table");
                    table.SetAttribute("name", this.ClassName);
                    ctype.AppendChild(table);

                    PropertyDefinition geomProp = null;
                    foreach (var prop in cls.Properties)
                    {
                        //If this is geometry, we'll handle it later
                        if (prop.Name == cls.DefaultGeometryPropertyName)
                        {
                            geomProp = prop;
                            continue;
                        }

                        //If this is a geometry mapped X/Y/Z property, skip it
                        if (prop.Name == this.XColumn ||
                            prop.Name == this.YColumn ||
                            prop.Name == this.ZColumn)
                            continue;

                        var el = doc.CreateElement("element");
                        el.SetAttribute("name", Utility.EncodeFDOName(prop.Name));

                        var col = doc.CreateElement("Column");
                        col.SetAttribute("name", prop.Name);

                        el.AppendChild(col);
                        ctype.AppendChild(el);
                    }

                    //Append geometry mapping
                    if (geomProp != null)
                    {
                        var el = doc.CreateElement("element");
                        el.SetAttribute("name", geomProp.Name);

                        if (!string.IsNullOrEmpty(this.XColumn))
                            el.SetAttribute("xColumnName", this.XColumn);
                        if (!string.IsNullOrEmpty(this.YColumn))
                            el.SetAttribute("yColumnName", this.YColumn);
                        if (!string.IsNullOrEmpty(this.ZColumn))
                            el.SetAttribute("zColumnName", this.ZColumn);

                        ctype.AppendChild(el);
                    }
                }
                currentNode.AppendChild(ctype);
            }
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("complexType"))
                throw new Exception("Bad document. Expected element: complexType"); //LOCALIZEME

            var sn = node.ParentNode.Attributes["name"];
            this.SchemaName = sn.Value;

            var cn = node.Attributes["name"];
            this.ClassName = Utility.DecodeFDOName(cn.Value.Substring(0, cn.Value.Length - "Type".Length));

            var cls = this.Parent.GetClass(this.SchemaName, this.ClassName);
            if (!string.IsNullOrEmpty(cls.DefaultGeometryPropertyName))
            {
                GeometricPropertyDefinition geom = (GeometricPropertyDefinition)cls.FindProperty(cls.DefaultGeometryPropertyName);
                this.SpatialContextName = geom.SpatialContextAssociation;
            }

            var table = node["Table"];
            var el = table.NextSibling;
            //foreach (System.Xml.XmlNode el in table.ChildNodes)
            while(el != null)
            {
                var colName = el.Attributes["name"];

                if (colName.Value == cls.DefaultGeometryPropertyName)
                {
                    var x = el.Attributes["xColumnName"];
                    var y = el.Attributes["yColumnName"];
                    var z = el.Attributes["zColumnName"];

                    if (x != null)
                        this.XColumn = x.Value;
                    if (y != null)
                        this.YColumn = y.Value;
                    if (z != null)
                        this.ZColumn = z.Value;
                }
                el = el.NextSibling;
            }
        }
    }
}
