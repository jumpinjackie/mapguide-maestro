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
using Maestro.MapViewer.Model;

namespace Maestro.MapViewer
{
    public delegate void NodeEventHandler(object sender, TreeNode node);

    public partial class Legend : UserControl, INotifyPropertyChanged
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
            this.ShowAllLayersAndGroups = false;
        }

        public event ItemDragEventHandler ItemDrag;

        /// <summary>
        /// Gets whether to display all layers and groups regardless of display settings
        /// and visibility
        /// </summary>
        public bool ShowAllLayersAndGroups { get; set; }

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
            _presenter = new LegendControlPresenter(this, _map);
        }

        void OnMapRefreshed(object sender, EventArgs e)
        {
            this.RefreshLegend();
        }

        private Dictionary<string, RuntimeMapLayer> _layers = new Dictionary<string, RuntimeMapLayer>();
        private Dictionary<string, RuntimeMapGroup> _groups = new Dictionary<string, RuntimeMapGroup>();
        private Dictionary<string, string> _layerDefinitionContents = new Dictionary<string, string>();

        internal bool GetVisibilityFlag(RuntimeMapGroup group)
        {
            return this.ShowAllLayersAndGroups;
        }

        internal bool GetVisibilityFlag(RuntimeMapLayer layer)
        {
            return layer.IsVisibleAtScale(_map.ViewScale);
        }

        private LegendControlPresenter _presenter;

        /// <summary>
        /// Refreshes this component
        /// </summary>
        public void RefreshLegend()
        {
            if (_noUpdate)
                return;

            if (_presenter == null)
                return;

            if (IsBusy)
                return;

            ResetTreeView();
            trvLegend.BeginUpdate();
            _legendUpdateStopwatch.Start();
            this.IsBusy = true;
            bgLegendUpdate.RunWorkerAsync();
        }

        private Stopwatch _legendUpdateStopwatch = new Stopwatch();

        private bool _busy = false;

        [Browsable(false)]
        public bool IsBusy
        {
            get { return _busy; }
            private set
            {
                if (_busy.Equals(value))
                    return;

                _busy = value;
                Trace.TraceInformation("Legend IsBusy: {0}", this.IsBusy);
                OnPropertyChanged("IsBusy");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        private void bgLegendUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _presenter.CreateNodes();
        }

        private void bgLegendUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.IsBusy = bgLegendUpdate.IsBusy;
            var nodes = e.Result as TreeNode[];
            if (nodes != null)
            {
                //Attach relevant context menus based on attached metadata
                AttachContextMenus(nodes);
                trvLegend.Nodes.AddRange(nodes);
            }
            trvLegend.EndUpdate();
            _legendUpdateStopwatch.Stop();
            Trace.TraceInformation("RefreshLegend: Completed in {0}ms", _legendUpdateStopwatch.ElapsedMilliseconds);
            _legendUpdateStopwatch.Reset();
        }

        private void AttachContextMenus(IEnumerable<TreeNode> nodes)
        {
            foreach (var n in nodes)
            {
                var lm = n.Tag as LegendNodeMetadata;
                if (lm != null)
                {
                    if (lm.IsGroup)
                    {
                        n.ContextMenuStrip = this.GroupContextMenu;
                    }
                    else
                    {
                        var lyrm = n.Tag as LayerNodeMetadata;
                        if (lyrm != null)
                            n.ContextMenuStrip = this.LayerContextMenu;
                    }
                }
                if (n.Nodes.Count > 0)
                    AttachContextMenus(AsEnumerable(n.Nodes));
            }
        }

        static IEnumerable<TreeNode> AsEnumerable(TreeNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                yield return nodes[i];
            }
        }

        public TreeNode SelectedNode { get { return trvLegend.SelectedNode; } }

        private static void ClearNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                    ClearNodes(node.Nodes);
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

        private bool HasVisibleParent(RuntimeMapGroup grp)
        {
            if (string.IsNullOrEmpty(grp.Group))
                return true;

            var current = _map.Groups[grp.Group];
            if (current != null)
                return current.Visible;
            return true;
        }

        private bool HasVisibleParent(RuntimeMapLayer layer)
        {
            if (string.IsNullOrEmpty(layer.Group))
                return true;

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

        internal void OnRequestRefresh()
        {
            if (this.Viewer != null)
                this.Viewer.RefreshMap();
        }

        private void trvLegend_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            _presenter.DrawNode(e, trvLegend.ShowPlusMinus, trvLegend.Font);
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
            trvLegend.SelectedNode = e.Node;
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

            var grpMeta = trvLegend.SelectedNode.Tag as GroupNodeMetadata;
            if (grpMeta != null)
            {
                return grpMeta.Group;
            }

            return null;
        }

        public bool ShowTooltips
        {
            get { return trvLegend.ShowNodeToolTips; }
            set { trvLegend.ShowNodeToolTips = value; }
        }

        public event NodeEventHandler NodeSelected;

        private void trvLegend_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var h = this.NodeSelected;
            if (h != null)
                h(this, e.Node);
        }

        public bool SelectOnRightClick { get; set; }

        private void trvLegend_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.SelectOnRightClick)
            {
                trvLegend.SelectedNode = trvLegend.GetNodeAt(e.X, e.Y);
            }
        }

        public TreeNode GetNodeAt(int x, int y)
        {
            return trvLegend.GetNodeAt(x, y);
        }

        private void trvLegend_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var h = this.ItemDrag;
            if (h != null)
                h(this, e);
        }
    }
}
