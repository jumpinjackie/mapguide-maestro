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

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    /// <summary>
    /// Defines the types of properties in a FDO class definition
    /// </summary>
    public enum PropertyDefinitionType : int
    {
        /// <summary>
        /// Data Properties
        /// </summary>
        Data = 100,
        /// <summary>
        /// Geometric Properties
        /// </summary>
        Geometry = 102,
        /// <summary>
        /// Raster Properties
        /// </summary>
        Raster = 104,
        /// <summary>
        /// Association Properties
        /// </summary>
        Association = 103,
        /// <summary>
        /// Object Properties
        /// </summary>
        Object = 101
    }

    /// <summary>
    /// Defines the valid types of property values
    /// </summary>
    public enum PropertyValueType : int
    {
        /// <summary>
        /// BLOB
        /// </summary>
        Blob = 10,
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// Byte
        /// </summary>
        Byte = 2,
        /// <summary>
        /// CLOB
        /// </summary>
        Clob = 11,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime = 3,
        /// <summary>
        /// Double
        /// </summary>
        Double = 5,
        /// <summary>
        /// Feature
        /// </summary>
        Feature = 12,
        /// <summary>
        /// Geometry
        /// </summary>
        Geometry = 13,
        /// <summary>
        /// Int16
        /// </summary>
        Int16 = 6,
        /// <summary>
        /// Int32
        /// </summary>
        Int32 = 7,
        /// <summary>
        /// Int64
        /// </summary>
        Int64 = 8,
        /// <summary>
        /// Invalid or Unknown
        /// </summary>
        Null = 0,
        /// <summary>
        /// Raster
        /// </summary>
        Raster = 14,
        /// <summary>
        /// Single
        /// </summary>
        Single = 4,
        /// <summary>
        /// String
        /// </summary>
        String = 9
    }

    /// <summary>
    /// Defines the valid data types of data properties
    /// </summary>
    public enum DataPropertyType : int
    {
        /// <summary>
        /// BLOB
        /// </summary>
        Blob = PropertyValueType.Blob,
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean = PropertyValueType.Boolean,
        /// <summary>
        /// Byte
        /// </summary>
        Byte = PropertyValueType.Byte,
        /// <summary>
        /// CLOB
        /// </summary>
        Clob = PropertyValueType.Clob,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime = PropertyValueType.DateTime,
        /// <summary>
        /// Double
        /// </summary>
        Double = PropertyValueType.Double,
        /// <summary>
        /// Int16
        /// </summary>
        Int16 = PropertyValueType.Int16,
        /// <summary>
        /// Int32
        /// </summary>
        Int32 = PropertyValueType.Int32,
        /// <summary>
        /// Int64
        /// </summary>
        Int64 = PropertyValueType.Int64,
        /// <summary>
        /// Single
        /// </summary>
        Single = PropertyValueType.Single,
        /// <summary>
        /// String
        /// </summary>
        String = PropertyValueType.String
    }

    /// <summary>
    /// Base class of all property definitions
    /// </summary>
    public abstract class PropertyDefinition : SchemaElement, IFdoSerializable, IExpressionPropertySource
    {
        /// <summary>
        /// Gets the parent class definition
        /// </summary>
        public ClassDefinition Parent { get; internal set; }

        /// <summary>
        /// Gets the type of property
        /// </summary>
        public abstract PropertyDefinitionType Type { get; }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public abstract void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode);

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public abstract void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr);

        /// <summary>
        /// Parses the specified XML node into a Property Definition
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public static PropertyDefinition Parse(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            PropertyDefinition prop = null;
            var nn = node.Attributes["name"]; //NOXLATE
            var nulln = node.Attributes["minOccurs"]; //NOXLATE

            string name = Utility.DecodeFDOName(nn.Value);
            string desc = string.Empty;

            //Description
            var docNode = node.SelectSingleNode("xs:annotation/xs:documentation", mgr); //NOXLATE
            if (docNode != null)
                desc = docNode.InnerText;

            if (node.Attributes["type"] != null && node.Attributes["type"].Value == "gml:AbstractGeometryType") //NOXLATE
            {
                prop = new GeometricPropertyDefinition(name, desc);
            }
            else if (node["xs:simpleType"] == null) //NOXLATE
            {
                prop = new RasterPropertyDefinition(name, desc);
            }
            else
            {
                if (node["xs:simpleType"] != null) //NOXLATE
                    prop = new DataPropertyDefinition(name, desc);
            }

            if (prop != null)
                prop.ReadXml(node, mgr);
            else
                throw new NotSupportedException(Strings.ErrorUnsupporteFdoSchemaXml);

            return prop;
        }

        /// <summary>
        /// Gets the expression data type
        /// </summary>
        public abstract ExpressionDataType ExpressionType
        {
            get;
        }

        /// <summary>
        /// Creates a clone of the specified instance
        /// </summary>
        /// <param name="prop">The instance to clone.</param>
        /// <returns></returns>
        public static PropertyDefinition Clone(PropertyDefinition prop)
        {
            switch (prop.Type)
            {
                case PropertyDefinitionType.Data:
                    {
                        var source = (DataPropertyDefinition)prop;
                        var dp = new DataPropertyDefinition(prop.Name, prop.Description);

                        dp.DataType = source.DataType;
                        dp.DefaultValue = source.DefaultValue;
                        dp.IsAutoGenerated = source.IsAutoGenerated;
                        dp.IsNullable = source.IsNullable;
                        dp.IsReadOnly = source.IsReadOnly;
                        dp.Length = source.Length;
                        dp.Precision = source.Precision;
                        dp.Scale = source.Scale;

                        return dp;
                    }
                case PropertyDefinitionType.Geometry:
                    {
                        var source = (GeometricPropertyDefinition)prop;
                        var gp = new GeometricPropertyDefinition(prop.Name, prop.Description);

                        gp.SpecificGeometryTypes = source.SpecificGeometryTypes;
                        gp.HasElevation = source.HasElevation;
                        gp.HasMeasure = source.HasMeasure;
                        gp.IsReadOnly = source.IsReadOnly;
                        gp.SpatialContextAssociation = source.SpatialContextAssociation;

                        return gp;
                    }
                case PropertyDefinitionType.Raster:
                    {
                        var source = (RasterPropertyDefinition)prop;
                        var rp = new RasterPropertyDefinition(prop.Name, prop.Description);

                        rp.DefaultImageXSize = source.DefaultImageXSize;
                        rp.DefaultImageYSize = source.DefaultImageYSize;
                        rp.IsNullable = source.IsNullable;
                        rp.IsReadOnly = source.IsReadOnly;
                        rp.SpatialContextAssociation = source.SpatialContextAssociation;

                        return rp;
                    }
            }
            throw new ArgumentException();
        }
    }
}
