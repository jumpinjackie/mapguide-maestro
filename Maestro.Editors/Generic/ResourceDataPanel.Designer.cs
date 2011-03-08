namespace Maestro.Editors.Generic
{
    partial class ResourceDataPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceDataPanel));
            this.resDataCtrl = new Maestro.Editors.Common.ResourceDataCtrl();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.resDataCtrl);
            // 
            // resDataCtrl
            // 
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.MarkedFile = "";
            this.resDataCtrl.MarkEnabled = false;
            this.resDataCtrl.Name = "resDataCtrl";
            // 
            // ResourceDataPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Resource Data Files";
            this.Name = "ResourceDataPanel";
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.Common.ResourceDataCtrl resDataCtrl;

    }
}
