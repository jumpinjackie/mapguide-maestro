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
using Maestro.Editors.FeatureSource.CoordSys;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.Common;

namespace Maestro.Editors.FeatureSource
{
    [ToolboxItem(false)]
    internal partial class CoordSysOverrideCtrl : EditorBindableCollapsiblePanel
    {
        public CoordSysOverrideCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _ed;
        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            _ed = service;
            _ed.RegisterCustomNotifier(this);
            _fs = (IFeatureSource)_ed.GetEditedResource();

            grdOverrides.AutoGenerateColumns = false;
            UpdateSpatialContextList();
        }

        private void UpdateSpatialContextList()
        {
            grdOverrides.DataSource = new List<ISpatialContextInfo>(_fs.SupplementalSpatialContextInfo);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new CoordSysOverrideDialog(_ed);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _fs.AddSpatialContextOverride(dlg.CsName, dlg.CoordinateSystemWkt);
                UpdateSpatialContextList();
                OnResourceChanged();
            }
        }

        private void grdOverrides_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                grdOverrides.Rows[e.RowIndex].Selected = true;
            }
            btnApplyAll.Enabled = grdOverrides.SelectedRows.Count > 0;
        }

        private void grdOverrides_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnDelete.Enabled = (grdOverrides.SelectedRows.Count == 1);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count != 1)
                return;

            var sc = (ISpatialContextInfo)grdOverrides.SelectedRows[0].DataBoundItem;
            _fs.RemoveSpatialContextOverride(sc);
            UpdateSpatialContextList();
            OnResourceChanged();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count != 1)
                return;

            var sc = (ISpatialContextInfo)grdOverrides.SelectedRows[0].DataBoundItem;
            var dlg = new CoordSysOverrideDialog(_ed);
            dlg.CsName = sc.Name;
            dlg.CoordinateSystemWkt = sc.CoordinateSystem;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                sc.Name = dlg.CsName;
                sc.CoordinateSystem = dlg.CoordinateSystemWkt;
                UpdateSpatialContextList();
                OnResourceChanged();
            }
        }

        private void btnApplyAll_Click(object sender, EventArgs e)
        {
            string wkt = _ed.GetCoordinateSystem();
            if (!string.IsNullOrEmpty(wkt))
            {
                foreach (DataGridViewRow row in grdOverrides.SelectedRows)
                {
                    var sci = (ISpatialContextInfo)row.DataBoundItem;
                    sci.CoordinateSystem = wkt;
                }
                UpdateSpatialContextList();
            }
        }

        private void btnLoadFromSc_Click(object sender, EventArgs e)
        {
            if (grdOverrides.Rows.Count > 0)
            {
                if (MessageBox.Show(Strings.QuestionResetFsOverrideList, Strings.TitleQuestion, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var sco = new List<ISpatialContextInfo>(_fs.SupplementalSpatialContextInfo);
                    foreach (var s in sco)
                    {
                        _fs.RemoveSpatialContextOverride(s);
                    }
                }
                else
                {
                    return;
                }
            }

            var scList = _fs.GetSpatialInfo(false);
            foreach (var sc in scList.SpatialContext)
            {
                _fs.AddSpatialContextOverride(sc.Name, sc.CoordinateSystemWkt);
            }
            UpdateSpatialContextList();
        }
    }
}
