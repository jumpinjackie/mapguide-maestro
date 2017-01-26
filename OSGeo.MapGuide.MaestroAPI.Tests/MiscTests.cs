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
using OSGeo.MapGuide.MaestroAPI.Internal;
using System;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class MiscTests
    {
        [Test]
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

        [Test]
        public void TestParseXyzmWkt()
        {
            var wkt1 = "POINT XYZM (1 2 3 4)";

            var reader = new FixedWKTReader();
            var geom = reader.Read(wkt1);
            Assert.NotNull(geom);
        }

        [Test]
        public void TestConnectionString()
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Foo"] = "sdfjkg";
            builder["Bar"] = "skgjuksdf";
            builder["Snafu"] = "asjdgjh;sdgj"; //Note the ; in the value
            builder["Whatever"] = "asjd=gjh;sdgj"; //Note the ; and = in the value

            var values = ConnectionProviderRegistry.ParseConnectionString(builder.ToString());
            Assert.AreEqual(values.Count, 4);

            Assert.AreEqual(builder["Foo"].ToString(), values["Foo"]);
            Assert.AreEqual(builder["Bar"].ToString(), values["Bar"]);
            Assert.AreEqual(builder["Snafu"].ToString(), values["Snafu"]);
            Assert.AreEqual(builder["Whatever"].ToString(), values["Whatever"]);
        }

        [Test]
        public void TestArgParser()
        {
            string[] args = { "-foo", "-bar:snafu", "-whatever:" };

            var parser = new ArgumentParser(args);
            Assert.IsFalse(parser.IsDefined("snafu"));
            Assert.IsTrue(parser.IsDefined("foo"));
            Assert.IsTrue(parser.IsDefined("bar"));
            Assert.IsTrue(parser.IsDefined("whatever"));
            Assert.AreEqual(string.Empty, parser.GetValue("whatever"));
            Assert.AreEqual(parser.GetValue("bar"), "snafu");
        }

        [Test]
        public void TestSiteVersions()
        {
            foreach (KnownSiteVersions ver in Enum.GetValues(typeof(KnownSiteVersions)))
            {
                var version = SiteVersions.GetVersion(ver);
                Assert.NotNull(version);
            }
        }
    }
}
