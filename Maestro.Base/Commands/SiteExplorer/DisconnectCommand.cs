#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class DisconnectCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            if (!CancelDisconnect())
            {
                var name = wb.ActiveSiteExplorer.ConnectionName;
                svc.RemoveConnection(name);
                wb.ActiveSiteExplorer.FullRefresh();
            }
        }

        internal static bool CancelDisconnect()
        {
            var wb = Workbench.Instance;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            if (wb.ActiveSiteExplorer != null)
            {
                var openEditors = omgr.OpenEditors;
                if (openEditors.Length > 0)
                {
                    bool dirty = false;
                    foreach (var ed in openEditors)
                    {
                        if (ed.IsDirty)
                        {
                            dirty = true;
                            break;
                        }
                    }

                    if (dirty && !MessageService.AskQuestion(Strings.ConfirmCloseEditors))
                        return true;
                }

                var editors = omgr.OpenEditors;
                foreach (var ed in openEditors)
                {
                    ed.Close(true);
                }
            }
            return false;
        }
    }
}
