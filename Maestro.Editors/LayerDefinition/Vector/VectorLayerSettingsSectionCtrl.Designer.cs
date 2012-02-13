namespace Maestro.Editors.LayerDefinition.Vector
{
    partial class VectorLayerSettingsSectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorLayerSettingsSectionCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.btnBrowseFeatureSource = new System.Windows.Forms.Button();
            this.grpFeatureClass = new System.Windows.Forms.GroupBox();
            this.btnBrowseGeometry = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseSchema = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGeometry = new System.Windows.Forms.TextBox();
            this.txtFeatureClass = new System.Windows.Forms.TextBox();
            this.grpLayerSettings = new System.Windows.Forms.GroupBox();
            this.btnEditTooltip = new System.Windows.Forms.Button();
            this.btnEditHyperlink = new System.Windows.Forms.Button();
            this.btnEditFilter = new System.Windows.Forms.Button();
            this.txtTooltip = new System.Windows.Forms.TextBox();
            this.txtHyperlink = new System.Windows.Forms.TextBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGoToFeatureSource = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.grpFeatureClass.SuspendLayout();
            this.grpLayerSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnGoToFeatureSource);
            this.contentPanel.Controls.Add(this.grpLayerSettings);
            this.contentPanel.Controls.Add(this.grpFeatureClass);
            this.contentPanel.Controls.Add(this.btnBrowseFeatureSource);
            this.contentPanel.Controls.Add(this.txtFeatureSource);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtFeatureSource
            // 
            resources.ApplyResources(this.txtFeatureSource, "txtFeatureSource");
            this.txtFeatureSource.Name = "txtFeatureSource";
            this.txtFeatureSource.ReadOnly = true;
            this.txtFeatureSource.TextChanged += new System.EventHandler(this.txtFeatureSource_TextChanged);
            // 
            // btnBrowseFeatureSource
            // 
            resources.ApplyResources(this.btnBrowseFeatureSource, "btnBrowseFeatureSource");
            this.btnBrowseFeatureSource.Name = "btnBrowseFeatureSource";
            this.btnBrowseFeatureSource.UseVisualStyleBackColor = true;
            this.btnBrowseFeatureSource.Click += new System.EventHandler(this.btnBrowseFeatureSource_Click);
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
            // grpLayerSettings
            // 
            resources.ApplyResources(this.grpLayerSettings, "grpLayerSettings");
            this.grpLayerSettings.Controls.Add(this.btnEditTooltip);
            this.grpLayerSettings.Controls.Add(this.btnEditHyperlink);
            this.grpLayerSettings.Controls.Add(this.btnEditFilter);
            this.grpLayerSettings.Controls.Add(this.txtTooltip);
            this.grpLayerSettings.Controls.Add(this.txtHyperlink);
            this.grpLayerSettings.Controls.Add(this.txtFilter);
            this.grpLayerSettings.Controls.Add(this.label6);
            this.grpLayerSettings.Controls.Add(this.label5);
            this.grpLayerSettings.Controls.Add(this.label4);
            this.grpLayerSettings.Name = "grpLayerSettings";
            this.grpLayerSettings.TabStop = false;
            // 
            // btnEditTooltip
            // 
            resources.ApplyResources(this.btnEditTooltip, "btnEditTooltip");
            this.btnEditTooltip.Name = "btnEditTooltip";
            this.btnEditTooltip.UseVisualStyleBackColor = true;
            this.btnEditTooltip.Click += new System.EventHandler(this.btnEditTooltip_Click);
            // 
            // btnEditHyperlink
            // 
            resources.ApplyResources(this.btnEditHyperlink, "btnEditHyperlink");
            this.btnEditHyperlink.Name = "btnEditHyperlink";
            this.btnEditHyperlink.UseVisualStyleBackColor = true;
            this.btnEditHyperlink.Click += new System.EventHandler(this.btnEditHyperlink_Click);
            // 
            // btnEditFilter
            // 
            resources.ApplyResources(this.btnEditFilter, "btnEditFilter");
            this.btnEditFilter.Name = "btnEditFilter";
            this.btnEditFilter.UseVisualStyleBackColor = true;
            this.btnEditFilter.Click += new System.EventHandler(this.btnEditFilter_Click);
            // 
            // txtTooltip
            // 
            resources.ApplyResources(this.txtTooltip, "txtTooltip");
            this.txtTooltip.Name = "txtTooltip";
            this.txtTooltip.TextChanged += new System.EventHandler(this.txtTooltip_TextChanged);
            // 
            // txtHyperlink
            // 
            resources.ApplyResources(this.txtHyperlink, "txtHyperlink");
            this.txtHyperlink.Name = "txtHyperlink";
            this.txtHyperlink.TextChanged += new System.EventHandler(this.txtHyperlink_TextChanged);
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnGoToFeatureSource
            // 
            resources.ApplyResources(this.btnGoToFeatureSource, "btnGoToFeatureSource");
            this.btnGoToFeatureSource.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.btnGoToFeatureSource.Name = "btnGoToFeatureSource";
            this.btnGoToFeatureSource.UseVisualStyleBackColor = true;
            this.btnGoToFeatureSource.Click += new System.EventHandler(this.btnGoToFeatureSource_Click);
            // 
            // VectorLayerSettingsSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "VectorLayerSettingsSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.grpFeatureClass.ResumeLayout(false);
            this.grpFeatureClass.PerformLayout();
            this.grpLayerSettings.ResumeLayout(false);
            this.grpLayerSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseFeatureSource;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpFeatureClass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpLayerSettings;
        private System.Windows.Forms.TextBox txtTooltip;
        private System.Windows.Forms.TextBox txtHyperlink;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEditTooltip;
        private System.Windows.Forms.Button btnEditHyperlink;
        private System.Windows.Forms.Button btnEditFilter;
        private System.Windows.Forms.Button btnBrowseGeometry;
        private System.Windows.Forms.Button btnBrowseSchema;
        private System.Windows.Forms.TextBox txtGeometry;
        private System.Windows.Forms.TextBox txtFeatureClass;
        private System.Windows.Forms.Button btnGoToFeatureSource;
    }
}
