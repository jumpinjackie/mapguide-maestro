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
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using Aga.Controls.Tree;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Editors.Common;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void OpenLayerEventHandler(object sender, string layerResourceId);

    [ToolboxItem(true)]
    internal partial class MapLayersSectionCtrl : EditorBindableCollapsiblePanel
    {
        public MapLayersSectionCtrl()
        {
            InitializeComponent();
        }

        private IMapDefinition _map;

        private DrawOrderLayerModel _doLayerModel;
        private GroupedLayerModel _grpLayerModel;
        private TiledLayerModel _tiledLayerModel;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);
            _map = (IMapDefinition)service.GetEditedResource();

            trvLayerDrawingOrder.Model = _doLayerModel = new DrawOrderLayerModel(_map);
            trvLayersGroup.Model = _grpLayerModel = new GroupedLayerModel(_map);
            trvBaseLayers.Model = _tiledLayerModel = new TiledLayerModel(_map);
        }

        private void RefreshModels()
        {
            _doLayerModel.Invalidate();
            _grpLayerModel.Invalidate();
        }

        public event OpenLayerEventHandler RequestLayerOpen;

        private void trvLayersGroup_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvLayersGroup.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as LayerItem;
                if (layer != null)
                {
                    var handler = this.RequestLayerOpen;
                    if (handler != null)
                        handler(this, layer.Tag.ResourceId);
                }
            }
        }

        private void trvLayerDrawingOrder_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvLayerDrawingOrder.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as LayerItem;
                if (layer != null)
                {
                    var handler = this.RequestLayerOpen;
                    if (handler != null)
                        handler(this, layer.Tag.ResourceId);
                }
            }
        }

        private void trvLayersGroup_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvLayersGroup.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as LayerItem;
                var group = node.Tag as GroupItem;
                if (layer != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
                    item.LayerChanged += (s, evt) => { OnResourceChanged(); };
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
                else if (group != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new GroupPropertiesCtrl(group.Tag);
                    item.GroupChanged += (s, evt) => { OnResourceChanged(); };
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
            }
        }

        private void trvLayerDrawingOrder_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvLayerDrawingOrder.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as LayerItem;
                if (layer != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
                    item.LayerChanged += (s, evt) => { OnResourceChanged(); };
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var selGroup = GetSelectedLayerGroupItem() as IMapLayerGroup;
            CreateNewGroup(selGroup);

            _grpLayerModel.Invalidate();
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            _map.RemoveGroup(group.Tag);
            _grpLayerModel.Invalidate();
        }

        private void btnGRPAddLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    string layerId = picker.ResourceID;
                    var selGroup = GetSelectedLayerGroupItem() as GroupItem;
                    CreateLayer(layerId, selGroup == null ? null : selGroup.Tag);

                    this.RefreshModels();
                }
            }
        }

        private void btnGRPRemoveLayer_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedLayerGroupItem() as LayerItem;
            if (layer != null)
            {
                _map.RemoveLayer(layer.Tag);
                this.RefreshModels();
            }
        }

        private void btnConvertLayerGroupToBaseGroup_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            if (group != null)
            {
                //...
                throw new NotImplementedException();
            }
        }

        private void btnDLAddLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    string layerId = picker.ResourceID;
                    var selGroup = GetSelectedDrawOrderItem() as GroupItem;
                    CreateLayer(layerId, selGroup != null ? selGroup.Tag : null);
                    this.RefreshModels();
                }
            }
        }

        private void btnDLRemoveLayer_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                _map.RemoveLayer(layer.Tag);
                this.RefreshModels();
            }
        }

        private void btnDLMoveLayerUp_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                _map.MoveUp(layer.Tag);
                _doLayerModel.Invalidate();

                RestoreDrawOrderSelection(layer);
            }
        }

        private void btnDLMoveLayerDown_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                _map.MoveDown(layer.Tag);
                _doLayerModel.Invalidate();

                RestoreDrawOrderSelection(layer);
            }
        }

       
        private void btnDLMoveLayerTop_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                _map.SetTopDrawOrder(layer.Tag);
                _doLayerModel.Invalidate();

                RestoreDrawOrderSelection(layer);
            }
        }

        private void btnDLMoveLayerBottom_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                _map.SetBottomDrawOrder(layer.Tag);
                _doLayerModel.Invalidate();

                RestoreDrawOrderSelection(layer);
            }
        }

        private static void RestoreSelection<TaggedType>(TreeViewAdv tree, Predicate<TaggedType> predicate) where TaggedType : class
        {
            //Restore selection
            TreeNodeAdv selectedNode = null;
            foreach (var node in tree.AllNodes)
            {
                var tag = node.Tag as TaggedType;
                
                if (tag != null && predicate(tag))
                {
                    selectedNode = node;
                    break;
                }
            }
            if (selectedNode != null)
                tree.SelectedNode = selectedNode;
        }

        private void RestoreBaseLayerSelection(BaseLayerItem item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            RestoreSelection<BaseLayerItem>(trvBaseLayers, (tag) => { return tag.Tag == item.Tag; });
        }

        private void RestoreDrawOrderSelection(LayerItem layer)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            RestoreSelection<LayerItem>(trvLayerDrawingOrder, (tag) => { return tag.Tag == layer.Tag; });
        }

        private void CreateNewGroup(IMapLayerGroup parentGroup)
        {
            int counter = 0;
            string prefix = Properties.Resources.NewLayerGroup;
            var group = _map.GetGroupByName(prefix);
            while (group != null)
            {
                counter++;
                prefix = Properties.Resources.NewLayerGroup + counter;
                group = _map.GetGroupByName(prefix);
            }

            group = _map.AddGroup(prefix);
            if (parentGroup != null)
                group.Group = parentGroup.Name;
        }

        private void CreateLayer(string layerId, IMapLayerGroup parentGroup)
        {
            int counter = 0;
            string prefix = ResourceIdentifier.GetName(layerId);
            string name = prefix;
            var layer = _map.GetLayerByName(name);
            while (layer != null)
            {
                counter++;
                name = prefix + counter;
                layer = _map.GetLayerByName(name);
            }

            if (parentGroup != null)
                layer = _map.AddLayer(parentGroup.Name, name, layerId);
            else
                layer = _map.AddLayer(null, name, layerId);
        }

        private object GetSelectedDrawOrderItem()
        {
            if (trvLayerDrawingOrder.SelectedNode != null)
            {
                return trvLayerDrawingOrder.SelectedNode.Tag;
            }
            return null;
        }

        private object GetSelectedLayerGroupItem()
        {
            if (trvLayersGroup.SelectedNode != null)
            {
                return trvLayersGroup.SelectedNode.Tag;
            }
            return null;
        }

        private void btnNewBaseLayerGroup_Click(object sender, EventArgs e)
        {
            _map.InitBaseMap();
            var grp = _map.BaseMap.AddBaseLayerGroup(GenerateBaseGroupName(_map));
            _tiledLayerModel.Invalidate();
        }

        private object GetSelectedTiledLayerItem()
        {
            if (trvBaseLayers.SelectedNode != null)
                return trvBaseLayers.SelectedNode.Tag;
            else
                return null;
        }

        private void btnRemoveBaseLayerGroup_Click(object sender, EventArgs e)
        {
            var group = GetSelectedTiledLayerItem() as BaseLayerGroupItem;
            if (group != null)
            {
                _map.BaseMap.RemoveBaseLayerGroup(group.Tag);
                _tiledLayerModel.Invalidate();
            }
        }

        private void btnAddBaseLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    _map.InitBaseMap();
                    string layerId = picker.ResourceID;
                    IBaseMapGroup grp = null;
                    var group = GetSelectedTiledLayerItem() as BaseLayerGroupItem;
                    if (group != null)
                    {
                        grp = group.Tag;
                    }
                    else
                    {
                        grp = _map.BaseMap.GetFirstGroup();
                        if (grp == null)
                        {
                            grp = _map.BaseMap.AddBaseLayerGroup(GenerateBaseGroupName(_map));
                        }
                    }
                    grp.AddLayer(GenerateBaseLayerName(layerId, _map.BaseMap), layerId);
                    _tiledLayerModel.Invalidate();
                }
            }
        }

        private static string GenerateBaseGroupName(IMapDefinition map)
        {
            map.InitBaseMap();
            int counter = 0;
            string name = Properties.Resources.BaseLayerGroup;
            if (map.BaseMap.GroupExists(name))
            {
                counter++;
                name = Properties.Resources.BaseLayerGroup + counter;
            }
            while (map.BaseMap.GroupExists(name))
            {
                counter++;
                name = Properties.Resources.BaseLayerGroup + counter;
            }
            return name;
        }

        private static string GenerateLayerName(string layerId, IMapDefinition baseMapDef)
        {
            Check.NotNull(baseMapDef, "baseMapDef");
            Check.NotEmpty(layerId, "layerId");

            int counter = 0;
            string prefix = ResourceIdentifier.GetName(layerId);
            string name = prefix;
            if (baseMapDef.GetLayerByName(name) != null)
            {
                name = prefix + counter;
            }
            while (baseMapDef.GetLayerByName(name) != null)
            {
                counter++;
                name = prefix + counter;
            }

            return name;
        }

        private static string GenerateBaseLayerName(string layerId, IBaseMapDefinition baseMapDef)
        {
            Check.NotNull(baseMapDef, "baseMapDef");
            Check.NotEmpty(layerId, "layerId");

            int counter = 0;
            string prefix = ResourceIdentifier.GetName(layerId);
            string name = prefix;
            if (baseMapDef.LayerExists(name))
            {
                name = prefix + counter;
            }
            while (baseMapDef.LayerExists(name))
            {
                counter++;
                name = prefix + counter;
            }

            return name;
        }

        private void btnRemoveBaseLayer_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedTiledLayerItem() as BaseLayerItem;
            if (layer != null)
            {
                var grp = layer.Parent;
                grp.RemoveBaseMapLayer(layer.Tag);
                _tiledLayerModel.Invalidate();
            }
        }

        private void btnMoveBaseLayerUp_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedTiledLayerItem() as BaseLayerItem;
            if (layer != null)
            {
                var grp = layer.Parent;
                grp.MoveUp(layer.Tag);
                var node = trvBaseLayers.SelectedNode.Parent;
                var path = trvBaseLayers.GetPath(node);
                _tiledLayerModel.Invalidate(path);

                RestoreBaseLayerSelection(layer);
            }
        }

        private void btnMoveBaseLayerDown_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedTiledLayerItem() as BaseLayerItem;
            if (layer != null)
            {
                var grp = layer.Parent;
                grp.MoveDown(layer.Tag);
                var node = trvBaseLayers.SelectedNode.Parent;
                var path = trvBaseLayers.GetPath(node);
                _tiledLayerModel.Invalidate(path);

                RestoreBaseLayerSelection(layer);
            }
        }

        private void trvBaseLayers_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvBaseLayers.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as BaseLayerItem;
                var group = node.Tag as BaseLayerGroupItem;
                var scale = node.Tag as ScaleItem;
                if (layer != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
                    item.LayerChanged += (s, evt) => { OnResourceChanged(); };
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
                else if (group != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new GroupPropertiesCtrl(group.Tag);
                    item.GroupChanged += (s, evt) => { OnResourceChanged(); };
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
                else if (scale != null)
                {
                    propertiesPanel.Controls.Clear();
                    var item = new FiniteScaleListCtrl(_map);
                    
                    item.Dock = DockStyle.Fill;
                    propertiesPanel.Controls.Add(item);
                }
            }
        }

        private void trvBaseLayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvBaseLayers.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as BaseLayerItem;
                if (layer != null)
                {
                    var handler = this.RequestLayerOpen;
                    if (handler != null)
                        handler(this, layer.Tag.ResourceId);
                }
            }
        }

        private void trvLayersGroup_ItemDrag(object sender, ItemDragEventArgs e)
        {
            trvLayersGroup.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private void trvLayersGroup_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        private static void HandleDragEnter(DragEventArgs e)
        {
            //Accepting all resource id drops
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //But only of the Layer Definition kind
            if (rids.Length == 1 && rids[0].ResourceType != ResourceTypes.LayerDefinition)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //Even in multiples
            foreach (var r in rids)
            {
                if (r.ResourceType != ResourceTypes.LayerDefinition)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }
        }

        private void trvLayersGroup_DragDrop(object sender, DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids == null || rids.Length == 0)
                return;

            IMapLayerGroup parent = null;
            var node = trvLayersGroup.GetNodeAt(trvLayersGroup.PointToClient(new Point(e.X, e.Y)));
            if (node != null)
            {
                var gi = node.Tag as GroupItem;
                if (gi != null)
                    parent = gi.Tag;
            }

            int added = 0;
            foreach (var rid in rids)
            {
                if (rid.ResourceType == ResourceTypes.LayerDefinition)
                { 
                    var name = GenerateLayerName(rid.ToString(), _map);
                    var layer = _map.AddLayer(parent == null ? null : parent.Name, name, rid.ToString());
                    added++;
                }
            }

            if (added > 0)
            {
                //TODO: Fine-grain invalidation
                RefreshModels();
            }
        }

        private void trvLayersGroup_DragOver(object sender, DragEventArgs e)
        {
            HandleDragOver(e);
        }

        private static void HandleDragOver(DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
        }

        private void trvLayerDrawingOrder_DragDrop(object sender, DragEventArgs e)
        {
            //TODO: Handle drag/drop re-ordering
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids != null && rids.Length > 0)
            {
                IMapLayer layer = null;
                var node = trvLayerDrawingOrder.GetNodeAt(trvLayerDrawingOrder.PointToClient(new Point(e.X, e.Y)));
                if (node != null)
                {
                    var li = node.Tag as LayerItem;
                    if (li != null)
                        layer = li.Tag;
                }

                int added = 0;
                foreach (var rid in rids)
                {
                    if (rid.ResourceType == ResourceTypes.LayerDefinition)
                    {
                        var name = GenerateLayerName(rid.ToString(), _map);
                        //var layer = _map.AddLayer(parent == null ? null : parent.Name, name, rid.ToString());
                        var lyr = _map.AddLayer(layer, null, name, rid.ToString());
                        added++;
                    }
                }

                if (added > 0)
                {
                    //TODO: Fine-grain invalidation
                    RefreshModels();
                }
            }
            else
            {
                var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                if (data != null && data.Length == 1)
                {
                    var li = data[0].Tag as LayerItem;
                    if (li != null)
                    {
                        IMapLayer sourceLayer = li.Tag;
                        IMapLayer targetLayer = null;
                        var node = trvLayerDrawingOrder.GetNodeAt(trvLayerDrawingOrder.PointToClient(new Point(e.X, e.Y)));
                        if (node != null)
                        {
                            var tli = node.Tag as LayerItem;
                            if (tli != null)
                                targetLayer = tli.Tag;
                        }

                        if (sourceLayer != null && targetLayer != null && sourceLayer != targetLayer)
                        {
                            int idx = _map.GetIndex(targetLayer);
                            if (idx >= 0)
                            {
                                _map.RemoveLayer(sourceLayer);
                                _map.InsertLayer(idx, sourceLayer);
                                RefreshModels();
                            }
                        }
                    }
                }
            }
        }

        private void trvLayerDrawingOrder_DragEnter(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (data == null)
            {
                HandleDragEnter(e);
            }
            else
            {
                var layer = data[0].Tag as LayerItem;
                if (layer == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }
        }

        private void trvLayerDrawingOrder_DragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (data == null)
            {
                HandleDragOver(e);
            }
            else
            {
                var li = data[0].Tag as LayerItem;
                if (li == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void trvLayerDrawingOrder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            trvLayerDrawingOrder.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private void trvBaseLayers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            trvBaseLayers.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private void trvBaseLayers_DragDrop(object sender, DragEventArgs e)
        {
            int added = 0;
            var rids = e.Data.GetData(typeof(ResourceIdentifier[])) as ResourceIdentifier[];
            if (rids != null && rids.Length > 0)
            {
                var node = trvLayersGroup.GetNodeAt(trvLayersGroup.PointToClient(new Point(e.X, e.Y)));

                IBaseMapGroup group = null;
                if (node != null && node.Tag is BaseLayerGroupItem)
                {
                    group = ((BaseLayerGroupItem)node.Tag).Tag;
                }

                //No group? Let's make one!
                if (group == null)
                {
                    _map.InitBaseMap();
                    group = _map.BaseMap.AddBaseLayerGroup(GenerateBaseGroupName(_map));

                    foreach (var rid in rids)
                    {
                        if (rid.ResourceType == ResourceTypes.LayerDefinition)
                        {
                            group.AddLayer(GenerateBaseLayerName(rid.ToString(), _map.BaseMap), rid.ToString());
                            added++;
                        }
                    }
                }
            }

            if (added > 0)
            {
                _tiledLayerModel.Invalidate();
            }
        }

        private void trvBaseLayers_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        private void trvBaseLayers_DragOver(object sender, DragEventArgs e)
        {
            HandleDragOver(e);
        }
    }
}
