namespace Maestro.Base.Editor
{
    partial class FsEditorOptionPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FsEditorOptionPanel));
            this.btnLocalPreview = new System.Windows.Forms.Button();
            this.btnEditConfiguration = new System.Windows.Forms.Button();
            this.btnSpatialContexts = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnSpatialContexts);
            this.contentPanel.Controls.Add(this.btnEditConfiguration);
            this.contentPanel.Controls.Add(this.btnLocalPreview);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // btnLocalPreview
            // 
            resources.ApplyResources(this.btnLocalPreview, "btnLocalPreview");
            this.btnLocalPreview.Name = "btnLocalPreview";
            this.btnLocalPreview.UseVisualStyleBackColor = true;
            this.btnLocalPreview.Click += new System.EventHandler(this.btnLocalPreview_Click);
            // 
            // btnEditConfiguration
            // 
            resources.ApplyResources(this.btnEditConfiguration, "btnEditConfiguration");
            this.btnEditConfiguration.Name = "btnEditConfiguration";
            this.btnEditConfiguration.UseVisualStyleBackColor = true;
            this.btnEditConfiguration.Click += new System.EventHandler(this.btnEditConfiguration_Click);
            // 
            // btnSpatialContexts
            // 
            resources.ApplyResources(this.btnSpatialContexts, "btnSpatialContexts");
            this.btnSpatialContexts.Name = "btnSpatialContexts";
            this.btnSpatialContexts.UseVisualStyleBackColor = true;
            this.btnSpatialContexts.Click += new System.EventHandler(this.btnSpatialContexts_Click);
            // 
            // FsEditorOptionPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Other Options";
            this.Name = "FsEditorOptionPanel";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEditConfiguration;
        private System.Windows.Forms.Button btnLocalPreview;
        private System.Windows.Forms.Button btnSpatialContexts;
    }
}
