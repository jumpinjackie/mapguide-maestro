#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Drawing;
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Initializes the main window
    /// </summary>
    public interface IWorkbenchInitializer
    {
        /// <summary>
        /// Gets whether to start the workbench maximized
        /// </summary>
        bool StartMaximized { get; }
        /// <summary>
        /// Gets the main window icon
        /// </summary>
        /// <returns></returns>
        Icon GetIcon();
        /// <summary>
        /// Gets the main menu
        /// </summary>
        /// <param name="workbench"></param>
        /// <returns></returns>
        MenuStrip GetMainMenu(WorkbenchBase workbench);
        /// <summary>
        /// Gets the main toolstrip
        /// </summary>
        /// <param name="workbench"></param>
        /// <returns></returns>
        ToolStrip GetMainToolStrip(WorkbenchBase workbench);
        /// <summary>
        /// Updates the status of the menu items
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="toolstrips"></param>
        void UpdateMenuItemStatus(MenuStrip menu, IEnumerable<ToolStrip> toolstrips);
        /// <summary>
        /// Gets the view content manager
        /// </summary>
        /// <returns></returns>
        IViewContentManager GetViewContentManager();
        /// <summary>
        /// Gets the close icon for documents
        /// </summary>
        /// <returns></returns>
        Image GetDocumentCloseIcon();
    }
}
