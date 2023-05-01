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


using System.Net;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    /// <summary>
    /// HTTP GET request options
    /// </summary>
    public interface IHttpGetRequestOptions
    {
        /// <summary>
        /// If the current session id has expired, attempt to establish a new session
        /// and resume the current operation with the re-established session id
        /// </summary>
        bool AutoRestartSession { get; }

        /// <summary>
        /// The current session id
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// Log the given HTTP response to the implementation-defined logging system
        /// </summary>
        /// <param name="resp"></param>
        void LogResponse(HttpWebResponse resp);

        /// <summary>
        /// Log the given failed HTTP request to the implementation-defined logging system
        /// </summary>
        /// <param name="wex"></param>
        void LogFailedRequest(WebException wex);

        /// <summary>
        /// Attempt to establish a new session
        /// </summary>
        /// <param name="throwException"></param>
        /// <returns></returns>
        bool RestartSession(bool throwException);
    }
}
