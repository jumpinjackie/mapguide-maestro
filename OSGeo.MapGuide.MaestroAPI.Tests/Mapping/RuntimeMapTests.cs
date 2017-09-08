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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Tests;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Mapping.Tests
{
    public static class ReflectionExtensions
    {
        public static ConstructorInfo GetInternalConstructor(this Type type, Type[] types)
        {
            return type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
        }
    }

    public class RuntimeMapTests
    {
        static RuntimeMap CreateMap(IServerConnection conn, IMapDefinition mdf, double val, bool b)
        {
            var ctor = typeof(RuntimeMap).GetInternalConstructor(new[] { typeof(IServerConnection), typeof(IMapDefinition), typeof(double), typeof(bool) });
            return ctor.Invoke(new object[] { conn, mdf, val, b }) as RuntimeMap;
        }

        static RuntimeMapLayer CreateLayer(RuntimeMap map, string name, string ldfId)
        {
            var ctor = typeof(RuntimeMapLayer).GetInternalConstructor(new[] { typeof(RuntimeMap) });
            var layer =  ctor.Invoke(new[] { map }) as RuntimeMapLayer;
            layer.Name = name;
            var pi = layer.GetType().GetProperty(nameof(layer.LayerDefinitionID));
            pi.SetValue(layer, ldfId);
            return layer;
        }

        static RuntimeMapGroup CreateGroup(RuntimeMap map, IBaseMapGroup grp)
        {
            var ctor = typeof(RuntimeMapGroup).GetInternalConstructor(new[] { typeof(RuntimeMap), typeof(IBaseMapGroup) });
            var group = ctor.Invoke(new object[] { map, grp }) as RuntimeMapGroup;
            return group;
        }

        static RuntimeMapGroup CreateGroup(RuntimeMap map, string name)
        {
            var ctor = typeof(RuntimeMapGroup).GetInternalConstructor(new[] { typeof(RuntimeMap), typeof(string) });
            var group = ctor.Invoke(new object[] { map, name }) as RuntimeMapGroup;
            return group;
        }

        private static RuntimeMap CreateTestMap()
        {
            var layers = new Dictionary<string, ILayerDefinition>();
            var mdf = (IMapDefinition)ObjectFactory.Deserialize(ResourceTypes.MapDefinition.ToString(), TestData.TestTiledMap.AsStream());
            mdf.ResourceID = "Library://UnitTest/Test.MapDefinition";
            foreach (var lyr in mdf.BaseMap.BaseMapLayerGroups.First().BaseMapLayer)
            {
                var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
                ldf.ResourceID = lyr.ResourceId;
                layers.Add(lyr.ResourceId, ldf);
            }
            var conn = new Mock<IServerConnection>();
            var mapSvc = new Mock<IMappingService>();
            var resSvc = new Mock<IResourceService>();
            var caps = new Mock<IConnectionCapabilities>();

            foreach (var kvp in layers)
            {
                resSvc.Setup(r => r.GetResource(kvp.Key)).Returns(kvp.Value);
            }
            resSvc.Setup(r => r.GetResource("Library://UnitTest/Test.MapDefinition")).Returns(mdf);

            foreach (var kvp in layers)
            {
                mapSvc.Setup(m => m.CreateMapLayer(It.IsAny<RuntimeMap>(), It.IsAny<IBaseMapLayer>()))
                      .Returns((RuntimeMap rtMap, IBaseMapLayer rl) => CreateLayer(rtMap, rl.Name, kvp.Key));
            }
            mapSvc.Setup(m => m.CreateMapGroup(It.IsAny<RuntimeMap>(), It.IsAny<IBaseMapGroup>()))
                  .Returns((RuntimeMap rtMap, IBaseMapGroup grp) => CreateGroup(rtMap, grp));
            mapSvc.Setup(m => m.CreateMapGroup(It.IsAny<RuntimeMap>(), It.IsAny<string>()))
                  .Returns((RuntimeMap rtMap, string name) => CreateGroup(rtMap, name));
            
            caps.Setup(c => c.SupportedServices).Returns(new int[] { (int)ServiceType.Mapping });
            caps.Setup(c => c.SupportedCommands).Returns(new int[0]);
            caps.Setup(c => c.GetMaxSupportedResourceVersion(It.IsAny<string>())).Returns(new Version(1, 0, 0));
            
            conn.Setup(c => c.Capabilities).Returns(caps.Object);
            conn.Setup(c => c.ResourceService).Returns(resSvc.Object);
            conn.Setup(c => c.GetService((int)ServiceType.Mapping)).Returns(mapSvc.Object);

            var map = CreateMap(conn.Object, mdf, 1.0, true);
            Assert.Equal(15, map.FiniteDisplayScaleCount);
            Assert.NotNull(map.Layers);
            Assert.NotNull(map.Groups);
            return map;
        }

        [Fact]
        public void UnsupportedConnectionTest()
        {
            var res = (IMapDefinition)ObjectFactory.Deserialize(ResourceTypes.MapDefinition.ToString(), TestData.TestTiledMap.AsStream());
            var conn = new Mock<IServerConnection>();
            var caps = new Mock<IConnectionCapabilities>();
            caps.Setup(c => c.SupportedServices).Returns(new int[0]);
            caps.Setup(c => c.SupportedCommands).Returns(new int[0]);
            conn.Setup(c => c.Capabilities).Returns(caps.Object);

            Assert.Throws<NotSupportedException>(() =>
            {
                try
                {
                    CreateMap(conn.Object, res, 1.0, true);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            });
        }

        [Fact]
        public void GetFiniteDisplayScaleAtTest()
        {
            var map = CreateTestMap();
            Assert.Throws<IndexOutOfRangeException>(() => map.GetFiniteDisplayScaleAt(-1));
            Assert.Throws<IndexOutOfRangeException>(() => map.GetFiniteDisplayScaleAt(16));
            for (int i = 0; i < map.FiniteDisplayScaleCount; i++)
            {
                Assert.True(map.GetFiniteDisplayScaleAt(i) > 0);
            }
        }

        [Fact]
        public void SetViewCenterTest()
        {
            var map = CreateTestMap();
            map.SetViewCenter(-37, 23);
            Assert.Equal(-37, map.ViewCenter.X);
            Assert.Equal(23, map.ViewCenter.Y);
        }

        [Fact]
        public void GetGroupByNameTest()
        {
            var map = CreateTestMap();
            Assert.NotNull(map.Groups["Base Layer Group"]);
            Assert.Null(map.Groups["asjgsfdsf"]);
        }

        [Fact]
        public void GetLayerByObjectIdTest()
        {
            var map = CreateTestMap();
            var lyr = map.Layers["Rail"];
            var lyr2 = map.GetLayerByObjectId(lyr.ObjectId);
            Assert.NotNull(lyr2);
            Assert.Equal(lyr, lyr2);
            Assert.Null(map.GetLayerByObjectId("asjgsfdsf"));
        }

        [Fact]
        public void InsertLayerTest()
        {
            var map = CreateTestMap();
            //Mock setup in CreateTestMap() will ensure we get a service that does
            //the things we require
            var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
            var layer = new Mock<IBaseMapLayer>();
            layer.Setup(l => l.Name).Returns("Test");
            layer.Setup(l => l.ResourceId).Returns("Library://Test/Test.LayerDefinition");
            var lyr = mapSvc.CreateMapLayer(map, layer.Object);
            int count = map.Layers.Count;
            map.Layers.Insert(0, lyr);
            Assert.Equal(count + 1, map.Layers.Count);
            var lyr2 = map.Layers[0];
            Assert.Equal(lyr, lyr2);
        }

        [Fact]
        public void SetLayerIndexTest()
        {
            var map = CreateTestMap();
            //Mock setup in CreateTestMap() will ensure we get a service that does
            //the things we require
            var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
            var layer = new Mock<IBaseMapLayer>();
            layer.Setup(l => l.Name).Returns("Test");
            layer.Setup(l => l.ResourceId).Returns("Library://Test/Test.LayerDefinition");
            var lyr = mapSvc.CreateMapLayer(map, layer.Object);
            int count = map.Layers.Count;
            int idx = map.Layers.Count - 1;
            map.Layers[idx] = lyr;
            Assert.Equal(count, map.Layers.Count);
            var lyr2 = map.Layers[idx];
            Assert.Equal(lyr, lyr2);
        }

        [Fact]
        public void RemoveLayerAtTest()
        {
            var map = CreateTestMap();
            
            Assert.Throws<ArgumentOutOfRangeException>(() => map.Layers.RemoveAt(5));
            int count = map.Layers.Count;
            var lyr = map.Layers[0];
            map.Layers.RemoveAt(0);
            Assert.Equal(count - 1, map.Layers.Count);
            Assert.Null(map.Layers[lyr.Name]);
        }

        [Fact]
        public void IndexOfLayerTest()
        {
            var map = CreateTestMap();
            Assert.True(map.IndexOfLayer("Rail") >= 0);
            Assert.True(map.IndexOfLayer("Parcels") >= 0);
            Assert.True(map.IndexOfLayer("HydrographicPolygons") >= 0);
            Assert.True(map.IndexOfLayer("sdjgdsfasdf") < 0);
        }

        [Fact]
        public void RemoveLayerTest()
        {
            var map = CreateTestMap();
            Assert.Equal(3, map.Layers.Count);
            map.Layers.Remove("Rail");
            Assert.Equal(2, map.Layers.Count);
            map.Layers.Remove("asdgjsdfdsf");
            Assert.Equal(2, map.Layers.Count);
        }

        [Fact]
        public void RemoveGroupTest()
        {
            var map = CreateTestMap();
            Assert.Equal(1, map.Groups.Count);
            map.Groups.Remove("Base Layer Group");
            Assert.Equal(0, map.Groups.Count);

            map = CreateTestMap();
            var grp = map.Groups[0];
            map.Groups.Remove(grp);
            Assert.Equal(0, map.Groups.Count);
        }

        [Fact]
        public void GetLayersOfGroupTest()
        {
            var map = CreateTestMap();
            var layers = map.GetLayersOfGroup("Base Layer Group");
            Assert.Equal(3, layers.Length);
            layers = map.GetLayersOfGroup("asdjgdsfd");
            Assert.Equal(0, layers.Length);
        }

        [Fact]
        public void GetGroupsOfGroupTest()
        {
            var map = CreateTestMap();
            var groups = map.GetGroupsOfGroup("Base Layer Group");
            Assert.Equal(0, groups.Length);
            groups = map.GetGroupsOfGroup("asdjsdfdsfd");
            Assert.Equal(0, groups.Length);
            var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
            var grp = mapSvc.CreateMapGroup(map, "Test");
            grp.Group = "Base Layer Group";
            map.Groups.Add(grp);
            Assert.Equal(2, map.Groups.Count);
            groups = map.GetGroupsOfGroup("Base Layer Group");
            Assert.Equal(1, groups.Length);
            groups = map.GetGroupsOfGroup("Test");
            Assert.Equal(0, groups.Length);
        }
        
        [Fact]
        public void GetLayerByNameTest()
        {
            var map = CreateTestMap();
            Assert.NotNull(map.Layers["Rail"]);
            Assert.NotNull(map.Layers["Parcels"]);
            Assert.NotNull(map.Layers["HydrographicPolygons"]);
            Assert.Null(map.Layers["sdhgdsafdsfd"]);
        }

        [Fact]
        public void UpdateMapDefinitionTest()
        {
            var map = CreateTestMap();
            IMapDefinition mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test Map");
            map.UpdateMapDefinition(mdf);
            Assert.Equal(0, mdf.GetDynamicLayerCount());
            Assert.Equal(0, mdf.GetGroupCount());
            Assert.NotNull(mdf.BaseMap);
            var grp = mdf.BaseMap.GetFirstGroup();
            Assert.NotNull(grp);
            Assert.Equal(3, grp.BaseMapLayer.Count());
            Assert.Equal("Base Layer Group", grp.Name);
        }

        [Fact]
        public void ToMapDefinitionTest()
        {
            var map = CreateTestMap();
            var mdf = map.ToMapDefinition(false);
            Assert.Equal(0, mdf.GetDynamicLayerCount());
            Assert.Equal(0, mdf.GetGroupCount());
            Assert.NotNull(mdf.BaseMap);
            var grp = mdf.BaseMap.GetFirstGroup();
            Assert.NotNull(grp);
            Assert.Equal(3, grp.BaseMapLayer.Count());
            Assert.Equal("Base Layer Group", grp.Name);

            mdf = map.ToMapDefinition(true);
            Assert.Equal(0, mdf.GetDynamicLayerCount());
            Assert.Equal(0, mdf.GetGroupCount());
            Assert.NotNull(mdf.BaseMap);
            grp = mdf.BaseMap.GetFirstGroup();
            Assert.NotNull(grp);
            Assert.Equal(3, grp.BaseMapLayer.Count());
            Assert.Equal("Base Layer Group", grp.Name);
        }
    }
}
