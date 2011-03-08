namespace Maestro.Editors.WebLayout
{
    partial class WebLayoutCommandsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebLayoutCommandsCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdCommands = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.invokeURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invokeScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.grpCommand = new System.Windows.Forms.GroupBox();
            this.contentPanel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommands)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpCommand);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.grdCommands);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // grdCommands
            // 
            this.grdCommands.AllowUserToAddRows = false;
            this.grdCommands.AllowUserToDeleteRows = false;
            this.grdCommands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.grdCommands, "grdCommands");
            this.grdCommands.Name = "grdCommands";
            this.grdCommands.ReadOnly = true;
            this.grdCommands.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCommands_CellContentClick);
            this.grdCommands.SelectionChanged += new System.EventHandler(this.grdCommands_SelectionChanged);
            this.grdCommands.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCommands_CellContentClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnExport,
            this.btnImport});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invokeURLToolStripMenuItem,
            this.invokeScriptToolStripMenuItem,
            this.searchToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.application__plus;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // invokeURLToolStripMenuItem
            // 
            this.invokeURLToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.icon_invokeurl;
            this.invokeURLToolStripMenuItem.Name = "invokeURLToolStripMenuItem";
            resources.ApplyResources(this.invokeURLToolStripMenuItem, "invokeURLToolStripMenuItem");
            this.invokeURLToolStripMenuItem.Click += new System.EventHandler(this.invokeURLToolStripMenuItem_Click);
            // 
            // invokeScriptToolStripMenuItem
            // 
            this.invokeScriptToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.icon_invokescript;
            this.invokeScriptToolStripMenuItem.Name = "invokeScriptToolStripMenuItem";
            resources.ApplyResources(this.invokeScriptToolStripMenuItem, "invokeScriptToolStripMenuItem");
            this.invokeScriptToolStripMenuItem.Click += new System.EventHandler(this.invokeScriptToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.icon_search;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            resources.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.application__minus;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Image = global::Maestro.Editors.Properties.Resources.application_export;
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = global::Maestro.Editors.Properties.Resources.application_import;
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // grpCommand
            // 
            resources.ApplyResources(this.grpCommand, "grpCommand");
            this.grpCommand.Name = "grpCommand";
            this.grpCommand.TabStop = false;
            // 
            // WebLayoutCommandsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Commands";
            this.Name = "WebLayoutCommandsCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommands)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox grpCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdCommands;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem invokeURLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invokeScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripButton btnImport;

    }
}
