namespace Maestro.AddIn.GeoRest.UI
{
    partial class FileSystemView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSystemView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.ctxFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRepresentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveMaestroConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.ctxFolder.SuspendLayout();
            this.ctxConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.btnOptions,
            this.toolStripSeparator1,
            this.btnPreview,
            this.btnRefresh});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.Image = global::Maestro.AddIn.GeoRest.Properties.Resources.plug;
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Image = global::Maestro.AddIn.GeoRest.Properties.Resources.magnifier;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Image = global::Maestro.AddIn.GeoRest.Properties.Resources.arrow_circle_045_left;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // fileTree
            // 
            resources.ApplyResources(this.fileTree, "fileTree");
            this.fileTree.ImageList = this.imgList;
            this.fileTree.Name = "fileTree";
            this.fileTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTree_NodeMouseDoubleClick);
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterSelect);
            this.fileTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterExpand);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "folder-horizontal.png");
            this.imgList.Images.SetKeyName(1, "document-task.png");
            this.imgList.Images.SetKeyName(2, "document.png");
            this.imgList.Images.SetKeyName(3, "odata.png");
            this.imgList.Images.SetKeyName(4, "json.png");
            this.imgList.Images.SetKeyName(5, "document-xaml.png");
            this.imgList.Images.SetKeyName(6, "image.png");
            this.imgList.Images.SetKeyName(7, "document-node.png");
            this.imgList.Images.SetKeyName(8, "document-template.png");
            // 
            // ctxFolder
            // 
            this.ctxFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConfigurationToolStripMenuItem});
            this.ctxFolder.Name = "ctxFolder";
            resources.ApplyResources(this.ctxFolder, "ctxFolder");
            // 
            // createConfigurationToolStripMenuItem
            // 
            this.createConfigurationToolStripMenuItem.Name = "createConfigurationToolStripMenuItem";
            resources.ApplyResources(this.createConfigurationToolStripMenuItem, "createConfigurationToolStripMenuItem");
            // 
            // ctxConfig
            // 
            this.ctxConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRepresentationToolStripMenuItem,
            this.editXMLToolStripMenuItem});
            this.ctxConfig.Name = "ctxConfig";
            resources.ApplyResources(this.ctxConfig, "ctxConfig");
            // 
            // addRepresentationToolStripMenuItem
            // 
            this.addRepresentationToolStripMenuItem.Name = "addRepresentationToolStripMenuItem";
            resources.ApplyResources(this.addRepresentationToolStripMenuItem, "addRepresentationToolStripMenuItem");
            // 
            // editXMLToolStripMenuItem
            // 
            this.editXMLToolStripMenuItem.Name = "editXMLToolStripMenuItem";
            resources.ApplyResources(this.editXMLToolStripMenuItem, "editXMLToolStripMenuItem");
            // 
            // btnOptions
            // 
            this.btnOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMaestroConfigToolStripMenuItem});
            resources.ApplyResources(this.btnOptions, "btnOptions");
            this.btnOptions.Image = global::Maestro.AddIn.GeoRest.Properties.Resources.application_list;
            this.btnOptions.Name = "btnOptions";
            // 
            // saveMaestroConfigToolStripMenuItem
            // 
            this.saveMaestroConfigToolStripMenuItem.Image = global::Maestro.AddIn.GeoRest.Properties.Resources.disk;
            this.saveMaestroConfigToolStripMenuItem.Name = "saveMaestroConfigToolStripMenuItem";
            resources.ApplyResources(this.saveMaestroConfigToolStripMenuItem, "saveMaestroConfigToolStripMenuItem");
            this.saveMaestroConfigToolStripMenuItem.Click += new System.EventHandler(this.saveMaestroConfigToolStripMenuItem_Click);
            // 
            // FileSystemView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.fileTree);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FileSystemView";
            resources.ApplyResources(this, "$this");
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ctxFolder.ResumeLayout(false);
            this.ctxConfig.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ContextMenuStrip ctxFolder;
        private System.Windows.Forms.ContextMenuStrip ctxConfig;
        private System.Windows.Forms.ToolStripMenuItem createConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRepresentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.ToolStripDropDownButton btnOptions;
        private System.Windows.Forms.ToolStripMenuItem saveMaestroConfigToolStripMenuItem;
    }
}
