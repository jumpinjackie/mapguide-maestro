namespace Maestro.Editors.TileSetDefinition
{
    partial class TileSetSettingsCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.grpExtents = new System.Windows.Forms.GroupBox();
            this.btnSetZoom = new System.Windows.Forms.Button();
            this.txtMaxY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMinY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            this.grpExtents.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grpExtents);
            this.contentPanel.Controls.Add(this.txtProvider);
            this.contentPanel.Controls.Add(this.grpSettings);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Size = new System.Drawing.Size(582, 292);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Provider";
            // 
            // grpSettings
            // 
            this.grpSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSettings.Location = new System.Drawing.Point(251, 47);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(316, 227);
            this.grpSettings.TabIndex = 2;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // txtProvider
            // 
            this.txtProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProvider.Location = new System.Drawing.Point(93, 13);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(474, 20);
            this.txtProvider.TabIndex = 3;
            // 
            // grpExtents
            // 
            this.grpExtents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpExtents.Controls.Add(this.btnSetZoom);
            this.grpExtents.Controls.Add(this.txtMaxY);
            this.grpExtents.Controls.Add(this.label4);
            this.grpExtents.Controls.Add(this.txtMaxX);
            this.grpExtents.Controls.Add(this.label5);
            this.grpExtents.Controls.Add(this.txtMinY);
            this.grpExtents.Controls.Add(this.label3);
            this.grpExtents.Controls.Add(this.txtMinX);
            this.grpExtents.Controls.Add(this.label2);
            this.grpExtents.Location = new System.Drawing.Point(13, 47);
            this.grpExtents.Name = "grpExtents";
            this.grpExtents.Size = new System.Drawing.Size(232, 227);
            this.grpExtents.TabIndex = 4;
            this.grpExtents.TabStop = false;
            this.grpExtents.Text = "Extents";
            // 
            // btnSetZoom
            // 
            this.btnSetZoom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSetZoom.Location = new System.Drawing.Point(13, 125);
            this.btnSetZoom.Name = "btnSetZoom";
            this.btnSetZoom.Size = new System.Drawing.Size(206, 48);
            this.btnSetZoom.TabIndex = 11;
            this.btnSetZoom.Text = "Set view to combined extent of current layers";
            this.btnSetZoom.Click += new System.EventHandler(this.btnSetZoom_Click);
            // 
            // txtMaxY
            // 
            this.txtMaxY.Location = new System.Drawing.Point(119, 87);
            this.txtMaxY.Name = "txtMaxY";
            this.txtMaxY.Size = new System.Drawing.Size(100, 20);
            this.txtMaxY.TabIndex = 7;
            this.txtMaxY.TextChanged += new System.EventHandler(this.txtMaxY_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(116, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Upper Y";
            // 
            // txtMaxX
            // 
            this.txtMaxX.Location = new System.Drawing.Point(13, 87);
            this.txtMaxX.Name = "txtMaxX";
            this.txtMaxX.Size = new System.Drawing.Size(100, 20);
            this.txtMaxX.TabIndex = 5;
            this.txtMaxX.TextChanged += new System.EventHandler(this.txtMaxX_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Upper X";
            // 
            // txtMinY
            // 
            this.txtMinY.Location = new System.Drawing.Point(119, 43);
            this.txtMinY.Name = "txtMinY";
            this.txtMinY.Size = new System.Drawing.Size(100, 20);
            this.txtMinY.TabIndex = 3;
            this.txtMinY.TextChanged += new System.EventHandler(this.txtMinY_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Lower Y";
            // 
            // txtMinX
            // 
            this.txtMinX.Location = new System.Drawing.Point(13, 43);
            this.txtMinX.Name = "txtMinX";
            this.txtMinX.Size = new System.Drawing.Size(100, 20);
            this.txtMinX.TabIndex = 1;
            this.txtMinX.TextChanged += new System.EventHandler(this.txtMinX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lower X";
            // 
            // TileSetSettingsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "Tile Set Settings";
            this.Name = "TileSetSettingsCtrl";
            this.Size = new System.Drawing.Size(582, 319);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.grpExtents.ResumeLayout(false);
            this.grpExtents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.GroupBox grpExtents;
        private System.Windows.Forms.TextBox txtMaxY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMinY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSetZoom;
    }
}
