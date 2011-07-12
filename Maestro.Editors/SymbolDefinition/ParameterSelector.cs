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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace Maestro.Editors.SymbolDefinition
{
    internal partial class ParameterSelector : Form
    {
        private List<IParameter> _unfiltered;
        private List<IParameter> _filtered;

        public ParameterSelector(IEnumerable<IParameter> parameters, IList<string> filteredTypes)
        {
            InitializeComponent();

            _unfiltered = new List<IParameter>(parameters);
            _filtered = new List<IParameter>();
            if (filteredTypes.Count > 0)
            {
                foreach (var sp in parameters)
                {
                    foreach (string str in filteredTypes)
                    {
                        if (sp.DataType == str)
                        {
                            _filtered.Add(sp);
                            break;
                        }
                    }
                }
            }

            this.IsFiltered = (filteredTypes.Count > 0);
        }

        public IParameter SelectedParameter
        {
            get 
            {
                if (grdParameters.SelectedRows.Count == 1)
                    return grdParameters.SelectedRows[0].DataBoundItem as IParameter;
                return null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private bool _isFiltered;

        internal bool IsFiltered
        {
            get { return _isFiltered; }
            set 
            { 
                _isFiltered = value;
                if (value)
                    grdParameters.DataSource = _filtered;
                else
                    grdParameters.DataSource = _unfiltered;
                lblFiltered.Visible = value;
            }
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            this.IsFiltered = !this.IsFiltered;
        }

        private void grdParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = grdParameters.Rows[e.RowIndex];
                row.Selected = true;
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        public static void ShowParameterSelector(IEnumerable<IParameter> symParameters, SymbolField sender)
        {
            var list = new List<string>();
            if (sender.SupportedEnhancedDataTypes != null && sender.SupportedEnhancedDataTypes.Length > 0)
            {
                foreach (var dt in sender.SupportedEnhancedDataTypes)
                {
                    list.Add(dt.ToString());
                }
            }

            var diag = new ParameterSelector(symParameters, list);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                sender.Content = "%" + diag.SelectedParameter.Name + "%";
            }
        }

        public static void ShowParameterSelector(IEnumerable<IParameter> symParameters, ref string content)
        {
            var list = new List<string>();
            var diag = new ParameterSelector(symParameters, list);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                content = "%" + diag.SelectedParameter.Name + "%";
            }
            else
            {
                content = null;
            }
        }
    }
}
