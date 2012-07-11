namespace Maestro.Editors.FeatureSource.Providers
{
    partial class GenericCtrl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.grdConnectionParameters = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbLongTransaction = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtTestResult = new System.Windows.Forms.TextBox();
            this.resDataCtrl = new Maestro.Editors.Common.ResourceDataCtrl();
            this.ctxEnumerable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pickAValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickADataStoreFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxProperty = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pickAnAliasedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickAnAliasedDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mGDATAFILEPATHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkSetCredentials = new System.Windows.Forms.LinkLabel();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdConnectionParameters)).BeginInit();
            this.ctxEnumerable.SuspendLayout();
            this.ctxProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.lnkSetCredentials);
            this.contentPanel.Controls.Add(this.txtTestResult);
            this.contentPanel.Controls.Add(this.btnTest);
            this.contentPanel.Controls.Add(this.cmbLongTransaction);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.resDataCtrl);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.grdConnectionParameters);
            this.contentPanel.Controls.Add(this.txtProvider);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtProvider
            // 
            resources.ApplyResources(this.txtProvider, "txtProvider");
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            // 
            // grdConnectionParameters
            // 
            this.grdConnectionParameters.AllowUserToAddRows = false;
            this.grdConnectionParameters.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdConnectionParameters, "grdConnectionParameters");
            this.grdConnectionParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdConnectionParameters.Name = "grdConnectionParameters";
            this.grdConnectionParameters.RowHeadersVisible = false;
            this.grdConnectionParameters.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdConnectionParameters_CellPainting);
            this.grdConnectionParameters.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdConnectionParameters_CellValueChanged);
            this.grdConnectionParameters.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdConnectionParameters_EditingControlShowing);
            this.grdConnectionParameters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdConnectionParameters_MouseClick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbLongTransaction
            // 
            resources.ApplyResources(this.cmbLongTransaction, "cmbLongTransaction");
            this.cmbLongTransaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLongTransaction.FormattingEnabled = true;
            this.cmbLongTransaction.Name = "cmbLongTransaction";
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtTestResult
            // 
            resources.ApplyResources(this.txtTestResult, "txtTestResult");
            this.txtTestResult.Name = "txtTestResult";
            this.txtTestResult.ReadOnly = true;
            // 
            // resDataCtrl
            // 
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.MarkedFile = "";
            this.resDataCtrl.MarkEnabled = true;
            this.resDataCtrl.Name = "resDataCtrl";
            // 
            // ctxEnumerable
            // 
            this.ctxEnumerable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pickAValueToolStripMenuItem,
            this.pickADataStoreFromListToolStripMenuItem});
            this.ctxEnumerable.Name = "ctxEnumerable";
            resources.ApplyResources(this.ctxEnumerable, "ctxEnumerable");
            // 
            // pickAValueToolStripMenuItem
            // 
            this.pickAValueToolStripMenuItem.Name = "pickAValueToolStripMenuItem";
            resources.ApplyResources(this.pickAValueToolStripMenuItem, "pickAValueToolStripMenuItem");
            this.pickAValueToolStripMenuItem.Click += new System.EventHandler(this.pickAValueToolStripMenuItem_Click);
            // 
            // pickADataStoreFromListToolStripMenuItem
            // 
            this.pickADataStoreFromListToolStripMenuItem.Name = "pickADataStoreFromListToolStripMenuItem";
            resources.ApplyResources(this.pickADataStoreFromListToolStripMenuItem, "pickADataStoreFromListToolStripMenuItem");
            this.pickADataStoreFromListToolStripMenuItem.Click += new System.EventHandler(this.pickADataStoreFromListToolStripMenuItem_Click);
            // 
            // ctxProperty
            // 
            this.ctxProperty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pickAnAliasedFileToolStripMenuItem,
            this.pickAnAliasedDirectoryToolStripMenuItem,
            this.mGDATAFILEPATHToolStripMenuItem});
            this.ctxProperty.Name = "ctxProperty";
            resources.ApplyResources(this.ctxProperty, "ctxProperty");
            // 
            // pickAnAliasedFileToolStripMenuItem
            // 
            this.pickAnAliasedFileToolStripMenuItem.Name = "pickAnAliasedFileToolStripMenuItem";
            resources.ApplyResources(this.pickAnAliasedFileToolStripMenuItem, "pickAnAliasedFileToolStripMenuItem");
            this.pickAnAliasedFileToolStripMenuItem.Click += new System.EventHandler(this.pickAnAliasedFileToolStripMenuItem_Click);
            // 
            // pickAnAliasedDirectoryToolStripMenuItem
            // 
            this.pickAnAliasedDirectoryToolStripMenuItem.Name = "pickAnAliasedDirectoryToolStripMenuItem";
            resources.ApplyResources(this.pickAnAliasedDirectoryToolStripMenuItem, "pickAnAliasedDirectoryToolStripMenuItem");
            this.pickAnAliasedDirectoryToolStripMenuItem.Click += new System.EventHandler(this.pickAnAliasedDirectoryToolStripMenuItem_Click);
            // 
            // mGDATAFILEPATHToolStripMenuItem
            // 
            this.mGDATAFILEPATHToolStripMenuItem.Name = "mGDATAFILEPATHToolStripMenuItem";
            resources.ApplyResources(this.mGDATAFILEPATHToolStripMenuItem, "mGDATAFILEPATHToolStripMenuItem");
            this.mGDATAFILEPATHToolStripMenuItem.Click += new System.EventHandler(this.useActiveResourceDataFile_Click);
            // 
            // lnkSetCredentials
            // 
            resources.ApplyResources(this.lnkSetCredentials, "lnkSetCredentials");
            this.lnkSetCredentials.Name = "lnkSetCredentials";
            this.lnkSetCredentials.TabStop = true;
            this.lnkSetCredentials.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSetCredentials_LinkClicked);
            // 
            // GenericCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "GenericCtrl";
            this.Controls.SetChildIndex(this.contentPanel, 0);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdConnectionParameters)).EndInit();
            this.ctxEnumerable.ResumeLayout(false);
            this.ctxProperty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdConnectionParameters;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label1;
        private Maestro.Editors.Common.ResourceDataCtrl resDataCtrl;
        private System.Windows.Forms.TextBox txtTestResult;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cmbLongTransaction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip ctxEnumerable;
        private System.Windows.Forms.ContextMenuStrip ctxProperty;
        private System.Windows.Forms.ToolStripMenuItem pickAValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pickAnAliasedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pickAnAliasedDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mGDATAFILEPATHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pickADataStoreFromListToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnkSetCredentials;
    }
}
