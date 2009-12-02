#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.Maestro.ResourceEditors;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceBrowser
{
    /// <summary>
    /// This class stores a local copy of all loaded resources in a tree like structure
    /// </summary>
    public class RepositoryCache
    {
        private ServerConnectionI m_connection;
        public ServerConnectionI Connection 
        { 
            get { return m_connection; }
            set
            {
                m_connection = value;
                Reset();
            }
        }

        /// <summary>
        /// The library root
        /// </summary>
        private ResourceFolder m_root = null;
        private Dictionary<string, ResourceFolder> m_folders;
        private ResourceEditorMap m_editors;

        /// <summary>
        /// The editor map has images and various helper functions
        /// </summary>
        public ResourceEditorMap EditorMap { get { return m_editors; } }
        /// <summary>
        /// This marker is used on treenodes to indicate that the parent node has not loaded
        /// </summary>
        public static readonly object NOT_LOADED_MARKER = new object();

        /// <summary>
        /// An event that is triggered when the cache is reset
        /// </summary>
        public event EventHandler CacheResetEvent;


        public RepositoryCache(ServerConnectionI connection, ResourceEditorMap editors)
        {
            m_connection = connection;
            m_editors = editors;
        }

        /// <summary>
        /// Reset the current cache contents, reloading anything from the server again
        /// </summary>
        public void Reset()
        {
            ResourceListResourceFolder tmp = new ResourceListResourceFolder();
            tmp.ResourceId = "Library://";
            m_root = new ResourceFolder(this, tmp);
            m_folders = new Dictionary<string, ResourceFolder>();
            RegisterFolder(m_root);
            if (CacheResetEvent != null)
                CacheResetEvent(this, new EventArgs());
        }

        /// <summary>
        /// Builds a treeview with nodes
        /// </summary>
        /// <param name="tree">The tree to update with new nodes</param>
        public void BuildTree(TreeView tree)
        {
            tree.Nodes.Clear();
            TreeNode rootnode = new TreeNode(m_connection.DisplayName, m_editors.ServerIcon, m_editors.ServerIcon);
            tree.Nodes.Add(rootnode);

            TreeNode dummy = new TreeNode("dummy");
            dummy.Tag = NOT_LOADED_MARKER;
            rootnode.Nodes.Add(dummy);

            if (m_root == null)
                Reset();

            rootnode.Expand();
            if (rootnode.Nodes.Count == 1 && rootnode.Nodes[0] == dummy)
                BuildNode(rootnode, false);
        }

        /// <summary>
        /// Registers the folder as loaded for quick lookup
        /// </summary>
        /// <param name="folder">The loaded folder</param>
        private void RegisterFolder(ResourceFolder folder)
        {
            m_folders[folder.ResourceId] = folder;
        }

        /// <summary>
        /// Unregisters the folder as loaded
        /// </summary>
        /// <param name="folder">The unloaded folder</param>
        private void UnregisterFolder(ResourceFolder folder)
        {
            m_folders.Remove(folder.ResourceId);
        }

        /// <summary>
        /// Builds the childnodes of a node
        /// </summary>
        /// <param name="node">The node to update</param>
        /// <param name="forceRebuild">True to re-read from the server, false to allow usage of the cache, if avalible</param>
        public void BuildNode(TreeNode node, bool forceRebuild)
        {
            ResourceFolder folder = FindFolder(node);

            if (forceRebuild || !folder.IsLoaded)
                folder.ReloadContents();
            
            folder.RebuildNode(node);
        }

        /// <summary>
        /// Gets a list of documents present at the current node
        /// </summary>
        /// <param name="node">The treenode to return the documents for</param>
        /// <returns>A list of documents at the present node</returns>
        public List<ResourceListResourceDocument> GetDocuments(TreeNode node)
        {
            return FindFolder(node).Documents;
        }

        /// <summary>
        /// Gets a list of documents present at the current node
        /// </summary>
        /// <param name="resId">The resourceId to return the documents for</param>
        /// <returns>A list of documents at the present node</returns>
        public List<ResourceListResourceDocument> GetDocuments(string resId)
        {
            return FindFolder(resId).Documents;
        }

        private ResourceFolder FindFolder(TreeNode node)
        {
            string resId = null; ;
            if (node.Parent == null)
                resId = "Library://";
            else if (node.Tag is ResourceListResourceFolder)
                resId = (node.Tag as ResourceListResourceFolder).ResourceId;

            return FindFolder(resId);
        }

        private ResourceFolder FindFolder(string resId)
        {
            if (!string.IsNullOrEmpty(resId))
            {
                if (m_folders.ContainsKey(resId))
                    return m_folders[resId];
                else
                    throw new Exception(Strings.RepositoryCache.UnknownNodeInternalError);
            }
            else
                throw new Exception(Strings.RepositoryCache.InvalidNodeInternalError);
        }

        /// <summary>
        /// Helper function to determine if the folder exists, trying to determine by cache use, then fallback to query the server.
        /// </summary>
        /// <param name="resId">The resource to determine existance for</param>
        /// <returns>A value indicating if the folder exists</returns>
        public bool FolderExists(string resId)
        {
            if (m_folders.ContainsKey(resId))
                return true;
            else if (m_folders.ContainsKey(new ResourceIdentifier(resId).ParentFolder))
            {
                ResourceFolder f = m_folders[new ResourceIdentifier(resId).ParentFolder];
                if (f.IsLoaded)
                    return false;
                else
                {
                    f.ReloadContents();
                    return m_folders.ContainsKey(resId);
                }
            }

            return m_connection.HasFolder(resId);
        }


        /// <summary>
        /// Helper function to determine if the resource document exists, trying to determine by cache use, then fallback to query the server.
        /// </summary>
        /// <param name="resId">The resource to determine existance for</param>
        /// <returns>A value indicating if the resource document exists</returns>
        public bool ResourceExists(string resId)
        {
            string parentFolder = new ResourceIdentifier(resId).ParentFolder;

            if (m_folders.ContainsKey(parentFolder))
            {
                ResourceFolder f = m_folders[parentFolder];
                foreach (ResourceListResourceDocument doc in f.Documents)
                    if (doc.ResourceId == resId)
                        return true;
                
                return false;
            }
            else
                return m_connection.ResourceExists(resId);
        }

        private class ResourceFolder
        {
            RepositoryCache m_parent;
            private ResourceListResourceFolder m_item;
            private List<ResourceListResourceDocument> m_documents = null;
            private List<ResourceFolder> m_folders = null;
            public List<ResourceListResourceDocument> Documents 
            { 
                get 
                {
                    if (m_documents == null)
                        ReloadContents();
                    return m_documents; 
                } 
            }

            public ResourceFolder(RepositoryCache parent, ResourceListResourceFolder item)
            {
                m_parent = parent;
                m_item = item;
            }

            public void ReloadContents()
            {
                if (m_folders != null)
                    foreach (ResourceFolder f in m_folders)
                        f.Unload();

                m_documents = new List<ResourceListResourceDocument>();
                m_folders = new List<ResourceFolder>();

                foreach (object o in m_parent.Connection.GetRepositoryResources(m_item.ResourceId, null, 1, false).Items)
                    if (o is ResourceListResourceFolder)
                        m_folders.Add(new ResourceFolder(m_parent, o as ResourceListResourceFolder));
                    else if (o is ResourceListResourceDocument)
                        m_documents.Add(o as ResourceListResourceDocument);

                foreach (ResourceFolder f in m_folders)
                    m_parent.RegisterFolder(f);

            }

            public void Unload()
            {
                if (m_folders != null)
                    foreach (ResourceFolder f in m_folders)
                        f.Unload();

                m_parent.UnregisterFolder(this);
            }

            public void RebuildNode(TreeNode node)
            {
                if (m_folders == null)
                    ReloadContents();

                node.Nodes.Clear();
                foreach (ResourceFolder folder in m_folders)
                {
                    TreeNode n = new TreeNode();
                    n.Text = m_parent.EditorMap.GetResourceNameFromResourceID(folder.ResourceId);
                    n.Tag = folder.m_item;
                    n.ImageIndex = n.SelectedImageIndex = m_parent.EditorMap.FolderIcon;
                    n.ToolTipText = string.Format(Strings.RepositoryCache.ResourceTooltip, new MaestroAPI.ResourceIdentifier(folder.ResourceId.Substring(0, folder.ResourceId.Length - 1) + ".Folder").Name, Strings.RepositoryCache.FolderName, folder.m_item.CreatedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture), folder.m_item.ModifiedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture));

                    TreeNode marker = new TreeNode("dummy");
                    marker.Tag = NOT_LOADED_MARKER;
                    n.Nodes.Add(marker);

                    node.Nodes.Add(n);
                }

                if (node.TreeView is ResourceTree && (node.TreeView as ResourceTree).HideDocuments)
                    return;

                foreach (ResourceListResourceDocument document in m_documents)
                {
                    TreeNode n = new TreeNode();
                    n.Text = m_parent.EditorMap.GetResourceNameFromResourceID(document.ResourceId);
                    n.Tag = document;
                    n.ImageIndex = n.SelectedImageIndex = m_parent.EditorMap.GetImageIndexFromResourceID(document.ResourceId);

                    n.ToolTipText = string.Format(Strings.RepositoryCache.ResourceTooltip, new MaestroAPI.ResourceIdentifier(document.ResourceId).Name, new MaestroAPI.ResourceIdentifier(document.ResourceId).Extension, document.CreatedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture), document.ModifiedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture));

                    if (new MaestroAPI.ResourceIdentifier(document.ResourceId).Extension == "LayerDefinition" || new MaestroAPI.ResourceIdentifier(document.ResourceId).Extension == "FeatureSource")
                    {
                        bool published = false;
                        string serviceType = new MaestroAPI.ResourceIdentifier(document.ResourceId).Extension == "LayerDefinition" ? "WMS" : "WFS";
                        if (document.ResourceDocumentHeader != null && document.ResourceDocumentHeader.Metadata != null && document.ResourceDocumentHeader.Metadata.Simple != null && document.ResourceDocumentHeader.Metadata.Simple.Property["_IsPublished"] == "1")
                            published = true;

                        n.ToolTipText += "\r\n" + string.Format(Strings.RepositoryCache.PublishedTooltip, serviceType, published);
                    }

                    node.Nodes.Add(n);
                }
            }

            public string Tooltip
            {
                get
                {
                    return "";
                }
            }

            public string Name
            {
                get
                {
                    return "";
                }
            }

            public string ResourceId
            {
                get { return m_item.ResourceId; }
            }

            public bool IsLoaded { get { return m_folders != null; } }
        }
    }
}
