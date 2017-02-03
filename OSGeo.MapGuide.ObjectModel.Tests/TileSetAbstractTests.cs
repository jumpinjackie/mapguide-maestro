#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    // This test suite exercises known implementations of ITileSetAbstract to ensure consistent behaviour

    [TestFixture]
    public class TileSetAbstractTests
    {
        [Test]
        public void FiniteScalesTest()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(3, 0, 0), "Test");
            var tsd = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));

            mdf.InitBaseMap();
            tsd.SetDefaultProviderParameters(300, 300, "", new double[] { });

            ITileSetAbstract tsMapDef = mdf.BaseMap;
            ITileSetAbstract tsTileSet = tsd;

            Assert.False(tsTileSet.SupportsCustomFiniteDisplayScalesUnconditionally);
            Assert.True(tsTileSet.SupportsCustomFiniteDisplayScales);

            tsMapDef.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0 });
            tsTileSet.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0 });

            Assert.AreEqual(3, tsMapDef.ScaleCount);
            Assert.AreEqual(3, tsTileSet.ScaleCount);

            tsMapDef.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0, 8.0, 16.0 });
            tsTileSet.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0, 8.0, 16.0 });

            Assert.AreEqual(5, tsMapDef.ScaleCount);
            Assert.AreEqual(5, tsTileSet.ScaleCount);

            Assert.AreEqual(1.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(1.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(1));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(2));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(2));

            Assert.AreEqual(8.0, tsMapDef.GetScaleAt(3));
            Assert.AreEqual(8.0, tsTileSet.GetScaleAt(3));

            Assert.AreEqual(16.0, tsMapDef.GetScaleAt(4));
            Assert.AreEqual(16.0, tsTileSet.GetScaleAt(4));

            tsMapDef.RemoveScaleAt(3);
            tsTileSet.RemoveScaleAt(3);

            Assert.AreEqual(1.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(1.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(1));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(2));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(2));

            Assert.AreEqual(16.0, tsMapDef.GetScaleAt(3));
            Assert.AreEqual(16.0, tsTileSet.GetScaleAt(3));

            tsMapDef.RemoveScaleAt(0);
            tsTileSet.RemoveScaleAt(0);

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(1));

            Assert.AreEqual(16.0, tsMapDef.GetScaleAt(2));
            Assert.AreEqual(16.0, tsTileSet.GetScaleAt(2));

            tsMapDef.RemoveScaleAt(2);
            tsTileSet.RemoveScaleAt(2);

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(1));

            Assert.AreEqual(2.0, tsMapDef.GetMinScale());
            Assert.AreEqual(2.0, tsTileSet.GetMinScale());

            tsMapDef.RemoveFiniteDisplayScale(1.2);
            tsTileSet.RemoveFiniteDisplayScale(1.2);

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(1));

            Assert.Throws<ArgumentOutOfRangeException>(() => tsMapDef.RemoveScaleAt(6));
            Assert.Throws<ArgumentOutOfRangeException>(() => tsTileSet.RemoveScaleAt(6));

            Assert.AreEqual(2.0, tsMapDef.GetScaleAt(0));
            Assert.AreEqual(2.0, tsTileSet.GetScaleAt(0));

            Assert.AreEqual(4.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(4.0, tsTileSet.GetScaleAt(1));

            tsMapDef.AddFiniteDisplayScale(3.0);
            tsTileSet.AddFiniteDisplayScale(3.0);

            Assert.AreEqual(3, tsMapDef.ScaleCount);
            Assert.AreEqual(3, tsTileSet.ScaleCount);

            Assert.AreEqual(3.0, tsMapDef.GetScaleAt(1));
            Assert.AreEqual(3.0, tsTileSet.GetScaleAt(1));

            tsMapDef.RemoveAllScales();
            tsTileSet.RemoveAllScales();

            Assert.AreEqual(0, tsMapDef.ScaleCount);
            Assert.AreEqual(0, tsTileSet.ScaleCount);
        }

        [Test]
        public void TestUnsupportedScaleOperations()
        {
            var tsd = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));

            tsd.SetXYZProviderParameters();

            ITileSetAbstract tsTileSet = tsd;

            Assert.False(tsTileSet.SupportsCustomFiniteDisplayScalesUnconditionally);
            Assert.False(tsTileSet.SupportsCustomFiniteDisplayScales);

            Assert.Throws<InvalidOperationException>(() => tsTileSet.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0 }));
            Assert.Throws<InvalidOperationException>(() => { var x = tsTileSet.ScaleCount; });
            Assert.Throws<InvalidOperationException>(() => tsTileSet.SetFiniteDisplayScales(new double[] { 1.0, 2.0, 4.0, 8.0, 16.0 }));
            Assert.Throws<InvalidOperationException>(() => tsTileSet.GetScaleAt(0));
            Assert.Throws<InvalidOperationException>(() => tsTileSet.RemoveScaleAt(3));
            Assert.Throws<InvalidOperationException>(() => tsTileSet.RemoveFiniteDisplayScale(1.2));
            Assert.Throws<InvalidOperationException>(tsTileSet.RemoveAllScales);
            Assert.Throws<InvalidOperationException>(() => tsTileSet.AddFiniteDisplayScale(1234.0));
        }

        [Test]
        public void GroupManagementTest()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(3, 0, 0), "Test");
            var tsd = ObjectFactory.CreateTileSetDefinition(new Version(3, 0, 0));

            mdf.InitBaseMap();
            tsd.SetDefaultProviderParameters(300, 300, "", new double[] { });

            ITileSetAbstract tsMapDef = mdf.BaseMap;
            ITileSetAbstract tsTileSet = tsd;

            //Default tile set has a base group (to satisfy minimum content model), so clear that first
            tsTileSet.RemoveBaseLayerGroup(tsTileSet.BaseMapLayerGroups.First());

            Assert.AreEqual(0, tsMapDef.GroupCount);
            Assert.AreEqual(0, tsTileSet.GroupCount);

            var grpMapDef = tsMapDef.AddBaseLayerGroup("Test");
            var grpTileSet = tsTileSet.AddBaseLayerGroup("Test");
            Assert.NotNull(grpMapDef);
            Assert.NotNull(grpTileSet);
            Assert.AreEqual("Test", grpMapDef.Name);
            Assert.AreEqual("Test", grpTileSet.Name);
            Assert.AreEqual(1, tsMapDef.GroupCount);
            Assert.AreEqual(1, tsTileSet.GroupCount);

            Assert.NotNull(grpMapDef.AddLayer("layer1", "Library://test/layer1.LayerDefinition"));
            Assert.NotNull(grpMapDef.AddLayer("layer2", "Library://test/layer2.LayerDefinition"));
            Assert.NotNull(grpTileSet.AddLayer("layer1", "Library://test/layer1.LayerDefinition"));
            Assert.NotNull(grpTileSet.AddLayer("layer2", "Library://test/layer2.LayerDefinition"));

            grpMapDef = tsMapDef.AddBaseLayerGroup("Test2");
            grpTileSet = tsTileSet.AddBaseLayerGroup("Test2");
            Assert.NotNull(grpMapDef);
            Assert.NotNull(grpTileSet);
            Assert.AreEqual("Test2", grpMapDef.Name);
            Assert.AreEqual("Test2", grpTileSet.Name);
            Assert.AreEqual(2, tsMapDef.GroupCount);
            Assert.AreEqual(2, tsTileSet.GroupCount);

            Assert.NotNull(grpMapDef.AddLayer("layer3", "Library://test/layer3.LayerDefinition"));
            Assert.NotNull(grpTileSet.AddLayer("layer3", "Library://test/layer3.LayerDefinition"));

            Assert.AreEqual(3, tsMapDef.GetBaseLayerCount());
            Assert.AreEqual(3, tsTileSet.GetBaseLayerCount());

            Assert.NotNull(tsMapDef.GetBaseLayerByName("layer1"));
            Assert.NotNull(tsMapDef.GetBaseLayerByName("layer2"));
            Assert.NotNull(tsMapDef.GetBaseLayerByName("layer3"));

            Assert.NotNull(tsTileSet.GetBaseLayerByName("layer1"));
            Assert.NotNull(tsTileSet.GetBaseLayerByName("layer2"));
            Assert.NotNull(tsTileSet.GetBaseLayerByName("layer3"));

            tsMapDef.RemoveBaseLayerGroup(tsMapDef.GetGroup("Test"));
            tsTileSet.RemoveBaseLayerGroup(tsTileSet.GetGroup("Test"));

            Assert.Null(tsMapDef.GetBaseLayerByName("layer1"));
            Assert.Null(tsMapDef.GetBaseLayerByName("layer2"));
            Assert.NotNull(tsMapDef.GetBaseLayerByName("layer3"));

            Assert.Null(tsTileSet.GetBaseLayerByName("layer1"));
            Assert.Null(tsTileSet.GetBaseLayerByName("layer2"));
            Assert.NotNull(tsTileSet.GetBaseLayerByName("layer3"));

            Assert.AreEqual(1, tsMapDef.GetBaseLayerCount());
            Assert.AreEqual(1, tsTileSet.GetBaseLayerCount());
        }
    }
}
