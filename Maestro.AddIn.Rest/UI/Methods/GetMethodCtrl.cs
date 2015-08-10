#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using Maestro.AddIn.Rest.Model;

namespace Maestro.AddIn.Rest.UI.Methods
{
    internal partial class GetMethodCtrl : UserControl
    {
        class ComputedProperty : INotifyPropertyChanged
        {
            private string _alias;
            private string _expression;

            public ComputedProperty()
            {
                _alias = string.Empty;
                _expression = string.Empty;
            }

            public string Alias
            {
                get { return _alias; }
                set
                {
                    if (value != _alias)
                    {
                        _alias = value;
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Alias)));
                    }
                }
            }
            public string Expression
            {
                get { return _expression; }
                set
                {
                    if (value != _expression)
                    {
                        _expression = value;
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expression)));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private RestSourceContext _context;
        private BindingList<ComputedProperty> _props;

        public GetMethodCtrl()
        {
            InitializeComponent();
            _props = new BindingList<ComputedProperty>();
            grdComputedProperties.DataSource = _props;
        }

        public void Init(RestSourceContext context)
        {
            _context = context;
            numMaxCount.Minimum = 0;
            numMaxCount.Maximum = Int32.MaxValue;
            numPageSize.Minimum = 0;
            numPageSize.Maximum = Int32.MaxValue;

            numMaxCount.Value = 500;
            numPageSize.Value = 100;

            lstProperties.DataSource = context.GetPropertyNames();
        }

        public dynamic WriteOptions()
        {
            dynamic opts = new ExpandoObject();

            if (chkPageSize.Checked)
                opts.PageSize = Convert.ToInt32(numPageSize.Value);

            if (chkMaxCount.Checked)
                opts.MaxCount = Convert.ToInt32(numMaxCount.Value);

            if (chkTransformTo.Checked)
                opts.TransformTo = txtTransformTo.Text;

            if (chkProperties.Checked)
                opts.Properties = new List<string>(lstProperties.SelectedItems.Cast<object>().Select(o => o.ToString()));

            if (chkComputedProperties.Checked)
            {
                opts.ComputedProperties = new ExpandoObject();
                var dict = (IDictionary<string, object>)opts.ComputedProperties;
                foreach (var prop in _props)
                {
                    dict.Add(prop.Alias, prop.Expression);
                }
            }

            return opts;
        }

        private void grdComputedProperties_SelectionChanged(object sender, EventArgs e)
        {
            btnEditExpression.Enabled = grdComputedProperties.SelectedRows.Count == 1;
        }

        private void btnEditExpression_Click(object sender, EventArgs e)
        {
            if (grdComputedProperties.SelectedRows.Count == 1)
            {
                var prop = grdComputedProperties.SelectedRows[0].DataBoundItem as ComputedProperty;
                if (prop != null)
                {
                    string expr = _context.EditExpression(prop.Expression);
                    if (expr != null)
                        prop.Expression = expr;
                }
            }
        }

        private void btnBrowseCs_Click(object sender, EventArgs e)
        {
            string code = _context.PickCoordinateSystemCode();
            if (code != null)
                txtTransformTo.Text = code;
        }
    }
}
