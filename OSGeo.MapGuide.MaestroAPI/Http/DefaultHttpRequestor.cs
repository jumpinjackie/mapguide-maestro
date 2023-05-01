#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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

using OSGeo.MapGuide.ObjectModels;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    /// <summary>
    /// The default implementation of <see cref="IHttpRequestor"/> that is backed
    /// by <see cref="HttpClient"/>
    /// </summary>
    internal class DefaultHttpRequestor : IHttpRequestor
    {
        private ICredentials _cred;
        readonly HttpClient _http;

        public DefaultHttpRequestor()
        {
            _http = new HttpClient();
        }

        public void AttachCredentials(ICredentials cred)
        {
            _cred = cred;
        }

        public byte[] DownloadData(string uri, IHttpGetRequestOptions options)
        {
            string prev_session = options.SessionID;
            try
            {
                var httpreq = HttpWebRequest.Create(uri);
                var httpresp = (HttpWebResponse)httpreq.GetResponse();
                options.LogResponse(httpresp);
                using (var st = httpresp.GetResponseStream())
                {
                    //Can't use pooled MS here as buffer is invalidated on disposal
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
                    options.LogFailedRequest((WebException)ex);

                if (!options.AutoRestartSession || !Utility.IsSessionExpiredException(ex) || !options.RestartSession(false))
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
                    uri = uri.Replace(prev_session, options.SessionID);

                    var httpreq = HttpWebRequest.Create(uri);
                    var httpresp = httpreq.GetResponse();

                    using (var st = httpresp.GetResponseStream())
                    {
                        using (var ms = MemoryStreamPool.GetStream())
                        {
                            Utility.CopyStream(st, ms);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
        }

        public Stream Get(string uri, IHttpGetRequestOptions options, int? requestTimeout)
        {
            string prev_session = options.SessionID;
            try
            {
                var httpreq = HttpWebRequest.Create(uri);

                if (requestTimeout.HasValue)
                    httpreq.Timeout = requestTimeout.Value;

                if (_cred != null)
                    httpreq.Credentials = _cred;
                var httpresp = (HttpWebResponse)httpreq.GetResponse();
                options.LogResponse(httpresp);
                return new WebResponseStream(httpresp);
            }
            catch (Exception ex)
            {
                if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                    options.LogFailedRequest((WebException)ex);

                var sessionRecreated = false;
                if (Utility.IsSessionExpiredException(ex))
                    sessionRecreated = options.RestartSession(false);
                if (!options.AutoRestartSession || !Utility.IsSessionExpiredException(ex) || !sessionRecreated)
                {
                    Exception ex2 = Utility.ThrowAsWebException(ex);
                    if (ex2 != ex)
                        throw ex2;
                    else
                        throw;
                }
                else
                {
                    uri = uri.Replace(prev_session, options.SessionID);
                    var httpreq = HttpWebRequest.Create(uri);
                    if (_cred != null)
                        httpreq.Credentials = _cred;
                    var httpresp = (HttpWebResponse)httpreq.GetResponse();
                    return new WebResponseStream(httpresp);
                }
            }
        }

        public Task<HttpResponseMessage> GetAsync(string uri) => _http.GetAsync(uri);
    }
}
