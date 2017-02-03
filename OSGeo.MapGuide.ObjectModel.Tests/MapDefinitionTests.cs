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

using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    [TestFixture]
    public class MapDefinitionTests
    {
        [Test]
        public void Envelope_Empty()
        {
            IEnvelope env = ObjectFactory.CreateEnvelope(0.0, 0.0, 0.0, 0.0);
            Assert.True(env.IsEmpty());
            env = null;
            Assert.True(env.IsEmpty());
            env = ObjectFactory.CreateEnvelope(-0.00000000001, -0.0000000000000001, 0.0000000000000000001, 0.000000000000001);
            Assert.False(env.IsEmpty());
        }

        [Test]
        public void CreateTest_V30()
        {
            var res = ObjectFactory.CreateMapDefinition(new Version(3, 0, 0), "TestMap");
            Assert.IsInstanceOf<IMapDefinition3>(res);
            IMapDefinition3 mdf = (IMapDefinition3)res;
            Assert.AreEqual("TestMap", res.Name);
            Assert.Null(mdf.BaseMap);
            Assert.AreEqual(TileSourceType.None, mdf.TileSourceType);
            Assert.Null(mdf.TileSetDefinitionID);

            Assert.Throws<ArgumentException>(() => mdf.TileSetDefinitionID = "Library://Test.asgkjdsf");
            mdf.TileSetDefinitionID = "Library://UnitTests/TileSets/Sheboygan.TileSetDefinition";

            Assert.AreEqual("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", mdf.TileSetDefinitionID);
            Assert.AreEqual(TileSourceType.External, mdf.TileSourceType);
            mdf.InitBaseMap();
            Assert.AreEqual(TileSourceType.Inline, mdf.TileSourceType);
            Assert.Null(mdf.TileSetDefinitionID);
        }

        [Test]
        public void DeserializationTest_V30()
        {
            var res = ObjectFactory.DeserializeXml(Properties.Resources.UT_LinkedTileSet);
            Assert.IsInstanceOf<IMapDefinition3>(res);
            IMapDefinition3 mdf = (IMapDefinition3)res;
            Assert.AreEqual("Base Map linked to Tile Set", mdf.Name);
            Assert.AreEqual("PROJCS[\"WGS84.PseudoMercator\",GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722293]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Popular Visualisation Pseudo Mercator\"],PARAMETER[\"false_easting\",0.000],PARAMETER[\"false_northing\",0.000],PARAMETER[\"central_meridian\",0.00000000000000],UNIT[\"Meter\",1.00000000000000]]", mdf.CoordinateSystem);
            Assert.NotNull(mdf.Extents);
            var extent = mdf.Extents;
            Assert.AreEqual(-9773613.7373958, extent.MinX);
            Assert.AreEqual(5417109.9090669, extent.MinY);
            Assert.AreEqual(-9758770.5921973, extent.MaxX);
            Assert.AreEqual(5435129.2308673, extent.MaxY);
            Assert.AreEqual(1, mdf.GetDynamicLayerCount());
            Assert.NotNull(mdf.GetLayerByName("RoadCenterLines"));
            Assert.AreEqual(TileSourceType.External, mdf.TileSourceType);
            Assert.AreEqual("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", mdf.TileSetDefinitionID);
        }

        [Test]
        public void ConvertToTileSetTest()
        {
            var res = ObjectFactory.DeserializeXml(Properties.Resources.OldTiledMap);
            Assert.IsInstanceOf<IMapDefinition>(res);
            var mdf = (IMapDefinition)res;

            var tsd = mdf.ConvertToTileSet(new Version(3, 0, 0));

            Assert.AreEqual(mdf.CoordinateSystem, tsd.GetDefaultCoordinateSystem());
            Assert.AreEqual(mdf.Extents.MinX, tsd.Extents.MinX);
            Assert.AreEqual(mdf.Extents.MinY, tsd.Extents.MinY);
            Assert.AreEqual(mdf.Extents.MaxX, tsd.Extents.MaxX);
            Assert.AreEqual(mdf.Extents.MaxY, tsd.Extents.MaxY);

            Assert.AreEqual(mdf.BaseMap.BaseMapLayerGroups.Count(), tsd.GroupCount);

            var origGroups = mdf.BaseMap.BaseMapLayerGroups.ToArray();
            var tsdGroups = tsd.BaseMapLayerGroups.ToArray();

            Assert.AreEqual(origGroups.Length, tsdGroups.Length);
            for (int i = 0; i < origGroups.Length; i++)
            {
                var oriGroup = origGroups[i];
                var tsdGroup = tsdGroups[i];

                Assert.AreEqual(oriGroup.ExpandInLegend, tsdGroup.ExpandInLegend);
                Assert.AreEqual(oriGroup.LegendLabel, tsdGroup.LegendLabel);
                Assert.AreEqual(oriGroup.Name, tsdGroup.Name);
                Assert.AreEqual(oriGroup.ShowInLegend, tsdGroup.ShowInLegend);
                Assert.AreEqual(oriGroup.Visible, tsdGroup.Visible);

                Assert.AreEqual(oriGroup.BaseMapLayer.Count(), tsdGroup.BaseMapLayer.Count());

                var oriLayers = oriGroup.BaseMapLayer.ToArray();
                var tsdLayers = tsdGroup.BaseMapLayer.ToArray();

                Assert.AreEqual(oriLayers.Length, tsdLayers.Length);

                for (int j = 0; j < oriLayers.Length; j++)
                {
                    var oriLayer = oriLayers[j];
                    var tsdLayer = tsdLayers[j];

                    Assert.AreEqual(oriLayer.ExpandInLegend, tsdLayer.ExpandInLegend);
                    Assert.AreEqual(oriLayer.LegendLabel, tsdLayer.LegendLabel);
                    Assert.AreEqual(oriLayer.Name, tsdLayer.Name);
                    Assert.AreEqual(oriLayer.ResourceId, tsdLayer.ResourceId);
                    Assert.AreEqual(oriLayer.Selectable, tsdLayer.Selectable);
                    Assert.AreEqual(oriLayer.ShowInLegend, tsdLayer.ShowInLegend);
                }
            }
        }
    }
}
