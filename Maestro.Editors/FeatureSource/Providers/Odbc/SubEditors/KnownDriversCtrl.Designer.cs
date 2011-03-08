namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    partial class KnownDriversCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KnownDriversCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.lstDriver = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // propGrid
            // 
            resources.ApplyResources(this.propGrid, "propGrid");
            this.propGrid.Name = "propGrid";
            // 
            // lstDriver
            // 
            resources.ApplyResources(this.lstDriver, "lstDriver");
            this.lstDriver.FormattingEnabled = true;
            this.lstDriver.Name = "lstDriver";
            this.lstDriver.SelectedIndexChanged += new System.EventHandler(this.lstDriver_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // KnownDriversCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstDriver);
            this.Controls.Add(this.propGrid);
            this.Controls.Add(this.label1);
            this.Name = "KnownDriversCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.ListBox lstDriver;
        private System.Windows.Forms.Label label2;
    }
}
