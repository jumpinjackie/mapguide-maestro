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
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;

namespace Maestro.Editors.FeatureSource.Providers
{
    internal partial class SetCredentialsDialog : Form
    {
        private string[] _propertyNames;

        public SetCredentialsDialog(string [] propertyNames)
        {
            InitializeComponent();
            _propertyNames = propertyNames;
        }

        public SetCredentialsDialog(string userProp, string passProp)
        {
            InitializeComponent();
            txtUserProperty.Text = userProp;
            txtPasswordProperty.Text = passProp;
            btnUserProperty.Visible = false;
            btnPasswordProperty.Visible = false;
        }

        public string UserProperty { get { return txtUserProperty.Text; } }

        public string PasswordProperty { get { return txtPasswordProperty.Text; } }

        public string Username { get { return txtUsername.Text; } }

        public string Password { get { return txtPassword.Text; } }

        private void btnUserProperty_Click(object sender, EventArgs e)
        {
            var list = new List<string>(_propertyNames);
            list.Remove(txtPasswordProperty.Text);

            string item = GenericItemSelectionDialog.SelectItem(null, null, list.ToArray());
            if (item != null)
            {
                txtUserProperty.Text = item;
                CheckSubmissionState();
            }
        }

        private void btnPasswordProperty_Click(object sender, EventArgs e)
        {
            var list = new List<string>(_propertyNames);
            list.Remove(txtUserProperty.Text);

            string item = GenericItemSelectionDialog.SelectItem(null, null, list.ToArray());
            if (item != null)
            {
                txtPasswordProperty.Text = item;
                CheckSubmissionState();
            }
        }

        private void CheckSubmissionState()
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtUserProperty.Text) && 
                            !string.IsNullOrEmpty(txtPasswordProperty.Text) && 
                            !string.IsNullOrEmpty(txtUsername.Text) && 
                            !string.IsNullOrEmpty(txtPassword.Text) && 
                            txtUserProperty.Text != txtPasswordProperty.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            CheckSubmissionState();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            CheckSubmissionState();
        }
    }
}
