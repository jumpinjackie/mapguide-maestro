namespace Maestro.Editors.MapDefinition
{
    partial class MapLayersSectionCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapLayersSectionCtrl));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TAB_GROUP = new System.Windows.Forms.TabPage();
            this.trvLayersGroup = new Aga.Controls.Tree.TreeViewAdv();
            this.NODE_GROUP_ICON = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.NODE_GROUP_TEXT = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddGroup = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGRPAddLayer = new System.Windows.Forms.ToolStripButton();
            this.btnGRPRemoveLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveLayerOrGroupUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveLayerOrGroupDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnConvertLayerGroupToBaseGroup = new System.Windows.Forms.ToolStripButton();
            this.TAB_DRAWING_ORDER = new System.Windows.Forms.TabPage();
            this.trvLayerDrawingOrder = new Aga.Controls.Tree.TreeViewAdv();
            this.NODE_DRAW_ICON = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.NODE_DRAW_TEXT = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnDLAddLayer = new System.Windows.Forms.ToolStripButton();
            this.btnDLRemoveLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDLMoveLayerUp = new System.Windows.Forms.ToolStripButton();
            this.btnDLMoveLayerDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDLMoveLayerTop = new System.Windows.Forms.ToolStripButton();
            this.btnDLMoveLayerBottom = new System.Windows.Forms.ToolStripButton();
            this.TAB_BASE_LAYERS = new System.Windows.Forms.TabPage();
            this.trvBaseLayers = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnNewBaseLayerGroup = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveBaseLayerGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddBaseLayer = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveBaseLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveBaseLayerUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveBaseLayerDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBaseLayerGroupToRegular = new System.Windows.Forms.ToolStripButton();
            this.btnInvokeMgCooker = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TAB_GROUP.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.TAB_DRAWING_ORDER.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.TAB_BASE_LAYERS.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.propertiesPanel);
            this.contentPanel.Controls.Add(this.splitter1);
            this.contentPanel.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TAB_GROUP);
            this.tabControl1.Controls.Add(this.TAB_DRAWING_ORDER);
            this.tabControl1.Controls.Add(this.TAB_BASE_LAYERS);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // TAB_GROUP
            // 
            this.TAB_GROUP.Controls.Add(this.trvLayersGroup);
            this.TAB_GROUP.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.TAB_GROUP, "TAB_GROUP");
            this.TAB_GROUP.Name = "TAB_GROUP";
            this.TAB_GROUP.UseVisualStyleBackColor = true;
            // 
            // trvLayersGroup
            // 
            this.trvLayersGroup.AllowDrop = true;
            this.trvLayersGroup.BackColor = System.Drawing.SystemColors.Window;
            this.trvLayersGroup.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvLayersGroup.DefaultToolTipProvider = null;
            resources.ApplyResources(this.trvLayersGroup, "trvLayersGroup");
            this.trvLayersGroup.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvLayersGroup.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvLayersGroup.Model = null;
            this.trvLayersGroup.Name = "trvLayersGroup";
            this.trvLayersGroup.NodeControls.Add(this.NODE_GROUP_ICON);
            this.trvLayersGroup.NodeControls.Add(this.NODE_GROUP_TEXT);
            this.trvLayersGroup.SelectedNode = null;
            this.trvLayersGroup.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.trvLayersGroup.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvLayersGroup_ItemDrag);
            this.trvLayersGroup.SelectionChanged += new System.EventHandler(this.trvLayersGroup_SelectionChanged);
            this.trvLayersGroup.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvLayersGroup_DragDrop);
            this.trvLayersGroup.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvLayersGroup_DragEnter);
            this.trvLayersGroup.DragOver += new System.Windows.Forms.DragEventHandler(this.trvLayersGroup_DragOver);
            this.trvLayersGroup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvLayersGroup_MouseDoubleClick);
            // 
            // NODE_GROUP_ICON
            // 
            this.NODE_GROUP_ICON.DataPropertyName = "Icon";
            this.NODE_GROUP_ICON.LeftMargin = 1;
            this.NODE_GROUP_ICON.ParentColumn = null;
            this.NODE_GROUP_ICON.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // NODE_GROUP_TEXT
            // 
            this.NODE_GROUP_TEXT.DataPropertyName = "Text";
            this.NODE_GROUP_TEXT.IncrementalSearchEnabled = true;
            this.NODE_GROUP_TEXT.LeftMargin = 3;
            this.NODE_GROUP_TEXT.ParentColumn = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddGroup,
            this.btnRemoveGroup,
            this.toolStripSeparator1,
            this.btnGRPAddLayer,
            this.btnGRPRemoveLayer,
            this.toolStripSeparator3,
            this.btnMoveLayerOrGroupUp,
            this.btnMoveLayerOrGroupDown,
            this.toolStripSeparator7,
            this.btnConvertLayerGroupToBaseGroup});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddGroup.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            resources.ApplyResources(this.btnAddGroup, "btnAddGroup");
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveGroup, "btnRemoveGroup");
            this.btnRemoveGroup.Image = global::Maestro.Editors.Properties.Resources.folder__minus;
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Click += new System.EventHandler(this.btnRemoveGroup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnGRPAddLayer
            // 
            this.btnGRPAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGRPAddLayer.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            resources.ApplyResources(this.btnGRPAddLayer, "btnGRPAddLayer");
            this.btnGRPAddLayer.Name = "btnGRPAddLayer";
            this.btnGRPAddLayer.Click += new System.EventHandler(this.btnGRPAddLayer_Click);
            // 
            // btnGRPRemoveLayer
            // 
            this.btnGRPRemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnGRPRemoveLayer, "btnGRPRemoveLayer");
            this.btnGRPRemoveLayer.Image = global::Maestro.Editors.Properties.Resources.layer__minus;
            this.btnGRPRemoveLayer.Name = "btnGRPRemoveLayer";
            this.btnGRPRemoveLayer.Click += new System.EventHandler(this.btnGRPRemoveLayer_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // btnMoveLayerOrGroupUp
            // 
            this.btnMoveLayerOrGroupUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveLayerOrGroupUp, "btnMoveLayerOrGroupUp");
            this.btnMoveLayerOrGroupUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveLayerOrGroupUp.Name = "btnMoveLayerOrGroupUp";
            this.btnMoveLayerOrGroupUp.Click += new System.EventHandler(this.btnMoveLayerOrGroupUp_Click);
            // 
            // btnMoveLayerOrGroupDown
            // 
            this.btnMoveLayerOrGroupDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveLayerOrGroupDown, "btnMoveLayerOrGroupDown");
            this.btnMoveLayerOrGroupDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveLayerOrGroupDown.Name = "btnMoveLayerOrGroupDown";
            this.btnMoveLayerOrGroupDown.Click += new System.EventHandler(this.btnMoveLayerOrGroupDown_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // btnConvertLayerGroupToBaseGroup
            // 
            this.btnConvertLayerGroupToBaseGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnConvertLayerGroupToBaseGroup, "btnConvertLayerGroupToBaseGroup");
            this.btnConvertLayerGroupToBaseGroup.Image = global::Maestro.Editors.Properties.Resources.map__arrow;
            this.btnConvertLayerGroupToBaseGroup.Name = "btnConvertLayerGroupToBaseGroup";
            this.btnConvertLayerGroupToBaseGroup.Click += new System.EventHandler(this.btnConvertLayerGroupToBaseGroup_Click);
            // 
            // TAB_DRAWING_ORDER
            // 
            this.TAB_DRAWING_ORDER.Controls.Add(this.trvLayerDrawingOrder);
            this.TAB_DRAWING_ORDER.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.TAB_DRAWING_ORDER, "TAB_DRAWING_ORDER");
            this.TAB_DRAWING_ORDER.Name = "TAB_DRAWING_ORDER";
            this.TAB_DRAWING_ORDER.UseVisualStyleBackColor = true;
            // 
            // trvLayerDrawingOrder
            // 
            this.trvLayerDrawingOrder.AllowDrop = true;
            this.trvLayerDrawingOrder.BackColor = System.Drawing.SystemColors.Window;
            this.trvLayerDrawingOrder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvLayerDrawingOrder.DefaultToolTipProvider = null;
            resources.ApplyResources(this.trvLayerDrawingOrder, "trvLayerDrawingOrder");
            this.trvLayerDrawingOrder.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvLayerDrawingOrder.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvLayerDrawingOrder.Model = null;
            this.trvLayerDrawingOrder.Name = "trvLayerDrawingOrder";
            this.trvLayerDrawingOrder.NodeControls.Add(this.NODE_DRAW_ICON);
            this.trvLayerDrawingOrder.NodeControls.Add(this.NODE_DRAW_TEXT);
            this.trvLayerDrawingOrder.SelectedNode = null;
            this.trvLayerDrawingOrder.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.trvLayerDrawingOrder.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvLayerDrawingOrder_ItemDrag);
            this.trvLayerDrawingOrder.SelectionChanged += new System.EventHandler(this.trvLayerDrawingOrder_SelectionChanged);
            this.trvLayerDrawingOrder.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvLayerDrawingOrder_DragDrop);
            this.trvLayerDrawingOrder.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvLayerDrawingOrder_DragEnter);
            this.trvLayerDrawingOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.trvLayerDrawingOrder_DragOver);
            this.trvLayerDrawingOrder.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvLayerDrawingOrder_MouseDoubleClick);
            // 
            // NODE_DRAW_ICON
            // 
            this.NODE_DRAW_ICON.DataPropertyName = "Icon";
            this.NODE_DRAW_ICON.LeftMargin = 1;
            this.NODE_DRAW_ICON.ParentColumn = null;
            this.NODE_DRAW_ICON.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // NODE_DRAW_TEXT
            // 
            this.NODE_DRAW_TEXT.DataPropertyName = "Text";
            this.NODE_DRAW_TEXT.IncrementalSearchEnabled = true;
            this.NODE_DRAW_TEXT.LeftMargin = 3;
            this.NODE_DRAW_TEXT.ParentColumn = null;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDLAddLayer,
            this.btnDLRemoveLayer,
            this.toolStripSeparator5,
            this.btnDLMoveLayerUp,
            this.btnDLMoveLayerDown,
            this.toolStripSeparator6,
            this.btnDLMoveLayerTop,
            this.btnDLMoveLayerBottom});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnDLAddLayer
            // 
            this.btnDLAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDLAddLayer.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            resources.ApplyResources(this.btnDLAddLayer, "btnDLAddLayer");
            this.btnDLAddLayer.Name = "btnDLAddLayer";
            this.btnDLAddLayer.Click += new System.EventHandler(this.btnDLAddLayer_Click);
            // 
            // btnDLRemoveLayer
            // 
            this.btnDLRemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDLRemoveLayer, "btnDLRemoveLayer");
            this.btnDLRemoveLayer.Image = global::Maestro.Editors.Properties.Resources.layer__minus;
            this.btnDLRemoveLayer.Name = "btnDLRemoveLayer";
            this.btnDLRemoveLayer.Click += new System.EventHandler(this.btnDLRemoveLayer_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // btnDLMoveLayerUp
            // 
            this.btnDLMoveLayerUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDLMoveLayerUp, "btnDLMoveLayerUp");
            this.btnDLMoveLayerUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnDLMoveLayerUp.Name = "btnDLMoveLayerUp";
            this.btnDLMoveLayerUp.Click += new System.EventHandler(this.btnDLMoveLayerUp_Click);
            // 
            // btnDLMoveLayerDown
            // 
            this.btnDLMoveLayerDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDLMoveLayerDown, "btnDLMoveLayerDown");
            this.btnDLMoveLayerDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnDLMoveLayerDown.Name = "btnDLMoveLayerDown";
            this.btnDLMoveLayerDown.Click += new System.EventHandler(this.btnDLMoveLayerDown_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // btnDLMoveLayerTop
            // 
            this.btnDLMoveLayerTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDLMoveLayerTop, "btnDLMoveLayerTop");
            this.btnDLMoveLayerTop.Image = global::Maestro.Editors.Properties.Resources.layers_stack_arrange;
            this.btnDLMoveLayerTop.Name = "btnDLMoveLayerTop";
            this.btnDLMoveLayerTop.Click += new System.EventHandler(this.btnDLMoveLayerTop_Click);
            // 
            // btnDLMoveLayerBottom
            // 
            this.btnDLMoveLayerBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDLMoveLayerBottom, "btnDLMoveLayerBottom");
            this.btnDLMoveLayerBottom.Image = global::Maestro.Editors.Properties.Resources.layers_stack_arrange_back;
            this.btnDLMoveLayerBottom.Name = "btnDLMoveLayerBottom";
            this.btnDLMoveLayerBottom.Click += new System.EventHandler(this.btnDLMoveLayerBottom_Click);
            // 
            // TAB_BASE_LAYERS
            // 
            this.TAB_BASE_LAYERS.Controls.Add(this.trvBaseLayers);
            this.TAB_BASE_LAYERS.Controls.Add(this.toolStrip3);
            resources.ApplyResources(this.TAB_BASE_LAYERS, "TAB_BASE_LAYERS");
            this.TAB_BASE_LAYERS.Name = "TAB_BASE_LAYERS";
            this.TAB_BASE_LAYERS.UseVisualStyleBackColor = true;
            // 
            // trvBaseLayers
            // 
            this.trvBaseLayers.AllowDrop = true;
            this.trvBaseLayers.BackColor = System.Drawing.SystemColors.Window;
            this.trvBaseLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvBaseLayers.DefaultToolTipProvider = null;
            resources.ApplyResources(this.trvBaseLayers, "trvBaseLayers");
            this.trvBaseLayers.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvBaseLayers.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvBaseLayers.Model = null;
            this.trvBaseLayers.Name = "trvBaseLayers";
            this.trvBaseLayers.NodeControls.Add(this.nodeIcon1);
            this.trvBaseLayers.NodeControls.Add(this.nodeTextBox1);
            this.trvBaseLayers.SelectedNode = null;
            this.trvBaseLayers.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.trvBaseLayers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvBaseLayers_ItemDrag);
            this.trvBaseLayers.SelectionChanged += new System.EventHandler(this.trvBaseLayers_SelectionChanged);
            this.trvBaseLayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragDrop);
            this.trvBaseLayers.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragEnter);
            this.trvBaseLayers.DragOver += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragOver);
            this.trvBaseLayers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvBaseLayers_MouseDoubleClick);
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Icon";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = null;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Text";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewBaseLayerGroup,
            this.btnRemoveBaseLayerGroup,
            this.toolStripSeparator2,
            this.btnAddBaseLayer,
            this.btnRemoveBaseLayer,
            this.toolStripSeparator4,
            this.btnMoveBaseLayerUp,
            this.btnMoveBaseLayerDown,
            this.toolStripSeparator8,
            this.btnBaseLayerGroupToRegular,
            this.btnInvokeMgCooker});
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.Name = "toolStrip3";
            // 
            // btnNewBaseLayerGroup
            // 
            this.btnNewBaseLayerGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewBaseLayerGroup.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            resources.ApplyResources(this.btnNewBaseLayerGroup, "btnNewBaseLayerGroup");
            this.btnNewBaseLayerGroup.Name = "btnNewBaseLayerGroup";
            this.btnNewBaseLayerGroup.Click += new System.EventHandler(this.btnNewBaseLayerGroup_Click);
            // 
            // btnRemoveBaseLayerGroup
            // 
            this.btnRemoveBaseLayerGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveBaseLayerGroup, "btnRemoveBaseLayerGroup");
            this.btnRemoveBaseLayerGroup.Image = global::Maestro.Editors.Properties.Resources.folder__minus;
            this.btnRemoveBaseLayerGroup.Name = "btnRemoveBaseLayerGroup";
            this.btnRemoveBaseLayerGroup.Click += new System.EventHandler(this.btnRemoveBaseLayerGroup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnAddBaseLayer
            // 
            this.btnAddBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnAddBaseLayer, "btnAddBaseLayer");
            this.btnAddBaseLayer.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.btnAddBaseLayer.Name = "btnAddBaseLayer";
            this.btnAddBaseLayer.Click += new System.EventHandler(this.btnAddBaseLayer_Click);
            // 
            // btnRemoveBaseLayer
            // 
            this.btnRemoveBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveBaseLayer, "btnRemoveBaseLayer");
            this.btnRemoveBaseLayer.Image = global::Maestro.Editors.Properties.Resources.layer__minus;
            this.btnRemoveBaseLayer.Name = "btnRemoveBaseLayer";
            this.btnRemoveBaseLayer.Click += new System.EventHandler(this.btnRemoveBaseLayer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // btnMoveBaseLayerUp
            // 
            this.btnMoveBaseLayerUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveBaseLayerUp, "btnMoveBaseLayerUp");
            this.btnMoveBaseLayerUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveBaseLayerUp.Name = "btnMoveBaseLayerUp";
            this.btnMoveBaseLayerUp.Click += new System.EventHandler(this.btnMoveBaseLayerUp_Click);
            // 
            // btnMoveBaseLayerDown
            // 
            this.btnMoveBaseLayerDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveBaseLayerDown, "btnMoveBaseLayerDown");
            this.btnMoveBaseLayerDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveBaseLayerDown.Name = "btnMoveBaseLayerDown";
            this.btnMoveBaseLayerDown.Click += new System.EventHandler(this.btnMoveBaseLayerDown_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // btnBaseLayerGroupToRegular
            // 
            this.btnBaseLayerGroupToRegular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnBaseLayerGroupToRegular, "btnBaseLayerGroupToRegular");
            this.btnBaseLayerGroupToRegular.Image = global::Maestro.Editors.Properties.Resources.arrow_curve_180_left;
            this.btnBaseLayerGroupToRegular.Name = "btnBaseLayerGroupToRegular";
            this.btnBaseLayerGroupToRegular.Click += new System.EventHandler(this.btnBaseGroupToRegular_Click);
            // 
            // btnInvokeMgCooker
            // 
            this.btnInvokeMgCooker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnInvokeMgCooker, "btnInvokeMgCooker");
            this.btnInvokeMgCooker.Name = "btnInvokeMgCooker";
            this.btnInvokeMgCooker.Click += new System.EventHandler(this.btnInvokeMgCooker_Click);
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // propertiesPanel
            // 
            resources.ApplyResources(this.propertiesPanel, "propertiesPanel");
            this.propertiesPanel.Name = "propertiesPanel";
            // 
            // MapLayersSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "MapLayersSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TAB_GROUP.ResumeLayout(false);
            this.TAB_GROUP.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TAB_DRAWING_ORDER.ResumeLayout(false);
            this.TAB_DRAWING_ORDER.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.TAB_BASE_LAYERS.ResumeLayout(false);
            this.TAB_BASE_LAYERS.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TAB_GROUP;
        private System.Windows.Forms.TabPage TAB_DRAWING_ORDER;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.Splitter splitter1;
        private Aga.Controls.Tree.TreeViewAdv trvLayersGroup;
        private Aga.Controls.Tree.TreeViewAdv trvLayerDrawingOrder;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnAddGroup;
        private System.Windows.Forms.ToolStripButton btnRemoveGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnGRPAddLayer;
        private System.Windows.Forms.ToolStripButton btnGRPRemoveLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnConvertLayerGroupToBaseGroup;
        private System.Windows.Forms.ToolStripButton btnDLAddLayer;
        private System.Windows.Forms.ToolStripButton btnDLRemoveLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnDLMoveLayerUp;
        private System.Windows.Forms.ToolStripButton btnDLMoveLayerDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnDLMoveLayerTop;
        private System.Windows.Forms.ToolStripButton btnDLMoveLayerBottom;
        private Aga.Controls.Tree.NodeControls.NodeIcon NODE_GROUP_ICON;
        private Aga.Controls.Tree.NodeControls.NodeTextBox NODE_GROUP_TEXT;
        private Aga.Controls.Tree.NodeControls.NodeIcon NODE_DRAW_ICON;
        private Aga.Controls.Tree.NodeControls.NodeTextBox NODE_DRAW_TEXT;
        private System.Windows.Forms.TabPage TAB_BASE_LAYERS;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton btnNewBaseLayerGroup;
        private System.Windows.Forms.ToolStripButton btnRemoveBaseLayerGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAddBaseLayer;
        private System.Windows.Forms.ToolStripButton btnRemoveBaseLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnMoveBaseLayerUp;
        private System.Windows.Forms.ToolStripButton btnMoveBaseLayerDown;
        private Aga.Controls.Tree.TreeViewAdv trvBaseLayers;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private System.Windows.Forms.ToolStripButton btnMoveLayerOrGroupUp;
        private System.Windows.Forms.ToolStripButton btnMoveLayerOrGroupDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnBaseLayerGroupToRegular;
        private System.Windows.Forms.ToolStripButton btnInvokeMgCooker;
    }
}
