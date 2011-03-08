namespace MaestroFsPreview
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fsPanel = new System.Windows.Forms.Panel();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.localFsPreviewCtrl = new Maestro.Editors.FeatureSource.Preview.LocalFeatureSourcePreviewCtrl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.fsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fsPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.localFsPreviewCtrl);
            // 
            // fsPanel
            // 
            this.fsPanel.Controls.Add(this.btnBrowse);
            this.fsPanel.Controls.Add(this.txtFeatureSource);
            this.fsPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.fsPanel, "fsPanel");
            this.fsPanel.Name = "fsPanel";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
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
            // localFsPreviewCtrl
            // 
            resources.ApplyResources(this.localFsPreviewCtrl, "localFsPreviewCtrl");
            this.localFsPreviewCtrl.Name = "localFsPreviewCtrl";
            this.localFsPreviewCtrl.SupportsSQL = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.fsPanel.ResumeLayout(false);
            this.fsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel fsPanel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Label label1;
        private Maestro.Editors.FeatureSource.Preview.LocalFeatureSourcePreviewCtrl localFsPreviewCtrl;

    }
}

