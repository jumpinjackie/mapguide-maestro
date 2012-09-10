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
    /// <summary>
    /// Represents a configuration document for the WMS provider.
    /// </summary>
    public class WmsConfigurationDocument : ConfigurationDocument
    {
        private List<RasterWmsItem> _rasterItems = new List<RasterWmsItem>();

        /// <summary>
        /// Gets an array of the added override items
        /// </summary>
        public RasterWmsItem[] RasterOverrides { get { return _rasterItems.ToArray(); } }

        /// <summary>
        /// Adds the specified override item
        /// </summary>
        /// <param name="item"></param>
        public void AddRasterItem(RasterWmsItem item) { _rasterItems.Add(item); }

        /// <summary>
        /// Removes the specified override item
        /// </summary>
        /// <param name="item"></param>
        public void RemoveRasterItem(RasterWmsItem item) { _rasterItems.Remove(item); }

        /// <summary>
        /// Write this document's schema mappings to the given XML document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var map = doc.CreateElement("SchemaMapping"); //NOXLATE
            map.SetAttribute("provider", "OSGeo.WMS.3.2"); //NOXLATE
            map.SetAttribute("xmlns", "http://fdowms.osgeo.org/schemas"); //NOXLATE
            map.SetAttribute("name", base._schemas[0].Name); //NOXLATE
            {
                foreach(var ritem in _rasterItems)
                {
                    var ctype = doc.CreateElement("complexType"); //NOXLATE
                    var ctypeName = doc.CreateAttribute("name"); //NOXLATE
                    ctypeName.Value = ritem.FeatureClass + "Type"; //NOXLATE
                    ctype.Attributes.Append(ctypeName);
                    {
                        ritem.WriteXml(doc, ctype);
                    }
                    map.AppendChild(ctype);
                }
            }
            currentNode.AppendChild(map);
        }

        /// <summary>
        /// Write this document's schema mappings from the given XML document
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="mgr">The namespace manager.</param>
        protected override void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            foreach (XmlNode map in node.ChildNodes)
            {
                if (map.Name != "SchemaMapping") //NOXLATE
                    continue;

                var prv = map.Attributes["provider"]; //NOXLATE
                if (prv == null)
                    throw new Exception(string.Format(Properties.Resources.ErrorBadDocumentExpectedAttribute, "provider"));

                var sn = map.Attributes["name"];
                if (sn == null)
                    throw new Exception(string.Format(Properties.Resources.ErrorBadDocumentExpectedAttribute, "name"));

                foreach (XmlNode clsMap in map.ChildNodes)
                {
                    if (clsMap.Name != "complexType") //NOXLATE
                        continue;

                    var cn = clsMap.Attributes["name"]; //NOXLATE
                    if (cn == null)
                        throw new Exception(string.Format(Properties.Resources.ErrorBadDocumentExpectedAttribute, "name"));

                    var rdf = clsMap.FirstChild;
                    if (rdf == null || rdf.Name != "RasterDefinition")
                        throw new Exception(string.Format(Properties.Resources.ErrorBadDocumentExpectedElement, "RasterDefinition"));

                    RasterWmsItem item = new RasterWmsItem();
                    item.ReadXml(rdf, mgr);

                    this.AddRasterItem(item);
                }
            }
        }
    }
}
