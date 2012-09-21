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
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class ExplodeThemeDialog : Form
    {
        private ExplodeThemeDialog()
        {
            InitializeComponent();
        }

        private IVectorScaleRange _parentRange;
        private IVectorStyle _style;
        private ILayerDefinition _parentLayer;
        private IEditorService _editor;

        public ExplodeThemeDialog(IEditorService editor, IVectorScaleRange parentRange, IVectorStyle style, ILayerDefinition parentLayer)
            : this()
        {
            if (style.StyleType == StyleType.Composite)
                throw new NotSupportedException(Strings.ErrorExplodingCompositeStyleNotSupported);

            _editor = editor;
            _style = style;
            _parentRange = parentRange;
            _parentLayer = parentLayer;

            txtLayersCreate.Text = style.RuleCount.ToString(CultureInfo.InvariantCulture);
            txtLayerNameFormat.Text = "{0} - {1} - {2}"; //NOXLATE
            EvaluateStates();

            if (!_editor.IsNew)
            {
                txtLayerPrefix.Text = ResourceIdentifier.GetName(_editor.ResourceID);
            }
            else
            {
                txtLayerPrefix.Text = "Theme"; //NOXLATE
            }
        }

        public string CreateInFolder
        {
            get { return txtFolder.Text; }
        }

        public string LayerNameFormat
        {
            get { return txtLayerNameFormat.Text; }
        }

        public string LayerPrefix
        {
            get { return txtLayerPrefix.Text; }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_editor.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFolder.Text = picker.ResourceID;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool EvaluateStates()
        {
            btnCreate.Enabled = IsValidToPreview() && ResourceIdentifier.IsFolderResource(txtFolder.Text);
            return btnCreate.Enabled;
        }

        private bool IsValidToPreview()
        {
            return !string.IsNullOrEmpty(txtLayerPrefix.Text) && txtLayerNameFormat.Text.Contains("{0}") && txtLayerNameFormat.Text.Contains("{1}") && txtLayerNameFormat.Text.Contains("{2}"); //NOXLATE
        }

        private void txtLayerNameFormat_TextChanged(object sender, EventArgs e)
        {
            GeneratePreviews();
        }

        private void GeneratePreviews()
        {
            lstLayerNameExamples.Items.Clear();
            if (IsValidToPreview())
            {
                string layerPrefix = txtLayerPrefix.Text;
                int rules = Math.Min(3, _style.RuleCount);
                string scaleRange = Utility.GetScaleRangeStr(_parentRange);
                for (int i = 0; i < rules; i++)
                {
                    var rule = _style.GetRuleAt(i);
                    string name = Utility.GenerateLayerName(txtLayerNameFormat.Text, layerPrefix, scaleRange, i, rule);
                    lstLayerNameExamples.Items.Add(name);
                }
                EvaluateStates();
            }
            else
            {
                lstLayerNameExamples.Items.Add(Strings.InvalidLayerNameFormat);
                EvaluateStates();
            }
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            EvaluateStates();
        }

        private void txtLayerPrefix_TextChanged(object sender, EventArgs e)
        {
            EvaluateStates();
            GeneratePreviews();
        }
    }
}
