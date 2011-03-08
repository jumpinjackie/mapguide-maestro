namespace Maestro.Editors.LoadProcedure
{
    partial class SdfTransformationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdfTransformationCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtCoordinateSystem = new System.Windows.Forms.TextBox();
            this.btnBrowseCs = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numGeneralizePercentage = new System.Windows.Forms.NumericUpDown();
            this.cmbSdfConflictStrategy = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralizePercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.label5);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.cmbSdfConflictStrategy);
            this.contentPanel.Controls.Add(this.numGeneralizePercentage);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.btnBrowseCs);
            this.contentPanel.Controls.Add(this.txtCoordinateSystem);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtCoordinateSystem
            // 
            resources.ApplyResources(this.txtCoordinateSystem, "txtCoordinateSystem");
            this.txtCoordinateSystem.Name = "txtCoordinateSystem";
            this.txtCoordinateSystem.ReadOnly = true;
            // 
            // btnBrowseCs
            // 
            resources.ApplyResources(this.btnBrowseCs, "btnBrowseCs");
            this.btnBrowseCs.Name = "btnBrowseCs";
            this.btnBrowseCs.UseVisualStyleBackColor = true;
            this.btnBrowseCs.Click += new System.EventHandler(this.btnBrowseCs_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numGeneralizePercentage
            // 
            resources.ApplyResources(this.numGeneralizePercentage, "numGeneralizePercentage");
            this.numGeneralizePercentage.Name = "numGeneralizePercentage";
            // 
            // cmbSdfConflictStrategy
            // 
            resources.ApplyResources(this.cmbSdfConflictStrategy, "cmbSdfConflictStrategy");
            this.cmbSdfConflictStrategy.FormattingEnabled = true;
            this.cmbSdfConflictStrategy.Name = "cmbSdfConflictStrategy";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // SdfTransformationCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Transformation";
            this.Name = "SdfTransformationCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralizePercentage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSdfConflictStrategy;
        private System.Windows.Forms.NumericUpDown numGeneralizePercentage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseCs;
        private System.Windows.Forms.TextBox txtCoordinateSystem;
        private System.Windows.Forms.Label label1;
    }
}
