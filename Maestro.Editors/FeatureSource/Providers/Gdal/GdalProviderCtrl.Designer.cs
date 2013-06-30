namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
    partial class GdalProviderCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GdalProviderCtrl));
            this.rdSingle = new System.Windows.Forms.RadioButton();
            this.rdComposite = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.cmbResamplingMethod = new System.Windows.Forms.ComboBox();
            this.chkResamplingMethod = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkResamplingMethod);
            this.contentPanel.Controls.Add(this.cmbResamplingMethod);
            this.contentPanel.Controls.Add(this.txtStatus);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Controls.Add(this.btnTest);
            this.contentPanel.Controls.Add(this.panel1);
            this.contentPanel.Controls.Add(this.rdComposite);
            this.contentPanel.Controls.Add(this.rdSingle);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // rdSingle
            // 
            resources.ApplyResources(this.rdSingle, "rdSingle");
            this.rdSingle.Checked = true;
            this.rdSingle.Name = "rdSingle";
            this.rdSingle.TabStop = true;
            this.rdSingle.UseVisualStyleBackColor = true;
            this.rdSingle.CheckedChanged += new System.EventHandler(this.OnTypeCheckedChanged);
            // 
            // rdComposite
            // 
            resources.ApplyResources(this.rdComposite, "rdComposite");
            this.rdComposite.Name = "rdComposite";
            this.rdComposite.TabStop = true;
            this.rdComposite.UseVisualStyleBackColor = true;
            this.rdComposite.CheckedChanged += new System.EventHandler(this.OnTypeCheckedChanged);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtStatus
            // 
            resources.ApplyResources(this.txtStatus, "txtStatus");
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            // 
            // cmbResamplingMethod
            // 
            resources.ApplyResources(this.cmbResamplingMethod, "cmbResamplingMethod");
            this.cmbResamplingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResamplingMethod.FormattingEnabled = true;
            this.cmbResamplingMethod.Name = "cmbResamplingMethod";
            this.cmbResamplingMethod.SelectedIndexChanged += new System.EventHandler(this.cmbResamplingMethod_SelectedIndexChanged);
            // 
            // chkResamplingMethod
            // 
            resources.ApplyResources(this.chkResamplingMethod, "chkResamplingMethod");
            this.chkResamplingMethod.Name = "chkResamplingMethod";
            this.chkResamplingMethod.UseVisualStyleBackColor = true;
            this.chkResamplingMethod.CheckedChanged += new System.EventHandler(this.chkResamplingMethod_CheckedChanged);
            // 
            // GdalProviderCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "GdalProviderCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdComposite;
        private System.Windows.Forms.RadioButton rdSingle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ComboBox cmbResamplingMethod;
        private System.Windows.Forms.CheckBox chkResamplingMethod;
    }
}
