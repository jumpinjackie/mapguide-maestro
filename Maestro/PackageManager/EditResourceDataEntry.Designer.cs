namespace OSGeo.MapGuide.Maestro.PackageManager
{
    partial class EditResourceDataEntry
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
            this.ResourceNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ContentTypeBox = new System.Windows.Forms.ComboBox();
            this.DataTypeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // ResourceNameBox
            // 
            this.ResourceNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceNameBox.Location = new System.Drawing.Point(80, 8);
            this.ResourceNameBox.Name = "ResourceNameBox";
            this.ResourceNameBox.Size = new System.Drawing.Size(200, 20);
            this.ResourceNameBox.TabIndex = 1;
            this.ResourceNameBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Content type";
            // 
            // ContentTypeBox
            // 
            this.ContentTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentTypeBox.FormattingEnabled = true;
            this.ContentTypeBox.Items.AddRange(new object[] {
            "text/xml",
            "application/octet-stream"});
            this.ContentTypeBox.Location = new System.Drawing.Point(80, 32);
            this.ContentTypeBox.Name = "ContentTypeBox";
            this.ContentTypeBox.Size = new System.Drawing.Size(200, 21);
            this.ContentTypeBox.TabIndex = 3;
            this.ContentTypeBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // DataTypeBox
            // 
            this.DataTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataTypeBox.FormattingEnabled = true;
            this.DataTypeBox.Items.AddRange(new object[] {
            "File",
            "Stream"});
            this.DataTypeBox.Location = new System.Drawing.Point(80, 56);
            this.DataTypeBox.Name = "DataTypeBox";
            this.DataTypeBox.Size = new System.Drawing.Size(200, 21);
            this.DataTypeBox.TabIndex = 5;
            this.DataTypeBox.SelectedIndexChanged += new System.EventHandler(this.ValidateForm);
            this.DataTypeBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Filename";
            // 
            // FilenameBox
            // 
            this.FilenameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FilenameBox.Location = new System.Drawing.Point(80, 80);
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.ReadOnly = true;
            this.FilenameBox.Size = new System.Drawing.Size(200, 20);
            this.FilenameBox.TabIndex = 7;
            // 
            // OKBtn
            // 
            this.OKBtn.Enabled = false;
            this.OKBtn.Location = new System.Drawing.Point(64, 112);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(72, 24);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(152, 112);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(72, 24);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // EditResourceDataEntry
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(289, 144);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.FilenameBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DataTypeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ContentTypeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ResourceNameBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditResourceDataEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit resourcedata details";
            this.Load += new System.EventHandler(this.EditResourceDataEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResourceNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ContentTypeBox;
        private System.Windows.Forms.ComboBox DataTypeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}