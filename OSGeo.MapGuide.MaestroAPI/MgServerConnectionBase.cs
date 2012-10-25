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
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Services;

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
            _canAutoRestartSession = true;
        }

        #region Session Management

        /// <summary>
        /// Gets or sets a value indicating if the session should automatically be restarted if it expires
        /// </summary>
        virtual public bool AutoRestartSession
        {
            get { return m_autoRestartSession; }
            set 
            {
                if (value && !_canAutoRestartSession)
                    throw new InvalidOperationException(Strings.ErrorConnectionCannotAutoRestartSession);
                m_autoRestartSession = value; 
            }
        }

        /// <summary>
        /// Determines whether session auto-recover is possible
        /// </summary>
        protected bool _canAutoRestartSession;

        /// <summary>
        /// Indicates this connection cannot use session recovery, normally due to the fact the connection was initialized
        /// with just a session id.
        /// </summary>
        protected void DisableAutoSessionRecovery()
        {
            this.AutoRestartSession = false;
            _canAutoRestartSession = false;
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
        /// Raised when the associated session id has changed. This would happen if the connection detected an expired session
        /// and built a new session
        /// </summary>
        public event EventHandler SessionIDChanged;

        /// <summary>
        /// Restarts the server session, and creates a new session ID
        /// </summary>
        /// <param name="throwException">If set to true, the call throws an exception if the call failed</param>
        /// <returns>True if the creation succeed, false otherwise</returns>
        public bool RestartSession(bool throwException)
        {
            var oldSessionId = this.SessionID;
            var ret = RestartSessionInternal(throwException);
            var newSessionId = this.SessionID;
            CheckAndRaiseSessionChanged(oldSessionId, newSessionId);
            return ret;
        }

        /// <summary>
        /// Raises the <see cref="E:OSGeo.MapGuide.MaestroAPI.MgServerConnectionBase.SessionIDChanged"/> event if the
        /// old and new session ids do not match
        /// </summary>
        /// <param name="oldSessionId"></param>
        /// <param name="newSessionId"></param>
        protected void CheckAndRaiseSessionChanged(string oldSessionId, string newSessionId)
        {
            if (!string.IsNullOrEmpty(oldSessionId))
            {
                if (oldSessionId != newSessionId)
                {
                    var h = this.SessionIDChanged;
                    if (h != null)
                        h(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Attempts to create a new session
        /// </summary>
        /// <param name="throwException"></param>
        /// <returns></returns>
        protected abstract bool RestartSessionInternal(bool throwException);

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
        /// Identifies those features that meet the specified spatial selection criteria. This operation is used to implement server-side selection. In addition to a selection set, this operation returns attribute information in case only one feature is selected. 
        /// </summary>
        /// <param name="rtMap">The runtime map to identify features</param>
        /// <param name="maxFeatures">The maximum number of features to return</param>
        /// <param name="wkt">The WKT of the filter geometry</param>
        /// <param name="persist">If true will update the selection set for the given map</param>
        /// <param name="selectionVariant">The type of spatial operator to use for the spatial query</param>
        /// <param name="extraOptions">Extra querying options</param>
        /// <returns></returns>
        public abstract string QueryMapFeatures(RuntimeMap rtMap, int maxFeatures, string wkt, bool persist, string selectionVariant, QueryMapOptions extraOptions);

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer the image should represent</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        public virtual System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type)
        {
            return GetLegendImage(scale, layerdefinition, themeIndex, type, 16, 16, "PNG"); //NOXLATE
        }

        /// <summary>
        /// Gets the legend image.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <param name="layerdefinition">The layerdefinition.</param>
        /// <param name="themeIndex">Index of the theme.</param>
        /// <param name="type">The type.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        abstract public System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi)
        {
            return this.RenderRuntimeMap(map, x, y, scale, width, height, dpi, "PNG", false); //NOXLATE
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi)
        {
            return this.RenderRuntimeMap(map, x1, y1, x2, y2, width, height, dpi, "PNG", false); //NOXLATE
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format)
        {
            return this.RenderRuntimeMap(map, x, y, scale, width, height, dpi, format, false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public virtual System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format)
        {
            return this.RenderRuntimeMap(map, x1, y1, x2, y2, width, height, dpi, format, false);
        }

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        public abstract System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format, bool clip);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
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
        public abstract System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip);

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

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <param name="selectionColor"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public abstract System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, Color selectionColor, int behaviour);

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
