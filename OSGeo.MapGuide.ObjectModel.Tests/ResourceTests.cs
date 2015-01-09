#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void TestCloning()
        {
            //Generated classes have built in Clone() methods. Verify they check out
            var app = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 2, 0));
            var app2 = app.Clone();
            Assert.AreNotSame(app, app2);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            var fs2 = fs.Clone();
            Assert.AreNotSame(fs, fs2);

            var ld = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            var ld2 = ld.Clone();
            Assert.AreNotSame(ld, ld2);

            var md = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "TestMap");
            var md2 = md.Clone();
            Assert.AreNotSame(md, md2);

            var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");
            var wl2 = wl.Clone();
            Assert.AreNotSame(wl, wl2);

            var sl = ObjectFactory.CreateSymbolLibrary();
            var sl2 = sl.Clone();
            Assert.AreNotSame(sl, sl2);

            var ssd = ObjectFactory.CreateSimpleSymbol(new Version(1, 0, 0), "Test", "Test Symbol");
            var ssd2 = ssd.Clone();
            Assert.AreNotSame(ssd, ssd2);

            var csd = ObjectFactory.CreateCompoundSymbol(new Version(1, 0, 0), "Test", "Test Symbol");
            var csd2 = csd.Clone();
            Assert.AreNotSame(csd, csd2);

            var pl = ObjectFactory.CreatePrintLayout();
            var pl2 = pl.Clone();
            Assert.AreNotSame(pl, pl2);
        }

        [Test]
        public void TestValidResourceIdentifiers()
        {
            //Verify that only valid resource identifiers can be assigned to certain resource types.

            IResource res = ObjectFactory.CreateFeatureSource("OSGeo.SDF");

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

            #endregion Feature Source

            res = ObjectFactory.CreateDrawingSource();

            #region Drawing Source

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
                res.ResourceID = "Library://UnitTests/Test.FeatureSource";
                Assert.Fail("Should've thrown exception on invalid resource id");
            }
            catch (InvalidOperationException) { }

            try
            {
                res.ResourceID = "Library://UnitTests/Test.DrawingSource";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Drawing Source

            res = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test Map");

            #region Map Definition

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
                res.ResourceID = "Library://UnitTests/Test.MapDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Map Definition

            res = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), "Library://Test.MapDefinition");

            #region Web Layout

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
                res.ResourceID = "Library://UnitTests/Test.WebLayout";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Web Layout

            res = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 2, 0));

            #region Application Definition

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
                res.ResourceID = "Library://UnitTests/Test.ApplicationDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Application Definition

            res = ObjectFactory.CreateSimpleSymbol(new Version(1, 0, 0), "Test", "Test Symbol");

            #region Simple Symbol Definition

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
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Simple Symbol Definition

            res = ObjectFactory.CreateCompoundSymbol(new Version(1, 0, 0), "Test", "Test Symbol");

            #region Compound Symbol Definition

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
                res.ResourceID = "Library://UnitTests/Test.SymbolDefinition";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Compound Symbol Definition

            res = ObjectFactory.CreateLoadProcedure(LoadType.Sdf, null);

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

            #endregion Load Procedure

            res = ObjectFactory.CreateLoadProcedure(LoadType.Shp, null);

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

            #endregion Load Procedure

            res = ObjectFactory.CreatePrintLayout();

            #region Print Layout

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
                res.ResourceID = "Library://UnitTests/Test.PrintLayout";
            }
            catch (Exception)
            {
                Assert.Fail("Resource ID should've checked out");
            }

            #endregion Print Layout
        }

        [Test]
        public void TestResourceTypeDescriptor()
        {
            var rtd = new ResourceTypeDescriptor(ResourceTypes.ApplicationDefinition.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "ApplicationDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.FeatureSource.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "FeatureSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "LayerDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LayerDefinition.ToString(), "1.1.0");
            Assert.AreEqual(rtd.XsdName, "LayerDefinition-1.1.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.LoadProcedure.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "LoadProcedure-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.MapDefinition.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "MapDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.DrawingSource.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "DrawingSource-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.PrintLayout.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "PrintLayout-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolDefinition.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "SymbolDefinition-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.SymbolLibrary.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "SymbolLibrary-1.0.0.xsd");

            rtd = new ResourceTypeDescriptor(ResourceTypes.WebLayout.ToString(), "1.0.0");
            Assert.AreEqual(rtd.XsdName, "WebLayout-1.0.0.xsd");
        }
    }
}
