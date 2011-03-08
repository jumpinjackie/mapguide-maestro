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
    public class GdalConfigurationDocument : ConfigurationDocument
    {
        private Dictionary<string, GdalRasterLocationItem> _items = new Dictionary<string, GdalRasterLocationItem>();

        public GdalRasterLocationItem AddLocation(GdalRasterLocationItem item)
        {
            if (!_items.ContainsKey(item.Location))
            {
                _items.Add(item.Location, item);
            }
            return _items[item.Location];
        }

        public bool RemoveLocation(GdalRasterLocationItem item)
        {
            return _items.Remove(item.Location);
        }

        public GdalRasterLocationItem AddLocation(string directory)
        {
            if (_items.ContainsKey(directory))
                return _items[directory];

            return AddLocation(new GdalRasterLocationItem() { Location = directory });
        }

        public GdalRasterLocationItem[] RasterLocations { get { return new List<GdalRasterLocationItem>(_items.Values).ToArray(); } }

        protected override void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            foreach (var schema in base._schemas)
            {
                var map = doc.CreateElement("SchemaMapping");
                map.SetAttribute("provider", "OSGeo.Gdal.3.2");
                map.SetAttribute("xmlns", "http://fdogrfp.osgeo.org/schemas");
                map.SetAttribute("name", schema.Name);
                {
                    var ctype = doc.CreateElement("complexType");
                    var ctypeName = doc.CreateAttribute("name");
                    ctypeName.Value = schema.Name + "Type";
                    ctype.Attributes.Append(ctypeName);
                    {
                        var rasType = doc.CreateElement("complexType");
                        var rasTypeName = doc.CreateAttribute("name");
                        rasTypeName.Value = "RasterTypeType";
                        rasType.Attributes.Append(rasTypeName);
                        {
                            var rasDef = doc.CreateElement("RasterDefinition");
                            var rasDefName = doc.CreateAttribute("name");
                            rasDefName.Value = "images";
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

                if (!prv.Value.StartsWith("OSGeo.Gdal"))
                    continue;

                //XmlNodeList list = map.SelectNodes("complexType");
                foreach (XmlNode schemaMap in map.ChildNodes)
                {
                    if (schemaMap.Name != "complexType")
                        continue;

                    var schemaName = schemaMap.Attributes["name"].Value;
                    schemaName = schemaName.Substring(0, schemaName.Length - 4);
                    if (!SchemaExists(schemaName))
                        continue;

                    var rasterType = schemaMap.FirstChild;
                    var rasterDef = rasterType.FirstChild;

                    if (rasterType.Name != "complexType")
                        throw new Exception("Bad document. Expected element: complexType");

                    if (rasterDef.Name != "RasterDefinition")
                        throw new Exception("Bad document. Expected element: RasterDefinition");

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
