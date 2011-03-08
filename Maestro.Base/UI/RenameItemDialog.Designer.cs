namespace Maestro.Base.UI
{
    partial class RenameItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameItemDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.chkUpdateRefs = new System.Windows.Forms.CheckBox();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtOld = new System.Windows.Forms.TextBox();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.lblExists = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkOverwrite
            // 
            resources.ApplyResources(this.chkOverwrite, "chkOverwrite");
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            // 
            // chkUpdateRefs
            // 
            resources.ApplyResources(this.chkUpdateRefs, "chkUpdateRefs");
            this.chkUpdateRefs.Checked = true;
            this.chkUpdateRefs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateRefs.Name = "chkUpdateRefs";
            this.chkUpdateRefs.UseVisualStyleBackColor = true;
            this.chkUpdateRefs.CheckedChanged += new System.EventHandler(this.chkUpdateRefs_CheckedChanged);
            // 
            // btnRename
            // 
            resources.ApplyResources(this.btnRename, "btnRename");
            this.btnRename.Name = "btnRename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtOld
            // 
            resources.ApplyResources(this.txtOld, "txtOld");
            this.txtOld.Name = "txtOld";
            this.txtOld.ReadOnly = true;
            // 
            // txtNew
            // 
            resources.ApplyResources(this.txtNew, "txtNew");
            this.txtNew.Name = "txtNew";
            this.txtNew.TextChanged += new System.EventHandler(this.txtNew_TextChanged);
            this.txtNew.MouseLeave += new System.EventHandler(this.txtNew_MouseLeave);
            // 
            // lblExists
            // 
            resources.ApplyResources(this.lblExists, "lblExists");
            this.lblExists.ForeColor = System.Drawing.Color.Red;
            this.lblExists.Name = "lblExists";
            // 
            // RenameItemDialog
            // 
            this.AcceptButton = this.btnRename;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.lblExists);
            this.Controls.Add(this.txtNew);
            this.Controls.Add(this.txtOld);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.chkUpdateRefs);
            this.Controls.Add(this.chkOverwrite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RenameItemDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.Windows.Forms.CheckBox chkUpdateRefs;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtOld;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Label lblExists;
    }
}