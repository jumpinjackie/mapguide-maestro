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
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.Collections;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public abstract class KeyValueCollectionTests<TCollection, TKey, TValue>
        where TValue : class
        where TCollection : KeyValueCollection<TKey, TValue>
    {
        protected abstract TCollection CreateCollection();

        protected abstract TValue CreateTestValue1(string desiredKey = null);

        protected abstract TValue CreateTestValue2(string desiredKey = null);

        public virtual void Collection_BasicManipulation()
        {
            var collection = CreateCollection();
            var val1 = CreateTestValue1();
            Assert.True(collection.IndexOf(val1) < 0);
            Assert.False(collection.Contains(val1));
            collection.Add(val1);
            Assert.True(collection.IndexOf(val1) >= 0);
            Assert.True(collection.Contains(val1));

            collection.Clear();
            Assert.Equal(0, collection.Count);

            var val2 = CreateTestValue2();
            collection.Insert(0, val1);
            collection.Insert(0, val2);

            Assert.Equal(2, collection.Count);
            Assert.False(collection.IsReadOnly);
        }

        public virtual void Collection_BasicManipulation_AsIList()
        {
            IList collection = CreateCollection();
            var val1 = CreateTestValue1();
            Assert.True(collection.IndexOf(val1) < 0);
            Assert.False(collection.Contains(val1));
            collection.Add(val1);
            Assert.True(collection.IndexOf(val1) >= 0);
            Assert.True(collection.Contains(val1));

            collection.Clear();
            Assert.Equal(0, collection.Count);

            var val2 = CreateTestValue2();
            collection.Insert(0, val1);
            collection.Insert(0, val2);

            Assert.Equal(2, collection.Count);
            Assert.False(collection.IsReadOnly);
            Assert.False(collection.IsFixedSize);
        }

        public virtual void Collection_IsEnumerable()
        {
            var collection = CreateCollection();
            var val1 = CreateTestValue1();
            var val2 = CreateTestValue2();
            collection.Add(val1);
            collection.Add(val2);
            foreach (var v in collection)
            {
                Assert.True(v == val1 || v == val2);
            }
        }

        public virtual void Collection_IsEnumerable_AsIList()
        {
            IList collection = CreateCollection();
            var val1 = CreateTestValue1();
            var val2 = CreateTestValue2();
            collection.Add(val1);
            collection.Add(val2);
            foreach (var v in collection)
            {
                Assert.True(v == val1 || v == val2);
            }
        }

        public virtual void Collection_InsertDuplicateKeyThrows()
        {
            var collection = CreateCollection();
            var val1 = CreateTestValue1("Key1");
            var val2 = CreateTestValue2("Key1");
            collection.Insert(0, val1);
            Assert.Throws<DuplicateKeyException>(() => collection.Insert(0, val2));
        }
    }
}
