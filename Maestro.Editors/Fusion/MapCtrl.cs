#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using Maestro.Editors.Fusion.MapEditors;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion
{
    [ToolboxItem(false)]
    internal partial class MapCtrl : UserControl, INotifyResourceChanged
    {
        private MapCtrl()
        {
            InitializeComponent();
        }

        private IMapWidget _widget;
        private readonly IMapGroup _group;
        private IApplicationDefinition _appDef;
        private IEditorService _edSvc;

        private class MapModel
        {
            private readonly IMap _map;

            public MapModel(IMap map)
            {
                _map = map;
            }

            public IMap Map => _map;

            public override string ToString()
            {
                var mgOpts = _map.OverlayOptions;
                var cmsOpts = _map.CmsMapOptions;
                if (cmsOpts != null)
                {
                    if (string.IsNullOrEmpty(_map.Type))
                        return Strings.CmsGeneric;
                    else
                        return $"{_map.Type} ({cmsOpts.Type})"; //NOXLATE
                }
                else if (mgOpts != null)
                {
                    return _map.Type;
                }
                else if (string.IsNullOrEmpty(_map.Type))
                {
                    return Strings.CmsGeneric;
                }
                else
                {
                    return _map.Type;
                }
            }
        }

        private readonly BindingList<MapModel> _models;

        public MapCtrl(IApplicationDefinition appDef, IMapGroup group, IEditorService edService, IMapWidget widget)
            : this()
        {
            _appDef = appDef;
            _widget = widget;
            _group = group;
            _edSvc = edService;
            _edSvc.RegisterCustomNotifier(this);
            _models = new BindingList<MapModel>();
            lstMaps.DataSource = _models;
            UpdateMapList();
            txtMapId.Text = group.id;

            LoadMapOptions();
        }

        private void UpdateMapList()
        {
            _models.Clear();
            foreach (var m in _group.Map.Select(x => new MapModel(x)))
            {
                _models.Add(m);
            }
        }

        private void LoadMapOptions()
        {
            foreach (var option in EditorFactory.GetAvailableOptions(_edSvc.CurrentConnection.SiteVersion, _group))
            {
                var ed = option;
                btnNewMap.DropDown.Items.Add(ed.Name, null, (s, e) =>
                {
                    bool bAddedCommercialLayer = false;
                    var map = ed.Action();
                    switch (map.Type)
                    {
                        case EditorFactory.Type_Google:
                            _appDef.SetValue("GoogleScript", EditorFactory.GOOGLE_URL);
                            bAddedCommercialLayer = true;
                            break;
                        case EditorFactory.Type_Bing:
                            bAddedCommercialLayer = true;
                            break;
                        case EditorFactory.Type_OSM:
                            bAddedCommercialLayer = true;
                            break;
                        case EditorFactory.Type_Stamen:
                            bAddedCommercialLayer = true;
                            break;
                    }

                    _group.AddMap(map);
                    _models.Add(new MapModel(map));
                    if (bAddedCommercialLayer)
                    {
                        foreach (var m in _group.Map)
                        {
                            if (m.Type == EditorFactory.Type_MapGuide)
                            {
                                m.OverlayOptions = m.CreateOverlayOptions(false, true, "EPSG:900913"); //NOXLATE
                            }
                        }
                    }
                    else
                    {
                        foreach (var m in _group.Map)
                        {
                            if (m.Type == EditorFactory.Type_MapGuide)
                            {
                                m.OverlayOptions = null;
                            }
                        }
                    }
                    _edSvc.HasChanged();
                });
            }
        }

        private void OnResourceChanged()
        {
            this.ResourceChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        private void txtMapId_TextChanged(object sender, EventArgs e)
        {
            _group.id = txtMapId.Text;
            if (_widget != null && txtMapId.Text != _widget.MapId)
            {
                _widget.MapId = txtMapId.Text;
            }
        }

        private void lstMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            var map = lstMaps.SelectedItem as MapModel;
            if (map != null)
            {
                btnRemoveMap.Enabled = btnMapUp.Enabled = btnMapDown.Enabled = true;

                grpChildMap.Controls.Clear();
                var control = EditorFactory.GetEditor(_edSvc, _group, map.Map);
                control.Dock = DockStyle.Fill;
                grpChildMap.Controls.Add(control);
            }
            else
            {
                btnRemoveMap.Enabled = btnMapUp.Enabled = btnMapDown.Enabled = false;
            }
        }

        private void btnRemoveMap_Click(object sender, EventArgs e)
        {
            var map = lstMaps.SelectedItem as MapModel;
            if (map != null)
            {
                _models.Remove(map);
                _group.RemoveMap(map.Map);
            }
        }

        private void btnMapUp_Click(object sender, EventArgs e)
        {
            var map = lstMaps.SelectedItem as MapModel;
            if (map != null)
            {
                int idx = _models.IndexOf(map);
                if (_group.MoveUp(map.Map))
                {
                    idx--;
                    UpdateMapList();
                    if (idx >= 0 && idx < lstMaps.Items.Count - 1)
                        lstMaps.SelectedIndex = idx;
                }
            }
        }

        private void btnMapDown_Click(object sender, EventArgs e)
        {
            var map = lstMaps.SelectedItem as MapModel;
            if (map != null)
            {
                int idx = _models.IndexOf(map);
                if (_group.MoveDown(map.Map))
                {
                    idx++;
                    UpdateMapList();
                    if (idx >= 0 && idx < lstMaps.Items.Count - 1)
                        lstMaps.SelectedIndex = idx;
                }
            }
        }
    }
}