namespace Maestro.Editors.Generic
{
    partial class ResourcePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourcePicker));
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.lstResources = new System.Windows.Forms.ListView();
            this.resImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.txtResourceId = new System.Windows.Forms.TextBox();
            this.cmbResourceFilter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRoot = new System.Windows.Forms.Button();
            this.repoView = new Maestro.Editors.Common.RepositoryView();
            this.btnUpOneLevel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPreviewNotAvailable = new System.Windows.Forms.Label();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.folderImageList = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.resIdComponentPanel = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefreshFolderView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.resIdComponentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Icon";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = null;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Name";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // lstResources
            // 
            resources.ApplyResources(this.lstResources, "lstResources");
            this.lstResources.LargeImageList = this.resImageList;
            this.lstResources.MultiSelect = false;
            this.lstResources.Name = "lstResources";
            this.lstResources.SmallImageList = this.resImageList;
            this.lstResources.UseCompatibleStateImageBehavior = false;
            this.lstResources.View = System.Windows.Forms.View.List;
            this.lstResources.SelectedIndexChanged += new System.EventHandler(this.lstResources_SelectedIndexChanged);
            this.lstResources.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstResources_MouseDoubleClick);
            // 
            // resImageList
            // 
            this.resImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.resImageList, "resImageList");
            this.resImageList.TransparentColor = System.Drawing.Color.Transparent;
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
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblFilter
            // 
            resources.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            // 
            // txtResourceId
            // 
            resources.ApplyResources(this.txtResourceId, "txtResourceId");
            this.txtResourceId.Name = "txtResourceId";
            this.txtResourceId.ReadOnly = true;
            // 
            // cmbResourceFilter
            // 
            resources.ApplyResources(this.cmbResourceFilter, "cmbResourceFilter");
            this.cmbResourceFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResourceFilter.FormattingEnabled = true;
            this.cmbResourceFilter.Name = "cmbResourceFilter";
            this.cmbResourceFilter.SelectedIndexChanged += new System.EventHandler(this.cmbResourceFilter_SelectedIndexChanged);
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
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRoot);
            this.splitContainer1.Panel1.Controls.Add(this.repoView);
            this.splitContainer1.Panel1.Controls.Add(this.btnUpOneLevel);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnRefreshFolderView);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.lstResources);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            // 
            // btnRoot
            // 
            resources.ApplyResources(this.btnRoot, "btnRoot");
            this.btnRoot.Image = global::Maestro.Editors.Properties.Resources.folder_tree;
            this.btnRoot.Name = "btnRoot";
            this.toolTip.SetToolTip(this.btnRoot, resources.GetString("btnRoot.ToolTip"));
            this.btnRoot.UseVisualStyleBackColor = true;
            this.btnRoot.Click += new System.EventHandler(this.btnRoot_Click);
            // 
            // repoView
            // 
            resources.ApplyResources(this.repoView, "repoView");
            this.repoView.Name = "repoView";
            this.repoView.SelectOnDrag = false;
            // 
            // btnUpOneLevel
            // 
            resources.ApplyResources(this.btnUpOneLevel, "btnUpOneLevel");
            this.btnUpOneLevel.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnUpOneLevel.Name = "btnUpOneLevel";
            this.toolTip.SetToolTip(this.btnUpOneLevel, resources.GetString("btnUpOneLevel.ToolTip"));
            this.btnUpOneLevel.UseVisualStyleBackColor = true;
            this.btnUpOneLevel.Click += new System.EventHandler(this.btnUpOneLevel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lblPreviewNotAvailable);
            this.groupBox1.Controls.Add(this.picPreview);
            this.groupBox1.Controls.Add(this.btnPreview);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblPreviewNotAvailable
            // 
            this.lblPreviewNotAvailable.ForeColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.lblPreviewNotAvailable, "lblPreviewNotAvailable");
            this.lblPreviewNotAvailable.Name = "lblPreviewNotAvailable";
            // 
            // picPreview
            // 
            resources.ApplyResources(this.picPreview, "picPreview");
            this.picPreview.Name = "picPreview";
            this.picPreview.TabStop = false;
            // 
            // btnPreview
            // 
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Image = global::Maestro.Editors.Properties.Resources.magnifier;
            this.btnPreview.Name = "btnPreview";
            this.toolTip.SetToolTip(this.btnPreview, resources.GetString("btnPreview.ToolTip"));
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // folderImageList
            // 
            this.folderImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.folderImageList, "folderImageList");
            this.folderImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtFolder
            // 
            resources.ApplyResources(this.txtFolder, "txtFolder");
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // resIdComponentPanel
            // 
            resources.ApplyResources(this.resIdComponentPanel, "resIdComponentPanel");
            this.resIdComponentPanel.Controls.Add(this.txtName);
            this.resIdComponentPanel.Controls.Add(this.label5);
            this.resIdComponentPanel.Controls.Add(this.label2);
            this.resIdComponentPanel.Controls.Add(this.txtFolder);
            this.resIdComponentPanel.Name = "resIdComponentPanel";
            // 
            // btnRefreshFolderView
            // 
            resources.ApplyResources(this.btnRefreshFolderView, "btnRefreshFolderView");
            this.btnRefreshFolderView.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            this.btnRefreshFolderView.Name = "btnRefreshFolderView";
            this.toolTip.SetToolTip(this.btnRefreshFolderView, resources.GetString("btnRefreshFolderView.ToolTip"));
            this.btnRefreshFolderView.UseVisualStyleBackColor = true;
            this.btnRefreshFolderView.Click += new System.EventHandler(this.btnRefreshFolderView_Click);
            // 
            // ResourcePicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.resIdComponentPanel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cmbResourceFilter);
            this.Controls.Add(this.txtResourceId);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ResourcePicker";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.resIdComponentPanel.ResumeLayout(false);
            this.resIdComponentPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstResources;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TextBox txtResourceId;
        private System.Windows.Forms.ComboBox cmbResourceFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private System.Windows.Forms.ImageList resImageList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Panel resIdComponentPanel;
        private System.Windows.Forms.ImageList folderImageList;
        private Common.RepositoryView repoView;
        private System.Windows.Forms.Button btnRoot;
        private System.Windows.Forms.Button btnUpOneLevel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label lblPreviewNotAvailable;
        private System.Windows.Forms.Button btnRefreshFolderView;
    }
}