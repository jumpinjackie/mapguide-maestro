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
using OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview.Query;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview
{
    public partial class ClassPreviewCtrl : UserControl
    {
        enum QueryMode
        {
            Standard,
            Aggregate
        }

        private ServerConnectionI _conn;
        private FdoProviderCapabilities _caps;
        private FeatureSourceDescription.FeatureSourceSchema _schema;
        private string _featureSourceId;
        private string _provider;

        private Dictionary<QueryMode, Control> _queryControls;

        public ClassPreviewCtrl(EditorInterface ed, string provider, FeatureSourceDescription.FeatureSourceSchema schema, string featureSourceId)
        {
            InitializeComponent();
            _conn = ed.CurrentConnection;
            _caps = _conn.GetProviderCapabilities(provider);
            _featureSourceId = featureSourceId;
            _provider = provider;
            _schema = schema;
            _queryControls = new Dictionary<QueryMode, Control>();

            List<QueryMode> modes = new List<QueryMode>();
            foreach (FdoProviderCapabilitiesCommandName cmd in _caps.Command.SupportedCommands)
            {
                if (cmd == FdoProviderCapabilitiesCommandName.Select)
                {
                    modes.Add(QueryMode.Standard);
                    _queryControls[QueryMode.Standard] = new StandardQueryCtrl(ed, _provider, _schema, _featureSourceId);
                    _queryControls[QueryMode.Standard].Dock = DockStyle.Fill;
                }
            }
            cmbQueryMode.ComboBox.DataSource = modes;
            cmbQueryMode.ComboBox.SelectedIndex = 0;
        }

        private QueryMode _selectedMode;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Control qc = _queryControls[_selectedMode];
            FeatureSetReader reader = null;
            if (typeof(StandardQueryCtrl).IsAssignableFrom(qc.GetType()))
            {
                StandardQueryCtrl std = (StandardQueryCtrl)qc;
                reader = _conn.QueryFeatureSource(_featureSourceId, _schema.Fullname, std.Filter, std.Properties, std.ComputedProperties);
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
                //while (reader.Read())
                foreach(FeatureSetRow row in reader)
                {
                    var drow = table.NewRow();
                    //var row = reader.Row;
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

        private void btnPreviewMap_Click(object sender, EventArgs e)
        {

        }

        private void cmbQueryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedMode = (QueryMode)cmbQueryMode.ComboBox.SelectedItem;
            LoadQueryControl();
        }

        private void LoadQueryControl()
        {
            queryPanel.Controls.Clear();
            queryPanel.Controls.Add(_queryControls[_selectedMode]);
        }
    }
}
