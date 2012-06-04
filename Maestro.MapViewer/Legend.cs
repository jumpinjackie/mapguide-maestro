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
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Xml;
using System.IO;

namespace Maestro.MapViewer
{
    public partial class Legend : UserControl
    {
        const string IMG_BROKEN = "lc_broken";
        const string IMG_DWF = "lc_dwf";
        const string IMG_GROUP = "lc_group";
        const string IMG_RASTER = "lc_raster";
        const string IMG_SELECT = "lc_select";
        const string IMG_THEME = "lc_theme";
        const string IMG_UNSELECT = "lc_unselect";
        const string IMG_OTHER = "icon_etc";

        private RuntimeMap _map;

        private Image _selectableIcon;
        private Image _unselectableIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        public Legend()
        {
            InitializeComponent();
            this.ThemeCompressionLimit = 25;
        }

        private IMapViewer _viewer;

        public IMapViewer Viewer
        {
            get { return _viewer; }
            set
            { 
                _viewer = value;
                if (_viewer != null && !this.DesignMode)
                {
                    _map = _viewer.GetMap();
                    _viewer.MapRefreshed += new EventHandler(OnMapRefreshed);
                    _viewer.MapLoaded += new EventHandler(OnMapLoaded);
                    _selectableIcon = Properties.Resources.lc_select;
                    _unselectableIcon = Properties.Resources.lc_unselect;
                }
            }
        }

        void OnMapLoaded(object sender, EventArgs e)
        {
            _map = _viewer.GetMap();
        }

        void OnMapRefreshed(object sender, EventArgs e)
        {
            this.RefreshLegend();
        }

        private Dictionary<string, RuntimeMapLayer> _layers = new Dictionary<string, RuntimeMapLayer>();
        private Dictionary<string, RuntimeMapGroup> _groups = new Dictionary<string, RuntimeMapGroup>();
        private Dictionary<string, string> _layerDefinitionContents = new Dictionary<string, string>();

        /// <summary>
        /// Refreshes this component
        /// </summary>
        public void RefreshLegend()
        {
            if (_noUpdate)
                return;

            if (_map == null)
                return;

            //System.Diagnostics.Trace.TraceInformation("MgLegend.RefreshLegend()");
            var scale = _map.ViewScale;
            var groups = _map.Groups;
            var layers = _map.Layers;

            ResetTreeView();

            _layerDefinitionContents.Clear();
            _layers.Clear();
            _groups.Clear();

            IResourceService resSvc = _map.ResourceService;

            trvLegend.BeginUpdate();
            try
            {
                //Process groups first
                List<RuntimeMapGroup> remainingNodes = new List<RuntimeMapGroup>();
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    _groups.Add(group.ObjectId, group);
                    if (!group.ShowInLegend)
                        continue;

                    //Add ones without parents first.
                    if (!string.IsNullOrEmpty(group.Group))
                    {
                        remainingNodes.Add(group);
                    }
                    else
                    {
                        var node = CreateGroupNode(group);
                        trvLegend.Nodes.Add(node);
                    }

                    while (remainingNodes.Count > 0)
                    {
                        List<RuntimeMapGroup> toRemove = new List<RuntimeMapGroup>();
                        for (int j = 0; j < remainingNodes.Count; j++)
                        {
                            var parentGroupName = remainingNodes[j].Group;
                            var parentGroup = groups[parentGroupName];
                            var parentId = parentGroup.ObjectId;

                            var nodes = trvLegend.Nodes.Find(parentId, false);
                            if (nodes.Length == 1)
                            {
                                var node = CreateGroupNode(remainingNodes[j]);
                                nodes[0].Nodes.Add(node);
                                toRemove.Add(remainingNodes[j]);
                            }
                        }
                        //Whittle down this list
                        if (toRemove.Count > 0)
                        {
                            foreach (var g in toRemove)
                            {
                                remainingNodes.Remove(g);
                            }
                        }
                    }
                }

                //Now process layers
                for (int i = 0; i < layers.Count; i++)
                {
                    var lyr = layers[i];
                    var ldfId = lyr.LayerDefinitionID;

                    if (!_layerDefinitionContents.ContainsKey(ldfId.ToString()))
                    {
                        _layerDefinitionContents[ldfId.ToString()] = string.Empty;
                    }
                }

                //TODO: Surely we can optimize this better
                var keys = new List<string>(_layerDefinitionContents.Keys);
                foreach (var lid in keys)
                {
                    using (var sr = new StreamReader(resSvc.GetResourceXmlData(lid)))
                    {
                        _layerDefinitionContents[lid] = sr.ReadToEnd();
                    }
                }

                List<RuntimeMapLayer> remainingLayers = new List<RuntimeMapLayer>();
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];
                    _layers.Add(layer.ObjectId, layer);

                    bool display = layer.ShowInLegend;
                    bool visible = layer.IsVisibleAtScale(_map.ViewScale);
                    if (!display)
                        continue;

                    if (!visible)
                        continue;

                    //Add ones without parents first.
                    if (!string.IsNullOrEmpty(layer.Group))
                    {
                        remainingLayers.Add(layer);
                    }
                    else
                    {
                        var node = CreateLayerNode(layer);
                        if (node != null)
                        {
                            trvLegend.Nodes.Add(node);
                            if (layer.ExpandInLegend)
                                node.Expand();
                        }
                    }

                    while (remainingLayers.Count > 0)
                    {
                        List<RuntimeMapLayer> toRemove = new List<RuntimeMapLayer>();
                        for (int j = 0; j < remainingLayers.Count; j++)
                        {
                            var parentGroup = remainingLayers[j].GetParentGroup();
                            var parentId = parentGroup.ObjectId;
                            var nodes = trvLegend.Nodes.Find(parentId, false);
                            if (nodes.Length == 1)
                            {
                                var node = CreateLayerNode(remainingLayers[j]);
                                if (node != null)
                                {
                                    nodes[0].Nodes.Add(node);
                                    if (remainingLayers[j].ExpandInLegend)
                                        node.Expand();
                                }
                                toRemove.Add(remainingLayers[j]);
                            }
                        }
                        //Whittle down this list
                        if (toRemove.Count > 0)
                        {
                            foreach (var g in toRemove)
                            {
                                remainingLayers.Remove(g);
                            }
                        }
                    }
                }

                //Now expand any relevant groups
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    if (group.ExpandInLegend)
                    {
                        var nodes = trvLegend.Nodes.Find(group.ObjectId, false);
                        if (nodes.Length == 1)
                        {
                            nodes[0].Expand();
                        }
                    }
                }
            }
            finally
            {
                trvLegend.EndUpdate();
            }
        }

        private static void ClearNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                    ClearNodes(node.Nodes);

                var layerMeta = node.Tag as LayerNodeMetadata;
                if (layerMeta != null && layerMeta.ThemeIcon != null)
                {
                    layerMeta.Layer = null;
                    layerMeta.ThemeIcon.Dispose();
                    layerMeta.ThemeIcon = null;
                }
            }
            nodes.Clear();
        }

        private void ResetTreeView()
        {
            ClearNodes(trvLegend.Nodes);
            imgLegend.Images.Clear();

            imgLegend.Images.Add(IMG_BROKEN, Properties.Resources.lc_broken);
            imgLegend.Images.Add(IMG_DWF, Properties.Resources.lc_dwf);
            imgLegend.Images.Add(IMG_GROUP, Properties.Resources.lc_group);
            imgLegend.Images.Add(IMG_RASTER, Properties.Resources.lc_raster);
            imgLegend.Images.Add(IMG_SELECT, Properties.Resources.lc_select);
            imgLegend.Images.Add(IMG_THEME, Properties.Resources.lc_theme);
            imgLegend.Images.Add(IMG_UNSELECT, Properties.Resources.lc_unselect);
            imgLegend.Images.Add(IMG_OTHER, Properties.Resources.icon_etc);
        }

        private TreeNode CreateLayerNode(RuntimeMapLayer layer)
        {
            var node = new TreeNode();
            node.Name = layer.ObjectId;
            node.Text = layer.LegendLabel;
            node.Checked = layer.Visible;
            node.ContextMenuStrip = this.LayerContextMenu;
            var lt = layer.Type;
            var fsId = layer.FeatureSourceID;

            if (fsId.EndsWith("DrawingSource"))
            {
                node.SelectedImageKey = node.ImageKey = IMG_DWF;
                node.Tag = new LayerNodeMetadata(layer);
                node.ToolTipText = string.Format(Properties.Resources.DrawingLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID);
            }
            else
            {
                string layerData = null;
                var ldfId = layer.LayerDefinitionID;
                if (_layerDefinitionContents.ContainsKey(ldfId.ToString()))
                    layerData = _layerDefinitionContents[ldfId.ToString()];

                if (layerData == null)
                    return null;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(layerData);
                int type = 0;
                XmlNodeList scaleRanges = doc.GetElementsByTagName("VectorScaleRange");
                if (scaleRanges.Count == 0)
                {
                    scaleRanges = doc.GetElementsByTagName("GridScaleRange");
                    if (scaleRanges.Count == 0)
                    {
                        scaleRanges = doc.GetElementsByTagName("DrawingLayerDefinition");
                        if (scaleRanges.Count == 0)
                            return null;
                        type = 2;
                    }
                    else
                        type = 1;
                }

                String[] typeStyles = new String[] { "PointTypeStyle", "LineTypeStyle", "AreaTypeStyle", "CompositeTypeStyle" };
                String[] ruleNames = new String[] { "PointRule", "LineRule", "AreaRule", "CompositeRule" };

                try
                {
                    Image layerIcon = _map.GetLegendImage(layer.LayerDefinitionID,
                                                          _map.ViewScale,
                                                          16,
                                                          16,
                                                          "PNG",
                                                          -1,
                                                          -1);
                    if (layerIcon != null)
                    {
                        string id = Guid.NewGuid().ToString();
                        imgLegend.Images.Add(id, layerIcon);
                        node.SelectedImageKey = node.ImageKey = id;
                        node.Tag = new LayerNodeMetadata(layer)
                        {
                            ThemeIcon = layerIcon
                        };
                        node.ToolTipText = string.Format(Properties.Resources.DefaultLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName);
                    }
                    else
                    {
                        node.SelectedImageKey = node.ImageKey = IMG_BROKEN;
                    }
                }
                catch
                {
                    node.SelectedImageKey = node.ImageKey = IMG_BROKEN;
                }

                for (int sc = 0; sc < scaleRanges.Count; sc++)
                {
                    XmlElement scaleRange = (XmlElement)scaleRanges[sc];
                    XmlNodeList minElt = scaleRange.GetElementsByTagName("MinScale");
                    XmlNodeList maxElt = scaleRange.GetElementsByTagName("MaxScale");
                    String minScale, maxScale;
                    minScale = "0";
                    maxScale = "1000000000000.0";   // as MDF's VectorScaleRange::MAX_MAP_SCALE
                    if (minElt.Count > 0)
                        minScale = minElt[0].ChildNodes[0].Value;
                    if (maxElt.Count > 0)
                        maxScale = maxElt[0].ChildNodes[0].Value;
                    
                    if (type != 0)
                        break;

                    for (int geomType = 0; geomType < typeStyles.Length; geomType++)
                    {
                        int catIndex = 0;
                        XmlNodeList typeStyle = scaleRange.GetElementsByTagName(typeStyles[geomType]);
                        for (int st = 0; st < typeStyle.Count; st++)
                        {
                            // We will check if this typestyle is going to be shown in the legend
                            XmlNodeList showInLegend = ((XmlElement)typeStyle[st]).GetElementsByTagName("ShowInLegend");
                            if (showInLegend.Count > 0)
                                if (!bool.Parse(showInLegend[0].ChildNodes[0].Value))
                                    continue;   // This typestyle does not need to be shown in the legend

                            XmlNodeList rules = ((XmlElement)typeStyle[st]).GetElementsByTagName(ruleNames[geomType]);
                            if (rules.Count > 1)
                            {
                                node.SelectedImageKey = node.ImageKey = IMG_THEME;
                                var layerMeta = node.Tag as LayerNodeMetadata;
                                if (layerMeta != null)
                                {
                                    layerMeta.ThemeIcon = Properties.Resources.lc_theme;
                                    node.ToolTipText = string.Format(Properties.Resources.ThemedLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName, rules.Count);
                                }
                                if (this.ThemeCompressionLimit > 0 && rules.Count > this.ThemeCompressionLimit)
                                {
                                    AddThemeRuleNode(layer, node, geomType, 0, rules, 0);
                                    node.Nodes.Add(CreateCompressedThemeNode(rules.Count - 2));
                                    AddThemeRuleNode(layer, node, geomType, rules.Count - 1, rules, rules.Count - 1);
                                }
                                else
                                {
                                    for (int r = 0; r < rules.Count; r++)
                                    {
                                        AddThemeRuleNode(layer, node, geomType, catIndex++, rules, r);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return node;
        }

        private void AddThemeRuleNode(RuntimeMapLayer layer, TreeNode node, int geomType, int catIndex, XmlNodeList rules, int r)
        {
            XmlElement rule = (XmlElement)rules[r];
            XmlNodeList label = rule.GetElementsByTagName("LegendLabel");
            XmlNodeList filter = rule.GetElementsByTagName("Filter");

            String labelText = "";
            if (label != null && label.Count > 0 && label[0].ChildNodes.Count > 0)
                labelText = label[0].ChildNodes[0].Value;
            //String filterText = "";
            //if (filter != null && filter.Count > 0 && filter[0].ChildNodes.Count > 0)
            //    filterText = filter[0].ChildNodes[0].Value;

            var child = CreateThemeRuleNode(layer.LayerDefinitionID, _map.ViewScale, labelText, (geomType + 1), catIndex);
            node.Nodes.Add(child);
        }

        private TreeNode CreateCompressedThemeNode(int count)
        {
            TreeNode node = new TreeNode();
            node.Text = (count + " other styles");
            node.ImageKey = node.SelectedImageKey = IMG_OTHER;
            node.Tag = new LayerNodeMetadata(null) {
                IsBaseLayer = false,
                ThemeIcon = Properties.Resources.icon_etc,
                IsThemeRule = true
            };
            return node;
        }

        private TreeNode CreateThemeRuleNode(string layerDefId, double viewScale, string labelText, int geomType, int categoryIndex)
        {
            Image layerIcon = null;
            try
            {
                layerIcon = _map.GetLegendImage(layerDefId,
                                                      viewScale,
                                                      16,
                                                      16,
                                                      "PNG",
                                                      geomType,
                                                      categoryIndex);
            }
            catch
            {
                layerIcon = Properties.Resources.lc_broken;
            }
            TreeNode node = new TreeNode();
            node.Text = labelText;
            if (layerIcon != null)
            {
                var tag = new LayerNodeMetadata(null)
                {
                    IsBaseLayer = false,
                    IsThemeRule = true
                };
                string id = Guid.NewGuid().ToString();
                tag.ThemeIcon = layerIcon;
                node.Tag = tag;
            }
            return node;
        }

        private TreeNode CreateGroupNode(RuntimeMapGroup group)
        {
            var node = new TreeNode();
            node.Name = group.ObjectId;
            node.Text = group.LegendLabel;
            node.Checked = group.Visible;
            node.SelectedImageKey = node.ImageKey = IMG_GROUP;
            node.Tag = new GroupNodeMetadata(group);
            node.ContextMenuStrip = this.GroupContextMenu;
            return node;
        }

        private double _scale;

        /// <summary>
        /// Sets the applicable scale
        /// </summary>
        /// <param name="scale"></param>
        public void SetScale(double scale)
        {
            _scale = scale;
            RefreshLegend();
        }

        class LegendNodeMetadata
        {
            public bool IsGroup { get; protected set; }
        }

        class GroupNodeMetadata : LegendNodeMetadata
        {
            internal RuntimeMapGroup Group { get; set; }

            public GroupNodeMetadata(RuntimeMapGroup group) 
            { 
                base.IsGroup = true;
                this.Group = group;
            }
        }

        class LayerNodeMetadata : LegendNodeMetadata
        {
            public LayerNodeMetadata(RuntimeMapLayer layer) 
            { 
                base.IsGroup = false;
                this.Layer = layer;
                this.IsSelectable = (layer != null) ? layer.Selectable : false;
                this.DrawSelectabilityIcon = (layer != null);
                this.IsThemeRule = false;
            }

            internal RuntimeMapLayer Layer { get; set; }

            public bool DrawSelectabilityIcon { get; set; }

            public bool IsSelectable { get; set; }

            public bool IsThemeRule { get; set; }

            public bool IsBaseLayer { get; set; }

            public Image ThemeIcon { get; set; }
        }

        private bool HasVisibleParent(RuntimeMapGroup grp)
        {
            var current = _map.Groups[grp.Group];
            if (current != null)
                return current.Visible;
            return true;
        }

        private bool HasVisibleParent(RuntimeMapLayer layer)
        {
            var current = _map.Groups[layer.Group];
            if (current != null)
                return current.Visible;

            return true;
        }

        private void trvLegend_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            if (((LegendNodeMetadata)e.Node.Tag).IsGroup) //Group
            {
                if (_groups.ContainsKey(e.Node.Name))
                {
                    var grp = _groups[e.Node.Name];
                    grp.Visible = e.Node.Checked;
                    var bVis = HasVisibleParent(grp);
                    if (bVis)
                        OnRequestRefresh();
                }
            }
            else //Layer
            {
                if (_layers.ContainsKey(e.Node.Name))
                {
                    var layer = _layers[e.Node.Name];
                    layer.Visible = e.Node.Checked;
                    var bVis = HasVisibleParent(layer);
                    if (bVis)
                    {
                        layer.ForceRefresh();
                        OnRequestRefresh();
                    }
                }
            }
        }

        private void trvLegend_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            if (((LegendNodeMetadata)e.Node.Tag).IsGroup) //Group
            {
                if (_groups.ContainsKey(e.Node.Name))
                {
                    _groups[e.Node.Name].ExpandInLegend = true;
                    _map.Save();
                }
            }
            else //Layer
            {
                if (_layers.ContainsKey(e.Node.Name))
                {
                    var layer = _layers[e.Node.Name];
                    layer.ExpandInLegend = true;
                    _map.Save();
                }
            }
        }

        private void trvLegend_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            if (((LegendNodeMetadata)e.Node.Tag).IsGroup) //Group
            {
                if (_groups.ContainsKey(e.Node.Name))
                {
                    _groups[e.Node.Name].ExpandInLegend = true;
                    _map.Save();
                }
            }
            else //Layer
            {
                if (_layers.ContainsKey(e.Node.Name))
                {
                    var layer = _layers[e.Node.Name];
                    layer.ExpandInLegend = true;
                    _map.Save();
                }
            }
        }

        private bool _noUpdate = false;

        private void OnRequestRefresh()
        {
            if (this.Viewer != null)
                this.Viewer.RefreshMap();
        }

        private static bool IsThemeLayerNode(TreeNode node)
        {
            var meta = node.Tag as LayerNodeMetadata;
            if (meta != null)
                return meta.ThemeIcon != null || meta.IsBaseLayer;

            return false;
        }

        private static bool IsLayerNode(TreeNode node)
        {
            var meta = node.Tag as LayerNodeMetadata;
            return meta != null;
        }

        private void trvLegend_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (IsLayerNode(e.Node) && !e.Bounds.IsEmpty)
            {
                //TODO: Render +/- for nodes with children (ie. Themed layers)
                Color backColor, foreColor;

                //For some reason, the default bounds are way off from what you would
                //expect it to be. So we apply this offset for any text/image draw operations
                int xoffset = -36;

                if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                {
                    backColor = SystemColors.Highlight;
                    foreColor = SystemColors.HighlightText;
                }
                else if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
                {
                    backColor = SystemColors.HotTrack;
                    foreColor = SystemColors.HighlightText;
                }
                else
                {
                    backColor = e.Node.BackColor;
                    foreColor = e.Node.ForeColor;
                }

                var tag = e.Node.Tag as LayerNodeMetadata;
                var checkBoxOffset = xoffset;
                var selectabilityOffset = xoffset + 16;
                var iconOffsetNoSelect = xoffset + 16;
                if (tag != null && tag.IsThemeRule) //No checkbox for theme rule nodes
                {
                    selectabilityOffset = xoffset;
                    iconOffsetNoSelect = xoffset;
                }
                var iconOffset = selectabilityOffset + 20;
                var textOffset = iconOffset + 20;
                var textOffsetNoSelect = iconOffsetNoSelect + 20;

                //Uncomment if you need to "see" the bounds of the node
                //e.Graphics.DrawRectangle(Pens.Black, e.Node.Bounds);

                if (tag != null && !tag.IsThemeRule) //No checkbox for theme rule nodes
                {
                    CheckBoxRenderer.DrawCheckBox(
                        e.Graphics,
                        new Point(e.Node.Bounds.X + checkBoxOffset, e.Node.Bounds.Y),
                        e.Node.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
                }
                if (tag != null)
                {
                    if (tag.DrawSelectabilityIcon)
                    {
                        var icon = tag.IsSelectable ? _selectableIcon : _unselectableIcon;
                        e.Graphics.DrawImage(icon, e.Node.Bounds.X + selectabilityOffset, e.Node.Bounds.Y);
                        Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                    }
                    if (tag.ThemeIcon != null)
                    {
                        if (tag.DrawSelectabilityIcon)
                        {
                            e.Graphics.DrawImage(tag.ThemeIcon, e.Node.Bounds.X + iconOffset, e.Node.Bounds.Y);
                            Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                        }
                        else
                        {
                            e.Graphics.DrawImage(tag.ThemeIcon, e.Node.Bounds.X + iconOffsetNoSelect, e.Node.Bounds.Y);
                            Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                        }
                    }

                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.DrawString(e.Node.Text, trvLegend.Font, brush, e.Node.Bounds.X + (tag.DrawSelectabilityIcon ? textOffset : textOffsetNoSelect), e.Node.Bounds.Y);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.DrawString(e.Node.Text, trvLegend.Font, brush, e.Node.Bounds.X + 17.0f + xoffset, e.Node.Bounds.Y);
                    }
                }

                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private ContextMenuStrip _grpContextMenu;

        private ContextMenuStrip _layerContextMenu;

        /// <summary>
        /// Gets or sets the context menu that is attached to group nodes
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The context menu to attach to group nodes")]
        public ContextMenuStrip GroupContextMenu
        {
            get { return _grpContextMenu; }
            set
            {
                if (_grpContextMenu != null)
                    _grpContextMenu.Opening -= OnGroupContextMenuOpening;

                _grpContextMenu = value;
                if (_grpContextMenu != null)
                    _grpContextMenu.Opening += OnGroupContextMenuOpening;
                if (!this.DesignMode)
                    RefreshLegend();
            }
        }

        private void OnGroupContextMenuOpening(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// Gets or sets the context menu that is attached to layer nodes
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The context menu to attach to layer nodes")]
        public ContextMenuStrip LayerContextMenu
        {
            get { return _layerContextMenu; }
            set
            {
                if (_layerContextMenu != null)
                    _layerContextMenu.Opening -= OnLayerContextMenuOpening;

                _layerContextMenu = value;
                if (_layerContextMenu != null)
                    _layerContextMenu.Opening += OnLayerContextMenuOpening;
                if (!this.DesignMode)
                    RefreshLegend();
            }
        }

        private int _themeCompressionLimit;

        /// <summary>
        /// Gets or sets the theme compression limit.
        /// </summary>
        /// <value>
        /// The theme compression limit.
        /// </value>
        [Category("MapGuide Viewer")]
        [Description("The number of rules a layer style must exceed in order to be displayed as a compressed theme")]
        public int ThemeCompressionLimit
        {
            get { return _themeCompressionLimit; }
            set { _themeCompressionLimit = value; }
        }

        private void OnLayerContextMenuOpening(object sender, CancelEventArgs e)
        {

        }

        private void trvLegend_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trvLegend.SelectedNode = e.Node;
            }
            var meta = e.Node.Tag as LayerNodeMetadata;
            if (meta != null && meta.DrawSelectabilityIcon)
            {
                //Toggle layer's selectability if it's within the bounds of the selectability icon
                var box = new Rectangle(
                    new Point((e.Node.Bounds.Location.X - 36) + 16, e.Node.Bounds.Location.Y), 
                    new Size(16, e.Node.Bounds.Height));
                if (box.Contains(e.X, e.Y))
                {
                    var layer = meta.Layer;
                    layer.Selectable = !layer.Selectable;
                    _map.Save();
                    meta.IsSelectable = layer.Selectable;

                    //TODO: This bounds is a guess. We should calculate the bounds as part of node rendering, so we know the exact bounds by which to invalidate
                    trvLegend.Invalidate(new Rectangle(e.Node.Bounds.Location.X - 36, e.Node.Bounds.Location.Y, e.Node.Bounds.Width + 36, e.Node.Bounds.Height));
                }
            }
        }

        /// <summary>
        /// Gets the selected layer
        /// </summary>
        /// <returns></returns>
        public RuntimeMapLayer GetSelectedLayer()
        {
            if (_map == null)
                return null;

            if (trvLegend.SelectedNode == null)
                return null;

            var lyr = trvLegend.SelectedNode.Tag as LayerNodeMetadata;
            if (lyr != null)
                return lyr.Layer;

            return null;
        }

        /// <summary>
        /// Gets the selected group
        /// </summary>
        /// <returns></returns>
        public RuntimeMapGroup GetSelectedGroup()
        {
            if (_map == null)
                return null;

            if (trvLegend.SelectedNode == null)
                return null;

            var grp = trvLegend.SelectedNode.Tag as GroupNodeMetadata;
            if (grp != null)
                return grp.Group;

            return null;
        }

        public bool ShowTooltips
        {
            get { return trvLegend.ShowNodeToolTips; }
            set { trvLegend.ShowNodeToolTips = value; }
        }
    }
}
