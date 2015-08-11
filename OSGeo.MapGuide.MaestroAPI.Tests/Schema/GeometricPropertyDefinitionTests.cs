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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Schema;
using NUnit.Framework;
namespace OSGeo.MapGuide.MaestroAPI.Schema.Tests
{
    [TestFixture]
    public class GeometricPropertyDefinitionTests
    {
        [Test]
        public void GeometricPropertyDefinitionTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.AreEqual("Foo", prop.Name);
            Assert.AreEqual("Bar", prop.Description);
            Assert.AreEqual(0, prop.SpecificGeometryTypes.Length);
            Assert.IsNullOrEmpty(prop.SpatialContextAssociation);
        }

        [Test]
        public void GeometryTypesToStringTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.AreEqual("Foo", prop.Name);
            Assert.AreEqual("Bar", prop.Description);
            Assert.AreEqual(0, prop.SpecificGeometryTypes.Length);
            Assert.IsNullOrEmpty(prop.SpatialContextAssociation);

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Solid;
            var types = new List<string>(prop.GeometryTypesToString().Split(' '));
            Assert.GreaterOrEqual(types.IndexOf("curve"), 0);
            Assert.GreaterOrEqual(types.IndexOf("solid"), 0);
            Assert.Less(types.IndexOf("point"), 0);
            Assert.Less(types.IndexOf("surface"), 0);
        }

        [Test]
        public void GetIndividualGeometricTypesTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.AreEqual("Foo", prop.Name);
            Assert.AreEqual("Bar", prop.Description);
            Assert.AreEqual(0, prop.SpecificGeometryTypes.Length);
            Assert.IsNullOrEmpty(prop.SpatialContextAssociation);

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Solid;

            var types = new List<FeatureGeometricType>(prop.GetIndividualGeometricTypes());
            Assert.GreaterOrEqual(types.IndexOf(FeatureGeometricType.Curve), 0);
            Assert.GreaterOrEqual(types.IndexOf(FeatureGeometricType.Solid), 0);
            Assert.Less(types.IndexOf(FeatureGeometricType.Point), 0);
            Assert.Less(types.IndexOf(FeatureGeometricType.Surface), 0);
        }

        [Test]
        public void SpecificGeometryTypesTest()
        {
            var prop = new GeometricPropertyDefinition("Foo", "Bar");
            Assert.AreEqual("Foo", prop.Name);
            Assert.AreEqual("Bar", prop.Description);
            Assert.AreEqual(0, prop.SpecificGeometryTypes.Length);
            Assert.IsNullOrEmpty(prop.SpatialContextAssociation);

            prop.GeometricTypes = FeatureGeometricType.Curve | FeatureGeometricType.Point;

            var types = new List<SpecificGeometryType>(prop.SpecificGeometryTypes);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.LineString), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.CurveString), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.MultiCurveString), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.MultiLineString), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.Point), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.MultiPoint), 0);
            
            prop.GeometricTypes = FeatureGeometricType.Point;
            types = new List<SpecificGeometryType>(prop.SpecificGeometryTypes);
            Assert.Less(types.IndexOf(SpecificGeometryType.LineString), 0);
            Assert.Less(types.IndexOf(SpecificGeometryType.CurveString), 0);
            Assert.Less(types.IndexOf(SpecificGeometryType.MultiCurveString), 0);
            Assert.Less(types.IndexOf(SpecificGeometryType.MultiLineString), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.Point), 0);
            Assert.GreaterOrEqual(types.IndexOf(SpecificGeometryType.MultiPoint), 0);
        }
    }
}
