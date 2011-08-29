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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mapImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnZoomScale = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.btnLowerRight = new System.Windows.Forms.Button();
            this.btnLowerLeft = new System.Windows.Forms.Button();
            this.btnUpperLeft = new System.Windows.Forms.Button();
            this.btnUpperRight = new System.Windows.Forms.Button();
            this.btnClearSelect = new System.Windows.Forms.Button();
            this.chkSelectFeatures = new System.Windows.Forms.CheckBox();
            this.btnZoomExtents = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.renderWorker = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCoordinates = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFeaturesSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mapImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(674, 465);
            this.splitContainer1.SplitterDistance = 549;
            this.splitContainer1.TabIndex = 0;
            // 
            // mapImage
            // 
            this.mapImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapImage.Location = new System.Drawing.Point(0, 0);
            this.mapImage.Name = "mapImage";
            this.mapImage.Size = new System.Drawing.Size(549, 465);
            this.mapImage.TabIndex = 0;
            this.mapImage.TabStop = false;
            this.mapImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapImage_MouseMove);
            this.mapImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapImage_MouseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(121, 465);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnZoomScale);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numScale);
            this.groupBox1.Controls.Add(this.btnLowerRight);
            this.groupBox1.Controls.Add(this.btnLowerLeft);
            this.groupBox1.Controls.Add(this.btnUpperLeft);
            this.groupBox1.Controls.Add(this.btnUpperRight);
            this.groupBox1.Controls.Add(this.btnClearSelect);
            this.groupBox1.Controls.Add(this.chkSelectFeatures);
            this.groupBox1.Controls.Add(this.btnZoomExtents);
            this.groupBox1.Controls.Add(this.btnZoomOut);
            this.groupBox1.Controls.Add(this.btnZoomIn);
            this.groupBox1.Controls.Add(this.btnDown);
            this.groupBox1.Controls.Add(this.btnRight);
            this.groupBox1.Controls.Add(this.btnLeft);
            this.groupBox1.Controls.Add(this.btnUp);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 336);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // btnZoomScale
            // 
            this.btnZoomScale.Location = new System.Drawing.Point(22, 295);
            this.btnZoomScale.Name = "btnZoomScale";
            this.btnZoomScale.Size = new System.Drawing.Size(66, 23);
            this.btnZoomScale.TabIndex = 17;
            this.btnZoomScale.Text = "Zoom";
            this.btnZoomScale.UseVisualStyleBackColor = true;
            this.btnZoomScale.Click += new System.EventHandler(this.btnZoomScale_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Current Scale";
            // 
            // numScale
            // 
            this.numScale.Location = new System.Drawing.Point(22, 268);
            this.numScale.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(66, 20);
            this.numScale.TabIndex = 15;
            this.numScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnLowerRight
            // 
            this.btnLowerRight.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_315;
            this.btnLowerRight.Location = new System.Drawing.Point(75, 93);
            this.btnLowerRight.Name = "btnLowerRight";
            this.btnLowerRight.Size = new System.Drawing.Size(30, 30);
            this.btnLowerRight.TabIndex = 14;
            this.btnLowerRight.UseVisualStyleBackColor = true;
            this.btnLowerRight.Click += new System.EventHandler(this.btnLowerRight_Click);
            // 
            // btnLowerLeft
            // 
            this.btnLowerLeft.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_225;
            this.btnLowerLeft.Location = new System.Drawing.Point(6, 93);
            this.btnLowerLeft.Name = "btnLowerLeft";
            this.btnLowerLeft.Size = new System.Drawing.Size(30, 30);
            this.btnLowerLeft.TabIndex = 13;
            this.btnLowerLeft.UseVisualStyleBackColor = true;
            this.btnLowerLeft.Click += new System.EventHandler(this.btnLowerLeft_Click);
            // 
            // btnUpperLeft
            // 
            this.btnUpperLeft.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_135;
            this.btnUpperLeft.Location = new System.Drawing.Point(6, 20);
            this.btnUpperLeft.Name = "btnUpperLeft";
            this.btnUpperLeft.Size = new System.Drawing.Size(30, 30);
            this.btnUpperLeft.TabIndex = 12;
            this.btnUpperLeft.UseVisualStyleBackColor = true;
            this.btnUpperLeft.Click += new System.EventHandler(this.btnUpperLeft_Click);
            // 
            // btnUpperRight
            // 
            this.btnUpperRight.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_045;
            this.btnUpperRight.Location = new System.Drawing.Point(74, 20);
            this.btnUpperRight.Name = "btnUpperRight";
            this.btnUpperRight.Size = new System.Drawing.Size(30, 30);
            this.btnUpperRight.TabIndex = 11;
            this.btnUpperRight.UseVisualStyleBackColor = true;
            this.btnUpperRight.Click += new System.EventHandler(this.btnUpperRight_Click);
            // 
            // btnClearSelect
            // 
            this.btnClearSelect.Image = global::Maestro.AddIn.Local.Properties.Resources.icon_clearselect;
            this.btnClearSelect.Location = new System.Drawing.Point(58, 183);
            this.btnClearSelect.Name = "btnClearSelect";
            this.btnClearSelect.Size = new System.Drawing.Size(30, 30);
            this.btnClearSelect.TabIndex = 8;
            this.btnClearSelect.UseVisualStyleBackColor = true;
            this.btnClearSelect.Click += new System.EventHandler(this.btnClearSelect_Click);
            // 
            // chkSelectFeatures
            // 
            this.chkSelectFeatures.AutoSize = true;
            this.chkSelectFeatures.Location = new System.Drawing.Point(22, 219);
            this.chkSelectFeatures.Name = "chkSelectFeatures";
            this.chkSelectFeatures.Size = new System.Drawing.Size(56, 17);
            this.chkSelectFeatures.TabIndex = 7;
            this.chkSelectFeatures.Text = "Select";
            this.chkSelectFeatures.UseVisualStyleBackColor = true;
            // 
            // btnZoomExtents
            // 
            this.btnZoomExtents.Image = global::Maestro.AddIn.Local.Properties.Resources.magnifier_zoom_fit;
            this.btnZoomExtents.Location = new System.Drawing.Point(22, 183);
            this.btnZoomExtents.Name = "btnZoomExtents";
            this.btnZoomExtents.Size = new System.Drawing.Size(30, 30);
            this.btnZoomExtents.TabIndex = 6;
            this.btnZoomExtents.UseVisualStyleBackColor = true;
            this.btnZoomExtents.Click += new System.EventHandler(this.btnZoomExtents_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Image = global::Maestro.AddIn.Local.Properties.Resources.magnifier_zoom_out;
            this.btnZoomOut.Location = new System.Drawing.Point(58, 147);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(30, 30);
            this.btnZoomOut.TabIndex = 5;
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Image = global::Maestro.AddIn.Local.Properties.Resources.magnifier_zoom_in;
            this.btnZoomIn.Location = new System.Drawing.Point(22, 147);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(30, 30);
            this.btnZoomIn.TabIndex = 4;
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_270;
            this.btnDown.Location = new System.Drawing.Point(40, 93);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 30);
            this.btnDown.TabIndex = 3;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnRight
            // 
            this.btnRight.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow;
            this.btnRight.Location = new System.Drawing.Point(74, 57);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(30, 30);
            this.btnRight.TabIndex = 2;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_180;
            this.btnLeft.Location = new System.Drawing.Point(6, 57);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(30, 30);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::Maestro.AddIn.Local.Properties.Resources.arrow_090;
            this.btnUp.Location = new System.Drawing.Point(40, 20);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 30);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // renderWorker
            // 
            this.renderWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.renderWorker_DoWork);
            this.renderWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.renderWorker_RunWorkerCompleted);
            this.renderWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.renderWorker_ProgressChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCoordinates,
            this.lblFeaturesSelected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 465);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(659, 17);
            this.lblCoordinates.Spring = true;
            this.lblCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFeaturesSelected
            // 
            this.lblFeaturesSelected.Name = "lblFeaturesSelected";
            this.lblFeaturesSelected.Size = new System.Drawing.Size(0, 17);
            // 
            // MapPreviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 487);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MapPreviewWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Map Preview";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox mapImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomExtents;
        private System.ComponentModel.BackgroundWorker renderWorker;
        private System.Windows.Forms.CheckBox chkSelectFeatures;
        private System.Windows.Forms.Button btnClearSelect;
        private System.Windows.Forms.Button btnLowerRight;
        private System.Windows.Forms.Button btnLowerLeft;
        private System.Windows.Forms.Button btnUpperLeft;
        private System.Windows.Forms.Button btnUpperRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.Button btnZoomScale;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinates;
        private System.Windows.Forms.ToolStripStatusLabel lblFeaturesSelected;
    }
}