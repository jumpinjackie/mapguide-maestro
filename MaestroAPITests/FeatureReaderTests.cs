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
using NUnit.Framework;
using System.Xml;
using System.IO;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Http;

namespace MaestroAPITests
{
    [TestFixture]
    public class FeatureReaderTests
    {
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

        /*
        [Test]
        public void TestXmlFeatureNullValues()
        {
            //Simulate post-#708 SELECTFEATURES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureSetReader(new MemoryStream(bytes));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsFalse(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsTrue(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            Assert.IsFalse(reader.Read()); //end of stream

            //Test the IEnumerable approach
            reader = new XmlFeatureSetReader(new MemoryStream(bytes));

            int i = 0;
            foreach (var feat in reader)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsFalse(feat.IsDBNull(2));
                        break;
                    case 1:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                    case 2:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsTrue(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                }
                i++;
            }
        }*/

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

        /*
        [Test]
        public void TestXmlAggregateNullValues()
        {
            //Simulate post-#708 SELECTAGGREGATES and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectAggregatesSample);
            var reader = new XmlAggregateSetReader(new MemoryStream(bytes));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsFalse(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsTrue(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            Assert.IsFalse(reader.Read()); //end of stream

            //Test the IEnumerable approach
            reader = new XmlAggregateSetReader(new MemoryStream(bytes));

            int i = 0;
            foreach (var feat in reader)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsFalse(feat.IsDBNull(2));
                        break;
                    case 1:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                    case 2:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsTrue(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                }
                i++;
            }
        }
         */

        public void TestXmlSqlNullValues()
        {
            //Simulate post-#708 EXECUTESQL and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectSqlSample);
            var reader = new XmlSqlResultReader(new MemoryStream(bytes));

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
            reader = new XmlSqlResultReader(new MemoryStream(bytes));

            int i = 0;
            while(reader.ReadNext())
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
        
        /*
        [Test]
        public void TestXmlSqlNullValues()
        {
            //Simulate post-#708 EXECUTESQL and verify reader properly handles null values in response
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectSqlSample);
            var reader = new XmlSqlResultReader(new MemoryStream(bytes));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsFalse(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsFalse(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            reader.Read();

            Assert.IsFalse(reader.IsDBNull(0));
            Assert.IsTrue(reader.IsDBNull(1));
            Assert.IsTrue(reader.IsDBNull(2));

            Assert.IsFalse(reader.Read()); //end of stream

            //Test the IEnumerable approach
            reader = new XmlSqlResultReader(new MemoryStream(bytes));

            int i = 0;
            foreach (var feat in reader)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsFalse(feat.IsDBNull(2));
                        break;
                    case 1:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsFalse(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                    case 2:
                        Assert.IsFalse(feat.IsDBNull(0));
                        Assert.IsTrue(feat.IsDBNull(1));
                        Assert.IsTrue(feat.IsDBNull(2));
                        break;
                }
                i++;
            }
        }*/
    }
}
