namespace Maestro.MapViewer
{
    partial class Legend
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
            this.trvLegend = new System.Windows.Forms.TreeView();
            this.imgLegend = new System.Windows.Forms.ImageList(this.components);
            this.bgLegendUpdate = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // trvLegend
            // 
            this.trvLegend.CheckBoxes = true;
            this.trvLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvLegend.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.trvLegend.ImageIndex = 0;
            this.trvLegend.ImageList = this.imgLegend;
            this.trvLegend.Location = new System.Drawing.Point(0, 0);
            this.trvLegend.Name = "trvLegend";
            this.trvLegend.SelectedImageIndex = 0;
            this.trvLegend.ShowLines = false;
            this.trvLegend.ShowNodeToolTips = true;
            this.trvLegend.Size = new System.Drawing.Size(171, 244);
            this.trvLegend.TabIndex = 0;
            this.trvLegend.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvLegend_AfterCheck);
            this.trvLegend.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvLegend_AfterCollapse);
            this.trvLegend.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvLegend_AfterExpand);
            this.trvLegend.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvLegend_DrawNode);
            this.trvLegend.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvLegend_ItemDrag);
            this.trvLegend.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvLegend_AfterSelect);
            this.trvLegend.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvLegend_NodeMouseClick);
            this.trvLegend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvLegend_MouseDown);
            // 
            // imgLegend
            // 
            this.imgLegend.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgLegend.ImageSize = new System.Drawing.Size(16, 16);
            this.imgLegend.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bgLegendUpdate
            // 
            this.bgLegendUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLegendUpdate_DoWork);
            this.bgLegendUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgLegendUpdate_RunWorkerCompleted);
            // 
            // Legend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trvLegend);
            this.Name = "Legend";
            this.Size = new System.Drawing.Size(171, 244);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvLegend;
        private System.Windows.Forms.ImageList imgLegend;
        private System.ComponentModel.BackgroundWorker bgLegendUpdate;
    }
}
