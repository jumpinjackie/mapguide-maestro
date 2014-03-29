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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorLayerStyleSectionCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDuplicate = new System.Windows.Forms.ToolStripButton();
            this.btnSort = new System.Windows.Forms.ToolStripButton();
            this.lstScaleRanges = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnKmlElevation = new System.Windows.Forms.Button();
            this.cmbMinScale = new System.Windows.Forms.ComboBox();
            this.cmbMaxScale = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpScaleRange = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grpScaleRange);
            this.contentPanel.Controls.Add(this.panel1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnDuplicate,
            this.btnSort});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.minus_circle;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDuplicate.Image = global::Maestro.Editors.Properties.Resources.document_copy;
            resources.ApplyResources(this.btnDuplicate, "btnDuplicate");
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // btnSort
            // 
            this.btnSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSort.Image = global::Maestro.Editors.Properties.Resources.sort_number;
            resources.ApplyResources(this.btnSort, "btnSort");
            this.btnSort.Name = "btnSort";
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // lstScaleRanges
            // 
            this.lstScaleRanges.DisplayMember = "ScaleDisplayString";
            resources.ApplyResources(this.lstScaleRanges, "lstScaleRanges");
            this.lstScaleRanges.FormattingEnabled = true;
            this.lstScaleRanges.Name = "lstScaleRanges";
            this.lstScaleRanges.SelectedIndexChanged += new System.EventHandler(this.lstScaleRanges_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnKmlElevation);
            this.groupBox1.Controls.Add(this.cmbMinScale);
            this.groupBox1.Controls.Add(this.cmbMaxScale);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnKmlElevation
            // 
            this.btnKmlElevation.Image = global::Maestro.Editors.Properties.Resources.ruler;
            resources.ApplyResources(this.btnKmlElevation, "btnKmlElevation");
            this.btnKmlElevation.Name = "btnKmlElevation";
            this.btnKmlElevation.UseVisualStyleBackColor = true;
            this.btnKmlElevation.Click += new System.EventHandler(this.btnKmlElevation_Click);
            // 
            // cmbMinScale
            // 
            resources.ApplyResources(this.cmbMinScale, "cmbMinScale");
            this.cmbMinScale.FormattingEnabled = true;
            this.cmbMinScale.Name = "cmbMinScale";
            this.cmbMinScale.TextChanged += new System.EventHandler(this.cmbMinScale_TextChanged);
            // 
            // cmbMaxScale
            // 
            resources.ApplyResources(this.cmbMaxScale, "cmbMaxScale");
            this.cmbMaxScale.FormattingEnabled = true;
            this.cmbMaxScale.Name = "cmbMaxScale";
            this.cmbMaxScale.TextChanged += new System.EventHandler(this.cmbMaxScale_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // grpScaleRange
            // 
            resources.ApplyResources(this.grpScaleRange, "grpScaleRange");
            this.grpScaleRange.Name = "grpScaleRange";
            this.grpScaleRange.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstScaleRanges);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // VectorLayerStyleSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "VectorLayerStyleSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnDuplicate;
        private System.Windows.Forms.ToolStripButton btnSort;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbMaxScale;
        private System.Windows.Forms.ComboBox cmbMinScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpScaleRange;
        private System.Windows.Forms.ListBox lstScaleRanges;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnKmlElevation;
        private System.Windows.Forms.Panel panel1;
    }
}
