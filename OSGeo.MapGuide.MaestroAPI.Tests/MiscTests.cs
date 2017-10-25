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
using OSGeo.MapGuide.MaestroAPI.CrossConnection;
using OSGeo.MapGuide.MaestroAPI.Internal;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class MiscTests
    {
        [Fact]
        public void TestParse3dWkt()
        {
            var wkt1 = "LINESTRING XYZ (218941.59990888927 173858.42946731683 0, 218931.73921854934 173868.56834443274 0)";
            var wkt2 = "POINT XYZ (1 2 3)";

            var reader = new FixedWKTReader();
            var geom1 = reader.Read(wkt1);
            Assert.NotNull(geom1);
            var geom2 = reader.Read(wkt2);
            Assert.NotNull(geom2);
        }

        [Fact]
        public void TestParseXyzmWkt()
        {
            var wkt1 = "POINT XYZM (1 2 3 4)";

            var reader = new FixedWKTReader();
            var geom = reader.Read(wkt1);
            Assert.NotNull(geom);
        }

        [Fact]
        public void TestConnectionString()
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Foo"] = "sdfjkg";
            builder["Bar"] = "skgjuksdf";
            builder["Snafu"] = "asjdgjh;sdgj"; //Note the ; in the value
            builder["Whatever"] = "asjd=gjh;sdgj"; //Note the ; and = in the value

            var values = ConnectionProviderRegistry.ParseConnectionString(builder.ToString());
            Assert.Equal(4, values.Count);

            Assert.Equal(builder["Foo"].ToString(), values["Foo"]);
            Assert.Equal(builder["Bar"].ToString(), values["Bar"]);
            Assert.Equal(builder["Snafu"].ToString(), values["Snafu"]);
            Assert.Equal(builder["Whatever"].ToString(), values["Whatever"]);
        }

        [Fact]
        public void TestArgParser()
        {
            string[] args = { "-foo", "-bar:snafu", "-whatever:" };

            var parser = new ArgumentParser(args);
            Assert.False(parser.IsDefined("snafu"));
            Assert.True(parser.IsDefined("foo"));
            Assert.True(parser.IsDefined("bar"));
            Assert.True(parser.IsDefined("whatever"));
            Assert.Equal(string.Empty, parser.GetValue("whatever"));
            Assert.Equal("snafu", parser.GetValue("bar"));

            var nvc = parser.GetAllArgumentsWithValues();
            Assert.Null(nvc["foo"]);
            Assert.Equal("snafu", nvc["bar"]);
            Assert.Null(nvc["whatever"]);
        }

        [Fact]
        public void TestSiteVersions()
        {
            foreach (KnownSiteVersions ver in Enum.GetValues(typeof(KnownSiteVersions)))
            {
                var version = SiteVersions.GetVersion(ver);
                Assert.NotNull(version);
            }
        }

        [Fact]
        public void TestPropertyInfo()
        {
            var pi = new PropertyInfo("Foo", typeof(string));
            Assert.Equal("Foo", pi.Name);
            Assert.Equal(typeof(string), pi.Type);
        }

        [Fact]
        public void TestEventArgs()
        {
            var args = new ResourceEventArgs("Library://Test.MapDefinition");
            Assert.Equal("Library://Test.MapDefinition", args.ResourceID);

            var args2 = new RequestEventArgs("Foo");
            Assert.Equal("Foo", args2.Data);

            var items = new[] { new LengthyOperationCallbackArgs.LengthyOperationItem("Foo") };
            var args3 = new LengthyOperationCallbackArgs(items);
            Assert.False(args3.Cancel);
            Assert.Equal(0, args3.Index);
            Assert.Equal(items, args3.Items);
            Assert.Equal("Foo", args3.Items[args3.Index].Itempath);
            Assert.Equal(LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.None, args3.Items[args3.Index].Status);

            var cmd = new Mock<ICommand>();
            var args4 = new CommandEventArgs(cmd.Object);
            Assert.NotNull(args4.Command);
        }

        [Fact]
        public void TestRebaseOptions()
        {
            Assert.Throws<ArgumentException>(() => { new RebaseOptions("Library://Foo.FeatureSource", "Library://Bar/"); });
            Assert.Throws<ArgumentException>(() => { new RebaseOptions("Library://Foo/", "Library://Bar.FeatureSource"); });

            var opts = new RebaseOptions("Library://Foo/", "Library://Bar/");
            Assert.Equal("Library://Foo/", opts.SourceFolder);
            Assert.Equal("Library://Bar/", opts.TargetFolder);
        }
    }
}
