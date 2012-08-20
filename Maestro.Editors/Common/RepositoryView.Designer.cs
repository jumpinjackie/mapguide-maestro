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
            this.trvRepository = new System.Windows.Forms.TreeView();
            this.resImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // trvRepository
            // 
            this.trvRepository.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvRepository.ImageIndex = 0;
            this.trvRepository.ImageList = this.resImageList;
            this.trvRepository.Location = new System.Drawing.Point(0, 0);
            this.trvRepository.Name = "trvRepository";
            this.trvRepository.SelectedImageIndex = 0;
            this.trvRepository.Size = new System.Drawing.Size(150, 150);
            this.trvRepository.TabIndex = 0;
            // 
            // resImageList
            // 
            this.resImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.resImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.resImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // RepositoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trvRepository);
            this.Name = "RepositoryView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvRepository;
        private System.Windows.Forms.ImageList resImageList;
    }
}
