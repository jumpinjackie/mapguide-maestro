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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using Maestro.Editors.Common;

namespace Maestro.Editors.LoadProcedure
{
    [ToolboxItem(false)]
    internal partial class SdfTransformationCtrl : EditorBindableCollapsiblePanel
    {
        public SdfTransformationCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;

            var lp = service.GetEditedResource() as ILoadProcedure;
            Debug.Assert(lp != null);

            service.RegisterCustomNotifier(this);

            var sdflp = lp.SubType as ISdfLoadProcedure;

            TextBoxBinder.BindText(txtCoordinateSystem, sdflp, "CoordinateSystem");
            NumericBinder.BindValueChanged(numGeneralizePercentage, sdflp, "Generalization");
            cmbSdfConflictStrategy.DataSource = Enum.GetValues(typeof(SdfKeyTreatmentType));
            ComboBoxBinder.BindSelectedIndexChanged(cmbSdfConflictStrategy, "SelectedItem", sdflp, "SdfKeyTreatment");
        }

        private void btnBrowseCs_Click(object sender, EventArgs e)
        {
            string cs = _service.GetCoordinateSystem();
            if (!string.IsNullOrEmpty(cs) && cs != txtCoordinateSystem.Text)
            {
                txtCoordinateSystem.Text = cs;
            }
        }
    }
}
