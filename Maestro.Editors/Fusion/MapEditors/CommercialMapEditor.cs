#region Disclaimer / License

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

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal partial class CommercialMapEditor : UserControl
    {
        private readonly IEditorService _edSvc;
        private readonly IMap _map;
        private readonly bool _init;
        const string BING_MAPS_EXTENSION_NAME = "BingMapKey";
        internal const string STADIA_MAPS_EXTENSION_NAME = "StadiaMapsKey";

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
                switch (_map.Type)
                {
                    case EditorFactory.Type_Bing:
                        {
                            grpApiKey.Text = Strings.ApiKeyDescBingMaps;
                            var bingMapsKey = appDef.GetValue(BING_MAPS_EXTENSION_NAME);
                            if (!string.IsNullOrEmpty(bingMapsKey))
                            {
                                txtApiKey.Text = bingMapsKey;
                                btnSetApiKey.Enabled = false;
                            }
                        }
                        break;
                    case EditorFactory.Type_Stamen:
                    case EditorFactory.Type_StadiaMaps:
                        {
                            grpApiKey.Text = Strings.ApiKeyDescStadiaMaps;
                            var stadiaMapsKey = appDef.GetValue(STADIA_MAPS_EXTENSION_NAME);
                            if (!string.IsNullOrEmpty(stadiaMapsKey))
                            {
                                txtApiKey.Text = stadiaMapsKey;
                                btnSetApiKey.Enabled = false;
                            }
                        }
                        break;
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

        private void btnSetApiKey_Click(object sender, EventArgs e)
        {
            var appDef = (IApplicationDefinition)_edSvc.GetEditedResource();
            switch (_map.Type)
            {
                case EditorFactory.Type_Bing:
                    {
                        appDef.SetValue(BING_MAPS_EXTENSION_NAME, txtApiKey.Text);
                        _edSvc.HasChanged();
                    }
                    break;
                case EditorFactory.Type_Stamen:
                case EditorFactory.Type_StadiaMaps:
                    {
                        appDef.SetValue(STADIA_MAPS_EXTENSION_NAME, txtApiKey.Text);
                        _edSvc.HasChanged();
                    }
                    break;
            }
            btnSetApiKey.Enabled = false;
        }

        private void txtApiKey_TextChanged(object sender, EventArgs e)
        {
            btnSetApiKey.Enabled = true;
        }
    }
}