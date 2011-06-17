namespace Maestro.Editors.SymbolDefinition
{
    partial class SimpleSymbolDefinitionEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleSymbolDefinitionEditorCtrl));
            this.generalSettingsCtrl = new Maestro.Editors.SymbolDefinition.GeneralSettingsCtrl();
            this.symbolGraphicsCtrl = new Maestro.Editors.SymbolDefinition.SymbolGraphicsCtrl();
            this.parametersCtrl = new Maestro.Editors.SymbolDefinition.ParametersCtrl();
            this.usageContextsCtrl = new Maestro.Editors.SymbolDefinition.UsageContextsCtrl();
            this.advancedSettingsCtrl = new Maestro.Editors.SymbolDefinition.AdvancedSettingsCtrl();
            this.SuspendLayout();
            // 
            // generalSettingsCtrl
            // 
            this.generalSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.generalSettingsCtrl, "generalSettingsCtrl");
            this.generalSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.generalSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.generalSettingsCtrl.HeaderText = "General";
            this.generalSettingsCtrl.Name = "generalSettingsCtrl";
            // 
            // symbolGraphicsCtrl
            // 
            this.symbolGraphicsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.symbolGraphicsCtrl, "symbolGraphicsCtrl");
            this.symbolGraphicsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.symbolGraphicsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.symbolGraphicsCtrl.HeaderText = "Graphics";
            this.symbolGraphicsCtrl.Name = "symbolGraphicsCtrl";
            // 
            // parametersCtrl
            // 
            this.parametersCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.parametersCtrl, "parametersCtrl");
            this.parametersCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.parametersCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.parametersCtrl.HeaderText = "Symbol Parameters";
            this.parametersCtrl.Name = "parametersCtrl";
            // 
            // usageContextsCtrl
            // 
            this.usageContextsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.usageContextsCtrl, "usageContextsCtrl");
            this.usageContextsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.usageContextsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.usageContextsCtrl.HeaderText = "Symbol Usage";
            this.usageContextsCtrl.Name = "usageContextsCtrl";
            // 
            // advancedSettingsCtrl
            // 
            this.advancedSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.advancedSettingsCtrl, "advancedSettingsCtrl");
            this.advancedSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.advancedSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.advancedSettingsCtrl.HeaderText = "Advanced";
            this.advancedSettingsCtrl.Name = "advancedSettingsCtrl";
            // 
            // SimpleSymbolDefinitionEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.advancedSettingsCtrl);
            this.Controls.Add(this.usageContextsCtrl);
            this.Controls.Add(this.parametersCtrl);
            this.Controls.Add(this.symbolGraphicsCtrl);
            this.Controls.Add(this.generalSettingsCtrl);
            this.Name = "SimpleSymbolDefinitionEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private GeneralSettingsCtrl generalSettingsCtrl;
        private SymbolGraphicsCtrl symbolGraphicsCtrl;
        private ParametersCtrl parametersCtrl;
        private UsageContextsCtrl usageContextsCtrl;
        private AdvancedSettingsCtrl advancedSettingsCtrl;

    }
}
