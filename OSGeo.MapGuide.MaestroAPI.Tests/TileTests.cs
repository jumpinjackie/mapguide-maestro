using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tile;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class TileTests
    {
        static readonly double[] SCALE_LIST = { 200000, 100000, 50000, 25000, 12500, 6250, 3125, 1562.5, 781.25, 390.625 };

        [Test]
        public void Test_MapTilingConfiguration_TotalCalculation()
        {
            var mdfId = "Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition";
            var conn = new Mock<IServerConnection>();
            var mockResSvc = new Mock<IResourceService>();

            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.ResourceID = mdfId;
            mdf.InitBaseMap();
            var group = mdf.BaseMap.AddBaseLayerGroup("Base Layer Group");
            group.Visible = true;
            foreach (double scale in SCALE_LIST.Reverse())
            {
                mdf.BaseMap.AddFiniteDisplayScale(scale);
            }
            mdf.SetExtents(-87.764986990962839, 43.691398128787782, -87.695521510899724, 43.797520000480347);

            mockResSvc.Setup(r => r.GetResource(It.Is<string>(arg => arg == mdfId))).Returns(mdf);

            conn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var tileRuns = new TilingRunCollection(conn.Object);
            tileRuns.Config.DPI = 96;
            tileRuns.Config.MetersPerUnit = 111319.490793274;
            tileRuns.Config.RandomizeTileSequence = false;
            tileRuns.Config.RetryCount = 5;
            tileRuns.Config.ThreadCount = 1;
            tileRuns.Config.TileWidth = 300;
            tileRuns.Config.TileHeight = 300;

            var tileConf = new MapTilingConfiguration(tileRuns, mdfId);

            tileConf.SetGroups("Base Layer Group");
            tileConf.SetScalesAndExtend(Enumerable.Range(0, 10).ToArray(), null);

            tileRuns.Maps.Add(tileConf);

            Assert.AreEqual(127472, tileConf.TotalTiles);
        }

        [Test]
        public void Test_MapTilingConfiguration_WithCustomBounds_TotalCalculation()
        {
            var mdfId = "Library://Samples/Sheboygan/MapsTiled/Sheboygan.MapDefinition";
            var conn = new Mock<IServerConnection>();
            var mockResSvc = new Mock<IResourceService>();

            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.ResourceID = mdfId;
            mdf.InitBaseMap();
            var group = mdf.BaseMap.AddBaseLayerGroup("Base Layer Group");
            group.Visible = true;
            foreach (double scale in SCALE_LIST.Reverse())
            {
                mdf.BaseMap.AddFiniteDisplayScale(scale);
            }
            mdf.SetExtents(-87.764986990962839, 43.691398128787782, -87.695521510899724, 43.797520000480347);

            mockResSvc.Setup(r => r.GetResource(It.Is<string>(arg => arg == mdfId))).Returns(mdf);

            conn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var tileRuns = new TilingRunCollection(conn.Object);
            tileRuns.Config.DPI = 96;
            tileRuns.Config.MetersPerUnit = 111319.490793274;
            tileRuns.Config.RandomizeTileSequence = false;
            tileRuns.Config.RetryCount = 5;
            tileRuns.Config.ThreadCount = 1;
            tileRuns.Config.TileWidth = 300;
            tileRuns.Config.TileHeight = 300;

            var tileConf = new MapTilingConfiguration(tileRuns, mdfId);

            var customExtents = ObjectFactory.CreateEnvelope(-87.764986990962839, 43.691398128787782, -87.695521510899724, 43.797520000480347);

            tileConf.SetGroups("Base Layer Group");
            tileConf.SetScalesAndExtend(Enumerable.Range(0, 10).ToArray(), customExtents);

            tileRuns.Maps.Add(tileConf);

            Assert.AreEqual(127472, tileConf.TotalTiles);

            customExtents = ObjectFactory.CreateEnvelope(-87.73, 43.71, -87.71, 43.75);
            tileConf.SetScalesAndExtend(Enumerable.Range(0, 10).ToArray(), customExtents);

            //I don't know the exact number here, but it should be less than the original and
            //greater than the bogus amount of 10 tiles
            Assert.Less(tileConf.TotalTiles, 127472);
            Assert.Greater(tileConf.TotalTiles, 10);
        }
    }
}
