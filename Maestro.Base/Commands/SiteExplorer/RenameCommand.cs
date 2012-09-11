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
using Maestro.Base.UI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class RenameCommand : AbstractMenuCommand
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
                var current = exp.SelectedItems[0];
                var parent = current.Parent;
                List<string> names = new List<string>();
                foreach (var item in parent.Children)
                {
                    if (item != exp.SelectedItems[0])
                    {
                        names.Add(item.Name);
                    }
                }

                if (!current.IsFolder && omgr.IsOpen(current.ResourceId, conn))
                {
                    MessageService.ShowMessage(Strings.CannotRenameAlreadyOpenedResource);
                    return;
                }

                var dlg = new RenameItemDialog(current.Name, names);
                if (dlg.ShowDialog(wb) == System.Windows.Forms.DialogResult.OK)
                {
                    string oldid = string.Empty;
                    string newid = string.Empty;
                    if (current.IsFolder)
                    {
                        oldid = parent.ResourceId + current.Name + "/"; //NOXLATE
                        newid = parent.ResourceId + dlg.NewName + "/"; //NOXLATE
                    }
                    else
                    {
                        oldid = parent.ResourceId + current.Name + "." + current.ResourceType; //NOXLATE
                        newid = parent.ResourceId + dlg.NewName + "." + current.ResourceType; //NOXLATE
                    }

                    if (omgr.IsOpen(newid, conn))
                    {
                        MessageService.ShowMessage(string.Format(Strings.CannotRenameToResourceAlreadyOpened, newid));
                        return;
                    }

                    var prog = new ProgressDialog();
                    prog.RunOperationAsync(wb, (worker, e, args) =>
                    {
                        LengthyOperationProgressCallBack cb = (s, cbArgs) =>
                        {
                            worker.ReportProgress(cbArgs.Progress, cbArgs.StatusMessage);
                        };

                        //Perform the operation
                        if (dlg.UpdateReferences)
                            conn.ResourceService.MoveResourceWithReferences(oldid, newid, null, cb);
                        else
                            conn.ResourceService.MoveResource(oldid, newid, dlg.Overwrite);
                        /*
                        if (current.IsFolder)
                        {
                            if (dlg.UpdateReferences)
                                conn.ResourceService.MoveFolderWithReferences(oldid, newid, null, cb);
                            else
                                conn.ResourceService.MoveFolder(oldid, newid, dlg.Overwrite);
                        }
                        else
                        {
                            if (dlg.UpdateReferences)
                            {
                                conn.ResourceService.MoveResourceWithReferences(oldid, newid, null, cb);
                            }
                            else
                                conn.ResourceService.MoveResource(oldid, newid, dlg.Overwrite);
                        }*/

                        current.Name = dlg.NewName;
                        if (dlg.Overwrite)
                            parent.RemoveChild(parent[dlg.NewName]);

                        return true;
                    });
                    

                    //Need to refresh the model because it still is called by the old name
                    var rid = new OSGeo.MapGuide.MaestroAPI.Resource.ResourceIdentifier(oldid);
                    var folder = rid.ParentFolder;
                    exp.RefreshModel(conn.DisplayName, folder);
                }
            }
        }
    }
}
