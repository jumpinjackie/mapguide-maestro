namespace Maestro.Editors.WatermarkDefinition
{
    partial class WatermarkSettingsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatermarkSettingsCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpPositionSettings = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numRotation = new System.Windows.Forms.NumericUpDown();
            this.numTransparency = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.rdTile = new System.Windows.Forms.RadioButton();
            this.rdXY = new System.Windows.Forms.RadioButton();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grpPositionSettings);
            this.contentPanel.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.rdXY);
            this.groupBox1.Controls.Add(this.rdTile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numTransparency);
            this.groupBox1.Controls.Add(this.numRotation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // grpPositionSettings
            // 
            resources.ApplyResources(this.grpPositionSettings, "grpPositionSettings");
            this.grpPositionSettings.Name = "grpPositionSettings";
            this.grpPositionSettings.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numRotation
            // 
            this.numRotation.DecimalPlaces = 5;
            resources.ApplyResources(this.numRotation, "numRotation");
            this.numRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numRotation.Name = "numRotation";
            // 
            // numTransparency
            // 
            this.numTransparency.DecimalPlaces = 5;
            resources.ApplyResources(this.numTransparency, "numTransparency");
            this.numTransparency.Name = "numTransparency";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // rdTile
            // 
            resources.ApplyResources(this.rdTile, "rdTile");
            this.rdTile.Name = "rdTile";
            this.rdTile.TabStop = true;
            this.rdTile.UseVisualStyleBackColor = true;
            this.rdTile.CheckedChanged += new System.EventHandler(this.OnPositionCheckChanged);
            // 
            // rdXY
            // 
            resources.ApplyResources(this.rdXY, "rdXY");
            this.rdXY.Name = "rdXY";
            this.rdXY.TabStop = true;
            this.rdXY.UseVisualStyleBackColor = true;
            this.rdXY.CheckedChanged += new System.EventHandler(this.OnPositionCheckChanged);
            // 
            // WatermarkSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "WatermarkSettingsCtrl";
            this.contentPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransparency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPositionSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdXY;
        private System.Windows.Forms.RadioButton rdTile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numTransparency;
        private System.Windows.Forms.NumericUpDown numRotation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
