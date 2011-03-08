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
using Topology.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Provides a forward-only, read-only iterator for reading data. You must call <see cref="ReadNext"/> 
    /// before you can access any data
    /// </summary>
    public interface IReader : IDisposable, IRecord
    {
        /// <summary>
        /// Advances the reader to the next item and determines whether there is another object to read. 
        /// </summary>
        /// <returns></returns>
        bool ReadNext();

        /// <summary>
        /// Closes the object, freeing any resources it may be holding. 
        /// </summary>
        void Close();
    }

    /// <summary>
    /// Defines the types of readers
    /// </summary>
    public enum ReaderType : int
    {
        /// <summary>
        /// The reader is a Data Reader
        /// </summary>
        Data = 1,
        /// <summary>
        /// The reader is a SQL Reader
        /// </summary>
        Sql = 2,
        /// <summary>
        /// The reader is a Feature Reader
        /// </summary>
        Feature = 0
    }

    /// <summary>
    /// Provides a means for resetting a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IRecord"/>
    /// instance
    /// </summary>
    public interface IRecordReset
    {
        void Update(IRecord record);
    }

    /// <summary>
    /// Provides a means for initializing a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IRecord"/>
    /// instance with <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.PropertyValue"/> instances
    /// </summary>
    public interface IRecordInitialize
    {
        /// <summary>
        /// Gets the specified property value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PropertyValue GetValue(string name);

        /// <summary>
        /// Adds the specified property value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void PutValue(string name, PropertyValue value);
    }

    /// <summary>
    /// Provides access to the property values within each result for a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IReader"/>
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// Gets the number of fields in this record
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// Gets the name of the field at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetName(int index);

        /// <summary>
        /// Gets the CLR type of the field at the specified index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        Type GetFieldType(int i);

        /// <summary>
        /// Gets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsNull(string name);

        /// <summary>
        /// Gets whether the property value at the specified index has a null property value. You must
        /// call this method first to determine if it is safe to call the corresponding GetXXX() methods
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool IsNull(int index);

        /// <summary>
        /// Gets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetBoolean(string name);

        /// <summary>
        /// Gets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        byte GetByte(string name);

        /// <summary>
        /// Gets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        byte[] GetBlob(string name);

        /// <summary>
        /// Gets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        char[] GetClob(string name);

        /// <summary>
        /// Gets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        double GetDouble(string name);

        /// <summary>
        /// Gets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DateTime GetDateTime(string name);

        /// <summary>
        /// Gets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        short GetInt16(string name);
        
        /// <summary>
        /// Gets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetInt32(string name);

        /// <summary>
        /// Gets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        long GetInt64(string name);

        /// <summary>
        /// Gets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        float GetSingle(string name);

        /// <summary>
        /// Gets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetString(string name);

        /// <summary>
        /// Gets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IGeometry GetGeometry(string name);

        /// <summary>
        /// Gets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool GetBoolean(int index);

        /// <summary>
        /// Gets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        byte GetByte(int index);

        /// <summary>
        /// Gets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        byte[] GetBlob(int index);

        /// <summary>
        /// Gets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        char[] GetClob(int index);

        /// <summary>
        /// Gets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        double GetDouble(int index);

        /// <summary>
        /// Gets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        DateTime GetDateTime(int index);
        
        /// <summary>
        /// Gets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        short GetInt16(int index);

        /// <summary>
        /// Gets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetInt32(int index);
        
        /// <summary>
        /// Gets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        long GetInt64(int index);

        /// <summary>
        /// Gets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        float GetSingle(int index);

        /// <summary>
        /// Gets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetString(int index);
        
        /// <summary>
        /// Gets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IGeometry GetGeometry(int index);

        //byte[] GetRaster(string name);
        //byte[] GetRaster(int index);

        /// <summary>
        /// Gets the object at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object this[int index] { get; }

        /// <summary>
        /// Gets the object value for the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object this[string name] { get; }
    }
}
