#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using Maestro.Shared.UI;
using System.Globalization;
using System.Collections.Specialized;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Diagnostics;
using Maestro.Editors.Fusion.MapEditors;

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
        private IMapGroup _group;
        private IApplicationDefinition _appDef;
        private IEditorService _edSvc;

        class MapModel
        {
            private IMap _map;

            public MapModel(IMap map)
            {
                _map = map;
            }

            public IMap Map
            {
                get { return _map; }
            }

            public override string ToString()
            {
                var mgOpts = _map.OverlayOptions;
                var cmsOpts = _map.CmsMapOptions;
                if (cmsOpts != null)
                {
                    if (string.IsNullOrEmpty(_map.Type))
                        return Strings.CmsGeneric;
                    else
                        return _map.Type + " (" + cmsOpts.Type + ")";
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

        private BindingList<MapModel> _models;

        public MapCtrl(IApplicationDefinition appDef, IMapGroup group, IEditorService edService, IMapWidget widget) : this() 
        {
            _appDef = appDef;
            _widget = widget;
            _group = group;
            _edSvc = edService;
            _edSvc.RegisterCustomNotifier(this);
            _models = new BindingList<MapModel>();
            lstMaps.DataSource = _models;
            foreach (var m in group.Map.Select(x => new MapModel(x)))
            {
                _models.Add(m);
            }
            txtMapId.Text = _widget.MapId;

            LoadMapOptions();
        }

        private void LoadMapOptions()
        {
            foreach (var option in EditorFactory.GetAvailableOptions(_group))
            {
                var ed = option;
                btnNewMap.DropDown.Items.Add(ed.Name, null, (s, e) =>
                {
                    var map = ed.Action();
                    _group.AddMap(map);
                    _models.Add(new MapModel(map));
                });
            }
        }

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        private void txtMapId_TextChanged(object sender, EventArgs e)
        {
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
                grpChildMap.Controls.Clear();
                var control = EditorFactory.GetEditor(_edSvc, _group, map.Map);
                control.Dock = DockStyle.Fill;
                grpChildMap.Controls.Add(control);
            }
        }
    }
}
