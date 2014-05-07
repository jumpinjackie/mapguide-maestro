namespace Maestro.Base.UI
{
    partial class ResourceDependencyListDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceDependencyListDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.lstUpstreamDependencies = new System.Windows.Forms.ListBox();
            this.btnSaveUpList = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveDownList = new System.Windows.Forms.Button();
            this.lstDownstreamDependencies = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstSelectedResources = new System.Windows.Forms.ListBox();
            this.chkIncludeSelected = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lstUpstreamDependencies
            // 
            resources.ApplyResources(this.lstUpstreamDependencies, "lstUpstreamDependencies");
            this.lstUpstreamDependencies.FormattingEnabled = true;
            this.lstUpstreamDependencies.Name = "lstUpstreamDependencies";
            // 
            // btnSaveUpList
            // 
            resources.ApplyResources(this.btnSaveUpList, "btnSaveUpList");
            this.btnSaveUpList.Name = "btnSaveUpList";
            this.btnSaveUpList.UseVisualStyleBackColor = true;
            this.btnSaveUpList.Click += new System.EventHandler(this.btnSaveUpList_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveDownList
            // 
            resources.ApplyResources(this.btnSaveDownList, "btnSaveDownList");
            this.btnSaveDownList.Name = "btnSaveDownList";
            this.btnSaveDownList.UseVisualStyleBackColor = true;
            this.btnSaveDownList.Click += new System.EventHandler(this.btnSaveDownList_Click);
            // 
            // lstDownstreamDependencies
            // 
            resources.ApplyResources(this.lstDownstreamDependencies, "lstDownstreamDependencies");
            this.lstDownstreamDependencies.FormattingEnabled = true;
            this.lstDownstreamDependencies.Name = "lstDownstreamDependencies";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lstUpstreamDependencies);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.lstDownstreamDependencies);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.lstSelectedResources);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lstSelectedResources
            // 
            resources.ApplyResources(this.lstSelectedResources, "lstSelectedResources");
            this.lstSelectedResources.FormattingEnabled = true;
            this.lstSelectedResources.Name = "lstSelectedResources";
            // 
            // chkIncludeSelected
            // 
            resources.ApplyResources(this.chkIncludeSelected, "chkIncludeSelected");
            this.chkIncludeSelected.Checked = true;
            this.chkIncludeSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeSelected.Name = "chkIncludeSelected";
            this.chkIncludeSelected.UseVisualStyleBackColor = true;
            // 
            // ResourceDependencyListDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.chkIncludeSelected);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSaveDownList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveUpList);
            this.Controls.Add(this.label1);
            this.Name = "ResourceDependencyListDialog";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstUpstreamDependencies;
        private System.Windows.Forms.Button btnSaveUpList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveDownList;
        private System.Windows.Forms.ListBox lstDownstreamDependencies;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstSelectedResources;
        private System.Windows.Forms.CheckBox chkIncludeSelected;
    }
}