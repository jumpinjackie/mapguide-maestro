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

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    /// <summary>
    /// A basic http client abstraction
    /// </summary>
    public interface IHttpRequestor
    {
        /// <summary>
        /// Attaches the given credentials for authentication purposes
        /// </summary>
        /// <param name="cred"></param>
        void AttachCredentials(ICredentials cred);

        /// <summary>
        /// Performs a synchronous GET request
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Stream Get(string uri, IHttpGetRequestOptions options);

        /// <summary>
        /// Performs an asynchronous GET request
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string uri);

        /// <summary>
        /// Performs a synchronous GET request with the expectation that the
        /// response will be a byte array
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        byte[] DownloadData(string uri, IHttpGetRequestOptions options);
    }
}
