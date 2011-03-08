namespace Maestro.Base.Editor
{
    partial class MapDefinitionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapDefinitionEditor));
            this.mapEditorCtrl = new Maestro.Editors.MapDefinition.MapDefinitionEditorCtrl();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.mapEditorCtrl);
            // 
            // mapEditorCtrl
            // 
            resources.ApplyResources(this.mapEditorCtrl, "mapEditorCtrl");
            this.mapEditorCtrl.Name = "mapEditorCtrl";
            // 
            // MapDefinitionEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "MapDefinitionEditor";
            this.panelBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.MapDefinition.MapDefinitionEditorCtrl mapEditorCtrl;
    }
}
