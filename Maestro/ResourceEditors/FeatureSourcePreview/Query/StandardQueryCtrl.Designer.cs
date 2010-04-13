namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourcePreview.Query
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
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
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
            this.tabProperties.SuspendLayout();
            this.TAB_SELECTED.SuspendLayout();
            this.TAB_COMPUTED.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExpressions)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFilter.Location = new System.Drawing.Point(15, 24);
            this.txtFilter.Multiline = true;
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(153, 139);
            this.txtFilter.TabIndex = 7;
            this.txtFilter.Enter += new System.EventHandler(this.txtFilter_Enter);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(12, 8);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(29, 13);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "Filter";
            // 
            // tabProperties
            // 
            this.tabProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabProperties.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabProperties.Controls.Add(this.TAB_SELECTED);
            this.tabProperties.Controls.Add(this.TAB_COMPUTED);
            this.tabProperties.Location = new System.Drawing.Point(174, 8);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(326, 165);
            this.tabProperties.TabIndex = 8;
            // 
            // TAB_SELECTED
            // 
            this.TAB_SELECTED.Controls.Add(this.btnCheckNone);
            this.TAB_SELECTED.Controls.Add(this.btnCheckAll);
            this.TAB_SELECTED.Controls.Add(this.chkProperties);
            this.TAB_SELECTED.Location = new System.Drawing.Point(4, 25);
            this.TAB_SELECTED.Name = "TAB_SELECTED";
            this.TAB_SELECTED.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_SELECTED.Size = new System.Drawing.Size(318, 136);
            this.TAB_SELECTED.TabIndex = 0;
            this.TAB_SELECTED.Text = "Selected Properties";
            this.TAB_SELECTED.UseVisualStyleBackColor = true;
            // 
            // btnCheckNone
            // 
            this.btnCheckNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckNone.Location = new System.Drawing.Point(89, 106);
            this.btnCheckNone.Name = "btnCheckNone";
            this.btnCheckNone.Size = new System.Drawing.Size(75, 23);
            this.btnCheckNone.TabIndex = 2;
            this.btnCheckNone.Text = "Check None";
            this.btnCheckNone.UseVisualStyleBackColor = true;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckAll.Location = new System.Drawing.Point(7, 106);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 1;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            // 
            // chkProperties
            // 
            this.chkProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkProperties.FormattingEnabled = true;
            this.chkProperties.Location = new System.Drawing.Point(0, 0);
            this.chkProperties.Name = "chkProperties";
            this.chkProperties.Size = new System.Drawing.Size(318, 94);
            this.chkProperties.TabIndex = 0;
            // 
            // TAB_COMPUTED
            // 
            this.TAB_COMPUTED.Controls.Add(this.btnDelete);
            this.TAB_COMPUTED.Controls.Add(this.btnAdd);
            this.TAB_COMPUTED.Controls.Add(this.grdExpressions);
            this.TAB_COMPUTED.Location = new System.Drawing.Point(4, 25);
            this.TAB_COMPUTED.Name = "TAB_COMPUTED";
            this.TAB_COMPUTED.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_COMPUTED.Size = new System.Drawing.Size(318, 136);
            this.TAB_COMPUTED.TabIndex = 1;
            this.TAB_COMPUTED.Text = "Computed Properties";
            this.TAB_COMPUTED.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(88, 107);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(6, 107);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grdExpressions
            // 
            this.grdExpressions.AllowUserToAddRows = false;
            this.grdExpressions.AllowUserToDeleteRows = false;
            this.grdExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdExpressions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExpressions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_ALIAS,
            this.COL_EXPR});
            this.grdExpressions.Location = new System.Drawing.Point(0, 0);
            this.grdExpressions.Name = "grdExpressions";
            this.grdExpressions.Size = new System.Drawing.Size(318, 100);
            this.grdExpressions.TabIndex = 0;
            // 
            // COL_ALIAS
            // 
            this.COL_ALIAS.HeaderText = "Alias";
            this.COL_ALIAS.Name = "COL_ALIAS";
            // 
            // COL_EXPR
            // 
            this.COL_EXPR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_EXPR.HeaderText = "Expression";
            this.COL_EXPR.Name = "COL_EXPR";
            // 
            // StandardQueryCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabProperties);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.lblFilter);
            this.Name = "StandardQueryCtl";
            this.Size = new System.Drawing.Size(503, 176);
            this.tabProperties.ResumeLayout(false);
            this.TAB_SELECTED.ResumeLayout(false);
            this.TAB_COMPUTED.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExpressions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TabPage TAB_SELECTED;
        private System.Windows.Forms.TabPage TAB_COMPUTED;
        private System.Windows.Forms.Button btnCheckNone;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.CheckedListBox chkProperties;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView grdExpressions;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_ALIAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_EXPR;

    }
}
