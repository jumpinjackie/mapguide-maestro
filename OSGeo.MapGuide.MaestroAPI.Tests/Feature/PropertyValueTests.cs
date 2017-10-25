#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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
using Moq;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Geometry;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Feature
{
    public class PropertyValueTests
    {
        [Fact]
        public void TestCase_BooleanValue()
        {
            var value = new BooleanValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Boolean, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = true;
            Assert.True(value.Value);
            Assert.False(value.IsNull);

            value = new BooleanValue(false);
            Assert.False(value.IsNull);
            Assert.False(value.Value);
            Assert.Equal(false.ToString(), value.ValueAsString());

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_BlobValue()
        {
            var value = new BlobValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Blob, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new byte[] { 1 };
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal(1, value.Value[0]);
            Assert.False(value.IsNull);

            value = new BlobValue(new byte[] { 1 });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal(1, value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_ByteValue()
        {
            byte d = 1;

            var value = new ByteValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Byte, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new ByteValue(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_ClobValue()
        {
            var value = new ClobValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Clob, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new char[] { 'a' };
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal('a', value.Value[0]);
            Assert.False(value.IsNull);

            value = new ClobValue(new char[] { 'a' });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal('a', value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_DateTimeValue()
        {
            var dt = new DateTime(2017, 1, 1);

            var value = new DateTimeValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.DateTime, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = dt;
            Assert.Equal(dt, value.Value);
            Assert.Equal("TIMESTAMP '2017-01-01 00:00:00'", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new DateTimeValue(dt);
            Assert.False(value.IsNull);
            Assert.Equal(dt, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_DoubleValue()
        {
            var d = 1.2;

            var value = new DoubleValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Double, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1.2", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new DoubleValue(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_GeometryValue()
        {
            var mockGeom = new Mock<IGeometryRef>();
            mockGeom.Setup(g => g.AsText()).Returns("POINT (0 0)");

            var value = new GeometryValue();
            Assert.Equal(PropertyDefinitionType.Geometry, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Geometry, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = mockGeom.Object;
            Assert.NotNull(value.Value);
            Assert.Equal("POINT (0 0)", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new GeometryValue(mockGeom.Object);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.Equal("POINT (0 0)", value.ValueAsString());

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_Int16Value()
        {
            short d = 1;

            var value = new Int16Value();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Int16, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int16Value(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_Int32Value()
        {
            int d = 1;

            var value = new Int32Value();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Int32, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int32Value(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_Int64Value()
        {
            long d = 1;

            var value = new Int64Value();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Int64, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int64Value(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_RasterValue()
        {
            var value = new RasterValue();
            Assert.Equal(PropertyDefinitionType.Raster, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Raster, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new byte[] { 1 };
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal(1, value.Value[0]);
            Assert.False(value.IsNull);

            value = new RasterValue(new byte[] { 1 });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.Single(value.Value);
            Assert.Equal(1, value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_SingleValue()
        {
            var d = 1.4f;

            var value = new SingleValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.Single, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.Equal(d, value.Value);
            Assert.Equal("1.4", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new SingleValue(d);
            Assert.False(value.IsNull);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_StringValue()
        {
            var d = "Foo";

            var value = new StringValue();
            Assert.Equal(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.Equal(PropertyValueType.String, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.Equal(d, value.Value);
            Assert.Equal("Foo", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new StringValue(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.Equal(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Fact]
        public void TestCase_ClrFdoTypeMap()
        {
            Assert.Equal(typeof(byte[]), ClrFdoTypeMap.GetClrType(DataPropertyType.Blob));
            Assert.Equal(typeof(bool), ClrFdoTypeMap.GetClrType(DataPropertyType.Boolean));
            Assert.Equal(typeof(byte), ClrFdoTypeMap.GetClrType(DataPropertyType.Byte));
            Assert.Equal(typeof(char[]), ClrFdoTypeMap.GetClrType(DataPropertyType.Clob));
            Assert.Equal(typeof(DateTime), ClrFdoTypeMap.GetClrType(DataPropertyType.DateTime));
            Assert.Equal(typeof(double), ClrFdoTypeMap.GetClrType(DataPropertyType.Double));
            Assert.Equal(typeof(short), ClrFdoTypeMap.GetClrType(DataPropertyType.Int16));
            Assert.Equal(typeof(int), ClrFdoTypeMap.GetClrType(DataPropertyType.Int32));
            Assert.Equal(typeof(long), ClrFdoTypeMap.GetClrType(DataPropertyType.Int64));
            Assert.Equal(typeof(float), ClrFdoTypeMap.GetClrType(DataPropertyType.Single));
            Assert.Equal(typeof(string), ClrFdoTypeMap.GetClrType(DataPropertyType.String));

            DataPropertyDefinition dp = null;

            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Blob;
            Assert.Equal(typeof(byte[]), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Boolean;
            Assert.Equal(typeof(bool), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Byte;
            Assert.Equal(typeof(byte), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Clob;
            Assert.Equal(typeof(char[]), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.DateTime;
            Assert.Equal(typeof(DateTime), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Double;
            Assert.Equal(typeof(double), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int16;
            Assert.Equal(typeof(short), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int32;
            Assert.Equal(typeof(int), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int64;
            Assert.Equal(typeof(long), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Single;
            Assert.Equal(typeof(float), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.String;
            Assert.Equal(typeof(string), ClrFdoTypeMap.GetClrType(dp));

            var gp = new GeometricPropertyDefinition("Geom", "");
            Assert.Equal(typeof(IGeometryRef), ClrFdoTypeMap.GetClrType(gp));

            var op = new RasterPropertyDefinition("Raster", "");
            Assert.Throws<ArgumentException>(() => { var t = ClrFdoTypeMap.GetClrType(op); });

            Assert.Equal(typeof(byte[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Blob));
            Assert.Equal(typeof(bool), ClrFdoTypeMap.GetClrType(PropertyValueType.Boolean));
            Assert.Equal(typeof(byte), ClrFdoTypeMap.GetClrType(PropertyValueType.Byte));
            Assert.Equal(typeof(char[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Clob));
            Assert.Equal(typeof(DateTime), ClrFdoTypeMap.GetClrType(PropertyValueType.DateTime));
            Assert.Equal(typeof(double), ClrFdoTypeMap.GetClrType(PropertyValueType.Double));
            Assert.Equal(typeof(IFeature[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Feature));
            Assert.Equal(typeof(IGeometryRef), ClrFdoTypeMap.GetClrType(PropertyValueType.Geometry));
            Assert.Equal(typeof(short), ClrFdoTypeMap.GetClrType(PropertyValueType.Int16));
            Assert.Equal(typeof(int), ClrFdoTypeMap.GetClrType(PropertyValueType.Int32));
            Assert.Equal(typeof(long), ClrFdoTypeMap.GetClrType(PropertyValueType.Int64));
            Assert.Equal(typeof(float), ClrFdoTypeMap.GetClrType(PropertyValueType.Single));
            Assert.Equal(typeof(string), ClrFdoTypeMap.GetClrType(PropertyValueType.String));
            
        }
    }
}
