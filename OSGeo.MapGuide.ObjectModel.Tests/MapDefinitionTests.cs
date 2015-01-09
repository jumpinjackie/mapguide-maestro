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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
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
            Assert.AreEqual(1, mdf.GetLayerCount());
            Assert.NotNull(mdf.GetLayerByName("RoadCenterLines"));
            Assert.AreEqual(TileSourceType.External, mdf.TileSourceType);
            Assert.AreEqual("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", mdf.TileSetDefinitionID);
        }
    }
}
