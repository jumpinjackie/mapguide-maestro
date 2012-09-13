namespace Maestro.Editors.FeatureSource.Preview
{
    partial class LocalFeatureSourcePreviewCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalFeatureSourcePreviewCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trvSchema = new System.Windows.Forms.TreeView();
            this.schemaImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSql = new System.Windows.Forms.ToolStripButton();
            this.btnStandard = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.tabPreviews = new System.Windows.Forms.TabControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvSchema);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabPreviews);
            // 
            // trvSchema
            // 
            resources.ApplyResources(this.trvSchema, "trvSchema");
            this.trvSchema.ImageList = this.schemaImageList;
            this.trvSchema.Name = "trvSchema";
            this.trvSchema.ShowNodeToolTips = true;
            this.trvSchema.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvSchema_AfterExpand);
            this.trvSchema.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvSchema_AfterSelect);
            // 
            // schemaImageList
            // 
            this.schemaImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("schemaImageList.ImageStream")));
            this.schemaImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.schemaImageList.Images.SetKeyName(0, "sitemap-application-blue.png");
            this.schemaImageList.Images.SetKeyName(1, "table.png");
            this.schemaImageList.Images.SetKeyName(2, "property.png");
            this.schemaImageList.Images.SetKeyName(3, "key.png");
            this.schemaImageList.Images.SetKeyName(4, "layer-shape.png");
            this.schemaImageList.Images.SetKeyName(5, "image.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnSql,
            this.btnStandard,
            this.btnClose});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnSql
            // 
            this.btnSql.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnSql, "btnSql");
            this.btnSql.Image = global::Maestro.Editors.Properties.Resources.sql;
            this.btnSql.Name = "btnSql";
            this.btnSql.Click += new System.EventHandler(this.btnSql_Click);
            // 
            // btnStandard
            // 
            this.btnStandard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnStandard, "btnStandard");
            this.btnStandard.Image = global::Maestro.Editors.Properties.Resources.magnifier;
            this.btnStandard.Name = "btnStandard";
            this.btnStandard.Click += new System.EventHandler(this.btnStandard_Click);
            // 
            // btnClose
            // 
            this.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Image = global::Maestro.Editors.Properties.Resources.cross;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabPreviews
            // 
            resources.ApplyResources(this.tabPreviews, "tabPreviews");
            this.tabPreviews.Name = "tabPreviews";
            this.tabPreviews.SelectedIndex = 0;
            // 
            // LocalFeatureSourcePreviewCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LocalFeatureSourcePreviewCtrl";
            resources.ApplyResources(this, "$this");
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnSql;
        private System.Windows.Forms.ToolStripButton btnStandard;
        private System.Windows.Forms.TabControl tabPreviews;
        private System.Windows.Forms.TreeView trvSchema;
        private System.Windows.Forms.ImageList schemaImageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClose;
    }
}
