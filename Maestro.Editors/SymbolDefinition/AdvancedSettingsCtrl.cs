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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace Maestro.Editors.SymbolDefinition
{
    [ToolboxItem(false)]
    internal partial class AdvancedSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public AdvancedSettingsCtrl()
        {
            InitializeComponent();
        }

        private ISimpleSymbolDefinition _sym;
        private IEditorService _edSvc;
        private bool _init = false;

        private IResizeBox _rbox;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);
            _sym = (ISimpleSymbolDefinition)_edSvc.GetEditedResource();
            _rbox = _sym.ResizeBox;
            try
            {
                _init = true;

                chkEnableResizeBox.Checked = (_rbox != null);
                grpResizeBox.Enabled = chkEnableResizeBox.Checked;

                if (_rbox == null)
                    _rbox = _sym.CreateResizeBox();

                symGrowControl.Items = SymbolField.GetItems<GrowControl>();

                symGrowControl.Bind(_rbox, "GrowControl");
                symHeight.Bind(_rbox, "SizeY");
                symWidth.Bind(_rbox, "SizeX");
                symXCoord.Bind(_rbox, "PositionX");
                symYCoord.Bind(_rbox, "PositionY");
            }
            finally
            {
                _init = false;
            }
        }

        private void chkEnableResizeBox_CheckedChanged(object sender, EventArgs e)
        {
            grpResizeBox.Enabled = chkEnableResizeBox.Checked;
            if (_init)
                return;

            if (chkEnableResizeBox.Checked)
                _sym.ResizeBox = _rbox;
            else
                _sym.ResizeBox = null;
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            OnResourceChanged();
        }

        private void OnRequestBrowse(SymbolField sender)
        {
            ParameterSelector.ShowParameterSelector(_sym.ParameterDefinition.Parameter, sender);
        }
    }
}
