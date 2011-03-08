namespace Maestro.Editors.Common
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorComboWithTransparency));
            this.transparencySlider = new System.Windows.Forms.TrackBar();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.colorCombo = new Maestro.Editors.Common.ColorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.transparencySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // transparencySlider
            // 
            resources.ApplyResources(this.transparencySlider, "transparencySlider");
            this.transparencySlider.Maximum = 255;
            this.transparencySlider.Name = "transparencySlider";
            this.transparencySlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.transparencySlider.ValueChanged += new System.EventHandler(this.transparencySlider_ValueChanged);
            // 
            // percentageLabel
            // 
            resources.ApplyResources(this.percentageLabel, "percentageLabel");
            this.percentageLabel.Name = "percentageLabel";
            // 
            // colorCombo
            // 
            resources.ApplyResources(this.colorCombo, "colorCombo");
            this.colorCombo.FormattingEnabled = true;
            this.colorCombo.Name = "colorCombo";
            this.colorCombo.SelectedIndexChanged += new System.EventHandler(this.colorCombo_SelectedIndexChanged);
            // 
            // ColorComboWithTransparency
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.transparencySlider);
            this.Controls.Add(this.colorCombo);
            this.Name = "ColorComboWithTransparency";
            resources.ApplyResources(this, "$this");
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
