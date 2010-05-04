namespace OSGeo.MapGuide.Maestro
{
    partial class FormLogin
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
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rdHttp = new System.Windows.Forms.RadioButton();
            this.rdTcpIp = new System.Windows.Forms.RadioButton();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkSavePassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSavePassword.Location = new System.Drawing.Point(206, 178);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(161, 16);
            this.chkSavePassword.TabIndex = 15;
            this.chkSavePassword.Text = "Save password on computer";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(206, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 32);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(94, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 32);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rdHttp
            // 
            this.rdHttp.AutoSize = true;
            this.rdHttp.Location = new System.Drawing.Point(61, 178);
            this.rdHttp.Name = "rdHttp";
            this.rdHttp.Size = new System.Drawing.Size(114, 17);
            this.rdHttp.TabIndex = 16;
            this.rdHttp.TabStop = true;
            this.rdHttp.Text = "Connect via HTTP";
            this.rdHttp.UseVisualStyleBackColor = true;
            this.rdHttp.CheckedChanged += new System.EventHandler(this.rdHttp_CheckedChanged);
            // 
            // rdTcpIp
            // 
            this.rdTcpIp.AutoSize = true;
            this.rdTcpIp.Location = new System.Drawing.Point(61, 202);
            this.rdTcpIp.Name = "rdTcpIp";
            this.rdTcpIp.Size = new System.Drawing.Size(121, 17);
            this.rdTcpIp.TabIndex = 17;
            this.rdTcpIp.TabStop = true;
            this.rdTcpIp.Text = "Connect via TCP/IP";
            this.rdTcpIp.UseVisualStyleBackColor = true;
            this.rdTcpIp.CheckedChanged += new System.EventHandler(this.rdTcpIp_CheckedChanged);
            // 
            // loginPanel
            // 
            this.loginPanel.Location = new System.Drawing.Point(12, 12);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(374, 160);
            this.loginPanel.TabIndex = 18;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 277);
            this.Controls.Add(this.loginPanel);
            this.Controls.Add(this.rdTcpIp);
            this.Controls.Add(this.rdHttp);
            this.Controls.Add(this.chkSavePassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log on to a MapGuide Server";
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
    }
}