namespace Maestro.Editors.FeatureSource
{
    partial class SpatialContextsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpatialContextsDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.grdSpatialContexts = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFeatureSource = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpatialContexts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdSpatialContexts
            // 
            this.grdSpatialContexts.AllowUserToAddRows = false;
            this.grdSpatialContexts.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdSpatialContexts, "grdSpatialContexts");
            this.grdSpatialContexts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSpatialContexts.Name = "grdSpatialContexts";
            this.grdSpatialContexts.ReadOnly = true;
            // 
            // lblCount
            // 
            resources.ApplyResources(this.lblCount, "lblCount");
            this.lblCount.Name = "lblCount";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblFeatureSource
            // 
            resources.ApplyResources(this.lblFeatureSource, "lblFeatureSource");
            this.lblFeatureSource.Name = "lblFeatureSource";
            // 
            // SpatialContextsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.lblFeatureSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.grdSpatialContexts);
            this.Controls.Add(this.btnClose);
            this.Name = "SpatialContextsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.grdSpatialContexts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView grdSpatialContexts;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFeatureSource;
    }
}