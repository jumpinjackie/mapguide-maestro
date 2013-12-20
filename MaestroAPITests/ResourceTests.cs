#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using NMock2;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System.Diagnostics;

namespace MaestroAPITests
{
    [TestFixture]
    public class ResourceTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreResourceTests)
                Assert.Ignore("Skipping ResourceTests because TestControl.IgnoreResourceTests = true");
        }

        private Mockery _mocks;

        [Test]
        public void TestCloning()
        {
            //Generated classes have built in Clone() methods. Verify they check out
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 2, 0, 0)));
            var caps = _mocks.NewMock<IConnectionCapabilities>();
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));
            foreach (var rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Stub.On(caps).Method("GetMaxSupportedResourceVersion").With(rt).Will(Return.Value(new Version(1, 0, 0)));
            }

            var app = ObjectFactory.DeserializeEmbeddedFlexLayout(conn);
            var app2 = app.Clone();
            Assert.AreNotSame(app, app2);

            var fs = new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.FeatureSourceType();
            var fs2 = fs.Clone();
            Assert.AreNotSame(fs, fs2);

            var ld = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector, new Version(1, 0, 0));
            var ld2 = ld.Clone();
            Assert.AreNotSame(ld, ld2);

            var md = ObjectFactory.CreateMapDefinition(conn, "TestMap");
            var md2 = md.Clone();
            Assert.AreNotSame(md, md2);

            var wl = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), "Library://Test.MapDefinition");
            var wl2 = wl.Clone();
            Assert.AreNotSame(wl, wl2);

            var sl = new OSGeo.MapGuide.ObjectModels.SymbolLibrary_1_0_0.SymbolLibraryType();
            var sl2 = sl.Clone();
            Assert.AreNotSame(sl, sl2);

            var ssd = ObjectFactory.CreateSimpleSymbol(conn, new Version(1, 0, 0), "Test", "Test Symbol");
            var ssd2 = ssd.Clone();
            Assert.AreNotSame(ssd, ssd2);

            var csd = ObjectFactory.CreateCompoundSymbol(conn, new Version(1, 0, 0), "Test", "Test Symbol");
            var csd2 = csd.Clone();
            Assert.AreNotSame(csd, csd2);

            var pl = ObjectFactory.CreatePrintLayout(conn);
            var pl2 = pl.Clone();
            Assert.AreNotSame(pl, pl2);
        }

        [Test]
        public void TestValidResourceIdentifiers()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 2, 0, 0)));
            var caps = _mocks.NewMock<IConnectionCapabilities>();
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));
            foreach (var rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Stub.On(caps).Method("GetMaxSupportedResourceVersion").With(rt).Will(Return.Value(new Version(1, 0, 0)));
            }

            //Verify that only valid resource identifiers can be assigned to certain resource types.

            IResource res = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF");
            #region Feature Source
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.FeatureSource";
            }
            catch (InvalidOperationException)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateDrawingSource(conn);
            #region Drawing Source
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.FeatureSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateMapDefinition(conn, "Test Map");
            #region Map Definition
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.MapDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), "Library://Test.MapDefinition");
            #region Web Layout
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.WebLayout";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.DeserializeEmbeddedFlexLayout(conn);
            #region Application Definition
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.ApplicationDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateSimpleSymbol(conn, new Version(1, 0, 0), "Test", "Test Symbol");
            #region Simple Symbol Definition
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateCompoundSymbol(conn, new Version(1, 0, 0), "Test", "Test Symbol");
            #region Compound Symbol Definition
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateLoadProcedure(conn, LoadType.Sdf, null);
            #region Load Procedure
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.LoadProcedure";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreateLoadProcedure(conn, LoadType.Shp, null);
            #region Load Procedure
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.LoadProcedure";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion

            res = ObjectFactory.CreatePrintLayout(conn);
            #region Print Layout
            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException){ }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.PrintLayout";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }
            #endregion
        }

        [Test]
        public void TestWebLayout()
        {
            var conn = _mocks.NewMock<IServerConnection>();

            var wl = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), "Library://Test.MapDefinition");
            Assert.IsNotNull(wl.CommandSet);
            Assert.IsNotNull(wl.ContextMenu);
            Assert.IsNotNull(wl.InformationPane);
            Assert.IsNotNull(wl.Map);
            Assert.IsNotNull(wl.StatusBar);
            Assert.IsNotNull(wl.TaskPane);
            Assert.IsNotNull(wl.ToolBar);
            Assert.IsNotNull(wl.ZoomControl);

            Assert.IsTrue(wl.CommandSet.CommandCount > 0);
            Assert.IsTrue(wl.ContextMenu.ItemCount > 0);
            Assert.IsTrue(wl.ToolBar.ItemCount > 0);

            Assert.AreEqual(wl.Map.ResourceId, "Library://Test.MapDefinition");

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
                Trace.TraceInformation("Found command (" + cmdName.ToString() + "): " + found);
                Assert.IsTrue(found);
            }
        }

        [Test]
        public void TestFeatureSource()
        {
            var conn = _mocks.NewMock<IServerConnection>();

            var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF");
            Assert.IsTrue(fs.ConnectionString.Length == 0);
            
            var connParams = new NameValueCollection();
            connParams["File"] = "%MG_DATA_FILE_PATH%Foo.sdf";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);
            Assert.AreEqual(fs.GetEmbeddedDataName(), "Foo.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());
            
            connParams.Clear();
            connParams["File"] = "%MG_DATA_FILE_PATH%Bar.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);
            Assert.AreEqual(fs.GetEmbeddedDataName(), "Bar.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());
            
            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SHP", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.IsEmpty(fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());
            
            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.AreEqual(fs.GetAliasedFileName(), "Test.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());
            
            connParams.Clear();
            connParams["DefaultFileLocation"] = "%MG_DATA_PATH_ALIAS[foobar]%Test.sdf";
            connParams["ReadOnly"] = "TRUE";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF", connParams);

            Assert.IsTrue(fs.UsesAliasedDataFiles);
            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.AreEqual(fs.GetAliasName(), "foobar");
            Assert.AreEqual(fs.GetAliasedFileName(), "Test.sdf");
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());
            
            connParams.Clear();
            connParams["Service"] = "(local)\\SQLEXPRESS";
            connParams["DataStore"] = "TEST";

            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SQLServerSpatial", connParams);

            Assert.IsFalse(fs.UsesEmbeddedDataFiles);
            Assert.IsFalse(fs.UsesAliasedDataFiles);

            Assert.Catch<InvalidOperationException>(() => fs.GetAliasedFileName());
            Assert.Catch<InvalidOperationException>(() => fs.GetAliasName());
            Assert.Catch<InvalidOperationException>(() => fs.GetEmbeddedDataName());
        }

        [Test]
        public void TestResourceTypeDescriptor()
        {
            var rtd = new ResourceTypeDescriptor(ResourceTypes.ApplicationDefinition, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "ApplicationDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.FeatureSource, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "FeatureSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "LayerDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.1.0");
            Assert.AreEqual(rtd.XsdName, "LayerDefinition-1.1.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LoadProcedure, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "LoadProcedure-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.MapDefinition, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "MapDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.PrintLayout, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "PrintLayout-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "SymbolDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolLibrary, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "SymbolLibrary-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.WebLayout, "1.0.0");
            Assert.AreEqual(rtd.XsdName, "WebLayout-1.0.0.xsd");
        }

        [Test]
        public void TestLayerDefinitionConversions()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var conv = new ResourceObjectConverter();
            var ldf = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector, new Version(1, 0, 0));
            ldf.ResourceID = "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition";
            ldf.SubLayer.ResourceId = "Library://Samples/Sheboygan/Data/Parcels.FeatureSource";
            ((IVectorLayerDefinition)ldf.SubLayer).FeatureName = "SHP_Schema:Parcels";
            ((IVectorLayerDefinition)ldf.SubLayer).Geometry = "Geometry";

            Assert.AreEqual("1.0.0", ldf.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LayerDefinition-1.0.0.xsd", ldf.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LayerDefinition-1.0.0.xsd", ldf.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), ldf.ResourceVersion);
            
            using (var fs = File.OpenWrite("LayerDef_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ldf))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf1 = (ILayerDefinition)conv.Convert(ldf, new Version(1, 1, 0));

            Assert.AreEqual("1.1.0", ldf1.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LayerDefinition-1.1.0.xsd", ldf1.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LayerDefinition-1.1.0.xsd", ldf1.ValidatingSchema);
            Assert.AreEqual(new Version(1, 1, 0), ldf1.ResourceVersion);
            Assert.NotNull(ldf1.CurrentConnection);

            using (var fs = File.OpenWrite("LayerDef_110.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ldf1))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf2 = (ILayerDefinition)conv.Convert(ldf1, new Version(1, 2, 0));

            Assert.AreEqual("1.2.0", ldf2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LayerDefinition-1.2.0.xsd", ldf2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LayerDefinition-1.2.0.xsd", ldf2.ValidatingSchema);
            Assert.AreEqual(new Version(1, 2, 0), ldf2.ResourceVersion);
            Assert.NotNull(ldf2.CurrentConnection);

            using (var fs = File.OpenWrite("LayerDef_120.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ldf2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf3 = (ILayerDefinition)conv.Convert(ldf2, new Version(1, 3, 0));

            Assert.AreEqual("1.3.0", ldf3.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LayerDefinition-1.3.0.xsd", ldf3.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LayerDefinition-1.3.0.xsd", ldf3.ValidatingSchema);
            Assert.AreEqual(new Version(1, 3, 0), ldf3.ResourceVersion);
            Assert.NotNull(ldf3.CurrentConnection);

            using (var fs = File.OpenWrite("LayerDef_130.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ldf3))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ldf4 = (ILayerDefinition)conv.Convert(ldf2, new Version(2, 3, 0));

            Assert.AreEqual("2.3.0", ldf4.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LayerDefinition-2.3.0.xsd", ldf4.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LayerDefinition-2.3.0.xsd", ldf4.ValidatingSchema);
            Assert.AreEqual(new Version(2, 3, 0), ldf4.ResourceVersion);
            Assert.NotNull(ldf4.CurrentConnection);
            Assert.IsTrue(ldf4.SubLayer is ISubLayerDefinition2);

            using (var fs = File.OpenWrite("LayerDef_230.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ldf4))
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

        [Test]
        public void TestMapDefinitionLayerInsert()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionLayerInsert");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();

            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(-1, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(layerCount + 1, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, "", ""); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, null, ""); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, "", null); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, null, null); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.FeatureSource"); });
            Assert.Throws<PreconditionException>(() => { mdf.InsertLayer(0, null, "Hydro", "Garbage"); });

            IMapLayer layer = mdf.InsertLayer(0, null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.AreEqual(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));

            layerCount = mdf.GetLayerCount();
            IMapLayer layer1 = mdf.InsertLayer(layerCount, null, "Hydro2", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.AreEqual(layerCount, mdf.GetIndex(layer1));
            Assert.True(layer1 == mdf.GetLayerByName("Hydro2"));
        }

        [Test]
        public void TestMapDefinitionLayerAdd()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionLayerAdd");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();

            Assert.Throws<PreconditionException>(() => { mdf.AddLayer("IDontExist", "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, "", ""); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, null, ""); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, "", null); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, null, null); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.FeatureSource"); });
            Assert.Throws<PreconditionException>(() => { mdf.AddLayer(null, "Hydro", "Garbage"); });

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.AreEqual(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.AreEqual(layerCount + 1, mdf.GetLayerCount());
        }

        [Test]
        public void TestMapDefinitionLayerRemove()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionLayerRemove");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.AreEqual(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.AreEqual(layerCount + 1, mdf.GetLayerCount());

            mdf.RemoveLayer(layer);
            Assert.True(mdf.GetIndex(layer) < 0);
            Assert.AreEqual(layerCount, mdf.GetLayerCount());
        }

        [Test]
        public void TestMapDefinitionLayerReordering()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionLayerReordering");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();

            Assert.Throws<PreconditionException>(() => { mdf.MoveDown(null); });
            Assert.Throws<PreconditionException>(() => { mdf.MoveUp(null); });
            Assert.Throws<PreconditionException>(() => { mdf.SetTopDrawOrder(null); });
            Assert.Throws<PreconditionException>(() => { mdf.SetBottomDrawOrder(null); });

            IMapLayer layer = mdf.AddLayer(null, "Hydro", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            Assert.AreEqual(0, mdf.GetIndex(layer));
            Assert.True(layer == mdf.GetLayerByName("Hydro"));
            Assert.AreEqual(layerCount + 1, mdf.GetLayerCount());

            int value = mdf.MoveUp(layer);
            Assert.AreEqual(0, value); //Already at top
            value = mdf.MoveDown(layer);
            Assert.AreEqual(1, value);
            mdf.SetBottomDrawOrder(layer);
            value = mdf.GetIndex(layer);
            Assert.AreEqual(mdf.GetLayerCount() - 1, value);
            value = mdf.MoveDown(layer);
            Assert.AreEqual(mdf.GetLayerCount() - 1, value); //Already at bottom
            value = mdf.MoveUp(layer);
            Assert.AreEqual(mdf.GetLayerCount() - 2, value);
            mdf.SetTopDrawOrder(layer);
            value = mdf.GetIndex(layer);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void TestMapDefinitionGroupAdd()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionGroupAdd");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();
            int groupCount = mdf.GetGroupCount();

            Assert.Throws<PreconditionException>(() => { mdf.AddGroup(null); });
            Assert.Throws<PreconditionException>(() => { mdf.AddGroup(""); });
            Assert.Throws<PreconditionException>(() => { mdf.AddGroup("Group1"); });

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.AreEqual(groupCount + 1, mdf.GetGroupCount());
            Assert.NotNull(mdf.GetGroupByName("Test"));
            Assert.True(group == mdf.GetGroupByName("Test"));
        }

        [Test]
        public void TestMapDefinitionGroupRemove()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionGroupRemove");
            SetupMapDefinitionForTest(mdf);
            int layerCount = mdf.GetLayerCount();
            int groupCount = mdf.GetGroupCount();

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.AreEqual(groupCount + 1, mdf.GetGroupCount());
            Assert.NotNull(mdf.GetGroupByName("Test"));
            Assert.True(group == mdf.GetGroupByName("Test"));

            mdf.RemoveGroup(group);
            Assert.AreEqual(groupCount, mdf.GetGroupCount());
            Assert.Null(mdf.GetGroupByName("Test"));
        }

        [Test]
        public void TestMapDefinitionGroupReordering()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "TestMapDefinitionGroupReordering");
            SetupMapDefinitionForTest(mdf);
            int groupCount = mdf.GetGroupCount();

            Assert.Throws<PreconditionException>(() => { mdf.MoveDown(null); });
            Assert.Throws<PreconditionException>(() => { mdf.MoveUp(null); });
            Assert.Throws<PreconditionException>(() => { mdf.SetTopDrawOrder(null); });
            Assert.Throws<PreconditionException>(() => { mdf.SetBottomDrawOrder(null); });

            IMapLayerGroup group = mdf.AddGroup("Test");
            Assert.AreEqual(groupCount, mdf.GetIndex(group));
            Assert.True(group == mdf.GetGroupByName("Test"));
            Assert.AreEqual(groupCount + 1, mdf.GetGroupCount());

            int value = mdf.MoveUpGroup(group);
            Assert.AreEqual(groupCount - 1, value); //Already at top
            value = mdf.MoveDownGroup(group);
            Assert.AreEqual(groupCount, value);
        }

        [Test]
        public void TestMapDefinitionConversions()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var mdf = ObjectFactory.CreateMapDefinition(conn, new Version(1, 0, 0), "Test Map");
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            Assert.AreEqual("1.0.0", mdf.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("MapDefinition-1.0.0.xsd", mdf.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("MapDefinition-1.0.0.xsd", mdf.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), mdf.ResourceVersion);

            using (var fs = File.OpenWrite("MapDef_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(mdf))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var mdf2 = (IMapDefinition)conv.Convert(mdf, new Version(2, 3, 0));

            Assert.AreEqual("2.3.0", mdf2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("MapDefinition-2.3.0.xsd", mdf2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("MapDefinition-2.3.0.xsd", mdf2.ValidatingSchema);
            Assert.AreEqual(new Version(2, 3, 0), mdf2.ResourceVersion);
            Assert.NotNull(mdf2.CurrentConnection);
            Assert.True(mdf2 is IMapDefinition2);

            using (var fs = File.OpenWrite("MapDef_230.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(mdf2))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Test]
        public void TestLoadProcedureConversions()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var lproc = ObjectFactory.CreateLoadProcedure(conn, LoadType.Sdf);
            lproc.ResourceID = "Library://Samples/Sheboygan/Load/Load.LoadProcedure";

            Assert.AreEqual("1.0.0", lproc.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LoadProcedure-1.0.0.xsd", lproc.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LoadProcedure-1.0.0.xsd", lproc.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), lproc.ResourceVersion);

            using (var fs = File.OpenWrite("LoadProc_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(lproc))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var lproc2 = (ILoadProcedure)conv.Convert(lproc, new Version(1, 1, 0));

            Assert.AreEqual("1.1.0", lproc2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LoadProcedure-1.1.0.xsd", lproc2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LoadProcedure-1.1.0.xsd", lproc2.ValidatingSchema);
            Assert.AreEqual(new Version(1, 1, 0), lproc2.ResourceVersion);
            Assert.NotNull(lproc2.CurrentConnection);

            using (var fs = File.OpenWrite("LoadProc_110.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(lproc2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var lproc3 = (ILoadProcedure)conv.Convert(lproc2, new Version(2, 2, 0));

            Assert.AreEqual("2.2.0", lproc3.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("LoadProcedure-2.2.0.xsd", lproc3.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("LoadProcedure-2.2.0.xsd", lproc3.ValidatingSchema);
            Assert.AreEqual(new Version(2, 2, 0), lproc3.ResourceVersion);
            Assert.NotNull(lproc3.CurrentConnection);

            using (var fs = File.OpenWrite("LoadProc_220.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(lproc3))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Test]
        public void TestWebLayoutConversions()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var wl = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), "Library://Test.MapDefinition");
            wl.ResourceID = "Library://Test.WebLayout";

            Assert.AreEqual("1.0.0", wl.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("WebLayout-1.0.0.xsd", wl.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("WebLayout-1.0.0.xsd", wl.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), wl.ResourceVersion);

            using (var fs = File.OpenWrite("WebLayout_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(wl))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var wl2 = (IWebLayout)conv.Convert(wl, new Version(1, 1, 0));

            Assert.AreEqual("1.1.0", wl2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("WebLayout-1.1.0.xsd", wl2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("WebLayout-1.1.0.xsd", wl2.ValidatingSchema);
            Assert.AreEqual(new Version(1, 1, 0), wl2.ResourceVersion);
            Assert.NotNull(wl2.CurrentConnection);
            Assert.True(wl2 is IWebLayout2);

            using (var fs = File.OpenWrite("WebLayout_110.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(wl2))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Test]
        public void TestSymbolDefinitionConversions()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var conv = new ResourceObjectConverter();

            var ssym = ObjectFactory.CreateSimpleSymbol(conn, new Version(1, 0, 0), "SimpleSymbolTest", "Test simple symbol");
            ssym.ResourceID = "Library://Samples/Sheboygan/Symbols/Test.SymbolDefinition";

            Assert.AreEqual("1.0.0", ssym.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("SymbolDefinition-1.0.0.xsd", ssym.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("SymbolDefinition-1.0.0.xsd", ssym.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), ssym.ResourceVersion);

            using (var fs = File.OpenWrite("SimpleSymDef_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ssym))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var ssym2 = (ISimpleSymbolDefinition)conv.Convert(ssym, new Version(1, 1, 0));

            Assert.AreEqual("1.1.0", ssym2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("SymbolDefinition-1.1.0.xsd", ssym2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("SymbolDefinition-1.1.0.xsd", ssym2.ValidatingSchema);
            Assert.AreEqual(new Version(1, 1, 0), ssym2.ResourceVersion);
            Assert.NotNull(ssym2.CurrentConnection);

            using (var fs = File.OpenWrite("SimpleSymDef_110.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(ssym2))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var csym = ObjectFactory.CreateCompoundSymbol(conn, new Version(1, 0, 0), "CompoundSymbolTest", "Test compound symbol");
            csym.ResourceID = "Library://Samples/Sheboygan/Symbols/Compound.SymbolDefinition";

            Assert.AreEqual("1.0.0", csym.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("SymbolDefinition-1.0.0.xsd", csym.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("SymbolDefinition-1.0.0.xsd", csym.ValidatingSchema);
            Assert.AreEqual(new Version(1, 0, 0), csym.ResourceVersion);

            using (var fs = File.OpenWrite("CompoundSymDef_100.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(csym))
                {
                    Utility.CopyStream(src, fs);
                }
            }

            var csym2 = (ICompoundSymbolDefinition)conv.Convert(csym, new Version(1, 1, 0));

            Assert.AreEqual("1.1.0", csym2.GetResourceTypeDescriptor().Version);
            Assert.AreEqual("SymbolDefinition-1.1.0.xsd", csym2.GetResourceTypeDescriptor().XsdName);
            Assert.AreEqual("SymbolDefinition-1.1.0.xsd", csym2.ValidatingSchema);
            Assert.AreEqual(new Version(1, 1, 0), csym2.ResourceVersion);
            Assert.NotNull(csym2.CurrentConnection);

            using (var fs = File.OpenWrite("CompoundSymDef_110.xml"))
            {
                using (var src = ResourceTypeRegistry.Serialize(csym2))
                {
                    Utility.CopyStream(src, fs);
                }
            }
        }

        [Test]
        public void TestMapDefinitionNestedGroupDelete()
        {
            var conn = _mocks.NewMock<IServerConnection>();
            var caps = _mocks.NewMock<IConnectionCapabilities>();
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));
            foreach (var rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Stub.On(caps).Method("GetMaxSupportedResourceVersion").With(rt).Will(Return.Value(new Version(1, 0, 0)));
            }

            IMapDefinition mdf = ObjectFactory.CreateMapDefinition(conn, "Test");
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
            Assert.AreEqual(1, mdf.GetGroupCount());
            Assert.AreEqual(1, mdf.GetLayerCount());
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
