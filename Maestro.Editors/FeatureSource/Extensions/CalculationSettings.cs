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
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Extensions
{
    [ToolboxItem(false)]
    internal partial class CalculationSettings : UserControl
    {
        private CalculationSettings()
        {
            InitializeComponent();
        }

        private IEditorService _edSvc;
        private ClassDefinition _cls;
        private IFeatureSource _parent;

        public CalculationSettings(IEditorService edSvc, ClassDefinition cls, IFeatureSource parent, ICalculatedProperty calc)
            : this()
        {
            _edSvc = edSvc;
            _cls = cls;
            _parent = parent;
            TextBoxBinder.BindText(txtExpression, calc, "Expression"); //NOXLATE
            TextBoxBinder.BindText(txtName, calc, "Name"); //NOXLATE
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string expr = _edSvc.EditExpression(txtExpression.Text, _cls, _parent.Provider, _parent.ResourceID, false);
            if (expr != null)
            {
                txtExpression.Text = expr;
            }
        }
    }
}
