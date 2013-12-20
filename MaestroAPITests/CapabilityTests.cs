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
using NMock2;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Http;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace MaestroAPITests
{
    [TestFixture]
    public class CapabilityTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreCapabilityTests)
                Assert.Ignore("Skipping CapabilityTests because TestControl.IgnoreCapabilityTests = true");
        }

        private Mockery _mocks;

        [Test]
        public void TestHttpCapabilities100()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 0)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type);
                                Assert.Fail("MGOS 1.0.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex) 
                            {
                                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
                            }
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type);
                                Assert.Fail("MGOS 1.0.0 doesn't support advanced symbology!");
                            }
                            catch (UnsupportedResourceTypeException ex) 
                            {
                                Assert.AreEqual(ex.ResourceType, ResourceTypes.SymbolDefinition);
                            }
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                if (rt != ResourceTypes.ApplicationDefinition && rt != ResourceTypes.SymbolDefinition)
                    Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
                else
                    Assert.IsFalse(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities110()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 1)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type);
                                Assert.Fail("MGOS 1.1.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex) 
                            {
                                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
                            }
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type);
                                Assert.Fail("MGOS 1.1.0 doesn't support advanced symbology!");
                            }
                            catch (UnsupportedResourceTypeException ex) 
                            {
                                Assert.AreEqual(ex.ResourceType, ResourceTypes.SymbolDefinition);
                            }
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                if (rt != ResourceTypes.ApplicationDefinition && rt != ResourceTypes.SymbolDefinition)
                    Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
                else
                    Assert.IsFalse(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities120()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 2)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type);
                                Assert.Fail("MGOS 1.2.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex) 
                            {
                                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
                            }
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                if (rt != ResourceTypes.ApplicationDefinition)
                    Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
                else
                    Assert.IsFalse(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities200()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 0)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 2, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities210()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 1)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 3, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities220()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 2)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 3, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(2, 2, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }

        [Test]
        public void TestHttpCapabilities230()
        {
            _mocks = new Mockery();
            var conn = _mocks.NewMock<IServerConnection>();
            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 3)));

            var caps = new HttpCapabilities(conn);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(2, 3, 0));
                        }
                        break;
                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(2, 2, 0));
                        }
                        break;
                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(2, 3, 0));
                        }
                        break;
                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 0, 0));
                        }
                        break;
                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                    case ResourceTypes.WatermarkDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(2, 3, 0));
                        }
                        break;
                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type);
                            Assert.AreEqual(version, new Version(1, 1, 0));
                        }
                        break;
                }
            }
            Assert.IsTrue(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Feature:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Fusion:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Mapping:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Resource:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                    case ServiceType.Tile:
                        {
                            Assert.IsTrue(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (ResourceTypes rt in Enum.GetValues(typeof(ResourceTypes)))
            {
                Assert.IsTrue(caps.IsSupportedResourceType(rt), rt.ToString());
            }
        }
    }
}
