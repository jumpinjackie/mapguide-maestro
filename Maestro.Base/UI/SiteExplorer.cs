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
using System.Linq;
using Maestro.Editors;
using OSGeo.MapGuide.MaestroAPI.CrossConnection;

namespace Maestro.Base.UI
{
    /// <summary>
    /// The site explorer
    /// </summary>
    internal partial class SiteExplorer : ViewContentBase, ISiteExplorer
    {
        /// <summary>
        /// Internal use only. Do not invoke directly. Use <see cref="ViewContentManager"/> for that
        /// </summary>
        public SiteExplorer()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(OnIdle);
            ndResource.ToolTipProvider = new RepositoryItemToolTipProvider();
            ndResource.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(OnNodeDrawText);

            var ts = ToolbarService.CreateToolStripItems("/Maestro/Shell/SiteExplorer/Toolbar", this, true); //NOXLATE
            tsSiteExplorer.Items.AddRange(ts);

            _connManager = ServiceRegistry.GetService<ServerConnectionManager>();

            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var clip = ServiceRegistry.GetService<ClipboardService>();
            _model = new RepositoryTreeModel(_connManager, trvResources, omgr, clip);
            trvResources.Model = _model;

            Workbench wb = Workbench.Instance;
            wb.ActiveDocumentChanged += OnActiveDocumentChanged;
        }

        void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            Workbench wb = Workbench.Instance;
            var ed = wb.ActiveEditor;
            if (ed != null)
            {
                if (ed.Resource != null && ed.EditorService != null)
                {
                    FocusOnNode(ed.Resource.CurrentConnection.DisplayName, ed.EditorService.ResourceID);
                }
            }
        }

        void OnIdle(object sender, EventArgs e)
        {
            foreach (var item in tsSiteExplorer.Items)
            {
                if (item is IStatusUpdate)
                    ((IStatusUpdate)item).UpdateStatus();
            }
        }

        public event EventHandler ActiveConnectionChanged;

        private void OnActiveConnectionChanged()
        {
            var h = this.ActiveConnectionChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        private string _currentConnectionName;

        private void trvResources_SelectionChanged(object sender, EventArgs e)
        {
            var connName = this.ConnectionName;
            if (_currentConnectionName == null)
            {
                _currentConnectionName = connName;
                OnActiveConnectionChanged();
            }
            else if (_currentConnectionName != connName)
            {
                _currentConnectionName = connName;
                OnActiveConnectionChanged();
            }

            var h = this.ItemsSelected;
            if (h != null)
                h(this, this.SelectedItems);
        }

        public event RepositoryItemEventHandler ItemsSelected;

        public string ConnectionName
        {
            get
            {
                if (trvResources.SelectedNode != null)
                {
                    var item = (RepositoryItem)trvResources.SelectedNode.Tag;
                    return item.ConnectionName;
                }
                else if (trvResources.SelectedNodes != null && trvResources.SelectedNodes.Count > 0)
                {
                    var item = (RepositoryItem)trvResources.SelectedNodes[0].Tag;
                    return item.ConnectionName;
                }
                else if (trvResources.Root.Children.Count == 1)
                {
                    var item = (RepositoryItem)trvResources.Root.Children[0].Tag;
                    return item.ConnectionName;
                }
                return null;
            }
        }

        private RepositoryTreeModel _model;
        private ServerConnectionManager _connManager;

        protected override void OnLoad(EventArgs e)
        {
            
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

        public void FullRefresh()
        {
            _model.FullRefresh();
            foreach(var node in trvResources.Root.Children)
            {
                node.Expand();
            }
        }

        public void RefreshModel(string connectionName)
        {
            RefreshModel(connectionName, null);
        }

        public void RefreshModel(string connectionName, string resId)
        {
            if (!string.IsNullOrEmpty(resId))
            {
                var rid = new ResourceIdentifier(resId);
                if (!rid.IsFolder)
                    resId = rid.ParentFolder;

                //If this node is not initially expanded, we get NRE on refresh
                ExpandNode(connectionName, resId);
                
                var path = _model.GetPathFromResourceId(connectionName, resId);
                while (path == null)
                {
                    resId = ResourceIdentifier.GetParentFolder(resId);
                    path = _model.GetPathFromResourceId(connectionName, resId);
                }

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
            if (!string.IsNullOrEmpty(resId))
            {
                SelectNode(connectionName, resId);
            }
            //trvResources.Root.Children[0].Expand();
        }

        private void trvResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvResources.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var item = node.Tag as RepositoryItem;
                if (item != null && !item.IsFolder)
                {
                    var conn = _connManager.GetConnection(RepositoryTreeModel.GetParentConnectionName(item));
                    var resMgr = ServiceRegistry.GetService<OpenResourceManager>();
                    resMgr.Open(item.ResourceId, conn, false, this);
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
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedFolder", trvResources, e.X, e.Y); //NOXLATE
                        else
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedDocument", trvResources, e.X, e.Y); //NOXLATE
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
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedDocuments", trvResources, e.X, e.Y); //NOXLATE
                        }
                        else if (folderCount == items.Length) //All selected folders
                        {
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedFolders", trvResources, e.X, e.Y); //NOXLATE
                        }
                        else //Mixed selection
                        {
                            MenuService.ShowContextMenu(this, "/Maestro/Shell/SiteExplorer/SelectedMixedResources", trvResources, e.X, e.Y); //NOXLATE
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


        public void ExpandNode(string connectionName, string folderId)
        {
            if (StringConstants.RootIdentifier.Equals(folderId))
                return;

            var path = _model.GetPathFromResourceId(connectionName, folderId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    node.IsExpanded = true;
                }
            }
        }

        public void SelectNode(string connectionName, string resourceId)
        {
            var path = _model.GetPathFromResourceId(connectionName, resourceId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    trvResources.SelectedNode = node;
                }
            }
        }

        public void FocusOnNode(string connectionName, string resourceId)
        {
            var path = _model.GetPathFromResourceId(connectionName, resourceId);
            if (path != null)
            {
                var node = trvResources.FindNode(path, true);
                if (node != null)
                {
                    trvResources.ScrollTo(node);
                }
            }
        }

        public void FlagNode(string connectionName, string resourceId, NodeFlagAction action)
        {
            var path = _model.GetPathFromResourceId(connectionName, resourceId);
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
                    trvResources.Invalidate();
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
            if (nodes != null && nodes.Length > 0)
            {
                IServerConnection conn = null;
                List<RepositoryHandle> rids = new List<RepositoryHandle>();
                foreach (var n in nodes)
                {
                    var ri = (RepositoryItem)n.Tag;
                    conn = _connManager.GetConnection(ri.ConnectionName);
                    rids.Add(new RepositoryHandle(new ResourceIdentifier(ri.ResourceId), conn));

                }
                trvResources.DoDragDrop(rids.ToArray(), DragDropEffects.All);
            }
        }

        private void trvResources_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
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
                    string connectionName = RepositoryTreeModel.GetParentConnectionName(item);
                    string folderId = item.ResourceId;

                    if (data.Length < 0)
                        return;

                    if (data.First().Connection.DisplayName == connectionName)
                    {
                        //I think it's nice to ask for confirmation
                        if (data.Length > 0)
                        {
                            if (!MessageService.AskQuestion(Strings.ConfirmMove))
                                return;
                        }

                        string[] folders = MoveResourcesWithinConnection(connectionName, data.Select(x => x.ResourceId.ToString()).ToArray(), folderId);

                        foreach (var fid in folders)
                        {
                            LoggingService.Info("Refreshing: " + fid + " on " + connectionName);  //NOXLATE
                            RefreshModel(connectionName, fid);
                        }
                    }
                    else
                    {
                        /*
                         * Consider the following layout:
                         * 
                         * ConnectionA (Root):
                         *      Samples
                         *          Sheboygan
                         *              Data
                         *                  *.FeatureSource
                         *              Layers
                         *                  *.LayerDefinition
                         *              Maps
                         *                  *.MapDefinition
                         * 
                         * ConnectionB (Root):
                         *      Foo
                         *      Bar
                         *          Snafu
                         * 
                         * These are the possible scenarios and outcomes:
                         * 
                         * Case 1 - Copy folder Samples/Sheboygan/Data into ConnectionB root:
                         * 
                         * Expect:
                         * 
                         * ConnectionB (Root):
                         *     Data
                         *          *.FeatureSource
                         *     Foo
                         *     Bar
                         *          Snafu
                         * 
                         * Case 2 - Copy Samples/Sheboygan/Data/*.FeatureSource into ConnectionB root:
                         * 
                         * Expect:
                         * 
                         * ConnectionB (Root):
                         *     *.FeatureSource
                         *     Foo
                         *     Bar
                         *          Snafu
                         *          
                         * Case 3 - Copy Samples/Sheboygan/Data/*.FeatureSource into Connection B/Foo:
                         * 
                         * Expect:
                         * 
                         * ConnectionB (Root):
                         *      Foo
                         *          *.FeatureSource
                         *      Bar
                         *          Snafu
                         *          
                         * Case 4 - Copy Samples/Sheboygan/Data into Connection B/Foo:
                         * 
                         * Expect:
                         * 
                         * ConnectionB (Root):
                         *      Foo
                         *          Data
                         *              *.FeatureSource
                         *      Bar
                         *          Snafu
                         *          
                         * Case 5 - Copy Samples/Sheboygan/Data into Connection B/Bar/Snafu:
                         * 
                         * Expect:
                         * 
                         * ConnectionB (Root):
                         *      Foo
                         *      Bar
                         *          Snafu
                         *              Data
                         *                  *.FeatureSource
                         * 
                         * Case 6 - Copy Samples/Sheboygan/Data/*.FeatureSource into Connection B/Bar/Snafu:
                         * 
                         * ConnectionB (Root):
                         *      Foo
                         *      Bar
                         *          Snafu
                         *              *.FeatureSource
                         * 
                         */


                        if (data.All(x => x.ResourceId.IsFolder))
                        {
                            if (data.Length > 1)
                            {
                                //folderId = GetCommonParent(data);
                                CopyResourcesToFolder(data, connectionName, folderId);
                            }
                            else
                            {
                                folderId += data.First().ResourceId.Name + "/";
                                CopyResourcesToFolder(data, connectionName, folderId);
                            }
                        }
                        else
                        {
                            CopyResourcesToFolder(data, connectionName, folderId);
                        }
                    }
                }
            }
        }

        internal string[] CopyResourcesToFolder(RepositoryHandle[] data, string targetConnectionName, string folderId)
        {
            string rootSourceParent = GetCommonParent(data);

            //There is an implicit assumption here that all items dropped come from the same connection
            var sourceConn = data.First().Connection;
            var targetConn = _connManager.GetConnection(targetConnectionName);
            var migrator = new ResourceMigrator(sourceConn, targetConn);

            //Collect all source ids
            var sourceIds = new List<string>();
            foreach (var resId in data.Select(x => x.ResourceId.ToString()))
            {
                if (ResourceIdentifier.IsFolderResource(resId))
                    sourceIds.AddRange(GetFullResourceList(sourceConn, resId));
                else
                    sourceIds.Add(resId);

            }

            var targets = new List<string>();
            foreach (var resId in sourceIds)
            {
                var dstId = resId.Replace(rootSourceParent, folderId);
                System.Diagnostics.Trace.TraceInformation("{0} => {1}", resId, dstId); //NOXLATE
                targets.Add(dstId);
            }

            bool overwrite = true;
            var existing = new List<string>();
            foreach (var resId in targets)
            {
                if (targetConn.ResourceService.ResourceExists(resId))
                {
                    existing.Add(resId);
                }
            }
            if (existing.Count > 0)
                overwrite = MessageService.AskQuestion(string.Format(Strings.PromptOverwriteOnTargetConnection, existing.Count));

            var wb = Workbench.Instance;
            var dlg = new ProgressDialog();
            var worker = new ProgressDialog.DoBackgroundWork((w, evt, args) =>
            {
                LengthyOperationProgressCallBack cb = (s, cbe) =>
                {
                    w.ReportProgress(cbe.Progress, cbe.StatusMessage);
                };

                return migrator.CopyResources(sourceIds.ToArray(), targets.ToArray(), overwrite, new RebaseOptions(rootSourceParent, folderId), cb);
            });

            var result = (string[])dlg.RunOperationAsync(wb, worker);
            RefreshModel(targetConn.DisplayName, folderId);
            ExpandNode(targetConn.DisplayName, folderId);
            return result; 
        }

        internal static string GetCommonParent(RepositoryHandle[] data)
        {
            if (data.Length > 0)
            {
                if (data.Length == 1)
                {
                    if (data[0].ResourceId.IsFolder)
                        return data[0].ResourceId.ToString();
                    else
                        return data[0].ResourceId.ParentFolder;
                }
                else
                {
                    int matches = 0;
                    string[] parts = data.First().ResourceId.ToString()
                                         .Substring(StringConstants.RootIdentifier.Length)
                                         .Split('/'); //NOXLATE
                    string test = StringConstants.RootIdentifier;
                    string parent = test;
                    int partIndex = 0;
                    //Use first one as a sample to see how far we can go. Keep going until we have
                    //a parent that doesn't match all of them. The one we recorded before then will
                    //be the common parent
                    while (true)
                    {
                        partIndex++;
                        if (partIndex > parts.Length) //Shouldn't happen, but just in case
                            break;

                        parent = test;

                        test = test + parts[partIndex - 1] + "/";
                        matches = data.Where(x => x.ResourceId.ResourceId.StartsWith(test)).Count();

                        if (matches > 0 && matches != data.Length)
                            break;
                    }
                    return parent;
                }
            }
            else
            {
                return StringConstants.RootIdentifier;
            }
        }

        internal static string GetCommonParent(RepositoryItem[] data)
        {
            if (data.Length > 0)
            {
                if (data.Length == 1)
                {
                    if (data[0].IsFolder)
                        return data[0].ResourceId.ToString();
                    else
                        return data[0].Parent.ResourceId;
                }
                else
                {
                    int matches = 0;
                    string[] parts = data.First().ResourceId.ToString()
                                         .Substring(StringConstants.RootIdentifier.Length)
                                         .Split('/'); //NOXLATE
                    string test = StringConstants.RootIdentifier;
                    string parent = test;
                    int partIndex = 0;
                    //Use first one as a sample to see how far we can go. Keep going until we have
                    //a parent that doesn't match all of them. The one we recorded before then will
                    //be the common parent
                    while (matches == data.Length)
                    {
                        parent = test;
                        partIndex++;
                        if (partIndex < parts.Length) //Shouldn't happen, but just in case
                            break;

                        test = test + parts[partIndex];
                        matches = data.Where(x => x.ResourceId.StartsWith(test)).Count();
                    }
                    return parent;
                }
            }
            else
            {
                return StringConstants.RootIdentifier;
            }
        }

        internal static IEnumerable<string> GetFullResourceList(IServerConnection sourceConn, string resId)
        {
            var list = sourceConn.ResourceService.GetRepositoryResources(resId, -1);
            foreach (var res in list.Children)
            {
                if (res.IsFolder)
                    continue;

                yield return res.ResourceId;
            }
        }

        private void trvResources_DragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
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

        private string [] MoveResourcesWithinConnection(string connectionName, ICollection<string> resIds, string folderId)
        {
            var wb = Workbench.Instance;
            var notMovedToTarget = new List<string>();
            var notMovedFromSource = new List<string>();
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var conn = _connManager.GetConnection(connectionName);

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
                        var target = folderId + rid.Name + "/"; //NOXLATE
                        conn.ResourceService.MoveResourceWithReferences(r, target, null, cb);
                    }
                    else
                    {
                        var rid = new ResourceIdentifier(r);
                        var target = folderId + rid.Name + "." + rid.Extension; //NOXLATE
                        if (omgr.IsOpen(r, conn))
                        {
                            notMovedFromSource.Add(r);
                            continue;
                        }

                        if (!omgr.IsOpen(target, conn))
                            conn.ResourceService.MoveResourceWithReferences(r, target, null, cb);
                        else
                            notMovedToTarget.Add(r);
                    }
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

            if (notMovedToTarget.Count > 0 || notMovedFromSource.Count > 0)
            {
                MessageService.ShowMessage(string.Format(
                    Strings.NotCopiedOrMovedDueToOpenEditors,
                    Environment.NewLine + string.Join(Environment.NewLine, notMovedToTarget.ToArray()) + Environment.NewLine,
                    Environment.NewLine + string.Join(Environment.NewLine, notMovedFromSource.ToArray()) + Environment.NewLine));
            }

            return new List<string>(affectedFolders).ToArray();
        }

        private void trvResources_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                new DeleteSelectedItemsCommand().Run();
            }
            else if (e.KeyCode == Keys.F2)
            {
                new RenameCommand().Run();
            }
        }

        private void trvResources_KeyDown(object sender, KeyEventArgs e)
        {
            //Note: Even though the attached context menu has the shortcuts specified
            //for Cut/Copy/Paste, I'm guessing the TreeViewAdv control is muffling the
            //event. Nevertheless this handler's got it covered and keeping those 
            //shortcuts there is useful as a visual reference, even if they don't work
            //the original way.

            //Note: We handle keydown when intercepting pressing the Control+C/X/V
            //because the keys are not actually released yet.
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        new CopyCommand().Run();
                        break;
                    case Keys.X:
                        new CutCommand().Run();
                        break;
                    case Keys.V:
                        new PasteCommand().Run();
                        break;
                }
            }
        }

        public string[] ConnectionNames
        {
            get { return _connManager.GetConnectionNames().ToArray(); }
        }
    }
}
