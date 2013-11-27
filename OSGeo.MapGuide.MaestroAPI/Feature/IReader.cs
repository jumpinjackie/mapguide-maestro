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
using GeoAPI.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Provides a forward-only, read-only iterator for reading data. You must call <see cref="ReadNext"/> 
    /// before you can access any data
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>You must call <see cref="M:ReadNext"/> before you can access the data</description></item>
    /// <item><description>For each property, determine the property type and then call the appropriate Get&lt;type&gt;() method to get the value of the property.</description></item>
    /// <item><description>The exception for this is if you are access the value via the indexer. In this case you only need determine the property type when casting from the System.Object that is returned by the indexer</description></item>
    /// </list>
    /// </remarks>
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
        /// <summary>
        /// Updates the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        void Update(IRecord record);
    }

    /// <summary>
    /// Provides a means for initializing a <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IRecord"/>
    /// instance with <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.PropertyValue"/> instances
    /// </summary>
    public interface IRecordInitialize
    {
        /// <summary>
        /// Gets the property names.
        /// </summary>
        IEnumerable<string> PropertyNames { get; }

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
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>For each property, determine the property type and then call the appropriate Get&lt;type&gt;() method to get the value of the property.</description></item>
    /// <item><description>The exception for this is if you are access the value via the indexer. In this case you only need determine the property type when casting from the System.Object that is returned by the indexer</description></item>
    /// </list>
    /// </remarks>
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

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        PropertyValueType GetPropertyType(string name);
        /// <summary>
        /// Gets the type of the property at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        PropertyValueType GetPropertyType(int index);
    }

    /// <summary>
    /// Defines a records whose properties can be modified
    /// </summary>
    /// <remarks>
    /// The default implementation of this interface is <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.MutableRecord"/>
    /// </remarks>
    public interface IMutableRecord : IRecord, IRecordInitialize
    {
        /// <summary>
        /// Gets or sets the object at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object this[int index] { set; get; }

        /// <summary>
        /// Gets or sets the object value for the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object this[string name] { set; get; }

        /// <summary>
        /// Sets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void SetNull(string name);

        /// <summary>
        /// Sets whether the property value at the specified index has a null property value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        void SetNull(int index);

        /// <summary>
        /// Sets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetBoolean(string name, bool value);

        /// <summary>
        /// Sets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetByte(string name, byte value);

        /// <summary>
        /// Sets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetBlob(string name, byte[] value);

        /// <summary>
        /// Sets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetClob(string name, char[] value);

        /// <summary>
        /// Sets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetDouble(string name, double value);

        /// <summary>
        /// Sets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetDateTime(string name, DateTime value);

        /// <summary>
        /// Sets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt16(string name, short value);

        /// <summary>
        /// Sets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt32(string name, int value);

        /// <summary>
        /// Sets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt64(string name, long value);

        /// <summary>
        /// Sets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetSingle(string name, float value);

        /// <summary>
        /// Sets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetString(string name, string value);

        /// <summary>
        /// Sets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetGeometry(string name, IGeometry value);

        /// <summary>
        /// Sets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetBoolean(int index, bool value);

        /// <summary>
        /// Sets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetByte(int index, byte value);

        /// <summary>
        /// Sets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetBlob(int index, byte[] value);

        /// <summary>
        /// Sets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetClob(int index, char[] value);

        /// <summary>
        /// Sets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetDouble(int index, double value);

        /// <summary>
        /// Sets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetDateTime(int index, DateTime value);

        /// <summary>
        /// Sets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt16(int index, short value);

        /// <summary>
        /// Sets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt32(int index, int value);

        /// <summary>
        /// Sets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetInt64(int index, long value);

        /// <summary>
        /// Sets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetSingle(int index, float value);

        /// <summary>
        /// Sets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetString(int index, string value);

        /// <summary>
        /// Sets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void SetGeometry(int index, IGeometry value);
    }
}
