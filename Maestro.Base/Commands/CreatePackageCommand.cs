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
using Maestro.Base.UI.Packaging;
using Maestro.Packaging;
using System.Linq;

namespace Maestro.Base.Commands
{
    internal class CreatePackageCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();

            var sel = exp.GetSelectedResources().ToArray();

            if (sel.Length <= 1)
            {
                //TODO: Will need to look at this again, when we decide to support
                //multiple connections to different sites from the same session. Right
                //now this is fine as we can only connect to one site at any time.
                var conn = connMgr.GetConnection(exp.ConnectionName);
                var dlg = new CreatePackageDialog(conn);

                if (sel.Length == 1)
                    dlg.FolderToPackage = sel[0].ResourceId;

                if (dlg.ShowDialog(wb) == System.Windows.Forms.DialogResult.OK)
                {
                    if (dlg.Source == CreatePackageDialog.PackageSource.Folder)
                    {
                        PackageProgress.CreatePackage(
                            wb,
                            conn,
                            dlg.FolderToPackage,
                            dlg.OutputFileName,
                            dlg.SelectedTypes,
                            dlg.RemoveTargetFolderOnRestore,
                            dlg.RestorePath);
                    }
                    else //Resource id list
                    {
                        PackageProgress.CreatePackage(
                            wb,
                            conn,
                            dlg.ResourceIds,
                            dlg.OutputFileName,
                            dlg.SelectedTypes,
                            dlg.RemoveTargetFolderOnRestore,
                            dlg.RestorePath);
                    }
                }
            }
        }
    }
}