namespace Maestro.Base.Editor
{
    partial class XmlEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlEditor));
            this.editor = new Maestro.Editors.Generic.XmlEditorCtrl();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.editor);
            resources.ApplyResources(this.panelBody, "panelBody");
            // 
            // editor
            // 
            this.editor.BackgroundColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.editor, "editor");
            this.editor.Name = "editor";
            this.editor.SupportsReReadFromSource = true;
            this.editor.TextColor = System.Drawing.SystemColors.WindowText;
            this.editor.TextFont = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editor.Validator = null;
            this.editor.XmlContent = "";
            this.editor.RequestReloadFromSource += new System.EventHandler(this.editor_RequestReloadFromSource);
            // 
            // XmlEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "XmlEditor";
            resources.ApplyResources(this, "$this");
            this.panelBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.Generic.XmlEditorCtrl editor;
    }
}
