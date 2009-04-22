#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceBrowser
{
    /// <summary>
    /// A common treeview control to display the contents of the server repository
    /// </summary>
    public partial class ResourceTree : TreeView
    {
        private RepositoryCache m_cache;
        private bool m_hideDocuments = false;

        /// <summary>
        /// Constructs a new tree
        /// </summary>
        public ResourceTree()
            : base()
        {
            base.BeforeExpand += new TreeViewCancelEventHandler(ResourceTree_BeforeExpand);
            this.TreeViewNodeSorter = new NodeSorter();
        }

        /// <summary>
        /// Constructs a new tree
        /// </summary>
        /// <param name="cache">The cache to use when building the tree</param>
        public ResourceTree(RepositoryCache cache)
            : this()
        {
            this.Cache = cache;
        }

        /// <summary>
        /// Gets or sets the current cache. Setting this value will cause the tree to refresh
        /// </summary>
        [Browsable(false)]
        public RepositoryCache Cache
        {
            get
            {
                return m_cache;
            }
            set
            {
                this.Nodes.Clear();
                m_cache = value;
                if (m_cache != null)
                {
                    this.ImageList = m_cache.EditorMap.SmallImageList;

                    m_cache.BuildTree(this);
                }
                else
                    this.Nodes.Clear();
            }
        }

        private void ResourceTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (m_cache != null)
                m_cache.BuildNode(e.Node, false);
        }

        /// <summary>
        /// Rebuild the contents of a particular node
        /// </summary>
        /// <param name="n">The node to refresh</param>
        public void RebuildNode(TreeNode n)
        {
            if (n.Tag is ResourceListResourceDocument)
                n = FindNode(new ResourceIdentifier((n.Tag as ResourceListResourceDocument).ResourceId).RepositoryPath);
                
            m_cache.BuildNode(n, true);
        }

        /// <summary>
        /// Rebuild the contents of a particular node
        /// </summary>
        /// <param name="n">The resourceId of the folder node to refresh</param>
        public void RebuildNode(string path)
        {
            RebuildNode(FindNode(path));
        }

        /// <summary>
        /// Gets or sets a value indicating if documents are displayed
        /// </summary>
        [Browsable(true), Description("Determines if the tree only displays folders or folders and documents"), Category("Behavior"), DefaultValue(false)]
        public bool HideDocuments
        {
            get { return m_hideDocuments; }
            set 
            { 
                m_hideDocuments = value;
                if (m_cache != null && this.Nodes.Count > 0)
                    RefreshTreeNodes();
            }
        }

        private TreeNode FindNode(string resourceId)
        {
            Queue<TreeNodeCollection> unexamined = new Queue<TreeNodeCollection>();
            unexamined.Enqueue(this.Nodes);

            while (unexamined.Count > 0)
            {
                TreeNodeCollection c = unexamined.Dequeue();
                foreach (TreeNode n in c)
                {
                    if (n.Tag is ResourceListResourceFolder && (n.Tag as ResourceListResourceFolder).ResourceId == resourceId)
                        return n;
                    else if (n.Tag is ResourceListResourceDocument && (n.Tag as ResourceListResourceDocument).ResourceId == resourceId)
                        return n;

                    if (n.Nodes.Count > 0)
                        unexamined.Enqueue(n.Nodes);
                }

            }

            return null;

        }

        /// <summary>
        /// Gets a list of documents at from the currently selected folder node
        /// </summary>
        /// <returns>A list of documents</returns>
        public List<ResourceListResourceDocument> GetDocuments()
        {
            if (this.SelectedNode == null)
                return new List<ResourceListResourceDocument>();
            return GetDocuments(this.SelectedNode);
        }
        /// <summary>
        /// Gets a list of documents at from the given folder node
        /// </summary>
        /// <param name="node">The node to return the documents for</param>
        /// <returns>A list of documents</returns>

        public List<ResourceListResourceDocument> GetDocuments(TreeNode node)
        {
            string resId = null;
            if (node.Parent == null)
                resId = "Library://";
            else if (node.Tag is ResourceListResourceFolder)
                resId = (node.Tag as ResourceListResourceFolder).ResourceId;

            if (resId == null)
                return new List<ResourceListResourceDocument>();
            else
            {
                List<ResourceListResourceDocument> docs = m_cache.GetDocuments(resId);
                docs.Sort(new DocumentSorter());
                return docs;
            }
        }

        /// <summary>
        /// Refreshes the entire tree, keeps as many folders open as there were before the refresh, and attempts to re-select the selected node after the refesh
        /// </summary>
        public void RefreshTreeNodes()
        {
            if (m_cache == null)
                throw new Exception("You must set the cache property before using the tree");

            string prevSelected = null;
            if (this.SelectedNode != null && this.SelectedNode.Tag != null)
                if (this.SelectedNode.Tag is ResourceListResourceFolder)
                    prevSelected = (this.SelectedNode.Tag as ResourceListResourceFolder).ResourceId;
                else if (this.SelectedNode.Tag is ResourceListResourceDocument)
                    prevSelected = (this.SelectedNode.Tag as ResourceListResourceDocument).ResourceId;

            List<string> opennodes = new List<string>();
            FindOpenNodes(this.Nodes, opennodes);

            m_cache.BuildTree(this);

            foreach (string s in opennodes)
            {
                TreeNode n = FindNode(s);
                if (n != null)
                    n.Expand();
            }

            SelectClosestParent(prevSelected);
        }

        /// <summary>
        /// Selects the node that is the closest parent to the given resourceId
        /// </summary>
        /// <param name="resourceId"></param>
        public void SelectClosestParent(string resourceId)
        {
            if (!string.IsNullOrEmpty(resourceId))
            {
                string endString = ResourceIdentifier.GetRepository(resourceId);

                while (resourceId != endString && this.SelectedNode == null)
                {
                    TreeNode n = FindNode(resourceId);
                    if (n == null)
                        resourceId = new ResourceIdentifier(resourceId).ParentFolder;
                    else
                        this.SelectedNode = n;
                }
            }
        }

        private void FindOpenNodes(TreeNodeCollection nodes, List<string> items)
        {
            foreach (TreeNode n in nodes)
                if (n.IsExpanded)
                {
                    if (n.Tag is OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)
                        items.Add((n.Tag as OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder).ResourceId);
                    FindOpenNodes(n.Nodes, items);
                }
        }

    }
}