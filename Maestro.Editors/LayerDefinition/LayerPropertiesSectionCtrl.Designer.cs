namespace Maestro.Editors.LayerDefinition
{
    partial class LayerPropertiesSectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerPropertiesSectionCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCheckAll = new System.Windows.Forms.ToolStripButton();
            this.btnUncheckAll = new System.Windows.Forms.ToolStripButton();
            this.btnInvert = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.grdProperties = new System.Windows.Forms.DataGridView();
            this.COL_VISIBLE = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_DISPLAY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.grdProperties);
            this.contentPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCheckAll,
            this.btnUncheckAll,
            this.btnInvert,
            this.toolStripSeparator1,
            this.btnMoveUp,
            this.btnMoveDown});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Image = global::Maestro.Editors.Properties.Resources.tick;
            resources.ApplyResources(this.btnCheckAll, "btnCheckAll");
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Image = global::Maestro.Editors.Properties.Resources.cross;
            resources.ApplyResources(this.btnUncheckAll, "btnUncheckAll");
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnInvert
            // 
            this.btnInvert.Image = global::Maestro.Editors.Properties.Resources.arrow_return_180;
            resources.ApplyResources(this.btnInvert, "btnInvert");
            this.btnInvert.Name = "btnInvert";
            this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // grdProperties
            // 
            this.grdProperties.AllowUserToAddRows = false;
            this.grdProperties.AllowUserToDeleteRows = false;
            this.grdProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_VISIBLE,
            this.COL_NAME,
            this.COL_DISPLAY});
            resources.ApplyResources(this.grdProperties, "grdProperties");
            this.grdProperties.Name = "grdProperties";
            this.grdProperties.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdProperties_CellEndEdit);
            this.grdProperties.SelectionChanged += new System.EventHandler(this.grdProperties_SelectionChanged);
            // 
            // COL_VISIBLE
            // 
            resources.ApplyResources(this.COL_VISIBLE, "COL_VISIBLE");
            this.COL_VISIBLE.Name = "COL_VISIBLE";
            // 
            // COL_NAME
            // 
            resources.ApplyResources(this.COL_NAME, "COL_NAME");
            this.COL_NAME.Name = "COL_NAME";
            this.COL_NAME.ReadOnly = true;
            // 
            // COL_DISPLAY
            // 
            this.COL_DISPLAY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.COL_DISPLAY, "COL_DISPLAY");
            this.COL_DISPLAY.Name = "COL_DISPLAY";
            // 
            // LayerPropertiesSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "LayerPropertiesSectionCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProperties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCheckAll;
        private System.Windows.Forms.ToolStripButton btnUncheckAll;
        private System.Windows.Forms.ToolStripButton btnInvert;
        private System.Windows.Forms.DataGridView grdProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.DataGridViewCheckBoxColumn COL_VISIBLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_DISPLAY;
    }
}
