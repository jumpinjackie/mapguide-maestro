namespace Maestro.Editors.FeatureSource.Providers.Odbc
{
    partial class OdbcProviderCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OdbcProviderCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnEditSchema = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtConnectionStatus = new System.Windows.Forms.TextBox();
            this.pnlMethod = new System.Windows.Forms.Panel();
            this.chkUse64Bit = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkUse64Bit);
            this.contentPanel.Controls.Add(this.pnlMethod);
            this.contentPanel.Controls.Add(this.groupBox3);
            this.contentPanel.Controls.Add(this.groupBox2);
            this.contentPanel.Controls.Add(this.btnReset);
            this.contentPanel.Controls.Add(this.btnEditSchema);
            this.contentPanel.Controls.Add(this.btnTest);
            this.contentPanel.Controls.Add(this.cmbMethod);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbMethod
            // 
            resources.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.SelectedIndexChanged += new System.EventHandler(this.cmbMethod_SelectedIndexChanged);
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnEditSchema
            // 
            resources.ApplyResources(this.btnEditSchema, "btnEditSchema");
            this.btnEditSchema.Name = "btnEditSchema";
            this.btnEditSchema.UseVisualStyleBackColor = true;
            this.btnEditSchema.Click += new System.EventHandler(this.btnEditSchema_Click);
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.txtConnStr);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtConnStr
            // 
            resources.ApplyResources(this.txtConnStr, "txtConnStr");
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.ReadOnly = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.txtConnectionStatus);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // txtConnectionStatus
            // 
            resources.ApplyResources(this.txtConnectionStatus, "txtConnectionStatus");
            this.txtConnectionStatus.Name = "txtConnectionStatus";
            this.txtConnectionStatus.ReadOnly = true;
            // 
            // pnlMethod
            // 
            resources.ApplyResources(this.pnlMethod, "pnlMethod");
            this.pnlMethod.Name = "pnlMethod";
            // 
            // chkUse64Bit
            // 
            resources.ApplyResources(this.chkUse64Bit, "chkUse64Bit");
            this.chkUse64Bit.Name = "chkUse64Bit";
            this.chkUse64Bit.UseVisualStyleBackColor = true;
            this.chkUse64Bit.CheckedChanged += new System.EventHandler(this.chkUse64Bit_CheckedChanged);
            // 
            // OdbcProviderCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "OdbcProviderCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEditSchema;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtConnectionStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.Panel pnlMethod;
        private System.Windows.Forms.CheckBox chkUse64Bit;
    }
}
