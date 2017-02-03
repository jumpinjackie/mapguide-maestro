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
using GeoAPI.Geometries;
using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Http;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class FeatureReaderTests
    {
        [Test]
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
                Assert.AreEqual(1, reader.FieldCount);
                Assert.AreEqual(iterations, reader.GetInt32("ID"));
                Assert.AreEqual(iterations, reader.GetInt32(0));
                Assert.AreEqual(PropertyValueType.Int32, reader.GetPropertyType(0));
                Assert.AreEqual(PropertyValueType.Int32, reader.GetPropertyType("ID"));
                iterations++;
            }
            reader.Close();
            Assert.AreEqual(fcount, iterations);
        }

        [Test]
        public void TestLimitingFeatureReader()
        {
            int limit = 5;

            var testBlob = new byte[] { 1, 2, 3 };
            var testClob = new char[] { 'a', 'b', 'c' };

            var mockGeom = new Mock<IGeometry>();
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
                    Assert.AreEqual(1, lr.GetByte(1));
                    Assert.AreEqual(1, lr.GetByte("BYTE"));
                    Assert.AreEqual(false, lr.GetBoolean(2));
                    Assert.AreEqual(false, lr.GetBoolean("BOOL"));
                    Assert.NotNull(lr.GetClob(3));
                    Assert.NotNull(lr.GetClob("CLOB"));
                    Assert.AreEqual(new DateTime(2017, 1, 28), lr.GetDateTime(4));
                    Assert.AreEqual(new DateTime(2017, 1, 28), lr.GetDateTime("DATE"));
                    Assert.AreEqual(1.0, lr.GetDouble(5));
                    Assert.AreEqual(1.0, lr.GetDouble("DOUBLE"));
                    Assert.IsNotNull(lr.GetFeatureObject(6));
                    Assert.IsNotNull(lr.GetFeatureObject("FEATURE"));
                    Assert.AreEqual("POINT (0 0)", lr.GetGeometry(7).AsText());
                    Assert.AreEqual("POINT (0 0)", lr.GetGeometry("GEOMETRY").AsText());
                    Assert.AreEqual(2, lr.GetInt16(8));
                    Assert.AreEqual(2, lr.GetInt16("INT16"));
                    Assert.AreEqual(3, lr.GetInt32(9));
                    Assert.AreEqual(3, lr.GetInt32("INT32"));
                    Assert.AreEqual(4, lr.GetInt64(10));
                    Assert.AreEqual(4, lr.GetInt64("INT64"));
                    Assert.AreEqual(1.0f, lr.GetSingle(11));
                    Assert.AreEqual(1.0f, lr.GetSingle("SINGLE"));
                    Assert.AreEqual("Foo", lr.GetString(12));
                    Assert.AreEqual("Foo", lr.GetString("STRING"));
                    iterated++;
                }
                lr.Close();
                Assert.AreEqual(limit, iterated);
            }
        }

        [Test]
        public void TestXmlFeatureNullValues()
        {
            //Simulate post-#708 SELECTFEATURES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));

            Assert.AreEqual(3, reader.FieldCount);

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsFalse(reader.IsNull(1));
            Assert.IsFalse(reader.IsNull(2));

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsFalse(reader.IsNull(1));
            Assert.IsTrue(reader.IsNull(2));

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsTrue(reader.IsNull(1));
            Assert.IsTrue(reader.IsNull(2));

            Assert.IsFalse(reader.ReadNext()); //end of stream

            //Test the IEnumerable approach
            reader = new XmlFeatureReader(new MemoryStream(bytes));

            int i = 0;
            foreach (var feat in reader)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsFalse(feat.IsNull(0));
                        Assert.IsFalse(feat.IsNull(1));
                        Assert.IsFalse(feat.IsNull(2));
                        break;

                    case 1:
                        Assert.IsFalse(feat.IsNull(0));
                        Assert.IsFalse(feat.IsNull(1));
                        Assert.IsTrue(feat.IsNull(2));
                        break;

                    case 2:
                        Assert.IsFalse(feat.IsNull(0));
                        Assert.IsTrue(feat.IsNull(1));
                        Assert.IsTrue(feat.IsNull(2));
                        break;
                }
                i++;
            }
        }

        [Test]
        public void TestXmlFeatureJoinValues()
        {
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.FeatureJoinSelectSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));

            Assert.AreEqual(40, reader.FieldCount);

            int count = 0;
            while (reader.ReadNext())
            {
                count++;
            }
            Assert.AreEqual(63, count);
        }

        [Test]
        public void TestXmlAggregateNullValues()
        {
            //Simulate post-#708 SELECTAGGREGATES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectAggregatesSample);
            var reader = new XmlDataReader(new MemoryStream(bytes));

            Assert.AreEqual(3, reader.FieldCount);

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsFalse(reader.IsNull(1));
            Assert.IsFalse(reader.IsNull(2));

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsFalse(reader.IsNull(1));
            Assert.IsTrue(reader.IsNull(2));

            reader.ReadNext();

            Assert.IsFalse(reader.IsNull(0));
            Assert.IsTrue(reader.IsNull(1));
            Assert.IsTrue(reader.IsNull(2));

            Assert.IsFalse(reader.ReadNext()); //end of stream

            //Test the IEnumerable approach
            reader = new XmlDataReader(new MemoryStream(bytes));

            int i = 0;
            while (reader.ReadNext())
            {
                switch (i)
                {
                    case 0:
                        Assert.IsFalse(reader.IsNull(0));
                        Assert.IsFalse(reader.IsNull(1));
                        Assert.IsFalse(reader.IsNull(2));
                        break;

                    case 1:
                        Assert.IsFalse(reader.IsNull(0));
                        Assert.IsFalse(reader.IsNull(1));
                        Assert.IsTrue(reader.IsNull(2));
                        break;

                    case 2:
                        Assert.IsFalse(reader.IsNull(0));
                        Assert.IsTrue(reader.IsNull(1));
                        Assert.IsTrue(reader.IsNull(2));
                        break;
                }
                i++;
            }
        }
    }
}
