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
using Maestro.Shared.UI;
using ICSharpCode.Core.WinForms;
using System.Windows.Forms;
using Maestro.Base.Services;

namespace Maestro.Base
{
    /// <summary>
    /// Initializes the main window
    /// </summary>
    public class WorkbenchInitializer : IWorkbenchInitializer
    {
        /// <summary>
        /// Initializes a new instance of the WorkbenchInitializer class
        /// </summary>
        /// <param name="bStartMaximized"></param>
        public WorkbenchInitializer(bool bStartMaximized) { this.StartMaximized = bStartMaximized; }

        /// <summary>
        /// Gets whether the workbench will start fully maximized
        /// </summary>
        public bool StartMaximized { get; private set; }

        /// <summary>
        /// Gets the main window icon
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Icon GetIcon()
        {
            return Properties.Resources.MapGuide_Maestro;
        }

        /// <summary>
        /// Updates the status of the menu items
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="toolstrips"></param>
        public void UpdateMenuItemStatus(MenuStrip menu, IEnumerable<ToolStrip> toolstrips)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (item is IStatusUpdate)
                    (item as IStatusUpdate).UpdateStatus();
            }

            foreach (ToolStrip ts in toolstrips)
            {
                foreach (ToolStripItem item in ts.Items)
                {
                    if (item is IStatusUpdate)
                        (item as IStatusUpdate).UpdateStatus();
                }
            }
        }

        /// <summary>
        /// Gets the main menu
        /// </summary>
        /// <param name="workbench"></param>
        /// <returns></returns>
        public MenuStrip GetMainMenu(WorkbenchBase workbench)
        {
            var menu = new System.Windows.Forms.MenuStrip();
            MenuService.AddItemsToMenu(menu.Items, workbench, "/Maestro/Shell/MainMenu"); //NOXLATE
            return menu;
        }

        /// <summary>
        /// Gets the main toolstrip
        /// </summary>
        /// <param name="workbench"></param>
        /// <returns></returns>
        public ToolStrip GetMainToolStrip(WorkbenchBase workbench)
        {
            return ToolbarService.CreateToolStrip(workbench, "/Maestro/Shell/Toolbars/Main"); //NOXLATE
        }

        /// <summary>
        /// Gets the view content manager
        /// </summary>
        /// <returns></returns>
        public IViewContentManager GetViewContentManager()
        {
            return ServiceRegistry.GetService<ViewContentManager>();
        }

        /// <summary>
        /// Gets the close icon for documents
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image GetDocumentCloseIcon()
        {
            return Properties.Resources.cross_small;
        }
    }
}
