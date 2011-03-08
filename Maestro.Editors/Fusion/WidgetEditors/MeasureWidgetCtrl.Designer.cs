namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class MeasureWidgetCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureWidgetCtrl));
            this.baseEditor = new Maestro.Editors.Fusion.WidgetEditors.WidgetEditorBase();
            this.SuspendLayout();
            // 
            // baseEditor
            // 
            resources.ApplyResources(this.baseEditor, "baseEditor");
            this.baseEditor.Name = "baseEditor";
            // 
            // MeasureWidgetCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.baseEditor);
            this.Name = "MeasureWidgetCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private WidgetEditorBase baseEditor;
    }
}
