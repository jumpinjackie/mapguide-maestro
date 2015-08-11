#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using Maestro.Editors.MapDefinition;
using Aga.Controls.Tree;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Tile;

namespace Maestro.Editors.TileSetDefinition
{
    [ToolboxItem(false)]
    internal partial class LayerStructureCtrl : EditorBindableCollapsiblePanel
    {
        public LayerStructureCtrl()
        {
            InitializeComponent();
        }

        private TiledLayerModel _tiledLayerModel;
        private ITileSetDefinition _tsd;
        private IEditorService _edSvc;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);
            _tsd = (ITileSetDefinition)service.GetEditedResource();
            trvBaseLayers.Model = _tiledLayerModel = new TiledLayerModel(_tsd);
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

        private static string GenerateBaseGroupName(ITileSetDefinition tsd)
        {
            int counter = 0;
            string name = Strings.BaseLayerGroup;
            if (tsd.GroupExists(name))
            {
                counter++;
                name = Strings.BaseLayerGroup + counter;
            }
            while (tsd.GroupExists(name))
            {
                counter++;
                name = Strings.BaseLayerGroup + counter;
            }
            return name;
        }

        private void btnNewBaseLayerGroup_Click(object sender, EventArgs e)
        {
            var grp = _tsd.AddBaseLayerGroup(GenerateBaseGroupName(_tsd));
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

        private void btnAddBaseLayer_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.LayerDefinition.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    string layerId = picker.ResourceID;
                    IBaseMapGroup grp = null;
                    var group = GetSelectedTiledLayerItem() as BaseLayerGroupItem;
                    if (group != null)
                    {
                        grp = group.Tag;
                    }
                    else
                    {
                        grp = _tsd.GetFirstGroup();
                        if (grp == null)
                        {
                            grp = _tsd.AddBaseLayerGroup(GenerateBaseGroupName(_tsd));
                        }
                    }
                    var bl = grp.AddLayer(GenerateBaseLayerName(layerId, _tsd), layerId);
                    _tiledLayerModel.Invalidate();
                    RestoreBaseLayerSelection(bl);
                }
            }
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

        private void btnInvokeMgCooker_Click(object sender, EventArgs e)
        {
            if (_edSvc.IsNew || _edSvc.IsDirty)
            {
                MessageBox.Show(Strings.SaveMapBeforeTiling);
                return;
            }

            if (_tsd.TileStoreParameters.TileProvider != "Default") //NOXLATE
            {
                MessageBox.Show(Maestro.Editors.Strings.MgCookerIncompatibleTileSet);
                return;
            }

            var conn = _edSvc.CurrentConnection;
            //HACK: Can't support other connection types beyond HTTP atm
            if (!conn.ProviderName.ToLower().Contains("maestro.http"))
            {
                MessageBox.Show(string.Format(Strings.UnsupportedConnectionType, conn.ProviderName));
                return;
            }

            if (_tsd.GroupCount == 0)
            {
                MessageBox.Show(Strings.NotATiledMap);
                return;
            }

            _edSvc.RunProcess("MgCooker",
                              "--" + TileRunParameters.PROVIDER + "=Maestro.Http",
                              "--" + TileRunParameters.CONNECTIONPARAMS + "=\"Url=" + conn.GetCustomProperty("BaseUrl").ToString() + ";SessionId=" + conn.SessionID + "\"",
                              "--" + TileRunParameters.MAPDEFINITIONS + "=" + _edSvc.ResourceID);
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
                    group = _tsd.AddBaseLayerGroup(GenerateBaseGroupName(_tsd));
                }

                IBaseMapLayer focusLayer = null;
                foreach (var rid in rids)
                {
                    if (rid.ResourceId.ResourceType == ResourceTypes.LayerDefinition.ToString())
                    {
                        focusLayer = group.AddLayer(GenerateBaseLayerName(rid.ResourceId.ToString(), _tsd), rid.ResourceId.ToString());
                        added++;
                    }
                }

                if (added > 0)
                {
                    _tiledLayerModel.Invalidate();
                    if (focusLayer != null)
                        RestoreBaseLayerSelection(focusLayer);
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
                        var srcGroup = _tsd.GetGroupForLayer(sourceLayer);
                        srcGroup.RemoveBaseMapLayer(sourceLayer);
                        targetGroup.InsertLayer(0, sourceLayer);

                        _tiledLayerModel.Invalidate();

                        //Keep group expanded
                        if (tli != null)
                            RestoreBaseLayerSelection(sourceLayer);
                    }
                    else if (sourceLayer != null && targetLayer != null && sourceLayer != targetLayer)
                    {
                        var srcGroup = _tsd.GetGroupForLayer(sourceLayer);
                        var dstGroup = _tsd.GetGroupForLayer(targetLayer);

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
            if (rids.Length == 1 && rids[0].ResourceId.ResourceType != ResourceTypes.LayerDefinition.ToString())
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //Even in multiples
            foreach (var r in rids)
            {
                if (r.ResourceId.ResourceType != ResourceTypes.LayerDefinition.ToString())
                {
                    e.Effect = DragDropEffects.None;
                    return;
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
                if (rids[0].Connection != _edSvc.CurrentConnection)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }

            e.Effect = DragDropEffects.Copy;
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

        private void trvBaseLayers_SelectionChanged(object sender, EventArgs e)
        {
            if (trvBaseLayers.SelectedNodes.Count == 1)
            {
                TreeNodeAdv node = trvBaseLayers.SelectedNodes[0];
                if (node != null)
                {
                    var layer = node.Tag as BaseLayerItem;
                    var group = node.Tag as BaseLayerGroupItem;

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
                }
            }
            else if (trvBaseLayers.SelectedNodes.Count > 1)
            {
                OnMultipleItemsSelected(trvBaseLayers.SelectedNodes);
            }
        }

        private void trvBaseLayers_KeyUp(object sender, KeyEventArgs e)
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

        public event OpenLayerEventHandler RequestLayerOpen;

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

        private void OnBaseLayerGroupItemSelected(BaseLayerGroupItem group)
        {
            btnAddBaseLayer.Enabled = true;
            btnRemoveBaseLayerGroup.Enabled = true;

            propertiesPanel.Controls.Clear();
            //var item = new GroupPropertiesCtrl(_tsd, group.Tag);
            //item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            //item.Dock = DockStyle.Fill;
            AddBaseGroupControl(group);
        }

        private void OnBaseLayerItemSelected(BaseLayerItem layer)
        {
            btnRemoveBaseLayer.Enabled = true;
            btnMoveBaseLayerDown.Enabled = true;
            btnMoveBaseLayerUp.Enabled = true;

            //var item = new LayerPropertiesCtrl(layer.Tag, _edSvc.ResourceService, _edSvc);
            //item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            //item.Dock = DockStyle.Fill;
            AddBaseLayerControl(layer);
        }

        private void AddMultiControl(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            propertiesPanel.Controls.Clear();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            List<object> values = new List<object>();
            for (int i = 0; i < nodes.Count; i++)
            {
                var bgrp = nodes[i].Tag as BaseLayerGroupItem;
                var blyr = nodes[i].Tag as BaseLayerItem;
                if (bgrp != null)
                {
                    values.Add(new Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseGroupItemDesigner(bgrp));
                }
                else if (blyr != null)
                {
                    values.Add(new Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseLayerItemDesigner(blyr));
                }
            }

            propertiesPanel.Controls.Add(commCtrl);

            commCtrl.SelectedObjects = values.ToArray();
        }

        private void AddBaseGroupControl(BaseLayerGroupItem group)
        {
            propertiesPanel.Controls.Clear();

            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            var item = new GroupPropertiesCtrl(_tsd, group.Tag);
            //item.GroupChanged += (s, evt) => { OnResourceChanged(); };
            item.GroupChanged += WeakEventHandler.Wrap((s, evt) => OnResourceChanged(), (eh) => item.GroupChanged -= eh);
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);

            ctrl.Dock = DockStyle.Fill;

            propertiesPanel.Controls.Add(ctrl);

            commCtrl.SelectedObject = new Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseGroupItemDesigner(group);
        }

        private void AddBaseLayerControl(BaseLayerItem layer)
        {
            propertiesPanel.Controls.Clear();

            Control ctrl = new Control();

            CommonPropertyCtrl commCtrl = new CommonPropertyCtrl();
            commCtrl.Dock = DockStyle.Fill;

            var item = new LayerPropertiesCtrl(layer.Tag, _edSvc);
            //item.LayerChanged += (s, evt) => { OnResourceChanged(); };
            item.LayerChanged += WeakEventHandler.Wrap((s, evt) => OnResourceChanged(), (eh) => item.LayerChanged -= eh);
            item.Dock = DockStyle.Top;

            ctrl.Controls.Add(commCtrl);
            ctrl.Controls.Add(item);

            ctrl.Dock = DockStyle.Fill;

            propertiesPanel.Controls.Add(ctrl);

            commCtrl.SelectedObject = new Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseLayerItemDesigner(layer);
        }

        private void RemoveSelectedTiledLayerItems(IEnumerable<BaseLayerGroupItem> groups)
        {
            foreach (var group in groups)
            {
                _tsd.RemoveBaseLayerGroup(group.Tag);
            }
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
        }

        private void RemoveSelectedTiledLayerItem(BaseLayerItem layer)
        {
            var grp = layer.Parent;
            grp.RemoveBaseMapLayer(layer.Tag);
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
        }

        private void RemoveSelectedTiledLayerItem(BaseLayerGroupItem group)
        {
            _tsd.RemoveBaseLayerGroup(group.Tag);
            propertiesPanel.Controls.Clear();
            _tiledLayerModel.Invalidate();
        }

        private void OnMultipleItemsSelected(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            bool bAllLayers = AllLayers(nodes);
            bool bAllGroups = AllGroups(nodes);
            bool bAllBaseLayers = AllBaseLayers(nodes);
            bool bAllBaseGroups = AllBaseGroups(nodes);

            btnNewBaseLayerGroup.Enabled = false;
            btnAddBaseLayer.Enabled = false;
            btnRemoveBaseLayer.Enabled = bAllBaseLayers;
            btnRemoveBaseLayerGroup.Enabled = bAllBaseGroups;

            AddMultiControl(nodes);
        }
        
        private static bool AllLayers(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var layer = node.Tag as LayerItem;
                if (layer == null)
                    return false;
            }
            return true;
        }

        private static bool AllBaseLayers(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var layer = node.Tag as BaseLayerItem;
                if (layer == null)
                    return false;
            }
            return true;
        }

        private static bool AllGroups(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var group = node.Tag as GroupItem;
                if (group == null)
                    return false;
            }
            return true;
        }

        private static bool AllBaseGroups(System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeAdv> nodes)
        {
            foreach (var node in nodes)
            {
                var group = node.Tag as BaseLayerGroupItem;
                if (group == null)
                    return false;
            }
            return true;
        }

        private static string GenerateBaseLayerName(string layerId, ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotEmpty(layerId, nameof(layerId));

            int counter = 0;
            string prefix = ResourceIdentifier.GetName(layerId);
            string name = prefix;
            if (tileSet.LayerExists(name))
            {
                name = prefix + counter;
            }
            while (tileSet.LayerExists(name))
            {
                counter++;
                name = prefix + counter;
            }

            return name;
        }

        private void RestoreBaseLayerSelection(IBaseMapLayer layer)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<BaseLayerItem>(trvBaseLayers, (tag) => { return tag.Tag == layer; });
            if (it != null)
                OnBaseLayerItemSelected(it);
        }

        private void RestoreBaseLayerSelection(BaseLayerItem item)
        {
            //The node tag will probably be different, but the wrapped
            //instance is what we're checking for
            var it = RestoreSelection<BaseLayerItem>(trvBaseLayers, (tag) => { return tag.Tag == item.Tag; });
            if (it != null)
                OnBaseLayerItemSelected(it);
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
    }
}
