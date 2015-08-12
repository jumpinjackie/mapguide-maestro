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

#endregion Disclaimer / License

using GeoAPI.Geometries;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IRecord"/>
    /// interface
    /// </summary>
    public abstract class RecordBase : IRecord, IRecordReset, IRecordInitialize
    {
        /// <summary>
        /// The map of property values
        /// </summary>
        protected Dictionary<string, PropertyValue> _values;

        /// <summary>
        /// A dictionary to map indexes to property names
        /// </summary>
        protected Dictionary<int, string> _ordinalMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordBase"/> class.
        /// </summary>
        protected RecordBase()
        {
            _values = new Dictionary<string, PropertyValue>();
            _ordinalMap = new Dictionary<int, string>();
        }

        /// <summary>
        /// Gets the specified property value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyValue GetValue(string name) => _values[name];

        /// <summary>
        /// Gets whether the specified named property exists
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns></returns>
        public bool PropertyExists(string name) => _values.ContainsKey(name);

        /// <summary>
        /// Adds the specified property value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void PutValue(string name, PropertyValue value)
        {
            if (_values.ContainsKey(name))
                throw new ArgumentException(string.Format(Strings.ErrorKeyAlreadyExists, name));

            _ordinalMap[this.FieldCount] = name;
            _values[name] = value;
        }

        /// <summary>
        /// Gets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNull(string name) => _values[name].IsNull;

        /// <summary>
        /// Gets whether the property value at the specified index has a null property value. You must
        /// call this method first to determine if it is safe to call the corresponding GetXXX() methods
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsNull(int index) => IsNull(_ordinalMap[index]);

        /// <summary>
        /// Gets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBoolean(string name) => ((BooleanValue)_values[name]).Value;

        /// <summary>
        /// Gets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte GetByte(string name) => ((ByteValue)_values[name]).Value;

        /// <summary>
        /// Gets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetBlob(string name) => ((BlobValue)_values[name]).Value;

        /// <summary>
        /// Gets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public char[] GetClob(string name) => ((ClobValue)_values[name]).Value;

        /// <summary>
        /// Gets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetDouble(string name) => ((DoubleValue)_values[name]).Value;

        /// <summary>
        /// Gets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string name) => ((DateTimeValue)_values[name]).Value;

        /// <summary>
        /// Gets the feature object.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(string name) => new FeatureArrayReader(((FeatureValue)_values[name]).Value);

        /// <summary>
        /// Gets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public short GetInt16(string name) => ((Int16Value)_values[name]).Value;

        /// <summary>
        /// Gets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt32(string name) => ((Int32Value)_values[name]).Value;

        /// <summary>
        /// Gets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetInt64(string name) => ((Int64Value)_values[name]).Value;

        /// <summary>
        /// Gets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetSingle(string name) => ((SingleValue)_values[name]).Value;

        /// <summary>
        /// Gets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name) => ((StringValue)_values[name]).Value;

        /// <summary>
        /// Gets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IGeometry GetGeometry(string name) => ((GeometryValue)_values[name]).Value;

        /// <summary>
        /// Gets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBoolean(int index) => GetBoolean(_ordinalMap[index]);

        /// <summary>
        /// Gets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte GetByte(int index) => GetByte(_ordinalMap[index]);

        /// <summary>
        /// Gets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] GetBlob(int index) => GetBlob(_ordinalMap[index]);

        /// <summary>
        /// Gets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char[] GetClob(int index) => GetClob(_ordinalMap[index]);

        /// <summary>
        /// Gets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetDouble(int index) => GetDouble(_ordinalMap[index]);

        /// <summary>
        /// Gets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        DateTime IRecord.GetDateTime(int index) => GetDateTime(_ordinalMap[index]);

        /// <summary>
        /// Gets the feature object.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(int index) => GetFeatureObject(_ordinalMap[index]);

        /// <summary>
        /// Gets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short GetInt16(int index) => GetInt16(_ordinalMap[index]);

        /// <summary>
        /// Gets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetInt32(int index) => GetInt32(_ordinalMap[index]);

        /// <summary>
        /// Gets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetInt64(int index) => GetInt64(_ordinalMap[index]);

        /// <summary>
        /// Gets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetSingle(int index) => GetSingle(_ordinalMap[index]);

        /// <summary>
        /// Gets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index) => GetString(_ordinalMap[index]);

        /// <summary>
        /// Gets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IGeometry GetGeometry(int index) => GetGeometry(_ordinalMap[index]);

        /// <summary>
        /// Gets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        public object this[int index] => this[_ordinalMap[index]];

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
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

        /// <summary>
        /// Gets the number of fields in this record
        /// </summary>
        public int FieldCount => _values.Count;

        /// <summary>
        /// Gets the name of the field at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetName(int index) => _ordinalMap[index];

        /// <summary>
        /// Gets the CLR type of the field at the specified index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i) => ClrFdoTypeMap.GetClrType(_values[GetName(i)].Type);

        /// <summary>
        /// Updates the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Update(IRecord record)
        {
            if (record.FieldCount != this.FieldCount)
                throw new InvalidOperationException(Strings.ErrorJaggedResultSet);

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

        /// <summary>
        /// Gets the property names.
        /// </summary>
        public IEnumerable<string> PropertyNames => _ordinalMap.Values;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public PropertyValueType GetPropertyType(string name) => _values[name].Type;

        /// <summary>
        /// Gets the type of the property at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public PropertyValueType GetPropertyType(int index) => GetPropertyType(_ordinalMap[index]);
    }
}