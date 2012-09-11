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
using System.Windows.Forms;
using Maestro.Base.Services;
using Maestro.Shared.UI;
using Aga.Controls.Tree;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.UI
{
    internal sealed class SiteExplorerDragDropHandler
    {
        internal static void OnDragEnter(ISiteExplorer sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Array a = e.Data.GetData(DataFormats.FileDrop) as Array;
                if (a != null && a.Length > 0)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        internal static void OnDragDrop(ISiteExplorer sender, DragEventArgs e, TreeNodeAdv droppedNode)
        {
            //If drop node specified, extract relevant folder, otherwise default to root (Library://)
            string folderId = StringConstants.RootIdentifier;
            IServerConnection conn = null;
            var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
                
            if (droppedNode != null)
            {
                var ri = droppedNode.Tag as RepositoryItem;
                if (ri != null)
                {
                    if (ri.IsFolder)
                        folderId = ri.ResourceId;
                    else
                        folderId = ri.Parent != null ? ri.Parent.ResourceId : StringConstants.RootIdentifier;
                }
                conn = mgr.GetConnection(ri.ConnectionName);
            }
            else
            {
                return;
            }

            Array a = e.Data.GetData(DataFormats.FileDrop) as Array;
            bool refresh = false;
            if (a != null && a.Length > 0)
            {
                DragDropHandlerService handlerSvc = ServiceRegistry.GetService<DragDropHandlerService>();
                for (int i = 0; i < a.Length; i++)
                {
                    string file = a.GetValue(i).ToString();

                    IList<IDragDropHandler> handlers = handlerSvc.GetHandlersForFile(file);

                    if (handlers.Count == 0)
                        continue;

                    if (handlers.Count == 1)
                    {
                        using (new WaitCursor(Workbench.Instance))
                        {
                            if (handlers[0].HandleDrop(conn, file, folderId))
                                refresh = true;
                        }
                    }

                    if (handlers.Count > 1)
                    {
                        //Resolve which handler to use
                    }
                }
            }
            if (refresh)
                sender.RefreshModel(conn.DisplayName, folderId);
        }
    }
}
