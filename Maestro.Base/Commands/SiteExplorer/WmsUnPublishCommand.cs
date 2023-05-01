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
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class WmsUnPublishCommand : AbstractMenuCommand
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

            if (exp.SelectedItems.Length == 1 && exp.SelectedItems[0] is WmsLayerRepositoryItem wmsLayer)
            {
                var ldfId = $"Library://{wmsLayer.LayerName}.LayerDefinition";
                if (MessageBox.Show(String.Format(Strings.WmsUnpublishConfirm, ldfId), Strings.WmsUnpublishConfirmTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var conn = connMgr.GetConnection(wmsLayer.ConnectionName);
                    var header = conn.ResourceService.GetResourceHeader(ldfId);

                    Utility.StripWmsMetadata(header);

                    conn.ResourceService.SetResourceHeader(ldfId, header);

                    //Refresh the site explorer so that the WMS layers node is invalidated
                    exp.RefreshModel(exp.ConnectionName);
                }
            }
        }
    }
}
