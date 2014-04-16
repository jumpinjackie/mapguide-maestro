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
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
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
            //TODO: Is WMS multi-schema? We should factor this in
            map.SetAttribute("name", base._schemas[0].Name); //NOXLATE
            {
                foreach(var ritem in _rasterItems)
                {
                    if (ritem.SchemaName != base._schemas[0].Name)
                        continue;

                    var ctype = doc.CreateElement("complexType"); //NOXLATE
                    var ctypeName = doc.CreateAttribute("name"); //NOXLATE
                    ctypeName.Value = Utility.EncodeFDOName(ritem.FeatureClass) + "Type"; //NOXLATE
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
                    throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "provider"));

                var sn = map.Attributes["name"];
                if (sn == null)
                    throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name"));

                foreach (XmlNode clsMap in map.ChildNodes)
                {
                    if (clsMap.Name != "complexType") //NOXLATE
                        continue;

                    var cn = clsMap.Attributes["name"]; //NOXLATE
                    if (cn == null)
                        throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name"));

                    var rdf = clsMap.FirstChild;
                    if (rdf == null || rdf.Name != "RasterDefinition")
                        throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "RasterDefinition"));

                    RasterWmsItem item = new RasterWmsItem();
                    item.SchemaName = sn.Value;
                    item.ReadXml(rdf, mgr);

                    this.AddRasterItem(item);
                }
            }
        }

        /// <summary>
        /// Removes any logical classes without physical mappings and vice versa, also ensures that the physical mapping refers
        /// to the correct logical raster property
        /// </summary>
        public void EnsureConsistency()
        {
            var removeClasses = new List<ClassDefinition>();
            var removeMappings = new List<RasterWmsItem>();
            foreach (var mapping in this.RasterOverrides)
            {
                var cls = this.GetClass(mapping.SchemaName, mapping.FeatureClass);
                if (cls == null)
                    removeMappings.Add(mapping);
            }
            //Triple nested loop? You know what they say about Big-O. If n is usually small
            //don't bother trying to optimize.
            foreach (var schema in this.Schemas)
            {
                foreach (var cls in schema.Classes)
                {
                    bool bFound = false;
                    foreach (var mapping in this.RasterOverrides)
                    {
                        if (mapping.SchemaName == schema.Name && mapping.FeatureClass == cls.Name)
                        {
                            bFound = true;
                            //Since we're here. Fix up the raster logical property if there's a mismatch
                            foreach (var prop in cls.Properties)
                            {
                                if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                                {
                                    if (prop.Name != mapping.RasterPropertyName)
                                        mapping.RasterPropertyName = prop.Name;
                                }
                            }
                            break;
                        }
                    }
                    if (!bFound)
                        removeClasses.Add(cls);
                }
            }
            foreach (var mapping in removeMappings)
            {
                this.RemoveRasterItem(mapping);
            }
            foreach (var cls in removeClasses)
            {
                var schema = cls.Parent;
                schema.RemoveClass(cls);
            }
        }

        /// <summary>
        /// Gets the default spatial context from this configuration document. If none is found, the first spatial
        /// context from the given Feature Source is used
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public string GetDefaultSpatialContext(IFeatureSource fs)
        {
            //BOGUS: This was not as sufficient as I originally thought, nevertheless this contains
            //information that would not exist if we constructed the document the old fashioned way.
            string defaultScName = string.Empty;
            if (this.SpatialContexts.Length > 0)
            {
                defaultScName = this.SpatialContexts[0].Name;
            }
            else
            {
                var list = fs.GetSpatialInfo(false);
                if (list.SpatialContext.Count > 0)
                {
                    defaultScName = list.SpatialContext[0].Name;
                }
                else //Really? What kind of WMS service are you????
                {
                    var sc = new FdoSpatialContextListSpatialContext()
                    {
                        Name = "EPSG:4326", //NOXLATE
                        Description = "Maestro-generated spatial context", //NOXLATE
                        CoordinateSystemName = "EPSG:4326", //NOXLATE
                        CoordinateSystemWkt = "GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722293]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.01745329251994]]", //NOXLATE
                        Extent = new FdoSpatialContextListSpatialContextExtent()
                        {
                            LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                            {
                                X = "-180.0", //NOXLATE
                                Y = "-90.0" //NOXLATE
                            },
                            UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                            {
                                X = "180.0", //NOXLATE
                                Y = "90.0" //NOXLATE
                            }
                        },
                        ExtentType = FdoSpatialContextListSpatialContextExtentType.Static,
                        IsActive = true,
                        XYTolerance = 0.0001,
                        ZTolerance = 0.0001,
                    };
                    this.AddSpatialContext(sc);
                    defaultScName = sc.Name;
                }
            }
            return defaultScName;
        }

        /// <summary>
        /// Ensures that classes in this document have an identity property and a raster property. Any classes which
        /// have neither, will have properties created for them
        /// </summary>
        /// <param name="defaultScName">The name of the default spatial context</param>
        public void EnsureRasterProperties(string defaultScName)
        {
            foreach (var schema in this.Schemas)
            {
                foreach (var cls in schema.Classes)
                {
                    //Add identity property if none found
                    if (cls.IdentityProperties.Count == 0)
                    {
                        cls.AddProperty(new DataPropertyDefinition("Id", string.Empty) //NOXLATE
                        {
                            DataType = DataPropertyType.String,
                            Length = 256,
                            IsNullable = false
                        }, true);
                    }
                    //Add raster property if there's only one property (the identity property we either just added or found)
                    if (cls.Properties.Count == 1)
                    {
                        cls.AddProperty(new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
                        {
                            DefaultImageXSize = 1024,
                            DefaultImageYSize = 1024,
                            SpatialContextAssociation = defaultScName
                        });
                    }
                    else
                    {
                        bool bFoundRaster = false;
                        //Try to find this raster property
                        foreach (var prop in cls.Properties)
                        {
                            if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                            {
                                bFoundRaster = true;
                                break;
                            }
                        }
                        if (!bFoundRaster)
                        {
                            cls.AddProperty(new RasterPropertyDefinition("Image", string.Empty) //NOXLATE
                            {
                                DefaultImageXSize = 1024,
                                DefaultImageYSize = 1024,
                                SpatialContextAssociation = defaultScName
                            });
                        }
                    }
                }
            }
        }
    }
}
