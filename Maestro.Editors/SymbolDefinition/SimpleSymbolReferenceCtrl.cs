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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;

namespace Maestro.Editors.SymbolDefinition
{
    [ToolboxItem(false)]
    internal partial class SimpleSymbolReferenceCtrl : UserControl
    {
        private IResourceService _resSvc;
        private IResourceIdReference _symRef;

        private bool _init = false;

        public SimpleSymbolReferenceCtrl(IResourceService resSvc, IResourceIdReference symRef)
        {
            InitializeComponent();
            _resSvc = resSvc;
            _symRef = symRef;

            try
            {
                _init = true;
                txtResourceId.Text = _symRef.ResourceId;
            }
            finally
            {
                _init = false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc,
                                                   ResourceTypes.SymbolDefinition,
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.ResourceID;
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }

        private void txtResourceId_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _symRef.ResourceId = txtResourceId.Text;
        }
    }
}
