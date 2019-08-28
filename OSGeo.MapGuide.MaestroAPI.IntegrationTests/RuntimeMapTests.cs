#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Xunit;
using MapModel = OSGeo.MapGuide.ObjectModels.RuntimeMap;

namespace MaestroAPITests
{
    public abstract class RuntimeMapFixture : IDisposable
    {
        public bool Skip { get; }

        public string SkipReason { get; }

        public RuntimeMapFixture()
        {
            string reason;
            if (this.ShouldIgnore(out reason))
            {
                this.Skip = true;
                this.SkipReason = reason;
            }
            else
            {
                this.Skip = false;
                this.SkipReason = string.Empty;
                SetupTestData();
            }
        }

        protected virtual bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            return false;
        }

        private void SetupTestData()
        {
            TestEnvironment.SetupTestData(this.Provider, () => this.CreateTestConnection());
        }

        public void Dispose()
        {
        }

        public abstract string Provider { get; }

        public abstract IServerConnection CreateTestConnection();
    }

    public abstract class RuntimeMapTests<T> : IClassFixture<T> 
        where T : RuntimeMapFixture
    {
        protected T _fixture;

        public RuntimeMapTests(T fixture)
        {
            _fixture = fixture;
        }

        public virtual void TestGroupAssignment()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);
            var gdf1 = mdf.AddGroup("Group1");
            var gdf2 = mdf.AddGroup("Group2");
            gdf2.Group = gdf1.Name;
            resSvc.SaveResourceAs(mdf, "Session:" + conn.SessionID + "//TestGroupAssignment.MapDefinition");
            mdf.ResourceID = "Session:" + conn.SessionID + "//TestGroupAssignment.MapDefinition";

            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + conn.SessionID + "//TestGroupAssignment.Map", mdf, 1.0);
            foreach (var grp in mdf.MapLayerGroup)
            {
                var rtGrp = mapSvc.CreateMapGroup(map, grp);
                Assert.Equal(rtGrp.Group, grp.Group);
            }
        }

        public virtual void TestExtentSerialization()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);
            mdf.Extents = ObjectFactory.CreateEnvelope(1.0, 2.0, 3.0, 4.0);

            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + conn.SessionID + "//TestExtentSerialization.Map", mdf, 1.0);
            map.Save();

            var map2 = mapSvc.OpenMap("Session:" + conn.SessionID + "//TestExtentSerialization.Map");
            Assert.Equal(1.0, map2.DataExtent.MinX);
            Assert.Equal(2.0, map2.DataExtent.MinY);
            Assert.Equal(3.0, map2.DataExtent.MaxX);
            Assert.Equal(4.0, map2.DataExtent.MaxY);
        }

        public virtual void TestCreate()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Create a runtime map from its map definition, verify layer/group
            //structure is the same

            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var map = mapSvc.CreateMap("Session:" + conn.SessionID + "//TestCreate1.Map", mdf, 1.0);
            var map2 = mapSvc.CreateMap("Session:" + conn.SessionID + "//TestCreate2.Map", "Library://UnitTests/Maps/Sheboygan.MapDefinition", 1.0);
            //var map3 = mapSvc.CreateMap("Library://UnitTests/Maps/Sheboygan.MapDefinition", 1.0);

            Assert.NotNull(map.Layers["Parcels"]);
            var parcelsLayers = map.Layers["Parcels"];
            Assert.Single(parcelsLayers.ScaleRanges);
            Assert.Equal(0.0, parcelsLayers.ScaleRanges[0].MinScale);
            Assert.Equal(13000.0, parcelsLayers.ScaleRanges[0].MaxScale);

            Assert.NotNull(map);
            Assert.NotNull(map2);
            //Assert.NotNull(map3);

            //Check resource ids
            //Assert.True(!string.IsNullOrEmpty(map3.ResourceID));
            Assert.Equal("Session:" + conn.SessionID + "//TestCreate1.Map", map.ResourceID);
            Assert.Equal("Session:" + conn.SessionID + "//TestCreate2.Map", map2.ResourceID);

            //Check layer/group structure
            Assert.True(Matches(map, mdf));
            Assert.True(Matches(map2, mdf));
            //Assert.True(Matches(map3, mdf));
        }

        protected bool Matches(RuntimeMap map, IMapDefinition mdf)
        {
            if (map.MapDefinition != mdf.ResourceID) return false;
            if (map.Groups.Count != mdf.GetGroupCount()) return false;
            if (map.Layers.Count != mdf.GetDynamicLayerCount()) return false;

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
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Create a runtime map from its map definition, modify some layers
            //and groups. Save it, open another instance and verify the changes
            //have stuck
            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
            Assert.NotNull(mapSvc);

            var mdf = resSvc.GetResource("Library://UnitTests/Maps/Sheboygan.MapDefinition") as IMapDefinition;
            Assert.NotNull(mdf);

            var mid = "Session:" + conn.SessionID + "//TestSave.Map";
            var map = mapSvc.CreateMap(mid, mdf, 1.0);
            //Doesn't exist yet because save isn't called
            if (CaresAboutRuntimeMapState) Assert.True(!resSvc.ResourceExists(mid));

            //Call save
            Assert.True(Matches(map, mdf));
            Assert.False(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);
            Assert.True(Matches(map, mdf));
            if (CaresAboutRuntimeMapState) Assert.True(resSvc.ResourceExists(mid));

            //Tests below not applicable if test suite doesn't care about runtime state
            if (!CaresAboutRuntimeMapState)
                return;

            //Open second runtime map instance
            var map2 = mapSvc.OpenMap(mid);
            Assert.False(map == map2);
            Assert.True(Matches(map2, mdf));

            //Tweak some settings
            var parcels = map2.Layers["Parcels"];
            //Check these values still came through
            Assert.Single(parcels.ScaleRanges);
            Assert.Equal(0.0, parcels.ScaleRanges[0].MinScale);
            Assert.Equal(13000.0, parcels.ScaleRanges[0].MaxScale);

            var rail = map2.Layers["Rail"];
            Assert.NotNull(parcels);

            parcels.Visible = false;
            parcels.Selectable = false;

            Assert.NotNull(rail);

            rail.Selectable = false;

            //Save
            Assert.False(Matches(map2, mdf));
            Assert.True(map2.IsDirty);
            map2.Save();
            Assert.False(map2.IsDirty);
            Assert.False(Matches(map2, mdf));

            var map3 = mapSvc.OpenMap(mid);
            Assert.False(Matches(map3, mdf));

            parcels = null;
            rail = null;

            parcels = map3.Layers["Parcels"];
            rail = map3.Layers["Rail"];
            Assert.NotNull(parcels);

            Assert.False(parcels.Visible);
            Assert.False(parcels.Selectable);

            Assert.NotNull(rail);

            Assert.False(rail.Selectable);
        }

        public string TestPrefix => _fixture.Provider;

        protected virtual bool CaresAboutRuntimeMapState => true;

        public virtual void TestRender75k()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Render a map of sheboygan at 75k
            //Only programmatically verify the returned stream can be fed to a
            //System.Drawing.Image object.

            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestRender75k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            Assert.False(map.IsDirty);
            map.ViewScale = 75000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            if (CaresAboutRuntimeMapState) Assert.True(!resSvc.ResourceExists(mid));
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

            //Render default
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender75k.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender75kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay75k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay75kConvenience.png", "PNG");

            //Turn off parcels
            var rail = map.Layers["Rail"];
            Assert.NotNull(rail);
            rail.Visible = false;
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
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
                    /*
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(ms))
                        { }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }
                    */
                }
            }
        }

        #endregion render helpers

        public virtual void TestLegendIconRendering()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestLegendIconRendering.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.True(!resSvc.ResourceExists(mid));
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

            int counter = 0;
            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImageStream(map.ViewScale, layer.LayerDefinitionID, -1, -1);
                using (var fw = File.OpenWrite(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.png"))
                {
                    icon.CopyTo(fw);
                }
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImageStream(map.ViewScale, layer.LayerDefinitionID, -1, -1, 16, 16, "JPG");
                using (var fw = File.OpenWrite(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.jpg"))
                {
                    icon.CopyTo(fw);
                }
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImageStream(map.ViewScale, layer.LayerDefinitionID, -1, -1, 16, 16, "GIF");
                using (var fw = File.OpenWrite(TestPrefix + "TestLegendIconRendering_" + counter + "_16x16.gif"))
                {
                    icon.CopyTo(fw);
                }
                counter++;
            }

            foreach (var layer in map.Layers)
            {
                var icon = mapSvc.GetLegendImageStream(map.ViewScale, layer.LayerDefinitionID, -1, -1, 160, 50, "PNG");
                using (var fw = File.OpenWrite(TestPrefix + "TestLegendIconRendering_" + counter + "_160x50.png"))
                {
                    icon.CopyTo(fw);
                }
                counter++;
            }
        }

        public virtual void TestRender12k()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Render a map of sheboygan at 12k
            //Only programmatically verify the returned stream can be fed to a
            //System.Drawing.Image object.
            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestRender12k.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            Assert.False(map.IsDirty);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.True(!resSvc.ResourceExists(mid));
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

            //Render default
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender12k.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender12kConvenience.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay12k.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay12kConvenience.png", "PNG");

            //Turn off parcels
            var parcels = map.Layers["Parcels"];
            Assert.NotNull(parcels);
            parcels.Visible = false;
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

            //Render again
            RenderAndVerify(mapSvc, map, TestPrefix + "TestRender12k_ParcelsBackOn.png", "PNG");
            RenderAndVerifyConvenience(map, TestPrefix + "TestRender12kConvenience_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerify(mapSvc, map, TestPrefix + "TestRenderOverlay12k_ParcelsBackOn.png", "PNG");
            RenderDynamicOverlayAndVerifyConvenience(map, TestPrefix + "TestRenderOverlay12kConvenience_ParcelsBackOn.png", "PNG");
        }

        public virtual void TestMapManipulation()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Render a map of sheboygan at 12k
            //Verify that layer removal and addition produces the expected rendered result
            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
            Trace.TraceInformation("Using MPU of: {0}", metersPerUnit);

            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestMapManipulation.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            //Doesn't exist yet because save isn't called
            Assert.True(!resSvc.ResourceExists(mid));
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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
            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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

            Assert.True(map.IsDirty);
            map.Save();
            Assert.False(map.IsDirty);

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
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //This is mainly to exercise Insert() and to verify that draw orders come back as expected
            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
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
            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestMapManipulation2.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            Assert.Empty(map.Layers);
            Assert.Empty(map.Groups);

            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group1"));//new RuntimeMapGroup(map, "Group1"));
            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group2"));//new RuntimeMapGroup(map, "Group2"));
            Assert.Equal(2, map.Groups.Count);

            Assert.NotNull(map.Groups["Group1"]);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Groups["Group3"]);

            var layer = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"));
            layer.Group = "Group1";

            map.Layers.Insert(0, layer);
            Assert.Single(map.Layers);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));

            var layer2 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Parcels.LayerDefinition"));
            map.Layers.Insert(0, layer2);
            layer2.Group = "Group1"; //Intentional

            Assert.Equal(2, map.Layers.Count);
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
            Assert.Equal(3, map.Layers.Count);
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

            Assert.Equal(2, map.GetLayersOfGroup("Group1").Length);
            Assert.Single(map.GetLayersOfGroup("Group2"));

            //Group1 has 2 layers
            map.Groups.Remove("Group1");
            Assert.Single(map.Layers);
            Assert.Null(map.Groups["Group1"]);
            Assert.True(map.Groups["Group2"] == map.Groups[0]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            //Removing layer doesn't affect its group. It will still be there
            map.Layers.Remove(layer3.Name);
            Assert.Empty(map.Layers);
            Assert.Single(map.Groups);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer3.ObjectId));

            map.Groups.Remove("Group2");
            Assert.Empty(map.Layers);
            Assert.Empty(map.Groups);
        }

        public virtual void TestMapManipulation3()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            //Pretty much the same as TestMapManipulation2() but with Add() instead of Insert()
            //Add() puts the item at the end of the collection, so draw orders should reflect
            //that
            var conn = _fixture.CreateTestConnection();
            var resSvc = conn.ResourceService;
            var mapSvc = conn.GetService((int)ServiceType.Mapping) as IMappingService;
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
            var cs = conn.CoordinateSystemCatalog.CreateCoordinateSystem(mdf.CoordinateSystem);
            metersPerUnit = cs.MetersPerUnit;
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
            var mid = "Session:" + conn.SessionID + "//" + TestPrefix + "TestMapManipulation2.Map";
            var map = mapSvc.CreateMap(mid, mdf, metersPerUnit);
            map.ViewScale = 12000;
            map.DisplayWidth = 1024;
            map.DisplayHeight = 1024;
            map.DisplayDpi = 96;

            Assert.Empty(map.Layers);
            Assert.Empty(map.Groups);

            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group1"));//new RuntimeMapGroup(map, "Group1"));
            map.Groups.Add(mapSvc.CreateMapGroup(map, "Group2"));//new RuntimeMapGroup(map, "Group2"));
            Assert.Equal(2, map.Groups.Count);

            Assert.NotNull(map.Groups["Group1"]);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Groups["Group3"]);

            var layer = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition"));
            layer.Group = "Group1";

            map.Layers.Add(layer);
            Assert.Single(map.Layers);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.True(map.Layers["HydrographicPolygons"] == map.Layers[0]);
            Assert.NotNull(map.Layers.GetByObjectId(layer.ObjectId));

            var layer2 = mapSvc.CreateMapLayer(map, (ILayerDefinition)resSvc.GetResource("Library://UnitTests/Layers/Parcels.LayerDefinition"));
            map.Layers.Add(layer2);
            layer2.Group = "Group1"; //Intentional

            Assert.Equal(2, map.Layers.Count);
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
            Assert.Equal(3, map.Layers.Count);
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

            Assert.Equal(2, map.GetLayersOfGroup("Group1").Length);
            Assert.Single(map.GetLayersOfGroup("Group2"));

            //Group1 has 2 layers
            map.Groups.Remove("Group1");
            Assert.Single(map.Layers);
            Assert.Null(map.Groups["Group1"]);
            Assert.True(map.Groups["Group2"] == map.Groups[0]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.NotNull(map.Layers.GetByObjectId(layer3.ObjectId));

            //Removing layer doesn't affect its group. It will still be there
            map.Layers.Remove(layer3.Name);
            Assert.Empty(map.Layers);
            Assert.Single(map.Groups);
            Assert.NotNull(map.Groups["Group2"]);
            Assert.Null(map.Layers.GetByObjectId(layer.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer2.ObjectId));
            Assert.Null(map.Layers.GetByObjectId(layer3.ObjectId));

            map.Groups.Remove("Group2");
            Assert.Empty(map.Layers);
            Assert.Empty(map.Groups);
        }

        public virtual void TestMapManipulation4()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string mapdefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            ResourceIdentifier rtmX = new ResourceIdentifier(mapdefinition);
            string rtmDef = "Library://UnitTests/Cache/" + rtmX.Fullpath.Replace(":", "_").Replace("/", "_");
            string mapName = rtmX.Name;
            ResourceIdentifier mapid = new ResourceIdentifier(mapName, ResourceTypes.Map, conn.SessionID);
            IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
            RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map
            Assert.False(rtm.IsDirty);
            rtm.Save();
            Assert.False(rtm.IsDirty);
            RuntimeMap tmprtm = mapSvc.CreateMap(mapid, mdef); // Create new map in data cache
            Assert.False(tmprtm.IsDirty);
            tmprtm.Save();
            Assert.False(tmprtm.IsDirty);

            RuntimeMap mymap = mapSvc.OpenMap(mapid);
        }

        public virtual void TestMapManipulation5()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            string mapdefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";
            IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
            RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map
            rtm.Save();
            Assert.False(rtm.IsDirty);
            RuntimeMap mymap = mapSvc.OpenMap("Session:" + conn.SessionID + "//" + rtm.Name + ".Map");
        }

        public virtual void TestMapAddDwfLayer()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
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
            var ldf = Utility.CreateDefaultLayer(conn, LayerType.Drawing);
            var dl = (IDrawingLayerDefinition)ldf.SubLayer;
            dl.ResourceId = ds.ResourceID;
            dl.Sheet = sheetName;
            ldf.ResourceID = "Session:" + conn.SessionID + "//TestDrawing.LayerDefinition";
            conn.ResourceService.SaveResource(ldf);

            //Add our dwf layer
            var rtLayer = mapSvc.CreateMapLayer(rtm, ldf);
            rtm.Layers.Add(rtLayer);

            rtm.Save();
            Assert.False(rtm.IsDirty);
            RuntimeMap mymap = mapSvc.OpenMap("Session:" + conn.SessionID + "//" + rtm.Name + ".Map");
        }

        public virtual void TestResourceEvents()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            bool deleteCalled = false;
            bool updateCalled = false;
            bool insertCalled = false;

            var conn = _fixture.CreateTestConnection();
            conn.ResourceService.ResourceAdded += (s, e) => { insertCalled = true; };
            conn.ResourceService.ResourceDeleted += (s, e) => { deleteCalled = true; };
            conn.ResourceService.ResourceUpdated += (s, e) => { updateCalled = true; };

            //This should raise ResourceAdded
            conn.ResourceService.SetResourceXmlData("Library://UnitTests/ResourceEvents/Test.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));

            //This should raise ResourceUpdated
            conn.ResourceService.SetResourceXmlData("Library://UnitTests/ResourceEvents/Test.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));

            //This should raise ResourceDeleted
            conn.ResourceService.DeleteResource("Library://UnitTests/ResourceEvents/Test.LayerDefinition");

            Assert.True(deleteCalled);
            Assert.True(updateCalled);
            Assert.True(insertCalled);
        }

        public virtual void TestCaseDuplicateLayerIds()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);
            try
            {
                var conn = _fixture.CreateTestConnection();
                IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                string mapdefinition = "Library://UnitTests/Maps/DuplicateLayerIds.MapDefinition";
                IMapDefinition mdef = (IMapDefinition)conn.ResourceService.GetResource(mapdefinition);
                RuntimeMap rtm = mapSvc.CreateMap(mdef); // Create new runtime map
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        public virtual void TestLargeMapCreatePerformance()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            TestMapCreate(50, 10);
            TestMapCreate(100, 25);
            TestMapCreate(200, 50);
        }

        private void TestMapCreate(int layerSize, int groupSize)
        {
            //Create a 200 layer, 50 group map. This is not part of the benchmark
            var conn = _fixture.CreateTestConnection();
            var mdf = Utility.CreateMapDefinition(conn, "LargeMap");
            mdf.ResourceID = "Library://UnitTests/LargeMapTest/LargeMap.MapDefinition";
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
                conn.ResourceService.SetResourceXmlData(lid, ms);
                mdf.AddLayer(groupName, layerName, lid);
            }

            Assert.True(Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0);
            var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            var mid = "Session:" + conn.SessionID + "//TestLargeMapCreatePerformance.Map";

            //Begin Benchmark
            var sw = new Stopwatch();
            sw.Start();
            var map = mapSvc.CreateMap(mid, mdf, 1.0);
            sw.Stop();

            string msg = "Create Map time for " + layerSize + " layer, " + groupSize + " group map: " + sw.ElapsedMilliseconds + "ms";
            Trace.WriteLine(msg);
        }
    }

    public class HttpRuntimeMapFixture : RuntimeMapFixture
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreHttpRuntimeMapTests)
                reason = "Skipping HttpRuntimeMapTests because TestControl.IgnoreHttpRuntimeMapTests = true";

            return TestControl.IgnoreHttpRuntimeMapTests;
        }

        public override string Provider => "Http";

        public override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestHttpConnection();
        }
    }

    public class HttpRuntimeMapTests : RuntimeMapTests<HttpRuntimeMapFixture>
    {
        public HttpRuntimeMapTests(HttpRuntimeMapFixture fixture) 
            : base(fixture)
        {
        }

        [SkippableFact]
        public override void TestGroupAssignment()
        {
            base.TestGroupAssignment();
        }

        [SkippableFact]
        public override void TestExtentSerialization()
        {
            base.TestExtentSerialization();
        }

        [SkippableFact]
        public override void TestResourceEvents()
        {
            base.TestResourceEvents();
        }

        [SkippableFact]
        public override void TestCreate()
        {
            base.TestCreate();
        }

        [SkippableFact]
        public override void TestSave()
        {
            base.TestSave();
        }

        [SkippableFact]
        public override void TestRender75k()
        {
            base.TestRender75k();
        }

        [SkippableFact]
        public override void TestRender12k()
        {
            base.TestRender12k();
        }

        [SkippableFact]
        public override void TestLegendIconRendering()
        {
            base.TestLegendIconRendering();
        }

        [SkippableFact]
        public override void TestMapManipulation()
        {
            base.TestMapManipulation();
        }

        [SkippableFact]
        public override void TestMapManipulation2()
        {
            base.TestMapManipulation2();
        }

        [SkippableFact]
        public override void TestMapManipulation3()
        {
            base.TestMapManipulation3();
        }

        [SkippableFact]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }

        [SkippableFact]
        public override void TestMapManipulation4()
        {
            base.TestMapManipulation4();
        }

        [SkippableFact]
        public override void TestMapManipulation5()
        {
            base.TestMapManipulation5();
        }

        [SkippableFact]
        public override void TestMapAddDwfLayer()
        {
            base.TestMapAddDwfLayer();
        }

        [SkippableFact]
        public override void TestCaseDuplicateLayerIds()
        {
            base.TestCaseDuplicateLayerIds();
        }

        [SkippableFact]
        public void TestCreateRuntimeMapRequest()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);

            var conn = _fixture.CreateTestConnection();
            Skip.If(conn.SiteVersion < new Version(2, 6), "Skipping test (TestCreateRuntimeMapRequest). MapGuide is older than 2.6");

            int[] cmds = conn.Capabilities.SupportedCommands;
            Assert.True(Array.IndexOf(cmds, (int)CommandType.CreateRuntimeMap) >= 0);

            //Barebones
            ICreateRuntimeMap create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            MapModel.IRuntimeMapInfo rtInfo = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }

            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Barebones with tiled
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";

            rtInfo = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count > 0);
            Assert.NotNull(rtInfo.FiniteDisplayScales);
            Assert.True(rtInfo.FiniteDisplayScales.Length > 0);

            //With Layer/Group structure
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups);

            rtInfo = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
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
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons);

            rtInfo = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.False(String.IsNullOrEmpty(rtInfo.IconMimeType));
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
                            Assert.False(String.IsNullOrEmpty(rule.IconBase64));
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Kitchen sink
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";
            create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);

            rtInfo = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.False(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.NotNull(layer.FeatureSource);
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.ClassName));
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.Geometry));
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.ResourceID));
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
                            Assert.False(String.IsNullOrEmpty(rule.IconBase64));
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            if (conn.SiteVersion >= new Version(3, 0))
            {
                create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
                create.MapDefinition = "Library://UnitTests/Maps/SheboyganLinked.MapDefinition";
                create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);

                rtInfo = create.Execute();
                {
                    var test = rtInfo as MapModel.IRuntimeMapInfo2;
                    Assert.NotNull(test);
                }
                MapModel.IRuntimeMapInfo2 rtInfo2 = (MapModel.IRuntimeMapInfo2)rtInfo;
                Assert.Equal("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", rtInfo2.TileSetDefinition);
                Assert.True(rtInfo2.TileWidth.HasValue);
                Assert.True(rtInfo2.TileHeight.HasValue);
            }
        }

        [SkippableFact]
        public void TestDescribeRuntimeMapRequest()
        {
            Skip.If(_fixture.Skip, _fixture.SkipReason);
            var conn = _fixture.CreateTestConnection();

            Skip.If(conn.SiteVersion < new Version(2, 6), "Skipping test (TestCreateRuntimeMapRequest). MapGuide is older than 2.6");
            
            int[] cmds = conn.Capabilities.SupportedCommands;
            Assert.True(Array.IndexOf(cmds, (int)CommandType.CreateRuntimeMap) >= 0);

            //Barebones
            ICreateRuntimeMap create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            MapModel.IRuntimeMapInfo map = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            IDescribeRuntimeMap describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            MapModel.IRuntimeMapInfo rtInfo = describe.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Barebones with tiled
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/SheboyganTiled.MapDefinition";

            map = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            rtInfo = describe.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count == 0);
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count > 0);
            Assert.NotNull(rtInfo.FiniteDisplayScales);
            Assert.True(rtInfo.FiniteDisplayScales.Length > 0);

            //With Layer/Group structure
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            map = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups);
            rtInfo = describe.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.True(String.IsNullOrEmpty(rtInfo.IconMimeType));
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
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            map = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons);
            rtInfo = describe.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.False(String.IsNullOrEmpty(rtInfo.IconMimeType));
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
                            Assert.False(String.IsNullOrEmpty(rule.IconBase64));
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            //Kitchen sink
            create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
            create.MapDefinition = "Library://UnitTests/Maps/Sheboygan.MapDefinition";

            map = create.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = map as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
            describe.Name = map.Name;
            describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);
            rtInfo = describe.Execute();
            if (conn.SiteVersion >= new Version(3, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo2;
                Assert.NotNull(test);
            }
            if (conn.SiteVersion >= new Version(4, 0))
            {
                var test = rtInfo as MapModel.IRuntimeMapInfo3;
                Assert.NotNull(test);
            }
            Assert.NotNull(rtInfo.CoordinateSystem);
            Assert.False(String.IsNullOrEmpty(rtInfo.IconMimeType));
            Assert.NotNull(rtInfo.Extents);
            Assert.NotNull(rtInfo.Layers);
            Assert.True(rtInfo.Layers.Count > 0);
            foreach (var layer in rtInfo.Layers)
            {
                Assert.NotNull(layer.FeatureSource);
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.ClassName));
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.Geometry));
                Assert.False(String.IsNullOrEmpty(layer.FeatureSource.ResourceID));
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
                            Assert.False(String.IsNullOrEmpty(rule.IconBase64));
                        }
                    }
                }
            }
            Assert.NotNull(rtInfo.Groups);
            Assert.True(rtInfo.Groups.Count == 0);

            if (conn.SiteVersion >= new Version(3, 0))
            {
                create = (ICreateRuntimeMap)conn.CreateCommand((int)CommandType.CreateRuntimeMap);
                create.MapDefinition = "Library://UnitTests/Maps/SheboyganLinked.MapDefinition";
                create.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);

                rtInfo = create.Execute();
                {
                    var test = rtInfo as MapModel.IRuntimeMapInfo2;
                    Assert.NotNull(test);
                }
                MapModel.IRuntimeMapInfo2 rtInfo2 = (MapModel.IRuntimeMapInfo2)rtInfo;
                Assert.Equal("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", rtInfo2.TileSetDefinition);
                Assert.True(rtInfo2.TileWidth.HasValue);
                Assert.True(rtInfo2.TileHeight.HasValue);

                describe = (IDescribeRuntimeMap)conn.CreateCommand((int)CommandType.DescribeRuntimeMap);
                describe.Name = rtInfo.Name;
                describe.RequestedFeatures = (int)(RuntimeMapRequestedFeatures.LayersAndGroups | RuntimeMapRequestedFeatures.Icons | RuntimeMapRequestedFeatures.FeatureSourceInformation);
                rtInfo = describe.Execute();
                {
                    var test = rtInfo as MapModel.IRuntimeMapInfo2;
                    Assert.NotNull(test);
                }
                rtInfo2 = (MapModel.IRuntimeMapInfo2)rtInfo;
                Assert.Equal("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", rtInfo2.TileSetDefinition);
                Assert.True(rtInfo2.TileWidth.HasValue);
                Assert.True(rtInfo2.TileHeight.HasValue);
            }
        }
    }
}