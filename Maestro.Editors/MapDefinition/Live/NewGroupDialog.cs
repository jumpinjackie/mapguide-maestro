#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Mapping;

namespace Maestro.Editors.MapDefinition.Live
{
    internal partial class NewGroupDialog : Form
    {
        private RuntimeMap _map;

        public NewGroupDialog(RuntimeMap map)
        {
            InitializeComponent();
            _map = map;
        }

        public string GroupName { get { return txtName.Text; } }

        public string GroupLabel { get { return txtLegendLabel.Text; } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string label = txtLegendLabel.Text.Trim();

            btnOK.Enabled = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(label) && _map.Groups[name] == null;
            lblDuplicateName.Visible = (_map.Groups[name] != null);
        }

        private void txtLegendLabel_TextChanged(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string label = txtLegendLabel.Text.Trim();

            btnOK.Enabled = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(label);
        }
    }
}
