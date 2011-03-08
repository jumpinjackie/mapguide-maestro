namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    partial class ManagedCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagedCtrl));
            this.resDataCtrl = new Maestro.Editors.Common.ResourceDataCtrl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // resDataCtrl
            // 
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.MarkedFile = "";
            this.resDataCtrl.MarkEnabled = true;
            this.resDataCtrl.Name = "resDataCtrl";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ManagedCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resDataCtrl);
            this.Name = "ManagedCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Maestro.Editors.Common.ResourceDataCtrl resDataCtrl;
        private System.Windows.Forms.Label label1;
    }
}
