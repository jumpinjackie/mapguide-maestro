#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Mapping
{
    public class LayerCollectionTests : KeyValueCollectionTests<RuntimeMapLayerCollection, string, RuntimeMapLayer>
    {
        protected override RuntimeMapLayerCollection CreateCollection()
        {
            var type = typeof(RuntimeMapLayerCollection);
            var ctor = type.GetInternalConstructor(new[] { typeof(RuntimeMap) });

            var map = Mock.Of<RuntimeMap>();
            return ctor.Invoke(new[] { map }) as RuntimeMapLayerCollection;
        }

        protected override RuntimeMapLayer CreateTestValue1(string desiredKey = null)
        {
            var id = Guid.NewGuid().ToString();
            var layer = new Mock<RuntimeMapLayer>();
            layer.Setup(l => l.Name).Returns(desiredKey ?? "TestValue1");
            layer.Setup(l => l.ObjectId).Returns(id);
            return layer.Object;
        }

        protected override RuntimeMapLayer CreateTestValue2(string desiredKey = null)
        {
            var id = Guid.NewGuid().ToString();
            var layer = new Mock<RuntimeMapLayer>();
            layer.Setup(l => l.Name).Returns(desiredKey ?? "TestValue2");
            layer.Setup(l => l.ObjectId).Returns(id);
            return layer.Object;
        }

        [Fact]
        public override void Collection_BasicManipulation()
        {
            base.Collection_BasicManipulation();
        }

        [Fact]
        public override void Collection_InsertDuplicateKeyThrows()
        {
            base.Collection_InsertDuplicateKeyThrows();
        }

        [Fact]
        public override void Collection_BasicManipulation_AsIList()
        {
            base.Collection_BasicManipulation_AsIList();
        }

        [Fact]
        public override void Collection_IsEnumerable()
        {
            base.Collection_IsEnumerable();
        }

        [Fact]
        public override void Collection_IsEnumerable_AsIList()
        {
            base.Collection_IsEnumerable_AsIList();
        }
    }

    public class GroupCollectionTests : KeyValueCollectionTests<RuntimeMapGroupCollection, string, RuntimeMapGroup>
    {
        protected override RuntimeMapGroupCollection CreateCollection()
        {
            var type = typeof(RuntimeMapGroupCollection);
            var ctor = type.GetInternalConstructor(new[] { typeof(RuntimeMap) });

            var map = Mock.Of<RuntimeMap>();
            return ctor.Invoke(new[] { map }) as RuntimeMapGroupCollection;
        }

        protected override RuntimeMapGroup CreateTestValue1(string desiredKey = null)
        {
            var layer = new Mock<RuntimeMapGroup>();
            layer.Setup(l => l.Name).Returns(desiredKey ?? "TestValue1");
            return layer.Object;
        }

        protected override RuntimeMapGroup CreateTestValue2(string desiredKey = null)
        {
            var layer = new Mock<RuntimeMapGroup>();
            layer.Setup(l => l.Name).Returns(desiredKey ?? "TestValue2");
            return layer.Object;
        }

        [Fact]
        public override void Collection_BasicManipulation()
        {
            base.Collection_BasicManipulation();
        }

        [Fact]
        public override void Collection_InsertDuplicateKeyThrows()
        {
            base.Collection_InsertDuplicateKeyThrows();
        }

        [Fact]
        public override void Collection_BasicManipulation_AsIList()
        {
            base.Collection_BasicManipulation_AsIList();
        }

        [Fact]
        public override void Collection_IsEnumerable()
        {
            base.Collection_IsEnumerable();
        }

        [Fact]
        public override void Collection_IsEnumerable_AsIList()
        {
            base.Collection_IsEnumerable_AsIList();
        }
    }
}
