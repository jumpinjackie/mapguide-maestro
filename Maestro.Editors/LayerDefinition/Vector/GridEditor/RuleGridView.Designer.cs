namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    partial class RuleGridView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleGridView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddRule = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteRule = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateTheme = new System.Windows.Forms.ToolStripButton();
            this.btnExplodeTheme = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefreshStylePreviews = new System.Windows.Forms.ToolStripButton();
            this.btnAutoRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnShowInLegend = new System.Windows.Forms.ToolStripButton();
            this.grdRules = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRules)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddRule,
            this.btnDeleteRule,
            this.toolStripSeparator2,
            this.btnCreateTheme,
            this.btnExplodeTheme,
            this.toolStripSeparator1,
            this.btnRefreshStylePreviews,
            this.btnAutoRefresh,
            this.btnShowInLegend});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddRule
            // 
            this.btnAddRule.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            this.btnAddRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(75, 22);
            this.btnAddRule.Text = "Add Rule";
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // btnDeleteRule
            // 
            this.btnDeleteRule.Enabled = false;
            this.btnDeleteRule.Image = global::Maestro.Editors.Properties.Resources.minus_circle;
            this.btnDeleteRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRule.Name = "btnDeleteRule";
            this.btnDeleteRule.Size = new System.Drawing.Size(91, 22);
            this.btnDeleteRule.Text = "Delete Rules";
            this.btnDeleteRule.ToolTipText = "Delete selected rules";
            this.btnDeleteRule.Click += new System.EventHandler(this.btnDeleteRule_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCreateTheme
            // 
            this.btnCreateTheme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateTheme.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateTheme.Image")));
            this.btnCreateTheme.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateTheme.Name = "btnCreateTheme";
            this.btnCreateTheme.Size = new System.Drawing.Size(23, 22);
            this.btnCreateTheme.Text = "Create Theme";
            this.btnCreateTheme.Click += new System.EventHandler(this.btnCreateTheme_Click);
            // 
            // btnExplodeTheme
            // 
            this.btnExplodeTheme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExplodeTheme.Enabled = false;
            this.btnExplodeTheme.Image = global::Maestro.Editors.Properties.Resources.arrow_split;
            this.btnExplodeTheme.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExplodeTheme.Name = "btnExplodeTheme";
            this.btnExplodeTheme.Size = new System.Drawing.Size(23, 22);
            this.btnExplodeTheme.Text = "Explode Theme";
            this.btnExplodeTheme.Click += new System.EventHandler(this.btnExplodeTheme_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefreshStylePreviews
            // 
            this.btnRefreshStylePreviews.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            this.btnRefreshStylePreviews.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshStylePreviews.Name = "btnRefreshStylePreviews";
            this.btnRefreshStylePreviews.Size = new System.Drawing.Size(143, 22);
            this.btnRefreshStylePreviews.Text = "Refresh Style Previews";
            this.btnRefreshStylePreviews.Click += new System.EventHandler(this.btnRefreshStylePreviews_Click);
            // 
            // btnAutoRefresh
            // 
            this.btnAutoRefresh.CheckOnClick = true;
            this.btnAutoRefresh.Image = global::Maestro.Editors.Properties.Resources.tick;
            this.btnAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAutoRefresh.Name = "btnAutoRefresh";
            this.btnAutoRefresh.Size = new System.Drawing.Size(97, 22);
            this.btnAutoRefresh.Text = "Auto-Refresh";
            this.btnAutoRefresh.ToolTipText = "Style previews for currently visible rules will be automatically updated as you s" +
    "croll the grid. Otherwise, you have to manually refresh these previews";
            // 
            // btnShowInLegend
            // 
            this.btnShowInLegend.Checked = true;
            this.btnShowInLegend.CheckOnClick = true;
            this.btnShowInLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnShowInLegend.Image = global::Maestro.Editors.Properties.Resources.document_tree;
            this.btnShowInLegend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowInLegend.Name = "btnShowInLegend";
            this.btnShowInLegend.Size = new System.Drawing.Size(111, 22);
            this.btnShowInLegend.Text = "Show In Legend";
            this.btnShowInLegend.ToolTipText = "These rules will be shown in the legend if checked";
            this.btnShowInLegend.Click += new System.EventHandler(this.btnShowInLegend_Click);
            // 
            // grdRules
            // 
            this.grdRules.AllowUserToAddRows = false;
            this.grdRules.AllowUserToDeleteRows = false;
            this.grdRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRules.Location = new System.Drawing.Point(0, 25);
            this.grdRules.Name = "grdRules";
            this.grdRules.Size = new System.Drawing.Size(785, 392);
            this.grdRules.TabIndex = 1;
            this.grdRules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRules_CellClick);
            this.grdRules.Scroll += new System.Windows.Forms.ScrollEventHandler(this.grdRules_Scroll);
            this.grdRules.SelectionChanged += new System.EventHandler(this.grdRules_SelectionChanged);
            // 
            // RuleGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdRules);
            this.Controls.Add(this.toolStrip1);
            this.Name = "RuleGridView";
            this.Size = new System.Drawing.Size(785, 417);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView grdRules;
        private System.Windows.Forms.ToolStripButton btnAddRule;
        private System.Windows.Forms.ToolStripButton btnDeleteRule;
        private System.Windows.Forms.ToolStripButton btnCreateTheme;
        private System.Windows.Forms.ToolStripButton btnExplodeTheme;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnShowInLegend;
        private System.Windows.Forms.ToolStripButton btnRefreshStylePreviews;
        private System.Windows.Forms.ToolStripButton btnAutoRefresh;
    }
}
