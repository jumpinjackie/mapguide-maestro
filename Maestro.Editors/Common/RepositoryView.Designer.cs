namespace Maestro.Editors.Common
{
    partial class RepositoryView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryView));
            this.trvRepository = new System.Windows.Forms.TreeView();
            this.resImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // trvRepository
            // 
            resources.ApplyResources(this.trvRepository, "trvRepository");
            this.trvRepository.ImageList = this.resImageList;
            this.trvRepository.Name = "trvRepository";
            this.trvRepository.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvRepository_ItemDrag);
            // 
            // resImageList
            // 
            this.resImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.resImageList, "resImageList");
            this.resImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // RepositoryView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.trvRepository);
            this.Name = "RepositoryView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvRepository;
        private System.Windows.Forms.ImageList resImageList;
    }
}
