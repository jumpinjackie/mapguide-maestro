namespace Maestro.Editors.LayerDefinition.Vector
{
    partial class VectorLayerStyleSectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorLayerStyleSectionCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnDuplicate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSort = new System.Windows.Forms.ToolStripButton();
            this.scaleRangeList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeList();
            this.contentPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.scaleRangeList);
            this.contentPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnDuplicate,
            this.toolStripSeparator1,
            this.btnSort});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.minus_circle;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Image = global::Maestro.Editors.Properties.Resources.document_copy;
            resources.ApplyResources(this.btnDuplicate, "btnDuplicate");
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnSort
            // 
            this.btnSort.Image = global::Maestro.Editors.Properties.Resources.sort_number;
            resources.ApplyResources(this.btnSort, "btnSort");
            this.btnSort.Name = "btnSort";
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // scaleRangeList
            // 
            resources.ApplyResources(this.scaleRangeList, "scaleRangeList");
            this.scaleRangeList.Name = "scaleRangeList";
            this.scaleRangeList.Owner = null;
            this.scaleRangeList.SelectionChanged += new System.EventHandler(this.scaleRangeList_SelectionChanged);
            this.scaleRangeList.ItemChanged += new System.EventHandler(this.scaleRangeList_ItemChanged);
            // 
            // VectorLayerStyleSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Layer Styles";
            this.Name = "VectorLayerStyleSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeList scaleRangeList;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnDuplicate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSort;
    }
}
