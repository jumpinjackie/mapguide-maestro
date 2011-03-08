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
    public class OdbcConfigurationDocument : ConfigurationDocument
    {
        private List<OdbcTableItem> _tables;

        public OdbcConfigurationDocument() { _tables = new List<OdbcTableItem>(); }

        public void AddOverride(OdbcTableItem item) 
        { 
            _tables.Add(item);
            item.Parent = this;
        }

        public void ClearMappings() { _tables.Clear(); }

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

        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            foreach (var fs in _schemas)
            {
                var items = GetMappingsForSchema(fs.Name);
                if (items.Count > 0)
                {
                    var map = doc.CreateElement("SchemaMapping");
                    //The version is required for data compatiblity with FDO. I don't think
                    //the actual value matters. So use a safe version of FDO
                    map.SetAttribute("provider", "OSGeo.ODBC.3.2"); 
                    map.SetAttribute("xmlns:rdb", "http://fdordbms.osgeo.org/schemas");
                    map.SetAttribute("xmlns", "http://fdoodbc.osgeo.org/schemas");
                    map.SetAttribute("name", fs.Name);

                    foreach (var item in items)
                    {
                        item.WriteXml(doc, map);
                    }

                    currentNode.AppendChild(map);
                }
            }
        }

        protected override void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            //var mappings = node.SelectNodes("SchemaMapping", mgr);
            foreach (XmlNode map in node.ChildNodes)
            {
                if (map.Name != "SchemaMapping")
                    continue;

                var sn = map.Attributes["name"];
                if (sn == null)
                    throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

                //XmlNodeList clsMaps = map.SelectNodes("complexType");
                foreach (XmlNode clsMap in map.ChildNodes)
                {
                    if (clsMap.Name != "complexType")
                        continue;

                    var item = new OdbcTableItem();
                    item.Parent = this;
                    item.SchemaName = sn.Value;
                    item.ReadXml(clsMap, mgr);

                    AddOverride(item);
                }
            }
        }

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
