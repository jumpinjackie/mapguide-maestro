namespace Maestro.Editors.DrawingSource
{
    partial class DrawingSourceEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingSourceEditorCtrl));
            this.sourceSectionCtrl = new Maestro.Editors.DrawingSource.SourceSectionCtrl();
            this.SuspendLayout();
            // 
            // sourceSectionCtrl
            // 
            this.sourceSectionCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.sourceSectionCtrl, "sourceSectionCtrl");
            this.sourceSectionCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.sourceSectionCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceSectionCtrl.HeaderText = "DWF Drawing Source";
            this.sourceSectionCtrl.Name = "sourceSectionCtrl";
            // 
            // DrawingSourceEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.sourceSectionCtrl);
            this.Name = "DrawingSourceEditorCtrl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private SourceSectionCtrl sourceSectionCtrl;

    }
}
