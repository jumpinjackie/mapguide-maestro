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
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Internal;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    public class XmlProperty
    {
        public XmlProperty(string name, PropertyValueType pvtype) 
        { 
            this.Name =name;
            this.Type = pvtype;
        }

        public string Name { get; private set; }

        public PropertyValueType Type { get; private set; } 
    }

    public class XmlRecord : RecordBase
    {
        public XmlRecord(XmlProperty[] properties, FixedWKTReader wktReader, XmlNodeList propertyNodes, string nameElement, string valueElement)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name;
                _ordinalMap[i] = name;

                switch (properties[i].Type)
                {
                    case PropertyValueType.Blob:
                        _values[name] = new BlobValue();
                        break;
                    case PropertyValueType.Boolean:
                        _values[name] = new BooleanValue();
                        break;
                    case PropertyValueType.Byte:
                        _values[name] = new ByteValue();
                        break;
                    case PropertyValueType.Clob:
                        _values[name] = new ClobValue();
                        break;
                    case PropertyValueType.DateTime:
                        _values[name] = new DateTimeValue();
                        break;
                    case PropertyValueType.Double:
                        _values[name] = new DoubleValue();
                        break;
                    case PropertyValueType.Feature:
                        _values[name] = new FeatureValue();
                        break;
                    case PropertyValueType.Geometry:
                        _values[name] = new GeometryValue();
                        break;
                    case PropertyValueType.Int16:
                        _values[name] = new Int16Value();
                        break;
                    case PropertyValueType.Int32:
                        _values[name] = new Int32Value();
                        break;
                    case PropertyValueType.Int64:
                        _values[name] = new Int64Value();
                        break;
                    case PropertyValueType.Raster:
                        _values[name] = new RasterValue();
                        break;
                    case PropertyValueType.Single:
                        _values[name] = new SingleValue();
                        break;
                    case PropertyValueType.String:
                        _values[name] = new StringValue();
                        break;
                }
            }

            foreach (XmlNode propNode in propertyNodes)
            {
                var name = propNode[nameElement].InnerText;
                var valueNode = propNode[valueElement];
                if (valueNode != null)
                {
                    var value = valueNode.InnerText;
                    switch (_values[name].Type)
                    {
                        case PropertyValueType.Blob:
                            ((BlobValue)_values[name]).Value = Encoding.UTF8.GetBytes(value);
                            break;
                        case PropertyValueType.Boolean:
                            ((BooleanValue)_values[name]).Value = XmlConvert.ToBoolean(value);
                            break;
                        case PropertyValueType.Byte:
                            ((ByteValue)_values[name]).Value = XmlConvert.ToByte(value);
                            break;
                        case PropertyValueType.Clob:
                            ((ClobValue)_values[name]).Value = value.ToCharArray();
                            break;
                        case PropertyValueType.DateTime:
                            var dt = ConvertToDateTime(value);
                            if (dt.HasValue)
                                ((DateTimeValue)_values[name]).Value = dt.Value;
                            break;
                        case PropertyValueType.Double:
                            ((DoubleValue)_values[name]).Value = XmlConvert.ToDouble(value);
                            break;
                        case PropertyValueType.Feature:
                            ((FeatureValue)_values[name]).Value = ConvertToFeatures(value);
                            break;
                        case PropertyValueType.Geometry:
                            ((GeometryValue)_values[name]).Value = wktReader.Read(value);
                            break;
                        case PropertyValueType.Int16:
                            ((Int16Value)_values[name]).Value = XmlConvert.ToInt16(value);
                            break;
                        case PropertyValueType.Int32:
                            ((Int32Value)_values[name]).Value = XmlConvert.ToInt32(value);
                            break;
                        case PropertyValueType.Int64:
                            ((Int64Value)_values[name]).Value = XmlConvert.ToInt64(value);
                            break;
                        case PropertyValueType.Raster:
                            ((RasterValue)_values[name]).Value = ConvertToRaster(value);
                            break;
                        case PropertyValueType.Single:
                            ((SingleValue)_values[name]).Value = XmlConvert.ToSingle(value);
                            break;
                        case PropertyValueType.String:
                            ((StringValue)_values[name]).Value = value;
                            break;
                    }
                }
            }
        }

        protected byte[] ConvertToRaster(string name)
        {
            throw new NotImplementedException();
        }

        protected IFeature[] ConvertToFeatures(string value)
        {
            throw new NotImplementedException();
        }

        protected DateTime? ConvertToDateTime(string value)
        {
            try
            {
                //Fix for broken ODBC provider
                string v = value;

                if (v.Trim().ToUpper().StartsWith("TIMESTAMP"))
                    v = v.Trim().Substring("TIMESTAMP".Length).Trim();
                else if (v.Trim().ToUpper().StartsWith("DATE"))
                    v = v.Trim().Substring("DATE".Length).Trim();
                else if (v.Trim().ToUpper().StartsWith("TIME"))
                    v = v.Trim().Substring("TIME".Length).Trim();

                if (v != value)
                {
                    if (v.StartsWith("'"))
                        v = v.Substring(1);
                    if (v.EndsWith("'"))
                        v = v.Substring(0, v.Length - 1);

                    return DateTime.Parse(v, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                }
                else
                    return XmlConvert.ToDateTime(v, XmlDateTimeSerializationMode.Unspecified);
            }
            catch
            {
                //Unfortunately FDO supports invalid dates, such as the 30th feb
                return null;
            }
        }
    }
}
