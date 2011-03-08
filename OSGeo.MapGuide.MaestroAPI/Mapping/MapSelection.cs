using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Serialization;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// Represents a map selection
    /// </summary>
    public class MapSelection : IBinarySerializable, IList<MapSelection.LayerSelection>
    {
        private RuntimeMap _map;
        private List<LayerSelection> _layers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        public MapSelection(RuntimeMap map)
        {
            _map = map;
            _layers = new List<LayerSelection>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        /// <param name="xml"></param>
        public MapSelection(RuntimeMap map, string xml)
            : this(map)
        {
            LoadXml(xml);
        }

        /// <summary>
        /// Initialize this selection from the specified xml string
        /// </summary>
        /// <param name="xml"></param>
        public void LoadXml(string xml)
        {
            _layers.Clear();

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
                    var l = _map.GetLayerByObjectId(guid);
                    if (l != null)
                    {
                        foreach (System.Xml.XmlNode c in n.SelectNodes("Class"))
                        {
                            if (c.Attributes["id"] != null)
                                if (c.Attributes["id"].Value == l.QualifiedClassName)
                                    _layers.Add(new LayerSelection(l, c.SelectNodes("ID")));
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

            foreach (LayerSelection layer in _layers)
            {
                System.Xml.XmlNode ln = root.AppendChild(doc.CreateElement("Layer"));
                ln.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.ObjectId;

                System.Xml.XmlNode cn = ln.AppendChild(doc.CreateElement("Class"));
                cn.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.QualifiedClassName;

                for (int i = 0; i < layer.Count; i++)
                    cn.AppendChild(doc.CreateElement("ID")).InnerText = layer.EncodeIDString(layer[i]);
            }

            return doc.OuterXml;
        }

        /// <summary>
        /// Represents a layer selection
        /// </summary>
        public class LayerSelection : IList<object[]>
        {
            private RuntimeMapLayer m_layer;
            private List<object[]> m_list = new List<object[]>();

            /// <summary>
            /// Gets the layer that contains the selected objects
            /// </summary>
            public RuntimeMapLayer Layer { get { return m_layer; } }

            /// <summary>
            /// Internal helper to construct a LayerSelection
            /// </summary>
            /// <param name="layer">The layer that the selection belongs to</param>
            /// <param name="ids">A list of xml &lt;ID&gt; nodes</param>
            internal LayerSelection(RuntimeMapLayer layer, System.Xml.XmlNodeList ids)
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
                    for (int i = 0; i < m_layer.IdentityProperties.Length; i++)
                    {
                        Type type = m_layer.IdentityProperties[i].Type;

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
                object[] tmp = new object[m_layer.IdentityProperties.Length];
                for (int i = 0; i < m_layer.IdentityProperties.Length; i++)
                {
                    Type type = m_layer.IdentityProperties[i].Type;

                    if (type == typeof(short))
                    {
                        tmp[i] = BitConverter.ToInt16(data, index);
                        index += MgBinarySerializer.UInt16Len;
                    }
                    else if (type == typeof(int))
                    {
                        tmp[i] = BitConverter.ToInt32(data, index);
                        index += MgBinarySerializer.UInt32Len;
                    }
                    else if (type == typeof(long))
                    {
                        tmp[i] = BitConverter.ToInt64(data, index);
                        index += MgBinarySerializer.UInt64Len;
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
            public LayerSelection(RuntimeMapLayer layer, IEnumerable<object[]> ids)
                : this(layer)
            {
                AddRange(ids);
            }

            /// <summary>
            /// Constructs a new LayerSelection with a number of selected featured
            /// </summary>
            /// <param name="layer">The layer to represent</param>
            public LayerSelection(RuntimeMapLayer layer)
            {
                if (layer == null)
                    throw new ArgumentNullException("layer");

                if (layer.IdentityProperties.Length == 0)
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

                if (values.Length != m_layer.IdentityProperties.Length)
                    throw new Exception(string.Format("The layers key consists of {0} columns, but only {1} columns were given", m_layer.IdentityProperties.Length, values.Length));

                object[] tmp = new object[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                        throw new Exception(string.Format("The value for {0} is null, which is not supported as a key", m_layer.IdentityProperties[i].Name));
                    if (values[i].GetType() != m_layer.IdentityProperties[i].Type)
                        try { tmp[i] = Convert.ChangeType(values[i], m_layer.IdentityProperties[i].Type); }
                        catch (Exception ex) { throw new Exception(string.Format("Failed to convert value for {0} from {1} to {2}", m_layer.IdentityProperties[i].Name, values[i].GetType(), m_layer.IdentityProperties[i].Type), ex); }
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
                    object[] tmp = new object[m_layer.IdentityProperties.Length];
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

        public void Serialize(MgBinarySerializer s)
        {
            var m_selection = new XmlDocument();
            m_selection.LoadXml(ToXml());
            if (m_selection["FeatureSet"] == null)
            {
                s.Write((int)0);
                return;
            }

            XmlNodeList lst = m_selection["FeatureSet"].SelectNodes("Layer");
            s.Write(lst.Count);
            foreach (XmlNode n in lst)
            {
                if (n.Attributes["id"] == null)
                    throw new Exception("A layer in selection had no id");
                s.Write(n.Attributes["id"].Value);

                XmlNodeList cls = n.SelectNodes("Class");
                s.Write(cls.Count);

                foreach (XmlNode c in cls)
                {
                    s.Write(c.Attributes["id"].Value);
                    XmlNodeList ids = c.SelectNodes("ID");
                    s.Write(ids.Count);

                    foreach (XmlNode id in ids)
                        s.Write(id.InnerText);
                }
            }
        }

        public void Deserialize(MgBinaryDeserializer d)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.AppendChild(doc.CreateElement("FeatureSet"));
            int layerCount = d.ReadInt32();
            for (int i = 0; i < layerCount; i++)
            {
                XmlNode layer = root.AppendChild(doc.CreateElement("Layer"));
                layer.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString();

                int classCount = d.ReadInt32();
                for (int j = 0; j < classCount; j++)
                {
                    XmlNode @class = layer.AppendChild(doc.CreateElement("Class"));
                    @class.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString();

                    int idCount = d.ReadInt32();
                    for (int k = 0; k < idCount; k++)
                        @class.AppendChild(doc.CreateElement("ID")).InnerText = d.ReadString();
                }
            }
            LoadXml(doc.OuterXml);
        }

        #region IList<MapSelection.LayerSelection> Members

        /// <summary>
        /// Returns the index of the given layer
        /// </summary>
        /// <param name="item">The layer to look for</param>
        /// <returns>The index of the layer, or -1 if the layer is not in the collection</returns>
        public int IndexOf(MapSelection.LayerSelection item)
        {
            return IndexOf(item.Layer);
        }

        /// <summary>
        /// Returns the index of the given layer
        /// </summary>
        /// <param name="item">The layer to look for</param>
        /// <returns>The index of the layer, or -1 if the layer is not in the collection</returns>
        public int IndexOf(RuntimeMapLayer layer)
        {
            for (int i = 0; i < _layers.Count; i++)
                if (_layers[i].Layer.ObjectId == layer.ObjectId)
                    return i;

            return 1;
        }

        /// <summary>
        /// Inserts a selection layer into the collection
        /// </summary>
        /// <param name="index">The index to place the item at</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, MapSelection.LayerSelection item)
        {
            _layers.Insert(index, item);
        }

        /// <summary>
        /// Inserts a selection layer into the collection
        /// </summary>
        /// <param name="index">The index to place the item at</param>
        /// <param name="layer">The layer.</param>
        public void Insert(int index, RuntimeMapLayer layer)
        {
            _layers.Insert(index, new LayerSelection(layer));
        }

        /// <summary>
        /// Removes the item at the given index
        /// </summary>
        /// <param name="index">The index to remove the item at</param>
        public void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the selection layer at a given index
        /// </summary>
        /// <param name="index">The index to get or set the item for</param>
        /// <returns>The item at the given index</returns>
        public MapSelection.LayerSelection this[int index]
        {
            get
            {
                return _layers[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _layers[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selection layer at a given index
        /// </summary>
        /// <param name="index">The index to get or set the item for</param>
        /// <returns>The item at the given index</returns>
        public MapSelection.LayerSelection this[RuntimeMapLayer index]
        {
            get
            {
                return _layers[IndexOf(index)];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _layers[IndexOf(index)] = value;
            }
        }

        #endregion

        #region ICollection<MapSelection.LayerSelection> Members

        /// <summary>
        /// Adds the specified layer selection
        /// </summary>
        /// <param name="item"></param>
        public void Add(MapSelection.LayerSelection item)
        {
            if (item == null)
                throw new ArgumentNullException();

            _layers.Add(item);
        }

        /// <summary>
        /// Adds the specified layer
        /// </summary>
        /// <param name="layer"></param>
        public void Add(RuntimeMapLayer layer)
        {
            if (!Contains(layer))
                Add(new LayerSelection(layer));
        }

        /// <summary>
        /// Clears this selction
        /// </summary>
        public void Clear()
        {
            _layers.Clear();
        }

        /// <summary>
        /// Gets whether this selection contains the specified layer
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(RuntimeMapLayer item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Gets whether this selection contains the specified layer selection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(MapSelection.LayerSelection item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(MapSelection.LayerSelection[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of layers in this selection
        /// </summary>
        public int Count
        {
            get { return _layers.Count; }
        }

        /// <summary>
        /// Gets whether this is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified layer selection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(MapSelection.LayerSelection item)
        {
            int ix = IndexOf(item);
            if (ix < 0)
                return false;

            _layers.RemoveAt(ix);
            return true;
        }

        #endregion

        #region IEnumerable<LayerSelection> Members

        /// <summary>
        /// Gets the layer selection enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MapSelection.LayerSelection> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_layers).GetEnumerator();
        }

        #endregion
    }
}
