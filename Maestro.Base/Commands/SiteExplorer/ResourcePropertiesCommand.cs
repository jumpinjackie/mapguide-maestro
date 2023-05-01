﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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
    internal class ResourcePropertiesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var items = wb.ActiveSiteExplorer.GetSelectedResources().ToArray();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            if (!IsValid(conn))
            {
                MessageService.ShowError(string.Format(Strings.ConnectionDoesNotSupportRequiredInterfaces, conn.ProviderName));
                return;
            }

            //Can only show properties of one selected item
            if (items.Length == 1)
            {
                var openMgr = ServiceRegistry.GetService<OpenResourceManager>();

                var icons = ResourceIconCache.CreateDefault();
                var dlg = new ResourcePropertiesDialog(icons, conn, items[0].ResourceId, openMgr, wb.ActiveSiteExplorer);
                dlg.ShowDialog(wb);
            }
        }

        private bool IsValid(OSGeo.MapGuide.MaestroAPI.IServerConnection conn)
        {
            return conn.Capabilities.SupportsResourceHeaders &&
                   conn.Capabilities.SupportsResourceReferences &&
                   conn.Capabilities.SupportsResourceSecurity &&
                   conn.Capabilities.SupportsWfsPublishing &&
                   conn.Capabilities.SupportsWmsPublishing;
        }
    }
}