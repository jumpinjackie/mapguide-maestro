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
using Topology.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base class of all MapGuide property values. Functions as a nullable box type
    /// around an underlying data type
    /// </summary>
    public abstract class PropertyValue
    {
        protected PropertyValue() { this.IsNull = true; }

        public virtual bool IsNull { get; protected set; }

        public virtual void SetNull() { this.IsNull = true; }

        public abstract PropertyValueType Type { get; }
    }

    /// <summary>
    /// Base class of all nullable value type property values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueTypePropertyValue<T> : PropertyValue where T : struct
    {
        protected ValueTypePropertyValue() : base() { }

        protected ValueTypePropertyValue(T value)
            : base()
        {
            _value = value;
        }

        private Nullable<T> _value;

        public override bool IsNull
        {
            get
            {
                return !_value.HasValue;
            }
        }

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
        protected ReferenceTypePropertyValue() : base() { }

        protected ReferenceTypePropertyValue(T value)
            : base()
        {
            _value = value;
            this.IsNull = false;
        }

        private T _value;

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
        public override PropertyValueType Type
        {
            get { return PropertyValueType.String; }
        }
    }

}
