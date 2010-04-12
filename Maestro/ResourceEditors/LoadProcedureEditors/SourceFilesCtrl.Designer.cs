namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class SourceFilesCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceFilesCtrl));
            this.lstSourceFiles = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddFiles = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveFiles = new System.Windows.Forms.ToolStripButton();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstSourceFiles
            // 
            this.lstSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSourceFiles.FormattingEnabled = true;
            this.lstSourceFiles.Location = new System.Drawing.Point(0, 25);
            this.lstSourceFiles.Name = "lstSourceFiles";
            this.lstSourceFiles.Size = new System.Drawing.Size(316, 121);
            this.lstSourceFiles.TabIndex = 3;
            this.lstSourceFiles.SelectedIndexChanged += new System.EventHandler(this.lstSourceFiles_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddFiles,
            this.btnRemoveFiles});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(316, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFiles.Image")));
            this.btnAddFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(70, 22);
            this.btnAddFiles.Text = "Add Files";
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // btnRemoveFiles
            // 
            this.btnRemoveFiles.Enabled = false;
            this.btnRemoveFiles.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveFiles.Image")));
            this.btnRemoveFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveFiles.Name = "btnRemoveFiles";
            this.btnRemoveFiles.Size = new System.Drawing.Size(90, 22);
            this.btnRemoveFiles.Text = "Remove Files";
            this.btnRemoveFiles.Click += new System.EventHandler(this.btnRemoveFiles_Click);
            // 
            // openFileDlg
            // 
            this.openFileDlg.Multiselect = true;
            // 
            // SourceFilesCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstSourceFiles);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SourceFilesCtrl";
            this.Size = new System.Drawing.Size(316, 150);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ListBox lstSourceFiles;
        protected System.Windows.Forms.ToolStrip toolStrip1;
        protected System.Windows.Forms.ToolStripButton btnAddFiles;
        protected System.Windows.Forms.ToolStripButton btnRemoveFiles;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
    }
}
