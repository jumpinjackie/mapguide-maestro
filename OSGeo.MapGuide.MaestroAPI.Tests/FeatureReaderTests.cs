#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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
using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Http;
using OSGeo.MapGuide.MaestroAPI.Schema;
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
            Assert.AreEqual(fcount, iterations);
        }

        [Test]
        public void TestLimitingFeatureReader()
        {
            int limit = 5;

            var mockFr = new Mock<IFeatureReader>();
            mockFr.Setup(r => r.ReadNext()).Returns(true);

            using (var lr = new LimitingFeatureReader(mockFr.Object, limit))
            {
                int iterated = 0;
                while (lr.ReadNext())
                {
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
