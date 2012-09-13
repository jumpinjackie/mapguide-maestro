#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Editors.Fusion
{
    [ToolboxItem(false)]
    internal partial class MenuCtrl : UserControl
    {
        private MenuCtrl()
        {
            InitializeComponent();
        }

        private IFlyoutItem _menu;
        private IEditorService _edSvc;
        private bool _init = false;

        public MenuCtrl(IFlyoutItem menu, IEditorService edSvc)
            : this()
        {
            _menu = menu;
            _edSvc = edSvc;
            _init = true;
            try
            {
                txtMenuLabel.Text = _menu.Label;
                txtImageClass.Text = _menu.ImageClass;
                txtImageUrl.Text = _menu.ImageUrl;
                txtTooltip.Text = _menu.Tooltip;
            }
            finally
            {
                _init = false;
            }
        }

        private void txtMenuLabel_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _menu.Label = txtMenuLabel.Text;
        }

        private void txtTooltip_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _menu.Tooltip = txtTooltip.Text;
        }

        private void txtImageUrl_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _menu.ImageUrl = txtImageUrl.Text;
        }

        private void txtImageClass_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _menu.ImageClass = txtImageClass.Text;
        }
    }
}
