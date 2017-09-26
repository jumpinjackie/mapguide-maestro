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
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class GdalConfigurationDocumentTests
    {
        [Fact]
        public void GdalDocument_AddLocationDeDupes()
        {
            var doc = new GdalConfigurationDocument();
            var loc = new GdalRasterLocationItem { Location = "C:\\temp\\location_1" };
            doc.AddLocation(loc);
            Assert.Equal(1, doc.RasterLocations.Length);
            var loc2 = new GdalRasterLocationItem { Location = "C:\\temp\\location_1" };
            doc.AddLocation(loc2);
            Assert.Equal(1, doc.RasterLocations.Length);
            var loc3 = new GdalRasterLocationItem { Location = "C:\\temp\\location_2" };
            doc.AddLocation(loc3);
            Assert.Equal(2, doc.RasterLocations.Length);
        }

        [Fact]
        public void GdalDocument_Remove()
        {
            var doc = new GdalConfigurationDocument();
            var loc = new GdalRasterLocationItem { Location = "C:\\temp\\location_1" };
            Assert.False(doc.RemoveLocation(loc));
            doc.AddLocation(loc);
            var loc2 = new GdalRasterLocationItem { Location = "C:\\temp\\location_1" };
            Assert.True(doc.RemoveLocation(loc2));
        }

        [Fact]
        public void GdalDocument_CalculateExtents()
        {
            var doc = new GdalConfigurationDocument();
            Assert.Null(doc.CalculateExtent());
            var loc = new GdalRasterLocationItem { Location = "C:\\temp\\location_1" };
            loc.AddItem(new GdalRasterItem { FileName = "1_1.tif", MinX = 1, MinY = 1, MaxX = 2, MaxY = 2 });
            doc.AddLocation(loc);
            var ext = doc.CalculateExtent();
            Assert.Equal(1, ext.MinX);
            Assert.Equal(1, ext.MinY);
            Assert.Equal(2, ext.MaxX);
            Assert.Equal(2, ext.MaxY);
            var loc2 = new GdalRasterLocationItem { Location = "C:\\temp\\location_2" };
            loc2.AddItem(new GdalRasterItem { FileName = "2_1.tif", MinX = -1, MinY = -1, MaxX = 2, MaxY = 2 });
            doc.AddLocation(loc2);
            ext = doc.CalculateExtent();
            Assert.Equal(-1, ext.MinX);
            Assert.Equal(-1, ext.MinY);
            Assert.Equal(2, ext.MaxX);
            Assert.Equal(2, ext.MaxY);
            loc2.AddItem(new GdalRasterItem { FileName = "1_2.tif", MinX = 2, MinY = 1, MaxX = 3, MaxY = 3 });
            ext = doc.CalculateExtent();
            Assert.Equal(-1, ext.MinX);
            Assert.Equal(-1, ext.MinY);
            Assert.Equal(3, ext.MaxX);
            Assert.Equal(3, ext.MaxY);
        }
    }
}
