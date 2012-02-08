namespace Maestro.Editors.LayerDefinition.Raster
{
    partial class RasterLayerSettingsSectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterLayerSettingsSectionCtrl));
            this.grpFeatureClass = new System.Windows.Forms.GroupBox();
            this.btnBrowseGeometry = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseSchema = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGeometry = new System.Windows.Forms.TextBox();
            this.txtFeatureClass = new System.Windows.Forms.TextBox();
            this.btnBrowseFeatureSource = new System.Windows.Forms.Button();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGoToFeatureSource = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.grpFeatureClass.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnGoToFeatureSource);
            this.contentPanel.Controls.Add(this.grpFeatureClass);
            this.contentPanel.Controls.Add(this.txtFeatureSource);
            this.contentPanel.Controls.Add(this.btnBrowseFeatureSource);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // grpFeatureClass
            // 
            resources.ApplyResources(this.grpFeatureClass, "grpFeatureClass");
            this.grpFeatureClass.Controls.Add(this.btnBrowseGeometry);
            this.grpFeatureClass.Controls.Add(this.label3);
            this.grpFeatureClass.Controls.Add(this.btnBrowseSchema);
            this.grpFeatureClass.Controls.Add(this.label2);
            this.grpFeatureClass.Controls.Add(this.txtGeometry);
            this.grpFeatureClass.Controls.Add(this.txtFeatureClass);
            this.grpFeatureClass.Name = "grpFeatureClass";
            this.grpFeatureClass.TabStop = false;
            // 
            // btnBrowseGeometry
            // 
            resources.ApplyResources(this.btnBrowseGeometry, "btnBrowseGeometry");
            this.btnBrowseGeometry.Name = "btnBrowseGeometry";
            this.btnBrowseGeometry.UseVisualStyleBackColor = true;
            this.btnBrowseGeometry.Click += new System.EventHandler(this.btnBrowseGeometry_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnBrowseSchema
            // 
            resources.ApplyResources(this.btnBrowseSchema, "btnBrowseSchema");
            this.btnBrowseSchema.Name = "btnBrowseSchema";
            this.btnBrowseSchema.UseVisualStyleBackColor = true;
            this.btnBrowseSchema.Click += new System.EventHandler(this.btnBrowseSchema_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtGeometry
            // 
            resources.ApplyResources(this.txtGeometry, "txtGeometry");
            this.txtGeometry.Name = "txtGeometry";
            this.txtGeometry.ReadOnly = true;
            // 
            // txtFeatureClass
            // 
            resources.ApplyResources(this.txtFeatureClass, "txtFeatureClass");
            this.txtFeatureClass.Name = "txtFeatureClass";
            this.txtFeatureClass.ReadOnly = true;
            // 
            // btnBrowseFeatureSource
            // 
            resources.ApplyResources(this.btnBrowseFeatureSource, "btnBrowseFeatureSource");
            this.btnBrowseFeatureSource.Name = "btnBrowseFeatureSource";
            this.btnBrowseFeatureSource.UseVisualStyleBackColor = true;
            this.btnBrowseFeatureSource.Click += new System.EventHandler(this.btnBrowseFeatureSource_Click);
            // 
            // txtFeatureSource
            // 
            resources.ApplyResources(this.txtFeatureSource, "txtFeatureSource");
            this.txtFeatureSource.Name = "txtFeatureSource";
            this.txtFeatureSource.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnGoToFeatureSource
            // 
            resources.ApplyResources(this.btnGoToFeatureSource, "btnGoToFeatureSource");
            this.btnGoToFeatureSource.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.btnGoToFeatureSource.Name = "btnGoToFeatureSource";
            this.btnGoToFeatureSource.UseVisualStyleBackColor = true;
            this.btnGoToFeatureSource.Click += new System.EventHandler(this.btnGoToFeatureSource_Click);
            // 
            // RasterLayerSettingsSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "RasterLayerSettingsSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.grpFeatureClass.ResumeLayout(false);
            this.grpFeatureClass.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFeatureClass;
        private System.Windows.Forms.Button btnBrowseGeometry;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseSchema;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGeometry;
        private System.Windows.Forms.TextBox txtFeatureClass;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Button btnBrowseFeatureSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGoToFeatureSource;
    }
}
