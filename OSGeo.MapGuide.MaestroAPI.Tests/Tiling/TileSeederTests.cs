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

using Moq;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Tiling
{
    public class TileSeederTests
    {
        [Fact]
        public void TileSeeder_WithDefaultWalker()
        {
            var mdfId = "Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition";
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.ResourceID = mdfId;
            mdf.InitBaseMap();
            var group = mdf.BaseMap.AddBaseLayerGroup("Base Layer Group");
            group.Visible = true;
            foreach (double scale in TileTests.SCALE_LIST.Reverse())
            {
                mdf.BaseMap.AddFiniteDisplayScale(scale);
            }
            mdf.SetExtents(-87.764986990962839, 43.691398128787782, -87.695521510899724, 43.797520000480347);

            var walkOptions = new TileWalkOptions(mdf);
            walkOptions.MetersPerUnit = 111319.490793274;
            var walker = new DefaultTileWalker(walkOptions);

            var mockTileSvc = new Mock<ITileService>();
            mockTileSvc
                .Setup(t => t.GetTile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => MemoryStreamPool.GetStream());

            var options = new TileSeederOptions();
            var seeder = new TileSeeder(mockTileSvc.Object, walker, options);
            var stats = seeder.Run();

            Assert.Equal(mdfId, stats.ResourceID);
            Assert.Equal(127472, stats.TilesRendered);

            mockTileSvc.Verify(t => t.GetTile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(stats.TilesRendered));
            /*
            var tiles = walker.GetTileList();
            for (int i = 0; i < tiles.Length; i++)
            {
                var t = tiles[i];
                mockTileSvc.Verify(ts => ts.GetTile(mdfId, t.GroupName, t.Col, t.Row, t.Scale), Times.Once());
            }
            */
        }

        [Fact]
        public void TileSeeder_WithDefaultWalkerAndCustomBounds()
        {
            var mdfId = "Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition";
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.ResourceID = mdfId;
            mdf.InitBaseMap();
            var group = mdf.BaseMap.AddBaseLayerGroup("Base Layer Group");
            group.Visible = true;
            foreach (double scale in TileTests.SCALE_LIST.Reverse())
            {
                mdf.BaseMap.AddFiniteDisplayScale(scale);
            }
            mdf.SetExtents(-87.764986990962839, 43.691398128787782, -87.695521510899724, 43.797520000480347);
            var ovExtents = ObjectFactory.CreateEnvelope(-87.7278601614039, 43.7443959276596, -87.7135994943579, 43.7592852552018);

            var walkOptions = new TileWalkOptions(mdf);
            walkOptions.MetersPerUnit = 111319.490793274;
            walkOptions.OverrideExtents = ovExtents;
            var walker = new DefaultTileWalker(walkOptions);

            var mockTileSvc = new Mock<ITileService>();
            mockTileSvc
                .Setup(t => t.GetTile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => MemoryStreamPool.GetStream());

            var options = new TileSeederOptions();
            var seeder = new TileSeeder(mockTileSvc.Object, walker, options);
            var stats = seeder.Run();

            Assert.Equal(mdfId, stats.ResourceID);

            //I don't know the exact number here, but it should be less than the original and
            //greater than the bogus amount of 10 tiles
            Assert.True(stats.TilesRendered < 127472);
            Assert.True(stats.TilesRendered > 10);

            mockTileSvc.Verify(t => t.GetTile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(stats.TilesRendered));
        }
    }
}
