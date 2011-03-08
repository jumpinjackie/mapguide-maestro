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
using Maestro.Shared.UI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using Aga.Controls.Tree;
using System.Diagnostics;

namespace Maestro.Editors.WebLayout
{
    [ToolboxItem(true)]
    internal partial class WebLayoutMenusCtrl : EditorBindableCollapsiblePanel
    {
        public WebLayoutMenusCtrl()
        {
            InitializeComponent();
        }

        private IWebLayout _wl;
        private BindingList<ICommand> _cmds = new BindingList<ICommand>();

        public override void Bind(IEditorService service)
        {
            _wl = (IWebLayout)service.GetEditedResource();
            _wl.CommandSet.CustomCommandAdded += OnCommandAdded;
            _wl.CommandSet.CustomCommandRemoved += OnCommandRemoved;

            edContextMenu.Bind(service, _wl, _wl.ContextMenu);
            edTaskMenu.Bind(service, _wl, _wl.TaskPane.TaskBar);
            edToolbar.Bind(service, _wl, _wl.ToolBar);

            foreach (var cmd in _wl.CommandSet.Commands)
            {
                _cmds.Add(cmd);
            }
            grdCommands.DataSource = _cmds;
        }

        protected override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();

            _wl.CommandSet.CustomCommandAdded -= OnCommandAdded;
            _wl.CommandSet.CustomCommandRemoved -= OnCommandRemoved;
        }

        void OnCommandRemoved(ICommand cmd)
        {
            _cmds.Remove(cmd);
        }

        void OnCommandAdded(ICommand cmd)
        {
            _cmds.Add(cmd);
        }

        private void grdCommands_DragLeave(object sender, EventArgs e)
        {
            ICommand cmd = GetSelectedCommand();

            if (cmd != null)
                grdCommands.DoDragDrop(cmd, DragDropEffects.All);
        }

        private ICommand GetSelectedCommand()
        {
            ICommand cmd = null;
            if (grdCommands.SelectedRows.Count == 1)
                cmd = grdCommands.SelectedRows[0].DataBoundItem as ICommand;
            else if (grdCommands.SelectedCells.Count == 1)
                cmd = grdCommands.Rows[grdCommands.SelectedCells[0].RowIndex].DataBoundItem as ICommand;
            return cmd;
        }

        private void grdCommands_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grdCommands.SelectedCells.Count == 1)
            {
                var cell = grdCommands.SelectedCells[0];
                grdCommands.ClearSelection();
                grdCommands.Rows[cell.RowIndex].Selected = true;
                btnAddFromCmdSet.Enabled = true;
            }
            else
            {
                btnAddFromCmdSet.Enabled = false;
            }
        }

        private void btnAddFromCmdSet_Click(object sender, EventArgs e)
        {
            var cmd = GetSelectedCommand();
            if (cmd != null && tabMenus.SelectedIndex >= 0)
            {
                var tab = tabMenus.TabPages[tabMenus.SelectedIndex];
                if (tab == TAB_CONTEXT_MENU)
                    edContextMenu.AddCommand(cmd);
                else if (tab == TAB_TASK_MENU)
                    edTaskMenu.AddCommand(cmd);
                else if (tab == TAB_TOOLBAR)
                    edToolbar.AddCommand(cmd);
            }
        }
    }
}
