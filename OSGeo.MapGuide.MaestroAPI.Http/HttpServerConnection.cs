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
using System.Net;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using System.ComponentModel;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Serialization;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Http;
using System.IO;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using System.Text;
using System.Collections.Generic;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition_1_0_0;
using OSGeo.MapGuide.MaestroAPI.Http.Commands;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using System.Diagnostics;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Primary http based connection to the MapGuide Server
	/// </summary>
    public class HttpServerConnection : MgServerConnectionBase, 
                                        IServerConnection, 
                                        IDisposable, 
                                        IFeatureService, 
                                        IResourceService, 
                                        ITileService, 
                                        IMappingService,
                                        IDrawingService,
                                        IFusionService,
                                        ISiteService
	{
		private RequestBuilder m_reqBuilder;   
		
		//These only change after server reboot, so it is probably safe to cache them
		private FeatureProviderRegistry m_featureProviders = null; //SHARED
		private Hashtable m_cachedProviderCapabilities = null; //SHARED
		private Version m_siteVersion; //SHARED

        private bool mAnonymousUser = false;

		internal HttpServerConnection()
			: base()
		{
			m_cachedProviderCapabilities = new Hashtable();
		}

        internal HttpServerConnection(RequestBuilder builder)
            : this()
        {
            m_reqBuilder = builder;
        }

        public override NameValueCollection CloneParameters
        {
            get
            {
                var nvc = new NameValueCollection();
                nvc[PARAM_URL] = this.BaseURL;
                nvc[CommandLineArguments.Provider] = this.ProviderName;
                nvc[CommandLineArguments.Session] = this.SessionID;
                nvc[PARAM_GEORESTURL] = this.GeoRestUrl;
                nvc[PARAM_GEORESTCONF] = this.GeoRestConfigPath;
                return nvc;
            }
        }

        private GeoRestConnection _geoRestConn;

        internal GeoRestConnection GeoRestConnection { get { return _geoRestConn; } }

        public string GeoRestUrl
        {
            get { return (_geoRestConn == null) ? string.Empty : _geoRestConn.Url; }
        }

        public string GeoRestConfigPath
        {
            get { return (_geoRestConn == null) ? string.Empty : _geoRestConn.ConfigPath; }
        }

        public override string ProviderName
        {
            get { return "Maestro.Http"; }
        }

        /// <summary>
        /// Gets whether this connection was initialised with an Anonymous login. If it was, it will return true. 
        /// If this was not, or it was initialised from an existing session id, then it will return false.
        /// </summary>
        public bool IsAnonymous
        {
            get
            {
                lock (SyncRoot)
                {
                    return mAnonymousUser;
                }
            }
        }

        public const string PARAM_URL = "Url";
        public const string PARAM_SESSION = "SessionId";
        public const string PARAM_LOCALE = "Locale";
        public const string PARAM_UNTESTED = "AllowUntestedVersion";
        public const string PARAM_USERNAME = "Username";
        public const string PARAM_PASSWORD = "Password";

        public const string PARAM_GEORESTURL = "GeoRestUrl";
        public const string PARAM_GEORESTCONF = "GeoRestConfigPath";

        private ICredentials _cred;

        private void InitConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion, string geoRestUrl, string geoRestConfigPath)
        {
            DisableAutoSessionRecovery();
            if (!string.IsNullOrEmpty(geoRestUrl))
            {
                _geoRestConn = new GeoRestConnection(geoRestUrl, geoRestConfigPath);
            }

            m_reqBuilder = new RequestBuilder(hosturl, locale, sessionid, true);
            string req = m_reqBuilder.GetSiteVersion();
            SiteVersion sv = null;
            try
            {
                sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), OpenRead(req));
            }
            catch (Exception ex)
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
                        m_reqBuilder = new RequestBuilder(hosturl, locale, sessionid, true);
                        req = m_reqBuilder.GetSiteVersion();
                        sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), OpenRead(req));
                        ok = true;
                    }
                }
                catch { }

                if (!ok) //Report original error
                    throw new Exception("Failed to connect, perhaps session is expired?\nExtended error info: " + NestedExceptionMessageProcessor.GetFullMessage(ex), ex);
            }
            if (!allowUntestedVersion)
                ValidateVersion(sv);

            lock (SyncRoot)
            {
                m_siteVersion = new Version(sv.Version);
            }
        }

        private void InitConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion, string geoRestUrl, string geoRestConfigPath)
        {
            if (!string.IsNullOrEmpty(geoRestUrl))
            {
                _geoRestConn = new GeoRestConnection(geoRestUrl, geoRestConfigPath);
            }

            m_reqBuilder = new RequestBuilder(hosturl, locale);
            mAnonymousUser = (username == "Anonymous");

            _cred = new NetworkCredential(username, password);
            string req = m_reqBuilder.CreateSession();

            m_username = username;
            m_password = password;

            try
            {
                this.RestartSession();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect, please check network connection and login information.\nExtended error info: " + NestedExceptionMessageProcessor.GetFullMessage(ex), ex);
            }

            if (!allowUntestedVersion)
                ValidateVersion(this.SiteVersion);
            m_username = username;
            m_password = password;
        }

        //This is the constructor used by ConnectionProviderRegistry.CreateConnection

        internal HttpServerConnection(NameValueCollection initParams)
            : this()
        {
            if (initParams[PARAM_URL] == null)
                throw new ArgumentException("Missing required connection parameter: " + PARAM_URL);

            string locale = null;
            bool allowUntestedVersion = true;

            string geoRestUrl = null;
            string geoRestConfig = null;

            if (initParams[PARAM_LOCALE] != null)
                locale = initParams[PARAM_LOCALE];
            if (initParams[PARAM_UNTESTED] != null)
                bool.TryParse(initParams[PARAM_UNTESTED], out allowUntestedVersion);

            if (initParams[PARAM_GEORESTURL] != null)
                geoRestUrl = initParams[PARAM_GEORESTURL];
            if (initParams[PARAM_GEORESTCONF] != null)
                geoRestConfig = initParams[PARAM_GEORESTCONF];

            if (initParams[PARAM_SESSION] != null) 
            {
                string sessionid = initParams[PARAM_SESSION];

                InitConnection(new Uri(initParams[PARAM_URL]), sessionid, locale, allowUntestedVersion, geoRestUrl, geoRestConfig);
            }
            else //Assuming username/password combination
            {
                string pwd = initParams[PARAM_PASSWORD] ?? string.Empty;
                if (initParams[PARAM_USERNAME] == null)
                    throw new ArgumentException("Missing required connection parameter: " + PARAM_USERNAME);

                InitConnection(new Uri(initParams[PARAM_URL]), initParams[PARAM_USERNAME], pwd, locale, allowUntestedVersion, geoRestUrl, geoRestConfig);
            }
        }

        [Obsolete("This will be removed in the future. Use ConnectionProviderRegistry.CreateConnection() instead")]
		public HttpServerConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion)
			: this()
		{
            InitConnection(hosturl, sessionid, locale, allowUntestedVersion, null, null);
		}

        [Obsolete("This will be removed in the future. Use ConnectionProviderRegistry.CreateConnection() instead")]
		public HttpServerConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion)
			: this()
		{
            InitConnection(hosturl, username, password, locale, allowUntestedVersion, null, null);
		}

		public override string SessionID
		{
			get { return m_reqBuilder.SessionID; }
		}

		public override ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren)
		{
			string req = m_reqBuilder.EnumerateResources(startingpoint, depth, type, computeChildren);
			
			//TODO: Cache?
			return (ResourceList)DeserializeObject(typeof(ResourceList), this.OpenRead(req));
		}

        public override FeatureProviderRegistryFeatureProvider[] FeatureProviders
		{
			get
			{
				string req = m_reqBuilder.GetFeatureProviders();
                
                lock (SyncRoot)
                {
                    if (m_featureProviders == null)
                        m_featureProviders = (FeatureProviderRegistry)DeserializeObject(typeof(FeatureProviderRegistry), this.OpenRead(req));
                }

                var providers = new FeatureProviderRegistryFeatureProvider[m_featureProviders.FeatureProvider.Count];
                int i = 0;
                foreach (var p in m_featureProviders.FeatureProvider)
                {
                    providers[i] = p;
                    i++;
                }
                return providers;
			}
		}

		public override string TestConnection(string featuresource)
		{
            string req = m_reqBuilder.TestConnection(featuresource);
            string result = string.Empty;
            try
            {
                byte[] x = this.DownloadData(req);
                //UGLY: Prune out the '\0' chars
                List<byte> bytes = new List<byte>();
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] > 0)
                    {
                        bytes.Add(x[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                result = Encoding.UTF8.GetString(bytes.ToArray());
            }
            catch (WebException wex)
            {
                LogFailedRequest(wex);

                if (wex.Response != null)
                {
                    try
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
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
                }

                if (wex.InnerException == null)
                    return wex.Message;
                else
                    return wex.InnerException.Message;
            }
            catch (Exception ex)
            {
                result = NestedExceptionMessageProcessor.GetFullMessage(ex);
            }

            return result;
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
                LogFailedRequest(wex);

                if (wex.Response != null)
                {
                    try
                    {
                        string result = "";
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
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
                }

                if (wex.InnerException == null)
                    return wex.Message;
                else
                    return wex.InnerException.Message;
            }
            catch (Exception ex)
            {
                return NestedExceptionMessageProcessor.GetFullMessage(ex);
            }

            return string.Empty;
		}

        public override System.IO.Stream GetResourceData(string resourceID, string dataname)
		{
			string req = m_reqBuilder.GetResourceData(resourceID, dataname);
            return this.OpenRead(req);
            /*
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			Utility.CopyStream(this.OpenRead(req), ms);

#if DEBUG_LASTMESSAGE
			using (System.IO.Stream s = System.IO.File.Open("lastSave.xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
				Utility.CopyStream(ms, s);
#endif
			ms.Position = 0;
			return ms;
             */
		}

		public override Stream GetResourceXmlData(string resourceID)
		{
			ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
			string req = m_reqBuilder.GetResourceContent(resourceID);
            return this.OpenRead(req);
		}

		public override void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback)
		{
            //Protect against session expired
            if (this.m_autoRestartSession && m_username != null && m_password != null)
                this.DownloadData(m_reqBuilder.GetSiteVersion());

            //Use the old code path if stream is under 50MB (implying seekable too)
            if (stream.CanSeek && stream.Length < 50 * 1024 * 1024)
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
                    req.Credentials = _cred;
                    outStream.Position = 0;

                    //TODO: We need a progress bar for the upload....
                    req.Timeout = 1000 * 60 * 15;
                    using (System.IO.Stream rs = req.GetRequestStream())
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
            else
            {
                //Dump to temp file
                string tmp = Path.GetTempFileName();
                try
                {
                    using (var fw = File.OpenWrite(tmp))
                    {
                        Utility.CopyStream(stream, fw);
                    }
                    var fi = new FileInfo(tmp);
                    NameValueCollection nvc = m_reqBuilder.SetResourceDataParams(resourceid, dataname, datatype);
                    nvc.Add("DATALENGTH", fi.Length.ToString());
                    HttpUploadFile(m_reqBuilder.HostURI, tmp, "DATA", "application/octet-stream", nvc, callback);
                }
                finally
                {
                    if (File.Exists(tmp))
                    {
                        try
                        {
                            File.Delete(tmp);
                            Debug.WriteLine("Deleted: " + tmp);
                        }
                        catch { }
                    }
                }
            }
		}
        
        //Source: http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        private void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, Utility.StreamCopyProgressDelegate callback)
        {
            Debug.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            //DO NOT BUFFER. Otherwise this will still OOM on really large files
            wr.AllowWriteStreamBuffering = false;
            wr.KeepAlive = true;
            wr.Credentials = _cred;

            //Pre-compute request body size
            {
                long contentLength = 0L;
                foreach (string key in nvc.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.ASCII.GetBytes(formitem);
                    contentLength += formitembytes.Length;
                    contentLength += boundarybytes.Length;
                }
                contentLength += boundarybytes.Length;
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.ASCII.GetBytes(header);
                contentLength += headerbytes.Length;
                var fi = new FileInfo(file);
                contentLength += fi.Length;
                contentLength += trailer.Length;

                wr.ContentLength = contentLength;
            }

            using (Stream rs = wr.GetRequestStream())
            {
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.ASCII.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.ASCII.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                /*
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                */
                Utility.CopyStream(fileStream, rs, callback, 1024);
                fileStream.Close();

                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    Debug.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }
            }
        }

        public override void SetResourceXmlData(string resourceid, System.IO.Stream content, System.IO.Stream header)
        {
            bool exists = ResourceExists(resourceid);

            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
#if DEBUG_LASTMESSAGE
			try 
			{
#endif
            //Protect against session expired
            if (this.m_autoRestartSession && m_username != null && m_password != null)
                this.DownloadData(m_reqBuilder.GetSiteVersion());

            System.Net.WebRequest req = m_reqBuilder.SetResource(resourceid, outStream, content, header);
            req.Credentials = _cred;
            outStream.Position = 0;
            try
            {
                using (System.IO.Stream rs = req.GetRequestStream())
                {
                    Utility.CopyStream(outStream, rs);
                    rs.Flush();
                }
                var wresp = req.GetResponse();
                if (wresp is HttpWebResponse)
                {
                    HttpWebResponse httpresp = (HttpWebResponse)wresp;
                    LogResponse(httpresp);
                }

                using (System.IO.Stream resp = wresp.GetResponseStream())
                {
                    //Do nothing... there is no return value
                }
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);

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

            if (exists)
                OnResourceUpdated(resourceid);
            else
                OnResourceAdded(resourceid);
        }
        
        private void LogResponse(HttpWebResponse resp)
        {
            OnRequestDispatched(string.Format("{0:d} {1} {2} {3}", resp.StatusCode, resp.StatusDescription, resp.Method, GetResponseString(resp)));
        }

        private string GetResponseString(HttpWebResponse resp)
        {
            if (resp.Method == "GET")
                return resp.ResponseUri.AbsolutePath + resp.ResponseUri.Query;
            else
                return resp.ResponseUri.AbsolutePath;
        }

        private void LogFailedRequest(WebException ex)
        {
            var resp = ex.Response as HttpWebResponse;
            if (resp != null)
            {
                OnRequestDispatched(string.Format("{0:d} {1} {2} {3}", resp.StatusCode, resp.StatusDescription, resp.Method, GetResponseString(resp)));
            }
        }

        public IReader ExecuteSqlQuery(string featureSourceID, string sql)
        {
            ResourceIdentifier.Validate(featureSourceID, ResourceTypes.FeatureSource);
            string req = m_reqBuilder.ExecuteSqlQuery(featureSourceID, sql);

            return new XmlSqlResultReader(this.OpenRead(req));
        }

		public IFeatureReader QueryFeatureSource(string resourceID, string schema, string query)
		{
			return QueryFeatureSource(resourceID, schema, query, null);
		}

		public IFeatureReader QueryFeatureSource(string resourceID, string schema)
		{
			return QueryFeatureSource(resourceID, schema, null, null);
		}

		public IFeatureReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns)
		{
            return QueryFeatureSource(resourceID, schema, query, columns, null);
		}

        public IFeatureReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns, NameValueCollection computedProperties)
        {
            return (IFeatureReader)QueryFeatureSourceCore(false, resourceID, schema, query, columns, computedProperties);
        }

        private IReader QueryFeatureSourceCore(bool aggregate, string resourceID, string schema, string query, string[] columns, NameValueCollection computedProperties)
        {
            //The request may execeed the url limit of the server, especially when using GeomFromText('...')
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Net.WebRequest req = m_reqBuilder.SelectFeatures(aggregate, resourceID, schema, query, columns, computedProperties, ms);
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

                var resp = (HttpWebResponse)req.GetResponse();
                LogResponse(resp);

                if (aggregate)
                    return new XmlDataReader(resp.GetResponseStream());
                else
                    return new XmlFeatureReader(resp.GetResponseStream());
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);
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


        public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns)
        {
            return QueryFeatureSourceCore(true, resourceID, schema, filter, columns, null);
        }

        public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, NameValueCollection aggregateFunctions)
        {
            return QueryFeatureSourceCore(true, resourceID, schema, filter, null, aggregateFunctions);
        }

        protected override FeatureSourceDescription DescribeFeatureSourceInternal(string resourceID)
		{
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            string req = m_reqBuilder.DescribeSchema(resourceID);

            try
            {
                return new FeatureSourceDescription(this.OpenRead(req));
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);
                try
                {
                    if (this.IsSessionExpiredException(ex) && this.AutoRestartSession && this.RestartSession(false))
                        return this.DescribeFeatureSource(resourceID);
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

        public override FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string[] classNames)
        {
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            string req = m_reqBuilder.DescribeSchemaPartial(resourceID, schema, classNames);
            try
            {
                var fsd = new FeatureSourceDescription(this.OpenRead(req));
                return fsd.Schemas[0];
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);
                try
                {
                    if (this.IsSessionExpiredException(ex) && this.AutoRestartSession && this.RestartSession(false))
                        return this.DescribeFeatureSourcePartial(resourceID, schema, classNames);
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

		public override FeatureSchema DescribeFeatureSource(string resourceID, string schema)
		{
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            string req = m_reqBuilder.DescribeSchema(resourceID, schema);

            try
            {
                var fsd = new FeatureSourceDescription(this.OpenRead(req));
                return fsd.Schemas[0];
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);
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

            OnResourceDeleted(resourceID);
		}

		

        public override Version SiteVersion
        {
            get 
            {
                lock (SyncRoot)
                {
                    return m_siteVersion;
                }
            }
        }

        //For unit testing purposes
        internal void SetSiteVersion(Version v)
        {
            lock (SyncRoot)
            {
                m_siteVersion = v;
            }
        }

        private ICoordinateSystemCatalog m_coordsys = null;
		//TODO: Figure out a strategy for cache invalidation 
		//TODO: Figure out if this can work with MapGuide EP 1.0 (just exclude it?)
		public ICoordinateSystemCatalog CoordinateSystemCatalog 
		{ 
			get 
			{ 
				if (this.SiteVersion < OSGeo.MapGuide.MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS1_1))
					return null;
				else
				{	
					if (m_coordsys == null)
						m_coordsys = new HttpCoordinateSystemCatalog(this, m_reqBuilder);
					return m_coordsys;
				}
			} 
		}

		public System.IO.Stream ExecuteOperation(System.Collections.Specialized.NameValueCollection param)
		{
			return this.OpenRead(m_reqBuilder.BuildRequest(param));
		}

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

				return s + " (v" + this.SiteVersion.ToString() + ")";
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
            bool exists = ResourceExists(newpath);

			string req = m_reqBuilder.CopyResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value

            if (exists)
                OnResourceUpdated(newpath);
            else
                OnResourceAdded(newpath);

            //HACK: the COPYRESOURCE call does not update timestamps of the target
            //if it already exists.
            Touch(newpath);
		}

		public override void CopyFolder(string oldpath, string newpath, bool overwrite)
		{
			oldpath = FixAndValidateFolderPath(oldpath);
			newpath = FixAndValidateFolderPath(newpath);

            bool exists = ResourceExists(newpath);

			string req = m_reqBuilder.CopyResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value

            if (exists)
                OnResourceUpdated(newpath);
            else
                OnResourceAdded(newpath);
		}

		public override void MoveResource(string oldpath, string newpath, bool overwrite)
		{
            bool exists = ResourceExists(newpath);

			string req = m_reqBuilder.MoveResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value

            OnResourceDeleted(oldpath);
            if (exists)
                OnResourceUpdated(newpath);
            else
                OnResourceAdded(newpath);
		}

		public override void MoveFolder(string oldpath, string newpath, bool overwrite)
		{
			oldpath = FixAndValidateFolderPath(oldpath);
			newpath = FixAndValidateFolderPath(newpath);

            bool exists = ResourceExists(newpath);

			string req = m_reqBuilder.MoveResource(oldpath, newpath, overwrite);

			using (System.IO.Stream resp = this.OpenRead(req))
				resp.ReadByte();
			//Do nothing... there is no return value

            OnResourceDeleted(oldpath);
            if (exists)
                OnResourceUpdated(newpath);
            else
                OnResourceAdded(newpath);
		}

        public override System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, Color selectionColor, int behavior)
        {
            //This API was introduced in MGOS 2.1 so this won't work with older versions
            if (this.SiteVersion < new Version(2, 1, 0))
                throw new NotSupportedException();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var req = m_reqBuilder.GetDynamicMapOverlayImage(map.Name, (selection == null ? string.Empty : selection.ToXml()), format, selectionColor, behavior);

            return this.OpenRead(req);
        }

        public override System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, bool keepSelection)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Net.WebRequest req = m_reqBuilder.GetDynamicMapOverlayImage(map.Name, (selection == null ? string.Empty : selection.ToXml()), format, ms);

            //Maksim reported that the rendering times out frequently, so now we wait 5 minutes
            req.Timeout = 5 * 60 * 1000;

            using (System.IO.Stream rs = req.GetRequestStream())
            {
                Utility.CopyStream(ms, rs);
                rs.Flush();
                var resp = req.GetResponse();
                var hwr = resp as HttpWebResponse;
                if (hwr != null)
                    LogResponse(hwr);

                return resp.GetResponseStream();
            }
        }

        public Stream RenderMapLegend(RuntimeMap map, int width, int height, System.Drawing.Color backgroundColor, string format)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            string req = m_reqBuilder.RenderMapLegend(map.Name, width, height, ColorTranslator.ToHtml(backgroundColor), format);

            return this.OpenRead(req);
        }

		public override System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format, bool clip)
		{
            var resourceId = map.ResourceID;
            ResourceIdentifier.Validate(resourceId, ResourceTypes.RuntimeMap);
			string mapname = resourceId.Substring(resourceId.IndexOf("//") + 2);
			mapname = mapname.Substring(0, mapname.LastIndexOf("."));
#if DEBUG
			string s = m_reqBuilder.GetMapImageUrl(mapname, format, null, x, y, scale, dpi, width, height, clip, null, null, null, null);
			return new System.IO.MemoryStream(this.DownloadData(s));
#else
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Net.WebRequest req = m_reqBuilder.GetMapImage(mapname, format, null, x, y, scale, dpi, width, height, clip, null, null, null, null, ms);
            
            //Maksim reported that the rendering times out frequently, so now we wait 5 minutes
            req.Timeout = 5 * 60 * 1000;

			using(System.IO.Stream rs = req.GetRequestStream())
			{
				Utility.CopyStream(ms, rs);
				rs.Flush();
				var resp = req.GetResponse();

                var hwr = resp as HttpWebResponse;
                if (hwr != null)
                    LogResponse(hwr);

                return resp.GetResponseStream();
			}

#endif
		}

        public override System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip)
        {
            var resourceId = map.ResourceID;
            ResourceIdentifier.Validate(resourceId, ResourceTypes.RuntimeMap);
            string mapname = resourceId.Substring(resourceId.IndexOf("//") + 2);
            mapname = mapname.Substring(0, mapname.LastIndexOf("."));
#if DEBUG
            string s = m_reqBuilder.GetMapImageUrl(mapname, format, null, x1, y1, x2, y2, dpi, width, height, clip, null, null, null, null);
            return new System.IO.MemoryStream(this.DownloadData(s));
#else
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Net.WebRequest req = m_reqBuilder.GetMapImage(mapname, format, null, x1, y1, x2, y2, dpi, width, height, clip, null, null, null, null, ms);
            
            //Maksim reported that the rendering times out frequently, so now we wait 5 minutes
            req.Timeout = 5 * 60 * 1000;

			using(System.IO.Stream rs = req.GetRequestStream())
			{
				Utility.CopyStream(ms, rs);
				rs.Flush();
                var resp = req.GetResponse();
                var hwr = resp as HttpWebResponse;

                if (hwr != null)
                    LogResponse(hwr);
    
                return resp.GetResponseStream();
			}

#endif
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
		public IApplicationDefinitionTemplateInfoSet GetApplicationTemplates()
		{
			//TODO: Caching these should be safe
            return (IApplicationDefinitionTemplateInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionTemplateInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationTemplates()));
		}

		/// <summary>
		/// Returns the avalible application widgets on the server
		/// </summary>
		/// <returns>The avalible application widgets on the server</returns>
		public IApplicationDefinitionWidgetInfoSet GetApplicationWidgets()
		{
			//TODO: Caching these should be safe
            return (IApplicationDefinitionWidgetInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionWidgetInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationWidgets()));
		}

		/// <summary>
		/// Returns the avalible widget containers on the server
		/// </summary>
		/// <returns>The avalible widget containers on the server</returns>
        public IApplicationDefinitionContainerInfoSet GetApplicationContainers()
		{
			//TODO: Caching these should be safe
			return (IApplicationDefinitionContainerInfoSet)base.DeserializeObject(typeof(ApplicationDefinitionContainerInfoSet), this.OpenRead(m_reqBuilder.EnumerateApplicationContainers()));
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

        private readonly object SyncRoot = new object();

		/// <summary>
		/// Restarts the server session, and creates a new session ID
		/// </summary>
		/// <param name="throwException">If set to true, the call throws an exception if the call failed</param>
		/// <returns>True if the creation succeed, false otherwise</returns>
		protected override bool RestartSessionInternal(bool throwException)
		{
			if (m_username == null || m_password == null)
				if (throwException)
					throw new Exception("Cannot recreate session, because connection was not opened with username and password");
				else
					return false;

			Uri hosturl = new Uri(m_reqBuilder.HostURI);
			string locale = m_reqBuilder.Locale;
            string oldSessionId = m_reqBuilder.SessionID;

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
                    else
                        CheckAndRaiseSessionChanged(oldSessionId, reqb.SessionID);
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
                        {
                            if (ex is WebException) //These exceptions, we just want the underlying message. No need for 50 bajillion nested exceptions
                                throw;
                            else //We don't know what this could be so grab everything
                                throw new Exception("Failed to connect, perhaps session is expired?\nExtended error info: " + ex.Message, ex);
                        }
                        else
                            return false;
                    }
                    else
                    {
                        CheckAndRaiseSessionChanged(oldSessionId, reqb.SessionID);
                    }
				}

				//Reset cached items
                lock (SyncRoot)
                {
                    m_siteVersion = new Version(((SiteVersion)DeserializeObject(typeof(SiteVersion), wc.OpenRead(reqb.GetSiteVersion()))).Version);

                    m_featureProviders = null;
                    m_cachedProviderCapabilities = null;
                    m_reqBuilder = reqb;
                }
               
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
		internal byte[] DownloadData(string req)
		{
			string prev_session = m_reqBuilder.SessionID;
			try
			{
                var httpreq = HttpWebRequest.Create(req);
                var httpresp = (HttpWebResponse)httpreq.GetResponse();
                LogResponse(httpresp);
                using (var st = httpresp.GetResponseStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        Utility.CopyStream(st, ms);
                        return ms.GetBuffer();
                    }
                }
			}
			catch (Exception ex)
			{
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);

                if (!this.m_autoRestartSession || !this.IsSessionExpiredException(ex) || !this.RestartSession(false))
                {
                    Exception ex2 = Utility.ThrowAsWebException(ex);
                    if (ex2 != ex)
                        throw ex2;
                    else
                        throw;
                }
                else
                {
                    //Do not try more than once
                    req = req.Replace(prev_session, m_reqBuilder.SessionID);

                    var httpreq = HttpWebRequest.Create(req);
                    var httpresp = httpreq.GetResponse();

                    using (var st = httpresp.GetResponseStream())
                    {
                        using (var ms = new MemoryStream())
                        {
                            Utility.CopyStream(st, ms);
                            return ms.GetBuffer();
                        }
                    }
                }
			}
		}

		/// <summary>
		/// Opens a stream for reading. Wrapper function that automatically recreates the session if it has expired.
		/// </summary>
		/// <param name="req">The request URI</param>
		/// <returns>The data at the given location</returns>
		internal System.IO.Stream OpenRead(string req)
		{
			string prev_session = m_reqBuilder.SessionID;
			try
			{
                var httpreq = HttpWebRequest.Create(req);
                if (_cred != null)
                    httpreq.Credentials = _cred;
                var httpresp = (HttpWebResponse)httpreq.GetResponse();
                LogResponse(httpresp);
                return httpresp.GetResponseStream();
			}
			catch (Exception ex)
			{
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);

                var sessionRecreated = false;
                if (this.IsSessionExpiredException(ex))
                    sessionRecreated = this.RestartSession(false);
                if (!this.m_autoRestartSession || !this.IsSessionExpiredException(ex) || !sessionRecreated)
                {
                    Exception ex2 = Utility.ThrowAsWebException(ex);
                    if (ex2 != ex)
                        throw ex2;
                    else
                        throw;
                }
                else
                {
                    req = req.Replace(prev_session, m_reqBuilder.SessionID);
                    var httpreq = HttpWebRequest.Create(req);
                    if (_cred != null)
                        httpreq.Credentials = _cred;
                    var httpresp = httpreq.GetResponse();
                    return httpresp.GetResponseStream();
                }
			}
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
        public override System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format)
        {
            string param = m_reqBuilder.GetLegendImage(scale, layerdefinition, themeIndex, type, width, height, format);
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
                    req.Credentials = _cred;
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
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);

                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;
            }
        }

        public override void UpdateRepository(string resourceId, ResourceFolderHeaderType header)
        {
            try
            {
                System.Net.WebRequest req = m_reqBuilder.UpdateRepository(resourceId, this.SerializeObject(header));
                req.Credentials = _cred;
                req.GetRequestStream().Flush();
                req.GetRequestStream().Close();

                byte[] buf = new byte[1];
                System.IO.Stream s = req.GetResponse().GetResponseStream();
                s.Read(buf, 0, 1);
                s.Close();
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    LogFailedRequest((WebException)ex);

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

        public override bool ResourceExists(string resourceid)
        {
            try
            {
                string req = m_reqBuilder.ResourceExists(resourceid);
                using (System.IO.Stream s = this.OpenRead(req))
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    return sr.ReadToEnd().Trim().Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception ex)
            {
                try { return base.ResourceExists(resourceid); }
                catch { throw ex; } //Throw original error
            }
        }

        public string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString)
        {
            string req = m_reqBuilder.GetConnectionPropertyValues(providerName, propertyName, partialConnectionString);
            using (System.IO.Stream s = this.OpenRead(req))
            {
                OSGeo.MapGuide.ObjectModels.Common.StringCollection strc = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(s);
                return strc.Item.ToArray();
            }
        }


        #region IDisposable Members

        public override void Dispose()
        {
            lock (SyncRoot)
            {
                if (m_featureProviders != null)
                    m_featureProviders = null;

                if (m_cachedProviderCapabilities != null)
                    m_cachedProviderCapabilities = null;
            }
        }

        #endregion

        public override System.IO.Stream GetTile(string mapdefinition, string baselayergroup, int col, int row, int scaleindex, string format)
        {
            string req = string.Empty;
            if (mAnonymousUser)
                req = m_reqBuilder.GetTileAnonymous(mapdefinition, baselayergroup, row, col, scaleindex, format);
            else
                req = m_reqBuilder.GetTile(mapdefinition, baselayergroup, row, col, scaleindex, format, _cred == null);
            return this.OpenRead(req);
        }

        /// <summary>
        /// Gets or sets the agent reported to MapGuide. 
        /// Free form text, will appear in the log files.
        /// Default is MapGuide Maestro API
        /// </summary>
        public string UserAgent
        {
            get 
            {
                if (m_reqBuilder != null)
                    return m_reqBuilder.UserAgent;
                return string.Empty;
            }
            set 
            { 
                if (m_reqBuilder != null)
                    m_reqBuilder.UserAgent = value; 
            }
        }

        public bool SupportsResourcePreviews
        {
            get { return true; }
        }

        public IFeatureService FeatureService
        {
            get { return this; }
        }

        public IResourceService ResourceService
        {
            get { return this; }
        }

        public IConnectionCapabilities Capabilities
        {
            get { return new HttpCapabilities(this); }
        }

        public IService GetService(int serviceType)
        {
            ServiceType st = (ServiceType)serviceType;
            switch (st)
            {
                case ServiceType.Drawing:
                case ServiceType.Feature:
                case ServiceType.Mapping:
                case ServiceType.Resource:
                case ServiceType.Tile:
                case ServiceType.Site:
                    return this;
                case ServiceType.Fusion:
                    if (this.SiteVersion >= new Version(2, 0))
                        return this;
                    break;
            }
            throw new UnsupportedServiceTypeException(st);
        }

        public const string PROP_USER_AGENT = "UserAgent";
        public const string PROP_BASE_URL = "BaseUrl";
       
        public override string[] GetCustomPropertyNames()
        {
            return new string[] { PROP_USER_AGENT, PROP_BASE_URL };
        }

        /// <summary>
        /// Gets or sets the number of worker threads to spawn when initializing
        /// a runtime map
        /// </summary>
        public int RuntimeMapWorkerCount
        {
            get;
            set;
        }

        public override Type GetCustomPropertyType(string name)
        {
            if (name == PROP_USER_AGENT)
                return typeof(string);
            else if (name == PROP_BASE_URL)
                return typeof(string);
            else
                throw new CustomPropertyNotFoundException();
        }

        public override void SetCustomProperty(string name, object value)
        {
            if (name == PROP_USER_AGENT)
                this.UserAgent = value.ToString();
            else
                throw new CustomPropertyNotFoundException();
        }

        public override object GetCustomProperty(string name)
        {
            if (name == PROP_USER_AGENT)
                return this.UserAgent;
            else if (name == PROP_BASE_URL)
                return this.BaseURL;
            else
                throw new CustomPropertyNotFoundException();
        }

        protected override IServerConnection GetInterface()
        {
            return this;
        }

        public System.IO.Stream DescribeDrawing(string resourceID)
        {
            string req = m_reqBuilder.DescribeDrawing(resourceID);
            return this.OpenRead(resourceID);
        }

        public string[] EnumerateDrawingLayers(string resourceID, string sectionName)
        {
            string req = m_reqBuilder.EnumerateDrawingLayers(resourceID, sectionName);
            using (System.IO.Stream s = this.OpenRead(req))
            {
                var list = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(s);
                //Workaround for #1727
                var dict = new Dictionary<string, string>();
                foreach (var it in list.Item)
                {
                    if (!dict.ContainsKey(it))
                        dict.Add(it, it);
                }
                return new List<string>(dict.Values).ToArray();
            }
        }

        public DrawingSectionResourceList EnumerateDrawingSectionResources(string resourceID, string sectionName)
        {
            string req = m_reqBuilder.EnumerateDrawingSectionResources(resourceID, sectionName);
            using (System.IO.Stream s = this.OpenRead(req))
                return this.DeserializeObject<DrawingSectionResourceList>(s);
        }

        public DrawingSectionList EnumerateDrawingSections(string resourceID)
        {
            string req = m_reqBuilder.EnumerateDrawingSections(resourceID);
            using (System.IO.Stream s = this.OpenRead(req))
                return this.DeserializeObject<DrawingSectionList>(s);
        }

        public string GetDrawingCoordinateSpace(string resourceID)
        {
            string req = m_reqBuilder.GetDrawingCoordinateSpace(resourceID);
            using (System.IO.StreamReader s = new System.IO.StreamReader(this.OpenRead(req)))
                return s.ReadToEnd();
        }

        public System.IO.Stream GetDrawing(string resourceID)
        {
            string req = m_reqBuilder.GetDrawing(resourceID);
            return this.OpenRead(req);
        }

        public System.IO.Stream GetLayer(string resourceID, string sectionName, string layerName)
        {
            string req = m_reqBuilder.GetDrawingLayer(resourceID, sectionName, layerName);
            return this.OpenRead(req);
        }

        public System.IO.Stream GetSection(string resourceID, string sectionName)
        {
            string req = m_reqBuilder.GetDrawingSection(resourceID, sectionName);
            return this.OpenRead(req);
        }

        public System.IO.Stream GetSectionResource(string resourceID, string resourceName)
        {
            string req = m_reqBuilder.GetDrawingSectionResource(resourceID, resourceName);
            return this.OpenRead(req);
        }

        public override DataStoreList EnumerateDataStores(string providerName, string partialConnString)
        {
            string req = m_reqBuilder.EnumerateDataStores(providerName, partialConnString);
            using (System.IO.Stream s = this.OpenRead(req))
            {
                var list = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.Common.DataStoreList>(s);
                return list;
            }
        }

        public FdoProviderCapabilities GetProviderCapabilities(string provider)
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

        public override ILongTransactionList GetLongTransactions(string resourceId, bool activeOnly)
        {
            string req = m_reqBuilder.GetLongTransactions(resourceId, activeOnly);
            return DeserializeObject<FdoLongTransactionList>(this.OpenRead(req));
        }

        public override ConfigurationDocument GetSchemaMapping(string provider, string partialConnString)
        {
            string req = m_reqBuilder.GetSchemaMapping(provider, partialConnString);
            return ConfigurationDocument.Load(this.OpenRead(req));
        }

        public override bool MoveFolderWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
        {
            if (this.SiteVersion >= new Version(2, 2)) //new way
            {
                //Unfortunately because this is all batched server-side, there is no
                //meaningful way to track progress
                LengthyOperationProgressArgs la = new LengthyOperationProgressArgs("Moving resource...", -1); //LOCALIZEME
                if (progress != null)
                    progress(this, la);

                oldpath = FixAndValidateFolderPath(oldpath);
                newpath = FixAndValidateFolderPath(newpath);

                string req = m_reqBuilder.MoveResource(oldpath, newpath, true);
                req += "&CASCADE=1";

                using (System.IO.Stream resp = this.OpenRead(req))
                    resp.ReadByte();

                return true;
            }
            else //old way
            {
                return base.MoveFolderWithReferences(oldpath, newpath, callback, progress);
            }
        }

        public override bool MoveResourceWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
        {
            if (this.SiteVersion >= new Version(2, 2)) //new way
            {
                //Unfortunately because this is all batched server-side, there is no
                //meaningful way to track progress
                LengthyOperationProgressArgs la = new LengthyOperationProgressArgs("Moving resource...", -1); //LOCALIZEME
                if (progress != null)
                    progress(this, la);

                string req = m_reqBuilder.MoveResource(oldpath, newpath, true);
                req += "&CASCADE=1";

                using (System.IO.Stream resp = this.OpenRead(req))
                    resp.ReadByte();

                return true;
            }
            else //old way
            {
                return base.MoveResourceWithReferences(oldpath, newpath, callback, progress);
            }
        }

        public override string QueryMapFeatures(RuntimeMap map, int maxFeatures, string wkt, bool persist, string selectionVariant, QueryMapOptions extraOptions)
        {
            string runtimeMapName = map.Name;
            //The request may execeed the url limit of the server, when large geometries
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Net.WebRequest req = m_reqBuilder.QueryMapFeatures(runtimeMapName, maxFeatures, wkt, persist, selectionVariant, extraOptions, ms);
            req.Timeout = 200 * 1000;
            ms.Position = 0;

            using (System.IO.Stream rs = req.GetRequestStream())
            {
                Utility.CopyStream(ms, rs);
                rs.Flush();
            }

            using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
                return sr.ReadToEnd();
        }

        public override string[] GetSchemas(string resourceId)
        {
            var req = m_reqBuilder.GetSchemas(resourceId);
            using (var s = this.OpenRead(req))
            {
                var sc = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(s);
                return sc.Item.ToArray();
            }
        }

        public override string[] GetClassNames(string resourceId, string schemaName)
        {
            var req = m_reqBuilder.GetClassNames(resourceId, schemaName);
            using (var s = this.OpenRead(req))
            {
                var sc = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(s);
                return sc.Item.ToArray();
            }
        }

        protected override ClassDefinition GetClassDefinitionInternal(string resourceId, string schemaName, string className)
        {
            var req = m_reqBuilder.GetClassDefinition(resourceId, schemaName, className);
            using (var s = this.OpenRead(req))
            {
                var fsd = new FeatureSourceDescription(s);
                //We can't just assume first class item is the one, as ones that do not take
                //class name hints will return the full schema
                var schema = fsd.GetSchema(schemaName);
                if (schema != null)
                {
                    if (schema.Classes.Count > 1)
                    {
                        //Since we have the full schema anyway, let's cache these other classes
                        ClassDefinition ret = null;
                        foreach (var cls in schema.Classes)
                        {
                            string key = resourceId + "!" + cls.QualifiedName;
                            m_classDefinitionCache[key] = cls;

                            if (cls.Name == className)
                                ret = cls;
                        }
                        return ret;
                    }
                    else
                    {
                        return schema.GetClass(className);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public override IServerConnection Clone()
        {
            if (this.IsAnonymous)
                return new HttpServerConnection(new Uri(this.ServerURI), "Anonymous", "", null, true);
            else
                return new HttpServerConnection(new Uri(this.ServerURI), this.SessionID, null, true);
        }

        public override SiteInformation GetSiteInfo()
        {
            var req = m_reqBuilder.GetSiteInfo();
            using (var s = this.OpenRead(req))
            {
                return this.DeserializeObject<SiteInformation>(s);
            }
        }

        public override ICommand CreateCommand(int cmdType)
        {
            CommandType ct = (CommandType)cmdType;
            if (ct == CommandType.InsertFeature)
                return new GeoRestInsertFeatures(this);
            else if (ct == CommandType.GetResourceContents)
                return new HttpGetResourceContents(this);
            else if (ct == CommandType.GetFdoCacheInfo)
                return new HttpGetFdoCacheInfo(this);
            else if (ct == CommandType.CreateRuntimeMap)
                return new HttpCreateRuntimeMap(this);
            else if (ct == CommandType.DescribeRuntimeMap)
                return new HttpDescribeRuntimeMap(this);
            return base.CreateCommand(cmdType);
        }

        internal FdoCacheInfo GetFdoCacheInfo()
        {
            var req = m_reqBuilder.GetFdoCacheInfo();
            using (var s = this.OpenRead(req))
            {
                var info = this.DeserializeObject<FdoCacheInfo>(s);

                return info;
            }
        }

        internal ObjectModels.RuntimeMap.IRuntimeMapInfo DescribeRuntimeMap(string mapName, int requestedFeatures, int iconsPerScaleRange, string iconFormat, int iconWidth, int iconHeight)
        {
            var req = m_reqBuilder.DescribeRuntimeMap(mapName, requestedFeatures, iconsPerScaleRange, iconFormat, iconWidth, iconHeight);
            using (var s = this.OpenRead(req))
            {
                var info = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.RuntimeMap.RuntimeMap>(s);
                return info;
            }
        }

        internal ObjectModels.RuntimeMap.IRuntimeMapInfo CreateRuntimeMap(string mapDefinition, string targetMapName, int requestedFeatures, int iconsPerScaleRange, string iconFormat, int iconWidth, int iconHeight)
        {
            var req = m_reqBuilder.CreateRuntimeMap(mapDefinition, targetMapName, requestedFeatures, iconsPerScaleRange, iconFormat, iconWidth, iconHeight);
            using (var s = this.OpenRead(req))
            {
                var info = this.DeserializeObject<OSGeo.MapGuide.ObjectModels.RuntimeMap.RuntimeMap>(s);
                return info;
            }
        }
    }
}
