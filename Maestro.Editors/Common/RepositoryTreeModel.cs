#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.Common
{
    class DummyNode : TreeNode
    {

    }

    internal class RepositoryModelItem
    {
        private IRepositoryItem _item;

        public RepositoryModelItem(IRepositoryItem item)
        {
            _item = item;
            if (this.IsRoot)
            {
                this.ImageIndex = RepositoryIcons.RES_ROOT;
            }
            else
            {
                if (this.IsFolder)
                    this.ImageIndex = RepositoryIcons.RES_FOLDER;
                else
                    this.ImageIndex = RepositoryIcons.GetImageIndexForResourceType(_item.ResourceType);
            }
        }

        public int ImageIndex { get; private set; }

        public string Name { get { return _item.Name; } }

        public string ResourceId { get { return _item.ResourceId; } }

        public bool HasChildren { get { return _item.HasChildren; } }

        public bool IsFolder
        {
            get { return _item.IsFolder; }
        }

        public bool IsRoot
        {
            get { return this.ResourceId == StringConstants.RootIdentifier; }
        }

        public IRepositoryItem Instance { get { return _item; } }
    }

    internal class RepositoryFolderTreeModel
    {
        internal event EventHandler ItemSelected;

        private IResourceService _resSvc;
        private TreeView _tree;

        public RepositoryModelItem SelectedItem
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

        private bool _bOmitEmptyFolders;

        private bool _bFoldersOnly;

        public bool FoldersOnly { get { return _bFoldersOnly; } }

        public bool OmitEmptyFolders { get { return _bOmitEmptyFolders; } }

        public RepositoryFolderTreeModel(IResourceService resSvc, TreeView tree, bool bFoldersOnly, bool bOmitEmptyFolders)
        {
            _resSvc = resSvc;
            _tree = tree;
            _bFoldersOnly = bFoldersOnly;
            _bOmitEmptyFolders = bOmitEmptyFolders;

            _tree.AfterExpand += new TreeViewEventHandler(OnNodeAfterExpand);
            _tree.AfterSelect += new TreeViewEventHandler(OnNodeAfterSelect);

            _tree.Nodes.Clear();
            InitRoot();
        }

        private void InitRoot()
        {
            StartUpdate();
            foreach (RepositoryModelItem item in GetChildren(null))
            {
                var node = CreateNode(item);
                _tree.Nodes.Add(node);
            }
            EndUpdate();
        }

        void OnNodeAfterSelect(object sender, TreeViewEventArgs e)
        {
            RepositoryModelItem item = (RepositoryModelItem)e.Node.Tag;
            SetSelectedItem(item);
        }

        private void SetSelectedItem(RepositoryModelItem item)
        {
            this.SelectedItem = item;
            var handler = this.ItemSelected;
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
            RepositoryModelItem item = (RepositoryModelItem)nodeToUpdate.Tag;
            if (IsNodeNotPopulated(nodeToUpdate))
                nodeToUpdate.Nodes.Clear();

            if (item.HasChildren && nodeToUpdate.Nodes.Count == 0)
            {
                StartUpdate();
                foreach (RepositoryModelItem child in GetChildren(item))
                {
                    var node = CreateNode(child);
                    nodeToUpdate.Nodes.Add(node);
                }
                EndUpdate();
            }
        }

        private static TreeNode CreateNode(RepositoryModelItem item)
        {
            var node = new TreeNode();
            node.Name = item.Name;
            node.Text = item.Name;
            node.Tag = item;
            node.ImageIndex = node.SelectedImageIndex = item.ImageIndex;
            if (item.IsFolder)
                node.Nodes.Add(new DummyNode());
            return node;
        }

        private System.Collections.IEnumerable GetSorted(ResourceList list)
        {
            //Sort them before returning them
            SortedList<string, RepositoryModelItem> folders = new SortedList<string, RepositoryModelItem>();
            SortedList<string, RepositoryModelItem> docs = new SortedList<string, RepositoryModelItem>();
            foreach (var item in list.Children)
            {
                if (item.IsFolder && !item.HasChildren && this.OmitEmptyFolders)
                    continue;

                if (item.IsFolder)
                    folders.Add(item.ResourceId, new RepositoryModelItem(item));
                else if (!HasFilteredTypes() || (HasFilteredTypes() && IsFilteredType(item.ResourceType)))
                    docs.Add(item.ResourceId, new RepositoryModelItem(item));

            }
            foreach (var folder in folders.Values)
            {
                yield return folder;
            }
            foreach (var doc in docs.Values)
            {
                yield return doc;
            }
        }

        private HashSet<ResourceTypes> _filteredTypes = new HashSet<ResourceTypes>();

        public void AddResourceTypeFilter(ResourceTypes rt) { _filteredTypes.Add(rt); }

        public void ClearResourceTypeFilters() { _filteredTypes.Clear(); }

        public bool HasFilteredTypes() { return _filteredTypes.Count > 0; }

        public bool IsFilteredType(ResourceTypes rt) { return _filteredTypes.Contains(rt); }

        public System.Collections.IEnumerable GetChildren(RepositoryModelItem item)
        {
            if (item == null)
            {
                var list = _resSvc.GetRepositoryResources(StringConstants.RootIdentifier, 0);
                return GetSorted(list);
            }
            else
            {
                if (item.HasChildren)
                {
                    var list = _resSvc.GetRepositoryResources(item.ResourceId, _bFoldersOnly ? ResourceTypes.Folder.ToString() : "", 1, true); //NOXLATE
                    return GetSorted(list);
                }
                else
                {
                    return new RepositoryModelItem[0];
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
                var item = (RepositoryModelItem)currentNode.Tag;
                if (folderId.Equals(item.ResourceId))
                {
                    _tree.SelectedNode = currentNode;
                    SetSelectedItem(item);
                    return;
                }
                nodeList = currentNode.Nodes;
            }

            foreach (TreeNode node in nodeList)
            {
                var folder = (RepositoryModelItem)node.Tag;
                if (folderId.StartsWith(folder.ResourceId))
                {
                    UpdateNode(node);
                    node.Expand();
                    NavigateTo(folderId, node);
                    break;
                }
            }
        }

        private TreeNode FindNode(string folderId, TreeNode currentNode)
        {
            TreeNodeCollection nodeList = null;

            if (currentNode == null)
            {
                nodeList = _tree.Nodes;
            }
            else
            {
                var item = (RepositoryModelItem)currentNode.Tag;
                if (folderId.Equals(item.ResourceId))
                {
                    return currentNode;
                }
                nodeList = currentNode.Nodes;
            }

            foreach (TreeNode node in nodeList)
            {
                var folder = (RepositoryModelItem)node.Tag;
                if (folderId.StartsWith(folder.ResourceId))
                {
                    UpdateNode(node);
                    node.Expand();
                    return FindNode(folderId, node);
                }
            }

            return null;
        }

        internal void Refresh(string folderId)
        {
            if (string.IsNullOrEmpty(folderId) || folderId == StringConstants.RootIdentifier)
            {
                InitRoot();
            }
            else
            {
                var node = FindNode(folderId, null);
                if (node != null)
                {
                    node.Nodes.Clear();
                    UpdateNode(node);
                }
            }
        }
    }
}
