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
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace Maestro.Editors.FeatureSource.Preview
{
    //TODO: IGeometry objects do not currently display (we should be showing the WKT form)
    [ToolboxItem(false)]
    internal partial class PreviewPane : UserControl, IQueryControl
    {
        private PreviewPane()
        {
            InitializeComponent();
        }

        private QueryMode _mode;
        private ClassDefinition _cls;
        private IFeatureService _featSvc;
        private string _fsId;

        public PreviewPane(string fsId, QueryMode mode, ClassDefinition cls, IFeatureService featSvc, FdoProviderCapabilities caps)
            : this()
        {
            _fsId = fsId;
            _mode = mode;
            _cls = cls;
            _featSvc = featSvc;

            IQueryControl ctrl = null;
            switch (_mode)
            {
                case QueryMode.SQL:
                    ctrl = new SqlQueryCtrl(fsId, featSvc);
                    _inner = ctrl;
                    break;
                case QueryMode.Standard:
                    ctrl = new StandardQueryCtrl(fsId, featSvc, cls, caps);
                    _inner = ctrl;
                    break;
            }

            if (ctrl == null)
            {
                throw new ArgumentException(Strings.UnknownQueryMode);
            }

            ctrl.Content.Dock = DockStyle.Fill;
            queryPane.Controls.Add(ctrl.Content);
        }

        private IQueryControl _inner;

        public IReader ExecuteQuery()
        {
            return _inner.ExecuteQuery();
        }

        public Control Content
        {
            get { return this; }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            grdResults.DataSource = null;
        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            btnClear.Enabled = false;
            btnStop.Enabled = true;
            btnRunQuery.Enabled = false;
            lblCount.Text = lblElapsed.Text = string.Empty;
            queryWorker.RunWorkerAsync();
        }

        class QueryResult
        {
            public DataTable Result { get; set; }

            public TimeSpan Duration { get; set; }
        }

        private void queryWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var res = new QueryResult();
            var sw = new Stopwatch();
            sw.Start();
            var reader = this.ExecuteQuery();
            try
            {   
                res.Result = new DataTable();
                InitTable(reader, res.Result);
                while (reader.ReadNext())
                {
                    if (queryWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        _cancelResult = res.Result;
                        break;
                    }

                    var row = res.Result.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.IsNull(i))
                            row[i] = DBNull.Value;
                        else
                            row[i] = reader[i];
                    }
                    res.Result.Rows.Add(row);
                }
            }
            finally
            {
                reader.Close();
                sw.Stop();
                if (queryWorker.CancellationPending)
                {
                    _cancelDuration = sw.Elapsed;
                }
            }
            e.Result = res;
        }

        private void InitTable(IReader reader, DataTable dataTable)
        {
            //foreach (var col in reader.Columns)
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                Type type = reader.GetFieldType(i);
                var column = new DataColumn(name, type);
                dataTable.Columns.Add(column);
            }
        }

        DataTable _cancelResult;
        TimeSpan? _cancelDuration;

        private void queryWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
            }
            else
            {
                if (e.Cancelled)
                {
                    grdResults.DataSource = _cancelResult;
                    lblElapsed.Text = string.Format(Strings.PreviewQueryElapsed, _cancelDuration.Value.TotalMilliseconds);
                    lblCount.Text = string.Format(Strings.PreviewRecordCount, _cancelResult.Rows.Count);
                }
                else
                {
                    var res = e.Result as QueryResult;
                    if (res != null)
                    {
                        grdResults.DataSource = res.Result;
                        lblElapsed.Text = string.Format(Strings.PreviewQueryElapsed, res.Duration.TotalMilliseconds);
                        lblCount.Text = string.Format(Strings.PreviewRecordCount, res.Result.Rows.Count);
                    }
                }
            }

            btnRunQuery.Enabled = true;
            btnStop.Enabled = false;
            btnClear.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (queryWorker.IsBusy)
            {
                queryWorker.CancelAsync();
            }
        }
    }
}
