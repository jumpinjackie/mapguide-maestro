#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using Maestro.Shared.UI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.WatermarkDefinition
{
    [ToolboxItem(false)]
    internal partial class WatermarkCollectionEditorCtrl : CollapsiblePanel
    {
        private IWatermarkCollection _watermarks;

        private BindingList<IWatermark> _list;

        private IEditorService _edSvc;

        public WatermarkCollectionEditorCtrl(IEditorService service, IWatermarkCollection watermarks)
        {
            InitializeComponent();
            grdWatermarks.AutoGenerateColumns = false;
            _watermarks = watermarks;
            _edSvc = service;
            _list = new BindingList<IWatermark>();
            foreach (var wm in _watermarks.Watermarks)
            {
                _list.Add(wm);
            }
            grdWatermarks.DataSource = _list;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.WatermarkDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    var wdf = (IWatermarkDefinition)_edSvc.ResourceService.GetResource(picker.ResourceID);
                    //var wm = wdf.CreateInstance();
                    var wm = _watermarks.AddWatermark(wdf);
                    var diag = new WatermarkInstanceEditorDialog(_edSvc, wm);
                    if (diag.ShowDialog() == DialogResult.OK)
                    {
                        _list.Add(wm);
                    }
                    else //Undo
                    {
                        _watermarks.RemoveWatermark(wm);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdWatermarks.SelectedRows.Count == 1)
            {
                var wm = grdWatermarks.SelectedRows[0].DataBoundItem as IWatermark;
                if (wm != null)
                {
                    var diag = new WatermarkInstanceEditorDialog(_edSvc, wm);
                    diag.ShowDialog();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (grdWatermarks.SelectedRows.Count == 1)
            {
                var wm = grdWatermarks.SelectedRows[0].DataBoundItem as IWatermark;
                if (wm != null)
                {
                    _list.Remove(wm);
                    _watermarks.RemoveWatermark(wm);
                }
            }
        }

        private void grdWatermarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit.Enabled = btnRemove.Enabled = false;
            if (e.RowIndex >= 0)
            {
                grdWatermarks.Rows[e.RowIndex].Selected = true;
                btnEdit.Enabled = btnRemove.Enabled = true;
            }
        }
    }
}
