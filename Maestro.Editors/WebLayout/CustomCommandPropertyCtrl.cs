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
using OSGeo.MapGuide.ObjectModels.WebLayout;
using Maestro.Editors.WebLayout.Commands;

namespace Maestro.Editors.WebLayout
{
    [ToolboxItem(false)]
    internal partial class CustomCommandPropertyCtrl : EditorBase
    {
        public CustomCommandPropertyCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
        }

        private bool _init = false;
        private ICommand _cmd;

        internal void Bind(ICommand command, IEditorService service)
        {
            Bind(service);
            try
            {
                _init = false;
                _cmd = command;
                //TextBoxBinder.BindText(txtDescription, command, "Description");
                //TextBoxBinder.BindText(txtDisabledIcon, command, "DisabledImageURL");
                //TextBoxBinder.BindText(txtEnabledIcon, command, "ImageURL");
                //TextBoxBinder.BindText(txtName, command, "Name");
                //TextBoxBinder.BindText(txtTitle, command, "Label");
                //TextBoxBinder.BindText(txtTooltip, command, "Tooltip");

                txtDescription.Text = command.Description;
                txtDisabledIcon.Text = command.DisabledImageURL;
                txtEnabledIcon.Text = command.ImageURL;
                txtName.Text = command.Name;
                txtTitle.Text = command.Label;
                txtTooltip.Text = command.Tooltip;

                if (typeof(IInvokeScriptCommand).IsAssignableFrom(command.GetType()))
                {
                    var ctrl = new InvokeScriptCtrl();
                    ctrl.Bind((IInvokeScriptCommand)command, service);
                    ctrl.Dock = DockStyle.Fill;
                    TAB_ADVANCED.Controls.Add(ctrl);
                }
                else if (typeof(IInvokeUrlCommand).IsAssignableFrom(command.GetType()))
                {
                    var ctrl = new InvokeURLCtrl();
                    ctrl.Bind((IInvokeUrlCommand)command, service);
                    ctrl.Dock = DockStyle.Fill;
                    TAB_ADVANCED.Controls.Add(ctrl);
                }
                else if (typeof(ISearchCommand).IsAssignableFrom(command.GetType()))
                {
                    var ctrl = new SearchCmdCtrl();
                    ctrl.Bind((ISearchCommand)command, service);
                    ctrl.Dock = DockStyle.Fill;
                    TAB_ADVANCED.Controls.Add(ctrl);
                }
                else
                {
                    //Not editable
                    txtTooltip.ReadOnly = false;
                    txtDescription.ReadOnly = false;
                    txtDisabledIcon.ReadOnly = false;
                    txtEnabledIcon.ReadOnly = false;
                    txtName.ReadOnly = true;
                    txtTitle.ReadOnly = false;
                    txtTooltip.ReadOnly = false;

                    tabProperties.TabPages.Remove(TAB_ADVANCED);
                }
            }
            finally
            {
                _init = true;
            }
        }

        private void txtEnabledIcon_TextChanged(object sender, EventArgs e)
        {
            var img = CommandIconCache.GetStandardCommandIcon(txtEnabledIcon.Text);
            if (img != null)
            {
                imgEnabled.Image = img;
            }
            else
            {
                imgEnabled.Image = Properties.Resources.cross_circle_frame;
            }

            if (!_init) return;
            _cmd.ImageURL = txtEnabledIcon.Text;
        }

        private void txtDisabledIcon_TextChanged(object sender, EventArgs e)
        {
            var img = CommandIconCache.GetStandardCommandIcon(txtDisabledIcon.Text);
            if (img != null)
            {
                imgDisabled.Image = img;
            }
            else
            {
                imgDisabled.Image = Properties.Resources.cross_circle_frame;
            }

            if (!_init) return;
            _cmd.DisabledImageURL = txtDisabledIcon.Text;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!_init) return;
            _cmd.Name = txtName.Text;
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (!_init) return;
            _cmd.Label = txtTitle.Text;
        }

        private void txtTooltip_TextChanged(object sender, EventArgs e)
        {
            if (!_init) return;
            _cmd.Tooltip = txtTooltip.Text;
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!_init) return;
            _cmd.Description = txtDescription.Text;
        }
    }
}
