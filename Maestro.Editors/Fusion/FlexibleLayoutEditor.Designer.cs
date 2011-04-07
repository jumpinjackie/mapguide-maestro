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
            this.settingsCtrl = new Maestro.Editors.Fusion.FlexLayoutSettingsCtrl();
            this.mapsCtrl = new Maestro.Editors.Fusion.MapSettingsCtrl();
            this.widgetsCtrl = new Maestro.Editors.Fusion.WidgetSettingsCtrl();
            this.SuspendLayout();
            // 
            // settingsCtrl
            // 
            this.settingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.settingsCtrl, "settingsCtrl");
            this.settingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.settingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsCtrl.HeaderText = "Flexible Layout Settings";
            this.settingsCtrl.Name = "settingsCtrl";
            // 
            // mapsCtrl
            // 
            this.mapsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.mapsCtrl, "mapsCtrl");
            this.mapsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.mapsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapsCtrl.HeaderText = "Maps";
            this.mapsCtrl.Name = "mapsCtrl";
            // 
            // widgetsCtrl
            // 
            this.widgetsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.widgetsCtrl, "widgetsCtrl");
            this.widgetsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.widgetsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.widgetsCtrl.HeaderText = "Widgets";
            this.widgetsCtrl.Name = "widgetsCtrl";
            // 
            // FlexibleLayoutEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.widgetsCtrl);
            this.Controls.Add(this.mapsCtrl);
            this.Controls.Add(this.settingsCtrl);
            this.Name = "FlexibleLayoutEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private FlexLayoutSettingsCtrl settingsCtrl;
        private MapSettingsCtrl mapsCtrl;
        private WidgetSettingsCtrl widgetsCtrl;
    }
}
