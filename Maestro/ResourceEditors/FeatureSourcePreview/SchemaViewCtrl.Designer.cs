namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview
{
    partial class SchemaViewCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaViewCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.trvSchema = new System.Windows.Forms.TreeView();
            this.btnStdQuery = new System.Windows.Forms.ToolStripButton();
            this.btnSqlQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.btnStdQuery,
            this.btnSqlQuery});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(229, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(65, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // trvSchema
            // 
            this.trvSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvSchema.Location = new System.Drawing.Point(0, 25);
            this.trvSchema.Name = "trvSchema";
            this.trvSchema.Size = new System.Drawing.Size(229, 322);
            this.trvSchema.TabIndex = 1;
            this.trvSchema.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvSchema_AfterSelect);
            // 
            // btnStdQuery
            // 
            this.btnStdQuery.Enabled = false;
            this.btnStdQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnStdQuery.Image")));
            this.btnStdQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStdQuery.Name = "btnStdQuery";
            this.btnStdQuery.Size = new System.Drawing.Size(71, 22);
            this.btnStdQuery.Text = "Standard";
            this.btnStdQuery.Click += new System.EventHandler(this.btnStdQuery_Click);
            // 
            // btnSqlQuery
            // 
            this.btnSqlQuery.Enabled = false;
            this.btnSqlQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnSqlQuery.Image")));
            this.btnSqlQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSqlQuery.Name = "btnSqlQuery";
            this.btnSqlQuery.Size = new System.Drawing.Size(46, 22);
            this.btnSqlQuery.Text = "SQL";
            this.btnSqlQuery.Click += new System.EventHandler(this.btnSqlQuery_Click);
            // 
            // SchemaViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trvSchema);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SchemaViewCtrl";
            this.Size = new System.Drawing.Size(229, 347);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.TreeView trvSchema;
        private System.Windows.Forms.ToolStripButton btnStdQuery;
        private System.Windows.Forms.ToolStripButton btnSqlQuery;
    }
}
