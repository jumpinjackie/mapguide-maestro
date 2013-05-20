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
            this.COL_DOCKABLE = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblNonDockableNote = new System.Windows.Forms.Label();
            this.tabWidgets = new System.Windows.Forms.TabControl();
            this.TAB_MAP_WIDGET = new System.Windows.Forms.TabPage();
            this.txtMapWidgetXml = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnSaveMapWidgetXml = new System.Windows.Forms.ToolStripButton();
            this.TAB_OTHER_WIDGETS = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWidgets)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tabWidgets.SuspendLayout();
            this.TAB_MAP_WIDGET.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.TAB_OTHER_WIDGETS.SuspendLayout();
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
            this.COL_DOCKABLE,
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
            // COL_DOCKABLE
            // 
            this.COL_DOCKABLE.DataPropertyName = "IsDockable";
            resources.ApplyResources(this.COL_DOCKABLE, "COL_DOCKABLE");
            this.COL_DOCKABLE.Name = "COL_DOCKABLE";
            this.COL_DOCKABLE.ReadOnly = true;
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
            // lblNonDockableNote
            // 
            resources.ApplyResources(this.lblNonDockableNote, "lblNonDockableNote");
            this.lblNonDockableNote.Name = "lblNonDockableNote";
            // 
            // tabWidgets
            // 
            resources.ApplyResources(this.tabWidgets, "tabWidgets");
            this.tabWidgets.Controls.Add(this.TAB_MAP_WIDGET);
            this.tabWidgets.Controls.Add(this.TAB_OTHER_WIDGETS);
            this.tabWidgets.Name = "tabWidgets";
            this.tabWidgets.SelectedIndex = 0;
            this.tabWidgets.SelectedIndexChanged += new System.EventHandler(this.tabWidgets_SelectedIndexChanged);
            this.tabWidgets.TabIndexChanged += new System.EventHandler(this.tabWidgets_TabIndexChanged);
            // 
            // TAB_MAP_WIDGET
            // 
            this.TAB_MAP_WIDGET.Controls.Add(this.txtMapWidgetXml);
            this.TAB_MAP_WIDGET.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.TAB_MAP_WIDGET, "TAB_MAP_WIDGET");
            this.TAB_MAP_WIDGET.Name = "TAB_MAP_WIDGET";
            this.TAB_MAP_WIDGET.UseVisualStyleBackColor = true;
            // 
            // txtMapWidgetXml
            // 
            resources.ApplyResources(this.txtMapWidgetXml, "txtMapWidgetXml");
            this.txtMapWidgetXml.IsReadOnly = false;
            this.txtMapWidgetXml.Name = "txtMapWidgetXml";
            this.txtMapWidgetXml.TextChanged += new System.EventHandler(this.txtMapWidgetXml_TextChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveMapWidgetXml});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnSaveMapWidgetXml
            // 
            resources.ApplyResources(this.btnSaveMapWidgetXml, "btnSaveMapWidgetXml");
            this.btnSaveMapWidgetXml.Image = global::Maestro.Editors.Properties.Resources.disk;
            this.btnSaveMapWidgetXml.Name = "btnSaveMapWidgetXml";
            this.btnSaveMapWidgetXml.Click += new System.EventHandler(this.btnSaveMapWidgetXml_Click);
            // 
            // TAB_OTHER_WIDGETS
            // 
            this.TAB_OTHER_WIDGETS.Controls.Add(this.groupBox1);
            this.TAB_OTHER_WIDGETS.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.TAB_OTHER_WIDGETS, "TAB_OTHER_WIDGETS");
            this.TAB_OTHER_WIDGETS.Name = "TAB_OTHER_WIDGETS";
            this.TAB_OTHER_WIDGETS.UseVisualStyleBackColor = true;
            // 
            // WidgetManagementDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.tabWidgets);
            this.Controls.Add(this.lblNonDockableNote);
            this.Controls.Add(this.btnClose);
            this.Name = "WidgetManagementDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWidgets)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabWidgets.ResumeLayout(false);
            this.TAB_MAP_WIDGET.ResumeLayout(false);
            this.TAB_MAP_WIDGET.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.TAB_OTHER_WIDGETS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.DataGridView grdWidgets;
        private System.Windows.Forms.DataGridViewCheckBoxColumn COL_DOCKABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_TYPE;
        private System.Windows.Forms.Label lblNonDockableNote;
        private System.Windows.Forms.TabControl tabWidgets;
        private System.Windows.Forms.TabPage TAB_MAP_WIDGET;
        private System.Windows.Forms.TabPage TAB_OTHER_WIDGETS;
        private ICSharpCode.TextEditor.TextEditorControl txtMapWidgetXml;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnSaveMapWidgetXml;

    }
}