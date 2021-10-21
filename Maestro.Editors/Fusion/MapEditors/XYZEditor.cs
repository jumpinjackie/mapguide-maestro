#region Disclaimer / License

// Copyright (C) 2018, Jackie Ng
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

using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Maestro.Editors.Fusion.MapEditors
{
    public partial class XYZEditor : UserControl
    {
        private IMap _map;
        private readonly IEditorService _edSvc;
        private IMapView _initialView;
        private IMapGroup _group;
        private bool _init;

        class ExternalUrl
        {
            public string URL { get; set; }
        }

        private BindingList<ExternalUrl> _urls = new BindingList<ExternalUrl>();

        public XYZEditor(IEditorService edSvc, IMapGroup group, IMap map)
        {
            InitializeComponent();
            _edSvc = edSvc;
            _map = map;
            _group = group;
            try
            {
                _init = true;
                foreach (var url in _map.GetXYZUrls())
                {
                    _urls.Add(new ExternalUrl { URL = url });
                }
                grdUrls.DataSource = _urls;
                txtName.Text = GetName();
            }
            finally
            {
                _init = false;
            }
        }

        private void grdUrls_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_init)
                return;

            SyncCurrentUrls();
            _edSvc.MarkDirty();
        }

        private string GetName()
        {
            var optEl = _map.Extension.Content.FirstOrDefault(el => el.Name == "Options") as XmlNode;
            if (optEl != null)
            {
                var nameEl = optEl.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.Name == "name");
                return nameEl?.InnerText;
            }
            return null;
        }

        private void SetName(string name)
        {
            var optEl = _map.Extension.Content.FirstOrDefault(el => el.Name == "Options") as XmlNode;
            if (optEl != null)
            {
                var nameEl = optEl.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.Name == "name");
                if (nameEl == null)
                {
                    nameEl = _map.CreateExtensionElement("name");
                    optEl.AppendChild(nameEl);
                }
                nameEl.InnerText = name;
                _edSvc.MarkDirty();
            }
        }

        private void SyncCurrentUrls()
        {
            _map.SetXYZUrls(_urls.Select(u => u.URL).ToArray());
        }

        private void btnAddTileSet_Click(object sender, EventArgs e)
        {
            var conn = _edSvc.CurrentConnection;
            using (var picker = new ResourcePicker(conn, ResourcePickerMode.OpenResource, new[] { ResourceTypes.TileSetDefinition.ToString() }))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    ITileSetDefinition tsd = (ITileSetDefinition)conn.ResourceService.GetResource(picker.ResourceID);
                    if (tsd.TileStoreParameters.TileProvider == "XYZ")
                    {
                        var baseGroup = tsd.BaseMapLayerGroups.FirstOrDefault();
                        var mapagent = conn.GetCustomProperty("BaseUrl").ToString(); //NOXLATE
                        mapagent += "mapagent/mapagent.fcgi?OPERATION=GETTILEIMAGE&VERSION=1.2.0&CLIENTAGENT=OpenLayers&USERNAME=Anonymous&MAPDEFINITION=" + tsd.ResourceID + "&BASEMAPLAYERGROUPNAME=" + baseGroup.Name + "&TILECOL=${y}&TILEROW=${x}&SCALEINDEX=${z}";
                        _urls.Add(new ExternalUrl { URL = mapagent });
                        SyncCurrentUrls();
                    }
                    else
                    {
                        MessageBox.Show(Strings.InvalidTileSetForFusionXYZLayer);
                    }
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            SetName(txtName.Text);
        }

        private void grdUrls_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (_init)
                return;

            SyncCurrentUrls();
            _edSvc.MarkDirty();
        }
    }
}
