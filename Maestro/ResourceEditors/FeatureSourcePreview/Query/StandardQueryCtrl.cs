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
using System.Collections.Specialized;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview.Query
{
    public partial class StandardQueryCtrl : UserControl
    {
        private EditorInterface _ed;
        private FeatureSourceDescription.FeatureSourceSchema _schema;
        private string _featureSourceId;
        private string _provider;

        public StandardQueryCtrl(EditorInterface ed, string provider, FeatureSourceDescription.FeatureSourceSchema schema, string featureSourceId)
        {
            InitializeComponent();
            _ed = ed;
            _schema = schema;
            _featureSourceId = featureSourceId;
            _provider = provider;

            foreach (FeatureSetColumn col in schema.Columns)
            {
                chkProperties.Items.Add(col.Name, true);
            }
        }

        public string Filter
        {
            get { return txtFilter.Text; }
        }

        public string[] Properties
        {
            get
            {
                List<string> props = new List<string>();
                foreach (object obj in chkProperties.CheckedItems)
                {
                    props.Add(obj.ToString());
                }
                return props.ToArray();
            }
        }

        public NameValueCollection ComputedProperties
        {
            get
            {
                NameValueCollection nvc = new NameValueCollection();
                foreach (DataGridViewRow row in grdExpressions.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        nvc.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
                    }
                }
                return nvc;
            }
        }

        private void txtFilter_Enter(object sender, EventArgs e)
        {
            string filter = _ed.EditExpression(txtFilter.Text, _schema, _provider, _featureSourceId);
            if (!string.IsNullOrEmpty(filter))
            {
                txtFilter.Text = filter;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdExpressions.SelectedRows.Count == 1)
            {
                grdExpressions.Rows.Remove(grdExpressions.SelectedRows[0]);
            }
        }

        private int exprCounter = 0;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string expr = _ed.EditExpression("", _schema, _provider, _featureSourceId);
            if (!string.IsNullOrEmpty(expr))
            {
                string alias = "Expr" + exprCounter++;

                grdExpressions.Rows.Add(alias, expr);
            }
        }
    }
}
