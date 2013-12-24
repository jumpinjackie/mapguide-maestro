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
using System.Linq;
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

        private void OnDynamicGroupItemSelected(GroupItem group)
        {
            btnAddGroup.Enabled = true;
            btnGRPAddLayer.Enabled = true;
            btnRemoveGroup.Enabled = true;
            btnMoveLayerOrGroupUp.Enabled = true;
            btnMoveLayerOrGroupDown.Enabled = true;
            btnConvertLayerGroupToBaseGroup.Enabled = true;

            propertiesPanel.Controls.Clear();
            //var item = new GroupPropertiesCtrl(_map, group.Tag);
            //item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            //item.Dock = DockStyle.Fill;
            var item = CreateGroupControl(group);
            _activeLayer = null;
            propertiesPanel.Controls.Add(item);
        }

        class LocalizedDisplayNameAttribute : DisplayNameAttribute
        {
            private readonly string resourceName;
            public LocalizedDisplayNameAttribute(string resourceName)
                : base()
            {
                this.resourceName = resourceName;
            }

            public override string DisplayName
            {
                get
                {
                    return Strings.ResourceManager.GetString(
                        this.resourceName,
                        Strings.Culture);
                }
            }
        }

        #region Designer Attributes

        internal class BaseLayerItemDesigner
        {
            private BaseLayerItem _layer;

            public BaseLayerItemDesigner(BaseLayerItem layer)
            {
                _layer = layer;
            }

            [Browsable(false)]
            internal BaseLayerItem Item { get { return _layer; } }

            [LocalizedDisplayName("MdfEditorSelectable")]
            public bool Selectable
            {
                get { return _layer.Tag.Selectable; }
                set { _layer.Tag.Selectable = value; }
            }

            [LocalizedDisplayName("MdfEditorShowInLegend")]
            public bool ShowInLegend
            {
                get { return _layer.Tag.ShowInLegend; }
                set { _layer.Tag.ShowInLegend = value; }
            }

            [LocalizedDisplayName("MdfEditorExpandInLegend")]
            public bool ExpandInLegend
            {
                get { return _layer.Tag.ExpandInLegend; }
                set { _layer.Tag.ExpandInLegend = value; }
            }
        }

        internal class BaseGroupItemDesigner
        {
            private BaseLayerGroupItem _group;

            public BaseGroupItemDesigner(BaseLayerGroupItem group)
            {
                _group = group;
            }

            [Browsable(false)]
            internal BaseLayerGroupItem Item { get { return _group; } }

            [LocalizedDisplayName("MdfEditorVisible")]
            public bool Visible
            {
                get { return _group.Tag.Visible; }
                set { _group.Tag.Visible = value; }
            }

            [LocalizedDisplayName("MdfEditorShowInLegend")]
            public bool ShowInLegend
            {
                get { return _group.Tag.ShowInLegend; }
                set { _group.Tag.ShowInLegend = value; }
            }

            [LocalizedDisplayName("MdfEditorExpandInLegend")]
            public bool ExpandInLegend
            {
                get { return _group.Tag.ExpandInLegend; }
                set { _group.Tag.ExpandInLegend = value; }
            }
        }

        internal class LayerItemDesigner
        {
            private LayerItem _layer;

            public LayerItemDesigner(LayerItem layer)
            {
                _layer = layer;
            }

            [Browsable(false)]
            internal LayerItem Item { get { return _layer; } }

            [LocalizedDisplayName("MdfEditorSelectable")]
            public bool Selectable
            {
                get { return _layer.Tag.Selectable; }
                set { _layer.Tag.Selectable = value; }
            }

            [LocalizedDisplayName("MdfEditorVisible")]
            public bool Visible
            {
                get { return _layer.Tag.Visible; }
                set { _layer.Tag.Visible = value; }
            }

            [LocalizedDisplayName("MdfEditorShowInLegend")]
            public bool ShowInLegend
            {
                get { return _layer.Tag.ShowInLegend; }
                set { _layer.Tag.ShowInLegend = value; }
            }

            [LocalizedDisplayName("MdfEditorExpandInLegend")]
            public bool ExpandInLegend
            {
                get { return _layer.Tag.ExpandInLegend; }
                set { _layer.Tag.ExpandInLegend = value; }
            }
        }

        internal class GroupItemDesigner
        {
            private GroupItem _group;

            public GroupItemDesigner(GroupItem group)
            {
                _group = group;
            }

            [Browsable(false)]
            internal GroupItem Item { get { return _group; } }

            [LocalizedDisplayName("MdfEditorVisible")]
            public bool Visible
            {
                get { return _group.Tag.Visible; }
                set { _group.Tag.Visible = value; }
            }

            [LocalizedDisplayName("MdfEditorShowInLegend")]
            public bool ShowInLegend
            {
                get { return _group.Tag.ShowInLegend; }
                set { _group.Tag.ShowInLegend = value; }
            }

            [LocalizedDisplayName("MdfEditorExpandInLegend")]
            public bool ExpandInLegend
            {
                get { return _group.Tag.ExpandInLegend; }
                set { _group.Tag.ExpandInLegend = value; }
            }
        }

        #endregion

        private Control CreateGroupControl(GroupItem group)
        {
            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            commCtrl.SelectedObject = new GroupItemDesigner(group);

            var item = new GroupPropertiesCtrl(_map, group.Tag);
            item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);
            
            ctrl.Dock = DockStyle.Fill;

            return ctrl;
        }

        #region Control Factories

        private Control CreateMultiControl(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            List<object> values = new List<object>();
            for (int i = 0; i < nodes.Count; i++ )
            {
                var grp = nodes[i].Tag as GroupItem;
                var lyr = nodes[i].Tag as LayerItem;
                var bgrp = nodes[i].Tag as BaseLayerGroupItem;
                var blyr = nodes[i].Tag as BaseLayerItem;
                if (grp != null)
                {
                    values.Add(new GroupItemDesigner(grp));
                }
                else if (lyr != null)
                {
                    values.Add(new LayerItemDesigner(lyr));
                }
                else if (bgrp != null)
                {
                    values.Add(new BaseGroupItemDesigner(bgrp));
                }
                else if (blyr != null)
                {
                    values.Add(new BaseLayerItemDesigner(blyr));
                }
            }

            commCtrl.SelectedObjects = values.ToArray();
            return commCtrl;
        }

        private Control CreateBaseGroupControl(BaseLayerGroupItem group)
        {
            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            commCtrl.SelectedObject = new BaseGroupItemDesigner(group);

            var item = new GroupPropertiesCtrl(_map, group.Tag);
            item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);

            ctrl.Dock = DockStyle.Fill;

            return ctrl;
        }

        private Control CreateBaseLayerControl(BaseLayerItem layer)
        {
            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            commCtrl.SelectedObject = new BaseLayerItemDesigner(layer);

            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService, _edSvc);
            item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);

            ctrl.Dock = DockStyle.Fill;

            return ctrl;
        }

        private Control CreateLayerControl(LayerItem layer)
        {
            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            commCtrl.SelectedObject = new LayerItemDesigner(layer);

            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService, _edSvc);
            item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);

            ctrl.Dock = DockStyle.Fill;

            return ctrl;
        }

        #endregion

        private void OnDynamicLayerItemSelected(LayerItem layer)
        {
            btnAddGroup.Enabled = true;             //This has to be true otherwise it never gets enabled again if the map has no existing groups
            btnGRPAddLayer.Enabled = false;
            btnGRPRemoveLayer.Enabled = true;
            btnMoveLayerOrGroupUp.Enabled = true;   //TODO: Disable if layer is top of its group
            btnMoveLayerOrGroupDown.Enabled = true; //TODO: Disable if layer is bottom of its group

            propertiesPanel.Controls.Clear();

            var item = CreateLayerControl(layer);
            _activeLayer = layer.Tag;
            propertiesPanel.Controls.Add(item);
        }

        static bool AllLayers(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach(var node in nodes)
            {
                var layer = node.Tag as LayerItem;
                if (layer == null)
                    return false;
            }
            return true;
        }

        static bool AllBaseLayers(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var layer = node.Tag as BaseLayerItem;
                if (layer == null)
                    return false;
            }
            return true;
        }

        static bool AllGroups(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var group = node.Tag as GroupItem;
                if (group == null)
                    return false;
            }
            return true;
        }

        static bool AllBaseGroups(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var group = node.Tag as BaseLayerGroupItem;
                if (group == null)
                    return false;
            }
            return true;
        }

        private void OnMultipleItemsSelected(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            bool bAllLayers = AllLayers(nodes);
            bool bAllGroups = AllGroups(nodes);
            bool bAllBaseLayers = AllBaseLayers(nodes);
            bool bAllBaseGroups = AllBaseGroups(nodes);

            btnAddGroup.Enabled = false;
            btnRemoveGroup.Enabled = bAllGroups;
            btnConvertLayerGroupToBaseGroup.Enabled = bAllGroups;
            btnGRPAddLayer.Enabled = false;
            btnGRPRemoveLayer.Enabled = bAllLayers;
            btnMoveLayerOrGroupUp.Enabled = false;
            btnMoveLayerOrGroupDown.Enabled = false;

            btnDLMoveLayerBottom.Enabled = false;
            btnDLMoveLayerDown.Enabled = false;
            btnDLMoveLayerTop.Enabled = false;
            btnDLMoveLayerUp.Enabled = false;
            btnDLRemoveLayer.Enabled = false;

            btnNewBaseLayerGroup.Enabled = false;
            btnAddBaseLayer.Enabled = false;
            btnRemoveBaseLayer.Enabled = bAllBaseLayers;
            btnRemoveBaseLayerGroup.Enabled = bAllBaseGroups;
            btnBaseLayerGroupToRegular.Enabled = bAllBaseGroups;

            propertiesPanel.Controls.Clear();

            var item = CreateMultiControl(nodes);
            _activeLayer = null;
            propertiesPanel.Controls.Add(item);
        }

        private IMapLayer _activeLayer;

        private void OnDrawOrderLayerItemSelected(LayerItem layer)
        {
            btnDLMoveLayerBottom.Enabled =
            btnDLMoveLayerDown.Enabled =
            btnDLMoveLayerTop.Enabled =
            btnDLMoveLayerUp.Enabled =
            btnDLRemoveLayer.Enabled = true;

            propertiesPanel.Controls.Clear();

            var item = CreateLayerControl(layer);
            _activeLayer = layer.Tag;
            propertiesPanel.Controls.Add(item);
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var selGroup = GetSelectedLayerGroupItem() as GroupItem;
            IMapLayerGroup parent = null;
            if (selGroup != null)
                parent = selGroup.Tag;
            var newGroup = CreateNewGroup(parent);

            _grpLayerModel.Invalidate();
            RestoreGroupSelection(newGroup);
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            var remove = new List<GroupItem>();
            foreach (var item in GetSelectedLayerGroupItems())
            {
                var group = item as GroupItem;
                if (group != null)
                {
                    remove.Add(group);
                }
            }
            RemoveSelectedLayerGroupItems(remove);
        }

        private void RemoveSelectedLayerGroupItems(IEnumerable<GroupItem> groups)
        {
            foreach (var group in groups)
            {
                _map.RemoveLayerGroupAndChildLayers(group.Tag.Name);
            }
            propertiesPanel.Controls.Clear();
            _grpLayerModel.Invalidate();
            _doLayerModel.Invalidate();
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
            foreach (var item in GetSelectedLayerGroupItems())
            {
                var layer = item as LayerItem;
                if (layer != null)
                {
                    RemoveSelectedLayerGroupItem(layer);
                }
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
            List<string> messages = new List<string>();
            foreach (var item in GetSelectedLayerGroupItems())
            {
                var group = item as GroupItem;
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
                    messages.Add(string.Format(Strings.LayerGroupConvertedToBaseLayerGroup, layGroup.Name, groupName));
                }
            }
            if (messages.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages.ToArray()));
                this.RefreshModels();
                tabControl1.SelectedIndex = 2; //Switch to Base Layer Groups
            }
        }

        private void btnBaseGroupToRegular_Click(object sender, EventArgs e)
        {
            var messages = new List<string>();
            foreach (var item in GetSelectedTiledLayerItems())
            {
                var group = item as BaseLayerGroupItem;
                if (group != null)
                {
                    int counter = 0;
                    string groupName = group.Tag.Name;
                    while (_map.GetGroupByName(groupName) != null)
                    {
                        counter++;
                        groupName = group.Tag.Name + "(" + counter + ")";
                    }
                    _map.AddGroup(groupName);
                    int layerCount = _map.GetLayerCount();
                    foreach (var layer in group.Tag.BaseMapLayer)
                    {
                        //We an avoid a duplicate name check because the Map Definition should already ensure uniqueness
                        //among existing layers
                        var dlayer = _map.AddLayer(groupName, layer.Name, layer.ResourceId);
                        dlayer.ExpandInLegend = layer.ExpandInLegend;
                        dlayer.LegendLabel = layer.LegendLabel;
                        dlayer.Selectable = layer.Selectable;
                        dlayer.ShowInLegend = layer.ShowInLegend;

                        //HACK-ish, but we need to relocate this
                        _map.RemoveLayer(dlayer);

                        //Add to bottom
                        _map.InsertLayer(layerCount, dlayer);
                        layerCount++;
                    }
                    //Detach the base layer group
                    _map.RemoveBaseLayerGroup(group.Tag, true);
                    messages.Add(string.Format(Strings.BaseLayerGroupConvertedToLayerGroup, group.Tag.Name, groupName));
                }
            }
            if (messages.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages.ToArray()));
                this.RefreshModels();
                tabControl1.SelectedIndex = 0; //Switch to Layer Groups
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
            string prefix = Strings.NewLayerGroup;
            var group = _map.GetGroupByName(prefix);
            while (group != null)
            {
                counter++;
                prefix = Strings.NewLayerGroup + counter;
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

        private IEnumerable<object> GetSelectedLayerGroupItems()
        {
            var result = new List<object>();
            var nodes = trvLayersGroup.SelectedNodes;
            if (nodes != null)
            {
                result.AddRange(nodes.Select(x => x.Tag));
            }
            return result;
        }

        private object GetSelectedTiledLayerItem()
        {
            if (trvBaseLayers.SelectedNode != null)
                return trvBaseLayers.SelectedNode.Tag;
            else
                return null;
        }

        private IEnumerable<object> GetSelectedTiledLayerItems()
        {
            var result = new List<object>();
            var nodes = trvBaseLayers.SelectedNodes;
            if (nodes != null)
            {
                result.AddRange(nodes.Select(x => x.Tag));
            }
            return result;
        }

        private void btnNewBaseLayerGroup_Click(object sender, EventArgs e)
        {
            _map.InitBaseMap();
            var grp = _map.BaseMap.AddBaseLayerGroup(GenerateBaseGroupName(_map));
            _tiledLayerModel.Invalidate();
        }

        private void btnRemoveBaseLayerGroup_Click(object sender, EventArgs e)
        {
            var remove = new List<BaseLayerGroupItem>();
            foreach (var item in GetSelectedTiledLayerItems())
            {
                var group = item as BaseLayerGroupItem;
                if (group != null)
                {
                    remove.Add(group);
                }
            }
            RemoveSelectedTiledLayerItems(remove);
        }

        private void RemoveSelectedTiledLayerItems(IEnumerable<BaseLayerGroupItem> groups)
        {
            foreach (var group in groups)
            {
                _map.RemoveBaseLayerGroup(group.Tag, true);
            }
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
        }

        private void RemoveSelectedTiledLayerItem(BaseLayerGroupItem group)
        {
            _map.RemoveBaseLayerGroup(group.Tag, true);
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
            string name = Strings.BaseLayerGroup;
            if (map.BaseMap.GroupExists(name))
            {
                counter++;
                name = Strings.BaseLayerGroup + counter;
            }
            while (map.BaseMap.GroupExists(name))
            {
                counter++;
                name = Strings.BaseLayerGroup + counter;
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
            foreach (var item in GetSelectedTiledLayerItems())
            {
                var layer = item as BaseLayerItem;
                if (layer != null)
                {
                    RemoveSelectedTiledLayerItem(layer);
                }
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

        /// <summary>
        /// Gets the index of the layer above the current layer index of the same group
        /// </summary>
        /// <param name="mdf"></param>
        /// <param name="layerIndex"></param>
        /// <param name="group"></param>
        /// <returns>The index of the layer below the current layer. Returns -1 if the current layer index is the top-most layer of the group</returns>
        private static int GetIndexOfLayerAbove(IMapDefinition mdf, int layerIndex, string group)
        {
            if (layerIndex > 0)
            {
                var list = new List<IMapLayer>(mdf.MapLayer);
                for (int i = layerIndex - 1; i >= 0; i--)
                {
                    if (list[i].Group == group)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the index of the layer below the current layer index of the same group
        /// </summary>
        /// <param name="mdf"></param>
        /// <param name="layerIndex"></param>
        /// <param name="group"></param>
        /// <returns>The index of the layer below the current layer. Returns -1 if the current layer index is the bottom-most layer of the group</returns>
        private static int GetIndexOfLayerBelow(IMapDefinition mdf, int layerIndex, string group)
        {
            if (layerIndex < mdf.GetLayerCount() - 1)
            {
                var list = new List<IMapLayer>(mdf.MapLayer);
                for (int i = layerIndex + 1; i < mdf.GetLayerCount(); i++)
                {
                    if (list[i].Group == group)
                        return i;
                }
            }
            return -1;
        }

        private void btnMoveLayerOrGroupUp_Click(object sender, EventArgs e)
        {
            object item = GetSelectedLayerGroupItem(); 
            var group = item as GroupItem;
            var layer = item as LayerItem;
            if (group != null)
            {
                var mdf = group.Tag.Parent;
                mdf.MoveUpGroup(group.Tag);

                _grpLayerModel.Invalidate();

                RestoreGroupSelection(group);
            }
            else if (layer != null)
            {
                var mdf = _map;
                var oLayer = layer.Tag;
                var layerIdx = mdf.GetIndex(oLayer);
                var newIndex = GetIndexOfLayerAbove(mdf, layerIdx, oLayer.Group);
                if (newIndex >= 0)
                {
                    mdf.RemoveLayer(oLayer);
                    mdf.InsertLayer(newIndex, oLayer);

                    _grpLayerModel.Invalidate();
                    _doLayerModel.Invalidate();     //This affects draw order too
                    RestoreLayerSelection(oLayer);
                }
                else
                {
                    MessageBox.Show(Strings.LayerAlreadyAtTopOfGroup);
                }
            }
        }

        private void btnMoveLayerOrGroupDown_Click(object sender, EventArgs e)
        {
            object item = GetSelectedLayerGroupItem();
            var group = item as GroupItem;
            var layer = item as LayerItem;
            if (group != null)
            {
                var mdf = group.Tag.Parent;
                mdf.MoveDownGroup(group.Tag);

                _grpLayerModel.Invalidate();

                RestoreGroupSelection(group);
            }
            else if (layer != null)
            {
                var mdf = _map;
                var oLayer = layer.Tag;
                var layerIdx = mdf.GetIndex(oLayer);
                var newIndex = GetIndexOfLayerBelow(mdf, layerIdx, oLayer.Group);
                if (newIndex >= 0)
                {
                    mdf.RemoveLayer(oLayer);
                    mdf.InsertLayer(newIndex, oLayer);

                    _grpLayerModel.Invalidate();
                    _doLayerModel.Invalidate();     //This affects draw order too
                    RestoreLayerSelection(oLayer);
                }
                else
                {
                    MessageBox.Show(Strings.LayerAlreadyAtBottomOfGroup);
                }
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
            btnBaseLayerGroupToRegular.Enabled = true;

            propertiesPanel.Controls.Clear();
            //var item = new GroupPropertiesCtrl(_map, group.Tag);
            //item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            //item.Dock = DockStyle.Fill;
            var item = CreateBaseGroupControl(group);
            _activeLayer = null;
            propertiesPanel.Controls.Add(item);
        }

        private void OnBaseLayerItemSelected(BaseLayerItem layer)
        {
            btnRemoveBaseLayer.Enabled = true;
            btnMoveBaseLayerDown.Enabled = true;
            btnMoveBaseLayerUp.Enabled = true;
            btnBaseLayerGroupToRegular.Enabled = false;

            propertiesPanel.Controls.Clear();
            //var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService, _edSvc);
            //item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            //item.Dock = DockStyle.Fill;
            var item = CreateBaseLayerControl(layer);
            _activeLayer = null;
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
                    {
                        parent = gi.Tag;
                    }
                    else
                    {
                        if (node.Parent != null)
                        {
                            gi = node.Parent.Tag as GroupItem;
                            if (gi != null)
                                parent = gi.Tag;
                        }
                    }
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
                    {
                        if (TargetIsAmongSource(nodes, gi) || TargetIsDescendant(nodes, gi))
                            return;

                        parent = gi.Tag;
                    }
                    else if (li != null)
                    {
                        return;
                        //parent = _map.GetGroupByName(li.Tag.Group);
                    }
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

        private static bool TargetIsAmongSource(TreeNodeAdv[] nodes, GroupItem target)
        {
            foreach (var n in nodes)
            {
                var gi = n.Tag as GroupItem;
                if (gi != null && gi == target)
                    return true;
            }
            return false;
        }

        private bool TargetIsDescendant(TreeNodeAdv[] nodes, GroupItem target)
        {
            foreach (var n in nodes)
            {
                var gi = n.Tag as GroupItem;
                if (gi != null)
                {
                    var grp = target.Tag;
                    while (!string.IsNullOrEmpty(grp.Group))
                    {
                        var parent = _map.GetGroupByName(grp.Group);
                        if (parent == gi.Tag)
                            return true;

                        grp = parent;
                    }
                }        
            }
            return false;
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

                    if (sourceLayer != null && targetGroup != null && targetGroup.GetIndex(sourceLayer) < 0) //Dropping to a different base layer group
                    {
                        var srcGroup = _map.BaseMap.GetGroupForLayer(sourceLayer);
                        srcGroup.RemoveBaseMapLayer(sourceLayer);
                        targetGroup.InsertLayer(0, sourceLayer);

                        _tiledLayerModel.Invalidate();

                        //Keep group expanded
                        if (tli != null)
                            RestoreBaseLayerSelection(sourceLayer);
                    }
                    else if (sourceLayer != null && targetLayer != null && sourceLayer != targetLayer)
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

        private void btnInvokeMgCooker_Click(object sender, EventArgs e)
        {
            if (_edSvc.IsNew || _edSvc.IsDirty)
            {
                MessageBox.Show(Strings.SaveMapBeforeTiling);
                return;
            }

            var conn = _map.CurrentConnection;
            //HACK: Can't support other connection types beyond HTTP atm
            if (!conn.ProviderName.ToLower().Contains("maestro.http"))
            {
                MessageBox.Show(string.Format(Strings.UnsupportedConnectionType, conn.ProviderName));
                return;
            }

            if (_map.BaseMap == null || _map.BaseMap.GroupCount == 0)
            {
                MessageBox.Show(Strings.NotATiledMap);
                return;
            }

            _edSvc.RunProcess("MgCooker",
                              "--provider=Maestro.Http",
                              "--connection-params=\"Url=" + conn.GetCustomProperty("BaseUrl").ToString() + ";SessionId=" + conn.SessionID + "\"",
                              "--mapdefinitions=" + _edSvc.ResourceID);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_activeLayer != null)
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0: //Logical
                        RestoreLayerSelection(_activeLayer);
                        break;
                    case 1: //Draw Order
                        RestoreDrawOrderSelection(_activeLayer);
                        break;
                    default:
                        _activeLayer = null;
                        break;
                }
            }
        }

        private void trvBaseLayers_SelectionChanged(object sender, EventArgs e)
        {
            if (trvBaseLayers.SelectedNodes.Count == 1)
            {
                TreeNodeAdv node = trvBaseLayers.SelectedNodes[0];
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
            else if (trvBaseLayers.SelectedNodes.Count > 1)
            {
                OnMultipleItemsSelected(trvBaseLayers.SelectedNodes);
            }
        }

        private void trvLayerDrawingOrder_SelectionChanged(object sender, EventArgs e)
        {
            if (trvLayerDrawingOrder.SelectedNodes.Count == 1)
            {
                TreeNodeAdv node = trvLayerDrawingOrder.SelectedNodes[0];
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
            else if (trvLayerDrawingOrder.SelectedNodes.Count > 1)
            {
                OnMultipleItemsSelected(trvLayerDrawingOrder.SelectedNodes);
            }
        }

        private void trvLayersGroup_SelectionChanged(object sender, EventArgs e)
        {
            if (trvLayersGroup.SelectedNodes.Count == 1)
            {
                TreeNodeAdv node = trvLayersGroup.SelectedNodes[0];
                if (node != null)
                {
                    var layer = node.Tag as LayerItem;
                    var group = node.Tag as GroupItem;

                    btnGRPRemoveLayer.Enabled = false;
                    btnRemoveGroup.Enabled = false;
                    btnMoveLayerOrGroupUp.Enabled = false;
                    btnMoveLayerOrGroupDown.Enabled = false;
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
            else if (trvLayersGroup.SelectedNodes.Count > 1)
            {
                OnMultipleItemsSelected(trvLayersGroup.SelectedNodes);
            }
        }
    }
}
