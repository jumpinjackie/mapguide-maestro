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
            label1 = new System.Windows.Forms.Label();
            txtFeatureServer = new System.Windows.Forms.TextBox();
            grpRaster = new System.Windows.Forms.GroupBox();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lstFeatureClasses = new System.Windows.Forms.ListBox();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnRemove = new System.Windows.Forms.ToolStripButton();
            btnReset = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            grdSpatialContexts = new System.Windows.Forms.DataGridView();
            COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            COL_CS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            grpLogicalClass = new System.Windows.Forms.GroupBox();
            lnkSwap = new System.Windows.Forms.LinkLabel();
            txtClassDescription = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtClassName = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            btnSwapAll = new System.Windows.Forms.Button();
            groupBox2.SuspendLayout();
            toolStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdSpatialContexts).BeginInit();
            grpLogicalClass.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // txtFeatureServer
            // 
            resources.ApplyResources(txtFeatureServer, "txtFeatureServer");
            txtFeatureServer.Name = "txtFeatureServer";
            txtFeatureServer.ReadOnly = true;
            // 
            // grpRaster
            // 
            resources.ApplyResources(grpRaster, "grpRaster");
            grpRaster.Name = "grpRaster";
            grpRaster.TabStop = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(btnSave, "btnSave");
            btnSave.Name = "btnSave";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            resources.ApplyResources(btnCancel, "btnCancel");
            btnCancel.Name = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // groupBox2
            // 
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Controls.Add(lstFeatureClasses);
            groupBox2.Controls.Add(toolStrip1);
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // lstFeatureClasses
            // 
            lstFeatureClasses.DisplayMember = "FeatureClass";
            resources.ApplyResources(lstFeatureClasses, "lstFeatureClasses");
            lstFeatureClasses.FormattingEnabled = true;
            lstFeatureClasses.Name = "lstFeatureClasses";
            lstFeatureClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            lstFeatureClasses.SelectedIndexChanged += lstFeatureClasses_SelectedIndexChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnRemove });
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.Name = "toolStrip1";
            // 
            // btnRemove
            // 
            resources.ApplyResources(btnRemove, "btnRemove");
            btnRemove.Image = Properties.Resources.cross_script;
            btnRemove.Name = "btnRemove";
            btnRemove.Click += btnRemove_Click;
            // 
            // btnReset
            // 
            resources.ApplyResources(btnReset, "btnReset");
            btnReset.Name = "btnReset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(grdSpatialContexts);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // grdSpatialContexts
            // 
            grdSpatialContexts.AllowUserToAddRows = false;
            grdSpatialContexts.AllowUserToDeleteRows = false;
            grdSpatialContexts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdSpatialContexts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { COL_NAME, COL_CS });
            resources.ApplyResources(grdSpatialContexts, "grdSpatialContexts");
            grdSpatialContexts.Name = "grdSpatialContexts";
            grdSpatialContexts.ReadOnly = true;
            grdSpatialContexts.RowHeadersVisible = false;
            grdSpatialContexts.CellClick += grdSpatialContexts_CellContentClick;
            // 
            // COL_NAME
            // 
            COL_NAME.DataPropertyName = "Name";
            resources.ApplyResources(COL_NAME, "COL_NAME");
            COL_NAME.Name = "COL_NAME";
            COL_NAME.ReadOnly = true;
            // 
            // COL_CS
            // 
            COL_CS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            COL_CS.DataPropertyName = "CoordinateSystemWkt";
            resources.ApplyResources(COL_CS, "COL_CS");
            COL_CS.Name = "COL_CS";
            COL_CS.ReadOnly = true;
            // 
            // grpLogicalClass
            // 
            resources.ApplyResources(grpLogicalClass, "grpLogicalClass");
            grpLogicalClass.Controls.Add(lnkSwap);
            grpLogicalClass.Controls.Add(txtClassDescription);
            grpLogicalClass.Controls.Add(label3);
            grpLogicalClass.Controls.Add(txtClassName);
            grpLogicalClass.Controls.Add(label2);
            grpLogicalClass.Name = "grpLogicalClass";
            grpLogicalClass.TabStop = false;
            // 
            // lnkSwap
            // 
            resources.ApplyResources(lnkSwap, "lnkSwap");
            lnkSwap.Name = "lnkSwap";
            lnkSwap.TabStop = true;
            lnkSwap.LinkClicked += lnkSwap_LinkClicked;
            // 
            // txtClassDescription
            // 
            resources.ApplyResources(txtClassDescription, "txtClassDescription");
            txtClassDescription.Name = "txtClassDescription";
            txtClassDescription.TextChanged += txtClassDescription_TextChanged;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // txtClassName
            // 
            resources.ApplyResources(txtClassName, "txtClassName");
            txtClassName.Name = "txtClassName";
            txtClassName.TextChanged += txtClassName_TextChanged;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // btnSwapAll
            // 
            resources.ApplyResources(btnSwapAll, "btnSwapAll");
            btnSwapAll.Name = "btnSwapAll";
            btnSwapAll.UseVisualStyleBackColor = true;
            btnSwapAll.Click += btnSwapAll_Click;
            // 
            // WmsAdvancedConfigurationDialog
            // 
            AcceptButton = btnSave;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            ControlBox = false;
            Controls.Add(btnSwapAll);
            Controls.Add(grpLogicalClass);
            Controls.Add(groupBox1);
            Controls.Add(btnReset);
            Controls.Add(groupBox2);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpRaster);
            Controls.Add(txtFeatureServer);
            Controls.Add(label1);
            Name = "WmsAdvancedConfigurationDialog";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdSpatialContexts).EndInit();
            grpLogicalClass.ResumeLayout(false);
            grpLogicalClass.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFeatureServer;
        private System.Windows.Forms.GroupBox grpRaster;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstFeatureClasses;
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRemove;
    }
}