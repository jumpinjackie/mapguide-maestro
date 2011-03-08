namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    partial class DSNCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DSNCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.lstDSN = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lstDSN
            // 
            resources.ApplyResources(this.lstDSN, "lstDSN");
            this.lstDSN.FormattingEnabled = true;
            this.lstDSN.Name = "lstDSN";
            this.lstDSN.SelectedIndexChanged += new System.EventHandler(this.lstDSN_SelectedIndexChanged);
            // 
            // DSNCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstDSN);
            this.Controls.Add(this.label1);
            this.Name = "DSNCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstDSN;
    }
}
