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

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IRecord"/>
    /// interface
    /// </summary>
    public abstract class RecordBase : IRecord, IRecordReset, IRecordInitialize
    {
        protected Dictionary<string, PropertyValue> _values;
        protected Dictionary<int, string> _ordinalMap;

        public RecordBase()
        {
            _values = new Dictionary<string, PropertyValue>();
            _ordinalMap = new Dictionary<int, string>();
        }

        public PropertyValue GetValue(string name)
        {
            return _values[name];
        }

        public void PutValue(string name, PropertyValue value)
        {
            if (_values.ContainsKey(name))
                throw new ArgumentException("Key " + name + " already exists"); //LOCALIZEME

            _values[name] = value;
        }

        public bool IsNull(string name)
        {
            return _values[name].IsNull;
        }

        public bool IsNull(int index)
        {
            return IsNull(_ordinalMap[index]);
        }

        public bool GetBoolean(string name)
        {
            return ((BooleanValue)_values[name]).Value;
        }

        public byte GetByte(string name)
        {
            return ((ByteValue)_values[name]).Value;
        }

        public byte[] GetBlob(string name)
        {
            return ((BlobValue)_values[name]).Value;
        }

        public char[] GetClob(string name)
        {
            return ((ClobValue)_values[name]).Value;
        }

        public double GetDouble(string name)
        {
            return ((DoubleValue)_values[name]).Value;
        }

        public DateTime GetDateTime(string name)
        {
            return ((DateTimeValue)_values[name]).Value;
        }

        public IFeatureReader GetFeatureObject(string name)
        {
            return new FeatureArrayReader(((FeatureValue)_values[name]).Value);
        }

        public short GetInt16(string name)
        {
            return ((Int16Value)_values[name]).Value;
        }

        public int GetInt32(string name)
        {
            return ((Int32Value)_values[name]).Value;
        }

        public long GetInt64(string name)
        {
            return ((Int64Value)_values[name]).Value;
        }

        public float GetSingle(string name)
        {
            return ((SingleValue)_values[name]).Value;
        }

        public string GetString(string name)
        {
            return ((StringValue)_values[name]).Value;
        }

        public Topology.Geometries.IGeometry GetGeometry(string name)
        {
            return ((GeometryValue)_values[name]).Value;
        }

        public bool GetBoolean(int index)
        {
            return GetBoolean(_ordinalMap[index]);
        }

        public byte GetByte(int index)
        {
            return GetByte(_ordinalMap[index]);
        }

        public byte[] GetBlob(int index)
        {
            return GetBlob(_ordinalMap[index]);
        }

        public char[] GetClob(int index)
        {
            return GetClob(_ordinalMap[index]);
        }

        public double GetDouble(int index)
        {
            return GetDouble(_ordinalMap[index]);
        }

        public DateTime GetDateTime(int index)
        {
            return GetDateTime(_ordinalMap[index]);
        }

        public IFeatureReader GetFeatureObject(int index)
        {
            return GetFeatureObject(_ordinalMap[index]);
        }

        public short GetInt16(int index)
        {
            return GetInt16(_ordinalMap[index]);
        }

        public int GetInt32(int index)
        {
            return GetInt32(_ordinalMap[index]);
        }

        public long GetInt64(int index)
        {
            return GetInt64(_ordinalMap[index]);
        }

        public float GetSingle(int index)
        {
            return GetSingle(_ordinalMap[index]);
        }

        public string GetString(int index)
        {
            return GetString(_ordinalMap[index]);
        }

        public Topology.Geometries.IGeometry GetGeometry(int index)
        {
            return GetGeometry(_ordinalMap[index]);
        }

        public object this[int index]
        {
            get { return this[_ordinalMap[index]]; }
        }

        public object this[string name]
        {
            get
            {
                switch (_values[name].Type)
                {
                    case PropertyValueType.Blob:
                        return GetBlob(name);
                    case PropertyValueType.Boolean:
                        return GetBoolean(name);
                    case PropertyValueType.Byte:
                        return GetByte(name);
                    case PropertyValueType.Clob:
                        return GetClob(name);
                    case PropertyValueType.DateTime:
                        return GetDateTime(name);
                    case PropertyValueType.Double:
                        return GetDouble(name);
                    case PropertyValueType.Feature:
                        return GetFeatureObject(name);
                    case PropertyValueType.Geometry:
                        return GetGeometry(name);
                    case PropertyValueType.Int16:
                        return GetInt16(name);
                    case PropertyValueType.Int32:
                        return GetInt32(name);
                    case PropertyValueType.Int64:
                        return GetInt64(name);
                    //case PropertyValueType.Raster:
                    //    return GetRaster(name);
                    case PropertyValueType.Single:
                        return GetSingle(name);
                    case PropertyValueType.String:
                        return GetString(name);
                }
                throw new ArgumentException();
            }
        }

        public int FieldCount
        {
            get { return _values.Count; }
        }

        public string GetName(int index)
        {
            return _ordinalMap[index];
        }

        public Type GetFieldType(int i)
        {
            return ClrFdoTypeMap.GetClrType(_values[GetName(i)].Type);
        }

        public void Update(IRecord record)
        {
            if (record.FieldCount != this.FieldCount)
                throw new InvalidOperationException("Incoming record must be structurally identical"); //LOCALIZEME

            foreach (var v in _values.Values)
            {
                v.SetNull();
            }

            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.IsNull(i))
                    continue;

                var val = _values[_ordinalMap[i]];
                var type = val.Type;
                switch (type)
                {
                    case PropertyValueType.Blob:
                        ((BlobValue)val).Value = record.GetBlob(i);
                        break;
                    case PropertyValueType.Boolean:
                        ((BooleanValue)val).Value = record.GetBoolean(i);
                        break;
                    case PropertyValueType.Byte:
                        ((ByteValue)val).Value = record.GetByte(i);
                        break;
                    case PropertyValueType.Clob:
                        ((ClobValue)val).Value = record.GetClob(i);
                        break;
                    case PropertyValueType.DateTime:
                        ((DateTimeValue)val).Value = record.GetDateTime(i);
                        break;
                    case PropertyValueType.Double:
                        ((DoubleValue)val).Value = record.GetDouble(i);
                        break;
                    //case PropertyValueType.Feature:
                    //    {
                    //        List<IFeature> features = new List<IFeature>();
                    //        foreach (var feat in record.GetFeatureObject(i))
                    //        {
                    //            features.Add(feat);
                    //        }
                    //        ((FeatureValue)val).Value = features.ToArray();
                    //    }
                    //    break;
                    case PropertyValueType.Geometry:
                        ((GeometryValue)val).Value = record.GetGeometry(i);
                        break;
                    case PropertyValueType.Int16:
                        ((Int16Value)val).Value = record.GetInt16(i);
                        break;
                    case PropertyValueType.Int32:
                        ((Int32Value)val).Value = record.GetInt32(i);
                        break;
                    case PropertyValueType.Int64:
                        ((Int64Value)val).Value = record.GetInt64(i);
                        break;
                    //case PropertyValueType.Raster:
                    //    ((RasterValue)val).Value = record.GetRaster(i);
                    //    break;
                    case PropertyValueType.Single:
                        ((SingleValue)val).Value = record.GetSingle(i);
                        break;
                    case PropertyValueType.String:
                        ((StringValue)val).Value = record.GetString(i);
                        break;
                }
            }
        }
    }
}
