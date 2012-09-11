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
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Raster
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class RasterLayerVisibilitySectionCtrl : EditorBindableCollapsiblePanel
    {
        public RasterLayerVisibilitySectionCtrl()
        {
            InitializeComponent();
            cmbBackgroundColor.ResetColors();
            cmbForegroundColor.ResetColors();
        }

        private IEditorService _edsvc;
        private IRasterLayerDefinition _rl;
        private IGridScaleRange _activeRange;
        private IGridColorStyle _colorStyle;

        private IGridColorRule _foregroundColor;
        private IGridColorRule _backgroundColor;

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            _init = true;
            try
            {
                _edsvc = service;
                _edsvc.RegisterCustomNotifier(this);
                _rl = (IRasterLayerDefinition)((ILayerDefinition)service.GetEditedResource()).SubLayer;

                _activeRange = _rl.GetScaleRangeAt(0);
                _colorStyle = _activeRange.ColorStyle;

                _backgroundColor = _colorStyle.GetColorRuleAt(0);
                _foregroundColor = _colorStyle.GetColorRuleAt(1);

                txtVisibleFrom.Text = (_activeRange.MinScale.HasValue ? _activeRange.MinScale.Value : 0).ToString();
                txtRebuildFactor.Text = _activeRange.RebuildFactor.ToString();
                cmbVisibleTo.Text = (_activeRange.MaxScale.HasValue ? _activeRange.MaxScale.Value.ToString() : Strings.Infinity);

                string fg = _foregroundColor.Color.GetValue();
                string bg = _backgroundColor.Color.GetValue();

                if (!string.IsNullOrEmpty(fg))
                    cmbForegroundColor.CurrentColor = Utility.ParseHTMLColor(fg);
                if (!string.IsNullOrEmpty(bg))
                    cmbBackgroundColor.CurrentColor = Utility.ParseHTMLColor(bg);
            }
            finally
            {
                _init = false;
            }
        }

        private void txtVisibleFrom_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtVisibleFrom.Text, NumberStyles.Any, CultureInfo.CurrentUICulture, out d))
            {
                _activeRange.MinScale = d;
            }
            else
            {
                _activeRange.MinScale = null;
            }
            OnResourceChanged();
        }

        private void cmbVisibleTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            cmbVisibleTo_TextChanged(sender, e);
        }

        private void txtRebuildFactor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtRebuildFactor.Text, NumberStyles.Any, CultureInfo.CurrentUICulture, out d))
                _activeRange.RebuildFactor = d;
            else
                _activeRange.RebuildFactor = 1;
            OnResourceChanged();
        }

        private void cmbForegroundColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _foregroundColor.Color.SetValue(Utility.SerializeHTMLColor(cmbForegroundColor.CurrentColor, false));
            OnResourceChanged();
        }

        private void cmbBackgroundColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _backgroundColor.Color.SetValue(Utility.SerializeHTMLColor(cmbBackgroundColor.CurrentColor, false));
            OnResourceChanged();
        }

        private void cmbVisibleTo_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(cmbVisibleTo.Text, NumberStyles.Any, CultureInfo.CurrentUICulture, out d))
            {
                _activeRange.MaxScale = d;
            }
            else
            {
                _activeRange.MaxScale = null;
            }
            OnResourceChanged();
        }
    }
}
