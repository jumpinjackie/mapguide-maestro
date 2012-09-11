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
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    /// <summary>
    /// Represents a configuration document for the ODBC provider. The ODBC configuration document allows you to declare
    /// certain tables as point feature classes by specifying the X, Y and optionally Z columns of the table
    /// </summary>
    public class OdbcConfigurationDocument : ConfigurationDocument
    {
        private List<OdbcTableItem> _tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcConfigurationDocument"/> class.
        /// </summary>
        public OdbcConfigurationDocument() { _tables = new List<OdbcTableItem>(); }

        /// <summary>
        /// Adds the specified table override
        /// </summary>
        /// <param name="item"></param>
        public void AddOverride(OdbcTableItem item) 
        { 
            _tables.Add(item);
            item.Parent = this;
        }

        /// <summary>
        /// Removes all table overrides
        /// </summary>
        public void ClearMappings() { _tables.Clear(); }

        /// <summary>
        /// Gets the table overrides for the specified schema
        /// </summary>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public List<OdbcTableItem> GetMappingsForSchema(string schemaName)
        {
            List<OdbcTableItem> items = new List<OdbcTableItem>();
            foreach (var item in _tables)
            {
                if (item.SchemaName.Equals(schemaName))
                    items.Add(item);
            }
            return items;
        }

        /*
        public override void WriteXml(XmlDocument doc, XmlNode currentNode)
        {
            //
            foreach (var schema in this.Schemas)
            {
                foreach (var cls in schema.Classes)
                {
                    var ov = this.GetOverride(schema.Name, cls.Name);
                    if (ov != null)
                    {
                        if (!string.IsNullOrEmpty(ov.XColumn))
                        {
                            cls.RemoveProperty(ov.XColumn);
                        }
                        if (!string.IsNullOrEmpty(ov.YColumn))
                        {
                            cls.RemoveProperty(ov.YColumn);
                        }
                        if (!string.IsNullOrEmpty(ov.ZColumn))
                        {
                            cls.RemoveProperty(ov.ZColumn);
                        }
                    }
                }
            }
            base.WriteXml(doc, currentNode);
        }*/

        /// <summary>
        /// Write this document's schema mappings to the given XML document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            foreach (var fs in _schemas)
            {
                var map = doc.CreateElement("SchemaMapping"); //NOXLATE
                //The version is required for data compatiblity with FDO. I don't think
                //the actual value matters. So use a safe version of FDO
                map.SetAttribute("provider", "OSGeo.ODBC.3.2"); //NOXLATE
                map.SetAttribute("xmlns:rdb", "http://fdordbms.osgeo.org/schemas"); //NOXLATE
                map.SetAttribute("xmlns", "http://fdoodbc.osgeo.org/schemas"); //NOXLATE
                map.SetAttribute("name", fs.Name); //NOXLATE
                var items = GetMappingsForSchema(fs.Name);
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        item.WriteXml(doc, map);
                    }
                }
                currentNode.AppendChild(map);
            }
        }

        /// <summary>
        /// Write this document's schema mappings from the given XML document
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="mgr">The namespace manager.</param>
        protected override void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            //var mappings = node.SelectNodes("SchemaMapping", mgr);
            foreach (XmlNode map in node.ChildNodes)
            {
                if (map.Name != "SchemaMapping") //NOXLATE
                    continue;

                var sn = map.Attributes["name"]; //NOXLATE
                if (sn == null)
                    throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name"));

                foreach (XmlNode clsMap in map.ChildNodes)
                {
                    if (clsMap.Name != "complexType") //NOXLATE
                        continue;

                    var item = new OdbcTableItem();
                    item.Parent = this;
                    item.SchemaName = sn.Value;
                    item.ReadXml(clsMap, mgr);

                    AddOverride(item);
                }
            }
        }

        /// <summary>
        /// Gets the table override for the specific feature class name.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public OdbcTableItem GetOverride(string schemaName, string className)
        {
            foreach (var item in _tables)
            {
                if (item.SchemaName.Equals(schemaName) && item.ClassName.Equals(className))
                    return item;
            }
            return null;
        }
    }
}
