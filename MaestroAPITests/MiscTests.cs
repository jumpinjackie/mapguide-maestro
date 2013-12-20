#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Internal;

namespace MaestroAPITests
{
    [TestFixture]
    public class MiscTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreMiscTests)
                Assert.Ignore("Skipping MiscTests because TestControl.IgnoreMiscTests = true");
        }

        [Test]
        public void TestParse3dWkt()
        {
            var wkt1 = "LINESTRING XYZ (218941.59990888927 173858.42946731683 0, 218931.73921854934 173868.56834443274 0)";
            var wkt2 = "POINT XYZ (1 2 3)";

            var reader = new FixedWKTReader();
            reader.Read(wkt1);
            reader.Read(wkt2);
        }

        [Test]
        public void TestParseXyzmWkt()
        {
            var wkt1 = "POINT XYZM (1 2 3 4)";

            var reader = new FixedWKTReader();
            reader.Read(wkt1);
        }
    }
}
