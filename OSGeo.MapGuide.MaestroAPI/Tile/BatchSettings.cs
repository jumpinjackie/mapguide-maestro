#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// This delegate is used to monitor progress on tile rendering
    /// </summary>
    /// <param name="map">The map currently being processed</param>
    /// <param name="group">The group being processed</param>
    /// <param name="scaleindex">The scaleindex being processed</param>
    /// <param name="row">The row being processed</param>
    /// <param name="column">The column being processed</param>
    /// <param name="cancel">A control flag to stop the tile rendering</param>
    /// <param name="state">The state that invoked the callback</param>
    public delegate void ProgressCallback(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref bool cancel);

    /// <summary>
    /// This delegate is used to monitor progress on tile rendering
    /// </summary>
    /// <param name="map">The map currently being processed</param>
    /// <param name="group">The group being processed</param>
    /// <param name="scaleindex">The scaleindex being processed</param>
    /// <param name="row">The row being processed</param>
    /// <param name="column">The column being processed</param>
    /// <param name="state">The state that invoked the callback</param>
    /// <param name="exception">The exception from the last attempt, set this to null to ignore the exception</param>
    public delegate void ErrorCallback(CallbackStates state, MapTilingConfiguration map, string group, int scaleindex, int row, int column, ref Exception exception);

    /// <summary>
    /// These are the avalible states for callbacks
    /// </summary>
    public enum CallbackStates
    {
        /// <summary>
        /// All maps are being rendered
        /// </summary>
        StartRenderAllMaps,
        /// <summary>
        /// A map is being rendered
        /// </summary>
        StartRenderMap,
        /// <summary>
        /// A group is being rendered
        /// </summary>
        StartRenderGroup,
        /// <summary>
        /// A scale is being rendered
        /// </summary>
        StartRenderScale,
        /// <summary>
        /// A tile is being rendered
        /// </summary>
        StartRenderTile,
        /// <summary>
        /// A tile has been rendered
        /// </summary>
        FinishRenderTile,
        /// <summary>
        /// A scale has been rendered
        /// </summary>
        FinishRenderScale,
        /// <summary>
        /// A group has been rendered
        /// </summary>
        FinishRenderGroup,
        /// <summary>
        /// A map has been rendered
        /// </summary>
        FinishRenderMap,
        /// <summary>
        /// All maps have been rendered
        /// </summary>
        FinishRenderAllMaps,
        /// <summary>
        /// A tile has failed to render
        /// </summary>
        FailedRenderingTile,
    }

    /// <summary>
    /// Class to hold settings for a batch run of tile building
    /// </summary>
    public class TilingRunCollection
    {
        /// <summary>
        /// A reference to the connection
        /// </summary>
        private IServerConnection m_connection;
        /// <summary>
        /// The list of maps
        /// </summary>
        private List<MapTilingConfiguration> m_maps;
        /// <summary>
        /// A default set of tile settings
        /// </summary>
        private TileRunParameters m_tileSettings = new TileRunParameters();

        /// <summary>
        /// A flag that indicates the rendering should stop
        /// </summary>
        private bool m_cancel;

        /// <summary>
        /// An event that can be used to pause MgCooker
        /// </summary>
        public System.Threading.ManualResetEvent PauseEvent = new System.Threading.ManualResetEvent(true);

        #region Events
        /// <summary>
        /// All maps are being rendered
        /// </summary>
        public event ProgressCallback BeginRenderingMaps;
        /// <summary>
        /// A map is being rendered
        /// </summary>
        public event ProgressCallback BeginRenderingMap;
        /// <summary>
        /// A group is being rendered
        /// </summary>
        public event ProgressCallback BeginRenderingGroup;
        /// <summary>
        /// A scale is being rendered
        /// </summary>
        public event ProgressCallback BeginRenderingScale;
        /// <summary>
        /// A tile is being rendered
        /// </summary>
        public event ProgressCallback BeginRenderingTile;
        /// <summary>
        /// All maps have been rendered
        /// </summary>
        public event ProgressCallback FinishRenderingMaps;
        /// <summary>
        /// A map has been rendered
        /// </summary>
        public event ProgressCallback FinishRenderingMap;
        /// <summary>
        /// A group has been rendered
        /// </summary>
        public event ProgressCallback FinishRenderingGroup;
        /// <summary>
        /// A scale has been rendered
        /// </summary>
        public event ProgressCallback FinishRenderingScale;
        /// <summary>
        /// A tile has been rendered
        /// </summary>
        public event ProgressCallback FinishRenderingTile;
        /// <summary>
        /// A tile has failed to render
        /// </summary>
        public event ErrorCallback FailedRenderingTile;

        internal void InvokeBeginRendering(MapTilingConfiguration batchMap)
        {
            if (this.BeginRenderingMap != null)
                this.BeginRenderingMap(CallbackStates.StartRenderMap, batchMap, null, -1, -1, -1, ref m_cancel);
            PauseEvent.WaitOne();
        }

        internal void InvokeFinishRendering(MapTilingConfiguration batchMap)
        {
            if (this.FinishRenderingMap != null)
                this.FinishRenderingMap(CallbackStates.FinishRenderMap, batchMap, null, -1, -1, -1, ref m_cancel);
        }

        internal void InvokeBeginRendering(MapTilingConfiguration batchMap, string group)
        {
            if (this.BeginRenderingGroup != null)
                this.BeginRenderingGroup(CallbackStates.StartRenderGroup, batchMap, group, -1, -1, -1, ref m_cancel);
            PauseEvent.WaitOne();
        }

        internal void InvokeFinishRendering(MapTilingConfiguration batchMap, string group)
        {
            if (this.FinishRenderingGroup != null)
                this.FinishRenderingGroup(CallbackStates.FinishRenderGroup, batchMap, group, -1, -1, -1, ref m_cancel);
        }

        internal void InvokeBeginRendering(MapTilingConfiguration batchMap, string group, int scaleindex)
        {
            if (this.BeginRenderingScale != null)
                this.BeginRenderingScale(CallbackStates.StartRenderScale, batchMap, group, scaleindex, -1, -1, ref m_cancel);
            PauseEvent.WaitOne();
        }

        internal void InvokeFinishRendering(MapTilingConfiguration batchMap, string group, int scaleindex)
        {
            if (this.FinishRenderingScale != null)
                this.FinishRenderingScale(CallbackStates.FinishRenderScale, batchMap, group, scaleindex, -1, -1, ref m_cancel);
        }

        internal void InvokeBeginRendering(MapTilingConfiguration batchMap, string group, int scaleindex, int row, int col)
        {
            if (this.BeginRenderingTile != null)
                this.BeginRenderingTile(CallbackStates.StartRenderTile, batchMap, group, scaleindex, row, col, ref m_cancel);
            PauseEvent.WaitOne();
        }

        internal void InvokeFinishRendering(MapTilingConfiguration batchMap, string group, int scaleindex, int row, int col)
        {
            if (this.FinishRenderingTile != null)
                this.FinishRenderingTile(CallbackStates.FinishRenderTile, batchMap, group, scaleindex, row, col, ref m_cancel);
        }

        internal Exception InvokeError(MapTilingConfiguration batchMap, string group, int scaleindex, int row, int col, ref Exception exception)
        {
            if (this.FailedRenderingTile != null)
                this.FailedRenderingTile(CallbackStates.FailedRenderingTile, batchMap, group, scaleindex, row, col, ref exception);

            return exception;
        }

        #endregion

        /// <summary>
        /// Constructs a new batch setup. If no maps are supplied, all maps in the repository is assumed.
        /// </summary>
        /// <param name="mapagent">The url to the mapagent.fcgi</param>
        /// <param name="username">The username to connect with</param>
        /// <param name="password">The password to connect with</param>
        /// <param name="maps">A list of maps to process, leave empty to process all layers</param>
        public TilingRunCollection(string mapagent, string username, string password, params string[] maps)
            : this(ConnectionProviderRegistry.CreateConnection("Maestro.Http", "Url", mapagent, "Username", username, "Password", password, "AllowUntestedVersions", "true"), maps) //NOXLATE
        {
        }

        /// <summary>
        /// Constructs a new batch setup
        /// </summary>
        /// <param name="connection"></param>
        public TilingRunCollection(IServerConnection connection)
        {
            m_connection = connection;
            m_maps = new List<MapTilingConfiguration>();
        }

        /// <summary>
        /// Constructs a new batch setup
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="maps"></param>
        public TilingRunCollection(IServerConnection connection, params string[] maps)
        {
            m_connection = connection;
            m_maps = new List<MapTilingConfiguration>();

            AddMapDefinitions(maps);
        }

        /// <summary>
        /// Adds the specified map definition ids
        /// </summary>
        /// <param name="maps"></param>
        public void AddMapDefinitions(string[] maps)
        {
            if (maps == null || maps.Length == 0 || (maps.Length == 1 && maps[0].Trim().Length == 0))
            {
                List<string> tmp = new List<string>();
                foreach (var doc in m_connection.ResourceService.GetRepositoryResources(StringConstants.RootIdentifier, ResourceTypes.MapDefinition.ToString()).Children)
                    tmp.Add(doc.ResourceId);
                maps = tmp.ToArray();
            }

            foreach (string s in maps)
            {
                MapTilingConfiguration bm = new MapTilingConfiguration(this, s);
                if (bm.Resolutions > 0)
                    m_maps.Add(bm);
            }
        }

        /// <summary>
        /// Sets the list of scale indexes
        /// </summary>
        /// <param name="scaleindexes"></param>
        public void SetScales(int[] scaleindexes)
        {
            foreach (MapTilingConfiguration bm in m_maps)
                bm.SetScales(scaleindexes);
        }

        /// <summary>
        /// Sets the list of groups
        /// </summary>
        /// <param name="groups"></param>
        public void SetGroups(string[] groups)
        {
            foreach (MapTilingConfiguration bm in m_maps)
                bm.SetGroups(groups);
        }

        /// <summary>
        /// Limits the number of rows
        /// </summary>
        /// <param name="limit"></param>
        public void LimitRows(long limit)
        {
            foreach (MapTilingConfiguration bm in m_maps)
                bm.LimitRows(limit);
        }

        /// <summary>
        /// Limits the number of columns
        /// </summary>
        /// <param name="limit"></param>
        public void LimitCols(long limit)
        {
            foreach (MapTilingConfiguration bm in m_maps)
                bm.LimitCols(limit);
        }

        /// <summary>
        /// Renders all tiles in all maps
        /// </summary>
        public void RenderAll()
        {
            m_cancel = false;

            if (this.BeginRenderingMaps != null)
                this.BeginRenderingMaps(CallbackStates.StartRenderAllMaps, null, null, -1, -1, -1, ref m_cancel);

            foreach (MapTilingConfiguration bm in this.Maps)
                if (m_cancel)
                    break;
                else
                    bm.Render();

            if (this.FinishRenderingMaps != null)
                this.FinishRenderingMaps(CallbackStates.FinishRenderAllMaps, null, null, -1, -1, -1, ref m_cancel);
        }

        /// <summary>
        /// The connection to the server
        /// </summary>
        public IServerConnection Connection { get { return m_connection; } }
        /// <summary>
        /// The list of map configurations to proccess
        /// </summary>
        public List<MapTilingConfiguration> Maps { get { return m_maps; } }

        /// <summary>
        /// The tile settings
        /// </summary>
        public TileRunParameters Config { get { return m_tileSettings; } }

        /// <summary>
        /// Gets a flag indicating if the rendering process is cancelled
        /// </summary>
        public bool Cancel { get { return m_cancel; } }
    }

    /// <summary>
    /// Class that represents a single map to build tiles for
    /// </summary>
    public class MapTilingConfiguration
    {
        /// <summary>
        /// A reference to the parent, and thus the connection
        /// </summary>
        private TilingRunCollection m_parent;
        /// <summary>
        /// The map read from MapGuide
        /// </summary>
        private IMapDefinition m_mapdefinition;

        /// <summary>
        /// The max extent of the map
        /// </summary>
        private IEnvelope m_maxExtent;

        /// <summary>
        /// The list of baselayer group names
        /// </summary>
        private string[] m_groups;

        /// <summary>
        /// For each entry there is two longs, row and column
        /// </summary>
        private long[][] m_dimensions;

        /// <summary>
        /// The max scale for the map
        /// </summary>
        private double m_maxscale;

        /// <summary>
        /// Conversion from supplied scaleindex to actual scaleindex
        /// </summary>
        private int[] m_scaleindexmap;

        /// <summary>
        /// The number of meters in an inch
        /// </summary>
        private const double INCH_TO_METER = 0.0254;

        /// <summary>
        /// Gets the list of groups
        /// </summary>
        public string[] Groups { get { return m_groups; } }

       

        /// <summary>
        /// The map's scales may have been modified, this array is a map of the new values
        /// </summary>
        public int[] ScaleIndexMap { get { return m_scaleindexmap; } }

        /// <summary>
        /// Constructs a new map to be processed
        /// </summary>
        /// <param name="parent">The parent entry</param>
        /// <param name="map">The resource id for the mapdefinition</param>
        public MapTilingConfiguration(TilingRunCollection parent, string map)
        {
            m_parent = parent;
            m_mapdefinition = (IMapDefinition)parent.Connection.ResourceService.GetResource(map);
            var baseMap = m_mapdefinition.BaseMap;

            if (baseMap != null &&
                baseMap.ScaleCount > 0)
            {
                m_groups = new string[baseMap.GroupCount];
                for (int i = 0; i < baseMap.GroupCount; i++)
                    m_groups[i] = baseMap.GetGroupAt(i).Name;

                m_maxscale = baseMap.GetMaxScale();
                CalculateDimensions();
            }
        }

        internal void CalculateDimensions()
        {
            int[] tmp = new int[this.MapDefinition.BaseMap.ScaleCount];
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = i;

            SetScales(tmp);
        }

        internal void CalculateDimensionsInternal()
        {
            if (m_mapdefinition.BaseMap.ScaleCount == 0)
            {
                m_scaleindexmap = new int[0];
                m_dimensions = new long[0][];
                return;
            }

            IEnvelope extents = this.MaxExtent ?? m_mapdefinition.Extents;
            double maxscale = m_maxscale;

            m_dimensions = new long[this.Resolutions][];
            m_scaleindexmap = new int[m_dimensions.Length];
            
            double width_in_meters = Math.Abs(m_parent.Config.MetersPerUnit * (extents.MaxX - extents.MinX));
            double height_in_meters = Math.Abs(m_parent.Config.MetersPerUnit * (extents.MaxY - extents.MinY));

            m_dimensions = new long[this.Resolutions][];
            for (int i = this.Resolutions - 1; i >= 0; i--)
            {
                long rows, cols, rowTileOffset = 0 , colTileOffset = 0;
                double scale = m_mapdefinition.BaseMap.GetScaleAt(i);
                
                if (m_parent.Config.UseOfficialMethod)
                {
                    //This is the algorithm proposed by the MapGuide team:
                    //http://www.nabble.com/Pre-Genererate--tiles-for-the-entire-map-at-all-pre-defined-zoom-scales-to6074037.html#a6078663
                    //
                    //Method description inline (in case nabble link disappears):
                    //
                    // The upper left corner of the extents of the map corresponds to tile (0,0). Then tile (1,0) is to the right of that and tile (0,1) is under tile (0,0).
                    // So assuming you know the extents of your map, you can calculate how many tiles it spans at the given scale, using the following
                    //
                    // number of tiles x = map width in meters  / ( 0.079375 * map_scale)
                    // number of tiles y = map height in meters / ( 0.079375 * map_scale)
                    //
                    // where 0.079375 = [inch to meter] / image DPI * tile size = 0.0254 / 96 * 300.
                    //
                    // This assumes you know the scale factor that converts your map width and height to meters. You can get this from the coordinate system of the map if you don't know it, but it's much easier to just plug in the number into this equation.
                    // 
                    // Also have in mind that you can also request tiles beyond the map extent (for example tile (-1, -1), however, there is probably no point to cache them unless you have valid data outside your initial map extents.
                    
                    //The tile extent in meters
                    double tileWidth  =((INCH_TO_METER / m_parent.Config.DPI * m_parent.Config.TileWidth) * (scale));
                    double tileHeight = ((INCH_TO_METER / m_parent.Config.DPI * m_parent.Config.TileHeight) * (scale));

                    //Using this algorithm, yields a negative number of columns/rows, if the max scale is larger than the max extent of the map.
                    rows = Math.Max(1, (int)Math.Ceiling((height_in_meters / tileHeight)));
                    cols = Math.Max(1, (int)Math.Ceiling((width_in_meters / tileWidth)));

                    if (m_maxExtent != null)
                    {
                        //The extent is overridden, so we need to adjust the start offsets
                        double offsetX = MaxExtent.MinX - m_mapdefinition.Extents.MinX;
                        double offsetY = m_mapdefinition.Extents.MaxY - MaxExtent.MaxY ;
                        rowTileOffset = (int)Math.Floor(offsetY / tileHeight);
                        colTileOffset = (int)Math.Floor(offsetX / tileWidth);

                        double offsetMaxX = MaxExtent.MaxX - m_mapdefinition.Extents.MinX;
                        double offsetMinY = m_mapdefinition.Extents.MaxY - MaxExtent.MinY;
                        int rowMinTileOffset = (int)Math.Floor(offsetMinY / tileHeight);
                        int colMaxTileOffset = (int)Math.Floor(offsetMaxX / tileWidth);

                        cols += (colMaxTileOffset - colTileOffset);
                        rows += (rowMinTileOffset - rowTileOffset);
                    }
                }
                else
                {
                    //This method assumes that the max scale is displayed on a screen with resolution 1920x1280.
                    //This display width/height is then multiplied up to calculate the pixelwidth of all subsequent
                    //scale ranges. Eg. if max scale range is 1:200, then scale range 1:100 is twice the size,
                    //meaning the full map at 1:100 fills 3840x2560 pixels.
                    //The width/height is then used to calculate the number of rows and columns of 300x300 pixel tiles.
                    
                    //The purpose of this method is to enabled tile generation without access to
                    //coordinate system properties

                    long pw = (long)(m_parent.Config.DisplayResolutionWidth * (1 / (scale / maxscale)));
                    long ph = (long)(m_parent.Config.DisplayResolutionHeight * (1 / (scale / maxscale)));

                    rows = (ph + (m_parent.Config.TileHeight - 1)) / m_parent.Config.TileHeight;
                    cols = (pw + (m_parent.Config.TileWidth - 1)) / m_parent.Config.TileWidth;
                    rows += rows % 2;
                    cols += cols % 2;
                }


                m_dimensions[i] = new long[] { rows, cols, rowTileOffset , colTileOffset};
            }
        }

        /// <summary>
        /// Sets the list of groups
        /// </summary>
        /// <param name="groups"></param>
        public void SetGroups(string[] groups)
        {
            List<string> g = new List<string>();
            for(int i = 0; i < m_groups.Length; i++)
                if (Array.IndexOf<string>(groups, m_groups[i]) >= 0)
                    g.Add(m_groups[i]);

            m_groups = g.ToArray();
        }


        /// <summary>
        /// Sets the list of scale indexes and sets the maximum extent to the given envelope
        /// </summary>
        /// <param name="scales"></param>
        /// <param name="envelope"></param>
        public void SetScalesAndExtend(int[] scales, IEnvelope envelope)
        {
            this.m_maxExtent = envelope;
            SetScales(scales);
        }


        /// <summary>
        /// Sets the list of scale indexes
        /// </summary>
        /// <param name="scaleindexes"></param>
        public void SetScales(int[] scaleindexes)
        {
            //TODO: Re-read scales from mapdef?
            SortedList<int, int> s = new SortedList<int, int>();
            foreach (int i in scaleindexes)
                if (!s.ContainsKey(i))
                    s.Add(i, i);

            List<int> keys = new List<int>(s.Keys);
            keys.Reverse();

            for (int i = m_mapdefinition.BaseMap.ScaleCount - 1; i >= 0; i--)
                if (!keys.Contains(i))
                    m_mapdefinition.BaseMap.RemoveScaleAt(i);

            CalculateDimensionsInternal();

            keys.Reverse();

            //Preserve the original scales
            m_scaleindexmap = new int[keys.Count];
            for (int i = 0; i < keys.Count; i++)
                m_scaleindexmap[i] = keys[i];
        }

        internal void LimitCols(long limit)
        {
            foreach (long[] d in m_dimensions)
                d[1] = Math.Min(limit, d[1]);
        }

        internal void LimitRows(long limit)
        {
            foreach (long[] d in m_dimensions)
                d[0] = Math.Min(limit, d[0]);
        }

        /// <summary>
        /// Gets the total number of tiles to be rendered
        /// </summary>
        public long TotalTiles
        {
            get
            {
                long t = 0;
                foreach (long[] d in m_dimensions)
                    t += d[0] * d[1];
                return t;
            }
        }

        /// <summary>
        /// Gets the number of resolutions for the map
        /// </summary>
        public int Resolutions
        {
            get
            {
                if (m_mapdefinition.BaseMap == null || m_mapdefinition.BaseMap.ScaleCount == 0)
                    return 0;
                else
                    return m_mapdefinition.BaseMap.ScaleCount;
            }
        }


        /// <summary>
        /// Renders all tiles in a given scale
        /// </summary>
        /// <param name="scaleindex">The scale to render</param>
        /// <param name="group">The name of the baselayer group</param>
        public void RenderScale(int scaleindex, string group)
        {
            m_parent.InvokeBeginRendering(this, group, scaleindex);

            if (!m_parent.Cancel)
            {
                int rows = (int)m_dimensions[scaleindex][0];
                int cols = (int)m_dimensions[scaleindex][1];
                int rowTileOffset = (int)m_dimensions[scaleindex][2];
                int colTileOffset = (int)m_dimensions[scaleindex][3];

                //If the MaxExtents are different from the actual bounds, we need a start offset offset

                RenderThreads settings = new RenderThreads(this, m_parent, m_scaleindexmap[scaleindex], group, m_mapdefinition.ResourceID, rows, cols, rowTileOffset, colTileOffset, m_parent.Config.RandomizeTileSequence);
                
                settings.RunAndWait();

                if (settings.TileSet.Count != 0 && !m_parent.Cancel)
                    throw new Exception(Strings.TS_ThreadFailureError);
            }

            m_parent.InvokeFinishRendering(this, group, scaleindex);

        }


        /// <summary>
        /// Renders all tiles in all scales
        /// </summary>
        /// <param name="group">The name of the baselayer group</param>
        public void RenderGroup(string group)
        {
            m_parent.InvokeBeginRendering(this, group);

            if (!m_parent.Cancel)
            {

                for (int i = this.Resolutions - 1; i >= 0; i--)
                    if (m_parent.Cancel)
                        break;
                    else
                        RenderScale(i, group);
            }

            m_parent.InvokeFinishRendering(this, group);

        }

        /// <summary>
        /// Renders all tiles in all groups in all scales
        /// </summary>
        public void Render()
        {
            m_parent.InvokeBeginRendering(this);

            if (!m_parent.Cancel)
                foreach (string s in m_groups)
                    if (m_parent.Cancel)
                        break;
                    else
                        RenderGroup(s);

            m_parent.InvokeFinishRendering(this);
        }

        /// <summary>
        /// Gets or sets the maximum extent used to calculate the tiles
        /// </summary>
        public IEnvelope MaxExtent
        {
            get
            {
                return m_maxExtent;
            }
            set
            {
                m_maxExtent = value;
                CalculateDimensions();
            }
        }

        /// <summary>
        /// Gets the resourceId for the map
        /// </summary>
        public string ResourceId { get { return m_mapdefinition.ResourceID; } }

        /// <summary>
        /// Gets the MapDefintion
        /// </summary>
        public IMapDefinition MapDefinition { get { return m_mapdefinition; } }

        /// <summary>
        /// Gets a reference to the parent tiling run collection
        /// </summary>
        public TilingRunCollection Parent { get { return m_parent; } }

       
    }

    /// <summary>
    /// Defines global parameters for a tiling run
    /// </summary>
    public class TileRunParameters
    {
        /// <summary>
        /// The meters per unit
        /// </summary>
        public double MetersPerUnit = 1;

        /// <summary>
        /// The display DPI
        /// </summary>
        public double DPI = 96;

        /// <summary>
        /// The tile width
        /// </summary>
        public int TileWidth = 300;

        /// <summary>
        /// The tile height
        /// </summary>
        public int TileHeight = 300;

        /// <summary>
        /// The number of times to retry
        /// </summary>
        public int RetryCount = 5;

        /// <summary>
        /// The display resolution width
        /// </summary>
        public int DisplayResolutionWidth = 1920;

        /// <summary>
        /// The display resolution height
        /// </summary>
        public int DisplayResolutionHeight = 1280;

        /// <summary>
        /// Gets or sets whether to use the official method of tile generation. Requires an accurate meters per unit value to work
        /// </summary>
        public bool UseOfficialMethod = false;

        /// <summary>
        /// Gets or sets whether to randomize the tile generation sequence
        /// </summary>
        public bool RandomizeTileSequence = false;
        private int m_threadCount = 1;

        /// <summary>
        /// Gets or sets the thread count
        /// </summary>
        public int ThreadCount
        {
            get { return m_threadCount; }
            set { m_threadCount = Math.Max(1, value); }
        }

        /// <summary>
        /// The render method
        /// </summary>
        public RenderMethodDelegate RenderMethod;

        /// <summary>
        /// Defines a tile render method
        /// </summary>
        /// <param name="map"></param>
        /// <param name="group"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="scale"></param>
        public delegate void RenderMethodDelegate(string map, string group, int col, int row, int scale);
    }
}
