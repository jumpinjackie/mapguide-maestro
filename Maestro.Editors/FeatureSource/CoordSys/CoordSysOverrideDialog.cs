#region Disclaimer / License

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

using System;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.CoordSys
{
    internal partial class CoordSysOverrideDialog : Form
    {
        private CoordSysOverrideDialog()
        {
            InitializeComponent();
        }

        public string CsName
        {
            get { return cmbName.Text; }
            set { cmbName.Text = value; }
        }

        public string CoordinateSystemWkt
        {
            get { return txtCoordinateSystem.Text; }
            set { txtCoordinateSystem.Text = value; }
        }

        private IEditorService _ed;

        public CoordSysOverrideDialog(IEditorService ed)
            : this()
        {
            _ed = ed;
            var list = _ed.CurrentConnection.FeatureService.GetSpatialContextInfo(_ed.EditedResourceID, false);
            foreach (var sc in list.SpatialContext)
            {
                cmbName.Items.Add(sc.Name);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnBrowseCs_Click(object sender, EventArgs e)
        {
            txtCoordinateSystem.Text = _ed.GetCoordinateSystem();
        }

        private void txtCoordinateSystem_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitStatus();
        }

        private void CheckSubmitStatus()
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtCoordinateSystem.Text) && !string.IsNullOrEmpty(cmbName.Text);
        }

        private void cmbName_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitStatus();
        }
    }
}