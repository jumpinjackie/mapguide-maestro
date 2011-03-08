namespace Maestro.Editors.LayerDefinition.Raster
{
    partial class RasterLayerAdvancedSectionCtrl
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
            this.chkAdvanced = new System.Windows.Forms.CheckBox();
            this.txtContrastFactor = new System.Windows.Forms.TextBox();
            this.txtBrightnessFactor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbTransparencyColor = new Maestro.Editors.Common.ColorComboBox();
            this.EnableHillshade = new System.Windows.Forms.CheckBox();
            this.HillshadeGroup = new System.Windows.Forms.GroupBox();
            this.txtHillshadeBand = new System.Windows.Forms.TextBox();
            this.txtHillshadeScaleFactor = new System.Windows.Forms.TextBox();
            this.txtHillshadeAzimuth = new System.Windows.Forms.TextBox();
            this.txtHillshadeAltitude = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.EnableSurface = new System.Windows.Forms.CheckBox();
            this.SurfaceGroup = new System.Windows.Forms.GroupBox();
            this.cmbSurfaceDefaultColor = new Maestro.Editors.Common.ColorComboBox();
            this.txtSurfaceBand = new System.Windows.Forms.TextBox();
            this.txtSurfaceScaleFactor = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSurfaceZeroValue = new System.Windows.Forms.TextBox();
            this.contentPanel.SuspendLayout();
            this.HillshadeGroup.SuspendLayout();
            this.SurfaceGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.EnableSurface);
            this.contentPanel.Controls.Add(this.SurfaceGroup);
            this.contentPanel.Controls.Add(this.EnableHillshade);
            this.contentPanel.Controls.Add(this.HillshadeGroup);
            this.contentPanel.Controls.Add(this.cmbTransparencyColor);
            this.contentPanel.Controls.Add(this.txtContrastFactor);
            this.contentPanel.Controls.Add(this.txtBrightnessFactor);
            this.contentPanel.Controls.Add(this.label6);
            this.contentPanel.Controls.Add(this.label5);
            this.contentPanel.Controls.Add(this.label7);
            this.contentPanel.Controls.Add(this.chkAdvanced);
            this.contentPanel.Size = new System.Drawing.Size(449, 364);
            // 
            // chkAdvanced
            // 
            this.chkAdvanced.AutoSize = true;
            this.chkAdvanced.Location = new System.Drawing.Point(15, 6);
            this.chkAdvanced.Name = "chkAdvanced";
            this.chkAdvanced.Size = new System.Drawing.Size(152, 17);
            this.chkAdvanced.TabIndex = 0;
            this.chkAdvanced.Text = "Enable Advanced Settings";
            this.chkAdvanced.UseVisualStyleBackColor = true;
            this.chkAdvanced.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // txtContrastFactor
            // 
            this.txtContrastFactor.Location = new System.Drawing.Point(159, 58);
            this.txtContrastFactor.Name = "txtContrastFactor";
            this.txtContrastFactor.Size = new System.Drawing.Size(104, 20);
            this.txtContrastFactor.TabIndex = 16;
            this.txtContrastFactor.Text = "0";
            this.txtContrastFactor.TextChanged += new System.EventHandler(this.txtContrastFactor_TextChanged);
            // 
            // txtBrightnessFactor
            // 
            this.txtBrightnessFactor.Location = new System.Drawing.Point(159, 34);
            this.txtBrightnessFactor.Name = "txtBrightnessFactor";
            this.txtBrightnessFactor.Size = new System.Drawing.Size(104, 20);
            this.txtBrightnessFactor.TabIndex = 15;
            this.txtBrightnessFactor.Text = "0";
            this.txtBrightnessFactor.TextChanged += new System.EventHandler(this.txtBrightnessFactor_TextChanged);
            // 
            // label6
            // 
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(30, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Contrast factor";
            // 
            // label5
            // 
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(30, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Brightness factor";
            // 
            // label7
            // 
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(30, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Transparency Color";
            // 
            // cmbTransparencyColor
            // 
            this.cmbTransparencyColor.FormattingEnabled = true;
            this.cmbTransparencyColor.Location = new System.Drawing.Point(159, 82);
            this.cmbTransparencyColor.Name = "cmbTransparencyColor";
            this.cmbTransparencyColor.Size = new System.Drawing.Size(104, 21);
            this.cmbTransparencyColor.TabIndex = 17;
            this.cmbTransparencyColor.SelectedIndexChanged += new System.EventHandler(this.cmbTransparencyColor_SelectedIndexChanged);
            // 
            // EnableHillshade
            // 
            this.EnableHillshade.Checked = true;
            this.EnableHillshade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableHillshade.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EnableHillshade.Location = new System.Drawing.Point(31, 109);
            this.EnableHillshade.Name = "EnableHillshade";
            this.EnableHillshade.Size = new System.Drawing.Size(96, 16);
            this.EnableHillshade.TabIndex = 23;
            this.EnableHillshade.Text = "Hillshade";
            this.EnableHillshade.CheckedChanged += new System.EventHandler(this.EnableHillshade_CheckedChanged);
            // 
            // HillshadeGroup
            // 
            this.HillshadeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HillshadeGroup.Controls.Add(this.txtHillshadeBand);
            this.HillshadeGroup.Controls.Add(this.txtHillshadeScaleFactor);
            this.HillshadeGroup.Controls.Add(this.txtHillshadeAzimuth);
            this.HillshadeGroup.Controls.Add(this.txtHillshadeAltitude);
            this.HillshadeGroup.Controls.Add(this.label10);
            this.HillshadeGroup.Controls.Add(this.label8);
            this.HillshadeGroup.Controls.Add(this.label9);
            this.HillshadeGroup.Controls.Add(this.label11);
            this.HillshadeGroup.Location = new System.Drawing.Point(15, 109);
            this.HillshadeGroup.Name = "HillshadeGroup";
            this.HillshadeGroup.Size = new System.Drawing.Size(419, 120);
            this.HillshadeGroup.TabIndex = 22;
            this.HillshadeGroup.TabStop = false;
            // 
            // txtHillshadeBand
            // 
            this.txtHillshadeBand.Location = new System.Drawing.Point(144, 69);
            this.txtHillshadeBand.Name = "txtHillshadeBand";
            this.txtHillshadeBand.Size = new System.Drawing.Size(104, 20);
            this.txtHillshadeBand.TabIndex = 10;
            this.txtHillshadeBand.TextChanged += new System.EventHandler(this.txtHillshadeBand_TextChanged);
            // 
            // txtHillshadeScaleFactor
            // 
            this.txtHillshadeScaleFactor.Location = new System.Drawing.Point(144, 93);
            this.txtHillshadeScaleFactor.Name = "txtHillshadeScaleFactor";
            this.txtHillshadeScaleFactor.Size = new System.Drawing.Size(104, 20);
            this.txtHillshadeScaleFactor.TabIndex = 9;
            this.txtHillshadeScaleFactor.Text = "0";
            this.txtHillshadeScaleFactor.TextChanged += new System.EventHandler(this.txtHillshadeScaleFactor_TextChanged);
            // 
            // txtHillshadeAzimuth
            // 
            this.txtHillshadeAzimuth.Location = new System.Drawing.Point(144, 45);
            this.txtHillshadeAzimuth.Name = "txtHillshadeAzimuth";
            this.txtHillshadeAzimuth.Size = new System.Drawing.Size(104, 20);
            this.txtHillshadeAzimuth.TabIndex = 8;
            this.txtHillshadeAzimuth.Text = "0";
            this.txtHillshadeAzimuth.TextChanged += new System.EventHandler(this.txtHillshadeAzimuth_TextChanged);
            // 
            // txtHillshadeAltitude
            // 
            this.txtHillshadeAltitude.Location = new System.Drawing.Point(144, 21);
            this.txtHillshadeAltitude.Name = "txtHillshadeAltitude";
            this.txtHillshadeAltitude.Size = new System.Drawing.Size(104, 20);
            this.txtHillshadeAltitude.TabIndex = 7;
            this.txtHillshadeAltitude.Text = "0";
            this.txtHillshadeAltitude.TextChanged += new System.EventHandler(this.txtHillshadeAltitude_TextChanged);
            // 
            // label10
            // 
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(16, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 16);
            this.label10.TabIndex = 3;
            this.label10.Text = "Altitude";
            // 
            // label8
            // 
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(16, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "Band";
            // 
            // label9
            // 
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(16, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "Azimuth";
            // 
            // label11
            // 
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(16, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 16);
            this.label11.TabIndex = 6;
            this.label11.Text = "Scale factor";
            // 
            // EnableSurface
            // 
            this.EnableSurface.Checked = true;
            this.EnableSurface.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableSurface.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EnableSurface.Location = new System.Drawing.Point(31, 235);
            this.EnableSurface.Name = "EnableSurface";
            this.EnableSurface.Size = new System.Drawing.Size(88, 16);
            this.EnableSurface.TabIndex = 25;
            this.EnableSurface.Text = "Surface";
            this.EnableSurface.CheckedChanged += new System.EventHandler(this.EnableSurface_CheckedChanged);
            // 
            // SurfaceGroup
            // 
            this.SurfaceGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SurfaceGroup.Controls.Add(this.cmbSurfaceDefaultColor);
            this.SurfaceGroup.Controls.Add(this.txtSurfaceBand);
            this.SurfaceGroup.Controls.Add(this.txtSurfaceScaleFactor);
            this.SurfaceGroup.Controls.Add(this.label12);
            this.SurfaceGroup.Controls.Add(this.label13);
            this.SurfaceGroup.Controls.Add(this.label14);
            this.SurfaceGroup.Controls.Add(this.label15);
            this.SurfaceGroup.Controls.Add(this.txtSurfaceZeroValue);
            this.SurfaceGroup.Location = new System.Drawing.Point(15, 235);
            this.SurfaceGroup.Name = "SurfaceGroup";
            this.SurfaceGroup.Size = new System.Drawing.Size(419, 120);
            this.SurfaceGroup.TabIndex = 24;
            this.SurfaceGroup.TabStop = false;
            // 
            // cmbSurfaceDefaultColor
            // 
            this.cmbSurfaceDefaultColor.FormattingEnabled = true;
            this.cmbSurfaceDefaultColor.Location = new System.Drawing.Point(144, 69);
            this.cmbSurfaceDefaultColor.Name = "cmbSurfaceDefaultColor";
            this.cmbSurfaceDefaultColor.Size = new System.Drawing.Size(104, 21);
            this.cmbSurfaceDefaultColor.TabIndex = 13;
            this.cmbSurfaceDefaultColor.SelectedIndexChanged += new System.EventHandler(this.cmbSurfaceDefaultColor_SelectedIndexChanged);
            // 
            // txtSurfaceBand
            // 
            this.txtSurfaceBand.Location = new System.Drawing.Point(144, 21);
            this.txtSurfaceBand.Name = "txtSurfaceBand";
            this.txtSurfaceBand.Size = new System.Drawing.Size(104, 20);
            this.txtSurfaceBand.TabIndex = 12;
            this.txtSurfaceBand.TextChanged += new System.EventHandler(this.txtSurfaceBand_TextChanged);
            // 
            // txtSurfaceScaleFactor
            // 
            this.txtSurfaceScaleFactor.Location = new System.Drawing.Point(144, 93);
            this.txtSurfaceScaleFactor.Name = "txtSurfaceScaleFactor";
            this.txtSurfaceScaleFactor.Size = new System.Drawing.Size(104, 20);
            this.txtSurfaceScaleFactor.TabIndex = 11;
            this.txtSurfaceScaleFactor.Text = "0";
            this.txtSurfaceScaleFactor.TextChanged += new System.EventHandler(this.txtSurfaceScaleFactor_TextChanged);
            // 
            // label12
            // 
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(16, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "Band";
            // 
            // label13
            // 
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(16, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(112, 16);
            this.label13.TabIndex = 5;
            this.label13.Text = "Default color";
            // 
            // label14
            // 
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(16, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 16);
            this.label14.TabIndex = 4;
            this.label14.Text = "Zero value";
            // 
            // label15
            // 
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(16, 96);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 16);
            this.label15.TabIndex = 6;
            this.label15.Text = "Scale factor";
            // 
            // txtSurfaceZeroValue
            // 
            this.txtSurfaceZeroValue.Location = new System.Drawing.Point(144, 45);
            this.txtSurfaceZeroValue.Name = "txtSurfaceZeroValue";
            this.txtSurfaceZeroValue.Size = new System.Drawing.Size(104, 20);
            this.txtSurfaceZeroValue.TabIndex = 10;
            this.txtSurfaceZeroValue.Text = "0";
            this.txtSurfaceZeroValue.TextChanged += new System.EventHandler(this.txtSurfaceZeroValue_TextChanged);
            // 
            // RasterLayerAdvancedSectionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "Advanced Settings";
            this.Name = "RasterLayerAdvancedSectionCtrl";
            this.Size = new System.Drawing.Size(449, 391);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.HillshadeGroup.ResumeLayout(false);
            this.HillshadeGroup.PerformLayout();
            this.SurfaceGroup.ResumeLayout(false);
            this.SurfaceGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtContrastFactor;
        private System.Windows.Forms.TextBox txtBrightnessFactor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkAdvanced;
        private System.Windows.Forms.CheckBox EnableSurface;
        private System.Windows.Forms.GroupBox SurfaceGroup;
        private System.Windows.Forms.TextBox txtSurfaceBand;
        private System.Windows.Forms.TextBox txtSurfaceScaleFactor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSurfaceZeroValue;
        private System.Windows.Forms.CheckBox EnableHillshade;
        private System.Windows.Forms.GroupBox HillshadeGroup;
        private System.Windows.Forms.TextBox txtHillshadeBand;
        private System.Windows.Forms.TextBox txtHillshadeScaleFactor;
        private System.Windows.Forms.TextBox txtHillshadeAzimuth;
        private System.Windows.Forms.TextBox txtHillshadeAltitude;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private Maestro.Editors.Common.ColorComboBox cmbTransparencyColor;
        private Maestro.Editors.Common.ColorComboBox cmbSurfaceDefaultColor;
    }
}
