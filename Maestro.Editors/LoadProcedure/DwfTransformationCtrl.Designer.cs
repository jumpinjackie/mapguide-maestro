namespace Maestro.Editors.LoadProcedure
{
    partial class DwfTransformationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DwfTransformationCtrl));
            this.btnBrowseCs = new System.Windows.Forms.Button();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnBrowseCs);
            this.contentPanel.Controls.Add(this.txtCoordinateSystem);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // btnBrowseCs
            // 
            resources.ApplyResources(this.btnBrowseCs, "btnBrowseCs");
            this.btnBrowseCs.Name = "btnBrowseCs";
            this.btnBrowseCs.UseVisualStyleBackColor = true;
            this.btnBrowseCs.Click += new System.EventHandler(this.btnBrowseCs_Click);
            // 
            // txtCoordinateSystem
            // 
            resources.ApplyResources(this.txtCoordinateSystem, "txtCoordinateSystem");
            this.txtCoordinateSystem.Name = "txtCoordinateSystem";
            this.txtCoordinateSystem.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DwfTransformationCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Transformation";
            this.Name = "DwfTransformationCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseCs;
        private System.Windows.Forms.TextBox txtCoordinateSystem;
        private System.Windows.Forms.Label label1;
    }
}
