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
    /// A dialog to allow the user to select a color with desired transparency
    /// </summary>
    public partial class ColorPickerDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerDialog"/> class.
        /// </summary>
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the selected color
        /// </summary>
        public Color SelectedColor
        {
            get { return colorComboWithTransparency1.CurrentColor; }
            set { colorComboWithTransparency1.CurrentColor = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
