namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    partial class WmsAdvancedConfigurationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WmsAdvancedConfigurationDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFeatureServer = new System.Windows.Forms.TextBox();
            this.grpRaster = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstFeatureClasses = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdSpatialContexts = new System.Windows.Forms.DataGridView();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_CS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpLogicalClass = new System.Windows.Forms.GroupBox();
            this.lnkSwap = new System.Windows.Forms.LinkLabel();
            this.txtClassDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSwapAll = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpatialContexts)).BeginInit();
            this.grpLogicalClass.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtFeatureServer
            // 
            resources.ApplyResources(this.txtFeatureServer, "txtFeatureServer");
            this.txtFeatureServer.Name = "txtFeatureServer";
            this.txtFeatureServer.ReadOnly = true;
            // 
            // grpRaster
            // 
            resources.ApplyResources(this.grpRaster, "grpRaster");
            this.grpRaster.Name = "grpRaster";
            this.grpRaster.TabStop = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.lstFeatureClasses);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lstFeatureClasses
            // 
            this.lstFeatureClasses.DisplayMember = "FeatureClass";
            resources.ApplyResources(this.lstFeatureClasses, "lstFeatureClasses");
            this.lstFeatureClasses.FormattingEnabled = true;
            this.lstFeatureClasses.Name = "lstFeatureClasses";
            this.lstFeatureClasses.SelectedIndexChanged += new System.EventHandler(this.lstFeatureClasses_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnRemove});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Image = global::Maestro.Editors.Properties.Resources.cross;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.grdSpatialContexts);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // grdSpatialContexts
            // 
            this.grdSpatialContexts.AllowUserToAddRows = false;
            this.grdSpatialContexts.AllowUserToDeleteRows = false;
            this.grdSpatialContexts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSpatialContexts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_NAME,
            this.COL_CS});
            resources.ApplyResources(this.grdSpatialContexts, "grdSpatialContexts");
            this.grdSpatialContexts.Name = "grdSpatialContexts";
            this.grdSpatialContexts.ReadOnly = true;
            this.grdSpatialContexts.RowHeadersVisible = false;
            this.grdSpatialContexts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSpatialContexts_CellContentClick);
            // 
            // COL_NAME
            // 
            this.COL_NAME.DataPropertyName = "Name";
            resources.ApplyResources(this.COL_NAME, "COL_NAME");
            this.COL_NAME.Name = "COL_NAME";
            this.COL_NAME.ReadOnly = true;
            // 
            // COL_CS
            // 
            this.COL_CS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_CS.DataPropertyName = "CoordinateSystemWkt";
            resources.ApplyResources(this.COL_CS, "COL_CS");
            this.COL_CS.Name = "COL_CS";
            this.COL_CS.ReadOnly = true;
            // 
            // grpLogicalClass
            // 
            resources.ApplyResources(this.grpLogicalClass, "grpLogicalClass");
            this.grpLogicalClass.Controls.Add(this.lnkSwap);
            this.grpLogicalClass.Controls.Add(this.txtClassDescription);
            this.grpLogicalClass.Controls.Add(this.label3);
            this.grpLogicalClass.Controls.Add(this.txtClassName);
            this.grpLogicalClass.Controls.Add(this.label2);
            this.grpLogicalClass.Name = "grpLogicalClass";
            this.grpLogicalClass.TabStop = false;
            // 
            // lnkSwap
            // 
            resources.ApplyResources(this.lnkSwap, "lnkSwap");
            this.lnkSwap.Name = "lnkSwap";
            this.lnkSwap.TabStop = true;
            this.lnkSwap.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSwap_LinkClicked);
            // 
            // txtClassDescription
            // 
            resources.ApplyResources(this.txtClassDescription, "txtClassDescription");
            this.txtClassDescription.Name = "txtClassDescription";
            this.txtClassDescription.TextChanged += new System.EventHandler(this.txtClassDescription_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtClassName
            // 
            resources.ApplyResources(this.txtClassName, "txtClassName");
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnSwapAll
            // 
            resources.ApplyResources(this.btnSwapAll, "btnSwapAll");
            this.btnSwapAll.Name = "btnSwapAll";
            this.btnSwapAll.UseVisualStyleBackColor = true;
            this.btnSwapAll.Click += new System.EventHandler(this.btnSwapAll_Click);
            // 
            // WmsAdvancedConfigurationDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnSwapAll);
            this.Controls.Add(this.grpLogicalClass);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpRaster);
            this.Controls.Add(this.txtFeatureServer);
            this.Controls.Add(this.label1);
            this.Name = "WmsAdvancedConfigurationDialog";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSpatialContexts)).EndInit();
            this.grpLogicalClass.ResumeLayout(false);
            this.grpLogicalClass.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFeatureServer;
        private System.Windows.Forms.GroupBox grpRaster;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstFeatureClasses;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnRemove;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdSpatialContexts;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_CS;
        private System.Windows.Forms.GroupBox grpLogicalClass;
        private System.Windows.Forms.TextBox txtClassDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkSwap;
        private System.Windows.Forms.Button btnSwapAll;
    }
}