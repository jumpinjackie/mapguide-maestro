namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    partial class ConnectionStringCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionStringCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.lnkApplyCredentials = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtConnStr
            // 
            resources.ApplyResources(this.txtConnStr, "txtConnStr");
            this.txtConnStr.Name = "txtConnStr";
            // 
            // lnkApplyCredentials
            // 
            resources.ApplyResources(this.lnkApplyCredentials, "lnkApplyCredentials");
            this.lnkApplyCredentials.Name = "lnkApplyCredentials";
            this.lnkApplyCredentials.TabStop = true;
            this.lnkApplyCredentials.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkApplyCredentials_LinkClicked);
            // 
            // ConnectionStringCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkApplyCredentials);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.label1);
            this.Name = "ConnectionStringCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.LinkLabel lnkApplyCredentials;
    }
}
