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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Maestro.MapViewer;
using OSGeo.MapGuide.MaestroAPI.Mapping;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// Description of LiveMapEditorDrawOrder.
    /// </summary>
    public partial class LiveMapEditorDrawOrder : UserControl
    {
        public LiveMapEditorDrawOrder()
        {
            InitializeComponent();
        }
        
        private RuntimeMap _map;
        private IMapViewer _viewer;
        
        public IMapViewer Viewer
        {
            get { return _viewer; }
            set 
            {
                if (_viewer != value) 
                {
                    if (_viewer != null)
                        _viewer.MapLoaded -= OnMapLoaded;
                    UnbindMap();
                    _viewer = value;
                    if (_viewer != null)
                    {
                        _map = _viewer.GetMap();
                        BindMap();
                        _viewer.MapLoaded += OnMapLoaded;
                    }
                }
            }
        }

        void OnMapLoaded(object sender, EventArgs e)
        {
            _map = _viewer.GetMap();
            BindMap();
        }
        
        private void OnMapLayersChanged(object sender, EventArgs e)
        {
            RefreshLayerList();
        }
        
        private void UnbindMap() 
        {
            if (_map == null)
                return;

            _map.Layers.CollectionChanged -= OnMapLayersChanged;
        }
        
        private void BindMap()
        {
            if (_map == null)
                return;

            RefreshLayerList();
            _map.Layers.CollectionChanged += OnMapLayersChanged;
        }

        private void RefreshLayerList()
        {
            try
            {
                bSuppressSelectedIndexChanged = true;
                var item = lstDrawOrder.SelectedItem as RuntimeMapLayer;
                lstDrawOrder.DataSource = _map.Layers.OrderBy(x => x.DisplayOrder).ToArray();
                if (item != null)
                {
                    var idx = lstDrawOrder.FindStringExact(item.LegendLabel);
                    if (idx >= 0)
                        lstDrawOrder.SelectedIndex = idx;
                }
            }
            finally
            {
                bSuppressSelectedIndexChanged = false;
            }
        }

        private bool bSuppressSelectedIndexChanged = false;

        private void lstDrawOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bSuppressSelectedIndexChanged)
                return;

            btnUp.Enabled = btnDown.Enabled = btnDelete.Enabled = false;
            var layer = lstDrawOrder.SelectedItem as RuntimeMapLayer;
            if (layer != null)
            {
                btnUp.Enabled = btnDown.Enabled = btnDelete.Enabled = true;
                var h = this.LayerChanged;
                if (h != null)
                    h(this, layer);
            }
        }

        public event LayerEventHandler LayerChanged;

        public event LayerEventHandler LayerDeleted;

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (_map == null)
                return;

            var layer = lstDrawOrder.SelectedItem as RuntimeMapLayer;
            if (layer != null)
            {
                int idx = _map.Layers.IndexOf(layer);
                if (idx > 0)
                    idx--;
                else
                    return;
                _map.Layers.SetNewIndex(idx, layer);
                this.Viewer.RefreshMap();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (_map == null)
                return;

            var layer = lstDrawOrder.SelectedItem as RuntimeMapLayer;
            if (layer != null)
            {
                int idx = _map.Layers.IndexOf(layer);
                if (idx < _map.Layers.Count - 1)
                    idx++;
                else
                    return;
                _map.Layers.SetNewIndex(idx, layer);
                this.Viewer.RefreshMap();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var layer = lstDrawOrder.SelectedItem as RuntimeMapLayer;
            if (layer != null)
            {
                _map.Layers.Remove(layer);
                this.Viewer.RefreshMap();
            }
        }
    }

    public delegate void LayerEventHandler(object sender, RuntimeMapLayer layer);
}
