namespace Maestro.Editors.WatermarkDefinition
{
    partial class WatermarkEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatermarkEditorCtrl));
            this.wmSettings = new Maestro.Editors.WatermarkDefinition.WatermarkSettingsCtrl();
            this.wmContent = new Maestro.Editors.WatermarkDefinition.WatermarkContentCtrl();
            this.SuspendLayout();
            // 
            // wmSettings
            // 
            this.wmSettings.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.wmSettings, "wmSettings");
            this.wmSettings.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.wmSettings.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.wmSettings.Name = "wmSettings";
            // 
            // wmContent
            // 
            this.wmContent.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.wmContent, "wmContent");
            this.wmContent.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.wmContent.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.wmContent.Name = "wmContent";
            // 
            // WatermarkEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.wmContent);
            this.Controls.Add(this.wmSettings);
            this.Name = "WatermarkEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private WatermarkSettingsCtrl wmSettings;
        private WatermarkContentCtrl wmContent;

    }
}
