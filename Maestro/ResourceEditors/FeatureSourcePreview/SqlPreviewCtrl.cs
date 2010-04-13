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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview
{
    public partial class SqlPreviewCtrl : UserControl
    {
        private EditorInterface _ed;

        public SqlPreviewCtrl(EditorInterface ed)
        {
            InitializeComponent();
            _ed = ed;
        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            string sql = txtSql.Text;
            FeatureSetReader reader = null;
            try
            {
                reader = _ed.CurrentConnection.ExecuteSqlQuery(_ed.ResourceId, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (reader != null)
            {
                DataTable table = new DataTable();
                foreach (FeatureSetColumn col in reader.Columns)
                {
                    if (col.Type == typeof(Topology.Geometries.IGeometry))
                        table.Columns.Add(col.Name, typeof(string)); //We want to "visualise" the textual form
                    else
                        table.Columns.Add(col.Name, col.Type);
                }
                while (reader.Read())
                {
                    var drow = table.NewRow();
                    var row = reader.Row;
                    foreach (FeatureSetColumn col in reader.Columns)
                    {
                        if (!row.IsValueNull(col.Name))
                        {
                            if (col.Type == typeof(Topology.Geometries.IGeometry))
                                drow[col.Name] = ((Topology.Geometries.IGeometry)row[col.Name]).AsText();
                            else
                                drow[col.Name] = row[col.Name];
                        }
                        else
                        {
                            drow[col.Name] = DBNull.Value;
                        }
                    }
                    table.Rows.Add(drow);
                }
                grdResults.DataSource = table;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            grdResults.DataSource = null;
        }
    }
}
