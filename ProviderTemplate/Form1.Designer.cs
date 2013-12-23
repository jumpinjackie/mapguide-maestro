namespace ProviderTemplate
{
    partial class Form1
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
            this.txtMgDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFxDir = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFxDir = new System.Windows.Forms.Button();
            this.btnMgDir = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rdPost22 = new System.Windows.Forms.RadioButton();
            this.rdPre22 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.txtMgVersion = new System.Windows.Forms.TextBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMgDir
            // 
            this.txtMgDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMgDir.Location = new System.Drawing.Point(12, 71);
            this.txtMgDir.Name = "txtMgDir";
            this.txtMgDir.Size = new System.Drawing.Size(522, 20);
            this.txtMgDir.TabIndex = 3;
            this.txtMgDir.Text = "C:\\Program Files\\OSGeo\\MapGuide\\Web\\www\\mapviewernet\\bin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MapGuide .net assemblies directory";
            // 
            // txtFxDir
            // 
            this.txtFxDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFxDir.Location = new System.Drawing.Point(12, 28);
            this.txtFxDir.Name = "txtFxDir";
            this.txtFxDir.Size = new System.Drawing.Size(522, 20);
            this.txtFxDir.TabIndex = 7;
            this.txtFxDir.Text = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = ".net 4.0 Framework directory (csc.exe)";
            // 
            // btnFxDir
            // 
            this.btnFxDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFxDir.Location = new System.Drawing.Point(540, 26);
            this.btnFxDir.Name = "btnFxDir";
            this.btnFxDir.Size = new System.Drawing.Size(26, 23);
            this.btnFxDir.TabIndex = 10;
            this.btnFxDir.Text = "...";
            this.btnFxDir.UseVisualStyleBackColor = true;
            this.btnFxDir.Click += new System.EventHandler(this.btnFxDir_Click);
            // 
            // btnMgDir
            // 
            this.btnMgDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMgDir.Location = new System.Drawing.Point(540, 70);
            this.btnMgDir.Name = "btnMgDir";
            this.btnMgDir.Size = new System.Drawing.Size(26, 23);
            this.btnMgDir.TabIndex = 12;
            this.btnMgDir.Text = "...";
            this.btnMgDir.UseVisualStyleBackColor = true;
            this.btnMgDir.Click += new System.EventHandler(this.btnMgDir_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "MapGuide API Version (x.y.z)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "MapGuide Assembly Type";
            // 
            // rdPost22
            // 
            this.rdPost22.AutoSize = true;
            this.rdPost22.Checked = true;
            this.rdPost22.Location = new System.Drawing.Point(175, 139);
            this.rdPost22.Name = "rdPost22";
            this.rdPost22.Size = new System.Drawing.Size(196, 17);
            this.rdPost22.TabIndex = 19;
            this.rdPost22.TabStop = true;
            this.rdPost22.Text = "2.2 and newer (OSGeo.MapGuide.*)";
            this.rdPost22.UseVisualStyleBackColor = true;
            // 
            // rdPre22
            // 
            this.rdPre22.AutoSize = true;
            this.rdPre22.Location = new System.Drawing.Point(377, 139);
            this.rdPre22.Name = "rdPre22";
            this.rdPre22.Size = new System.Drawing.Size(179, 17);
            this.rdPre22.TabIndex = 20;
            this.rdPre22.Text = "Pre-2.2 (MapGuideDotNetApi.dll)";
            this.rdPre22.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtMessages);
            this.groupBox2.Location = new System.Drawing.Point(12, 185);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 189);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            // 
            // txtMessages
            // 
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(3, 16);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtMessages.Size = new System.Drawing.Size(548, 170);
            this.txtMessages.TabIndex = 0;
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Location = new System.Drawing.Point(491, 380);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 23;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // worker
            // 
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // txtMgVersion
            // 
            this.txtMgVersion.Location = new System.Drawing.Point(175, 103);
            this.txtMgVersion.Name = "txtMgVersion";
            this.txtMgVersion.Size = new System.Drawing.Size(100, 20);
            this.txtMgVersion.TabIndex = 24;
            this.txtMgVersion.Text = "2.4.0";
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(12, 162);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(84, 17);
            this.chkDebug.TabIndex = 25;
            this.chkDebug.Text = "Build Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 415);
            this.Controls.Add(this.chkDebug);
            this.Controls.Add(this.txtMgVersion);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.rdPre22);
            this.Controls.Add(this.rdPost22);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnMgDir);
            this.Controls.Add(this.btnFxDir);
            this.Controls.Add(this.txtFxDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMgDir);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Maestro LocalNative provider builder";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMgDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFxDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFxDir;
        private System.Windows.Forms.Button btnMgDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rdPost22;
        private System.Windows.Forms.RadioButton rdPre22;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.TextBox txtMgVersion;
        private System.Windows.Forms.CheckBox chkDebug;
    }
}

