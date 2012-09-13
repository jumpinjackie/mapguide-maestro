#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
    /// A record whose properties can be modified
    /// </summary>
    public class MutableRecordBase : RecordBase, IMutableRecord
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="prototype"></param>
        protected MutableRecordBase(IRecordInitialize prototype)
        {
            _values.Clear();
            if (prototype != null)
            {
                foreach (string name in prototype.PropertyNames)
                {
                    var src = prototype.GetValue(name);
                    _values[name] = ClonePropertyValue(src);
                }
            }
        }

        /// <summary>
        /// Creates a clone of the specified <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.PropertyValue"/>
        /// </summary>
        /// <param name="propertyValue">The instance to clone</param>
        /// <returns>The cloned instance</returns>
        protected PropertyValue ClonePropertyValue(PropertyValue propertyValue)
        {
            if (propertyValue.IsNull)
            {
                switch (propertyValue.Type)
                {
                    case PropertyValueType.Blob:
                        return new BlobValue();
                    case PropertyValueType.Boolean:
                        return new BooleanValue();
                    case PropertyValueType.Byte:
                        return new ByteValue();
                    case PropertyValueType.Clob:
                        return new ClobValue();
                    case PropertyValueType.DateTime:
                        return new DateTimeValue();
                    case PropertyValueType.Double:
                        return new DoubleValue();
                    case PropertyValueType.Feature:
                        return new FeatureValue();
                    case PropertyValueType.Geometry:
                        return new GeometryValue();
                    case PropertyValueType.Int16:
                        return new Int16Value();
                    case PropertyValueType.Int32:
                        return new Int32Value();
                    case PropertyValueType.Int64:
                        return new Int64Value();
                    case PropertyValueType.Raster:
                        return new RasterValue();
                    case PropertyValueType.Single:
                        return new SingleValue();
                    case PropertyValueType.String:
                        return new StringValue();
                }
            }
            else
            {
                switch (propertyValue.Type)
                {
                    case PropertyValueType.Blob:
                        return new BlobValue(((BlobValue)propertyValue).Value);
                    case PropertyValueType.Boolean:
                        return new BooleanValue(((BooleanValue)propertyValue).Value);
                    case PropertyValueType.Byte:
                        return new ByteValue(((ByteValue)propertyValue).Value);
                    case PropertyValueType.Clob:
                        return new ClobValue(((ClobValue)propertyValue).Value);
                    case PropertyValueType.DateTime:
                        return new DateTimeValue(((DateTimeValue)propertyValue).Value);
                    case PropertyValueType.Double:
                        return new DoubleValue(((DoubleValue)propertyValue).Value);
                    case PropertyValueType.Feature:
                        return new FeatureValue(((FeatureValue)propertyValue).Value);
                    case PropertyValueType.Geometry:
                        return new GeometryValue(((GeometryValue)propertyValue).Value);
                    case PropertyValueType.Int16:
                        return new Int16Value(((Int16Value)propertyValue).Value);
                    case PropertyValueType.Int32:
                        return new Int32Value(((Int32Value)propertyValue).Value);
                    case PropertyValueType.Int64:
                        return new Int64Value(((Int64Value)propertyValue).Value);
                    case PropertyValueType.Raster:
                        return new RasterValue(((RasterValue)propertyValue).Value);
                    case PropertyValueType.Single:
                        return new SingleValue(((SingleValue)propertyValue).Value);
                    case PropertyValueType.String:
                        return new StringValue(((StringValue)propertyValue).Value);
                }
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        public object this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                this[_ordinalMap[index]] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        public object this[string name]
        {
            get
            {
                return this[name];
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException(Strings.ErrorObjectSetNullValuesNotPermitted);

                switch (_values[name].Type)
                {
                    case PropertyValueType.Blob:
                        SetBlob(name, (byte[])value);
                        break;
                    case PropertyValueType.Boolean:
                        SetBoolean(name, (bool)value);
                        break;
                    case PropertyValueType.Byte:
                        SetByte(name, (byte)value);
                        break;
                    case PropertyValueType.Clob:
                        SetClob(name, (char[])value);
                        break;
                    case PropertyValueType.DateTime:
                        SetDateTime(name, (DateTime)value);
                        break;
                    case PropertyValueType.Double:
                        SetDouble(name, (double)value);
                        break;
                    case PropertyValueType.Geometry:
                        SetGeometry(name, (IGeometry)value);
                        break;
                    case PropertyValueType.Int16:
                        SetInt16(name, (short)value);
                        break;
                    case PropertyValueType.Int32:
                        SetInt32(name, (int)value);
                        break;
                    case PropertyValueType.Int64:
                        SetInt64(name, (long)value);
                        break;
                    case PropertyValueType.Single:
                        SetSingle(name, (float)value);
                        break;
                    case PropertyValueType.String:
                        SetString(name, (string)value);
                        break;
                }
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Sets whether the specified property name has a null property value
        /// </summary>
        /// <param name="name"></param>
        public void SetNull(string name)
        {
            _values[name].SetNull();
        }

        /// <summary>
        /// Sets whether the property value at the specified index has a null property value.
        /// </summary>
        /// <param name="index"></param>
        public void SetNull(int index)
        {
            SetNull(_ordinalMap[index]);
        }

        /// <summary>
        /// Sets the boolean value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetBoolean(string name, bool value)
        {
            var propVal = _values[name] as BooleanValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Boolean.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the byte value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetByte(string name, byte value)
        {
            var propVal = _values[name] as ByteValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Byte.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the blob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetBlob(string name, byte[] value)
        {
            var propVal = _values[name] as BlobValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Blob.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the clob value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetClob(string name, char[] value)
        {
            var propVal = _values[name] as ClobValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Clob.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the double value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetDouble(string name, double value)
        {
            var propVal = _values[name] as DoubleValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Double.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the datetime value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetDateTime(string name, DateTime value)
        {
            var propVal = _values[name] as DateTimeValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.DateTime.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the int16 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInt16(string name, short value)
        {
            var propVal = _values[name] as Int16Value;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Int16.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the int32 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInt32(string name, int value)
        {
            var propVal = _values[name] as Int32Value;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Int32.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the int64 value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetInt64(string name, long value)
        {
            var propVal = _values[name] as Int64Value;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Int64.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the single value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetSingle(string name, float value)
        {
            var propVal = _values[name] as SingleValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Single.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the string value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetString(string name, string value)
        {
            var propVal = _values[name] as StringValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.String.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the geometry value of the specified property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetGeometry(string name, IGeometry value)
        {
            var propVal = _values[name] as GeometryValue;
            if (propVal == null)
                throw new InvalidOperationException(string.Format(Strings.ERR_PROPERTY_VALUE_NOT_OF_TYPE, name, PropertyValueType.Geometry.ToString()));

            propVal.Value = value;
        }

        /// <summary>
        /// Sets the boolean value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetBoolean(int index, bool value)
        {
            SetBoolean(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the byte value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetByte(int index, byte value)
        {
            SetByte(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the blob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetBlob(int index, byte[] value)
        {
            SetBlob(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the clob value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetClob(int index, char[] value)
        {
            SetClob(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the double value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetDouble(int index, double value)
        {
            SetDouble(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the datetime value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetDateTime(int index, DateTime value)
        {
            SetDateTime(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the int16 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetInt16(int index, short value)
        {
            SetInt16(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the int32 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetInt32(int index, int value)
        {
            SetInt32(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the int64 value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetInt64(int index, long value)
        {
            SetInt64(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the single value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetSingle(int index, float value)
        {
            SetSingle(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the string value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetString(int index, string value)
        {
            SetString(_ordinalMap[index], value);
        }

        /// <summary>
        /// Sets the geometry value at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetGeometry(int index, IGeometry value)
        {
            SetGeometry(_ordinalMap[index], value);
        }
    }

    /// <summary>
    /// A record whose properties can be modified and allows for adding of new 
    /// <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.PropertyValue"/> instances
    /// </summary>
    public class MutableRecord : MutableRecordBase
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public MutableRecord() : base(null) { }
    }
}