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

using OSGeo.MapGuide.MaestroAPI.Tests;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Specialized;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class FeatureSourceTests
    {
        [Fact]
        public void FeatureSourceDeserializationWithFullContentModel()
        {
            IResource res = ObjectFactory.DeserializeXml(Utils.ReadAllText($"Resources{System.IO.Path.DirectorySeparatorChar}FeatureSource-1.0.0.txt"));
            Assert.NotNull(res);
            Assert.Equal("FeatureSource", res.ResourceType);
            Assert.Equal(res.ResourceVersion, new Version(1, 0, 0));
            IFeatureSource fs = res as IFeatureSource;
            Assert.NotNull(fs);
        }

        [Fact]
        public void TestFeatureSourceFileParameters()
        {
            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            Assert.True(fs.ConnectionString.Length == 0);

            var connParams = new NameValueCollection();
            connParams["File"] = "%MG_DATA_FILE_PATH%Foo.sdf";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.True(fs.UsesEmbeddedDataFiles);
            Assert.False(fs.UsesAliasedDataFiles);
            Assert.Equal("Foo.sdf", fs.GetEmbeddedDataName());
            Assert.Throws<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Throws<InvalidOperationException>(() => fs.GetAliasName());

            connParams.Clear();
            connParams["File"] = "%MG_DATA_FILE_PATH%Bar.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.True(fs.UsesEmbeddedDataFiles);
            Assert.False(fs.UsesAliasedDataFiles);
            Assert.Equal("Bar.sdf", fs.GetEmbeddedDataName());
            Assert.Throws<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Throws<InvalidOperationException>(() => fs.GetAliasName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SHP", connParams);

            Assert.True(fs.UsesAliasedDataFiles);
            Assert.False(fs.UsesEmbeddedDataFiles);
            Assert.Equal("foobar", fs.GetAliasName());
            Assert.True(string.IsNullOrEmpty(fs.GetAliasedFileName()));
            Assert.Throws<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.True(fs.UsesAliasedDataFiles);
            Assert.False(fs.UsesEmbeddedDataFiles);
            Assert.Equal("foobar", fs.GetAliasName());
            Assert.Equal("Test.sdf", fs.GetAliasedFileName());
            Assert.Throws<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", connParams);

            Assert.True(fs.UsesAliasedDataFiles);
            Assert.False(fs.UsesEmbeddedDataFiles);
            Assert.Equal("foobar", fs.GetAliasName());
            Assert.Equal("Test.sdf", fs.GetAliasedFileName());
            Assert.Throws<InvalidOperationException>(() => fs.GetEmbeddedDataName());

            connParams.Clear();
            connParams["Service"] = "(local)\\SQLEXPRESS";
            connParams["DataStore"] = "TEST";

            fs = ObjectFactory.CreateFeatureSource("OSGeo.SQLServerSpatial", connParams);

            Assert.False(fs.UsesEmbeddedDataFiles);
            Assert.False(fs.UsesAliasedDataFiles);

            Assert.Throws<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Throws<InvalidOperationException>(() => fs.GetAliasName());
            Assert.Throws<InvalidOperationException>(() => fs.GetEmbeddedDataName());
        }
    }
}