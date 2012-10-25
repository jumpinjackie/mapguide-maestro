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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal partial class NewFolderDialog : Form
    {
        private List<string> _folderNames;

        public NewFolderDialog(string name, string [] folderNames)
        {
            InitializeComponent();
            _folderNames = new List<string>(folderNames);
            txtName.Text = name;
        }

        public string FolderName { get { return txtName.Text; } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            btnCreate.Enabled = false;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorProvider.SetError(txtName, Strings.Required);
                return;
            }
            else if (_folderNames.Contains(txtName.Text))
            {
                errorProvider.SetError(txtName, Strings.FolderNameExists);
                return;
            }

            btnCreate.Enabled = true;
        }
    }
}
