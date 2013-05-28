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
using System.Windows.Forms;

using Maestro.MapViewer;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.MapViewer.Model;
using System.Diagnostics;
using Maestro.Editors.MapDefinition.Live;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// A Live Map Editor component that displays the legend of the currently edited map
    /// </summary>
    public partial class LiveMapEditorLegend : UserControl
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public LiveMapEditorLegend()
        {
            InitializeComponent();
            legendCtrl.NodeSelected += new NodeEventHandler(OnInnerNodeSelected);
        }
        
        private void OnInnerNodeSelected(object sender, TreeNode e)
        {
            var h = this.NodeSelected;
            if (h != null)
                h(this, e);
        }

        /// <summary>
        /// Raised when a node in the legend is deleted
        /// </summary>
        public event NodeEventHandler NodeDeleted;

        /// <summary>
        /// Raised when a node in the legend is selected
        /// </summary>
        public event NodeEventHandler NodeSelected;
        
        /// <summary>
        /// Gets or sets the associated map viewer
        /// </summary>
        public IMapViewer Viewer
        {
            get { return legendCtrl.Viewer; }
            set { legendCtrl.Viewer = value; }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var map = this.Viewer.GetMap();
            if (map != null)
            {
                var diag = new Live.NewGroupDialog(map);
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
                    var group = mapSvc.CreateMapGroup(map, diag.GroupName);
                    group.LegendLabel = diag.GroupLabel;
                    group.Visible = true;
                    group.ShowInLegend = true;
                    map.Groups.Add(group);
                    legendCtrl.Viewer.RefreshMap();
                }
            }
        }

        internal static string GenerateUniqueName(string prefix, RuntimeMapLayerCollection layers)
        {
            int counter = 0;
            string name = prefix;
            while (layers[name] != null)
            {
                counter++;
                name = prefix + counter;
            }
            return name;
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            var map = this.Viewer.GetMap();
            if (map != null)
            {
                using (var picker = new ResourcePicker(map.CurrentConnection.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
                {
                    if (picker.ShowDialog() == DialogResult.OK)
                    {
                        var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
                        var layer = mapSvc.CreateMapLayer(map, ((ILayerDefinition)map.CurrentConnection.ResourceService.GetResource(picker.ResourceID)));
                        layer.Name = GenerateUniqueName(ResourceIdentifier.GetName(picker.ResourceID), map.Layers);
                        layer.LegendLabel = ResourceIdentifier.GetName(picker.ResourceID);
                        layer.Visible = true;
                        layer.ShowInLegend = true;
                        map.Layers.Insert(0, layer);
                        legendCtrl.Viewer.RefreshMap();
                    }
                }
            }
        }

        private void addLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var map = this.Viewer.GetMap();
            if (map != null)
            {
                var node = legendCtrl.SelectedNode;
                if (node != null)
                {
                    var grp = node.Tag as GroupNodeMetadata;
                    if (grp != null)
                    {
                        using (var picker = new ResourcePicker(map.CurrentConnection.ResourceService, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
                        {
                            if (picker.ShowDialog() == DialogResult.OK)
                            {
                                var mapSvc = (IMappingService)map.CurrentConnection.GetService((int)ServiceType.Mapping);
                                var layer = mapSvc.CreateMapLayer(map, ((ILayerDefinition)map.CurrentConnection.ResourceService.GetResource(picker.ResourceID)));
                                layer.Name = GenerateUniqueName(ResourceIdentifier.GetName(picker.ResourceID), map.Layers);
                                layer.LegendLabel = ResourceIdentifier.GetName(picker.ResourceID);
                                layer.Group = grp.Name;
                                layer.Visible = true;
                                layer.ShowInLegend = true;
                                map.Layers.Insert(0, layer);
                                legendCtrl.Viewer.RefreshMap();
                            }
                        }
                    }
                }
            }
        }

        private void removeThisGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var map = this.Viewer.GetMap();
            if (map != null)
            {
                var node = legendCtrl.SelectedNode;
                if (node != null)
                {
                    var grp = node.Tag as GroupNodeMetadata;
                    if (grp != null)
                    {
                        var group = map.Groups[grp.Name];
                        if (group != null)
                        {
                            map.Groups.Remove(group);
                            legendCtrl.Viewer.RefreshMap();
                        }
                    }
                }
            }
        }

        private void removeThisLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var map = this.Viewer.GetMap();
            if (map != null)
            {
                var node = legendCtrl.SelectedNode;
                if (node != null)
                {
                    var lyr = node.Tag as LayerNodeMetadata;
                    if (lyr != null)
                    {
                        var layer = map.Layers[lyr.Name];
                        if (layer != null)
                        {
                            map.Layers.Remove(layer);
                            legendCtrl.Viewer.RefreshMap();
                        }
                    }
                }
            }
        }

        private void AddLayerDefinition(ResourceDragMessage message, GroupNodeMetadata groupMeta)
        {
            if (ResourceIdentifier.GetResourceType(message.ResourceID) == ResourceTypes.LayerDefinition)
            {
                var map = this.Viewer.GetMap();
                var conn = map.CurrentConnection;
                var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                var ldf = (ILayerDefinition)conn.ResourceService.GetResource(message.ResourceID);
                var rtLayer = mapSvc.CreateMapLayer(map, ldf);

                //Set some desired properties if not already set
                if (string.IsNullOrEmpty(rtLayer.LegendLabel))
                    rtLayer.LegendLabel = rtLayer.Name;
                rtLayer.ShowInLegend = true;
                rtLayer.ExpandInLegend = true;
                rtLayer.Selectable = true;

                if (groupMeta != null)
                    rtLayer.Group = groupMeta.Name;

                map.Layers.Insert(0, rtLayer);
                this.Viewer.RefreshMap();
            }
        }

        /// <summary>
        /// Raised when a drag occurs
        /// </summary>
        public event ItemDragEventHandler ItemDrag
        {
            add { legendCtrl.ItemDrag += value; }
            remove { legendCtrl.ItemDrag -= value; }
        }

        internal void HandleDragDrop(DragEventArgs e)
        {
            var layer = e.Data.GetData(typeof(LayerDragMessage)) as LayerDragMessage;
            var group = e.Data.GetData(typeof(GroupDragMessage)) as GroupDragMessage;
            var res = e.Data.GetData(typeof(ResourceDragMessage)) as ResourceDragMessage;
            var pt = legendCtrl.PointToClient(new Point(e.X, e.Y));
            var node = legendCtrl.GetNodeAt(pt.X, pt.Y);
            if (node != null)
            {
                var groupMeta = node.Tag as GroupNodeMetadata;
                if (groupMeta != null)
                {
                    if (layer != null)
                    {
                        if (groupMeta.Name != layer.GroupName)
                        {
                            var map = this.Viewer.GetMap();
                            var layerObj = map.Layers[layer.LayerName];
                            layerObj.Group = groupMeta.Name;
                            map.Save();
                            this.legendCtrl.RefreshLegend(); //No viewer refresh. Group structure changes do not affect draw order
                        }
                    }
                    else if (group != null)
                    {
                        if (groupMeta.Name != group.GroupName)
                        {
                            var map = this.Viewer.GetMap();
                            var groupObj = map.Groups[group.GroupName];
                            groupObj.Group = groupMeta.Name;
                            map.Save();
                            this.legendCtrl.RefreshLegend(); //No viewer refresh. Group structure changes do not affect draw order
                        }
                    }
                    else if (res != null)
                    {
                        if (groupMeta != null)
                        {
                            AddLayerDefinition(res, groupMeta);
                        }
                    }
                }
            }
            else
            {
                if (layer != null)
                {
                    var map = this.Viewer.GetMap();
                    var layerObj = map.Layers[layer.LayerName];
                    layerObj.Group = string.Empty;
                    map.Save();
                    this.legendCtrl.RefreshLegend(); //No viewer refresh. Group structure changes do not affect draw order
                }
                else if (group != null)
                {
                    var map = this.Viewer.GetMap();
                    var groupObj = map.Groups[group.GroupName];
                    groupObj.Group = string.Empty;
                    map.Save();
                    this.legendCtrl.RefreshLegend(); //No viewer refresh. Group structure changes do not affect draw order
                }
                else if (res != null)
                {
                    AddLayerDefinition(res, null);
                }
            }
        }

        internal void HandleDragOver(DragEventArgs e)
        {
            var layer = e.Data.GetData(typeof(LayerDragMessage)) as LayerDragMessage;
            var group = e.Data.GetData(typeof(GroupDragMessage)) as GroupDragMessage;
            var res = e.Data.GetData(typeof(ResourceDragMessage)) as ResourceDragMessage;
            var pt = legendCtrl.PointToClient(new Point(e.X, e.Y));
            var node = legendCtrl.GetNodeAt(pt.X, pt.Y);
            if (node != null)
            {
                var groupMeta = node.Tag as GroupNodeMetadata;
                if (groupMeta != null)
                {
                    if (layer != null)
                    {
                        if (groupMeta.Name != layer.GroupName)
                            e.Effect = DragDropEffects.Copy;
                        else
                            e.Effect = DragDropEffects.None;
                    }
                    else if (group != null)
                    {
                        if (groupMeta.Name != group.GroupName)
                            e.Effect = DragDropEffects.Copy;
                        else
                            e.Effect = DragDropEffects.None;
                    }
                    else if (res != null)
                    {

                        if (groupMeta != null)
                            e.Effect = DragDropEffects.Copy;
                        else
                            e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else 
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                if (layer != null || group != null || res != null)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        internal void HandleDragEnter(DragEventArgs e)
        {
            Trace.TraceInformation("HandleDragEnter(e)");
        }

        internal void HandleItemDrag(ItemDragEventArgs e)
        {
            var node = e.Item as TreeNode;
            if (node != null)
            {
                var layerMeta = node.Tag as LayerNodeMetadata;
                var groupMeta = node.Tag as GroupNodeMetadata;
                if (layerMeta != null)
                {
                    this.DoDragDrop(new LayerDragMessage(layerMeta.ParentGroupName, layerMeta.Name), DragDropEffects.Copy);
                }
                else if (groupMeta != null)
                {
                    this.DoDragDrop(new GroupDragMessage(groupMeta.Name), DragDropEffects.Copy);
                }
            }
        }
    }
}
