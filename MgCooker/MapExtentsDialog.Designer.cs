namespace MgCooker
{
    partial class MapExtentsDialog
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
            this.defaultToolbar = new Maestro.MapViewer.DefaultToolbar();
            this.mapViewer = new Maestro.MapViewer.MapViewer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.legend = new Maestro.MapViewer.Legend();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.txtMaxY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.mapStatusTracker = new Maestro.MapViewer.MapStatusTracker();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // defaultToolbar
            // 
            this.defaultToolbar.Location = new System.Drawing.Point(0, 0);
            this.defaultToolbar.Name = "defaultToolbar";
            this.defaultToolbar.Size = new System.Drawing.Size(871, 25);
            this.defaultToolbar.TabIndex = 0;
            this.defaultToolbar.Text = "defaultToolbar1";
            this.defaultToolbar.Viewer = this.mapViewer;
            // 
            // mapViewer
            // 
            this.mapViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer.Location = new System.Drawing.Point(240, 25);
            this.mapViewer.MaxScale = 1000000000;
            this.mapViewer.MinScale = 10;
            this.mapViewer.MouseWheelDelayRenderInterval = 800;
            this.mapViewer.Name = "mapViewer";
            this.mapViewer.PointPixelBuffer = 2;
            this.mapViewer.SelectionColor = System.Drawing.Color.Blue;
            this.mapViewer.Size = new System.Drawing.Size(431, 450);
            this.mapViewer.TabIndex = 4;
            this.mapViewer.Text = "mapViewer1";
            this.mapViewer.TooltipDelayInterval = 1000;
            this.mapViewer.ZoomInFactor = 0.5D;
            this.mapViewer.ZoomOutFactor = 2D;
            this.mapViewer.MapRefreshed += new System.EventHandler(this.mapViewer_MapRefreshed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblScale,
            this.lblSelected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(871, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(0, 17);
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(856, 17);
            this.lblScale.Spring = true;
            // 
            // lblSelected
            // 
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(0, 17);
            // 
            // legend
            // 
            this.legend.Dock = System.Windows.Forms.DockStyle.Left;
            this.legend.GroupContextMenu = null;
            this.legend.LayerContextMenu = null;
            this.legend.Location = new System.Drawing.Point(0, 25);
            this.legend.Name = "legend";
            this.legend.SelectOnRightClick = false;
            this.legend.ShowAllLayersAndGroups = false;
            this.legend.ShowTooltips = true;
            this.legend.Size = new System.Drawing.Size(240, 450);
            this.legend.TabIndex = 2;
            this.legend.ThemeCompressionLimit = 25;
            this.legend.Viewer = this.mapViewer;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAccept);
            this.panel1.Controls.Add(this.txtMaxY);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtMaxX);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtMinY);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtMinX);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPrompt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(671, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 450);
            this.panel1.TabIndex = 3;
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Enabled = false;
            this.btnAccept.Location = new System.Drawing.Point(27, 376);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(146, 58);
            this.btnAccept.TabIndex = 9;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // txtMaxY
            // 
            this.txtMaxY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxY.Location = new System.Drawing.Point(27, 233);
            this.txtMaxY.Name = "txtMaxY";
            this.txtMaxY.ReadOnly = true;
            this.txtMaxY.Size = new System.Drawing.Size(146, 20);
            this.txtMaxY.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "MaxY";
            // 
            // txtMaxX
            // 
            this.txtMaxX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxX.Location = new System.Drawing.Point(27, 195);
            this.txtMaxX.Name = "txtMaxX";
            this.txtMaxX.ReadOnly = true;
            this.txtMaxX.Size = new System.Drawing.Size(146, 20);
            this.txtMaxX.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "MaxX";
            // 
            // txtMinY
            // 
            this.txtMinY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinY.Location = new System.Drawing.Point(27, 156);
            this.txtMinY.Name = "txtMinY";
            this.txtMinY.ReadOnly = true;
            this.txtMinY.Size = new System.Drawing.Size(146, 20);
            this.txtMinY.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MinY";
            // 
            // txtMinX
            // 
            this.txtMinX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinX.Location = new System.Drawing.Point(27, 114);
            this.txtMinX.Name = "txtMinX";
            this.txtMinX.ReadOnly = true;
            this.txtMinX.Size = new System.Drawing.Size(146, 20);
            this.txtMinX.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "MinX";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrompt.Location = new System.Drawing.Point(6, 15);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(191, 54);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Pan/zoom the map to the desired extents and click Accept to use the current view " +
    "as the extents";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mapStatusTracker
            // 
            this.mapStatusTracker.CoordinatesLabel = this.lblCoordinates;
            this.mapStatusTracker.ScaleLabel = this.lblScale;
            this.mapStatusTracker.SelectedLabel = this.lblSelected;
            this.mapStatusTracker.Viewer = this.mapViewer;
            // 
            // MapExtentsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 497);
            this.Controls.Add(this.mapViewer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.legend);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.defaultToolbar);
            this.Name = "MapExtentsDialog";
            this.Text = "Specify Map Extents";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Maestro.MapViewer.DefaultToolbar defaultToolbar;
        private Maestro.MapViewer.MapViewer mapViewer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private Maestro.MapViewer.Legend legend;
        private System.Windows.Forms.Panel panel1;
        private Maestro.MapViewer.MapStatusTracker mapStatusTracker;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.TextBox txtMaxY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPrompt;
    }
}