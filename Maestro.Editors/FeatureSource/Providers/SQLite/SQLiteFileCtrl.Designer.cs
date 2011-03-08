namespace Maestro.Editors.FeatureSource.Providers.SQLite
{
    partial class SQLiteFileCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLiteFileCtrl));
            this.chkUseFdoMetadata = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkUseFdoMetadata);
            this.contentPanel.Controls.SetChildIndex(this.chkUseFdoMetadata, 0);
            this.contentPanel.Controls.SetChildIndex(this.resDataCtrl, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdManaged, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdUnmanaged, 0);
            // 
            // chkUseFdoMetadata
            // 
            resources.ApplyResources(this.chkUseFdoMetadata, "chkUseFdoMetadata");
            this.chkUseFdoMetadata.Name = "chkUseFdoMetadata";
            this.chkUseFdoMetadata.UseVisualStyleBackColor = true;
            this.chkUseFdoMetadata.CheckedChanged += new System.EventHandler(this.chkUseFdoMetadata_CheckedChanged);
            // 
            // SQLiteFileCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "SQLite Feature Source";
            this.Name = "SQLiteFileCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseFdoMetadata;
    }
}
