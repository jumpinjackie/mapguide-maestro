namespace Maestro.Editors.FeatureSource
{
    partial class CoordSysOverrideCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordSysOverrideCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApplyAll = new System.Windows.Forms.ToolStripButton();
            this.btnLoadFromSc = new System.Windows.Forms.ToolStripButton();
            this.grdOverrides = new System.Windows.Forms.DataGridView();
            this.COL_SOURCE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_TARGET = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOverrides)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grdOverrides);
            this.contentPanel.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnApplyAll,
            this.btnLoadFromSc});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.globe__plus;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Image = global::Maestro.Editors.Properties.Resources.globe__pencil;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.globe__minus;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnApplyAll
            // 
            resources.ApplyResources(this.btnApplyAll, "btnApplyAll");
            this.btnApplyAll.Image = global::Maestro.Editors.Properties.Resources.globe__arrow;
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.Click += new System.EventHandler(this.btnApplyAll_Click);
            // 
            // btnLoadFromSc
            // 
            this.btnLoadFromSc.Image = global::Maestro.Editors.Properties.Resources.globe__plus;
            resources.ApplyResources(this.btnLoadFromSc, "btnLoadFromSc");
            this.btnLoadFromSc.Name = "btnLoadFromSc";
            this.btnLoadFromSc.Click += new System.EventHandler(this.btnLoadFromSc_Click);
            // 
            // grdOverrides
            // 
            this.grdOverrides.AllowUserToAddRows = false;
            this.grdOverrides.AllowUserToDeleteRows = false;
            this.grdOverrides.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOverrides.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_SOURCE,
            this.COL_TARGET});
            resources.ApplyResources(this.grdOverrides, "grdOverrides");
            this.grdOverrides.Name = "grdOverrides";
            this.grdOverrides.ReadOnly = true;
            this.grdOverrides.RowHeadersVisible = false;
            this.grdOverrides.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdOverrides_CellContentClick);
            this.grdOverrides.SelectionChanged += new System.EventHandler(this.grdOverrides_SelectionChanged);
            this.grdOverrides.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdOverrides_CellContentClick);
            // 
            // COL_SOURCE
            // 
            this.COL_SOURCE.DataPropertyName = "Name";
            resources.ApplyResources(this.COL_SOURCE, "COL_SOURCE");
            this.COL_SOURCE.Name = "COL_SOURCE";
            this.COL_SOURCE.ReadOnly = true;
            // 
            // COL_TARGET
            // 
            this.COL_TARGET.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_TARGET.DataPropertyName = "CoordinateSystem";
            resources.ApplyResources(this.COL_TARGET, "COL_TARGET");
            this.COL_TARGET.Name = "COL_TARGET";
            this.COL_TARGET.ReadOnly = true;
            // 
            // CoordSysOverrideCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Coordinate System Overrides";
            this.Name = "CoordSysOverrideCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOverrides)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdOverrides;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_SOURCE;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_TARGET;
        private System.Windows.Forms.ToolStripButton btnApplyAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnLoadFromSc;
    }
}
