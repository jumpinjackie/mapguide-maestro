namespace Maestro.Base.UI.Packaging
{
    partial class CreatePackageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePackageDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBrowseRestorePath = new System.Windows.Forms.Button();
            this.lnkAll = new System.Windows.Forms.LinkLabel();
            this.BrowseTargetFilename = new System.Windows.Forms.Button();
            this.txtPackageFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAllowedTypes = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRestorePath = new System.Windows.Forms.TextBox();
            this.chkRestorePath = new System.Windows.Forms.CheckBox();
            this.chkRemoveTargetFolderOnRestore = new System.Windows.Forms.CheckBox();
            this.BrowseResourcePath = new System.Windows.Forms.Button();
            this.txtResourcePath = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdPackageFolder = new System.Windows.Forms.RadioButton();
            this.rdResourceList = new System.Windows.Forms.RadioButton();
            this.txtResourceIdList = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnBrowseRestorePath
            // 
            resources.ApplyResources(this.btnBrowseRestorePath, "btnBrowseRestorePath");
            this.btnBrowseRestorePath.Name = "btnBrowseRestorePath";
            this.btnBrowseRestorePath.UseVisualStyleBackColor = true;
            this.btnBrowseRestorePath.Click += new System.EventHandler(this.btnBrowseRestorePath_Click);
            // 
            // lnkAll
            // 
            resources.ApplyResources(this.lnkAll, "lnkAll");
            this.lnkAll.Name = "lnkAll";
            this.lnkAll.TabStop = true;
            this.lnkAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAll_LinkClicked);
            // 
            // BrowseTargetFilename
            // 
            resources.ApplyResources(this.BrowseTargetFilename, "BrowseTargetFilename");
            this.BrowseTargetFilename.Name = "BrowseTargetFilename";
            this.BrowseTargetFilename.UseVisualStyleBackColor = true;
            this.BrowseTargetFilename.Click += new System.EventHandler(this.BrowseTargetFilename_Click);
            // 
            // txtPackageFilename
            // 
            resources.ApplyResources(this.txtPackageFilename, "txtPackageFilename");
            this.txtPackageFilename.Name = "txtPackageFilename";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkAllowedTypes
            // 
            resources.ApplyResources(this.chkAllowedTypes, "chkAllowedTypes");
            this.chkAllowedTypes.CheckOnClick = true;
            this.chkAllowedTypes.FormattingEnabled = true;
            this.chkAllowedTypes.Name = "chkAllowedTypes";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtRestorePath
            // 
            resources.ApplyResources(this.txtRestorePath, "txtRestorePath");
            this.txtRestorePath.Name = "txtRestorePath";
            // 
            // chkRestorePath
            // 
            resources.ApplyResources(this.chkRestorePath, "chkRestorePath");
            this.chkRestorePath.Name = "chkRestorePath";
            this.chkRestorePath.UseVisualStyleBackColor = true;
            this.chkRestorePath.CheckedChanged += new System.EventHandler(this.chkRestorePath_CheckedChanged);
            // 
            // chkRemoveTargetFolderOnRestore
            // 
            resources.ApplyResources(this.chkRemoveTargetFolderOnRestore, "chkRemoveTargetFolderOnRestore");
            this.chkRemoveTargetFolderOnRestore.Name = "chkRemoveTargetFolderOnRestore";
            this.chkRemoveTargetFolderOnRestore.UseVisualStyleBackColor = true;
            // 
            // BrowseResourcePath
            // 
            resources.ApplyResources(this.BrowseResourcePath, "BrowseResourcePath");
            this.BrowseResourcePath.Name = "BrowseResourcePath";
            this.BrowseResourcePath.UseVisualStyleBackColor = true;
            this.BrowseResourcePath.Click += new System.EventHandler(this.BrowseResourcePath_Click);
            // 
            // txtResourcePath
            // 
            resources.ApplyResources(this.txtResourcePath, "txtResourcePath");
            this.txtResourcePath.Name = "txtResourcePath";
            this.txtResourcePath.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.txtPackageFilename);
            this.groupBox1.Controls.Add(this.btnBrowseRestorePath);
            this.groupBox1.Controls.Add(this.chkRemoveTargetFolderOnRestore);
            this.groupBox1.Controls.Add(this.lnkAll);
            this.groupBox1.Controls.Add(this.chkRestorePath);
            this.groupBox1.Controls.Add(this.BrowseTargetFilename);
            this.groupBox1.Controls.Add(this.txtRestorePath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkAllowedTypes);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // rdPackageFolder
            // 
            resources.ApplyResources(this.rdPackageFolder, "rdPackageFolder");
            this.rdPackageFolder.Checked = true;
            this.rdPackageFolder.Name = "rdPackageFolder";
            this.rdPackageFolder.TabStop = true;
            this.rdPackageFolder.UseVisualStyleBackColor = true;
            // 
            // rdResourceList
            // 
            resources.ApplyResources(this.rdResourceList, "rdResourceList");
            this.rdResourceList.Name = "rdResourceList";
            this.rdResourceList.UseVisualStyleBackColor = true;
            // 
            // txtResourceIdList
            // 
            resources.ApplyResources(this.txtResourceIdList, "txtResourceIdList");
            this.txtResourceIdList.Name = "txtResourceIdList";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtResourcePath);
            this.groupBox2.Controls.Add(this.txtResourceIdList);
            this.groupBox2.Controls.Add(this.BrowseResourcePath);
            this.groupBox2.Controls.Add(this.rdResourceList);
            this.groupBox2.Controls.Add(this.rdPackageFolder);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CreatePackageDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "CreatePackageDialog";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button BrowseTargetFilename;
        private System.Windows.Forms.TextBox txtPackageFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chkAllowedTypes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRestorePath;
        private System.Windows.Forms.CheckBox chkRestorePath;
        private System.Windows.Forms.CheckBox chkRemoveTargetFolderOnRestore;
        private System.Windows.Forms.Button BrowseResourcePath;
        private System.Windows.Forms.TextBox txtResourcePath;
        private System.Windows.Forms.LinkLabel lnkAll;
        private System.Windows.Forms.Button btnBrowseRestorePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtResourceIdList;
        private System.Windows.Forms.RadioButton rdResourceList;
        private System.Windows.Forms.RadioButton rdPackageFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}