namespace Maestro.Base
{
    partial class ValidationResultsDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationResultsDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkNotices = new System.Windows.Forms.CheckBox();
            this.chkWarnings = new System.Windows.Forms.CheckBox();
            this.chkErrors = new System.Windows.Forms.CheckBox();
            this.SaveReportBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.lstIssues = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnOpen = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.chkNotices);
            this.panel1.Controls.Add(this.chkWarnings);
            this.panel1.Controls.Add(this.chkErrors);
            this.panel1.Controls.Add(this.SaveReportBtn);
            this.panel1.Controls.Add(this.CancelBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // chkNotices
            // 
            resources.ApplyResources(this.chkNotices, "chkNotices");
            this.chkNotices.Checked = true;
            this.chkNotices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotices.Name = "chkNotices";
            this.chkNotices.UseVisualStyleBackColor = true;
            this.chkNotices.CheckedChanged += new System.EventHandler(this.OnResultFilterCheckedChanged);
            // 
            // chkWarnings
            // 
            resources.ApplyResources(this.chkWarnings, "chkWarnings");
            this.chkWarnings.Checked = true;
            this.chkWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarnings.Name = "chkWarnings";
            this.chkWarnings.UseVisualStyleBackColor = true;
            this.chkWarnings.CheckedChanged += new System.EventHandler(this.OnResultFilterCheckedChanged);
            // 
            // chkErrors
            // 
            resources.ApplyResources(this.chkErrors, "chkErrors");
            this.chkErrors.Checked = true;
            this.chkErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkErrors.Name = "chkErrors";
            this.chkErrors.UseVisualStyleBackColor = true;
            this.chkErrors.CheckedChanged += new System.EventHandler(this.OnResultFilterCheckedChanged);
            // 
            // SaveReportBtn
            // 
            resources.ApplyResources(this.SaveReportBtn, "SaveReportBtn");
            this.SaveReportBtn.Name = "SaveReportBtn";
            this.SaveReportBtn.UseVisualStyleBackColor = true;
            this.SaveReportBtn.Click += new System.EventHandler(this.SaveReportBtn_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // lstIssues
            // 
            this.lstIssues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            resources.ApplyResources(this.lstIssues, "lstIssues");
            this.lstIssues.GridLines = true;
            this.lstIssues.MultiSelect = false;
            this.lstIssues.Name = "lstIssues";
            this.lstIssues.SmallImageList = this.imageList1;
            this.lstIssues.UseCompatibleStateImageBehavior = false;
            this.lstIssues.View = System.Windows.Forms.View.Details;
            this.lstIssues.SelectedIndexChanged += new System.EventHandler(this.lstIssues_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Information.ico");
            this.imageList1.Images.SetKeyName(1, "Warning.ico");
            this.imageList1.Images.SetKeyName(2, "Error.ico");
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // btnOpen
            // 
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // ValidationResultsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.CancelBtn;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lstIssues);
            this.Controls.Add(this.panel1);
            this.Name = "ValidationResultsDialog";
            this.ShowIcon = false;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ListView lstIssues;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button SaveReportBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckBox chkNotices;
        private System.Windows.Forms.CheckBox chkWarnings;
        private System.Windows.Forms.CheckBox chkErrors;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnOpen;
    }
}