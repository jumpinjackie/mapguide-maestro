#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Xml;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    /// <summary>
    /// Dummy class that represents an unknown data type
    /// </summary>
    public class UnmappedDataType
    {
    }

	/// <summary>
	/// Class that represents a the layout of a feature source
	/// </summary>
	public class FeatureSourceDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSourceDescription"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FeatureSourceDescription(System.IO.Stream stream)
        {
            List<FeatureSchema> schemas = new List<FeatureSchema>();

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("xs", XmlNamespaces.XS);
            mgr.AddNamespace("gml", XmlNamespaces.GML);
            mgr.AddNamespace("fdo", XmlNamespaces.FDO);

            //Assume XML configuration document
            XmlNodeList schemaNodes = doc.SelectNodes("fdo:DataStore/xs:schema", mgr);
            if (schemaNodes.Count == 0) //Then assume FDO schema
                schemaNodes = doc.SelectNodes("xs:schema", mgr);

            foreach (XmlNode sn in schemaNodes)
            {
                FeatureSchema fs = new FeatureSchema();
                fs.ReadXml(sn, mgr);
                schemas.Add(fs);
            }
            this.Schemas = schemas.ToArray();
        }

        /// <summary>
        /// Gets whether this description is a partial description (ie. It doesn't represent the full feature source)
        /// </summary>
        public bool IsPartial { get; internal set; }

        /// <summary>
        /// Gets an array of feature schemas in this feature source
        /// </summary>
        public FeatureSchema[] Schemas { get; private set; }

        /// <summary>
        /// Gets a feature schema by its name
        /// </summary>
        /// <param name="schemaName"></param>
        /// <returns>The matching feature schema. null if not found</returns>
        public FeatureSchema GetSchema(string schemaName)
        {
            foreach (var fsc in this.Schemas)
            {
                if (fsc.Name.Equals(schemaName))
                {
                    return fsc;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets an array of feature schema names
        /// </summary>
        public string[] SchemaNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (var fsc in this.Schemas)
                {
                    names.Add(fsc.Name);
                }
                return names.ToArray();
            }
        }

        /// <summary>
        /// Gets all the Class Definitions in this feature source. In the event of identically named Class Definitions beloning
        /// in different parent schemas. Use the <see cref="M:OSGeo.MapGuide.MaestroAPI.Schema.ClassDefinition.QualifiedName"/> property
        /// to distinguish them.
        /// </summary>
        public IEnumerable<ClassDefinition> AllClasses
        {
            get
            {
                foreach (var fsc in this.Schemas)
                {
                    foreach (var cls in fsc.Classes)
                    {
                        yield return cls;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the specified class definition by its name and parent schema name
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="className"></param>
        /// <returns>The matching class definition. null if not found</returns>
        public ClassDefinition GetClass(string schemaName, string className)
        {
            var fsc = GetSchema(schemaName);
            if (fsc != null)
            {
                foreach (var cls in fsc.Classes)
                {
                    if (cls.Name.Equals(className))
                        return cls;
                }
            }
            return null;
        }

        #region old impl
        /*
        private ClassDefinition[] m_classes;

        private string[] m_schemaNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSourceDescription"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
		public FeatureSourceDescription(System.IO.Stream stream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(stream);

			if (doc.FirstChild.Name != "xml")
				throw new Exception("Bad document");

            XmlNode root;
            if (doc.ChildNodes.Count == 2 && doc.ChildNodes[1].Name == "fdo:DataStore")
                root = doc.ChildNodes[1];
            else if (doc.ChildNodes.Count != 2 || doc.ChildNodes[1].Name != "xs:schema")
                throw new Exception("Bad document");
            else
                root = doc;

			XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
			mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			mgr.AddNamespace("gml", "http://www.opengis.net/gml");
			mgr.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");

            var keys = new Dictionary<string, string[]>();
            var classMap = new Dictionary<string, ClassDefinition>();
            XmlNodeList lst = root.SelectNodes("xs:schema/xs:complexType[@abstract='false']", mgr);
            m_classes = new ClassDefinition[lst.Count];
            for (int i = 0; i < m_classes.Length; i++)
            {
                m_classes[i] = new ClassDefinition(lst[i], mgr);
                classMap.Add(m_classes[i].QualifiedName, m_classes[i]);
            }
            XmlNodeList keyNodes = root.SelectNodes("xs:schema/xs:element[@abstract='false']", mgr);
            foreach (XmlNode keyNode in keyNodes)
            {
                var typeAttr = keyNode.Attributes["type"];
                if (typeAttr != null)
                {
                    string clsName = typeAttr.Value.Substring(0, typeAttr.Value.Length - 4); //class name is suffixed with type
                    if (classMap.ContainsKey(clsName))
                    {
                        List<string> keyFieldNames = new List<string>();

                        var cls = classMap[clsName];
                        XmlNodeList keyFields = keyNode.SelectNodes("xs:key/xs:field", mgr);
                        foreach (XmlNode keyField in keyFields)
                        {
                            var xpathAttr = keyField.Attributes["xpath"];
                            if (xpathAttr != null)
                            {
                                keyFieldNames.Add(xpathAttr.Value);
                            }
                        }

                        cls.MarkIdentityProperties(keyFieldNames);
                    }
                }
            }

            var snames = new List<string>();
            foreach (string qn in classMap.Keys)
            {
                string[] tokens = qn.Split(':');
                if (!snames.Contains(tokens[0]))
                    snames.Add(tokens[0]);
            }
            m_schemaNames = snames.ToArray();
		}

        /// <summary>
        /// Gets the schema names.
        /// </summary>
        /// <value>The schema names.</value>
        public string[] SchemaNames { get { return m_schemaNames; } }

        /// <summary>
        /// Gets the classes.
        /// </summary>
        /// <value>The classes.</value>
		public ClassDefinition[] Classes { get { return m_classes; } }

        /// <summary>
        /// Gets the <see cref="OSGeo.MapGuide.MaestroAPI.ClassDefinition"/> at the specified index.
        /// </summary>
        /// <value></value>
		public ClassDefinition this[int index] { get { return m_classes[index]; } }
        /// <summary>
        /// Gets the <see cref="OSGeo.MapGuide.MaestroAPI.ClassDefinition"/> at the specified index.
        /// </summary>
        /// <value></value>
		public ClassDefinition this[string index] 
		{
			get 
			{
				for(int i =0 ;i<m_classes.Length; i++)
					if (m_classes[i].Name == index)
						return m_classes[i];

				throw new OverflowException("No such item found: " + index);
			}
        }
        */
        #endregion

        /// <summary>
        /// Gets whether there are any class definitions
        /// </summary>
        /// <returns></returns>
        public bool HasClasses()
        {
            if (this.Schemas.Length == 0)
                return false;

            foreach (var fsc in this.Schemas)
            {
                if (fsc.Classes.Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the specified class definition by its fully qualified name
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <returns>The matching class definition. null if not found</returns>
        public ClassDefinition GetClass(string qualifiedName)
        {
            Check.NotEmpty(qualifiedName, "qualifiedName");
            var tokens = qualifiedName.Split(':');
            if (tokens.Length != 2)
                throw new ArgumentException("Not a qualified class name: " + qualifiedName); //LOCALIZEME

            return GetClass(tokens[0], tokens[1]);
        }
    }
}
