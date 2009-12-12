namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
    partial class ColorComboWithTransparency
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
            this.transparencySlider = new System.Windows.Forms.TrackBar();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.colorCombo = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.transparencySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // transparencySlider
            // 
            this.transparencySlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.transparencySlider.Location = new System.Drawing.Point(0, 24);
            this.transparencySlider.Maximum = 255;
            this.transparencySlider.Name = "transparencySlider";
            this.transparencySlider.Size = new System.Drawing.Size(256, 45);
            this.transparencySlider.TabIndex = 1;
            this.transparencySlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.transparencySlider.ValueChanged += new System.EventHandler(this.transparencySlider_ValueChanged);
            // 
            // percentageLabel
            // 
            this.percentageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.percentageLabel.Location = new System.Drawing.Point(264, 24);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(36, 23);
            this.percentageLabel.TabIndex = 2;
            this.percentageLabel.Text = "0%";
            this.percentageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // colorCombo
            // 
            this.colorCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorCombo.FormattingEnabled = true;
            this.colorCombo.Location = new System.Drawing.Point(0, 0);
            this.colorCombo.Name = "colorCombo";
            this.colorCombo.Size = new System.Drawing.Size(305, 21);
            this.colorCombo.TabIndex = 0;
            this.colorCombo.SelectedIndexChanged += new System.EventHandler(this.colorCombo_SelectedIndexChanged);
            // 
            // ColorComboWithTransparency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.transparencySlider);
            this.Controls.Add(this.colorCombo);
            this.Name = "ColorComboWithTransparency";
            this.Size = new System.Drawing.Size(305, 49);
            ((System.ComponentModel.ISupportInitialize)(this.transparencySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorComboBox colorCombo;
        private System.Windows.Forms.TrackBar transparencySlider;
        private System.Windows.Forms.Label percentageLabel;
    }
}
