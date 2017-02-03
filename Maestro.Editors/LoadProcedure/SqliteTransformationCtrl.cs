#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using Maestro.Editors.Common;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using System.ComponentModel;

namespace Maestro.Editors.LoadProcedure
{
    [ToolboxItem(false)]
    internal partial class SqliteTransformationCtrl : EditorBindableCollapsiblePanel
    {
        public SqliteTransformationCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
            var lp = service.GetEditedResource() as ILoadProcedure;
            var slp = lp.SubType as ISqliteLoadProcedure;

            TextBoxBinder.BindText(txtCoordinateSystem, slp, "CoordinateSystem");
            NumericBinder.BindValueChanged(numGeneralizePercentage, slp, "Generalization");
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