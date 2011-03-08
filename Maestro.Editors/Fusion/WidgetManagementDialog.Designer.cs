namespace Maestro.Editors.Fusion
{
    partial class WidgetManagementDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetManagementDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdWidgets = new System.Windows.Forms.DataGridView();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWidgets)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.grdWidgets);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // grdWidgets
            // 
            this.grdWidgets.AllowUserToAddRows = false;
            this.grdWidgets.AllowUserToDeleteRows = false;
            this.grdWidgets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdWidgets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_NAME,
            this.COL_TYPE});
            resources.ApplyResources(this.grdWidgets, "grdWidgets");
            this.grdWidgets.Name = "grdWidgets";
            this.grdWidgets.ReadOnly = true;
            this.grdWidgets.RowHeadersVisible = false;
            this.grdWidgets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdWidgets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdWidgets_CellClick);
            this.grdWidgets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdWidgets_CellClick);
            // 
            // COL_NAME
            // 
            this.COL_NAME.DataPropertyName = "Name";
            resources.ApplyResources(this.COL_NAME, "COL_NAME");
            this.COL_NAME.Name = "COL_NAME";
            this.COL_NAME.ReadOnly = true;
            // 
            // COL_TYPE
            // 
            this.COL_TYPE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_TYPE.DataPropertyName = "Type";
            resources.ApplyResources(this.COL_TYPE, "COL_TYPE");
            this.COL_TYPE.Name = "COL_TYPE";
            this.COL_TYPE.ReadOnly = true;
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
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.gear__plus;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.gear__minus;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // WidgetManagementDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WidgetManagementDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWidgets)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.DataGridView grdWidgets;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_TYPE;

    }
}