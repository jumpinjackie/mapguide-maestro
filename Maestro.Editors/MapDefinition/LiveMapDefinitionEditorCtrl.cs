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
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.MapViewer.Model;
using Maestro.Editors.MapDefinition.Live;
using Maestro.MapViewer;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// A Live Map Definition editor control
    /// </summary>
    [ToolboxItem(true)]
    public partial class LiveMapDefinitionEditorCtrl : EditorBase
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public LiveMapDefinitionEditorCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the editor service
        /// </summary>
        public IEditorService EditorService
        {
            get;
            private set;
        }

        /// <summary>
        /// Synchronizes the internal Map Definition with the state of the Runtime Map.
        /// 
        /// Call this before attempting access to the internal Map Definition to ensure a consistent state
        /// </summary>
        public void SyncMap()
        {
            _rtMap.UpdateMapDefinition(_shadowCopy);
        }

        /// <summary>
        /// Gets the map definiton that's being edited
        /// </summary>
        /// <returns></returns>
        public IMapDefinition GetMapDefinition()
        {
            return _shadowCopy;
        }

        /// <summary>
        /// Refreshes the viewer
        /// </summary>
        public void RefreshMap()
        {
            viewer.RefreshMap();
        }

        /// <summary>
        /// Gets the viewer
        /// </summary>
        public IMapViewer Viewer { get { return viewer; } }

        private RuntimeMap _rtMap;
        private IMapDefinition _shadowCopy;
        private IMappingService _mapSvc;

        /// <summary>
        /// Gets the runtime map being edited
        /// </summary>
        public RuntimeMap Map
        {
            get { return _rtMap; }
        }

        /// <summary>
        /// Binds the specified editor service to this editor
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            this.EditorService = service;
            service.RegisterCustomNotifier(this);

            _shadowCopy = (IMapDefinition)service.GetEditedResource();
            _mapSvc = (IMappingService)_shadowCopy.CurrentConnection.GetService((int)ServiceType.Mapping);
            _rtMap = _mapSvc.CreateMap(_shadowCopy);
            repoView.Init(service.ResourceService, new ResourceTypes[] {
                ResourceTypes.LayerDefinition,
                ResourceTypes.FeatureSource
            }, new ResourceTypes[] {
                ResourceTypes.LayerDefinition,
                ResourceTypes.FeatureSource
            });

            ReloadViewer();
        }

        /// <summary>
        /// Rebuilds the current Runtime Map
        /// </summary>
        public void RebuildRuntimeMap()
        {
            _rtMap = _mapSvc.CreateMap(_shadowCopy);
        }

        /// <summary>
        /// Reloads the current viewer
        /// </summary>
        public void ReloadViewer()
        {
            viewer.LoadMap(_rtMap);
        }

        private void legendCtrl_NodeSelected(object sender, TreeNode node)
        {
            var layer = node.Tag as LayerNodeMetadata;
            var group = node.Tag as GroupNodeMetadata;
            //Nothing to edit for theme rule nodes
            if (layer == null && group == null)
                propGrid.SelectedObject = null;
            else
                propGrid.SelectedObject = node.Tag;
        }

        private void drawOrderCtrl_LayerSelected(object sender, RuntimeMapLayer layer)
        {
            propGrid.SelectedObject = new LayerNodeMetadata(layer);
        }

        private void drawOrderCtrl_LayerDeleted(object sender, RuntimeMapLayer layer)
        {
            if (layer == propGrid.SelectedObject)
                propGrid.SelectedObject = null;
        }

        private void legendCtrl_NodeDeleted(object sender, TreeNode node)
        {
            var layer = node.Tag as LayerNodeMetadata;
            //Nothing to edit for theme rule nodes
            if (layer != null && layer == propGrid.SelectedObject)
                propGrid.SelectedObject = null;
        }

        private void repoView_RequestAddToMap(object sender, EventArgs e)
        {
            var item = repoView.SelectedItem;
            if (item != null && item.ResourceType == ResourceTypes.LayerDefinition)
            {
                var layer = _mapSvc.CreateMapLayer(_rtMap, ((ILayerDefinition)this.EditorService.ResourceService.GetResource(item.ResourceId)));
                layer.Name = LiveMapEditorLegend.GenerateUniqueName(ResourceIdentifier.GetName(item.ResourceId), _rtMap.Layers);
                layer.LegendLabel = ResourceIdentifier.GetName(item.ResourceId);
                layer.Visible = true;
                layer.ShowInLegend = true;
                _rtMap.Layers.Insert(0, layer);
                viewer.RefreshMap();
            }
        }

        private void repoView_RequestEdit(object sender, EventArgs e)
        {
            var res = repoView.SelectedItem;

            var ed = new Maestro.Editors.Common.EditorWindow(_shadowCopy.CurrentConnection, res.ResourceId);
            ed.ShowDialog();
        }

        private void repoView_ItemSelected(object sender, EventArgs e)
        {
            viewer.ActiveTool = MapViewer.MapActiveTool.None;
        }

        private void repoView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            viewer.ActiveTool = MapViewer.MapActiveTool.None;
            var item = repoView.SelectedItem;
            if (item != null && !item.IsFolder)
                repoView.DoDragDrop(new ResourceDragMessage(item.ResourceId), DragDropEffects.Copy);
        }

        private void legendCtrl_DragEnter(object sender, DragEventArgs e)
        {
            legendCtrl.HandleDragEnter(e);
        }

        private void legendCtrl_DragOver(object sender, DragEventArgs e)
        {
            legendCtrl.HandleDragOver(e);
        }

        private void legendCtrl_DragDrop(object sender, DragEventArgs e)
        {
            legendCtrl.HandleDragDrop(e);
        }

        private void legendCtrl_ItemDrag(object sender, ItemDragEventArgs e)
        {
            legendCtrl.HandleItemDrag(e);
        }
    }
}
