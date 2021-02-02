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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Maestro.Base.Commands.SiteExplorer
{
    public class GetXYZTileSetUrlCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                if (wb.ActiveSiteExplorer != null)
                {
                    var items = wb.ActiveSiteExplorer.SelectedItems;
                    if (items.Length == 1)
                    {
                        var it = items[0];
                        if (it.ResourceType == ResourceTypes.TileSetDefinition.ToString())
                        {
                            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);
                            if (conn.ProviderName.ToUpper() != "MAESTRO.HTTP") //NOXLATE
                            {
                                MessageBox.Show(Strings.XYZUrlGen_UnsupportedConnection);
                                return;
                            }

                            string baseUrl = conn.GetCustomProperty("BaseUrl").ToString(); //NOXLATE
                            if (!baseUrl.EndsWith("/")) //NOXLATE
                                baseUrl += "/"; //NOXLATE

                            string agent = "";
                            var urls = new Dictionary<string, string>();
                            var tsd = (ITileSetDefinition)conn.ResourceService.GetResource(it.ResourceId);

                            foreach (var group in tsd.BaseMapLayerGroups)
                            {
                                var urlTemplate = baseUrl + $"mapagent/mapagent.fcgi?OPERATION=GETTILEIMAGE&VERSION=1.2.0&CLIENTAGENT={agent}&USERNAME=Anonymous&MAPDEFINITION={it.ResourceId}&BASEMAPLAYERGROUPNAME={group.Name}&TILECOL={{y}}&TILEROW={{x}}&SCALEINDEX={{z}}";
                                urls[group.Name] = urlTemplate;
                            }

                            using (var diag = new XYZUrlsDialog(urls))
                            {
                                diag.ShowDialog();
                            }
                        }
                    }
                }
            }
        }
    }
}
