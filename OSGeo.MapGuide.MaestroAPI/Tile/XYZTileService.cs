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

using OSGeo.MapGuide.MaestroAPI.Services;
using System.IO;
using System.Net;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    public class XYZTileService : ITileService
    {
        readonly string _urlTemplate;

        public XYZTileService(string urlTemplate)
        {
            //Convert into a string.Format-able form
            _urlTemplate = urlTemplate.Replace("{x}", "{0}").Replace("{y}", "{1}").Replace("{z}", "{2}");
        }

        public Stream GetTile(string mapDefinition, string baseLayerGroup, int column /* Y */, int row /* X */, int scaleIndex /* Z */)
        {
            var url = string.Format(_urlTemplate, row, column, scaleIndex);
            var req = HttpWebRequest.Create(url);

            var resp = req.GetResponse();
            return resp.GetResponseStream();
        }
    }
}
