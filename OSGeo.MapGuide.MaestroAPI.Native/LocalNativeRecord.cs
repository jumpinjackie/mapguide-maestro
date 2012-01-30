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
using OSGeo.MapGuide.MaestroAPI.Internal;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeRecord : RecordBase
    {
        public LocalNativeRecord(MgReader reader, FixedWKTReader mgReader, MgAgfReaderWriter agfRw, MgWktReaderWriter wktRw)
        {
            for (int i = 0; i < reader.GetPropertyCount(); i++)
            {
                string name = reader.GetPropertyName(i);

                _ordinalMap[i] = name;

                var pt = (PropertyValueType)reader.GetPropertyType(name);
                switch (pt)
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

            for (int i = 0; i < reader.GetPropertyCount(); i++)
            {
                string name = _ordinalMap[i];
                GetByteReaderMethod getblob = () => { return reader.GetBLOB(name); };
                GetByteReaderMethod getclob = () => { return reader.GetCLOB(name); };
                GetByteReaderMethod getgeom = () => { return reader.GetGeometry(name); };
                if (!reader.IsNull(name))
                {
                    var pt = (PropertyValueType)reader.GetPropertyType(name);
                    switch (pt)
                    {
                        case PropertyValueType.Blob:
                            ((BlobValue)_values[name]).Value = Utility.StreamAsArray(new MgReadOnlyStream(getblob));
                            break;
                        case PropertyValueType.Boolean:
                            ((BooleanValue)_values[name]).Value = reader.GetBoolean(name);
                            break;
                        case PropertyValueType.Byte:
                            ((ByteValue)_values[name]).Value = reader.GetByte(name);
                            break;
                        case PropertyValueType.Clob:
                            byte [] b = Utility.StreamAsArray(new MgReadOnlyStream(getclob));
                            ((ClobValue)_values[name]).Value = Encoding.UTF8.GetChars(b);
                            break;
                        case PropertyValueType.DateTime:
                            ((DateTimeValue)_values[name]).Value = Utility.ConvertMgDateTime(reader.GetDateTime(name));
                            break;
                        case PropertyValueType.Double:
                            ((DoubleValue)_values[name]).Value = reader.GetDouble(name);
                            break;
                        //case PropertyValueType.Feature:
                        case PropertyValueType.Geometry:
                            try
                            {
                                //TODO: See if SWIG issues come into play here
                                var geom = agfRw.Read(reader.GetGeometry(name));
                                var wkt = wktRw.Write(geom);
                                ((GeometryValue)_values[name]).Value = mgReader.Read(wkt);
                            }
                            catch //Invalid geometry fail!
                            {
                                ((GeometryValue)_values[name]).SetNull();
                            }
                            break;
                        case PropertyValueType.Int16:
                            ((Int16Value)_values[name]).Value = reader.GetInt16(name);
                            break;
                        case PropertyValueType.Int32:
                            ((Int32Value)_values[name]).Value = reader.GetInt32(name);
                            break;
                        case PropertyValueType.Int64:
                            ((Int64Value)_values[name]).Value = reader.GetInt64(name);
                            break;
                        case PropertyValueType.Single:
                            ((SingleValue)_values[name]).Value = reader.GetSingle(name);
                            break;
                        case PropertyValueType.String:
                            ((StringValue)_values[name]).Value = reader.GetString(name);
                            break;
                    }
                }
            }
        }
    }
}
