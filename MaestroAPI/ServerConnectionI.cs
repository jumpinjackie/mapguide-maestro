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
using System.Net;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Primary connection to the MapGuide Server
	/// </summary>
	public interface ServerConnectionI : IDisposable
	{
		string SessionID { get; }
		ResourceList RepositoryResources { get; }
		ResourceList GetRepositoryResources();
		ResourceList GetRepositoryResources(int depth);
		ResourceList GetRepositoryResources(string startingpoint, int depth);
		ResourceList GetRepositoryResources(string startingpoint);
		ResourceList GetRepositoryResources(string startingpoint, string type);
		ResourceList GetRepositoryResources(string startingpoint, string type, int depth);
		FeatureProviderRegistryFeatureProviderCollection FeatureProviders { get; }
		string TestConnection(string providername, NameValueCollection parameters);
		string TestConnection(string featuresource);
		string TestConnection(FeatureSource feature);

		/// <summary>
		/// Gets or sets a value indicating if the session should automatically be restarted if it expires
		/// </summary>
		bool AutoRestartSession { get; set; }

		/// <summary>
		/// Placeholder for actual method call
		/// </summary>
		void DescribeSchema();


		OSGeo.MapGuide.MaestroAPI.WebLayout GetWebLayout(string resourceID);
		OSGeo.MapGuide.MaestroAPI.MapDefinition GetMapDefinition(string resourceID);
		OSGeo.MapGuide.MaestroAPI.LayerDefinition GetLayerDefinition(string resourceID);
		OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType GetApplicationDefinition(string resourceID);
		OSGeo.MapGuide.MaestroAPI.FdoProviderCapabilities GetProviderCapabilities(string provider);
		System.IO.MemoryStream GetResourceData(string resourceID, string dataname);
        OSGeo.MapGuide.MaestroAPI.ResourceDocumentHeaderType GetResourceHeader(string resourceID);
        OSGeo.MapGuide.MaestroAPI.ResourceFolderHeaderType GetFolderHeader(string resourceID);
		OSGeo.MapGuide.MaestroAPI.FeatureSource GetFeatureSource(string resourceID);
		byte[] GetResourceXmlData(string resourceID);
		object GetResource(string resourceID);

		/// <summary>
		/// Removes the version numbers from a providername
		/// </summary>
		/// <param name="providername">The name of the provider, with or without version numbers</param>
		/// <returns>The provider name without version numbers</returns>
		string RemoveVersionFromProviderName(string providername);

		/// <summary>
		/// Returns an installed provider, given the name of the provider
		/// </summary>
		/// <param name="providername">The name of the provider</param>
		/// <returns>The first matching provider or null</returns>
		FeatureProviderRegistryFeatureProvider GetFeatureProvider(string providername);
		System.IO.Stream GetMapDWF(string resourceID);
		object DeserializeObject(Type type, System.IO.Stream data);
        T DeserializeObject<T>(System.IO.Stream data);
		System.IO.MemoryStream SerializeObject(object o);
		void SerializeObject(object o, System.IO.Stream stream);
		Version MaxTestedVersion { get; }
		void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, System.IO.Stream stream);
        void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback);
        void SetResourceXmlData(string resourceid, System.IO.Stream stream);
		FeatureSetReader QueryFeatureSource(string resourceID, string schema, string query);
		FeatureSetReader QueryFeatureSource(string resourceID, string schema);
		FeatureSetReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns);
		FeatureSourceDescription DescribeFeatureSource(string resourceID);
		FeatureSourceDescription DescribeFeatureSource(string resourceID, string schema);
		FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly);
        void SetFolderHeader(string resourceID, ResourceFolderHeaderType header);
        void SetResourceHeader(string resourceID, ResourceDocumentHeaderType header);

        /// <summary>
        /// Creates a runtime map on the server. 
        /// The map name will be the name of the resource, without path information.
        /// This is equivalent to the way the AJAX viewer creates the runtime map.
        /// </summary>
        /// <param name="resourceID">The mapDefinition resource id</param>
        void CreateRuntimeMap(string resourceID);
        
        /// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="mapdefinition">The mapdefinition to base the map on</param>
		void CreateRuntimeMap(string resourceID, string mapdefinition);

		/// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The mapdefinition to base the map on</param>
		void CreateRuntimeMap(string resourceID, MapDefinition map);

		/// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The mapdefinition to base the map on</param>
		void CreateRuntimeMap(string resourceID, RuntimeClasses.RuntimeMap map);

		/// <summary>
		/// Updates an existing runtime map
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The runtime map to update with</param>
		void SaveRuntimeMap(string resourceID, RuntimeClasses.RuntimeMap map);
		void DeleteResourceData(string resourceID, string dataname);
		ResourceDataList EnumerateResourceData(string resourceID);
		void DeleteResource(string resourceID);
		void DeleteFolder(string folderPath);
		RuntimeClasses.RuntimeMap GetRuntimeMap(string resourceID);
		Version SiteVersion { get; }
		bool DisableValidation { get; set; }
		CoordinateSystem CoordinateSystem { get; }
		bool HasFolder(string folderpath);
		void CreateFolder(string folderpath);

		/// <summary>
		/// Gets a string that can be used to identify the server by a user
		/// </summary>
		string DisplayName { get; }

		ResourceReferenceList EnumerateResourceReferences(string resourceid);
		void CopyResource(string oldpath, string newpath, bool overwrite);
		void CopyFolder(string oldpath, string newpath, bool overwrite);
		void MoveResource(string oldpath, string newpath, bool overwrite);
		void MoveFolder(string oldpath, string newpath, bool overwrite);
		bool MoveResourceWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress);
		bool MoveFolderWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress);
		bool CopyFolderWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress);
		bool ResourceExists(string resourceid);
		System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi);

		/// <summary>
		/// Saves a WebLayout, using its originating resourceId
		/// </summary>
		/// <param name="resource">The WebLayout to save</param>
		void SaveResource(WebLayout resource);

		/// <summary>
		/// Saves a FeatureSource, using its originating resourceId
		/// </summary>
		/// <param name="resource">The FeatureSource to save</param>
		void SaveResource(FeatureSource resource);

		/// <summary>
		/// Saves a LayerDefinition, using its originating resourceId
		/// </summary>
		/// <param name="resource">The LayerDefinition to save</param>
		void SaveResource(LayerDefinition resource);

		/// <summary>
		/// Saves a MapDefinition, using its originating resourceId
		/// </summary>
		/// <param name="resource">The MapDefintion to save</param>
		void SaveResource(MapDefinition resource);

		/// <summary>
		/// Saves an object into the repository
		/// </summary>
		/// <param name="resource">The object to save</param>
		/// <param name="resourceid">The resourceId to save the object as</param>
		void SaveResourceAs(object resource, string resourceid);

		/// <summary>
		/// Saves a MapDefinition under a different resourceID
		/// </summary>
		/// <param name="resource">The MapDefinition to save</param>
		/// <param name="resourceid">The new path of the MapDefinition</param>
		void SaveResourceAs(MapDefinition resource, string resourceid);

		/// <summary>
		/// Saves a LayerDefinition under a different resourceID
		/// </summary>
		/// <param name="resource">The LayerDefinition to save</param>
		/// <param name="resourceid">The new path of the LayerDefinition</param>
		void SaveResourceAs(LayerDefinition resource, string resourceid);

		/// <summary>
		/// Saves a FeatureSource under a different resourceID
		/// </summary>
		/// <param name="resource">The FeatureSource to save</param>
		/// <param name="resourceid">The new path of the FeatureSource</param>
		void SaveResourceAs(FeatureSource resource, string resourceid);

		/// <summary>
		/// Saves a WebLayout under a different resourceID
		/// </summary>
		/// <param name="resource">The WebLayout to save</param>
		/// <param name="resourceid">The new path of the WebLayout</param>
		void SaveResourceAs(WebLayout resource, string resourceid);

		ApplicationDefinitionTemplateInfoSet GetApplicationTemplates();

		/// <summary>
		/// Returns the avalible application widgets on the server
		/// </summary>
		/// <returns>The avalible application widgets on the server</returns>
		ApplicationDefinitionWidgetInfoSet GetApplicationWidgets();

		/// <summary>
		/// Returns the avalible widget containers on the server
		/// </summary>
		/// <returns>The avalible widget containers on the server</returns>
		ApplicationDefinitionContainerInfoSet GetApplicationContainers();

		/// <summary>
		/// Gets the resource type from a resourceID
		/// </summary>
		/// <param name="resourceID">The resourceID for the resource</param>
		/// <returns>The type of the given item, throws an exception if the type does not exist</returns>
		Type GetResourceType(string resourceID);

		/// <summary>
		/// Gets the resource type from a resourceID
		/// </summary>
		/// <param name="resourceID">The resourceID for the resource</param>
		/// <returns>The type of the given item, returns null if no such type exists</returns>
		Type TryGetResourceType(string resourceID);

		/// <summary>
		/// Gets the names of the identity properties from a feature
		/// </summary>
		/// <param name="resourceID">The resourceID for the FeatureSource</param>
		/// <param name="classname">The classname of the feature, including schema</param>
		/// <returns>A string array with the found identities</returns>
		string[] GetIdentityProperties(string resourceID, string classname);

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
		/// Sets the selection of a map
		/// </summary>
		/// <param name="runtimeMap">The resourceID of the runtime map</param>
		/// <param name="selectionXml">The selection xml</param>
		void SetSelectionXml(string runtimeMap, string selectionXml);

		/// <summary>
		/// Gets the selection from a map
		/// </summary>
		/// <param name="runtimeMap">The resourceID of the runtime map</param>
		/// <returns>The selection xml</returns>
		string GetSelectionXml(string runtimeMap);

		/// <summary>
		/// Enumerates all unmanaged folders, meaning alias'ed folders
		/// </summary>
		/// <param name="type">The type of data to return</param>
		/// <param name="filter">A filter applied to the items</param>
		/// <param name="recursive">True if the list should contains recursive results</param>
		/// <param name="startpath">The path to retrieve the data from</param>
		/// <returns>A list of unmanaged data</returns>
		UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type);

        /// <summary>
		/// Selects features from a runtime map, returning a selection Xml.
		/// </summary>
		/// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
		/// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
		/// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
		/// <param name="attributes">The type of layer to include in the query</param>
		/// <param name="raw">True if the result should contain the tooltip and link info</param>
		/// <returns>The selection Xml, or an empty string if there were no data.</returns>
        string QueryMapFeatures(string runtimemap, string wkt, bool persist, QueryMapFeaturesLayerAttributes attributes, bool raw);

        /// <summary>
		/// Selects features from a runtime map, returning a selection Xml.
		/// </summary>
		/// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
		/// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
		/// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
		/// <returns>The selection Xml, or an empty string if there were no data.</returns>
        string QueryMapFeatures(string runtimemap, string wkt, bool persist);

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer the image should represent</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type);

        /// <summary>
        /// Upload a MapGuide Package file to the server
        /// </summary>
        /// <param name="filename">Name of the file to upload</param>
        /// <param name="callback">A callback argument used to display progress. May be null.</param>
        void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback);

        /// <summary>
        /// Gets a list of all users on the server
        /// </summary>
        /// <returns>The list of users</returns>
        UserList EnumerateUsers();

        /// <summary>
        /// Gets a list of users in a group
        /// </summary>
        /// <param name="group">The group to retrieve the users from</param>
        /// <returns>The list of users</returns>
        UserList EnumerateUsers(string group);

        /// <summary>
        /// Gets a list of all groups on the server
        /// </summary>
        /// <returns>The list of groups</returns>
        GroupList EnumerateGroups();
	}
}
