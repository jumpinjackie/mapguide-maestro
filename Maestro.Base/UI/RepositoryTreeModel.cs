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

        public RepositoryItem(IRepositoryItem item)
        {
            _name = string.Empty;
            _children = new Dictionary<string, RepositoryItem>();

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
                    default:
                        this.Icon = Properties.Resources.document;
                        break;
                }
            }

            _notify = true;
        }

        public bool Contains(string name)
        {
            return _children.ContainsKey(name);
        }

        public RepositoryItem this[string name]
        {
            get { return _children[name]; }
        }

        internal RepositoryTreeModel Model
        {
            get;
            set;
        }

        public RepositoryItem Parent
        {
            get;
            private set;
        }

        public IEnumerable<RepositoryItem> Children
        {
            get { return _children.Values; }
        }

        public string NameQualified
        {
            get 
            {
                if (this.ResourceType == ResourceTypes.Folder.ToString())
                    return this.Name;
                else
                    return this.Name + "." + this.ResourceType; 
            }
        }

        internal void AddChildWithoutNotification(RepositoryItem item)
        {
            item.Parent = this;
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

        public void AddChild(RepositoryItem item)
        {
            item.Parent = this;
            _children.Add(item.NameQualified, item);
            NotifyStructureChanged(this);
        }

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

        public bool IsRoot
        {
            get { return this.ResourceId == "Library://"; }
        }

        public string ResourceId
        {
            get;
            internal set;
        }

        private string _name;

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
                    this.ResourceId = parentid + this.NameQualified + ((IsFolder) ? "/" : "");
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

        public string ResourceType
        {
            get;
            internal set;
        }

        private string _owner;

        public string Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                NotifyNodesChanged();
            }
        }

        public DateTime CreatedDate
        {
            get;
            internal set;
        }

        public DateTime ModifiedDate
        {
            get;
            internal set;
        }

        public bool IsFolder
        {
            get { return this.ResourceId.EndsWith("/"); }
        }

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

        public enum ClipboardAction
        {
            Copy,
            Cut,
            None
        }

        private ClipboardAction _action = ClipboardAction.None;
        
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

    public class RepositoryItemToolTipProvider : IToolTipProvider
    {
        public string GetToolTip(TreeNodeAdv node, Aga.Controls.Tree.NodeControls.NodeControl nodeControl)
        {
            RepositoryItem item = node.Tag as RepositoryItem;
            if (item != null && !item.IsRoot)
            {
                return string.Format(Properties.Resources.SITE_EXPLORER_TOOLTIP_TEMPLATE, Environment.NewLine, item.Name, item.ResourceType, item.CreatedDate, item.ModifiedDate, item.Owner);
            }
            return string.Empty;
        }
    }


    /// <summary>
    /// Defines the repository model for the treeview
    /// </summary>
    public class RepositoryTreeModel : TreeModelBase
    {
        private RepositoryItem _rootNode;
        private TreeViewAdv _tree;
        private string _connectionName;

        private IServerConnection _conn;
        private OpenResourceManager _openResMgr;
        private ClipboardService _clip;

        public RepositoryTreeModel(IServerConnection conn, TreeViewAdv tree, string connName, OpenResourceManager openResMgr, ClipboardService clip)
        {
            _conn = conn;
            _tree = tree;
            _connectionName = connName;
            _openResMgr = openResMgr;
            _clip = clip;
        }

        private System.Collections.IEnumerable GetSorted(ResourceList list)
        {
            //Sort them before returning them
            SortedList<string, RepositoryItem> folders = new SortedList<string, RepositoryItem>();
            SortedList<string, RepositoryItem> docs = new SortedList<string, RepositoryItem>();
            foreach (var item in list.Children)
            {
                var it = new RepositoryItem(item);
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
            if (_openResMgr.IsOpen(item.ResourceId))
            {
                item.IsOpen = true;
                var ed = _openResMgr.GetOpenEditor(item.ResourceId);
                if (ed.IsDirty)
                    item.IsDirty = true;
            }
            item.ClipboardState = _clip.GetClipboardState(item.ResourceId);
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                var list = _conn.ResourceService.GetRepositoryResources("Library://", 0);
                if (list.Items.Count != 1)
                {
                    throw new InvalidOperationException(); //Huh?
                }
                _rootNode = new RepositoryItem((IRepositoryItem)list.Items[0]);
                _rootNode.Name = _connectionName;
                _rootNode.Model = this;
                yield return _rootNode;
            }
            else
            {
                var node = treePath.LastNode as RepositoryItem;
                if (node != null && node.IsFolder) //Can't enumerate children of documents
                {
                    node.ClearChildrenWithoutNotification();
                    var list = _conn.ResourceService.GetRepositoryResources(node.ResourceId, "", 1, false);
                    foreach (RepositoryItem item in GetSorted(list))
                    {
                        node.AddChildWithoutNotification(item);
                        ApplyCurrentItemState(item);
                        yield return item;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

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
            if (node == _rootNode)
            {
                return new TreePath(node);
            }
            else
            {
                Stack<object> stack = new Stack<object>();
                while (node != _rootNode)
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

        internal TreePath GetPathFromResourceId(string resId)
        {
            if ("Library://".Equals(resId))
                return GetPath(_rootNode);
            
            string[] components = ResourceIdentifier.GetPath(resId).Split('/');
            if (!ResourceIdentifier.IsFolderResource(resId))
            {
                //Fix extension to last component
                components[components.Length - 1] = components[components.Length - 1] + "." + ResourceIdentifier.GetResourceType(resId).ToString();
            }
            RepositoryItem current = _rootNode;
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
