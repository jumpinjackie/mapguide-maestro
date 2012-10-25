#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.ComponentModel;
using System.Text;

using OSGeo.MapGuide.MaestroAPI.Exceptions;
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// A generic key/value collection
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TVal">The type of the value.</typeparam>
    public abstract class KeyValueCollection<TKey, TVal> : IList<TVal>, IList where TVal : class
    {
        /// <summary>
        /// The internal list of value
        /// </summary>
        protected List<TVal> _values;
        /// <summary>
        /// The internal dictionary of values keyed by its key
        /// </summary>
        protected Dictionary<TKey, TVal> _valuesByKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueCollection&lt;TKey, TVal&gt;"/> class.
        /// </summary>
        protected KeyValueCollection()
        {
            _values = new List<TVal>();
            _valuesByKey = new Dictionary<TKey, TVal>();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TVal item)
        {
            return _values.IndexOf(item);
        }

        /// <summary>
        /// Determines the index of a specific key in the collection
        /// </summary>
        /// <param name="key">The key of the object to locate in the collection</param>
        /// <returns>
        /// The index of <paramref name="key"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TKey key)
        {
            var item = this[key];
            return IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public virtual void Insert(int index, TVal item)
        {
            var key = SelectKey(item);
            if (_valuesByKey.ContainsKey(key))
                throw new DuplicateKeyException(string.Format(Strings.DuplicateKeyExceptionMessage, key));

            OnBeforeItemAdded(item);
            _values.Insert(index, item);
            _valuesByKey.Add(key, item);
            OnItemAdded(item);
            OnCollectionChanged();
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public void RemoveAt(int index)
        {
            var item = this[index];
            OnBeforeItemRemove(item);
            _values.RemoveAt(index);

            if (item != null)
            {
                var key = SelectKey(item);
                _valuesByKey.Remove(key);

                OnItemRemoved(item);
                OnCollectionChanged();
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        ///   </returns>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public virtual TVal this[int index]
        {
            get
            {
                return _values[index];
            }
            set
            {
                if (_values[index] != null)
                    RemoveAt(index);
                _values[index] = value;
                OnCollectionChanged();
            }
        }

        /// <summary>
        /// Sets the new index of the specified object. This object must already be present inside the collection
        /// </summary>
        /// <param name="newIndex"></param>
        /// <param name="item"></param>
        public void SetNewIndex(int newIndex, TVal item)
        {
            int idx = this.IndexOf(item);
            if (idx >= 0)
            {
                bool bSuccess = false;
                try
                {
                    bSuppressCollectionChanged = true;
                    this.RemoveAt(idx);
                    this.Insert(newIndex, item);
                    bSuccess = true;
                }
                finally
                {
                    bSuppressCollectionChanged = false;
                    if (bSuccess)
                        OnCollectionChanged();
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///   </exception>
        public virtual void Add(TVal item)
        {
            var key = SelectKey(item);
            if (_valuesByKey.ContainsKey(key))
                throw new DuplicateKeyException(string.Format(Strings.DuplicateKeyExceptionMessage, key));

            OnBeforeItemAdded(item);
            _values.Add(item);
            _valuesByKey.Add(key, item);
            OnItemAdded(item);
            OnCollectionChanged();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///   </exception>
        public virtual void Clear()
        {
            bool hasRemovedAnItem = false;
            try 
            {
                bSuppressCollectionChanged = true;
                //We don't call Clear() directly because we need to propagate removal of each
                //item back to the map
                var items = new List<TVal>(this);
                foreach (var item in items)
                {
                    Remove(item);
                    hasRemovedAnItem = true;
                }
                //This shouldn't happen
                if (_values.Count > 0)
                {
                    System.Diagnostics.Trace.TraceWarning("Expected empty values collection!"); //NOXLATE
                    _values.Clear();
                }
                if (_valuesByKey.Count > 0)
                {
                    System.Diagnostics.Trace.TraceWarning("Expected empty values collection!"); //NOXLATE
                    _valuesByKey.Clear();
                }
            }
            finally {
                bSuppressCollectionChanged = false;
                if (hasRemovedAnItem)
                    OnCollectionChanged();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(TVal item)
        {
            return _values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array"/> is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex"/> is less than 0.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array"/> is multidimensional.
        /// -or-
        ///   <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <typeparamref name="TVal"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        ///   </exception>
        public void CopyTo(TVal[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///   </returns>
        public int Count
        {
            get { return _values.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        ///   </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///   </exception>
        public bool Remove(TVal item)
        {
            OnBeforeItemRemove(item);
            var ret = _values.Remove(item);
            if (ret)
            {
                var key = SelectKey(item);
                _valuesByKey.Remove(key);

                OnItemRemoved(item);
                OnCollectionChanged();
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TVal> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        ///   </returns>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="key"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public TVal this[TKey key]
        {
            get { return _valuesByKey.ContainsKey(key) ? _valuesByKey[key] : null; }
            set 
            { 
                _valuesByKey[key] = value;
                OnCollectionChanged();
            }
        }
        
        /// <summary>
        /// Raised when the collection has been modified
        /// </summary>
        public event EventHandler CollectionChanged;
        
        private bool bSuppressCollectionChanged = false;
        
        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event
        /// </summary>
        protected virtual void OnCollectionChanged()
        {
            if (bSuppressCollectionChanged)
                return;
        
            var h = this.CollectionChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called before an item is added
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void OnBeforeItemAdded(TVal item) { }

        /// <summary>
        /// Called before an item is removed
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void OnBeforeItemRemove(TVal item) { }

        /// <summary>
        /// Called after an item has been added
        /// </summary>
        /// <param name="item">The item.</param>
        protected abstract void OnItemAdded(TVal item);

        /// <summary>
        /// Called after an item has been removed. Note this is only called if the remove
        /// operation removed the item in question
        /// </summary>
        /// <param name="value">The value.</param>
        protected abstract void OnItemRemoved(TVal value);

        /// <summary>
        /// Selects the key given the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected abstract TKey SelectKey(TVal value);

        int IList.Add(object value)
        {
            this.Add((TVal)value);
            return this.Count - 1;
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Contains((TVal)value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((TVal)value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (TVal)value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.Remove((TVal)value);
        }

        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (TVal)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((TVal[])array, index);
        }

        int ICollection.Count
        {
            get { return this.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        private readonly object _syncRoot = new object();

        object ICollection.SyncRoot
        {
            get { return _syncRoot; }
        }
    }

    /// <summary>
    /// A collection of runtime map layers
    /// </summary>
    public class RuntimeMapLayerCollection : KeyValueCollection<string, RuntimeMapLayer>
    {
        private RuntimeMap _parent;

        private Dictionary<string, RuntimeMapLayer> _layerIdMap;

        internal RuntimeMapLayerCollection(RuntimeMap parent)
        {
            _parent = parent;
            _layerIdMap = new Dictionary<string, RuntimeMapLayer>();
        }

        /// <summary>
        /// Adds the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Add(RuntimeMapLayer layer)
        {
            //calculate and set the zorder for the new layer
            RuntimeMapLayer prevLayer = (this.Count == 0) ? null : this[this.Count - 1];
            double zOrder = prevLayer == null ? RuntimeMap.Z_ORDER_TOP : prevLayer.DisplayOrder + RuntimeMap.Z_ORDER_INCREMENT;
            layer.DisplayOrder = zOrder;

            base.Add(layer);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public override void Insert(int index, RuntimeMapLayer item)
        {
            CalculateDisplayOrder(index, item);
            base.Insert(index, item);
        }

        /// <summary>
        /// Called when [before item added].
        /// </summary>
        /// <param name="layer">The layer.</param>
        protected override void OnBeforeItemAdded(RuntimeMapLayer layer)
        {
            if (_layerIdMap.ContainsKey(layer.ObjectId))
                throw new DuplicateKeyException(string.Format(Strings.DuplicateKeyExceptionMessage, layer.ObjectId));
        }

        /// <summary>
        /// Called when [item added].
        /// </summary>
        /// <param name="layer">The layer.</param>
        protected override void OnItemAdded(RuntimeMapLayer layer)
        {
            _layerIdMap[layer.ObjectId] = layer;
            _parent.OnLayerAdded(layer);
        }

        /// <summary>
        /// Called when [item removed].
        /// </summary>
        /// <param name="layer">The layer.</param>
        protected override void OnItemRemoved(RuntimeMapLayer layer)
        {
            if (_layerIdMap.ContainsKey(layer.ObjectId))
                _layerIdMap.Remove(layer.ObjectId);
            _parent.OnLayerRemoved(layer);
        }

        /// <summary>
        /// Selects the key given the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override string SelectKey(RuntimeMapLayer value)
        {
            return value.Name;
        }

        /// <summary>
        /// Gets a runtime map layer by its object id.
        /// </summary>
        /// <param name="id">The object id.</param>
        /// <returns></returns>
        public RuntimeMapLayer GetByObjectId(string id)
        {
            return _layerIdMap.ContainsKey(id) ? _layerIdMap[id] : null;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        ///   </returns>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        ///   </exception>
        public override RuntimeMapLayer this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                CalculateDisplayOrder(index, value);
                base[index] = value;
            }
        }

        /// <summary>
        /// Removes the specified layer by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            var layer = this[name];
            if (layer != null)
                Remove(layer);
        }

        private void CalculateDisplayOrder(int index, RuntimeMapLayer value)
        {
            //calculate zorder for the new layer
            double zOrderLow, zOrderHigh;
            RuntimeMapLayer layer;
            if (index == 0)
            {
                zOrderLow = 0;
                layer = base.Count > 0 ? base[index] : null;
                if (layer != null)
                    zOrderHigh = layer.DisplayOrder;
                else
                    zOrderHigh = 2.0 * RuntimeMap.Z_ORDER_INCREMENT;
            }
            else
            {
                layer = base[index - 1];
                zOrderLow = layer.DisplayOrder;
                layer = base.Count > index ? base[index] : null;
                zOrderHigh = layer != null ? layer.DisplayOrder : zOrderLow + 2.0 * RuntimeMap.Z_ORDER_INCREMENT;
            }
            value.DisplayOrder = (zOrderLow + (zOrderHigh - zOrderLow) / 2.0);
        }
    }

    /// <summary>
    /// A collection of runtime map groups
    /// </summary>
    public class RuntimeMapGroupCollection : KeyValueCollection<string, RuntimeMapGroup>
    {
        private RuntimeMap _parent;

        internal RuntimeMapGroupCollection(RuntimeMap parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Called after an item has been added
        /// </summary>
        /// <param name="item">The item.</param>
        protected override void OnItemAdded(RuntimeMapGroup item)
        {
            _parent.OnGroupAdded(item);
        }

        /// <summary>
        /// Called after an item has been removed. Note this is only called if the remove
        /// operation removed the item in question
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void OnItemRemoved(RuntimeMapGroup value)
        {
            _parent.OnGroupRemoved(value);
        }

        /// <summary>
        /// Selects the key given the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override string SelectKey(RuntimeMapGroup value)
        {
            return value.Name;
        }

        /// <summary>
        /// Removes the specified group by its name.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public void Remove(string groupName)
        {
            var group = this[groupName];
            if (group != null)
                Remove(group);
        }
    }
}
