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
using Moq;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class ResourceTests
    {
        [Fact]
        public void TestCredentialWriter()
        {
            var conn = new Mock<IServerConnection>();
            var mockRs = new Mock<IResourceService>();
            conn.Setup(c => c.ResourceService).Returns(mockRs.Object);

            var fs = new Mock<IFeatureSource>();
            Assert.Throws<ArgumentException>(() => fs.Object.SetEncryptedCredentials(conn.Object, "Foo", "bar"));
            var fs2 = new Mock<IFeatureSource>();
            fs2.Setup(f => f.ResourceID).Returns("Library://Test.FeatureSource");
            fs2.Object.SetEncryptedCredentials(conn.Object, "Foo", "bar");

            mockRs.Verify(r => r.SetResourceData("Library://Test.FeatureSource", StringConstants.MgUserCredentialsResourceData, ResourceDataType.String, It.IsAny<Stream>()), Times.Once);
        }

        [Fact]
        public void TestDeserializeEmbeddedFlexLayout()
        {
            var flex1 = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 3, 0));
            Assert.NotNull(flex1);
            var flex2 = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 3, 0));
            Assert.NotNull(flex2);
        }

        [Fact]
        public void TestWebLayout()
        {
            var conn = new Mock<IServerConnection>();

            var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");
            Assert.NotNull(wl.CommandSet);
            Assert.NotNull(wl.ContextMenu);
            Assert.NotNull(wl.InformationPane);
            Assert.NotNull(wl.Map);
            Assert.NotNull(wl.StatusBar);
            Assert.NotNull(wl.TaskPane);
            Assert.NotNull(wl.ToolBar);
            Assert.NotNull(wl.ZoomControl);

            Assert.True(wl.CommandSet.CommandCount > 0);
            Assert.True(wl.ContextMenu.ItemCount > 0);
            Assert.True(wl.ToolBar.ItemCount > 0);

            Assert.Equal(wl.Map.ResourceId, "Library://Test.MapDefinition");

            //Verify all built-in commands are available
            Array cmdNames = Enum.GetValues(typeof(BuiltInCommandType));
            foreach (var cmdName in cmdNames)
            {
                bool found = false;
                foreach (var cmd in wl.CommandSet.Commands)
                {
                    if (cmd.Name == cmdName.ToString())
                    {
                        found = true;
                    }
                }
                //Trace.TraceInformation("Found command (" + cmdName.ToString() + "): " + found);
                Assert.True(found);
            }
        }

        [Fact]
        public void TestLayerDefinitionConversions()
        {
            var conn = new Mock<IServerConnection>();
            var conv = new ResourceObjectConverter();
            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.ResourceID = "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition";
            ldf.SubLayer.ResourceId = "Library://Samples/Sheboygan/Data/Parcels.FeatureSource";
            ((IVectorLayerDefinition)ldf.SubLayer).FeatureName = "SHP_Schema:Parcels";
            ((IVectorLayerDefinition)ldf.SubLayer).Geometry = "Geometry";

            Assert.Equal("1.0.0", ldf.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-1.0.0.xsd", ldf.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-1.0.0.xsd", ldf.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), ldf.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LayerDef_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf1 = (ILayerDefinition)conv.Convert(ldf, new Version(1, 1, 0));

            Assert.Equal("1.1.0", ldf1.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-1.1.0.xsd", ldf1.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-1.1.0.xsd", ldf1.ValidatingSchema);
            Assert.Equal(new Version(1, 1, 0), ldf1.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LayerDef_110.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf1))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf2 = (ILayerDefinition)conv.Convert(ldf1, new Version(1, 2, 0));

            Assert.Equal("1.2.0", ldf2.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-1.2.0.xsd", ldf2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-1.2.0.xsd", ldf2.ValidatingSchema);
            Assert.Equal(new Version(1, 2, 0), ldf2.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LayerDef_120.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf3 = (ILayerDefinition)conv.Convert(ldf2, new Version(1, 3, 0));

            Assert.Equal("1.3.0", ldf3.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-1.3.0.xsd", ldf3.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-1.3.0.xsd", ldf3.ValidatingSchema);
            Assert.Equal(new Version(1, 3, 0), ldf3.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LayerDef_130.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf3))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf4 = (ILayerDefinition)conv.Convert(ldf2, new Version(2, 3, 0));

            Assert.Equal("2.3.0", ldf4.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-2.3.0.xsd", ldf4.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-2.3.0.xsd", ldf4.ValidatingSchema);
            Assert.Equal(new Version(2, 3, 0), ldf4.ResourceVersion);
            Assert.True(ldf4.SubLayer is ISubLayerDefinition2);

            using (var fs = Utils.OpenTempWrite("LayerDef_230.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf4))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var vl4 = ldf4.SubLayer as IVectorLayerDefinition;
            vl4.Url = "http://www.google.com";

            var ldf5 = (ILayerDefinition)conv.Convert(ldf4, new Version(2, 4, 0));

            Assert.Equal("2.4.0", ldf5.GetResourceTypeDescriptor().Version);
            Assert.Equal("LayerDefinition-2.4.0.xsd", ldf5.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LayerDefinition-2.4.0.xsd", ldf5.ValidatingSchema);
            Assert.Equal(new Version(2, 4, 0), ldf5.ResourceVersion);
            Assert.True(ldf5.SubLayer is ISubLayerDefinition2);

            var vl5 = ldf5.SubLayer as IVectorLayerDefinition;
            Assert.Equal("http://www.google.com", vl5.Url);

            using (var fs = Utils.OpenTempWrite("LayerDef_240.xml"))
            {
                using (var src = ObjectFactory.Serialize(ldf5))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        private static void SetupMapDefinitionForTest(IMapDefinition mdf)
        {
            mdf.AddGroup("Group1");
            mdf.AddLayer("Group1", "Parcels", "Library://UnitTests/Layers/Parcels.LayerDefinition");
            mdf.AddLayer("Group1", "Rail", "Library://UnitTests/Layers/Rail.LayerDefinition");
            mdf.AddGroup("Group2");
        }

        [Fact]
        public void TestMapDefinitionLayerInsert()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionLayerInsert");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();

            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(-1, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(layerCount + 1, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, "", ""); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, null, ""); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, "", null); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, null, null); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.FeatureSource"); });
            Assert.Throws<ArgumentException>(() => { mdf.InsertLayer(0, null, "Hydro", "Garbage"); });

            IMapLayer layer = mdf.InsertLayer(0, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.Equal(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));

            layerCount = mdf.GetDynamicLayerCount();
            IMapLayer layer1 = mdf.InsertLayer(layerCount, null, "Hydro2", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.Equal(layerCount, mdf.GetIndex(layer1));
            Assert.True(layer1 == mdf.GetLayerByName("Hydro2"));
        }

        [Fact]
        public void TestMapDefinitionLayerAdd()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionLayerAdd");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();

            Assert.Throws<ArgumentException>(() => { mdf.AddLayer("IDontExist", "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, "", ""); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, null, ""); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, "", null); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, null, null); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.FeatureSource"); });
            Assert.Throws<ArgumentException>(() => { mdf.AddLayer(null, "Hydro", "Garbage"); });

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.Equal(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.Equal(layerCount + 1, mdf.GetDynamicLayerCount());
        }

        [Fact]
        public void TestMapDefinitionLayerRemove()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionLayerRemove");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.Equal(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.Equal(layerCount + 1, mdf.GetDynamicLayerCount());

            mdf.RemoveLayer(layer);
            Assert.True(mdf.GetIndex(layer) < 0);
            Assert.Equal(layerCount, mdf.GetDynamicLayerCount());
        }

        [Fact]
        public void TestMapDefinitionLayerReordering()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionLayerReordering");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();

            Assert.Throws<ArgumentNullException>(() => { mdf.MoveDown(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.MoveUp(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.SetTopDrawOrder(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.SetBottomDrawOrder(null); });

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.Equal(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.Equal(layerCount + 1, mdf.GetDynamicLayerCount());

            int value = mdf.MoveUp(layer);
            Assert.Equal(0, value); //Already at top
            value = mdf.MoveDown(layer);
            Assert.Equal(1, value);
            mdf.SetBottomDrawOrder(layer);
            value = mdf.GetIndex(layer);
            Assert.Equal(mdf.GetDynamicLayerCount() - 1, value);
            value = mdf.MoveDown(layer);
            Assert.Equal(mdf.GetDynamicLayerCount() - 1, value); //Already at bottom
            value = mdf.MoveUp(layer);
            Assert.Equal(mdf.GetDynamicLayerCount() - 2, value);
            mdf.SetTopDrawOrder(layer);
            value = mdf.GetIndex(layer);
            Assert.Equal(0, value);
        }

        [Fact]
        public void TestMapDefinitionGroupAdd()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionGroupAdd");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();
            int groupCount = mdf.GetGroupCount();

            Assert.Throws<ArgumentException>(() => { mdf.AddGroup(null); });
            Assert.Throws<ArgumentException>(() => { mdf.AddGroup(""); });
            Assert.Throws<ArgumentException>(() => { mdf.AddGroup("Group1"); });

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.Equal(groupCount + 1, mdf.GetGroupCount());
            Assert.NotNull(mdf.GetGroupByName("Test"));
            Assert.True(group == mdf.GetGroupByName("Test"));
        }

        [Fact]
        public void TestMapDefinitionGroupRemove()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionGroupRemove");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetDynamicLayerCount();
            int groupCount = mdf.GetGroupCount();

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.Equal(groupCount + 1, mdf.GetGroupCount());
            Assert.NotNull(mdf.GetGroupByName("Test"));
            Assert.True(group == mdf.GetGroupByName("Test"));

            mdf.RemoveGroup(group);
            Assert.Equal(groupCount, mdf.GetGroupCount());
            Assert.Null(mdf.GetGroupByName("Test"));
        }

        [Fact]
        public void TestMapDefinitionGroupReordering()
        {
            var conn = new Mock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMapDefinitionGroupReordering");
            SetupMapDefinitionForTest(mdf);
            int groupCount = mdf.GetGroupCount();

            Assert.Throws<ArgumentNullException>(() => { mdf.MoveDown(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.MoveUp(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.SetTopDrawOrder(null); });
            Assert.Throws<ArgumentNullException>(() => { mdf.SetBottomDrawOrder(null); });

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.Equal(groupCount, mdf.GetIndex(group));
            Assert.True(group == mdf.GetGroupByName("Test"));
            Assert.Equal(groupCount + 1, mdf.GetGroupCount());

            int value = mdf.MoveUpGroup(group);
            Assert.Equal(groupCount - 1, value); //Already at top
            value = mdf.MoveDownGroup(group);
            Assert.Equal(groupCount, value);
        }

        [Fact]
        public void TestMapDefinitionConversions()
        {
            var conn = new Mock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test Map");
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            Assert.Equal("1.0.0", mdf.GetResourceTypeDescriptor().Version);
            Assert.Equal("MapDefinition-1.0.0.xsd", mdf.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("MapDefinition-1.0.0.xsd", mdf.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), mdf.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("MapDef_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(mdf))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var mdf2 = (IMapDefinition)conv.Convert(mdf, new Version(2, 3, 0));

            Assert.Equal("2.3.0", mdf2.GetResourceTypeDescriptor().Version);
            Assert.Equal("MapDefinition-2.3.0.xsd", mdf2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("MapDefinition-2.3.0.xsd", mdf2.ValidatingSchema);
            Assert.Equal(new Version(2, 3, 0), mdf2.ResourceVersion);
            Assert.True(mdf2 is IMapDefinition2);

            using (var fs = Utils.OpenTempWrite("MapDef_230.xml"))
            {
                using (var src = ObjectFactory.Serialize(mdf2))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Fact]
        public void TestLoadProcedureConversions()
        {
            var conn = new Mock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var lproc = ObjectFactory.CreateLoadProcedure(LoadType.Sdf);
            lproc.ResourceID = "Library://Samples/Sheboygan/Load/Load.LoadProcedure";

            Assert.Equal("1.0.0", lproc.GetResourceTypeDescriptor().Version);
            Assert.Equal("LoadProcedure-1.0.0.xsd", lproc.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LoadProcedure-1.0.0.xsd", lproc.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), lproc.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LoadProc_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(lproc))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var lproc2 = (ILoadProcedure)conv.Convert(lproc, new Version(1, 1, 0));

            Assert.Equal("1.1.0", lproc2.GetResourceTypeDescriptor().Version);
            Assert.Equal("LoadProcedure-1.1.0.xsd", lproc2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LoadProcedure-1.1.0.xsd", lproc2.ValidatingSchema);
            Assert.Equal(new Version(1, 1, 0), lproc2.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LoadProc_110.xml"))
            {
                using (var src = ObjectFactory.Serialize(lproc2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var lproc3 = (ILoadProcedure)conv.Convert(lproc2, new Version(2, 2, 0));

            Assert.Equal("2.2.0", lproc3.GetResourceTypeDescriptor().Version);
            Assert.Equal("LoadProcedure-2.2.0.xsd", lproc3.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("LoadProcedure-2.2.0.xsd", lproc3.ValidatingSchema);
            Assert.Equal(new Version(2, 2, 0), lproc3.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("LoadProc_220.xml"))
            {
                using (var src = ObjectFactory.Serialize(lproc3))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Fact]
        public void TestWebLayoutConversions()
        {
            var conn = new Mock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");
            wl.ResourceID = "Library://Test.WebLayout";

            Assert.Equal("1.0.0", wl.GetResourceTypeDescriptor().Version);
            Assert.Equal("WebLayout-1.0.0.xsd", wl.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("WebLayout-1.0.0.xsd", wl.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), wl.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("WebLayout_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(wl))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var wl2 = (IWebLayout)conv.Convert(wl, new Version(1, 1, 0));

            Assert.Equal("1.1.0", wl2.GetResourceTypeDescriptor().Version);
            Assert.Equal("WebLayout-1.1.0.xsd", wl2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("WebLayout-1.1.0.xsd", wl2.ValidatingSchema);
            Assert.Equal(new Version(1, 1, 0), wl2.ResourceVersion);
            Assert.True(wl2 is IWebLayout2);

            using (var fs = Utils.OpenTempWrite("WebLayout_110.xml"))
            {
                using (var src = ObjectFactory.Serialize(wl2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var maptip = wl.CommandSet.Commands.OfType<IBasicCommand>().Where(cmd => cmd.Action == BasicCommandActionType.MapTip);
            Assert.False(maptip.Any());

            var wl3 = (IWebLayout)conv.Convert(wl, new Version(2, 4, 0));
            Assert.Equal("2.4.0", wl3.GetResourceTypeDescriptor().Version);
            Assert.Equal("WebLayout-2.4.0.xsd", wl3.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("WebLayout-2.4.0.xsd", wl3.ValidatingSchema);
            Assert.Equal(new Version(2, 4, 0), wl3.ResourceVersion);

            maptip = wl3.CommandSet.Commands.OfType<IBasicCommand>().Where(cmd => cmd.Action == BasicCommandActionType.MapTip);
            Assert.True(maptip.Any());
        }

        [Fact]
        public void TestSymbolDefinitionConversions()
        {
            var conn = new Mock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var ssym = ObjectFactory.CreateSimpleSymbol(new Version(1, 0, 0), "SimpleSymbolTest", "Test simple symbol");
            ssym.ResourceID = "Library://Samples/Sheboygan/Symbols/Test.SymbolDefinition";

            Assert.Equal("1.0.0", ssym.GetResourceTypeDescriptor().Version);
            Assert.Equal("SymbolDefinition-1.0.0.xsd", ssym.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("SymbolDefinition-1.0.0.xsd", ssym.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), ssym.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("SimpleSymDef_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(ssym))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ssym2 = (ISimpleSymbolDefinition)conv.Convert(ssym, new Version(1, 1, 0));

            Assert.Equal("1.1.0", ssym2.GetResourceTypeDescriptor().Version);
            Assert.Equal("SymbolDefinition-1.1.0.xsd", ssym2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("SymbolDefinition-1.1.0.xsd", ssym2.ValidatingSchema);
            Assert.Equal(new Version(1, 1, 0), ssym2.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("SimpleSymDef_110.xml"))
            {
                using (var src = ObjectFactory.Serialize(ssym2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var csym = ObjectFactory.CreateCompoundSymbol(new Version(1, 0, 0), "CompoundSymbolTest", "Test compound symbol");
            csym.ResourceID = "Library://Samples/Sheboygan/Symbols/Compound.SymbolDefinition";

            Assert.Equal("1.0.0", csym.GetResourceTypeDescriptor().Version);
            Assert.Equal("SymbolDefinition-1.0.0.xsd", csym.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("SymbolDefinition-1.0.0.xsd", csym.ValidatingSchema);
            Assert.Equal(new Version(1, 0, 0), csym.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("CompoundSymDef_100.xml"))
            {
                using (var src = ObjectFactory.Serialize(csym))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var csym2 = (ICompoundSymbolDefinition)conv.Convert(csym, new Version(1, 1, 0));

            Assert.Equal("1.1.0", csym2.GetResourceTypeDescriptor().Version);
            Assert.Equal("SymbolDefinition-1.1.0.xsd", csym2.GetResourceTypeDescriptor().XsdName);
            Assert.Equal("SymbolDefinition-1.1.0.xsd", csym2.ValidatingSchema);
            Assert.Equal(new Version(1, 1, 0), csym2.ResourceVersion);

            using (var fs = Utils.OpenTempWrite("CompoundSymDef_110.xml"))
            {
                using (var src = ObjectFactory.Serialize(csym2))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Fact]
        public void TestMapDefinitionNestedGroupDelete()
        {
            var conn = new Mock<IServerConnection>();
            var caps = new Mock<IConnectionCapabilities>();
            
            foreach (var rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                caps.Setup(c => c.GetMaxSupportedResourceVersion(rt.ToString())).Returns(new Version(1, 0, 0));
            }
            conn.Setup(c => c.Capabilities).Returns(caps.Object);
            IMapDefinition mdf = Utility.CreateMapDefinition(conn.Object, "Test");
            /*

             [G] Group1
                [L] Layer1
                [G] Group2
                   [L] Layer2
                   [G] Group3
                      [L] Layer3
             [G] Group4
                [L] Layer4

             */
            var grp1 = mdf.AddGroup("Group1");
            var grp2 = mdf.AddGroup("Group2");
            var grp3 = mdf.AddGroup("Group3");
            var grp4 = mdf.AddGroup("Group4");

            grp3.Group = "Group2";
            grp2.Group = "Group1";

            var lyr1 = mdf.AddLayer("Group1", "Layer1", "Library://Test.LayerDefinition");
            var lyr2 = mdf.AddLayer("Group2", "Layer2", "Library://Test.LayerDefinition");
            var lyr3 = mdf.AddLayer("Group3", "Layer3", "Library://Test.LayerDefinition");
            var lyr4 = mdf.AddLayer("Group4", "Layer4", "Library://Test.LayerDefinition");

            //Delete group 1. Expect the following structure
            /*
             [G] Group4
                [L] Layer4
             */

            mdf.RemoveLayerGroupAndChildLayers("Group1");
            Assert.Equal(1, mdf.GetGroupCount());
            Assert.Equal(1, mdf.GetDynamicLayerCount());
            Assert.Null(mdf.GetLayerByName("Layer1"));
            Assert.Null(mdf.GetLayerByName("Layer2"));
            Assert.Null(mdf.GetLayerByName("Layer3"));
            Assert.NotNull(mdf.GetLayerByName("Layer4"));
            Assert.Null(mdf.GetGroupByName("Group1"));
            Assert.Null(mdf.GetGroupByName("Group2"));
            Assert.Null(mdf.GetGroupByName("Group3"));
            Assert.NotNull(mdf.GetGroupByName("Group4"));
        }
    }
}
