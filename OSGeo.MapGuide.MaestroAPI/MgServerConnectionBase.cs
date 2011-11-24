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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

using ObjCommon = OSGeo.MapGuide.ObjectModels.Common;
using AppDef = OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Mapping;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Base class of all MapGuide connection classes. Covers functionality encompassed by
    /// the MapGuide Geospatial Platform API and the MapGuide-specific services (Site, Rendering,
    /// Mapping, Tile, Drawing)
    /// </summary>
    public abstract class MgServerConnectionBase : PlatformConnectionBase
    {
        /// <summary>
        /// A flag that indicates if a session will be automatically restarted
        /// </summary>
        protected bool m_autoRestartSession = false;

        /// <summary>
        /// The username used to open this connection, if any
        /// </summary>
        protected string m_username;

        /// <summary>
        /// The password used to open this connection, if any
        /// </summary>
        protected string m_password;

        /// <summary>
        /// cached user list
        /// </summary>
        protected ObjCommon.UserList m_cachedUserList = null;

        /// <summary>
        /// cached group list
        /// </summary>
        protected ObjCommon.GroupList m_cachedGroupList = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MgServerConnectionBase"/> class.
        /// </summary>
        protected MgServerConnectionBase()
            : base()
        {
            m_username = null;
            m_password = null;
        }

        #region Session Management

        /// <summary>
        /// Gets or sets a value indicating if the session should automatically be restarted if it expires
        /// </summary>
        virtual public bool AutoRestartSession
        {
            get { return m_autoRestartSession; }
            set { m_autoRestartSession = value; }
        }

        /// <summary>
        /// Determines if an exception is a "Session Expired" exception.
        /// </summary>
        /// <param name="ex">The exception to evaluate</param>
        /// <returns>True if the exception is a session expired exception</returns>
        abstract public bool IsSessionExpiredException(Exception ex);

        /// <summary>
        /// Restarts the server session, and creates a new session ID
        /// </summary>
        public void RestartSession()
        {
            RestartSession(true);
        }

        /// <summary>
        /// Restarts the server session, and creates a new session ID
        /// </summary>
        /// <param name="throwException">If set to true, the call throws an exception if the call failed</param>
        /// <returns>True if the creation succeed, false otherwise</returns>
        abstract public bool RestartSession(bool throwException);

        #endregion

        #region Site

        /// <summary>
        /// Gets the site info.
        /// </summary>
        /// <returns></returns>
        public abstract ObjCommon.SiteInformation GetSiteInfo();

        /// <summary>
        /// Gets a list of all users on the server
        /// </summary>
        /// <returns>The list of users</returns>
        public virtual ObjCommon.UserList EnumerateUsers()
        {
            return this.EnumerateUsers(null);
        }

        /// <summary>
        /// Gets a list of users in a group
        /// </summary>
        /// <param name="group">The group to retrieve the users from</param>
        /// <returns>The list of users</returns>
        abstract public ObjCommon.UserList EnumerateUsers(string group);

        /// <summary>
        /// Gets a list of all groups on the server
        /// </summary>
        /// <returns>The list of groups</returns>
        abstract public ObjCommon.GroupList EnumerateGroups();

        #endregion

        #region Rendering

        /// <summary>
        /// Selects features from a runtime map, returning a selection Xml.
        /// </summary>
        /// <param name="runtimeMapName">The map to query. NOT a resourceID, only the map name!</param>
        /// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
        /// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
        /// <returns>The selection Xml, or an empty string if there were no data.</returns>
        public string QueryMapFeatures(string runtimeMapName, string wkt, bool persist)
        {
            return QueryMapFeatures(runtimeMapName, wkt, persist, QueryMapFeaturesLayerAttributes.Default, false);
        }

        /// <summary>
        /// Selects features from a runtime map, returning a selection Xml.
        /// </summary>
        /// <param name="runtimeMapName">The map to query. NOT a resourceID, only the map name!</param>
        /// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
        /// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
        /// <param name="attributes">The type of layer to include in the query</param>
        /// <param name="raw">True if the result should contain the tooltip and link info</param>
        /// <returns>The selection Xml, or an empty string if there were no data.</returns>
        abstract public string QueryMapFeatures(string runtimeMapName, string wkt, bool persist, QueryMapFeaturesLayerAttributes attributes, bool raw);

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer the image should represent</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        abstract public System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type);



        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi)
        {
            return this.RenderRuntimeMap(resourceId, x, y, scale, width, height, dpi, "PNG", false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi)
        {
            return this.RenderRuntimeMap(resourceId, x1, y1, x2, y2, width, height, dpi, "PNG", false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format)
        {
            return this.RenderRuntimeMap(resourceId, x, y, scale, width, height, dpi, format, false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format)
        {
            return this.RenderRuntimeMap(resourceId, x1, y1, x2, y2, width, height, dpi, format, false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        public abstract System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format, bool clip);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        public abstract System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip);

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format)
        {
            return RenderDynamicOverlay(map, selection, format, true);
        }

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <param name="keepSelection"></param>
        /// <returns></returns>
        public abstract System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, bool keepSelection);

        #endregion

        #region Tile

        /// <summary>
        /// Gets the tile.
        /// </summary>
        /// <param name="mapdefinition">The mapdefinition.</param>
        /// <param name="baselayergroup">The baselayergroup.</param>
        /// <param name="col">The col.</param>
        /// <param name="row">The row.</param>
        /// <param name="scaleindex">The scaleindex.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public abstract System.IO.Stream GetTile(string mapdefinition, string baselayergroup, int col, int row, int scaleindex, string format);

        #endregion
    }
}
