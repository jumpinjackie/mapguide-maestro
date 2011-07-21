namespace Maestro.Editors.LayerDefinition
{
    partial class VectorLayerEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorLayerEditorCtrl));
            this.resSettings = new Maestro.Editors.LayerDefinition.Vector.VectorLayerSettingsSectionCtrl();
            this.layerProperties = new Maestro.Editors.LayerDefinition.LayerPropertiesSectionCtrl();
            this.layerStyles = new Maestro.Editors.LayerDefinition.Vector.VectorLayerStyleSectionCtrl();
            this.SuspendLayout();
            // 
            // resSettings
            // 
            this.resSettings.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.resSettings, "resSettings");
            this.resSettings.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.resSettings.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resSettings.Name = "resSettings";
            // 
            // layerProperties
            // 
            this.layerProperties.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.layerProperties, "layerProperties");
            this.layerProperties.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.layerProperties.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layerProperties.Name = "layerProperties";
            // 
            // layerStyles
            // 
            this.layerStyles.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.layerStyles, "layerStyles");
            this.layerStyles.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.layerStyles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layerStyles.Name = "layerStyles";
            // 
            // VectorLayerEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.layerStyles);
            this.Controls.Add(this.layerProperties);
            this.Controls.Add(this.resSettings);
            this.Name = "VectorLayerEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.LayerDefinition.Vector.VectorLayerSettingsSectionCtrl resSettings;
        private LayerPropertiesSectionCtrl layerProperties;
        private Maestro.Editors.LayerDefinition.Vector.VectorLayerStyleSectionCtrl layerStyles;
    }
}
