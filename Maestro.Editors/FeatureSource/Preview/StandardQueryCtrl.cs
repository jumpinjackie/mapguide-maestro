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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Collections.Specialized;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace Maestro.Editors.FeatureSource.Preview
{
    [ToolboxItem(false)]
    internal partial class StandardQueryCtrl : UserControl, IQueryControl
    {
        private StandardQueryCtrl()
        {
            InitializeComponent();
        }

        private IFeatureService _featSvc;
        private string _fsId;
        private ClassDefinition _cls;
        private FdoProviderCapabilities _caps;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardQueryCtrl"/> class.
        /// </summary>
        /// <param name="fsId">The fs id.</param>
        /// <param name="featSvc">The feat SVC.</param>
        /// <param name="cls">The CLS.</param>
        /// <param name="caps">The caps.</param>
        public StandardQueryCtrl(string fsId, IFeatureService featSvc, ClassDefinition cls, FdoProviderCapabilities caps)
            : this()
        {
            _fsId = fsId;
            _featSvc = featSvc;
            _cls = cls;
            _caps = caps;
            foreach (var prop in cls.Properties)
            {
                chkProperties.Items.Add(prop.Name, true);
            }
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <returns></returns>
        public IReader ExecuteQuery()
        {
            return _featSvc.QueryFeatureSource(_fsId, _cls.QualifiedName, txtFilter.Text, GetProperties(), GetComputedColumns());
        }

        private NameValueCollection GetComputedColumns()
        {
            var nvc = new NameValueCollection();
            foreach (DataGridViewRow row in grdExpressions.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    nvc.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
            }
            return nvc;
        }

        private string[] GetProperties()
        {
            List<string> names = new List<string>();
            foreach (var obj in chkProperties.CheckedItems)
            {
                names.Add(obj.ToString());
            }
            return names.ToArray();
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public Control Content
        {
            get { return this; }
        }

        private void txtFilter_Click(object sender, EventArgs e)
        {
            var ed = FdoExpressionEditorFactory.Create(); //new ExpressionEditor();
            ed.Initialize(_featSvc, _caps, _cls, _fsId, false);
            ed.Expression = txtFilter.Text;
            if (ed.ShowDialog() == DialogResult.OK)
            {
                txtFilter.Text = ed.Expression;
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkProperties.Items.Count; i++)
            {
                chkProperties.SetItemChecked(i, true);
            }
        }

        private void btnCheckNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkProperties.Items.Count; i++)
            {
                chkProperties.SetItemChecked(i, false);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var ed = FdoExpressionEditorFactory.Create();// new ExpressionEditor();
            ed.Initialize(_featSvc, _caps, _cls, _fsId, false);
            if (ed.ShowDialog() == DialogResult.OK)
            {
                grdExpressions.Rows.Add(GenerateAlias(), ed.Expression);
            }
        }

        private string GenerateAlias()
        {
            int counter = 1;
            string name = "Expr" + counter; //NOXLATE
            while (AliasExists(name))
            {
                counter++;
                name = "Expr" + counter; //NOXLATE
            }
            return name;
        }

        private bool AliasExists(string name)
        {
            foreach (DataGridViewRow row in grdExpressions.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[0].Value.ToString().Equals(name))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdExpressions.SelectedRows.Count == 1)
            {
                var row = grdExpressions.SelectedRows[0];
                grdExpressions.Rows.Remove(row);
            }
        }
    }
}
