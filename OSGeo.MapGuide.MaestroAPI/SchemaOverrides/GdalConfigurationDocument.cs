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
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    /// <summary>
    /// A configuration document for the GDAL raster provider. A GDAL configuration document serves as a raster image catalog. Allowing
    /// the GDAL provider to optimally select the correct raster images to return for the given queried extents.
    /// </summary>
    public class GdalConfigurationDocument : ConfigurationDocument
    {
        private Dictionary<string, GdalRasterLocationItem> _items = new Dictionary<string, GdalRasterLocationItem>();

        /// <summary>
        /// Adds the specified raster location to this document
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public GdalRasterLocationItem AddLocation(GdalRasterLocationItem item)
        {
            if (!_items.ContainsKey(item.Location))
            {
                _items.Add(item.Location, item);
            }
            return _items[item.Location];
        }

        /// <summary>
        /// Removes the specified raster location from this document
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveLocation(GdalRasterLocationItem item)
        {
            return _items.Remove(item.Location);
        }

        /// <summary>
        /// Calculates the combined extent that encompasses all the raster images in this document.
        /// </summary>
        /// <returns></returns>
        public IEnvelope CalculateExtent()
        {
            IEnvelope env = null;
            foreach (var loc in _items.Values)
            {
                if (env == null)
                    env = loc.CalculateExtents();
                else
                    env.ExpandToInclude(loc.CalculateExtents());
            }
            return env;
        }

        /// <summary>
        /// Adds the specified directory to this document
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public GdalRasterLocationItem AddLocation(string directory)
        {
            if (_items.ContainsKey(directory))
                return _items[directory];

            return AddLocation(new GdalRasterLocationItem() { Location = directory });
        }

        /// <summary>
        /// Gets an array of all the raster locations for this document
        /// </summary>
        public GdalRasterLocationItem[] RasterLocations { get { return new List<GdalRasterLocationItem>(_items.Values).ToArray(); } }

        /// <summary>
        /// Write this document's schema mappings to the given XML document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            foreach (var schema in base._schemas)
            {
                var map = doc.CreateElement("SchemaMapping"); //NOXLATE
                map.SetAttribute("provider", "OSGeo.Gdal.3.2"); //NOXLATE
                map.SetAttribute("xmlns", "http://fdogrfp.osgeo.org/schemas"); //NOXLATE
                map.SetAttribute("name", schema.Name); //NOXLATE
                {
                    var ctype = doc.CreateElement("complexType"); //NOXLATE
                    var ctypeName = doc.CreateAttribute("name"); //NOXLATE
                    ctypeName.Value = schema.Name + "Type"; //NOXLATE
                    ctype.Attributes.Append(ctypeName);
                    {
                        var rasType = doc.CreateElement("complexType"); //NOXLATE
                        var rasTypeName = doc.CreateAttribute("name"); //NOXLATE
                        rasTypeName.Value = "RasterTypeType"; //NOXLATE
                        rasType.Attributes.Append(rasTypeName);
                        {
                            var rasDef = doc.CreateElement("RasterDefinition"); //NOXLATE
                            var rasDefName = doc.CreateAttribute("name"); //NOXLATE
                            rasDefName.Value = "images"; //NOXLATE
                            rasDef.Attributes.Append(rasDefName);

                            foreach (var loc in _items.Values)
                            {
                                loc.WriteXml(doc, rasDef);
                            }
                            rasType.AppendChild(rasDef);
                        }
                        ctype.AppendChild(rasType);
                    }
                    map.AppendChild(ctype);
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
            //XmlNodeList mappings = node.SelectNodes("SchemaMapping");
            foreach (XmlNode map in node.ChildNodes)
            {
                if (map.Name != "SchemaMapping") //NOXLATE
                    continue;

                var prv = map.Attributes["provider"]; //NOXLATE
                if (prv == null)
                    throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "provider"));

                var sn = map.Attributes["name"]; //NOXLATE
                if (sn == null)
                    throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name"));

                if (!prv.Value.StartsWith("OSGeo.Gdal")) //NOXLATE
                    continue;

                //XmlNodeList list = map.SelectNodes("complexType");
                foreach (XmlNode schemaMap in map.ChildNodes)
                {
                    if (schemaMap.Name != "complexType") //NOXLATE
                        continue;

                    var schemaName = schemaMap.Attributes["name"].Value; //NOXLATE
                    schemaName = schemaName.Substring(0, schemaName.Length - 4);
                    if (!SchemaExists(schemaName))
                        continue;

                    var rasterType = schemaMap.FirstChild;
                    var rasterDef = rasterType.FirstChild;

                    if (rasterType.Name != "complexType") //NOXLATE
                        throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "complexType"));

                    if (rasterDef.Name != "RasterDefinition") //NOXLATE
                        throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "RasterDefinition"));

                    foreach (XmlNode loc in rasterDef.ChildNodes)
                    {
                        var location = new GdalRasterLocationItem();
                        location.ReadXml(loc, mgr);

                        AddLocation(location);
                    }
                }
            }
        }

        private bool SchemaExists(string schemaName)
        {
            foreach (var schema in _schemas)
            {
                if (schema.Name == schemaName)
                    return true;
            }
            return false;
        }
    }
}
