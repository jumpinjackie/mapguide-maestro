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

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A generic dialog to prompt for a find and replace token
    /// </summary>
    public partial class FindReplaceDialog : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FindReplaceDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the token to search for
        /// </summary>
        public string FindToken
        {
            get { return txtFind.Text; }
        }

        /// <summary>
        /// Gets the token to replace with
        /// </summary>
        public string ReplaceToken
        {
            get { return txtReplace.Text; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            btnReplace.Enabled = (!string.IsNullOrEmpty(txtFind.Text)) && (txtFind.Text != txtReplace.Text);
        }

        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            btnReplace.Enabled = (!string.IsNullOrEmpty(txtFind.Text)) && (txtFind.Text != txtReplace.Text);
        }
    }
}
