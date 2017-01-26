#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

#endregion Disclaimer / License
using GeoAPI.Geometries;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Feature
{
    [TestFixture]
    public class PropertyValueTests
    {
        [Test]
        public void TestCase_BooleanValue()
        {
            var value = new BooleanValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Boolean, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = true;
            Assert.True(value.Value);
            Assert.False(value.IsNull);

            value = new BooleanValue(false);
            Assert.False(value.IsNull);
            Assert.False(value.Value);
            Assert.AreEqual(false.ToString(), value.ValueAsString());

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_BlobValue()
        {
            var value = new BlobValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Blob, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new byte[] { 1 };
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual(1, value.Value[0]);
            Assert.False(value.IsNull);

            value = new BlobValue(new byte[] { 1 });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual(1, value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_ByteValue()
        {
            byte d = 1;

            var value = new ByteValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Byte, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new ByteValue(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_ClobValue()
        {
            var value = new ClobValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Clob, value.Type);

            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new char[] { 'a' };
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual('a', value.Value[0]);
            Assert.False(value.IsNull);

            value = new ClobValue(new char[] { 'a' });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual('a', value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_DateTimeValue()
        {
            var dt = new DateTime(2017, 1, 1);

            var value = new DateTimeValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.DateTime, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = dt;
            Assert.NotNull(value.Value);
            Assert.AreEqual(dt, value.Value);
            Assert.AreEqual("TIMESTAMP '2017-01-01 00:00:00'", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new DateTimeValue(dt);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(dt, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_DoubleValue()
        {
            var d = 1.2;

            var value = new DoubleValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Double, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1.2", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new DoubleValue(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_Int16Value()
        {
            short d = 1;

            var value = new Int16Value();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Int16, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int16Value(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_Int32Value()
        {
            int d = 1;

            var value = new Int32Value();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Int32, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int32Value(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_Int64Value()
        {
            long d = 1;

            var value = new Int64Value();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Int64, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new Int64Value(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_RasterValue()
        {
            var value = new RasterValue();
            Assert.AreEqual(PropertyDefinitionType.Raster, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Raster, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<NotSupportedException>(() => { var v = value.ValueAsString(); });
            value.Value = new byte[] { 1 };
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual(1, value.Value[0]);
            Assert.False(value.IsNull);

            value = new RasterValue(new byte[] { 1 });
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(1, value.Value.Length);
            Assert.AreEqual(1, value.Value[0]);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_SingleValue()
        {
            var d = 1.4f;

            var value = new SingleValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.Single, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("1.4", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new SingleValue(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_StringValue()
        {
            var d = "Foo";

            var value = new StringValue();
            Assert.AreEqual(PropertyDefinitionType.Data, value.PropertyDefType);
            Assert.AreEqual(PropertyValueType.String, value.Type);
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
            Assert.Throws<Exception>(() => { var v = value.ValueAsString(); });
            value.Value = d;
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);
            Assert.AreEqual("Foo", value.ValueAsString());
            Assert.False(value.IsNull);

            value = new StringValue(d);
            Assert.False(value.IsNull);
            Assert.NotNull(value.Value);
            Assert.AreEqual(d, value.Value);

            value.SetNull();
            Assert.True(value.IsNull);
            Assert.Throws<Exception>(() => { var v = value.Value; });
        }

        [Test]
        public void TestCase_ClrFdoTypeMap()
        {
            Assert.AreEqual(typeof(byte[]), ClrFdoTypeMap.GetClrType(DataPropertyType.Blob));
            Assert.AreEqual(typeof(bool), ClrFdoTypeMap.GetClrType(DataPropertyType.Boolean));
            Assert.AreEqual(typeof(byte), ClrFdoTypeMap.GetClrType(DataPropertyType.Byte));
            Assert.AreEqual(typeof(char[]), ClrFdoTypeMap.GetClrType(DataPropertyType.Clob));
            Assert.AreEqual(typeof(DateTime), ClrFdoTypeMap.GetClrType(DataPropertyType.DateTime));
            Assert.AreEqual(typeof(double), ClrFdoTypeMap.GetClrType(DataPropertyType.Double));
            Assert.AreEqual(typeof(short), ClrFdoTypeMap.GetClrType(DataPropertyType.Int16));
            Assert.AreEqual(typeof(int), ClrFdoTypeMap.GetClrType(DataPropertyType.Int32));
            Assert.AreEqual(typeof(long), ClrFdoTypeMap.GetClrType(DataPropertyType.Int64));
            Assert.AreEqual(typeof(float), ClrFdoTypeMap.GetClrType(DataPropertyType.Single));
            Assert.AreEqual(typeof(string), ClrFdoTypeMap.GetClrType(DataPropertyType.String));

            DataPropertyDefinition dp = null;

            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Blob;
            Assert.AreEqual(typeof(byte[]), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Boolean;
            Assert.AreEqual(typeof(bool), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Byte;
            Assert.AreEqual(typeof(byte), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Clob;
            Assert.AreEqual(typeof(char[]), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.DateTime;
            Assert.AreEqual(typeof(DateTime), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Double;
            Assert.AreEqual(typeof(double), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int16;
            Assert.AreEqual(typeof(short), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int32;
            Assert.AreEqual(typeof(int), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Int64;
            Assert.AreEqual(typeof(long), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.Single;
            Assert.AreEqual(typeof(float), ClrFdoTypeMap.GetClrType(dp));
            dp = new DataPropertyDefinition("Data", "");
            dp.DataType = DataPropertyType.String;
            Assert.AreEqual(typeof(string), ClrFdoTypeMap.GetClrType(dp));

            var gp = new GeometricPropertyDefinition("Geom", "");
            Assert.AreEqual(typeof(IGeometry), ClrFdoTypeMap.GetClrType(gp));

            var op = new RasterPropertyDefinition("Raster", "");
            Assert.Throws<ArgumentException>(() => { var t = ClrFdoTypeMap.GetClrType(op); });

            Assert.AreEqual(typeof(byte[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Blob));
            Assert.AreEqual(typeof(bool), ClrFdoTypeMap.GetClrType(PropertyValueType.Boolean));
            Assert.AreEqual(typeof(byte), ClrFdoTypeMap.GetClrType(PropertyValueType.Byte));
            Assert.AreEqual(typeof(char[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Clob));
            Assert.AreEqual(typeof(DateTime), ClrFdoTypeMap.GetClrType(PropertyValueType.DateTime));
            Assert.AreEqual(typeof(double), ClrFdoTypeMap.GetClrType(PropertyValueType.Double));
            Assert.AreEqual(typeof(IFeature[]), ClrFdoTypeMap.GetClrType(PropertyValueType.Feature));
            Assert.AreEqual(typeof(IGeometry), ClrFdoTypeMap.GetClrType(PropertyValueType.Geometry));
            Assert.AreEqual(typeof(short), ClrFdoTypeMap.GetClrType(PropertyValueType.Int16));
            Assert.AreEqual(typeof(int), ClrFdoTypeMap.GetClrType(PropertyValueType.Int32));
            Assert.AreEqual(typeof(long), ClrFdoTypeMap.GetClrType(PropertyValueType.Int64));
            Assert.AreEqual(typeof(float), ClrFdoTypeMap.GetClrType(PropertyValueType.Single));
            Assert.AreEqual(typeof(string), ClrFdoTypeMap.GetClrType(PropertyValueType.String));
            
        }
    }
}
