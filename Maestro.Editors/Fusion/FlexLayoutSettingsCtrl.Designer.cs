namespace Maestro.Editors.Fusion
{
    partial class FlexLayoutSettingsCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexLayoutSettingsCtrl));
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lstTemplates = new System.Windows.Forms.ListView();
            tplImageList = new System.Windows.Forms.ImageList(components);
            txtTitle = new System.Windows.Forms.TextBox();
            txtTemplateUrl = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            btnShowInBrowser = new System.Windows.Forms.Button();
            cmbPublicUrl = new System.Windows.Forms.ComboBox();
            btnCopyClipboard = new System.Windows.Forms.Button();
            btnManageProjections = new System.Windows.Forms.Button();
            contentPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(btnManageProjections);
            contentPanel.Controls.Add(btnCopyClipboard);
            contentPanel.Controls.Add(cmbPublicUrl);
            contentPanel.Controls.Add(btnShowInBrowser);
            contentPanel.Controls.Add(label3);
            contentPanel.Controls.Add(txtTemplateUrl);
            contentPanel.Controls.Add(txtTitle);
            contentPanel.Controls.Add(groupBox1);
            contentPanel.Controls.Add(label2);
            contentPanel.Controls.Add(label1);
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(lstTemplates);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // lstTemplates
            // 
            resources.ApplyResources(lstTemplates, "lstTemplates");
            lstTemplates.LargeImageList = tplImageList;
            lstTemplates.MultiSelect = false;
            lstTemplates.Name = "lstTemplates";
            lstTemplates.UseCompatibleStateImageBehavior = false;
            lstTemplates.SelectedIndexChanged += lstTemplates_SelectedIndexChanged;
            // 
            // tplImageList
            // 
            tplImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(tplImageList, "tplImageList");
            tplImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtTitle
            // 
            resources.ApplyResources(txtTitle, "txtTitle");
            txtTitle.Name = "txtTitle";
            // 
            // txtTemplateUrl
            // 
            resources.ApplyResources(txtTemplateUrl, "txtTemplateUrl");
            txtTemplateUrl.Name = "txtTemplateUrl";
            txtTemplateUrl.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // btnShowInBrowser
            // 
            resources.ApplyResources(btnShowInBrowser, "btnShowInBrowser");
            btnShowInBrowser.Name = "btnShowInBrowser";
            btnShowInBrowser.UseVisualStyleBackColor = true;
            btnShowInBrowser.Click += btnShowInBrowser_Click;
            // 
            // cmbPublicUrl
            // 
            resources.ApplyResources(cmbPublicUrl, "cmbPublicUrl");
            cmbPublicUrl.DisplayMember = "Display";
            cmbPublicUrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPublicUrl.FormattingEnabled = true;
            cmbPublicUrl.Name = "cmbPublicUrl";
            // 
            // btnCopyClipboard
            // 
            resources.ApplyResources(btnCopyClipboard, "btnCopyClipboard");
            btnCopyClipboard.Name = "btnCopyClipboard";
            btnCopyClipboard.UseVisualStyleBackColor = true;
            btnCopyClipboard.Click += btnCopyClipboard_Click;
            // 
            // btnManageProjections
            // 
            resources.ApplyResources(btnManageProjections, "btnManageProjections");
            btnManageProjections.Name = "btnManageProjections";
            btnManageProjections.UseVisualStyleBackColor = true;
            btnManageProjections.Click += btnManageProjections_Click;
            // 
            // FlexLayoutSettingsCtrl
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            Name = "FlexLayoutSettingsCtrl";
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lstTemplates;
        private System.Windows.Forms.ImageList tplImageList;
        private System.Windows.Forms.TextBox txtTemplateUrl;
        private System.Windows.Forms.Button btnShowInBrowser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPublicUrl;
        private System.Windows.Forms.Button btnCopyClipboard;
        private System.Windows.Forms.Button btnManageProjections;
    }
}
