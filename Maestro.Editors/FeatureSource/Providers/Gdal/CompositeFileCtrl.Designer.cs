namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
    partial class CompositeFileCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeFileCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.browseFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.lstView = new System.Windows.Forms.ListView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.browseAliasedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseAliasedFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(504, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseFilesToolStripMenuItem,
            this.browseFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.browseAliasedFileToolStripMenuItem,
            this.browseAliasedFolderToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(58, 22);
            this.btnAdd.Text = "Add";
            // 
            // browseFilesToolStripMenuItem
            // 
            this.browseFilesToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.document;
            this.browseFilesToolStripMenuItem.Name = "browseFilesToolStripMenuItem";
            this.browseFilesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.browseFilesToolStripMenuItem.Text = "Browse Files";
            this.browseFilesToolStripMenuItem.Click += new System.EventHandler(this.browseFilesToolStripMenuItem_Click);
            // 
            // browseFolderToolStripMenuItem
            // 
            this.browseFolderToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder_horizontal;
            this.browseFolderToolStripMenuItem.Name = "browseFolderToolStripMenuItem";
            this.browseFolderToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.browseFolderToolStripMenuItem.Text = "Browse Folder";
            this.browseFolderToolStripMenuItem.Click += new System.EventHandler(this.browseFolderToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lstView
            // 
            this.lstView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstView.LargeImageList = this.imgList;
            this.lstView.Location = new System.Drawing.Point(0, 25);
            this.lstView.Name = "lstView";
            this.lstView.Size = new System.Drawing.Size(504, 127);
            this.lstView.SmallImageList = this.imgList;
            this.lstView.TabIndex = 1;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.View = System.Windows.Forms.View.List;
            this.lstView.SelectedIndexChanged += new System.EventHandler(this.lstView_SelectedIndexChanged);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "image.png");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(3, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Note that all paths are as seen by the MapGuide server, not Maestro";
            // 
            // btnRebuild
            // 
            this.btnRebuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRebuild.Location = new System.Drawing.Point(392, 158);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(109, 29);
            this.btnRebuild.TabIndex = 3;
            this.btnRebuild.Text = "Rebuild All";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.btnRebuild_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            // 
            // browseAliasedFileToolStripMenuItem
            // 
            this.browseAliasedFileToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.document;
            this.browseAliasedFileToolStripMenuItem.Name = "browseAliasedFileToolStripMenuItem";
            this.browseAliasedFileToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.browseAliasedFileToolStripMenuItem.Text = "Browse Aliased File";
            this.browseAliasedFileToolStripMenuItem.Click += new System.EventHandler(this.browseAliasedFileToolStripMenuItem_Click);
            // 
            // browseAliasedFolderToolStripMenuItem
            // 
            this.browseAliasedFolderToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.folder_horizontal;
            this.browseAliasedFolderToolStripMenuItem.Name = "browseAliasedFolderToolStripMenuItem";
            this.browseAliasedFolderToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.browseAliasedFolderToolStripMenuItem.Text = "Browse Aliased Folder";
            this.browseAliasedFolderToolStripMenuItem.Click += new System.EventHandler(this.browseAliasedFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // CompositeFileCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRebuild);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CompositeFileCtrl";
            this.Size = new System.Drawing.Size(504, 193);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView lstView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem browseFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem browseAliasedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseAliasedFolderToolStripMenuItem;
    }
}
