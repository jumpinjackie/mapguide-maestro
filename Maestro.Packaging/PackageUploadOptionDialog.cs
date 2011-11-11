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
    /// The method of package uploading
    /// </summary>
    public enum PackageUploadMethod
    {
        /// <summary>
        /// The offical method. Uses the ApplyResourcePackage API. This will wholly succeed or fail. This may
        /// have issues with certain packages due to size and encrypted content.
        /// </summary>
        Transactional,
        /// <summary>
        /// This method can partially succeed and/or partially fail. Failed operations are logged. This method
        /// can have its progress measured and is generally unaffected by size and encryption issues. This method
        /// is recommended for large packages or for packages that fail to load in transactional mode.
        /// </summary>
        NonTransactional
    }

    /// <summary>
    /// Dialog for showing package loading options
    /// </summary>
    public partial class PackageUploadOptionDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageUploadOptionDialog"/> class.
        /// </summary>
        public PackageUploadOptionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the selected upload method
        /// </summary>
        public PackageUploadMethod Method
        {
            get
            {
                PackageUploadMethod method = PackageUploadMethod.Transactional;
                if (rdTransactional.Checked)
                    method = PackageUploadMethod.Transactional;
                else if (rdNonTransactional.Checked)
                    method = PackageUploadMethod.NonTransactional;
                return method;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
