namespace Maestro.Editors.FeatureSource.Preview
{
    partial class StandardQueryCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardQueryCtrl));
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.TAB_SELECTED = new System.Windows.Forms.TabPage();
            this.btnCheckNone = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.chkProperties = new System.Windows.Forms.CheckedListBox();
            this.TAB_COMPUTED = new System.Windows.Forms.TabPage();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.grdExpressions = new System.Windows.Forms.DataGridView();
            this.COL_ALIAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_EXPR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.tabProperties.SuspendLayout();
            this.TAB_SELECTED.SuspendLayout();
            this.TAB_COMPUTED.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExpressions)).BeginInit();
            this.SuspendLayout();
            // 
            // tabProperties
            // 
            resources.ApplyResources(this.tabProperties, "tabProperties");
            this.tabProperties.Controls.Add(this.TAB_SELECTED);
            this.tabProperties.Controls.Add(this.TAB_COMPUTED);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            // 
            // TAB_SELECTED
            // 
            this.TAB_SELECTED.Controls.Add(this.btnCheckNone);
            this.TAB_SELECTED.Controls.Add(this.btnCheckAll);
            this.TAB_SELECTED.Controls.Add(this.chkProperties);
            resources.ApplyResources(this.TAB_SELECTED, "TAB_SELECTED");
            this.TAB_SELECTED.Name = "TAB_SELECTED";
            this.TAB_SELECTED.UseVisualStyleBackColor = true;
            // 
            // btnCheckNone
            // 
            resources.ApplyResources(this.btnCheckNone, "btnCheckNone");
            this.btnCheckNone.Name = "btnCheckNone";
            this.btnCheckNone.UseVisualStyleBackColor = true;
            this.btnCheckNone.Click += new System.EventHandler(this.btnCheckNone_Click);
            // 
            // btnCheckAll
            // 
            resources.ApplyResources(this.btnCheckAll, "btnCheckAll");
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // chkProperties
            // 
            resources.ApplyResources(this.chkProperties, "chkProperties");
            this.chkProperties.FormattingEnabled = true;
            this.chkProperties.Name = "chkProperties";
            // 
            // TAB_COMPUTED
            // 
            this.TAB_COMPUTED.Controls.Add(this.btnDelete);
            this.TAB_COMPUTED.Controls.Add(this.btnAdd);
            this.TAB_COMPUTED.Controls.Add(this.grdExpressions);
            resources.ApplyResources(this.TAB_COMPUTED, "TAB_COMPUTED");
            this.TAB_COMPUTED.Name = "TAB_COMPUTED";
            this.TAB_COMPUTED.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grdExpressions
            // 
            this.grdExpressions.AllowUserToAddRows = false;
            this.grdExpressions.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdExpressions, "grdExpressions");
            this.grdExpressions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExpressions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_ALIAS,
            this.COL_EXPR});
            this.grdExpressions.Name = "grdExpressions";
            // 
            // COL_ALIAS
            // 
            resources.ApplyResources(this.COL_ALIAS, "COL_ALIAS");
            this.COL_ALIAS.Name = "COL_ALIAS";
            // 
            // COL_EXPR
            // 
            this.COL_EXPR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.COL_EXPR, "COL_EXPR");
            this.COL_EXPR.Name = "COL_EXPR";
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Click += new System.EventHandler(this.txtFilter_Click);
            // 
            // lblFilter
            // 
            resources.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            // 
            // StandardQueryCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tabProperties);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.lblFilter);
            this.Name = "StandardQueryCtrl";
            resources.ApplyResources(this, "$this");
            this.tabProperties.ResumeLayout(false);
            this.TAB_SELECTED.ResumeLayout(false);
            this.TAB_COMPUTED.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExpressions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TabPage TAB_SELECTED;
        private System.Windows.Forms.Button btnCheckNone;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.CheckedListBox chkProperties;
        private System.Windows.Forms.TabPage TAB_COMPUTED;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView grdExpressions;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_ALIAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_EXPR;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
    }
}
