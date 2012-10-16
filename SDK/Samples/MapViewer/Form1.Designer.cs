namespace MapViewer
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMapDefinitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.legend1 = new Maestro.MapViewer.Legend();
            this.mapViewer = new Maestro.MapViewer.MapViewer();
            this.defaultToolbar1 = new Maestro.MapViewer.DefaultToolbar();
            this.mapStatusTracker1 = new Maestro.MapViewer.MapStatusTracker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(667, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMapDefinitionToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openMapDefinitionToolStripMenuItem
            // 
            this.openMapDefinitionToolStripMenuItem.Name = "openMapDefinitionToolStripMenuItem";
            this.openMapDefinitionToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openMapDefinitionToolStripMenuItem.Text = "Open Map Definition";
            this.openMapDefinitionToolStripMenuItem.Click += new System.EventHandler(this.openMapDefinitionToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblSelected,
            this.lblScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 370);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(667, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(0, 17);
            // 
            // lblSelected
            // 
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(652, 17);
            this.lblSelected.Spring = true;
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.legend1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mapViewer);
            this.splitContainer1.Size = new System.Drawing.Size(667, 321);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.TabIndex = 2;
            // 
            // legend1
            // 
            this.legend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend1.GroupContextMenu = null;
            this.legend1.LayerContextMenu = null;
            this.legend1.Location = new System.Drawing.Point(0, 0);
            this.legend1.Name = "legend1";
            this.legend1.SelectOnRightClick = false;
            this.legend1.ShowAllLayersAndGroups = false;
            this.legend1.ShowTooltips = true;
            this.legend1.Size = new System.Drawing.Size(222, 321);
            this.legend1.TabIndex = 0;
            this.legend1.ThemeCompressionLimit = 25;
            this.legend1.Viewer = this.mapViewer;
            // 
            // mapViewer
            // 
            this.mapViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer.Location = new System.Drawing.Point(0, 0);
            this.mapViewer.MaxScale = 1000000000;
            this.mapViewer.MinScale = 10;
            this.mapViewer.MouseWheelDelayRenderInterval = 800;
            this.mapViewer.Name = "mapViewer";
            this.mapViewer.PointPixelBuffer = 2;
            this.mapViewer.SelectionColor = System.Drawing.Color.Blue;
            this.mapViewer.Size = new System.Drawing.Size(441, 321);
            this.mapViewer.TabIndex = 0;
            this.mapViewer.Text = "mapViewer1";
            this.mapViewer.TooltipDelayInterval = 1000;
            this.mapViewer.ZoomInFactor = 0.5D;
            this.mapViewer.ZoomOutFactor = 2D;
            // 
            // defaultToolbar1
            // 
            this.defaultToolbar1.Location = new System.Drawing.Point(0, 24);
            this.defaultToolbar1.Name = "defaultToolbar1";
            this.defaultToolbar1.Size = new System.Drawing.Size(667, 25);
            this.defaultToolbar1.TabIndex = 3;
            this.defaultToolbar1.Text = "defaultToolbar1";
            this.defaultToolbar1.Viewer = this.mapViewer;
            // 
            // mapStatusTracker1
            // 
            this.mapStatusTracker1.CoordinatesLabel = this.lblCoordinates;
            this.mapStatusTracker1.ScaleLabel = this.lblScale;
            this.mapStatusTracker1.SelectedLabel = this.lblSelected;
            this.mapStatusTracker1.Viewer = this.mapViewer;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 392);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.defaultToolbar1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Map Viewer Example";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMapDefinitionToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private Maestro.MapViewer.Legend legend1;
        private Maestro.MapViewer.MapViewer mapViewer;
        private Maestro.MapViewer.DefaultToolbar defaultToolbar1;
        private Maestro.MapViewer.MapStatusTracker mapStatusTracker1;
    }
}

