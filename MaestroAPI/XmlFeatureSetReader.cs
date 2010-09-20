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
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI
{
    public class XmlFeatureSetReader : FeatureSetReader
    {
        private XmlTextReader m_reader;

		//TODO: Make internal
        public XmlFeatureSetReader(Stream m_source) : base()
		{
			m_reader = new XmlTextReader(m_source);
			m_reader.WhitespaceHandling = WhitespaceHandling.Significant;

			//First we extract the response layout
			m_reader.Read();
			if (m_reader.Name != "xml")
				throw new Exception("Bad document");
			m_reader.Read();
			if (m_reader.Name != "FeatureSet" && m_reader.Name != "PropertySet" && m_reader.Name != "RowSet")
				throw new Exception("Bad document");

            m_reader.Read();
            if (m_reader.Name != "xs:schema" && m_reader.Name != "PropertyDefinitions" && m_reader.Name != "ColumnDefinitions")
                throw new Exception("Bad document");

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(m_reader.ReadOuterXml());
			XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
			mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			mgr.AddNamespace("gml", "http://www.opengis.net/gml");
			mgr.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");

			//TODO: Assumes there is only one type returned... perhaps more can be returned....
			XmlNodeList lst = doc.SelectNodes("xs:schema/xs:complexType/xs:complexContent/xs:extension/xs:sequence/xs:element", mgr);
			if (lst.Count == 0)
				lst = doc.SelectNodes("xs:schema/xs:complexType/xs:sequence/xs:element", mgr);
            if (lst.Count == 0)
                lst = doc.SelectNodes("PropertyDefinitions/PropertyDefinition");
            if (lst.Count == 0)
                lst = doc.SelectNodes("ColumnDefinitions/Column");
			FeatureSetColumn[] cols = new FeatureSetColumn[lst.Count];
			for(int i = 0;i<lst.Count;i++)
                cols[i] = new XmlFeatureSetColumn(lst[i]);

            InitColumns(cols);

			m_row = null;

			if (m_reader.Name != "Features" && m_reader.Name != "Properties" && m_reader.Name != "Rows")
				throw new Exception("Bad document");

			m_reader.Read();

            if (m_reader.NodeType != XmlNodeType.EndElement)
            {
                if (m_reader.Name == "Features")
                    m_reader = null; //No features :(
                else if (m_reader.Name == "PropertyCollection" || m_reader.Name == "Row")
                {
                    //OK
                }
                else if (m_reader.Name != "Feature")
                    throw new Exception("Bad document");
            }
		}

        protected override bool ReadInternal()
        {
            if (m_reader == null || (m_reader.Name != "Feature" && m_reader.Name != "PropertyCollection" && m_reader.Name != "Row"))
            {
                m_row = null;
                return false;
            }
            return true;
        }

        protected override FeatureSetRow ProcessFeatureRow()
        {
            string xmlfragment = m_reader.ReadOuterXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlfragment);

            FeatureSetRow row = null;

            if (doc["Row"] != null)
                row = new XmlFeatureSetRow(this, doc["Row"]);
            else if (doc["Feature"] == null)
                row = new XmlFeatureSetRow(this, doc["PropertyCollection"]);
            else
                row = new XmlFeatureSetRow(this, doc["Feature"]);

            if (m_reader.Name != "Feature" && m_reader.Name != "PropertyCollection" && m_reader.Name != "Row")
            {
                m_reader.Close();
                m_reader = null;
            }

            return row;
        }

        protected override void CloseInternal()
        {
            
        }

        public override int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Data.DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public override int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class XmlFeatureSetRow : FeatureSetRow
    {
        internal XmlFeatureSetRow(FeatureSetReader parent, XmlNode node)
			: base(parent)
		{
            string nodeName = "Property";
            if (node.Name == "Row")
                nodeName = "Column";

            foreach (XmlNode p in node.SelectNodes(nodeName))
            {
                int ordinal = GetOrdinal(p["Name"].InnerText);
                if (!m_nulls[ordinal])
                    throw new Exception("Bad document, multiple: " + p["Name"].InnerText + " values in a single feature");
                m_nulls[ordinal] = false;

                XmlNode valueNode = p["Value"];
                if (valueNode == null)
                {
                    m_nulls[ordinal] = true;
                }
                else
                {
                    if (parent.Columns[ordinal].Type == typeof(string) || parent.Columns[ordinal].Type == Utility.UnmappedType)
                        m_items[ordinal] = valueNode.InnerText;
                    else if (parent.Columns[ordinal].Type == typeof(int))
                        m_items[ordinal] = XmlConvert.ToInt32(valueNode.InnerText);
                    else if (parent.Columns[ordinal].Type == typeof(long))
                        m_items[ordinal] = XmlConvert.ToInt64(valueNode.InnerText);
                    else if (parent.Columns[ordinal].Type == typeof(short))
                        m_items[ordinal] = XmlConvert.ToInt16(valueNode.InnerText);
                    else if (parent.Columns[ordinal].Type == typeof(double))
                        m_items[ordinal] = XmlConvert.ToDouble(valueNode.InnerText);
                    else if (parent.Columns[ordinal].Type == typeof(bool))
                        m_items[ordinal] = XmlConvert.ToBoolean(valueNode.InnerText);
                    else if (parent.Columns[ordinal].Type == typeof(DateTime))
                    {
                        try
                        {
                            //Fix for broken ODBC provider
                            string v = valueNode.InnerText;

                            if (v.Trim().ToUpper().StartsWith("TIMESTAMP"))
                                v = v.Trim().Substring("TIMESTAMP".Length).Trim();
                            else if (v.Trim().ToUpper().StartsWith("DATE"))
                                v = v.Trim().Substring("DATE".Length).Trim();
                            else if (v.Trim().ToUpper().StartsWith("TIME"))
                                v = v.Trim().Substring("TIME".Length).Trim();

                            if (v != valueNode.InnerText)
                            {
                                if (v.StartsWith("'"))
                                    v = v.Substring(1);
                                if (v.EndsWith("'"))
                                    v = v.Substring(0, v.Length - 1);

                                m_items[ordinal] = DateTime.Parse(v, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                            else
                                m_items[ordinal] = XmlConvert.ToDateTime(v, XmlDateTimeSerializationMode.Unspecified);
                        }
                        catch (Exception ex)
                        {
                            //Unfortunately FDO supports invalid dates, such as the 30th feb
                            m_nulls[ordinal] = true;
                            m_items[ordinal] = ex;
                        }
                    }
                    else if (parent.Columns[ordinal].Type == Utility.GeometryType)
                    {
                        m_items[ordinal] = valueNode.InnerText;
                        if (string.IsNullOrEmpty(valueNode.InnerText))
                        {
                            m_nulls[ordinal] = true;
                            m_items[ordinal] = null;
                        }
                        else
                            m_lazyloadGeometry[ordinal] = true;
                    }
                    else
                        throw new Exception("Unknown type: " + parent.Columns[ordinal].Type.FullName);
                }
            }
		}
    }

    public class GeometryMetadata
    {
        public const string GEOM_TYPES = "GEOM_TYPES";

        public const string GEOM_TYPE_POINT = "point";
        public const string GEOM_TYPE_CURVE = "curve";
        public const string GEOM_TYPE_SURFACE = "surface";
        public const string GEOM_TYPE_SOLID = "solid";
    }

    public class XmlFeatureSetColumn : FeatureSetColumn
    {
        internal XmlFeatureSetColumn(XmlNode node) : base()
		{
            if (node.Name == "PropertyDefinition" || node.Name == "Column")
            {
                m_name = node["Name"].InnerText;
                m_allowNull = true;
                switch (node["Type"].InnerText.ToLower().Trim())
                {
                    case "string":
                        m_type = typeof(string);
                        break;
                    case "byte":
                        m_type = typeof(Byte);
                        break;
                    case "int32":
                    case "int":
                    case "integer":
                        m_type = typeof(int);
                        break;
                    case "int16":
                        m_type = typeof(short);
                        break;
                    case "int64":
                    case "long":
                        m_type = typeof(long);
                        break;
                    case "float":
                    case "single":
                        m_type = typeof(float);
                        break;
                    case "double":
                    case "decimal":
                        m_type = typeof(double);
                        break;
                    case "boolean":
                    case "bool":
                        m_type = typeof(bool);
                        return;
                    case "datetime":
                    case "date":
                        m_type = typeof(DateTime);
                        break;
                    case "raster":
                        m_type = Utility.RasterType;
                        break;
                    case "geometry":
                        m_type = Utility.GeometryType;
                        break;
                    default:
                        //throw new Exception("Failed to find appropriate type for: " + node["xs:simpleType"]["xs:restriction"].Attributes["base"].Value);
                        m_type = Utility.UnmappedType;
                        break;
                }
            }
            else
            {
                m_name = node.Attributes["name"].Value;
                m_allowNull = node.Attributes["minOccurs"] != null && node.Attributes["minOccurs"].Value == "0";
                if (node.Attributes["type"] != null && node.Attributes["type"].Value == "gml:AbstractGeometryType")
                {
                    m_type = Utility.GeometryType;
                    this.SetMetadata(GeometryMetadata.GEOM_TYPES, node.Attributes["fdo:geometricTypes"].Value);
                }
                else if (node["xs:simpleType"] == null)
                {
                    m_type = Utility.RasterType;
                }
                else
                {
                    switch (node["xs:simpleType"]["xs:restriction"].Attributes["base"].Value.ToLower())
                    {
                        case "xs:string":
                            m_type = typeof(string);
                            break;
                        case "fdo:byte":
                            m_type = typeof(Byte);
                            break;
                        case "fdo:int32":
                            m_type = typeof(int);
                            break;
                        case "fdo:int16":
                            m_type = typeof(short);
                            break;
                        case "fdo:int64":
                            m_type = typeof(long);
                            break;
                        case "xs:float":
                        case "xs:single":
                        case "fdo:single":
                            m_type = typeof(float);
                            break;
                        case "xs:double":
                        case "xs:decimal":
                            m_type = typeof(double);
                            break;
                        case "xs:boolean":
                            m_type = typeof(bool);
                            return;
                        case "xs:datetime":
                            m_type = typeof(DateTime);
                            break;
                        default:
                            //throw new Exception("Failed to find appropriate type for: " + node["xs:simpleType"]["xs:restriction"].Attributes["base"].Value);
                            m_type = Utility.UnmappedType;
                            break;
                    }
                }
            }
		}
    }
}
