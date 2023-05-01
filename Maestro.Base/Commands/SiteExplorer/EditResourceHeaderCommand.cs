﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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
using System.Linq;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class EditResourceHeaderCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var sel = exp.GetSelectedResources().ToArray();
            if (sel.Length == 1)
            {
                var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                var conn = connMgr.GetConnection(exp.ConnectionName);
                if (!IsValid(conn))
                {
                    MessageService.ShowError(Strings.ConnectionDoesNotSupportRequiredInterfaces);
                    return;
                }

                var item = sel[0];
                var diag = new ResourceHeaderXmlDialog(item.ResourceId, conn.ResourceService);
                if (item.IsFolder)
                {
                    var header = conn.ResourceService.GetFolderHeader(item.ResourceId);
                    diag.LoadFolderHeader(header);
                }
                else
                {
                    var header = conn.ResourceService.GetResourceHeader(item.ResourceId);
                    diag.LoadResourceHeader(header);
                }
                diag.ShowDialog();
            }
        }

        private bool IsValid(OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            return conn.Capabilities.SupportsResourceHeaders;
        }
    }
}