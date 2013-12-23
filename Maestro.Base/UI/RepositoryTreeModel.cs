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
using Aga.Controls.Tree;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Drawing;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Collections.ObjectModel;
using Maestro.Base.Services;

namespace Maestro.Base.UI
{
    /// <summary>
    /// Models an object in the repository
    /// </summary>
    public class RepositoryItem
    {
        private Dictionary<string, RepositoryItem> _children;

        private bool _notify = false;

        internal RepositoryItem(string connectionName, IRepositoryItem item)
        {
            _name = string.Empty;
            _children = new Dictionary<string, RepositoryItem>();

            this.ConnectionName = connectionName;
            this.CreatedDate = item.CreatedDate;
            this.ModifiedDate = item.ModifiedDate;
            this.Owner = item.Owner;
            this.ResourceId = item.ResourceId;
            this.ResourceType = item.ResourceType.ToString();
            this.Name = item.Name; //set name last because update logic requires other properties be set already
            

            if (this.IsRoot)
            {
                this.Icon = Properties.Resources.server;
            }
            else
            {
                switch (item.ResourceType)
                {
                    case ResourceTypes.DrawingSource:
                        this.Icon = Properties.Resources.blueprints;
                        break;
                    case ResourceTypes.FeatureSource:
                        this.Icon = Properties.Resources.database_share;
                        break;
                    case ResourceTypes.Folder:
                        this.Icon = Properties.Resources.folder_horizontal;
                        break;
                    case ResourceTypes.LayerDefinition:
                        this.Icon = Properties.Resources.layer;
                        break;
                    case ResourceTypes.MapDefinition:
                        this.Icon = Properties.Resources.map;
                        break;
                    case ResourceTypes.WebLayout:
                        this.Icon = Properties.Resources.application_browser;
                        break;
                    case ResourceTypes.ApplicationDefinition:
                        this.Icon = Properties.Resources.applications_stack;
                        break;
                    case ResourceTypes.SymbolLibrary:
                        this.Icon = Properties.Resources.images_stack;
                        break;
                    case ResourceTypes.PrintLayout:
                        this.Icon = Properties.Resources.printer;
                        break;
                    case ResourceTypes.SymbolDefinition:
                        this.Icon = Properties.Resources.marker;
                        break;
                    case ResourceTypes.WatermarkDefinition:
                        this.Icon = Properties.Resources.water;
                        break;
                    default:
                        this.Icon = Properties.Resources.document;
                        break;
                }
            }

            _notify = true;
        }

        /// <summary>
        /// Gets whether the specified child item (name) exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return _children.ContainsKey(name);
        }

        /// <summary>
        /// Gets the child RepositoryItem by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RepositoryItem this[string name]
        {
            get { return _children[name]; }
        }

        internal RepositoryTreeModel Model
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the parent of this item
        /// </summary>
        public RepositoryItem Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the children of this item
        /// </summary>
        public IEnumerable<RepositoryItem> Children
        {
            get { return _children.Values; }
        }

        /// <summary>
        /// Gets the qualified name of this item
        /// </summary>
        public string NameQualified
        {
            get 
            {
                if (this.ResourceType == ResourceTypes.Folder.ToString())
                    return this.Name;
                else
                    return this.Name + "." + this.ResourceType; //NOXLATE
            }
        }

        internal void AddChildWithoutNotification(RepositoryItem item)
        {
            item.Parent = this;
            if (!_children.ContainsKey(item.NameQualified))
                _children.Add(item.NameQualified, item);
        }

        internal void RemoveChildWithoutNotification(RepositoryItem item)
        {
            if (_children.ContainsKey(item.NameQualified) && item.Parent == this)
            {
                if (_children.Remove(item.NameQualified))
                {
                    item.Parent = null;
                }
            }
        }

        /// <summary>
        /// Adds the specified item as a child item
        /// </summary>
        /// <param name="item"></param>
        public void AddChild(RepositoryItem item)
        {
            item.Parent = this;
            _children.Add(item.NameQualified, item);
            NotifyStructureChanged(this);
        }

        /// <summary>
        /// Removes the child item
        /// </summary>
        /// <param name="item"></param>
        public void RemoveChild(RepositoryItem item)
        {
            if (_children.ContainsKey(item.NameQualified) && item.Parent == this)
            {
                if (_children.Remove(item.NameQualified))
                {
                    item.Parent = null;
                    NotifyStructureChanged(this);
                }
            }
        }

        private void NotifyStructureChanged(RepositoryItem repositoryItem)
        {
            if (!_notify)
                return;

            var model = FindModel();
            if (model != null && this.Parent != null)
            {
                TreePath path = model.GetPath(repositoryItem);
                if (path != null)
                {
                    var args = new TreePathEventArgs(path);
                    model.RaiseStructureChanged(args);
                }
            }
        }

        /// <summary>
        /// Gets whether this item is a root node
        /// </summary>
        public bool IsRoot
        {
            get { return this.ResourceId == StringConstants.RootIdentifier; }
        }

        /// <summary>
        /// Gets the resource id
        /// </summary>
        public string ResourceId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the associated connection
        /// </summary>
        public string ConnectionName
        {
            get;
            internal set;
        }

        private string _name;

        /// <summary>
        /// Gets the name of this resource
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                var oldq = this.NameQualified;
                _name = value;
                if (!oldq.Equals(this.NameQualified))
                {
                    if (IsRoot)
                        return;

                    string parentid = ResourceIdentifier.GetParentFolder(this.ResourceId);
                    this.ResourceId = parentid + this.NameQualified + ((IsFolder) ? "/" : string.Empty); //NOXLATE
                    NotifyNodesChanged();
                }
            }
        }

        /// <summary>
        /// Finds the first attached model
        /// </summary>
        /// <returns></returns>
        private RepositoryTreeModel FindModel()
        {
            RepositoryItem item = this;
            while (item != null)
            {
                if (item.Model != null)
                    return item.Model;
                item = item.Parent;
            }
            return null;
        }

        private void NotifyNodesChanged()
        {
            if (!_notify)
                return;

            var model = FindModel();
            if (model != null)
            {
                TreePath path = model.GetPath(this);
                if (path != null)
                {
                    var args = new TreeModelEventArgs(path, new object[] { this });
                    model.RaiseNodesChanged(args);
                }
            }
        }

        /// <summary>
        /// Gets the resource type
        /// </summary>
        public string ResourceType
        {
            get;
            internal set;
        }

        private string _owner;

        /// <summary>
        /// Gets or sets the resource owner
        /// </summary>
        public string Owner
        {
            get { return _owner; }
            internal set
            {
                _owner = value;
                NotifyNodesChanged();
            }
        }

        /// <summary>
        /// Gets the date this resource was created
        /// </summary>
        public DateTime CreatedDate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the modified date of this resource
        /// </summary>
        public DateTime ModifiedDate
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether this item is a folder
        /// </summary>
        public bool IsFolder
        {
            get { return this.ResourceId.EndsWith("/"); } //NOXLATE
        }

        /// <summary>
        /// Gets the icon for this item
        /// </summary>
        public Image Icon
        {
            get;
            internal set;
        }

        internal void ClearChildrenWithoutNotification()
        {
            _children.Clear();
        }

        private bool _open = false;
        private bool _dirty = false;
        //private bool _clipboarded = false;

        internal bool IsOpen
        {
            get { return _open; }
            set 
            { 
                _open = value;
                _dirty = !value;
                this.NotifyNodesChanged();
            }
        }

        internal bool IsDirty
        {
            get { return _dirty; }
            set 
            { 
                _dirty = value;
                _open = !value;
                this.NotifyNodesChanged();
            }
        }

        /// <summary>
        /// Defines valid clipboard actions for Site Explorer 
        /// </summary>
        public enum ClipboardAction
        {
            /// <summary>
            /// The node was copied
            /// </summary>
            Copy,
            /// <summary>
            /// The node was cut
            /// </summary>
            Cut,
            /// <summary>
            /// No clipboard action applied
            /// </summary>
            None
        }

        private ClipboardAction _action = ClipboardAction.None;
        
        /// <summary>
        /// Gets the clipboard state of this item
        /// </summary>
        public ClipboardAction ClipboardState
        {
            get { return _action; }
            set
            {
                _action = value;
                this.NotifyNodesChanged();
            }
        }

        internal void Reset()
        {
            _action = ClipboardAction.None;
            _dirty = false;
            _open = false;
            this.NotifyNodesChanged();
        }
    }

    /// <summary>
    /// Provides tooltips for resources in the Site Explorer
    /// </summary>
    public class RepositoryItemToolTipProvider : IToolTipProvider
    {
        /// <summary>
        /// Gets the tooltip
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeControl"></param>
        /// <returns></returns>
        public string GetToolTip(TreeNodeAdv node, Aga.Controls.Tree.NodeControls.NodeControl nodeControl)
        {
            RepositoryItem item = node.Tag as RepositoryItem;
            if (item != null && !item.IsRoot)
            {
                return string.Format(Strings.SITE_EXPLORER_TOOLTIP_TEMPLATE, Environment.NewLine, item.Name, item.ResourceType, item.CreatedDate, item.ModifiedDate, item.Owner);
            }
            return string.Empty;
        }
    }


    /// <summary>
    /// Defines the repository model for the treeview
    /// </summary>
    public class RepositoryTreeModel : TreeModelBase
    {
        private TreeViewAdv _tree;

        private ServerConnectionManager _connManager;
        private OpenResourceManager _openResMgr;
        private ClipboardService _clip;

        internal RepositoryTreeModel(ServerConnectionManager connManager, TreeViewAdv tree, OpenResourceManager openResMgr, ClipboardService clip)
        {
            _connManager = connManager;
            _tree = tree;
            _openResMgr = openResMgr;
            _clip = clip;
        }

        private System.Collections.IEnumerable GetSorted(string connectionName, ResourceList list)
        {
            //Sort them before returning them
            SortedList<string, RepositoryItem> folders = new SortedList<string, RepositoryItem>();
            SortedList<string, RepositoryItem> docs = new SortedList<string, RepositoryItem>();
            foreach (var item in list.Children)
            {
                var it = new RepositoryItem(connectionName, item);
                it.Model = this;
                if (it.IsFolder)
                    folders.Add(it.ResourceId, it);
                else
                    docs.Add(it.ResourceId, it);
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

        /// <summary>
        /// Restores node ui state from before refresh
        /// </summary>
        /// <param name="item"></param>
        private void ApplyCurrentItemState(RepositoryItem item)
        {
            var conn = _connManager.GetConnection(item.ConnectionName);
            if (_openResMgr.IsOpen(item.ResourceId, conn))
            {
                item.IsOpen = true;
                var ed = _openResMgr.GetOpenEditor(item.ResourceId, conn);
                if (ed.IsDirty)
                    item.IsDirty = true;
            }
            item.ClipboardState = _clip.GetClipboardState(item.ResourceId);
        }

        private Dictionary<string, RepositoryItem> _rootNodes = new Dictionary<string, RepositoryItem>();

        /// <summary>
        /// Gets the child nodes in the given tree path
        /// </summary>
        /// <param name="treePath"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                _rootNodes.Clear();
                var roots = new List<RepositoryItem>();
                foreach (var connName in _connManager.GetConnectionNames())
                {
                    if (_rootNodes.ContainsKey(connName))
                        continue;

                    var conn = _connManager.GetConnection(connName);

                    var list = conn.ResourceService.GetRepositoryResources(StringConstants.RootIdentifier, 0); //NOXLATE
                    if (list.Items.Count != 1)
                    {
                        throw new InvalidOperationException(); //Huh?
                    }
                    var connNode = new RepositoryItem(connName, (IRepositoryItem)list.Items[0]);
                    Debug.Assert(connNode.Parent == null);
                    Debug.Assert(connNode.IsRoot);
                    connNode.Name = connName;
                    connNode.Model = this;

                    if (!_rootNodes.ContainsKey(connName))
                    {
                        _rootNodes[connName] = connNode;
                        roots.Add(connNode);
                    }
                }

                foreach (var r in roots)
                {
                    yield return r;
                }
            }
            else
            {
                var node = treePath.LastNode as RepositoryItem;
                if (node != null && node.IsFolder) //Can't enumerate children of documents
                {
                    string connName = GetParentConnectionName(node);
                    var conn = _connManager.GetConnection(connName);
                    node.ClearChildrenWithoutNotification();
                    var list = conn.ResourceService.GetRepositoryResources(node.ResourceId, string.Empty, 1, false); //NOXLATE
                    foreach (RepositoryItem item in GetSorted(connName, list))
                    {
                        node.AddChildWithoutNotification(item);
                        ApplyCurrentItemState(item);
                        Debug.Assert(item.Parent != null);
                        Debug.Assert(!item.IsRoot);
                        yield return item;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        internal static string GetParentConnectionName(RepositoryItem item)
        {
            if (!string.IsNullOrEmpty(item.ConnectionName))
                return item.ConnectionName;

            var current = item.Parent;
            if (current != null)
            {
                current = item.Parent;
                while (current != null)
                {
                    current = current.Parent;
                }
                Debug.Assert(!string.IsNullOrEmpty(current.ConnectionName));
                return current.ConnectionName;
            }
            else 
            {
                Debug.Assert(!string.IsNullOrEmpty(item.ConnectionName));
                return item.ConnectionName;
            }
        }

        /// <summary>
        /// Refreshes this model
        /// </summary>
        public override void Refresh()
        {
            //We have to override this because the base impl does not 
            //preserve the tree path of the selected node (and thus expand
            //all the nodes from the root to this node)
            //
            //Which is also why we need to pass a reference to 
            //the TreeViewAdv in the ctor
            var selected = _tree.GetPath(_tree.SelectedNode);
            OnStructureChanged(new TreePathEventArgs(selected));
        }

        internal void FullRefresh()
        {
            OnStructureChanged(new TreePathEventArgs());
        }

        /// <summary>
        /// Gets whether the specified tree path is a path to a leaf node
        /// </summary>
        /// <param name="treePath"></param>
        /// <returns></returns>
        public override bool IsLeaf(TreePath treePath)
        {
            return !((RepositoryItem)treePath.LastNode).IsFolder;
        }

        internal void RaiseNodesChanged(TreeModelEventArgs args)
        {
            base.OnNodesChanged(args);
        }

        internal TreePath GetPath(RepositoryItem node)
        {
            if (node.IsRoot)
            {
                return new TreePath(node);
            }
            else
            {
                Stack<object> stack = new Stack<object>();
                while (!node.IsRoot)
                {
                    stack.Push(node);
                    node = node.Parent;
                }
                stack.Push(node);
                return new TreePath(stack.ToArray());
            }
        }

        internal void RaiseStructureChanged(TreePathEventArgs args)
        {
            base.OnStructureChanged(args);
        }

        internal TreePath GetPathFromResourceId(string connectionName, string resId)
        {
            var rootNode = _rootNodes[connectionName];
            if (StringConstants.RootIdentifier.Equals(resId))
                return GetPath(rootNode);

            string[] components = ResourceIdentifier.GetPath(resId).Split('/'); //NOXLATE
            if (!ResourceIdentifier.IsFolderResource(resId))
            {
                //Fix extension to last component
                components[components.Length - 1] = components[components.Length - 1] + "." + ResourceIdentifier.GetResourceType(resId).ToString();
            }
            RepositoryItem current = rootNode;
            for (int i = 0; i < components.Length; i++)
            {
                if (current.Contains(components[i]))
                    current = current[components[i]];
                else
                    return null;
            }
            return GetPath(current);
        }
    }
}
