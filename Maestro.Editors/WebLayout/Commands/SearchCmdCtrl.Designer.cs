namespace Maestro.Editors.WebLayout.Commands
{
    partial class SearchCmdCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCmdCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFrame = new System.Windows.Forms.TextBox();
            this.cmbTargetFrame = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnBrowseLayer = new System.Windows.Forms.Button();
            this.txtLayer = new System.Windows.Forms.TextBox();
            this.grdOutputColumns = new System.Windows.Forms.DataGridView();
            this.COL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_PROPERTY = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOutputColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtFrame);
            this.panel1.Controls.Add(this.cmbTargetFrame);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.btnBrowseLayer);
            this.panel1.Controls.Add(this.txtLayer);
            this.panel1.Controls.Add(this.grdOutputColumns);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.numLimit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPrompt);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // txtFrame
            // 
            resources.ApplyResources(this.txtFrame, "txtFrame");
            this.txtFrame.Name = "txtFrame";
            // 
            // cmbTargetFrame
            // 
            resources.ApplyResources(this.cmbTargetFrame, "cmbTargetFrame");
            this.cmbTargetFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetFrame.FormattingEnabled = true;
            this.cmbTargetFrame.Name = "cmbTargetFrame";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnBrowseLayer
            // 
            resources.ApplyResources(this.btnBrowseLayer, "btnBrowseLayer");
            this.btnBrowseLayer.Name = "btnBrowseLayer";
            this.btnBrowseLayer.UseVisualStyleBackColor = true;
            this.btnBrowseLayer.Click += new System.EventHandler(this.btnBrowseLayer_Click);
            // 
            // txtLayer
            // 
            resources.ApplyResources(this.txtLayer, "txtLayer");
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.ReadOnly = true;
            this.txtLayer.TextChanged += new System.EventHandler(this.txtLayer_TextChanged);
            // 
            // grdOutputColumns
            // 
            resources.ApplyResources(this.grdOutputColumns, "grdOutputColumns");
            this.grdOutputColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOutputColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_NAME,
            this.COL_PROPERTY});
            this.grdOutputColumns.Name = "grdOutputColumns";
            // 
            // COL_NAME
            // 
            this.COL_NAME.DataPropertyName = "Name";
            resources.ApplyResources(this.COL_NAME, "COL_NAME");
            this.COL_NAME.Name = "COL_NAME";
            this.COL_NAME.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.COL_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // COL_PROPERTY
            // 
            this.COL_PROPERTY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_PROPERTY.DataPropertyName = "Property";
            resources.ApplyResources(this.COL_PROPERTY, "COL_PROPERTY");
            this.COL_PROPERTY.Name = "COL_PROPERTY";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // numLimit
            // 
            resources.ApplyResources(this.numLimit, "numLimit");
            this.numLimit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtPrompt
            // 
            resources.ApplyResources(this.txtPrompt, "txtPrompt");
            this.txtPrompt.Name = "txtPrompt";
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
            // SearchCmdCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SearchCmdCtrl";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOutputColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdOutputColumns;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_NAME;
        private System.Windows.Forms.DataGridViewComboBoxColumn COL_PROPERTY;
        private System.Windows.Forms.Button btnBrowseLayer;
        private System.Windows.Forms.TextBox txtLayer;
        private System.Windows.Forms.TextBox txtFrame;
        private System.Windows.Forms.ComboBox cmbTargetFrame;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}
