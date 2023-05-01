﻿#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal partial class CommercialMapEditor : UserControl
    {
        private readonly IEditorService _edSvc;
        private readonly IMap _map;
        private readonly bool _init;
        const string BING_MAPS_EXTENSION_NAME = "BingMapKey";

        public CommercialMapEditor(IEditorService edSvc, IMap map, string[] types)
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

                var appDef = (IApplicationDefinition)_edSvc.GetEditedResource();
                var bingMapsKey = appDef.GetValue(BING_MAPS_EXTENSION_NAME);
                if (!string.IsNullOrEmpty(bingMapsKey))
                {
                    txtBingMapsApiKey.Text = bingMapsKey;
                    btnSetBingMapsApiKey.Enabled = false;
                }
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

        private void btnSetBingMapsApiKey_Click(object sender, EventArgs e)
        {
            var appDef = (IApplicationDefinition)_edSvc.GetEditedResource();
            appDef.SetValue(BING_MAPS_EXTENSION_NAME, txtBingMapsApiKey.Text); //NOXLATE
            _edSvc.HasChanged();
            btnSetBingMapsApiKey.Enabled = false;
        }

        private void txtBingMapsApiKey_TextChanged(object sender, EventArgs e)
        {
            btnSetBingMapsApiKey.Enabled = true;
        }
    }
}