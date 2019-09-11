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

using OSGeo.MapGuide.MaestroAPI.Http;
using OSGeo.MapGuide.MaestroAPI.Services;
using Polly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// An implementation of <see cref="OSGeo.MapGuide.MaestroAPI.Services.ITileService"/> for fetching XYZ tiles
    /// </summary>
    public class XYZTileService : ITileService
    {
        readonly HttpClient _http;
        private string _urlTemplate;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="urlTemplate">The URL of the XYZ tile service/endpoint. The URL must have {x}, {y} and {z} placeholders</param>
        public XYZTileService(string urlTemplate)
        {
            _http = new HttpClient();
            this.SetUrlTemplate(urlTemplate);
        }

        public void SetUrlTemplate(string urlTemplate)
        {
            //Convert into a string.Format-able form
            _urlTemplate = urlTemplate.Replace("{x}", "{0}").Replace("{y}", "{1}").Replace("{z}", "{2}");
        }

        /// <summary>
        /// Gets the tile at the given XYZ coordinate
        /// </summary>
        /// <param name="mapDefinition">Un-used parameter</param>
        /// <param name="baseLayerGroup">Un-used parameters</param>
        /// <param name="column">Y value</param>
        /// <param name="row">X value</param>
        /// <param name="scaleIndex">Z value</param>
        /// <returns></returns>
        public Stream GetTile(string mapDefinition, string baseLayerGroup, int column /* Y */, int row /* X */, int scaleIndex /* Z */)
        {
            var url = string.Format(_urlTemplate, row, column, scaleIndex);
            return OpenReadWithExponentialBackoff(url);
        }

        const int SECONDS = 1000;

        internal Stream OpenReadWithExponentialBackoff(string url)
        {
            var jitterer = new Random();
            const int retries = 5;
            Stream stream = null;
            try
            {
                stream = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(retries, retryAttempt => //Calculate delay for next attempt
                    {
                        return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                            + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                    }, (lastException, nextRetry) => //OnRetry
                    {
                        string requestUrl = url; //Capture for debuggability
                        //The URL given is assumed to be stateless, so there is no need
                        //for checking if we need to rebuild an expired session

                        //var sessionRecreated = false;
                        //if (this.IsSessionExpiredException(lastException))
                        //    sessionRecreated = this.RestartSession(false);
                    })
                    .Execute(() =>
                    {
                        var httpreq = (HttpWebRequest)HttpWebRequest.Create(url);
                        httpreq.Timeout = 10 * SECONDS;
                        httpreq.KeepAlive = true;
                        var httpresp = (HttpWebResponse)httpreq.GetResponse();
                        return new WebResponseStream(httpresp);
                    });
            }
            catch (Exception ex)
            {
                //if (typeof(WebException).IsAssignableFrom(ex.GetType()))
                //    LogFailedRequest((WebException)ex);
                Exception ex2 = Utility.ThrowAsWebException(ex);
                if (ex2 != ex)
                    throw ex2;
                else
                    throw;
            }
            return stream;
        }

        /// <summary>
        /// Reads a tile from MapGuide
        /// </summary>
        /// <param name="mapDefinition"></param>
        /// <param name="baseLayerGroup"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="scaleIndex"></param>
        /// <returns></returns>
        public async Task<System.IO.Stream> GetTileAsync(string mapDefinition, string baseLayerGroup, int column, int row, int scaleIndex)
        {
            var url = string.Format(_urlTemplate, row, column, scaleIndex);
            var jitterer = new Random();
            const int retries = 5;
            var stream = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(retries, retryAttempt => //Calculate delay for next attempt
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                })
                .ExecuteAsync(async () =>
                {
                    var response = await _http.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStreamAsync();
                });

            return stream;
        }
    }
}
