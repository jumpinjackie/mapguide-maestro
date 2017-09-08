#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Http;
using OSGeo.MapGuide.MaestroAPI.Mapping.Tests;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class FeatureReaderTests
    {
        static XmlFeatureReader CreateXmlFeatureReader(Stream stream)
        {
            var ctor = typeof(XmlFeatureReader).GetInternalConstructor(new[] { typeof(Stream) });
            return ctor.Invoke(new[] { stream }) as XmlFeatureReader;
        }

        static XmlDataReader CreateXmlDataReader(Stream stream)
        {
            var ctor = typeof(XmlDataReader).GetInternalConstructor(new[] { typeof(Stream) });
            return ctor.Invoke(new[] { stream }) as XmlDataReader;
        }

        [Fact]
        public void TestFeatureArrayReader()
        {
            var fcount = 5;
            var clsDef = new ClassDefinition("Test", "");
            clsDef.AddProperty(new DataPropertyDefinition("ID", "")
            {
                DataType = DataPropertyType.Int32
            }, true);
            var features = Enumerable.Range(0, fcount).Select(i =>
            {
                var feat = new Mock<IFeature>();
                feat.Setup(f => f.ClassDefinition).Returns(clsDef);
                feat.Setup(f => f.FieldCount).Returns(1);
                feat.Setup(f => f.GetInt32(It.Is<string>(arg => arg == "ID"))).Returns(i);
                feat.Setup(f => f.GetInt32(It.Is<int>(arg => arg == 0))).Returns(i);
                return feat.Object;
            });

            var iterations = 0;
            var reader = new FeatureArrayReader(features.ToArray());
            while (reader.ReadNext())
            {
                Assert.Equal(1, reader.FieldCount);
                Assert.Equal(iterations, reader.GetInt32("ID"));
                Assert.Equal(iterations, reader.GetInt32(0));
                Assert.Equal(PropertyValueType.Int32, reader.GetPropertyType(0));
                Assert.Equal(PropertyValueType.Int32, reader.GetPropertyType("ID"));
                iterations++;
            }
            reader.Close();
            Assert.Equal(fcount, iterations);
        }

        [Fact]
        public void TestLimitingFeatureReader()
        {
            int limit = 5;

            var testBlob = new byte[] { 1, 2, 3 };
            var testClob = new char[] { 'a', 'b', 'c' };

            var mockGeom = new Mock<IGeometryRef>();
            mockGeom.Setup(g => g.AsText()).Returns("POINT (0 0)");

            var mockFr = new Mock<IFeatureReader>();
            mockFr.Setup(r => r.ReadNext()).Returns(true);

            mockFr.Setup(r => r.GetBlob(It.Is<string>(arg => arg == "BLOB"))).Returns(testBlob);
            mockFr.Setup(r => r.GetByte(It.Is<string>(arg => arg == "BYTE"))).Returns(1);
            mockFr.Setup(r => r.GetBoolean(It.Is<string>(arg => arg == "BOOL"))).Returns(false);
            mockFr.Setup(r => r.GetClob(It.Is<string>(arg => arg == "CLOB"))).Returns(testClob);
            mockFr.Setup(r => r.GetDateTime(It.Is<string>(arg => arg == "DATE"))).Returns(new DateTime(2017, 1, 28));
            mockFr.Setup(r => r.GetDouble(It.Is<string>(arg => arg == "DOUBLE"))).Returns(1.0);
            mockFr.Setup(r => r.GetFeatureObject(It.Is<string>(arg => arg == "FEATURE"))).Returns(Mock.Of<IFeatureReader>());
            mockFr.Setup(r => r.GetGeometry(It.Is<string>(arg => arg == "GEOMETRY"))).Returns(mockGeom.Object);
            mockFr.Setup(r => r.GetInt16(It.Is<string>(arg => arg == "INT16"))).Returns(2);
            mockFr.Setup(r => r.GetInt32(It.Is<string>(arg => arg == "INT32"))).Returns(3);
            mockFr.Setup(r => r.GetInt64(It.Is<string>(arg => arg == "INT64"))).Returns(4);
            mockFr.Setup(r => r.GetSingle(It.Is<string>(arg => arg == "SINGLE"))).Returns(1.0f);
            mockFr.Setup(r => r.GetString(It.Is<string>(arg => arg == "STRING"))).Returns("Foo");

            mockFr.Setup(r => r.GetBlob(It.Is<int>(arg => arg == 0))).Returns(testBlob);
            mockFr.Setup(r => r.GetByte(It.Is<int>(arg => arg == 1))).Returns(1);
            mockFr.Setup(r => r.GetBoolean(It.Is<int>(arg => arg == 2))).Returns(false);
            mockFr.Setup(r => r.GetClob(It.Is<int>(arg => arg == 3))).Returns(testClob);
            mockFr.Setup(r => r.GetDateTime(It.Is<int>(arg => arg == 4))).Returns(new DateTime(2017, 1, 28));
            mockFr.Setup(r => r.GetDouble(It.Is<int>(arg => arg == 5))).Returns(1.0);
            mockFr.Setup(r => r.GetFeatureObject(It.Is<int>(arg => arg == 6))).Returns(Mock.Of<IFeatureReader>());
            mockFr.Setup(r => r.GetGeometry(It.Is<int>(arg => arg == 7))).Returns(mockGeom.Object);
            mockFr.Setup(r => r.GetInt16(It.Is<int>(arg => arg == 8))).Returns(2);
            mockFr.Setup(r => r.GetInt32(It.Is<int>(arg => arg == 9))).Returns(3);
            mockFr.Setup(r => r.GetInt64(It.Is<int>(arg => arg == 10))).Returns(4);
            mockFr.Setup(r => r.GetSingle(It.Is<int>(arg => arg == 11))).Returns(1.0f);
            mockFr.Setup(r => r.GetString(It.Is<int>(arg => arg == 12))).Returns("Foo");

            using (var lr = new LimitingFeatureReader(mockFr.Object, limit))
            {
                int iterated = 0;
                while (lr.ReadNext())
                {
                    Assert.NotNull(lr.GetBlob(0));
                    Assert.NotNull(lr.GetBlob("BLOB"));
                    Assert.Equal(1, lr.GetByte(1));
                    Assert.Equal(1, lr.GetByte("BYTE"));
                    Assert.Equal(false, lr.GetBoolean(2));
                    Assert.Equal(false, lr.GetBoolean("BOOL"));
                    Assert.NotNull(lr.GetClob(3));
                    Assert.NotNull(lr.GetClob("CLOB"));
                    Assert.Equal(new DateTime(2017, 1, 28), lr.GetDateTime(4));
                    Assert.Equal(new DateTime(2017, 1, 28), lr.GetDateTime("DATE"));
                    Assert.Equal(1.0, lr.GetDouble(5));
                    Assert.Equal(1.0, lr.GetDouble("DOUBLE"));
                    Assert.NotNull(lr.GetFeatureObject(6));
                    Assert.NotNull(lr.GetFeatureObject("FEATURE"));
                    Assert.Equal("POINT (0 0)", lr.GetGeometry(7).AsText());
                    Assert.Equal("POINT (0 0)", lr.GetGeometry("GEOMETRY").AsText());
                    Assert.Equal(2, lr.GetInt16(8));
                    Assert.Equal(2, lr.GetInt16("INT16"));
                    Assert.Equal(3, lr.GetInt32(9));
                    Assert.Equal(3, lr.GetInt32("INT32"));
                    Assert.Equal(4, lr.GetInt64(10));
                    Assert.Equal(4, lr.GetInt64("INT64"));
                    Assert.Equal(1.0f, lr.GetSingle(11));
                    Assert.Equal(1.0f, lr.GetSingle("SINGLE"));
                    Assert.Equal("Foo", lr.GetString(12));
                    Assert.Equal("Foo", lr.GetString("STRING"));
                    iterated++;
                }
                lr.Close();
                Assert.Equal(limit, iterated);
            }
        }

        [Fact]
        public void TestXmlFeatureNullValues()
        {
            //Simulate post-#708 SELECTFEATURES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(TestData.SelectFeatureSample);
            var reader = CreateXmlFeatureReader(new MemoryStream(bytes));

            Assert.Equal(3, reader.FieldCount);

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.False(reader.IsNull(1));
            Assert.False(reader.IsNull(2));

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.False(reader.IsNull(1));
            Assert.True(reader.IsNull(2));

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.True(reader.IsNull(1));
            Assert.True(reader.IsNull(2));

            Assert.False(reader.ReadNext()); //end of stream

            //Test the IEnumerable approach
            reader = CreateXmlFeatureReader(new MemoryStream(bytes));

            int i = 0;
            foreach (var feat in reader)
            {
                switch (i)
                {
                    case 0:
                        Assert.False(feat.IsNull(0));
                        Assert.False(feat.IsNull(1));
                        Assert.False(feat.IsNull(2));
                        break;

                    case 1:
                        Assert.False(feat.IsNull(0));
                        Assert.False(feat.IsNull(1));
                        Assert.True(feat.IsNull(2));
                        break;

                    case 2:
                        Assert.False(feat.IsNull(0));
                        Assert.True(feat.IsNull(1));
                        Assert.True(feat.IsNull(2));
                        break;
                }
                i++;
            }
        }

        [Fact]
        public void TestXmlFeatureJoinValues()
        {
            var bytes = Encoding.UTF8.GetBytes(TestData.FeatureJoinSelectSample);
            var reader = CreateXmlFeatureReader(new MemoryStream(bytes));

            Assert.Equal(40, reader.FieldCount);

            int count = 0;
            while (reader.ReadNext())
            {
                count++;
            }
            Assert.Equal(63, count);
        }

        [Fact]
        public void TestXmlAggregateNullValues()
        {
            //Simulate post-#708 SELECTAGGREGATES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(TestData.SelectAggregatesSample);
            var reader = CreateXmlDataReader(new MemoryStream(bytes));

            Assert.Equal(3, reader.FieldCount);

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.False(reader.IsNull(1));
            Assert.False(reader.IsNull(2));

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.False(reader.IsNull(1));
            Assert.True(reader.IsNull(2));

            reader.ReadNext();

            Assert.False(reader.IsNull(0));
            Assert.True(reader.IsNull(1));
            Assert.True(reader.IsNull(2));

            Assert.False(reader.ReadNext()); //end of stream

            //Test the IEnumerable approach
            reader = CreateXmlDataReader(new MemoryStream(bytes));

            int i = 0;
            while (reader.ReadNext())
            {
                switch (i)
                {
                    case 0:
                        Assert.False(reader.IsNull(0));
                        Assert.False(reader.IsNull(1));
                        Assert.False(reader.IsNull(2));
                        break;

                    case 1:
                        Assert.False(reader.IsNull(0));
                        Assert.False(reader.IsNull(1));
                        Assert.True(reader.IsNull(2));
                        break;

                    case 2:
                        Assert.False(reader.IsNull(0));
                        Assert.True(reader.IsNull(1));
                        Assert.True(reader.IsNull(2));
                        break;
                }
                i++;
            }
        }
    }
}
