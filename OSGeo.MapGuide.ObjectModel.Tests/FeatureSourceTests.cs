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

using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModel.Tests
{
    [TestFixture]
    public class FeatureSourceTests
    {
        [Test]
        public void FeatureSourceDeserializationWithFullContentModel()
        {
            IResource res = ObjectFactory.DeserializeXml(Properties.Resources.FeatureSource_1_0_0);
            Assert.NotNull(res);
            Assert.AreEqual(res.ResourceType, "FeatureSource");
            Assert.AreEqual(res.ResourceVersion, new Version(1, 0, 0));
            IFeatureSource fs = res as IFeatureSource;
            Assert.NotNull(fs);
        }

        [Test]
        public void TestFeatureSourceFileParameters()
        {
            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            Assert.IsTrue(fs.ConnectionString.Length == 0);

            var connParams = new NameValueCollection();
            connParams["File"] = "%MG_DATA_FILE_PATH%Foo.sdf";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);
            Assert.AreEqual(fs.GetEmbeddedDataName(), "Foo.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());

            connParams.Clear();
            connParams["File"] = "%MG_DATA_FILE_PATH%Bar.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);
            Assert.AreEqual(fs.GetEmbeddedDataName(), "Bar.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SHP", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.IsEmpty(fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.AreEqual(fs.GetAliasedFileName(), "Test.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.AreEqual(fs.GetAliasedFileName(), "Test.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["Service"] = "(local)\\SQLEXPRESS";
            connParams["DataStore"] = "TEST";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SQLServerSpatial", connParams);

            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);

            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());
        }
    }
}