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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Security.AccessControl;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// A generic dialog for selecting folders or resource documents
    /// </summary>
    public partial class ResourcePicker : Form
    {
        const int RES_UNKNOWN = 0;
        const int RES_FEATURESOURCE = 1;
        const int RES_LAYERDEFINITION = 2;
        const int RES_MAPDEFINITION = 3;
        const int RES_WEBLAYOUT = 4;
        const int RES_SYMBOLLIBRARY = 5;
        const int RES_PRINTLAYOUT = 6;
        const int RES_DRAWINGSOURCE = 7;
        const int RES_APPLICATIONDEFINITION = 8;

        private ResourceTypes[] _resTypes;

        private ResourcePicker()
        {
            InitializeComponent();
            _resTypes = new ResourceTypes[] 
            {
                ResourceTypes.ApplicationDefinition,
                ResourceTypes.DrawingSource,
                ResourceTypes.FeatureSource,
                ResourceTypes.Folder,
                ResourceTypes.LayerDefinition,
                ResourceTypes.LoadProcedure,
                ResourceTypes.MapDefinition,
                ResourceTypes.PrintLayout,
                ResourceTypes.SymbolDefinition,
                ResourceTypes.SymbolLibrary,
                ResourceTypes.WebLayout
            };
            cmbResourceFilter.DataSource = _resTypes;
        }

        private IResourceService _resSvc;

        private bool _resourceMode = false;

        private RepositoryFolderTreeModel _model;

        /// <summary>
        /// Constructs a new instance. Use this overload to select any resource type. If only
        /// folder selection is desired, set <see cref="SelectFoldersOnly"/> to true before
        /// showing the dialog
        /// </summary>
        /// <param name="resSvc">The res SVC.</param>
        /// <param name="mode">The mode.</param>
        public ResourcePicker(IResourceService resSvc, ResourcePickerMode mode)
            : this()
        {
            _resSvc = resSvc;
            _model = new RepositoryFolderTreeModel(_resSvc, trvFolders);
            _model.FolderSelected += OnFolderSelected;
            this.UseFilter = true;
            this.Mode = mode;
        }

        void OnFolderSelected(object sender, EventArgs e)
        {
            UpdateDocumentList();
        }

        /// <summary>
        /// Sets the starting point.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        public void SetStartingPoint(string folderId)
        {
            if (!ResourceIdentifier.IsFolderResource(folderId))
                throw new ArgumentException(string.Format(Properties.Resources.NotAFolder, folderId));
            _model.NavigateTo(folderId);
        }

        private ResourcePickerMode _mode = ResourcePickerMode.OpenResource;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public ResourcePickerMode Mode
        {
            get { return _mode; }
            private set
            {
                _mode = value;
                switch (_mode)
                {
                    case ResourcePickerMode.OpenFolder:
                        {
                            this.Text = Properties.Resources.SelectFolder;
                            this.SelectFoldersOnly = true;
                        } 
                        break;
                    case ResourcePickerMode.SaveResource:
                        {
                            this.Text = Properties.Resources.SaveResource;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a new instance. Use this overload to select only resources of a specific type.
        /// You cannot select folders in this mode. Attempting to set <see cref="SelectFoldersOnly"/> to
        /// true will throw an <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="resSvc">The res SVC.</param>
        /// <param name="resFilter">The res filter.</param>
        /// <param name="mode">The mode.</param>
        public ResourcePicker(IResourceService resSvc, ResourceTypes resFilter, ResourcePickerMode mode)
            : this(resSvc, mode)
        {
            if (mode == ResourcePickerMode.OpenFolder)
                throw new InvalidOperationException(string.Format(Properties.Resources.ModeNotAllowed, mode));

            this.Filter = resFilter;
            this.UseFilter = true;

            _resourceMode = true;
            cmbResourceFilter.Enabled = false;
        }

        /// <summary>
        /// Gets or sets the resource filter. If a filter value is specified, browsing
        /// is locked to that particular resource type, otherwise all resource type can be
        /// selected
        /// </summary>
        public ResourceTypes Filter
        {
            get { return (ResourceTypes)cmbResourceFilter.SelectedItem; }
            set
            {
                if (Array.IndexOf<ResourceTypes>(_resTypes, value) < 0)
                    throw new InvalidOperationException("Cannot use specified resource type as filter: " + value); //LOCALIZE

                cmbResourceFilter.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to use a resource filter. If set to false, when selecting a folder
        /// all resource types are returned, otherwise only children of the specified type are returned
        /// </summary>
        internal bool UseFilter
        {
            get { return cmbResourceFilter.Visible; }
            set 
            {
                if (value && this.SelectFoldersOnly)
                    throw new InvalidOperationException("Cannot specify a filter when SelectFoldersOnly is true"); //LOCALIZE
                cmbResourceFilter.Visible = value; lblFilter.Visible = value; 
            }
        }
        
        /// <summary>
        /// Gets or sets whether to select folders only. If true, the document view is disabled and 
        /// <see cref="UseFilter"/> is set to false
        /// </summary>
        private bool SelectFoldersOnly
        {
            get { return splitContainer1.Panel2Collapsed; }
            set 
            {
                if (_resourceMode && value)
                    throw new InvalidOperationException("Cannot specify to select folders when dialog is initialized with a resource filter"); //LOCALIZE

                if (value)
                    this.UseFilter = false;

                splitContainer1.Panel2Collapsed = value;
                resIdComponentPanel.Visible = !value;
                if (value)
                    txtResourceId.Text = string.Empty;
            }
        }

        /// <summary>
        /// Gets the resource id of the selected item
        /// </summary>
        public string ResourceID
        {
            get { return txtResourceId.Text; }
        }

        private void UpdateResourceId()
        {
            btnOK.Enabled = false;
            if (this.SelectFoldersOnly)
            {
                txtResourceId.Text = txtFolder.Text;
                if (!string.IsNullOrEmpty(txtFolder.Text) && ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                {
                    btnOK.Enabled = true;
                }
            }
            else
            {
                txtResourceId.Text = txtFolder.Text + txtName.Text + "." + cmbResourceFilter.SelectedItem.ToString();
                if (!ResourceIdentifier.IsFolderResource(txtResourceId.Text) && !string.IsNullOrEmpty(txtName.Text))
                {
                    btnOK.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_mode == ResourcePickerMode.SaveResource)
            {
                if (ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                {
                    MessageBox.Show(Properties.Resources.InvalidResourceIdFolder);
                    return;
                }
                else
                {
                    if (!ResourceIdentifier.Validate(txtResourceId.Text))
                    {
                        MessageBox.Show(Properties.Resources.InvalidResourceId);
                        return;
                    }
                    else
                    {
                        if (ResourceIdentifier.GetResourceType(txtResourceId.Text) != (ResourceTypes)cmbResourceFilter.SelectedItem)
                        {
                            MessageBox.Show(Properties.Resources.InvalidResourceIdNotSpecifiedType);
                            return;
                        }
                    }

                    if (_resSvc.ResourceExists(txtResourceId.Text))
                    {
                        if (MessageBox.Show(Properties.Resources.OverwriteResource, Properties.Resources.SaveResource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void UpdateDocumentList()
        {   
            if (_model != null)
            {
                RepositoryFolder folder = _model.SelectedFolder;
                if (folder != null)
                {
                    txtFolder.Text = folder.ResourceId;

                    if (!this.SelectFoldersOnly)
                    {
                        ResourceList list = null;
                        if (!this.UseFilter)
                            list = _resSvc.GetRepositoryResources(folder.ResourceId, 1);
                        else
                            list = _resSvc.GetRepositoryResources(folder.ResourceId, this.Filter.ToString(), 1);

                        PopulateDocumentList(list);
                    }
                }
            }
        }

        private void PopulateDocumentList(ResourceList list)
        {
            lstResources.Clear();
            foreach (var item in list.Items)
            {
                var doc = item as ResourceListResourceDocument;
                if (doc != null)
                {
                    var li = new ListViewItem(doc.Name);
                    li.Tag = doc;

                    try
                    {
                        var rt = ResourceIdentifier.GetResourceType(doc.ResourceId);
                        switch (rt)
                        {
                            case ResourceTypes.ApplicationDefinition:
                                li.ImageIndex = RES_APPLICATIONDEFINITION;
                                break;
                            case ResourceTypes.DrawingSource:
                                li.ImageIndex = RES_DRAWINGSOURCE;
                                break;
                            case ResourceTypes.FeatureSource:
                                li.ImageIndex = RES_FEATURESOURCE;
                                break;
                            case ResourceTypes.LayerDefinition:
                                li.ImageIndex = RES_LAYERDEFINITION;
                                break;
                            case ResourceTypes.MapDefinition:
                                li.ImageIndex = RES_MAPDEFINITION;
                                break;
                            case ResourceTypes .PrintLayout:
                                li.ImageIndex = RES_PRINTLAYOUT;
                                break;
                            case ResourceTypes.SymbolLibrary:
                                li.ImageIndex = RES_SYMBOLLIBRARY;
                                break;
                            case ResourceTypes.WebLayout:
                                li.ImageIndex = RES_WEBLAYOUT;
                                break;
                            default:
                                li.ImageIndex = RES_UNKNOWN;
                                break;
                        }
                    }
                    catch
                    {
                        li.ImageIndex = RES_UNKNOWN;
                    }

                    lstResources.Items.Add(li);
                }
            }
        }

        private void lstResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResources.SelectedItems.Count == 1)
            {
                var item = lstResources.SelectedItems[0];
                var doc = item.Tag as ResourceListResourceDocument;
                if (doc != null)
                {
                    txtName.Text = ResourceIdentifier.GetName(doc.ResourceId);
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
        }

        private void cmbResourceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
            UpdateDocumentList();
        }

        private void lstResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = lstResources.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                var doc = item.Tag as ResourceListResourceDocument;
                if (doc != null)
                {
                    txtName.Text = ResourceIdentifier.GetName(doc.ResourceId);
                    btnOK.PerformClick();
                }
            }
        }
    }

    /// <summary>
    /// Defines the various modes this resource picker can be in
    /// </summary>
    public enum ResourcePickerMode
    {
        /// <summary>
        /// 
        /// </summary>
        OpenResource,
        /// <summary>
        /// 
        /// </summary>
        SaveResource,
        /// <summary>
        /// 
        /// </summary>
        OpenFolder
    }

    internal class RepositoryFolder
    {
        private IRepositoryItem _item;

        public RepositoryFolder(IRepositoryItem item)
        {
            _item = item;
        }

        public string Name { get { return _item.Name; } }

        public string ResourceId { get { return _item.ResourceId; } }

        public bool HasChildren { get { return _item.HasChildren; } }

        public Image Icon
        {
            get
            {
                if (IsRoot)
                {
                    return Properties.Resources.server;
                }
                else
                {
                    return Properties.Resources.folder_horizontal;
                }
            }
        }

        public bool IsRoot
        {
            get { return this.ResourceId == "Library://"; }
        }
    }

    internal class RepositoryFolderTreeModel
    {
        class DummyNode : TreeNode
        {

        }

        internal event EventHandler FolderSelected;

        private IResourceService _resSvc;
        private TreeView _tree;

        public RepositoryFolder SelectedFolder
        {
            get;
            private set;
        }

        private void StartUpdate()
        {
            _tree.BeginUpdate();
            _tree.Cursor = Cursors.WaitCursor;
        }

        private void EndUpdate()
        {
            _tree.EndUpdate();
            _tree.Cursor = Cursors.Default;
        }

        public RepositoryFolderTreeModel(IResourceService resSvc, TreeView tree)
        {
            _resSvc = resSvc;
            _tree = tree;

            _tree.AfterExpand += new TreeViewEventHandler(OnNodeAfterExpand);
            _tree.AfterSelect += new TreeViewEventHandler(OnNodeAfterSelect);

            StartUpdate();
            foreach (RepositoryFolder folder in GetChildren(null))
            {
                var node = CreateNode(folder);
                _tree.Nodes.Add(node);
            }
            EndUpdate();
        }

        void OnNodeAfterSelect(object sender, TreeViewEventArgs e)
        {
            RepositoryFolder folder = (RepositoryFolder)e.Node.Tag;
            SetSelectedFolder(folder);
        }

        private void SetSelectedFolder(RepositoryFolder folder)
        {
            this.SelectedFolder = folder;
            var handler = this.FolderSelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        bool IsNodeNotPopulated(TreeNode node)
        {
            return node.Nodes.Count == 1 && node.Nodes[0].GetType() == typeof(DummyNode);
        }

        void OnNodeAfterExpand(object sender, TreeViewEventArgs e)
        {
            UpdateNode(e.Node);
        }

        private void UpdateNode(TreeNode nodeToUpdate)
        {
            RepositoryFolder folder = (RepositoryFolder)nodeToUpdate.Tag;
            if (IsNodeNotPopulated(nodeToUpdate))
                nodeToUpdate.Nodes.Clear();

            if (folder.HasChildren && nodeToUpdate.Nodes.Count == 0)
            {
                StartUpdate();
                foreach (RepositoryFolder f in GetChildren(folder))
                {
                    var node = CreateNode(f);
                    nodeToUpdate.Nodes.Add(node);
                }
                EndUpdate();
            }
        }

        const int IDX_SERVER = 0;
        const int IDX_FOLDER = 1;

        private static TreeNode CreateNode(RepositoryFolder folder)
        {
            var node = new TreeNode();
            node.Name = folder.Name;
            node.Text = folder.Name;
            node.Tag = folder;
            node.ImageIndex = node.SelectedImageIndex = folder.IsRoot ? IDX_SERVER : IDX_FOLDER;
            node.Nodes.Add(new DummyNode());
            return node;
        }

        private System.Collections.IEnumerable GetSorted(ResourceList list)
        {
            //Sort them before returning them
            SortedList<string, RepositoryFolder> folders = new SortedList<string, RepositoryFolder>();
            foreach (var item in list.Children)
            {
                if (item.IsFolder)
                    folders.Add(item.Name, new RepositoryFolder(item));
            }
            foreach (var folder in folders.Values)
            {
                yield return folder;
            }
        }

        public System.Collections.IEnumerable GetChildren(RepositoryFolder folder)
        {
            if (folder == null)
            {
                var list = _resSvc.GetRepositoryResources("Library://", 0);
                return GetSorted(list);
            }
            else
            {
                if (folder.HasChildren)
                {
                    var list = _resSvc.GetRepositoryResources(folder.ResourceId, ResourceTypes.Folder.ToString(), 1, true);
                    return GetSorted(list);
                }
                else
                {
                    return new RepositoryFolder[0];
                }
            }
        }

        internal void NavigateTo(string folderId)
        {
            NavigateTo(folderId, null);
        }

        internal void NavigateTo(string folderId, TreeNode currentNode)
        {
            TreeNodeCollection nodeList = null;

            if (currentNode == null)
            {
                nodeList = _tree.Nodes;
            }
            else
            {
                var folder = (RepositoryFolder)currentNode.Tag;
                if (folderId.Equals(folder.ResourceId))
                {
                    _tree.SelectedNode = currentNode;
                    SetSelectedFolder(folder);
                    return;
                }
                nodeList = currentNode.Nodes;
            }

            foreach (TreeNode node in nodeList)
            {
                var folder = (RepositoryFolder)node.Tag;
                if (folderId.StartsWith(folder.ResourceId))
                {
                    UpdateNode(node);
                    node.Expand();
                    NavigateTo(folderId, node);
                    break;
                }
            }
        }
    }
}
