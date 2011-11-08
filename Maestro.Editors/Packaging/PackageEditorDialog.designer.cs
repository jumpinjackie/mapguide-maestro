namespace Maestro.Editors.Packaging
{
    partial class PackageEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageEditorDialog));
            this.MainGroup = new System.Windows.Forms.SplitContainer();
            this.ResourceTree = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.InsertDeleteCommands = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DeleteResourceButton = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ResourceDataFileList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.AddResourceData = new System.Windows.Forms.ToolStripButton();
            this.DeleteResourceData = new System.Windows.Forms.ToolStripButton();
            this.EditResourceData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveResourceData = new System.Windows.Forms.ToolStripButton();
            this.label3 = new System.Windows.Forms.Label();
            this.ContentFilePath = new System.Windows.Forms.TextBox();
            this.HeaderFilepath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BrowseResourceDataFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveResourceDataFile = new System.Windows.Forms.SaveFileDialog();
            this.SavePackageDialog = new System.Windows.Forms.SaveFileDialog();
            this.AddResourceButton = new System.Windows.Forms.ToolStripButton();
            this.AddFolderButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MainGroup.Panel1.SuspendLayout();
            this.MainGroup.Panel2.SuspendLayout();
            this.MainGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            resources.ApplyResources(this.MainGroup, "MainGroup");
            this.MainGroup.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.MainGroup.Name = "MainGroup";
            // 
            // MainGroup.Panel1
            // 
            this.MainGroup.Panel1.Controls.Add(this.ResourceTree);
            this.MainGroup.Panel1.Controls.Add(this.panel1);
            this.MainGroup.Panel1.Controls.Add(this.toolStrip1);
            // 
            // MainGroup.Panel2
            // 
            this.MainGroup.Panel2.Controls.Add(this.panel3);
            this.MainGroup.Panel2.Controls.Add(this.label3);
            this.MainGroup.Panel2.Controls.Add(this.ContentFilePath);
            this.MainGroup.Panel2.Controls.Add(this.HeaderFilepath);
            this.MainGroup.Panel2.Controls.Add(this.label2);
            this.MainGroup.Panel2.Controls.Add(this.label1);
            resources.ApplyResources(this.MainGroup.Panel2, "MainGroup.Panel2");
            this.MainGroup.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.MainGroup_Panel2_Paint);
            // 
            // ResourceTree
            // 
            this.ResourceTree.AllowDrop = true;
            resources.ApplyResources(this.ResourceTree, "ResourceTree");
            this.ResourceTree.LabelEdit = true;
            this.ResourceTree.Name = "ResourceTree";
            this.ResourceTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragDrop);
            this.ResourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterSelect);
            this.ResourceTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourceTree_ItemDrag);
            this.ResourceTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragOver);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.InsertDeleteCommands);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // InsertDeleteCommands
            // 
            resources.ApplyResources(this.InsertDeleteCommands, "InsertDeleteCommands");
            this.InsertDeleteCommands.Name = "InsertDeleteCommands";
            this.InsertDeleteCommands.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddFolderButton,
            this.AddResourceButton,
            this.toolStripSeparator2,
            this.DeleteResourceButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // DeleteResourceButton
            // 
            resources.ApplyResources(this.DeleteResourceButton, "DeleteResourceButton");
            this.DeleteResourceButton.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.DeleteResourceButton.Name = "DeleteResourceButton";
            this.DeleteResourceButton.Click += new System.EventHandler(this.DeleteResourceButton_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.ResourceDataFileList);
            this.panel3.Controls.Add(this.toolStrip2);
            this.panel3.Name = "panel3";
            // 
            // ResourceDataFileList
            // 
            this.ResourceDataFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5});
            resources.ApplyResources(this.ResourceDataFileList, "ResourceDataFileList");
            this.ResourceDataFileList.FullRowSelect = true;
            this.ResourceDataFileList.MultiSelect = false;
            this.ResourceDataFileList.Name = "ResourceDataFileList";
            this.ResourceDataFileList.UseCompatibleStateImageBehavior = false;
            this.ResourceDataFileList.View = System.Windows.Forms.View.Details;
            this.ResourceDataFileList.SelectedIndexChanged += new System.EventHandler(this.ResourcDataFileList_SelectedIndexChanged);
            this.ResourceDataFileList.DoubleClick += new System.EventHandler(this.ResourceDataFileList_DoubleClick);
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
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddResourceData,
            this.DeleteResourceData,
            this.EditResourceData,
            this.toolStripSeparator1,
            this.SaveResourceData});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // AddResourceData
            // 
            this.AddResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddResourceData.Image = global::Maestro.Editors.Properties.Resources.document__plus;
            resources.ApplyResources(this.AddResourceData, "AddResourceData");
            this.AddResourceData.Name = "AddResourceData";
            this.AddResourceData.Click += new System.EventHandler(this.AddResourceData_Click);
            // 
            // DeleteResourceData
            // 
            this.DeleteResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DeleteResourceData, "DeleteResourceData");
            this.DeleteResourceData.Image = global::Maestro.Editors.Properties.Resources.document__minus;
            this.DeleteResourceData.Name = "DeleteResourceData";
            this.DeleteResourceData.Click += new System.EventHandler(this.DeleteResourceData_Click);
            // 
            // EditResourceData
            // 
            this.EditResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.EditResourceData, "EditResourceData");
            this.EditResourceData.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            this.EditResourceData.Name = "EditResourceData";
            this.EditResourceData.Click += new System.EventHandler(this.EditResourceData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // SaveResourceData
            // 
            this.SaveResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.SaveResourceData, "SaveResourceData");
            this.SaveResourceData.Image = global::Maestro.Editors.Properties.Resources.disk;
            this.SaveResourceData.Name = "SaveResourceData";
            this.SaveResourceData.Click += new System.EventHandler(this.SaveResourceData_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ContentFilePath
            // 
            resources.ApplyResources(this.ContentFilePath, "ContentFilePath");
            this.ContentFilePath.Name = "ContentFilePath";
            this.ContentFilePath.ReadOnly = true;
            // 
            // HeaderFilepath
            // 
            resources.ApplyResources(this.HeaderFilepath, "HeaderFilepath");
            this.HeaderFilepath.Name = "HeaderFilepath";
            this.HeaderFilepath.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.CancelBtn);
            this.ButtonPanel.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.ButtonPanel, "ButtonPanel");
            this.ButtonPanel.Name = "ButtonPanel";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MainGroup);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // BrowseResourceDataFile
            // 
            resources.ApplyResources(this.BrowseResourceDataFile, "BrowseResourceDataFile");
            // 
            // SaveResourceDataFile
            // 
            resources.ApplyResources(this.SaveResourceDataFile, "SaveResourceDataFile");
            // 
            // SavePackageDialog
            // 
            this.SavePackageDialog.DefaultExt = "mgp";
            resources.ApplyResources(this.SavePackageDialog, "SavePackageDialog");
            // 
            // AddResourceButton
            // 
            this.AddResourceButton.Image = global::Maestro.Editors.Properties.Resources.document__plus;
            resources.ApplyResources(this.AddResourceButton, "AddResourceButton");
            this.AddResourceButton.Name = "AddResourceButton";
            this.AddResourceButton.Click += new System.EventHandler(this.AddResourceButton_Click);
            // 
            // AddFolderButton
            // 
            this.AddFolderButton.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            resources.ApplyResources(this.AddFolderButton, "AddFolderButton");
            this.AddFolderButton.Name = "AddFolderButton";
            this.AddFolderButton.Click += new System.EventHandler(this.AddFolderButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // PackageEditorDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ButtonPanel);
            this.Name = "PackageEditorDialog";
            this.Load += new System.EventHandler(this.PackageEditor_Load);
            this.MainGroup.Panel1.ResumeLayout(false);
            this.MainGroup.Panel1.PerformLayout();
            this.MainGroup.Panel2.ResumeLayout(false);
            this.MainGroup.Panel2.PerformLayout();
            this.MainGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ButtonPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer MainGroup;
        private System.Windows.Forms.TreeView ResourceTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton DeleteResourceButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ContentFilePath;
        private System.Windows.Forms.TextBox HeaderFilepath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton AddResourceData;
        private System.Windows.Forms.ToolStripButton EditResourceData;
        private System.Windows.Forms.ToolStripButton DeleteResourceData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton SaveResourceData;
        private System.Windows.Forms.ListView ResourceDataFileList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.OpenFileDialog BrowseResourceDataFile;
        private System.Windows.Forms.SaveFileDialog SaveResourceDataFile;
        private System.Windows.Forms.SaveFileDialog SavePackageDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox InsertDeleteCommands;
        private System.Windows.Forms.ToolStripButton AddFolderButton;
        private System.Windows.Forms.ToolStripButton AddResourceButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}