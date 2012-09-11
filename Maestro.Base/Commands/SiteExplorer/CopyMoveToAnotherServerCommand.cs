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
using OSGeo.MapGuide.MaestroAPI.CrossConnection;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class CopyMoveToAnotherServerCommand : AbstractCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var exp = wb.ActiveSiteExplorer;
            if (exp.SelectedItems.Length > 0)
            {
                var source = svc.GetConnection(exp.ConnectionName);
                var login = new LoginDialog();
                if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var target = login.Connection;
                    var migrate = new CopyMoveToServerDialog(source, target);

                    var srcIds = new List<string>();
                    foreach (var item in exp.SelectedItems)
                    {
                        srcIds.Add(item.ResourceId);
                    }

                    migrate.SourceResourceIds = srcIds.ToArray();

                    if (migrate.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        int affected = DoMigrate(source, target, migrate);
                        MessageService.ShowMessage(string.Format(Strings.ItemsMigrated, affected));
                        if (affected > 0 && migrate.SelectedAction == MigrationAction.Move)
                        {
                            var parent = exp.SelectedItems[0].Parent;
                            if (parent != null)
                                exp.RefreshModel(source.DisplayName, parent.ResourceId);
                            else
                                exp.RefreshModel(source.DisplayName, StringConstants.RootIdentifier);
                        }
                    }
                }
            }
        }

        private static int DoMigrate(OSGeo.MapGuide.MaestroAPI.IServerConnection source, OSGeo.MapGuide.MaestroAPI.IServerConnection target, CopyMoveToServerDialog migrate)
        {
            var diag = new ProgressDialog();
            diag.CancelAbortsThread = true;
            var method = new ProgressDialog.DoBackgroundWork((worker, e, args) =>
            {
                var src = (IServerConnection)args[0];
                var dst = (IServerConnection)args[1];
                var ids = (string[])args[2];
                var folder = (string)args[3];
                var overwrite = (bool)args[4];
                var act = (MigrationAction)args[5];

                var cb = new LengthyOperationProgressCallBack((sender, cbe) =>
                {
                    worker.ReportProgress(cbe.Progress, cbe.StatusMessage);
                });

                var migrator = new ResourceMigrator(source, target);
                int affected = 0;
                switch (act)
                {
                    case MigrationAction.Copy:
                        affected = migrator.CopyResources(ids, folder, overwrite, cb);
                        break;
                    case MigrationAction.Move:
                        affected = migrator.MoveResources(ids, folder, overwrite, cb);
                        break;
                }
                return affected;
            });

            return (int)diag.RunOperationAsync(Workbench.Instance, method, source, target, migrate.SourceResourceIds, migrate.TargetFolder, migrate.OverwriteResources, migrate.SelectedAction);
        }
    }
}
