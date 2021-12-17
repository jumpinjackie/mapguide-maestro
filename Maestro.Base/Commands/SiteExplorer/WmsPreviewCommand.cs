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

using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Commands;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class WmsPreviewCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            if (connMgr.GetConnectionNames().Count == 0)
            {
                MessageBox.Show(Strings.NoOpenConnections);
                return;
            }

            if (exp.SelectedItems.Length == 1  && exp.SelectedItems[0] is WmsLayerRepositoryItem wmsLayer)
            {
                var conn = connMgr.GetConnection(wmsLayer.ConnectionName);
                if (conn.SiteVersion >= new System.Version(4, 0))
                {
                    var mapagent = conn.GetCustomProperty("BaseUrl")?.ToString(); //NOXLATE
                    if (!string.IsNullOrWhiteSpace(mapagent))
                    {
                        //Normalize
                        if (!mapagent.EndsWith("/"))
                            mapagent += "/";

                        //Compute our preview url
                        var wmsVer = "1.3.0";
                        var width = 800;
                        var height = 600;
                        var bbox = Uri.EscapeDataString(wmsLayer.BBOX.ConvertToString(wmsLayer.Crs, WmsVersion.v1_3_0));
                        var srs = Uri.EscapeDataString(wmsLayer.Crs);
                        var layerId = Uri.EscapeDataString(wmsLayer.LayerName);
                        var previewUrl = $"{mapagent}mapagent/mapagent.fcgi?REQUEST=GETMAP&SERVICE=WMS&VERSION={wmsVer}&FORMAT=application%2Fopenlayers&LAYERS={layerId}&BBOX={bbox}&SRS={srs}&WIDTH={width}&HEIGHT={height}&BGCOLOR=0x0000FF&TRANSPARENT=FALSE";

                        var ps = new ProcessStartInfo(previewUrl) //NOXLATE
                        {
                            UseShellExecute = true,
                            Verb = "open"
                        };
                        Process.Start(ps);
                    }
                }
                else
                {
                    MessageBox.Show(Strings.WmsPreviewUnsupportedVersion);
                }
            }
        }
    }
}
