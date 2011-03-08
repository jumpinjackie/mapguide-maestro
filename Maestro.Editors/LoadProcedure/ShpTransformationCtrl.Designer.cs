namespace Maestro.Editors.LoadProcedure
{
    partial class ShpTransformationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShpTransformationCtrl));
            this.label4 = new System.Windows.Forms.Label();
            this.numGeneralizePercentage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseCs = new System.Windows.Forms.Button();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkConvertToSdf = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralizePercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkConvertToSdf);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.numGeneralizePercentage);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.btnBrowseCs);
            this.contentPanel.Controls.Add(this.txtCoordinateSystem);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numGeneralizePercentage
            // 
            resources.ApplyResources(this.numGeneralizePercentage, "numGeneralizePercentage");
            this.numGeneralizePercentage.Name = "numGeneralizePercentage";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // chkConvertToSdf
            // 
            resources.ApplyResources(this.chkConvertToSdf, "chkConvertToSdf");
            this.chkConvertToSdf.Name = "chkConvertToSdf";
            this.chkConvertToSdf.UseVisualStyleBackColor = true;
            // 
            // ShpTransformationCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Transformation";
            this.Name = "ShpTransformationCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralizePercentage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numGeneralizePercentage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseCs;
        private System.Windows.Forms.TextBox txtCoordinateSystem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkConvertToSdf;
    }
}
