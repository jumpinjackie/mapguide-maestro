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

        internal void Bind(ICommand command, IEditorService service)
        {
            Bind(service);

            TextBoxBinder.BindText(txtDescription, command, "Description");
            TextBoxBinder.BindText(txtDisabledIcon, command, "DisabledImageURL");
            TextBoxBinder.BindText(txtEnabledIcon, command, "ImageURL");
            TextBoxBinder.BindText(txtName, command, "Name");
            TextBoxBinder.BindText(txtTitle, command, "Label");
            TextBoxBinder.BindText(txtTooltip, command, "Tooltip");

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
                txtTooltip.ReadOnly = true;
                txtDescription.ReadOnly = true;
                txtDisabledIcon.ReadOnly = true;
                txtEnabledIcon.ReadOnly = true;
                txtName.ReadOnly = true;
                txtTitle.ReadOnly = true;
                txtTooltip.ReadOnly = true;

                tabProperties.TabPages.Remove(TAB_ADVANCED);
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
        }
    }
}
