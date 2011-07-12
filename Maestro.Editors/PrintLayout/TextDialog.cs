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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.PrintLayout
{
    internal partial class TextDialog : Form
    {
        public TextDialog()
        {
            InitializeComponent();
        }

        public string TextString
        {
            get { return txtContent.Text; }
            set { txtContent.Text = value; }
        }

        public float PositionLeft
        {
            get { return Convert.ToSingle(numPosLeft.Value); }
            set { numPosLeft.Value = Convert.ToDecimal(value); }
        }

        public float PositionBottom
        {
            get { return Convert.ToSingle(numPosBottom.Value); }
            set { numPosBottom.Value = Convert.ToDecimal(value); }
        }

        public string PositionUnits
        {
            get { return cmbPositionUnits.Text; }
            set { cmbPositionUnits.Text = value; }
        }

        public string FontName
        {
            get { return txtFontName.Text; }
            set { txtFontName.Text = value; }
        }

        public new float FontHeight
        {
            get { return Convert.ToSingle(numFontHeight.Value); }
            set { numFontHeight.Value = Convert.ToDecimal(value); }
        }

        public string FontUnits
        {
            get { return cmbFontUnits.Text; }
            set { cmbFontUnits.Text = value; }
        }

        private void btnBrowseFontName_Click(object sender, EventArgs e)
        {
            using (var dlg = new FontDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.FontName = dlg.Font.Name;
                    this.FontHeight = dlg.Font.Height;
                    this.FontUnits = "points";
                }
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
    }
}
