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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Login;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;

namespace RtMapInspector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;
        private IMappingService _mappingSvc;
        private RuntimeMap _rtMap;

        #region Design-time property wrappers

        class PointDecorator
        {
            private string _str;

            public PointDecorator(IPoint2D pt)
            {
                _str = "(" + pt.X + ", " + pt.Y + ")";
            }

            public override string ToString()
            {
                return _str;
            }
        }

        class BoxDecorator
        {
            private string _str;

            public BoxDecorator(IEnvelope env)
            {
                _str = string.Format("( ({0}, {1}),  ({2}, {3}) )", env.MinX, env.MinY, env.MaxX, env.MaxY);
            }

            public override string ToString()
            {
                return _str;
            }
        }

        class MapDecorator
        {
            private RuntimeMap _map;

            [Category("Map Properties")]
            [ReadOnly(true)]
            public string Name
            {
                get { return _map.Name; }
                set { _map.Name = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public Color BackgroundColor
            {
                get { return _map.BackgroundColor; }
                set { _map.BackgroundColor = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public int DisplayDpi
            {
                get { return _map.DisplayDpi; }
                set { _map.DisplayDpi = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public int DisplayHeight
            {
                get { return _map.DisplayHeight; }
                set { _map.DisplayWidth = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public int DisplayWidth
            {
                get { return _map.DisplayWidth; }
                set { _map.DisplayWidth = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public string ResourceId
            {
                get { return _map.ResourceID; }
                set { _map.ResourceID = value; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public string ObjectId
            {
                get { return _map.ObjectId; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public string CoordinateSystem
            {
                get { return _map.CoordinateSystem; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public double MetersPerUnit
            {
                get { return _map.MetersPerUnit; }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public PointDecorator ViewCenter
            {
                get { return new PointDecorator(_map.ViewCenter); }
            }

            [Category("Map Properties")]
            [ReadOnly(true)]
            public BoxDecorator DataExtent
            {
                get { return new BoxDecorator(_map.DataExtent); }
            }

            public MapDecorator(RuntimeMap map)
            {
                _map = map;
            }
        }

        class GroupDecorator
        {
            private RuntimeMapGroup _group;

            public GroupDecorator(RuntimeMapGroup group)
            {
                _group = group;
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public bool ExpandInLegend
            {
                get { return _group.ExpandInLegend; }
                set { _group.ExpandInLegend = value; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public string Group
            {
                get { return _group.Group; }
                set { _group.Group = value; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public string LegendLabel
            {
                get { return _group.LegendLabel; }
                set { _group.LegendLabel = value; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public string Name
            {
                get { return _group.Name; }
                set { _group.Name = value; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public string ObjectId
            {
                get { return _group.ObjectId; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public bool ShowInLegend
            {
                get { return _group.ShowInLegend; }
                set { _group.ShowInLegend = value; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public int Type
            {
                get { return _group.Type; }
            }

            [Category("Group properties")]
            [ReadOnly(true)]
            public bool Visible
            {
                get { return _group.Visible; }
                set { _group.Visible = value; }
            }
        }

        class LayerDecorator
        {
            private RuntimeMapLayer _layer;

            public LayerDecorator(RuntimeMapLayer layer)
            {
                _layer = layer;
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public double DisplayOrder
            {
                get { return _layer.DisplayOrder; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool ExpandInLegend
            {
                get { return _layer.ExpandInLegend; }
                set { _layer.ExpandInLegend = value; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string FeatureSourceID
            {
                get { return _layer.FeatureSourceID; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string Filter
            {
                get { return _layer.Filter; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string GeometryPropertyName
            {
                get { return _layer.GeometryPropertyName; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string Group
            {
                get { return _layer.Group; }
                set { _layer.Group = value; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool HasTooltips
            {
                get { return _layer.HasTooltips; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string[] IdentityProperties
            {
                get { return new IdentityPropertyCollection(_layer.IdentityProperties).ToArray(); }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string LayerDefinition
            {
                get { return _layer.LayerDefinitionID; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string LegendLabel
            {
                get { return _layer.LegendLabel; }
                set { _layer.LegendLabel = value; } 
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string Name
            {
                get { return _layer.Name; }
                set { _layer.Name = value; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool NeedsRefresh
            {
                get { return _layer.NeedsRefresh; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string ObjectId
            {
                get { return _layer.ObjectId; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string FeatureClass
            {
                get { return _layer.QualifiedClassName; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string[] ScaleRanges
            {
                get { return new ScaleRangeCollection(_layer.ScaleRanges).ToArray(); }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public string SchemaName
            {
                get { return _layer.SchemaName; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool Selectable
            {
                get { return _layer.Selectable; }
                set { _layer.Selectable = value; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool ShowInLegend
            {
                get { return _layer.ShowInLegend; }
                set { _layer.ShowInLegend = value; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public int Type
            {
                get { return _layer.Type; }
            }

            [Category("Layer Properties")]
            [ReadOnly(true)]
            public bool Visible
            {
                get { return _layer.Visible; }
                set { _layer.Visible = value; }
            }
        }

        class DrawOrderDisplayItem
        {
            public string Label { get; private set; }

            public LayerDecorator Decorator { get; private set; }

            public DrawOrderDisplayItem(RuntimeMapLayer layer)
            {
                this.Label = layer.Name + " (" + layer.LegendLabel + ")";
                this.Decorator = new LayerDecorator(layer);
            }

            public override string ToString()
            {
                return this.Label;
            }
        }

        class ScaleRangeCollection : List<string>
        {
            public ScaleRangeCollection(RuntimeMapLayer.ScaleRange[] ranges)
            {
                foreach(var s in ranges )
                {
                    Add(s.MinScale + " : " + s.MaxScale);
                }
            }
        }

        class IdentityPropertyCollection : List<string>
        {
            public IdentityPropertyCollection(PropertyInfo[] props)
            {
                foreach (var p in props)
                {
                    Add(p.Name + " (" + p.Type.FullName + ")");
                }
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var login = new LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                _conn = login.Connection;
                if (Array.IndexOf(_conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) < 0)
                {
                    MessageBox.Show(Strings.ErrIncompatibleConnection);
                    Application.Exit();
                }
                _mappingSvc = (IMappingService)_conn.GetService((int)ServiceType.Mapping);
            }
            else
            {
                Application.Exit();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            _rtMap = null;
            properties.SelectedObject = null;
            trvLayersAndGroups.Nodes.Clear();
            lstDrawOrder.Items.Clear();
            trvSelection.Nodes.Clear();
            if (rdMapName.Checked)
            {
                _rtMap = _mappingSvc.OpenMap(string.Format("Session:{0}//{1}.Map", txtSessionId.Text, txtMapName.Text));
            }
            else if (rdResourceId.Checked)
            {
                _rtMap = _mappingSvc.OpenMap(txtResourceId.Text);
            }

            if (_rtMap == null)
            {
                MessageBox.Show(Strings.ErrFailedRuntimeMapOpen);
                return;
            }

            InitTabs();
        }

        const int IDX_GROUP = 0;
        const int IDX_GROUP_HIDDEN = 1;
        const int IDX_LAYER = 2;
        const int IDX_LAYER_HIDDEN = 3;
        const int IDX_MAP = 4;

        private TreeNode CreateGroupNode(RuntimeMapGroup group)
        {
            var node = new TreeNode(group.LegendLabel);
            node.Tag = new GroupDecorator(group);
            node.ImageIndex = node.SelectedImageIndex = group.Visible ? IDX_GROUP : IDX_GROUP_HIDDEN;

            foreach (var grp in _rtMap.GetGroupsOfGroup(group.Name))
            {
                node.Nodes.Add(CreateGroupNode(grp));
            }

            foreach (var layer in _rtMap.GetLayersOfGroup(group.Name))
            {
                node.Nodes.Add(CreateLayerNode(layer));
            }

            return node;
        }

        private TreeNode CreateLayerNode(RuntimeMapLayer layer)
        {
            var node = new TreeNode((layer.LegendLabel != String.Empty) ? layer.LegendLabel : String.Format("[{0}]", layer.Name));
            node.Tag = new LayerDecorator(layer);
            node.ImageIndex = node.SelectedImageIndex = layer.Visible ? IDX_LAYER : IDX_LAYER_HIDDEN;
            return node;
        }

        private void InitTabs()
        {
            try
            {
                trvLayersAndGroups.BeginUpdate();
                var node = new TreeNode(_rtMap.Name);
                node.ImageIndex = node.SelectedImageIndex = IDX_MAP;
                node.Tag = new MapDecorator(_rtMap);
                trvLayersAndGroups.Nodes.Add(node);
                foreach (var group in _rtMap.Groups)
                {
                    if (group.Group == String.Empty)
                        node.Nodes.Add(CreateGroupNode(group));
                }
            }
            finally
            {
                trvLayersAndGroups.EndUpdate();
            }

            try
            {
                lstDrawOrder.BeginUpdate();
                foreach (var layer in _rtMap.Layers)
                {
                    lstDrawOrder.Items.Add(new DrawOrderDisplayItem(layer));
                }
            }
            finally
            {
                lstDrawOrder.EndUpdate();
            }

            var sel = _rtMap.Selection;
            if (sel != null)
            {
                try
                {
                    trvSelection.BeginUpdate();
                    for (int i = 0; i < sel.Count; i++)
                    {
                        var rtLayer = sel[i].Layer;
                        var node = new TreeNode(rtLayer.Name + " (" + sel[i].Count + " objects selected)");
                        node.Tag = new LayerDecorator(rtLayer);
                        node.ImageIndex = node.SelectedImageIndex = IDX_LAYER;
                        trvSelection.Nodes.Add(node);
                        for (int j = 0; j < sel[i].Count; j++)
                        {
                            node.Nodes.Add(Stringify(sel[i][j]));
                        }
                    }
                }
                finally
                {
                    trvSelection.EndUpdate();
                }
            }
        }

        static string Stringify(object[] values)
        {
            string[] list = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                list[i] = values[i].ToString();
            }
            return string.Join("', '", list);
        }

        private void trvLayersAndGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            properties.SelectedObject = e.Node.Tag;
        }

        private void lstDrawOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lstDrawOrder.SelectedItem as DrawOrderDisplayItem;
            if (item != null)
                properties.SelectedObject = item.Decorator;
        }
    }
}
