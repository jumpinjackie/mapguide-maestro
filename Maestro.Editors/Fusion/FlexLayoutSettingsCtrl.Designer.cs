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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexLayoutSettingsCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstTemplates = new System.Windows.Forms.ListView();
            this.tplImageList = new System.Windows.Forms.ImageList(this.components);
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtTemplateUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPublicUrl = new System.Windows.Forms.TextBox();
            this.btnShowInBrowser = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnShowInBrowser);
            this.contentPanel.Controls.Add(this.txtPublicUrl);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.txtTemplateUrl);
            this.contentPanel.Controls.Add(this.txtTitle);
            this.contentPanel.Controls.Add(this.groupBox1);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.label1);
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lstTemplates);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstTemplates
            // 
            resources.ApplyResources(this.lstTemplates, "lstTemplates");
            this.lstTemplates.LargeImageList = this.tplImageList;
            this.lstTemplates.MultiSelect = false;
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.UseCompatibleStateImageBehavior = false;
            this.lstTemplates.SelectedIndexChanged += new System.EventHandler(this.lstTemplates_SelectedIndexChanged);
            // 
            // tplImageList
            // 
            this.tplImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.tplImageList, "tplImageList");
            this.tplImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtTitle
            // 
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            // 
            // txtTemplateUrl
            // 
            resources.ApplyResources(this.txtTemplateUrl, "txtTemplateUrl");
            this.txtTemplateUrl.Name = "txtTemplateUrl";
            this.txtTemplateUrl.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtPublicUrl
            // 
            resources.ApplyResources(this.txtPublicUrl, "txtPublicUrl");
            this.txtPublicUrl.Name = "txtPublicUrl";
            this.txtPublicUrl.ReadOnly = true;
            // 
            // btnShowInBrowser
            // 
            resources.ApplyResources(this.btnShowInBrowser, "btnShowInBrowser");
            this.btnShowInBrowser.Name = "btnShowInBrowser";
            this.btnShowInBrowser.UseVisualStyleBackColor = true;
            this.btnShowInBrowser.Click += new System.EventHandler(this.btnShowInBrowser_Click);
            // 
            // FlexLayoutSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Flexible Layout Settings";
            this.Name = "FlexLayoutSettingsCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TextBox txtPublicUrl;
        private System.Windows.Forms.Label label3;
    }
}
