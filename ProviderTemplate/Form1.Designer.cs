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
            this.btnMgDir = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.txtMgVersion = new System.Windows.Forms.TextBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAdditionalReferences = new System.Windows.Forms.TextBox();
            this.btnSaveMessages = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMgDir
            // 
            this.txtMgDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMgDir.Location = new System.Drawing.Point(12, 71);
            this.txtMgDir.Name = "txtMgDir";
            this.txtMgDir.Size = new System.Drawing.Size(703, 20);
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
            // btnMgDir
            // 
            this.btnMgDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMgDir.Location = new System.Drawing.Point(721, 70);
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
            this.label6.Location = new System.Drawing.Point(9, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "MapGuide API Version (x.y.z)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtMessages);
            this.groupBox2.Location = new System.Drawing.Point(12, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(735, 164);
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
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(729, 145);
            this.txtMessages.TabIndex = 0;
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Location = new System.Drawing.Point(672, 468);
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
            this.txtMgVersion.Location = new System.Drawing.Point(175, 16);
            this.txtMgVersion.Name = "txtMgVersion";
            this.txtMgVersion.Size = new System.Drawing.Size(100, 20);
            this.txtMgVersion.TabIndex = 24;
            this.txtMgVersion.Text = "2.4.0";
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(15, 106);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(84, 17);
            this.chkDebug.TabIndex = 25;
            this.chkDebug.Text = "Build Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(297, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Additional Assembly References (one per line)";
            // 
            // txtAdditionalReferences
            // 
            this.txtAdditionalReferences.Location = new System.Drawing.Point(300, 123);
            this.txtAdditionalReferences.Multiline = true;
            this.txtAdditionalReferences.Name = "txtAdditionalReferences";
            this.txtAdditionalReferences.Size = new System.Drawing.Size(444, 160);
            this.txtAdditionalReferences.TabIndex = 27;
            this.txtAdditionalReferences.Text = "GeoAPI.dll";
            // 
            // btnSaveMessages
            // 
            this.btnSaveMessages.Location = new System.Drawing.Point(12, 468);
            this.btnSaveMessages.Name = "btnSaveMessages";
            this.btnSaveMessages.Size = new System.Drawing.Size(96, 23);
            this.btnSaveMessages.TabIndex = 28;
            this.btnSaveMessages.Text = "Save Messages";
            this.btnSaveMessages.UseVisualStyleBackColor = true;
            this.btnSaveMessages.Click += new System.EventHandler(this.btnSaveMessages_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 503);
            this.Controls.Add(this.btnSaveMessages);
            this.Controls.Add(this.txtAdditionalReferences);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkDebug);
            this.Controls.Add(this.txtMgVersion);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnMgDir);
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
        private System.Windows.Forms.Button btnMgDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.TextBox txtMgVersion;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAdditionalReferences;
        private System.Windows.Forms.Button btnSaveMessages;
    }
}

