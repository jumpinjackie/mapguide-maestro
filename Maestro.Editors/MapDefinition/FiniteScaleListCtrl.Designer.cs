namespace Maestro.Editors.MapDefinition
{
    partial class FiniteScaleListCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiniteScaleListCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenerateScales = new System.Windows.Forms.Button();
            this.numScales = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbRounding = new System.Windows.Forms.ComboBox();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numMaxScale = new System.Windows.Forms.NumericUpDown();
            this.numMinScale = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstDisplayScales = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEditScalesManually = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveScale = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCmsScaleList = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinScale)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnGenerateScales);
            this.groupBox1.Controls.Add(this.numScales);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbRounding);
            this.groupBox1.Controls.Add(this.cmbMethod);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numMaxScale);
            this.groupBox1.Controls.Add(this.numMinScale);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnGenerateScales
            // 
            resources.ApplyResources(this.btnGenerateScales, "btnGenerateScales");
            this.btnGenerateScales.Name = "btnGenerateScales";
            this.btnGenerateScales.UseVisualStyleBackColor = true;
            this.btnGenerateScales.Click += new System.EventHandler(this.btnGenerateScales_Click);
            // 
            // numScales
            // 
            resources.ApplyResources(this.numScales, "numScales");
            this.numScales.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numScales.Name = "numScales";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbRounding
            // 
            resources.ApplyResources(this.cmbRounding, "cmbRounding");
            this.cmbRounding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRounding.FormattingEnabled = true;
            this.cmbRounding.Name = "cmbRounding";
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.Name = "cmbMethod";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numMaxScale
            // 
            resources.ApplyResources(this.numMaxScale, "numMaxScale");
            this.numMaxScale.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numMaxScale.Name = "numMaxScale";
            // 
            // numMinScale
            // 
            resources.ApplyResources(this.numMinScale, "numMinScale");
            this.numMinScale.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numMinScale.Name = "numMinScale";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.lstDisplayScales);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lstDisplayScales
            // 
            resources.ApplyResources(this.lstDisplayScales, "lstDisplayScales");
            this.lstDisplayScales.FormattingEnabled = true;
            this.lstDisplayScales.Name = "lstDisplayScales";
            this.lstDisplayScales.SelectedIndexChanged += new System.EventHandler(this.lstDisplayScales_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEditScalesManually,
            this.btnRemoveScale,
            this.toolStripSeparator1,
            this.btnCmsScaleList});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnEditScalesManually
            // 
            this.btnEditScalesManually.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditScalesManually.Image = global::Maestro.Editors.Properties.Resources.document__pencil;
            resources.ApplyResources(this.btnEditScalesManually, "btnEditScalesManually");
            this.btnEditScalesManually.Name = "btnEditScalesManually";
            this.btnEditScalesManually.Click += new System.EventHandler(this.btnEditScalesManually_Click);
            // 
            // btnRemoveScale
            // 
            this.btnRemoveScale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveScale, "btnRemoveScale");
            this.btnRemoveScale.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnRemoveScale.Name = "btnRemoveScale";
            this.btnRemoveScale.Click += new System.EventHandler(this.btnRemoveScale_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnCmsScaleList
            // 
            this.btnCmsScaleList.Image = global::Maestro.Editors.Properties.Resources.layers_stack;
            resources.ApplyResources(this.btnCmsScaleList, "btnCmsScaleList");
            this.btnCmsScaleList.Name = "btnCmsScaleList";
            this.btnCmsScaleList.Click += new System.EventHandler(this.btnCmsScaleList_Click);
            // 
            // FiniteScaleListCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiniteScaleListCtrl";
            resources.ApplyResources(this, "$this");
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinScale)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbRounding;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numMaxScale;
        private System.Windows.Forms.NumericUpDown numMinScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerateScales;
        private System.Windows.Forms.NumericUpDown numScales;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstDisplayScales;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnEditScalesManually;
        private System.Windows.Forms.ToolStripButton btnRemoveScale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnCmsScaleList;
    }
}
