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

using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class ResourceTests
    {
        [Fact]
        public void TestCloning()
        {
            //Generated classes have built in Clone() methods. Verify they check out
            var app = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 2, 0));
            var app2 = app.Clone();
            Assert.NotEqual(app, app2);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            var fs2 = fs.Clone();
            Assert.NotEqual(fs, fs2);

            var ld = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            var ld2 = ld.Clone();
            Assert.NotEqual(ld, ld2);

            var md = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMap");
            var md2 = md.Clone();
            Assert.NotEqual(md, md2);

            var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");
            var wl2 = wl.Clone();
            Assert.NotEqual(wl, wl2);

            var sl = ObjectFactory.CreateSymbolLibrary();
            var sl2 = sl.Clone();
            Assert.NotEqual(sl, sl2);

            var ssd = ObjectFactory.CreateSimpleSymbol(new Version(1, 0, 0), "Test", "Test Symbol");
            var ssd2 = ssd.Clone();
            Assert.NotEqual(ssd, ssd2);

            var csd = ObjectFactory.CreateCompoundSymbol(new Version(1, 0, 0), "Test", "Test Symbol");
            var csd2 = csd.Clone();
            Assert.NotEqual(csd, csd2);

            var pl = ObjectFactory.CreatePrintLayout();
            var pl2 = pl.Clone();
            Assert.NotEqual(pl, pl2);
        }

        [Fact]
        public void TestValidResourceIdentifiers()
        {
            //Verify that only valid resource identifiers can be assigned to certain resource types.

            IResource res = ObjectFactory.CreateFeatureSource("OSGeo.SDF");

            #region Feature Source

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.FeatureSource";
            }
            catch (InvalidOperationException)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Feature Source

            res = ObjectFactory.CreateDrawingSource();

            #region Drawing Source

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.FeatureSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Drawing Source

            res = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test Map");

            #region Map Definition

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.MapDefinition";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Map Definition

            res = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");

            #region Web Layout

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.WebLayout";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Web Layout

            res = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 2, 0));

            #region Application Definition

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.ApplicationDefinition";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Application Definition

            res = ObjectFactory.CreateSimpleSymbol(new Version(1, 0, 0), "Test", "Test Symbol");

            #region Simple Symbol Definition

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Simple Symbol Definition

            res = ObjectFactory.CreateCompoundSymbol(new Version(1, 0, 0), "Test", "Test Symbol");

            #region Compound Symbol Definition

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Compound Symbol Definition

            res = ObjectFactory.CreateLoadProcedure(LoadType.Sdf, null);

            #region Load Procedure

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.LoadProcedure";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Load Procedure

            res = ObjectFactory.CreateLoadProcedure(LoadType.Shp, null);

            #region Load Procedure

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.LoadProcedure";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Load Procedure

            res = ObjectFactory.CreatePrintLayout();

            #region Print Layout

            try
            {
                res.ResourceID = "dklgjlahekjedjfd";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
                Assert.True(false, "Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.PrintLayout";
            }
            catch (Exception)
            {
                Assert.True(false, "Resource ID should've checked out");
            }

            #endregion Print Layout
        }

        [Fact]
        public void TestResourceTypeDescriptor()
        {
            var rtd = new ResourceTypeDescriptor(ResourceTypes.ApplicationDefinition.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "ApplicationDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.FeatureSource.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "FeatureSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "LayerDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.1.0");
            Assert.Equal(rtd.XsdName, "LayerDefinition-1.1.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "LoadProcedure-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "MapDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.PrintLayout.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "PrintLayout-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "SymbolDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolLibrary.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "SymbolLibrary-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "1.0.0");
            Assert.Equal(rtd.XsdName, "WebLayout-1.0.0.xsd");
        }
    }
}
