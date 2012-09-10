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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    internal partial class ElevationDialog : Form
    {
        private IEditorService _edSvc;
        private IVectorScaleRange2 _vsr2;

        private ClassDefinition _clsDef;
        private string _provider;
        private string _featureSourceId;

        private IElevationSettings _elSettings;

        private bool _init;

        public ElevationDialog(IEditorService edSvc, IVectorScaleRange2 vsr2, string featureSourceId, ClassDefinition clsDef, string provider)
        {
            InitializeComponent();

            _edSvc = edSvc;
            _vsr2 = vsr2;
            _clsDef = clsDef;
            _provider = provider;
            _featureSourceId = featureSourceId;

            cmbUnits.DataSource = Enum.GetValues(typeof(LengthUnitType));
            cmbZOffsetType.DataSource = Enum.GetValues(typeof(ElevationTypeType));

            _elSettings = vsr2.ElevationSettings;
            grpSettings.Enabled = (_elSettings != null);
            chkEnabled.Checked = (_elSettings != null);

            if (_elSettings == null)
                _elSettings = vsr2.Create("0", "0", ElevationTypeType.RelativeToGround, LengthUnitType.Meters); //NOXLATE

            try
            {
                _init = true;

                txtZExtrusion.Text = _elSettings.ZExtrusion;
                txtZOffset.Text = _elSettings.ZOffset;
                cmbUnits.SelectedItem = _elSettings.Unit;
                cmbZOffsetType.SelectedItem = _elSettings.ZOffsetType;
            }
            finally
            {
                _init = false;
            }
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            grpSettings.Enabled = chkEnabled.Checked;
        }

        private void txtZOffset_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _elSettings.ZOffset = txtZOffset.Text;
        }

        private void txtZExtrusion_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _elSettings.ZExtrusion = txtZExtrusion.Text;
        }

        private void cmbZOffsetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _elSettings.ZOffsetType = (ElevationTypeType)cmbZOffsetType.SelectedItem;
        }

        private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _elSettings.Unit = (LengthUnitType)cmbUnits.SelectedItem;
        }

        private void btnZOffset_Click(object sender, EventArgs e)
        {
            string expr = _edSvc.EditExpression(txtZOffset.Text, _clsDef, _provider, _featureSourceId, true);
            if (expr != null)
                txtZOffset.Text = expr;
        }

        private void btnZExtrusion_Click(object sender, EventArgs e)
        {
            string expr = _edSvc.EditExpression(txtZExtrusion.Text, _clsDef, _provider, _featureSourceId, true);
            if (expr != null)
                txtZExtrusion.Text = expr;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Attach or detach based on checkbox
            if (chkEnabled.Checked)
                _vsr2.ElevationSettings = _elSettings;
            else
                _vsr2.ElevationSettings = null;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
