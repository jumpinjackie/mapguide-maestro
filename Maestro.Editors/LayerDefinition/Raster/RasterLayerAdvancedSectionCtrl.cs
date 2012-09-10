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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Raster
{
    [ToolboxItem(false)]
    internal partial class RasterLayerAdvancedSectionCtrl : EditorBindableCollapsiblePanel
    {
        public RasterLayerAdvancedSectionCtrl()
        {
            InitializeComponent();
            cmbTransparencyColor.ResetColors();
            cmbSurfaceDefaultColor.ResetColors();
        }

        private IEditorService _edsvc;
        private IRasterLayerDefinition _rl;
        private IGridScaleRange _activeRange;

        private IGridColorStyle _colorStyle;
        private IGridSurfaceStyle _surfaceStyle;
        private IHillShade _hillShade;

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            _init = true;
            try
            {
                _edsvc = service;
                _rl = (IRasterLayerDefinition)((ILayerDefinition)service.GetEditedResource()).SubLayer;
                _edsvc.RegisterCustomNotifier(this);

                //Set active range. There has to be one!
                _activeRange = _rl.GetScaleRangeAt(0);

                //Assign color style. If this range has none, we'll create
                //one and assign that. Changing the checkbox will attach/detach
                //this object
                if (_activeRange.ColorStyle == null)
                    _colorStyle = _activeRange.CreateColorStyle();
                else
                    _colorStyle = _activeRange.ColorStyle;

                //Assign surface style. If this range has none, we'll create
                //one and assign that. Changing the checkbox will attach/detach
                //this object
                if (_activeRange.SurfaceStyle == null)
                    _surfaceStyle = _activeRange.CreateSurfaceStyle();
                else
                    _surfaceStyle = _activeRange.SurfaceStyle;

                if (_colorStyle.HillShade == null)
                    _hillShade = _colorStyle.CreateHillShade();
                else
                    _hillShade = _colorStyle.HillShade;

                txtBrightnessFactor.Text = _colorStyle.BrightnessFactor.HasValue ? _colorStyle.BrightnessFactor.Value.ToString() : "0"; //NOXLATE
                txtContrastFactor.Text = _colorStyle.ContrastFactor.HasValue ? _colorStyle.ContrastFactor.Value.ToString() : "0"; //NOXLATE

                if (!string.IsNullOrEmpty(_colorStyle.TransparencyColor))
                {
                    cmbTransparencyColor.CurrentColor = Utility.ParseHTMLColor(_colorStyle.TransparencyColor);
                }

                txtHillshadeAltitude.Text = _hillShade.Altitude.ToString();
                txtHillshadeAzimuth.Text = _hillShade.Azimuth.ToString();
                txtHillshadeBand.Text = _hillShade.Band;
                txtHillshadeScaleFactor.Text = _hillShade.ScaleFactor.ToString();

                txtSurfaceBand.Text = _surfaceStyle.Band;
                txtSurfaceScaleFactor.Text = _surfaceStyle.ScaleFactor.ToString();
                txtSurfaceZeroValue.Text = _surfaceStyle.ZeroValue.ToString();

                if (!string.IsNullOrEmpty(_surfaceStyle.DefaultColor))
                    cmbSurfaceDefaultColor.CurrentColor = Utility.ParseHTMLColor(_surfaceStyle.DefaultColor);

                chkAdvanced.Checked = _colorStyle.ContrastFactor.HasValue && _colorStyle.BrightnessFactor.HasValue && !string.IsNullOrEmpty(_colorStyle.TransparencyColor);
                txtBrightnessFactor.Enabled = txtContrastFactor.Enabled = cmbTransparencyColor.Enabled = chkAdvanced.Checked;
                EnableSurface.Checked = (_activeRange.SurfaceStyle != null);
                EnableHillshade.Checked = (_colorStyle.HillShade != null);
            }
            finally
            {
                _init = false;
            }
        }

        private void chkAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (chkAdvanced.Checked)
            {
                txtBrightnessFactor.Enabled = txtContrastFactor.Enabled = cmbTransparencyColor.Enabled = true;
                //Restore values from UI fields, can't call the actual event handler methods
                //because _init = true so they do nothing
                double d;
                if (double.TryParse(txtBrightnessFactor.Text, out d))
                    _colorStyle.BrightnessFactor = d;
                else
                    _colorStyle.BrightnessFactor = null;
                if (double.TryParse(txtContrastFactor.Text, out d))
                    _colorStyle.ContrastFactor = d;
                else
                    _colorStyle.ContrastFactor = null;
                _colorStyle.TransparencyColor = Utility.SerializeHTMLColor(cmbTransparencyColor.CurrentColor, false);
                //Check if attached
                EnableSurface.Checked = (_activeRange.SurfaceStyle != null);
                EnableHillshade.Checked = (_colorStyle.HillShade != null);
            }
            else
            {
                _colorStyle.BrightnessFactor = null;
                _colorStyle.ContrastFactor = null;
                _colorStyle.TransparencyColor = null;

                txtBrightnessFactor.Enabled = txtContrastFactor.Enabled = cmbTransparencyColor.Enabled = false;

                //Detach
                EnableSurface.Checked = false;
                EnableHillshade.Checked = false;
                _colorStyle.HillShade = null;
                _activeRange.SurfaceStyle = null;
            }
            OnResourceChanged();
        }

        private void EnableHillshade_CheckedChanged(object sender, EventArgs e)
        {
            HillshadeGroup.Enabled = EnableHillshade.Checked;

            if (_init)
                return;

            if (HillshadeGroup.Enabled)
            {
                _colorStyle.HillShade = _hillShade;
            }
            else
            {
                _colorStyle.HillShade = null;
            }
        }

        private void EnableSurface_CheckedChanged(object sender, EventArgs e)
        {
            SurfaceGroup.Enabled = EnableSurface.Checked;

            if (_init)
                return;

            if (SurfaceGroup.Enabled)
            {
                _activeRange.SurfaceStyle = _surfaceStyle;
            }
            else
            {
                _activeRange.SurfaceStyle = null;
            }
        }

        private void txtBrightnessFactor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtBrightnessFactor.Text, out d))
                _colorStyle.BrightnessFactor = d;
            else
                _colorStyle.BrightnessFactor = null;

            OnResourceChanged();
        }

        private void txtContrastFactor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtContrastFactor.Text, out d))
                _colorStyle.ContrastFactor = d;
            else
                _colorStyle.ContrastFactor = null;

            OnResourceChanged();
        }

        private void cmbTransparencyColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _colorStyle.TransparencyColor = Utility.SerializeHTMLColor(cmbTransparencyColor.CurrentColor, false);
            OnResourceChanged();
        }

        private void txtHillshadeAltitude_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtHillshadeAltitude.Text, out d))
            {
                _hillShade.Altitude = d;
                OnResourceChanged();
            }
        }

        private void txtHillshadeAzimuth_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtHillshadeAzimuth.Text, out d))
            {
                _hillShade.Azimuth = d;
                OnResourceChanged();
            }
        }

        private void txtHillshadeBand_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _hillShade.Band = txtHillshadeBand.Text;
            OnResourceChanged();
        }

        private void txtHillshadeScaleFactor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtHillshadeScaleFactor.Text, out d))
            {
                _hillShade.ScaleFactor = d;
                OnResourceChanged();
            }
        }

        private void txtSurfaceBand_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _surfaceStyle.Band = txtSurfaceBand.Text;
            OnResourceChanged();
        }

        private void txtSurfaceZeroValue_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtSurfaceZeroValue.Text, out d))
            {
                _surfaceStyle.ZeroValue = d;
                OnResourceChanged();
            }
        }

        private void cmbSurfaceDefaultColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _surfaceStyle.DefaultColor = Utility.SerializeHTMLColor(cmbSurfaceDefaultColor.CurrentColor, false);
            OnResourceChanged();
        }

        private void txtSurfaceScaleFactor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtSurfaceScaleFactor.Text, out d))
            {
                _surfaceStyle.ScaleFactor = d;
                OnResourceChanged();
            }
        }
    }
}
