#region Disclaimer / License

// Copyright (C) 2023, Jackie Ng
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
using System.Net;

namespace Maestro.Editors.Fusion
{
    internal class EpsgLookupResult
    {
        public string Proj4 { get; set; }
    }

    internal static class EpsgLookupExtensions
    {
        static DummyGetRequestOptions _smOptions = new DummyGetRequestOptions();

        public static EpsgLookupResult SyncFetch(this IHttpRequestor http, string epsgCode)
        {
            using var s = http.Get($"https://spatialreference.org/ref/epsg/{epsgCode}/proj4.txt", _smOptions);
            string proj4 = new System.IO.StreamReader(s).ReadToEnd();

            return new EpsgLookupResult { Proj4 = proj4 };
        }
    }

    internal class DummyGetRequestOptions : IHttpGetRequestOptions
    {
        public bool AutoRestartSession => false;

        public string SessionID => string.Empty;

        public void LogFailedRequest(WebException wex) { }

        public void LogResponse(HttpWebResponse resp) { }

        public bool RestartSession(bool throwException) => false;
    }
}
