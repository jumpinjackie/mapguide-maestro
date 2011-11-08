namespace Maestro.Editors.Packaging
{
    partial class AddResourceEntryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddResourceEntryDialog));
            this.UseHeader = new System.Windows.Forms.CheckBox();
            this.BrowseHeaderButton = new System.Windows.Forms.Button();
            this.HeaderPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ContentPath = new System.Windows.Forms.TextBox();
            this.BrowseContentButton = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.BrowseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AlternateName = new System.Windows.Forms.TextBox();
            this.UseAlternateName = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // UseHeader
            // 
            resources.ApplyResources(this.UseHeader, "UseHeader");
            this.UseHeader.Name = "UseHeader";
            this.UseHeader.UseVisualStyleBackColor = true;
            this.UseHeader.CheckedChanged += new System.EventHandler(this.UseHeader_CheckedChanged);
            // 
            // BrowseHeaderButton
            // 
            resources.ApplyResources(this.BrowseHeaderButton, "BrowseHeaderButton");
            this.BrowseHeaderButton.Name = "BrowseHeaderButton";
            this.BrowseHeaderButton.UseVisualStyleBackColor = true;
            this.BrowseHeaderButton.Click += new System.EventHandler(this.BrowseHeaderButton_Click);
            // 
            // HeaderPath
            // 
            resources.ApplyResources(this.HeaderPath, "HeaderPath");
            this.HeaderPath.Name = "HeaderPath";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ContentPath
            // 
            resources.ApplyResources(this.ContentPath, "ContentPath");
            this.ContentPath.Name = "ContentPath";
            this.ContentPath.TextChanged += new System.EventHandler(this.ContentPath_TextChanged);
            // 
            // BrowseContentButton
            // 
            resources.ApplyResources(this.BrowseContentButton, "BrowseContentButton");
            this.BrowseContentButton.Name = "BrowseContentButton";
            this.BrowseContentButton.UseVisualStyleBackColor = true;
            this.BrowseContentButton.Click += new System.EventHandler(this.BrowseContentButton_Click);
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // BrowseFileDialog
            // 
            resources.ApplyResources(this.BrowseFileDialog, "BrowseFileDialog");
            // 
            // AlternateName
            // 
            resources.ApplyResources(this.AlternateName, "AlternateName");
            this.AlternateName.Name = "AlternateName";
            // 
            // UseAlternateName
            // 
            resources.ApplyResources(this.UseAlternateName, "UseAlternateName");
            this.UseAlternateName.Name = "UseAlternateName";
            this.UseAlternateName.UseVisualStyleBackColor = true;
            this.UseAlternateName.CheckedChanged += new System.EventHandler(this.UseAlternateName_CheckedChanged);
            // 
            // AddResourceEntryDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.AlternateName);
            this.Controls.Add(this.UseAlternateName);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.BrowseContentButton);
            this.Controls.Add(this.ContentPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HeaderPath);
            this.Controls.Add(this.BrowseHeaderButton);
            this.Controls.Add(this.UseHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddResourceEntryDialog";
            this.Load += new System.EventHandler(this.AddResourceEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox UseHeader;
        private System.Windows.Forms.Button BrowseHeaderButton;
        private System.Windows.Forms.TextBox HeaderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ContentPath;
        private System.Windows.Forms.Button BrowseContentButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.OpenFileDialog BrowseFileDialog;
        private System.Windows.Forms.TextBox AlternateName;
        private System.Windows.Forms.CheckBox UseAlternateName;
    }
}