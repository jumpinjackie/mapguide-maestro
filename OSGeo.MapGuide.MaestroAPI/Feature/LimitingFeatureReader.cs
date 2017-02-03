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

using System;
using System.Collections.Generic;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    internal class LimitingFeatureReader : IFeatureReader
    {
        private IFeatureReader _reader;
        private int _limit;
        private int _read;

        public LimitingFeatureReader(IFeatureReader reader, int limit)
        {
            _limit = limit;
            _reader = reader;
            _read = 0;
        }

        public bool ReadNext()
        {
            if (_read == _limit)
                return false;

            bool bRet = _reader.ReadNext();
            _read++;
            return bRet;
        }

        public void Close() => _reader?.Close();

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }

        public int FieldCount => _reader.FieldCount;

        public string GetName(int index) => _reader.GetName(index);
        public Type GetFieldType(int i) => _reader.GetFieldType(i);
        public bool IsNull(string name) => _reader.IsNull(name);
        public bool IsNull(int index) => _reader.IsNull(index);
        public bool GetBoolean(string name) => _reader.GetBoolean(name);
        public byte GetByte(string name) => _reader.GetByte(name);
        public byte[] GetBlob(string name) => _reader.GetBlob(name);
        public char[] GetClob(string name) => _reader.GetClob(name);
        public double GetDouble(string name) => _reader.GetDouble(name);
        public DateTime GetDateTime(string name) => _reader.GetDateTime(name);
        public short GetInt16(string name) => _reader.GetInt16(name);
        public int GetInt32(string name) => _reader.GetInt32(name);
        public long GetInt64(string name) => _reader.GetInt64(name);
        public float GetSingle(string name) => _reader.GetSingle(name);
        public string GetString(string name) => _reader.GetString(name);
        public GeoAPI.Geometries.IGeometry GetGeometry(string name) => _reader.GetGeometry(name);
        public bool GetBoolean(int index) => _reader.GetBoolean(index);
        public byte GetByte(int index) => _reader.GetByte(index);
        public byte[] GetBlob(int index) => _reader.GetBlob(index);
        public char[] GetClob(int index) => _reader.GetClob(index);
        public double GetDouble(int index) => _reader.GetDouble(index);
        public DateTime GetDateTime(int index) => _reader.GetDateTime(index);
        public short GetInt16(int index) => _reader.GetInt16(index);
        public int GetInt32(int index) => _reader.GetInt32(index);
        public long GetInt64(int index) => _reader.GetInt64(index);
        public float GetSingle(int index) => _reader.GetSingle(index);
        public string GetString(int index) => _reader.GetString(index);
        public GeoAPI.Geometries.IGeometry GetGeometry(int index) => _reader.GetGeometry(index);

        public object this[int index] => _reader[index];

        public object this[string name] => _reader[name];

        public Schema.PropertyValueType GetPropertyType(string name) => _reader.GetPropertyType(name);
        public Schema.PropertyValueType GetPropertyType(int index) => _reader.GetPropertyType(index);

        public Schema.ClassDefinition ClassDefinition => _reader.ClassDefinition;

        public IFeatureReader GetFeatureObject(string name) => _reader.GetFeatureObject(name);
        public IFeatureReader GetFeatureObject(int index) => _reader.GetFeatureObject(index);
        public IEnumerator<IFeature> GetEnumerator() => _reader.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _reader.GetEnumerator();
    }
}