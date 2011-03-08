namespace Maestro.Editors.MapDefinition
{
    partial class MapDefinitionEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapDefinitionEditorCtrl));
            this.mapSettingsCtrl = new Maestro.Editors.MapDefinition.MapSettingsSectionCtrl();
            this.mapLayersCtrl = new Maestro.Editors.MapDefinition.MapLayersSectionCtrl();
            this.SuspendLayout();
            // 
            // mapSettingsCtrl
            // 
            this.mapSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.mapSettingsCtrl, "mapSettingsCtrl");
            this.mapSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.mapSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapSettingsCtrl.HeaderText = "Map Settings";
            this.mapSettingsCtrl.Name = "mapSettingsCtrl";
            // 
            // mapLayersCtrl
            // 
            this.mapLayersCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.mapLayersCtrl, "mapLayersCtrl");
            this.mapLayersCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.mapLayersCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapLayersCtrl.HeaderText = "Layers";
            this.mapLayersCtrl.Name = "mapLayersCtrl";
            // 
            // MapDefinitionEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.mapLayersCtrl);
            this.Controls.Add(this.mapSettingsCtrl);
            this.Name = "MapDefinitionEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private MapSettingsSectionCtrl mapSettingsCtrl;
        private MapLayersSectionCtrl mapLayersCtrl;
    }
}
