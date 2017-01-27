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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class UtilityTests
    {
        [Test]
        public void Utility_IsZero()
        {
            Assert.True(Utility.IsZero(0.0));
            Assert.True(Utility.IsZero(0.0f));
            Assert.False(Utility.IsZero(0.1));
            Assert.False(Utility.IsZero(0.1f));
            Assert.False(Utility.IsZero(-0.1));
            Assert.False(Utility.IsZero(-0.1f));
        }

        [Test]
        public void Utility_MakeWktPolygon()
        {
            var wkt = Utility.MakeWktPolygon(1, 2, 3, 4);
            Assert.AreEqual("POLYGON((1 2, 3 2, 3 4, 1 4, 1 2))", wkt);
        }

        [Test]
        public void Utility_MakeWktCircle()
        {
            var wkt = Utility.MakeWktCircle(1, 2, 3);
            Assert.AreEqual("CURVEPOLYGON ((-2 2 (CIRCULARARCSEGMENT (1 -1, 4 2), CIRCULARARCSEGMENT (1 5, -2 2))))", wkt);
        }

        [Test]
        public void Utility_IsDbXmlError()
        {
            Assert.True(Utility.IsDbXmlError(new Exception("MgDbXmlException")));
            Assert.True(Utility.IsDbXmlError(new Exception("MgXmlParserException")));
        }

        [Test]
        public void Utility_HasOriginalXml()
        {
            var ex = new Exception("MgDbXmlException");
            ex.Data[Utility.XML_EXCEPTION_KEY] = "<Foo></Foo>";

            var ex2 = new Exception("MgDbXmlException");
            
            Assert.True(Utility.HasOriginalXml(ex));
            Assert.False(Utility.HasOriginalXml(ex2));
        }

        [Test]
        public void Utility_ToConnectionString()
        {
            var nvc = new NameValueCollection();
            nvc["Foo"] = "Bar";
            nvc["Baz"] = "Snafu";
            Assert.AreEqual("Foo=Bar;Baz=Snafu", Utility.ToConnectionString(nvc));
        }

        [Test]
        public void Utility_StripVersionFromProviderName()
        {
            Assert.AreEqual("OSGeo.SDF", Utility.StripVersionFromProviderName("OSGeo.SDF.4.0"));
            Assert.AreEqual("OSGeo.SDF", Utility.StripVersionFromProviderName("OSGeo.SDF.4"));
            Assert.AreEqual("OSGeo.SDF", Utility.StripVersionFromProviderName("OSGeo.SDF"));
        }

        [Test]
        public void Utility_IsSuccessfulConnectionTestResult()
        {
            Assert.True(Utility.IsSuccessfulConnectionTestResult("No errors"));
            Assert.True(Utility.IsSuccessfulConnectionTestResult("true"));
            Assert.True(Utility.IsSuccessfulConnectionTestResult("TRUE"));
            Assert.False(Utility.IsSuccessfulConnectionTestResult("false"));
            Assert.False(Utility.IsSuccessfulConnectionTestResult(""));
            Assert.False(Utility.IsSuccessfulConnectionTestResult(null));
        }
    }
}
