namespace Maestro.Base.Editor
{
    partial class FeatureSourceEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceEditor));
            this.fsEditorCtrl = new Maestro.Editors.FeatureSource.FeatureSourceEditorCtrl();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.fsEditorCtrl);
            // 
            // fsEditorCtrl
            // 
            resources.ApplyResources(this.fsEditorCtrl, "fsEditorCtrl");
            this.fsEditorCtrl.Name = "fsEditorCtrl";
            // 
            // FeatureSourceEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "FeatureSourceEditor";
            this.panelBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.FeatureSource.FeatureSourceEditorCtrl fsEditorCtrl;
    }
}
