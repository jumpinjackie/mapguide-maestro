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

#endregion Disclaimer / License

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Extensions
{
    [ToolboxItem(false)]
    internal partial class CalculationSettings : UserControl
    {
        private CalculationSettings()
        {
            InitializeComponent();
        }

        private readonly IEditorService _edSvc;
        private readonly ClassDefinition _cls;
        private readonly IFeatureSource _parent;

        public CalculationSettings(IEditorService edSvc, ClassDefinition cls, IFeatureSource parent, ICalculatedProperty calc)
            : this()
        {
            _edSvc = edSvc;
            _cls = cls;
            _parent = parent;
            TextBoxBinder.BindText(txtExpression, calc, nameof(calc.Expression));
            TextBoxBinder.BindText(txtName, calc, nameof(calc.Name));
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string expr = _edSvc.EditExpression(txtExpression.Text, _cls, _parent.Provider, _parent.ResourceID, Common.ExpressionEditorMode.Expression, false);
            if (expr != null)
            {
                txtExpression.Text = expr;
            }
        }
    }
}