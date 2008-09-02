#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
	/// Primary http based connection to the MapGuide Server
	/// </summary>
	public class HttpServerConnection : ServerConnectionBase, ServerConnectionI, IDisposable
	{
		private WebClient m_wc;
		private RequestBuilder m_reqBuilder;
		
		//These only change after server reboot, so it is probably safe to cache them
		private FeatureProviderRegistry m_featureProviders = null;
		private Hashtable m_cachedProviderCapabilities = null;
		private Version m_siteVersion;

		internal HttpServerConnection()
			: base()
		{
			m_wc = new WebClient();
			m_cachedProviderCapabilities = new Hashtable();

		}

		public HttpServerConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion)
			: this()
		{
			m_reqBuilder = new RequestBuilder(hosturl, locale, sessionid);
			string req = m_reqBuilder.GetSiteVersion();
			SiteVersion sv = null;
			try
			{
				sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), m_wc.OpenRead(req));
			}
			catch(Exception ex)
			{
				sv = null;
				bool ok = false;
				try
				{
					//Retry, and append missing path, if applicable
					if (!hosturl.ToString().EndsWith("/mapagent/mapagent.fcgi"))
					{
						string tmp = hosturl.ToString();
						if (!tmp.EndsWith("/"))
							tmp += "/";
						hosturl = new Uri(tmp + "mapagent/mapagent.fcgi");
						m_reqBuilder = new RequestBuilder(hosturl, locale, sessionid);
						req = m_reqBuilder.GetSiteVersion();
						sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), m_wc.OpenRead(req));
						ok = true;
					}
				}
				catch {}
				
				if (!ok) //Report original error
					throw new Exception("Failed to connect, perhaps session is expired?\nExtended error info: " + ex.Message, ex);
			}
			if (!allowUntestedVersion)
				ValidateVersion(sv);
			m_siteVersion = new Version(sv.Version);
		}

		public HttpServerConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion)
			: this()
		{
			m_reqBuilder = new RequestBuilder(hosturl, locale);
			m_wc.Credentials = new NetworkCredential(username, password);
			string req = m_reqBuilder.CreateSession();

			m_username = username;
			m_password = password;

			try
			{
				this.RestartSession();
			}
			catch(Exception ex)
			{
				throw new Exception("Failed to connect, please check network connection and login information.\nExtended error info: " + ex.Message, ex);
			}

			if (!allowUntestedVersion)
				ValidateVersion(m_siteVersion);
			m_username = username;
			m_password = password;
		}

		public override string SessionID
		{
			get { return m_reqBuilder.SessionID; }
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
			string req = m_reqBuilder.EnumerateResources(startingpoint, depth, type);
			
			//TODO: Cache?
			return (ResourceList)DeserializeObject(typeof(ResourceList), this.OpenRead(req));
		}

		public override FeatureProviderRegistryFeatureProviderCollection FeatureProviders
		{
			get
			{
				string req = m_reqBuilder.GetFeatureProviders();
			
				if (m_featureProviders == null)
					m_featureProviders = (FeatureProviderRegistry)DeserializeObject(typeof(FeatureProviderRegistry), this.OpenRead(req));

				return m_featureProviders.FeatureProvider;
			}
		}

		internal WebClient WebClient { get { return m_wc; } }

		public override string TestConnection(string featuresource)
		{
			string req = m_reqBuilder.TestConnection(featuresource);
				
			try
			{
				byte[] x = this.DownloadData(req);
			}
			catch (WebException wex)
			{
				if (wex.Response != null)
					try
					{
						string result = "";
						using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
						{
							Utility.CopyStream(wex.Response.GetResponseStream(), ms);
							result = System.Text.Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
						}

						if (result.ToLower().IndexOf("<body>") > 0)
							result = result.Substring(result.ToLower().IndexOf("<body>") + 6);

						if (result.ToLower().IndexOf("</body>") > 0)
							result = result.Substring(0, result.ToLower().IndexOf("</body>"));

						return result;
					}
					catch
					{
					}

				if (wex.InnerException == null)
					return wex.Message;
				else
					return wex.InnerException.Message;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			return string.Empty;
		}

		public string TestConnection(string providername, NameValueCollection parameters)
		{
			string req = m_reqBuilder.TestConnection(providername, parameters);
			//System.IO.MemoryStream msx = new System.IO.MemoryStream();
			//System.Net.WebRequest reqp = m_reqBuilder.TestConnectionPost(providername, parameters, msx);
				
			try
			{
				/*msx.Position = 0;
				Utility.CopyStream(msx, reqp.GetRequestStream());
				reqp.GetRequestStream().Flush();
				int f = reqp.GetResponse().GetResponseStream().ReadByte();*/
				byte[] x = this.DownloadData(req);
			}
			catch (WebException wex)
			{
				if (wex.Response != null)
					try
					{
						string result = "";
						using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
						{
							Utility.CopyStream(wex.Response.GetResponseStream(), ms);
							result = System.Text.Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
						}

						if (result.ToLower().IndexOf("<body>") > 0)
							result = result.Substring(result.ToLower().IndexOf("<body>") + 6);

						if (result.ToLower().IndexOf("</body>") > 0)
							result = result.Substring(0, result.ToLower().IndexOf("</body>"));

						return result;
					}
					catch
					{
					}

				if (wex.InnerException == null)
					return wex.Message;
				else
					return wex.InnerException.Message;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			return string.Empty;
		}

		/// <summary>
		/// Placeholder for actual method call
		/// </summary>
		public void DescribeSchema()
		{
			throw new NotImplementedException();
		}



		public OSGeo.MapGuide.MaestroAPI.FdoProviderCapabilities GetProviderCapabilities(string provider)
		{
            if (m_cachedProviderCapabilities == null)
                m_cachedProviderCapabilities = new Hashtable();

			if (m_cachedProviderCapabilities.ContainsKey(provider))
				return (FdoProviderCapabilities)m_cachedProviderCapabilities[provider];

			string req = m_reqBuilder.GetProviderCapabilities(provider);

			//TODO: Cache?
			FdoProviderCapabilities o = (FdoProviderCapabilities)DeserializeObject(typeof(FdoProviderCapabilities), this.OpenRead(req));
			return o;
		}

		public override System.IO.MemoryStream GetResourceData(string resourceID, string dataname)
		{
			string req = m_reqBuilder.GetResourceData(resourceID, dataname);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			Utility.CopyStream(this.OpenRead(req), ms);

#if DEBUG_LASTMESSAGE
			using (System.IO.Stream s = System.IO.File.Open("lastSave.xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
				Utility.CopyStream(ms, s);
#endif
			ms.Position = 0;
			return ms;
		}

        public WebHeaderCollection LastResponseHeaders
        {
            get
            {
                return m_wc.ResponseHeaders;
            }
        }

		public override byte[] GetResourceXmlData(string resourceID)
		{
			ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
			string req = m_reqBuilder.GetResourceContent(resourceID);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			using(System.IO.Stream s = this.OpenRead(req))
				return Utility.StreamAsArray(s);
		}

		/*public object DeserializeItem(System.IO.Stream s)
		{
			if (!s.CanSeek)
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				Utility.CopyStream(s, ms);
				s = ms;
			}

			XmlTextReader xtr = new XmlTextReader(s);
			string r;
			if(xtr.Read())
				r = xtr.Name;
			else
				return null;

			return null;

		}*/

		public System.IO.Stream GetMapDWF(string resourceID)
		{
			ResourceIdentifier.Validate(resourceID, ResourceTypes.MapDefinition);

			string req = m_reqBuilder.GetMapDWF(resourceID);
			return this.OpenRead(req);
		}

		public override void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback)
		{

#if DEBUG_LASTMESSAGE
			using (System.IO.Stream s = System.IO.File.Open("lastSaveData.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
				Utility.CopyStream(stream, s);
#endif
			if (stream.CanSeek)
				stream.Position = 0;

			System.IO.MemoryStream outStream = new System.IO.MemoryStream();
#if DEBUG_LASTMESSAGE
			try 
			{
#endif
				System.Net.WebRequest req = m_reqBuilder.SetResourceData(resourceid, dataname, datatype, outStream, stream, callback);
				req.Credentials = m_wc.Credentials;
				outStream.Position = 0;
				
				//Protect against session expired
				if (this.m_autoRestartSession && m_username != null && m_password != null)
					this.DownloadData(m_reqBuilder.GetSiteVersion());

				//TODO: We need a progress bar for the upload....
				req.Timeout = 1000 * 60 * 15;
				using(System.IO.Stream rs = req.GetRequestStream())
				{
					Utility.CopyStream(outStream, rs);
					rs.Flush();
				}
				using (System.IO.Stream resp = req.GetResponse().GetResponseStream())
				{
					//Do nothing... there is no return value
				}
#if DEBUG_LASTMESSAGE
			} 
			catch 
			{
				using (System.IO.Stream s = System.IO.File.OpenWrite("lastPost.txt"))
					Utility.CopyStream(outStream, s);

				throw;
			}
#endif
					
		}

        public override void SetResourceXmlData(string resourceid, System.IO.Stream content, System.IO.Stream header)
        {
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
#if DEBUG_LASTMESSAGE
			try 
			{
#endif
            //Protect against session expired
            if (this.m_autoRestartSession && m_username != null && m_password != null)
                this.DownloadData(m_reqBuilder.GetSiteVersion());

            System.Net.WebRequest req = m_reqBuilder.SetResource(resourceid, outStream, content, header);
            req.Credentials = m_wc.Credentials;
            outStream.Position = 0;
            try
            {
                using (System.IO.Stream rs = req.GetRequestStream())
                {
                    Utility.CopyStream(outStream, rs);
                    rs.Flush();
                }
                using (System.IO.Stream resp = req.GetResponse().GetResponseStream())
                {
                    //Do nothing... there is no return value
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;
            }
#if DEBUG_LASTMESSAGE
			} 
			catch 
			{
				if (outStream.CanSeek)
					outStream.Position = 0;
				using (System.IO.Stream s = System.IO.File.OpenWrite("lastPost.txt"))
					Utility.CopyStream(outStream, s);

				throw;
			}
#endif
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
			//The request may execeed the url limit of the server, especially when using GeomFromText('...')
			ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Net.WebRequest req = m_reqBuilder.SelectFeatures(resourceID, schema, query, columns, ms);
			req.Timeout = 200 * 1000;
			ms.Position = 0;
#if DEBUG
			string xq = m_reqBuilder.reqAsUrl(resourceID, schema, query, columns);
#endif
            try
            {
                using (System.IO.Stream rs = req.GetRequestStream())
                {
                    Utility.CopyStream(ms, rs);
                    rs.Flush();
                }

                return new FeatureSetReader(req.GetResponse().GetResponseStream());
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.IsSessionExpiredException(ex) && this.AutoRestartSession && this.RestartSession(false))
                        return this.QueryFeatureSource(resourceID, schema, query, columns);
                }
                catch
                {
                    //Throw the original exception, not the secondary one
                }

                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;

            }
		}

		public FeatureSourceDescription DescribeFeatureSource(string resourceID)
		{
			return DescribeFeatureSource(resourceID, "");
		}

		public FeatureSourceDescription DescribeFeatureSource(string resourceID, string schema)
		{
			if (schema != null && schema.IndexOf(":") > 0)
				schema = schema.Substring(0, schema.IndexOf(":"));
			ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
			string req = m_reqBuilder.DescribeSchema(resourceID, schema);

            try
            {
                return new FeatureSourceDescription(this.OpenRead(req));
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.IsSessionExpiredException(ex) && this.AutoRestartSession && this.RestartSession(false))
                        return this.DescribeFeatureSource(resourceID, schema);
                }
                catch
                {
                    //Throw the original exception, not the secondary one
                }

                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;

            }
		}

		/// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="mapdefinition">The mapdefinition to base the map on</param>
		public override void CreateRuntimeMap(string resourceID, string mapdefinition)
		{
			ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
			MapDefinition map = this.GetMapDefinition(mapdefinition);
			CreateRuntimeMap(resourceID, map);
		}

		/// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The mapdefinition to base the map on</param>
		public void CreateRuntimeMap(string resourceID, MapDefinition map)
		{
            ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
			RuntimeClasses.RuntimeMap m = new RuntimeClasses.RuntimeMap(map);
			CreateRuntimeMap(resourceID, m);
		}

		/// <summary>
		/// Creates a runtime map on the server
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The mapdefinition to base the map on</param>
		public void CreateRuntimeMap(string resourceID, RuntimeClasses.RuntimeMap map)
		{
            ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
			string selectionID = resourceID.Substring(0, resourceID.LastIndexOf(".")) + ".Selection";
			SetResourceXmlData(resourceID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(RUNTIMEMAP_XML)));
			SetResourceXmlData(selectionID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(RUNTIMEMAP_SELECTION_XML)));

			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			BinarySerializer.MgBinarySerializer serializer = new BinarySerializer.MgBinarySerializer(ms, m_siteVersion);
			RuntimeClasses.Selection sel = new RuntimeClasses.Selection();
			sel.Serialize(serializer);
			ms.Position = 0;
			SetResourceData(selectionID, "RuntimeData", ResourceDataType.Stream, ms);

			SaveRuntimeMap(resourceID, map);
		}
		/// <summary>
		/// Updates an existing runtime map
		/// </summary>
		/// <param name="resourceID">The target resource id for the runtime map</param>
		/// <param name="map">The runtime map to update with</param>
		public void SaveRuntimeMap(string resourceID, RuntimeClasses.RuntimeMap map)
		{
            ResourceIdentifier.Validate(resourceID, ResourceTypes.RuntimeMap);
			if (!resourceID.StartsWith("Session:" + this.m_reqBuilder.SessionID + "//") || !resourceID.EndsWith(".Map"))
				throw new Exception("Runtime maps must be in the current session repository");

			if (map == null)
				throw new ArgumentNullException("map");

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
			string req = m_reqBuilder.DeleteResourceData(resourceID, dataname);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value
		}

		public ResourceDataList EnumerateResourceData(string resourceID)
		{
			string req = m_reqBuilder.EnumerateResourceData(resourceID);

			using (System.IO.Stream resp = this.OpenRead(req))
				return (ResourceDataList)DeserializeObject(typeof(ResourceDataList), resp);
		}

		public override void DeleteResource(string resourceID)
		{
			string req = m_reqBuilder.DeleteResource(resourceID);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
				//Do nothing... there is no return value
		}

		public void DeleteFolder(string folderPath)
		{
			folderPath = FixAndValidateFolderPath(folderPath);

			string req = m_reqBuilder.DeleteResource(folderPath);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value
		}


		public RuntimeClasses.RuntimeMap GetRuntimeMap(string resourceID)
		{
			if (!resourceID.StartsWith("Session:" + this.m_reqBuilder.SessionID + "//") || !resourceID.EndsWith(".Map"))
				throw new Exception("Runtime maps must be in the current session repository");

			RuntimeClasses.RuntimeMap m = new RuntimeClasses.RuntimeMap();
			m.Deserialize(new BinarySerializer.MgBinaryDeserializer(this.GetResourceData(resourceID, "RuntimeData"), this.SiteVersion));
			if (this.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
				m.DeserializeLayerData(new BinarySerializer.MgBinaryDeserializer(this.GetResourceData(resourceID, "LayerGroupData"), this.SiteVersion));
		
			m.CurrentConnection = this;
			return m;
		}

		public override Version SiteVersion { get { return m_siteVersion; } }

		private CoordinateSystem m_coordsys = null;
		//TODO: Figure out a strategy for cache invalidation 
		//TODO: Figure out if this can work with MapGuide EP 1.0 (just exclude it?)
		public CoordinateSystem CoordinateSystem 
		{ 
			get 
			{ 
				if (m_siteVersion < OSGeo.MapGuide.MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS1_1))
					return null;
				else
				{	
					if (m_coordsys == null)
						m_coordsys = new CoordinateSystem(this, m_reqBuilder);
					return m_coordsys;
				}
			} 
		}

		public System.IO.Stream ExecuteOperation(System.Collections.Specialized.NameValueCollection param)
		{
			return this.OpenRead(m_reqBuilder.BuildRequest(param));
		}

//		public void CreateFolder(string folderpath)
//		{
//			folderpath = FixAndValidateFolderPath(folderpath);
//			System.IO.MemoryStream outStream = new System.IO.MemoryStream();
//			System.Net.WebRequest req = m_reqBuilder.SetResource(folderpath, outStream, null, null);
//			req.Credentials = m_wc.Credentials;
//			outStream.Position = 0;
//			using(System.IO.Stream rs = req.GetRequestStream())
//			{
//				Utility.CopyStream(outStream, rs);
//				rs.Flush();
//			}
//			using (System.IO.Stream resp = req.GetResponse().GetResponseStream())
//			{
//				//Do nothing... there is no return value
//			}
//		}

		/// <summary>
		/// Returns the Uri for the mapagent
		/// </summary>
		public string ServerURI { get { return m_reqBuilder.HostURI; } }


		/// <summary>
		/// Gets a string that can be used to identify the server by a user
		/// </summary>
		public string DisplayName
		{
			get 
			{
				string s = m_reqBuilder.HostURI;
				if (s.ToLower().EndsWith("/mapagent/mapagent.fcgi"))
					s = s.Substring(0, s.Length - "/mapagent/mapagent.fcgi".Length);
				else if (s.ToLower().EndsWith("/mapagent/mapagent.exe"))
					s = s.Substring(0, s.Length - "/mapagent/mapagent.exe".Length);

				/*if (m_wc.Credentials as NetworkCredential != null)
					s += " [" + (m_wc.Credentials as NetworkCredential).UserName + "]"; */

				return s;
			}
		}

		public override ResourceReferenceList EnumerateResourceReferences(string resourceid)
		{
			string req = m_reqBuilder.EnumerateResourceReferences(resourceid);

			using (System.IO.Stream resp = this.OpenRead(req))
				return (ResourceReferenceList)DeserializeObject(typeof(ResourceReferenceList), resp);
		}


		public override void CopyResource(string oldpath, string newpath, bool overwrite)
		{
			string req = m_reqBuilder.CopyResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value

		}

		public override void CopyFolder(string oldpath, string newpath, bool overwrite)
		{
			oldpath = FixAndValidateFolderPath(oldpath);
			newpath = FixAndValidateFolderPath(newpath);

			string req = m_reqBuilder.CopyResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value
		}

		public override void MoveResource(string oldpath, string newpath, bool overwrite)
		{
			string req = m_reqBuilder.MoveResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value
		}

		public override void MoveFolder(string oldpath, string newpath, bool overwrite)
		{
			oldpath = FixAndValidateFolderPath(oldpath);
			newpath = FixAndValidateFolderPath(newpath);

			string req = m_reqBuilder.MoveResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value
		}



		public System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi)
		{
            ResourceIdentifier.Validate(resourceId, ResourceTypes.RuntimeMap);
			string mapname = resourceId.Substring(resourceId.IndexOf("//") + 2);
			mapname = mapname.Substring(0, mapname.LastIndexOf("."));
#if DEBUG
			string s = m_reqBuilder.GetMapImageUrl(mapname, "PNG", null, x, y, scale, dpi, width, height, null, null, null, null);
			return new System.IO.MemoryStream(this.DownloadData(s));
#else
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Net.WebRequest req = m_reqBuilder.GetMapImage(mapname, "PNG", null, x, y, scale, dpi, width, height, null, null, null, null, ms);
            
            //Maksim reported that the rendering times out frequently, so now we wait 5 minutes
            req.Timeout = 5 * 60 * 1000;

			using(System.IO.Stream rs = req.GetRequestStream())
			{
				Utility.CopyStream(ms, rs);
				rs.Flush();
				return req.GetResponse().GetResponseStream();
			}

#endif
		}

/*		/// <summary>
		/// Selects features from a runtime map, returning a selection Xml.
		/// </summary>
		/// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
		/// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
		/// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
		/// <returns>The selection Xml, or an empty string if there were no data.</returns>
		public string QueryMapFeatures(string runtimemap, string wkt, bool persist)
		{
			return QueryMapFeatures(runtimemap, wkt, persist, QueryMapFeaturesLayerAttributes.Default, false);
		}*/

		/// <summary>
		/// Selects features from a runtime map, returning a selection Xml.
		/// </summary>
		/// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
		/// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
		/// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
		/// <param name="attributes">The type of layer to include in the query</param>
		/// <param name="raw">True if the result should contain the tooltip and link info</param>
		/// <returns>The selection Xml, or an empty string if there were no data.</returns>
		public override string QueryMapFeatures(string runtimemap, string wkt, bool persist, QueryMapFeaturesLayerAttributes attributes, bool raw)
		{
			//The request may execeed the url limit of the server, when large geometries
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Net.WebRequest req = m_reqBuilder.QueryMapFeatures(runtimemap, persist, wkt, ms, attributes);
			req.Timeout = 200 * 1000;
			ms.Position = 0;

			using(System.IO.Stream rs = req.GetRequestStream())
			{
				Utility.CopyStream(ms, rs);
				rs.Flush();
			}

			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
#if DEBUG
			System.IO.MemoryStream xms = new System.IO.MemoryStream();
			Utility.CopyStream(req.GetResponse().GetResponseStream(), xms);
			xms.Position = 0;
			string f = System.Text.Encoding.UTF8.GetString(xms.ToArray());
			if (raw)
				return f;
			xms.Position = 0;
			doc.Load(xms);

#else
			
			if (raw)
			{
				System.IO.MemoryStream xms = new System.IO.MemoryStream();
				Utility.CopyStream(req.GetResponse().GetResponseStream(), xms);
				return System.Text.Encoding.UTF8.GetString(xms.ToArray());
			}

			doc.Load(req.GetResponse().GetResponseStream());
#endif
			if (doc.SelectSingleNode("FeatureInformation/FeatureSet") != null && doc["FeatureInformation"]["FeatureSet"].ChildNodes.Count > 0)
				return "<FeatureSet>" + doc["FeatureInformation"]["FeatureSet"].InnerXml + "</FeatureSet>";
			else 
				return "";
		}


		public override bool IsSessionExpiredException(Exception ex)
		{
			if (ex != null && ex.GetType() == typeof(System.Net.WebException))
			{
				System.Net.WebException wex = (System.Net.WebException)ex;
				if (wex.Message.ToLower().IndexOf("session expired") >= 0 || wex.Message.ToLower().IndexOf("session not found") >= 0 || wex.Message.ToLower().IndexOf("mgsessionexpiredexception") >= 0)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns the avalible application templates on the server
		/// </summary>
		/// <returns>The avalible application templates on the server</returns>
		public override ApplicationDefinitionTemplateInfoSet GetApplicationTemplates()
		{
			//TODO: Caching these should be safe
			return (ApplicationDefinitionTemplateInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionTemplateInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationTemplates()));
		}

		/// <summary>
		/// Returns the avalible application widgets on the server
		/// </summary>
		/// <returns>The avalible application widgets on the server</returns>
		public override ApplicationDefinitionWidgetInfoSet GetApplicationWidgets()
		{
			//TODO: Caching these should be safe
			return (ApplicationDefinitionWidgetInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionWidgetInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationWidgets()));
		}

		/// <summary>
		/// Returns the avalible widget containers on the server
		/// </summary>
		/// <returns>The avalible widget containers on the server</returns>
		public override ApplicationDefinitionContainerInfoSet GetApplicationContainers()
		{
			//TODO: Caching these should be safe
			return (ApplicationDefinitionContainerInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionContainerInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationContainers()));
		}

		/// <summary>
		/// Returns the spatial info for a given featuresource
		/// </summary>
		/// <param name="resourceID">The ID of the resource to query</param>
		/// <param name="activeOnly">Query only active items</param>
		/// <returns>A list of spatial contexts</returns>
		public override FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly)
		{
			string req = m_reqBuilder.GetSpatialContextInfo(resourceID, activeOnly);

			FdoSpatialContextList o = (FdoSpatialContextList)DeserializeObject(typeof(FdoSpatialContextList), this.OpenRead(req));
			return o;
		}

		/// <summary>
		/// Gets the names of the identity properties from a feature
		/// </summary>
		/// <param name="resourceID">The resourceID for the FeatureSource</param>
		/// <param name="classname">The classname of the feature, including schema</param>
		/// <returns>A string array with the found identities</returns>
		public override string[] GetIdentityProperties(string resourceID, string classname)
		{
			string[] parts = classname.Split(':');
			string req;
			if (parts.Length == 2)
				req = m_reqBuilder.GetIdentityProperties(resourceID, parts[0], parts[1]);
			else if (parts.Length == 1)
				req = m_reqBuilder.GetIdentityProperties(resourceID, null, parts[0]);
			else
				throw new Exception("Unable to parse classname into class and schema: " + classname);


			XmlDocument doc = new XmlDocument();
			doc.Load(this.OpenRead(req));
			XmlNodeList lst = doc.SelectNodes("/PropertyDefinitions/PropertyDefinition/Name");
			string[] ids = new string[lst.Count];
			for(int i = 0; i < lst.Count; i++)
				ids[i] = lst[i].InnerText;

			return ids;
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

			Uri hosturl = new Uri(m_reqBuilder.HostURI);
			string locale = m_reqBuilder.Locale;

			try
			{
				RequestBuilder reqb = new RequestBuilder(hosturl, locale);
				WebClient wc = new WebClient();
				wc.Credentials = new NetworkCredential(m_username, m_password);
				string req = reqb.CreateSession();

				try
				{
					reqb.SessionID = System.Text.Encoding.Default.GetString(wc.DownloadData(req));
                    if (reqb.SessionID.IndexOf("<") >= 0)
                        throw new Exception("Invalid server token recieved: " + reqb.SessionID);
				}
				catch (Exception ex)
				{
					reqb.SessionID = null;
					bool ok = false;
					try
					{
						//Retry, and append missing path, if applicable
						if (!hosturl.ToString().EndsWith("/mapagent/mapagent.fcgi"))
						{
							string tmp = hosturl.ToString();
							if (!tmp.EndsWith("/"))
								tmp += "/";
							hosturl = new Uri(tmp + "mapagent/mapagent.fcgi");
							reqb = new RequestBuilder(hosturl, locale);
							req = reqb.CreateSession();
							reqb.SessionID = System.Text.Encoding.Default.GetString(wc.DownloadData(req));
                            if (reqb.SessionID.IndexOf("<") >= 0)
                                throw new Exception("Invalid server token recieved: " + reqb.SessionID);
				
							ok = true;
						}
					}
					catch {}

					if (!ok)
					{
						if (throwException) //Report original error
							throw new Exception("Failed to connect, perhaps session is expired?\nExtended error info: " + ex.Message, ex);
						else
							return false;
					}
				}

				//Reset cached items
				m_siteVersion = new Version(((SiteVersion)DeserializeObject(typeof(SiteVersion), wc.OpenRead(reqb.GetSiteVersion()))).Version);

				m_featureProviders = null;
				m_cachedProviderCapabilities = null;
				m_reqBuilder = reqb;
                //This ensures we do not hit the connection limit in .Net
                try { m_wc.Dispose(); }
                catch { }
				m_wc = wc;

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
		/// Downloads data as a byte array. Wrapper function that automatically recreates the session if it has expired.
		/// </summary>
		/// <param name="req">The request URI</param>
		/// <returns>The data at the given location</returns>
		protected byte[] DownloadData(string req)
		{
			string prev_session = m_reqBuilder.SessionID;
			try
			{
				return m_wc.DownloadData(req);
			}
			catch (Exception ex)
			{
                if (!this.m_autoRestartSession || !this.IsSessionExpiredException(ex) || !this.RestartSession(false))
                {
                    Exception ex2 = Utility.ThrowAsWebException(ex);
                    if (ex2 != ex)
                        throw ex2;
                    else
                        throw;
                }
                else
                    //Do not try more than once
                    return m_wc.DownloadData(req.Replace(prev_session, m_reqBuilder.SessionID)); 
			}
		}

		/// <summary>
		/// Opens a stream for reading. Wrapper function that automatically recreates the session if it has expired.
		/// </summary>
		/// <param name="req">The request URI</param>
		/// <returns>The data at the given location</returns>
		protected System.IO.Stream OpenRead(string req)
		{
			string prev_session = m_reqBuilder.SessionID;
			try
			{
				return m_wc.OpenRead(req);
			}
			catch (Exception ex)
			{
                if (!this.m_autoRestartSession || !this.IsSessionExpiredException(ex) || !this.RestartSession(false))
                {
                    Exception ex2 = Utility.ThrowAsWebException(ex);
                    if (ex2 != ex)
                        throw ex2;
                    else
                        throw;
                }
                else
                    //Do not try more than once
                    return m_wc.OpenRead(req.Replace(prev_session, m_reqBuilder.SessionID)); 
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
			string selectionID = runtimeMap.Substring(0, runtimeMap.LastIndexOf(".")) + ".Selection";
			//Assumes the runtime map is created, and has the selection resource
			//SetResourceXmlData(selectionID, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(RUNTIMEMAP_SELECTION_XML)));

			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			BinarySerializer.MgBinarySerializer serializer = new BinarySerializer.MgBinarySerializer(ms, m_siteVersion);
			RuntimeClasses.Selection sel = new RuntimeClasses.Selection(selectionXml);
			sel.Serialize(serializer);
			ms.Position = 0;
			SetResourceData(selectionID, "RuntimeData", ResourceDataType.Stream, ms);
		}

		/// <summary>
		/// Gets the selection from a map
		/// </summary>
		/// <param name="runtimeMap">The resourceID of the runtime map</param>
		/// <returns>The selection xml</returns>
		public override string GetSelectionXml(string runtimeMap)
		{
            ResourceIdentifier.Validate(runtimeMap, ResourceTypes.RuntimeMap);
			string selectionID = runtimeMap.Substring(0, runtimeMap.LastIndexOf(".")) + ".Selection";
			System.IO.MemoryStream ms = GetResourceData(selectionID, "RuntimeData");
			BinarySerializer.MgBinaryDeserializer deserializer = new BinarySerializer.MgBinaryDeserializer(ms, m_siteVersion);
			RuntimeClasses.Selection sel = new RuntimeClasses.Selection();
			sel.Deserialize(deserializer);
			return sel.SelectionXml;
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
			string req = m_reqBuilder.EnumerateUnmanagedData(startpath, filter, recursive, type);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			using(System.IO.Stream s = this.OpenRead(req))
				Utility.CopyStream(s, ms);
			ms.Position = 0;
			return (UnmanagedDataList)DeserializeObject(typeof(UnmanagedDataList), ms);
		}

		/// <summary>
		/// Gets the base url, ea.: http://localhost/mapguide/
		/// </summary>
		public string BaseURL 
		{ 
			get 
			{ 
				string baseurl = this.ServerURI;
				if (baseurl.ToLower().EndsWith("/mapagent.fcgi"))
					baseurl = baseurl.Substring(0, baseurl.Length - "mapagent.fcgi".Length);
			
				if (baseurl.ToLower().EndsWith("/mapagent/"))
					baseurl = baseurl.Substring(0, baseurl.Length - "mapagent/".Length);
				else if (baseurl.ToLower().EndsWith("/mapagent"))
					baseurl = baseurl.Substring(0, baseurl.Length - "mapagent".Length);

				return baseurl;
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
            string param = m_reqBuilder.GetLegendImage(scale, layerdefinition, themeIndex, type);
            return new System.Drawing.Bitmap(this.OpenRead(param));
        }


        /// <summary>
        /// Upload a MapGuide Package file to the server
        /// </summary>
        /// <param name="filename">Name of the file to upload</param>
        /// <param name="callback">A callback argument used to display progress. May be null.</param>
        public override void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    System.Net.WebRequest req = m_reqBuilder.ApplyPackage(fs, callback);
                    req.Credentials = m_wc.Credentials;
                    req.GetRequestStream().Flush();
                    req.GetRequestStream().Close();

                    byte[] buf = new byte[1];
                    System.IO.Stream s = req.GetResponse().GetResponseStream();
                    s.Read(buf, 0, 1);
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;
            }
        }

        public override object GetFolderOrResourceHeader(string resourceID)
        {
            string req = m_reqBuilder.GetResourceHeader(resourceID);
			using(System.IO.Stream s = this.OpenRead(req))
            if (ResourceIdentifier.IsFolderResource(resourceID))
                return this.DeserializeObject<ResourceFolderHeaderType>(s);
            else
                return this.DeserializeObject<ResourceDocumentHeaderType>(s);
        }

        /// <summary>
        /// Gets a list of users in a group
        /// </summary>
        /// <param name="group">The group to retrieve the users from</param>
        /// <returns>The list of users</returns>
        public override UserList EnumerateUsers(string group)
        {
            if (m_cachedUserList == null)
            {
                string req = m_reqBuilder.EnumerateUsers(group);
                using (System.IO.Stream s = this.OpenRead(req))
                    m_cachedUserList = this.DeserializeObject<UserList>(s);
            }
            return m_cachedUserList;
        }

        /// <summary>
        /// Gets a list of all groups on the server
        /// </summary>
        /// <returns>The list of groups</returns>
        public override GroupList EnumerateGroups()
        {
            if (m_cachedGroupList == null)
            {
                string req = m_reqBuilder.EnumerateGroups();
                using (System.IO.Stream s = this.OpenRead(req))
                    m_cachedGroupList = this.DeserializeObject<GroupList>(s);
            }
            return m_cachedGroupList;
        }


        #region IDisposable Members

        public override void Dispose()
        {
            if (m_featureProviders != null)
                m_featureProviders = null;

            if (m_cachedProviderCapabilities != null)
                m_cachedProviderCapabilities = null;

            if (m_wc != null)
            {
                try { m_wc.Dispose(); }
                catch { }
                m_wc = null;
            }
        }

        #endregion
    }
}
