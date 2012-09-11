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
            mgr.AddNamespace("xs", XmlNamespaces.XS); //NOXLATE
            mgr.AddNamespace("gml", XmlNamespaces.GML); //NOXLATE
            mgr.AddNamespace("fdo", XmlNamespaces.FDO); //NOXLATE

            //Assume XML configuration document
            XmlNodeList schemaNodes = doc.SelectNodes("fdo:DataStore/xs:schema", mgr); //NOXLATE
            if (schemaNodes.Count == 0) //Then assume FDO schema
                schemaNodes = doc.SelectNodes("xs:schema", mgr); //NOXLATE

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
            Check.NotEmpty(qualifiedName, "qualifiedName"); //NOXLATE
            var tokens = qualifiedName.Split(':'); //NOXLATE
            if (tokens.Length != 2)
                throw new ArgumentException(string.Format(Strings.ErrorNotAQualifiedClassName, qualifiedName));

            return GetClass(tokens[0], tokens[1]);
        }

        /// <summary>
        /// Internal ctor for cloning purposes
        /// </summary>
        /// <param name="schemas"></param>
        /// <param name="bPartial"></param>
        internal FeatureSourceDescription(List<FeatureSchema> schemas, bool bPartial)
        {
            this.Schemas = schemas.ToArray();
            this.IsPartial = bPartial;
        }

        /// <summary>
        /// Creates a clone of the specified instance
        /// </summary>
        /// <param name="fsd">The instance to clone</param>
        /// <returns></returns>
        public static FeatureSourceDescription Clone(FeatureSourceDescription fsd)
        {
            var schemas = new List<FeatureSchema>();
            foreach (var fs in fsd.Schemas)
            {
                schemas.Add(FeatureSchema.Clone(fs));
            }
            return new FeatureSourceDescription(schemas, fsd.IsPartial);
        }
    }
}
