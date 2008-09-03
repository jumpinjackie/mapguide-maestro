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
using OSGeo.MapGuide;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// A connection to the server, using the Native API.
	/// Only works locally, or with special firewall rules for the server.
	/// </summary>
	public class LocalNativeConnection : ServerConnectionBase, ServerConnectionI, IDisposable 
	{
		private OSGeo.MapGuide.MgSiteConnection m_con;
		private string m_locale;
		private string m_sessionId;

		private LocalNativeConnection()
			: base()
		{
		}

		public LocalNativeConnection(string sessionid)
			: this()
		{
			MgUserInformation mgui = new MgUserInformation(sessionid);
			m_con = new MgSiteConnection(); 
			m_con.Open(mgui);
			m_sessionId = sessionid;
		}

		public LocalNativeConnection(string configFile, string username, string password, string locale)
			: this()
		{
			m_username = username;
			m_password = password;
			m_locale = locale;
			
			OSGeo.MapGuide.MapGuideApi.MgInitializeWebTier(configFile);
			//Throws an exception if it fails
			RestartSession();
		}


        /// <summary>
        /// Returns a working copy of the site connection.
        /// </summary>
        private MgSiteConnection Con
        {
            get
            {
                //It seems that the connection 'forgets' that it is logged in.
                if (string.IsNullOrEmpty(m_con.GetSite().GetCurrentSession()))
                    m_con.Open(new MgUserInformation(this.SessionID));
                return m_con;
            }
        }

		#region ServerConnectionI Members

		public override string SessionID
		{
			get
			{
				return m_sessionId;
			}
		}

		public ResourceList RepositoryResources
		{
			get
			{
				return GetRepositoryResources();
			}
		}

		public override ResourceList GetRepositoryResources(string startingpoint, string type, int depth)
		{
			if (type == null)
				type = "";
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			System.Reflection.MethodInfo mi = res.GetType().GetMethod("EnumerateResources", new Type[] { typeof(MgResourceIdentifier), typeof(int), typeof(string) });
			return (ResourceList) base.DeserializeObject(typeof(ResourceList), Utility.MgStreamToNetStream(res, mi, new object[] {new MgResourceIdentifier(startingpoint), depth, type }));
		}

		public override FeatureProviderRegistryFeatureProviderCollection FeatureProviders
		{
			get
			{
				MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
				return (FeatureProviderRegistryFeatureProviderCollection) base.DeserializeObject(typeof(ResourceList), Utility.MgStreamToNetStream(fes, fes.GetType().GetMethod("GetFeatureProviders"), new object[] { }));
			}
		}

		public string TestConnection(string providername, System.Collections.Specialized.NameValueCollection parameters)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (parameters != null)
			{
				foreach(System.Collections.DictionaryEntry de in parameters)
					sb.Append((string)de.Key + "=" + (string)de.Value + "\t");
				if (sb.Length > 0)
					sb.Length--;
			}
			return fes.TestConnection(providername, sb.ToString()) ? "No errors" : "Unspecified errors";
		}

		public override string TestConnection(string featuresource)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			return fes.TestConnection(new MgResourceIdentifier(featuresource)) ? "No errors" : "Unspecified errors";
		}

		public void DescribeSchema()
		{
			throw new MissingMethodException();
		}


		public FdoProviderCapabilities GetProviderCapabilities(string provider)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.ResourceService) as MgFeatureService;
			return (FdoProviderCapabilities) base.DeserializeObject(typeof(FdoProviderCapabilities), Utility.MgStreamToNetStream(fes, fes.GetType().GetMethod("GetCapabilities"), new object[] { provider }));
		}

		public override System.IO.MemoryStream GetResourceData(string resourceID, string dataname)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			return Utility.MgStreamToNetStream(res, res.GetType().GetMethod("GetResourceData"), new object[] { new MgResourceIdentifier(resourceID), dataname });
		}


		public override byte[] GetResourceXmlData(string resourceID)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			return Utility.MgStreamToNetStream(res, res.GetType().GetMethod("GetResourceContent"), new object[] { new MgResourceIdentifier(resourceID) }).ToArray();
		}

		public override FeatureProviderRegistryFeatureProvider GetFeatureProvider(string providername)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			System.IO.MemoryStream ms = Utility.MgStreamToNetStream(fes, fes.GetType().GetMethod("GetCapabilities"), new object[] { providername });
			return (FeatureProviderRegistryFeatureProvider)DeserializeObject(typeof(FeatureProviderRegistryFeatureProvider), ms);
		}

		public System.IO.Stream GetMapDWF(string resourceID)
		{
			throw new MissingMethodException();
		}

		public override void SetResourceData(string resourceid, string dataname, OSGeo.MapGuide.MaestroAPI.ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback)
		{
            byte[] data = Utility.StreamAsArray(stream);
            if (callback != null)
                callback(0, data.Length, data.Length);
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgByteReader reader = new MgByteReader(data, data.Length, "binary/octet-stream");
			res.SetResourceData(new MgResourceIdentifier(resourceid), dataname, datatype.ToString(), reader);
            
            if (callback != null)
                callback(data.Length, 0, data.Length);
        }

		public override void SetResourceXmlData(string resourceid, System.IO.Stream content, System.IO.Stream header)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;

            byte[] bufHeader = header == null ? new byte[0] : Utility.StreamAsArray(header);
            byte[] bufContent = content == null ? new byte[0] : Utility.StreamAsArray(content);
            MgByteReader rH = bufHeader.Length == 0 ? null : new MgByteReader(bufHeader, bufHeader.Length, "text/xml");
            MgByteReader rC = bufContent.Length == 0 ? null : new MgByteReader(bufContent, bufContent.Length, "text/xml");
            res.SetResource(new MgResourceIdentifier(resourceid), rC, rH);
		}

		public FeatureSetReader QueryFeatureSource(string resourceID, string schema, string query)
		{
			return QueryFeatureSource(resourceID, schema, query, null);
		}

		public FeatureSetReader QueryFeatureSource(string resourceID, string schema)
		{
			return QueryFeatureSource(resourceID, schema, null, null);
		}

		public FeatureSetReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			MgFeatureQueryOptions mgf = new MgFeatureQueryOptions();
			if (query != null)
				mgf.SetFilter(query);

			if (columns != null && columns.Length != 0)
				foreach(string s in columns)
					mgf.AddFeatureProperty(s);

			return new FeatureSetReader(fes.SelectFeatures(new MgResourceIdentifier(resourceID), schema, mgf));
		}

		public override FeatureSourceDescription DescribeFeatureSource(string resourceID)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(fes.DescribeSchemaAsXml(new MgResourceIdentifier(resourceID), "")));
			return new FeatureSourceDescription(ms);
		}

		public override FeatureSourceDescription DescribeFeatureSource(string resourceID, string schema)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
            if (schema != null && schema.IndexOf(":") > 0)
                schema = schema.Split(':')[0];
			System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(fes.DescribeSchemaAsXml(new MgResourceIdentifier(resourceID), schema)));
			return new FeatureSourceDescription(ms);
		}

		public override void CreateRuntimeMap(string resourceID, string mapdefinition)
		{
            string mapname = new ResourceIdentifier(resourceID).Path;
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgMap map = new MgMap();
			map.Create(res, new MgResourceIdentifier(mapdefinition), mapname);
			map.Save(res, new MgResourceIdentifier(resourceID));
        
            //Grrr... This does not happen automatically, even though it is required!
            MgSelection sel = new MgSelection(map);
            sel.Save(res, mapname);
        }

		void OSGeo.MapGuide.MaestroAPI.ServerConnectionI.CreateRuntimeMap(string resourceID, MapDefinition map)
		{
			ResourceIdentifier guid = new ResourceIdentifier(Guid.NewGuid().ToString(), ResourceTypes.MapDefinition, this.SessionID);
			try
			{
				SaveResourceAs(map, guid);
				CreateRuntimeMap(resourceID, guid);
			}
			finally
			{
				try { DeleteResource(guid); }
				catch { }
			}
		}

		void OSGeo.MapGuide.MaestroAPI.ServerConnectionI.CreateRuntimeMap(string resourceID, OSGeo.MapGuide.MaestroAPI.RuntimeClasses.RuntimeMap map)
		{
            if (!ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap))
                throw new ArgumentException("Invalid resourceId", "resourceID");

			string selectionID = resourceID.Substring(0, resourceID.LastIndexOf(".")) + ".Selection";
			SetResourceXmlData(resourceID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(HttpServerConnection.RUNTIMEMAP_XML)));
			SetResourceXmlData(selectionID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(HttpServerConnection.RUNTIMEMAP_SELECTION_XML)));

			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			BinarySerializer.MgBinarySerializer serializer = new BinarySerializer.MgBinarySerializer(ms, this.SiteVersion);
			RuntimeClasses.Selection sel = new RuntimeClasses.Selection();
			sel.Serialize(serializer);
			ms.Position = 0;
			SetResourceData(selectionID, "RuntimeData", ResourceDataType.Stream, ms);

			SaveRuntimeMap(resourceID, map);
		}

		public void SaveRuntimeMap(string resourceID, RuntimeClasses.RuntimeMap map)
		{
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
				map.Serialize(new BinarySerializer.MgBinarySerializer(ms, this.SiteVersion));
				if (this.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				{
					ms2 = new System.IO.MemoryStream();
					map.SerializeLayerData(new BinarySerializer.MgBinarySerializer(ms2, this.SiteVersion));
				}

				SetResourceData(resourceID, "RuntimeData", ResourceDataType.Stream, ms);
				if (ms2 != null)
					SetResourceData(resourceID, "LayerGroupData", ResourceDataType.Stream, ms2);
			}
			finally
			{
				map.Name = r;
				map.ResourceID = t;
			}
		}

		public void DeleteResourceData(string resourceID, string dataname)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.DeleteResourceData(new MgResourceIdentifier(resourceID), dataname);
		}

		public ResourceDataList EnumerateResourceData(string resourceID)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			System.IO.MemoryStream ms = Utility.MgStreamToNetStream(res, res.GetType().GetMethod("EnumerateResourceData"), new object[] { new MgResourceIdentifier(resourceID) });			
			return (ResourceDataList)DeserializeObject(typeof(ResourceDataList), ms);
		}

		public override void DeleteResource(string resourceID)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.DeleteResource(new MgResourceIdentifier(resourceID));
		}

		public void DeleteFolder(string folderPath)
		{
			if (!folderPath.EndsWith("/"))
				folderPath += "/";
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.DeleteResource(new MgResourceIdentifier(folderPath));
		}

		public RuntimeClasses.RuntimeMap GetRuntimeMap(string resourceID)
		{
			RuntimeClasses.RuntimeMap m = new RuntimeClasses.RuntimeMap();
			m.Deserialize(new BinarySerializer.MgBinaryDeserializer(this.GetResourceData(resourceID, "RuntimeData"), this.SiteVersion));
			if (this.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				m.DeserializeLayerData(new BinarySerializer.MgBinaryDeserializer(this.GetResourceData(resourceID, "LayerGroupData"), this.SiteVersion));
		
			m.CurrentConnection = this;
			return m;
		}

		public override Version SiteVersion
		{
			get
			{
				return SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2);
			}
		}


		public CoordinateSystem CoordinateSystem
		{
			get
			{
				throw new MissingMethodException();
			}
		}

		public string DisplayName
		{
			get
			{
				return this.Con.GetSite().GetCurrentSiteAddress();
			}
		}

		public override ResourceReferenceList EnumerateResourceReferences(string resourceid)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			System.IO.MemoryStream ms = Utility.MgStreamToNetStream(res, res.GetType().GetMethod("EnumerateReferences"), new object[] { new MgResourceIdentifier(resourceid) });
			return (ResourceReferenceList)DeserializeObject(typeof(ResourceReferenceList), ms);
		}

		public override void CopyResource(string oldpath, string newpath, bool overwrite)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.CopyResource(new MgResourceIdentifier(oldpath), new MgResourceIdentifier(newpath), overwrite);
		}

		public override void CopyFolder(string oldpath, string newpath, bool overwrite)
		{
			if (!oldpath.EndsWith("/"))
				oldpath += "/";
			if (!newpath.EndsWith("/"))
				newpath += "/";

			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.CopyResource(new MgResourceIdentifier(oldpath), new MgResourceIdentifier(newpath), overwrite);
		}

		public override void MoveResource(string oldpath, string newpath, bool overwrite)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.MoveResource(new MgResourceIdentifier(oldpath), new MgResourceIdentifier(newpath), overwrite);
		}

		public override void MoveFolder(string oldpath, string newpath, bool overwrite)
		{
			if (!oldpath.EndsWith("/"))
				oldpath += "/";
			if (!newpath.EndsWith("/"))
				newpath += "/";

			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			res.MoveResource(new MgResourceIdentifier(oldpath), new MgResourceIdentifier(newpath), overwrite);
		}

		public System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi)
		{
			MgRenderingService rnd = this.Con.CreateService(MgServiceType.RenderingService) as MgRenderingService;
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgGeometryFactory gf = new MgGeometryFactory();

			string mapname = new ResourceIdentifier(resourceId).Path;

			MgMap map = new MgMap();
			map.Open(res, mapname);
			MgSelection sel = new MgSelection(map);
			MgColor color = new MgColor(map.GetBackgroundColor());
		
			object[] args = new object[] { map, sel, gf.CreateCoordinateXY(x, y), scale, width, height, color, "PNG", true };
			return Utility.MgStreamToNetStream(rnd, rnd.GetType().GetMethod("RenderMap"), args);
		}

		public override bool IsSessionExpiredException(Exception ex)
		{
			return ex != null && ex.GetType() == typeof(OSGeo.MapGuide.MgSessionExpiredException) ||  ex.GetType() == typeof(OSGeo.MapGuide.MgSessionNotFoundException);
		}

		/// <summary>
		/// Returns the avalible application templates on the server
		/// </summary>
		/// <returns>The avalible application templates on the server</returns>
		public override ApplicationDefinitionTemplateInfoSet GetApplicationTemplates()
		{
			throw new MissingMethodException();
		}

		/// <summary>
		/// Returns the avalible application widgets on the server
		/// </summary>
		/// <returns>The avalible application widgets on the server</returns>
		public override ApplicationDefinitionWidgetInfoSet GetApplicationWidgets()
		{
			throw new MissingMethodException();
		}

		/// <summary>
		/// Returns the avalible widget containers on the server
		/// </summary>
		/// <returns>The avalible widget containers on the server</returns>
		public override ApplicationDefinitionContainerInfoSet GetApplicationContainers()
		{
			throw new MissingMethodException();
		}

		/// <summary>
		/// Returns the spatial info for a given featuresource
		/// </summary>
		/// <param name="resourceID">The ID of the resource to query</param>
		/// <param name="activeOnly">Query only active items</param>
		/// <returns>A list of spatial contexts</returns>
		public override FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			MgSpatialContextReader rd = fes.GetSpatialContexts(new MgResourceIdentifier(resourceID), activeOnly);
            return this.DeserializeObject(typeof(FdoSpatialContextList), Utility.MgStreamToNetStream(rd, rd.GetType().GetMethod("ToXml"), null)) as FdoSpatialContextList;
		}

		/// <summary>
		/// Gets the names of the identity properties from a feature
		/// </summary>
		/// <param name="resourceID">The resourceID for the FeatureSource</param>
		/// <param name="classname">The classname of the feature, including schema</param>
		/// <returns>A string array with the found identities</returns>
		public override string[] GetIdentityProperties(string resourceID, string classname)
		{
			MgFeatureService fes = this.Con.CreateService(MgServiceType.FeatureService) as MgFeatureService;
			string[] parts = classname.Split(':');
            MgResourceIdentifier resId = new MgResourceIdentifier(resourceID);
			MgPropertyDefinitionCollection props;
		    if (parts.Length == 1)
				parts = new string[] { classname };
			else if (parts.Length != 2)
				throw new Exception("Unable to parse classname into class and schema: " + classname);

            foreach(MgClassDefinition cdef in fes.DescribeSchema(resId, parts[0])[0].GetClasses())
                if (parts.Length == 1 || cdef.Name.ToLower().Trim().Equals(parts[1].ToLower().Trim()))
                {
                    props = cdef.GetIdentityProperties();

                    string[] res = new string[props.Count];
                    for (int i = 0; i < props.Count; i++)
                        res[i] = (props[i] as MgProperty).Name;

                    return res;
                }

            throw new Exception("Unable to find class: " + parts[1] + " in schema " + parts[0]);
		}

		/// <summary>
		/// Restarts the server session, and creates a new session ID
		/// </summary>
		/// <param name="throwException">If set to true, the call throws an exception if the call failed</param>
		/// <returns>True if the creation succeed, false otherwise</returns>
		public override bool RestartSession(bool throwException)
		{
			if (m_username == null || m_password == null)
				if (throwException)
					throw new Exception("Cannot recreate session, because connection was not opened with username and password");
				else
					return false;

			try
			{
				MgUserInformation mgui = new MgUserInformation(m_username, m_password);
				if (m_locale != null)
					mgui.SetLocale(m_locale);
				else
					mgui.SetLocale("en");
				MgSiteConnection con = new MgSiteConnection(); 
				con.Open(mgui);
				string s = con.GetSite().CreateSession();
				if (s == null || s.Trim().Length == 0)
					throw new Exception("Failed to retrieve new session identifier");

				m_sessionId = s;
				m_con = con;
				return true;
			}
			catch
			{
				if (throwException)
					throw;
				else
					return false;
			}
		}

		/// <summary>
		/// Sets the selection of a map
		/// </summary>
		/// <param name="runtimeMap">The resourceID of the runtime map</param>
		/// <param name="selectionXml">The selection xml</param>
		public override void SetSelectionXml(string runtimeMap, string selectionXml)
		{
			ResourceIdentifier.Validate(runtimeMap, ResourceTypes.RuntimeMap);
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgMap map = new MgMap();
            string mapname = new ResourceIdentifier(runtimeMap).Path;
			map.Open(res, mapname);
			MgSelection sel = new MgSelection(map, selectionXml);
			sel.Save(res, mapname);
		}

		/// <summary>
		/// Gets the selection from a map
		/// </summary>
		/// <param name="runtimeMap">The resourceID of the runtime map</param>
		/// <returns>The selection xml</returns>
		public override string GetSelectionXml(string runtimeMap)
		{
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgMap map = new MgMap();
            string mapname = new ResourceIdentifier(runtimeMap).Path;
			map.Open(res, mapname);
			MgSelection sel = new MgSelection(map);
			return sel.ToXml();
		}

		/// <summary>
		/// Enumerates all unmanaged folders, meaning alias'ed folders
		/// </summary>
		/// <param name="type">The type of data to return</param>
		/// <param name="filter">A filter applied to the items</param>
		/// <param name="recursive">True if the list should contains recursive results</param>
		/// <param name="startpath">The path to retrieve the data from</param>
		/// <returns>A list of unmanaged data</returns>
		public override UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
		{
			throw new MissingMethodException();
		}

        public override string QueryMapFeatures(string runtimemap, string wkt, bool persist, QueryMapFeaturesLayerAttributes attributes, bool raw)
        {
            MgRenderingService rs = this.Con.CreateService(MgServiceType.RenderingService) as MgRenderingService;
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
			MgMap map = new MgMap();
			string mapname = runtimemap.IndexOf(":") > 0 ? new ResourceIdentifier(runtimemap).Path : runtimemap;
			map.Open(res, mapname);

            MgWktReaderWriter r = new MgWktReaderWriter();
            MgFeatureInformation info = rs.QueryFeatures(map, null, r.Read(wkt), (int)MgFeatureSpatialOperations.Intersects, "", -1, (int)attributes);

            string xml = System.Text.Encoding.UTF8.GetString(Utility.MgStreamToNetStream(info, info.GetType().GetMethod("ToXml"), null).ToArray()).Trim();

            if (persist)
            {
                MgSelection sel = new MgSelection(map, xml);
                sel.Save(res, mapname);
            }
 
            return xml;
        }

        /// <summary>
        /// Upload a MapGuide Package file to the server
        /// </summary>
        /// <param name="filename">Name of the file to upload</param>
        /// <param name="callback">A callback argument used to display progress. May be null.</param>
        public override void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);

            if (callback != null)
                callback(0, fi.Length, fi.Length);

			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
            MgByteReader rd = new MgByteReader(filename, "application/octet-stream");
            res.ApplyResourcePackage(rd);
            rd.Dispose();


            if (callback != null)
                callback(fi.Length, 0, fi.Length);
        }

        public override object GetFolderOrResourceHeader(string resourceID)
        {
			MgResourceService res = this.Con.CreateService(MgServiceType.ResourceService) as MgResourceService;
            if (ResourceIdentifier.IsFolderResource(resourceID))
                return this.DeserializeObject<ResourceFolderHeaderType>(Utility.MgStreamToNetStream(res, res.GetType().GetMethod("GetResourceHeader"), new object[] { new MgResourceIdentifier(resourceID) }));
            else
                return this.DeserializeObject<ResourceDocumentHeaderType>(Utility.MgStreamToNetStream(res, res.GetType().GetMethod("GetResourceHeader"), new object[] { new MgResourceIdentifier(resourceID) }));
        }


        /// <summary>
        /// Gets a list of users in a group
        /// </summary>
        /// <param name="group">The group to retrieve the users from</param>
        /// <returns>The list of users</returns>
        public override UserList EnumerateUsers(string group)
        {
            if (m_cachedUserList == null)
                m_cachedUserList = this.DeserializeObject<UserList>(Utility.MgStreamToNetStream(this.Con.GetSite(), this.Con.GetSite().GetType().GetMethod("EnumerateUsers"), new object[] { group }));
            return m_cachedUserList;
        }

        /// <summary>
        /// Gets a list of all groups on the server
        /// </summary>
        /// <returns>The list of groups</returns>
        public override GroupList EnumerateGroups()
        {
            if (m_cachedGroupList == null)
                m_cachedGroupList = this.DeserializeObject<GroupList>(Utility.MgStreamToNetStream(this.Con.GetSite(), this.Con.GetSite().GetType().GetMethod("EnumerateGroups"), null));
            return m_cachedGroupList;
        }


		#endregion

        #region IDisposable Members

        public override void Dispose()
        {
            if (m_con != null)
            {
                m_con.Dispose();
                m_con = null;
            }
        }

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer the image should represent</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        public override System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type)
        {
            MgMappingService ms = this.Con.CreateService(MgServiceType.MappingService) as MgMappingService;
            MgResourceIdentifier ldef = new MgResourceIdentifier(layerdefinition);

            return new System.Drawing.Bitmap(Utility.MgStreamToNetStream(ms, ms.GetType().GetMethod("GenerateLegendImage"), new object[] { ldef, scale, 16, 16, "PNG", type, themeIndex }));
        }

        #endregion
    }
}
