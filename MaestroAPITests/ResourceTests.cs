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

namespace MaestroAPITests
{
    [TestFixture]
    public class ResourceTests
    {
        private Mockery _mocks;

        [Test]
        public void TestCloning()
        {
            //Generated classes have built in Clone() methods. Verify they check out
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();

            var app = ObjectFactory.DeserializeEmbeddedFlexLayout();
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

            var ssd = new SimpleSymbolDefinition();
            var ssd2 = ssd.Clone();
            Assert.AreNotSame(ssd, ssd2);

            var csd = new CompoundSymbolDefinition();
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

            res = ObjectFactory.DeserializeEmbeddedFlexLayout();
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

            res = ObjectFactory.CreateSimpleSymbol(conn);
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

            res = ObjectFactory.CreateCompoundSymbol(conn);
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
        public void TestNoConversionOnIdenticalVersion()
        {
            //Verify origial reference is returned if we're converting a resource to the same version
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();

            var conv = new ResourceConverter(new List<IResourceConverter>());
            var targetVer = new Version(1, 0, 0);

            var app = ObjectFactory.DeserializeEmbeddedFlexLayout();
            var app2 = conv.Upgrade(app, targetVer);
            Assert.AreSame(app, app2);

            var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF");
            var fs2 = conv.Upgrade(fs, targetVer);
            Assert.AreSame(fs, fs2);

            var ld = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector, new Version(1, 0, 0));
            var ld2 = conv.Upgrade(ld, targetVer);
            Assert.AreSame(ld, ld2);

            var md = ObjectFactory.CreateMapDefinition(conn, "Test Map");
            var md2 = conv.Upgrade(md, targetVer);
            Assert.AreSame(md, md2);

            var wl = ObjectFactory.CreateWebLayout(conn, new Version(1, 0, 0), "Library://Test.MapDefinition");
            var wl2 = conv.Upgrade(wl, targetVer);
            Assert.AreSame(wl, wl2);

            var ssd = ObjectFactory.CreateSimpleSymbol(conn);
            var ssd2 = conv.Upgrade(ssd, targetVer);
            Assert.AreSame(ssd, ssd2);

            var csd = ObjectFactory.CreateCompoundSymbol(conn);
            var csd2 = conv.Upgrade(csd, targetVer);
            Assert.AreSame(csd, csd2);

            var pl = ObjectFactory.CreatePrintLayout(conn);
            var pl2 = conv.Upgrade(pl, targetVer);
            Assert.AreSame(pl, pl2);
        }

        [Test]
        public void TestSingleUpgrade()
        {
            _mocks = new Mockery();

            var orig = _mocks.NewMock<IResource>();
            Stub.On(orig).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(orig).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(orig).Method("Clone").WithAnyArguments().Will(Return.Value(orig));

            var res2 = _mocks.NewMock<IResource>();
            Stub.On(res2).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(res2).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 1, 0)));

            var upg = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(upg).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(upg).Method("Convert").WithAnyArguments().Will(Return.Value(res2));

            var upgList = new List<IResourceConverter>();
            upgList.Add(upg);
            var conv = new ResourceConverter(upgList);

            var obj = conv.Upgrade(orig, new Version(1, 1, 0));
            Assert.AreEqual(obj.ResourceVersion, new Version(1, 1, 0));

            try
            {
                //No 1.0.0 -> 1.2.0 converter registered. Should fail.
                obj = conv.Upgrade(orig, new Version(1, 2, 0));
                Assert.Fail("An exception should've been thrown (no upgrade path)");
            }
            catch (ResourceConversionException)
            {

            }
        }
        
        [Test]
        [ExpectedException(typeof(ResourceConversionException))]
        public void TestClashingUpgraders()
        {
            _mocks = new Mockery();
            var upg = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(upg).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));

            var upg2 = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg2).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg2).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 3, 0)));
            Stub.On(upg2).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));

            var upgList = new List<IResourceConverter>();
            upgList.Add(upg);
            upgList.Add(upg2);
            var conv = new ResourceConverter(upgList);
        }

        [Test]
        public void TestIncrementalUpgrade()
        {
            //Verify a resource is upgraded to the correct version
            //when going through multiple upgraders

            _mocks = new Mockery();

            var orig = _mocks.NewMock<IResource>();
            Stub.On(orig).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(orig).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(orig).Method("Clone").WithAnyArguments().Will(Return.Value(orig));

            var orig11 = _mocks.NewMock<IResource>();
            Stub.On(orig11).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(orig11).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(orig11).Method("Clone").WithAnyArguments().Will(Return.Value(orig11));

            var orig12 = _mocks.NewMock<IResource>();
            Stub.On(orig12).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 2, 0)));
            Stub.On(orig12).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(orig12).Method("Clone").WithAnyArguments().Will(Return.Value(orig12));

            var upg = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(upg).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(upg).Method("Convert").WithAnyArguments().Will(Return.Value(orig11));

            var upg2 = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg2).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(upg2).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 2, 0)));
            Stub.On(upg2).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(upg2).Method("Convert").WithAnyArguments().Will(Return.Value(orig12));

            var upgList = new List<IResourceConverter>() { upg, upg2 };
            var conv = new ResourceConverter(upgList);

            var obj = conv.Upgrade(orig, new Version(1, 2, 0));
            Assert.AreEqual(obj.ResourceVersion, new Version(1, 2, 0));
        }

        [Test]
        [ExpectedException(typeof(ResourceConversionException))]
        public void TestBrokenUpgradePath()
        {
            //Verify exception thrown when there is a version gap
            //in the upgrade path

            _mocks = new Mockery();
            var upg = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 1, 0)));
            Stub.On(upg).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));

            var upg2 = _mocks.NewMock<IResourceConverter>();
            Stub.On(upg2).GetProperty("SourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(upg2).GetProperty("TargetVersion").Will(Return.Value(new Version(1, 3, 0)));
            Stub.On(upg2).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));

            var orig = _mocks.NewMock<IResource>();
            Stub.On(orig).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(orig).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(orig).Method("Clone").WithAnyArguments().Will(Return.Value(orig));

            var upgList = new List<IResourceConverter>();
            upgList.Add(upg);
            upgList.Add(upg2);
            var conv = new ResourceConverter(upgList);

            //There's no 1.1.0 -> 1.2.0 upgrader registered. This should fail.
            var obj = conv.Upgrade(orig, new Version(1, 3, 0));
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
                Console.WriteLine("Found command (" + cmdName.ToString() + "): " + found);
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
    }
}
