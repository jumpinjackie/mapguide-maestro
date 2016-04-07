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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    [TestFixture]
    public class TileSetDefinitionTests
    {
        [Test]
        public void CreateTest()
        {
            var res = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));
            Assert.IsInstanceOf<ITileSetDefinition>(res);
            ITileSetDefinition tsd = (ITileSetDefinition)res;
            Assert.AreEqual("Default", tsd.TileStoreParameters.TileProvider);
            tsd.SetXYZProviderParameters();
            Assert.AreEqual("XYZ", tsd.TileStoreParameters.TileProvider);
            var p = tsd.GetParameter("TileFormat");
            Assert.NotNull(p);
            Assert.AreEqual("PNG", p.Value);
            Assert.AreEqual("PNG", tsd.GetTileFormat());
            p = tsd.GetParameter("TilePath");
            Assert.NotNull(p);
            Assert.AreEqual("%MG_TILE_CACHE_PATH%", p.Value);
            Assert.AreEqual("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());

            tsd.SetDefaultProviderParameters(256, 256, "coordsys", new double[] { 100.5, 200.5, 300.5 });
            p = tsd.GetParameter("TileWidth");
            Assert.NotNull(p);
            Assert.AreEqual("256", p.Value);
            Assert.AreEqual(256, tsd.GetDefaultTileWidth());
            p = tsd.GetParameter("TileHeight");
            Assert.NotNull(p);
            Assert.AreEqual("256", p.Value);
            Assert.AreEqual(256, tsd.GetDefaultTileHeight());
            p = tsd.GetParameter("CoordinateSystem");
            Assert.NotNull(p);
            Assert.AreEqual("coordsys", p.Value);
            Assert.AreEqual("coordsys", tsd.GetDefaultCoordinateSystem());
            p = tsd.GetParameter("FiniteScaleList");
            Assert.NotNull(p);
            Assert.AreEqual("300.5,200.5,100.5", p.Value);
            var value = tsd.GetDefaultFiniteScaleList();
            Assert.AreEqual(3, value.Length);
            Assert.Contains(100.5, value);
            Assert.Contains(200.5, value);
            Assert.Contains(300.5, value);
            p = tsd.GetParameter("TileFormat");
            Assert.NotNull(p);
            Assert.AreEqual("PNG", p.Value);
            Assert.AreEqual("PNG", tsd.GetTileFormat());
            p = tsd.GetParameter("TilePath");
            Assert.NotNull(p);
            Assert.AreEqual("%MG_TILE_CACHE_PATH%", p.Value);
            Assert.AreEqual("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());
        }

        [Test]
        public void DeserializationTest()
        {
            var res = ObjectFactory.DeserializeXml(Properties.Resources.UT_BaseMap);
            Assert.IsInstanceOf<ITileSetDefinition>(res);
            ITileSetDefinition tsd = (ITileSetDefinition)res;
            Assert.AreEqual("%MG_TILE_CACHE_PATH%", tsd.GetTilePath());
            Assert.AreEqual(256, tsd.GetDefaultTileWidth());
            Assert.AreEqual(256, tsd.GetDefaultTileHeight());
            Assert.AreEqual("PNG", tsd.GetTileFormat());
            
            var values = tsd.GetDefaultFiniteScaleList();
            Assert.AreEqual(10, values.Length);
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
            Assert.AreEqual(-87.79786601383196, ext.MinX);
            Assert.AreEqual(-87.66452777186925, ext.MaxX);
            Assert.AreEqual(43.6868578621819, ext.MinY);
            Assert.AreEqual(43.8037962206133, ext.MaxY);

            Assert.AreEqual(1, tsd.BaseMapLayerGroups.Count());
            var grp = tsd.BaseMapLayerGroups.First();
            Assert.AreEqual("BaseLayers", grp.Name);
            Assert.True(grp.Visible);
            Assert.True(grp.ShowInLegend);
            Assert.True(grp.ExpandInLegend);
            Assert.AreEqual("Base Layers", grp.LegendLabel);
            Assert.AreEqual(2, grp.BaseMapLayer.Count());
        }
    }
}
