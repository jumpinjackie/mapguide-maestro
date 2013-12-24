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

namespace Maestro.Editors.WatermarkDefinition
{
    [ToolboxItem(false)]
    internal partial class WatermarkSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public WatermarkSettingsCtrl()
        {
            InitializeComponent();
        }

        private IWatermarkDefinition _wm;
        private ITilePosition _tile;
        private IXYPosition _xy;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);
            _wm = (IWatermarkDefinition)service.GetEditedResource();

            NumericBinder.BindValueChanged(numRotation, _wm.Appearance, "Rotation");
            NumericBinder.BindValueChanged(numTransparency, _wm.Appearance, "Transparency");

            if (_wm.Position.Type == PositionType.Tile)
                _tile = (ITilePosition)_wm.Position;
            else if (_wm.Position.Type == PositionType.XY)
                _xy = (IXYPosition)_wm.Position;

            if (_tile == null)
            {
                _tile = _wm.CreateTilePosition();
                rdXY.Checked = true;
            }
            else if (_xy == null)
            {
                _xy = _wm.CreateXYPosition();
                rdTile.Checked = true;
            }
        }

        private void OnPositionCheckChanged(object sender, EventArgs e)
        {
            grpPositionSettings.Controls.Clear();
            Control c = null;
            if (rdTile.Checked)
                c = new TilePositionEditor(_tile, _edSvc);
            else if (rdXY.Checked)
                c = new XYPositionEditor(_xy, _edSvc);

            if (c != null)
            {
                c.Dock = DockStyle.Fill;
                grpPositionSettings.Controls.Add(c);
            }

            if (rdTile.Checked)
                _wm.Position = _tile;
            else if (rdXY.Checked)
                _wm.Position = _xy;
        }
    }
}
