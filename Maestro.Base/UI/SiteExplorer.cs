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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core.WinForms;
using Aga.Controls.Tree;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using ICSharpCode.Core;
using Maestro.Base.UI.Preferences;
using Maestro.Shared.UI;
using Maestro.Base.Commands.SiteExplorer;
using Maestro.Base.Commands;

namespace Maestro.Base.UI
{
    public partial class SiteExplorer : ViewContentBase, ISiteExplorer
    {
        private IServerConnection _conn;

        /// <summary>
        /// Internal use only. Do not invoke directly. Use <see cref="ViewContentManager"/> for that
        /// </summary>
        public SiteExplorer()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(OnIdle);
            ndResource.ToolTipProvider = new RepositoryItemToolTipProvider();
            ndResource.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(OnNodeDrawText);
        }

        void OnIdle(object sender, EventArgs e)
        {
            foreach (var item in tsSiteExplorer.Items)
            {
                if (item is IStatusUpdate)
                    ((IStatusUpdate)item).UpdateStatus();
            }
        }

        public string ConnectionName
        {
            get;
            private set;
        }

        public SiteExplorer(string name) : this()
        {
            this.ConnectionName = name;
        }

        private RepositoryTreeModel _model;

        protected override void OnLoad(EventArgs e)
        {
            this.Title = Properties.Resources.Content_SiteExplorer;
            this.Description = string.Format("{0}: {1}", Properties.Resources.Content_SiteExplorer, this.ConnectionName);

            var ts = ToolbarService.CreateToolStripItems("/Maestro/Shell/SiteExplorer/Toolbar", this, true);
            tsSiteExplorer.Items.AddRange(ts);

            var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
            _conn = mgr.GetConnection(this.ConnectionName);

            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var clip = ServiceRegistry.GetService<ClipboardService>(); 
            _model = new RepositoryTreeModel(_conn, trvResources, this.ConnectionName, omgr, clip);
            trvResources.Model = _model;
        }

        public override bool AllowUserClose
        {
            get
            {
                return false;
            }
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Left;
            }
        }

        public void RefreshModel()
        {
            RefreshModel(null);
        }

        public void RefreshModel(string resId)
        {
            if (!string.IsNullOrEmpty(resId))
            {
                var rid = new ResourceIdentifier(resId);
                if (!rid.IsFolder)
                    resId = rid.ParentFolder;
                
                //If this node is not initially expanded, we get NRE on refresh
                ExpandNode(resId);

                var path = _model.GetPathFromResourceId(resId);
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    //Walk back up until node has children. We want to refresh from this node down
                    while (node.Children.Count == 0 && node != trvResources.Root)
                        node = node.Parent;
                }
                trvResources.SelectedNode = node;
            }
            _model.Refresh();
            trvResources.Root.Children[0].Expand();
        }

        private void trvResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvResources.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var item = node.Tag as RepositoryItem;
                if (item != null && !item.IsFolder)
                {
                    var resMgr = ServiceRegistry.GetService<OpenResourceManager>();
                    resMgr.Open(item.ResourceId, _conn, false, this);
                }
            }
        }

        private void trvResources_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var items = this.SelectedItems;
                if (items.Length > 0)
                {
                    if (items.Length == 1) //Single select
                    {
                        RepositoryItem item = items[0];
                        if (item.IsFolder)
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedFolder", trvResources, e.X, e.Y);
                        else
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedDocument", trvResources, e.X, e.Y);
                    }
                    else //Multi select
                    {
                        //All must be uniform type
                        int folderCount = 0;

                        foreach (var item in items)
                        {
                            if (item.IsFolder)
                                folderCount++;
                        }

                        if (folderCount == 0) //All selected documents
                        {
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedDocuments", trvResources, e.X, e.Y);
                        }
                        else if (folderCount == items.Length) //All selected folders
                        {
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedFolders", trvResources, e.X, e.Y);
                        }
                        else //Mixed selection
                        {
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedMixedResources", trvResources, e.X, e.Y);
                        }
                    }
                }
            }
        }


        public RepositoryItem[] SelectedItems
        {
            get 
            {
                List<RepositoryItem> items = new List<RepositoryItem>();
                if (trvResources.SelectedNodes.Count > 0)
                {
                    foreach (var node in trvResources.SelectedNodes)
                    {
                        items.Add((RepositoryItem)node.Tag);
                    }
                }
                return items.ToArray();
            }
        }

        private void trvResources_Expanding(object sender, TreeViewAdvEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void trvResources_Expanded(object sender, TreeViewAdvEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.Cursor = Cursors.Default;
                }));
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }


        public void ExpandNode(string folderId)
        {
            if ("Library://".Equals(folderId))
                return;

            var path = _model.GetPathFromResourceId(folderId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    node.IsExpanded = true;
                }
            }
        }

        public void SelectNode(string resourceId)
        {
            var path = _model.GetPathFromResourceId(resourceId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    trvResources.SelectedNode = node;
                }
            }
        }

        public void FlagNode(string resourceId, NodeFlagAction action)
        {
            var path = _model.GetPathFromResourceId(resourceId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    var item = (RepositoryItem)node.Tag;
                    switch (action)
                    {
                        //case NodeFlagAction.IndicateCopy:
                        //case NodeFlagAction.IndicateCut:
                        //    item.IsClipboarded = true;
                        //    break;
                        case NodeFlagAction.HighlightDirty:
                            item.IsDirty = true;
                            break;
                        case NodeFlagAction.HighlightOpen:
                            item.IsOpen = true;
                            break;
                        case NodeFlagAction.None:
                            item.Reset();
                            break;
                    }
                }
            }
        }

        void OnNodeDrawText(object sender, Aga.Controls.Tree.NodeControls.DrawEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            var ocolor = PropertyService.Get(ConfigProperties.OpenColor, Color.LightGreen);
            var dcolor = PropertyService.Get(ConfigProperties.DirtyColor, Color.Pink);

            var item = (RepositoryItem)e.Node.Tag;
            var ctx = e.Context;
            if (item.ClipboardState != RepositoryItem.ClipboardAction.None)
            {
                var oldFont = e.Font;
                e.Font = new Font(oldFont.FontFamily, oldFont.Size, oldFont.Style | FontStyle.Italic);
            }
            if (item.IsDirty)
                e.BackgroundBrush = new SolidBrush(dcolor);
            else if (item.IsOpen)
                e.BackgroundBrush = new SolidBrush(ocolor);
        }

        private void trvResources_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var nodes = e.Item as TreeNodeAdv[];
            if (nodes != null)
            {
                List<ResourceIdentifier> rids = new List<ResourceIdentifier>();
                foreach (var n in nodes)
                {
                    rids.Add(new ResourceIdentifier(((RepositoryItem)n.Tag).ResourceId));
                }
                trvResources.DoDragDrop(rids.ToArray(), DragDropEffects.All);
            }
        }

        private void trvResources_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (data == null)
            {
                //See if the mouse is currently over a node
                var node = trvResources.GetNodeAt(trvResources.PointToClient(new Point(e.X, e.Y)));
                SiteExplorerDragDropHandler.OnDragDrop(this, e, node);
            }
            else
            {
                //See if the mouse is currently over a node
                var node = trvResources.GetNodeAt(trvResources.PointToClient(new Point(e.X, e.Y)));
                if (node == null)
                    return;

                //Can only drop in a folder
                var item = node.Tag as RepositoryItem;
                if (item != null && item.IsFolder)
                {
                    string folderId = item.ResourceId;
                    List<string> resIds = new List<string>();
                    foreach (var n in data)
                    {
                        resIds.Add(n.ToString());
                    }

                    //I think it's nice to ask for confirmation
                    if (resIds.Count > 0)
                    {
                        if (!MessageService.AskQuestion(Properties.Resources.ConfirmMove))
                            return;
                    }

                    string[] folders = MoveResources(resIds, folderId);

                    foreach (var fid in folders)
                    {
                        LoggingService.Info("Refreshing: " + fid);
                        RefreshModel(fid);
                    }
                }
            }
        }

        private void trvResources_DragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (data == null)
            {
                SiteExplorerDragDropHandler.OnDragEnter(this, e);
            }
            else
            {
                //See if the mouse is currently over a node
                var node = trvResources.GetNodeAt(trvResources.PointToClient(new Point(e.X, e.Y)));
                if (node == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                //Is it a folder?
                var item = node.Tag as RepositoryItem;
                if (item != null && item.IsFolder)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void trvResources_DragEnter(object sender, DragEventArgs e)
        {
            //TODO: There is a whole lot of interesting things we can do here
            // (eg. Create a Feature Source from a dragged SDF file)
        }

        private string [] MoveResources(ICollection<string> resIds, string folderId)
        {
            var wb = Workbench.Instance;
            var dlg = new ProgressDialog();
            var worker = new ProgressDialog.DoBackgroundWork((w, e, args) =>
            {
                LengthyOperationProgressCallBack cb = (sender, cbe) =>
                {
                    w.ReportProgress(cbe.Progress, cbe.StatusMessage);
                };

                var f = (string)args[0];
                var resourceIds = (ICollection<string>)args[1];

                foreach (var r in resourceIds)
                {
                    if (ResourceIdentifier.IsFolderResource(r))
                    {
                        //IMPORTANT: We need to tweak the target resource id
                        //otherwise the content *inside* the source folder is
                        //moved instead of the folder itself!
                        var rid = new ResourceIdentifier(r);
                        var target = folderId + rid.Name + "/";
                        //_conn.ResourceService.MoveFolderWithReferences(r, target, null, cb);
                        _conn.ResourceService.MoveResourceWithReferences(r, target, null, cb);
                    }
                    else
                        _conn.ResourceService.MoveResourceWithReferences(r, folderId, null, cb);
                    //string msg = string.Format("Moving {0} to {1}", r, folderId);
                }

                //Collect affected folders and refresh them
                Dictionary<string, string> folders = new Dictionary<string, string>();
                folders.Add(folderId, folderId);
                foreach (var n in resourceIds)
                {
                    var ri = new ResourceIdentifier(n);
                    var parent = ri.ParentFolder;
                    if (parent != null && !folders.ContainsKey(parent))
                        folders.Add(parent, parent);
                }

                return folders.Keys;
            });

            var affectedFolders = (IEnumerable<string>)dlg.RunOperationAsync(wb, worker, folderId, resIds);
            return new List<string>(affectedFolders).ToArray();
        }

        private void trvResources_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                new DeleteSelectedItemsCommand().Run();
            }
        }
    }
}
