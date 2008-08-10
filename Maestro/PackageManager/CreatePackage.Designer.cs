namespace OSGeo.MapGuide.Maestro.PackageManager
{
    partial class CreatePackage
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
            this.label1 = new System.Windows.Forms.Label();
            this.ResourcePath = new System.Windows.Forms.TextBox();
            this.BrowseResourcePath = new System.Windows.Forms.Button();
            this.RemoveTargeOnRestore = new System.Windows.Forms.CheckBox();
            this.EnableRestorePath = new System.Windows.Forms.CheckBox();
            this.RestorePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AllowedTypes = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.BrowseTargetFilename = new System.Windows.Forms.Button();
            this.PackageFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting point";
            // 
            // ResourcePath
            // 
            this.ResourcePath.Location = new System.Drawing.Point(128, 16);
            this.ResourcePath.Name = "ResourcePath";
            this.ResourcePath.Size = new System.Drawing.Size(216, 20);
            this.ResourcePath.TabIndex = 1;
            // 
            // BrowseResourcePath
            // 
            this.BrowseResourcePath.Location = new System.Drawing.Point(344, 16);
            this.BrowseResourcePath.Name = "BrowseResourcePath";
            this.BrowseResourcePath.Size = new System.Drawing.Size(24, 20);
            this.BrowseResourcePath.TabIndex = 2;
            this.BrowseResourcePath.Text = "...";
            this.BrowseResourcePath.UseVisualStyleBackColor = true;
            this.BrowseResourcePath.Click += new System.EventHandler(this.BrowseResourcePath_Click);
            // 
            // RemoveTargeOnRestore
            // 
            this.RemoveTargeOnRestore.AutoSize = true;
            this.RemoveTargeOnRestore.Location = new System.Drawing.Point(16, 96);
            this.RemoveTargeOnRestore.Name = "RemoveTargeOnRestore";
            this.RemoveTargeOnRestore.Size = new System.Drawing.Size(197, 17);
            this.RemoveTargeOnRestore.TabIndex = 3;
            this.RemoveTargeOnRestore.Text = "Remove target folder when restoring";
            this.RemoveTargeOnRestore.UseVisualStyleBackColor = true;
            // 
            // EnableRestorePath
            // 
            this.EnableRestorePath.AutoSize = true;
            this.EnableRestorePath.Enabled = false;
            this.EnableRestorePath.Location = new System.Drawing.Point(16, 72);
            this.EnableRestorePath.Name = "EnableRestorePath";
            this.EnableRestorePath.Size = new System.Drawing.Size(87, 17);
            this.EnableRestorePath.TabIndex = 4;
            this.EnableRestorePath.Text = "Restore path";
            this.EnableRestorePath.UseVisualStyleBackColor = true;
            this.EnableRestorePath.CheckedChanged += new System.EventHandler(this.EnableRestorePath_CheckedChanged);
            // 
            // RestorePath
            // 
            this.RestorePath.Enabled = false;
            this.RestorePath.Location = new System.Drawing.Point(128, 72);
            this.RestorePath.Name = "RestorePath";
            this.RestorePath.Size = new System.Drawing.Size(216, 20);
            this.RestorePath.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Include these types in the package";
            // 
            // AllowedTypes
            // 
            this.AllowedTypes.FormattingEnabled = true;
            this.AllowedTypes.Location = new System.Drawing.Point(16, 144);
            this.AllowedTypes.Name = "AllowedTypes";
            this.AllowedTypes.Size = new System.Drawing.Size(352, 109);
            this.AllowedTypes.TabIndex = 7;
            this.AllowedTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.AllowedTypes_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 274);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 39);
            this.panel1.TabIndex = 8;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(191, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 24);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OKBtn.Location = new System.Drawing.Point(95, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 24);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // BrowseTargetFilename
            // 
            this.BrowseTargetFilename.Location = new System.Drawing.Point(344, 40);
            this.BrowseTargetFilename.Name = "BrowseTargetFilename";
            this.BrowseTargetFilename.Size = new System.Drawing.Size(24, 20);
            this.BrowseTargetFilename.TabIndex = 11;
            this.BrowseTargetFilename.Text = "...";
            this.BrowseTargetFilename.UseVisualStyleBackColor = true;
            this.BrowseTargetFilename.Click += new System.EventHandler(this.BrowseTargetFilename_Click);
            // 
            // PackageFilename
            // 
            this.PackageFilename.Location = new System.Drawing.Point(128, 40);
            this.PackageFilename.Name = "PackageFilename";
            this.PackageFilename.Size = new System.Drawing.Size(216, 20);
            this.PackageFilename.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "File name";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "mgp";
            this.saveFileDialog.Filter = "MapGuide Packages (*.mgp)|*.mgp|Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            this.saveFileDialog.Title = "Select where to store the file";
            // 
            // CreatePackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(383, 313);
            this.Controls.Add(this.BrowseTargetFilename);
            this.Controls.Add(this.PackageFilename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AllowedTypes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RestorePath);
            this.Controls.Add(this.EnableRestorePath);
            this.Controls.Add(this.RemoveTargeOnRestore);
            this.Controls.Add(this.BrowseResourcePath);
            this.Controls.Add(this.ResourcePath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreatePackage";
            this.Text = "Create a new package";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResourcePath;
        private System.Windows.Forms.Button BrowseResourcePath;
        private System.Windows.Forms.CheckBox RemoveTargeOnRestore;
        private System.Windows.Forms.CheckBox EnableRestorePath;
        private System.Windows.Forms.TextBox RestorePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox AllowedTypes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button BrowseTargetFilename;
        private System.Windows.Forms.TextBox PackageFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}