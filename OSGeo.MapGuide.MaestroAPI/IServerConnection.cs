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
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// <para>
    /// MapGuide Platform connection interface. This is the root object of the Maestro API which typically 
    /// represents a session with a MapGuide Server. Connections are created through the 
    /// <see cref="T:OSGeo.MapGuide.MaestroAPI.ConnectionProviderRegistry"/> class.
    /// </para>
    /// <para>
    /// All implementations supports the base services of the MapGuide Geospatial API:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///         <description>Resource Service (<see cref="T:OSGeo.MapGuide.MaestroAPI.Services.IResourceService"/>) for manipulation of repositories and resources</description>
    ///     </item>
    ///     <item>
    ///         <description>Feature Service (<see cref="T:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService"/>) an abstraction layer for querying feature data in technology-independent manner.</description>
    ///     </item>
    ///     <item>
    ///         <description>Coordinate System Catalog (<see cref="T:OSGeo.MapGuide.MaestroAPI.CoordinateSystem.ICoordinateSystemCatalog"/> for querying coordinate systems and for translating WKT, cs code and EPSG codes to other forms</description>    
    ///     </item> 
    /// </list>
    /// <para>
    /// Additional services are supported at various levels depending on the implementation. 
    /// The <see cref="P:OSGeo.MapGuide.MaestroAPI.IServerConnection.Capabilities"/> property provides information about 
    /// what features, services and resource types are not supported.
    /// </para>
    /// </summary>
    public interface IServerConnection
    {
        /// <summary>
        /// Gets the name of the provider of this implementation
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Gets a collection of name-value parameters required to create another copy
        /// of this connection via the <see cref="T:OSGeo.MapGuide.MaestroAPI.ConnectionProviderRegistry"/>
        /// </summary>
        /// <returns></returns>
        NameValueCollection CloneParameters { get; }

        /// <summary>
        /// Returns a clone copy of this connection
        /// </summary>
        /// <returns></returns>
        IServerConnection Clone();

        /// <summary>
        /// Executes the specified load procedure
        /// </summary>
        /// <param name="loadProc"></param>
        /// <param name="callback"></param>
        /// <param name="ignoreUnsupportedFeatures"></param>
        /// <returns></returns>
        string[] ExecuteLoadProcedure(ILoadProcedure loadProc, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack callback, bool ignoreUnsupportedFeatures);
        
        /// <summary>
        /// Executes the load procedure indicated by the specified resource id
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="callback"></param>
        /// <param name="ignoreUnsupportedFeatures"></param>
        /// <returns></returns>
        string[] ExecuteLoadProcedure(string resourceID, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack callback, bool ignoreUnsupportedFeatures);

        /// <summary>
        /// Gets the feature service
        /// </summary>
        IFeatureService FeatureService { get; }

        /// <summary>
        /// Gets the resource service
        /// </summary>
        IResourceService ResourceService { get; }

        /// <summary>
        /// Creates a command of the specified type
        /// </summary>
        /// <remarks>
        /// Some commands may not be supported by the connection. You can find out if the connection supports a particular command through the <see cref="P:OSGeo.MapGuide.MaestroAPI.IServerConnection.Capabilities"/>
        /// </remarks>
        /// <param name="commandType">The type of command to create. See <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.CommandType"/> for allowed values</param>
        /// <returns></returns>
        ICommand CreateCommand(int commandType);

        /// <summary>
        /// Gets the capabilities for this connection. The capabilities describes
        /// what commands and services are supported by this connection
        /// </summary>
        IConnectionCapabilities Capabilities { get; }

        /// <summary>
        /// Gets the session ID for this connection
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// Raised when the session ID has changed
        /// </summary>
        event EventHandler SessionIDChanged;

        /// <summary>
        /// Gets or sets a value indicating if the session should automatically be restarted if it expires
        /// </summary>
        bool AutoRestartSession { get; set; }

        /// <summary>
        /// Gets the service based on the given type
        /// </summary>
        /// <remarks>
        /// Some commands may not be supported by the connection. You can find out if the connection supports a particular command through the <see cref="P:OSGeo.MapGuide.MaestroAPI.IServerConnection.Capabilities"/>
        /// </remarks>
        /// <param name="serviceType">The type of service to create. See <see cref="T:OSGeo.MapGuide.MaestroAPI.Services.ServiceType"/> for allowed values</param>
        /// <returns></returns>
        IService GetService(int serviceType);

        /// <summary>
        /// Gets the max tested version
        /// </summary>
        Version MaxTestedVersion { get; }

        /// <summary>
        /// Gets the version of the site we're connected to
        /// </summary>
        Version SiteVersion { get; }

        /// <summary>
        /// Enables/Disables resource validation
        /// </summary>
        bool DisableValidation { get; set; }

        /// <summary>
        /// Gets the coordinate system catalog
        /// </summary>
        ICoordinateSystemCatalog CoordinateSystemCatalog { get; }
        
        /// <summary>
        /// Gets a string that can be used to identify the server by a user
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Restarts the server session, and creates a new session ID
        /// </summary>
        void RestartSession();

        /// <summary>
        /// Restarts the server session, and creates a new session ID
        /// </summary>
        /// <param name="throwException">If set to true, the call throws an exception if the call failed</param>
        /// <returns>True if the creation succeed, false otherwise</returns>
        bool RestartSession(bool throwException);

        /// <summary>
        /// Enumerates the names of all custom properties for this connection
        /// </summary>
        /// <returns></returns>
        string[] GetCustomPropertyNames();

        /// <summary>
        /// Gets the type of the specified property name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Type GetCustomPropertyType(string name);

        /// <summary>
        /// Sets the value of the specified property name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetCustomProperty(string name, object value);

        /// <summary>
        /// Gets the value of the specified property name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetCustomProperty(string name);

        /// <summary>
        /// Raised when a outbound request has been dispatched
        /// </summary>
        event RequestEventHandler RequestDispatched;

        /// <summary>
        /// Returns a meters-per-unit calculator
        /// </summary>
        /// <returns></returns>
        IMpuCalculator GetCalculator();
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void RequestEventHandler(object sender, RequestEventArgs e);

    /// <summary>
    /// event object containing dispatched request infromation
    /// </summary>
    public class RequestEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public RequestEventArgs(string data)
        {
            this.Data = data;
        }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class ConnectionExtensionMethods
    {
        /// <summary>
        /// Generates the session resource id.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="resType">Type of the res.</param>
        /// <returns></returns>
        public static string GenerateSessionResourceId(this IServerConnection conn, ResourceTypes resType)
        {
            Guid id = Guid.NewGuid();
            return conn.GenerateSessionResourceId(id.ToString(), resType);
        }

        /// <summary>
        /// Generates the session resource id.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="name">The name.</param>
        /// <param name="resType">Type of the res.</param>
        /// <returns></returns>
        public static string GenerateSessionResourceId(this IServerConnection conn, string name, ResourceTypes resType)
        {
            return "Session:" + conn.SessionID + "//" + name + "." + resType.ToString(); //NOXLATE
        }
    }
}
