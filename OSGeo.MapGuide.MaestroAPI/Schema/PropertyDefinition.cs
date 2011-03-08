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

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    /// <summary>
    /// Defines the types of properties in a FDO class definition
    /// </summary>
    public enum PropertyDefinitionType
    {
        Data = 100,
        Geometry = 102,
        Raster = 104,
        Association = 103,
        Object = 101
    }

    /// <summary>
    /// Defines the valid types of property values
    /// </summary>
    public enum PropertyValueType
    {
        Blob = 10,
        Boolean = 1,
        Byte = 2,
        Clob = 11,
        DateTime = 3,
        Double = 5,
        Feature = 12,
        Geometry = 13,
        Int16 = 6,
        Int32 = 7,
        Int64 = 8,
        Null = 0,
        Raster = 14,
        Single = 4,
        String = 9
    }

    /// <summary>
    /// Defines the valid data types of data properties
    /// </summary>
    public enum DataPropertyType
    {
        Blob = PropertyValueType.Blob,
        Boolean = PropertyValueType.Boolean,
        Byte = PropertyValueType.Byte,
        Clob = PropertyValueType.Clob,
        DateTime = PropertyValueType.DateTime,
        Double = PropertyValueType.Double,
        Int16 = PropertyValueType.Int16,
        Int32 = PropertyValueType.Int32,
        Int64 = PropertyValueType.Int64,
        Single = PropertyValueType.Single,
        String = PropertyValueType.String
    }

    /// <summary>
    /// Base class of all property definitions
    /// </summary>
    public abstract class PropertyDefinition : SchemaElement, IFdoSerializable
    {
        /// <summary>
        /// Gets the parent class definition
        /// </summary>
        public ClassDefinition Parent { get; internal set; }

        /// <summary>
        /// Gets the type of property
        /// </summary>
        public abstract PropertyDefinitionType Type { get; }

        public abstract void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode);

        public abstract void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr);

        public static PropertyDefinition Parse(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            PropertyDefinition prop = null;
            var nn = node.Attributes["name"];
            var nulln = node.Attributes["minOccurs"];

            string name = Utility.DecodeFDOName(nn.Value);
            string desc = string.Empty;

            if (node.Attributes["type"] != null && node.Attributes["type"].Value == "gml:AbstractGeometryType")
            {
                prop = new GeometricPropertyDefinition(name, desc);
            }
            else if (node["xs:simpleType"] == null)
            {
                prop = new RasterPropertyDefinition(name, desc);
            }
            else
            {
                if (node["xs:simpleType"] != null)
                    prop = new DataPropertyDefinition(name, desc);
            }

            if (prop != null)
                prop.ReadXml(node, mgr);
            else
                throw new NotSupportedException("Unrecognized element. Only a subset of the FDO logical schema is supported here"); //LOCALIZEME

            return prop;
        }
    }
}
