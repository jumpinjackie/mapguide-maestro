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
using Maestro.Editors.MapDefinition.Live;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// A Live Map Editor component that displays the layers of the currently edited map by draw order
    /// </summary>
    public partial class LiveMapEditorDrawOrder : UserControl
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public LiveMapEditorDrawOrder()
        {
            InitializeComponent();
            //HACK: http://social.msdn.microsoft.com/Forums/zh/winformsdatacontrols/thread/2db8e07a-6aa8-4865-9b59-c02025eaf317
            lstDrawOrder.CreateControl();
        }
        
        private RuntimeMap _map;
        private IMapViewer _viewer;
        
        /// <summary>
        /// Gets or sets the viewer instance
        /// </summary>
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
                OnLayerSelected(layer);
            }
        }

        private void OnLayerSelected(RuntimeMapLayer layer)
        {
            var h = this.LayerSelected;
            if (h != null)
                h(this, layer);
        }

        /// <summary>
        /// Raised when a layer has been selected
        /// </summary>
        public event LayerEventHandler LayerSelected;

        /// <summary>
        /// Raised when a layer has been removed from this view
        /// </summary>
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
                if (lstDrawOrder.SelectedIndex != idx)
                    lstDrawOrder.SelectedIndex = idx;
                else
                    OnLayerSelected(layer);
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
                if (lstDrawOrder.SelectedIndex != idx)
                    lstDrawOrder.SelectedIndex = idx;
                else
                    OnLayerSelected(layer);
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

        private void lstDrawOrder_DragOver(object sender, DragEventArgs e)
        {
            var res = e.Data.GetData(typeof(ResourceDragMessage)) as ResourceDragMessage;
            var layer = e.Data.GetData(typeof(RuntimeMapLayer)) as RuntimeMapLayer;
            if (layer != null)
                e.Effect = DragDropEffects.Move;
            else if (res != null && ResourceIdentifier.GetResourceType(res.ResourceID) == ResourceTypes.LayerDefinition)
                e.Effect = DragDropEffects.Copy;
        }

        private void lstDrawOrder_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void lstDrawOrder_DragDrop(object sender, DragEventArgs e)
        {
            var pt = lstDrawOrder.PointToClient(new Point(e.X, e.Y));
            var index = lstDrawOrder.IndexFromPoint(pt);
            if (index < 0)
                index = lstDrawOrder.Items.Count - 1;

            var res = e.Data.GetData(typeof(ResourceDragMessage)) as ResourceDragMessage;
            var layer = e.Data.GetData(typeof(RuntimeMapLayer)) as RuntimeMapLayer;
            if (layer != null)
            {
                _map.Layers.SetNewIndex(index, layer);
                if (lstDrawOrder.SelectedIndex != index)
                    lstDrawOrder.SelectedIndex = index;
                else
                    OnLayerSelected(layer);
                this.Viewer.RefreshMap();
            }
            else if (res != null && ResourceIdentifier.GetResourceType(res.ResourceID) == ResourceTypes.LayerDefinition)
            {
                var conn = _map.CurrentConnection;
                var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                layer = mapSvc.CreateMapLayer(_map, (ILayerDefinition)conn.ResourceService.GetResource(res.ResourceID));
                _map.Layers.Insert(0, layer);
                if (lstDrawOrder.SelectedIndex != 0)
                    lstDrawOrder.SelectedIndex = 0;
                else
                    OnLayerSelected(layer);
                this.Viewer.RefreshMap();
            }
        }

        private void lstDrawOrder_MouseDown(object sender, MouseEventArgs e)
        {
            var item = lstDrawOrder.SelectedItem as RuntimeMapLayer;
            if (item != null)
                lstDrawOrder.DoDragDrop(item, DragDropEffects.Move);
        }
    }

    /// <summary>
    /// Represents a method that handles events relating to layer manipulation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="layer"></param>
    public delegate void LayerEventHandler(object sender, RuntimeMapLayer layer);
}
