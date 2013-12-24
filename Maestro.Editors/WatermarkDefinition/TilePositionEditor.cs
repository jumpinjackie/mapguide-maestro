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
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;

namespace Maestro.Editors.WatermarkDefinition
{
    [ToolboxItem(false)]
    internal partial class TilePositionEditor : UserControl
    {
        private ITilePosition _pos;
        private IEditorService _edSvc;

        private bool _init = false;

        public TilePositionEditor(ITilePosition pos, IEditorService service)
        {
            InitializeComponent();
            _pos = pos;
            _edSvc = service;

            try
            {
                _init = true;

                cmbHorizontalAlignment.DataSource = Enum.GetValues(typeof(HorizontalAlignmentType));
                cmbVerticalAlignment.DataSource = Enum.GetValues(typeof(VerticalAlignmentType));
                cmbHorizontalUnits.DataSource = Enum.GetValues(typeof(UnitType));
                cmbVerticalUnits.DataSource = Enum.GetValues(typeof(UnitType));

                numTileWidth.Value = Convert.ToDecimal(_pos.TileWidth);
                numTileHeight.Value = Convert.ToDecimal(_pos.TileHeight);

                cmbVerticalAlignment.SelectedItem = _pos.VerticalPosition.Alignment;
                cmbVerticalUnits.SelectedItem = _pos.VerticalPosition.Unit;
                numVerticalOffset.Value = Convert.ToDecimal(_pos.VerticalPosition.Offset);

                cmbHorizontalUnits.SelectedItem = _pos.HorizontalPosition.Unit;
                cmbHorizontalAlignment.SelectedItem = _pos.HorizontalPosition.Alignment;
                numHorizontalOffset.Value = Convert.ToDecimal(_pos.HorizontalPosition.Offset);
            }
            finally
            {
                _init = false;
            }
        }

        private void numTileWidth_ValueChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.TileWidth = Convert.ToDouble(numTileWidth.Value);
            _edSvc.MarkDirty();
        }

        private void numTileHeight_ValueChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.TileHeight = Convert.ToDouble(numTileHeight.Value);
            _edSvc.MarkDirty();
        }

        private void numHorizontalOffset_ValueChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.HorizontalPosition.Offset = Convert.ToDouble(numHorizontalOffset.Value);
            _edSvc.MarkDirty();
        }

        private void cmbHorizontalUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.HorizontalPosition.Unit = (UnitType)cmbHorizontalUnits.SelectedItem;
            _edSvc.MarkDirty();
        }

        private void cmbHorizontalAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.HorizontalPosition.Alignment = (HorizontalAlignmentType)cmbHorizontalAlignment.SelectedItem;
            _edSvc.MarkDirty();
        }

        private void numVerticalOffset_ValueChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.VerticalPosition.Offset = Convert.ToDouble(numVerticalOffset.Value);
            _edSvc.MarkDirty();
        }

        private void cmbVerticalUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.VerticalPosition.Unit = (UnitType)cmbVerticalUnits.SelectedItem;
            _edSvc.MarkDirty();
        }

        private void cmbVerticalAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init || _pos == null)
                return;

            _pos.VerticalPosition.Alignment = (VerticalAlignmentType)cmbVerticalAlignment.SelectedItem;
            _edSvc.MarkDirty();
        }
    }
}
