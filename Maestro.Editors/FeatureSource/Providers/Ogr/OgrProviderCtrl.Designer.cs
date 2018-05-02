namespace Maestro.Editors.FeatureSource.Providers.Ogr
{
    partial class OgrProviderCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grdOtherProperties = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtConnectionStatus = new System.Windows.Forms.TextBox();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOtherProperties)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.groupBox3);
            this.contentPanel.Controls.Add(this.btnTest);
            this.contentPanel.Controls.Add(this.grdOtherProperties);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.chkReadOnly);
            this.contentPanel.Controls.Add(this.txtDataSource);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Size = new System.Drawing.Size(510, 302);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Source";
            // 
            // txtDataSource
            // 
            this.txtDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataSource.Location = new System.Drawing.Point(137, 15);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(367, 20);
            this.txtDataSource.TabIndex = 1;
            this.txtDataSource.TextChanged += new System.EventHandler(this.txtDataSource_TextChanged);
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(137, 41);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(76, 17);
            this.chkReadOnly.TabIndex = 2;
            this.chkReadOnly.Text = "Read Only";
            this.chkReadOnly.UseVisualStyleBackColor = true;
            this.chkReadOnly.CheckedChanged += new System.EventHandler(this.chkReadOnly_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Other Properties";
            // 
            // grdOtherProperties
            // 
            this.grdOtherProperties.AllowUserToAddRows = false;
            this.grdOtherProperties.AllowUserToDeleteRows = false;
            this.grdOtherProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOtherProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOtherProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_NAME,
            this.Value});
            this.grdOtherProperties.Location = new System.Drawing.Point(137, 64);
            this.grdOtherProperties.Name = "grdOtherProperties";
            this.grdOtherProperties.Size = new System.Drawing.Size(367, 146);
            this.grdOtherProperties.TabIndex = 4;
            this.grdOtherProperties.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdOtherProperties_CellPainting);
            this.grdOtherProperties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdOtherProperties_CellValueChanged);
            this.grdOtherProperties.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdOtherProperties_EditingControlShowing);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(17, 228);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(105, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtConnectionStatus);
            this.groupBox3.Location = new System.Drawing.Point(137, 228);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(370, 61);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Messages";
            // 
            // txtConnectionStatus
            // 
            this.txtConnectionStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnectionStatus.Location = new System.Drawing.Point(3, 16);
            this.txtConnectionStatus.Multiline = true;
            this.txtConnectionStatus.Name = "txtConnectionStatus";
            this.txtConnectionStatus.ReadOnly = true;
            this.txtConnectionStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionStatus.Size = new System.Drawing.Size(364, 42);
            this.txtConnectionStatus.TabIndex = 7;
            // 
            // COL_NAME
            // 
            this.COL_NAME.HeaderText = "Name";
            this.COL_NAME.Name = "COL_NAME";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // OgrProviderCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "OGR Feature Source";
            this.Name = "OgrProviderCtrl";
            this.Size = new System.Drawing.Size(510, 329);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOtherProperties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdOtherProperties;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtConnectionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}
