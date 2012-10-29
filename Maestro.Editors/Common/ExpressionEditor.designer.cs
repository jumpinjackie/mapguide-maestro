namespace Maestro.Editors.Common
{
    partial class ExpressionEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpressionEditor));
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnProperties = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnFunctions = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnFilter = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCondition = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSpatial = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ColumnValue = new System.Windows.Forms.ToolStripComboBox();
            this.LookupValues = new System.Windows.Forms.ToolStripButton();
            this.ColumnName = new System.Windows.Forms.ToolStripComboBox();
            this._autoCompleteTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ExpressionText = new ICSharpCode.TextEditor.TextEditorControl();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProperties,
            this.btnFunctions,
            this.btnFilter,
            this.toolStripSeparator1,
            this.ColumnValue,
            this.LookupValues,
            this.ColumnName});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnProperties
            // 
            this.btnProperties.Image = global::Maestro.Editors.Properties.Resources.property;
            resources.ApplyResources(this.btnProperties, "btnProperties");
            this.btnProperties.Name = "btnProperties";
            // 
            // btnFunctions
            // 
            this.btnFunctions.Image = global::Maestro.Editors.Properties.Resources.function;
            resources.ApplyResources(this.btnFunctions, "btnFunctions");
            this.btnFunctions.Name = "btnFunctions";
            // 
            // btnFilter
            // 
            this.btnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCondition,
            this.btnSpatial,
            this.btnDistance});
            this.btnFilter.Image = global::Maestro.Editors.Properties.Resources.funnel;
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            // 
            // btnCondition
            // 
            this.btnCondition.Name = "btnCondition";
            resources.ApplyResources(this.btnCondition, "btnCondition");
            // 
            // btnSpatial
            // 
            this.btnSpatial.Image = global::Maestro.Editors.Properties.Resources.grid;
            this.btnSpatial.Name = "btnSpatial";
            resources.ApplyResources(this.btnSpatial, "btnSpatial");
            // 
            // btnDistance
            // 
            this.btnDistance.Image = global::Maestro.Editors.Properties.Resources.ruler;
            this.btnDistance.Name = "btnDistance";
            resources.ApplyResources(this.btnDistance, "btnDistance");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ColumnValue
            // 
            this.ColumnValue.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ColumnValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnValue.DropDownWidth = 180;
            resources.ApplyResources(this.ColumnValue, "ColumnValue");
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.SelectedIndexChanged += new System.EventHandler(this.ColumnValue_SelectedIndexChanged);
            // 
            // LookupValues
            // 
            this.LookupValues.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.LookupValues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.LookupValues, "LookupValues");
            this.LookupValues.Image = global::Maestro.Editors.Properties.Resources.table__arrow;
            this.LookupValues.Name = "LookupValues";
            this.LookupValues.Click += new System.EventHandler(this.LookupValues_Click);
            // 
            // ColumnName
            // 
            this.ColumnName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnName.DropDownWidth = 180;
            this.ColumnName.Name = "ColumnName";
            resources.ApplyResources(this.ColumnName, "ColumnName");
            this.ColumnName.SelectedIndexChanged += new System.EventHandler(this.ColumnName_SelectedIndexChanged);
            this.ColumnName.Click += new System.EventHandler(this.ColumnName_Click);
            // 
            // ExpressionText
            // 
            resources.ApplyResources(this.ExpressionText, "ExpressionText");
            this.ExpressionText.IsReadOnly = false;
            this.ExpressionText.Name = "ExpressionText";
            this.ExpressionText.ShowLineNumbers = false;
            this.ExpressionText.ShowVRuler = false;
            // 
            // ExpressionEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.ExpressionText);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "ExpressionEditor";
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnFilter;
        private System.Windows.Forms.ToolStripMenuItem btnCondition;
        private System.Windows.Forms.ToolStripMenuItem btnSpatial;
        private System.Windows.Forms.ToolStripMenuItem btnDistance;
        private System.Windows.Forms.ToolTip _autoCompleteTooltip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox ColumnName;
        private System.Windows.Forms.ToolStripButton LookupValues;
        private System.Windows.Forms.ToolStripComboBox ColumnValue;
        private System.Windows.Forms.ToolStripDropDownButton btnProperties;
        private System.Windows.Forms.ToolStripDropDownButton btnFunctions;
        private ICSharpCode.TextEditor.TextEditorControl ExpressionText;
    }
}