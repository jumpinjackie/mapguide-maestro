namespace Maestro.Editors.Common
{
    partial class ResourceDataCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceDataCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnDownload = new System.Windows.Forms.ToolStripButton();
            this.btnMark = new System.Windows.Forms.ToolStripButton();
            this.lstDataFiles = new System.Windows.Forms.ListView();
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnDownload,
            this.btnMark});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.document__plus;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.document__minus;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Image = global::Maestro.Editors.Properties.Resources.drive_download;
            resources.ApplyResources(this.btnDownload, "btnDownload");
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnMark
            // 
            this.btnMark.Image = global::Maestro.Editors.Properties.Resources.tick;
            resources.ApplyResources(this.btnMark, "btnMark");
            this.btnMark.Name = "btnMark";
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // lstDataFiles
            // 
            this.lstDataFiles.AllowDrop = true;
            resources.ApplyResources(this.lstDataFiles, "lstDataFiles");
            this.lstDataFiles.Name = "lstDataFiles";
            this.lstDataFiles.ShowItemToolTips = true;
            this.lstDataFiles.SmallImageList = this.imgIcons;
            this.lstDataFiles.UseCompatibleStateImageBehavior = false;
            this.lstDataFiles.View = System.Windows.Forms.View.List;
            this.lstDataFiles.SelectedIndexChanged += new System.EventHandler(this.lstDataFiles_SelectedIndexChanged);
            this.lstDataFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstDataFiles_DragDrop);
            this.lstDataFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstDataFiles_DragEnter);
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "document.png");
            // 
            // ResourceDataCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lstDataFiles);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ResourceDataCtrl";
            resources.ApplyResources(this, "$this");
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnDownload;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ListView lstDataFiles;
        private System.Windows.Forms.ToolStripButton btnMark;
        private System.Windows.Forms.ImageList imgIcons;
    }
}
