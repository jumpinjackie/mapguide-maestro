namespace Maestro.Login
{
    partial class LoginDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginDialog));
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rdHttp = new System.Windows.Forms.RadioButton();
            this.rdTcpIp = new System.Windows.Forms.RadioButton();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.rdLocal = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // chkSavePassword
            // 
            resources.ApplyResources(this.chkSavePassword, "chkSavePassword");
            this.chkSavePassword.Name = "chkSavePassword";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rdHttp
            // 
            resources.ApplyResources(this.rdHttp, "rdHttp");
            this.rdHttp.Name = "rdHttp";
            this.rdHttp.TabStop = true;
            this.rdHttp.UseVisualStyleBackColor = true;
            this.rdHttp.CheckedChanged += new System.EventHandler(this.rdHttp_CheckedChanged);
            // 
            // rdTcpIp
            // 
            resources.ApplyResources(this.rdTcpIp, "rdTcpIp");
            this.rdTcpIp.Name = "rdTcpIp";
            this.rdTcpIp.TabStop = true;
            this.rdTcpIp.UseVisualStyleBackColor = true;
            this.rdTcpIp.CheckedChanged += new System.EventHandler(this.rdTcpIp_CheckedChanged);
            // 
            // loginPanel
            // 
            resources.ApplyResources(this.loginPanel, "loginPanel");
            this.loginPanel.Name = "loginPanel";
            // 
            // rdLocal
            // 
            resources.ApplyResources(this.rdLocal, "rdLocal");
            this.rdLocal.Name = "rdLocal";
            this.rdLocal.TabStop = true;
            this.rdLocal.UseVisualStyleBackColor = true;
            this.rdLocal.CheckedChanged += new System.EventHandler(this.rdLocal_CheckedChanged);
            // 
            // LoginDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.rdLocal);
            this.Controls.Add(this.loginPanel);
            this.Controls.Add(this.rdTcpIp);
            this.Controls.Add(this.rdHttp);
            this.Controls.Add(this.chkSavePassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSavePassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rdHttp;
        private System.Windows.Forms.RadioButton rdTcpIp;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.RadioButton rdLocal;
    }
}