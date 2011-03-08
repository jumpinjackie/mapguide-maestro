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
using System.Text;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using System.IO;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace MaestroAPITests
{
    using Ldf110 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_1_0;
    using Ldf120 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_2_0;
    using Ldf130 = OSGeo.MapGuide.ObjectModels.LayerDefinition_1_3_0;

    using Lp110 = OSGeo.MapGuide.ObjectModels.LoadProcedure_1_1_0;
    using Lp220 = OSGeo.MapGuide.ObjectModels.LoadProcedure_2_2_0;
    using WL110 = OSGeo.MapGuide.ObjectModels.WebLayout_1_1_0;
    using OSGeo.MapGuide.ObjectModels;
    using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
    using OSGeo.MapGuide.MaestroAPI.Resource;
    using OSGeo.MapGuide.ObjectModels.LoadProcedure;
    using System.Diagnostics;
    using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;

    [SetUpFixture]
    public class TestBootstrap
    {
        //Guard variable to prevent duplicate registration on repeated test runs of a single test session
        static bool _registered = false;

        [SetUp]
        public void Setup()
        {
            if (!_registered)
            {
                //Layer Definition 1.1.0
                ResourceValidatorSet.RegisterValidator(new Ldf110.LayerDefinitionValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.1.0"),
                    new ResourceSerializationCallback(Ldf110.LdfEntryPoint.Serialize),
                    new ResourceDeserializationCallback(Ldf110.LdfEntryPoint.Deserialize));
                ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 1, 0), new LayerCreatorFunc(Ldf110.LdfEntryPoint.CreateDefault));

                //Layer Definition 1.2.0
                ResourceValidatorSet.RegisterValidator(new Ldf120.LayerDefinitionValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.2.0"),
                    new ResourceSerializationCallback(Ldf120.LdfEntryPoint.Serialize),
                    new ResourceDeserializationCallback(Ldf120.LdfEntryPoint.Deserialize));
                ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 2, 0), new LayerCreatorFunc(Ldf120.LdfEntryPoint.CreateDefault));

                //Layer Definition 1.3.0
                ResourceValidatorSet.RegisterValidator(new Ldf130.LayerDefinitionValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.LayerDefinition, "1.3.0"),
                    new ResourceSerializationCallback(Ldf130.LdfEntryPoint.Serialize),
                    new ResourceDeserializationCallback(Ldf130.LdfEntryPoint.Deserialize));
                ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 3, 0), new LayerCreatorFunc(Ldf130.LdfEntryPoint.CreateDefault));

                //Load Procedure 1.1.0
                ResourceValidatorSet.RegisterValidator(new Lp110.LoadProcedureValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.LoadProcedure, "1.1.0"),
                    new ResourceSerializationCallback(Lp110.LoadProcEntryPoint.Serialize),
                    new ResourceDeserializationCallback(Lp110.LoadProcEntryPoint.Deserialize));

                //Load Procedure 1.1.0 schema offers nothing new for the ones we want to support, so nothing to register
                //with the ObjectFactory

                //Load Procedure 2.2.0
                ResourceValidatorSet.RegisterValidator(new Lp220.LoadProcedureValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.LoadProcedure, "2.2.0"),
                    new ResourceSerializationCallback(Lp220.LoadProcEntryPoint.Serialize),
                    new ResourceDeserializationCallback(Lp220.LoadProcEntryPoint.Deserialize));
                ObjectFactory.RegisterLoadProcedureFactoryMethod(LoadType.Sqlite, new LoadProcCreatorFunc(Lp220.LoadProcEntryPoint.CreateDefaultSqlite));

                //Web Layout 1.1.0
                ResourceValidatorSet.RegisterValidator(new WL110.WebLayoutValidator());
                ResourceTypeRegistry.RegisterResource(
                    new ResourceTypeDescriptor(ResourceTypes.WebLayout, "1.1.0"),
                    new ResourceSerializationCallback(WL110.WebLayoutEntryPoint.Serialize),
                    new ResourceDeserializationCallback(WL110.WebLayoutEntryPoint.Deserialize));
                ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 1, 0), new WebLayoutCreatorFunc(WL110.WebLayoutEntryPoint.CreateDefault));

                _registered = true;
            }
        }

        [TearDown]
        public void Teardown()
        {
        }
    }

    public abstract class RuntimeMapTests
    {
        protected IServerConnection _conn;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            try
            {
                _conn = CreateTestConnection();
                SetupTestData();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetupTestData()
        {
            var resSvc = _conn.ResourceService;

            resSvc.DeleteResource("Library://UnitTests/");

            resSvc.SetResourceXmlData("Library://UnitTests/Maps/Sheboygan.MapDefinition", File.OpenRead("TestData/MappingService/UT_Sheboygan.mdf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Rail.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Parcels.LayerDefinition", File.OpenRead("TestData/TileService/UT_Parcels.ldf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Rail.FeatureSource", File.OpenRead("TestData/MappingService/UT_Rail.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Parcels.FeatureSource", File.OpenRead("TestData/TileService/UT_Parcels.fs"));

            resSvc.SetResourceData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", "UT_HydrographicPolygons.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Rail.FeatureSource", "UT_Rail.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_Rail.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Parcels.FeatureSource", "UT_Parcels.sdf", ResourceDataType.File, File.OpenRead("TestData/TileService/UT_Parcels.sdf"));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            
        }

        protected abstract IServerConnection CreateTestConnection();

        public virtual void TestCreate()
        {
            //Create a runtime map from its map definition, verify layer/group
            //structure is the same

            var resSvc = _conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + _conn.SessionID + "//TestCreate1.Map", mdf, 1.0);
            var map2 = mapSvc.CreateMap("Session:" + _conn.SessionID + "//TestCreate2.Map", "Library://UnitTests/Maps/Sheboygan.MapDefinition", 1.0);
            //var map3 = mapSvc.CreateMap("Library://UnitTests/Maps/Sheboygan.MapDefinition", 1.0);

            Assert.NotNull(map);
            Assert.NotNull(map2);
            //Assert.NotNull(map3);

            //Check resource ids
            //Assert.IsTrue(!string.IsNullOrEmpty(map3.ResourceID));
            Assert.AreEqual("Session:" + _conn.SessionID + "//TestCreate1.Map", map.ResourceID);
            Assert.AreEqual("Session:" + _conn.SessionID + "//TestCreate2.Map", map2.ResourceID);

            //Check layer/group structure
            Assert.IsTrue(Matches(map, mdf));
            Assert.IsTrue(Matches(map2, mdf));
            //Assert.IsTrue(Matches(map3, mdf));
        }

        private bool Matches(RuntimeMap map, IMapDefinition mdf)
        {
            if (map.MapDefinition != mdf.ResourceID) return false;
            if (map.Groups.Length != mdf.GetGroupCount()) return false;
            if (map.Layers.Length != mdf.GetLayerCount()) return false;

            foreach (var layer in map.Layers)
            {
                var ldfr = mdf.GetLayerByName(layer.Name);
                if (ldfr == null) return false;

                if (layer.LayerDefinitionID != ldfr.ResourceId) return false;
                if (layer.LegendLabel != ldfr.LegendLabel) return false;
                if (layer.Visible != ldfr.Visible) return false;
                if (layer.Selectable != ldfr.Selectable) return false;
                if (layer.ShowInLegend != ldfr.ShowInLegend) return false;
                if (layer.ExpandInLegend != ldfr.ExpandInLegend) return false;
            }

            foreach (var group in map.Groups)
            {
                var grp = mdf.GetGroupByName(group.Name);
                if (grp == null) return false;

                if (group.ExpandInLegend != grp.ExpandInLegend) return false;
                if (group.Group != grp.Group) return false;
                if (group.LegendLabel != grp.LegendLabel) return false;
                if (group.Name != grp.Name) return false;
                if (group.ShowInLegend != grp.ShowInLegend) return false;
                if (group.Visible != grp.Visible) return false;
            }

            return true;
        }

        public virtual void TestSave()
        {
            //Create a runtime map from its map definition, modify some layers
            //and groups. Save it, open another instance and verify the changes
            //have stuck
            var resSvc = _conn.ResourceService;
            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            var mid = "Session:" + _conn.SessionID + "//TestSave.Map";
            var map = mapSvc.CreateMap(mid, mdf, 1.0);
            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            
            //Call save
            Assert.IsTrue(Matches(map, mdf));
            map.Save();
            Assert.IsTrue(Matches(map, mdf));
            Assert.IsTrue(resSvc.ResourceExists(mid));

            //Open second runtime map instance
            var map2 = mapSvc.OpenMap(mid);
            Assert.IsFalse(map == map2);
            Assert.IsTrue(Matches(map2, mdf));

            //Tweak some settings
            var parcels = map2.GetLayerByName("Parcels");
            var rail = map2.GetLayerByName("Rail");
            Assert.NotNull(parcels);

            parcels.Visible = false;
            parcels.Selectable = false;

            Assert.NotNull(rail);

            rail.Selectable = false;

            //Save
            Assert.IsFalse(Matches(map2, mdf));
            map2.Save();
            Assert.IsFalse(Matches(map2, mdf));

            var map3 = mapSvc.OpenMap(mid);
            Assert.IsFalse(Matches(map3, mdf));

            parcels = null;
            rail = null;

            parcels = map3.GetLayerByName("Parcels");
            rail = map3.GetLayerByName("Rail");
            Assert.NotNull(parcels);

            Assert.IsFalse(parcels.Visible);
            Assert.IsFalse(parcels.Selectable);

            Assert.NotNull(rail);

            Assert.IsFalse(rail.Selectable);
        }

        public virtual void TestRender75k()
        {
            //Render a map of sheboygan at 75k
            //Only programmatically verify the returned stream can be fed to a
            //System.Drawing.Image object. 

            var resSvc = _conn.ResourceService;
            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            //FIXME: We have a problem. Can we calculate this value without MgCoordinateSystem and just using the WKT?
            //The answer to this will answer whether we can actually support the Rendering Service API over http 
            //using pure client-side runtime maps 
            //
            //The hard-coded value here was the output of MgCoordinateSystem.ConvertCoordinateSystemUnitsToMeters(1.0)
            //for this particular map.
            double metersPerUnit = 111319.490793274;
            var cs = CoordinateSystemBase.Create(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnitX;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + _conn.SessionID + "//TestRender75k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 75000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            map.Save();

            //Render default
            RenderAndVerify(mapSvc, map, "TestRender75k.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender75kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay75k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay75kConvenience.png", "PNG");

            //Turn off parcels
            var rail = map.GetLayerByName("Rail");
            Assert.NotNull(rail);
            rail.Visible = false;
            map.Save();

            //Render again
            RenderAndVerify(mapSvc, map, "TestRender75k_NoRail.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender75kConvenience_NoRail.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay75k_NoRail.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay75kConvenience_NoRail.png", "PNG");

            //Turn Rail back on
            rail = null;
            rail = map.GetLayerByName("Rail");
            Assert.NotNull(rail);
            rail.Visible = true;
            map.Save();

            //Render again
            RenderAndVerify(mapSvc, map, "TestRender75k_RailBackOn.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender75kConvenience_RailBackOn.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay75k_RailBackOn.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay75kConvenience_RailBackOn.png", "PNG");
        }

        #region render helpers

        private static void RenderDynamicOverlayAndVerifyConvenience(RuntimeMap map, string fileName, string format)
        {
            using (var stream = map.RenderDynamicOverlay(format, true))
            {
                using (var ms = new MemoryStream())
                using (var ms2 = new MemoryStream())
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Utility.CopyStream(stream, ms);
                    Utility.CopyStream(ms, ms2);
                    Utility.CopyStream(ms, fs);
                    //See if System.Drawing.Image accepts this
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                }
            }
        }

        private static void RenderDynamicOverlayAndVerify(IMappingService mapSvc, RuntimeMap map, string fileName, string format)
        {
            using (var stream = mapSvc.RenderDynamicOverlay(map, map.Selection, format))
            {
                using (var ms = new MemoryStream())
                using (var ms2 = new MemoryStream())
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Utility.CopyStream(stream, ms);
                    Utility.CopyStream(ms, ms2);
                    Utility.CopyStream(ms, fs);
                    //See if System.Drawing.Image accepts this
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                }
            }
        }

        private static void RenderAndVerify(IMappingService mapSvc, RuntimeMap map, string fileName, string format)
        {
            using (var stream = mapSvc.RenderRuntimeMap(map.ResourceID, map.ViewCenter.X, map.ViewCenter.Y, map.ViewScale, map.DisplayWidth, map.DisplayHeight, map.DisplayDpi, format))
            {
                using (var ms = new MemoryStream())
                using (var ms2 = new MemoryStream())
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Utility.CopyStream(stream, ms);
                    Utility.CopyStream(ms, ms2);
                    Utility.CopyStream(ms, fs);
                    //See if System.Drawing.Image accepts this
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                }
            }
        }

        private static void RenderAndVerifyConvenience(RuntimeMap map, string fileName, string format)
        {
            using (var stream = map.Render(format))
            {
                using (var ms = new MemoryStream())
                using (var ms2 = new MemoryStream())
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Utility.CopyStream(stream, ms);
                    Utility.CopyStream(ms, ms2);
                    Utility.CopyStream(ms, fs);
                    //See if System.Drawing.Image accepts this
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                }
            }
        }

        #endregion

        public virtual void TestRender12k()
        { 
            //Render a map of sheboygan at 12k
            //Only programmatically verify the returned stream can be fed to a
            //System.Drawing.Image object. 

            var resSvc = _conn.ResourceService;
            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            //FIXME: We have a problem. Can we calculate this value without MgCoordinateSystem and just using the WKT?
            //The answer to this will answer whether we can actually support the Rendering Service API over http 
            //using pure client-side runtime maps 
            //
            //The hard-coded value here was the output of MgCoordinateSystem.ConvertCoordinateSystemUnitsToMeters(1.0)
            //for this particular map.
            double metersPerUnit = 111319.490793274;
            var cs = CoordinateSystemBase.Create(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnitX;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + _conn.SessionID + "//TestRender12k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            map.Save();

            //Render default
            RenderAndVerify(mapSvc, map, "TestRender12k.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender12kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay12k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay12kConvenience.png", "PNG");

            //Turn off parcels
            var parcels = map.GetLayerByName("Parcels");
            Assert.NotNull(parcels);
            parcels.Visible = false;
            map.Save();
            
            //Render again
            RenderAndVerify(mapSvc, map, "TestRender12k_NoParcels.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender12kConvenience_NoParcels.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay12k_NoParcels.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay12kConvenience_NoParcels.png", "PNG");

            //Turn parcels back on
            parcels = null;
            parcels = map.GetLayerByName("Parcels");
            Assert.NotNull(parcels);
            parcels.Visible = true;
            map.Save();

            //Render again
            RenderAndVerify(mapSvc, map, "TestRender12k_ParcelsBackOn.png", "PNG");
            RenderAndVerifyConvenience(map, "TestRender12kConvenience_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, "TestRenderOverlay12k_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, "TestRenderOverlay12kConvenience_ParcelsBackOn.png", "PNG");
        }

        public virtual void TestResourceEvents()
        {
            bool deleteCalled = false;
            bool updateCalled = false;
            bool insertCalled = false;

            var conn = CreateTestConnection();
            conn.ResourceService.ResourceAdded += (s, e) => { insertCalled = true; };
            conn.ResourceService.ResourceDeleted += (s, e) => { deleteCalled = true; };
            conn.ResourceService.ResourceUpdated += (s, e) => { updateCalled = true; };

            //This should raise ResourceAdded
            conn.ResourceService.SetResourceXmlData("Library://UnitTests/ResourceEvents/Test.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));

            //This should raise ResourceUpdated
            conn.ResourceService.SetResourceXmlData("Library://UnitTests/ResourceEvents/Test.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));

            //This should raise ResourceDeleted
            conn.ResourceService.DeleteResource("Library://UnitTests/ResourceEvents/Test.LayerDefinition");

            Assert.IsTrue(deleteCalled);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(insertCalled);
        }

        public virtual void TestLargeMapCreatePerformance()
        {
            TestMapCreate(50, 10);
            TestMapCreate(100, 25);
            TestMapCreate(200, 50);
        }

        private void TestMapCreate(int layerSize, int groupSize)
        {
            //Create a 200 layer, 50 group map. This is not part of the benchmark
            var mdf = ObjectFactory.CreateMapDefinition(_conn, "LargeMap");
            string root = "Library://UnitTests/LargeMapTest/";

            //We have to create 200 unique layer definitions (same content) otherwise only one GetResourceContent
            //call is issued, thus not truly reflecting on how a 200 layer map would really perform
            //
            //But that doesn't mean we have to read the same file 200 times
            MemoryStream ms = new MemoryStream();
            using (var fs = File.OpenRead("TestData/MappingService/UT_Rail.ldf"))
            {
                Utility.CopyStream(fs, ms);
            }

            int step = layerSize / groupSize;
            int g = 0;
            for (int i = 0; i < layerSize; i++)
            {
                if (i % step == 0)
                    g++;

                string layerName = "Layer" + i;
                string groupName = "Group" + g;

                if (mdf.GetGroupByName(groupName) == null)
                {
                    mdf.AddGroup(groupName);
                }

                //Rewind
                ms.Position = 0;

                string lid = root + "Layers/Layer" + i + ".LayerDefinition";
                _conn.ResourceService.SetResourceXmlData(lid, ms);
                mdf.AddLayer(groupName, layerName, lid);
            }

            Assert.IsTrue(Array.IndexOf(_conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0);
            var mapSvc = (IMappingService)_conn.GetService((int)ServiceType.Mapping);
            var mid = "Session:" + _conn.SessionID + "//TestLargeMapCreatePerformance.Map";

            //Begin Benchmark
            var sw = new Stopwatch();
            sw.Start();
            var map = mapSvc.CreateMap(mid, mdf, 1.0);
            sw.Stop();

            string msg = "Create Map time for " + layerSize + " layer, " + groupSize + " group map: " + sw.ElapsedMilliseconds + "ms";
            Trace.WriteLine(msg);
        }
    }

    [TestFixture(Ignore = true)]
    public class HttpRuntimeMapTests : RuntimeMapTests
    {
        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                "Url", "http://" + Environment.MachineName + "/mapguide/mapagent/mapagent.fcgi",
                "Username", "Administrator",
                "Password", "admin");
        }

        [Test]
        public override void TestResourceEvents()
        {
            base.TestResourceEvents();
        }

        [Test]
        public override void TestCreate()
        {
            base.TestCreate();
        }

        [Test]
        public override void TestSave()
        {
            base.TestSave();
        }

        [Test]
        public override void TestRender75k()
        {
            base.TestRender75k();
        }

        [Test]
        public override void TestRender12k()
        {
            base.TestRender12k();
        }

        [Test]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }
    }

    [TestFixture(Ignore = true)]
    public class LocalRuntimeMapTests : RuntimeMapTests
    {
        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative",
                "ConfigFile", "webconfig.ini",
                "Username", "Administrator",
                "Password", "admin");
        }

        [Test]
        public override void TestResourceEvents()
        {
            base.TestResourceEvents();
        }

        [Test]
        public override void TestCreate()
        {
            base.TestCreate();
        }

        [Test]
        public override void TestSave()
        {
            base.TestSave();
        }

        [Test]
        public override void TestRender75k()
        {
            base.TestRender75k();
        }

        [Test]
        public override void TestRender12k()
        {
            base.TestRender12k();
        }

        [Test]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }
    }
}
