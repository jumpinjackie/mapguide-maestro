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
using Maestro.Shared.UI;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class DeleteSelectedItemsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var exp = wb.ActiveSiteExplorer;
            if (exp != null)
            {
                var conn = svc.GetConnection(exp.ConnectionName);
                var resSvc = conn.ResourceService;
                var items = exp.SelectedItems;
                if (items.Length > 0)
                {
                    if (MessageService.AskQuestion(Properties.Resources.ConfirmDelete))
                    {
                        if (ConfirmDeleteOpenResources(items, omgr.OpenEditors))
                        {
                            //Close any open editors on these resources before deleting them
                            foreach (var i in items)
                            {
                                omgr.CloseEditors(i.ResourceId, true);
                            }
                            DoDelete(wb, resSvc, items);
                        }
                    }

                    //Refresh the parent. Multi-select is only allowed from same parent
                    //so we should get the same result
                    var parent = ResourceIdentifier.GetParentFolder(items[0].ResourceId);
                    if (parent == null) //root?
                        exp.RefreshModel(null);
                    else
                        exp.RefreshModel(parent);
                }
            }
        }

        private static bool ConfirmDeleteOpenResources(RepositoryItem[] items, Maestro.Base.Editor.IEditorViewContent[] editors)
        {
            Check.NotNull(items, "items");
            Check.NotNull(editors, "editors");
            Dictionary<string, string> resIds = new Dictionary<string, string>();
            foreach (var item in items)
            {
                resIds.Add(item.ResourceId, item.ResourceId);
            }
            bool isDeletingOpenResource = false;
            foreach (var ed in editors)
            {
                if (resIds.ContainsKey(ed.EditorService.ResourceID))
                {
                    isDeletingOpenResource = true;
                    break;
                }
            }

            if (isDeletingOpenResource && !MessageService.AskQuestion(Properties.Resources.ConfirmDeleteOpenResource))
                return false;

            return true;
        }

        private void DoDelete(Workbench wb, OSGeo.MapGuide.MaestroAPI.Services.IResourceService resSvc, RepositoryItem[] items)
        {
            var pdlg = new ProgressDialog();
            pdlg.CancelAbortsThread = true;

            string[] args = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                args[i] = items[i].ResourceId;
            }

            var work = new Maestro.Shared.UI.ProgressDialog.DoBackgroundWork((worker, e, target) =>
            {
                int deleted = 0;

                if (target == null || target.Length == 0)
                    return deleted;

                int step = 100 / target.Length;
                int current = 0;

                foreach (RepositoryItem item in target)
                {
                    if (worker.CancellationPending || e.Cancel)
                        return deleted;

                    current += step;

                    if (item.IsRoot) //Wait a minute...!
                    {
                        continue;
                    }
                    else
                    {
                        resSvc.DeleteResource(item.ResourceId);
                        deleted++;
                        worker.ReportProgress(current, item.ResourceId);
                    }
                }

                //collect affected parents and update the model
                foreach (RepositoryItem item in target)
                {
                    var parent = item.Parent;
                    if (parent != null)
                    {
                        parent.RemoveChild(item);
                    }
                }

                return deleted;
            });

            pdlg.RunOperationAsync(wb, work, items);
        }
    }
}
