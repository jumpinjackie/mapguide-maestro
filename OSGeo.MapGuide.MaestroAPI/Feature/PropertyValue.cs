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
using System.Globalization;
using GeoAPI.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base class of all MapGuide property values. Functions as a nullable box type
    /// around an underlying data type
    /// </summary>
    public abstract class PropertyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValue"/> class.
        /// </summary>
        protected PropertyValue() { this.IsNull = true; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is null; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsNull { get; protected set; }

        /// <summary>
        /// Sets the value to null.
        /// </summary>
        public virtual void SetNull() { this.IsNull = true; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public abstract PropertyValueType Type { get; }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public abstract PropertyDefinitionType PropertyDefType { get; }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public abstract string ValueAsString();
    }

    /// <summary>
    /// Base class of all nullable value type property values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueTypePropertyValue<T> : PropertyValue where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypePropertyValue&lt;T&gt;"/> class.
        /// </summary>
        protected ValueTypePropertyValue() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypePropertyValue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected ValueTypePropertyValue(T value)
            : base()
        {
            _value = value;
        }

        private Nullable<T> _value;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is null; otherwise, <c>false</c>.
        /// </value>
        public override bool IsNull
        {
            get
            {
                return !_value.HasValue;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value
        {
            get
            {
                if (IsNull)
                    throw new Exception(Strings.ErrorNullValue); //LOCALIZEME

                return _value.Value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Sets the value to null.
        /// </summary>
        public override void SetNull()
        {
            _value = null;
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Data; }
        }
    }

    /// <summary>
    /// Base class of all reference type property values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReferenceTypePropertyValue<T> : PropertyValue where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTypePropertyValue&lt;T&gt;"/> class.
        /// </summary>
        protected ReferenceTypePropertyValue() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTypePropertyValue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected ReferenceTypePropertyValue(T value)
            : base()
        {
            this.Value = value;
        }

        private T _value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value
        {
            get
            {
                if (IsNull)
                    throw new Exception(Strings.ErrorNullValue);

                return _value;
            }
            set
            {
                _value = value;
                this.IsNull = (value == null);
            }
        }
    }

    /// <summary>
    /// Stores byte data
    /// </summary>
    public class ByteValue : ValueTypePropertyValue<byte>
    {
        /// <summary>
        /// Initializes this instance
        /// </summary>
        public ByteValue() : base() { }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        public ByteValue(byte value) : base() { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Byte; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores boolean data
    /// </summary>
    public class BooleanValue : ValueTypePropertyValue<bool>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public BooleanValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public BooleanValue(bool value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Boolean; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString();
        }
    }

    /// <summary>
    /// Stores blob data
    /// </summary>
    public class BlobValue : ReferenceTypePropertyValue<byte[]>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public BlobValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public BlobValue(byte[] value) : base(value) { } 

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Blob; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Data; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Stores clob data
    /// </summary>
    public class ClobValue : ReferenceTypePropertyValue<char[]>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public ClobValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public ClobValue(char[] value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Clob; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Data; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Stores datetime data
    /// </summary>
    public class DateTimeValue : ValueTypePropertyValue<DateTime>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public DateTimeValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public DateTimeValue(DateTime value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.DateTime; }
        }

        static string PadLeft(string str, char ch, int totalChars)
        {
            var value = str;
            while (value.Length < totalChars)
                value = ch + value;
            return value;
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            var dt = this.Value;
            return string.Format("TIMESTAMP '{0}-{1}-{2} {3}:{4}:{5}'", //NOXLATE
                PadLeft(dt.Year.ToString(), '0', 4), //NOXLATE
                PadLeft(dt.Month.ToString(), '0', 2), //NOXLATE
                PadLeft(dt.Year.ToString(), '0', 2), //NOXLATE
                PadLeft(dt.Hour.ToString(), '0', 2), //NOXLATE
                PadLeft(dt.Minute.ToString(), '0', 2), //NOXLATE
                PadLeft(dt.Second.ToString(), '0', 2)); //NOXLATE
        }
    }

    /// <summary>
    /// Stores double data
    /// </summary>
    public class DoubleValue : ValueTypePropertyValue<double>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public DoubleValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public DoubleValue(double value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Double; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores feature data
    /// </summary>
    public class FeatureValue : ReferenceTypePropertyValue<IFeature[]>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public FeatureValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public FeatureValue(IFeature[] values) : base(values) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Feature; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Object; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Stores geometry data
    /// </summary>
    public class GeometryValue : ReferenceTypePropertyValue<IGeometry>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public GeometryValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public GeometryValue(IGeometry value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Geometry; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Geometry; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.AsText();
        }
    }

    /// <summary>
    /// Stores int16 data
    /// </summary>
    public class Int16Value : ValueTypePropertyValue<short>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int16Value() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int16Value(short value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int16; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores int32 data
    /// </summary>
    public class Int32Value : ValueTypePropertyValue<int>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int32Value() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int32Value(int value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int32; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores int64 data
    /// </summary>
    public class Int64Value : ValueTypePropertyValue<long>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int64Value() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public Int64Value(long value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int64; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores raster data
    /// </summary>
    public class RasterValue : ReferenceTypePropertyValue<byte[]>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public RasterValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public RasterValue(byte[] value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Raster; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Raster; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Stores float data
    /// </summary>
    public class SingleValue : ValueTypePropertyValue<float>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public SingleValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public SingleValue(float value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Single; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Stores string data
    /// </summary>
    public class StringValue : ReferenceTypePropertyValue<string>
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public StringValue() : base() { }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public StringValue(string value) : base(value) { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.String; }
        }

        /// <summary>
        /// Gets the suggsted property definition type for this value
        /// </summary>
        /// <value>
        /// The suggsted property definition type.
        /// </value>
        public override PropertyDefinitionType PropertyDefType
        {
            get { return PropertyDefinitionType.Data; }
        }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        /// <returns></returns>
        public override string ValueAsString()
        {
            if (IsNull)
                throw new Exception(Strings.ErrorNullValue);

            return this.Value;
        }
    }
}
