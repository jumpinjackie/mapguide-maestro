namespace Maestro.Editors.FeatureSource.Providers.Sdf
{
    partial class SdfFileCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdfFileCtrl));
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkReadOnly);
            this.contentPanel.Controls.SetChildIndex(this.resDataCtrl, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdManaged, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdUnmanaged, 0);
            this.contentPanel.Controls.SetChildIndex(this.chkReadOnly, 0);
            // 
            // chkReadOnly
            // 
            resources.ApplyResources(this.chkReadOnly, "chkReadOnly");
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.UseVisualStyleBackColor = true;
            this.chkReadOnly.CheckedChanged += new System.EventHandler(this.chkReadOnly_CheckedChanged);
            // 
            // SdfFileCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "SDF Feature Source";
            this.Name = "SdfFileCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkReadOnly;
    }
}
