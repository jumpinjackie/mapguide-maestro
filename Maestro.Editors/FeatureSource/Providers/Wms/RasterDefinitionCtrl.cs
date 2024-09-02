#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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

            txtImageFormat.TextChanged += TxtImageFormat_TextChanged;
            chkTransparent.CheckedChanged += ChkTransparent_CheckedChanged;
            cmbBackground.SelectedIndexChanged += CmbBackground_SelectedIndexChanged;
            txtElevation.TextChanged += TxtElevation_TextChanged;
            txtEpsg.TextChanged += TxtEpsg_TextChanged;
            txtTime.TextChanged += TxtTime_TextChanged;

            _config = config;
            _fs = (IFeatureSource)edsvc.GetEditedResource();
            _item = item;
            _edsvc = edsvc;

            this.BindItem(item);
        }

        internal void BindItem(RasterWmsItem item)
        {
            cmbBackground.ResetColors();

            _item = null;

            txtImageFormat.Text = item.ImageFormat;
            chkTransparent.Checked = item.IsTransparent;
            cmbBackground.CurrentColor = item.BackgroundColor;
            txtElevation.Text = item.ElevationDimension;
            txtEpsg.Text = item.SpatialContextName;
            txtTime.Text = item.Time;

            _item = item;

            var names = new List<string>();
            foreach (var layer in item.Layers)
            {
                names.Add(layer.Name);
            }
            txtLayers.Lines = names.ToArray();
            lnkUpdate.Enabled = false;
        }

        private void TxtTime_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.Time = txtTime.Text;
        }

        private void TxtEpsg_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.SpatialContextName = txtEpsg.Text;
        }

        private void TxtElevation_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.ElevationDimension = txtElevation.Text;
        }

        private void CmbBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.BackgroundColor = cmbBackground.CurrentColor;
        }

        private void ChkTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.IsTransparent = chkTransparent.Checked;
        }

        private void TxtImageFormat_TextChanged(object sender, EventArgs e)
        {
            if (_item != null)
                _item.ImageFormat = txtImageFormat.Text;
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