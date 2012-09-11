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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Diagnostics;
using System.Collections.Specialized;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.FeatureSource.Providers.Odbc;

namespace Maestro.Editors.FeatureSource.Providers
{
    [ToolboxItem(false)]
    internal partial class GenericCtrl : EditorBindableCollapsiblePanel
    {
        internal GenericCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;
        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
            _fs = _service.GetEditedResource() as IFeatureSource;

            txtProvider.Text = _fs.Provider;

            Debug.Assert(_fs != null);
            resDataCtrl.DataListChanged += (sender, e) => { OnResourceChanged(); };
            resDataCtrl.Init(service);
            InitGrid();
        }

        private string GetPartialConnectionString()
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            foreach (DataGridViewRow row in grdConnectionParameters.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    if (!string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                        builder[row.Cells[0].Value.ToString()] = row.Cells[1].Value;
                }
            }
            return builder.ToString();
        }

        private Dictionary<string, DataGridViewComboBoxCell> _enumCells = new Dictionary<string, DataGridViewComboBoxCell>();

        private void InitGrid()
        {
            grdConnectionParameters.Rows.Clear();
            grdConnectionParameters.Columns.Clear();
            var prov = _service.FeatureService.GetFeatureProvider(_fs.Provider);

            var colName = new DataGridViewColumn();
            colName.Name = "COL_NAME"; //NOXLATE
            colName.HeaderText = Strings.ColHeaderName;
            colName.ReadOnly = true;
            colName.CellTemplate = new DataGridViewTextBoxCell();
            var colValue = new DataGridViewColumn();
            colValue.Name = "COL_VALUE"; //NOXLATE
            colValue.HeaderText = Strings.ColHeaderValue;
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colValue.CellTemplate = new DataGridViewTextBoxCell();

            grdConnectionParameters.Columns.Add(colName);
            grdConnectionParameters.Columns.Add(colValue);

            foreach (var p in prov.ConnectionProperties)
            {
                var row = new DataGridViewRow();
                var nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = p.Name;
                nameCell.ToolTipText = p.LocalizedName;

                var currentValue = _fs.GetConnectionProperty(p.Name);
                DataGridViewCell valueCell = null;
                if (p.Enumerable)
                {
                    valueCell = new DataGridViewTextBoxCell();
                    valueCell.Tag = p;
                    valueCell.Value = currentValue;
                }
                else
                {
                    valueCell = new DataGridViewTextBoxCell();
                    valueCell.Tag = p;
                    valueCell.Value = currentValue;
                }

                if (string.IsNullOrEmpty(currentValue) && !string.IsNullOrEmpty(p.DefaultValue))
                {
                    valueCell.Value = p.DefaultValue;
                    _fs.SetConnectionProperty(p.Name, p.DefaultValue);
                }

                row.Cells.Add(nameCell);
                row.Cells.Add(valueCell);

                if (p.Protected)
                    pwdCells.Add(valueCell);

                grdConnectionParameters.Rows.Add(row);
            }
        }

        private List<DataGridViewCell> pwdCells = new List<DataGridViewCell>();

        private bool IsPasswordCell(DataGridViewCell cell)
        {
            return pwdCells.Contains(cell);
        }

        private void grdConnectionParameters_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = grdConnectionParameters.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell != null && IsPasswordCell(cell) && cell.Value != null)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    Graphics g = e.Graphics;
                    g.DrawString(new string('*', cell.Value.ToString().Length), this.Font, new SolidBrush(Color.Black), e.CellBounds); //NOXLATE
                    e.Handled = true;
                }
            }
        }

        private void grdConnectionParameters_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = grdConnectionParameters.SelectedCells[0];
            if (cell != null)
            {
                TextBox t = e.Control as TextBox;
                if (t != null)
                {
                    t.UseSystemPasswordChar = IsPasswordCell(cell);
                }
            }
        }

        private NameValueCollection GetConnectionParameters()
        {
            var nvc = new NameValueCollection();

            foreach (DataGridViewRow row in grdConnectionParameters.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    nvc.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
                }
            }

            return nvc;
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            txtTestResult.Text = string.Empty;
            var param = GetConnectionParameters();

            var cloneFs = (IFeatureSource)_fs.Clone();
            _service.ResourceService.SaveResourceAs(cloneFs, "Session:" + _service.SessionID + "//" + Guid.NewGuid().ToString() + ".FeatureSource"); //NOXLATE
            
            cloneFs.ClearConnectionProperties();
            foreach (var key in param.AllKeys)
            {
                cloneFs.SetConnectionProperty(key, param[key]);
            }
            _service.ResourceService.SaveResource(cloneFs);

            string msg = _service.FeatureService.TestConnection(cloneFs.ResourceID);

            if (string.IsNullOrEmpty(msg))
                msg = Strings.TestConnectionNoErrors;

            txtTestResult.Text = msg;
        }

        private void pickAValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdConnectionParameters.SelectedCells.Count != 1)
                return;

            var cell = grdConnectionParameters.SelectedCells[0];
            List<string> values = new List<string>();
            try
            {
                var prop = grdConnectionParameters.Rows[cell.RowIndex].Cells[0].Value.ToString();

                var p = cell.Tag as OSGeo.MapGuide.ObjectModels.Common.FeatureProviderRegistryFeatureProviderConnectionProperty;
                if (p != null)
                {
                    if (p.Enumerable && p.Value.Count > 0)
                    {
                        values.AddRange(p.Value);
                    }
                    else
                    {
                        values.AddRange(_service.FeatureService.GetConnectionPropertyValues(txtProvider.Text, prop, GetPartialConnectionString()));
                    }
                }
            }
            catch
            {
            }

            if (values.Count > 0)
            {
                var selected = GenericItemSelectionDialog.SelectItem(null, null, values.ToArray());
                if (!string.IsNullOrEmpty(selected))
                {
                    cell.Value = selected;
                }
            }
            else
            {
                MessageBox.Show(Strings.PropEnumNoValues);
            }
        }

        private void pickADataStoreFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdConnectionParameters.SelectedCells.Count != 1)
                return;

            var cell = grdConnectionParameters.SelectedCells[0];
            var values = new List<string>();
            try
            {
                var prop = grdConnectionParameters.Rows[cell.RowIndex].Cells[0].Value.ToString();

                var p = cell.Tag as OSGeo.MapGuide.ObjectModels.Common.FeatureProviderRegistryFeatureProviderConnectionProperty;
                if (p != null && p.Enumerable)
                {
                    var list = _service.FeatureService.EnumerateDataStores(txtProvider.Text, GetPartialConnectionString());
                    foreach(var ds in list.DataStore)
                    {
                        values.Add(ds.Name);
                    }
                }
            }
            catch { }

            if (values.Count > 0)
            {
                var selected = GenericItemSelectionDialog.SelectItem(null, null, values.ToArray());
                if (!string.IsNullOrEmpty(selected))
                {
                    cell.Value = selected;
                }
            }
            else
            {
                MessageBox.Show(Strings.PropEnumNoValues);
            }
        }

        private void grdConnectionParameters_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var ht = grdConnectionParameters.HitTest(e.X, e.Y);
                if (ht.ColumnIndex >= 0 && ht.RowIndex >= 0)
                {
                    var cell = grdConnectionParameters[ht.ColumnIndex, ht.RowIndex];
                    //Right clicked a "Value" cell
                    if (cell != null && ht.ColumnIndex == 1)
                    {
                        grdConnectionParameters.ClearSelection();
                        cell.Selected = true;

                        var pt = grdConnectionParameters.PointToScreen(new Point(e.X, e.Y));

                        var p = cell.Tag as OSGeo.MapGuide.ObjectModels.Common.FeatureProviderRegistryFeatureProviderConnectionProperty;
                        if (p.Enumerable)
                            ctxEnumerable.Show(pt.X, pt.Y);
                        else
                            ctxProperty.Show(pt.X, pt.Y);
                    }
                }
            }
        }

        private void pickAnAliasedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdConnectionParameters.SelectedCells.Count != 1)
                return;

            var cell = grdConnectionParameters.SelectedCells[0];

            using (var dlg = new UnmanagedFileBrowser(_service.ResourceService))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    cell.Value = dlg.SelectedItem;
                }
            }
        }

        private void pickAnAliasedDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdConnectionParameters.SelectedCells.Count != 1)
                return;

            var cell = grdConnectionParameters.SelectedCells[0];

            using (var dlg = new UnmanagedFileBrowser(_service.ResourceService))
            {
                dlg.SelectFoldersOnly = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var value = dlg.SelectedItem;
                    cell.Value = value;
                }
            }
        }

        private void useActiveResourceDataFile_Click(object sender, EventArgs e)
        {
            if (grdConnectionParameters.SelectedCells.Count != 1)
                return;

            var cell = grdConnectionParameters.SelectedCells[0];

            var file = resDataCtrl.MarkedFile;
            if (!string.IsNullOrEmpty(file))
            {
                var value = "%MG_DATA_FILE_PATH%" + file; //NOXLATE
                cell.Value = value;
            }
            else
            {
                MessageBox.Show(Strings.NoActiveDataFile);
            }
        }

        private void grdConnectionParameters_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Only care about changes in "Value" cells
            if (e.ColumnIndex == 1)
            {
                var name = grdConnectionParameters[0, e.RowIndex].Value.ToString();
                var value = grdConnectionParameters[e.ColumnIndex, e.RowIndex].Value;
                _fs.SetConnectionProperty(name, value == null ? string.Empty : value.ToString());
            }
        }

        private void lnkSetCredentials_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var prov = _service.FeatureService.GetFeatureProvider(_fs.Provider);
            var connProps = new List<string>();
            foreach (var p in prov.ConnectionProperties)
            {
                if (!p.Enumerable)
                    connProps.Add(p.Name);
            }
            using (var diag = new SetCredentialsDialog(connProps.ToArray()))
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    _fs.SetConnectionProperty(diag.UserProperty, StringConstants.MgUsernamePlaceholder);
                    _fs.SetConnectionProperty(diag.PasswordProperty, StringConstants.MgPasswordPlaceholder);
                    _fs.SetEncryptedCredentials(diag.Username, diag.Password);
                    _service.SyncSessionCopy();
                    InitGrid();
                    resDataCtrl.Init(_service);
                }
            }
        }
    }
}
