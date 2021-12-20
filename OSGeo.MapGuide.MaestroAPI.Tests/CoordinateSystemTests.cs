#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class CoordinateSystemTests
    {
        [Fact]
        public void Test4326To3857()
        {
            var csCat = new Mock<CoordinateSystemCatalog>();
            csCat.CallBase = true;

            var csSrcWkt = "GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722356]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.01745329251994]]";
            var csDstWkt = "PROJCS[\"WGS84.PseudoMercator\",GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722356]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Popular Visualisation Pseudo Mercator\"],PARAMETER[\"false_easting\",0.000],PARAMETER[\"false_northing\",0.000],PARAMETER[\"central_meridian\",0.00000000000000],UNIT[\"Meter\",1.00000000000000]]";

            var csSrc = csCat.Object.CreateCoordinateSystem(csSrcWkt);
            var csDst = csCat.Object.CreateCoordinateSystem(csDstWkt);
            
            Assert.NotNull(csSrc);
            Assert.NotNull(csDst);

            var xform = csCat.Object.CreateTransform(csSrcWkt, csDstWkt);

            Assert.NotNull(xform);

            double lat = -37.8248948, lng = 144.9558146;
            Assert.True(xform.Transform(lng, lat, out var tx, out var ty));

            Assert.Equal(16136407.468796169, tx, 8);
            Assert.Equal(-4554718.7730180807, ty, 8);

            var invxform = csCat.Object.CreateTransform(csDstWkt, csSrcWkt);
            Assert.True(invxform.Transform(tx, ty, out var nlng, out var nlat));

            Assert.Equal(lat, nlat, 6);
            Assert.Equal(lng, nlng, 6);
        }
    }
}
