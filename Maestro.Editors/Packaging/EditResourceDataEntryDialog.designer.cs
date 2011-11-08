namespace Maestro.Editors.Packaging
{
    partial class EditResourceDataEntryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditResourceDataEntryDialog));
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
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ResourceNameBox
            // 
            resources.ApplyResources(this.ResourceNameBox, "ResourceNameBox");
            this.ResourceNameBox.Name = "ResourceNameBox";
            this.ResourceNameBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ContentTypeBox
            // 
            resources.ApplyResources(this.ContentTypeBox, "ContentTypeBox");
            this.ContentTypeBox.FormattingEnabled = true;
            this.ContentTypeBox.Items.AddRange(new object[] {
            resources.GetString("ContentTypeBox.Items"),
            resources.GetString("ContentTypeBox.Items1")});
            this.ContentTypeBox.Name = "ContentTypeBox";
            this.ContentTypeBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // DataTypeBox
            // 
            resources.ApplyResources(this.DataTypeBox, "DataTypeBox");
            this.DataTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataTypeBox.FormattingEnabled = true;
            this.DataTypeBox.Items.AddRange(new object[] {
            resources.GetString("DataTypeBox.Items"),
            resources.GetString("DataTypeBox.Items1")});
            this.DataTypeBox.Name = "DataTypeBox";
            this.DataTypeBox.SelectedIndexChanged += new System.EventHandler(this.ValidateForm);
            this.DataTypeBox.TextChanged += new System.EventHandler(this.ValidateForm);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // FilenameBox
            // 
            resources.ApplyResources(this.FilenameBox, "FilenameBox");
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.ReadOnly = true;
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
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // EditResourceDataEntryDialog
            // 
            this.AcceptButton = this.OKBtn;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
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
            this.Name = "EditResourceDataEntryDialog";
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