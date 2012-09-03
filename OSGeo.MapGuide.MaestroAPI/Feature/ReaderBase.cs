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
using GeoAPI.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/>
    /// interface
    /// </summary>
    public abstract class ReaderBase : IReader
    {
        /// <summary>
        /// Gets the current iterated record
        /// </summary>
        public IRecord Current { get; private set; }

        /// <summary>
        /// Gets the type of the reader.
        /// </summary>
        /// <value>
        /// The type of the reader.
        /// </value>
        public abstract ReaderType ReaderType { get; }

        /// <summary>
        /// Gets the number of fields in this record
        /// </summary>
        public int FieldCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name of the field at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract string GetName(int index);

        /// <summary>
        /// Advances the reader to the next item and determines whether there is another object to read.
        /// </summary>
        /// <returns></returns>
        public bool ReadNext()
        {
            this.Current = ReadNextRecord();
            return this.Current != null;
        }

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <returns></returns>
        protected abstract IRecord ReadNextRecord();

        /// <summary>
        /// Closes the object, freeing any resources it may be holding.
        /// </summary>
        public virtual void Close() { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Gets the CLR type of the field at the specified index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract Type GetFieldType(int i);

        /// <summary>
        /// Gets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNull(string name)
        {
            return this.Current.IsNull(name);
        }

        /// <summary>
        /// Gets whether the property value at the specified index has a null property value. You must
        /// call this method first to determine if it is safe to call the corresponding GetXXX() methods
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsNull(int index)
        {
            return this.Current.IsNull(index);
        }

        /// <summary>
        /// Gets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBoolean(string name)
        {
            return this.Current.GetBoolean(name);
        }

        /// <summary>
        /// Gets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte GetByte(string name)
        {
            return this.Current.GetByte(name);
        }

        /// <summary>
        /// Gets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetBlob(string name)
        {
            return this.Current.GetBlob(name);
        }

        /// <summary>
        /// Gets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public char[] GetClob(string name)
        {
            return this.Current.GetClob(name);
        }

        /// <summary>
        /// Gets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetDouble(string name)
        {
            return this.Current.GetDouble(name);
        }

        /// <summary>
        /// Gets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string name)
        {
            return this.Current.GetDateTime(name);
        }

        /// <summary>
        /// Gets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public short GetInt16(string name)
        {
            return this.Current.GetInt16(name);
        }

        /// <summary>
        /// Gets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt32(string name)
        {
            return this.Current.GetInt32(name);
        }

        /// <summary>
        /// Gets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetInt64(string name)
        {
            return this.Current.GetInt64(name);
        }

        /// <summary>
        /// Gets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetSingle(string name)
        {
            return this.Current.GetSingle(name);
        }

        /// <summary>
        /// Gets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            return this.Current.GetString(name);
        }

        /// <summary>
        /// Gets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IGeometry GetGeometry(string name)
        {
            return this.Current.GetGeometry(name);
        }

        /// <summary>
        /// Gets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBoolean(int index)
        {
            return this.Current.GetBoolean(index);
        }

        /// <summary>
        /// Gets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte GetByte(int index)
        {
            return this.Current.GetByte(index);
        }

        /// <summary>
        /// Gets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] GetBlob(int index)
        {
            return this.Current.GetBlob(index);
        }

        /// <summary>
        /// Gets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char[] GetClob(int index)
        {
            return this.Current.GetClob(index);
        }

        /// <summary>
        /// Gets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetDouble(int index)
        {
            return this.Current.GetDouble(index);
        }

        /// <summary>
        /// Gets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int index)
        {
            return this.Current.GetDateTime(index);
        }

        /// <summary>
        /// Gets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short GetInt16(int index)
        {
            return this.Current.GetInt16(index);
        }

        /// <summary>
        /// Gets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetInt32(int index)
        {
            return this.Current.GetInt32(index);
        }

        /// <summary>
        /// Gets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long GetInt64(int index)
        {
            return this.Current.GetInt64(index);
        }

        /// <summary>
        /// Gets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetSingle(int index)
        {
            return this.Current.GetSingle(index);
        }

        /// <summary>
        /// Gets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index)
        {
            return this.Current.GetString(index);
        }

        /// <summary>
        /// Gets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IGeometry GetGeometry(int index)
        {
            return this.Current.GetGeometry(index);
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        public object this[int index]
        {
            get { return this.Current[index]; }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        public object this[string name]
        {
            get { return this.Current[name]; }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract OSGeo.MapGuide.MaestroAPI.Schema.PropertyValueType GetPropertyType(string name);

        /// <summary>
        /// Gets the type of the property at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public abstract OSGeo.MapGuide.MaestroAPI.Schema.PropertyValueType GetPropertyType(int index);
    }
}
