namespace Maestro.Editors.Preview
{
    partial class MapPreviewDialog
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
            this.lblSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.legend = new Maestro.MapViewer.Legend();
            this.mapStatusTracker = new Maestro.MapViewer.MapStatusTracker();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpOtherTools = new System.Windows.Forms.GroupBox();
            this.btnGetMapKml = new System.Windows.Forms.Button();
            this.lnkZoomToScale = new System.Windows.Forms.LinkLabel();
            this.numZoomToScale = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpOtherTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZoomToScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // defaultToolbar
            // 
            this.defaultToolbar.Location = new System.Drawing.Point(0, 0);
            this.defaultToolbar.Name = "defaultToolbar";
            this.defaultToolbar.Size = new System.Drawing.Size(784, 25);
            this.defaultToolbar.TabIndex = 0;
            this.defaultToolbar.Viewer = this.mapViewer;
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
            this.mapViewer.Size = new System.Drawing.Size(381, 515);
            this.mapViewer.TabIndex = 0;
            this.mapViewer.TooltipDelayInterval = 1000;
            this.mapViewer.ZoomInFactor = 0.5D;
            this.mapViewer.ZoomOutFactor = 2D;
            this.mapViewer.MapScaleChanged += new System.EventHandler(this.mapViewer_MapScaleChanged);
            this.mapViewer.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.mapViewer_PropertyChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblSelected,
            this.lblScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
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
            this.lblSelected.Size = new System.Drawing.Size(769, 17);
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.legend);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mapViewer);
            this.splitContainer1.Size = new System.Drawing.Size(584, 515);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 2;
            // 
            // legend
            // 
            this.legend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend.GroupContextMenu = null;
            this.legend.LayerContextMenu = null;
            this.legend.Location = new System.Drawing.Point(0, 0);
            this.legend.Name = "legend";
            this.legend.SelectOnRightClick = false;
            this.legend.ShowAllLayersAndGroups = false;
            this.legend.ShowTooltips = true;
            this.legend.Size = new System.Drawing.Size(199, 515);
            this.legend.TabIndex = 0;
            this.legend.ThemeCompressionLimit = 25;
            this.legend.Viewer = this.mapViewer;
            // 
            // mapStatusTracker
            // 
            this.mapStatusTracker.CoordinatesLabel = this.lblCoordinates;
            this.mapStatusTracker.ScaleLabel = this.lblScale;
            this.mapStatusTracker.SelectedLabel = this.lblSelected;
            this.mapStatusTracker.Viewer = this.mapViewer;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(784, 515);
            this.splitContainer2.SplitterDistance = 584;
            this.splitContainer2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.grpOtherTools);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 515);
            this.panel1.TabIndex = 1;
            // 
            // grpOtherTools
            // 
            this.grpOtherTools.Controls.Add(this.btnGetMapKml);
            this.grpOtherTools.Controls.Add(this.lnkZoomToScale);
            this.grpOtherTools.Controls.Add(this.numZoomToScale);
            this.grpOtherTools.Controls.Add(this.label5);
            this.grpOtherTools.Location = new System.Drawing.Point(16, 367);
            this.grpOtherTools.Name = "grpOtherTools";
            this.grpOtherTools.Size = new System.Drawing.Size(165, 115);
            this.grpOtherTools.TabIndex = 1;
            this.grpOtherTools.TabStop = false;
            this.grpOtherTools.Text = "Other Tools";
            // 
            // btnGetMapKml
            // 
            this.btnGetMapKml.Location = new System.Drawing.Point(10, 74);
            this.btnGetMapKml.Name = "btnGetMapKml";
            this.btnGetMapKml.Size = new System.Drawing.Size(94, 23);
            this.btnGetMapKml.TabIndex = 4;
            this.btnGetMapKml.Text = "Get Map KML";
            this.btnGetMapKml.UseVisualStyleBackColor = true;
            this.btnGetMapKml.Click += new System.EventHandler(this.btnGetMapKml_Click);
            // 
            // lnkZoomToScale
            // 
            this.lnkZoomToScale.AutoSize = true;
            this.lnkZoomToScale.Location = new System.Drawing.Point(93, 32);
            this.lnkZoomToScale.Name = "lnkZoomToScale";
            this.lnkZoomToScale.Size = new System.Drawing.Size(40, 13);
            this.lnkZoomToScale.TabIndex = 3;
            this.lnkZoomToScale.TabStop = true;
            this.lnkZoomToScale.Text = "(Zoom)";
            this.lnkZoomToScale.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkZoomToScale_LinkClicked);
            // 
            // numZoomToScale
            // 
            this.numZoomToScale.Location = new System.Drawing.Point(10, 48);
            this.numZoomToScale.Name = "numZoomToScale";
            this.numZoomToScale.Size = new System.Drawing.Size(135, 20);
            this.numZoomToScale.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Zoom To Scale";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCoordinateSystem);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtMaxY);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMaxX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMinY);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMinX);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 351);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Extents";
            // 
            // txtMaxY
            // 
            this.txtMaxY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxY.Location = new System.Drawing.Point(10, 167);
            this.txtMaxY.Name = "txtMaxY";
            this.txtMaxY.ReadOnly = true;
            this.txtMaxY.Size = new System.Drawing.Size(135, 20);
            this.txtMaxY.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Max Y";
            // 
            // txtMaxX
            // 
            this.txtMaxX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxX.Location = new System.Drawing.Point(10, 129);
            this.txtMaxX.Name = "txtMaxX";
            this.txtMaxX.ReadOnly = true;
            this.txtMaxX.Size = new System.Drawing.Size(135, 20);
            this.txtMaxX.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Max X";
            // 
            // txtMinY
            // 
            this.txtMinY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinY.Location = new System.Drawing.Point(10, 89);
            this.txtMinY.Name = "txtMinY";
            this.txtMinY.ReadOnly = true;
            this.txtMinY.Size = new System.Drawing.Size(135, 20);
            this.txtMinY.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Min Y";
            // 
            // txtMinX
            // 
            this.txtMinX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinX.Location = new System.Drawing.Point(10, 49);
            this.txtMinX.Name = "txtMinX";
            this.txtMinX.ReadOnly = true;
            this.txtMinX.Size = new System.Drawing.Size(135, 20);
            this.txtMinX.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Coordinate System";
            // 
            // txtCoordinateSystem
            // 
            this.txtCoordinateSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCoordinateSystem.Location = new System.Drawing.Point(10, 207);
            this.txtCoordinateSystem.Multiline = true;
            this.txtCoordinateSystem.Name = "txtCoordinateSystem";
            this.txtCoordinateSystem.ReadOnly = true;
            this.txtCoordinateSystem.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCoordinateSystem.Size = new System.Drawing.Size(135, 123);
            this.txtCoordinateSystem.TabIndex = 9;
            // 
            // MapPreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.defaultToolbar);
            this.Name = "MapPreviewDialog";
            this.Text = "Map Preview";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpOtherTools.ResumeLayout(false);
            this.grpOtherTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZoomToScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapViewer.DefaultToolbar defaultToolbar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MapViewer.Legend legend;
        private MapViewer.MapViewer mapViewer;
        private MapViewer.MapStatusTracker mapStatusTracker;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpOtherTools;
        private System.Windows.Forms.LinkLabel lnkZoomToScale;
        private System.Windows.Forms.NumericUpDown numZoomToScale;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetMapKml;
        private System.Windows.Forms.TextBox txtCoordinateSystem;
        private System.Windows.Forms.Label label6;
    }
}