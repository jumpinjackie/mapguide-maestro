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
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class TileSetDefinitionTests
    {
        [Fact]
        public void CreateTest()
        {
            var res = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));
            Assert.IsAssignableFrom<ITileSetDefinition>(res);
            ITileSetDefinition tsd = (ITileSetDefinition)res;
            Assert.Equal("Default", tsd.TileStoreParameters.TileProvider);
            tsd.SetXYZProviderParameters();
            Assert.Equal("XYZ", tsd.TileStoreParameters.TileProvider);
            var p = tsd.GetParameter("TileFormat");
            Assert.NotNull(p);
            Assert.Equal("PNG", p.Value);
            Assert.Equal("PNG", tsd.GetTileFormat());
            p = tsd.GetParameter("TilePath");
            Assert.NotNull(p);
            Assert.Equal("%MG_TILE_CACHE_PATH%", p.Value);
            Assert.Equal("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());

            tsd.SetDefaultProviderParameters(256, 256, "coordsys", new double[] { 100.5, 200.5, 300.5 });
            p = tsd.GetParameter("TileWidth");
            Assert.NotNull(p);
            Assert.Equal("256", p.Value);
            Assert.Equal(256, tsd.GetDefaultTileWidth());
            p = tsd.GetParameter("TileHeight");
            Assert.NotNull(p);
            Assert.Equal("256", p.Value);
            Assert.Equal(256, tsd.GetDefaultTileHeight());
            p = tsd.GetParameter("CoordinateSystem");
            Assert.NotNull(p);
            Assert.Equal("coordsys", p.Value);
            Assert.Equal("coordsys", tsd.GetDefaultCoordinateSystem());
            p = tsd.GetParameter("FiniteScaleList");
            Assert.NotNull(p);
            Assert.Equal("300.5,200.5,100.5", p.Value);
            var value = tsd.GetDefaultFiniteScaleList();
            Assert.Equal(3, value.Length);
            Assert.Contains(100.5, value);
            Assert.Contains(200.5, value);
            Assert.Contains(300.5, value);
            p = tsd.GetParameter("TileFormat");
            Assert.NotNull(p);
            Assert.Equal("PNG", p.Value);
            Assert.Equal("PNG", tsd.GetTileFormat());
            p = tsd.GetParameter("TilePath");
            Assert.NotNull(p);
            Assert.Equal("%MG_TILE_CACHE_PATH%", p.Value);
            Assert.Equal("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());
        }

        [Fact]
        public void DeserializationTest()
        {
            var tsd = ObjectFactory.DeserializeXml(Utils.ReadAllText($"Resources{System.IO.Path.DirectorySeparatorChar}UT_BaseMap.tsd")) as ITileSetDefinition;
            Assert.NotNull(tsd);
            Assert.Equal("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());
            Assert.Equal(256, tsd.GetDefaultTileWidth());
            Assert.Equal(256, tsd.GetDefaultTileHeight());
            Assert.Equal("PNG", tsd.GetTileFormat());
            
            var values = tsd.GetDefaultFiniteScaleList();
            Assert.Equal(10, values.Length);
            Assert.Contains(200000, values);
            Assert.Contains(100000, values);
            Assert.Contains(50000, values);
            Assert.Contains(25000, values);
            Assert.Contains(12500, values);
            Assert.Contains(6250, values);
            Assert.Contains(3125, values);
            Assert.Contains(1562.5, values);
            Assert.Contains(781.25, values);
            Assert.Contains(390.625, values);
            Assert.False(String.IsNullOrEmpty(tsd.GetDefaultCoordinateSystem()));
            
            var ext = tsd.Extents;
            Assert.Equal(-87.79786601383196, ext.MinX);
            Assert.Equal(-87.66452777186925, ext.MaxX);
            Assert.Equal(43.6868578621819, ext.MinY);
            Assert.Equal(43.8037962206133, ext.MaxY);

            Assert.Single(tsd.BaseMapLayerGroups);
            var grp = tsd.BaseMapLayerGroups.First();
            Assert.Equal("BaseLayers", grp.Name);
            Assert.True(grp.Visible);
            Assert.True(grp.ShowInLegend);
            Assert.True(grp.ExpandInLegend);
            Assert.Equal("Base Layers", grp.LegendLabel);
            Assert.Equal(2, grp.BaseMapLayer.Count());
        }
    }
}
