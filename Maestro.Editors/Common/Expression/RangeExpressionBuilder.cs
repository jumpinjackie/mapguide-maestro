#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common.Expression
{
    internal partial class RangeExpressionBuilder : Form
    {
        private class LookupItem
        {
            public double Min { get; set; }

            public double Max { get; set; }

            public string Value { get; set; }
        }

        private BindingList<LookupItem> _items;

        private RangeExpressionBuilder()
        {
            InitializeComponent();
            _items = new BindingList<LookupItem>();
            grdRangeItems.DataSource = _items;
        }

        public RangeExpressionBuilder(string[] propertyNames)
            : this()
        {
            cmbProperty.DataSource = propertyNames;
        }

        public string GetExpression()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("RANGE(" + cmbProperty.Text + ", " + txtDefaultValue.Text);
            foreach (var item in _items)
            {
                sb.Append(", " + item.Min.ToString(CultureInfo.InvariantCulture) + ", " + item.Max.ToString(CultureInfo.InvariantCulture) + ", " + item.Value);
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