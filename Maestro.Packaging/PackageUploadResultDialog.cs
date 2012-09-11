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

namespace Maestro.Packaging
{
    /// <summary>
    /// A dialog to present the results of a non-transactional package upload
    /// </summary>
    public partial class PackageUploadResultDialog : Form
    {
        private PackageUploadResultDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageUploadResultDialog"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public PackageUploadResultDialog(UploadPackageResult result)
            : this()
        {
            lblFailed.Text = string.Format(Strings.PackageOperationsFailed, result.Failed.Count);
            lblSkipped.Text = string.Format(Strings.PackageOperationsSkipped, result.SkipOperations.Count);
            lblSucceeded.Text = string.Format(Strings.PackageOperationsSucceeded, result.Successful.Count);
            //grdFailed.DataSource = result.Failed;
            grdFailed.Columns.Add("Resource ID", Strings.HeaderResourceId); //NOXLATE
            grdFailed.Columns.Add("Operation", Strings.HeaderOperation); //NOXLATE
            grdFailed.Columns.Add("Error", Strings.HeaderError); //NOXLATE
            foreach (var op in result.Failed.Keys)
            {
                grdFailed.Rows.Add(op.ResourceId, op.OperationName, result.Failed[op].ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }
    }
}
