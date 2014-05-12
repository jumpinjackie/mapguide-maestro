namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class CompositeStyleListCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeStyleListCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstStyles = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.pointCompositeStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineCompositeStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.areaPolygonCompositeStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstStyles);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // lstStyles
            // 
            this.lstStyles.DisplayMember = "DisplayString";
            resources.ApplyResources(this.lstStyles, "lstStyles");
            this.lstStyles.FormattingEnabled = true;
            this.lstStyles.Name = "lstStyles";
            this.lstStyles.SelectedIndexChanged += new System.EventHandler(this.lstStyles_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointCompositeStyleToolStripMenuItem,
            this.lineCompositeStyleToolStripMenuItem,
            this.areaPolygonCompositeStyleToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // pointCompositeStyleToolStripMenuItem
            // 
            this.pointCompositeStyleToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_select_point;
            this.pointCompositeStyleToolStripMenuItem.Name = "pointCompositeStyleToolStripMenuItem";
            resources.ApplyResources(this.pointCompositeStyleToolStripMenuItem, "pointCompositeStyleToolStripMenuItem");
            this.pointCompositeStyleToolStripMenuItem.Click += new System.EventHandler(this.pointCompositeStyleToolStripMenuItem_Click);
            // 
            // lineCompositeStyleToolStripMenuItem
            // 
            this.lineCompositeStyleToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_shape_line;
            this.lineCompositeStyleToolStripMenuItem.Name = "lineCompositeStyleToolStripMenuItem";
            resources.ApplyResources(this.lineCompositeStyleToolStripMenuItem, "lineCompositeStyleToolStripMenuItem");
            this.lineCompositeStyleToolStripMenuItem.Click += new System.EventHandler(this.lineCompositeStyleToolStripMenuItem_Click);
            // 
            // areaPolygonCompositeStyleToolStripMenuItem
            // 
            this.areaPolygonCompositeStyleToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.layer_shape_polygon;
            this.areaPolygonCompositeStyleToolStripMenuItem.Name = "areaPolygonCompositeStyleToolStripMenuItem";
            resources.ApplyResources(this.areaPolygonCompositeStyleToolStripMenuItem, "areaPolygonCompositeStyleToolStripMenuItem");
            this.areaPolygonCompositeStyleToolStripMenuItem.Click += new System.EventHandler(this.areaPolygonCompositeStyleToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // CompositeStyleListCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompositeStyleListCtrl";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstStyles;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem pointCompositeStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineCompositeStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem areaPolygonCompositeStyleToolStripMenuItem;
    }
}
