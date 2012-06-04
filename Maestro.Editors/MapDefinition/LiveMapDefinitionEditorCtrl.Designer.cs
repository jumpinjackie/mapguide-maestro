namespace Maestro.Editors.MapDefinition
{
    partial class LiveMapDefinitionEditorCtrl
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabLayersAndGroups = new System.Windows.Forms.TabControl();
            this.TAB_LEGEND = new System.Windows.Forms.TabPage();
            this.legendCtrl = new Maestro.MapViewer.Legend();
            this.ctxLegend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeThisGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeThisLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewer = new Maestro.MapViewer.MapViewer();
            this.TAB_DRAW_ORDER = new System.Windows.Forms.TabPage();
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.TAB_PROPERTIES = new System.Windows.Forms.TabPage();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.toolbar = new Maestro.MapViewer.DefaultToolbar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.mapStatusTracker = new Maestro.MapViewer.MapStatusTracker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabLayersAndGroups.SuspendLayout();
            this.TAB_LEGEND.SuspendLayout();
            this.ctxLegend.SuspendLayout();
            this.grpContextMenu.SuspendLayout();
            this.layerContextMenu.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.TAB_PROPERTIES.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.viewer);
            this.splitContainer1.Size = new System.Drawing.Size(800, 486);
            this.splitContainer1.SplitterDistance = 289;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabLayersAndGroups);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabProperties);
            this.splitContainer2.Size = new System.Drawing.Size(289, 486);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabLayersAndGroups
            // 
            this.tabLayersAndGroups.Controls.Add(this.TAB_LEGEND);
            this.tabLayersAndGroups.Controls.Add(this.TAB_DRAW_ORDER);
            this.tabLayersAndGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLayersAndGroups.Location = new System.Drawing.Point(0, 0);
            this.tabLayersAndGroups.Name = "tabLayersAndGroups";
            this.tabLayersAndGroups.SelectedIndex = 0;
            this.tabLayersAndGroups.Size = new System.Drawing.Size(289, 167);
            this.tabLayersAndGroups.TabIndex = 0;
            // 
            // TAB_LEGEND
            // 
            this.TAB_LEGEND.Controls.Add(this.legendCtrl);
            this.TAB_LEGEND.Location = new System.Drawing.Point(4, 22);
            this.TAB_LEGEND.Name = "TAB_LEGEND";
            this.TAB_LEGEND.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_LEGEND.Size = new System.Drawing.Size(281, 141);
            this.TAB_LEGEND.TabIndex = 0;
            this.TAB_LEGEND.Text = "Legend";
            this.TAB_LEGEND.UseVisualStyleBackColor = true;
            // 
            // legendCtrl
            // 
            this.legendCtrl.ContextMenuStrip = this.ctxLegend;
            this.legendCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legendCtrl.GroupContextMenu = this.grpContextMenu;
            this.legendCtrl.LayerContextMenu = this.layerContextMenu;
            this.legendCtrl.Location = new System.Drawing.Point(3, 3);
            this.legendCtrl.Name = "legendCtrl";
            this.legendCtrl.ShowTooltips = true;
            this.legendCtrl.Size = new System.Drawing.Size(275, 135);
            this.legendCtrl.TabIndex = 0;
            this.legendCtrl.ThemeCompressionLimit = 25;
            this.legendCtrl.Viewer = this.viewer;
            this.legendCtrl.NodeSelected += new Maestro.MapViewer.NodeSelectionEventHandler(this.legendCtrl_NodeSelected);
            // 
            // ctxLegend
            // 
            this.ctxLegend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewGroupToolStripMenuItem,
            this.addNewLayerToolStripMenuItem});
            this.ctxLegend.Name = "ctxLegend";
            this.ctxLegend.Size = new System.Drawing.Size(160, 48);
            // 
            // addNewGroupToolStripMenuItem
            // 
            this.addNewGroupToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            this.addNewGroupToolStripMenuItem.Name = "addNewGroupToolStripMenuItem";
            this.addNewGroupToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addNewGroupToolStripMenuItem.Text = "Add New Group";
            // 
            // addNewLayerToolStripMenuItem
            // 
            this.addNewLayerToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.addNewLayerToolStripMenuItem.Name = "addNewLayerToolStripMenuItem";
            this.addNewLayerToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addNewLayerToolStripMenuItem.Text = "Add New Layer";
            // 
            // grpContextMenu
            // 
            this.grpContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLayerToolStripMenuItem,
            this.removeThisGroupToolStripMenuItem});
            this.grpContextMenu.Name = "grpContextMenu";
            this.grpContextMenu.Size = new System.Drawing.Size(179, 48);
            // 
            // addLayerToolStripMenuItem
            // 
            this.addLayerToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.addLayerToolStripMenuItem.Name = "addLayerToolStripMenuItem";
            this.addLayerToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.addLayerToolStripMenuItem.Text = "Add Layer";
            // 
            // removeThisGroupToolStripMenuItem
            // 
            this.removeThisGroupToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder__minus;
            this.removeThisGroupToolStripMenuItem.Name = "removeThisGroupToolStripMenuItem";
            this.removeThisGroupToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.removeThisGroupToolStripMenuItem.Text = "Remove This Group";
            // 
            // layerContextMenu
            // 
            this.layerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeThisLayerToolStripMenuItem});
            this.layerContextMenu.Name = "layerContextMenu";
            this.layerContextMenu.Size = new System.Drawing.Size(174, 26);
            // 
            // removeThisLayerToolStripMenuItem
            // 
            this.removeThisLayerToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer__minus;
            this.removeThisLayerToolStripMenuItem.Name = "removeThisLayerToolStripMenuItem";
            this.removeThisLayerToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeThisLayerToolStripMenuItem.Text = "Remove This Layer";
            // 
            // viewer
            // 
            this.viewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.Location = new System.Drawing.Point(0, 0);
            this.viewer.Name = "viewer";
            this.viewer.PointPixelBuffer = 2;
            this.viewer.SelectionColor = System.Drawing.Color.Blue;
            this.viewer.Size = new System.Drawing.Size(507, 486);
            this.viewer.TabIndex = 1;
            this.viewer.Text = "mapViewer1";
            this.viewer.ZoomInFactor = 0.5D;
            this.viewer.ZoomOutFactor = 2D;
            // 
            // TAB_DRAW_ORDER
            // 
            this.TAB_DRAW_ORDER.Location = new System.Drawing.Point(4, 22);
            this.TAB_DRAW_ORDER.Name = "TAB_DRAW_ORDER";
            this.TAB_DRAW_ORDER.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_DRAW_ORDER.Size = new System.Drawing.Size(197, 141);
            this.TAB_DRAW_ORDER.TabIndex = 1;
            this.TAB_DRAW_ORDER.Text = "Draw Order";
            this.TAB_DRAW_ORDER.UseVisualStyleBackColor = true;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.TAB_PROPERTIES);
            this.tabProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProperties.Location = new System.Drawing.Point(0, 0);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(289, 315);
            this.tabProperties.TabIndex = 0;
            // 
            // TAB_PROPERTIES
            // 
            this.TAB_PROPERTIES.Controls.Add(this.propGrid);
            this.TAB_PROPERTIES.Location = new System.Drawing.Point(4, 22);
            this.TAB_PROPERTIES.Name = "TAB_PROPERTIES";
            this.TAB_PROPERTIES.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_PROPERTIES.Size = new System.Drawing.Size(281, 289);
            this.TAB_PROPERTIES.TabIndex = 1;
            this.TAB_PROPERTIES.Text = "Properties";
            this.TAB_PROPERTIES.UseVisualStyleBackColor = true;
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(3, 3);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(275, 283);
            this.propGrid.TabIndex = 0;
            // 
            // toolbar
            // 
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(800, 25);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "defaultToolbar1";
            this.toolbar.Viewer = this.viewer;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblSelected,
            this.lblScale});
            this.statusStrip.Location = new System.Drawing.Point(0, 511);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(0, 17);
            // 
            // lblSelected
            // 
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(785, 17);
            this.lblSelected.Spring = true;
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(0, 17);
            // 
            // mapStatusTracker
            // 
            this.mapStatusTracker.CoordinatesLabel = this.lblCoordinates;
            this.mapStatusTracker.ScaleLabel = this.lblScale;
            this.mapStatusTracker.SelectedLabel = this.lblSelected;
            this.mapStatusTracker.Viewer = this.viewer;
            // 
            // LiveMapDefinitionEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolbar);
            this.Name = "LiveMapDefinitionEditorCtrl";
            this.Size = new System.Drawing.Size(800, 533);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabLayersAndGroups.ResumeLayout(false);
            this.TAB_LEGEND.ResumeLayout(false);
            this.ctxLegend.ResumeLayout(false);
            this.grpContextMenu.ResumeLayout(false);
            this.layerContextMenu.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.TAB_PROPERTIES.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabLayersAndGroups;
        private System.Windows.Forms.TabPage TAB_LEGEND;
        private System.Windows.Forms.TabPage TAB_DRAW_ORDER;
        private MapViewer.MapViewer viewer;
        private MapViewer.DefaultToolbar toolbar;
        private MapViewer.Legend legendCtrl;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private MapViewer.MapStatusTracker mapStatusTracker;
        private System.Windows.Forms.ContextMenuStrip grpContextMenu;
        private System.Windows.Forms.ContextMenuStrip layerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeThisLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeThisGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxLegend;
        private System.Windows.Forms.ToolStripMenuItem addNewGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewLayerToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TabPage TAB_PROPERTIES;
        private System.Windows.Forms.PropertyGrid propGrid;
    }
}
