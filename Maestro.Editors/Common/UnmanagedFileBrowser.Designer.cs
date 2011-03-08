namespace Maestro.Editors.Common
{
    partial class UnmanagedFileBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnmanagedFileBrowser));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.trvFolders = new Aga.Controls.Tree.TreeViewAdv();
            this.NODE_ICON = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.NODE_NAME = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.lstResources = new System.Windows.Forms.ListView();
            this.imgFileList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.trvFolders);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstResources);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // trvFolders
            // 
            resources.ApplyResources(this.trvFolders, "trvFolders");
            this.trvFolders.BackColor = System.Drawing.SystemColors.Window;
            this.trvFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trvFolders.DefaultToolTipProvider = null;
            this.trvFolders.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvFolders.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvFolders.LoadOnDemand = true;
            this.trvFolders.Model = null;
            this.trvFolders.Name = "trvFolders";
            this.trvFolders.NodeControls.Add(this.NODE_ICON);
            this.trvFolders.NodeControls.Add(this.NODE_NAME);
            this.trvFolders.SelectedNode = null;
            this.trvFolders.SelectionChanged += new System.EventHandler(this.trvFolders_SelectionChanged);
            // 
            // NODE_ICON
            // 
            this.NODE_ICON.DataPropertyName = "Icon";
            this.NODE_ICON.LeftMargin = 1;
            this.NODE_ICON.ParentColumn = null;
            this.NODE_ICON.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // NODE_NAME
            // 
            this.NODE_NAME.DataPropertyName = "Name";
            this.NODE_NAME.IncrementalSearchEnabled = true;
            this.NODE_NAME.LeftMargin = 3;
            this.NODE_NAME.ParentColumn = null;
            // 
            // lstResources
            // 
            resources.ApplyResources(this.lstResources, "lstResources");
            this.lstResources.LargeImageList = this.imgFileList;
            this.lstResources.MultiSelect = false;
            this.lstResources.Name = "lstResources";
            this.lstResources.SmallImageList = this.imgFileList;
            this.lstResources.UseCompatibleStateImageBehavior = false;
            this.lstResources.View = System.Windows.Forms.View.List;
            this.lstResources.SelectedIndexChanged += new System.EventHandler(this.lstResources_SelectedIndexChanged);
            // 
            // imgFileList
            // 
            this.imgFileList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFileList.ImageStream")));
            this.imgFileList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFileList.Images.SetKeyName(0, "document.png");
            this.imgFileList.Images.SetKeyName(1, "application.png");
            this.imgFileList.Images.SetKeyName(2, "document-word.png");
            this.imgFileList.Images.SetKeyName(3, "document-access.png");
            this.imgFileList.Images.SetKeyName(4, "document-excel.png");
            this.imgFileList.Images.SetKeyName(5, "document-excel-csv.png");
            this.imgFileList.Images.SetKeyName(6, "document-film.png");
            this.imgFileList.Images.SetKeyName(7, "document-globe.png");
            this.imgFileList.Images.SetKeyName(8, "document-image.png");
            this.imgFileList.Images.SetKeyName(9, "document-pdf.png");
            this.imgFileList.Images.SetKeyName(10, "document-php.png");
            this.imgFileList.Images.SetKeyName(11, "document-powerpoint.png");
            this.imgFileList.Images.SetKeyName(12, "document-text.png");
            this.imgFileList.Images.SetKeyName(13, "document-zipper.png");
            this.imgFileList.Images.SetKeyName(14, "document-code.png");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtItem
            // 
            resources.ApplyResources(this.txtItem, "txtItem");
            this.txtItem.Name = "txtItem";
            this.txtItem.ReadOnly = true;
            this.txtItem.TextChanged += new System.EventHandler(this.txtFile_TextChanged);
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
            // 
            // UnmanagedFileBrowser
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "UnmanagedFileBrowser";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label3;
        private Aga.Controls.Tree.TreeViewAdv trvFolders;
        private System.Windows.Forms.ListView lstResources;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Aga.Controls.Tree.NodeControls.NodeIcon NODE_ICON;
        private Aga.Controls.Tree.NodeControls.NodeTextBox NODE_NAME;
        private System.Windows.Forms.ImageList imgFileList;
    }
}