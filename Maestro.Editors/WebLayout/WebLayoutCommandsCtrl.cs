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
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class WebLayoutCommandsCtrl : EditorBindableCollapsiblePanel
    {
        public WebLayoutCommandsCtrl()
        {
            InitializeComponent();
            _commands = new BindingList<CommandDecorator>();
            grdCommands.DataSource = _commands;
        }

        private IWebLayout _wl;
        private IEditorService _edsvc;

        private BindingList<CommandDecorator> _commands;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            _wl = (IWebLayout)_edsvc.GetEditedResource();
            //Populate command set
            foreach (var cmd in _wl.CommandSet.Commands)
            {
                _commands.Add(new CommandDecorator(cmd));
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
                        _wl.CommandSet.AddCommand(_commands[e.NewIndex].DecoratedInstance);
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
                var dec = (CommandDecorator)row.DataBoundItem;
                var cmd = dec.DecoratedInstance;
                if (typeof(IInvokeScriptCommand).IsAssignableFrom(cmd.GetType()))
                {
                    cmds.Add(cmd);
                }
                else if (typeof(IInvokeUrlCommand).IsAssignableFrom(cmd.GetType()))
                {
                    cmds.Add(cmd);
                }
                else if (typeof(ISearchCommand).IsAssignableFrom(cmd.GetType()))
                {
                    cmds.Add(cmd);
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

        private void SetSelectedCommand(CommandDecorator cmd)
        {
            grdCommands.ClearSelection();
            foreach (DataGridViewRow row in grdCommands.Rows)
            {
                if (row.DataBoundItem == cmd)
                {
                    //HACK: This is a long-winded way of simulating a programmatic
                    //click of the cell that contains this databound item
                    //
                    //See: http://social.msdn.microsoft.com/forums/en-US/winformsdatacontrols/thread/47e9c3ef-a8de-48c9-8e0d-4f3fdd34517e/
                    grdCommands.FirstDisplayedScrollingRowIndex = row.Index;
                    grdCommands.Refresh();
                    grdCommands.CurrentCell = row.Cells[1];
                    row.Selected = true;
                    grdCommands_CellContentClick(this, new DataGridViewCellEventArgs(1, row.Index));
                    break;
                }
            }
        }

        private void invokeURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cmd = _wl.CreateInvokeUrlCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Strings.InvokeUrlCmdDescription;
            var dec = new CommandDecorator(cmd);
            _commands.Add(dec);
            SetSelectedCommand(dec);
        }

        private void invokeScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cmd = _wl.CreateInvokeScriptCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Strings.InvokeScriptCmdDescription;
            var dec = new CommandDecorator(cmd);
            _commands.Add(dec);
            SetSelectedCommand(dec);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_wl.Map.ResourceId))
            {
                MessageBox.Show(Strings.SpecifyMapForWebLayout);
                return;
            }

            var cmd = _wl.CreateSearchCommand();
            cmd.Description = cmd.Label = cmd.Tooltip = Strings.SearchCmdDescription;
            var dec = new CommandDecorator(cmd);
            _commands.Add(dec);
            SetSelectedCommand(dec);
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

                //HACK: Mono workaround. It has problems with DataBoundItem
                var dec = row.DataBoundItem as CommandDecorator;
                if (dec != null)
                {
                    ICommand cmd = dec.DecoratedInstance;
                    cmdCtrl.Bind(cmd, _edsvc);
                    grpCommand.Controls.Add(cmdCtrl);
                    EvaluateCommands();
                }
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
                var cmd = grdCommands.SelectedRows[0].DataBoundItem as CommandDecorator;
                if (cmd != null)
                {
                    var iurl = cmd.DecoratedInstance as IInvokeUrlCommand;
                    var iscr = cmd.DecoratedInstance as IInvokeScriptCommand;
                    var srch = cmd.DecoratedInstance as ISearchCommand;

                    WebLayoutRegion[] regions;
                    if (iurl != null)
                    {
                        if (_wl.IsCommandReferenced(iurl.Name, out regions))
                        {
                            if (!Ask(Strings.DeleteCommand, string.Format(Strings.PromptDeleteCommand, GetAsString(regions, ", "))))
                                return;
                        }

                        using (new WaitCursor(this))
                        {
                            int deleted = _wl.RemoveAllReferences(iurl.Name);
                            _wl.CommandSet.RemoveCommand(iurl);
                            _commands.Remove(cmd);
                            ClearCommandUI();
                        }
                    }
                    else if (iscr != null)
                    {
                        if (_wl.IsCommandReferenced(iscr.Name, out regions))
                        {
                            if (!Ask(Strings.DeleteCommand, string.Format(Strings.PromptDeleteCommand, GetAsString(regions, ", "))))
                                return;
                        }

                        using (new WaitCursor(this))
                        {
                            _wl.RemoveAllReferences(iscr.Name);
                            _wl.CommandSet.RemoveCommand(iscr);
                            _commands.Remove(cmd);
                            ClearCommandUI();
                        }
                    }
                    else if (srch != null)
                    {
                        if (_wl.IsCommandReferenced(srch.Name, out regions))
                        {
                            if (!Ask(Strings.DeleteCommand, string.Format(Strings.PromptDeleteCommand, GetAsString(regions, ", "))))
                                return;
                        }

                        using (new WaitCursor(this))
                        {
                            _wl.RemoveAllReferences(srch.Name);
                            _wl.CommandSet.RemoveCommand(srch);
                            _commands.Remove(cmd);
                            ClearCommandUI();
                        }
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
                MessageBox.Show(Strings.ExportNoCommandsSelected);
                return;
            }
            else
            {
                List<string> selectedCmds = new List<string>();
                foreach (DataGridViewRow row in grdCommands.SelectedRows)
                {
                    var cmd = ((CommandDecorator)row.DataBoundItem).DecoratedInstance;
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
                    MessageBox.Show(Strings.ExportNoCustomCommandsSelected);
                    return;
                }

                using (var save = DialogFactory.SaveFile())
                {
                    save.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickXml, "xml"); //NOXLATE
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        _wl.ExportCustomCommands(save.FileName, selectedCmds.ToArray());
                        MessageBox.Show(string.Format(Strings.CustomCommandsExported, save.FileName));
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                open.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickXml, "xml"); //NOXLATE
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
                            _commands.Add(new CommandDecorator(cmd));

                            if (imported.NameChanged)
                                sb.AppendLine(imported.ToString());
                        }

                        MessageBox.Show(string.Format(Strings.CustomCommandsImported, cmds.Length, open.FileName, sb.ToString()));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Strings.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    //HACK: Mono workaround for bugs due to binding to a BindingList of interfaces.
    //So use a concrete decorator class that wraps the internal interface and bind to that
    //instead

    class CommandDecorator
    {
        private ICommand _cmd;

        [Browsable(false)]
        internal ICommand DecoratedInstance { get { return _cmd; } }

        public CommandDecorator(ICommand cmd) { _cmd = cmd; }

        public string Name
        {
            get
            {
                return _cmd.Name;
            }
            set
            {
                if (value != _cmd.Name)
                {
                    _cmd.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Label
        {
            get
            {
                return _cmd.Label;
            }
            set
            {
                if (value != _cmd.Label)
                {
                    _cmd.Label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        public TargetViewerType TargetViewer
        {
            get
            {
                return _cmd.TargetViewer;
            }
            set
            {
                if (value != _cmd.TargetViewer)
                {
                    _cmd.TargetViewer = value;
                    OnPropertyChanged("TargetViewer");
                }
            }
        }

        public string Tooltip
        {
            get
            {
                return _cmd.Tooltip;
            }
            set
            {
                if (value != _cmd.Tooltip)
                {
                    _cmd.Tooltip = value;
                    OnPropertyChanged("Tooltip");
                }
            }
        }

        public string Description
        {
            get
            {
                return _cmd.Description;
            }
            set
            {
                if (value != _cmd.Description)
                {
                    _cmd.Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string ImageURL
        {
            get
            {
                return _cmd.ImageURL;
            }
            set
            {
                if (value != _cmd.ImageURL)
                {
                    _cmd.ImageURL = value;
                    OnPropertyChanged("ImageURL");
                }
            }
        }

        public string DisabledImageURL
        {
            get
            {
                return _cmd.DisabledImageURL;
            }
            set
            {
                if (value != _cmd.DisabledImageURL)
                {
                    _cmd.DisabledImageURL = value;
                    OnPropertyChanged("DisabledImageURL");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
