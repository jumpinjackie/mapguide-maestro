using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// A wrapper class to help deal with the selection xml format
    /// </summary>
    public class Selection : IList<Selection.LayerSelection>
    {
        private RuntimeClasses.RuntimeMap m_map;
        private List<LayerSelection> m_layers;

        /// <summary>
        /// Constructs a new selection helper
        /// </summary>
        /// <param name="map">The map that the selection belongs to</param>
        public Selection(RuntimeClasses.RuntimeMap map)
        {
            m_layers = new List<LayerSelection>();
            m_map = map;
        }

        /// <summary>
        /// Constructs a new selection helper
        /// </summary>
        /// <param name="map">The map that the selection belongs to</param>
        /// <param name="xml">A selection xml to initialize the selection with</param>
        public Selection(RuntimeClasses.RuntimeMap map, string xml)
            : this(map)
        {
            FromXml(xml);
        }

        /// <summary>
        /// Resets the selectionhelper to match the provided xml
        /// </summary>
        /// <param name="xml">The selection xml</param>
        public void FromXml(string xml)
        {
            m_layers.Clear();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xml))
                doc.LoadXml(xml);

            //There are two variations
            System.Xml.XmlNodeList lst = doc.SelectNodes("FeatureSet/Layer");
            if (lst.Count == 0)
                lst = doc.SelectNodes("FeatureInformation/FeatureSet/Layer");

            foreach (System.Xml.XmlNode n in lst)
            {
                if (n.Attributes["id"] != null)
                {
                    string guid = n.Attributes["id"].Value;
                    RuntimeClasses.RuntimeMapLayer l = m_map.Layers.FindByGuid(guid);
                    if (l != null)
                    {
                        foreach (System.Xml.XmlNode c in n.SelectNodes("Class"))
                        {
                            if (c.Attributes["id"] != null)
                                if (c.Attributes["id"].Value == l.SchemaName + ":" + l.FeatureName)
                                    m_layers.Add(new LayerSelection(l, c.SelectNodes("ID")));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns an xml document that represents the current map selection
        /// </summary>
        /// <returns>An xml document that represents the current map selection</returns>
        public string ToXml()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode root = doc.AppendChild(doc.CreateElement("FeatureSet"));

            foreach (LayerSelection layer in m_layers)
            {
                System.Xml.XmlNode ln = root.AppendChild(doc.CreateElement("Layer"));
                ln.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.Guid;

                System.Xml.XmlNode cn = ln.AppendChild(doc.CreateElement("Class"));
                cn.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.SchemaName + ":" + layer.Layer.FeatureName;

                for (int i = 0; i < layer.Count; i++)
                    cn.AppendChild(doc.CreateElement("ID")).InnerText = layer.EncodeIDString(layer[i]);
            }

            return doc.OuterXml;
        }

        /// <summary>
        /// Returns the selection Xml, the same as ToXml()
        /// </summary>
        /// <returns>The selection xml</returns>
        public override string ToString()
        {
            return ToXml();
        }

        /// <summary>
        /// Helper class to represent a layer with selected objects
        /// </summary>
        public class LayerSelection : IList<object[]>
        {
            private RuntimeClasses.RuntimeMapLayer m_layer;
            private List<object[]> m_list = new List<object[]>();

            /// <summary>
            /// Gets the layer that contains the selected objects
            /// </summary>
            public RuntimeClasses.RuntimeMapLayer Layer { get { return m_layer; } }

            /// <summary>
            /// Internal helper to construct a LayerSelection
            /// </summary>
            /// <param name="layer">The layer that the selection belongs to</param>
            /// <param name="ids">A list of xml &lt;ID&gt; nodes</param>
            internal LayerSelection(RuntimeClasses.RuntimeMapLayer layer, System.Xml.XmlNodeList ids)
                : this(layer)
            {
                foreach (System.Xml.XmlNode n in ids)
                    Add(ParseIDString(n.InnerXml));
            }

            /// <summary>
            /// Encodes the given combined keyset into an ID string for use in the Xml
            /// </summary>
            /// <param name="values">The combined key</param>
            /// <returns>A base64 encoded ID string</returns>
            public string EncodeIDString(object[] values)
            {
                object[] tmp = NormalizeAndValidate(values);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    for (int i = 0; i < m_layer.IDs.Count; i++)
                    {
                        Type type = m_layer.IDs[i].Value;

                        if (type == typeof(short))
                        {
                            byte[] x = BitConverter.GetBytes((short)tmp[i]);
                            ms.Write(x, 0, x.Length);
                        }
                        else if (type == typeof(int))
                        {
                            byte[] x = BitConverter.GetBytes((int)tmp[i]);
                            ms.Write(x, 0, x.Length);
                        }
                        else if (type == typeof(long))
                        {
                            byte[] x = BitConverter.GetBytes((long)tmp[i]);
                            ms.Write(x, 0, x.Length);
                        }
                        else if (type == typeof(string))
                        {
                            byte[] x = System.Text.Encoding.UTF8.GetBytes((string)tmp[i]);
                            ms.Write(x, 0, x.Length);
                            ms.WriteByte(0);
                        }
                        else
                            throw new Exception(string.Format("The type {0} is not supported for primary keys", type.ToString()));
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }

            /// <summary>
            /// Parses a base64 encoded string with key values
            /// </summary>
            /// <param name="id">The base64 encoded ID string</param>
            /// <returns>The composite value key</returns>
            public object[] ParseIDString(string id)
            {
                int index = 0;
                byte[] data = Convert.FromBase64String(id);
                object[] tmp = new object[m_layer.IDs.Count];
                for (int i = 0; i < m_layer.IDs.Count; i++)
                {
                    Type type = m_layer.IDs[i].Value;

                    if (type == typeof(short))
                    {
                        tmp[i] = BitConverter.ToInt16(data, index);
                        index += BinarySerializer.MgBinarySerializer.UInt16Len;
                    }
                    else if (type == typeof(int))
                    {
                        tmp[i] = BitConverter.ToInt32(data, index);
                        index += BinarySerializer.MgBinarySerializer.UInt32Len;
                    }
                    else if (type == typeof(long))
                    {
                        tmp[i] = BitConverter.ToInt64(data, index);
                        index += BinarySerializer.MgBinarySerializer.UInt64Len;
                    }
                    else if (type == typeof(string))
                    {
                        int pos = index;
                        while (pos < data.Length && data[pos] != 0)
                            pos++;

                        if (pos >= data.Length)
                            throw new Exception("Bad null encoded string");

                        tmp[i] = System.Text.Encoding.UTF8.GetString(data, index, pos - index);
                        index = pos + 1;
                    }
                    else
                        throw new Exception(string.Format("The type {0} is not supported for primary keys", type.ToString()));
                }

                return tmp;
            }

            /// <summary>
            /// Constructs a new LayerSelection with a number of selected featured
            /// </summary>
            /// <param name="layer">The layer to represent</param>
            /// <param name="ids">The list of composite IDs that the layer supports</param>
            public LayerSelection(RuntimeClasses.RuntimeMapLayer layer, IEnumerable<object[]> ids)
                : this(layer)
            {
                AddRange(ids);
            }

            /// <summary>
            /// Constructs a new LayerSelection with a number of selected featured
            /// </summary>
            /// <param name="layer">The layer to represent</param>
            public LayerSelection(RuntimeClasses.RuntimeMapLayer layer)
            {
                if (layer == null)
                    throw new ArgumentNullException("layer");

                if (layer.IDs == null || layer.IDs.Count == 0)
                    throw new Exception("The layer does not have a primary key, and cannot be used for selection");

                m_layer = layer;
            }

            /// <summary>
            /// Adds a composite key to the selection
            /// </summary>
            /// <param name="values">The composite key</param>
            public void Add(object[] values)
            {
                object[] tmp = NormalizeAndValidate(values);

                if (!this.Contains(tmp))
                    m_list.Add(tmp);
            }

            /// <summary>
            /// Ensures that the composite key types match the layers ID column types.
            /// The returned array is a copy of the one passed in
            /// </summary>
            /// <param name="values">The composite key</param>
            /// <returns>A copy of the composite key</returns>
            private object[] NormalizeAndValidate(object[] values)
            {
                if (values == null)
                    throw new ArgumentNullException("values");

                if (values.Length != m_layer.IDs.Count)
                    throw new Exception(string.Format("The layers key consists of {0} columns, but only {1} columns were given", m_layer.IDs.Count, values.Length));

                object[] tmp = new object[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                        throw new Exception(string.Format("The value for {0} is null, which is not supported as a key", m_layer.IDs[i].Key));
                    if (values[i].GetType() != m_layer.IDs[i].Value)
                        try { tmp[i] = Convert.ChangeType(values[i], m_layer.IDs[i].Value); }
                        catch (Exception ex) { throw new Exception(string.Format("Failed to convert value for {0} from {1} to {2}", m_layer.IDs[i].Key, values[i].GetType(), m_layer.IDs[i].Value), ex); }
                    else
                        tmp[i] = values[i];
                }

                return tmp;
            }

            /// <summary>
            /// Adds a number of composite keys
            /// </summary>
            /// <param name="lst">A list of composite keys</param>
            public void AddRange(IEnumerable<object[]> lst)
            {
                foreach (object[] x in lst)
                    Add(x);
            }

            #region IList<object[]> Members

            /// <summary>
            /// Returns the index of the given composite key
            /// </summary>
            /// <param name="item">The composite key to look for</param>
            /// <returns>The index of the composite key or -1 if the key is not found</returns>
            public int IndexOf(object[] item)
            {
                object[] tmp = NormalizeAndValidate(item);

                for (int i = 0; i < m_list.Count; i++)
                {
                    object[] tmpx = m_list[i];

                    bool matches = true;
                    for (int j = 0; j < tmpx.Length; j++)
                        if (tmpx[j] != item[j])
                        {
                            matches = false;
                            break;
                        }

                    if (matches)
                        return i;
                }

                return -1;
            }

            /// <summary>
            /// Inserts a key at the specified location
            /// </summary>
            /// <param name="index">The index to insert the key at</param>
            /// <param name="item">The key to insert</param>
            public void Insert(int index, object[] item)
            {
                object[] tmp = NormalizeAndValidate(item);

                int ix = IndexOf(tmp);

                if (ix >= 0)
                    RemoveAt(ix);

                Insert(index, item);
            }

            /// <summary>
            /// Removes the element at the specified location
            /// </summary>
            /// <param name="index">The index of the item to remove</param>
            public void RemoveAt(int index)
            {
                m_list.RemoveAt(index);
            }

            /// <summary>
            /// Gets or sets the composite key for the specified index
            /// </summary>
            /// <param name="index">The index for the composite key</param>
            /// <returns>The composite key</returns>
            public object[] this[int index]
            {
                get
                {
                    object[] tmp = new object[m_layer.IDs.Count];
                    Array.Copy(m_list[index], tmp, tmp.Length);
                    return tmp;
                }
                set
                {
                    m_list[index] = NormalizeAndValidate(value);
                }
            }

            #endregion

            #region ICollection<object[]> Members

            /// <summary>
            /// Removes all composite keys from the collection
            /// </summary>
            public void Clear()
            {
                m_list.Clear();
            }

            /// <summary>
            /// Returns a value indicating if the composite key is contained in the collection
            /// </summary>
            /// <param name="item">The composite key to look for</param>
            /// <returns>True if the collection contains the composite key, false otherwise</returns>
            public bool Contains(object[] item)
            {
                return IndexOf(item) >= 0;
            }

            /// <summary>
            /// Not implemented
            /// </summary>
            /// <param name="array"></param>
            /// <param name="arrayIndex"></param>
            public void CopyTo(object[][] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns the number of composite keys (and thus selected objects)
            /// </summary>
            public int Count
            {
                get { return m_list.Count; }
            }

            /// <summary>
            /// Gets a value indicating if the collection is read-only
            /// </summary>
            public bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Removes the given composite key from the collection
            /// </summary>
            /// <param name="item">The composite key to remove</param>
            /// <returns>True if the composite key was found and removed, false otherwise</returns>
            public bool Remove(object[] item)
            {
                int ix = IndexOf(item);
                if (ix < 0)
                    return false;
                
                m_list.RemoveAt(ix);
                return true;
            }

            #endregion

            #region IEnumerable<object[]> Members

            /// <summary>
            /// Returns an enumerator for the collection
            /// </summary>
            /// <returns>An enumerator for the collection</returns>
            public IEnumerator<object[]> GetEnumerator()
            {
                return m_list.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            /// <summary>
            /// Returns an enumerator for the collection
            /// </summary>
            /// <returns>An enumerator for the collection</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((System.Collections.IEnumerable)m_list).GetEnumerator();
            }

            #endregion
        }


        #region IList<LayerSelection> Members

        /// <summary>
        /// Returns the index of the given layer
        /// </summary>
        /// <param name="item">The layer to look for</param>
        /// <returns>The index of the layer, or -1 if the layer is not in the collection</returns>
        public int IndexOf(Selection.LayerSelection item)
        {
            return IndexOf(item.Layer);
        }

        /// <summary>
        /// Returns the index of the given layer
        /// </summary>
        /// <param name="item">The layer to look for</param>
        /// <returns>The index of the layer, or -1 if the layer is not in the collection</returns>
        public int IndexOf(RuntimeClasses.RuntimeMapLayer layer)
        {
            for (int i = 0; i < m_layers.Count; i++)
                if (m_layers[i].Layer.Guid == layer.Guid)
                    return i;
            
            return 1;
        }

        /// <summary>
        /// Inserts a selection layer into the collection
        /// </summary>
        /// <param name="index">The index to place the item at</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, Selection.LayerSelection item)
        {
            m_layers.Insert(index, item);
        }

        /// <summary>
        /// Inserts a selection layer into the collection
        /// </summary>
        /// <param name="index">The index to place the item at</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, RuntimeClasses.RuntimeMapLayer layer)
        {
            m_layers.Insert(index, new LayerSelection(layer));
        }

        /// <summary>
        /// Removes the item at the given index
        /// </summary>
        /// <param name="index">The index to remove the item at</param>
        public void RemoveAt(int index)
        {
            m_layers.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the selection layer at a given index
        /// </summary>
        /// <param name="index">The index to get or set the item for</param>
        /// <returns>The item at the given index</returns>
        public Selection.LayerSelection this[int index]
        {
            get
            {
                return m_layers[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_layers[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selection layer at a given index
        /// </summary>
        /// <param name="index">The index to get or set the item for</param>
        /// <returns>The item at the given index</returns>
        public Selection.LayerSelection this[RuntimeClasses.RuntimeMapLayer index]
        {
            get
            {
                return m_layers[IndexOf(index)];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_layers[IndexOf(index)] = value;
            }
        }

        #endregion

        #region ICollection<LayerSelection> Members

        public void Add(Selection.LayerSelection item)
        {
            if (item == null)
                throw new ArgumentNullException();
            
            m_layers.Add(item);
        }

        public void Add(RuntimeClasses.RuntimeMapLayer layer)
        {
            if (!Contains(layer))
                Add(new LayerSelection(layer));
        }

        public void Clear()
        {
            m_layers.Clear();
        }

        public bool Contains(RuntimeClasses.RuntimeMapLayer item)
        {
            return IndexOf(item) >= 0;
        }

        public bool Contains(Selection.LayerSelection item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(Selection.LayerSelection[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return m_layers.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Selection.LayerSelection item)
        {
            int ix = IndexOf(item);
            if (ix < 0)
                return false;

            m_layers.RemoveAt(ix);
            return true;
        }

        #endregion

        #region IEnumerable<LayerSelection> Members

        public IEnumerator<Selection.LayerSelection> GetEnumerator()
        {
            return m_layers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)m_layers).GetEnumerator();
        }

        #endregion
    }
}
