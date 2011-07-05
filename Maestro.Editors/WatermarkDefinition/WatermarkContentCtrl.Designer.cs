namespace Maestro.Editors.WatermarkDefinition
{
    partial class WatermarkContentCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatermarkContentCtrl));
            this.rdText = new System.Windows.Forms.RadioButton();
            this.rdImage = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEditText = new System.Windows.Forms.Button();
            this.btnEditImage = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // rdText
            // 
            resources.ApplyResources(this.rdText, "rdText");
            this.rdText.Name = "rdText";
            this.rdText.TabStop = true;
            this.rdText.UseVisualStyleBackColor = true;
            this.rdText.CheckedChanged += new System.EventHandler(this.wmdTypeCheckedChanged);
            // 
            // rdImage
            // 
            resources.ApplyResources(this.rdImage, "rdImage");
            this.rdImage.Name = "rdImage";
            this.rdImage.TabStop = true;
            this.rdImage.UseVisualStyleBackColor = true;
            this.rdImage.CheckedChanged += new System.EventHandler(this.wmdTypeCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEditImage);
            this.groupBox1.Controls.Add(this.btnEditText);
            this.groupBox1.Controls.Add(this.rdText);
            this.groupBox1.Controls.Add(this.rdImage);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnEditText
            // 
            resources.ApplyResources(this.btnEditText, "btnEditText");
            this.btnEditText.Name = "btnEditText";
            this.btnEditText.UseVisualStyleBackColor = true;
            this.btnEditText.Click += new System.EventHandler(this.btnEditText_Click);
            // 
            // btnEditImage
            // 
            resources.ApplyResources(this.btnEditImage, "btnEditImage");
            this.btnEditImage.Name = "btnEditImage";
            this.btnEditImage.UseVisualStyleBackColor = true;
            this.btnEditImage.Click += new System.EventHandler(this.btnEditImage_Click);
            // 
            // WatermarkContentCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "WatermarkContentCtrl";
            this.contentPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdText;
        private System.Windows.Forms.RadioButton rdImage;
        private System.Windows.Forms.Button btnEditImage;
        private System.Windows.Forms.Button btnEditText;

    }
}
