namespace Maestro.Editors.Migration
{
    partial class CopyMoveToServerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyMoveToServerDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstResources = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddResource = new System.Windows.Forms.ToolStripButton();
            this.btnAddFolder = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowseTarget = new System.Windows.Forms.Button();
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstResources);
            this.groupBox1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstResources
            // 
            resources.ApplyResources(this.lstResources, "lstResources");
            this.lstResources.FormattingEnabled = true;
            this.lstResources.Name = "lstResources";
            this.lstResources.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstResources.SelectedIndexChanged += new System.EventHandler(this.lstResources_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddResource,
            this.btnAddFolder,
            this.btnRemove});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddResource
            // 
            this.btnAddResource.Image = global::Maestro.Editors.Properties.Resources.document__plus;
            resources.ApplyResources(this.btnAddResource, "btnAddResource");
            this.btnAddResource.Name = "btnAddResource";
            this.btnAddResource.Click += new System.EventHandler(this.btnAddResource_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            resources.ApplyResources(this.btnAddFolder, "btnAddFolder");
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowseTarget);
            this.groupBox2.Controls.Add(this.txtTargetFolder);
            this.groupBox2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnBrowseTarget
            // 
            resources.ApplyResources(this.btnBrowseTarget, "btnBrowseTarget");
            this.btnBrowseTarget.Name = "btnBrowseTarget";
            this.btnBrowseTarget.UseVisualStyleBackColor = true;
            this.btnBrowseTarget.Click += new System.EventHandler(this.btnBrowseTarget_Click);
            // 
            // txtTargetFolder
            // 
            resources.ApplyResources(this.txtTargetFolder, "txtTargetFolder");
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkOverwrite);
            this.groupBox3.Controls.Add(this.cmbAction);
            this.groupBox3.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkOverwrite
            // 
            resources.ApplyResources(this.chkOverwrite, "chkOverwrite");
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            this.chkOverwrite.CheckedChanged += new System.EventHandler(this.chkOverwrite_CheckedChanged);
            // 
            // cmbAction
            // 
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            resources.ApplyResources(this.cmbAction, "cmbAction");
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.cmbAction_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CopyMoveToServerDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CopyMoveToServerDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.Windows.Forms.ComboBox cmbAction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstResources;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddResource;
        private System.Windows.Forms.ToolStripButton btnAddFolder;
        private System.Windows.Forms.ToolStripButton btnRemove;
        private System.Windows.Forms.Button btnBrowseTarget;
        private System.Windows.Forms.TextBox txtTargetFolder;
    }
}