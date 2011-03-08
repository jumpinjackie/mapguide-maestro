namespace Maestro.Base.Editor
{
    partial class LoadProcedureEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadProcedureEditor));
            this.lpEditor = new Maestro.Editors.LoadProcedure.LoadProcedureEditorCtrl();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.lpEditor);
            // 
            // lpEditor
            // 
            resources.ApplyResources(this.lpEditor, "lpEditor");
            this.lpEditor.Name = "lpEditor";
            // 
            // LoadProcedureEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "LoadProcedureEditor";
            this.panelBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.LoadProcedure.LoadProcedureEditorCtrl lpEditor;
    }
}
