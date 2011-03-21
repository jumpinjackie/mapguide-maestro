namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    partial class RasterDefinitionCtrl
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
            this.label6 = new System.Windows.Forms.Label();
            this.txtElevation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTransparent = new System.Windows.Forms.CheckBox();
            this.txtImageFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBackground = new Maestro.Editors.Common.ColorComboBox();
            this.txtLayers = new System.Windows.Forms.TextBox();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtEpsg = new System.Windows.Forms.TextBox();
            this.btnSelectCs = new System.Windows.Forms.Button();
            this.btnSelectFormat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Layer CS";
            // 
            // txtElevation
            // 
            this.txtElevation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtElevation.Location = new System.Drawing.Point(108, 110);
            this.txtElevation.Name = "txtElevation";
            this.txtElevation.Size = new System.Drawing.Size(155, 20);
            this.txtElevation.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Elevation";
            // 
            // txtTime
            // 
            this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTime.Location = new System.Drawing.Point(108, 84);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(155, 20);
            this.txtTime.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Background Color";
            // 
            // chkTransparent
            // 
            this.chkTransparent.AutoSize = true;
            this.chkTransparent.Location = new System.Drawing.Point(108, 29);
            this.chkTransparent.Name = "chkTransparent";
            this.chkTransparent.Size = new System.Drawing.Size(83, 17);
            this.chkTransparent.TabIndex = 14;
            this.chkTransparent.Text = "Transparent";
            this.chkTransparent.UseVisualStyleBackColor = true;
            // 
            // txtImageFormat
            // 
            this.txtImageFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageFormat.Location = new System.Drawing.Point(108, 3);
            this.txtImageFormat.Name = "txtImageFormat";
            this.txtImageFormat.ReadOnly = true;
            this.txtImageFormat.Size = new System.Drawing.Size(119, 20);
            this.txtImageFormat.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Image Format";
            // 
            // cmbBackground
            // 
            this.cmbBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBackground.FormattingEnabled = true;
            this.cmbBackground.Location = new System.Drawing.Point(108, 57);
            this.cmbBackground.Name = "cmbBackground";
            this.cmbBackground.Size = new System.Drawing.Size(155, 21);
            this.cmbBackground.TabIndex = 16;
            // 
            // txtLayers
            // 
            this.txtLayers.AcceptsReturn = true;
            this.txtLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLayers.Location = new System.Drawing.Point(3, 16);
            this.txtLayers.Multiline = true;
            this.txtLayers.Name = "txtLayers";
            this.txtLayers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLayers.Size = new System.Drawing.Size(249, 87);
            this.txtLayers.TabIndex = 0;
            this.txtLayers.TextChanged += new System.EventHandler(this.txtLayers_TextChanged);
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.Enabled = false;
            this.lnkUpdate.Location = new System.Drawing.Point(170, 0);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(42, 13);
            this.lnkUpdate.TabIndex = 1;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update";
            this.lnkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdate_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lnkUpdate);
            this.groupBox1.Controls.Add(this.txtLayers);
            this.groupBox1.Location = new System.Drawing.Point(8, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 106);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layers (one line per layer name)";
            // 
            // txtEpsg
            // 
            this.txtEpsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEpsg.Location = new System.Drawing.Point(108, 136);
            this.txtEpsg.Name = "txtEpsg";
            this.txtEpsg.ReadOnly = true;
            this.txtEpsg.Size = new System.Drawing.Size(119, 20);
            this.txtEpsg.TabIndex = 25;
            // 
            // btnSelectCs
            // 
            this.btnSelectCs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectCs.Location = new System.Drawing.Point(233, 134);
            this.btnSelectCs.Name = "btnSelectCs";
            this.btnSelectCs.Size = new System.Drawing.Size(30, 23);
            this.btnSelectCs.TabIndex = 26;
            this.btnSelectCs.Text = "...";
            this.btnSelectCs.UseVisualStyleBackColor = true;
            this.btnSelectCs.Click += new System.EventHandler(this.btnSelectCs_Click);
            // 
            // btnSelectFormat
            // 
            this.btnSelectFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFormat.Location = new System.Drawing.Point(233, 1);
            this.btnSelectFormat.Name = "btnSelectFormat";
            this.btnSelectFormat.Size = new System.Drawing.Size(30, 23);
            this.btnSelectFormat.TabIndex = 27;
            this.btnSelectFormat.Text = "...";
            this.btnSelectFormat.UseVisualStyleBackColor = true;
            this.btnSelectFormat.Click += new System.EventHandler(this.btnSelectFormat_Click);
            // 
            // RasterDefinitionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSelectFormat);
            this.Controls.Add(this.btnSelectCs);
            this.Controls.Add(this.txtEpsg);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtElevation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbBackground);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkTransparent);
            this.Controls.Add(this.txtImageFormat);
            this.Controls.Add(this.label2);
            this.Name = "RasterDefinitionCtrl";
            this.Size = new System.Drawing.Size(269, 290);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtElevation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label4;
        private Maestro.Editors.Common.ColorComboBox cmbBackground;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkTransparent;
        private System.Windows.Forms.TextBox txtImageFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLayers;
        private System.Windows.Forms.LinkLabel lnkUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtEpsg;
        private System.Windows.Forms.Button btnSelectCs;
        private System.Windows.Forms.Button btnSelectFormat;
    }
}
