#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using OSGeo.MapGuide.MaestroAPI.Geometry;
using System;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Wraps a <see cref="IFeatureReader"/> to only allow up the specified number of iterations
    /// </summary>
    public class LimitingFeatureReader : IFeatureReader
    {
        private IFeatureReader _reader;
        readonly int _limit;
        private int _read;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="limit"></param>
        public LimitingFeatureReader(IFeatureReader reader, int limit)
        {
            _limit = limit;
            _reader = reader;
            _read = 0;
        }

        /// <summary>
        /// Advances the reader to the next item and determines whether there is another object to read.
        /// </summary>
        /// <returns></returns>
        public bool ReadNext()
        {
            if (_read == _limit)
                return false;

            bool bRet = _reader.ReadNext();
            _read++;
            return bRet;
        }

        /// <summary>
        /// Closes the object, freeing any resources it may be holding.
        /// </summary>
        public void Close() => _reader?.Close();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }

        /// <summary>
        /// Gets the number of fields in this record
        /// </summary>
        public int FieldCount => _reader.FieldCount;

        /// <summary>
        /// Gets the name of the field at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetName(int index) => _reader.GetName(index);

        /// <summary>
        /// Gets the CLR type of the field at the specified index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i) => _reader.GetFieldType(i);

        /// <summary>
        /// Gets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNull(string name) => _reader.IsNull(name);

        /// <summary>
        /// Gets whether the property value at the specified index has a null property value. You must
        /// call this method first to determine if it is safe to call the corresponding GetXXX() methods
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsNull(int index) => _reader.IsNull(index);

        /// <summary>
        /// Gets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBoolean(string name) => _reader.GetBoolean(name);

        /// <summary>
        /// Gets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte GetByte(string name) => _reader.GetByte(name);

        /// <summary>
        /// Gets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetBlob(string name) => _reader.GetBlob(name);

        /// <summary>
        /// Gets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public char[] GetClob(string name) => _reader.GetClob(name);

        /// <summary>
        /// Gets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetDouble(string name) => _reader.GetDouble(name);

        /// <summary>
        /// Gets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string name) => _reader.GetDateTime(name);

        /// <summary>
        /// Gets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public short GetInt16(string name) => _reader.GetInt16(name);

        /// <summary>
        /// Gets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt32(string name) => _reader.GetInt32(name);

        /// <summary>
        /// Gets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetInt64(string name) => _reader.GetInt64(name);

        /// <summary>
        /// Gets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetSingle(string name) => _reader.GetSingle(name);

        /// <summary>
        /// Gets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name) => _reader.GetString(name);

        /// <summary>
        /// Gets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IGeometryRef GetGeometry(string name) => _reader.GetGeometry(name);

        /// <summary>
        /// Gets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBoolean(int index) => _reader.GetBoolean(index);

        /// <summary>
        /// Gets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte GetByte(int index) => _reader.GetByte(index);

        /// <summary>
        /// Gets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] GetBlob(int index) => _reader.GetBlob(index);

        /// <summary>
        /// Gets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char[] GetClob(int index) => _reader.GetClob(index);

        /// <summary>
        /// Gets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetDouble(int index) => _reader.GetDouble(index);

        /// <summary>
        /// Gets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int index) => _reader.GetDateTime(index);

        /// <summary>
        /// Gets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short GetInt16(int index) => _reader.GetInt16(index);

        /// <summary>
        /// Gets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetInt32(int index) => _reader.GetInt32(index);

        /// <summary>
        /// Gets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetInt64(int index) => _reader.GetInt64(index);

        /// <summary>
        /// Gets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetSingle(int index) => _reader.GetSingle(index);

        /// <summary>
        /// Gets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index) => _reader.GetString(index);

        /// <summary>
        /// Gets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IGeometryRef GetGeometry(int index) => _reader.GetGeometry(index);

        /// <summary>
        /// Gets the object at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index] => _reader[index];

        /// <summary>
        /// Gets the object value for the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name] => _reader[name];

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Schema.PropertyValueType GetPropertyType(string name) => _reader.GetPropertyType(name);

        /// <summary>
        /// Gets the type of the property at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Schema.PropertyValueType GetPropertyType(int index) => _reader.GetPropertyType(index);

        /// <summary>
        /// Gets the class definition of the object currently being read. If the user has requested
        /// only a subset of the class properties (as specified in the filter text), the class
        /// definition reflects what the user has requested, rather than the full class definition.
        /// </summary>
        public Schema.ClassDefinition ClassDefinition => _reader.ClassDefinition;

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(string name) => _reader.GetFeatureObject(name);

        /// <summary>
        /// Gets a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader"/> containing
        /// all the nested features at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IFeatureReader GetFeatureObject(int index) => _reader.GetFeatureObject(index);

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IFeature> GetEnumerator() => _reader.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _reader.GetEnumerator();
    }
}