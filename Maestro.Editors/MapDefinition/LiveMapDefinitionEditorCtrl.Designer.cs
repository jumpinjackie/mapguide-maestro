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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveMapDefinitionEditorCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabLayersAndGroups = new System.Windows.Forms.TabControl();
            this.TAB_LEGEND = new System.Windows.Forms.TabPage();
            this.legendCtrl = new Maestro.Editors.MapDefinition.LiveMapEditorLegend();
            this.viewer = new Maestro.MapViewer.MapViewer();
            this.TAB_DRAW_ORDER = new System.Windows.Forms.TabPage();
            this.drawOrderCtrl = new Maestro.Editors.MapDefinition.LiveMapEditorDrawOrder();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabRepo = new System.Windows.Forms.TabControl();
            this.TAB_REPO = new System.Windows.Forms.TabPage();
            this.repoView = new Maestro.Editors.MapDefinition.LiveMapEditorRepositoryView();
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
            this.tabLayersAndGroups.SuspendLayout();
            this.TAB_LEGEND.SuspendLayout();
            this.TAB_DRAW_ORDER.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabRepo.SuspendLayout();
            this.TAB_REPO.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.TAB_PROPERTIES.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabLayersAndGroups);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            // 
            // tabLayersAndGroups
            // 
            this.tabLayersAndGroups.Controls.Add(this.TAB_LEGEND);
            this.tabLayersAndGroups.Controls.Add(this.TAB_DRAW_ORDER);
            resources.ApplyResources(this.tabLayersAndGroups, "tabLayersAndGroups");
            this.tabLayersAndGroups.ImageList = this.imageList1;
            this.tabLayersAndGroups.Name = "tabLayersAndGroups";
            this.tabLayersAndGroups.SelectedIndex = 0;
            // 
            // TAB_LEGEND
            // 
            this.TAB_LEGEND.Controls.Add(this.legendCtrl);
            resources.ApplyResources(this.TAB_LEGEND, "TAB_LEGEND");
            this.TAB_LEGEND.Name = "TAB_LEGEND";
            this.TAB_LEGEND.UseVisualStyleBackColor = true;
            // 
            // legendCtrl
            // 
            this.legendCtrl.AllowDrop = true;
            resources.ApplyResources(this.legendCtrl, "legendCtrl");
            this.legendCtrl.Name = "legendCtrl";
            this.legendCtrl.Viewer = this.viewer;
            this.legendCtrl.NodeDeleted += new Maestro.MapViewer.NodeEventHandler(this.legendCtrl_NodeDeleted);
            this.legendCtrl.NodeSelected += new Maestro.MapViewer.NodeEventHandler(this.legendCtrl_NodeSelected);
            this.legendCtrl.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.legendCtrl_ItemDrag);
            this.legendCtrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.legendCtrl_DragDrop);
            this.legendCtrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.legendCtrl_DragEnter);
            this.legendCtrl.DragOver += new System.Windows.Forms.DragEventHandler(this.legendCtrl_DragOver);
            // 
            // viewer
            // 
            this.viewer.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.viewer, "viewer");
            this.viewer.MaxScale = 1000000000;
            this.viewer.MinScale = 10;
            this.viewer.MouseWheelDelayRenderInterval = 800;
            this.viewer.Name = "viewer";
            this.viewer.PointPixelBuffer = 2;
            this.viewer.SelectionColor = System.Drawing.Color.Blue;
            this.viewer.TooltipDelayInterval = 1000;
            this.viewer.ZoomInFactor = 0.5D;
            this.viewer.ZoomOutFactor = 2D;
            // 
            // TAB_DRAW_ORDER
            // 
            this.TAB_DRAW_ORDER.Controls.Add(this.drawOrderCtrl);
            resources.ApplyResources(this.TAB_DRAW_ORDER, "TAB_DRAW_ORDER");
            this.TAB_DRAW_ORDER.Name = "TAB_DRAW_ORDER";
            this.TAB_DRAW_ORDER.UseVisualStyleBackColor = true;
            // 
            // drawOrderCtrl
            // 
            this.drawOrderCtrl.AllowDrop = true;
            resources.ApplyResources(this.drawOrderCtrl, "drawOrderCtrl");
            this.drawOrderCtrl.Name = "drawOrderCtrl";
            this.drawOrderCtrl.Viewer = this.viewer;
            this.drawOrderCtrl.LayerSelected += new Maestro.Editors.MapDefinition.LayerEventHandler(this.drawOrderCtrl_LayerSelected);
            this.drawOrderCtrl.LayerDeleted += new Maestro.Editors.MapDefinition.LayerEventHandler(this.drawOrderCtrl_LayerDeleted);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "layer.png");
            this.imageList1.Images.SetKeyName(1, "layers-stack.png");
            this.imageList1.Images.SetKeyName(2, "property.png");
            this.imageList1.Images.SetKeyName(3, "folder-tree.png");
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.viewer);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabRepo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabProperties);
            // 
            // tabRepo
            // 
            this.tabRepo.Controls.Add(this.TAB_REPO);
            resources.ApplyResources(this.tabRepo, "tabRepo");
            this.tabRepo.ImageList = this.imageList1;
            this.tabRepo.Name = "tabRepo";
            this.tabRepo.SelectedIndex = 0;
            // 
            // TAB_REPO
            // 
            this.TAB_REPO.Controls.Add(this.repoView);
            resources.ApplyResources(this.TAB_REPO, "TAB_REPO");
            this.TAB_REPO.Name = "TAB_REPO";
            this.TAB_REPO.UseVisualStyleBackColor = true;
            // 
            // repoView
            // 
            resources.ApplyResources(this.repoView, "repoView");
            this.repoView.Name = "repoView";
            this.repoView.ItemSelected += new System.EventHandler(this.repoView_ItemSelected);
            this.repoView.RequestAddToMap += new System.EventHandler(this.repoView_RequestAddToMap);
            this.repoView.RequestEdit += new System.EventHandler(this.repoView_RequestEdit);
            this.repoView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.repoView_ItemDrag);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.TAB_PROPERTIES);
            resources.ApplyResources(this.tabProperties, "tabProperties");
            this.tabProperties.ImageList = this.imageList1;
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            // 
            // TAB_PROPERTIES
            // 
            this.TAB_PROPERTIES.Controls.Add(this.propGrid);
            resources.ApplyResources(this.TAB_PROPERTIES, "TAB_PROPERTIES");
            this.TAB_PROPERTIES.Name = "TAB_PROPERTIES";
            this.TAB_PROPERTIES.UseVisualStyleBackColor = true;
            // 
            // propGrid
            // 
            resources.ApplyResources(this.propGrid, "propGrid");
            this.propGrid.Name = "propGrid";
            // 
            // toolbar
            // 
            resources.ApplyResources(this.toolbar, "toolbar");
            this.toolbar.Name = "toolbar";
            this.toolbar.Viewer = this.viewer;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblSelected,
            this.lblScale});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            resources.ApplyResources(this.lblCoordinates, "lblCoordinates");
            // 
            // lblSelected
            // 
            this.lblSelected.Name = "lblSelected";
            resources.ApplyResources(this.lblSelected, "lblSelected");
            this.lblSelected.Spring = true;
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            resources.ApplyResources(this.lblScale, "lblScale");
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolbar);
            this.Name = "LiveMapDefinitionEditorCtrl";
            resources.ApplyResources(this, "$this");
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabLayersAndGroups.ResumeLayout(false);
            this.TAB_LEGEND.ResumeLayout(false);
            this.TAB_DRAW_ORDER.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabRepo.ResumeLayout(false);
            this.TAB_REPO.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.TAB_PROPERTIES.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Maestro.Editors.MapDefinition.LiveMapEditorDrawOrder drawOrderCtrl;
        private Maestro.Editors.MapDefinition.LiveMapEditorLegend legendCtrl;

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabLayersAndGroups;
        private System.Windows.Forms.TabPage TAB_LEGEND;
        private System.Windows.Forms.TabPage TAB_DRAW_ORDER;
        private MapViewer.MapViewer viewer;
        private MapViewer.DefaultToolbar toolbar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private MapViewer.MapStatusTracker mapStatusTracker;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TabPage TAB_PROPERTIES;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabRepo;
        private System.Windows.Forms.TabPage TAB_REPO;
        private LiveMapEditorRepositoryView repoView;
        private System.Windows.Forms.ImageList imageList1;
    }
}
