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

namespace OSGeo.MapGuide.MaestroAPI.RuntimeClasses
{
	/// <summary>
	/// Implementation of the runtime map. This class is prone to breaking if the MapGuide server code changes.
	/// An implementation of RFC 21 can correct this problem.
	/// </summary>
	public class RuntimeMap : OSGeo.MapGuide.MaestroAPI.MapDefinition 
	{

		protected string m_objid;
		protected const int MgBinaryVersion = 262144; //1;
		protected const int ClassId = 11500; //30500;
		protected double[] m_center = new double[2];
		protected double m_scale;
		protected ResourceIdentifier m_resourceID;

		protected new RuntimeMapLayerCollection m_mapLayer;
		protected new RuntimeMapGroupCollection m_mapLayerGroup;

		Box2DType m_dataExtent = new Box2DType();
		int m_displaydpi;
		int m_display_width;
		int m_display_height;
		double m_metersPerUnit;
		int m_layerRefreshMode;

		double[] m_finiteScales = new double[0];
		ArrayList m_changeList = new ArrayList();
		byte[] m_layersGroupBlob = new byte[0];

#if DEBUG
		public RuntimeMap(BinarySerializer.MgBinaryDeserializer ds, BinarySerializer.MgBinaryDeserializer ds2)
			: this()
		{
			Deserialize(ds);
			DeserializeLayerData(ds2);
		}
#endif

		internal RuntimeMap()
			: base()
		{
			m_mapLayer = new RuntimeMapLayerCollection();
			m_mapLayerGroup = new RuntimeMapGroupCollection();
		}

		public RuntimeMap(OSGeo.MapGuide.MaestroAPI.MapDefinition map)
			: this()
		{
			if (map.CurrentConnection == null)
				throw new Exception("Map must have a connection for construction to work");
            this.CurrentConnection = map.CurrentConnection;
			this.m_resourceID = map.ResourceId;
			base.m_resourceId = map.ResourceId;
			base.m_name = map.Name;
			this.m_objid = Guid.NewGuid().ToString();
			this.m_center[0] = ((map.Extents.MaxX - map.Extents.MinX) / 2) + map.Extents.MinX;
			this.m_center[1] = ((map.Extents.MaxY - map.Extents.MinY) / 2) + map.Extents.MinY;
			base.m_extents = new Box2DType();
			base.m_extents.MinX = map.Extents.MinX;
			base.m_extents.MaxX = map.Extents.MaxX;
			base.m_extents.MinY = map.Extents.MinY;
			base.m_extents.MaxY = map.Extents.MaxY;
			m_dataExtent.MinX = map.Extents.MinX;
			m_dataExtent.MinY = map.Extents.MinY;
			m_dataExtent.MaxX = map.Extents.MaxX;
			m_dataExtent.MaxY = map.Extents.MaxY;
			base.m_backgroundColor = map.BackgroundColor;
			base.m_coordinateSystem = map.CoordinateSystem;
			this.m_metersPerUnit = 1.0; 
			//TODO: Create coordsys and get real Meters Per Unit from Coordsys

			if (this.m_mapLayer == null)
				this.m_mapLayer = new RuntimeMapLayerCollection();

			int dispIndex = 0;
			foreach(OSGeo.MapGuide.MaestroAPI.MapLayerType layer in map.Layers)
			{
				if (layer.Parent == null)
					layer.Parent = map;
				RuntimeMapLayer rtl = new RuntimeMapLayer(layer);
                rtl.SetParent(this);
				rtl.DisplayOrder = (++dispIndex) * 1000;
				this.m_mapLayer.Add(rtl);

			}

			foreach(OSGeo.MapGuide.MaestroAPI.MapLayerGroupType group in map.LayerGroups)
			{
				if (group.Parent != null)
					group.Parent = map;
				RuntimeMapGroup rtg = new RuntimeMapGroup(group);
				this.m_mapLayerGroup.Add(rtg);
			}

            if (map.BaseMapDefinition != null && map.BaseMapDefinition.BaseMapLayerGroup != null)
                foreach (OSGeo.MapGuide.MaestroAPI.BaseMapLayerGroupCommonType group in map.BaseMapDefinition.BaseMapLayerGroup)
                {
                    if (group.BaseMapLayer != null)
                        foreach(BaseMapLayerType layer in group.BaseMapLayer)
                        {
                            if (layer.Parent == null)
                                layer.Parent = map;
				            RuntimeMapLayer rtl = new RuntimeMapLayer(layer, group.Name, true);
                            rtl.Type = OSGeo.MapGuide.MgLayerType.BaseMap;
                            rtl.SetParent(this);
				            rtl.DisplayOrder = (++dispIndex) * 1000;
				            this.m_mapLayer.Add(rtl);
                        }

                    RuntimeMapGroup rtg = new RuntimeMapGroup(group);
                    this.m_mapLayerGroup.Add(rtg);
                }

			if (map.BaseMapDefinition != null && map.BaseMapDefinition.FiniteDisplayScale != null)
			{
				m_finiteScales = new double[map.BaseMapDefinition.FiniteDisplayScale.Count];
				for(int i =0; i < map.BaseMapDefinition.FiniteDisplayScale.Count; i++)
					m_finiteScales[i] = map.BaseMapDefinition.FiniteDisplayScale[i];
			}
			else
				m_finiteScales = new double[0];

			/*if (this.Layers.Count > 0)
			{
				SortedList sl = new SortedList();
				foreach(RuntimeMapLayer l in this.Layers)
					foreach(double d in l.Scaleranges)
						if (d > 0.0 && !sl.ContainsKey(d))
							sl.Add(d, d);

				int i = 0;
				m_finiteScales = new double[sl.Count];
				foreach(double d in sl.Keys)
					m_finiteScales[i++] = d;
			}
			else
				m_finiteScales = new double[] {0.0, RuntimeMapLayer.InfinityScale };*/
			

			m_layerRefreshMode = 1;
			m_scale = 500;
			m_displaydpi = 96;
			m_display_width = 1024;
			m_display_height = 768;

		}

		public new RuntimeMapLayerCollection Layers
		{ 
			get { return m_mapLayer; } 
			set { m_mapLayer = value; }
		}

		public new RuntimeMapGroupCollection LayerGroups
		{ 
			get { return m_mapLayerGroup; } 
			set { m_mapLayerGroup = value; }
		}

		internal void Serialize(BinarySerializer.MgBinarySerializer s)
		{
			if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				s.Write(MgBinaryVersion);
				s.WriteResourceIdentifier(m_resourceID);
			}

			s.Write(base.m_name);
			s.Write(m_objid);
			s.WriteResourceIdentifier(base.m_resourceId);
			s.Write(base.m_coordinateSystem);
			base.m_extents.Serialize(s);
			s.WriteCoordinates(m_center, 0);
			s.Write(m_scale);
			m_dataExtent.Serialize(s);
			s.Write(m_displaydpi);
			s.Write(m_display_width );
			s.Write(m_display_height);
			s.Write(Utility.SerializeHTMLColor(base.m_backgroundColor, true));
			s.Write(m_metersPerUnit);

			if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				s.Write(m_layerRefreshMode);
				
			s.Write(m_finiteScales.Length);
			foreach(double d in m_finiteScales)
				s.Write(d);


			if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{

				SerializeChangeMap(s);
				s.Write((int)0);
			}
			else
			{
				SerializeLayerData(s);
				SerializeChangeMap(s);
			}
		}

		internal void SerializeLayerData(BinarySerializer.MgBinarySerializer s)
		{
			if (m_mapLayerGroup == null)
				s.Write((int)0);
			else
			{
				s.Write((int)m_mapLayerGroup.Count);
				foreach(RuntimeMapGroup g in m_mapLayerGroup)
					g.Serialize(s);
			}

			if (m_mapLayer == null)
				s.Write((int)0);
			else
			{
				s.Write(m_mapLayer.Count);
				foreach(RuntimeMapLayer t in m_mapLayer)
					t.Serialize(s);
			}
		}

		protected void SerializeChangeMap(BinarySerializer.MgBinarySerializer s)
		{
			s.Write(m_changeList.Count);
			foreach(ChangeList cl in m_changeList)
			{
				s.Write(cl.IsLayer);
				s.Write(cl.ObjectId);

				s.Write(cl.Changes.Count);
				foreach(Change c in cl.Changes)
				{
					s.Write((int)c.Type);
					s.Write(c.Params);
				}
			}
		}


		internal void Deserialize(BinarySerializer.MgBinaryDeserializer d)
		{
			if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				if (d.ReadInt32() != MgBinaryVersion)
					throw new Exception("Invalid map version");
				m_resourceID = d.ReadResourceIdentifier();
			}


			base.m_name = d.ReadString();
			m_objid = d.ReadString();
		
			base.m_resourceId = d.ReadResourceIdentifier();


			base.m_coordinateSystem = d.ReadString();
			base.m_extents = new Box2DType();
			base.m_extents.Deserialize(d);
			m_center = d.ReadCoordinates();
			m_scale = d.ReadDouble();

			m_dataExtent.Deserialize(d);
			m_displaydpi = d.ReadInt32();
			m_display_width = d.ReadInt32();
			m_display_height = d.ReadInt32();
			base.m_backgroundColor = Utility.ParseHTMLColor(d.ReadString());
			m_metersPerUnit = d.ReadDouble();

			if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				m_layerRefreshMode = d.ReadInt32();

			ArrayList finiteScales = new ArrayList();
			int finiteScaleCount = d.ReadInt32();
			while(finiteScaleCount-- > 0)
				finiteScales.Add(d.ReadDouble());
			m_finiteScales = (double[])finiteScales.ToArray(typeof(double));

			m_changeList = new ArrayList();
			if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				m_changeList = DeserializeChangeMap(d);

				int mapLayerCount = d.ReadInt32();
				if (mapLayerCount != 0)
					throw new Exception("On new versions, there should be no layer data in map");
			}
			else
			{
				//ri.LayerGroupBlob = d.ReadStreamRepeat(d.ReadInt32());
			
				DeserializeLayerData(d);
				m_changeList = DeserializeChangeMap(d);
			}
		}

		internal void DeserializeLayerData(BinarySerializer.MgBinaryDeserializer d)
		{
			//if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				int groupCount = d.ReadInt32();
				m_mapLayerGroup = new RuntimeMapGroupCollection();
				for(int i = 0; i < groupCount; i++)
				{
					RuntimeMapGroup g = new RuntimeMapGroup();
					g.Deserialize(d);
					m_mapLayerGroup.Add(g);
				}

			}

			int mapLayerCount = d.ReadInt32();
			m_mapLayer = new RuntimeMapLayerCollection();
			while (mapLayerCount-- > 0)
			{
				RuntimeMapLayer t = new RuntimeMapLayer();
				t.Deserialize(d);
				m_mapLayer.Add(t);
			}
		}

		protected ArrayList DeserializeChangeMap(BinarySerializer.MgBinaryDeserializer d)
		{
			int changeListCount = d.ReadInt32();
			ArrayList l = new ArrayList();
			while(changeListCount-- > 0)
			{
				bool isLayer = d.ReadByte() > 0;
				string objid = d.ReadString();

				ChangeList c = new ChangeList(objid, isLayer);

				int changeCount = d.ReadInt32();
				while(changeCount-- > 0)
				{
					//Split up to avoid dependency on argument evaluation order
					int ctype = d.ReadInt32();
					c.Changes.Add(new Change((Change.ChangeType)ctype, d.ReadString()));
					
				}
				l.Add(c);
			}
			return l;
		}

		public string ObjectID
		{
			get { return m_objid; }
			set { m_objid = value; }
		}

		public double[] Center
		{
			get { return m_center; }
			set { m_center = value; }
		}

		public double Scale
		{
			get { return m_scale; }
			set { m_scale = value; }
		}

		public Box2DType DataExtent
		{
			get { return m_dataExtent; }
			set { m_dataExtent = value; }
		}

		public int DisplayDPI 
		{
			get { return m_displaydpi; }
			set { m_displaydpi = value; }
		}

		public int DisplayWidth
		{
			get { return m_display_width; }
			set { m_display_width = value; }
		}

		public int DisplayHeight
		{
			get { return m_display_height; }
			set { m_display_height = value; }
		}

		public double MetersPerUnit
		{
			get { return m_metersPerUnit; }
			set { m_metersPerUnit = value; }
		}

		public int LayerRefreshMode
		{
			get { return m_layerRefreshMode; }
			set { m_layerRefreshMode = value; }
		}

		public double[] FiniteScales
		{
			get { return m_finiteScales; }
			set { m_finiteScales = value; }
		}

		public ArrayList ChangeList
		{
			get { return m_changeList; }
			set { m_changeList = value; }
		}

		public byte[] LayerGroupBlob
		{
			get { return m_layersGroupBlob; }
			set { m_layersGroupBlob = value; }
		}

		public ResourceIdentifier ResourceID
		{
			get { return m_resourceID; }
			set { m_resourceID = value; }
		}

		/// <summary>
		/// Gets or sets the connection used in various operations performed on this object
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public new ServerConnectionI CurrentConnection
		{
			get { return m_serverConnection; }
			set 
			{ 
				m_serverConnection = value;
				foreach(RuntimeMapLayer layer in this.Layers)
					layer.SetParent(this);
				foreach(RuntimeMapGroup group in this.LayerGroups)
					group.SetParent(this);
			}
		}


	}

	public class RuntimeMapGroup
		: BaseMapLayerType 
	{
		private int m_type;
		private bool m_visible;
		private string m_parentGroup;
		protected string m_objectid;

		public new RuntimeMap Parent { get { return m_parent; } }
		internal void SetParent(RuntimeMap parent) { m_parent = parent; }
		protected new RuntimeMap m_parent = null;

		public RuntimeMapGroup()
			: base()
		{
			m_type = MgLayerGroupType.Normal;
			m_objectid = Guid.NewGuid().ToString();
		}

        public RuntimeMapGroup(BaseMapLayerGroupCommonType group)
            : this()
        {
            base.ExpandInLegend = group.ExpandInLegend;
            base.LegendLabel = group.LegendLabel;
            base.Name = group.Name;
            base.ShowInLegend = group.ShowInLegend;

            m_visible = group.Visible;
            m_parentGroup = "";
            m_objectid = Guid.NewGuid().ToString();
            m_type = MgLayerGroupType.BaseMap;
        }


		public RuntimeMapGroup(MapLayerGroupType group)
			: this()
		{
			base.ExpandInLegend = group.ExpandInLegend;
			base.LegendLabel = group.LegendLabel;
			base.Name = group.Name;
			base.ShowInLegend = group.ShowInLegend;

			m_visible = group.Visible;
			m_parentGroup = group.Group;
			m_objectid = Guid.NewGuid().ToString();
		}
		
		internal void Deserialize(BinarySerializer.MgBinaryDeserializer d)
		{
			m_parentGroup = d.ReadString();
			int objid = d.ReadClassId();
			if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				if (objid != 12001)
					throw new Exception("Group must have object id 12001, but had: " + objid);
			}
			else if (objid != 19001)
				throw new Exception("Group must have object id 19001, but had: " + objid);
			
			base.m_name = d.ReadString();
			m_objectid = d.ReadString();
			m_type = d.ReadInt32();
			m_visible = d.ReadBool();
			base.m_showInLegend = d.ReadBool();
			base.m_expandInLegend = d.ReadBool();
			base.m_legendLabel = d.ReadString();
		}

		internal void Serialize(BinarySerializer.MgBinarySerializer s)
		{
			s.Write(m_parentGroup);
			if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				s.WriteClassId(12001);
			else
				s.WriteClassId(19001);
			s.Write(base.m_name);
			s.Write(m_objectid);
			s.Write(m_type);
			s.Write(m_visible);
			s.Write(base.m_showInLegend);
			s.Write(base.m_expandInLegend);
			s.Write(base.m_legendLabel);
		}

		public string ParentGroup 
		{ 
			get { return m_parentGroup; }
			set { m_parentGroup = value; }
		}

		public int Type 
		{
			get { return m_type; }
			set { m_type = value; }
		}

		public bool Visible
		{
			get { return m_visible; }
			set { m_visible = value; }
		}

		public string ObjectID 
		{
			get { return m_objectid; }
			set { m_objectid = value; }
		}

		/// <summary>
		/// Gets the full path of the group
		/// </summary>
		/// <param name="separator">The string used to separate the individual levels</param>
		/// <param name="parent">The parent mapdefinition, use null for the current parent</param>
		/// <returns></returns>
		public string GetFullPath(string separator, RuntimeMap parent)
		{
			if (parent == null)
				parent = m_parent;

			if (parent == null)
				throw new Exception("Cannot determine full path on group that is not attached to a map");
			if (m_parentGroup == null || m_parentGroup.Length == 0)
				return m_name;
			else
			{
				RuntimeMapGroup mlg = null;
				if (parent.LayerGroups != null)
					foreach(RuntimeMapGroup g in parent.LayerGroups)
						if (g.Name == m_parentGroup)
						{
							mlg = g;
							break;
						}
				if (mlg == null)
					throw new Exception("Failed to locate group named: " + m_parentGroup);
				return mlg.GetFullPath(separator, parent) + separator + m_name;
			}
		}

		/// <summary>
		/// Returns a list of layers present in the current group. This method does NOT run in O(n).
		/// </summary>
		public RuntimeMapLayerCollection Layers
		{
			get 
			{
				if (m_parent == null)
					return null;

				string s = this.GetFullPath("/", m_parent);
				RuntimeMapLayerCollection layers = new RuntimeMapLayerCollection();
				foreach(RuntimeMapLayer ml in m_parent.Layers)
					if (ml.GetFullPath("/", m_parent).StartsWith(s))
						layers.Add(ml);

				return layers;
			}
		}

	}

	public class RuntimeMapLayer
		: MapLayerType
	{
		protected double[] m_scaleRanges;
		protected int m_type;
		protected ArrayList m_ids;
		protected bool m_needRefresh;
		protected double m_displayOrder;
		protected string m_featureSourceId;
		protected string m_featureName;
		protected string m_geometry;
		protected string m_guid;

		//V 1.2 Runtime extensions
		protected bool m_hasTooltips = false;
		protected string m_schemaName = "";

		public new RuntimeMap Parent { get { return m_parent; } }
		internal void SetParent(RuntimeMap parent) { m_parent = parent; }
		protected new RuntimeMap m_parent = null;

		public const double InfinityScale = 1000000000000.0;

		public RuntimeMapLayer(RuntimeMap parent)
			: this()
		{
			this.m_parent = parent;
		}

		public RuntimeMapLayer()
			: base()
		{
			m_scaleRanges = new double[] {0.0, InfinityScale};
			m_type = OSGeo.MapGuide.MgLayerType.Dynamic;
			m_ids = new ArrayList();
			m_schemaName = "";

			m_featureName = "";
			m_featureSourceId = "";
			m_geometry = "";
			m_guid = System.Guid.NewGuid().ToString();
		}

        public RuntimeMapLayer(OSGeo.MapGuide.MaestroAPI.BaseMapLayerType layer, string group, bool visible)
            : this()
        {
            base.m_resourceId = layer.ResourceId;
            base.m_expandInLegend = layer.ExpandInLegend;
            base.m_group = group;
            base.m_legendLabel = layer.LegendLabel;
            base.m_name = layer.Name;
            base.m_selectable = layer.Selectable;
            base.m_showInLegend = layer.ShowInLegend;
            base.m_visible = visible;

            OSGeo.MapGuide.MaestroAPI.LayerDefinition ldef = layer.Parent.CurrentConnection.GetLayerDefinition(layer.ResourceId);
            if (ldef.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
            {
                OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vld = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)ldef.Item;
                this.m_needRefresh = false;
                this.m_displayOrder = 0;

                this.m_featureSourceId = vld.ResourceId;
                this.m_featureName = vld.FeatureName;
                this.m_schemaName = vld.FeatureName.IndexOf(":") > 0 ? vld.FeatureName.Substring(0, vld.FeatureName.IndexOf(":")) : "";
                this.m_geometry = vld.Geometry;

                if (vld.VectorScaleRange != null)
                {
                    m_scaleRanges = new double[vld.VectorScaleRange.Count * 2];
                    for (int i = 0; i < vld.VectorScaleRange.Count; i++)
                    {
                        m_scaleRanges[i * 2] = vld.VectorScaleRange[i].MinScaleSpecified ? vld.VectorScaleRange[i].MinScale : 0;
                        m_scaleRanges[i * 2 + 1] = vld.VectorScaleRange[i].MaxScaleSpecified ? vld.VectorScaleRange[i].MaxScale : InfinityScale;
                    }
                }
                m_hasTooltips = (vld.ToolTip != null && vld.ToolTip.Trim().Length > 0) || (vld.Url != null && vld.Url.Trim().Length > 0);

                if (base.m_selectable && base.m_visible)
                    try { FindResourceIDs(layer.Parent.CurrentConnection); }
                    catch { }

            }
            else if (ldef.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType != null)
            {
                OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld = (OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType)ldef.Item;
                this.m_needRefresh = false;
                this.m_displayOrder = 0;

                this.m_featureSourceId = gld.ResourceId;
                this.m_featureName = gld.FeatureName;
                this.m_geometry = gld.Geometry;

                if (gld.GridScaleRange != null)
                {
                    m_scaleRanges = new double[gld.GridScaleRange.Count * 2];
                    for (int i = 0; i < gld.GridScaleRange.Count; i++)
                    {
                        m_scaleRanges[i * 2] = gld.GridScaleRange[i].MinScaleSpecified ? gld.GridScaleRange[i].MinScale : 0;
                        m_scaleRanges[i * 2 + 1] = gld.GridScaleRange[i].MaxScaleSpecified ? gld.GridScaleRange[i].MaxScale : InfinityScale;
                    }
                }
            }
            else if (ldef.Item as OSGeo.MapGuide.MaestroAPI.DrawingLayerDefinitionType != null)
            {
                OSGeo.MapGuide.MaestroAPI.DrawingLayerDefinitionType dld = (OSGeo.MapGuide.MaestroAPI.DrawingLayerDefinitionType)ldef.Item;
                this.m_needRefresh = false;
                this.m_displayOrder = 0;

                throw new Exception("Drawing layers are not support in runtime map creation");
            }
            else
            {
                throw new Exception("Layer " + ldef.ResourceId + " had an invalid or unknown type");
            }

            if (m_ids == null)
                m_ids = new ArrayList();

        }

		public RuntimeMapLayer(OSGeo.MapGuide.MaestroAPI.MapLayerType layer)
			: this(layer, layer.Group, layer.Visible)
		{
		}


		/// <summary>
		/// Reads the primary key information from the layers featuresource
		/// </summary>
		private void FindResourceIDs(ServerConnectionI con)
		{
			OSGeo.MapGuide.MaestroAPI.FeatureSource fs = con.GetFeatureSource(m_featureSourceId);
            //TODO: Should not be hardcoded, but it is the fastest way!
            if (fs.Provider.StartsWith("OSGeo.Gdal") || fs.Provider.StartsWith("OSGeo.WMS") || fs.Provider.StartsWith("Autodesk.Raster"))
            {
                if (m_ids == null)
                    m_ids = new ArrayList();
                return;
            }

			string[] ids = fs.GetIdentityProperties(m_featureName);
			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm = con.GetFeatureSourceSchema(m_featureSourceId, m_featureName);

			if (scm != null)
			{
				m_ids = new ArrayList();
				foreach(string id in ids)
					foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn fsc in scm.Columns)
						if (fsc.Name == id)
						{
							m_ids.Add(new object[] {(short)Utility.ConvertNetTypeToMgType(fsc.Type), id});
							break;
						}
			}
		}


		/// <summary>
		/// Reads the primary key information from the layers featuresource
		/// </summary>
		private void FindResourceIDs()
		{
			if (this.Parent == null)
				throw new Exception("Cannot reload ID's because the layers parent is not set");

			FindResourceIDs(this.Parent.CurrentConnection);
		}

		internal void Deserialize(BinarySerializer.MgBinaryDeserializer d)
		{
			m_group = d.ReadString();

			int classid = d.ReadClassId();
			if (d.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 19003)
				throw new Exception("Resource Identifier expected, but got: " + classid.ToString());
			if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 30501)
				throw new Exception("Resource Identifier expected, but got: " + classid.ToString());

			m_resourceId = d.ReadResourceIdentifier();
			
			if (d.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				m_name = d.ReadString();
				m_guid = d.ReadString();
				m_type = d.ReadInt32();

				m_visible = d.ReadByte() > 0;
				m_selectable = d.ReadByte() > 0;
				m_showInLegend = d.ReadByte() > 0;
				m_expandInLegend = d.ReadByte() > 0;

				m_legendLabel = d.ReadString();
				m_needRefresh = d.ReadByte() > 0;
				m_displayOrder = d.ReadDouble();

				ArrayList scaleRanges = new ArrayList();
				int scales = d.ReadInt32();
				while(scales-- > 0)
					scaleRanges.Add(d.ReadDouble());
				m_scaleRanges = (double[])scaleRanges.ToArray(typeof(double));

				m_featureSourceId = d.ReadString();
				m_featureName = d.ReadString();
				m_geometry = d.ReadString();

				ArrayList ids = new ArrayList();
				int idCount = d.ReadInt32();

				while(idCount-- > 0)
				{
					short idType = d.ReadInt16();
					string idName = d.ReadString();
					ids.Add(new object[] { idType, idName } );
				}

				m_ids = ids;
			}
			else
			{
				//AAARGH!!! Now they bypass their own header system ....
				m_name = d.ReadInternalString();
				m_guid = d.ReadInternalString();
				m_type = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);

				int flags = d.ReadStreamRepeat(1)[0];
				m_visible = (flags & 1) > 0;
				m_selectable = (flags & 2) > 0;
				m_showInLegend = (flags & 4) > 0;
				m_expandInLegend = (flags & 8) > 0;
				m_needRefresh = (flags & 16) > 0;
				m_hasTooltips = (flags & 32) > 0;

				m_legendLabel = d.ReadInternalString();
				m_displayOrder = BitConverter.ToDouble(d.ReadStreamRepeat(8), 0);

				ArrayList scaleRanges = new ArrayList();
				int scales = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);
				while(scales-- > 0)
					scaleRanges.Add(BitConverter.ToDouble(d.ReadStreamRepeat(8), 0));
				m_scaleRanges = (double[])scaleRanges.ToArray(typeof(double));

				m_featureSourceId = d.ReadInternalString();
				m_featureName = d.ReadInternalString();
				m_schemaName = d.ReadInternalString();
				m_geometry = d.ReadInternalString();

				ArrayList ids = new ArrayList();
				int idCount = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);

				while(idCount-- > 0)
				{
					short idType = BitConverter.ToInt16(d.ReadStreamRepeat(2), 0);
					string idName = d.ReadInternalString();
					ids.Add(new object[] { idType, idName } );
				}

				m_ids = ids;
			}
		}

		internal void Serialize(BinarySerializer.MgBinarySerializer s)
		{
			s.Write(m_group);

			if (s.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
				s.WriteClassId(19003);
			else
				s.WriteClassId(30501);

			s.WriteResourceIdentifier(m_resourceId);

			if (s.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
			{
				s.Write(m_name);
				s.Write(m_guid);

				s.Write(m_type);

				s.Write((byte)(m_visible ? 1 : 0));
				s.Write((byte)(m_selectable ? 1 : 0));
				s.Write((byte)(m_showInLegend ? 1 : 0));
				s.Write((byte)(m_expandInLegend ? 1 : 0));

				s.Write(m_legendLabel);
				s.Write((byte)(m_needRefresh ? 1 : 0));
				s.Write(m_displayOrder);

				s.Write(m_scaleRanges.Length);
				foreach(double d in m_scaleRanges)
					s.Write(d);
			
				s.Write(m_featureSourceId);
				s.Write(m_featureName);
				s.Write(m_geometry);

				s.Write(m_ids.Count);
				foreach(object[] x in m_ids)
				{
					s.Write((short)x[0]);
					s.Write((string)x[1]);
				}
			}
			else
			{
				s.WriteStringInternal(m_name);
				s.WriteStringInternal(m_guid);
				s.WriteRaw(BitConverter.GetBytes(m_type));
				int flags = 0;
				flags |= m_visible ? 1 : 0;
				flags |= m_selectable ? 2 : 0;
				flags |= m_showInLegend ? 4 : 0;
				flags |= m_expandInLegend ? 8 : 0;
				flags |= m_needRefresh ? 16 : 0;
				flags |= m_hasTooltips ? 32 : 0;
				s.WriteRaw(new byte[] {(byte)flags});

				s.WriteStringInternal(m_legendLabel);
				s.WriteRaw(BitConverter.GetBytes(m_displayOrder));

				s.WriteRaw(BitConverter.GetBytes(m_scaleRanges.Length));
				foreach(double d in m_scaleRanges)
					s.WriteRaw(BitConverter.GetBytes(d));

				s.WriteStringInternal(m_featureSourceId);
				s.WriteStringInternal(m_featureName);
				s.WriteStringInternal(m_schemaName);
				s.WriteStringInternal(m_geometry);

				s.WriteRaw(BitConverter.GetBytes(m_ids.Count));
				foreach(object[] x in m_ids)
				{
					s.WriteRaw(BitConverter.GetBytes((short)x[0]));
					s.WriteStringInternal((string)x[1]);
				}


			}
		}

		
		public double DisplayOrder 
		{ 
			get { return m_displayOrder; }
			set { m_displayOrder = value; }
		}

		public bool NeedRefresh
		{
			get { return m_needRefresh; }
			set { m_needRefresh = value; }
		}

		public string FeatureSourceID 
		{
			get { return m_featureSourceId;}
			set { m_featureSourceId = value; }
		}
		public string FeatureName
		{
			get { return m_featureName; }
			set { m_featureName = value; }
		}

		public string Geometry
		{
			get { return m_geometry; }
			set { m_geometry = value; }
		}

		public string Guid 
		{
			get { return m_guid; }
			set { m_guid = value; }
		}

		public FeatureSetReader Query()
		{
			return Query(null, null, null);
		}

		public FeatureSetReader Query(string filter)
		{
			return Query(filter, null, null);
		}

        public FeatureSetReader Query(string filter, string[] columns)
        {
            return Query(filter, columns, null);
        }

		public FeatureSetReader Query(string filter, string[] columns, System.Collections.Specialized.NameValueCollection computedProperties)
		{
			//TODO: This is SLOW, and also breaks the OGR provider :(
			/*
			//Merge in the layer filter
			OSGeo.MapGuide.MaestroAPI.LayerDefinition ldef = m_parent.CurrentConnection.GetLayerDefinition(this.ResourceId);
			if (ldef.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType))
			{
				string f = ((OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)ldef.Item).Filter;
				if (f != null && f.Trim().Length != 0)
					filter = "(" + filter + ") AND (" + f + ")";
			}
			*/

			return m_parent.CurrentConnection.QueryFeatureSource(this.FeatureSourceID, this.FeatureName, filter, columns, computedProperties);
		}

		/// <summary>
		/// Gets the full path of the layer
		/// </summary>
		/// <param name="separator">The string used to separate the individual levels</param>
		/// <param name="parent">The parent mapdefinition, use null for the current parent</param>
		/// <returns></returns>
		public string GetFullPath(string separator, RuntimeMap parent)
		{
			if (parent == null)
				parent = m_parent;

			if (parent == null)
				throw new Exception("Cannot determine full path on group that is not attached to a map");
			if (m_group == null || m_group.Length == 0)
				return m_name;
			else
			{
				RuntimeMapGroup mlg = null;
				if (parent.LayerGroups != null)
					foreach(RuntimeMapGroup g in parent.LayerGroups)
						if (g.Name == m_group)
						{
							mlg = g;
							break;
						}
				if (mlg == null)
					throw new Exception("Failed to locate layer named: " + m_group);
				return mlg.GetFullPath(separator, parent) + separator + m_name;
			}
		}


		public string SchemaName
		{
			get { return m_schemaName; }
			set { m_schemaName = value; }
		}

		public bool HasTooltips
		{
			get { return m_hasTooltips; }
			set { m_hasTooltips = value; }
		}

		public override bool Selectable
		{
			get { return m_selectable; }
			set 
			{ 
				if (m_visible && value && (m_ids == null || m_ids.Count == 0))
					FindResourceIDs();
				m_selectable = value; 
			}
		}

		public override bool Visible
		{
			get { return m_visible; }
			set 
			{ 
				if (m_selectable && value && (m_ids == null || m_ids.Count == 0))
					FindResourceIDs();
				m_visible = value; 
			}
		}

		public double[] Scaleranges
		{
			get { return m_scaleRanges; }
			set { m_scaleRanges = value; }
		}

		public int Type
		{
			get { return m_type; }
			set { m_type = value; }
		}

		public ArrayList IDs 
		{
			get 
			{ 
				if (m_ids == null || m_ids.Count == 0)
					FindResourceIDs();
				return m_ids; 
			}
			set { m_ids = value; }
		}
		
	}

	public class RuntimeMapLayerCollection : System.Collections.CollectionBase 
	{
		/// <summary>
		/// Gets or sets the index of a layer, given the layer name.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		/// <param name="name">The name of the layer</param>
		/// <returns>The index of the layer, or -1 if no such layer could be found</returns>
		public int IndexOf(string name)
		{
			int rml = -1;
			for(int i = 0; i < this.Count; i++)
				if (this[i].Name == name)
					return i;
				else if (rml == -1 && this[i].Name.ToLower() == name.ToLower())
					rml = i;

			return rml;

		}
		
		/// <summary>
		/// Gets a value indicating if the layer exists in the map.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		/// <param name="name">The name of the layer</param>
		/// <returns>True if the layer was found, false otherwise</returns>
		public bool Contains(string name)
		{
			return IndexOf(name) != -1;
		}

		/// <summary>
		/// Gets or sets a layer based on the layers name.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		public RuntimeMapLayer this[string name]
		{
			get 
			{
				int ix = IndexOf(name);
				if (ix == -1)
					throw new IndexOutOfRangeException("The layer named: " + name + " was not found in the map");
				else
					return this[ix];
			}
			set 
			{
				int ix = IndexOf(name);
				if (ix == -1)
					throw new IndexOutOfRangeException("The layer named: " + name + " was not found");
				else
					this[ix] = value;
			}
		}
        
		public RuntimeMapLayer this[int idx] 
		{
			get 
			{
				return ((RuntimeMapLayer)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(RuntimeMapLayer value) 
		{
			return base.InnerList.Add(value);
		}

		public void Insert(int index, RuntimeMapLayer layer)
		{
			base.InnerList.Insert(index, layer);
		}
	}


	public class RuntimeMapGroupCollection : System.Collections.CollectionBase 
	{
		/// <summary>
		/// Gets or sets the index of a layer, given the layer name.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		/// <param name="name">The name of the layer</param>
		/// <returns>The index of the layer, or -1 if no such layer could be found</returns>
		public int IndexOf(string name)
		{
			int rml = -1;
			for(int i = 0; i < this.Count; i++)
				if (this[i].Name == name)
					return i;
				else if (rml == -1 && this[i].Name.ToLower() == name.ToLower())
					rml = i;

			return rml;

		}
		
		/// <summary>
		/// Gets a value indicating if the layer exists in the map.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		/// <param name="name">The name of the layer</param>
		/// <returns>True if the layer was found, false otherwise</returns>
		public bool Contains(string name)
		{
			return IndexOf(name) != -1;
		}

		/// <summary>
		/// Gets or sets a layer based on the layers name.
		/// Is case sensitive but will search case insensitive if no layer matches with case sensitive.
		/// </summary>
		public RuntimeMapGroup this[string name]
		{
			get 
			{
				int ix = IndexOf(name);
				if (ix == -1)
					throw new IndexOutOfRangeException("The group named: " + name + " was not found in the map");
				else
					return this[ix];
			}
			set 
			{
				int ix = IndexOf(name);
				if (ix == -1)
					throw new IndexOutOfRangeException("The group named: " + name + " was not found");
				else
					this[ix] = value;
			}
		}
        
		public RuntimeMapGroup this[int idx] 
		{
			get 
			{
				return ((RuntimeMapGroup)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(RuntimeMapGroup value) 
		{
			return base.InnerList.Add(value);
		}

		public void Insert(int index, RuntimeMapGroup layer)
		{
			base.InnerList.Insert(index, layer);
		}
	}

}
