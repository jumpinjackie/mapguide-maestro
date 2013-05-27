namespace Maestro.Editors.MapDefinition
{
    partial class LiveMapEditorRepositoryView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveMapEditorRepositoryView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddToMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.repoView = new Maestro.Editors.Common.RepositoryView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddToMap,
            this.toolStripSeparator1,
            this.btnEdit,
            this.btnRefresh});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddToMap
            // 
            this.btnAddToMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnAddToMap, "btnAddToMap");
            this.btnAddToMap.Image = global::Maestro.Editors.Properties.Resources.map__plus;
            this.btnAddToMap.Name = "btnAddToMap";
            this.btnAddToMap.Click += new System.EventHandler(this.btnAddToMap_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // repoView
            // 
            resources.ApplyResources(this.repoView, "repoView");
            this.repoView.Name = "repoView";
            this.repoView.SelectOnDrag = true;
            this.repoView.ItemSelected += new System.EventHandler(this.repoView_ItemSelected);
            this.repoView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.repoView_ItemDrag);
            // 
            // LiveMapEditorRepositoryView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.repoView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LiveMapEditorRepositoryView";
            resources.ApplyResources(this, "$this");
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Common.RepositoryView repoView;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnAddToMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
