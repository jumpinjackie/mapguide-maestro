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
using System.Collections.Specialized;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Globalization;
using System.Drawing;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Collection class for building web requests to the MapGuide Server
    /// </summary>
    internal class RequestBuilder
    {
        private string m_userAgent = "MapGuide Maestro API";
        private string m_hosturi;
        private string m_sessionID = null;
        private string m_locale = null;

        internal RequestBuilder(Uri hosturi, string locale, string sessionid, bool bIncludeSessionIdInRequests)
        {
            m_hosturi = hosturi.AbsoluteUri;
            m_locale = locale;
            m_sessionID = sessionid;
            m_userAgent += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.IncludeSessionIdInRequestParams = bIncludeSessionIdInRequests;
        }

        internal RequestBuilder(Uri hosturi, string locale)
            : this (hosturi, locale, null, false)
        {
        }

        /// <summary>
        /// Indicates if the session id should be included as the SESSION request parameter. This needs to be set to true if
        /// the HTTP connection was initialized with only a session id, otherwise it can be set to false if the connection
        /// was initialized with a username/password as requests use the already established authenticated credentials.
        /// </summary>
        internal bool IncludeSessionIdInRequestParams { get; private set; }

        internal string Locale { get { return m_locale; } }

        internal string CreateSession()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CREATESESSION");
            param.Add("VERSION", "1.0.0");
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string GetSiteVersion()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETSITEVERSION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string GetFeatureProviders()
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETFEATUREPROVIDERS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            return m_hosturi + "?" + EncodeParameters(param);

        }

        internal string EnumerateResources(string startingpoint, int depth, string type, bool computeChildren)
        {
            if (type == null)
                type = "";

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATERESOURCES");
            param.Add("VERSION", "1.0.0");
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            param.Add("RESOURCEID", startingpoint);
            param.Add("DEPTH", depth.ToString());
            param.Add("TYPE", type);
            param.Add("COMPUTECHILDREN", computeChildren ? "1" : "0");
            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string TestConnection(string featuresource)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "TESTCONNECTION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            param.Add("RESOURCEID", featuresource);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal System.Net.WebRequest TestConnectionPost(string providername, NameValueCollection parameters, System.IO.Stream outStream)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "TESTCONNECTION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("PROVIDER", providername);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach(string k in parameters)
            {
                sb.Append(k);
                sb.Append("=");
                sb.Append(parameters[k]);
                sb.Append(";");
            }
            if (sb.Length != 0)
                sb.Length = sb.Length - 1;

            //TODO: Figure out how to encode the '%' character...
            param.Add("CONNECTIONSTRING", sb.ToString());

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;
            return req;
        }


        internal string TestConnection(string providername, NameValueCollection parameters)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "TESTCONNECTION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("PROVIDER", providername);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach(string k in parameters)
            {
                sb.Append(k);
                sb.Append("=");
                sb.Append(parameters[k]);
                sb.Append(";");
            }
            if (sb.Length != 0)
                sb.Length = sb.Length - 1;

            //TODO: Figure out how to encode the '%' character...
            param.Add("CONNECTIONSTRING", sb.ToString());

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string SessionID
        {
            get { return m_sessionID; }
            set { m_sessionID = value; }
        }

        private string EncodeParameters(NameValueCollection param, bool bAddSessionId)
        {
            if (bAddSessionId && this.IncludeSessionIdInRequestParams && param["SESSION"] == null)
                param["SESSION"] = m_sessionID;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in param.Keys)
            {
                sb.Append(EncodeParameter(s, param[s]));
                sb.Append("&");
            }

            return sb.ToString(0, sb.Length - 1);
        }

        private string EncodeParameters(NameValueCollection param)
        {
            return EncodeParameters(param, true);
        }

        private string EncodeParameter(string name, string value)
        {
            return System.Web.HttpUtility.UrlEncode(name) + "=" + System.Web.HttpUtility.UrlEncode(value);
        }


        public string GetMapDWF(string id)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAP");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("DWFVERSION", "6.01");
            param.Add("EMAPVERSION", "1.0");
            param.Add("MAPDEFINITION", id);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetResourceContent(string id)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETRESOURCECONTENT");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetResourceData(string id, string name)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETRESOURCEDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);
            param.Add("DATANAME", name);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateResourceData(string id)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATERESOURCEDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string DeleteResourceData(string id, string name)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DELETERESOURCEDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);
            param.Add("DATANAME", name);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string GetResourceHeader(string id)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETRESOURCEHEADER");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string SetResource(string id)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "SETRESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        private void EncodeFormParameters(string boundary, NameValueCollection param, System.IO.Stream responseStream)
        {
            foreach(string s in param.Keys)
            {
                string val = param[s];
                if (string.IsNullOrEmpty(val))
                    continue;

                System.IO.MemoryStream content = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(val));
                AppendFormContent(s, null, boundary, responseStream, content, null);
            }
        }

        /// <summary>
        /// Writes a value/file to a form-multipart HttpRequest stream
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="filename">The name of the file uploaded, set to null if this parameter is not a file</param>
        /// <param name="boundary">The request boundary string</param>
        /// <param name="responseStream">The stream to write to</param>
        /// <param name="dataStream">The stream to read from. When using non-file parameters, use UTF8 encoding</param>
        private void AppendFormContent(string name, string filename, string boundary, System.IO.Stream responseStream, System.IO.Stream dataStream, Utility.StreamCopyProgressDelegate callback)
        {
            string contenttype;
            if (filename == null)
            {
                filename = "";
                contenttype = "";
            }
            else
            {
                filename = " filename=\"" + filename + "\"";
                contenttype = "\r\nContent-Type: application/octet-stream";
            }

            byte[] headers = System.Text.Encoding.UTF8.GetBytes(string.Concat(new String[] { "Content-Disposition: form-data; name=\"" + name + "\";" + filename , "\"", contenttype, "\r\n\r\n"}));
            responseStream.Write(headers, 0, headers.Length);

            Utility.CopyStream(dataStream, responseStream, callback, 0);

            byte[] footer =  System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            responseStream.Write(footer, 0, footer.Length);
        }

        public System.Net.WebRequest SetResource(string id, System.IO.Stream outStream, System.IO.Stream content, System.IO.Stream header)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "SETRESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);

            EncodeFormParameters(boundary, param, outStream);
            if (content != null)
                AppendFormContent("CONTENT", "content.xml", boundary, outStream, content, null);					
            if (header != null)
                AppendFormContent("HEADER", "header.xml", boundary, outStream, header, null);

            req.ContentLength = outStream.Length;
            return req;
        }

        internal NameValueCollection SetResourceDataParams(string resourceid, string dataname, ResourceDataType datatype)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "SETRESOURCEDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceid);
            param.Add("DATANAME", dataname);
            param.Add("DATATYPE", datatype.ToString());

            return param;
        }

        public System.Net.WebRequest SetResourceData(string id, string dataname, ResourceDataType datatype, System.IO.Stream outStream, System.IO.Stream content, Utility.StreamCopyProgressDelegate callback)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "SETRESOURCEDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", id);
            param.Add("DATANAME", dataname);
            param.Add("DATATYPE", datatype.ToString());

            //This does not appear to be used anywhere in the MG WebTier code
            //anyway, set this if stream supports seeking
            if (content.CanSeek)
                param.Add("DATALENGTH", content.Length.ToString());

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);

            EncodeFormParameters(boundary, param, outStream);
            AppendFormContent("DATA", "content.bin", boundary, outStream, content, callback);					

            req.ContentLength = outStream.Length;
            return req;
        }

        internal string reqAsUrl(string resourceId, string classname, string filter, string[] columns)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "SELECTFEATURES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            param.Add("CLASSNAME", classname);
            
            //Using the very standard TAB character for column seperation
            //  ... nice if your data has "," or ";" in the column names :)
            if (columns != null)
                param.Add("PROPERTIES", string.Join("\t", columns));

            //param.Add("COMPUTED_ALIASES", computed_aliases);
            //param.Add("COMPUTED_PROPERTIES", computed_properties);
            if (filter != null)
                param.Add("FILTER", filter);
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string ExecuteSqlQuery(string featureSourceID, string sql)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "EXECUTESQLQUERY");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", featureSourceID);
            param.Add("SQL", sql);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public System.Net.WebRequest SelectFeatures(bool aggregate, string resourceId, string classname, string filter, string[] columns, NameValueCollection computedProperties, System.IO.Stream outStream)
        {
            NameValueCollection param = new NameValueCollection();
            if (aggregate)
                param.Add("OPERATION", "SELECTAGGREGATES");
            else
                param.Add("OPERATION", "SELECTFEATURES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            param.Add("CLASSNAME", classname);
            
            //Using the very standard TAB character for column seperation
            //  ... nice if your data has "," or ";" in the column names :)
            if (columns != null)
                param.Add("PROPERTIES", string.Join("\t", columns));

            if (computedProperties != null && computedProperties.Count > 0)
            {
                System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
                System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>();

                foreach(string s in computedProperties.Keys)
                {
                    keys.Add(s);
                    values.Add(computedProperties[s]);
                }

                param.Add("COMPUTED_ALIASES", string.Join("\t", keys.ToArray()));
                param.Add("COMPUTED_PROPERTIES", string.Join("\t", values.ToArray()));
            }
            if (filter != null)
                param.Add("FILTER", filter);

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }

        private System.Net.WebRequest PrepareFormContent(System.IO.Stream outStream, out string boundary)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(m_hosturi);
            boundary = "---------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] initialBound = System.Text.Encoding.UTF8.GetBytes(string.Concat("--", boundary, "\r\n"));
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.Timeout = 20 * 1000;
            req.Method = "POST";
            outStream.Write(initialBound, 0, initialBound.Length);
            return req;
        }

        public string DescribeSchema(string resourceId, string schema)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            param.Add("SCHEMA", schema);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string DescribeSchema(string resourceID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetProviderCapabilities(string provider)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETPROVIDERCAPABILITIES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("PROVIDER", provider);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string EnumerateCategories()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.ENUMERATECATEGORIES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string EnumerateCoordinateSystems(string category)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.ENUMERATECOORDINATESYSTEMS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSCATEGORY", category);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string ConvertWktToCoordinateSystemCode(string wkt)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.CONVERTWKTTOCOORDINATESYSTEMCODE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSWKT", wkt);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string ConvertCoordinateSystemCodeToWkt(string code)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.CONVERTCOORDINATESYSTEMCODETOWKT");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSCODE", code);

            return m_hosturi + "?" + EncodeParameters(param);
        }


        public string ConvertWktToEpsgCode(string wkt)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.CONVERTWKTTOEPSGCODE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSWKT", wkt);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string ConvertEpsgCodeToWkt(string code)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.CONVERTEPSGCODETOWKT");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSCODE", code);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetBaseLibrary()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.GETBASELIBRARY");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string IsValidCoordSys(string wkt)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CS.ISVALID");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("CSWKT", wkt);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string DeleteResource(string resourceid)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DELETERESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceid);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string MoveResource(string source, string target, bool overwrite)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "MOVERESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("SOURCE", source);
            param.Add("DESTINATION", target);
            param.Add("OVERWRITE", overwrite ? "1" : "0");

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string CopyResource(string source, string target, bool overwrite)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "COPYRESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("SOURCE", source);
            param.Add("DESTINATION", target);
            param.Add("OVERWRITE", overwrite ? "1" : "0");

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateResourceReferences(string resourceid)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATERESOURCEREFERENCES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceid);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public System.Net.WebRequest GetMapImage(string mapname, string format, string selectionXml, double centerX, double centerY, double scale, int dpi, int width, int height, bool clip, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups, System.IO.Stream outStream)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAPIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);
            param.Add("CLIENTAGENT", m_userAgent);

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (selectionXml != null && selectionXml.Length != 0)
                param.Add("SELECTION", selectionXml);
            
            param.Add("SETVIEWCENTERX", Utility.SerializeDigit(centerX));
            param.Add("SETVIEWCENTERY", Utility.SerializeDigit(centerY));
            param.Add("SETVIEWSCALE", Utility.SerializeDigit(scale));
            param.Add("SETDISPLAYDPI", dpi.ToString());
            param.Add("SETDISPLAYWIDTH", width.ToString());
            param.Add("SETDISPLAYHEIGHT", height.ToString());

            if (showlayers != null && showlayers.Length > 0)
                param.Add("SHOWLAYERS", string.Join(",", showlayers));
            if (hidelayers != null && hidelayers.Length > 0)
                param.Add("HIDELAYERS", string.Join(",", hidelayers));
            if (showgroups != null && showgroups.Length > 0)
                param.Add("SHOWGROUPS", string.Join(",", showgroups));
            if (hidegroups != null && hidegroups.Length > 0)
                param.Add("HIDEGROUPS", string.Join(",", hidegroups));

            param.Add("CLIP", clip ? "1" : "0");
            //TODO: Find out if this actually works...
            //param.Add("REFRESHLAYERS", ...)

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }

        public System.Net.WebRequest GetMapImage(string mapname, string format, string selectionXml, double x1, double y1, double x2, double y2, int dpi, int width, int height, bool clip, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups, System.IO.Stream outStream)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAPIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);
            param.Add("CLIENTAGENT", m_userAgent);

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (selectionXml != null && selectionXml.Length != 0)
                param.Add("SELECTION", selectionXml);

            param.Add("SETDATAEXTENT", Utility.SerializeDigit(x1) + ", " + Utility.SerializeDigit(y1) + ", " + Utility.SerializeDigit(x2) + ", " + Utility.SerializeDigit(y2));
            param.Add("SETDISPLAYDPI", dpi.ToString());
            param.Add("SETDISPLAYWIDTH", width.ToString());
            param.Add("SETDISPLAYHEIGHT", height.ToString());

            if (showlayers != null && showlayers.Length > 0)
                param.Add("SHOWLAYERS", string.Join(",", showlayers));
            if (hidelayers != null && hidelayers.Length > 0)
                param.Add("HIDELAYERS", string.Join(",", hidelayers));
            if (showgroups != null && showgroups.Length > 0)
                param.Add("SHOWGROUPS", string.Join(",", showgroups));
            if (hidegroups != null && hidegroups.Length > 0)
                param.Add("HIDEGROUPS", string.Join(",", hidegroups));

            param.Add("CLIP", clip ? "1" : "0");
            //TODO: Find out if this actually works...
            //param.Add("REFRESHLAYERS", ...)

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }
        
        public string BuildRequest(NameValueCollection param)
        {
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetMapImageUrl(string mapname, string format, string selectionXml, double centerX, double centerY, double scale, int dpi, int width, int height, bool clip, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAPIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);
            
            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (selectionXml != null && selectionXml.Length != 0)
                param.Add("SELECTION", selectionXml);
            

            param.Add("SETVIEWCENTERX", Utility.SerializeDigit(centerX));
            param.Add("SETVIEWCENTERY", Utility.SerializeDigit(centerY));
            param.Add("SETVIEWSCALE", Utility.SerializeDigit(scale));
            param.Add("SETDISPLAYDPI", dpi.ToString());
            param.Add("SETDISPLAYWIDTH", width.ToString());
            param.Add("SETDISPLAYHEIGHT", height.ToString());

            if (showlayers != null && showlayers.Length > 0)
                param.Add("SHOWLAYERS", string.Join(",", showlayers));
            if (hidelayers != null && hidelayers.Length > 0)
                param.Add("HIDELAYERS", string.Join(",", hidelayers));
            if (showgroups != null && showgroups.Length > 0)
                param.Add("SHOWGROUPS", string.Join(",", showgroups));
            if (hidegroups != null && hidegroups.Length > 0)
                param.Add("HIDEGROUPS", string.Join(",", hidegroups));

            //TODO: Find out if this actually works...
            //param.Add("REFRESHLAYERS", ...)

            param.Add("CLIP", clip ? "1" : "0");

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetMapImageUrl(string mapname, string format, string selectionXml, double x1, double y1, double x2, double y2, int dpi, int width, int height, bool clip, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAPIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (selectionXml != null && selectionXml.Length != 0)
                param.Add("SELECTION", selectionXml);


            param.Add("SETDATAEXTENT", Utility.SerializeDigit(x1) + ", " + Utility.SerializeDigit(y1) + ", " + Utility.SerializeDigit(x2) + ", " + Utility.SerializeDigit(y2));
            param.Add("SETDISPLAYDPI", dpi.ToString());
            param.Add("SETDISPLAYWIDTH", width.ToString());
            param.Add("SETDISPLAYHEIGHT", height.ToString());

            if (showlayers != null && showlayers.Length > 0)
                param.Add("SHOWLAYERS", string.Join(",", showlayers));
            if (hidelayers != null && hidelayers.Length > 0)
                param.Add("HIDELAYERS", string.Join(",", hidelayers));
            if (showgroups != null && showgroups.Length > 0)
                param.Add("SHOWGROUPS", string.Join(",", showgroups));
            if (hidegroups != null && hidegroups.Length > 0)
                param.Add("HIDEGROUPS", string.Join(",", hidegroups));

            param.Add("CLIP", clip ? "1" : "0");

            //TODO: Find out if this actually works...
            //param.Add("REFRESHLAYERS", ...)

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public System.Net.WebRequest QueryMapFeatures(string mapname, bool persist, string geometry, System.IO.Stream outStream, QueryMapFeaturesLayerAttributes attributes)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "QUERYMAPFEATURES");
            param.Add("VERSION", "1.0.0");
            param.Add("PERSIST", persist ? "1" : "0");
            param.Add("MAPNAME", mapname);
            param.Add("SESSION", m_sessionID);
            param.Add("GEOMETRY", geometry);
            param.Add("SELECTIONVARIANT", "INTERSECTS");
            param.Add("MAXFEATURES", "-1");
            param.Add("LAYERATTRIBUTEFILTER", ((int)attributes).ToString());
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }

        internal System.Net.WebRequest QueryMapFeatures(string runtimeMapName, int maxFeatures, string wkt, bool persist, string selectionVariant, Services.QueryMapOptions extraOptions, System.IO.Stream outStream)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "QUERYMAPFEATURES");
            param.Add("VERSION", "1.0.0");
            param.Add("PERSIST", persist ? "1" : "0");
            param.Add("MAPNAME", runtimeMapName);
            param.Add("SESSION", m_sessionID);
            param.Add("GEOMETRY", wkt);
            param.Add("SELECTIONVARIANT", selectionVariant);
            param.Add("MAXFEATURES", maxFeatures.ToString(CultureInfo.InvariantCulture));
            if (extraOptions != null)
            {
                param.Add("LAYERATTRIBUTEFILTER", ((int)extraOptions.LayerAttributeFilter).ToString());
                if (!string.IsNullOrEmpty(extraOptions.FeatureFilter))
                    param.Add("FEATUREFILTER", extraOptions.FeatureFilter);
                if (extraOptions.LayerNames != null && extraOptions.LayerNames.Length > 0)
                    param.Add("LAYERNAMES", string.Join(",", extraOptions.LayerNames));
            }
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }

        public string EnumerateApplicationTemplates()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEAPPLICATIONTEMPLATES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateApplicationWidgets()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEAPPLICATIONWIDGETS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateApplicationContainers()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEAPPLICATIONCONTAINERS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetSpatialContextInfo(string resourceID, bool activeOnly)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETSPATIALCONTEXTS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("RESOURCEID", resourceID);
            param.Add("ACTIVEONLY", activeOnly ? "1" : "0");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string HostURI { get { return m_hosturi; } }

        public string GetIdentityProperties(string resourceID, string schema, string classname)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETIDENTITYPROPERTIES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("RESOURCEID", resourceID);
            param.Add("SCHEMA", schema);
            param.Add("CLASSNAME", classname);
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEUNMANAGEDDATA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (startpath != null)
                param.Add("PATH", startpath);
            if (filter != null)
                param.Add("FILTER", filter);
            param.Add("RECURSIVE", recursive ? "1" : "0");
            if (type == UnmanagedDataTypes.Files)
                param.Add("Type", "Files");
            else if (type == UnmanagedDataTypes.Folders)
                param.Add("Type", "Folders");
            else
                param.Add("Type", "Both");

            if (m_locale != null)
                param.Add("LOCALE", m_locale);
            
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateUsers(string group)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEUSERS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (!string.IsNullOrEmpty(group))
                param.Add("GROUP", group);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateGroups()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEGROUPS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetLegendImage(double scale, string layerdef, int themeIndex, int type, int width, int height, string format)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETLEGENDIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("SCALE", scale.ToString(System.Globalization.CultureInfo.InvariantCulture));
            param.Add("LAYERDEFINITION", layerdef);
            param.Add("THEMECATEGORY", themeIndex.ToString());
            param.Add("WIDTH", width.ToString());
            param.Add("HEIGHT", height.ToString());
            param.Add("FORMAT", format);
            param.Add("TYPE", type.ToString());
            param.Add("CLIENTAGENT", m_userAgent);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetTile(string mapdefinition, string groupname, int row, int col, int scaleindex, string format, bool includeSessionID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETTILEIMAGE");
            param.Add("VERSION", "1.2.0");
            if (includeSessionID)
                param.Add("SESSION", m_sessionID);
            param.Add("SCALEINDEX", scaleindex.ToString());
            param.Add("MAPDEFINITION", mapdefinition);
            param.Add("FORMAT", format);
            param.Add("BASEMAPLAYERGROUPNAME", groupname);
            param.Add("TILECOL", col.ToString());
            param.Add("TILEROW", row.ToString());
            param.Add("CLIENTAGENT", m_userAgent);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetTileAnonymous(string mapdefinition, string groupname, int row, int col, int scaleindex, string format)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETTILEIMAGE");
            param.Add("VERSION", "1.2.0");
            param.Add("USERNAME", "Anonymous");
            param.Add("PASSWORD", "");
            param.Add("SCALEINDEX", scaleindex.ToString());
            param.Add("MAPDEFINITION", mapdefinition);
            param.Add("FORMAT", format);
            param.Add("BASEMAPLAYERGROUPNAME", groupname);
            param.Add("TILECOL", col.ToString());
            param.Add("TILEROW", row.ToString());
            param.Add("CLIENTAGENT", m_userAgent);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public System.Net.WebRequest ApplyPackage(System.IO.Stream filestream, Utility.StreamCopyProgressDelegate callback)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "APPLYRESOURCEPACKAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAX_FILE_SIZE", "100000000");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            string boundary;
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            req.Timeout = 1000 * 60 * 5; //Wait up to five minutes

            EncodeFormParameters(boundary, param, outStream);

            /*try { req.ContentLength = outStream.Length + filestream.Length; }
            catch {}*/

            System.IO.Stream s = req.GetRequestStream();

            Utility.CopyStream(outStream, s);
            outStream.Dispose();

            AppendFormContent("PACKAGE", "package.mgp", boundary, s, filestream, callback);
            s.Close();

            return req;
        }

        public System.Net.WebRequest UpdateRepository(string resourceId, System.IO.Stream headerstream)
        {
            if (m_sessionID == null)
                throw new Exception("Connection is not yet logged in");

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "UPDATEREPOSITORY");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("RESOURCEID", resourceId);
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            string boundary;
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);

            EncodeFormParameters(boundary, param, outStream);
            System.IO.Stream s = req.GetRequestStream();

            Utility.CopyStream(outStream, s);
            outStream.Dispose();

            AppendFormContent("HEADER", "header.xml", boundary, s, headerstream, null);
            s.Close();

            return req;
        }

        public string ResourceExists(string resourceId)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "RESOURCEEXISTS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETCONNECTIONPROPERTYVALUES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("PROVIDER", providerName);
            param.Add("PROPERTY", propertyName);
            param.Add("CONNECTIONSTRING", partialConnectionString);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string EnumerateDataStores(string providerName, string partialConnString)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEDATASTORES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("PROVIDER", providerName);
            param.Add("CONNECTIONSTRING", partialConnString);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string UserAgent
        {
            get { return m_userAgent; }
            set { m_userAgent = value; }
        }

        public string DescribeDrawing(string resourceID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBEDRAWING");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateDrawingSectionResources(string resourceID, string sectionName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEDRAWINGSECTIONRESOURCES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            param.Add("SECTION", sectionName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateDrawingSections(string resourceID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEDRAWINGSECTIONS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetDrawing(string resourceID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDRAWING");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetDrawingLayer(string resourceID, string sectionName, string layerName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDRAWINGLAYER");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            param.Add("SECTION", sectionName);
            param.Add("LAYER", layerName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetDrawingSection(string resourceID, string sectionName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDRAWINGSECTION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            param.Add("SECTION", sectionName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetDrawingSectionResource(string resourceID, string resourceName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDRAWINGSECTIONRESOURCE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            param.Add("RESOURCENAME", resourceName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string EnumerateDrawingLayers(string resourceID, string sectionName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "ENUMERATEDRAWINGLAYERS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            param.Add("SECTION", sectionName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetDrawingCoordinateSpace(string resourceID)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDRAWINGCOORDINATESPACE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetSchemas(string resourceId)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETSCHEMAS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetClassNames(string resourceId, string schemaName)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETCLASSES");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            if (!string.IsNullOrEmpty(schemaName))
                param.Add("SCHEMA", schemaName);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string DescribeSchemaPartial(string resourceID, string schemaName, string[] classNames)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceID);
            if (!string.IsNullOrEmpty(schemaName))
                param.Add("SCHEMA", schemaName);
            param.Add("CLASSNAMES", string.Join(".", classNames));
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetClassDefinition(string resourceId, string schemaName, string className)
        {
            //BOGUS: GETCLASSDEFINITION is FUBAR (#2015)
            //
            //Fortunately, we can workaround this via DESCRIBEFEATURESCHEMA with CLASSNAMES hint.
            //This should still tap into FDO RFC23 enhancements server-side where applicable

            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            if (!string.IsNullOrEmpty(schemaName))
                param.Add("SCHEMA", schemaName);
            param.Add("CLASSNAMES", className);

            /*
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETCLASSDEFINITION");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            param.Add("RESOURCEID", resourceId);
            if (!string.IsNullOrEmpty(schemaName))
                param.Add("SCHEMA", schemaName);
            param.Add("CLASSNAME", className);
            */
            return m_hosturi + "?" + EncodeParameters(param);
        }

        public string GetSiteInfo()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETSITEINFO");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);
            if (m_locale != null)
                param.Add("LOCALE", m_locale);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string GetDynamicMapOverlayImage(string mapname, string selectionXml, string format, Color selectionColor, int behavior)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDYNAMICMAPOVERLAYIMAGE");
            param.Add("VERSION", "2.1.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);
            param.Add("CLIENTAGENT", m_userAgent);
            param.Add("SELECTIONCOLOR", Utility.SerializeHTMLColorRGBA(selectionColor, true));
            param.Add("BEHAVIOR", behavior.ToString(CultureInfo.InvariantCulture));

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (!string.IsNullOrEmpty(selectionXml))
                param.Add("SELECTION", selectionXml);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal System.Net.WebRequest GetDynamicMapOverlayImage(string mapname, string selectionXml, string format, System.IO.Stream outStream)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETDYNAMICMAPOVERLAYIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapname);
            param.Add("CLIENTAGENT", m_userAgent);

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            if (selectionXml != null && selectionXml.Length != 0)
                param.Add("SELECTION", selectionXml);

            string boundary;
            System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
            EncodeFormParameters(boundary, param, outStream);
            req.ContentLength = outStream.Length;

            return req;
        }

        internal string GetFdoCacheInfo()
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETFDOCACHEINFO");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("FORMAT", "text/xml");
            param.Add("CLIENTAGENT", m_userAgent);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string RenderMapLegend(string mapName, int width, int height, string color, string format)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETMAPLEGENDIMAGE");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapName);
            param.Add("CLIENTAGENT", m_userAgent);

            if (format != null && format.Length != 0)
                param.Add("FORMAT", format);

            param.Add("WIDTH", width.ToString(CultureInfo.InvariantCulture));
            param.Add("HEIGHT", height.ToString(CultureInfo.InvariantCulture));

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string GetLongTransactions(string resourceId, bool activeOnly)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETLONGTRANSACTIONS");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("RESOURCEID", resourceId);
            param.Add("ACTIVEONLY", activeOnly ? "1" : "0");

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string GetSchemaMapping(string provider, string partialConnString)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "GETSCHEMAMAPPING");
            param.Add("VERSION", "1.0.0");
            param.Add("SESSION", m_sessionID);
            param.Add("PROVIDER", provider);
            param.Add("CONNECTIONSTRING", partialConnString);

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string DescribeRuntimeMap(string mapName, int requestedFeatures, int iconsPerScaleRange, string iconFormat, int iconWidth, int iconHeight)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "DESCRIBERUNTIMEMAP");
            param.Add("VERSION", "2.6.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPNAME", mapName);
            param.Add("REQUESTEDFEATURES", requestedFeatures.ToString());
            param.Add("ICONSPERSCALERANGE", iconsPerScaleRange.ToString());
            param.Add("ICONFORMAT", iconFormat.ToString());
            param.Add("ICONWIDTH", iconWidth.ToString());
            param.Add("ICONHEIGHT", iconHeight.ToString());

            return m_hosturi + "?" + EncodeParameters(param);
        }

        internal string CreateRuntimeMap(string mapDefinition, string targetMapName, int requestedFeatures, int iconsPerScaleRange, string iconFormat, int iconWidth, int iconHeight)
        {
            NameValueCollection param = new NameValueCollection();
            param.Add("OPERATION", "CREATERUNTIMEMAP");
            param.Add("VERSION", "2.6.0");
            param.Add("SESSION", m_sessionID);
            param.Add("MAPDEFINITION", mapDefinition);
            param.Add("TARGETMAPNAME", targetMapName);
            param.Add("REQUESTEDFEATURES", requestedFeatures.ToString());
            param.Add("ICONSPERSCALERANGE", iconsPerScaleRange.ToString());
            param.Add("ICONFORMAT", iconFormat.ToString());
            param.Add("ICONWIDTH", iconWidth.ToString());
            param.Add("ICONHEIGHT", iconHeight.ToString());

            return m_hosturi + "?" + EncodeParameters(param);
        }
    }
}
