namespace Maestro.TestViewer
{
    partial class MainForm
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
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.saveBackToMapDefinitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legend1 = new Maestro.MapViewer.Legend();
            this.mapViewer1 = new Maestro.MapViewer.MapViewer();
            this.defaultToolbar1 = new Maestro.MapViewer.DefaultToolbar();
            this.mapStatusTracker1 = new Maestro.MapViewer.MapStatusTracker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(655, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMapDefinitionToolStripMenuItem,
            this.saveBackToMapDefinitionToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openMapDefinitionToolStripMenuItem
            // 
            this.openMapDefinitionToolStripMenuItem.Name = "openMapDefinitionToolStripMenuItem";
            this.openMapDefinitionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.openMapDefinitionToolStripMenuItem.Text = "Open Map Definition";
            this.openMapDefinitionToolStripMenuItem.Click += new System.EventHandler(this.openMapDefinitionToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblSelected,
            this.lblScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 453);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(655, 22);
            this.statusStrip1.TabIndex = 2;
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
            this.lblSelected.Size = new System.Drawing.Size(640, 17);
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
            this.splitContainer1.Panel2.Controls.Add(this.mapViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(655, 404);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 3;
            // 
            // saveBackToMapDefinitionToolStripMenuItem
            // 
            this.saveBackToMapDefinitionToolStripMenuItem.Enabled = false;
            this.saveBackToMapDefinitionToolStripMenuItem.Name = "saveBackToMapDefinitionToolStripMenuItem";
            this.saveBackToMapDefinitionToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.saveBackToMapDefinitionToolStripMenuItem.Text = "Save back to Map Definition";
            this.saveBackToMapDefinitionToolStripMenuItem.Click += new System.EventHandler(this.saveBackToMapDefinitionToolStripMenuItem_Click);
            // 
            // legend1
            // 
            this.legend1.AllowDrop = true;
            this.legend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend1.GroupContextMenu = null;
            this.legend1.LayerContextMenu = null;
            this.legend1.Location = new System.Drawing.Point(0, 0);
            this.legend1.Name = "legend1";
            this.legend1.ShowTooltips = false;
            this.legend1.Size = new System.Drawing.Size(217, 404);
            this.legend1.TabIndex = 0;
            this.legend1.ThemeCompressionLimit = 25;
            this.legend1.Viewer = this.mapViewer1;
            this.legend1.DragDrop += new System.Windows.Forms.DragEventHandler(this.legend1_DragDrop);
            this.legend1.DragEnter += new System.Windows.Forms.DragEventHandler(this.legend1_DragEnter);
            this.legend1.DragOver += new System.Windows.Forms.DragEventHandler(this.legend1_DragOver);
            this.legend1.DragLeave += new System.EventHandler(this.legend1_DragLeave);
            // 
            // mapViewer1
            // 
            this.mapViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer1.Location = new System.Drawing.Point(0, 0);
            this.mapViewer1.Name = "mapViewer1";
            this.mapViewer1.PointPixelBuffer = 2;
            this.mapViewer1.SelectionColor = System.Drawing.Color.Blue;
            this.mapViewer1.Size = new System.Drawing.Size(434, 404);
            this.mapViewer1.TabIndex = 1;
            this.mapViewer1.Text = "mapViewer1";
            this.mapViewer1.ZoomInFactor = 0.5D;
            this.mapViewer1.ZoomOutFactor = 2D;
            // 
            // defaultToolbar1
            // 
            this.defaultToolbar1.Location = new System.Drawing.Point(0, 24);
            this.defaultToolbar1.Name = "defaultToolbar1";
            this.defaultToolbar1.Size = new System.Drawing.Size(655, 25);
            this.defaultToolbar1.TabIndex = 0;
            this.defaultToolbar1.Text = "defaultToolbar1";
            this.defaultToolbar1.Viewer = this.mapViewer1;
            // 
            // mapStatusTracker1
            // 
            this.mapStatusTracker1.CoordinatesLabel = this.lblCoordinates;
            this.mapStatusTracker1.ScaleLabel = this.lblScale;
            this.mapStatusTracker1.SelectedLabel = this.lblSelected;
            this.mapStatusTracker1.Viewer = this.mapViewer1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 475);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.defaultToolbar1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Maestro Test Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMapDefinitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private MapViewer.MapViewer mapViewer1;
        private MapViewer.DefaultToolbar defaultToolbar1;
        private MapViewer.MapStatusTracker mapStatusTracker1;
        private MapViewer.Legend legend1;
        private System.Windows.Forms.ToolStripMenuItem saveBackToMapDefinitionToolStripMenuItem;
    }
}

