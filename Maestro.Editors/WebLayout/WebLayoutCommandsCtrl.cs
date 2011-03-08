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
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Editors.WebLayout
{
    [ToolboxItem(true)]
    internal partial class WebLayoutCommandsCtrl : EditorBindableCollapsiblePanel
    {
        public WebLayoutCommandsCtrl()
        {
            InitializeComponent();
            _commands = new BindingList<ICommand>();
            grdCommands.DataSource = _commands;
        }

        private IWebLayout _wl;
        private IEditorService _edsvc;

        private BindingList<ICommand> _commands;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            _wl = (IWebLayout)_edsvc.GetEditedResource();
            //Populate command set
            foreach (var cmd in _wl.CommandSet.Commands)
            {
                _commands.Add(cmd);
            }
            _commands.ListChanged += OnCommandSetListChanged;
        }

        protected override void UnsubscribeEventHandlers()
        {
            _commands.ListChanged -= OnCommandSetListChanged;

            base.UnsubscribeEventHandlers();
        }

        private bool listChangedDisabled = false;

        void OnCommandSetListChanged(object sender, ListChangedEventArgs e)
        {
            if (listChangedDisabled)
                return;

            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        _wl.CommandSet.AddCommand(_commands[e.NewIndex]);
                    }
                    break;
            }
            OnResourceChanged();
        }

        private ICommand[] GetCustomCommands()
        {
            var cmds = new List<ICommand>();
            foreach (var c in _wl.CommandSet.Commands)
            {
                var type = c.GetType();
                if (typeof(IInvokeUrlCommand).IsAssignableFrom(type))
                {
                    cmds.Add(c);
                }
                else if (typeof(IInvokeScriptCommand).IsAssignableFrom(type))
                {
                    cmds.Add(c);
                }
                else if (typeof(ISearchCommand).IsAssignableFrom(type))
                {
                    cmds.Add(c);
                }
            }
            return cmds.ToArray();
        }

        private ICommand[] GetSelectedCustomCommands()
        {
            var cmds = new List<ICommand>();
            foreach (DataGridViewRow row in grdCommands.SelectedRows)
            {
                if (typeof(IInvokeScriptCommand).IsAssignableFrom(row.DataBoundItem.GetType()))
                {
                    cmds.Add((ICommand)row.DataBoundItem);
                }
                else if (typeof(IInvokeUrlCommand).IsAssignableFrom(row.DataBoundItem.GetType()))
                {
                    cmds.Add((ICommand)row.DataBoundItem);
                }
                else if (typeof(ISearchCommand).IsAssignableFrom(row.DataBoundItem.GetType()))
                {
                    cmds.Add((ICommand)row.DataBoundItem);
                }
            }
            return cmds.ToArray();
        }

        private void EvaluateCommands()
        {
            var customCmds = GetCustomCommands();
            var selectedCmds = GetSelectedCustomCommands();
            btnDelete.Enabled = selectedCmds.Length == 1;
            btnExport.Enabled = customCmds.Length > 0;
        }

        private void invokeURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cmd = _wl.CreateInvokeUrlCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Properties.Resources.InvokeUrlCmdDescription;
            //_wl.CommandSet.AddCommand(cmd);
            _commands.Add(cmd);
        }

        private void invokeScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cmd = _wl.CreateInvokeScriptCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Properties.Resources.InvokeScriptCmdDescription;
            //_wl.CommandSet.AddCommand(cmd);
            _commands.Add(cmd);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_wl.Map.ResourceId))
            {
                MessageBox.Show(Properties.Resources.SpecifyMapForWebLayout);
                return;
            }

            var cmd = _wl.CreateSearchCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Properties.Resources.SearchCmdDescription;
            //_wl.CommandSet.AddCommand(cmd);
            _commands.Add(cmd);
        }

        private void grdCommands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = grdCommands.Rows[e.RowIndex];
                row.Selected = true;

                ClearCommandUI();

                var cmdCtrl = new CustomCommandPropertyCtrl();
                cmdCtrl.Dock = DockStyle.Fill;
                cmdCtrl.Bind((ICommand)row.DataBoundItem, _edsvc);

                grpCommand.Controls.Add(cmdCtrl);

                EvaluateCommands();
            }
        }

        private void ClearCommandUI()
        {
            foreach (Control ctrl in grpCommand.Controls)
            {
                ctrl.Dispose();
            }
            grpCommand.Controls.Clear();
        }

        private static bool Ask(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdCommands.SelectedRows.Count == 1)
            {
                var iurl = grdCommands.SelectedRows[0].DataBoundItem as IInvokeUrlCommand;
                var iscr = grdCommands.SelectedRows[0].DataBoundItem as IInvokeScriptCommand;
                var srch = grdCommands.SelectedRows[0].DataBoundItem as ISearchCommand;

                WebLayoutRegion[] regions;
                if (iurl != null)
                {
                    if (_wl.IsCommandReferenced(iurl.Name, out regions))
                    {
                        if (!Ask(Properties.Resources.DeleteCommand, string.Format(Properties.Resources.PromptDeleteCommand, GetAsString(regions, ", "))))
                            return;
                    }

                    using (new WaitCursor(this))
                    {
                        _wl.CommandSet.RemoveCommand(iurl);
                        _commands.Remove(iurl);
                        int deleted = _wl.RemoveAllReferences(iurl.Name);
                        ClearCommandUI();
                    }
                }
                else if (iscr != null)
                {
                    if (_wl.IsCommandReferenced(iscr.Name, out regions))
                    {
                        if (!Ask(Properties.Resources.DeleteCommand, string.Format(Properties.Resources.PromptDeleteCommand, GetAsString(regions, ", "))))
                            return;
                    }

                    using (new WaitCursor(this))
                    {
                        _wl.CommandSet.RemoveCommand(iscr);
                        _commands.Remove(iurl);
                        _wl.RemoveAllReferences(iscr.Name);
                        ClearCommandUI();
                    }
                }
                else if (srch != null)
                {
                    if (_wl.IsCommandReferenced(srch.Name, out regions))
                    {
                        if (!Ask(Properties.Resources.DeleteCommand, string.Format(Properties.Resources.PromptDeleteCommand, GetAsString(regions, ", "))))
                            return;
                    }

                    using (new WaitCursor(this))
                    {
                        _wl.CommandSet.RemoveCommand(srch);
                        _commands.Remove(iurl);
                        _wl.RemoveAllReferences(srch.Name);
                        ClearCommandUI();
                    }
                }
            }
        }

        private static string GetAsString(WebLayoutRegion[] regions, string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < regions.Length; i++)
            {
                sb.Append(regions[i].ToString());
                if (i < regions.Length - 1)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (grdCommands.SelectedRows.Count == 0)
            {
                MessageBox.Show(Properties.Resources.ExportNoCommandsSelected);
                return;
            }
            else
            {
                List<string> selectedCmds = new List<string>();
                foreach (DataGridViewRow row in grdCommands.SelectedRows)
                {
                    var cmd = (ICommand)row.DataBoundItem;
                    var cmdType = cmd.GetType();
                    if (typeof(IInvokeScriptCommand).IsAssignableFrom(cmdType))
                        selectedCmds.Add(cmd.Name);
                    else if (typeof(IInvokeUrlCommand).IsAssignableFrom(cmdType))
                        selectedCmds.Add(cmd.Name);
                    else if (typeof(ISearchCommand).IsAssignableFrom(cmdType))
                        selectedCmds.Add(cmd.Name);
                }

                if (selectedCmds.Count == 0)
                {
                    MessageBox.Show(Properties.Resources.ExportNoCustomCommandsSelected);
                    return;
                }

                using (var save = DialogFactory.SaveFile())
                {
                    save.Filter = Properties.Resources.FilterXml;
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        _wl.ExportCustomCommands(save.FileName, selectedCmds.ToArray());
                        MessageBox.Show(string.Format(Properties.Resources.CustomCommandsExported, save.FileName));
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    listChangedDisabled = true;
                    try
                    {
                        var cmds = _wl.ImportCustomCommands(open.FileName);
                        StringBuilder sb = new StringBuilder();

                        foreach (var imported in cmds)
                        {
                            ICommand cmd = _wl.GetCommandByName(imported.ImportedName);
                            _commands.Add(cmd);

                            if (imported.NameChanged)
                                sb.AppendLine(imported.ToString());
                        }

                        MessageBox.Show(string.Format(Properties.Resources.CustomCommandsImported, cmds.Length, open.FileName, sb.ToString()));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Properties.Resources.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        listChangedDisabled = false;
                    }
                }
            }
        }

        private void grdCommands_SelectionChanged(object sender, EventArgs e)
        {
            EvaluateCommands();
        }
    }
}
