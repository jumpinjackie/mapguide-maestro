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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System.Diagnostics;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal partial class CommercialMapEditor : UserControl
    {
        private IEditorService _edSvc;
        private IMap _map;
        private bool _init;

        public CommercialMapEditor(IEditorService edSvc, IMap map, string [] types)
        {
            InitializeComponent();
            _edSvc = edSvc;
            _map = map;
            try
            {
                _init = true;
                txtType.Text = map.Type;
                var opts = map.CmsMapOptions;
                Debug.Assert(opts != null);
                txtName.Text = opts.Name;
                txtSubType.Text = opts.Type;
            }
            finally
            {
                _init = false;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _map.CmsMapOptions.Name = txtName.Text;
            _edSvc.HasChanged();
        }
    }
}
