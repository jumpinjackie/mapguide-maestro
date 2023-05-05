namespace Maestro.Editors.Fusion
{
    partial class FlexibleLayoutEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexibleLayoutEditor));
            settingsCtrl = new FlexLayoutSettingsCtrl();
            mapsCtrl = new MapSettingsCtrl();
            widgetsCtrl = new WidgetSettingsCtrl();
            SuspendLayout();
            // 
            // settingsCtrl
            // 
            settingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(settingsCtrl, "settingsCtrl");
            settingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            settingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            settingsCtrl.Name = "settingsCtrl";
            // 
            // mapsCtrl
            // 
            mapsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(mapsCtrl, "mapsCtrl");
            mapsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            mapsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            mapsCtrl.Name = "mapsCtrl";
            // 
            // widgetsCtrl
            // 
            widgetsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(widgetsCtrl, "widgetsCtrl");
            widgetsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            widgetsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            widgetsCtrl.Name = "widgetsCtrl";
            // 
            // FlexibleLayoutEditor
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            Controls.Add(widgetsCtrl);
            Controls.Add(mapsCtrl);
            Controls.Add(settingsCtrl);
            Name = "FlexibleLayoutEditor";
            ResumeLayout(false);
        }

        #endregion

        private FlexLayoutSettingsCtrl settingsCtrl;
        private MapSettingsCtrl mapsCtrl;
        private WidgetSettingsCtrl widgetsCtrl;
    }
}
