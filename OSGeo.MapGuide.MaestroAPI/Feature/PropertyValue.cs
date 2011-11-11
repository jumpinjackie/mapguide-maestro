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
                    throw new Exception("Null Value"); //LOCALIZEME

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
            _value = value;
            this.IsNull = false;
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
                    throw new Exception("Null Value"); //LOCALIZEME

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
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Byte; }
        }
    }

    /// <summary>
    /// Stores boolean data
    /// </summary>
    public class BooleanValue : ValueTypePropertyValue<bool>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Boolean; }
        }
    }

    /// <summary>
    /// Stores blob data
    /// </summary>
    public class BlobValue : ReferenceTypePropertyValue<byte[]>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Blob; }
        }
    }

    /// <summary>
    /// Stores clob data
    /// </summary>
    public class ClobValue : ReferenceTypePropertyValue<char[]>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Clob; }
        }
    }

    /// <summary>
    /// Stores datetime data
    /// </summary>
    public class DateTimeValue : ValueTypePropertyValue<DateTime>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.DateTime; }
        }
    }

    /// <summary>
    /// Stores double data
    /// </summary>
    public class DoubleValue : ValueTypePropertyValue<double>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Double; }
        }
    }

    /// <summary>
    /// Stores feature data
    /// </summary>
    public class FeatureValue : ReferenceTypePropertyValue<IFeature[]>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Feature; }
        }
    }

    /// <summary>
    /// Stores geometry data
    /// </summary>
    public class GeometryValue : ReferenceTypePropertyValue<IGeometry>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Geometry; }
        }
    }

    /// <summary>
    /// Stores int16 data
    /// </summary>
    public class Int16Value : ValueTypePropertyValue<short>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int16; }
        }
    }

    /// <summary>
    /// Stores int32 data
    /// </summary>
    public class Int32Value : ValueTypePropertyValue<int>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int32; }
        }
    }

    /// <summary>
    /// Stores int64 data
    /// </summary>
    public class Int64Value : ValueTypePropertyValue<long>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Int64; }
        }
    }

    /// <summary>
    /// Stores raster data
    /// </summary>
    public class RasterValue : ReferenceTypePropertyValue<byte[]>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Raster; }
        }
    }

    /// <summary>
    /// Stores float data
    /// </summary>
    public class SingleValue : ValueTypePropertyValue<float>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.Single; }
        }
    }

    /// <summary>
    /// Stores string data
    /// </summary>
    public class StringValue : ReferenceTypePropertyValue<string>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PropertyValueType Type
        {
            get { return PropertyValueType.String; }
        }
    }

}
