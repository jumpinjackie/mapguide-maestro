namespace Maestro.Editors.FeatureSource.Preview
{
    partial class PreviewPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewPane));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRunQuery = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.queryPane = new System.Windows.Forms.Panel();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.queryWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRunQuery,
            this.btnStop,
            this.btnClear});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnRunQuery
            // 
            this.btnRunQuery.Image = global::Maestro.Editors.Properties.Resources.control;
            resources.ApplyResources(this.btnRunQuery, "btnRunQuery");
            this.btnRunQuery.Name = "btnRunQuery";
            this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Image = global::Maestro.Editors.Properties.Resources.control_stop_square;
            this.btnStop.Name = "btnStop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // queryPane
            // 
            resources.ApplyResources(this.queryPane, "queryPane");
            this.queryPane.Name = "queryPane";
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCount,
            this.lblElapsed});
            resources.ApplyResources(this.statusBar, "statusBar");
            this.statusBar.Name = "statusBar";
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            resources.ApplyResources(this.lblCount, "lblCount");
            // 
            // lblElapsed
            // 
            this.lblElapsed.Name = "lblElapsed";
            resources.ApplyResources(this.lblElapsed, "lblElapsed");
            this.lblElapsed.Spring = true;
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToOrderColumns = true;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.grdResults, "grdResults");
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            // 
            // queryWorker
            // 
            this.queryWorker.WorkerSupportsCancellation = true;
            this.queryWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.queryWorker_DoWork);
            this.queryWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.queryWorker_RunWorkerCompleted);
            // 
            // PreviewPane
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grdResults);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.queryPane);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PreviewPane";
            resources.ApplyResources(this, "$this");
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel queryPane;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.ToolStripButton btnRunQuery;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.ComponentModel.BackgroundWorker queryWorker;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.ToolStripStatusLabel lblElapsed;
    }
}
