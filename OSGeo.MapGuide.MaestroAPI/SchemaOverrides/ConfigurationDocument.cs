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
    /// <summary>
    /// The base class of all configuration documents
    /// </summary>
    public abstract class ConfigurationDocument : IFdoSerializable
    {
        /// <summary>
        /// The list of spatial contexts
        /// </summary>
        protected List<IFdoSpatialContext> _spatialContexts;

        /// <summary>
        /// The list of logical schemas
        /// </summary>
        protected List<FeatureSchema> _schemas;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDocument"/> class.
        /// </summary>
        protected ConfigurationDocument() 
        { 
            _spatialContexts = new List<IFdoSpatialContext>();
            _schemas = new List<FeatureSchema>();
        }

        /// <summary>
        /// Gets the array of spatial contexts.
        /// </summary>
        public IFdoSpatialContext[] SpatialContexts { get { return _spatialContexts.ToArray(); } }

        /// <summary>
        /// Gets the array of logical schemas.
        /// </summary>
        public FeatureSchema[] Schemas { get { return _schemas.ToArray(); } }

        /// <summary>
        /// Adds the spatial context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void AddSpatialContext(IFdoSpatialContext context) { _spatialContexts.Add(context); }

        /// <summary>
        /// Removes the spatial context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void RemoveSpatialContext(IFdoSpatialContext context) { _spatialContexts.Remove(context); }

        /// <summary>
        /// Gets the spatial context by name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IFdoSpatialContext GetSpatialContext(string name)
        {
            foreach (var ctx in _spatialContexts)
            {
                if (ctx.Name.Equals(name))
                    return ctx;
            }
            return null;
        }

        /// <summary>
        /// Gets the array spatial context names.
        /// </summary>
        /// <returns></returns>
        public string[] GetSpatialContextNames()
        {
            List<string> names = new List<string>();

            foreach (var ctx in _spatialContexts)
            {
                names.Add(ctx.Name);
            }

            return names.ToArray();
        }

        /// <summary>
        /// Adds the logical schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public void AddSchema(FeatureSchema schema) { _schemas.Add(schema); }

        /// <summary>
        /// Removes the logical schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public void RemoveSchema(FeatureSchema schema) { _schemas.Remove(schema); }

        /// <summary>
        /// Gets the schema by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FeatureSchema GetSchema(string name)
        {
            foreach (var fsc in _schemas)
            {
                if (fsc.Name.Equals(name))
                    return fsc;
            }
            return null;
        }

        /// <summary>
        /// Gets the class definition by schema and class names
        /// </summary>
        /// <param name="schemaName">Name of the schema.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public ClassDefinition GetClass(string schemaName, string className)
        {
            var fs = GetSchema(schemaName);
            return fs.GetClass(className);
        }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public virtual void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var dstore = doc.CreateElement("fdo", "DataStore", XmlNamespaces.FDO); //NOXLATE
            dstore.SetAttribute("xmlns:xs", XmlNamespaces.XS); //NOXLATE
            dstore.SetAttribute("xmlns:xsi", XmlNamespaces.XSI); //NOXLATE
            dstore.SetAttribute("xmlns:xlink", XmlNamespaces.XLINK); //NOXLATE
            dstore.SetAttribute("xmlns:gml", XmlNamespaces.GML); //NOXLATE
            dstore.SetAttribute("xmlns:fdo", XmlNamespaces.FDO); //NOXLATE
            dstore.SetAttribute("xmlns:fds", XmlNamespaces.FDS); //NOXLATE
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

        /// <summary>
        /// Write this document's schema mappings to the given XML document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        protected abstract void WriteSchemaMappings(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode);

        /// <summary>
        /// Write this document's schema mappings from the given XML document
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="mgr">The namespace manager.</param>
        protected abstract void ReadSchemaMappings(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr);

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("fdo:DataStore")) //NOXLATE
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "fdo:DataStore")); //NOXLATE

            _spatialContexts.Clear();
            _schemas.Clear();

            XmlNodeList csNodes = node.SelectNodes("gml:DerivedCRS", mgr); //NOXLATE
            foreach (XmlNode cs in csNodes)
            {
                var context = new FdoSpatialContextListSpatialContext();
                context.ReadXml(cs, mgr);

                AddSpatialContext(context);
            }

            XmlNodeList schemaNodes = node.SelectNodes("xs:schema", mgr); //NOXLATE
            foreach (XmlNode sn in schemaNodes)
            {
                FeatureSchema fs = new FeatureSchema();
                fs.ReadXml(sn, mgr);
                AddSchema(fs);
            }

            ReadSchemaMappings(node, mgr);
        }

        /// <summary>
        /// Returns the XML form of this document
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            this.WriteXml(doc, doc);
            return doc.OuterXml;
        }

        /// <summary>
        /// Creates a configuration document from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>The configuration document</returns>
        public static ConfigurationDocument LoadXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return LoadInternal(doc);
        }

        private static ConfigurationDocument LoadInternal(XmlDocument doc)
        {
            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("xs", XmlNamespaces.XS); //NOXLATE
            mgr.AddNamespace("xsi", XmlNamespaces.XSI); //NOXLATE
            mgr.AddNamespace("fdo", XmlNamespaces.FDO); //NOXLATE
            mgr.AddNamespace("gml", XmlNamespaces.GML); //NOXLATE
            mgr.AddNamespace("xlink", XmlNamespaces.XLINK); //NOXLATE
            mgr.AddNamespace("fds", XmlNamespaces.FDS); //NOXLATE

            ConfigurationDocument conf = null;
            var root = doc.DocumentElement;
            if (root == null || root.Name != "fdo:DataStore") //NOXLATE
                return null;

            //Sample the first schema mapping node. Even if there are multiples
            //they will all be the same provider

            //NOTE: Why does the XPath query (commented out) fail? 

            var map = root.LastChild; //root.SelectSingleNode("SchemaMapping"); 
            if (map != null && map.Name == "SchemaMapping") //NOXLATE
            {
                var prov = map.Attributes["provider"]; //NOXLATE
                if (prov != null)
                {
                    if (prov.Value.StartsWith("OSGeo.ODBC")) //NOXLATE
                        conf = new OdbcConfigurationDocument();
                    else if (prov.Value.StartsWith("OSGeo.Gdal")) //NOXLATE
                        conf = new GdalConfigurationDocument();
                    else if (prov.Value.StartsWith("OSGeo.WMS")) //NOXLATE
                        conf = new WmsConfigurationDocument();
                    else
                        conf = new GenericConfigurationDocument();
                }
            }

            if (conf != null)
            {
                conf.ReadXml(doc.SelectSingleNode("fdo:DataStore", mgr), mgr); //NOXLATE
                return conf;
            }

            return null;
        }

        /// <summary>
        /// Creates a configuration document from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The configuration document</returns>
        public static ConfigurationDocument Load(System.IO.Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            return LoadInternal(doc);
        }
    }
}
