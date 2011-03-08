namespace Maestro.Editors.LoadProcedure
{
    partial class LoadTargetCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadTargetCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkCreateFeatureSources = new System.Windows.Forms.CheckBox();
            this.chkCreateLayers = new System.Windows.Forms.CheckBox();
            this.txtFeatureSourceRoot = new System.Windows.Forms.TextBox();
            this.txtLayerRoot = new System.Windows.Forms.TextBox();
            this.txtTargetRoot = new System.Windows.Forms.TextBox();
            this.txtFeatureFolderName = new System.Windows.Forms.TextBox();
            this.txtLayerFolderName = new System.Windows.Forms.TextBox();
            this.btnBrowseRoot = new System.Windows.Forms.Button();
            this.btnBrowseLayerRoot = new System.Windows.Forms.Button();
            this.btnBrowseFeatureRoot = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnBrowseFeatureRoot);
            this.contentPanel.Controls.Add(this.btnBrowseLayerRoot);
            this.contentPanel.Controls.Add(this.btnBrowseRoot);
            this.contentPanel.Controls.Add(this.txtLayerFolderName);
            this.contentPanel.Controls.Add(this.txtFeatureFolderName);
            this.contentPanel.Controls.Add(this.txtTargetRoot);
            this.contentPanel.Controls.Add(this.txtLayerRoot);
            this.contentPanel.Controls.Add(this.txtFeatureSourceRoot);
            this.contentPanel.Controls.Add(this.chkCreateLayers);
            this.contentPanel.Controls.Add(this.chkCreateFeatureSources);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkCreateFeatureSources
            // 
            resources.ApplyResources(this.chkCreateFeatureSources, "chkCreateFeatureSources");
            this.chkCreateFeatureSources.Name = "chkCreateFeatureSources";
            this.chkCreateFeatureSources.UseVisualStyleBackColor = true;
            // 
            // chkCreateLayers
            // 
            resources.ApplyResources(this.chkCreateLayers, "chkCreateLayers");
            this.chkCreateLayers.Name = "chkCreateLayers";
            this.chkCreateLayers.UseVisualStyleBackColor = true;
            // 
            // txtFeatureSourceRoot
            // 
            resources.ApplyResources(this.txtFeatureSourceRoot, "txtFeatureSourceRoot");
            this.txtFeatureSourceRoot.Name = "txtFeatureSourceRoot";
            // 
            // txtLayerRoot
            // 
            resources.ApplyResources(this.txtLayerRoot, "txtLayerRoot");
            this.txtLayerRoot.Name = "txtLayerRoot";
            // 
            // txtTargetRoot
            // 
            resources.ApplyResources(this.txtTargetRoot, "txtTargetRoot");
            this.txtTargetRoot.Name = "txtTargetRoot";
            // 
            // txtFeatureFolderName
            // 
            resources.ApplyResources(this.txtFeatureFolderName, "txtFeatureFolderName");
            this.txtFeatureFolderName.Name = "txtFeatureFolderName";
            // 
            // txtLayerFolderName
            // 
            resources.ApplyResources(this.txtLayerFolderName, "txtLayerFolderName");
            this.txtLayerFolderName.Name = "txtLayerFolderName";
            // 
            // btnBrowseRoot
            // 
            resources.ApplyResources(this.btnBrowseRoot, "btnBrowseRoot");
            this.btnBrowseRoot.Name = "btnBrowseRoot";
            this.btnBrowseRoot.UseVisualStyleBackColor = true;
            this.btnBrowseRoot.Click += new System.EventHandler(this.btnBrowseRoot_Click);
            // 
            // btnBrowseLayerRoot
            // 
            resources.ApplyResources(this.btnBrowseLayerRoot, "btnBrowseLayerRoot");
            this.btnBrowseLayerRoot.Name = "btnBrowseLayerRoot";
            this.btnBrowseLayerRoot.UseVisualStyleBackColor = true;
            this.btnBrowseLayerRoot.Click += new System.EventHandler(this.btnBrowseLayerRoot_Click);
            // 
            // btnBrowseFeatureRoot
            // 
            resources.ApplyResources(this.btnBrowseFeatureRoot, "btnBrowseFeatureRoot");
            this.btnBrowseFeatureRoot.Name = "btnBrowseFeatureRoot";
            this.btnBrowseFeatureRoot.UseVisualStyleBackColor = true;
            this.btnBrowseFeatureRoot.Click += new System.EventHandler(this.btnBrowseFeatureRoot_Click);
            // 
            // LoadTargetCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Load Settings";
            this.Name = "LoadTargetCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseFeatureRoot;
        private System.Windows.Forms.Button btnBrowseLayerRoot;
        private System.Windows.Forms.Button btnBrowseRoot;
        private System.Windows.Forms.TextBox txtLayerFolderName;
        private System.Windows.Forms.TextBox txtFeatureFolderName;
        private System.Windows.Forms.TextBox txtTargetRoot;
        private System.Windows.Forms.TextBox txtLayerRoot;
        private System.Windows.Forms.TextBox txtFeatureSourceRoot;
        private System.Windows.Forms.CheckBox chkCreateLayers;
        private System.Windows.Forms.CheckBox chkCreateFeatureSources;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
