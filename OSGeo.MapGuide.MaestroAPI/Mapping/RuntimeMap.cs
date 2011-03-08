#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Serialization;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Commands;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    //TODO: Verify the code examples here :)

    /// <summary>
    /// Represents a runtime instance of a Map Definition
    /// </summary>
    /// <remarks>
    /// If you want to use this instance with the Rendering Service APIs, it is important to set the correct
    /// meters per unit value before calling the <see cref="T:OSGeo.MapGuide.MaestroAPI.Mapping.RuntimeMap.Save"/> method, as an incorrect meters
    /// per unit value will produce incorrect images. 
    /// 
    /// Also note that to improve the creation performance, certain implementations of <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/>
    /// offer a <see cref="T:OSGeo.MapGuide.MaestroAPI.Mapping.IRuntimeMapSetup"/> helper to return a series of layer definitions in a batch (fetching
    /// layer definitions one at a time is the main performance bottleneck for large maps), batching can improve creation times by:
    /// 
    ///  - HTTP: 2x
    ///  - Local: 3x (if using MapGuide 2.2 APIs. As this takes advantage of the GetResourceContents() API introduced in 2.2). For older versions of MapGuide there is no batching.
    /// 
    /// In particular, the HTTP implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConection"/> uses the <see cref="T:System.Threading.ThreadPool"/>
    /// class to fetch multiple layer definitions in parallel. If your code uses this implementation, be aware of this face and the performance implications
    /// involved, as an excessively large thread pool size may negatively affect stability of your MapGuide Server.
    /// 
    /// </remarks>
    /// <example>
    /// How to create a <see cref="RuntimeMap"/> with the correct meters per unit value using the MgCoordinateSystem API
    /// <code>
    /// 
    ///     IServerConnection conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", 
    ///         "Username", "Administrator",
    ///         "Password", "admin",
    ///         "Url", "http://localhost/mapguide/mapagent/mapagent.fcgi");
    ///         
    ///     //Create the Mapping Service. Some implementations of IServerConnection may not support this service, so
    ///     //its best to inspect the capability object of this connection to determine if this service type is supported
    ///     IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
    /// 
    ///     //Get our map definition
    ///     ResourceIdentifier resId = new ResourceIdentifier("Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition");
    ///     IMapDefinition mdf = (IMapDefinition)conn.ResourceService.GetResource(resId.ToString());
    ///     
    ///     //Calculate the meters per unit value, this requires the official MapGuide API. Otherwise, you need
    ///     //to know this value up-front in order to render images with this instance
    ///     double metersPerUnit = 1.0;
    ///     if (!string.IsNullOrEmpty(mdf.CoordinateSystem))
    ///     {
    ///         MgCoordinateSystemFactory factory = new MgCoordinateSystemFactory();
    ///         MgCoordinateSystem cs = factory.Create(mdf.CoordinateSystem);
    ///         metersPerUnit = cs.ConvertCoordinateSystemUnitsToMeters(1.0);
    ///     }
    ///     
    ///     //Generate our runtime map resource id. This must be session-based
    ///     ResourceIdentifier rtMapId = new ResourceIdentifier(resId.Name, ResourceTypes.RuntimeMap, conn.SessionID);
    ///     
    ///     //Create the runtime map using our meters per unit value
    ///     RuntimeMap map = mapSvc.CreateRuntimeMap(rtMapId, mdf, metersPerUnit);
    ///     
    ///     //Set some display parameters for this map
    ///     map.DisplayWidth = 1024;
    ///     map.DisplayHeight = 1024;
    ///     map.DisplayDpi = 96;
    /// 
    ///     //We have to save it first before we can render from it or use any other API that requires this 
    ///     //current map state. Remember to call Save() everytime you change the state of the map
    ///     map.Save();
    ///     
    ///     //Now we can render a map
    ///     using(Stream stream = mapSvc.RenderDynamicOverlay(map, null, "PNG"))
    ///     {
    ///         //Write this stream out to a file
    ///         using (var fs = new FileStream("RenderMap.png", FileMode.OpenOrCreate))
    ///         {
    ///             int read = 0;
    ///             do
	/// 		    {
    /// 			    read = source.Read(buf, 0, buf.Length);
    /// 			    target.Write(buf, 0, read);
    /// 		    } while (read > 0);
    ///         }
    ///     }
    /// 
    /// </code>
    /// </example>
    public class RuntimeMap : MapObservable
    {
        internal IFeatureService FeatureService { get; set; }

        internal IResourceService ResourceService { get; set; }

        internal Version SiteVersion { get; private set; }

        /// <summary>
        /// The layer collection
        /// </summary>
        protected List<RuntimeMapLayer> _layers;
        
        /// <summary>
        /// The group collection
        /// </summary>
        protected List<RuntimeMapGroup> _groups;

        /// <summary>
        /// The id to layer lookup dictionary
        /// </summary>
        protected Dictionary<string, RuntimeMapLayer> _layerIdMap;

        private IMappingService _mapSvc;
        private IGetResourceContents _getRes;

        internal RuntimeMap(IServerConnection conn)
        {
            _disableChangeTracking = true;
            
            this.SiteVersion = conn.SiteVersion;
            this.SessionId = conn.SessionID;
            this.ObjectId = Guid.NewGuid().ToString();
            m_changeList = new Dictionary<string, ChangeList>();
            _finiteDisplayScales = new double[0];
            this.ResourceService = conn.ResourceService;
            this.FeatureService = conn.FeatureService;
            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0)
            {
                _mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            }
            if (Array.IndexOf(conn.Capabilities.SupportedCommands, (int)CommandType.GetResourceContents) >= 0)
            {
                _getRes = (IGetResourceContents)conn.CreateCommand((int)CommandType.GetResourceContents);
            }
            _layers = new List<RuntimeMapLayer>();
            _groups = new List<RuntimeMapGroup>();
            _layerIdMap = new Dictionary<string, RuntimeMapLayer>();
            this.Selection = new MapSelection(this);
        }

        static IEnumerable<string> GetLayerIds(IMapDefinition mdf)
        {
            foreach (var layer in mdf.MapLayer)
            {
                yield return layer.ResourceId;
            }
            if (mdf.BaseMap != null)
            {
                foreach (var group in mdf.BaseMap.BaseMapLayerGroup)
                {
                    foreach (var layer in group.BaseMapLayer)
                    {
                        yield return layer.ResourceId;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeMap"/> class.
        /// </summary>
        /// <param name="mdf">The map definition to create this map from.</param>
        /// <param name="metersPerUnit">The meters per unit value</param>
        internal RuntimeMap(IMapDefinition mdf, double metersPerUnit)
            : this(mdf.CurrentConnection)
        {
            this.MetersPerUnit = metersPerUnit;

            this.MapDefinition = mdf.ResourceID;
            this.MapExtent = mdf.Extents.Clone();
            this.DataExtent = mdf.Extents.Clone();
            this.BackgroundColor = mdf.BackgroundColor;
            this.CoordinateSystem = mdf.CoordinateSystem;
            
            //TODO: infer real mpu from coordinate system

            //If a setup helper exists, use it to get required layers in a single
            //batch. Eliminating lots of chatter for really large maps.
            if (_getRes != null)
            {
                Trace.TraceInformation("[RuntimeMap.ctor]: Batching layer requests");
                var res = _getRes.Execute(GetLayerIds(mdf));
                //Pre-populate layer def cache so GetLayerDefinition() returns these
                //instead of making a new request
                foreach (var key in res.Keys)
                {
                    var layer = res[key] as ILayerDefinition;
                    if (layer != null)
                        layerDefinitionCache.Add(key, layer);
                }
                Trace.TraceInformation("[RuntimeMap.ctor]: {0} layers pre-cached", layerDefinitionCache.Count);
            }

            int dispIndex = 0;
            //Load map layers
            foreach (var layer in mdf.MapLayer)
            {
                var rtl = new RuntimeMapLayer(this, layer, GetLayerDefinition(layer.ResourceId));
                rtl.DisplayOrder = (++dispIndex) * 1000;
                AddLayerInternal(rtl);
            }

            //Load map groups
            foreach (var group in mdf.MapLayerGroup)
            {
                var grp = new RuntimeMapGroup(this, group);
                _groups.Add(grp);
            }

            //If base map specified load layers and groups there
            if (mdf.BaseMap != null)
            {
                var bm = mdf.BaseMap;
                foreach (var group in bm.BaseMapLayerGroup)
                {
                    if (group.HasLayers())
                    {
                        foreach (var layer in group.BaseMapLayer)
                        {
                            var rtl = new RuntimeMapLayer(this, layer, GetLayerDefinition(layer.ResourceId)) { Visible = true };
                            rtl.Type = RuntimeMapLayer.kBaseMap;
                            rtl.DisplayOrder = (++dispIndex) * 1000;
                            AddLayerInternal(rtl);
                        }
                    }
                    var rtg = new RuntimeMapGroup(this, group);
                    _groups.Add(rtg);
                }

                //Init finite display scales
                if (bm.ScaleCount > 0)
                {
                    _finiteDisplayScales = new double[bm.ScaleCount];
                    for (int i = 0; i < bm.ScaleCount; i++)
                    {
                        _finiteDisplayScales[i] = bm.GetScaleAt(i);
                    }
                }
            }

            this.LayerRefreshMode = 1;
            this.ViewScale = 1.0; //TODO: Calc from extents and other parameters
            this.DisplayDpi = 96;
            this.DisplayWidth = 0;
            this.DisplayHeight = 0;
            this.ViewCenter = this.DataExtent.Center();

            _disableChangeTracking = false;
        }

        /// <summary>
        /// Gets or sets the map extents.
        /// </summary>
        /// <value>The map extents.</value>
        public IEnvelope MapExtent
        {
            get;
            private set;
        }
        
        /// <summary>
        /// The data extent
        /// </summary>
        protected IEnvelope _dataExtent;

        /// <summary>
        /// Gets or sets the data extent.
        /// </summary>
        /// <value>The data extent.</value>
        public IEnvelope DataExtent
        {
            get
            {
                return _dataExtent;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (_dataExtent == null)
                {
                    _dataExtent = value;
                }
                else
                {
                    _dataExtent.MaxX = value.MaxX;
                    _dataExtent.MaxY = value.MaxY;
                    _dataExtent.MinX = value.MinX;
                    _dataExtent.MinY = value.MinY;
                }
            }
        }

        /// <summary>
        /// The dpi
        /// </summary>
        protected int _dpi;

        /// <summary>
        /// Gets or sets the display dpi.
        /// </summary>
        /// <value>The display dpi.</value>
        public int DisplayDpi
        {
            get
            {
                return _dpi;
            }
            set
            {
                SetField(ref _dpi, value, "DisplayDpi");
            }
        }

        /// <summary>
        /// The display height
        /// </summary>
        protected int _dispHeight;

        /// <summary>
        /// Gets or sets the display height.
        /// </summary>
        /// <value>The display height.</value>
        public int DisplayHeight
        {
            get
            {
                return _dispHeight;
            }
            set
            {
                _dispHeight = value;
            }
        }

        /// <summary>
        /// The display width
        /// </summary>
        protected int _dispWidth;

        /// <summary>
        /// Gets or sets the display width.
        /// </summary>
        /// <value>The display width.</value>
        public int DisplayWidth
        {
            get
            {
                return _dispWidth;
            }
            set
            {
                _dispWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the map definition resource id
        /// </summary>
        /// <value>The map definition resource id.</value>
        public string MapDefinition
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        /// <value>The object id.</value>
        public string ObjectId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId
        {
            get;
            internal set;
        }

        /// <summary>
        /// The view center
        /// </summary>
        protected IPoint2D _viewCenter;

        /// <summary>
        /// Gets or sets the view center.
        /// </summary>
        /// <value>The view center.</value>
        public IPoint2D ViewCenter
        {
            get
            {
                return _viewCenter;
            }
            set
            {
                _viewCenter = value;
            }
        }

        /// <summary>
        /// The view scale
        /// </summary>
        protected double _viewScale;

        /// <summary>
        /// Gets or sets the view scale.
        /// </summary>
        /// <value>The view scale.</value>
        public double ViewScale
        {
            get
            {
                return _viewScale;
            }
            set
            {
                SetField(ref _viewScale, value, "ViewScale");
            }
        }

        /// <summary>
        /// The name of the map
        /// </summary>
        protected string _name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }

        /// <summary>
        /// The Coordinate System WKT of the map
        /// </summary>
        protected string _mapSrs;

        /// <summary>
        /// Gets or sets the coordinate system in WKT format
        /// </summary>
        /// <value>The coordinate system in WKT format.</value>
        public string CoordinateSystem
        {
            get { return _mapSrs; }
            internal set { SetField(ref _mapSrs, value, "CoordinateSystem"); }
        }

        /// <summary>
        /// The background color of the map
        /// </summary>
        protected System.Drawing.Color _bgColor;

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public System.Drawing.Color BackgroundColor
        {
            get
            {
                return _bgColor;
            }
            set
            {
                SetField(ref _bgColor, value, "BackgroundColor");
            }
        }

        private string _resId;

        /// <summary>
        /// Gets or sets the resource ID. When setting, if the name of this map has 
        /// not been specified already, the name will be set based on this resource id
        /// </summary>
        /// <value>The resource ID.</value>
        public string ResourceID
        {
            get { return _resId; }
            set 
            { 
                SetField(ref _resId, value, "ResourceID");
                if (this.Name == null)
                    this.Name = ResourceIdentifier.GetName(_resId);
            }
        }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        public ResourceTypes ResourceType
        {
            get { return ResourceTypes.RuntimeMap; }
        }

        /// <summary>
        /// Gets the meters per unit value.
        /// </summary>
        /// <value>The meters per unit.</value>
        public double MetersPerUnit
        {
            get;
            internal set;
        }

        /// <summary>
        /// MapGuide internal value
        /// </summary>
        protected const int MgBinaryVersion = 262144; //1;

        /// <summary>
        /// MapGuide internal class id
        /// </summary>
        protected const int ClassId = 11500; //30500;

        /// <summary>
        /// Gets the layer refresh mode.
        /// </summary>
        /// <value>The layer refresh mode.</value>
        public int LayerRefreshMode
        {
            get;
            private set;
        }

        private double[] _finiteDisplayScales;

        /// <summary>
        /// Serializes this instance to the specified binary stream
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(MgBinarySerializer s)
        {
            if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                s.Write(MgBinaryVersion);
                s.WriteResourceIdentifier(this.ResourceID);
            }

            s.Write(this.Name);
            s.Write(this.ObjectId);
            s.WriteResourceIdentifier(this.MapDefinition);
            s.Write(this.CoordinateSystem);
            //base.m_extents.Serialize(s);
            SerializeExtent(this.MapExtent, s);
            s.WriteCoordinates(new double[] { this.ViewCenter.X, this.ViewCenter.Y }, 0);
            s.Write(this.ViewScale);
            SerializeExtent(this.DataExtent, s);
            s.Write(this.DisplayDpi);
            s.Write(this.DisplayWidth);
            s.Write(this.DisplayHeight);
            s.Write(Utility.SerializeHTMLColor(this.BackgroundColor, true));
            s.Write(this.MetersPerUnit);

            if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
                s.Write(this.LayerRefreshMode);

            s.Write(_finiteDisplayScales.Length);
            foreach (double d in _finiteDisplayScales)
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

        private static void SerializeExtent(IEnvelope env, MgBinarySerializer s)
        {
            if (s.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
                s.WriteClassId(18001);
            else
                s.WriteClassId(20001);

            s.Write((int)0);

            s.Write(env.MinX);
            s.Write(env.MinY);
            s.Write(env.MaxY);
            s.Write(env.MaxY);
        }

        private Dictionary<string, ChangeList> m_changeList;

        internal class Change
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

            public ChangeType Type { get; private set; }
            public string Params { get; private set; }

            public Change()
            {
            }

            public Change(ChangeType type, string param)
            {
                this.Type = type;
                this.Params = param;
            }
        }

        internal class ChangeList
        {
            public string ObjectId { get; private set; }
            public bool IsLayer { get; private set; }
            public List<Change> Changes { get; private set; }

            public ChangeList()
            {
                this.Changes = new List<Change>();
            }

            public ChangeList(string objectId, bool isLayer)
                : this()
            {
                this.ObjectId = objectId;
                this.IsLayer = isLayer;
            }
        }

        private void SerializeChangeMap(MgBinarySerializer s)
        {
            s.Write(m_changeList.Count);
            foreach (ChangeList cl in m_changeList.Values)
            {
                s.Write(cl.IsLayer);
                s.Write(cl.ObjectId);

                s.Write(cl.Changes.Count);
                foreach (Change c in cl.Changes)
                {
                    s.Write((int)c.Type);
                    s.Write(c.Params);
                }
            }
        }

        /// <summary>
        /// Serializes the layer data to the specified binary stream
        /// </summary>
        /// <param name="s"></param>
        protected void SerializeLayerData(MgBinarySerializer s)
        {
            s.Write((int)_groups.Count);
            foreach (var g in _groups)
                g.Serialize(s);

            s.Write(_layers.Count);
            foreach (var t in _layers)
                t.Serialize(s);
        }

        /// <summary>
        /// Initializes this instance from the specified binary stream
        /// </summary>
        /// <param name="d"></param>
        public void Deserialize(MgBinaryDeserializer d)
        {
            _disableChangeTracking = true;

            if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                if (d.ReadInt32() != MgBinaryVersion)
                    throw new Exception("Invalid map version");
                this.ResourceID = d.ReadResourceIdentifier();
            }


            this.Name = d.ReadString();
            this.ObjectId = d.ReadString();

            this.MapDefinition = d.ReadResourceIdentifier();

            this.CoordinateSystem = d.ReadString();
            this.MapExtent = DeserializeExtents(d);
            var cc = d.ReadCoordinates();
            if (this.ViewCenter != null)
            {
                this.ViewCenter.X = cc[0];
                this.ViewCenter.Y = cc[1];
            }
            else
            {
                this.ViewCenter = ObjectFactory.CreatePoint2D(cc[0], cc[1]);
            }
            this.ViewScale = d.ReadDouble();

            this.DataExtent = DeserializeExtents(d);
            this.DisplayDpi = d.ReadInt32();
            this.DisplayWidth = d.ReadInt32();
            this.DisplayHeight = d.ReadInt32();
            this.BackgroundColor = Utility.ParseHTMLColor(d.ReadString());
            this.MetersPerUnit = d.ReadDouble();

            if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
                this.LayerRefreshMode = d.ReadInt32();

            var fds = new List<double>();
            int finiteScaleCount = d.ReadInt32();
            while (finiteScaleCount-- > 0)
                fds.Add(d.ReadDouble());

            _finiteDisplayScales = fds.ToArray();

            m_changeList = new Dictionary<string, ChangeList>();
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

            _disableChangeTracking = false;
        }

        private static IEnvelope DeserializeExtents(MgBinaryDeserializer d)
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

            double minx = Math.Min(x1, x2);
            double miny = Math.Min(y1, y2);
            double maxx = Math.Max(x1, x2);
            double maxy = Math.Max(y1, y2);

            return ObjectFactory.CreateEnvelope(minx, miny, maxx, maxy);
        }

        private Dictionary<string, ChangeList> DeserializeChangeMap(MgBinaryDeserializer d)
        {
            Dictionary<string, ChangeList> changes = new Dictionary<string, ChangeList>();
            int changeListCount = d.ReadInt32();
            while (changeListCount-- > 0)
            {
                bool isLayer = d.ReadByte() > 0;
                string objid = d.ReadString();

                ChangeList c = null;
                if (!changes.ContainsKey(objid))
                {
                    c = new ChangeList(objid, isLayer);
                    changes.Add(c.ObjectId, c);
                }
                else
                {
                    c = changes[objid];
                }
                
                int changeCount = d.ReadInt32();
                while (changeCount-- > 0)
                {
                    //Split up to avoid dependency on argument evaluation order
                    int ctype = d.ReadInt32();
                    c.Changes.Add(new Change((Change.ChangeType)ctype, d.ReadString()));
                }
                
            }
            return changes;
        }

        internal void DeserializeLayerData(MgBinaryDeserializer d)
        {
            int groupCount = d.ReadInt32();

            _groups.Clear();

            for (int i = 0; i < groupCount; i++)
            {
                RuntimeMapGroup g = new RuntimeMapGroup();
                g.Deserialize(d);
                _groups.Add(g);
            }

            int mapLayerCount = d.ReadInt32();

            _layers.Clear();
            _layerIdMap.Clear();

            while (mapLayerCount-- > 0)
            {
                RuntimeMapLayer t = new RuntimeMapLayer(this);
                t.Deserialize(d);
                AddLayerInternal(t);
            }
        }

        /// <summary>
        /// Gets the selection set
        /// </summary>
        /// <value>The selection.</value>
        public MapSelection Selection
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the group by its specified name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RuntimeMapGroup GetGroupByName(string name)
        {
            Check.NotNull(name, "name");
            foreach (var group in _groups)
            {
                if (name.Equals(group.Name))
                    return group;
            }
            return null;
        }

        /// <summary>
        /// Gets the layer by object id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public RuntimeMapLayer GetLayerByObjectId(string id)
        {
            if (_layerIdMap.ContainsKey(id))
                return _layerIdMap[id];

            return null;
        }

        /// <summary>
        /// Gets an array of all layers in this map
        /// </summary>
        /// <value>The layers.</value>
        public RuntimeMapLayer[] Layers
        {
            get { return _layers.ToArray(); }
        }

        /// <summary>
        /// Gets an array of all groups in this map
        /// </summary>
        /// <value>The groups.</value>
        public RuntimeMapGroup[] Groups
        {
            get { return _groups.ToArray(); }
        }

        /// <summary>
        /// A cache of Layer Definition objects. Used to reduce lookup time of the same layer definitions
        /// </summary>
        protected Dictionary<string, ILayerDefinition> layerDefinitionCache = new Dictionary<string, ILayerDefinition>();

        /// <summary>
        /// Creates and adds the layer to the map
        /// </summary>
        /// <param name="layerDefinitionId">The layer definition id.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public RuntimeMapLayer AddLayer(string layerDefinitionId, RuntimeMapGroup group)
        {
            ILayerDefinition ldf = GetLayerDefinition(layerDefinitionId);

            var layer = new RuntimeMapLayer(this, ldf);

            AddLayerInternal(layer);

            return layer;
        }

        private ILayerDefinition GetLayerDefinition(string layerDefinitionId)
        {
            ILayerDefinition ldf = null;

            if (layerDefinitionCache.ContainsKey(layerDefinitionId))
            {
                ldf = layerDefinitionCache[layerDefinitionId];
            }
            else
            {
                ResourceIdentifier.Validate(layerDefinitionId, ResourceTypes.LayerDefinition);
                ldf = (ILayerDefinition)this.ResourceService.GetResource(layerDefinitionId);

                layerDefinitionCache[layerDefinitionId] = ldf;
            }
            return ldf;
        }

        /// <summary>
        /// Adds the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        protected void AddLayerInternal(RuntimeMapLayer layer)
        {
            _layers.Add(layer);
            _layerIdMap[layer.ObjectId] = layer;

            OnLayerAdded(layer);
        }

        /// <summary>
        /// Creates the group and adds it to the map
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RuntimeMapGroup AddGroup(string name)
        {
            var group = new RuntimeMapGroup(this, name);
            AddGroupInternal(group);
            return group;
        }

        private void AddGroupInternal(RuntimeMapGroup group)
        {
            _groups.Add(group);
            OnGroupAdded(group);
        }

        /// <summary>
        /// Removes the layer
        /// </summary>
        /// <param name="layer">The layer.</param>
        protected void RemoveLayerInternal(RuntimeMapLayer layer)
        {
            if (_layers.Remove(layer))
            {
                OnLayerRemoved(layer);
            }
        }

        /// <summary>
        /// Removes the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void RemoveLayer(RuntimeMapLayer layer)
        {
            Check.NotNull(layer, "layer");
            RemoveLayerInternal(layer);
        }

        /// <summary>
        /// Removes the specified group.
        /// </summary>
        /// <param name="group">The group.</param>
        public void RemoveGroup(RuntimeMapGroup group)
        {
            Check.NotNull(group, "group");
            RemoveGroupInternal(group);
        }

        private void RemoveGroupInternal(RuntimeMapGroup group)
        {
            if (_groups.Remove(group))
            {
                OnGroupRemoved(group);
            }
        }

        /// <summary>
        /// Gets the layers of the specified group
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public RuntimeMapLayer[] GetLayersOfGroup(string groupName)
        {
            Check.NotEmpty(groupName, "groupName");
            List<RuntimeMapLayer> layers = new List<RuntimeMapLayer>();
            foreach (var lyr in _layers)
            {
                if (groupName.Equals(lyr.Group))
                    layers.Add(lyr);
            }
            return layers.ToArray();
        }

        /// <summary>
        /// Saves this instance. The changes are propagated back to the MapGuide Server
        /// </summary>
        public virtual void Save()
        {
            Save(this.ResourceID);
        }

        /// <summary>
        /// A dummy resource, used for the runtime map
        /// </summary>
        internal const string RUNTIMEMAP_XML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Map></Map>";

        /// <summary>
        /// A dummy resource, used for the runtime map
        /// </summary>
        internal const string RUNTIMEMAP_SELECTION_XML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Selection></Selection>";

        private void Save(string resourceID)
        {
            var map = this;

            string selectionID = resourceID.Substring(0, resourceID.LastIndexOf(".")) + ".Selection";
            this.ResourceService.SetResourceXmlData(resourceID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(RUNTIMEMAP_XML)));
            this.ResourceService.SetResourceXmlData(selectionID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(RUNTIMEMAP_SELECTION_XML)));

            ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
            if (!resourceID.StartsWith("Session:" + this.SessionId + "//") || !resourceID.EndsWith(".Map"))
                throw new Exception("Runtime maps must be in the current session repository");

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.MemoryStream ms2 = null;

            //Apparently the name is used to reconstruct the resourceId rather than pass it around
            //inside the map server
            string r = map.Name;
            string t = map.ResourceID;

            string mapname = resourceID.Substring(resourceID.IndexOf("//") + 2);
            mapname = mapname.Substring(0, mapname.LastIndexOf("."));
            map.Name = mapname;
            map.ResourceID = resourceID;

            try
            {
                map.Serialize(new MgBinarySerializer(ms, this.SiteVersion));
                if (this.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
                {
                    ms2 = new System.IO.MemoryStream();
                    map.SerializeLayerData(new MgBinarySerializer(ms2, this.SiteVersion));
                }

                this.ResourceService.SetResourceData(resourceID, "RuntimeData", ResourceDataType.Stream, ms);
                if (ms2 != null)
                    this.ResourceService.SetResourceData(resourceID, "LayerGroupData", ResourceDataType.Stream, ms2);

                SaveSelectionXml(resourceID);
            }
            finally
            {
                map.Name = r;
                map.ResourceID = t;
            }
        }

        private void SaveSelectionXml(string resourceID)
        {
            if (this.Selection == null)
                return;

            ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
            string selectionID = resourceID.Substring(0, resourceID.LastIndexOf(".")) + ".Selection";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            MgBinarySerializer serializer = new MgBinarySerializer(ms, this.SiteVersion);
            this.Selection.Serialize(serializer);
            ms.Position = 0;
            this.ResourceService.SetResourceData(selectionID, "RuntimeData", ResourceDataType.Stream, ms);
        }

        /// <summary>
        /// Gets the layer by its specified name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RuntimeMapLayer GetLayerByName(string name)
        {
            Check.NotEmpty(name, "name");
            foreach (var layer in _layers)
            {
                if (name.Equals(layer.Name))
                    return layer;
            }
            return null;
        }

        #region change tracking

        //This mirrors the unmanaged implementation of MgMapBase
        //Turns out the 2.x implementation didn't track these map element changes

        internal void TrackChange(string objectId, bool isLayer, Change.ChangeType type, string param)
        {
            if (_disableChangeTracking)
                return;

            ChangeList changes = null;
            if (!m_changeList.ContainsKey(objectId))
            {
                changes = new ChangeList(objectId, isLayer);
                m_changeList.Add(objectId, changes);
                
            }
            changes = m_changeList[objectId];

            var change = new Change(type, param);
            changes.Changes.Add(change);
        }

        /// <summary>
        /// Clears all tracked changes
        /// </summary>
        protected void ClearChanges()
        {
            m_changeList.Clear();
        }

        /// <summary>
        /// Called when a group is removed
        /// </summary>
        /// <param name="group"></param>
        protected void OnGroupRemoved(RuntimeMapGroup group)
        {
            //???
            var layers = GetLayersOfGroup(group.Name);
            foreach (var lyr in layers)
            {
                RemoveLayerInternal(lyr);
            }

            TrackChange(group.ObjectId, false, Change.ChangeType.removed, string.Empty);
        }

        /// <summary>
        /// Called when a group is added
        /// </summary>
        /// <param name="group"></param>
        protected void OnGroupAdded(RuntimeMapGroup group)
        {
            //???

            TrackChange(group.ObjectId, false, Change.ChangeType.added, string.Empty);
        }

        internal void OnGroupVisibilityChanged(RuntimeMapGroup group, string visbility)
        {
            TrackChange(group.ObjectId, false, Change.ChangeType.visibilityChanged, visbility);

            //???
        }

        internal void OnGroupDisplayInLegendChanged(RuntimeMapGroup group, string displayInLegendState)
        {
            TrackChange(group.ObjectId, false, Change.ChangeType.displayInLegendChanged, displayInLegendState);
        }

        internal void OnGroupLegendLabelChanged(RuntimeMapGroup group, string legendLabel)
        {
            TrackChange(group.ObjectId, false, Change.ChangeType.legendLabelChanged, legendLabel);
        }

        internal void OnGroupParentChanged(RuntimeMapGroup group, string parentId)
        {
            TrackChange(group.ObjectId, false, Change.ChangeType.parentChanged, parentId);
        }

        internal void OnLayerRemoved(RuntimeMapLayer layer)
        {
            //???
            if (_layerIdMap.ContainsKey(layer.ObjectId))
                _layerIdMap.Remove(layer.ObjectId);

            TrackChange(layer.ObjectId, true, Change.ChangeType.removed, string.Empty);
        }

        internal void OnLayerAdded(RuntimeMapLayer layer)
        {
            //???

            TrackChange(layer.ObjectId, true, Change.ChangeType.added, string.Empty);
        }

        internal void OnLayerVisibilityChanged(RuntimeMapLayer layer, string visibility)
        {
            //???

            TrackChange(layer.ObjectId, true, Change.ChangeType.visibilityChanged, visibility);
        }

        internal void OnLayerDisplayInLegendChanged(RuntimeMapLayer layer, string displayInLegendState)
        {
            TrackChange(layer.ObjectId, true, Change.ChangeType.displayInLegendChanged, displayInLegendState);
        }

        internal void OnLayerLegendLabelChanged(RuntimeMapLayer layer, string legendLabel)
        {
            TrackChange(layer.ObjectId, true, Change.ChangeType.legendLabelChanged, legendLabel);
        }

        internal void OnLayerParentChanged(RuntimeMapLayer layer, string parentId)
        {
            TrackChange(layer.ObjectId, true, Change.ChangeType.parentChanged, parentId);
        }

        internal void OnLayerSelectabilityChanged(RuntimeMapLayer layer, string selectability)
        {
            TrackChange(layer.ObjectId, true, Change.ChangeType.selectabilityChanged, selectability);
        }

        internal void OnLayerDefinitionChanged(RuntimeMapLayer layer)
        {
            TrackChange(layer.ObjectId, true, Change.ChangeType.definitionChanged, string.Empty);
        }

        #endregion

        #region convenience methods
        /// <summary>
        /// Convenience method for rendering a bitmap of the current map
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public System.IO.Stream Render(string format)
        {
            if (_mapSvc == null)
                throw new NotSupportedException();

            return _mapSvc.RenderRuntimeMap(
                this.ResourceID, 
                this.ViewCenter.X, 
                this.ViewCenter.Y, 
                this.ViewScale, 
                this.DisplayWidth, 
                this.DisplayHeight, 
                this.DisplayDpi, 
                format);
        }

        /// <summary>
        /// Convenience method for rendering a dynamic overlay of the current map
        /// </summary>
        /// <param name="format"></param>
        /// <param name="keepSelection"></param>
        /// <returns></returns>
        public System.IO.Stream RenderDynamicOverlay(string format, bool keepSelection)
        {
            if (_mapSvc == null)
                throw new NotSupportedException();

            return _mapSvc.RenderDynamicOverlay(
                this,
                this.Selection,
                format,
                keepSelection);
        }
        #endregion
    }
}
