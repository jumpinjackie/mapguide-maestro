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
    using OSGeo.MapGuide.ObjectModels;
    using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
    using OSGeo.MapGuide.MaestroAPI.Resource;
    using OSGeo.MapGuide.ObjectModels.LoadProcedure;
    using System.Diagnostics;
    using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
    using System.Drawing;
    using OSGeo.MapGuide.ExtendedObjectModels;
    using OSGeo.MapGuide.ObjectModels.LayerDefinition;
    using OSGeo.MapGuide.ObjectModels.DrawingSource;
    using OSGeo.MapGuide.MaestroAPI.Commands;
    using MapModel = OSGeo.MapGuide.ObjectModels.RuntimeMap;

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
                TestEnvironment.Initialize("TestMaestroAPI.xml");
                TestEnvironment.PrintSummary();

                ResourceValidatorLoader.LoadStockValidators();
                ModelSetup.Initialize();
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

        protected virtual bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            return false;
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            string reason;
            if (this.ShouldIgnore(out reason))
            {
                Assert.Ignore(reason);
            }
            else
            {
                _conn = CreateTestConnection();
                SetupTestData();
            }
        }

        private void SetupTestData()
        {
            var resSvc = _conn.ResourceService;

            resSvc.DeleteResource("Library://UnitTests/");

            resSvc.SetResourceXmlData("Library://UnitTests/Maps/Sheboygan.MapDefinition", File.OpenRead("TestData/MappingService/UT_Sheboygan.mdf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganTiled.MapDefinition", File.OpenRead("UserTestData/TestTiledMap.xml"));

            resSvc.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Rail.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Parcels.LayerDefinition", File.OpenRead("TestData/TileService/UT_Parcels.ldf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Rail.FeatureSource", File.OpenRead("TestData/MappingService/UT_Rail.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Parcels.FeatureSource", File.OpenRead("TestData/TileService/UT_Parcels.fs"));

            resSvc.SetResourceData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", "UT_HydrographicPolygons.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Rail.FeatureSource", "UT_Rail.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_Rail.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Parcels.FeatureSource", "UT_Parcels.sdf", ResourceDataType.File, File.OpenRead("TestData/TileService/UT_Parcels.sdf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Data/SpaceShip.DrawingSource", File.OpenRead("TestData/DrawingService/SpaceShipDrawingSource.xml"));
            resSvc.SetResourceData("Library://UnitTests/Data/SpaceShip.DrawingSource", "SpaceShip.dwf", ResourceDataType.File, File.OpenRead("TestData/DrawingService/SpaceShip.dwf"));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            
        }

        protected abstract IServerConnection CreateTestConnection();

        public virtual void TestGroupAssignment()
        {
            var resSvc = _conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);
            var gdf1 = mdf.AddGroup("Group1");
            var gdf2 = mdf.AddGroup("Group2");
            gdf2.Group = gdf1.Name;
            resSvc.SaveResourceAs(mdf, "Session:" + _conn.SessionID + "//TestGroupAssignment.MapDefinition");
            mdf.ResourceID = "Session:" + _conn.SessionID + "//TestGroupAssignment.MapDefinition";

            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + _conn.SessionID + "//TestGroupAssignment.Map", mdf, 1.0);
            foreach(var grp in mdf.MapLayerGroup)
            {
                var rtGrp = mapSvc.CreateMapGroup(map, grp);
                Assert.AreEqual(rtGrp.Group, grp.Group);
            }
        }

        public virtual void TestExtentSerialization()
        {
            var resSvc = _conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);
            mdf.Extents = ObjectFactory.CreateEnvelope(1.0, 2.0, 3.0, 4.0);

            var mapSvc = _conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + _conn.SessionID + "//TestExtentSerialization.Map", mdf, 1.0);
            map.Save();

            var map2 = mapSvc.OpenMap("Session:" + _conn.SessionID + "//TestExtentSerialization.Map");
            Assert.AreEqual(1.0, map2.DataExtent.MinX);
            Assert.AreEqual(2.0, map2.DataExtent.MinY);
            Assert.AreEqual(3.0, map2.DataExtent.MaxX);
            Assert.AreEqual(4.0, map2.DataExtent.MaxY);
        }

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

            Assert.NotNull(map.Layers["Parcels"]);
            var parcelsLayers = map.Layers["Parcels"];
            Assert.AreEqual(1, parcelsLayers.ScaleRanges.Length);
            Assert.AreEqual(0.0, parcelsLayers.ScaleRanges[0].MinScale);
            Assert.AreEqual(13000.0, parcelsLayers.ScaleRanges[0].MaxScale);

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

        protected bool Matches(RuntimeMap map, IMapDefinition mdf)
        {
            if (map.MapDefinition != mdf.ResourceID) return false;
            if (map.Groups.Count != mdf.GetGroupCount()) return false;
            if (map.Layers.Count != mdf.GetLayerCount()) return false;

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
            if (CaresAboutRuntimeMapState) Assert.IsTrue(!resSvc.ResourceExists(mid));
            
            //Call save
            Assert.IsTrue(Matches(map, mdf));
            Assert.IsFalse(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);
            Assert.IsTrue(Matches(map, mdf));
            if (CaresAboutRuntimeMapState) Assert.IsTrue(resSvc.ResourceExists(mid));

            //Tests below not applicable if test suite doesn't care about runtime state
            if (!CaresAboutRuntimeMapState)
                return;

            //Open second runtime map instance
            var map2 = mapSvc.OpenMap(mid);
            Assert.IsFalse(map == map2);
            Assert.IsTrue(Matches(map2, mdf));

            //Tweak some settings
            var parcels = map2.Layers["Parcels"];
            //Check these values still came through
            Assert.AreEqual(1, parcels.ScaleRanges.Length);
            Assert.AreEqual(0.0, parcels.ScaleRanges[0].MinScale);
            Assert.AreEqual(13000.0, parcels.ScaleRanges[0].MaxScale);

            var rail = map2.Layers["Rail"];
            Assert.NotNull(parcels);

            parcels.Visible = false;
            parcels.Selectable = false;

            Assert.NotNull(rail);

            rail.Selectable = false;

            //Save
            Assert.IsFalse(Matches(map2, mdf));
            Assert.IsTrue(map2.IsDirty);
            map2.Save();
            Assert.IsFalse(map2.IsDirty);
            Assert.IsFalse(Matches(map2, mdf));

            var map3 = mapSvc.OpenMap(mid);
            Assert.IsFalse(Matches(map3, mdf));

            parcels = null;
            rail = null;

            parcels = map3.Layers["Parcels"];
            rail = map3.Layers["Rail"];
            Assert.NotNull(parcels);

            Assert.IsFalse(parcels.Visible);
            Assert.IsFalse(parcels.Selectable);

            Assert.NotNull(rail);

            Assert.IsFalse(rail.Selectable);
        }

        public abstract string TestPrefix { get; }

        protected virtual bool CaresAboutRuntimeMapState { get { return true; } }

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

            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestRender75k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            Assert.IsFalse(map.IsDirty);
            map.ViewScale = 75000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            if (CaresAboutRuntimeMapState) Assert.IsTrue(!resSvc.ResourceExists(mid));
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render default
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender75k.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender75kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay75k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay75kConvenience.png", "PNG");

            //Turn off parcels
            var rail = map.Layers["Rail"];
            Assert.NotNull(rail);
            rail.Visible = false;
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender75k_NoRail.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender75kConvenience_NoRail.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay75k_NoRail.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay75kConvenience_NoRail.png", "PNG");

            //Turn Rail back on
            rail = null;
            rail = map.Layers["Rail"];
            Assert.NotNull(rail);
            rail.Visible = true;
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender75k_RailBackOn.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender75kConvenience_RailBackOn.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay75k_RailBackOn.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay75kConvenience_RailBackOn.png", "PNG");
        }

        #region render helpers

        private static void RenderLegendAndVerifyConvenience(RuntimeMap map, int width, int height, string fileName, string format)
        {
            using (var stream = map.RenderMapLegend(width, height, Color.White, format))
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

        private static void RenderLegendAndVerify(IMappingService mapSvc, RuntimeMap map, int width, int height, string fileName, string format)
        {
            using (var stream = mapSvc.RenderMapLegend(map, width, height, Color.White, format))
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
            using (var stream = mapSvc.RenderRuntimeMap(map, map.ViewCenter.X, map.ViewCenter.Y, map.ViewScale, map.DisplayWidth, map.DisplayHeight, map.DisplayDpi, format))
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

        public virtual void TestLegendIconRendering()
        {
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

            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestLegendIconRendering.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            int counter = 0;
            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImage(map.ViewScale, layer.LayerDefinitionID, -1, -1);
                icon.Save(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.png");
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImage(map.ViewScale, layer.LayerDefinitionID, -1, -1, 16, 16, "JPG");
                icon.Save(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.jpg");
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImage(map.ViewScale, layer.LayerDefinitionID, -1, -1, 16, 16, "GIF");
                icon.Save(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.gif");
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImage(map.ViewScale, layer.LayerDefinitionID, -1, -1, 160, 50, "PNG");
                icon.Save(TestPrefix + "TestLegendIconRendering_" + counter + "_160x50.png");
                counter++;
            }
        }

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

            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestRender12k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            Assert.IsFalse(map.IsDirty);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render default
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender12k.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender12kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay12k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay12kConvenience.png", "PNG");

            //Turn off parcels
            var parcels = map.Layers["Parcels"];
            Assert.NotNull(parcels);
            parcels.Visible = false;
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);
            
            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender12k_NoParcels.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender12kConvenience_NoParcels.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay12k_NoParcels.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay12kConvenience_NoParcels.png", "PNG");

            //Turn parcels back on
            parcels = null;
            parcels = map.Layers["Parcels"];
            Assert.NotNull(parcels);
            parcels.Visible = true;
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender12k_ParcelsBackOn.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender12kConvenience_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay12k_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay12kConvenience_ParcelsBackOn.png", "PNG");
        }

        public virtual void TestMapManipulation()
        {
            //Render a map of sheboygan at 12k
            //Verify that layer removal and addition produces the expected rendered result

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

            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestMapManipulation.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.IsTrue(!resSvc.ResourceExists(mid));
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render default
            RenderAndVerify(mapSvc, map, TestPrefix + "TestMapManipulation12kWithRail.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestMapManipulation12kConvenienceWithRail.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestMapManipulationOverlay12kWithRail.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestMapManipulationOverlay12kConvenienceWithRail.png", "PNG");

            RenderLegendAndVerify(mapSvc, map, 200, 600, TestPrefix + "TestLegend12kWithRail.png", "PNG");
            RenderLegendAndVerifyConvenience(map, 200, 600, TestPrefix + "TestLegend12kConvenienceWithRail.png", "PNG");

            //Remove parcels
            var rail = map.Layers["Rail"];
            Assert.NotNull(rail);
            map.Layers.Remove(rail);
            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestMapManipulation12k_RailRemoved.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestMapManipulation12kConvenience_RailRemoved.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestMapManipulationOverlay12k_RailRemoved.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestMapManipulationOverlay12kConvenience_RailRemoved.png", "PNG");

            RenderLegendAndVerify(mapSvc, map, 200, 600, TestPrefix + "TestLegend12k_RailRemoved.png", "PNG");
            RenderLegendAndVerifyConvenience(map, 200, 600, TestPrefix + "TestLegend12kConvenience_RailRemoved.png", "PNG");

            //Add rail again
            rail = null;
            rail = map.Layers["Rail"];
            Assert.Null(rail);

            rail = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Rail.LayerDefinition"));
            rail.LegendLabel = "Rail";
            rail.Visible = true;
            rail.ShowInLegend = true;
            rail.ExpandInLegend = true;

            map.Layers.Insert(0, rail);

            //map.AddLayer(rail);
            //Set draw order above parcels
            //var parcels = map.GetLayerByName("Parcels");
            //rail.SetDrawOrder(parcels.DisplayOrder - 0.000001);

            Assert.IsTrue(map.IsDirty);
            map.Save();
            Assert.IsFalse(map.IsDirty);

            //Render again. Rail should be above parcels
            RenderAndVerify(mapSvc, map, TestPrefix + "TestMapManipulation12k_RailReAdded.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestMapManipulation12kConvenience_RailReAdded.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestMapManipulationOverlay12k_RailReAdded.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestMapManipulationOverlay12kConvenience_RailReAdded.png", "PNG");

            RenderLegendAndVerify(mapSvc, map, 200, 600, TestPrefix + "TestLegend12k_RailReAdded.png", "PNG");
            RenderLegendAndVerifyConvenience(map, 200, 600, TestPrefix + "TestLegend12kConvenience_RailReAdded.png", "PNG");
        }

        public virtual void TestMapManipulation2()
        {
            //This is mainly to exercise Insert() and to verify that draw orders come back as expected

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

            //Empty the layer/group list because we will add them individually
            var removeLayers = new List<IMapLayer>(mdf.MapLayer);
            foreach (var removeMe in removeLayers)
                mdf.RemoveLayer(removeMe);

            var removeGroups = new List<IMapLayerGroup>(mdf.MapLayerGroup);
            foreach (var removeMe in removeGroups)
                mdf.RemoveGroup(removeMe);

            var testResourceId = "Library://UnitTests/Maps/Sheboygan_" + TestPrefix + "_TestMapManipulation2.MapDefinition";
            resSvc.SaveResourceAs(mdf, testResourceId);
            mdf = resSvc.GetResource(testResourceId) as IMapDefinition;
            Assert.NotNull(mdf);

            //Now create our runtime map
            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestMapManipulation2.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(0, map.Groups.Count);

            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group1"));//new RuntimeMapGroup(map, "Group1"));
            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group2"));//new RuntimeMapGroup(map, "Group2"));
            Assert.AreEqual(2, map.Groups.Count);

            Assert.NotNull(map.Groups["Group1"]);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Groups["Group3"]);

            var layer = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"));
            layer.Group = "Group1";

            map.Layers.Insert(0, layer);
            Assert.AreEqual(1, map.Layers.Count);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));

            var layer2 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Parcels.LayerDefinition"));
            map.Layers.Insert(0, layer2);
            layer2.Group = "Group1"; //Intentional

            Assert.AreEqual(2, map.Layers.Count);
            Assert.True(layer2 == map.Layers[0]);
            Assert.False(layer2 == map.Layers[1]);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.NotNull(map.Layers["Parcels"]);
            Assert.True(map.Layers["Parcels"] == map.Layers[0]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[1]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer2.ObjectId));
            //The important one
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[1].DisplayOrder);

            var layer3 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Rail.LayerDefinition"));
            layer3.Group = "Group2";
            map.Layers.Insert(0, layer3);
            Assert.AreEqual(3, map.Layers.Count);
            Assert.True(layer3 == map.Layers[0]);
            Assert.False(layer3 == map.Layers[1]);
            Assert.False(layer3 == map.Layers[2]);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.NotNull(map.Layers["Parcels"]);
            Assert.NotNull(map.Layers["Rail"]);
            Assert.True(map.Layers["Rail"] == map.Layers[0]);
            Assert.True(map.Layers["Parcels"] == map.Layers[1]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[2]);
            //The important one
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[1].DisplayOrder);
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[2].DisplayOrder);
            Assert.True(map.Layers[1].DisplayOrder < map.Layers[2].DisplayOrder);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            Assert.AreEqual(2, map.GetLayersOfGroup("Group1").Length);
            Assert.AreEqual(1, map.GetLayersOfGroup("Group2").Length);

            //Group1 has 2 layers
            map.Groups.Remove("Group1");
            Assert.AreEqual(1, map.Layers.Count);
            Assert.Null(map.Groups["Group1"]);
            Assert.True(map.Groups["Group2"] == map.Groups[0]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            //Removing layer doesn't affect its group. It will still be there
            map.Layers.Remove(layer3.Name);
            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(1, map.Groups.Count);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer3.ObjectId));

            map.Groups.Remove("Group2");
            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(0, map.Groups.Count);
        }

        public virtual void TestMapManipulation3()
        {
            //Pretty much the same as TestMapManipulation2() but with Add() instead of Insert()
            //Add() puts the item at the end of the collection, so draw orders should reflect
            //that

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

            //Empty the layer/group list because we will add them individually
            var removeLayers = new List<IMapLayer>(mdf.MapLayer);
            foreach (var removeMe in removeLayers)
                mdf.RemoveLayer(removeMe);

            var removeGroups = new List<IMapLayerGroup>(mdf.MapLayerGroup);
            foreach (var removeMe in removeGroups)
                mdf.RemoveGroup(removeMe);

            var testResourceId = "Library://UnitTests/Maps/Sheboygan_" + TestPrefix + "_TestMapManipulation3.MapDefinition";
            resSvc.SaveResourceAs(mdf, testResourceId);
            mdf = resSvc.GetResource(testResourceId) as IMapDefinition;
            Assert.NotNull(mdf);

            //Now create our runtime map
            var mid = "Session:" + _conn.SessionID + "//" + TestPrefix + "TestMapManipulation2.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(0, map.Groups.Count);

            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group1"));//new RuntimeMapGroup(map, "Group1"));
            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group2"));//new RuntimeMapGroup(map, "Group2"));
            Assert.AreEqual(2, map.Groups.Count);

            Assert.NotNull(map.Groups["Group1"]);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Groups["Group3"]);

            var layer = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"));
            layer.Group = "Group1";

            map.Layers.Add(layer);
            Assert.AreEqual(1, map.Layers.Count);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));

            var layer2 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Parcels.LayerDefinition"));
            map.Layers.Add(layer2);
            layer2.Group = "Group1"; //Intentional

            Assert.AreEqual(2, map.Layers.Count);
            Assert.False(layer2 == map.Layers[0]);
            Assert.True(layer2 == map.Layers[1]);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.NotNull(map.Layers["Parcels"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.True(map.Layers["Parcels"] == map.Layers[1]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer2.ObjectId));
            //The important one
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[1].DisplayOrder);

            var layer3 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Rail.LayerDefinition"));
            layer3.Group = "Group2";
            map.Layers.Add(layer3);
            Assert.AreEqual(3, map.Layers.Count);
            Assert.False(layer3 == map.Layers[0]);
            Assert.False(layer3 == map.Layers[1]);
            Assert.True(layer3 == map.Layers[2]);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.NotNull(map.Layers["Parcels"]);
            Assert.NotNull(map.Layers["Rail"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.True(map.Layers["Parcels"] == map.Layers[1]);
            Assert.True(map.Layers["Rail"] == map.Layers[2]);
            //The important one
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[1].DisplayOrder);
            Assert.True(map.Layers[0].DisplayOrder < map.Layers[2].DisplayOrder);
            Assert.True(map.Layers[1].DisplayOrder < map.Layers[2].DisplayOrder);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            Assert.AreEqual(2, map.GetLayersOfGroup("Group1").Length);
            Assert.AreEqual(1, map.GetLayersOfGroup("Group2").Length);

            //Group1 has 2 layers
            map.Groups.Remove("Group1");
            Assert.AreEqual(1, map.Layers.Count);
            Assert.Null(map.Groups["Group1"]);
            Assert.True(map.Groups["Group2"] == map.Groups[0]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            //Removing layer doesn't affect its group. It will still be there
            map.Layers.Remove(layer3.Name);
            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(1, map.Groups.Count);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer3.ObjectId));

            map.Groups.Remove("Group2");
            Assert.AreEqual(0, map.Layers.Count);
            Assert.AreEqual(0, map.Groups.Count);
        }

        public virtual void TestMapManipulation4()
        {
            IServerConnection conn = CreateTestConnection();
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string mapdefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            ResourceIdentifier rtmX = new ResourceIdentifier(mapdefinition);
            string rtmDef = "Library://UnitTests/Cache/" + rtmX.Fullpath.Replace(":", "_").Replace("/", "_");
            string mapName = rtmX.Name;
            ResourceIdentifier mapid = new ResourceIdentifier(mapName, ResourceTypes.RuntimeMap, conn.SessionID);
            IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
            RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map
            Assert.IsFalse(rtm.IsDirty);
            rtm.Save();
            Assert.IsFalse(rtm.IsDirty);
            RuntimeMap tmprtm = mapSvc.CreateMap(mapid, mdef); // Create new map in data cache
            Assert.IsFalse(tmprtm.IsDirty);
            tmprtm.Save();
            Assert.IsFalse(tmprtm.IsDirty);

            RuntimeMap mymap = mapSvc.OpenMap(mapid); 
        }

        public virtual void TestMapManipulation5()
        {
            IServerConnection conn = CreateTestConnection();
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string mapdefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";
            IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
            RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map
            rtm.Save();
            Assert.IsFalse(rtm.IsDirty);
            RuntimeMap mymap = mapSvc.OpenMap("Session:" + conn.SessionID + "//" + rtm.Name + ".Map");
        }

        public virtual void TestMapAddDwfLayer()
        {
            IServerConnection conn = CreateTestConnection();
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string mapdefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";
            IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
            RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map

            var ds = (IDrawingSource)conn.ResourceService.GetResource("Library://UnitTests/Data/SpaceShip.DrawingSource");
            //The DWF in question can't be interrogated for extents, so flub it
            string sheetName = string.Empty;
            foreach (var sheet in ds.Sheet)
            {
                sheet.Extent = ObjectFactory.CreateEnvelope(-100000, -100000, 100000, 100000);
                if (string.IsNullOrEmpty(sheetName))
                    sheetName = sheet.Name;
            }
            conn.ResourceService.SaveResource(ds);
            var ldf = ObjectFactory.CreateDefaultLayer(conn, LayerType.Drawing);
            var dl = (IDrawingLayerDefinition)ldf.SubLayer;
            dl.ResourceId = ds.ResourceID;
            dl.Sheet = sheetName;
            ldf.ResourceID = "Session:" + conn.SessionID + "//TestDrawing.LayerDefinition";
            conn.ResourceService.SaveResource(ldf);

            //Add our dwf layer
            var rtLayer = mapSvc.CreateMapLayer(rtm, ldf);
            rtm.Layers.Add(rtLayer);

            rtm.Save();
            Assert.IsFalse(rtm.IsDirty);
            RuntimeMap mymap = mapSvc.OpenMap("Session:" + conn.SessionID + "//" + rtm.Name + ".Map");
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

    [TestFixture]
    public class HttpRuntimeMapTests : RuntimeMapTests
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreHttpRuntimeMapTests)
                reason = "Skipping HttpRuntimeMapTests because TestControl.IgnoreHttpRuntimeMapTests = true";

            return TestControl.IgnoreHttpRuntimeMapTests;
        }

        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestHttpConnection();
        }

        public override string TestPrefix
        {
            get { return "Http"; }
        }

        [Test]
        public override void TestGroupAssignment()
        {
            base.TestGroupAssignment();
        }

        [Test]
        public override void TestExtentSerialization()
        {
            base.TestExtentSerialization();
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
        public override void TestLegendIconRendering()
        {
            base.TestLegendIconRendering();
        }

        [Test]
        public override void TestMapManipulation()
        {
            base.TestMapManipulation();
        }

        [Test]
        public override void TestMapManipulation2()
        {
            base.TestMapManipulation2();
        }

        [Test]
        public override void TestMapManipulation3()
        {
            base.TestMapManipulation3();
        }

        [Test]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }
        
        [Test]
        public override void TestMapManipulation4()
        {
            base.TestMapManipulation4();
        }

        [Test]
        public override void TestMapManipulation5()
        {
            base.TestMapManipulation5();
        }

        [Test]
        public override void TestMapAddDwfLayer()
        {
            base.TestMapAddDwfLayer();
        }

        [Test]
        public void TestCreateRuntimeMapRequest()
        {
            if (_conn.SiteVersion < new Version(2, 6))
            {
                Assert.Ignore("Skipping test (TestCreateRuntimeMapRequest). MapGuide is older than 2.6");
                return;
            }

            int[] cmds = _conn.Capabilities.SupportedCommands;
            Assert.True(Array.IndexOf(cmds, (int)CommandType.CreateRuntimeMap) >= 0);

            //Barebones
            ICreateRuntimeMap create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            MapModel.IRuntimeMapInfo rtInfo = create.Execute();
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);
            
            //Barebones with tiled
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";

            rtInfo = create.Execute();
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count > 0);
            Assert.NotNull(rtInfo.FiniteDisplayScales);
            Assert.True(rtInfo.FiniteDisplayScales.Length > 0);

            //With Layer/Group structure
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups);

            rtInfo = create.Execute();
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.Null(layer.FeatureSource);
                Assert.True(layer.ScaleRanges.Count > 0);
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //With Layer/Group structure and inline icons
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons);

            rtInfo = create.Execute();
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNotNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.Null(layer.FeatureSource);
                Assert.True(layer.ScaleRanges.Count > 0);
                foreach (var sr in layer.ScaleRanges)
                {
                    Assert.NotNull(sr.FeatureStyle);
                    Assert.True(sr.FeatureStyle.Count > 0);
                    foreach (var feat in sr.FeatureStyle)
                    {
                        Assert.NotNull(feat.Rules);
                        Assert.True(feat.Rules.Count > 0);

                        foreach (var rule in feat.Rules)
                        {
                            Assert.IsNotNullOrEmpty(rule.IconBase64);
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Kitchen sink
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);

            rtInfo = create.Execute();
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNotNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.NotNull(layer.FeatureSource);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.ClassName);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.Geometry);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.ResourceID);
                Assert.True(layer.ScaleRanges.Count > 0);
                foreach (var sr in layer.ScaleRanges)
                {
                    Assert.NotNull(sr.FeatureStyle);
                    Assert.True(sr.FeatureStyle.Count > 0);
                    foreach (var feat in sr.FeatureStyle)
                    {
                        Assert.NotNull(feat.Rules);
                        Assert.True(feat.Rules.Count > 0);

                        foreach (var rule in feat.Rules)
                        {
                            Assert.IsNotNullOrEmpty(rule.IconBase64);
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);
        }

        [Test]
        public void TestDescribeRuntimeMapRequest()
        {
            if (_conn.SiteVersion < new Version(2, 6))
            {
                Assert.Ignore("Skipping test (TestCreateRuntimeMapRequest). MapGuide is older than 2.6");
                return;
            }

            int[] cmds = _conn.Capabilities.SupportedCommands;
            Assert.True(Array.IndexOf(cmds, (int)CommandType.CreateRuntimeMap) >= 0);

            //Barebones
            ICreateRuntimeMap create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            MapModel.IRuntimeMapInfo map = create.Execute();
            IDescribeRuntimeMap describe = (IDescribeRuntimeMap)_conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            MapModel.IRuntimeMapInfo rtInfo = describe.Execute();

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Barebones with tiled
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";

            map = create.Execute();
            describe = (IDescribeRuntimeMap)_conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            rtInfo = describe.Execute();

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count > 0);
            Assert.NotNull(rtInfo.FiniteDisplayScales);
            Assert.True(rtInfo.FiniteDisplayScales.Length > 0);

            //With Layer/Group structure
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            map = create.Execute();
            describe = (IDescribeRuntimeMap)_conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups);
            rtInfo = describe.Execute();

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.Null(layer.FeatureSource);
                Assert.True(layer.ScaleRanges.Count > 0);
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //With Layer/Group structure and inline icons
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            map = create.Execute();
            describe = (IDescribeRuntimeMap)_conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons);
            rtInfo = describe.Execute();

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNotNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.Null(layer.FeatureSource);
                Assert.True(layer.ScaleRanges.Count > 0);
                foreach (var sr in layer.ScaleRanges)
                {
                    Assert.NotNull(sr.FeatureStyle);
                    Assert.True(sr.FeatureStyle.Count > 0);
                    foreach (var feat in sr.FeatureStyle)
                    {
                        Assert.NotNull(feat.Rules);
                        Assert.True(feat.Rules.Count > 0);

                        foreach (var rule in feat.Rules)
                        {
                            Assert.IsNotNullOrEmpty(rule.IconBase64);
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Kitchen sink
            create = (ICreateRuntimeMap)_conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            
            map = create.Execute();
            describe = (IDescribeRuntimeMap)_conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);
            rtInfo = describe.Execute();

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.IsNotNullOrEmpty(rtInfo.IconMimeType);
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.NotNull(layer.FeatureSource);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.ClassName);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.Geometry);
                Assert.IsNotNullOrEmpty(layer.FeatureSource.ResourceID);
                Assert.True(layer.ScaleRanges.Count > 0);
                foreach (var sr in layer.ScaleRanges)
                {
                    Assert.NotNull(sr.FeatureStyle);
                    Assert.True(sr.FeatureStyle.Count > 0);
                    foreach (var feat in sr.FeatureStyle)
                    {
                        Assert.NotNull(feat.Rules);
                        Assert.True(feat.Rules.Count > 0);

                        foreach (var rule in feat.Rules)
                        {
                            Assert.IsNotNullOrEmpty(rule.IconBase64);
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);
        }
    }

    [TestFixture]
    public class LocalNativeRuntimeMapTests : RuntimeMapTests
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalNativeRuntimeMapTests)
                reason = "Skipping LocalNativeRuntimeMapTests because TestControl.IgnoreLocalNativeRuntimeMapTests = true";

            return TestControl.IgnoreLocalNativeRuntimeMapTests;
        }

        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalNativeConnection();
        }

        public override string TestPrefix
        {
            get { return "LocalNative"; }
        }

        [Test]
        public override void TestGroupAssignment()
        {
            base.TestGroupAssignment();
        }

        [Test]
        public override void TestExtentSerialization()
        {
            base.TestExtentSerialization();
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
        public override void TestLegendIconRendering()
        {
            base.TestLegendIconRendering();
        }

        [Test]
        public override void TestMapManipulation()
        {
            base.TestMapManipulation();
        }

        [Test]
        public override void TestMapManipulation2()
        {
            base.TestMapManipulation2();
        }

        [Test]
        public override void TestMapManipulation3()
        {
            base.TestMapManipulation3();
        }

        [Test]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }

        [Test]
        public override void TestMapAddDwfLayer()
        {
            base.TestMapAddDwfLayer();
        }
    }

    [TestFixture]
    public class LocalRuntimeMapTests : RuntimeMapTests
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalRuntimeMapTests)
                reason = "Skipping LocalRuntimeMapTests because TestControl.IgnoreLocalRuntimeMapTests = true";

            return TestControl.IgnoreLocalRuntimeMapTests;
        }

        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalConnection();
        }

        public override string TestPrefix
        {
            get { return "Local"; }
        }

        protected override bool CaresAboutRuntimeMapState
        {
            get
            {
                return false;
            }
        }

        [Test]
        public override void TestGroupAssignment()
        {
            base.TestGroupAssignment();
        }

        /*
        [Test]
        public override void TestExtentSerialization()
        {
            base.TestExtentSerialization();
        }*/

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
        public override void TestLegendIconRendering()
        {
            base.TestLegendIconRendering();
        }

        [Test]
        public override void TestMapManipulation()
        {
            base.TestMapManipulation();
        }

        [Test]
        public override void TestMapManipulation2()
        {
            base.TestMapManipulation2();
        }

        [Test]
        public override void TestMapManipulation3()
        {
            base.TestMapManipulation3();
        }

        /*
        [Test]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }*/
    }
}
