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
    public class DefaultWalkerTests
    {
        [Fact]
        public void DefaultWalker_TotalCalculation_ShouldMatchMgCookerOriginal()
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

            var tiles = walker.GetTileList();
            Assert.Equal(mdfId, walker.ResourceID);
            Assert.Equal(127472, tiles.Length);
        }

        [Fact]
        public void DefaultWalker_WithCustomBounds_TotalCalculation_ShouldMatchMgCookerOriginal()
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
            var walker = new DefaultTileWalker(walkOptions);

            var tiles = walker.GetTileList();
            Assert.Equal(mdfId, walker.ResourceID);
            Assert.Equal(127472, tiles.Length);

            walkOptions.OverrideExtents = ovExtents;
            walker = new DefaultTileWalker(walkOptions);
            tiles = walker.GetTileList();

            //I don't know the exact number here, but it should be less than the original and
            //greater than the bogus amount of 10 tiles
            Assert.True(tiles.Length < 127472);
            Assert.True(tiles.Length > 10);
        }
    }
}
