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
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public class WmsConfigurationDocument : ConfigurationDocument
    {
        private List<RasterWmsItem> _rasterItems = new List<RasterWmsItem>();

        public RasterWmsItem[] RasterOverrides { get { return _rasterItems.ToArray(); } }

        public void AddRasterItem(RasterWmsItem item) { _rasterItems.Add(item); }

        public void RemoveRasterItem(RasterWmsItem item) { _rasterItems.Remove(item); }

        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var map = doc.CreateElement("SchemaMapping");
            map.SetAttribute("provider", "OSGeo.WMS.3.2");
            map.SetAttribute("xmlns", "http://fdowms.osgeo.org/schemas");
            map.SetAttribute("name", base._schemas[0].Name);
            {
                foreach(var ritem in _rasterItems)
                {
                    var ctype = doc.CreateElement("complexType");
                    var ctypeName = doc.CreateAttribute("name");
                    ctypeName.Value = ritem.FeatureClass + "Type";
                    ctype.Attributes.Append(ctypeName);
                    {
                        ritem.WriteXml(doc, ctype);
                    }
                    map.AppendChild(ctype);
                }
            }
            currentNode.AppendChild(map);
        }

        protected override void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            //XmlNodeList mappings = node.SelectNodes("SchemaMapping");
            foreach (XmlNode map in node.ChildNodes)
            {
                if (map.Name != "SchemaMapping")
                    continue;

                var prv = map.Attributes["provider"];
                if (prv == null)
                    throw new Exception("Bad document. Expected attribute: provider"); //LOCALIZEME

                var sn = map.Attributes["name"];
                if (sn == null)
                    throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

                //XmlNodeList list = map.SelectNodes("complexType");
                foreach (XmlNode clsMap in map.ChildNodes)
                {
                    if (clsMap.Name != "complexType")
                        continue;

                    var cn = clsMap.Attributes["name"];
                    if (cn == null)
                        throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

                    var rdf = clsMap.FirstChild;
                    if (rdf == null || rdf.Name != "RasterDefinition")
                        throw new Exception("Bad document. Expected element: RasterDefinition"); //LOCALIZEME

                    RasterWmsItem item = new RasterWmsItem();
                    item.ReadXml(rdf, mgr);

                    this.AddRasterItem(item);
                }
            }
        }
    }
}
