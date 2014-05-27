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
#endregion
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales.SymbolParamEditors
{
    internal partial class BooleanEditor : Form
    {
        public BooleanEditor()
        {
            InitializeComponent();
        }

        public void SetDataType(DataType2 dt2, bool value)
        {
            this.Text = dt2.ToString();
            if (value)
                rdTrue.Checked = true;
            else
                rdFalse.Checked = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateResult();
            base.OnLoad(e);
        }

        public bool Result
        {
            get;
            private set;
        }

        private void UpdateResult()
        {
            if (rdTrue.Checked)
                this.Result = true;
            else if (rdFalse.Checked)
                this.Result = false;
        }

        private void rdTrue_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void rdFalse_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
