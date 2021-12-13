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


using OSGeo.MapGuide.MaestroAPI.Commands;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Http.Commands
{
    internal class HttpGetWfsCapabilities : IGetWfsCapabilities
    {
        private readonly HttpServerConnection _conn;

        public HttpGetWfsCapabilities(HttpServerConnection conn)
        {
            _conn = conn;
        }

        public IServerConnection Parent => _conn;

        public Stream Execute(WfsVersion version)
        {
            var req = _conn.RequestBuilder.GetWfsCapabilities(version);
            return _conn.OpenRead(req);
        }

        public async Task<Stream> ExecuteAsync(WfsVersion version, CancellationToken cancel = default)
        {
            var req = _conn.RequestBuilder.GetWfsCapabilities(version);
            var resp = await _conn.GetAsync(req);

            resp.EnsureSuccessStatusCode();
            var stream = await resp.Content.ReadAsStreamAsync();
            return stream;
        }
    }
}
