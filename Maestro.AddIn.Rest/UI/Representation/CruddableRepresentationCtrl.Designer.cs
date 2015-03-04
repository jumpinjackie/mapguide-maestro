namespace Maestro.AddIn.Rest.UI.Representation
{
    partial class CruddableRepresentationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CruddableRepresentationCtrl));
            this.chkGet = new System.Windows.Forms.CheckBox();
            this.chkPost = new System.Windows.Forms.CheckBox();
            this.chkPut = new System.Windows.Forms.CheckBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.TAB_GET = new System.Windows.Forms.TabPage();
            this.grpGetPerms = new System.Windows.Forms.GroupBox();
            this.getPermsCtrl = new Maestro.AddIn.Rest.UI.Representation.PermissionsCtrl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.getMethodCtrl1 = new Maestro.AddIn.Rest.UI.Methods.GetMethodCtrl();
            this.TAB_POST = new System.Windows.Forms.TabPage();
            this.grpPostPerms = new System.Windows.Forms.GroupBox();
            this.postPermsCtrl = new Maestro.AddIn.Rest.UI.Representation.PermissionsCtrl();
            this.TAB_PUT = new System.Windows.Forms.TabPage();
            this.grpPutPerms = new System.Windows.Forms.GroupBox();
            this.putPermsCtrl = new Maestro.AddIn.Rest.UI.Representation.PermissionsCtrl();
            this.TAB_DELETE = new System.Windows.Forms.TabPage();
            this.grpDeletePerms = new System.Windows.Forms.GroupBox();
            this.deletePermsCtrl = new Maestro.AddIn.Rest.UI.Representation.PermissionsCtrl();
            this.tabs.SuspendLayout();
            this.TAB_GET.SuspendLayout();
            this.grpGetPerms.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TAB_POST.SuspendLayout();
            this.grpPostPerms.SuspendLayout();
            this.TAB_PUT.SuspendLayout();
            this.grpPutPerms.SuspendLayout();
            this.TAB_DELETE.SuspendLayout();
            this.grpDeletePerms.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkGet
            // 
            resources.ApplyResources(this.chkGet, "chkGet");
            this.chkGet.Name = "chkGet";
            this.chkGet.UseVisualStyleBackColor = true;
            this.chkGet.CheckedChanged += new System.EventHandler(this.chkGet_CheckedChanged);
            // 
            // chkPost
            // 
            resources.ApplyResources(this.chkPost, "chkPost");
            this.chkPost.Name = "chkPost";
            this.chkPost.UseVisualStyleBackColor = true;
            this.chkPost.CheckedChanged += new System.EventHandler(this.chkPost_CheckedChanged);
            // 
            // chkPut
            // 
            resources.ApplyResources(this.chkPut, "chkPut");
            this.chkPut.Name = "chkPut";
            this.chkPut.UseVisualStyleBackColor = true;
            this.chkPut.CheckedChanged += new System.EventHandler(this.chkPut_CheckedChanged);
            // 
            // chkDelete
            // 
            resources.ApplyResources(this.chkDelete, "chkDelete");
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.UseVisualStyleBackColor = true;
            this.chkDelete.CheckedChanged += new System.EventHandler(this.chkDelete_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabs
            // 
            resources.ApplyResources(this.tabs, "tabs");
            this.tabs.Controls.Add(this.TAB_GET);
            this.tabs.Controls.Add(this.TAB_POST);
            this.tabs.Controls.Add(this.TAB_PUT);
            this.tabs.Controls.Add(this.TAB_DELETE);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            // 
            // TAB_GET
            // 
            this.TAB_GET.Controls.Add(this.grpGetPerms);
            this.TAB_GET.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.TAB_GET, "TAB_GET");
            this.TAB_GET.Name = "TAB_GET";
            this.TAB_GET.UseVisualStyleBackColor = true;
            // 
            // grpGetPerms
            // 
            this.grpGetPerms.Controls.Add(this.getPermsCtrl);
            resources.ApplyResources(this.grpGetPerms, "grpGetPerms");
            this.grpGetPerms.Name = "grpGetPerms";
            this.grpGetPerms.TabStop = false;
            // 
            // getPermsCtrl
            // 
            resources.ApplyResources(this.getPermsCtrl, "getPermsCtrl");
            this.getPermsCtrl.Name = "getPermsCtrl";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.getMethodCtrl1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // getMethodCtrl1
            // 
            resources.ApplyResources(this.getMethodCtrl1, "getMethodCtrl1");
            this.getMethodCtrl1.Name = "getMethodCtrl1";
            // 
            // TAB_POST
            // 
            this.TAB_POST.Controls.Add(this.grpPostPerms);
            resources.ApplyResources(this.TAB_POST, "TAB_POST");
            this.TAB_POST.Name = "TAB_POST";
            this.TAB_POST.UseVisualStyleBackColor = true;
            // 
            // grpPostPerms
            // 
            this.grpPostPerms.Controls.Add(this.postPermsCtrl);
            resources.ApplyResources(this.grpPostPerms, "grpPostPerms");
            this.grpPostPerms.Name = "grpPostPerms";
            this.grpPostPerms.TabStop = false;
            // 
            // postPermsCtrl
            // 
            resources.ApplyResources(this.postPermsCtrl, "postPermsCtrl");
            this.postPermsCtrl.Name = "postPermsCtrl";
            // 
            // TAB_PUT
            // 
            this.TAB_PUT.Controls.Add(this.grpPutPerms);
            resources.ApplyResources(this.TAB_PUT, "TAB_PUT");
            this.TAB_PUT.Name = "TAB_PUT";
            this.TAB_PUT.UseVisualStyleBackColor = true;
            // 
            // grpPutPerms
            // 
            this.grpPutPerms.Controls.Add(this.putPermsCtrl);
            resources.ApplyResources(this.grpPutPerms, "grpPutPerms");
            this.grpPutPerms.Name = "grpPutPerms";
            this.grpPutPerms.TabStop = false;
            // 
            // putPermsCtrl
            // 
            resources.ApplyResources(this.putPermsCtrl, "putPermsCtrl");
            this.putPermsCtrl.Name = "putPermsCtrl";
            // 
            // TAB_DELETE
            // 
            this.TAB_DELETE.Controls.Add(this.grpDeletePerms);
            resources.ApplyResources(this.TAB_DELETE, "TAB_DELETE");
            this.TAB_DELETE.Name = "TAB_DELETE";
            this.TAB_DELETE.UseVisualStyleBackColor = true;
            // 
            // grpDeletePerms
            // 
            this.grpDeletePerms.Controls.Add(this.deletePermsCtrl);
            resources.ApplyResources(this.grpDeletePerms, "grpDeletePerms");
            this.grpDeletePerms.Name = "grpDeletePerms";
            this.grpDeletePerms.TabStop = false;
            // 
            // deletePermsCtrl
            // 
            resources.ApplyResources(this.deletePermsCtrl, "deletePermsCtrl");
            this.deletePermsCtrl.Name = "deletePermsCtrl";
            // 
            // CruddableRepresentationCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkDelete);
            this.Controls.Add(this.chkPut);
            this.Controls.Add(this.chkPost);
            this.Controls.Add(this.chkGet);
            this.Name = "CruddableRepresentationCtrl";
            this.tabs.ResumeLayout(false);
            this.TAB_GET.ResumeLayout(false);
            this.grpGetPerms.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.TAB_POST.ResumeLayout(false);
            this.grpPostPerms.ResumeLayout(false);
            this.TAB_PUT.ResumeLayout(false);
            this.grpPutPerms.ResumeLayout(false);
            this.TAB_DELETE.ResumeLayout(false);
            this.grpDeletePerms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkGet;
        private System.Windows.Forms.CheckBox chkPost;
        private System.Windows.Forms.CheckBox chkPut;
        private System.Windows.Forms.CheckBox chkDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage TAB_GET;
        private Methods.GetMethodCtrl getMethodCtrl1;
        private System.Windows.Forms.TabPage TAB_POST;
        private System.Windows.Forms.TabPage TAB_PUT;
        private System.Windows.Forms.TabPage TAB_DELETE;
        private System.Windows.Forms.GroupBox grpGetPerms;
        private System.Windows.Forms.GroupBox groupBox1;
        private PermissionsCtrl getPermsCtrl;
        private System.Windows.Forms.GroupBox grpPostPerms;
        private PermissionsCtrl postPermsCtrl;
        private System.Windows.Forms.GroupBox grpPutPerms;
        private PermissionsCtrl putPermsCtrl;
        private System.Windows.Forms.GroupBox grpDeletePerms;
        private PermissionsCtrl deletePermsCtrl;
    }
}
