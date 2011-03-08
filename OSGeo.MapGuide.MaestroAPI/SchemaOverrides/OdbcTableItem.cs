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
    public class OdbcTableItem : IFdoSerializable
    {
        internal OdbcConfigurationDocument Parent { get; set; }

        public string SchemaName { get; set; }

        public string ClassName { get; set; }

        public string SpatialContextName { get; set; }

        public string XColumn { get; set; }

        public string YColumn { get; set; }

        public string ZColumn { get; set; }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var cls = this.Parent.GetClass(this.SchemaName, this.ClassName);
            if (cls != null)
            {
                var ctype = doc.CreateElement("complexType");
                ctype.SetAttribute("name", this.ClassName + "Type");
                {
                    var table = doc.CreateElement("Table");
                    table.SetAttribute("name", this.ClassName);
                    {
                        PropertyDefinition geomProp = null;
                        foreach (var prop in cls.Properties)
                        {
                            //If this is geometry, we'll handle it later
                            if (prop.Name == cls.DefaultGeometryPropertyName)
                            {
                                geomProp = prop;
                                continue;
                            }

                            var el = doc.CreateElement("element");
                            el.SetAttribute("name", prop.Name);

                            var col = doc.CreateElement("Column");
                            col.SetAttribute("name", prop.Name);

                            el.AppendChild(col);
                            table.AppendChild(el);
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

                            table.AppendChild(el);
                        }
                    }
                    ctype.AppendChild(table);
                }
                currentNode.AppendChild(ctype);
            }
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("complexType"))
                throw new Exception("Bad document. Expected element: complexType"); //LOCALIZEME

            var sn = node.ParentNode.Attributes["name"];
            this.SchemaName = sn.Value;

            var cn = node.Attributes["name"];
            this.ClassName = cn.Value.Substring(0, cn.Value.Length - "Type".Length);

            var cls = this.Parent.GetClass(this.SchemaName, this.ClassName);
            if (!string.IsNullOrEmpty(cls.DefaultGeometryPropertyName))
            {
                GeometricPropertyDefinition geom = (GeometricPropertyDefinition)cls.FindProperty(cls.DefaultGeometryPropertyName);
                this.SpatialContextName = geom.SpatialContextAssociation;
            }

            var table = node["Table"];

            foreach (System.Xml.XmlNode el in table.ChildNodes)
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
            }
        }
    }
}
