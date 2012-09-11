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
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Login;
using Maestro.Editors.Migration;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.CrossConnection;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class MigrateResourceCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var exp = wb.ActiveSiteExplorer;
            if (exp.SelectedItems.Length == 1 && !exp.SelectedItems[0].IsFolder)
            {
                var source = svc.GetConnection(exp.ConnectionName);
                var login = new LoginDialog();
                if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var target = login.Connection;
                    var dlg = new MigrateDialog(source, target);
                    dlg.ResourceID = exp.SelectedItems[0].ResourceId;

                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DoMigrate(source, target, dlg.ResourceID, dlg.DependentResources, dlg.OverwriteExisting);
                        MessageService.ShowMessage(string.Format(Strings.ResourceMigrated, exp.SelectedItems[0].ResourceId));
                    }
                }
            }
        }

        private void DoMigrate(IServerConnection source, IServerConnection target, string resourceId, string[] dependentResourceIds, bool overwrite)
        {
            var diag = new ProgressDialog();
            diag.CancelAbortsThread = true;
            var method = new ProgressDialog.DoBackgroundWork((worker, e, args) =>
            {
                var src = (IServerConnection)args[0];
                var dst = (IServerConnection)args[1];
                var resId = (string)args[2];
                var dependents = (string[])args[3];
                var overwriteExisting = (bool)args[4];

                var cb = new LengthyOperationProgressCallBack((sender, cbe) =>
                {
                    worker.ReportProgress(cbe.Progress, cbe.StatusMessage);
                });

                var migrator = new ResourceMigrator(source, target);
                migrator.MigrateResource(resId, dependentResourceIds, overwriteExisting, cb);
                return true;
            });

            diag.RunOperationAsync(Workbench.Instance, method, source, target, resourceId, dependentResourceIds, overwrite);
        }
    }
}
