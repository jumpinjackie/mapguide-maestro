#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

using System;
using System.Collections.Specialized;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class UtilityTests
    {
        [Theory]
        [InlineData(0.0, true)]
        [InlineData(0.0f, true)]
        [InlineData(0.1, false)]
        [InlineData(0.1f, false)]
        [InlineData(-0.1, false)]
        [InlineData(-0.1f, false)]
        public void Utility_IsZero(double val, bool expected)
        {
            Assert.Equal(expected, Utility.IsZero(val));
        }

        [Fact]
        public void Utility_MakeWktPolygon()
        {
            var wkt = Utility.MakeWktPolygon(1, 2, 3, 4);
            Assert.Equal("POLYGON((1 2, 3 2, 3 4, 1 4, 1 2))", wkt);
        }

        [Fact]
        public void Utility_MakeWktCircle()
        {
            var wkt = Utility.MakeWktCircle(1, 2, 3);
            Assert.Equal("CURVEPOLYGON ((-2 2 (CIRCULARARCSEGMENT (1 -1, 4 2), CIRCULARARCSEGMENT (1 5, -2 2))))", wkt);
        }

        [Fact]
        public void Utility_IsDbXmlError()
        {
            Assert.True(Utility.IsDbXmlError(new Exception("MgDbXmlException")));
            Assert.True(Utility.IsDbXmlError(new Exception("MgXmlParserException")));
        }

        [Fact]
        public void Utility_HasOriginalXml()
        {
            var ex = new Exception("MgDbXmlException");
            ex.Data[Utility.XML_EXCEPTION_KEY] = "<Foo></Foo>";

            var ex2 = new Exception("MgDbXmlException");
            
            Assert.True(Utility.HasOriginalXml(ex));
            Assert.False(Utility.HasOriginalXml(ex2));
        }

        [Fact]
        public void Utility_ToConnectionString()
        {
            var nvc = new NameValueCollection();
            nvc["Foo"] = "Bar";
            nvc["Baz"] = "Snafu";
            Assert.Equal("Foo=Bar;Baz=Snafu", Utility.ToConnectionString(nvc));
        }

        [Theory]
        [InlineData("OSGeo.SDF.4.0", "OSGeo.SDF")]
        [InlineData("OSGeo.SDF.4", "OSGeo.SDF")]
        [InlineData("OSGeo.SDF", "OSGeo.SDF")]
        [InlineData("InvalidProviderName", "InvalidProviderName")]
        public void Utility_StripVersionFromProviderName(string qualified, string expected)
        {
            Assert.Equal(expected, Utility.StripVersionFromProviderName(qualified));
        }

        [Fact]
        public void Utility_IsSuccessfulConnectionTestResult()
        {
            Assert.True(Utility.IsSuccessfulConnectionTestResult("No errors"));
            Assert.True(Utility.IsSuccessfulConnectionTestResult("true"));
            Assert.True(Utility.IsSuccessfulConnectionTestResult("TRUE"));
            Assert.False(Utility.IsSuccessfulConnectionTestResult("false"));
            Assert.False(Utility.IsSuccessfulConnectionTestResult(""));
            Assert.False(Utility.IsSuccessfulConnectionTestResult(null));
        }

        [Fact]
        public void Utility_ParseQueryString()
        {
            var param = Utility.ParseQueryString("a=b&c=d");
            Assert.Equal(2, param.Count);
            Assert.Equal("b", param["a"]);
            Assert.Equal("d", param["c"]);

            param = Utility.ParseQueryString("a=b&c=d&");
            Assert.Equal(2, param.Count);
            Assert.Equal("b", param["a"]);
            Assert.Equal("d", param["c"]);

            param = Utility.ParseQueryString("a=&c=d&");
            Assert.Equal(2, param.Count);
            Assert.Equal("", param["a"]);
            Assert.Equal("d", param["c"]);

            param = Utility.ParseQueryString("a=b &c=d&");
            Assert.Equal(2, param.Count);
            Assert.Equal("b ", param["a"]);
            Assert.Equal("d", param["c"]);
        }

        [Theory]
        [InlineData("FF00FF", false)]
        [InlineData("FF00FF12", false)]
        [InlineData("123", true)]
        [InlineData("123ADEF", true)]
        public void Utility_ParseHTMLColor(string color, bool expectThrow)
        {
            try
            {
                var c = Utility.ParseHTMLColor(color);
                if (expectThrow)
                    Assert.True(false, "This was supposed to throw");
            }
            catch (Exception ex)
            {
                if (!expectThrow)
                    Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("FF00FF", false)]
        [InlineData("FF00FF12", false)]
        [InlineData("123", true)]
        [InlineData("123ADEF", true)]
        public void Utility_ParseHTMLColorARGB(string color, bool expectThrow)
        {
            try
            {
                var c = Utility.ParseHTMLColorARGB(color);
                if (expectThrow)
                    Assert.True(false, "This was supposed to throw");
            }
            catch (Exception ex)
            {
                if (!expectThrow)
                    Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("FF00FF", false)]
        [InlineData("FF00FF12", false)]
        [InlineData("123", true)]
        [InlineData("123ADEF", true)]
        public void Utility_ParseHTMLColorRGBA(string color, bool expectThrow)
        {
            try
            {
                var c = Utility.ParseHTMLColorRGBA(color);
                if (expectThrow)
                    Assert.True(false, "This was supposed to throw");
            }
            catch (Exception ex)
            {
                if (!expectThrow)
                    Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Utility_SerializeDigit()
        {
            Assert.Equal("1.1", Utility.SerializeDigit(1.1));
            Assert.Equal("1.1", Utility.SerializeDigit(1.1f));
        }
    }
}
