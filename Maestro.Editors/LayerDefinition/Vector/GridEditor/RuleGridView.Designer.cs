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
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.btnDuplicateRule = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateTheme = new System.Windows.Forms.ToolStripButton();
            this.btnExplodeTheme = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefreshStylePreviews = new System.Windows.Forms.ToolStripButton();
            this.btnShowInLegend = new System.Windows.Forms.ToolStripButton();
            this.btnDisplayAsText = new System.Windows.Forms.ToolStripButton();
            this.btnAllowOverpost = new System.Windows.Forms.ToolStripButton();
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
            this.btnUp,
            this.btnDown,
            this.btnDuplicateRule,
            this.toolStripSeparator3,
            this.btnCreateTheme,
            this.btnExplodeTheme,
            this.toolStripSeparator1,
            this.btnRefreshStylePreviews,
            this.btnShowInLegend,
            this.btnDisplayAsText,
            this.btnAllowOverpost});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddRule
            // 
            this.btnAddRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddRule.Image = global::Maestro.Editors.Properties.Resources.plus;
            this.btnAddRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(23, 22);
            this.btnAddRule.Text = "Add";
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // btnDeleteRule
            // 
            this.btnDeleteRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteRule.Enabled = false;
            this.btnDeleteRule.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDeleteRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRule.Name = "btnDeleteRule";
            this.btnDeleteRule.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteRule.Text = "Delete";
            this.btnDeleteRule.ToolTipText = "Delete selected rules";
            this.btnDeleteRule.Click += new System.EventHandler(this.btnDeleteRule_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Enabled = false;
            this.btnUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "Move Up";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDown.Enabled = false;
            this.btnDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Text = "Move Down";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDuplicateRule
            // 
            this.btnDuplicateRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDuplicateRule.Enabled = false;
            this.btnDuplicateRule.Image = global::Maestro.Editors.Properties.Resources.document_copy;
            this.btnDuplicateRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDuplicateRule.Name = "btnDuplicateRule";
            this.btnDuplicateRule.Size = new System.Drawing.Size(23, 22);
            this.btnDuplicateRule.Text = "Duplicate";
            this.btnDuplicateRule.ToolTipText = "Duplicate Selected Rule";
            this.btnDuplicateRule.Click += new System.EventHandler(this.btnDuplicateRule_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
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
            this.btnRefreshStylePreviews.Size = new System.Drawing.Size(115, 22);
            this.btnRefreshStylePreviews.Text = "Refresh Previews";
            this.btnRefreshStylePreviews.Click += new System.EventHandler(this.btnRefreshStylePreviews_Click);
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
            // btnDisplayAsText
            // 
            this.btnDisplayAsText.CheckOnClick = true;
            this.btnDisplayAsText.Image = global::Maestro.Editors.Properties.Resources.tick;
            this.btnDisplayAsText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisplayAsText.Name = "btnDisplayAsText";
            this.btnDisplayAsText.Size = new System.Drawing.Size(106, 22);
            this.btnDisplayAsText.Text = "Display As Text";
            this.btnDisplayAsText.Click += new System.EventHandler(this.btnDisplayAsText_Click);
            // 
            // btnAllowOverpost
            // 
            this.btnAllowOverpost.CheckOnClick = true;
            this.btnAllowOverpost.Image = global::Maestro.Editors.Properties.Resources.tick;
            this.btnAllowOverpost.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAllowOverpost.Name = "btnAllowOverpost";
            this.btnAllowOverpost.Size = new System.Drawing.Size(108, 22);
            this.btnAllowOverpost.Text = "Allow Overpost";
            this.btnAllowOverpost.Click += new System.EventHandler(this.btnAllowOverpost_Click);
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
        private System.Windows.Forms.ToolStripButton btnDisplayAsText;
        private System.Windows.Forms.ToolStripButton btnAllowOverpost;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ToolStripButton btnDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnDuplicateRule;
    }
}
