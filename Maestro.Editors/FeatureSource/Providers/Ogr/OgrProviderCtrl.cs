#region Disclaimer / License

// Copyright (C) 2018, Jackie Ng
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;

namespace Maestro.Editors.FeatureSource.Providers.Ogr
{
    [ToolboxItem(false)]
    public partial class OgrProviderCtrl : EditorBindableCollapsiblePanel
    {
        public OgrProviderCtrl()
        {
            InitializeComponent();
        }

        const string P_DATASOURCE = "DataSource";
        const string P_READONLY = "ReadOnly";

        private IEditorService _service;
        private IFeatureSource _fs;
        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            _init = true;
            _service = service;
            _fs = (IFeatureSource)_service.GetEditedResource();

            var values = _fs.GetConnectionProperties();

            txtDataSource.Text = values[P_DATASOURCE];
            chkReadOnly.Checked = values[P_READONLY] == "TRUE";

            var prov = _service.CurrentConnection.FeatureService.GetFeatureProvider("OSGeo.OGR");
            foreach (var p in prov.ConnectionProperties.Where(p => p.Name != P_DATASOURCE && p.Name != P_READONLY))
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

                grdOtherProperties.Rows.Add(row);
            }

            _init = false;
        }

        private List<DataGridViewCell> pwdCells = new List<DataGridViewCell>();

        private bool IsPasswordCell(DataGridViewCell cell)
        {
            return pwdCells.Contains(cell);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtConnectionStatus.Text = string.Empty;
            //Flush back to session before testing
            _service.SyncSessionCopy();
            string result = _service.CurrentConnection.FeatureService.TestConnection(_fs.ResourceID);

            txtConnectionStatus.Text = string.Format(Strings.FdoConnectionStatus, result);
        }

        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _fs.SetConnectionProperty(P_READONLY, chkReadOnly.Checked ? "TRUE" : "FALSE");
        }

        private void txtDataSource_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _fs.SetConnectionProperty(P_DATASOURCE, txtDataSource.Text);
        }

        private void grdOtherProperties_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = grdOtherProperties.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell != null && IsPasswordCell(cell) && cell.Value != null)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    Graphics g = e.Graphics;
                    g.DrawString(new string('*', cell.Value.ToString().Length), this.Font, new SolidBrush(Color.Black), e.CellBounds); //NOXLATE
                    e.Handled = true;
                }
            }
        }

        private void grdOtherProperties_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = grdOtherProperties.SelectedCells[0];
            if (cell != null)
            {
                TextBox t = e.Control as TextBox;
                if (t != null)
                {
                    t.UseSystemPasswordChar = IsPasswordCell(cell);
                }
            }
        }

        private void grdOtherProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Only care about changes in "Value" cells
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var name = grdOtherProperties[0, e.RowIndex].Value.ToString();
                var value = grdOtherProperties[e.ColumnIndex, e.RowIndex].Value;
                _fs.SetConnectionProperty(name, value == null ? string.Empty : value.ToString());
            }
        }
    }
}
