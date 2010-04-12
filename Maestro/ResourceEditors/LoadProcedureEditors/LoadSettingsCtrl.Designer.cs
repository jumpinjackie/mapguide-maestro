namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class LoadSettingsCtrl
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLayerFolderName = new System.Windows.Forms.TextBox();
            this.txtFeatureSourceFolderName = new System.Windows.Forms.TextBox();
            this.btnBrowseLayerRoot = new System.Windows.Forms.Button();
            this.txtLayerRoot = new System.Windows.Forms.TextBox();
            this.btnBrowseFsRoot = new System.Windows.Forms.Button();
            this.txtFeatureSourceRoot = new System.Windows.Forms.TextBox();
            this.chkLoadFeatureSources = new System.Windows.Forms.CheckBox();
            this.chkLoadLayers = new System.Windows.Forms.CheckBox();
            this.btnBrowseResourceRoot = new System.Windows.Forms.Button();
            this.txtRootPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(375, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Folder Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(142, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "In this path";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Create";
            // 
            // txtLayerFolderName
            // 
            this.txtLayerFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLayerFolderName.Location = new System.Drawing.Point(378, 73);
            this.txtLayerFolderName.Name = "txtLayerFolderName";
            this.txtLayerFolderName.Size = new System.Drawing.Size(107, 20);
            this.txtLayerFolderName.TabIndex = 29;
            // 
            // txtFeatureSourceFolderName
            // 
            this.txtFeatureSourceFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeatureSourceFolderName.Location = new System.Drawing.Point(378, 50);
            this.txtFeatureSourceFolderName.Name = "txtFeatureSourceFolderName";
            this.txtFeatureSourceFolderName.Size = new System.Drawing.Size(107, 20);
            this.txtFeatureSourceFolderName.TabIndex = 28;
            // 
            // btnBrowseLayerRoot
            // 
            this.btnBrowseLayerRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLayerRoot.Location = new System.Drawing.Point(346, 71);
            this.btnBrowseLayerRoot.Name = "btnBrowseLayerRoot";
            this.btnBrowseLayerRoot.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseLayerRoot.TabIndex = 27;
            this.btnBrowseLayerRoot.Text = "...";
            this.btnBrowseLayerRoot.UseVisualStyleBackColor = true;
            this.btnBrowseLayerRoot.Click += new System.EventHandler(this.btnBrowseLayerRoot_Click);
            // 
            // txtLayerRoot
            // 
            this.txtLayerRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLayerRoot.Location = new System.Drawing.Point(145, 73);
            this.txtLayerRoot.Name = "txtLayerRoot";
            this.txtLayerRoot.ReadOnly = true;
            this.txtLayerRoot.Size = new System.Drawing.Size(195, 20);
            this.txtLayerRoot.TabIndex = 26;
            // 
            // btnBrowseFolderRoot
            // 
            this.btnBrowseFsRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFsRoot.Location = new System.Drawing.Point(346, 48);
            this.btnBrowseFsRoot.Name = "btnBrowseFolderRoot";
            this.btnBrowseFsRoot.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseFsRoot.TabIndex = 25;
            this.btnBrowseFsRoot.Text = "...";
            this.btnBrowseFsRoot.UseVisualStyleBackColor = true;
            this.btnBrowseFsRoot.Click += new System.EventHandler(this.btnBrowseFolderRoot_Click);
            // 
            // txtFeatureSourceRoot
            // 
            this.txtFeatureSourceRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeatureSourceRoot.Location = new System.Drawing.Point(145, 50);
            this.txtFeatureSourceRoot.Name = "txtFeatureSourceRoot";
            this.txtFeatureSourceRoot.ReadOnly = true;
            this.txtFeatureSourceRoot.Size = new System.Drawing.Size(195, 20);
            this.txtFeatureSourceRoot.TabIndex = 24;
            // 
            // chkLoadFeatureSources
            // 
            this.chkLoadFeatureSources.AutoSize = true;
            this.chkLoadFeatureSources.Location = new System.Drawing.Point(4, 52);
            this.chkLoadFeatureSources.Name = "chkLoadFeatureSources";
            this.chkLoadFeatureSources.Size = new System.Drawing.Size(104, 17);
            this.chkLoadFeatureSources.TabIndex = 23;
            this.chkLoadFeatureSources.Text = "Feature Sources";
            this.chkLoadFeatureSources.UseVisualStyleBackColor = true;
            this.chkLoadFeatureSources.CheckedChanged += new System.EventHandler(this.chkLoadFeatureSources_CheckedChanged);
            // 
            // chkLoadLayers
            // 
            this.chkLoadLayers.AutoSize = true;
            this.chkLoadLayers.Location = new System.Drawing.Point(4, 75);
            this.chkLoadLayers.Name = "chkLoadLayers";
            this.chkLoadLayers.Size = new System.Drawing.Size(104, 17);
            this.chkLoadLayers.TabIndex = 22;
            this.chkLoadLayers.Text = "Layer Definitions";
            this.chkLoadLayers.UseVisualStyleBackColor = true;
            this.chkLoadLayers.CheckedChanged += new System.EventHandler(this.chkLoadLayers_CheckedChanged);
            // 
            // btnBrowseResourceRoot
            // 
            this.btnBrowseResourceRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseResourceRoot.Location = new System.Drawing.Point(459, 0);
            this.btnBrowseResourceRoot.Name = "btnBrowseResourceRoot";
            this.btnBrowseResourceRoot.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseResourceRoot.TabIndex = 21;
            this.btnBrowseResourceRoot.Text = "...";
            this.btnBrowseResourceRoot.UseVisualStyleBackColor = true;
            this.btnBrowseResourceRoot.Click += new System.EventHandler(this.btnBrowseResourceRoot_Click);
            // 
            // txtRootPath
            // 
            this.txtRootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootPath.Location = new System.Drawing.Point(145, 2);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.ReadOnly = true;
            this.txtRootPath.Size = new System.Drawing.Size(308, 20);
            this.txtRootPath.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Load Resources Into Folder";
            // 
            // LoadSettingsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLayerFolderName);
            this.Controls.Add(this.txtFeatureSourceFolderName);
            this.Controls.Add(this.btnBrowseLayerRoot);
            this.Controls.Add(this.txtLayerRoot);
            this.Controls.Add(this.btnBrowseFsRoot);
            this.Controls.Add(this.txtFeatureSourceRoot);
            this.Controls.Add(this.chkLoadFeatureSources);
            this.Controls.Add(this.chkLoadLayers);
            this.Controls.Add(this.btnBrowseResourceRoot);
            this.Controls.Add(this.txtRootPath);
            this.Controls.Add(this.label3);
            this.Name = "LoadSettingsCtrl";
            this.Size = new System.Drawing.Size(488, 96);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label label6;
        protected System.Windows.Forms.Label label5;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.TextBox txtLayerFolderName;
        protected System.Windows.Forms.TextBox txtFeatureSourceFolderName;
        protected System.Windows.Forms.Button btnBrowseLayerRoot;
        protected System.Windows.Forms.TextBox txtLayerRoot;
        protected System.Windows.Forms.Button btnBrowseFsRoot;
        protected System.Windows.Forms.TextBox txtFeatureSourceRoot;
        protected System.Windows.Forms.CheckBox chkLoadFeatureSources;
        protected System.Windows.Forms.CheckBox chkLoadLayers;
        protected System.Windows.Forms.Button btnBrowseResourceRoot;
        protected System.Windows.Forms.TextBox txtRootPath;
        protected System.Windows.Forms.Label label3;
    }
}
