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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.WebLayout;

namespace Maestro.Editors.WebLayout
{
    [ToolboxItem(true)]
    internal partial class WebLayout3SettingsCtrl : EditorBindableCollapsiblePanel
    {
        private IWebLayout3 _wl;

        public WebLayout3SettingsCtrl()
        {
            InitializeComponent();
            txtStartupScript.SetHighlighting("JavaScript");
        }

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            try
            {
                _init = true;
                _wl = (IWebLayout3)service.GetEditedResource();

                txtSelectionColor.Text = _wl.SelectionColor;
                numPointBuffer.Value = _wl.PointSelectionBuffer;
                cmbMapImageFormat.SelectedItem = _wl.MapImageFormat;
                cmbSelectionImageFormat.SelectedItem = _wl.SelectionImageFormat;
                txtStartupScript.Text = _wl.StartupScript;
            }
            finally
            {
                _init = false;
            }
        }

        private void btnSelectionColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                var color = colorDialog.Color;
                txtSelectionColor.Text = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColorRGBA(color, true);
            }
        }

        private void numPointBuffer_ValueChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _wl.PointSelectionBuffer = Convert.ToInt32(numPointBuffer.Value);
        }

        private void cmbMapImageFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (cmbMapImageFormat.SelectedItem != null)
                _wl.MapImageFormat = cmbMapImageFormat.SelectedItem.ToString();
        }

        private void cmbSelectionImageFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            if (cmbSelectionImageFormat.SelectedItem != null)
                _wl.SelectionImageFormat = cmbSelectionImageFormat.SelectedItem.ToString();
        }

        private void txtSelectionColor_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _wl.SelectionColor = txtSelectionColor.Text;
        }

        private void txtStartupScript_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _wl.StartupScript = txtStartupScript.Text;
        }
    }
}
