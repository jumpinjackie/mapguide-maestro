namespace OSGeo.MapGuide.Maestro.PackageManager
{
    partial class AddResourceEntry
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
            this.UseHeader.AutoSize = true;
            this.UseHeader.Location = new System.Drawing.Point(8, 8);
            this.UseHeader.Name = "UseHeader";
            this.UseHeader.Size = new System.Drawing.Size(77, 17);
            this.UseHeader.TabIndex = 0;
            this.UseHeader.Text = "Header file";
            this.UseHeader.UseVisualStyleBackColor = true;
            this.UseHeader.CheckedChanged += new System.EventHandler(this.UseHeader_CheckedChanged);
            // 
            // BrowseHeaderButton
            // 
            this.BrowseHeaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseHeaderButton.Enabled = false;
            this.BrowseHeaderButton.Location = new System.Drawing.Point(337, 8);
            this.BrowseHeaderButton.Name = "BrowseHeaderButton";
            this.BrowseHeaderButton.Size = new System.Drawing.Size(24, 20);
            this.BrowseHeaderButton.TabIndex = 1;
            this.BrowseHeaderButton.Text = "...";
            this.BrowseHeaderButton.UseVisualStyleBackColor = true;
            this.BrowseHeaderButton.Click += new System.EventHandler(this.BrowseHeaderButton_Click);
            // 
            // HeaderPath
            // 
            this.HeaderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPath.Enabled = false;
            this.HeaderPath.Location = new System.Drawing.Point(112, 8);
            this.HeaderPath.Name = "HeaderPath";
            this.HeaderPath.Size = new System.Drawing.Size(225, 20);
            this.HeaderPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Content file";
            // 
            // ContentPath
            // 
            this.ContentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentPath.Location = new System.Drawing.Point(112, 40);
            this.ContentPath.Name = "ContentPath";
            this.ContentPath.Size = new System.Drawing.Size(225, 20);
            this.ContentPath.TabIndex = 4;
            this.ContentPath.TextChanged += new System.EventHandler(this.ContentPath_TextChanged);
            // 
            // BrowseContentButton
            // 
            this.BrowseContentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseContentButton.Location = new System.Drawing.Point(337, 40);
            this.BrowseContentButton.Name = "BrowseContentButton";
            this.BrowseContentButton.Size = new System.Drawing.Size(24, 20);
            this.BrowseContentButton.TabIndex = 5;
            this.BrowseContentButton.Text = "...";
            this.BrowseContentButton.UseVisualStyleBackColor = true;
            this.BrowseContentButton.Click += new System.EventHandler(this.BrowseContentButton_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.Location = new System.Drawing.Point(108, 104);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(196, 104);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // BrowseFileDialog
            // 
            this.BrowseFileDialog.Filter = "All files (*.*)|*.*";
            this.BrowseFileDialog.Title = "Select the file to use";
            // 
            // AlternateName
            // 
            this.AlternateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AlternateName.Enabled = false;
            this.AlternateName.Location = new System.Drawing.Point(112, 72);
            this.AlternateName.Name = "AlternateName";
            this.AlternateName.Size = new System.Drawing.Size(225, 20);
            this.AlternateName.TabIndex = 9;
            // 
            // UseAlternateName
            // 
            this.UseAlternateName.AutoSize = true;
            this.UseAlternateName.Location = new System.Drawing.Point(8, 72);
            this.UseAlternateName.Name = "UseAlternateName";
            this.UseAlternateName.Size = new System.Drawing.Size(97, 17);
            this.UseAlternateName.TabIndex = 8;
            this.UseAlternateName.Text = "Alternate name";
            this.UseAlternateName.UseVisualStyleBackColor = true;
            this.UseAlternateName.CheckedChanged += new System.EventHandler(this.UseAlternateName_CheckedChanged);
            // 
            // AddResourceEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(371, 135);
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
            this.Name = "AddResourceEntry";
            this.Text = "Add a resource";
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