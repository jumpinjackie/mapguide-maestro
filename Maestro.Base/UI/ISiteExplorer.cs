﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maestro.Base.UI
{
    /// <summary>
    /// Represents the Site Explorer
    /// </summary>
    public interface ISiteExplorer : IViewContent
    {
        /// <summary>
        /// Raised when the active connection has changed
        /// </summary>
        event EventHandler ActiveConnectionChanged;

        /// <summary>
        /// The name of the active <see cref="IServerConnection"/> associated with this site explorer. The active connection
        /// is the connection whose node or child nodes is currently selected
        /// </summary>
        string ConnectionName { get; }

        /// <summary>
        /// Gets the array of connection names.
        /// </summary>
        string[] ConnectionNames { get; }

        /// <summary>
        /// Performs a full refresh of the tree model
        /// </summary>
        void FullRefresh();

        /// <summary>
        /// Refreshes the tree model
        /// </summary>
        /// <param name="connectionName">The name of the connection</param>
        void RefreshModel(string connectionName);

        /// <summary>
        /// Refreshes the tree model from the specified resource id
        /// </summary>
        /// <param name="connectionName">The name of the connection</param>
        /// <param name="resId"></param>
        void RefreshModel(string connectionName, string resId);

        /// <summary>
        /// Expands the node indicated by the specified id
        /// </summary>
        /// <param name="connectionName">The name of the connection</param>
        /// <param name="folderId"></param>
        void ExpandNode(string connectionName, string folderId);

        /// <summary>
        /// Selects the node indicated by the specified id
        /// </summary>
        /// <param name="connectionName">The name of the connection</param>
        /// <param name="resourceId"></param>
        void SelectNode(string connectionName, string resourceId);

        /// <summary>
        /// Flags the node indicated by the specified action
        /// </summary>
        /// <param name="connectionName">The name of the connection</param>
        /// <param name="resourceId"></param>
        /// <param name="action"></param>
        void FlagNode(string connectionName, string resourceId, NodeFlagAction action);

        /// <summary>
        /// Gets the items currently selected
        /// </summary>
        ISiteExplorerNode[] SelectedItems { get; }

        /// <summary>
        /// Raised when the selected item(s) changes
        /// </summary>
        event RepositoryItemEventHandler ItemsSelected;
    }

    public static class ExtensionMethods
    {
        public static IEnumerable<RepositoryItem> GetSelectedResources(this ISiteExplorer exp)
        {
            return exp.SelectedItems.OfType<RepositoryItem>();
        }
    }

    /// <summary>
    /// An EventArgs that carries RepositoryItem instances
    /// </summary>
    public class RepositoryItemEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of RepositoryItemEventArgs
        /// </summary>
        /// <param name="items"></param>
        public RepositoryItemEventArgs(ISiteExplorerNode[] items)
        {
            this.Items = items;
        }

        /// <summary>
        /// Gets the affected site explorer items
        /// </summary>
        public ISiteExplorerNode[] Items { get; }
    }

    /// <summary>
    /// Defines a method to handle item selection in the Site Explorer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RepositoryItemEventHandler(object sender, RepositoryItemEventArgs e);

    /// <summary>
    /// Defines a set of possible actions that can be performed on nodes in the Site Explorer
    /// </summary>
    public enum NodeFlagAction
    {
        /*
        /// <summary>
        /// Indicate that the node has been cut and placed on the clipboard
        /// </summary>
        IndicateCut,
        /// <summary>
        /// Indicate that the node has been copied and placed on the clipboard
        /// </summary>
        IndicateCopy,*/

        /// <summary>
        /// Highlight the affected node with a pre-defined back color to indicate open
        /// </summary>
        HighlightOpen,

        /// <summary>
        /// Highlight the affected node with a pre-defined back color to indicate dirty state
        /// </summary>
        HighlightDirty,

        /// <summary>
        /// Reset node to default styles
        /// </summary>
        None
    }
}