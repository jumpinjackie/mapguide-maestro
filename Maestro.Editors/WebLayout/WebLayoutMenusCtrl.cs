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
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class WebLayoutMenusCtrl : EditorBindableCollapsiblePanel
    {
        public WebLayoutMenusCtrl()
        {
            InitializeComponent();
        }

        private IWebLayout _wl;
        private BindingList<CommandDecorator> _cmds = new BindingList<CommandDecorator>();
        private Dictionary<string, CommandDecorator> _cmdsByName = new Dictionary<string, CommandDecorator>();

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
                var dec = new CommandDecorator(cmd);
                _cmds.Add(dec);
                _cmdsByName[dec.Name] = dec;
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
            if (_cmdsByName.ContainsKey(cmd.Name))
            {
                var dec = _cmdsByName[cmd.Name];
                _cmds.Remove(dec);
                _cmdsByName.Remove(dec.Name);
            }
        }

        void OnCommandAdded(ICommand cmd)
        {
            var dec = new CommandDecorator(cmd);
            _cmds.Add(dec);
            _cmdsByName[dec.Name] = dec;
        }

        private void grdCommands_DragLeave(object sender, EventArgs e)
        {
            ICommand cmd = GetSelectedCommand();

            if (cmd != null)
                grdCommands.DoDragDrop(cmd, DragDropEffects.All);
        }

        private ICommand GetSelectedCommand()
        {
            CommandDecorator cmd = null;
            if (grdCommands.SelectedRows.Count == 1)
                cmd = grdCommands.SelectedRows[0].DataBoundItem as CommandDecorator;
            else if (grdCommands.SelectedCells.Count == 1)
                cmd = grdCommands.Rows[grdCommands.SelectedCells[0].RowIndex].DataBoundItem as CommandDecorator;
            return cmd.DecoratedInstance;
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
            else if (grdCommands.SelectedRows.Count == 1)
            {
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
