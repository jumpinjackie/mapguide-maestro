#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition.v1_0_0;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    /// <summary>
    /// The new http mapagent server connection based on HttpClient
    /// </summary>
    public class NeoHttpServerConnection : MgServerConnectionBase,
                                           IServerConnection,
                                           IDisposable,
                                           IHttpRequest,
                                           IFeatureService,
                                           IResourceService,
                                           ITileService,
                                           IMappingService,
                                           IDrawingService,
                                           IFusionService,
                                           ISiteService
    {
        private HttpClient _httpClient;
        private bool _isAnonymous;
        private RequestBuilder _reqBuilder;
        private ConcurrentDictionary<string, object> _properties;
        private Lazy<ICoordinateSystemCatalog> _catalog;
        private Lazy<GroupList> _userGroups;
        private Lazy<FeatureProviderRegistry> _fdoProviders;

        private NeoHttpServerConnection()
            : base()
        {
            _httpClient = new HttpClient();
            _properties = new ConcurrentDictionary<string, object>();
            _catalog = new Lazy<ICoordinateSystemCatalog>(() =>
            {
                return new HttpCoordinateSystemCatalog(this, _reqBuilder);
            });
            _userGroups = new Lazy<GroupList>(() => 
            {
                var req = _reqBuilder.EnumerateGroups();
                return OpenReadSync<GroupList>(req);
            });
            _fdoProviders = new Lazy<FeatureProviderRegistry>(() =>
            {
                var req = _reqBuilder.GetFeatureProviders();
                return OpenReadSync<FeatureProviderRegistry>(req);
            });
        }

        //This is the constructor used by ConnectionProviderRegistry.CreateConnection

        internal NeoHttpServerConnection(NameValueCollection initParams)
            : this()
        {
            if (initParams[Parameters.Url] == null)
                throw new ArgumentException("Missing required connection parameter: " + Parameters.Url);

            string locale = null;
            bool allowUntestedVersion = true;

            if (initParams[Parameters.Locale] != null)
                locale = initParams[Parameters.Locale];
            if (initParams[Parameters.AllowUntestedVersion] != null)
                bool.TryParse(initParams[Parameters.AllowUntestedVersion], out allowUntestedVersion);

            if (initParams[Parameters.SessionId] != null)
            {
                string sessionid = initParams[Parameters.SessionId];

                InitConnection(new Uri(initParams[Parameters.Url]), sessionid, locale, allowUntestedVersion);
            }
            else //Assuming username/password combination
            {
                string pwd = initParams[Parameters.Password] ?? string.Empty;
                if (initParams[Parameters.Username] == null)
                    throw new ArgumentException("Missing required connection parameter: " + Parameters.Username);

                InitConnection(new Uri(initParams[Parameters.Url]), initParams[Parameters.Username], pwd, locale, allowUntestedVersion);
            }
        }

        internal NeoHttpServerConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion)
            : this()
        {
            InitConnection(hosturl, sessionid, locale, allowUntestedVersion);
        }

        internal NeoHttpServerConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion)
            : this()
        {
            InitConnection(hosturl, username, password, locale, allowUntestedVersion);
        }

        private void InitConnection(Uri hosturl, string sessionid, string locale, bool allowUntestedVersion)
        {
            DisableAutoSessionRecovery();
            IHttpRequest request = this;
            _reqBuilder = new RequestBuilder(hosturl, locale, sessionid, true);
            _httpClient.BaseAddress = new Uri(_reqBuilder.HostURI);
            string req = _reqBuilder.GetSiteVersion();
            SiteVersion sv = null;
            try
            {
                sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), request.OpenReadSync(req));
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
                        _reqBuilder = new RequestBuilder(hosturl, locale, sessionid, true);
                        req = _reqBuilder.GetSiteVersion();
                        sv = (SiteVersion)DeserializeObject(typeof(SiteVersion), request.OpenReadSync(req));
                        ok = true;
                    }
                }
                catch { }

                if (!ok) //Report original error
                    throw new Exception("Failed to connect, perhaps session is expired?\nExtended error info: " + NestedExceptionMessageProcessor.GetFullMessage(ex), ex);
            }
            if (!allowUntestedVersion)
                ValidateVersion(sv);

            _properties[nameof(SiteVersion)] = new Version(sv.Version);
        }

        private void InitConnection(Uri hosturl, string username, string password, string locale, bool allowUntestedVersion)
        {
            _reqBuilder = new RequestBuilder(hosturl, locale);
            _httpClient.BaseAddress = new Uri(_reqBuilder.HostURI);
            _isAnonymous = (username == "Anonymous");

            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            string req = _reqBuilder.CreateSession();

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

        public class CustomProperties
        {
            public const string UserAgent = nameof(UserAgent);
            public const string BaseUrl = nameof(BaseUrl);
        }

        public class Parameters
        {
            public const string Url = nameof(Url);
            public const string SessionId = nameof(SessionId);
            public const string Locale = nameof(Locale);
            public const string AllowUntestedVersion = nameof(AllowUntestedVersion);
            public const string Username = nameof(Username);
            public const string Password = nameof(Password);
        }

        public IFeatureService FeatureService => this;

        public IResourceService ResourceService => this;

        public IConnectionCapabilities Capabilities => new NeoHttpCapabilities(this);

        public ICoordinateSystemCatalog CoordinateSystemCatalog
        {
            get
            {
                if (this.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_1))
                    return null;

                return _catalog.Value;
            }
        }

        public string DisplayName
        {
            get
            {
                string s = _reqBuilder.HostURI;
                if (s.ToLower().EndsWith("/mapagent/mapagent.fcgi"))
                    s = s.Substring(0, s.Length - "/mapagent/mapagent.fcgi".Length);
                else if (s.ToLower().EndsWith("/mapagent/mapagent.exe"))
                    s = s.Substring(0, s.Length - "/mapagent/mapagent.exe".Length);

                /*if (m_wc.Credentials as NetworkCredential != null)
                    s += " [" + (m_wc.Credentials as NetworkCredential).UserName + "]"; */

                return $"{s} (v{this.SiteVersion.ToString() })";
            }
        }

        public override string ProviderName => "Maestro.Http"; //NOXLATE

        /// <summary>
        /// Gets the base url, ea.: http://localhost/mapguide/
        /// </summary>
        private string GetBaseUrl()
        {
            string baseurl = _reqBuilder.HostURI;
            if (baseurl.ToLower().EndsWith("/mapagent.fcgi")) //NOXLATE
                baseurl = baseurl.Substring(0, baseurl.Length - "mapagent.fcgi".Length); //NOXLATE

            if (baseurl.ToLower().EndsWith("/mapagent/")) //NOXLATE
                baseurl = baseurl.Substring(0, baseurl.Length - "mapagent/".Length); //NOXLATE
            else if (baseurl.ToLower().EndsWith("/mapagent")) //NOXLATE
                baseurl = baseurl.Substring(0, baseurl.Length - "mapagent".Length); //NOXLATE

            return baseurl;
        }

        public override NameValueCollection CloneParameters
        {
            get
            {
                var nvc = new NameValueCollection();
                nvc[Parameters.Url] = _reqBuilder.HostURI;
                nvc[CommandLineArguments.Provider] = this.ProviderName;
                nvc[CommandLineArguments.Session] = this.SessionID;
                return nvc;
            }
        }

        public override string SessionID => _reqBuilder.SessionID;

        public override FeatureProviderRegistryFeatureProvider[] FeatureProviders
        {
            get
            {
                var providers = _fdoProviders.Value;
                return providers.FeatureProvider.ToArray();
            }
        }

        public override Version SiteVersion
        {
            get
            {
                object ver;
                if (_properties.TryGetValue(nameof(SiteVersion), out ver))
                {
                    return ver as Version;
                }
                return null;
            }
        }

        public override IServerConnection Clone()
        {
            if (_isAnonymous)
                return new NeoHttpServerConnection(new Uri(_reqBuilder.HostURI), "Anonymous", string.Empty, null, true); //NOXLATE
            else
                return new NeoHttpServerConnection(new Uri(_reqBuilder.HostURI), this.SessionID, null, true);
        }

        private async Task<IReader> QueryFeatureSourceCoreAsync(bool aggregate, string resourceID, string schema, string query, string[] columns, NameValueCollection computedProperties)
        {
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            var stream = await _reqBuilder.SelectFeaturesAsync(_httpClient, aggregate, resourceID, schema, query, columns, computedProperties).ConfigureAwait(false);
            if (aggregate)
                return new XmlDataReader(stream);
            else
                return new XmlFeatureReader(stream);
        }

        private IReader QueryFeatureSourceCore(bool aggregate, string resourceID, string schema, string query, string[] columns, NameValueCollection computedProperties)
        {
            return QueryFeatureSourceCoreAsync(aggregate, resourceID, schema, query, columns, computedProperties).Result;
        }

        public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns)
            => QueryFeatureSourceCore(true, resourceID, schema, filter, columns, null);

        public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, NameValueCollection aggregateFunctions)
            => QueryFeatureSourceCore(true, resourceID, schema, filter, null, aggregateFunctions);

        private IHttpRequest HttpRequest => this;

        private void ResourceCopyOrMove(string oldpath, string newpath, bool overwrite, Func<string, string, bool, string> reqBuilder, bool deleteFlag)
        {
            oldpath = FixAndValidateFolderPath(oldpath);
            newpath = FixAndValidateFolderPath(newpath);

            bool exists = ResourceExists(newpath);

            string req = reqBuilder(oldpath, newpath, overwrite);

            HttpRequest.SendRequestSync(req);

            if (deleteFlag)
                OnResourceDeleted(oldpath);
            if (exists)
                OnResourceUpdated(newpath);
            else
                OnResourceAdded(newpath);
        }

        public override void CopyFolder(string oldpath, string newpath, bool overwrite)
            => ResourceCopyOrMove(oldpath, newpath, overwrite, _reqBuilder.CopyResource, false);

        public override void CopyResource(string oldpath, string newpath, bool overwrite)
        {
            ResourceCopyOrMove(oldpath, newpath, overwrite, _reqBuilder.CopyResource, false);
            //HACK: the COPYRESOURCE call does not update timestamps of the target
            //if it already exists.
            Touch(newpath);
        }

        public override void DeleteResource(string resourceID)
        {
            var req = _reqBuilder.DeleteResource(resourceID);
            HttpRequest.SendRequestSync(req);
        }

        public void DeleteResourceData(string resourceID, string dataname)
        {
            var req = _reqBuilder.DeleteResourceData(resourceID, dataname);
            HttpRequest.SendRequestSync(req);
        }

        public Stream DescribeDrawing(string resourceID)
        {
            var req = _reqBuilder.DescribeDrawing(resourceID);
            return HttpRequest.OpenReadSync(req);
        }

        public override FeatureSchema DescribeFeatureSource(string resourceID, string schema)
        {
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            var req = _reqBuilder.DescribeSchema(resourceID, schema);
            using (var stream = HttpRequest.OpenReadSync(req))
            {
                var fsd = new FeatureSourceDescription(stream);
                return fsd.Schemas[0];
            }
        }

        public override FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string[] classNames)
        {
            ResourceIdentifier.Validate(resourceID, ResourceTypes.FeatureSource);
            var req = _reqBuilder.DescribeSchemaPartial(resourceID, schema, classNames);
            using (var stream = HttpRequest.OpenReadSync(req))
            {
                var fsd = new FeatureSourceDescription(stream);
                return fsd.Schemas[0];
            }
        }

        public override DataStoreList EnumerateDataStores(string providerName, string partialConnString)
        {
            var req = _reqBuilder.EnumerateDataStores(providerName, partialConnString);
            return OpenReadSync<DataStoreList>(req);
        }

        public string[] EnumerateDrawingLayers(string resourceID, string sectionName)
        {
            var req = _reqBuilder.EnumerateDrawingLayers(resourceID, sectionName);
            return OpenReadSync<ObjectModels.Common.StringCollection>(req)?.Item?.Distinct()?.ToArray() ?? new string[0];
        }

        public DrawingSectionResourceList EnumerateDrawingSectionResources(string resourceID, string sectionName)
        {
            var req = _reqBuilder.EnumerateDrawingSectionResources(resourceID, sectionName);
            return OpenReadSync<DrawingSectionResourceList>(req);
        }

        public DrawingSectionList EnumerateDrawingSections(string resourceID)
        {
            var req = _reqBuilder.EnumerateDrawingSections(resourceID);
            return OpenReadSync<DrawingSectionList>(req);
        }

        public override GroupList EnumerateGroups() => _userGroups.Value;

        public ResourceDataList EnumerateResourceData(string resourceID)
        {
            var req = _reqBuilder.EnumerateResourceData(resourceID);
            return OpenReadSync<ResourceDataList>(req);
        }

        public override ResourceReferenceList EnumerateResourceReferences(string resourceid)
        {
            var req = _reqBuilder.EnumerateResourceReferences(resourceid);
            return OpenReadSync<ResourceReferenceList>(req);
        }

        public override UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
        {
            var req = _reqBuilder.EnumerateUnmanagedData(startpath, filter, recursive, type);
            return OpenReadSync<UnmanagedDataList>(req);
        }

        public override UserList EnumerateUsers(string group)
        {
            var req = _reqBuilder.EnumerateUsers(group);
            return OpenReadSync<UserList>(req);
        }

        public IReader ExecuteSqlQuery(string featureSourceID, string sql)
        {
            throw new NotSupportedException();
        }

        public IApplicationDefinitionContainerInfoSet GetApplicationContainers()
        {
            var req = _reqBuilder.EnumerateApplicationContainers();
            return OpenReadSync<ApplicationDefinitionContainerInfoSet>(req);
        }

        public IApplicationDefinitionTemplateInfoSet GetApplicationTemplates()
        {
            var req = _reqBuilder.EnumerateApplicationTemplates();
            return OpenReadSync<ApplicationDefinitionTemplateInfoSet>(req);
        }

        public IApplicationDefinitionWidgetInfoSet GetApplicationWidgets()
        {
            var req = _reqBuilder.EnumerateApplicationWidgets();
            return OpenReadSync<ApplicationDefinitionWidgetInfoSet>(req);
        }

        public override string[] GetClassNames(string resourceId, string schemaName)
        {
            var req = _reqBuilder.GetClassNames(resourceId, schemaName);
            return OpenReadSync<ObjectModels.Common.StringCollection>(req)?.Item?.Distinct()?.ToArray() ?? new string[0];
        }

        public string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString)
        {
            var req = _reqBuilder.GetConnectionPropertyValues(providerName, propertyName, partialConnectionString);
            return OpenReadSync<ObjectModels.Common.StringCollection>(req)?.Item?.Distinct()?.ToArray() ?? new string[0];
        }

        public string UserAgent
        {
            get { return _reqBuilder?.UserAgent ?? string.Empty; }
            set
            {
                if (_reqBuilder != null)
                    _reqBuilder.UserAgent = value;
            }
        }

        public override object GetCustomProperty(string name)
        {
            switch (name)
            {
                case CustomProperties.UserAgent:
                    return this.UserAgent;
                case CustomProperties.BaseUrl:
                    return _reqBuilder.HostURI;
                default:
                    throw new CustomPropertyNotFoundException();
            }
        }

        private Lazy<string[]> _customPropertyNames = new Lazy<string[]>(() => new[] { CustomProperties.BaseUrl, CustomProperties.UserAgent });

        public override string[] GetCustomPropertyNames() => _customPropertyNames.Value;

        public override Type GetCustomPropertyType(string name)
        {
            switch (name)
            {
                case CustomProperties.UserAgent:
                    return typeof(string);
                case CustomProperties.BaseUrl:
                    return typeof(string);
                default:
                    throw new CustomPropertyNotFoundException();
            }
        }

        public Stream GetDrawing(string resourceID)
        {
            return HttpRequest.OpenReadSync(_reqBuilder.GetDrawing(resourceID));
        }

        public string GetDrawingCoordinateSpace(string resourceID)
        {
            using (var s = new StreamReader(HttpRequest.OpenReadSync(_reqBuilder.GetDrawingCoordinateSpace(resourceID))))
            {
                return s.ReadToEnd();
            }
        }

        public override object GetFolderOrResourceHeader(string resourceId)
        {
            if (ResourceIdentifier.IsFolderResource(resourceId))
                return this.OpenReadSync<ResourceFolderHeaderType>(_reqBuilder.GetResourceHeader(resourceId));
            else
                return this.OpenReadSync<ResourceDocumentHeaderType>(_reqBuilder.GetResourceHeader(resourceId));
        }

        public override string[] GetIdentityProperties(string resourceID, string classname)
        {
            string[] parts = classname.Split(':');
            string req;
            if (parts.Length == 2)
                req = _reqBuilder.GetIdentityProperties(resourceID, parts[0], parts[1]);
            else if (parts.Length == 1)
                req = _reqBuilder.GetIdentityProperties(resourceID, null, parts[0]);
            else
                throw new Exception($"Unable to parse classname into class and schema: {classname}");

            var doc = new XmlDocument();
            doc.Load(HttpRequest.OpenReadSync(req));
            var lst = doc.SelectNodes("/PropertyDefinitions/PropertyDefinition/Name");
            string[] ids = new string[lst.Count];
            for (int i = 0; i < lst.Count; i++)
                ids[i] = lst[i].InnerText;

            return ids;
        }

        public Stream GetLayer(string resourceID, string sectionName, string layerName)
        {
            var req = _reqBuilder.GetDrawingLayer(resourceID, sectionName, layerName);
            return HttpRequest.OpenReadSync(req);
        }

        public override Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format)
        {
            var req = _reqBuilder.GetLegendImage(scale, layerdefinition, themeIndex, type, width, height, format);
            return Image.FromStream(HttpRequest.OpenReadSync(req));
        }

        public override ILongTransactionList GetLongTransactions(string resourceId, bool activeOnly)
        {
            var req = _reqBuilder.GetLongTransactions(resourceId, activeOnly);
            return this.OpenReadSync<FdoLongTransactionList>(req);
        }

        public IFdoProviderCapabilities GetProviderCapabilities(string provider)
        {
            var req = _reqBuilder.GetProviderCapabilities(provider);
            return this.OpenReadSync<ObjectModels.Capabilities.v1_1_0.FdoProviderCapabilities>(req);
        }

        public override ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren)
        {
            var req = _reqBuilder.EnumerateResources(startingpoint, depth, type, computeChildren);
            return this.OpenReadSync<ResourceList>(req);
        }

        public override Stream GetResourceData(string resourceID, string dataname)
        {
            var req = _reqBuilder.GetResourceData(resourceID, dataname);
            return HttpRequest.OpenReadSync(req);
        }

        public override Stream GetResourceXmlData(string resourceID)
        {
            var req = _reqBuilder.GetResourceContent(resourceID);
            return HttpRequest.OpenReadSync(req);
        }

        public override ConfigurationDocument GetSchemaMapping(string provider, string partialConnString)
        {
            var req = _reqBuilder.GetSchemaMapping(provider, partialConnString);
            return ConfigurationDocument.Load(HttpRequest.OpenReadSync(req));
        }

        public override string[] GetSchemas(string resourceId)
        {
            var req = _reqBuilder.GetSchemas(resourceId);
            return this.OpenReadSync<ObjectModels.Common.StringCollection>(req)?.Item?.Distinct()?.ToArray() ?? new string[0];
        }

        public Stream GetSection(string resourceID, string sectionName)
        {
            IHttpRequest hr = this;
            var req = _reqBuilder.GetDrawingSection(resourceID, sectionName);
            return hr.OpenReadSync(req);
        }

        public Stream GetSectionResource(string resourceID, string resourceName)
        {
            IHttpRequest hr = this;
            var req = _reqBuilder.GetDrawingSectionResource(resourceID, resourceName);
            return hr.OpenReadSync(req);
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

        public override SiteInformation GetSiteInfo()
        {
            return this.OpenReadSync<SiteInformation>(_reqBuilder.GetSiteInfo());
        }

        public override FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly)
        {
            var req = _reqBuilder.GetSpatialContextInfo(resourceID, activeOnly);
            return this.OpenReadSync<FdoSpatialContextList>(req);
        }

        public override Stream GetTile(string mapdefinition, string baselayergroup, int col, int row, int scaleindex, string format)
        {
            IHttpRequest hr = this;
            var req = string.Empty;
            if (_isAnonymous)
                req = _reqBuilder.GetTileAnonymous(mapdefinition, baselayergroup, row, col, scaleindex, format);
            else
                req = _reqBuilder.GetTile(mapdefinition, baselayergroup, row, col, scaleindex, format, true);
            return hr.OpenReadSync(req);
        }

        public override bool IsSessionExpiredException(Exception ex)
            => (ex.Message.ToLower().IndexOf("session expired") >= 0 || ex.Message.ToLower().IndexOf("session not found") >= 0 || ex.Message.ToLower().IndexOf("mgsessionexpiredexception") >= 0);

        public override void MoveFolder(string oldpath, string newpath, bool overwrite)
            => ResourceCopyOrMove(oldpath, newpath, overwrite, _reqBuilder.MoveResource, true);

        public override void MoveResource(string oldpath, string newpath, bool overwrite)
            => ResourceCopyOrMove(oldpath, newpath, overwrite, _reqBuilder.MoveResource, true);

        public override IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames, NameValueCollection computedProperties)
            => (IFeatureReader)QueryFeatureSourceCore(false, resourceID, className, filter, propertyNames, computedProperties);

        public override string QueryMapFeatures(RuntimeMap rtMap, int maxFeatures, string wkt, bool persist, string selectionVariant, QueryMapOptions extraOptions)
        {
            return _reqBuilder.QueryMapFeaturesAsync(_httpClient, rtMap.Name, maxFeatures, wkt, persist, selectionVariant, extraOptions).Result;
        }

        public override Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, bool keepSelection)
        {
            return _reqBuilder.GetDynamicMapOverlayImageAsync(_httpClient, map.Name, (selection == null ? string.Empty : selection.ToXml()), format).Result;
        }

        public override Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, Color selectionColor, int behaviour)
        {
            return _reqBuilder.GetDynamicMapOverlayImageAsync(_httpClient, map.Name, (selection == null ? string.Empty : selection.ToXml()), format, selectionColor, behaviour).Result;
        }

        public Stream RenderMapLegend(RuntimeMap map, int width, int height, Color backgroundColor, string format)
        {
            IHttpRequest hr = this;
            var req = _reqBuilder.RenderMapLegend(map.Name, width, height, ColorTranslator.ToHtml(backgroundColor), format);
            return hr.OpenReadSync(req);
        }

        public override Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format, bool clip)
        {
            var req = _reqBuilder.GetMapImageUrl(map.Name, format, null, x, y, scale, dpi, width, height, clip, null, null, null, null);
            return HttpRequest.OpenReadSync(req);
        }

        public override Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip)
        {
            var req = _reqBuilder.GetMapImageUrl(map.Name, format, null, x1, y1, x2, y2, dpi, width, height, clip, null, null, null, null);
            return HttpRequest.OpenReadSync(req);
        }

        public override void SetCustomProperty(string name, object value)
        {
            if (name == CustomProperties.UserAgent)
                this.UserAgent = value.ToString();
            else
                throw new CustomPropertyNotFoundException();
        }

        public override void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, Stream stream, Utility.StreamCopyProgressDelegate callback)
        {
            _reqBuilder.SetResourceDataAsync(_httpClient, resourceid, dataname, datatype, stream, callback).Wait();
        }

        public override void SetResourceXmlData(string resourceId, Stream content, Stream header)
        {
            _reqBuilder.SetResourceAsync(_httpClient, resourceId, content, header).Wait();
        }

        public string TestConnection(string providername, NameValueCollection parameters)
        {
            IHttpRequest hr = this;
            var req = _reqBuilder.TestConnection(providername, parameters);
            byte[] x = hr.DownloadDataSync(req);
            return Encoding.UTF8.GetString(x.Where(b => b > 0).ToArray());
        }

        public override string TestConnection(string featuresource)
        {
            IHttpRequest hr = this;
            var req = _reqBuilder.TestConnection(featuresource);
            byte[] x = hr.DownloadDataSync(req);
            return Encoding.UTF8.GetString(x.Where(b => b > 0).ToArray());
        }

        public override void UpdateRepository(string resourceId, ResourceFolderHeaderType header)
        {
            _reqBuilder.UpdateRepositoryAsync(_httpClient, resourceId, this.SerializeObject(header)).Wait();
        }

        public override void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _reqBuilder.ApplyPackageAsync(_httpClient, fs, callback).Wait();
            }
        }

        protected override FeatureSourceDescription DescribeFeatureSourceInternal(string resourceId)
        {
            ResourceIdentifier.Validate(resourceId, ResourceTypes.FeatureSource);
            IHttpRequest hr = this;
            var req = _reqBuilder.DescribeSchema(resourceId);
            using (var stream = hr.OpenReadSync(req))
            {
                return new FeatureSourceDescription(stream);
            }
        }

        protected override ClassDefinition GetClassDefinitionInternal(string resourceId, string schemaName, string className)
        {
            ResourceIdentifier.Validate(resourceId, ResourceTypes.FeatureSource);
            IHttpRequest hr = this;
            var req = _reqBuilder.GetClassDefinition(resourceId, schemaName, className);
            using (var stream = hr.OpenReadSync(req))
            {
                var fsd = new FeatureSourceDescription(stream);
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

        public override bool ResourceExists(string resourceID)
        {
            try
            {
                var req = _reqBuilder.ResourceExists(resourceID);
                using (var s = HttpRequest.OpenReadSync(req))
                using (var sr = new StreamReader(s))
                    return sr.ReadToEnd().Trim().Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception ex)
            {
                try { return base.ResourceExists(resourceID); }
                catch { throw ex; } //Throw original error
            }
        }

        protected override IServerConnection GetInterface() => this;

        protected override bool RestartSessionInternal(bool throwException)
        {
            if (m_username == null || m_password == null)
            {
                if (throwException)
                    throw new Exception("Cannot recreate session, because connection was not opened with username and password"); //LOCALIZEME
                else
                    return false;
            }

            Uri hosturl = new Uri(_reqBuilder.HostURI);
            string locale = _reqBuilder.Locale;
            string oldSessionId = _reqBuilder.SessionID;

            RequestBuilder reqb = new RequestBuilder(hosturl, locale);
            var byteArray = Encoding.ASCII.GetBytes($"{m_username}:{m_password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var req = reqb.CreateSession();
            try
            {
                reqb.SessionID = Encoding.Default.GetString(HttpRequest.DownloadDataSync(req));
                if (reqb.SessionID.IndexOf("<") >= 0)
                    throw new Exception($"Invalid server token recieved: {reqb.SessionID}"); //LOCALIZEME
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
                    if (!hosturl.ToString().EndsWith("/mapagent/mapagent.fcgi")) //NOXLATE
                    {
                        string tmp = hosturl.ToString();
                        if (!tmp.EndsWith("/")) //NOXLATE
                            tmp += "/"; //NOXLATE
                        hosturl = new Uri($"{tmp}mapagent/mapagent.fcgi"); //NOXLATE
                        reqb = new RequestBuilder(hosturl, locale);
                        req = reqb.CreateSession();
                        reqb.SessionID = Encoding.Default.GetString(HttpRequest.DownloadDataSync(req));
                        if (reqb.SessionID.IndexOf("<") >= 0) //NOXLATE
                            throw new Exception($"Invalid server token recieved: {reqb.SessionID}"); //NOXLATE
                        ok = true;
                    }
                }
                catch { }

                if (!ok)
                {
                    if (throwException) //Report original error
                    {
                        if (ex is WebException) //These exceptions, we just want the underlying message. No need for 50 bajillion nested exceptions
                            throw;
                        else //We don't know what this could be so grab everything
                            throw new Exception($"Failed to connect, perhaps session is expired?\nExtended error info: {ex.Message}", ex); //LOCALIZEME
                    }
                    else
                        return false;
                }
                else
                {
                    CheckAndRaiseSessionChanged(oldSessionId, reqb.SessionID);
                }
            }

            var v = new Version(((SiteVersion)DeserializeObject(typeof(SiteVersion), HttpRequest.OpenReadSync(reqb.GetSiteVersion()))).Version);
            _properties[nameof(SiteVersion)] = v;
            _reqBuilder = reqb;
            return true;
        }

        private async Task<byte[]> DownloadDataAsync(string req)
        {
            var result = await _httpClient.GetAsync(req).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        private async Task<Stream> OpenReadAsync(string req)
        {
            var result = await _httpClient.GetAsync(req).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        private async Task OpenReadVoidAsync(string req)
        {
            var result = await _httpClient.GetAsync(req).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using (stream)
            {
                stream.ReadByte();
            }
        }

        byte[] IHttpRequest.DownloadDataSync(string req)
        {
            return this.DownloadDataAsync(req).Result;
        }

        Stream IHttpRequest.OpenReadSync(string req)
        {
            return this.OpenReadAsync(req).Result;
        }

        void IHttpRequest.SendRequestSync(string req)
        {
            this.OpenReadVoidAsync(req).Wait();
        }

        private T OpenReadSync<T>(string req)
        {
            IHttpRequest hr = this;
            using (var stream = hr.OpenReadSync(req))
            {
                return this.DeserializeObject<T>(stream);
            }
        }
    }
}
