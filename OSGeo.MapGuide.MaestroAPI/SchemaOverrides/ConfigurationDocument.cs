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
using OSGeo.MapGuide.ObjectModels.Common;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public abstract class ConfigurationDocument : IFdoSerializable
    {
        protected List<IFdoSpatialContext> _spatialContexts;

        protected List<FeatureSchema> _schemas;

        public ConfigurationDocument() 
        { 
            _spatialContexts = new List<IFdoSpatialContext>();
            _schemas = new List<FeatureSchema>();
        }

        public void AddSpatialContext(IFdoSpatialContext context) { _spatialContexts.Add(context); }

        public void RemoveSpatialContext(IFdoSpatialContext context) { _spatialContexts.Remove(context); }

        public IFdoSpatialContext GetSpatialContext(string name)
        {
            foreach (var ctx in _spatialContexts)
            {
                if (ctx.Name.Equals(name))
                    return ctx;
            }
            return null;
        }

        public string[] GetSpatialContextNames()
        {
            List<string> names = new List<string>();

            foreach (var ctx in _spatialContexts)
            {
                names.Add(ctx.Name);
            }

            return names.ToArray();
        }

        public void AddSchema(FeatureSchema schema) { _schemas.Add(schema); }

        public void RemoveSchema(FeatureSchema schema) { _schemas.Remove(schema); }

        public FeatureSchema GetSchema(string name)
        {
            foreach (var fsc in _schemas)
            {
                if (fsc.Name.Equals(name))
                    return fsc;
            }
            return null;
        }

        public ClassDefinition GetClass(string schemaName, string className)
        {
            var fs = GetSchema(schemaName);
            return fs.GetClass(className);
        }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var dstore = doc.CreateElement("fdo", "DataStore", XmlNamespaces.FDO);
            dstore.SetAttribute("xmlns:xs", XmlNamespaces.XS);
            dstore.SetAttribute("xmlns:xsi", XmlNamespaces.XSI);
            dstore.SetAttribute("xmlns:xlink", XmlNamespaces.XLINK);
            dstore.SetAttribute("xmlns:gml", XmlNamespaces.GML);
            dstore.SetAttribute("xmlns:fdo", XmlNamespaces.FDO);
            dstore.SetAttribute("xmlns:fds", XmlNamespaces.FDS);
            foreach (var sc in _spatialContexts)
            {
                sc.WriteXml(doc, dstore);
            }
            foreach (var sc in _schemas)
            {
                sc.WriteXml(doc, dstore);
            }
            
            WriteSchemaMappings(doc, dstore);
            currentNode.AppendChild(dstore);
        }

        protected abstract void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode);

        protected abstract void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr);

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("fdo:DataStore"))
                throw new Exception("Bad document. Expected element fdo:DataStore"); //LOCALIZEME

            _spatialContexts.Clear();
            _schemas.Clear();

            XmlNodeList csNodes = node.SelectNodes("gml:DerivedCRS", mgr);
            foreach (XmlNode cs in csNodes)
            {
                var context = new FdoSpatialContextListSpatialContext();
                context.ReadXml(cs, mgr);

                AddSpatialContext(context);
            }

            XmlNodeList schemaNodes = node.SelectNodes("xs:schema", mgr);
            foreach (XmlNode sn in schemaNodes)
            {
                FeatureSchema fs = new FeatureSchema();
                fs.ReadXml(sn, mgr);
                AddSchema(fs);
            }

            ReadSchemaMappings(node, mgr);
        }

        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            this.WriteXml(doc, doc);
            return doc.OuterXml;
        }

        public static ConfigurationDocument LoadXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("xs", XmlNamespaces.XS);
            mgr.AddNamespace("xsi", XmlNamespaces.XSI);
            mgr.AddNamespace("fdo", XmlNamespaces.FDO);
            mgr.AddNamespace("gml", XmlNamespaces.GML);
            mgr.AddNamespace("xlink", XmlNamespaces.XLINK);
            mgr.AddNamespace("fds", XmlNamespaces.FDS);

            ConfigurationDocument conf = null;
            var root = doc.DocumentElement;
            if (root == null || root.Name != "fdo:DataStore")
                return null;

            //Sample the first schema mapping node. Even if there are multiples
            //they will all be the same provider

            //NOTE: Why does the XPath query (commented out) fail? 

            var map = root.LastChild; //root.SelectSingleNode("SchemaMapping"); 
            if (map != null && map.Name == "SchemaMapping")
            {
                var prov = map.Attributes["provider"];
                if (prov != null)
                {
                    if (prov.Value.StartsWith("OSGeo.ODBC"))
                        conf = new OdbcConfigurationDocument();
                    else if (prov.Value.StartsWith("OSGeo.Gdal"))
                        conf = new GdalConfigurationDocument();
                    else if (prov.Value.StartsWith("OSGeo.WMS"))
                        conf = new WmsConfigurationDocument();
                    else
                        conf = new GenericConfigurationDocument();
                }
            }

            if (conf != null)
            {
                conf.ReadXml(doc.SelectSingleNode("fdo:DataStore", mgr), mgr);
                return conf;
            }

            return null;
        }
    }
}
