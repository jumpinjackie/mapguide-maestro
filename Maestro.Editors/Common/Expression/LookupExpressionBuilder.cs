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

#endregion Disclaimer / License

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common.Expression
{
    internal partial class LookupExpressionBuilder : Form
    {
        private class LookupItem
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        private BindingList<LookupItem> _items;

        private LookupExpressionBuilder()
        {
            InitializeComponent();
            _items = new BindingList<LookupItem>();
            grdRangeItems.DataSource = _items;
        }

        public LookupExpressionBuilder(string[] propertyNames)
            : this()
        {
            cmbProperty.DataSource = propertyNames;
        }

        public string GetExpression()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("LOOKUP(" + cmbProperty.Text + ", " + txtDefaultValue.Text);
            foreach (var item in _items)
            {
                sb.Append(", " + item.Key + ", " + item.Value);
            }
            sb.Append(")");
            return sb.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtPreview.Text = this.GetExpression();
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