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
using System;
using System.Collections.Generic;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Schema.Tests
{
    public class GeometricPropertyDefinitionTests
    {
        [Fact]
        public void GeometricPropertyDefinitionTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(0, prop.SpecificGeometryTypes.Length);
            Assert.True(String.IsNullOrEmpty(prop.SpatialContextAssociation));
        }

        [Fact]
        public void GeometryTypesToStringTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(0, prop.SpecificGeometryTypes.Length);
            Assert.True(String.IsNullOrEmpty(prop.SpatialContextAssociation));

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Solid;
            var types = new List<string>(prop.GeometryTypesToString().Split(' '));
            Assert.True(types.IndexOf("curve") >= 0);
            Assert.True(types.IndexOf("solid") >= 0);
            Assert.True(types.IndexOf("point") < 0);
            Assert.True(types.IndexOf("surface") < 0);
        }

        [Fact]
        public void GetIndividualGeometricTypesTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(0, prop.SpecificGeometryTypes.Length);
            Assert.True(String.IsNullOrEmpty(prop.SpatialContextAssociation));

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Solid;

            var types = new List<FeatureGeometricType>(prop.GetIndividualGeometricTypes());
            Assert.True(types.IndexOf(FeatureGeometricType.Curve) >= 0);
            Assert.True(types.IndexOf(FeatureGeometricType.Solid) >= 0);
            Assert.True(types.IndexOf(FeatureGeometricType.Point) < 0);
            Assert.True(types.IndexOf(FeatureGeometricType.Surface) < 0);
        }

        [Fact]
        public void SpecificGeometryTypesTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(0, prop.SpecificGeometryTypes.Length);
            Assert.True(String.IsNullOrEmpty(prop.SpatialContextAssociation));

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Point;

            var types = new List<SpecificGeometryType>(prop.SpecificGeometryTypes);
            Assert.True(types.IndexOf(SpecificGeometryType.LineString) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.CurveString) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiCurveString) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiLineString) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.Point) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiPoint) >= 0);
            
            prop.GeometricTypes = FeatureGeometricType.Point;
            types = new List<SpecificGeometryType>(prop.SpecificGeometryTypes);
            Assert.True(types.IndexOf(SpecificGeometryType.LineString) < 0);
            Assert.True(types.IndexOf(SpecificGeometryType.CurveString) < 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiCurveString) < 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiLineString) < 0);
            Assert.True(types.IndexOf(SpecificGeometryType.Point) >= 0);
            Assert.True(types.IndexOf(SpecificGeometryType.MultiPoint) >= 0);
        }
    }
}
