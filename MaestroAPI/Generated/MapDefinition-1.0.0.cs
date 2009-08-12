#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <summary>
    /// Defines a Map, in either the library or session repository
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("MapDefinition",  Namespace="", IsNullable=false)]
    public class MapDefinition 
	{
		protected ServerConnectionI m_serverConnection;

		/// <summary>
		/// Gets or sets the connection used in various operations performed on this object
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public ServerConnectionI CurrentConnection
		{
			get { return m_serverConnection; }
			set 
			{ 
				m_serverConnection = value;
				foreach(MapLayerType layer in this.Layers)
					layer.Parent = this;
			}
		}

		protected string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; } 
		}

		public static readonly string SchemaName = "MapDefinition-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

		protected string m_name;
		protected string m_coordinateSystem;
		protected Box2DType m_extents;
		protected System.Drawing.Color m_backgroundColor;
		protected string m_metadata;
		protected MapLayerTypeCollection m_mapLayer;
		protected MapLayerGroupTypeCollection m_mapLayerGroup;
		protected MapDefinitionTypeBaseMapDefinition m_baseMapDefinition;

		public MapDefinition()
		{
			m_name = "";
			m_coordinateSystem = "";
			m_extents = new Box2DType();
			m_backgroundColor = System.Drawing.Color.FromArgb(0,0,0);
			m_metadata = "";
			m_mapLayer = new MapLayerTypeCollection();
			m_mapLayerGroup = new MapLayerGroupTypeCollection();
			m_baseMapDefinition = new MapDefinitionTypeBaseMapDefinition();
		}
        
		/// <remarks/>
		public string Name 
		{
			get 
			{
				return this.m_name;
			}
			set 
			{
				this.m_name = value;
			}
		}
        
		/// <remarks/>
		public string CoordinateSystem 
		{
			get 
			{
				return this.m_coordinateSystem;
			}
			set 
			{
				this.m_coordinateSystem = value;
			}
		}
        
		/// <remarks/>
		public Box2DType Extents 
		{
			get 
			{
				return this.m_extents;
			}
			set 
			{
				this.m_extents = value;
			}
		}
        

		[System.Xml.Serialization.XmlElementAttribute("BackgroundColor")]
		public string BackGroundColorAsHTML
		{
			get 
			{ 
				return Utility.SerializeHTMLColor(m_backgroundColor, true);
				/*return new byte[] 
				{
					m_backgroundColor.A,
					m_backgroundColor.R,
					m_backgroundColor.G,
					m_backgroundColor.B
				};*/
			}
			set 
			{
				if (value == null)
					m_backgroundColor = System.Drawing.Color.White;
				else
					m_backgroundColor = Utility.ParseHTMLColor(value); //System.Drawing.Color.FromArgb(value[0], value[1], value[2], value[3]);
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnore()]
		public System.Drawing.Color BackgroundColor 
		{
			get 
			{
				return this.m_backgroundColor;
			}
			set 
			{
				this.m_backgroundColor = value;
			}
		}
        
		/// <summary>
		/// Gets or sets the Metadata for the map
		/// </summary>
		public string Metadata 
		{
			get 
			{
				return this.m_metadata;
			}
			set 
			{
				this.m_metadata = value;
			}
		}
        
		/// <summary>
		/// Gets or sets the layers associated with this map
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute("MapLayer")]
		public MapLayerTypeCollection Layers 
		{
			get { return this.m_mapLayer; }
			set { this.m_mapLayer = value; }
		}
        
		/// <summary>
		/// Gets or sets the groups defined in this map
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute("MapLayerGroup")]
		public MapLayerGroupTypeCollection LayerGroups 
		{
			get { return this.m_mapLayerGroup; }
			set { this.m_mapLayerGroup = value; }
		}
        
		/// <remarks/>
		public MapDefinitionTypeBaseMapDefinition BaseMapDefinition 
		{
			get 
			{
				return this.m_baseMapDefinition;
			}
			set 
			{
				this.m_baseMapDefinition = value;
			}
		}
	}


	public class Change
	{
		public enum ChangeType
		{
			removed,
			added,
			visibilityChanged,
			displayInLegendChanged,
			legendLabelChanged,
			parentChanged,
			selectabilityChanged,
			definitionChanged
		};

		private ChangeType m_type;
		private string m_params;

		public ChangeType Type { get { return m_type; } }
		public string Params { get { return m_params; } }

		public Change()
		{
		}

		public Change(ChangeType type, string param)
		{
			m_type = type;
			m_params = param;
		}
	}

	public class ChangeList
	{
		private string m_objectId;
		private bool m_isLayer;
		private ArrayList m_changes;

		public string ObjectId { get { return m_objectId; } } 
		public bool IsLayer { get { return m_isLayer; } }
		public ArrayList Changes { get { return m_changes; } }

		public ChangeList()
		{
			m_changes = new ArrayList();
		}

		public ChangeList(string objectId, bool isLayer)
			: this()
		{
			m_objectId = objectId;
			m_isLayer = isLayer;
		}
	}
        
    /// <remarks/>
    public class Box2DType
		//: BinarySerializer.IBinarySerializeable 
	{
        
        private System.Double m_minX;
        private System.Double m_maxX;
        private System.Double m_minY;
        private System.Double m_maxY;

        /// <remarks/>
        public System.Double MinX 
		{
            get {
                return this.m_minX;
            }
            set {
                this.m_minX = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxX {
            get {
                return this.m_maxX;
            }
            set {
                this.m_maxX = value;
            }
        }
        
        /// <remarks/>
        public System.Double MinY {
            get {
                return this.m_minY;
            }
            set {
                this.m_minY = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxY {
            get {
                return this.m_maxY;
            }
            set {
                this.m_maxY = value;
            }
        }

		internal void Deserialize(BinarySerializer.MgBinaryDeserializer d)
		{
			int classid = d.ReadClassId();
			if (d.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 18001)
				throw new Exception("Invalid class identifier, expected Box2D");
			if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 20001)
				throw new Exception("Invalid class identifier, expected Box2D");

			int dimensions = d.ReadInt32();
			if (dimensions != 2 && dimensions != 0)
				throw new Exception("Bounding box for map had " + dimensions.ToString() + " dimensions, 2 was expected");
			double x1 = d.ReadDouble();
			double y1 = d.ReadDouble();

			double x2 = d.ReadDouble();
			double y2 = d.ReadDouble();

			m_minX = Math.Min(x1, x2);
			m_minY = Math.Min(y1, y2);
			m_maxX = Math.Max(x1, x2);
			m_maxY = Math.Max(y1, y2);
		}

		internal void Serialize(BinarySerializer.MgBinarySerializer s)
		{
			if (s.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
				s.WriteClassId(18001);
			else
				s.WriteClassId(20001);

			s.Write((int)0);

			s.Write(m_minX);
			s.Write(m_minY);
			s.Write(m_maxX);
			s.Write(m_maxY);
		}
	}
    
    /// <remarks/>
    public class MapDefinitionTypeBaseMapDefinition {
        
        private DoubleCollection m_finiteDisplayScale;
        
        private BaseMapLayerGroupCommonTypeCollection m_baseMapLayerGroup;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FiniteDisplayScale")]
        public DoubleCollection FiniteDisplayScale {
            get {
                return this.m_finiteDisplayScale;
            }
            set {
                this.m_finiteDisplayScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BaseMapLayerGroup")]
        public BaseMapLayerGroupCommonTypeCollection BaseMapLayerGroup {
            get {
                return this.m_baseMapLayerGroup;
            }
            set {
                this.m_baseMapLayerGroup = value;
            }
        }
    }
    
    /// <remarks/>
    public class BaseMapLayerGroupCommonType : MapLayerGroupCommonType {
        
        private BaseMapLayerTypeCollection m_baseMapLayer;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BaseMapLayer")]
        public BaseMapLayerTypeCollection BaseMapLayer {
            get {
                return this.m_baseMapLayer;
            }
            set {
                this.m_baseMapLayer = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MapLayerType))]
    public class BaseMapLayerType {
        
        protected string m_name;
        protected string m_resourceId;
        protected bool m_selectable;
        protected bool m_showInLegend;
        protected string m_legendLabel;
        protected bool m_expandInLegend;
       
		[System.Xml.Serialization.XmlIgnore()]
		public MapDefinition Parent { get { return m_parent; } set { m_parent = value; } }

		protected MapDefinition m_parent = null;

        /// <remarks/>
        public string Name 
		{
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
        /// <remarks/>
        public virtual bool Selectable {
            get {
                return this.m_selectable;
            }
            set {
                this.m_selectable = value;
            }
        }
        
        /// <remarks/>
        public bool ShowInLegend {
            get {
                return this.m_showInLegend;
            }
            set {
                this.m_showInLegend = value;
            }
        }
        
        /// <remarks/>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
        
        /// <remarks/>
        public bool ExpandInLegend {
            get {
                return this.m_expandInLegend;
            }
            set {
                this.m_expandInLegend = value;
            }
        }

    }
    
    /// <remarks/>
    public class MapLayerType 
		: BaseMapLayerType
	{
        

        protected bool m_visible;
        protected string m_group = "";

		public MapLayerType()
			: base()
		{
		}
        
        /// <summary>
        /// Gets or sets the layer visibility, this attribute is combined with the layers DisplayScale, to determine actual visibility.
        /// </summary>
        public virtual bool Visible {
            get { return this.m_visible; }
            set { this.m_visible = value; }
        }
        
        /// <summary>
        /// Gets or sets the Group that this layer belongs to
        /// </summary>
        public string Group {
            get { return this.m_group; }
            set { this.m_group = value; }
        }
        
		/// <summary>
		/// Gets the full path of the group
		/// </summary>
		/// <param name="separator">The string used to separate the individual levels</param>
		/// <param name="parent">The parent mapdefinition, use null for the current parent</param>
		/// <returns>The full path or null if no such group is found</returns>
		public string GetFullPath(string separator, MapDefinition parent)
		{
            //TODO: Protect against infinite recursion
            if (parent == null)
				parent = m_parent;

			if (parent == null)
				throw new Exception("Cannot determine full path on group that is not attached to a map");
			if (m_group == null || m_group.Length == 0)
				return m_name;
			else
			{
				MapLayerGroupType mlg = null;
				if (parent.LayerGroups != null)
					foreach(MapLayerGroupType g in parent.LayerGroups)
						if (g.Name == m_group)
						{
							mlg = g;
							break;
						}
                if (mlg == null)
                    return null;
				return mlg.GetFullPath(separator, parent) + separator + m_name;
			}
		}

		/// <summary>
		/// Returns a list of layers present in the current group. This method does NOT run in O(n).
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public MapLayerTypeCollection Layers
		{
			get 
			{
				if (m_parent == null)
					return null;

				string s = this.GetFullPath("/", m_parent);
				MapLayerTypeCollection layers = new MapLayerTypeCollection();
				foreach(MapLayerType ml in m_parent.Layers)
					if (ml.GetFullPath("/", m_parent).StartsWith(s))
						layers.Add(ml);

				return layers;
			}
		}


    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseMapLayerGroupCommonType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MapLayerGroupType))]
    public class MapLayerGroupCommonType {
        
        protected string m_name;
        protected bool m_visible;
        protected bool m_showInLegend;
        protected bool m_expandInLegend;
        protected string m_legendLabel;

        /// <summary>
        /// Gets or sets the name of the group
        /// </summary>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }

        /// <summary>
        /// Gets or set the group visibility
        /// </summary>
        public bool Visible {
            get {
                return this.m_visible;
            }
            set {
                this.m_visible = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating if this group is displayed in the map legend
        /// </summary>
        public bool ShowInLegend {
            get {
                return this.m_showInLegend;
            }
            set {
                this.m_showInLegend = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating if this groups layers are displayed in the map legend
        /// </summary>
        public bool ExpandInLegend {
            get {
                return this.m_expandInLegend;
            }
            set {
                this.m_expandInLegend = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the label to use for this group
        /// </summary>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
    }
    
    /// <remarks/>
    public class MapLayerGroupType : MapLayerGroupCommonType {
        
        private string m_group;
        
        /// <remarks/>
        public string Group {
            get {
                return this.m_group;
            }
            set {
                this.m_group = value;
            }
        }

		protected MapDefinition m_parent = null;
		internal MapDefinition Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}
        
		/// <summary>
		/// Gets the full path of the group
		/// </summary>
		/// <param name="separator">The string used to separate the individual levels</param>
		/// <param name="parent">The parent mapdefinition, use null for the current parent</param>
		/// <returns></returns>
		public string GetFullPath(string separator, MapDefinition parent)
		{
            //TODO: Protect against infinite recursion
			if (parent == null)
				parent = m_parent;

			if (parent == null)
				throw new Exception("Cannot determine full path on group that is not attached to a map");
			if (m_group == null || m_group.Length == 0)
				return m_name;
			else
			{
				MapLayerGroupType mlg = null;
				if (parent.LayerGroups != null)
					foreach(MapLayerGroupType g in parent.LayerGroups)
						if (g.Name == m_group)
						{
							mlg = g;
							break;
						}
                if (mlg == null)
                    return null;
				return mlg.GetFullPath(separator, parent) + separator + m_name;
			}
		}
	}
        
    public class MapLayerTypeCollection : System.Collections.CollectionBase {
        
        public MapLayerType this[int idx] {
            get {
                return ((MapLayerType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(MapLayerType value) {
            return base.InnerList.Add(value);
        }

		public int IndexOf(MapLayerType value)
		{
			return base.InnerList.IndexOf(value);
		}
    }
    
    public class MapLayerGroupTypeCollection : System.Collections.CollectionBase {
        
        public MapLayerGroupType this[int idx] {
            get {
                return ((MapLayerGroupType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(MapLayerGroupType value) {
            return base.InnerList.Add(value);
        }
	
		public int IndexOf(MapLayerGroupType value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
    
    public class DoubleCollection : System.Collections.CollectionBase {
        
        public double this[int idx] {
            get {
                return ((double)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(System.Double value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class BaseMapLayerGroupCommonTypeCollection : System.Collections.CollectionBase {
        
        public BaseMapLayerGroupCommonType this[int idx] {
            get {
                return ((BaseMapLayerGroupCommonType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(BaseMapLayerGroupCommonType value) {
            return base.InnerList.Add(value);
        }

        public void Insert(int index, BaseMapLayerGroupCommonType value)
        {
            base.InnerList.Insert(index, value);
        }

        public int IndexOf(BaseMapLayerGroupCommonType value)
        {
            return base.InnerList.IndexOf(value);
        }
    }
    
    public class BaseMapLayerTypeCollection : System.Collections.CollectionBase {
        
        public BaseMapLayerType this[int idx] {
            get {
                return ((BaseMapLayerType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(BaseMapLayerType value) {
            return base.InnerList.Add(value);
        }

        public void Insert(int index, BaseMapLayerType value)
        {
            base.InnerList.Insert(index, value);
        }

        public int IndexOf(BaseMapLayerType value)
        {
            return base.InnerList.IndexOf(value);
        }

    }
}
