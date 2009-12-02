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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePackage));
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
            this.LibraryLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ResourcePath
            // 
            resources.ApplyResources(this.ResourcePath, "ResourcePath");
            this.ResourcePath.Name = "ResourcePath";
            // 
            // BrowseResourcePath
            // 
            resources.ApplyResources(this.BrowseResourcePath, "BrowseResourcePath");
            this.BrowseResourcePath.Name = "BrowseResourcePath";
            this.BrowseResourcePath.UseVisualStyleBackColor = true;
            this.BrowseResourcePath.Click += new System.EventHandler(this.BrowseResourcePath_Click);
            // 
            // RemoveTargeOnRestore
            // 
            resources.ApplyResources(this.RemoveTargeOnRestore, "RemoveTargeOnRestore");
            this.RemoveTargeOnRestore.Name = "RemoveTargeOnRestore";
            this.RemoveTargeOnRestore.UseVisualStyleBackColor = true;
            // 
            // EnableRestorePath
            // 
            resources.ApplyResources(this.EnableRestorePath, "EnableRestorePath");
            this.EnableRestorePath.Name = "EnableRestorePath";
            this.EnableRestorePath.UseVisualStyleBackColor = true;
            this.EnableRestorePath.CheckedChanged += new System.EventHandler(this.EnableRestorePath_CheckedChanged);
            // 
            // RestorePath
            // 
            resources.ApplyResources(this.RestorePath, "RestorePath");
            this.RestorePath.Name = "RestorePath";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // AllowedTypes
            // 
            this.AllowedTypes.FormattingEnabled = true;
            resources.ApplyResources(this.AllowedTypes, "AllowedTypes");
            this.AllowedTypes.Name = "AllowedTypes";
            this.AllowedTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.AllowedTypes_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // BrowseTargetFilename
            // 
            resources.ApplyResources(this.BrowseTargetFilename, "BrowseTargetFilename");
            this.BrowseTargetFilename.Name = "BrowseTargetFilename";
            this.BrowseTargetFilename.UseVisualStyleBackColor = true;
            this.BrowseTargetFilename.Click += new System.EventHandler(this.BrowseTargetFilename_Click);
            // 
            // PackageFilename
            // 
            resources.ApplyResources(this.PackageFilename, "PackageFilename");
            this.PackageFilename.Name = "PackageFilename";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "mgp";
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // LibraryLabel
            // 
            resources.ApplyResources(this.LibraryLabel, "LibraryLabel");
            this.LibraryLabel.Name = "LibraryLabel";
            // 
            // CreatePackage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.LibraryLabel);
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
        private System.Windows.Forms.Label LibraryLabel;
    }
}