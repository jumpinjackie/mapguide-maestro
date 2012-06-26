
namespace Maestro.Editors.MapDefinition
{
    partial class LiveMapEditorLegend
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddLayer = new System.Windows.Forms.ToolStripButton();
            this.btnAddGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxLegend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeThisGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeThisLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendCtrl = new Maestro.MapViewer.Legend();
            this.toolStrip1.SuspendLayout();
            this.ctxLegend.SuspendLayout();
            this.grpContextMenu.SuspendLayout();
            this.layerContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddLayer,
            this.btnAddGroup,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(275, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.btnAddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(80, 22);
            this.btnAddLayer.Text = "Add Layer";
            this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            this.btnAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(85, 22);
            this.btnAddGroup.Text = "Add Group";
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ctxLegend
            // 
            this.ctxLegend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewGroupToolStripMenuItem,
            this.addNewLayerToolStripMenuItem});
            this.ctxLegend.Name = "ctxLegend";
            this.ctxLegend.Size = new System.Drawing.Size(160, 48);
            this.ctxLegend.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // addNewGroupToolStripMenuItem
            // 
            this.addNewGroupToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            this.addNewGroupToolStripMenuItem.Name = "addNewGroupToolStripMenuItem";
            this.addNewGroupToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addNewGroupToolStripMenuItem.Text = "Add New Group";
            this.addNewGroupToolStripMenuItem.Click += new System.EventHandler(this.btnAddGroup_Click);
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
            this.addLayerToolStripMenuItem.Click += new System.EventHandler(this.addLayerToolStripMenuItem_Click);
            // 
            // removeThisGroupToolStripMenuItem
            // 
            this.removeThisGroupToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder__minus;
            this.removeThisGroupToolStripMenuItem.Name = "removeThisGroupToolStripMenuItem";
            this.removeThisGroupToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.removeThisGroupToolStripMenuItem.Text = "Remove This Group";
            this.removeThisGroupToolStripMenuItem.Click += new System.EventHandler(this.removeThisGroupToolStripMenuItem_Click);
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
            this.removeThisLayerToolStripMenuItem.Click += new System.EventHandler(this.removeThisLayerToolStripMenuItem_Click);
            // 
            // legendCtrl
            // 
            this.legendCtrl.ContextMenuStrip = this.ctxLegend;
            this.legendCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legendCtrl.GroupContextMenu = this.grpContextMenu;
            this.legendCtrl.LayerContextMenu = this.layerContextMenu;
            this.legendCtrl.Location = new System.Drawing.Point(0, 25);
            this.legendCtrl.Name = "legendCtrl";
            this.legendCtrl.SelectOnRightClick = true;
            this.legendCtrl.ShowAllLayersAndGroups = true;
            this.legendCtrl.ShowTooltips = true;
            this.legendCtrl.Size = new System.Drawing.Size(275, 356);
            this.legendCtrl.TabIndex = 1;
            this.legendCtrl.ThemeCompressionLimit = 25;
            this.legendCtrl.Viewer = null;
            // 
            // LiveMapEditorLegend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.legendCtrl);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LiveMapEditorLegend";
            this.Size = new System.Drawing.Size(275, 381);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ctxLegend.ResumeLayout(false);
            this.grpContextMenu.ResumeLayout(false);
            this.layerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddGroup;
        private System.Windows.Forms.ToolStripButton btnAddLayer;
        private Maestro.MapViewer.Legend legendCtrl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ContextMenuStrip ctxLegend;
        private System.Windows.Forms.ToolStripMenuItem addNewGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewLayerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip grpContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeThisGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip layerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeThisLayerToolStripMenuItem;
    }
}
