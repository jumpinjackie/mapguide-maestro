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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using Maestro.Editors.SymbolDefinition.GraphicsEditors;

namespace Maestro.Editors.WatermarkDefinition
{
    [ToolboxItem(false)]
    internal partial class WatermarkContentCtrl : EditorBindableCollapsiblePanel
    {
        public WatermarkContentCtrl()
        {
            InitializeComponent();
        }

        private IWatermarkDefinition _wmd;
        private ISimpleSymbolDefinition _sym;

        private ITextGraphic _text;
        private IImageGraphic _image;

        private bool _init = false;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _init = true;
            _edSvc = service;
            try
            {
                _wmd = (IWatermarkDefinition)service.GetEditedResource();
                _sym = (ISimpleSymbolDefinition)_wmd.Content;

                //NOTE: We are assuming there is only one graphic element here.
                foreach (var g in _sym.Graphics)
                {
                    if (g.Type == GraphicElementType.Text)
                        _text = (ITextGraphic)g;
                    else if (g.Type == GraphicElementType.Image)
                        _image = (IImageGraphic)g;
                }

                if (_text != null || _image != null)
                {
                    if (_text != null)
                        rdText.Checked = true;
                    else if (_image != null)
                        rdImage.Checked = true;

                    CheckEditStates();
                }
                else
                {
                    //Text, I choose you
                    rdText.Checked = true;
                    CheckEditStates();
                    
                    _text = _sym.CreateTextGraphics();
                    _image = _sym.CreateImageGraphics();
                    SetActiveGraphicElement(_text);
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void wmdTypeCheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            CheckEditStates();
            if (rdText.Checked)
                SetActiveGraphicElement(_text);
            else if (rdImage.Checked)
                SetActiveGraphicElement(_image);
        }

        private void CheckEditStates()
        {
            btnEditImage.Enabled = btnEditText.Enabled = false;
            if (rdText.Checked)
                btnEditText.Enabled = true;
            else if (rdImage.Checked)
                btnEditImage.Enabled = true;
        }

        private void SetActiveGraphicElement(IGraphicBase g)
        {
            _sym.ClearGraphics();
            _sym.AddGraphics(g);
            OnResourceChanged();
        }

        private void btnEditText_Click(object sender, EventArgs e)
        {
            var diag = new TextDialog(this, _sym, _text);
            diag.ShowDialog(this);
            OnResourceChanged();
        }

        private void btnEditImage_Click(object sender, EventArgs e)
        {
            var diag = new ImageDialog(this, _edSvc.ResourceService, _sym, _image);
            diag.ShowDialog(this);
            OnResourceChanged();
        }
    }
}
