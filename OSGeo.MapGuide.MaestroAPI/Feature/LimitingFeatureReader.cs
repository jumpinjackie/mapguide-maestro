#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using System.Linq;
using System.Text;

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

        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }

        public int FieldCount
        {
            get { return _reader.FieldCount; }
        }

        public string GetName(int index)
        {
            return _reader.GetName(index);
        }

        public Type GetFieldType(int i)
        {
            return _reader.GetFieldType(i);
        }

        public bool IsNull(string name)
        {
            return _reader.IsNull(name);
        }

        public bool IsNull(int index)
        {
            return _reader.IsNull(index);
        }

        public bool GetBoolean(string name)
        {
            return _reader.GetBoolean(name);
        }

        public byte GetByte(string name)
        {
            return _reader.GetByte(name);
        }

        public byte[] GetBlob(string name)
        {
            return _reader.GetBlob(name);
        }

        public char[] GetClob(string name)
        {
            return _reader.GetClob(name);
        }

        public double GetDouble(string name)
        {
            return _reader.GetDouble(name);
        }

        public DateTime GetDateTime(string name)
        {
            return _reader.GetDateTime(name);
        }

        public short GetInt16(string name)
        {
            return _reader.GetInt16(name);
        }

        public int GetInt32(string name)
        {
            return _reader.GetInt32(name);
        }

        public long GetInt64(string name)
        {
            return _reader.GetInt64(name);
        }

        public float GetSingle(string name)
        {
            return _reader.GetSingle(name);
        }

        public string GetString(string name)
        {
            return _reader.GetString(name);
        }

        public GeoAPI.Geometries.IGeometry GetGeometry(string name)
        {
            return _reader.GetGeometry(name);
        }

        public bool GetBoolean(int index)
        {
            return _reader.GetBoolean(index);
        }

        public byte GetByte(int index)
        {
            return _reader.GetByte(index);
        }

        public byte[] GetBlob(int index)
        {
            return _reader.GetBlob(index);
        }

        public char[] GetClob(int index)
        {
            return _reader.GetClob(index);
        }

        public double GetDouble(int index)
        {
            return _reader.GetDouble(index);
        }

        public DateTime GetDateTime(int index)
        {
            return _reader.GetDateTime(index);
        }

        public short GetInt16(int index)
        {
            return _reader.GetInt16(index);
        }

        public int GetInt32(int index)
        {
            return _reader.GetInt32(index);
        }

        public long GetInt64(int index)
        {
            return _reader.GetInt64(index);
        }

        public float GetSingle(int index)
        {
            return _reader.GetSingle(index);
        }

        public string GetString(int index)
        {
            return _reader.GetString(index);
        }

        public GeoAPI.Geometries.IGeometry GetGeometry(int index)
        {
            return _reader.GetGeometry(index);
        }

        public object this[int index]
        {
            get { return _reader[index]; }
        }

        public object this[string name]
        {
            get { return _reader[name]; }
        }

        public Schema.PropertyValueType GetPropertyType(string name)
        {
            return _reader.GetPropertyType(name);
        }

        public Schema.PropertyValueType GetPropertyType(int index)
        {
            return _reader.GetPropertyType(index);
        }

        public Schema.ClassDefinition ClassDefinition
        {
            get { return _reader.ClassDefinition; }
        }

        public IFeatureReader GetFeatureObject(string name)
        {
            return _reader.GetFeatureObject(name);
        }

        public IFeatureReader GetFeatureObject(int index)
        {
            return _reader.GetFeatureObject(index);
        }

        public IEnumerator<IFeature> GetEnumerator()
        {
            return _reader.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _reader.GetEnumerator();
        }
    }
}
