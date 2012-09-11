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
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.Common;

namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    [ToolboxItem(false)]
    internal partial class RasterDefinitionCtrl : UserControl
    {
        private WmsConfigurationDocument _config;
        private RasterWmsItem _item;
        private IEditorService _edsvc;
        private IFeatureSource _fs;

        public RasterDefinitionCtrl(WmsConfigurationDocument config, RasterWmsItem item, IEditorService edsvc)
        {
            InitializeComponent();
            cmbBackground.ResetColors();
            _config = config;

            _fs = (IFeatureSource)edsvc.GetEditedResource();
            _item = item;
            _edsvc = edsvc;

            txtImageFormat.Text = item.ImageFormat;
            chkTransparent.Checked = item.IsTransparent;
            cmbBackground.CurrentColor = item.BackgroundColor;
            txtElevation.Text = item.ElevationDimension;
            txtEpsg.Text = item.SpatialContextName;
            txtTime.Text = item.Time;

            txtImageFormat.TextChanged += (s, e) => { item.ImageFormat = txtImageFormat.Text; };
            chkTransparent.CheckedChanged += (s, e) => { item.IsTransparent = chkTransparent.Checked; };
            cmbBackground.SelectedIndexChanged += (s, e) => { item.BackgroundColor = cmbBackground.CurrentColor; };
            txtElevation.TextChanged += (s, e) => { item.ElevationDimension = txtElevation.Text; };
            txtEpsg.TextChanged += (s, e) => { item.SpatialContextName = txtEpsg.Text; };
            txtTime.TextChanged += (s, e) => { item.Time = txtTime.Text; };

            List<string> names = new List<string>();
            foreach (var layer in item.Layers)
            {
                names.Add(layer.Name);
            }
            txtLayers.Lines = names.ToArray();
            lnkUpdate.Enabled = false;
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _item.RemoveAllLayers();
            foreach (var line in txtLayers.Lines)
            {
                if (!string.IsNullOrEmpty(line) && line.Trim().Length > 0)
                {
                    _item.AddLayer(new WmsLayerDefinition(line.Trim()));
                }
            }
            MessageBox.Show(Strings.WmsLayersUpdated);
            lnkUpdate.Enabled = false;
        }

        private void txtLayers_TextChanged(object sender, EventArgs e)
        {
            lnkUpdate.Enabled = true;
        }

        private void btnSelectCs_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            foreach (var sn in _config.SpatialContexts)
            {
                names.Add(sn.Name);
            }

            var item = GenericItemSelectionDialog.SelectItem(null, null, names.ToArray());
            if (item != null)
            {
                txtEpsg.Text = item;
            }
        }

        private void btnSelectFormat_Click(object sender, EventArgs e)
        {
            string[] formats = { RasterWmsItem.WmsImageFormat.GIF, 
                                 RasterWmsItem.WmsImageFormat.JPG,
                                 RasterWmsItem.WmsImageFormat.PNG,
                                 RasterWmsItem.WmsImageFormat.TIF };
            var item = GenericItemSelectionDialog.SelectItem(null, null, formats);
            if (item != null)
            {
                txtImageFormat.Text = item;
            }
        }
    }
}
