#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales.SymbolParamEditors
{
    public partial class NumberEditor : Form
    {
        public NumberEditor()
        {
            InitializeComponent();
        }

        public void SetDataType(DataType2 dt, decimal value)
        {
            switch (dt)
            {
                case DataType2.Integer:
                    numericUpDown.Minimum = int.MinValue;
                    numericUpDown.Maximum = int.MaxValue;
                    numericUpDown.Value = value;
                    this.Text = dt.ToString();
                    break;

                case DataType2.Real:
                case DataType2.LineWeight:
                    numericUpDown.Minimum = decimal.MinValue;
                    numericUpDown.Maximum = decimal.MaxValue;
                    numericUpDown.Value = value;
                    this.Text = dt.ToString();
                    break;

                case DataType2.Angle:
                    numericUpDown.Minimum = Convert.ToDecimal(0.0);
                    numericUpDown.Maximum = Convert.ToDecimal(360.0);
                    numericUpDown.Value = value;
                    this.Text = dt.ToString();
                    break;

                case DataType2.ObliqueAngle:
                    numericUpDown.Minimum = Convert.ToDecimal(-85.0);
                    numericUpDown.Maximum = Convert.ToDecimal(85.0);
                    numericUpDown.Value = value;
                    this.Text = dt.ToString();
                    break;

                case DataType2.TrackSpacing:
                    numericUpDown.Minimum = Convert.ToDecimal(0.75);
                    numericUpDown.Maximum = Convert.ToDecimal(10.0);
                    numericUpDown.Value = value;
                    this.Text = dt.ToString();
                    break;
            }
        }

        /// <summary>
        /// Gets the value of this editor
        /// </summary>
        public decimal Value
        {
            get { return numericUpDown.Value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}