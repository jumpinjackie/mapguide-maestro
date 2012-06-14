namespace Maestro.Base.Editor
{
    partial class EditorContentBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorContentBase));
            this.upgradePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.sessionRestartPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.upgradePanel.SuspendLayout();
            this.sessionRestartPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // upgradePanel
            // 
            this.upgradePanel.BackColor = System.Drawing.SystemColors.Info;
            this.upgradePanel.Controls.Add(this.label1);
            this.upgradePanel.Controls.Add(this.btnUpgrade);
            resources.ApplyResources(this.upgradePanel, "upgradePanel");
            this.upgradePanel.Name = "upgradePanel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnUpgrade
            // 
            resources.ApplyResources(this.btnUpgrade, "btnUpgrade");
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // panelBody
            // 
            resources.ApplyResources(this.panelBody, "panelBody");
            this.panelBody.Name = "panelBody";
            // 
            // sessionRestartPanel
            // 
            this.sessionRestartPanel.BackColor = System.Drawing.SystemColors.Info;
            this.sessionRestartPanel.Controls.Add(this.label2);
            this.sessionRestartPanel.Controls.Add(this.btnReload);
            resources.ApplyResources(this.sessionRestartPanel, "sessionRestartPanel");
            this.sessionRestartPanel.Name = "sessionRestartPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnReload
            // 
            resources.ApplyResources(this.btnReload, "btnReload");
            this.btnReload.Name = "btnReload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // EditorContentBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.sessionRestartPanel);
            this.Controls.Add(this.upgradePanel);
            this.Name = "EditorContentBase";
            resources.ApplyResources(this, "$this");
            this.upgradePanel.ResumeLayout(false);
            this.sessionRestartPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel upgradePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpgrade;
        protected System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Panel sessionRestartPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReload;
    }
}
