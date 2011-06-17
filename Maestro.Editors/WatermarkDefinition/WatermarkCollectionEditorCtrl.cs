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
    public partial class WatermarkCollectionEditorCtrl : CollapsiblePanel
    {
        private IWatermarkCollection _watermarks;

        private BindingList<IWatermark> _list;

        private IResourceService _resSvc;

        public WatermarkCollectionEditorCtrl(IResourceService resSvc, IWatermarkCollection watermarks)
        {
            InitializeComponent();
            _watermarks = watermarks;
            _resSvc = resSvc;
            _list = new BindingList<IWatermark>();
            foreach (var wm in _watermarks.Watermarks)
            {
                _list.Add(wm);
            }
            grdWatermarks.DataSource = _list;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc, ResourceTypes.WatermarkDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var wdf = (IWatermarkDefinition)_resSvc.GetResource(picker.ResourceID);
                    var wm = wdf.CreateInstance();
                    var diag = new WatermarkInstanceEditorDialog(_resSvc, wm);
                    if (diag.ShowDialog() == DialogResult.OK)
                    {
                        _list.Add(wm);
                        _watermarks.AddWatermark(wm);
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
                    var diag = new WatermarkInstanceEditorDialog(_resSvc, wm);
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
    }
}
