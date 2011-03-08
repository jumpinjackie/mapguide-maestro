namespace Maestro.Editors.FeatureSource.Extensions
{
    partial class JoinSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoinSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtJoinName = new System.Windows.Forms.TextBox();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdAssociation = new System.Windows.Forms.RadioButton();
            this.rdInner = new System.Windows.Forms.RadioButton();
            this.rdRightOuter = new System.Windows.Forms.RadioButton();
            this.rdLeftOuter = new System.Windows.Forms.RadioButton();
            this.chkForceOneToOne = new System.Windows.Forms.CheckBox();
            this.grdJoinKeys = new System.Windows.Forms.DataGridView();
            this.COL_PRIMARY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_SECONDARY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddKey = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteKey = new System.Windows.Forms.ToolStripButton();
            this.btnBrowseSecondaryClass = new System.Windows.Forms.Button();
            this.txtSecondaryClass = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdJoinKeys)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtJoinName
            // 
            resources.ApplyResources(this.txtJoinName, "txtJoinName");
            this.txtJoinName.Name = "txtJoinName";
            // 
            // txtFeatureSource
            // 
            resources.ApplyResources(this.txtFeatureSource, "txtFeatureSource");
            this.txtFeatureSource.Name = "txtFeatureSource";
            this.txtFeatureSource.ReadOnly = true;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
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
            // rdAssociation
            // 
            this.rdAssociation.Image = global::Maestro.Editors.Properties.Resources.databases_relation;
            resources.ApplyResources(this.rdAssociation, "rdAssociation");
            this.rdAssociation.Name = "rdAssociation";
            this.rdAssociation.TabStop = true;
            this.rdAssociation.UseVisualStyleBackColor = true;
            this.rdAssociation.CheckedChanged += new System.EventHandler(this.rdJoinTypeChanged);
            // 
            // rdInner
            // 
            this.rdInner.Image = global::Maestro.Editors.Properties.Resources.sql_join_inner;
            resources.ApplyResources(this.rdInner, "rdInner");
            this.rdInner.Name = "rdInner";
            this.rdInner.TabStop = true;
            this.rdInner.UseVisualStyleBackColor = true;
            this.rdInner.CheckedChanged += new System.EventHandler(this.rdJoinTypeChanged);
            // 
            // rdRightOuter
            // 
            this.rdRightOuter.Image = global::Maestro.Editors.Properties.Resources.sql_join_right;
            resources.ApplyResources(this.rdRightOuter, "rdRightOuter");
            this.rdRightOuter.Name = "rdRightOuter";
            this.rdRightOuter.TabStop = true;
            this.rdRightOuter.UseVisualStyleBackColor = true;
            this.rdRightOuter.CheckedChanged += new System.EventHandler(this.rdJoinTypeChanged);
            // 
            // rdLeftOuter
            // 
            this.rdLeftOuter.Checked = true;
            this.rdLeftOuter.Image = global::Maestro.Editors.Properties.Resources.sql_join_left;
            resources.ApplyResources(this.rdLeftOuter, "rdLeftOuter");
            this.rdLeftOuter.Name = "rdLeftOuter";
            this.rdLeftOuter.TabStop = true;
            this.rdLeftOuter.UseVisualStyleBackColor = true;
            this.rdLeftOuter.CheckedChanged += new System.EventHandler(this.rdJoinTypeChanged);
            // 
            // chkForceOneToOne
            // 
            resources.ApplyResources(this.chkForceOneToOne, "chkForceOneToOne");
            this.chkForceOneToOne.Name = "chkForceOneToOne";
            this.chkForceOneToOne.UseVisualStyleBackColor = true;
            // 
            // grdJoinKeys
            // 
            this.grdJoinKeys.AllowUserToAddRows = false;
            this.grdJoinKeys.AllowUserToDeleteRows = false;
            this.grdJoinKeys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdJoinKeys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_PRIMARY,
            this.COL_SECONDARY});
            resources.ApplyResources(this.grdJoinKeys, "grdJoinKeys");
            this.grdJoinKeys.Name = "grdJoinKeys";
            this.grdJoinKeys.ReadOnly = true;
            this.grdJoinKeys.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdJoinKeys_CellClick);
            this.grdJoinKeys.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdJoinKeys_CellClick);
            // 
            // COL_PRIMARY
            // 
            this.COL_PRIMARY.DataPropertyName = "FeatureClassProperty";
            resources.ApplyResources(this.COL_PRIMARY, "COL_PRIMARY");
            this.COL_PRIMARY.Name = "COL_PRIMARY";
            this.COL_PRIMARY.ReadOnly = true;
            // 
            // COL_SECONDARY
            // 
            this.COL_SECONDARY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_SECONDARY.DataPropertyName = "AttributeClassProperty";
            resources.ApplyResources(this.COL_SECONDARY, "COL_SECONDARY");
            this.COL_SECONDARY.Name = "COL_SECONDARY";
            this.COL_SECONDARY.ReadOnly = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.grdJoinKeys);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddKey,
            this.btnDeleteKey});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddKey
            // 
            this.btnAddKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddKey.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAddKey, "btnAddKey");
            this.btnAddKey.Name = "btnAddKey";
            this.btnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
            // 
            // btnDeleteKey
            // 
            this.btnDeleteKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDeleteKey, "btnDeleteKey");
            this.btnDeleteKey.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDeleteKey.Name = "btnDeleteKey";
            this.btnDeleteKey.Click += new System.EventHandler(this.btnDeleteKey_Click);
            // 
            // btnBrowseSecondaryClass
            // 
            resources.ApplyResources(this.btnBrowseSecondaryClass, "btnBrowseSecondaryClass");
            this.btnBrowseSecondaryClass.Name = "btnBrowseSecondaryClass";
            this.btnBrowseSecondaryClass.UseVisualStyleBackColor = true;
            this.btnBrowseSecondaryClass.Click += new System.EventHandler(this.btnBrowseSecondaryClass_Click);
            // 
            // txtSecondaryClass
            // 
            resources.ApplyResources(this.txtSecondaryClass, "txtSecondaryClass");
            this.txtSecondaryClass.Name = "txtSecondaryClass";
            this.txtSecondaryClass.ReadOnly = true;
            // 
            // JoinSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnBrowseSecondaryClass);
            this.Controls.Add(this.txtSecondaryClass);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkForceOneToOne);
            this.Controls.Add(this.rdAssociation);
            this.Controls.Add(this.rdInner);
            this.Controls.Add(this.rdRightOuter);
            this.Controls.Add(this.rdLeftOuter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFeatureSource);
            this.Controls.Add(this.txtJoinName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "JoinSettings";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.grdJoinKeys)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJoinName;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdLeftOuter;
        private System.Windows.Forms.RadioButton rdRightOuter;
        private System.Windows.Forms.RadioButton rdInner;
        private System.Windows.Forms.RadioButton rdAssociation;
        private System.Windows.Forms.CheckBox chkForceOneToOne;
        private System.Windows.Forms.DataGridView grdJoinKeys;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_PRIMARY;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_SECONDARY;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddKey;
        private System.Windows.Forms.ToolStripButton btnDeleteKey;
        private System.Windows.Forms.Button btnBrowseSecondaryClass;
        private System.Windows.Forms.TextBox txtSecondaryClass;
    }
}
