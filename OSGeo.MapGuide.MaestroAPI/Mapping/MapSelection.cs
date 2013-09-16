using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Serialization;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// Represents a map selection
    /// </summary>
    /// <example>
    /// This sample shows how to list selected features in ASP.net. The sample expects 3 parameters:
    /// <list type="number">
    ///     <item><description>The session ID (SESSION)</description></item>
    ///     <item><description>The runtime map name (MAPNAME)</description></item>
    ///     <item><description>The selection XML (SELECTION)</description></item>
    /// </list>
    /// <code>
    /// <![CDATA[
    /// string agent = "http://localhost/mapguide/mapagent/mapagent.fcgi";
    /// IServerConnection conn = ConnectionProviderRegistry.CreateConnection(
    ///            "Maestro.Http",
    ///            "Url", agent,
    ///            "SessionId", Request.Params["SESSION"]);
    ///
    /// IMappingService mpSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
    /// string rtMapId = "Session:" + conn.SessionID + "//" + Request.Params["MAPNAME"] + ".Map";
    /// RuntimeMap rtMap = mpSvc.OpenMap(rtMapId);
    ///
    /// string xml = Request.Params["SELECTION"];
    /// //The map selection contains one or more layer selections
    /// //each containing a one or more sets of identity property values
    /// //(because a feature may have multiple identity properties)
    ///
    /// MapSelection selection = new MapSelection(rtMap, System.Web.HttpUtility.UrlDecode(xml));
    /// if (selection.Count > 0)
    /// {
    ///     StringBuilder sb = new StringBuilder();
    ///     for (int i = 0; i < selection.Count; i++)
    ///     {
    ///         MapSelection.LayerSelection layerSel = selection[i];
    ///         sb.Append("<p>Layer: " + layerSel.Layer.Name + " (" + layerSel.Count + " selected item)");
    ///         sb.Append("<table>");          
    ///         for (int j = 0; j < layerSel.Count; j++)
    ///         {
    ///             sb.Append("<tr>");
    ///             object[] values = layerSel[j];
    ///             for (int k = 0; k < values.Length; k++)
    ///             {
    ///                 sb.Append("<td>");
    ///                 sb.Append(values[k].ToString());
    ///                 sb.Append("</td>");
    ///             }
    ///             sb.AppendFormat("<td><a href='FeatureInfo.aspx?MAPNAME={0}&SESSION={1}&LAYERID={2}&ID={3}'>More Info</a></td>",
    ///                 rtMap.Name,
    ///                 conn.SessionID,
    ///                 layerSel.Layer.ObjectId,
    ///                 System.Web.HttpUtility.UrlEncode(layerSel.EncodeIDString(values)));
    ///             sb.Append("</tr>");
    ///         }
    ///         sb.Append("</table>");
    ///         Response.WriteLine("<p>Showing IDs of selected features</p>");
    ///         Response.WriteLine(sb.ToString());
    ///     }
    /// }
    /// else
    /// {
    ///     Response.WriteLine("Nothing selected. Select some features first then run this sample again.");
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
            System.Xml.XmlNodeList lst = doc.SelectNodes("FeatureSet/Layer"); //NOXLATE
            if (lst.Count == 0)
                lst = doc.SelectNodes("FeatureInformation/FeatureSet/Layer"); //NOXLATE

            foreach (System.Xml.XmlNode n in lst)
            {
                if (n.Attributes["id"] != null) //NOXLATE
                {
                    string guid = n.Attributes["id"].Value; //NOXLATE
                    var l = _map.Layers.GetByObjectId(guid);
                    if (l != null)
                    {
                        foreach (System.Xml.XmlNode c in n.SelectNodes("Class")) //NOXLATE
                        {
                            if (c.Attributes["id"] != null) //NOXLATE
                                if (c.Attributes["id"].Value == l.QualifiedClassName) //NOXLATE
                                    _layers.Add(new LayerSelection(l, c.SelectNodes("ID"))); //NOXLATE
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
            System.Xml.XmlNode root = doc.AppendChild(doc.CreateElement("FeatureSet")); //NOXLATE

            foreach (LayerSelection layer in _layers)
            {
                System.Xml.XmlNode ln = root.AppendChild(doc.CreateElement("Layer")); //NOXLATE
                ln.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.ObjectId; //NOXLATE

                System.Xml.XmlNode cn = ln.AppendChild(doc.CreateElement("Class")); //NOXLATE
                cn.Attributes.Append(doc.CreateAttribute("id")).Value = layer.Layer.QualifiedClassName; //NOXLATE

                for (int i = 0; i < layer.Count; i++)
                    cn.AppendChild(doc.CreateElement("ID")).InnerText = layer.EncodeIDString(layer[i]); //NOXLATE
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
            /// Adds records from the specified reader into this selection
            /// </summary>
            /// <param name="reader">The reader</param>
            /// <param name="limit">The maximum number of records to add. Specify -1 for all</param>
            /// <returns>Number of records added</returns>
            public int AddFeatures(IReader reader, int limit)
            {
                int added = 0;
                if (limit < 0)
                {
                    while (reader.ReadNext())
                    {
                        AddFeature(reader);
                        added++;
                    }
                }
                else
                {
                    while (reader.ReadNext() && added < limit)
                    {
                        AddFeature(reader);
                        added++;
                    }
                }
                reader.Close();
                return added;
            }

            /// <summary>
            /// Adds the specified record to the selection
            /// </summary>
            /// <param name="record"></param>
            public void AddFeature(IRecord record)
            {
                var idProps = m_layer.IdentityProperties;
                object[] values = new object[idProps.Length];
                for (int i = 0; i < idProps.Length; i++)
                {
                    var prop = idProps[i];
                    //Don't null check because identity property values cannot be null
                    values[i] = record[prop.Name];
                }
                Add(values);
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
                        else if (type == typeof(double))
                        {
                            byte[] x = BitConverter.GetBytes((double)tmp[i]);
                            ms.Write(x, 0, x.Length);
                        }
                        else if (type == typeof(string))
                        {
                            byte[] x = System.Text.Encoding.UTF8.GetBytes((string)tmp[i]);
                            ms.Write(x, 0, x.Length);
                            ms.WriteByte(0);
                        }
                        else
                            throw new Exception(string.Format(Strings.ErrorUnsupportedPkType, type.ToString()));
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
                return m_layer.ParseSelectionValues(id);
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
                    throw new ArgumentNullException("layer"); //NOXLATE

                if (layer.IdentityProperties.Length == 0 && layer.Parent.StrictSelection)
                    throw new Exception(Strings.ErrorLayerHasNoPk);

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
                    throw new ArgumentNullException("values"); //NOXLATE

                if (values.Length != m_layer.IdentityProperties.Length)
                    throw new Exception(string.Format(Strings.ErrorLayerKeyMismatch, m_layer.IdentityProperties.Length, values.Length));

                object[] tmp = new object[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                        throw new Exception(string.Format(Strings.ErrorNullKeyValue, m_layer.IdentityProperties[i].Name));
                    if (values[i].GetType() != m_layer.IdentityProperties[i].Type)
                        try { tmp[i] = Convert.ChangeType(values[i], m_layer.IdentityProperties[i].Type); }
                        catch (Exception ex) { throw new Exception(string.Format(Strings.ErrorFailedValueConversion, m_layer.IdentityProperties[i].Name, values[i].GetType(), m_layer.IdentityProperties[i].Type), ex); }
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

        /// <summary>
        /// Serializes this instance using the specified serializer.
        /// </summary>
        /// <param name="s">The serializer.</param>
        public void Serialize(MgBinarySerializer s)
        {
            var m_selection = new XmlDocument();
            m_selection.LoadXml(ToXml());
            if (m_selection["FeatureSet"] == null) //NOXLATE
            {
                s.Write((int)0);
                return;
            }

            XmlNodeList lst = m_selection["FeatureSet"].SelectNodes("Layer"); //NOXLATE
            s.Write(lst.Count);
            foreach (XmlNode n in lst)
            {
                if (n.Attributes["id"] == null) //NOXLATE
                    throw new Exception(Strings.ErrorSelectedLayerHasNoId);
                s.Write(n.Attributes["id"].Value); //NOXLATE

                XmlNodeList cls = n.SelectNodes("Class"); //NOXLATE
                s.Write(cls.Count);

                foreach (XmlNode c in cls)
                {
                    s.Write(c.Attributes["id"].Value); //NOXLATE
                    XmlNodeList ids = c.SelectNodes("ID"); //NOXLATE
                    s.Write(ids.Count);

                    foreach (XmlNode id in ids)
                        s.Write(id.InnerText);
                }
            }
        }

        /// <summary>
        /// Deserializes this object using the specified deserializer.
        /// </summary>
        /// <param name="d">The deserializer.</param>
        public void Deserialize(MgBinaryDeserializer d)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.AppendChild(doc.CreateElement("FeatureSet")); //NOXLATE
            int layerCount = d.ReadInt32();
            for (int i = 0; i < layerCount; i++)
            {
                XmlNode layer = root.AppendChild(doc.CreateElement("Layer")); //NOXLATE
                layer.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString(); //NOXLATE

                int classCount = d.ReadInt32();
                for (int j = 0; j < classCount; j++)
                {
                    XmlNode @class = layer.AppendChild(doc.CreateElement("Class")); //NOXLATE
                    @class.Attributes.Append(doc.CreateAttribute("id")).Value = d.ReadString(); //NOXLATE

                    int idCount = d.ReadInt32();
                    for (int k = 0; k < idCount; k++)
                        @class.AppendChild(doc.CreateElement("ID")).InnerText = d.ReadString(); //NOXLATE
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
        /// <param name="layer">The layer.</param>
        /// <returns>
        /// The index of the layer, or -1 if the layer is not in the collection
        /// </returns>
        public int IndexOf(RuntimeMapLayer layer)
        {
            for (int i = 0; i < _layers.Count; i++)
                if (_layers[i].Layer.ObjectId == layer.ObjectId)
                    return i;

            return -1;
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
        /// Gets the selection layer at a given index
        /// </summary>
        /// <param name="index">The index to get or set the item for</param>
        /// <returns>The item at the given index</returns>
        public MapSelection.LayerSelection this[RuntimeMapLayer index]
        {
            get
            {
                return _layers[IndexOf(index)];
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
            {
                var sel = new LayerSelection(layer);
                Add(sel);
            }
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
        ///   </exception>
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
