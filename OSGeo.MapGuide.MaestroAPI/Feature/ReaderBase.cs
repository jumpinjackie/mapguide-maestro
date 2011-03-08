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

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/>
    /// interface
    /// </summary>
    public abstract class ReaderBase : IReader
    {
        public IRecord Current { get; private set; }

        public abstract ReaderType ReaderType { get; }

        public int FieldCount
        {
            get;
            protected set;
        }

        public abstract string GetName(int index);

        public bool ReadNext()
        {
            this.Current = ReadNextRecord();
            return this.Current != null;
        }

        protected abstract IRecord ReadNextRecord();

        public virtual void Close() { }

        public virtual void Dispose() { }

        public abstract Type GetFieldType(int i);

        public bool IsNull(string name)
        {
            return this.Current.IsNull(name);
        }

        public bool IsNull(int index)
        {
            return this.Current.IsNull(index);
        }

        public bool GetBoolean(string name)
        {
            return this.Current.GetBoolean(name);
        }

        public byte GetByte(string name)
        {
            return this.Current.GetByte(name);
        }

        public byte[] GetBlob(string name)
        {
            return this.Current.GetBlob(name);
        }

        public char[] GetClob(string name)
        {
            return this.Current.GetClob(name);
        }

        public double GetDouble(string name)
        {
            return this.Current.GetDouble(name);
        }

        public DateTime GetDateTime(string name)
        {
            return this.Current.GetDateTime(name);
        }

        

        public short GetInt16(string name)
        {
            return this.Current.GetInt16(name);
        }

        public int GetInt32(string name)
        {
            return this.Current.GetInt32(name);
        }

        public long GetInt64(string name)
        {
            return this.Current.GetInt64(name);
        }

        public float GetSingle(string name)
        {
            return this.Current.GetSingle(name);
        }

        public string GetString(string name)
        {
            return this.Current.GetString(name);
        }

        public Topology.Geometries.IGeometry GetGeometry(string name)
        {
            return this.Current.GetGeometry(name);
        }

        public bool GetBoolean(int index)
        {
            return this.Current.GetBoolean(index);
        }

        public byte GetByte(int index)
        {
            return this.Current.GetByte(index);
        }

        public byte[] GetBlob(int index)
        {
            return this.Current.GetBlob(index);
        }

        public char[] GetClob(int index)
        {
            return this.Current.GetClob(index);
        }

        public double GetDouble(int index)
        {
            return this.Current.GetDouble(index);
        }

        public DateTime GetDateTime(int index)
        {
            return this.Current.GetDateTime(index);
        }

        public short GetInt16(int index)
        {
            return this.Current.GetInt16(index);
        }

        public int GetInt32(int index)
        {
            return this.Current.GetInt32(index);
        }

        public long GetInt64(int index)
        {
            return this.Current.GetInt64(index);
        }

        public float GetSingle(int index)
        {
            return this.Current.GetSingle(index);
        }

        public string GetString(int index)
        {
            return this.Current.GetString(index);
        }

        public Topology.Geometries.IGeometry GetGeometry(int index)
        {
            return this.Current.GetGeometry(index);
        }

        public object this[int index]
        {
            get { return this.Current[index]; }
        }

        public object this[string name]
        {
            get { return this.Current[name]; }
        }
    }
}
