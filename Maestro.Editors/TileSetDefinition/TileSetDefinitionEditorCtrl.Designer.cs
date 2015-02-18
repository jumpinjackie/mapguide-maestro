namespace Maestro.Editors.TileSetDefinition
{
    partial class TileSetDefinitionEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileSetDefinitionEditorCtrl));
            this.layerStructureCtrl = new Maestro.Editors.TileSetDefinition.LayerStructureCtrl();
            this.tileSetSettingsCtrl = new Maestro.Editors.TileSetDefinition.TileSetSettingsCtrl();
            this.SuspendLayout();
            // 
            // layerStructureCtrl
            // 
            this.layerStructureCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.layerStructureCtrl, "layerStructureCtrl");
            this.layerStructureCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.layerStructureCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.layerStructureCtrl.Name = "layerStructureCtrl";
            // 
            // tileSetSettingsCtrl
            // 
            this.tileSetSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.tileSetSettingsCtrl, "tileSetSettingsCtrl");
            this.tileSetSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.tileSetSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tileSetSettingsCtrl.Name = "tileSetSettingsCtrl";
            // 
            // TileSetDefinitionEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.layerStructureCtrl);
            this.Controls.Add(this.tileSetSettingsCtrl);
            this.Name = "TileSetDefinitionEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private LayerStructureCtrl layerStructureCtrl;
        private TileSetSettingsCtrl tileSetSettingsCtrl;
    }
}
