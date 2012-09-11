#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class SetupFolderStructureCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            if (exp.SelectedItems.Length == 1)
            {
                var current = exp.SelectedItems[0].ResourceId;

                List<string> names = new List<string>()
                {
                    Strings.Folder_Data,
                    Strings.Folder_Layers,
                    Strings.Folder_Layouts,
                    Strings.Folder_Maps,
                    Strings.Folder_Symbols
                };
                if (conn.SiteVersion >= new Version(2, 3))
                {
                    names.Add(Strings.Folder_Watermarks);
                }
                foreach (var n in names)
                {
                    string fid = current + n + "/"; //NOXLATE
                    if (!conn.ResourceService.ResourceExists(fid))
                    {
                        conn.ResourceService.SetResourceXmlData(fid, null);
                        LoggingService.Info("Created Folder: " + fid);
                    }
                }
                exp.RefreshModel(conn.DisplayName, current);
                exp.ExpandNode(conn.DisplayName, current);
            }
        }
    }
}
