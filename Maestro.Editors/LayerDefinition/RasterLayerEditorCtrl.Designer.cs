namespace Maestro.Editors.LayerDefinition
{
    partial class RasterLayerEditorCtrl
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
            this.rasterLayerSettingsSectionCtrl = new Maestro.Editors.LayerDefinition.Raster.RasterLayerSettingsSectionCtrl();
            this.rasterLayerVisibilitySectionCtrl = new Maestro.Editors.LayerDefinition.Raster.RasterLayerVisibilitySectionCtrl();
            this.rasterLayerAdvancedSectionCtrl = new Maestro.Editors.LayerDefinition.Raster.RasterLayerAdvancedSectionCtrl();
            this.SuspendLayout();
            // 
            // rasterLayerSettingsSectionCtrl
            // 
            this.rasterLayerSettingsSectionCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            this.rasterLayerSettingsSectionCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.rasterLayerSettingsSectionCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.rasterLayerSettingsSectionCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.rasterLayerSettingsSectionCtrl.HeaderText = "Feature Source Settings";
            this.rasterLayerSettingsSectionCtrl.Location = new System.Drawing.Point(0, 0);
            this.rasterLayerSettingsSectionCtrl.Name = "rasterLayerSettingsSectionCtrl";
            this.rasterLayerSettingsSectionCtrl.Size = new System.Drawing.Size(594, 145);
            this.rasterLayerSettingsSectionCtrl.TabIndex = 0;
            // 
            // rasterLayerVisibilitySectionCtrl
            // 
            this.rasterLayerVisibilitySectionCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            this.rasterLayerVisibilitySectionCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.rasterLayerVisibilitySectionCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.rasterLayerVisibilitySectionCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.rasterLayerVisibilitySectionCtrl.HeaderText = "Layer Visbility";
            this.rasterLayerVisibilitySectionCtrl.Location = new System.Drawing.Point(0, 145);
            this.rasterLayerVisibilitySectionCtrl.Name = "rasterLayerVisibilitySectionCtrl";
            this.rasterLayerVisibilitySectionCtrl.Size = new System.Drawing.Size(594, 197);
            this.rasterLayerVisibilitySectionCtrl.TabIndex = 1;
            // 
            // rasterLayerAdvancedSectionCtrl
            // 
            this.rasterLayerAdvancedSectionCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            this.rasterLayerAdvancedSectionCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.rasterLayerAdvancedSectionCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.rasterLayerAdvancedSectionCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.rasterLayerAdvancedSectionCtrl.HeaderText = "Advanced Settings";
            this.rasterLayerAdvancedSectionCtrl.Location = new System.Drawing.Point(0, 342);
            this.rasterLayerAdvancedSectionCtrl.Name = "rasterLayerAdvancedSectionCtrl";
            this.rasterLayerAdvancedSectionCtrl.Size = new System.Drawing.Size(594, 391);
            this.rasterLayerAdvancedSectionCtrl.TabIndex = 2;
            // 
            // RasterLayerEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.rasterLayerAdvancedSectionCtrl);
            this.Controls.Add(this.rasterLayerVisibilitySectionCtrl);
            this.Controls.Add(this.rasterLayerSettingsSectionCtrl);
            this.Name = "RasterLayerEditorCtrl";
            this.Size = new System.Drawing.Size(594, 539);
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.LayerDefinition.Raster.RasterLayerSettingsSectionCtrl rasterLayerSettingsSectionCtrl;
        private Maestro.Editors.LayerDefinition.Raster.RasterLayerVisibilitySectionCtrl rasterLayerVisibilitySectionCtrl;
        private Maestro.Editors.LayerDefinition.Raster.RasterLayerAdvancedSectionCtrl rasterLayerAdvancedSectionCtrl;

    }
}
