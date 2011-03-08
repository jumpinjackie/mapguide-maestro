namespace Maestro.Editors.LayerDefinition
{
    partial class DrawingLayerEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingLayerEditorCtrl));
            this.drawingSettingsCtrl = new Maestro.Editors.LayerDefinition.Drawing.DrawingLayerSettingsCtrl();
            this.SuspendLayout();
            // 
            // drawingSettingsCtrl
            // 
            this.drawingSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.drawingSettingsCtrl, "drawingSettingsCtrl");
            this.drawingSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.drawingSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drawingSettingsCtrl.HeaderText = "Drawing Source Settings";
            this.drawingSettingsCtrl.Name = "drawingSettingsCtrl";
            // 
            // DrawingLayerEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.drawingSettingsCtrl);
            this.Name = "DrawingLayerEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private Maestro.Editors.LayerDefinition.Drawing.DrawingLayerSettingsCtrl drawingSettingsCtrl;
    }
}
