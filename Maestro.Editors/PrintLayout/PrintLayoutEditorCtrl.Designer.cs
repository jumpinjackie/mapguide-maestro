namespace Maestro.Editors.PrintLayout
{
    partial class PrintLayoutEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintLayoutEditorCtrl));
            this.printSettingsCtrl = new Maestro.Editors.PrintLayout.PrintPagePropertiesSectionCtrl();
            this.printLogosCtrl = new Maestro.Editors.PrintLayout.PrintCustomLogosSectionCtrl();
            this.printTextCtrl = new Maestro.Editors.PrintLayout.PrintCustomTextSectionCtrl();
            this.SuspendLayout();
            // 
            // printSettingsCtrl
            // 
            this.printSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.printSettingsCtrl, "printSettingsCtrl");
            this.printSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.printSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printSettingsCtrl.HeaderText = "Page and Layout Properties";
            this.printSettingsCtrl.Name = "printSettingsCtrl";
            // 
            // printLogosCtrl
            // 
            this.printLogosCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.printLogosCtrl, "printLogosCtrl");
            this.printLogosCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.printLogosCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printLogosCtrl.HeaderText = "Custom Logos";
            this.printLogosCtrl.Name = "printLogosCtrl";
            // 
            // printTextCtrl
            // 
            this.printTextCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.printTextCtrl, "printTextCtrl");
            this.printTextCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.printTextCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printTextCtrl.HeaderText = "Custom Text";
            this.printTextCtrl.Name = "printTextCtrl";
            // 
            // PrintLayoutEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.printTextCtrl);
            this.Controls.Add(this.printLogosCtrl);
            this.Controls.Add(this.printSettingsCtrl);
            this.Name = "PrintLayoutEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private PrintPagePropertiesSectionCtrl printSettingsCtrl;
        private PrintCustomLogosSectionCtrl printLogosCtrl;
        private PrintCustomTextSectionCtrl printTextCtrl;

    }
}
