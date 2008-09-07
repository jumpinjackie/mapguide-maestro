namespace OSGeo.MapGuide.Maestro.PackageManager
{
    partial class PackageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageEditor));
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LoaderGroup = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.MainGroup = new System.Windows.Forms.SplitContainer();
            this.ResourceTree = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddFolderButton = new System.Windows.Forms.ToolStripButton();
            this.AddResourceButton = new System.Windows.Forms.ToolStripButton();
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
            this.BrowseResourceDataFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveResourceDataFile = new System.Windows.Forms.SaveFileDialog();
            this.SavePackageDialog = new System.Windows.Forms.SaveFileDialog();
            this.ButtonPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.LoaderGroup.SuspendLayout();
            this.MainGroup.Panel1.SuspendLayout();
            this.MainGroup.Panel2.SuspendLayout();
            this.MainGroup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.CancelBtn);
            this.ButtonPanel.Controls.Add(this.OKBtn);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 344);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(450, 55);
            this.ButtonPanel.TabIndex = 0;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(232, 16);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(72, 24);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.Enabled = false;
            this.OKBtn.Location = new System.Drawing.Point(144, 16);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(72, 24);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LoaderGroup);
            this.panel2.Controls.Add(this.MainGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 344);
            this.panel2.TabIndex = 1;
            // 
            // LoaderGroup
            // 
            this.LoaderGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LoaderGroup.Controls.Add(this.progressBar1);
            this.LoaderGroup.Location = new System.Drawing.Point(8, 128);
            this.LoaderGroup.Name = "LoaderGroup";
            this.LoaderGroup.Size = new System.Drawing.Size(432, 56);
            this.LoaderGroup.TabIndex = 1;
            this.LoaderGroup.TabStop = false;
            this.LoaderGroup.Text = "Reading package content, please wait...";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(16, 24);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(400, 24);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // MainGroup
            // 
            this.MainGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGroup.Location = new System.Drawing.Point(0, 0);
            this.MainGroup.Name = "MainGroup";
            // 
            // MainGroup.Panel1
            // 
            this.MainGroup.Panel1.Controls.Add(this.ResourceTree);
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
            this.MainGroup.Panel2.Enabled = false;
            this.MainGroup.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.MainGroup_Panel2_Paint);
            this.MainGroup.Size = new System.Drawing.Size(450, 344);
            this.MainGroup.SplitterDistance = 168;
            this.MainGroup.TabIndex = 0;
            this.MainGroup.Visible = false;
            // 
            // ResourceTree
            // 
            this.ResourceTree.AllowDrop = true;
            this.ResourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceTree.LabelEdit = true;
            this.ResourceTree.Location = new System.Drawing.Point(0, 25);
            this.ResourceTree.Name = "ResourceTree";
            this.ResourceTree.Size = new System.Drawing.Size(168, 319);
            this.ResourceTree.TabIndex = 0;
            this.ResourceTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragDrop);
            this.ResourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterSelect);
            this.ResourceTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourceTree_ItemDrag);
            this.ResourceTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragOver);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddFolderButton,
            this.AddResourceButton,
            this.DeleteResourceButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(168, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // AddFolderButton
            // 
            this.AddFolderButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddFolderButton.Image = ((System.Drawing.Image)(resources.GetObject("AddFolderButton.Image")));
            this.AddFolderButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddFolderButton.Name = "AddFolderButton";
            this.AddFolderButton.Size = new System.Drawing.Size(23, 22);
            this.AddFolderButton.ToolTipText = "Add a new folder";
            this.AddFolderButton.Click += new System.EventHandler(this.AddFolderButton_Click);
            // 
            // AddResourceButton
            // 
            this.AddResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddResourceButton.Image = ((System.Drawing.Image)(resources.GetObject("AddResourceButton.Image")));
            this.AddResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddResourceButton.Name = "AddResourceButton";
            this.AddResourceButton.Size = new System.Drawing.Size(23, 22);
            this.AddResourceButton.ToolTipText = "Add a new resource to the package";
            this.AddResourceButton.Click += new System.EventHandler(this.AddResourceButton_Click);
            // 
            // DeleteResourceButton
            // 
            this.DeleteResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteResourceButton.Enabled = false;
            this.DeleteResourceButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteResourceButton.Image")));
            this.DeleteResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteResourceButton.Name = "DeleteResourceButton";
            this.DeleteResourceButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteResourceButton.ToolTipText = "Delete the selected item";
            this.DeleteResourceButton.Click += new System.EventHandler(this.DeleteResourceButton_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.ResourceDataFileList);
            this.panel3.Controls.Add(this.toolStrip2);
            this.panel3.Location = new System.Drawing.Point(16, 88);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(248, 248);
            this.panel3.TabIndex = 5;
            // 
            // ResourceDataFileList
            // 
            this.ResourceDataFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5});
            this.ResourceDataFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceDataFileList.FullRowSelect = true;
            this.ResourceDataFileList.Location = new System.Drawing.Point(0, 25);
            this.ResourceDataFileList.MultiSelect = false;
            this.ResourceDataFileList.Name = "ResourceDataFileList";
            this.ResourceDataFileList.Size = new System.Drawing.Size(248, 223);
            this.ResourceDataFileList.TabIndex = 1;
            this.ResourceDataFileList.UseCompatibleStateImageBehavior = false;
            this.ResourceDataFileList.View = System.Windows.Forms.View.Details;
            this.ResourceDataFileList.SelectedIndexChanged += new System.EventHandler(this.ResourcDataFileList_SelectedIndexChanged);
            this.ResourceDataFileList.DoubleClick += new System.EventHandler(this.ResourceDataFileList_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Content type";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Data type";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File";
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
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(248, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // AddResourceData
            // 
            this.AddResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddResourceData.Image = ((System.Drawing.Image)(resources.GetObject("AddResourceData.Image")));
            this.AddResourceData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddResourceData.Name = "AddResourceData";
            this.AddResourceData.Size = new System.Drawing.Size(23, 22);
            this.AddResourceData.ToolTipText = "Add a file";
            this.AddResourceData.Click += new System.EventHandler(this.AddResourceData_Click);
            // 
            // DeleteResourceData
            // 
            this.DeleteResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteResourceData.Enabled = false;
            this.DeleteResourceData.Image = ((System.Drawing.Image)(resources.GetObject("DeleteResourceData.Image")));
            this.DeleteResourceData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteResourceData.Name = "DeleteResourceData";
            this.DeleteResourceData.Size = new System.Drawing.Size(23, 22);
            this.DeleteResourceData.ToolTipText = "Delete the selected file";
            this.DeleteResourceData.Click += new System.EventHandler(this.DeleteResourceData_Click);
            // 
            // EditResourceData
            // 
            this.EditResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditResourceData.Enabled = false;
            this.EditResourceData.Image = ((System.Drawing.Image)(resources.GetObject("EditResourceData.Image")));
            this.EditResourceData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditResourceData.Name = "EditResourceData";
            this.EditResourceData.Size = new System.Drawing.Size(23, 22);
            this.EditResourceData.ToolTipText = "Edit the selected files metadata";
            this.EditResourceData.Click += new System.EventHandler(this.EditResourceData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SaveResourceData
            // 
            this.SaveResourceData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveResourceData.Enabled = false;
            this.SaveResourceData.Image = ((System.Drawing.Image)(resources.GetObject("SaveResourceData.Image")));
            this.SaveResourceData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveResourceData.Name = "SaveResourceData";
            this.SaveResourceData.Size = new System.Drawing.Size(23, 22);
            this.SaveResourceData.ToolTipText = "Save the selected file";
            this.SaveResourceData.Click += new System.EventHandler(this.SaveResourceData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data files";
            // 
            // ContentFilePath
            // 
            this.ContentFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentFilePath.Location = new System.Drawing.Point(80, 32);
            this.ContentFilePath.Name = "ContentFilePath";
            this.ContentFilePath.ReadOnly = true;
            this.ContentFilePath.Size = new System.Drawing.Size(184, 20);
            this.ContentFilePath.TabIndex = 3;
            // 
            // HeaderFilepath
            // 
            this.HeaderFilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderFilepath.Location = new System.Drawing.Point(80, 8);
            this.HeaderFilepath.Name = "HeaderFilepath";
            this.HeaderFilepath.ReadOnly = true;
            this.HeaderFilepath.Size = new System.Drawing.Size(184, 20);
            this.HeaderFilepath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Content file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Header file";
            // 
            // BrowseResourceDataFile
            // 
            this.BrowseResourceDataFile.Filter = "All files (*.*)|*.*";
            this.BrowseResourceDataFile.Title = "Select a file to add";
            // 
            // SaveResourceDataFile
            // 
            this.SaveResourceDataFile.Filter = "All files (*.*)|*.*";
            this.SaveResourceDataFile.Title = "Select where to save the file";
            // 
            // SavePackageDialog
            // 
            this.SavePackageDialog.DefaultExt = "mgp";
            this.SavePackageDialog.Filter = "MapGuide Packages (*.mgp)|*.mgp|Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            this.SavePackageDialog.Title = "Select where to store the file";
            // 
            // PackageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(450, 399);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ButtonPanel);
            this.Name = "PackageEditor";
            this.Text = "Package Editor";
            this.Load += new System.EventHandler(this.PackageEditor_Load);
            this.ButtonPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.LoaderGroup.ResumeLayout(false);
            this.MainGroup.Panel1.ResumeLayout(false);
            this.MainGroup.Panel1.PerformLayout();
            this.MainGroup.Panel2.ResumeLayout(false);
            this.MainGroup.Panel2.PerformLayout();
            this.MainGroup.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton AddFolderButton;
        private System.Windows.Forms.ToolStripButton DeleteResourceButton;
        private System.Windows.Forms.GroupBox LoaderGroup;
        private System.Windows.Forms.ProgressBar progressBar1;
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
        private System.Windows.Forms.ToolStripButton AddResourceButton;
        private System.Windows.Forms.SaveFileDialog SavePackageDialog;
    }
}