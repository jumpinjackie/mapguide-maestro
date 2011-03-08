namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
    partial class SingleFileCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.btnBrowseDir = new System.Windows.Forms.Button();
            this.btnBrowseAliasDir = new System.Windows.Forms.Button();
            this.btnBrowseAliasFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File or Folder path";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(17, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(479, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Note that all paths are as seen by the MapGuide server, not Maestro";
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(20, 33);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(316, 20);
            this.txtPath.TabIndex = 4;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Location = new System.Drawing.Point(7, 15);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseFile.TabIndex = 5;
            this.btnBrowseFile.Text = "...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // btnBrowseDir
            // 
            this.btnBrowseDir.Image = global::Maestro.Editors.Properties.Resources.folder_horizontal;
            this.btnBrowseDir.Location = new System.Drawing.Point(40, 15);
            this.btnBrowseDir.Name = "btnBrowseDir";
            this.btnBrowseDir.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseDir.TabIndex = 6;
            this.btnBrowseDir.UseVisualStyleBackColor = true;
            this.btnBrowseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);
            // 
            // btnBrowseAliasDir
            // 
            this.btnBrowseAliasDir.Image = global::Maestro.Editors.Properties.Resources.folder_horizontal;
            this.btnBrowseAliasDir.Location = new System.Drawing.Point(39, 15);
            this.btnBrowseAliasDir.Name = "btnBrowseAliasDir";
            this.btnBrowseAliasDir.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseAliasDir.TabIndex = 8;
            this.btnBrowseAliasDir.UseVisualStyleBackColor = true;
            this.btnBrowseAliasDir.Click += new System.EventHandler(this.btnBrowseAliasDir_Click);
            // 
            // btnBrowseAliasFile
            // 
            this.btnBrowseAliasFile.Location = new System.Drawing.Point(6, 15);
            this.btnBrowseAliasFile.Name = "btnBrowseAliasFile";
            this.btnBrowseAliasFile.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseAliasFile.TabIndex = 7;
            this.btnBrowseAliasFile.Text = "...";
            this.btnBrowseAliasFile.UseVisualStyleBackColor = true;
            this.btnBrowseAliasFile.Click += new System.EventHandler(this.btnBrowseAliasFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBrowseFile);
            this.groupBox1.Controls.Add(this.btnBrowseDir);
            this.groupBox1.Location = new System.Drawing.Point(342, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(76, 44);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File System";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnBrowseAliasFile);
            this.groupBox2.Controls.Add(this.btnBrowseAliasDir);
            this.groupBox2.Location = new System.Drawing.Point(424, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(75, 44);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Alias";
            // 
            // SingleFileCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SingleFileCtrl";
            this.Size = new System.Drawing.Size(515, 255);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.Button btnBrowseDir;
        private System.Windows.Forms.Button btnBrowseAliasDir;
        private System.Windows.Forms.Button btnBrowseAliasFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
