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
using Maestro.MapViewer.Model;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Maestro.MapViewer
{
    class LegendControlPresenter
    {
        const string IMG_BROKEN = "lc_broken";
        const string IMG_DWF = "lc_dwf";
        const string IMG_GROUP = "lc_group";
        const string IMG_RASTER = "lc_raster";
        const string IMG_SELECT = "lc_select";
        const string IMG_THEME = "lc_theme";
        const string IMG_UNSELECT = "lc_unselect";
        const string IMG_OTHER = "icon_etc";

        private IResourceService _resSvc;
        private IServerConnection _provider;
        private RuntimeMap _map;

        private Image _selectableIcon;
        private Image _unselectableIcon;

        private Legend _legend;

        private Dictionary<string, LayerNodeMetadata> _layers = new Dictionary<string, LayerNodeMetadata>();
        private Dictionary<string, GroupNodeMetadata> _groups = new Dictionary<string, GroupNodeMetadata>();

        public LegendControlPresenter(Legend legend, RuntimeMap map)
        {
            _legend = legend;
            _map = map;
            _provider = _map.CurrentConnection;
            _resSvc = _provider.ResourceService;
            InitInitialSelectabilityStates();
            _selectableIcon = Properties.Resources.lc_select;
            _unselectableIcon = Properties.Resources.lc_unselect;
        }

        private void InitInitialSelectabilityStates()
        {
            if (_map != null)
            {
                _layers.Clear();
                var layers = _map.Layers;
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];
                    RuntimeMapGroup group = null;
                    if (!string.IsNullOrEmpty(layer.Group))
                        group = _map.Groups[layer.Group];
                    _layers[layer.ObjectId] = new LayerNodeMetadata(layer, layer.Selectable);
                }
            }
        }

        private bool HasVisibleParent(RuntimeMapGroup grp)
        {
            var current = grp.Group;
            if (current != null)
            {
                var parent = _map.Groups[current];
                if (parent != null)
                {
                    return parent.Visible;
                }
            }
            return true;
        }

        private bool HasVisibleParent(RuntimeMapLayer layer)
        {
            var current = layer.Group;
            if (current != null)
            {
                var parent = _map.Groups[current];
                if (parent != null)
                {
                    return parent.Visible;
                }
            }
            return true;
        }

        class RuleData
        {
            public int GeomType;
            public XmlNodeList RuleNodes;
        }

        private TreeNode CreateLayerNode(RuntimeMapLayer layer)
        {
            var node = new TreeNode();
            node.Name = layer.ObjectId;
            node.Text = layer.LegendLabel;
            node.Checked = layer.Visible;
            //node.ContextMenuStrip = _legend.LayerContextMenu;
            var lt = layer.Type;
            var fsId = layer.FeatureSourceID;

            LayerNodeMetadata layerMeta = null;
            if (fsId.EndsWith("DrawingSource"))
            {
                node.SelectedImageKey = node.ImageKey = IMG_DWF;
                bool bInitiallySelectable = layer.Selectable;
                if (_layers.ContainsKey(layer.ObjectId))
                {
                    layerMeta = _layers[layer.ObjectId];
                    bInitiallySelectable = layerMeta.WasInitiallySelectable;
                }
                else //If not in the dictionary, assume it is a dynamically added layer
                {
                    RuntimeMapGroup group = null;
                    if (!string.IsNullOrEmpty(layer.Group))
                        group = _map.Groups[layer.Group];
                    layerMeta = new LayerNodeMetadata(layer, bInitiallySelectable);
                    _layers[layer.ObjectId] = layerMeta;
                }
                node.Tag = layerMeta;
                node.ToolTipText = string.Format(Properties.Resources.DrawingLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID);
            }
            else //Vector or Grid layer
            {
                var ldfId = layer.LayerDefinitionID;
                if (_layers.ContainsKey(layer.ObjectId))
                {
                    layerMeta = _layers[layer.ObjectId];
                }
                else
                {
                    RuntimeMapGroup group = null;
                    if (!string.IsNullOrEmpty(layer.Group))
                        group = _map.Groups[layer.Group];
                    layerMeta = new LayerNodeMetadata(layer, layer.Selectable);
                    _layers[layer.ObjectId] = layerMeta;
                }
                if (string.IsNullOrEmpty(layerMeta.LayerDefinitionContent))
                    return null;

                node.Tag = layerMeta;

                const int LAYER_VECTOR = 0;
                const int LAYER_RASTER = 1;
                const int LAYER_DWF = 2;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(layerMeta.LayerDefinitionContent);
                int type = LAYER_VECTOR;
                XmlNodeList scaleRanges = doc.GetElementsByTagName("VectorScaleRange");
                if (scaleRanges.Count == 0)
                {
                    scaleRanges = doc.GetElementsByTagName("GridScaleRange");
                    if (scaleRanges.Count == 0)
                    {
                        scaleRanges = doc.GetElementsByTagName("DrawingLayerDefinition");
                        if (scaleRanges.Count == 0)
                            return null;
                        type = LAYER_DWF;
                    }
                    else
                        type = LAYER_RASTER;
                }

                String[] typeStyles = new String[] { "PointTypeStyle", "LineTypeStyle", "AreaTypeStyle", "CompositeTypeStyle" };
                String[] ruleNames = new String[] { "PointRule", "LineRule", "AreaRule", "CompositeRule" };

                node.ToolTipText = string.Format(Properties.Resources.DefaultLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName);
                //Do this if not cached already from a previous run
                if (!layerMeta.HasTheme() || !layerMeta.HasDefaultIcons())
                {
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

                        if (type != LAYER_VECTOR)
                            break;

                        bool bComposite = false;

                        //Check TS count. Give precedence to composite type styles
                        List<XmlNode> typeStyleCol = new List<XmlNode>();
                        XmlNodeList styleNodes = scaleRange.GetElementsByTagName(typeStyles[3]);
                        List<RuleData> rules = new List<RuleData>();
                        if (styleNodes.Count > 0)
                        {
                            foreach (XmlNode n in styleNodes)
                            {
                                // We will check if this typestyle is going to be shown in the legend
                                XmlNodeList showInLegend = ((XmlElement)n).GetElementsByTagName("ShowInLegend"); //NOXLATE
                                if (showInLegend.Count > 0)
                                    if (!bool.Parse(showInLegend[0].ChildNodes[0].Value))
                                        continue;   // This typestyle does not need to be shown in the legend

                                typeStyleCol.Add(n);

                                var ruleData = new RuleData();
                                ruleData.GeomType = 3;
                                ruleData.RuleNodes = ((XmlElement)n).GetElementsByTagName(ruleNames[3]);
                                if (ruleData.RuleNodes.Count > 0)
                                    rules.Add(ruleData);
                            }

                            bComposite = true;
                        }
                        else
                        {
                            for (int t = 0; t < 3; t++)
                            {
                                styleNodes = scaleRange.GetElementsByTagName(typeStyles[t]);
                                foreach (XmlNode n in styleNodes)
                                {
                                    // We will check if this typestyle is going to be shown in the legend
                                    XmlNodeList showInLegend = ((XmlElement)n).GetElementsByTagName("ShowInLegend"); //NOXLATE
                                    if (showInLegend.Count > 0)
                                        if (!bool.Parse(showInLegend[0].ChildNodes[0].Value))
                                            continue;   // This typestyle does not need to be shown in the legend

                                    typeStyleCol.Add(n);

                                    var ruleData = new RuleData();
                                    ruleData.GeomType = t;
                                    ruleData.RuleNodes = ((XmlElement)n).GetElementsByTagName(ruleNames[t]);
                                    if (ruleData.RuleNodes.Count > 0)
                                        rules.Add(ruleData);
                                }
                            }
                        }

                        //No type styles. Skip
                        if (typeStyleCol.Count == 0)
                            continue;

                        //Determine if this is themed or not
                        int nTotalRules = 0;
                        foreach (RuleData r in rules)
                        {
                            nTotalRules += r.RuleNodes.Count;
                        }
                        bool bThemed = nTotalRules > 1;
                        if (bThemed)
                        {
                            int catIndex = 0;
                            for (int i = 0; i < rules.Count; i++)
                            {
                                RuleData theRule = rules[i];
                                ThemeCategory themeCat = new ThemeCategory()
                                {
                                    MinScale = minScale,
                                    MaxScale = maxScale,
                                    GeometryType = theRule.GeomType
                                };

                                //Non-composite styles must be processed once
                                if (layerMeta.CategoryExists(themeCat) && theRule.GeomType != 3)
                                    continue;

                                layerMeta.SetDefaultIcon(themeCat, Properties.Resources.lc_theme);
                                node.ToolTipText = string.Format(Properties.Resources.ThemedLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName, nTotalRules);

                                if (_legend.ThemeCompressionLimit > 0 && theRule.RuleNodes.Count > _legend.ThemeCompressionLimit)
                                {
                                    AddThemeRuleNode(layerMeta, themeCat, node, theRule.GeomType, catIndex, theRule.RuleNodes, 0);
                                    node.Nodes.Add(CreateCompressedThemeNode(layerMeta, themeCat, theRule.RuleNodes.Count - 2));
                                    AddThemeRuleNode(layerMeta, themeCat, node, theRule.GeomType, (catIndex + (theRule.RuleNodes.Count - 1)), theRule.RuleNodes, theRule.RuleNodes.Count - 1);
                                }
                                else
                                {
                                    for (int r = 0; r < theRule.RuleNodes.Count; r++)
                                    {
                                        AddThemeRuleNode(layerMeta, themeCat, node, theRule.GeomType, (catIndex + r), theRule.RuleNodes, r);
                                    }
                                }
                                //Only bump catIndex if composite, as category indexes for composite styles are handled differently
                                if (bComposite)
                                    catIndex += theRule.RuleNodes.Count;
                            }
                        }
                        else
                        {
                            Trace.Assert(rules.Count == 1);
                            Trace.Assert(rules[0].RuleNodes.Count == 1);
                            RuleData theRule = rules[0];

                            ThemeCategory themeCat = new ThemeCategory()
                            {
                                MinScale = minScale,
                                MaxScale = maxScale,
                                GeometryType = theRule.GeomType
                            };

                            if (layerMeta.CategoryExists(themeCat))
                                continue;

                            if (LayerNodeMetadata.ScaleIsApplicable(_map.ViewScale, themeCat))
                            {
                                if (!layerMeta.HasDefaultIconsAt(_map.ViewScale))
                                {
                                    try
                                    {
                                        var img = _map.GetLegendImage(layer.LayerDefinitionID,
                                                                                _map.ViewScale,
                                                                                16,
                                                                                16,
                                                                                "PNG",
                                                                                (theRule.GeomType + 1),
                                                                                -1);
                                        legendCallCount++;
                                        layerMeta.SetDefaultIcon(themeCat, img);
                                        node.ToolTipText = string.Format(Properties.Resources.DefaultLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName);
                                    }
                                    catch
                                    {
                                        //layerMeta.SetDefaultIcon(themeCat, Properties.Resources.lc_broken);
                                    }
                                }
                            }
                        }

                        //for (int geomType = 0; geomType < typeStyles.Length; geomType++)
                        //{
                        //    ThemeCategory themeCat = new ThemeCategory()
                        //    {
                        //        MinScale = minScale,
                        //        MaxScale = maxScale,
                        //        GeometryType = geomType
                        //    };

                        //    if (layerMeta.CategoryExists(themeCat))
                        //        continue;

                        //    int catIndex = 0;
                        //    XmlNodeList typeStyle = scaleRange.GetElementsByTagName(typeStyles[geomType]);
                        //    for (int st = 0; st < typeStyle.Count; st++)
                        //    {
                        //        // We will check if this typestyle is going to be shown in the legend
                        //        XmlNodeList showInLegend = ((XmlElement)typeStyle[st]).GetElementsByTagName("ShowInLegend");
                        //        if (showInLegend.Count > 0)
                        //            if (!bool.Parse(showInLegend[0].ChildNodes[0].Value))
                        //                continue;   // This typestyle does not need to be shown in the legend

                        //        XmlNodeList rules = ((XmlElement)typeStyle[st]).GetElementsByTagName(ruleNames[geomType]);
                        //        if (rules.Count > 1)
                        //        {
                        //            layerMeta.SetDefaultIcon(themeCat, Properties.Resources.lc_theme);
                        //            node.ToolTipText = string.Format(Properties.Resources.ThemedLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName, rules.Count);

                        //            if (_legend.ThemeCompressionLimit > 0 && rules.Count > _legend.ThemeCompressionLimit)
                        //            {
                        //                AddThemeRuleNode(layerMeta, themeCat, node, geomType, 0, rules, 0);
                        //                node.Nodes.Add(CreateCompressedThemeNode(layerMeta, themeCat, rules.Count - 2));
                        //                AddThemeRuleNode(layerMeta, themeCat, node, geomType, rules.Count - 1, rules, rules.Count - 1);
                        //            }
                        //            else
                        //            {
                        //                for (int r = 0; r < rules.Count; r++)
                        //                {
                        //                    AddThemeRuleNode(layerMeta, themeCat, node, geomType, catIndex++, rules, r);
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (LayerNodeMetadata.ScaleIsApplicable(_map.ViewScale, themeCat))
                        //            {
                        //                if (!layerMeta.HasDefaultIconsAt(_map.ViewScale))
                        //                {
                        //                    try
                        //                    {
                        //                        var img = _map.GetLegendImage(layer.LayerDefinitionID,
                        //                                                                _map.ViewScale,
                        //                                                                16,
                        //                                                                16,
                        //                                                                "PNG",
                        //                                                                -1,
                        //                                                                -1);
                        //                        legendCallCount++;
                        //                        layerMeta.SetDefaultIcon(themeCat, img);
                        //                        node.ToolTipText = string.Format(Properties.Resources.DefaultLayerTooltip, Environment.NewLine, layer.Name, layer.FeatureSourceID, layer.QualifiedClassName);
                        //                    }
                        //                    catch
                        //                    {
                        //                        //layerMeta.SetDefaultIcon(themeCat, Properties.Resources.lc_broken);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                else //Already cached
                {
                    Trace.TraceInformation("Icons already cached for: " + layer.Name);
                    node.Nodes.AddRange(layerMeta.CreateThemeNodesFromCachedMetadata(_map.ViewScale));
                }
            }

            return node;
        }

        private void AddThemeRuleNode(LayerNodeMetadata layerMeta, ThemeCategory themeCat, TreeNode node, int geomType, int catIndex, XmlNodeList rules, int ruleOffset)
        {
            XmlElement rule = (XmlElement)rules[ruleOffset];
            XmlNodeList label = rule.GetElementsByTagName("LegendLabel");
            XmlNodeList filter = rule.GetElementsByTagName("Filter");

            String labelText = "";
            if (label != null && label.Count > 0 && label[0].ChildNodes.Count > 0)
                labelText = label[0].ChildNodes[0].Value;
            //String filterText = "";
            //if (filter != null && filter.Count > 0 && filter[0].ChildNodes.Count > 0)
            //    filterText = filter[0].ChildNodes[0].Value;

            if (LayerNodeMetadata.ScaleIsApplicable(_map.ViewScale, themeCat))
            {
                var child = CreateThemeRuleNode(layerMeta, themeCat, _map.ViewScale, labelText, (geomType + 1), catIndex);
                node.Nodes.Add(child);
            }
            else
            {

            }
        }

        private TreeNode CreateCompressedThemeNode(LayerNodeMetadata layer, ThemeCategory cat, int count)
        {
            TreeNode node = new TreeNode();
            node.Text = (count + " other styles");
            node.ImageKey = node.SelectedImageKey = IMG_OTHER;
            var meta = new LayerThemeNodeMetadata(true, Properties.Resources.icon_etc, node.Text);
            node.Tag = meta;
            layer.AddThemeNode(cat, meta);
            return node;
        }

        private TreeNode CreateThemeRuleNode(LayerNodeMetadata layer, ThemeCategory themeCat, double viewScale, string labelText, int geomType, int categoryIndex)
        {
            var lyr = layer.Layer;
            TreeNode node = new TreeNode();
            node.Text = labelText;

            Image icon = _map.GetLegendImage(lyr.LayerDefinitionID,
                                             viewScale,
                                             16,
                                             16,
                                             "PNG",
                                             geomType,
                                             categoryIndex);
            legendCallCount++;

            var tag = new LayerThemeNodeMetadata(false, icon, labelText);
            layer.AddThemeNode(themeCat, tag);
            node.Tag = tag;

            return node;
        }

        private TreeNode CreateGroupNode(RuntimeMapGroup group)
        {
            var node = new TreeNode();
            node.Name = group.ObjectId;
            node.Text = group.LegendLabel;
            node.Checked = group.Visible;
            node.SelectedImageKey = node.ImageKey = IMG_GROUP;
            var meta = new GroupNodeMetadata(group);
            node.Tag = meta;
            _groups[group.ObjectId] = meta;
            //node.ContextMenuStrip = _legend.GroupContextMenu;
            return node;
        }

        private int legendCallCount = 0;

        public TreeNode[] CreateNodes()
        {
            List<TreeNode> topLevelNodes = new List<TreeNode>();
            var scale = _map.ViewScale;
            if (scale < 10.0)
                return topLevelNodes.ToArray();

            var nodesById = new Dictionary<string, TreeNode>();
            var groupsById = new Dictionary<string, TreeNode>();
            var groups = _map.Groups;
            var layers = _map.Layers;

            legendCallCount = 0;

            //Process groups first
            List<RuntimeMapGroup> remainingNodes = new List<RuntimeMapGroup>();
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                if (!_legend.ShowAllLayersAndGroups && !group.ShowInLegend)
                    continue;

                //Add ones without parents first. Queue up child groups
                if (!string.IsNullOrEmpty(group.Group))
                {
                    remainingNodes.Add(group);
                }
                else
                {
                    var node = CreateGroupNode(group);
                    topLevelNodes.Add(node);
                    nodesById.Add(group.ObjectId, node);
                    groupsById.Add(group.ObjectId, node);
                }
            }

            //Process child groups
            while (remainingNodes.Count > 0)
            {
                List<RuntimeMapGroup> toRemove = new List<RuntimeMapGroup>();
                //Establish parent-child relationships for any child groups here
                for (int j = 0; j < remainingNodes.Count; j++)
                {
                    var grpName = remainingNodes[j].Group;
                    var parentGroup = _map.Groups[grpName];
                    if (parentGroup != null && nodesById.ContainsKey(parentGroup.ObjectId))
                    {
                        var node = CreateGroupNode(remainingNodes[j]);
                        nodesById[parentGroup.ObjectId].Nodes.Add(node);
                        var grpId = remainingNodes[j].ObjectId;
                        //Got to add this group node too, otherwise we could infinite
                        //loop looking for a parent that's not registered
                        nodesById.Add(grpId, node);
                        groupsById.Add(grpId, node);

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

            //Collect all resource contents in a batch
            var layerIds = new List<string>();
            //Also collect the layer metadata nodes to create or update
            var layerMetaNodesToUpdate = new Dictionary<string, RuntimeMapLayer>();
            //Now process layers. Layers without metadata nodes or without layer definition content
            //are added to the list
            for (int i = 0; i < layers.Count; i++)
            {
                var lyr = layers[i];
                bool display = lyr.ShowInLegend;
                bool visible = lyr.IsVisibleAtScale(_map.ViewScale);
                if (!_legend.ShowAllLayersAndGroups && !display)
                    continue;

                if (!_legend.ShowAllLayersAndGroups && !visible)
                    continue;

                if (_layers.ContainsKey(lyr.ObjectId))
                {
                    if (string.IsNullOrEmpty(_layers[lyr.ObjectId].LayerDefinitionContent))
                    {
                        var ldfId = lyr.LayerDefinitionID;
                        layerIds.Add(ldfId.ToString());
                        layerMetaNodesToUpdate[ldfId.ToString()] = lyr;
                    }
                }
                else
                {
                    var ldfId = lyr.LayerDefinitionID;
                    layerIds.Add(ldfId.ToString());
                    layerMetaNodesToUpdate[ldfId.ToString()] = lyr;
                }
            }

            if (layerIds.Count > 0)
            {
                int added = 0;
                int updated = 0;
                //Fetch the contents and create/update the required layer metadata nodes
                //TODO: Surely we can optimize this better
                foreach (var lid in layerIds)
                {
                    using (var sr = new StreamReader(_resSvc.GetResourceXmlData(lid)))
                    {
                        string content = sr.ReadToEnd();

                        var lyr = layerMetaNodesToUpdate[lid];
                        var objId = lyr.ObjectId;
                        LayerNodeMetadata meta = null;
                        if (_layers.ContainsKey(objId))
                        {
                            meta = _layers[objId];
                            updated++;
                        }
                        else
                        {
                            RuntimeMapGroup group = null;
                            if (!string.IsNullOrEmpty(lyr.Group))
                                group = _map.Groups[lyr.Group];
                            meta = new LayerNodeMetadata(lyr, lyr.Selectable);
                            _layers[objId] = meta;
                            added++;
                        }
                        meta.LayerDefinitionContent = content;
                    }
                }
                Trace.TraceInformation("CreateNodes: {0} layer contents added, {1} layer contents updated", added, updated);
            }

            //Now create our layer nodes
            List<RuntimeMapLayer> remainingLayers = new List<RuntimeMapLayer>();
            //NOTE: We're taking a page out of the Fusion playbook of reverse iterating the layer
            //collection and prepending the nodes, as this control suffered the same problem as the
            //Legend widget in Fusion. Doing it this way eliminates the need for doing an extra pass to fix
            //the layer/group ordering, which may make an impact on really chunky maps.
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                var layer = layers[i];

                bool display = layer.ShowInLegend;
                bool visible = layer.IsVisibleAtScale(_map.ViewScale);
                if (!_legend.ShowAllLayersAndGroups && !display)
                    continue;

                if (!_legend.ShowAllLayersAndGroups && !visible)
                    continue;

                //Add ones without parents first. Queue up parented layers
                if (!string.IsNullOrEmpty(layer.Group))
                {
                    remainingLayers.Add(layer);
                }
                else
                {
                    var node = CreateLayerNode(layer);
                    if (node != null)
                    {
                        topLevelNodes.Insert(0, node);
                        nodesById.Add(layer.ObjectId, node);
                        if (layer.ExpandInLegend)
                            node.Expand();
                    }
                }
            }

            //Process parented layers
            while (remainingLayers.Count > 0)
            {
                List<RuntimeMapLayer> toRemove = new List<RuntimeMapLayer>();
                for (int j = remainingLayers.Count - 1; j >= 0; j--)
                {
                    var grpName = remainingLayers[j].Group;
                    var parentGroup = _map.Groups[grpName];
                    if (parentGroup != null)
                    {
                        if (nodesById.ContainsKey(parentGroup.ObjectId))
                        {
                            var node = CreateLayerNode(remainingLayers[j]);
                            if (node != null)
                            {
                                nodesById[parentGroup.ObjectId].Nodes.Add(node); //.Insert(0, node);
                                if (remainingLayers[j].ExpandInLegend)
                                    node.Expand();
                            }
                            toRemove.Add(remainingLayers[j]);
                        }
                        else //Group may not even be set to show in legend. Check for this
                        {
                            if (!parentGroup.ShowInLegend)
                                toRemove.Add(remainingLayers[j]);
                        }
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

            //Now expand any relevant groups
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                if (group.ExpandInLegend)
                {
                    var groupId = group.ObjectId;
                    if (nodesById.ContainsKey(groupId))
                    {
                        nodesById[groupId].Expand();
                    }
                }
            }
            Trace.TraceInformation("{0} calls made to GenerateLegendImage", legendCallCount);
            return topLevelNodes.ToArray();
        }

        private static bool IsThemeLayerNode(TreeNode node)
        {
            var meta = node.Tag as LayerThemeNodeMetadata;
            if (meta != null)
                return true;

            return false;
        }

        private static bool IsLayerNode(TreeNode node)
        {
            var meta = node.Tag as LayerNodeMetadata;
            return meta != null;
        }

        internal void DrawNode(DrawTreeNodeEventArgs e, bool showPlusMinus, Font font)
        {
            var currentScale = _map.ViewScale;
            var layerMeta = e.Node.Tag as LayerNodeMetadata;
            var themeMeta = e.Node.Tag as LayerThemeNodeMetadata;
            if (!e.Bounds.IsEmpty && (layerMeta != null || themeMeta != null))
            {
                Color backColor, foreColor;

                //For some reason, the default bounds are way off from what you would
                //expect it to be. So we apply this offset for any text/image draw operations
                int xoffset = -36;
                if (showPlusMinus && e.Node.Nodes.Count > 0)
                {
                    // Use the VisualStyles renderer to use the proper OS-defined glyphs
                    Rectangle glyphRect = new Rectangle(e.Node.Bounds.X - 52, e.Node.Bounds.Y, 16, 16);
                    if (Application.RenderWithVisualStyles)
                    {
                        VisualStyleElement element = (e.Node.IsExpanded) ?
                            VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;

                        VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                        renderer.DrawBackground(e.Graphics, glyphRect);
                    }
                    else //Visual Styles disabled, fallback to drawing the +/- using geometric primitives
                    {
                        int h = 8;
                        int w = 8;
                        int x = glyphRect.X;
                        int y = glyphRect.Y + (glyphRect.Height / 2) - 4;

                        //Draw the -
                        e.Graphics.DrawRectangle(new Pen(SystemBrushes.ControlDark), x, y, w, h);
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), x + 1, y + 1, w - 1, h - 1);
                        e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Black)), x + 2, y + 4, x + w - 2, y + 4);

                        //Draw the |
                        if (!e.Node.IsExpanded)
                            e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Black)), x + 4, y + 2, x + 4, y + h - 2);
                    }
                }

                bool bDrawSelection = false;
                if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                {
                    backColor = SystemColors.Highlight;
                    foreColor = SystemColors.HighlightText;
                    bDrawSelection = true;
                }
                else if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
                {
                    backColor = SystemColors.HotTrack;
                    foreColor = SystemColors.HighlightText;
                }
                else
                {
                    backColor = e.Node.BackColor;
                    foreColor = (layerMeta != null && !layerMeta.Layer.IsVisibleAtScale(_map.ViewScale)) ? SystemColors.InactiveCaptionText : Color.Black;
                }

                var checkBoxOffset = xoffset;
                var selectabilityOffset = xoffset + 16;
                var iconOffsetNoSelect = xoffset + 16;
                if (layerMeta != null && !layerMeta.Checkable) //No checkbox for theme rule nodes
                {
                    selectabilityOffset = xoffset;
                    iconOffsetNoSelect = xoffset;
                }
                var iconOffset = selectabilityOffset + 20;
                var textOffset = iconOffset + 20;
                var textOffsetNoSelect = iconOffsetNoSelect + 20;

                //Uncomment if you need to "see" the bounds of the node
                //e.Graphics.DrawRectangle(Pens.Black, e.Node.Bounds);

                if (layerMeta != null && layerMeta.Checkable) //No checkbox for theme rule nodes
                {
                    if (Application.RenderWithVisualStyles)
                    {
                        CheckBoxRenderer.DrawCheckBox(
                            e.Graphics,
                            new Point(e.Node.Bounds.X + checkBoxOffset, e.Node.Bounds.Y),
                            e.Node.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
                    }
                    else
                    {
                        //We don't have to do this, but with Visual Styles disabled, there is a noticeable jarring visual difference from DrawDefault'd checkboxes
                        //So we might as well emulate that style for the sake of consistency
                        var rect = new Rectangle(e.Node.Bounds.X + checkBoxOffset, e.Node.Bounds.Y, 16, 16);
                        ControlPaint.DrawCheckBox(e.Graphics, rect, e.Node.Checked ? ButtonState.Checked | ButtonState.Flat : ButtonState.Flat);
                        rect.Inflate(-2, -2);
                        e.Graphics.DrawRectangle(new Pen(Brushes.Black, 2.0f), rect);
                    }
                }
               
                if (layerMeta != null)
                {
                    if (bDrawSelection)
                    {
                        var size = e.Graphics.MeasureString(e.Node.Text, font);
                        using (var brush = new SolidBrush(backColor))
                        {
                            e.Graphics.FillRectangle(brush,
                                                     e.Node.Bounds.X + (layerMeta.DrawSelectabilityIcon ? textOffset : textOffsetNoSelect),
                                                     e.Node.Bounds.Y,
                                                     size.Width,
                                                     size.Height);
                        }
                    }

                    if (layerMeta.DrawSelectabilityIcon)
                    {
                        var icon = layerMeta.IsSelectable ? _selectableIcon : _unselectableIcon;
                        e.Graphics.DrawImage(icon, e.Node.Bounds.X + selectabilityOffset, e.Node.Bounds.Y);
                        //Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                    }

                    Image layerIcon = null;
                    if (layerMeta.IsRaster)
                    {
                        layerIcon = Properties.Resources.lc_raster;
                    }
                    else if (layerMeta.IsDwf)
                    {
                        layerIcon = Properties.Resources.lc_dwf;
                    }
                    else
                    {
                        layerIcon = layerMeta.GetDefaultIcon(currentScale);
                        if (layerIcon == null &&_legend.ShowAllLayersAndGroups)
                            layerIcon = Properties.Resources.lc_broken;
                    }
                    if (layerIcon != null)
                    {
                        if (layerMeta.DrawSelectabilityIcon)
                        {
                            e.Graphics.DrawImage(layerIcon, e.Node.Bounds.X + iconOffset, e.Node.Bounds.Y);
                            //Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                        }
                        else
                        {
                            e.Graphics.DrawImage(layerIcon, e.Node.Bounds.X + iconOffsetNoSelect, e.Node.Bounds.Y);
                            //Trace.TraceInformation("Painted icon at ({0},{1})", e.Node.Bounds.X, e.Node.Bounds.Y);
                        }
                    }

                    using (SolidBrush brush = new SolidBrush(foreColor))
                    {
                        e.Graphics.DrawString(e.Node.Text, font, brush, e.Node.Bounds.X + (layerMeta.DrawSelectabilityIcon ? textOffset : textOffsetNoSelect), e.Node.Bounds.Y);
                    }
                }
                else if (themeMeta != null)
                {
                    if (bDrawSelection)
                    {
                        var size = e.Graphics.MeasureString(e.Node.Text, font);
                        using (var brush = new SolidBrush(backColor))
                        {
                            e.Graphics.FillRectangle(brush,
                                                     e.Node.Bounds.X + textOffsetNoSelect,
                                                     e.Node.Bounds.Y,
                                                     size.Width,
                                                     size.Height);
                        }
                    }

                    if (themeMeta.ThemeIcon != null)
                    {
                        e.Graphics.DrawImage(themeMeta.ThemeIcon, e.Node.Bounds.X + iconOffsetNoSelect, e.Node.Bounds.Y);
                    }

                    using (SolidBrush brush = new SolidBrush(foreColor))
                    {
                        e.Graphics.DrawString(e.Node.Text, font, brush, e.Node.Bounds.X + textOffsetNoSelect, e.Node.Bounds.Y);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(foreColor))
                    {
                        e.Graphics.DrawString(e.Node.Text, font, brush, e.Node.Bounds.X + 17.0f + xoffset, e.Node.Bounds.Y);
                    }
                }

                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        internal void SetGroupExpandInLegend(string objectId, bool expand)
        {
            if (_groups.ContainsKey(objectId))
            {
                var grp = _groups[objectId].Group;
                grp.ExpandInLegend = expand;
                _map.Save();
            }
        }

        internal void SetLayerExpandInLegend(string objectId, bool expand)
        {
            if (_layers.ContainsKey(objectId))
            {
                var lyr = _layers[objectId].Layer;
                lyr.ExpandInLegend = expand;
                _map.Save();
            }
        }

        internal void SetGroupVisible(string objectId, bool bChecked)
        {
            if (_groups.ContainsKey(objectId))
            {
                var grp = _groups[objectId].Group;
                grp.Visible = bChecked;
                var bVis = HasVisibleParent(grp);
                if (bVis)
                    _legend.OnRequestRefresh();
            }
        }

        internal void SetLayerVisible(string objectId, bool bChecked)
        {
            if (_layers.ContainsKey(objectId))
            {
                var layer = _layers[objectId].Layer;
                layer.Visible = bChecked;
                var bVis = HasVisibleParent(layer);
                if (bVis)
                {
                    layer.ForceRefresh();
                    _legend.OnRequestRefresh();
                }
            }
        }
    }

    namespace Model
    {
        /// <summary>
        /// Metadata about a node in the legend
        /// </summary>
        public abstract class LegendNodeMetadata
        {
            /// <summary>
            /// Gets whether the node is a group
            /// </summary>
            public bool IsGroup { get; protected set; }

            /// <summary>
            /// Gets the object id
            /// </summary>
            public abstract string ObjectId { get; }

            /// <summary>
            /// Gets whether the node is checkable
            /// </summary>
            public bool Checkable { get; protected set; }
        }

        /// <summary>
        /// Group metadata
        /// </summary>
        [DebuggerDisplay("Name = {GroupName}, Label = {LegendLabel}")]
        public class GroupNodeMetadata : LegendNodeMetadata
        {
            internal RuntimeMapGroup Group { get; private set; }

            /// <summary>
            /// Gets the associated group
            /// </summary>
            /// <returns></returns>
            public RuntimeMapGroup GetGroup() { return this.Group; }

            internal GroupNodeMetadata(RuntimeMapGroup group)
            {
                base.IsGroup = true;
                this.Group = group;
                this.Checkable = true;
            }

            /// <summary>
            /// Gets the legend label
            /// </summary>
            public string LegendLabel { get { return this.Group.LegendLabel; } }

            /// <summary>
            /// Gets the name of the parent group
            /// </summary>
            public string ParentGroupName { get { return this.Group.Group; } }

            /// <summary>
            /// Gets the name of this group
            /// </summary>
            public string Name { get { return this.Group.Name; } }

            /// <summary>
            /// Gets the unique id of this group
            /// </summary>
            public override string ObjectId
            {
                get { return this.Group.ObjectId; }
            }
        }
        
        /// <summary>
        /// Layer theme node metadata
        /// </summary>
        [DebuggerDisplay("Layer Theme Node")]
        public class LayerThemeNodeMetadata : LegendNodeMetadata
        {
            internal LayerThemeNodeMetadata(bool bPlaceholder, Image themeIcon, string labelText)
            {
                this.IsPlaceholder = bPlaceholder;
                this.ThemeIcon = themeIcon;
                this.Label = labelText;
                this.Checkable = false;
            }

            /// <summary>
            /// Gets whether this node is a placeholder
            /// </summary>
            public bool IsPlaceholder { get; private set; }
            
            /// <summary>
            /// Gets the icon for this theme node
            /// </summary>
            public Image ThemeIcon { get; set; }

            /// <summary>
            /// Gets the label for this theme node
            /// </summary>
            public string Label { get; private set; }

            /// <summary>
            /// Gets the unique id for this node
            /// </summary>
            public override string ObjectId
            {
                get { return string.Empty; }
            }
        }

        /// <summary>
        /// Represents a theme category
        /// </summary>
        public class ThemeCategory
        {
            /// <summary>
            /// Gets the lower bound scale
            /// </summary>
            public string MinScale { get; set; }

            /// <summary>
            /// Gets the upper bound scale
            /// </summary>
            public string MaxScale { get; set; }

            /// <summary>
            /// Gets the geometry type this category is applicable to
            /// </summary>
            public int GeometryType { get; set; }

            /// <summary>
            /// Gets the hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    // Suitable nullity checks etc, of course :)
                    if (MinScale != null)
                        hash = hash * 23 + MinScale.GetHashCode();
                    if (MaxScale != null)
                        hash = hash * 23 + MaxScale.GetHashCode();
                    hash = hash * 23 + GeometryType.GetHashCode();
                    return hash;
                }
            }

            /// <summary>
            /// Checks whether this instance is equal to the specified instance
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                var cat = obj as ThemeCategory;
                if (cat == null)
                    return false;

                return this.MaxScale == cat.MaxScale &&
                       this.MinScale == cat.MinScale &&
                       this.GeometryType == cat.GeometryType;
            }
        }

        /// <summary>
        /// Information about a layer node
        /// </summary>
        [DebuggerDisplay("Name = {Layer.Name}, Label = {Layer.LegendLabel}")]
        public class LayerNodeMetadata : LegendNodeMetadata
        {
            /// <summary>
            /// Initializes a new instance of the LayerNodeMetadata class
            /// </summary>
            /// <param name="layer"></param>
            public LayerNodeMetadata(RuntimeMapLayer layer)
                : this(layer, layer.Selectable)
            {
            }

            internal LayerNodeMetadata(RuntimeMapLayer layer, bool bInitiallySelectable)
            {
                base.IsGroup = false;
                this.Layer = layer;
                this.IsSelectable = (layer != null) ? layer.Selectable : false;
                this.DrawSelectabilityIcon = (layer != null && bInitiallySelectable);
                this.WasInitiallySelectable = bInitiallySelectable;
                this.LayerDefinitionContent = null;
                this.Checkable = (layer.Type != RuntimeMapLayer.kBaseMap);
                _themeNodes = new Dictionary<ThemeCategory, List<LayerThemeNodeMetadata>>();
                _defaultIcons = new Dictionary<ThemeCategory, Image>();
            }

            /// <summary>
            /// Gets the parent group name
            /// </summary>
            public string ParentGroupName { get { return this.Layer.Group; } }

            /// <summary>
            /// Gets the name
            /// </summary>
            public string Name { get { return this.Layer.Name; } }

            /// <summary>
            /// Gets the object id
            /// </summary>
            public override string ObjectId
            {
                get { return this.Layer.ObjectId; }
            }

            private bool? _isRaster;

            /// <summary>
            /// Gets whether this layer is a raster layer
            /// </summary>
            public bool IsRaster
            {
                get
                {
                    if (_isRaster.HasValue)
                        return _isRaster.Value;

                    if (!string.IsNullOrEmpty(this.LayerDefinitionContent))
                        _isRaster = this.LayerDefinitionContent.Contains("<GridLayerDefinition");

                    if (_isRaster.HasValue)
                        return _isRaster.Value;

                    throw new Exception("Layer metadata not fully initialized"); //Shouldn't get to here
                }
            }

            private bool? _isDwf;

            /// <summary>
            /// Gets whether this layer is a DWF-based drawing layer
            /// </summary>
            public bool IsDwf
            {
                get
                {
                    if (_isDwf.HasValue)
                        return _isRaster.Value;

                    if (!string.IsNullOrEmpty(this.LayerDefinitionContent))
                        _isDwf = this.LayerDefinitionContent.Contains("<DrawingLayerDefinition");

                    if (_isDwf.HasValue)
                        return _isRaster.Value;

                    throw new Exception("Layer metadata not fully initialized"); //Shouldn't get to here
                }
            }

            private Dictionary<ThemeCategory, Image> _defaultIcons;

            /// <summary>
            /// Sets the default icon of the given theme category
            /// </summary>
            /// <param name="cat"></param>
            /// <param name="image"></param>
            public void SetDefaultIcon(ThemeCategory cat, Image image)
            {
                _defaultIcons[cat] = image;
            }

            /// <summary>
            /// Gets the default icon for the specified scale
            /// </summary>
            /// <param name="scale"></param>
            /// <returns></returns>
            public Image GetDefaultIcon(double scale)
            {
                foreach (var cat in _defaultIcons.Keys)
                {
                    if (ScaleIsApplicable(scale, cat))
                        return _defaultIcons[cat];
                }
                return null;
            }

            //public Image Icon { get; set; }

            internal RuntimeMapLayer Layer { get; private set; }

            internal bool DrawSelectabilityIcon { get; set; }

            /// <summary>
            /// Gets or sets whether the layer is selectable
            /// </summary>
            public bool IsSelectable { get; set; }

            internal bool WasInitiallySelectable { get; private set; }

            /// <summary>
            /// Gets or sets whether the layer is a base (tiled) layer
            /// </summary>
            public bool IsBaseLayer { get; set; }

            /// <summary>
            /// Gets or sets the Layer Definition XML content
            /// </summary>
            public string LayerDefinitionContent { get; set; }

            private Dictionary<ThemeCategory, List<LayerThemeNodeMetadata>> _themeNodes;

            /// <summary>
            /// Gets the theme nodes for this layer
            /// </summary>
            /// <param name="category"></param>
            /// <returns></returns>
            public List<LayerThemeNodeMetadata> GetThemeNodes(ThemeCategory category)
            {
                if (_themeNodes.ContainsKey(category))
                    return _themeNodes[category];
                return null;
            }

            /// <summary>
            /// Adds a theme node
            /// </summary>
            /// <param name="category"></param>
            /// <param name="themeMeta"></param>
            public void AddThemeNode(ThemeCategory category, LayerThemeNodeMetadata themeMeta)
            {
                if (!_themeNodes.ContainsKey(category))
                    _themeNodes[category] = new List<LayerThemeNodeMetadata>();

                _themeNodes[category].Add(themeMeta);
            }

            internal bool HasDefaultIcons()
            {
                return (_defaultIcons.Count > 0);
            }

            internal bool HasTheme()
            {
                if (_themeNodes.Count == 0)
                    return false;

                foreach (var coll in _themeNodes.Values)
                    if (coll.Count > 0)
                        return true;

                return false;
            }

            internal TreeNode[] CreateThemeNodesFromCachedMetadata(double scale)
            {
                var nodes = new List<TreeNode>();

                //Find the applicable scale range(s)
                foreach (var cat in _themeNodes.Keys)
                {
                    bool bApplicable = ScaleIsApplicable(scale, cat);

                    if (bApplicable)
                    {
                        var metadata = _themeNodes[cat];
                        nodes.AddRange(CreateThemeNodes(metadata));
                    }
                }

                return nodes.ToArray();
            }

            internal static bool ScaleIsApplicable(double scale, ThemeCategory cat)
            {
                bool bApplicable = false;

                bool bHasMin = !string.IsNullOrEmpty(cat.MinScale);
                bool bHasMax = !string.IsNullOrEmpty(cat.MaxScale);

                if (bHasMin)
                {
                    double minVal = double.Parse(cat.MinScale);
                    if (bHasMax) //bHasMin = true, bHasMax = true
                    {
                        double maxVal = double.Parse(cat.MaxScale);
                        if (scale >= minVal && scale < maxVal)
                            bApplicable = true;
                    }
                    else         //bHasMin = true, bHasMax = false
                    {
                        if (scale >= minVal)
                            bApplicable = true;
                    }
                }
                else
                {
                    if (bHasMax) //bHasMin = false, bHasMax = true
                    {
                        double maxVal = double.Parse(cat.MaxScale);
                        if (scale < maxVal)
                            bApplicable = true;
                    }
                    else         //bHasMin = false, bHasMax = false
                    {
                        bApplicable = true;
                    }
                }
                return bApplicable;
            }

            private IEnumerable<TreeNode> CreateThemeNodes(List<LayerThemeNodeMetadata> metadata)
            {
                foreach (var meta in metadata)
                {
                    var node = new TreeNode();
                    node.Text = meta.Label;
                    node.Tag = meta;
                    yield return node;
                }
            }

            internal bool HasDefaultIconsAt(double scale)
            {
                foreach (var cat in _defaultIcons.Keys)
                {
                    if (ScaleIsApplicable(scale, cat))
                        return true;
                }
                return false;
            }

            internal bool CategoryExists(ThemeCategory themeCat)
            {
                return _themeNodes.ContainsKey(themeCat);
            }
        }
    }
}
