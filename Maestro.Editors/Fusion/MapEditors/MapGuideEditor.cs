#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal partial class MapGuideEditor : UserControl
    {
        private IMap _map;
        private IEditorService _edSvc;
        private IMapView _initialView;
        private IMapGroup _group;
        private bool _init;

        public MapGuideEditor(IEditorService edSvc, IMapGroup group, IMap map)
        {
            InitializeComponent();
            _edSvc = edSvc;
            _map = map;
            _group = group;
            try
            {
                _init = true;
                cmbSelectionColor.ResetColors();
                txtMapDefinition.Text = _map.GetMapDefinition();

                _initialView = group.InitialView;
                chkSingleTiled.Checked = _map.SingleTile;
                chkOverride.Checked = (_initialView != null);
                if (_initialView == null)
                    _initialView = group.CreateInitialView(0.0, 0.0, 0.0);

                txtViewX.Text = _initialView.CenterX.ToString(CultureInfo.InvariantCulture);
                txtViewY.Text = _initialView.CenterY.ToString(CultureInfo.InvariantCulture);
                txtViewScale.Text = _initialView.Scale.ToString(CultureInfo.InvariantCulture);

                var selOverlay = _map.GetValue("SelectionAsOverlay"); //NOXLATE
                var selColor = _map.GetValue("SelectionColor"); //NOXLATE

                if (!string.IsNullOrEmpty(selColor))
                    cmbSelectionColor.CurrentColor = Utility.ParseHTMLColorRGBA(selColor.Substring(2)); //Strip the "0x" part

                if (!string.IsNullOrEmpty(selOverlay))
                {
                    bool b = true;
                    if (bool.TryParse(selOverlay, out b))
                        chkSelectionAsOverlay.Checked = b;
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void txtViewX_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtViewX.Text, out d))
            {
                _initialView.CenterX = d;
                _edSvc.HasChanged();
            }
        }

        private void txtViewY_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtViewY.Text, out d))
            {
                _initialView.CenterY = d;
                _edSvc.HasChanged();
            }
        }

        private void txtViewScale_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtViewScale.Text, out d))
            {
                _initialView.Scale = d;
                _edSvc.HasChanged();
            }
        }

        private void txtMapDefinition_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _map.SetMapDefinition(txtMapDefinition.Text);
            _edSvc.HasChanged();
        }

        private void chkSingleTiled_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _map.SingleTile = chkSingleTiled.Checked;
            _edSvc.HasChanged();
        }

        private void cmbSelectionColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _map.SetValue("SelectionColor", "0x" + Utility.SerializeHTMLColorRGBA(cmbSelectionColor.CurrentColor, true)); //NOXLATE
            _edSvc.HasChanged();
        }

        private void chkSelectionAsOverlay_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _map.SetValue("SelectionAsOverlay", chkSelectionAsOverlay.Checked.ToString().ToLower()); //NOXLATE
            _edSvc.HasChanged();
        }

        private void chkOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (chkOverride.Checked)
                _group.InitialView = _initialView;
            else
                _group.InitialView = null;

            _edSvc.HasChanged();
        }

        private void btnBrowseMdf_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.MapDefinition.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtMapDefinition.Text = picker.ResourceID;
                }
            }
        }
    }
}