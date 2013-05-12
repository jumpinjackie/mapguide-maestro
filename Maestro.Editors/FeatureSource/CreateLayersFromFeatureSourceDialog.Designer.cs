namespace Maestro.Editors.FeatureSource
{
    partial class CreateLayersFromFeatureSourceDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateLayersFromFeatureSourceDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCreateTargetFolder = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstFeatureClasses = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFeatureSource = new System.Windows.Forms.Button();
            this.btnCreateTarget = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lnkClear = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtFeatureSource
            // 
            resources.ApplyResources(this.txtFeatureSource, "txtFeatureSource");
            this.txtFeatureSource.Name = "txtFeatureSource";
            this.txtFeatureSource.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtCreateTargetFolder
            // 
            resources.ApplyResources(this.txtCreateTargetFolder, "txtCreateTargetFolder");
            this.txtCreateTargetFolder.Name = "txtCreateTargetFolder";
            this.txtCreateTargetFolder.ReadOnly = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lnkClear);
            this.groupBox1.Controls.Add(this.lnkSelectAll);
            this.groupBox1.Controls.Add(this.lstFeatureClasses);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstFeatureClasses
            // 
            resources.ApplyResources(this.lstFeatureClasses, "lstFeatureClasses");
            this.lstFeatureClasses.FormattingEnabled = true;
            this.lstFeatureClasses.Name = "lstFeatureClasses";
            this.lstFeatureClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstFeatureClasses.SelectedIndexChanged += new System.EventHandler(this.lstFeatureClasses_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnFeatureSource
            // 
            resources.ApplyResources(this.btnFeatureSource, "btnFeatureSource");
            this.btnFeatureSource.Name = "btnFeatureSource";
            this.btnFeatureSource.UseVisualStyleBackColor = true;
            this.btnFeatureSource.Click += new System.EventHandler(this.btnFeatureSource_Click);
            // 
            // btnCreateTarget
            // 
            resources.ApplyResources(this.btnCreateTarget, "btnCreateTarget");
            this.btnCreateTarget.Name = "btnCreateTarget";
            this.btnCreateTarget.UseVisualStyleBackColor = true;
            this.btnCreateTarget.Click += new System.EventHandler(this.btnCreateTarget_Click);
            // 
            // btnCreate
            // 
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lnkSelectAll
            // 
            resources.ApplyResources(this.lnkSelectAll, "lnkSelectAll");
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // lnkClear
            // 
            resources.ApplyResources(this.lnkClear, "lnkClear");
            this.lnkClear.Name = "lnkClear";
            this.lnkClear.TabStop = true;
            this.lnkClear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClear_LinkClicked);
            // 
            // CreateLayersFromFeatureSourceDialog
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCreateTarget);
            this.Controls.Add(this.btnFeatureSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtCreateTargetFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFeatureSource);
            this.Controls.Add(this.label1);
            this.Name = "CreateLayersFromFeatureSourceDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCreateTargetFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstFeatureClasses;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFeatureSource;
        private System.Windows.Forms.Button btnCreateTarget;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel lnkClear;
        private System.Windows.Forms.LinkLabel lnkSelectAll;
    }
}