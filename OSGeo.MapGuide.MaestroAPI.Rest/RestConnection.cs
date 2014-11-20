#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.ObjectModels.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Rest
{
    /// <summary>
    /// RestConnection providers a IServerConnection interface to the mapguide-rest REST API
    /// </summary>
    public class RestConnection : MgServerConnectionBase,
                                  IServerConnection,
                                  IFeatureService,
                                  IResourceService
    {
        public const string PARAM_URL = "Url";
        public const string PARAM_USERNAME = "Username";
        public const string PARAM_PASSWORD = "Password";

        private string _restRootUrl;

        internal RestClient MakeClient()
        {
            return new RestClient(_restRootUrl);
        }

        private RestRequest MakeRequest(string part, Method method = Method.GET)
        {
            var req = new RestRequest(part, method);
            req.AddParameter("session", this.SessionID);
            return req;
        }

        private void LogResponse(IRestResponse resp)
        {
            OnRequestDispatched(string.Format("{0} {1} - {2}", resp.Request.Method, resp.Request.Resource, resp.StatusCode));
        }

        private IRestResponse ExecuteRequest(RestClient client, IRestRequest req)
        {
            var resp = client.Execute(req);
            LogResponse(resp);
            return resp;
        }

        private IRestResponse<T> ExecuteRequest<T>(RestClient client, IRestRequest req) where T : class, new()
        {
            var resp = client.Execute<T>(req);
            LogResponse(resp);
            return resp;
        }

        internal RestConnection()
        {

        }

        //This is the constructor used by ConnectionProviderRegistry.CreateConnection

        internal RestConnection(NameValueCollection initParams)
            : this()
        {
            if (initParams[PARAM_URL] == null)
                throw new ArgumentException("Missing required connection parameter: " + PARAM_URL);

            string pwd = initParams[PARAM_PASSWORD] ?? string.Empty;
            if (initParams[PARAM_USERNAME] == null)
                throw new ArgumentException("Missing required connection parameter: " + PARAM_USERNAME);

            _restRootUrl = initParams[PARAM_URL];
            m_username = initParams[PARAM_USERNAME];
            m_password = pwd;

            var client = MakeClient();
            client.Authenticator = new HttpBasicAuthenticator(m_username, m_password);

            var req = new RestRequest("session", Method.POST);
            var resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
            _sessionId = resp.Content;
            CheckAndRaiseSessionChanged(null, _sessionId);

            lock (SyncRoot)
            {
                req = MakeRequest("site/version", Method.GET);
                req.AddParameter("session", _sessionId);

                resp = ExecuteRequest(client, req);

                _siteVersion = new Version(resp.Content);
                _providers = null;
            }
        }

        public override string ProviderName
        {
            get { return "Maestro.Rest"; }
        }

        public override System.Collections.Specialized.NameValueCollection CloneParameters
        {
            get
            {
                var nvc = new NameValueCollection();
                nvc[PARAM_URL] = _restRootUrl;
                nvc[CommandLineArguments.Provider] = this.ProviderName;
                nvc[CommandLineArguments.Session] = this.SessionID;
                return nvc;
            }
        }

        private string _sessionId;

        public override string SessionID
        {
            get
            {
                return _sessionId;
            }
        }

        protected override IServerConnection GetInterface()
        {
            return this;
        }

        public override void Dispose()
        {
            
        }

        public override IServerConnection Clone()
        {
            return new RestConnection();
        }

        internal void ValidateResponse(IRestResponse resp)
        {
            int status = (int)resp.StatusCode;
            //Throw if status code is not in the 200-series
            if (status < 200 || status > 299)
                throw new RestServiceException(resp.Content, resp.ContentType);
        }

        private System.IO.Stream FetchRuntimeMapRepresentationAsStream(Mapping.RuntimeMap rtMap, string representationSuffix)
        {
            return FetchRuntimeMapRepresentationAsStream(rtMap, representationSuffix, null);
        }

        private System.IO.Stream FetchRuntimeMapRepresentationAsStream(Mapping.RuntimeMap rtMap, string representationSuffix, Action<IRestRequest> applyParametersCallback)
        {
            var client = MakeClient();
            var req = MakeRequest("session/{sessionId}/{mapName}.Map" + representationSuffix, Method.GET);
            req.AddUrlSegment("sessionId", rtMap.SessionId);
            req.AddUrlSegment("mapName", rtMap.Name);
            if (applyParametersCallback != null)
                applyParametersCallback(req);

            //NOTE: RestSharp either doesn't support streamed responses or they do and it is not compatible with our current API signatures
            return new MemoryStream(client.DownloadData(req));
        }

        private T FetchRuntimeMapRepresentation<T>(Mapping.RuntimeMap rtMap, string representationSuffix) where T : class, new()
        {
            return FetchRuntimeMapRepresentation<T>(rtMap, representationSuffix, null);
        }

        private T FetchRuntimeMapRepresentation<T>(Mapping.RuntimeMap rtMap, string representationSuffix, Action<IRestRequest> applyParametersCallback) where T : class, new()
        {
            var client = MakeClient();
            var req = MakeRequest("session/{sessionId}/{mapName}.Map" + representationSuffix, Method.GET);
            req.AddUrlSegment("sessionId", rtMap.SessionId);
            req.AddUrlSegment("mapName", rtMap.Name);
            if (applyParametersCallback != null)
                applyParametersCallback(req);

            IRestResponse<T> resp = ExecuteRequest<T>(client, req);
            ValidateResponse(resp);
            return GetData(resp);
        }

        private string FetchResourceRepresentationAsString(string resourceid, string representationSuffix)
        {
            return FetchResourceRepresentationAsString(resourceid, representationSuffix, null);
        }

        private string FetchResourceRepresentationAsString(string resourceid, string representationSuffix, Action<IRestRequest> applyParametersCallback)
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceid);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddParameter("session", this.SessionID);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }

            //NOTE: RestSharp either doesn't support streamed responses or they do and it is not compatible with our current API signatures
            var resp = ExecuteRequest(client, req);
            return resp.Content;
        }

        private System.IO.Stream FetchResourceRepresentationAsStream(string resourceid, string representationSuffix)
        {
            return FetchResourceRepresentationAsStream(resourceid, representationSuffix, null);
        }

        private System.IO.Stream FetchResourceRepresentationAsStream(string resourceid, string representationSuffix, Action<IRestRequest> applyParametersCallback)
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceid);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddParameter("session", this.SessionID);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }

            //NOTE: RestSharp either doesn't support streamed responses or they do and it is not compatible with our current API signatures
            return new MemoryStream(client.DownloadData(req));
        }

        private T FetchResourceRepresentation<T>(string resourceid, string representationSuffix) where T : class, new()
        {
            return FetchResourceRepresentation<T>(resourceid, representationSuffix, null);
        }

        private T FetchResourceRepresentation<T>(string resourceid, string representationSuffix, Action<IRestRequest> applyParametersCallback) where T : class, new()
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceid);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddParameter("session", this.SessionID);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}" + representationSuffix, Method.GET);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                if (applyParametersCallback != null)
                    applyParametersCallback(req);
            }

            IRestResponse<T> resp = ExecuteRequest<T>(client, req);
            ValidateResponse(resp);
            return GetData(resp);
        }

        public override System.IO.Stream GetResourceXmlData(string resourceID)
        {
            return FetchResourceRepresentationAsStream(resourceID, "/content.xml");
        }

        public override void DeleteResource(string resourceid)
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceid);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}", Method.DELETE);
                req.AddParameter("session", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}", Method.DELETE);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
            }

            IRestResponse resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        private T GetData<T>(IRestResponse<T> resp)
        {
            //HACK: It seems we can't trust RestSharp to properly deserialize the XML content
            //(ie. The .Data property will not be what we expect) so delegate to our existing deserializer
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(resp.Content)))
            {
                return DeserializeObject<T>(ms);
            }
        }

        public override ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren)
        {
            Check.IsFolderArgument(startingpoint, "startingpoint");
            ResourceIdentifier ri = new ResourceIdentifier(startingpoint);
            if (ri.IsInSessionRepository)
            {
                //Can't enumerate session folders
                throw new NotSupportedException();
            }
            else
            {
                if (startingpoint == "Library://")
                {
                    var req = MakeRequest("library/list.xml", Method.GET);
                    req.AddParameter("session", this.SessionID);
                    req.AddParameter("type", type);
                    req.AddParameter("depth", depth);
                    req.AddParameter("computechildren", computeChildren ? 1 : 0);

                    var client = MakeClient();
                    IRestResponse<ResourceList> resp = ExecuteRequest<ResourceList>(client, req);
                    ValidateResponse(resp);
                    return GetData(resp);
                }
                else
                {
                    return FetchResourceRepresentation<ResourceList>(startingpoint, "/list.xml", (req) =>
                    {
                        req.AddParameter("type", type);
                        req.AddParameter("depth", depth);
                        req.AddParameter("computechildren", computeChildren ? 1 : 0);
                    });
                }
            }
        }

        public override ResourceReferenceList EnumerateResourceReferences(string resourceid)
        {
            return FetchResourceRepresentation<ResourceReferenceList>(resourceid, "/references.xml");
        }

        public override void CopyResource(string oldpath, string newpath, bool overwrite)
        {
            var client = MakeClient();
            var req = MakeRequest("services/copyresource", Method.POST);
            req.AddParameter("source", oldpath);
            req.AddParameter("destination", newpath);
            req.AddParameter("overwrite", overwrite ? 1 : 0);
            var resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        public override void CopyFolder(string oldpath, string newpath, bool overwrite)
        {
            Check.IsFolderArgument(oldpath, "oldpath");
            Check.IsFolderArgument(newpath, "newpath");
            CopyResource(oldpath, newpath, overwrite);
        }

        public override void MoveResource(string oldpath, string newpath, bool overwrite)
        {
            var client = MakeClient();
            var req = MakeRequest("services/moveresource", Method.POST);
            req.AddParameter("source", oldpath);
            req.AddParameter("destination", newpath);
            req.AddParameter("overwrite", overwrite ? 1 : 0);
            req.AddParameter("cascade", 1);
            var resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        public override void MoveFolder(string oldpath, string newpath, bool overwrite)
        {
            Check.IsFolderArgument(oldpath, "oldpath");
            Check.IsFolderArgument(newpath, "newpath");
            MoveResource(oldpath, newpath, overwrite);
        }

        public override System.IO.Stream GetResourceData(string resourceID, string dataname)
        {
            return FetchResourceRepresentationAsStream(resourceID, "/data/{dataName}", (req) =>
            {
                req.AddUrlSegment("dataName", dataname);
            });
        }

        public override void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback)
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceid);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}/data/{dataName}", Method.DELETE);
                req.AddParameter("session", this.SessionID);
                req.AddParameter("datatype", datatype.ToString());
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddUrlSegment("dataName", dataname);
                req.AddFile("file", (os) => {
                    Utility.CopyStream(stream, os, callback, 1024);
                }, dataname);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}", Method.DELETE);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddParameter("datatype", datatype.ToString());
                req.AddFile("file", (os) =>
                {
                    Utility.CopyStream(stream, os, callback, 1024);
                }, dataname);
            }

            IRestResponse resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        public override void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
        {
            var client = MakeClient();
            var req = MakeRequest("library");
            req.AddFile("package", (os) => {
                using (var fr = File.OpenRead(filename))
                {
                    Utility.CopyStream(fr, os, callback, 1024);
                }
            }, filename);
            IRestResponse resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        public override void UpdateRepository(string resourceId, ResourceFolderHeaderType header)
        {
            throw new NotImplementedException();
        }

        public override object GetFolderOrResourceHeader(string resourceId)
        {
            if (ResourceIdentifier.IsFolderResource(resourceId))
                return FetchResourceRepresentation<ResourceFolderHeaderType>(resourceId, "/header.xml");
            else
                return FetchResourceRepresentation<ResourceDocumentHeaderType>(resourceId, "/header.xml");
        }

        public override void SetResourceXmlData(string resourceId, System.IO.Stream content, System.IO.Stream header)
        {
            var client = MakeClient();
            var ri = new ResourceIdentifier(resourceId);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}/contentorheader", Method.POST);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddParameter("session", this.SessionID);
                if (content != null)
                {
                    req.AddFile("content", (os) =>
                    {
                        content.CopyTo(os);
                    }, "content");
                }
                if (header != null)
                {
                    req.AddFile("header", (os) =>
                    {
                        header.CopyTo(os);
                    }, "header");
                }
            }
            else //Session-based
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}/contentorheader", Method.POST);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddUrlSegment("session", this.SessionID);
                using (var sr = new StreamReader(content))
                {
                    req.AddParameter("text/xml", sr.ReadToEnd(), ParameterType.RequestBody);
                }
                if (content != null)
                {
                    req.AddFile("content", (os) =>
                    {
                        content.CopyTo(os);
                    }, "content");
                }
                if (header != null)
                {
                    req.AddFile("header", (os) =>
                    {
                        header.CopyTo(os);
                    }, "header");
                }
            }
        }

        public override UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
        {
            var client = MakeClient();
            RestRequest req = MakeRequest("services/listunmanageddata.xml", Method.POST);
            req.AddParameter("path", startpath);
            req.AddParameter("type", type);
            req.AddParameter("filter", filter);
            req.AddParameter("recursive", recursive ? 1 : 0);

            IRestResponse<UnmanagedDataList> resp = ExecuteRequest<UnmanagedDataList>(client, req);
            ValidateResponse(resp);
            return GetData(resp);
        }

        public void DeleteResourceData(string resourceID, string dataname)
        {
            var client = MakeClient();
            ResourceIdentifier ri = new ResourceIdentifier(resourceID);
            RestRequest req = null;
            if (ri.IsInLibrary)
            {
                req = MakeRequest("library/{resourcePath}/data/{dataName}", Method.DELETE);
                req.AddParameter("session", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddUrlSegment("dataName", dataname);
            }
            else //session
            {
                req = MakeRequest("session/{sessionId}/{resourcePath}/data/{dataName}", Method.DELETE);
                req.AddUrlSegment("sessionId", this.SessionID);
                req.AddUrlSegment("resourcePath", ri.Path + "." + ri.ResourceType);
                req.AddUrlSegment("dataName", dataname);
            }

            IRestResponse resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
        }

        public ResourceDataList EnumerateResourceData(string resourceID)
        {
            return FetchResourceRepresentation<ResourceDataList>(resourceID, "/datalist.xml");
        }

        public override string TestConnection(string featuresource)
        {
            using (var sr = new StreamReader(FetchResourceRepresentationAsStream(featuresource, "/status")))
            {
                return sr.ReadToEnd();
            }
        }

        private FeatureProviderRegistryFeatureProvider[] _providers;

        public override FeatureProviderRegistryFeatureProvider[] FeatureProviders
        {
            get
            {
                lock(SyncRoot)
                {
                    if (_providers != null)
                    {
                        var client = MakeClient();
                        var req = MakeRequest("providers.xml", Method.GET);
                        var resp = ExecuteRequest<FeatureProviderRegistry>(client, req);
                        _providers = resp.Data.FeatureProvider.ToArray();
                    }
                }
                return _providers;
            }
        }

        public override FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly /* unused */)
        {
            return FetchResourceRepresentation<FdoSpatialContextList>(resourceID, "/spatialcontexts.xml");
        }

        public override string[] GetIdentityProperties(string resourceID, string qualifiedClassName)
        {
            string schemaName = null;
            string className = qualifiedClassName;
            if (qualifiedClassName.Contains(":"))
            {
                string [] tokens = qualifiedClassName.Split(':');
                if (tokens.Length == 2)
                {
                    schemaName = tokens[0];
                    className = tokens[1];
                }
            }
            var clsDef = GetClassDefinitionInternal(resourceID, schemaName, className);
            return clsDef.IdentityProperties.Select(x => x.Name).ToArray();
        }

        public override Schema.FeatureSchema DescribeFeatureSource(string resourceID, string schema)
        {
            using (var xml = FetchResourceRepresentationAsStream(resourceID, "/schema.xml/{schemaName}", (req) =>
            {
                req.AddUrlSegment("schemaName", schema);
            }))
            {
                var fsd = new FeatureSourceDescription(xml);
                return fsd.GetSchema(schema);
            }
        }

        public override Schema.FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string[] classNames)
        {
            using (var xml = FetchResourceRepresentationAsStream(resourceID, "/schema.xml/{schemaName}", (req) =>
            {
                req.AddUrlSegment("schemaName", schema);
                req.AddParameter("classnames", string.Join(",", classNames));
            }))
            {
                var fsd = new FeatureSourceDescription(xml);
                return fsd.GetSchema(schema);
            }
        }

        protected override Schema.FeatureSourceDescription DescribeFeatureSourceInternal(string resourceId)
        {
            var schemas = FetchResourceRepresentation<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(resourceId, "schemas.xml");
            if (schemas.Item.Count > 0)
            {
                using (var xml = FetchResourceRepresentationAsStream(resourceId, "/schema.xml/{schemaName}", (req) => 
                {
                    req.AddUrlSegment("schemaName", schemas.Item[0]);
                }))
                {
                    return new FeatureSourceDescription(xml);
                }
            }
            return null;
        }

        protected override Schema.ClassDefinition GetClassDefinitionInternal(string resourceId, string schemaName, string className)
        {
            using (var xml = FetchResourceRepresentationAsStream(resourceId, "/classdef.xml/{schemaName}/{className}"))
            {
                var fsd = new FeatureSourceDescription(xml);
                //We can't just assume first class item is the one, as ones that do not take
                //class name hints will return the full schema
                var schema = fsd.GetSchema(schemaName);
                if (schema != null)
                {
                    if (schema.Classes.Count > 1)
                    {
                        ClassDefinition ret = null;
                        foreach (var cls in schema.Classes)
                        {
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

        public override Feature.IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns)
        {
            throw new NotImplementedException();
        }

        public override Feature.IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, System.Collections.Specialized.NameValueCollection aggregateFunctions)
        {
            throw new NotImplementedException();
        }

        public override DataStoreList EnumerateDataStores(string providerName, string partialConnString)
        {
            var client = MakeClient();
            var req = MakeRequest("services/enumeratedatastores", Method.POST);
            req.AddParameter("provider", providerName);
            req.AddParameter("connection", partialConnString);
            var resp = ExecuteRequest<DataStoreList>(client, req);
            return GetData(resp);
        }

        public override string[] GetSchemas(string resourceId)
        {
            var resp = FetchResourceRepresentation<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(resourceId, "/schemas.xml");
            if (resp.Item != null)
                return resp.Item.ToArray();
            else
                return new string[0];
        }

        public override string[] GetClassNames(string resourceId, string schemaName)
        {
            var resp = FetchResourceRepresentation<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(resourceId, "/classes.xml/{schemaName}", (req) =>
            {
                req.AddParameter("schemaName", schemaName);
            });
            if (resp.Item != null)
                return resp.Item.ToArray();
            else
                return new string[0];
        }

        public override ILongTransactionList GetLongTransactions(string resourceId, bool activeOnly)
        {
            return FetchResourceRepresentation<FdoLongTransactionList>(resourceId, "/longtransactions", (req) =>
            {
                req.AddParameter("active", activeOnly ? 1 : 0);
            });
        }

        public override SchemaOverrides.ConfigurationDocument GetSchemaMapping(string provider, string partialConnString)
        {
            var client = MakeClient();
            var req = MakeRequest("services/getschemamapping.xml", Method.GET);
            req.AddParameter("provider", provider);
            req.AddParameter("connection", partialConnString);
            var resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
            return SchemaOverrides.ConfigurationDocument.LoadXml(resp.Content);
        }

        public override Feature.IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames, System.Collections.Specialized.NameValueCollection computedProperties)
        {
            throw new NotImplementedException();
        }

        public IFdoProviderCapabilities GetProviderCapabilities(string provider)
        {
            var client = MakeClient();
            var req = MakeRequest("providers/{provider}/capabilities.xml", Method.GET);
            req.AddUrlSegment("provider", provider);
            var resp = ExecuteRequest<OSGeo.MapGuide.ObjectModels.Capabilities_1_1_0.FdoProviderCapabilities>(client, req);
            ValidateResponse(resp);
            return GetData(resp);
        }

        public string TestConnection(string providerName, NameValueCollection parameters)
        {
            var client = MakeClient();
            var req = MakeRequest("services/testconnection/{providerName}", Method.POST);
            req.AddUrlSegment("providerName", providerName);
            foreach (string key in parameters.Keys)
            {
                req.AddParameter(key, parameters[key]);
            }
            var resp = ExecuteRequest(client, req);
            ValidateResponse(resp);
            return resp.Content;
        }

        public string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString)
        {
            var client = MakeClient();
            var req = MakeRequest("providers/{provider}/connectvalues.xml/{property}", Method.GET);
            req.AddUrlSegment("provider", providerName);
            req.AddUrlSegment("property", propertyName);

            req.AddParameter("session", this.SessionID);
            req.AddParameter("connection", partialConnectionString);
            var resp = ExecuteRequest<OSGeo.MapGuide.ObjectModels.Common.StringCollection>(client, req);
            ValidateResponse(resp);
            return GetData(resp).Item.ToArray();
        }

        public Feature.IReader ExecuteSqlQuery(string featureSourceID, string sql)
        {
            throw new NotSupportedException();
        }

        private Version _siteVersion;

        public override Version SiteVersion
        {
            get
            {
                return _siteVersion;
            }
        }

        public override string[] GetCustomPropertyNames()
        {
            return new string[0];
        }

        public override Type GetCustomPropertyType(string name)
        {
            throw new NotImplementedException();
        }

        public override void SetCustomProperty(string name, object value)
        {
            
        }

        public override object GetCustomProperty(string name)
        {
            throw new NotImplementedException();
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
            get { return new RestCapabilities(this); }
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

        private readonly object SyncRoot = new object();

        protected override bool RestartSessionInternal(bool throwException)
        {
            if (m_username == null || m_password == null)
                if (throwException)
                    throw new Exception("Cannot recreate session, because connection was not opened with username and password");
                else
                    return false;

            string oldSessionId = _sessionId;

            try
            {
                var client = MakeClient();
                client.Authenticator = new HttpBasicAuthenticator(m_username, m_password);

                var req = MakeRequest("session", Method.POST);
                var resp = ExecuteRequest(client, req);
                ValidateResponse(resp);
                _sessionId = resp.Content;
                CheckAndRaiseSessionChanged(oldSessionId, _sessionId);

                //Reset cached items
                lock (SyncRoot)
                {
                    req = MakeRequest("site/version", Method.GET);
                    req.AddParameter("session", _sessionId);

                    resp = ExecuteRequest(client, req);

                    _siteVersion = new Version(resp.Content);
                    _providers = null;
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

        public override SiteInformation GetSiteInfo()
        {
            var client = MakeClient();
            var req = MakeRequest("site/info.xml", Method.GET);
            req.AddParameter("session", this.SessionID);
            var resp = ExecuteRequest<SiteInformation>(client, req);
            return GetData(resp);
        }

        public override UserList EnumerateUsers(string group)
        {
            var client = MakeClient();
            var req = MakeRequest("site/groups/{groupName}/users.xml", Method.GET);
            req.AddUrlSegment("groupName", group);
            req.AddParameter("session", this.SessionID);
            var resp = ExecuteRequest<UserList>(client, req);
            return GetData(resp);
        }

        public override GroupList EnumerateGroups()
        {
            var client = MakeClient();
            var req = MakeRequest("site/groups.xml", Method.GET);
            req.AddParameter("session", this.SessionID);
            var resp = ExecuteRequest<GroupList>(client, req);
            return GetData(resp);
        }

        public override string QueryMapFeatures(Mapping.RuntimeMap rtMap, int maxFeatures, string wkt, bool persist, string selectionVariant, QueryMapOptions extraOptions)
        {
            var client = MakeClient();
            var req = MakeRequest("session/{sessionId}/{mapName}.Selection", Method.PUT);
            req.AddUrlSegment("sessionId", this.SessionID);
            req.AddUrlSegment("mapName", rtMap.Name);

            req.AddParameter("maxfeatures", maxFeatures);
            req.AddParameter("geometry", wkt);
            req.AddParameter("persist", persist ? 1 : 0);
            req.AddParameter("selectionvariant", selectionVariant);

            //The Maestro API signature is spec'd for the pre-2.6 QUERYMAPFEATURES
            req.AddParameter("requestdata", 4 | 8);

            if (extraOptions != null)
            {
                req.AddParameter("featurefilter", extraOptions.FeatureFilter);
                req.AddParameter("layerattributefilter", (int)extraOptions.LayerAttributeFilter);
                req.AddParameter("layernames", extraOptions.LayerNames);
            }

            var resp = ExecuteRequest(client, req);
            return resp.Content;
        }

        public override System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format)
        {
            return System.Drawing.Image.FromStream(FetchResourceRepresentationAsStream(layerdefinition, "/legend/{scale}/{geomType}/{themecat}/icon.{type}", (req) =>
            {
                req.AddUrlSegment("scale", scale.ToString(CultureInfo.InvariantCulture));
                req.AddUrlSegment("geomType", type.ToString(CultureInfo.InvariantCulture));
                req.AddUrlSegment("themecat", themeIndex.ToString(CultureInfo.InvariantCulture));
                req.AddUrlSegment("type", format.ToLower());

                req.AddParameter("width", width);
                req.AddParameter("height", height);
            }));
        }

        public override Stream RenderRuntimeMap(Mapping.RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format, bool clip)
        {
            return FetchRuntimeMapRepresentationAsStream(map, "/image.{type}", (req) =>
            {
                req.AddUrlSegment("type", format.ToLower());

                req.AddParameter("x", x);
                req.AddParameter("y", y);
                req.AddParameter("scale", scale);
                req.AddParameter("width", width);
                req.AddParameter("height", height);
                req.AddParameter("dpi", dpi);
                req.AddParameter("clip", clip ? 1 : 0);
            });
        }

        public override Stream RenderRuntimeMap(Mapping.RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip)
        {
            double x, y, scale;
            map.ComputeCenterAndScale(x1, y1, x2, y2, width, height, out x, out y, out scale);

            return FetchRuntimeMapRepresentationAsStream(map, "/image.{type}", (req) =>
            {
                req.AddUrlSegment("type", format.ToLower());
                req.AddParameter("x", x);
                req.AddParameter("y", y);
                req.AddParameter("scale", scale);
                req.AddParameter("width", width);
                req.AddParameter("height", height);
                req.AddParameter("dpi", dpi);
                req.AddParameter("clip", clip ? 1 : 0);
            });
        }

        public override Stream RenderDynamicOverlay(Mapping.RuntimeMap map, Mapping.MapSelection selection, string format, bool keepSelection)
        {
            return FetchRuntimeMapRepresentationAsStream(map, "/overlayimage.{type}", (req) =>
            {
                req.AddUrlSegment("type", format.ToLower());

                req.AddParameter("keepselection", keepSelection ? 1 : 0);
            });
        }

        public override Stream RenderDynamicOverlay(Mapping.RuntimeMap map, Mapping.MapSelection selection, string format, System.Drawing.Color selectionColor, int behaviour)
        {
            return FetchRuntimeMapRepresentationAsStream(map, "/overlayimage.{type}", (req) =>
            {
                req.AddUrlSegment("type", format.ToLower());

                req.AddParameter("selectioncolor", Utility.SerializeHTMLColorRGBA(selectionColor, true));
                req.AddParameter("behavior", behaviour);
            });
        }

        public override Stream GetTile(string mapdefinition, string baselayergroup, int col, int row, int scaleindex, string format)
        {
            return FetchResourceRepresentationAsStream(mapdefinition, "/tile.{type}/{groupName}/{scale}/{col}/{row}", (req) =>
            {
                req.AddUrlSegment("type", format.ToLower());
                req.AddUrlSegment("groupName", baselayergroup);
                req.AddUrlSegment("scale", scaleindex.ToString(CultureInfo.InvariantCulture));
                req.AddUrlSegment("col", col.ToString(CultureInfo.InvariantCulture));
                req.AddUrlSegment("row", row.ToString(CultureInfo.InvariantCulture));
            });
        }

        public IService GetService(int serviceType)
        {
            ServiceType st = (ServiceType)serviceType;
            switch (st)
            {
                case ServiceType.Feature:
                    return this;
                case ServiceType.Resource:
                    return this;
                default:
                    throw new ArgumentException(string.Format(Strings.InvalidOrUnsupportedServiceType, serviceType), "serviceType");
            }
        }

        public CoordinateSystem.ICoordinateSystemCatalog CoordinateSystemCatalog
        {
            get { return new RestCoordinateSystemCatalog(this); }
        }

        public string DisplayName
        {
            get
            {
                return _restRootUrl + " (v" + this.SiteVersion.ToString() + ")";
            }
        }
    }
}
