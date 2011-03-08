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

namespace Maestro.Editors.MapDefinition
{
    internal partial class ManualScaleEditor : Form
    {
        private ManualScaleEditor()
        {
            InitializeComponent();
        }

        public ManualScaleEditor(IEnumerable<double> scales) 
            : this()
        {
            List<string> values = new List<string>();
            foreach (var d in scales)
            {
                values.Add(d.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture));
            }
            txtScales.Text = string.Join(Environment.NewLine, values.ToArray());
        }

        public double[] Scales
        {
            get
            {
                List<double> scales = new List<double>();
                string[] values = txtScales.Lines;
                foreach (var str in values)
                {
                    scales.Add(double.Parse(str, System.Threading.Thread.CurrentThread.CurrentUICulture));
                }
                return scales.ToArray();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
