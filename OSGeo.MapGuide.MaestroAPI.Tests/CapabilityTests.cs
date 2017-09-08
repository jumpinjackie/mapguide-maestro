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
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Mapping.Tests;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class CapabilityTests
    {
        static IConnectionCapabilities CreateHttpCapabilities(IServerConnection conn)
        {
            var type = typeof(HttpServerConnection).Assembly.GetTypes().FirstOrDefault(t => t.Name == "HttpCapabilities");
            var ctor = type.GetInternalConstructor(new[] { typeof(IServerConnection) });
            return ctor.Invoke(new[] { conn }) as IConnectionCapabilities;
        }

        [Fact]
        public void TestHttpCapabilities100()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(1, 0));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type.ToString());
                                Assert.True(false, "MGOS 1.0.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex)
                            {
                                Assert.Equal(ex.ResourceType, ResourceTypes.ApplicationDefinition.ToString());
                            }
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type.ToString());
                                Assert.True(false, "MGOS 1.0.0 doesn't support advanced symbology!");
                            }
                            catch (UnsupportedResourceTypeException ex)
                            {
                                Assert.Equal(ex.ResourceType, ResourceTypes.SymbolDefinition.ToString());
                            }
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.ApplicationDefinition.ToString()), ResourceTypes.ApplicationDefinition.ToString());
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.SymbolDefinition.ToString()), ResourceTypes.SymbolDefinition.ToString());
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities110()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(1, 1));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type.ToString());
                                Assert.True(false, "MGOS 1.1.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex)
                            {
                                Assert.Equal(ex.ResourceType, ResourceTypes.ApplicationDefinition.ToString());
                            }
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type.ToString());
                                Assert.True(false, "MGOS 1.1.0 doesn't support advanced symbology!");
                            }
                            catch (UnsupportedResourceTypeException ex)
                            {
                                Assert.Equal(ex.ResourceType, ResourceTypes.SymbolDefinition.ToString());
                            }
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.ApplicationDefinition.ToString()), ResourceTypes.ApplicationDefinition.ToString());
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.SymbolDefinition.ToString()), ResourceTypes.SymbolDefinition.ToString());
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities120()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(1, 2));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            try
                            {
                                caps.GetMaxSupportedResourceVersion(type.ToString());
                                Assert.True(false, "MGOS 1.2.0 doesn't support fusion!");
                            }
                            catch (UnsupportedResourceTypeException ex)
                            {
                                Assert.Equal(ex.ResourceType, ResourceTypes.ApplicationDefinition.ToString());
                            }
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) < 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.ApplicationDefinition.ToString()), ResourceTypes.ApplicationDefinition.ToString());
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities200()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 0));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 2, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities210()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 1));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 3, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities220()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 2));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 3, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 2, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
            Assert.False(caps.IsSupportedResourceType(ResourceTypes.WatermarkDefinition.ToString()), ResourceTypes.WatermarkDefinition.ToString());
        }

        [Fact]
        public void TestHttpCapabilities230()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 3));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 3, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 2, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 3, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;

                    case ResourceTypes.WatermarkDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 3, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 1, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
        }

        [Fact]
        public void TestHttpCapabilities240()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 4));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 2, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WatermarkDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
        }

        [Fact]
        public void TestHttpCapabilities250()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 5));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 2, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WatermarkDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
        }

        [Fact]
        public void TestHttpCapabilities260()
        {
            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.SiteVersion).Returns(new Version(2, 6));
            var caps = CreateHttpCapabilities(conn.Object);

            foreach (ResourceTypes type in Enum.GetValues(typeof(ResourceTypes)))
            {
                switch (type)
                {
                    case ResourceTypes.ApplicationDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.DrawingSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.FeatureSource:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.LayerDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.LoadProcedure:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 2, 0));
                        }
                        break;

                    case ResourceTypes.MapDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.PrintLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolLibrary:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(1, 0, 0));
                        }
                        break;

                    case ResourceTypes.SymbolDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WatermarkDefinition:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 4, 0));
                        }
                        break;

                    case ResourceTypes.WebLayout:
                        {
                            var version = caps.GetMaxSupportedResourceVersion(type.ToString());
                            Assert.Equal(version, new Version(2, 6, 0));
                        }
                        break;
                }
            }
            Assert.True(caps.SupportsResourcePreviews);
            int[] services = caps.SupportedServices;
            foreach (ServiceType st in Enum.GetValues(typeof(ServiceType)))
            {
                switch (st)
                {
                    case ServiceType.Drawing:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Feature:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Fusion:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Mapping:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Resource:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;

                    case ServiceType.Tile:
                        {
                            Assert.True(Array.IndexOf<int>(services, (int)st) >= 0);
                        }
                        break;
                }
            }

            foreach (string rt in caps.SupportedResourceTypes)
            {
                Assert.True(caps.IsSupportedResourceType(rt), rt);
            }
        }
    }
}
