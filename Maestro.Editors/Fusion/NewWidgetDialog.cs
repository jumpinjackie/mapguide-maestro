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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Editors.Fusion
{
    internal partial class NewWidgetDialog : Form
    {
        private IApplicationDefinition _appDef;
        private FlexibleLayoutEditorContext _context;

        public NewWidgetDialog(IApplicationDefinition appDef, FlexibleLayoutEditorContext context)
        {
            InitializeComponent();
            _appDef = appDef;
            _context = context;

            cmbWidgets.DisplayMember = "Type"; //NOXLATE
            cmbWidgets.DataSource = _context.GetAllWidgets();
        }

        public IWidgetInfo SelectedWidget { get { return cmbWidgets.SelectedItem as IWidgetInfo; } }

        public string WidgetName { get { return txtName.Text; } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(string.Format(Strings.FieldRequired, label2.Text));
                return;
            }

            if (_appDef.WidgetNameExists(txtName.Text))
            {
                MessageBox.Show(string.Format(Strings.WidgetNameExists, txtName.Text));
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
