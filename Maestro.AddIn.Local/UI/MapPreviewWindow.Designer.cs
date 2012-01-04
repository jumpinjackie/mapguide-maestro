namespace Maestro.AddIn.Local.UI
{
    partial class MapPreviewWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapPreviewWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFeaturesSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelectedFeatures = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.legend = new OSGeo.MapGuide.Viewer.MgLegend();
            this.propertyPane = new OSGeo.MapGuide.Viewer.MgPropertyPane();
            this.viewer = new OSGeo.MapGuide.Viewer.MgMapViewer();
            this.toolbar = new OSGeo.MapGuide.Viewer.MgDefaultToolbar();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblFeaturesSelected,
            this.lblScale,
            this.lblSelectedFeatures,
            this.lblMapSize});
            this.statusStrip1.Location = new System.Drawing.Point(0, 465);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(0, 17);
            this.lblCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFeaturesSelected
            // 
            this.lblFeaturesSelected.Name = "lblFeaturesSelected";
            this.lblFeaturesSelected.Size = new System.Drawing.Size(659, 17);
            this.lblFeaturesSelected.Spring = true;
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(0, 17);
            // 
            // lblSelectedFeatures
            // 
            this.lblSelectedFeatures.Name = "lblSelectedFeatures";
            this.lblSelectedFeatures.Size = new System.Drawing.Size(0, 17);
            // 
            // lblMapSize
            // 
            this.lblMapSize.Name = "lblMapSize";
            this.lblMapSize.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer1.Size = new System.Drawing.Size(674, 440);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.legend);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyPane);
            this.splitContainer2.Size = new System.Drawing.Size(224, 440);
            this.splitContainer2.SplitterDistance = 223;
            this.splitContainer2.TabIndex = 0;
            // 
            // legend
            // 
            this.legend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend.GroupContextMenu = null;
            this.legend.LayerContextMenu = null;
            this.legend.Location = new System.Drawing.Point(0, 0);
            this.legend.Name = "legend";
            this.legend.Size = new System.Drawing.Size(224, 223);
            this.legend.TabIndex = 0;
            this.legend.ThemeCompressionLimit = 25;
            // 
            // propertyPane
            // 
            this.propertyPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyPane.Location = new System.Drawing.Point(0, 0);
            this.propertyPane.Name = "propertyPane";
            this.propertyPane.Size = new System.Drawing.Size(224, 213);
            this.propertyPane.TabIndex = 1;
            // 
            // viewer
            // 
            this.viewer.ConvertTiledGroupsToNonTiled = true;
            this.viewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.Location = new System.Drawing.Point(0, 0);
            this.viewer.Name = "viewer";
            this.viewer.SelectionColor = System.Drawing.Color.Blue;
            this.viewer.Size = new System.Drawing.Size(446, 440);
            this.viewer.TabIndex = 0;
            this.viewer.Text = "mgMapViewer1";
            this.viewer.ZoomInFactor = 0.75;
            this.viewer.ZoomOutFactor = 1.35;
            // 
            // toolbar
            // 
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(674, 25);
            this.toolbar.TabIndex = 3;
            this.toolbar.Text = "mgDefaultToolbar1";
            this.toolbar.Viewer = null;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(106, 22);
            this.toolStripButton1.Text = "Zoom To Scale";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // MapPreviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 487);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MapPreviewWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Map Preview";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblFeaturesSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedFeatures;
        private System.Windows.Forms.ToolStripStatusLabel lblMapSize;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private OSGeo.MapGuide.Viewer.MgLegend legend;
        private OSGeo.MapGuide.Viewer.MgPropertyPane propertyPane;
        private OSGeo.MapGuide.Viewer.MgMapViewer viewer;
        private OSGeo.MapGuide.Viewer.MgDefaultToolbar toolbar;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}