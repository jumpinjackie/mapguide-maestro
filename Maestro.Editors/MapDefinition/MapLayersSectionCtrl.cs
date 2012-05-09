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

    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class MapLayersSectionCtrl : EditorBindableCollapsiblePanel
    {
        public MapLayersSectionCtrl()
        {
            InitializeComponent();
            trvBaseLayers.KeyUp += new KeyEventHandler(trvBaseLayers_KeyUp);
            trvLayerDrawingOrder.KeyUp += new KeyEventHandler(trvLayerDrawingOrder_KeyUp);
            trvLayersGroup.KeyUp += new KeyEventHandler(trvLayersGroup_KeyUp);
        }

        void trvLayersGroup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                var group = GetSelectedLayerGroupItem() as GroupItem;
                var layer = GetSelectedLayerGroupItem() as LayerItem;
                if (e.KeyCode == Keys.Delete)
                {
                    if (layer != null)
                    {
                        RemoveSelectedLayerGroupItem(layer);
                    }
                    else if (group != null)
                    {
                        RemoveSelectedLayerGroupItem(group);
                    }
                }
                else
                {
                    if (layer != null)
                    {
                        OnDynamicLayerItemSelected(layer);
                    }
                    else if (group != null)
                    {
                        OnDynamicGroupItemSelected(group);
                    }
                }
            }
        }

        void trvLayerDrawingOrder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                var layer = GetSelectedDrawOrderItem() as LayerItem;
                if (layer != null)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        RemoveSelectedDrawOrderLayerItem(layer);
                    }
                    else
                    {
                        OnDrawOrderLayerItemSelected(layer);
                    }
                }
            }
        }

        void trvBaseLayers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                var group = GetSelectedTiledLayerItem() as BaseLayerGroupItem;
                var layer = GetSelectedTiledLayerItem() as BaseLayerItem;
                if (e.KeyCode == Keys.Delete)
                {
                    if (group != null)
                    {
                        RemoveSelectedTiledLayerItem(group);
                    }
                    else if (layer != null)
                    {
                        RemoveSelectedTiledLayerItem(layer);
                    }
                }
                else
                {
                    if (layer != null)
                    {
                        OnBaseLayerItemSelected(layer);
                    }
                    else if (group != null)
                    {
                        OnBaseLayerGroupItemSelected(group);
                    }
                }
            }
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
            _tiledLayerModel.Invalidate();
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

                btnGRPRemoveLayer.Enabled = false;
                btnRemoveGroup.Enabled = false;
                btnMoveGroupUp.Enabled = false;
                btnMoveGroupDown.Enabled = false;
                btnConvertLayerGroupToBaseGroup.Enabled = false;

                if (layer != null)
                {
                    OnDynamicLayerItemSelected(layer);
                }
                else if (group != null)
                {
                    OnDynamicGroupItemSelected(group);
                }
            }
        }

        private void OnDynamicGroupItemSelected(GroupItem group)
        {
            btnRemoveGroup.Enabled = true;
            btnMoveGroupUp.Enabled = true;
            btnMoveGroupDown.Enabled = true;
            btnConvertLayerGroupToBaseGroup.Enabled = true;

            propertiesPanel.Controls.Clear();
            var item = new GroupPropertiesCtrl(group.Tag);
            item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
        }

        private void OnDynamicLayerItemSelected(LayerItem layer)
        {
            btnGRPRemoveLayer.Enabled = true;

            propertiesPanel.Controls.Clear();
            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
            item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
        }

        private void trvLayerDrawingOrder_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNodeAdv node = trvLayerDrawingOrder.GetNodeAt(new Point(e.X, e.Y));
            if (node != null)
            {
                var layer = node.Tag as LayerItem;
                btnDLMoveLayerBottom.Enabled =
                btnDLMoveLayerDown.Enabled =
                btnDLMoveLayerTop.Enabled =
                btnDLMoveLayerUp.Enabled =
                btnDLRemoveLayer.Enabled = false;

                if (layer != null)
                {
                    OnDrawOrderLayerItemSelected(layer);
                }
            }
        }

        private void OnDrawOrderLayerItemSelected(LayerItem layer)
        {
            btnDLMoveLayerBottom.Enabled =
            btnDLMoveLayerDown.Enabled =
            btnDLMoveLayerTop.Enabled =
            btnDLMoveLayerUp.Enabled =
            btnDLRemoveLayer.Enabled = true;

            propertiesPanel.Controls.Clear();
            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
            item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var selGroup = GetSelectedLayerGroupItem() as IMapLayerGroup;
            var newGroup = CreateNewGroup(selGroup);

            _grpLayerModel.Invalidate();
            RestoreGroupSelection(newGroup);
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            if (group != null)
            {
                RemoveSelectedLayerGroupItem(group);
            }
        }

        private void RemoveSelectedLayerGroupItem(GroupItem group)
        {
            _map.RemoveLayerGroupAndChildLayers(group.Tag.Name);
            propertiesPanel.Controls.Clear();
            _grpLayerModel.Invalidate();
            _doLayerModel.Invalidate();
        }

        private void btnGRPAddLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    string layerId = picker.ResourceID;
                    var selGroup = GetSelectedLayerGroupItem() as GroupItem;
                    var layer = CreateLayer(layerId, selGroup == null ? null : selGroup.Tag);
                    this.RefreshModels();
                    RestoreLayerSelection(layer);
                }
            }
        }

        private void btnGRPRemoveLayer_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedLayerGroupItem() as LayerItem;
            if (layer != null)
            {
                RemoveSelectedLayerGroupItem(layer);
            }
        }

        private void RemoveSelectedLayerGroupItem(LayerItem layer)
        {
            _map.RemoveLayer(layer.Tag);
            propertiesPanel.Controls.Clear();
            this.RefreshModels();
        }

        private void btnConvertLayerGroupToBaseGroup_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            if (group != null)
            {
                var layGroup = group.Tag;
                var layers = _map.GetLayersForGroup(layGroup.Name);

                if (_map.BaseMap == null)
                    _map.InitBaseMap();

                int counter = 1;
                string groupName = layGroup.Name;
                var blg = _map.BaseMap.GetGroup(groupName);
                while (blg != null)
                {
                    groupName = layGroup.Name + " (" + counter + ")";
                    counter++;

                    blg = _map.BaseMap.GetGroup(groupName);
                }
                blg = _map.BaseMap.AddBaseLayerGroup(groupName);
                blg.LegendLabel = layGroup.LegendLabel;

                foreach (var layer in layers)
                {
                    var bl = blg.AddLayer(layer.Name, layer.ResourceId);
                    bl.LegendLabel = layer.LegendLabel;
                    bl.Selectable = layer.Selectable;
                    bl.ShowInLegend = layer.ShowInLegend;
                    bl.ExpandInLegend = layer.ExpandInLegend;
                }

                _map.RemoveLayerGroupAndChildLayers(layGroup.Name);
                MessageBox.Show(string.Format(Properties.Resources.LayerGroupConvertedToBaseLayerGroup, layGroup.Name, groupName));
                this.RefreshModels();
                tabControl1.SelectedIndex = 2; //Switch to Base Layer Groups
            }
        }

        private void btnDLAddLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    string layerId = picker.ResourceID;
                    var selGroup = GetSelectedDrawOrderItem() as GroupItem;
                    var layer = CreateLayer(layerId, selGroup != null ? selGroup.Tag : null);
                    this.RefreshModels();
                    RestoreDrawOrderSelection(layer);
                }
            }
        }

        private void btnDLRemoveLayer_Click(object sender, EventArgs e)
        {
            var layer = GetSelectedDrawOrderItem() as LayerItem;
            if (layer != null)
            {
                RemoveSelectedDrawOrderLayerItem(layer);
            }
        }

        private void RemoveSelectedDrawOrderLayerItem(LayerItem layer)
        {
            _map.RemoveLayer(layer.Tag);
            propertiesPanel.Controls.Clear();
            this.RefreshModels();
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

        private static TaggedType RestoreSelection<TaggedType>(TreeViewAdv tree, Predicate<TaggedType> predicate) where TaggedType : class
        {
            TaggedType ret = null;

            //Restore selection
            TreeNodeAdv selectedNode = null;
            foreach (var node in tree.AllNodes)
            {
                var tag = node.Tag as TaggedType;
                
                if (tag != null && predicate(tag))
                {
                    selectedNode = node;
                    ret = tag;
                    break;
                }
            }
            if (selectedNode != null)
                tree.SelectedNode = selectedNode;

            return ret;
        }

        private static void ExpandNode<TaggedType>(TreeViewAdv tree, Predicate<TaggedType> predicate) where TaggedType : class
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
            {
                var n = selectedNode;
                while (n != null)
                {
                    n.Expand();
                    n = n.Parent;
                }
            }
        }

        private void RestoreLayerSelection(LayerItem item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<LayerItem>(trvLayersGroup, (tag) => { return tag.Tag == item.Tag; });
            if (it != null)
                OnDynamicLayerItemSelected(it);
        }

        private void RestoreLayerSelection(IMapLayer item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<LayerItem>(trvLayersGroup, (tag) => { return tag.Tag == item; });
            if (it != null)
                OnDynamicLayerItemSelected(it);
        }

        private void RestoreGroupSelection(GroupItem item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<GroupItem>(trvLayersGroup, (tag) => { return tag.Tag == item.Tag; });
            if (it != null)
                OnDynamicGroupItemSelected(it);
        }

        private void RestoreGroupSelection(IMapLayerGroup group)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<GroupItem>(trvLayersGroup, (tag) => { return tag.Tag == group; });
            if (it != null)
                OnDynamicGroupItemSelected(it);
        }

        private void RestoreBaseLayerSelection(BaseLayerItem item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<BaseLayerItem>(trvBaseLayers, (tag) => { return tag.Tag == item.Tag; });
            if (it != null)
                OnBaseLayerItemSelected(it);
        }

        private void RestoreBaseLayerSelection(IBaseMapLayer layer)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<BaseLayerItem>(trvBaseLayers, (tag) => { return tag.Tag == layer; });
            if (it != null)
                OnBaseLayerItemSelected(it);
        }

        private void RestoreDrawOrderSelection(LayerItem layer)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var lyr = RestoreSelection<LayerItem>(trvLayerDrawingOrder, (tag) => { return tag.Tag == layer.Tag; });
            if (lyr != null)
                OnDrawOrderLayerItemSelected(lyr);
        }

        private void RestoreDrawOrderSelection(IMapLayer layer)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var lyr = RestoreSelection<LayerItem>(trvLayerDrawingOrder, (tag) => { return tag.Tag == layer; });
            if (lyr != null)
                OnDrawOrderLayerItemSelected(lyr);
        }

        private IMapLayerGroup CreateNewGroup(IMapLayerGroup parentGroup)
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

            return group;
        }

        private IMapLayer CreateLayer(string layerId, IMapLayerGroup parentGroup)
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

            return layer;
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
                RemoveSelectedTiledLayerItem(group);
            }
        }

        private void RemoveSelectedTiledLayerItem(BaseLayerGroupItem group)
        {
            _map.BaseMap.RemoveBaseLayerGroup(group.Tag);
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
        }

        private void btnAddBaseLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
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
                    var bl = grp.AddLayer(GenerateBaseLayerName(layerId, _map.BaseMap), layerId);
                    _tiledLayerModel.Invalidate();
                    RestoreBaseLayerSelection(bl);
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
                RemoveSelectedTiledLayerItem(layer);
            }
        }

        private void RemoveSelectedTiledLayerItem(BaseLayerItem layer)
        {
            var grp = layer.Parent;
            grp.RemoveBaseMapLayer(layer.Tag);
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
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

        private void btnMoveGroupUp_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            if (group != null)
            {
                var mdf = group.Tag.Parent;
                mdf.MoveUpGroup(group.Tag);

                _grpLayerModel.Invalidate();

                RestoreGroupSelection(group);
            }
        }

        private void btnMoveGroupDown_Click(object sender, EventArgs e)
        {
            var group = GetSelectedLayerGroupItem() as GroupItem;
            if (group != null)
            {
                var mdf = group.Tag.Parent;
                mdf.MoveDownGroup(group.Tag);

                _grpLayerModel.Invalidate();

                RestoreGroupSelection(group);
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

                btnRemoveBaseLayerGroup.Enabled = false;
                btnRemoveBaseLayer.Enabled = false;
                btnMoveBaseLayerDown.Enabled = false;
                btnMoveBaseLayerUp.Enabled = false;
                btnAddBaseLayer.Enabled = false;

                if (layer != null)
                {
                    OnBaseLayerItemSelected(layer);
                }
                else if (group != null)
                {
                    OnBaseLayerGroupItemSelected(group);
                }
                else if (scale != null)
                {
                    OnFiniteScaleListSelected();
                }
            }
        }

        private void OnFiniteScaleListSelected()
        {
            propertiesPanel.Controls.Clear();
            var item = new FiniteScaleListCtrl(_map, _edSvc);

            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
        }

        private void OnBaseLayerGroupItemSelected(BaseLayerGroupItem group)
        {
            btnAddBaseLayer.Enabled = true;
            btnRemoveBaseLayerGroup.Enabled = true;

            propertiesPanel.Controls.Clear();
            var item = new GroupPropertiesCtrl(group.Tag);
            item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
        }

        private void OnBaseLayerItemSelected(BaseLayerItem layer)
        {
            btnRemoveBaseLayer.Enabled = true;
            btnMoveBaseLayerDown.Enabled = true;
            btnMoveBaseLayerUp.Enabled = true;

            propertiesPanel.Controls.Clear();
            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService);
            item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Fill;
            propertiesPanel.Controls.Add(item);
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
            var rids = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //But only of the Layer Definition kind
            if (rids.Length == 1 && rids[0].ResourceId.ResourceType != ResourceTypes.LayerDefinition)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //Even in multiples
            foreach (var r in rids)
            {
                if (r.ResourceId.ResourceType != ResourceTypes.LayerDefinition)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }
        }

        private void trvLayersGroup_DragDrop(object sender, DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
            var nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (rids != null && rids.Length > 0)
            {
                IMapLayerGroup parent = null;
                var clientPt = trvLayersGroup.PointToClient(new Point(e.X, e.Y));
                var node = trvLayersGroup.GetNodeAt(clientPt);
                if (node != null)
                {
                    var gi = node.Tag as GroupItem;
                    if (gi != null)
                        parent = gi.Tag;
                }

                int added = 0;
                foreach (var rid in rids)
                {
                    if (rid.ResourceId.ResourceType == ResourceTypes.LayerDefinition)
                    {
                        var name = GenerateLayerName(rid.ResourceId.ToString(), _map);
                        var layer = _map.AddLayer(parent == null ? null : parent.Name, name, rid.ResourceId.ToString());
                        added++;
                    }
                }

                if (added > 0)
                {
                    //TODO: Fine-grain invalidation
                    RefreshModels();
                    if (parent != null)
                        ExpandNode<GroupItem>(trvLayersGroup, (tag) => { return tag.Tag == parent; });
                }
            }
            else if (nodes != null && nodes.Length > 0)
            {
                IMapLayerGroup parent = null;
                var clientPt = trvLayersGroup.PointToClient(new Point(e.X, e.Y));
                var node = trvLayersGroup.GetNodeAt(clientPt);
                if (node != null)
                {
                    var gi = node.Tag as GroupItem;
                    var li = node.Tag as LayerItem;
                    if (gi != null)
                        parent = gi.Tag;
                    else if (li != null)
                        parent = _map.GetGroupByName(li.Tag.Group);
                }

                int moved = 0;
                //Add to this group
                foreach (var n in nodes)
                {
                    var gi = n.Tag as GroupItem;
                    var li = n.Tag as LayerItem;

                    //Re-assign parent
                    if (gi != null)
                    {
                        gi.Tag.Group = parent == null ? string.Empty : parent.Name;
                        moved++;
                    }
                    else if (li != null)
                    {
                        li.Tag.Group = parent == null ? string.Empty : parent.Name;
                        moved++;
                    }
                }

                if (moved > 0)
                {
                    //TODO: Fine-grain invalidation
                    RefreshModels();
                    if (parent != null)
                        ExpandNode<GroupItem>(trvLayersGroup, (tag) => { return tag.Tag == parent; });
                    OnResourceChanged();
                }
            }
        }

        private void trvLayersGroup_DragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (data == null)
            {
                HandleDragOver(e);
            }
            else
            {
                var li = data[0].Tag as LayerItem;
                var gi = data[0].Tag as GroupItem;
                if (li == null && gi == null)
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

        private void HandleDragOver(DragEventArgs e)
        {
            var rids = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
            if (rids == null || rids.Length == 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            else
            {
                //All handles should have the same connection, so sample the first
                //Must be the same connection as this current editor
                if (rids[0].Connection != _edSvc.GetEditedResource().CurrentConnection)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }

            e.Effect = DragDropEffects.Copy;
        }

        private void trvLayerDrawingOrder_DragDrop(object sender, DragEventArgs e)
        {
            //TODO: Handle drag/drop re-ordering
            var rids = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
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
                    if (rid.ResourceId.ResourceType == ResourceTypes.LayerDefinition)
                    {
                        var name = GenerateLayerName(rid.ResourceId.ToString(), _map);
                        //var layer = _map.AddLayer(parent == null ? null : parent.Name, name, rid.ToString());
                        var lyr = _map.AddLayer(layer, null, name, rid.ResourceId.ToString());
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
            var rids = e.Data.GetData(typeof(RepositoryHandle[])) as RepositoryHandle[];
            var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (rids != null && rids.Length > 0)
            {
                int added = 0;
                var node = trvBaseLayers.GetNodeAt(trvBaseLayers.PointToClient(new Point(e.X, e.Y)));

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
                }

                foreach (var rid in rids)
                {
                    if (rid.ResourceId.ResourceType == ResourceTypes.LayerDefinition)
                    {
                        group.AddLayer(GenerateBaseLayerName(rid.ResourceId.ToString(), _map.BaseMap), rid.ResourceId.ToString());
                        added++;
                    }
                }

                if (added > 0)
                {
                    _tiledLayerModel.Invalidate();
                }
            }
            else if (data != null && data.Length == 1)
            {
                var li = data[0].Tag as BaseLayerItem;
                if (li != null)
                {
                    IBaseMapLayer sourceLayer = li.Tag;
                    IBaseMapLayer targetLayer = null;
                    IBaseMapGroup targetGroup = null;
                    var node = trvBaseLayers.GetNodeAt(trvBaseLayers.PointToClient(new Point(e.X, e.Y)));
                    BaseLayerItem tli = null;
                    if (node != null)
                    {
                        tli = node.Tag as BaseLayerItem;
                        var tlg = node.Tag as BaseLayerGroupItem;
                        if (tli != null)
                            targetLayer = tli.Tag;
                        else if (tlg != null)
                            targetGroup = tlg.Tag;
                    }

                    if (sourceLayer != null && targetLayer != null && sourceLayer != targetLayer)
                    {
                        var srcGroup = _map.BaseMap.GetGroupForLayer(sourceLayer);
                        var dstGroup = _map.BaseMap.GetGroupForLayer(targetLayer);

                        if (srcGroup != null)
                        {
                            if (srcGroup == dstGroup)
                            {
                                int idx = srcGroup.GetIndex(targetLayer);
                                if (idx >= 0)
                                {
                                    srcGroup.RemoveBaseMapLayer(sourceLayer);
                                    srcGroup.InsertLayer(idx, sourceLayer);

                                    _tiledLayerModel.Invalidate();

                                    //Keep group expanded
                                    if (tli != null)
                                        RestoreBaseLayerSelection(sourceLayer);
                                }
                            }
                            else
                            {
                                srcGroup.RemoveBaseMapLayer(sourceLayer);
                                dstGroup.InsertLayer(0, targetLayer);

                                _tiledLayerModel.Invalidate();

                                //Keep group expanded
                                if (tli != null)
                                    RestoreBaseLayerSelection(targetLayer);
                            }
                        }
                    }
                }
            }
        }

        private void trvBaseLayers_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        private void trvBaseLayers_DragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
            if (data == null)
            {
                HandleDragOver(e);
            }
            else
            {
                var li = data[0].Tag as BaseLayerItem;
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
    }
}
