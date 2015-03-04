namespace Maestro.AddIn.Rest.UI.Methods
{
    partial class GetMethodCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetMethodCtrl));
            this.chkMaxCount = new System.Windows.Forms.CheckBox();
            this.chkPageSize = new System.Windows.Forms.CheckBox();
            this.numPageSize = new System.Windows.Forms.NumericUpDown();
            this.numMaxCount = new System.Windows.Forms.NumericUpDown();
            this.chkTransformTo = new System.Windows.Forms.CheckBox();
            this.txtTransformTo = new System.Windows.Forms.TextBox();
            this.btnBrowseCs = new System.Windows.Forms.Button();
            this.chkComputedProperties = new System.Windows.Forms.CheckBox();
            this.grdComputedProperties = new System.Windows.Forms.DataGridView();
            this.lstProperties = new System.Windows.Forms.ListBox();
            this.chkProperties = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEditExpression = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numPageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdComputedProperties)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkMaxCount
            // 
            resources.ApplyResources(this.chkMaxCount, "chkMaxCount");
            this.chkMaxCount.Name = "chkMaxCount";
            this.chkMaxCount.UseVisualStyleBackColor = true;
            // 
            // chkPageSize
            // 
            resources.ApplyResources(this.chkPageSize, "chkPageSize");
            this.chkPageSize.Name = "chkPageSize";
            this.chkPageSize.UseVisualStyleBackColor = true;
            // 
            // numPageSize
            // 
            resources.ApplyResources(this.numPageSize, "numPageSize");
            this.numPageSize.Name = "numPageSize";
            // 
            // numMaxCount
            // 
            resources.ApplyResources(this.numMaxCount, "numMaxCount");
            this.numMaxCount.Name = "numMaxCount";
            // 
            // chkTransformTo
            // 
            resources.ApplyResources(this.chkTransformTo, "chkTransformTo");
            this.chkTransformTo.Name = "chkTransformTo";
            this.chkTransformTo.UseVisualStyleBackColor = true;
            // 
            // txtTransformTo
            // 
            resources.ApplyResources(this.txtTransformTo, "txtTransformTo");
            this.txtTransformTo.Name = "txtTransformTo";
            this.txtTransformTo.ReadOnly = true;
            // 
            // btnBrowseCs
            // 
            resources.ApplyResources(this.btnBrowseCs, "btnBrowseCs");
            this.btnBrowseCs.Name = "btnBrowseCs";
            this.btnBrowseCs.UseVisualStyleBackColor = true;
            this.btnBrowseCs.Click += new System.EventHandler(this.btnBrowseCs_Click);
            // 
            // chkComputedProperties
            // 
            resources.ApplyResources(this.chkComputedProperties, "chkComputedProperties");
            this.chkComputedProperties.Name = "chkComputedProperties";
            this.chkComputedProperties.UseVisualStyleBackColor = true;
            // 
            // grdComputedProperties
            // 
            this.grdComputedProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.grdComputedProperties, "grdComputedProperties");
            this.grdComputedProperties.Name = "grdComputedProperties";
            this.grdComputedProperties.SelectionChanged += new System.EventHandler(this.grdComputedProperties_SelectionChanged);
            // 
            // lstProperties
            // 
            this.lstProperties.FormattingEnabled = true;
            resources.ApplyResources(this.lstProperties, "lstProperties");
            this.lstProperties.Name = "lstProperties";
            this.lstProperties.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            // 
            // chkProperties
            // 
            resources.ApplyResources(this.chkProperties, "chkProperties");
            this.chkProperties.Name = "chkProperties";
            this.chkProperties.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdComputedProperties);
            this.panel1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEditExpression});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnEditExpression
            // 
            resources.ApplyResources(this.btnEditExpression, "btnEditExpression");
            this.btnEditExpression.Name = "btnEditExpression";
            this.btnEditExpression.Click += new System.EventHandler(this.btnEditExpression_Click);
            // 
            // GetMethodCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstProperties);
            this.Controls.Add(this.chkProperties);
            this.Controls.Add(this.chkComputedProperties);
            this.Controls.Add(this.btnBrowseCs);
            this.Controls.Add(this.txtTransformTo);
            this.Controls.Add(this.chkTransformTo);
            this.Controls.Add(this.numMaxCount);
            this.Controls.Add(this.numPageSize);
            this.Controls.Add(this.chkPageSize);
            this.Controls.Add(this.chkMaxCount);
            this.Name = "GetMethodCtrl";
            ((System.ComponentModel.ISupportInitialize)(this.numPageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdComputedProperties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkMaxCount;
        private System.Windows.Forms.CheckBox chkPageSize;
        private System.Windows.Forms.NumericUpDown numPageSize;
        private System.Windows.Forms.NumericUpDown numMaxCount;
        private System.Windows.Forms.CheckBox chkTransformTo;
        private System.Windows.Forms.TextBox txtTransformTo;
        private System.Windows.Forms.Button btnBrowseCs;
        private System.Windows.Forms.CheckBox chkComputedProperties;
        private System.Windows.Forms.DataGridView grdComputedProperties;
        private System.Windows.Forms.ListBox lstProperties;
        private System.Windows.Forms.CheckBox chkProperties;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnEditExpression;
    }
}
