namespace OSGeo.MapGuide.Maestro
{
    partial class FormExpression
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHint = new System.Windows.Forms.Label();
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
            this.ExpressionText = new System.Windows.Forms.TextBox();
            this._autoCompleteTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(289, 6);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(377, 6);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblHint);
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 231);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 40);
            this.panel1.TabIndex = 4;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(12, 11);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(202, 13);
            this.lblHint.TabIndex = 4;
            this.lblHint.Text = "Press Alt + Right to invoke auto-complete";
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(464, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnProperties
            // 
            this.btnProperties.Image = global::OSGeo.MapGuide.Maestro.Properties.Resources.table;
            this.btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(85, 22);
            this.btnProperties.Text = "Properties";
            // 
            // btnFunctions
            // 
            this.btnFunctions.Image = global::OSGeo.MapGuide.Maestro.Properties.Resources.sum;
            this.btnFunctions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFunctions.Name = "btnFunctions";
            this.btnFunctions.Size = new System.Drawing.Size(82, 22);
            this.btnFunctions.Text = "Functions";
            // 
            // btnFilter
            // 
            this.btnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCondition,
            this.btnSpatial,
            this.btnDistance});
            this.btnFilter.Image = global::OSGeo.MapGuide.Maestro.Properties.Resources.bricks;
            this.btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(60, 22);
            this.btnFilter.Text = "Filter";
            // 
            // btnCondition
            // 
            this.btnCondition.Name = "btnCondition";
            this.btnCondition.Size = new System.Drawing.Size(130, 22);
            this.btnCondition.Text = "Condition";
            // 
            // btnSpatial
            // 
            this.btnSpatial.Name = "btnSpatial";
            this.btnSpatial.Size = new System.Drawing.Size(130, 22);
            this.btnSpatial.Text = "Spatial";
            // 
            // btnDistance
            // 
            this.btnDistance.Name = "btnDistance";
            this.btnDistance.Size = new System.Drawing.Size(130, 22);
            this.btnDistance.Text = "Distance";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ColumnValue
            // 
            this.ColumnValue.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ColumnValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnValue.Enabled = false;
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.Size = new System.Drawing.Size(90, 25);
            this.ColumnValue.SelectedIndexChanged += new System.EventHandler(this.ColumnValue_SelectedIndexChanged);
            // 
            // LookupValues
            // 
            this.LookupValues.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.LookupValues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LookupValues.Enabled = false;
            this.LookupValues.Image = global::OSGeo.MapGuide.Maestro.Properties.Resources.bullet_go;
            this.LookupValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LookupValues.Name = "LookupValues";
            this.LookupValues.Size = new System.Drawing.Size(23, 22);
            this.LookupValues.ToolTipText = "Click to lookup values from the selected column";
            this.LookupValues.Click += new System.EventHandler(this.LookupValues_Click);
            // 
            // ColumnName
            // 
            this.ColumnName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.Size = new System.Drawing.Size(90, 25);
            this.ColumnName.ToolTipText = "Select the column to read values from";
            this.ColumnName.SelectedIndexChanged += new System.EventHandler(this.ColumnName_SelectedIndexChanged);
            this.ColumnName.Click += new System.EventHandler(this.ColumnName_Click);
            // 
            // ExpressionText
            // 
            this.ExpressionText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpressionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpressionText.HideSelection = false;
            this.ExpressionText.Location = new System.Drawing.Point(0, 25);
            this.ExpressionText.Multiline = true;
            this.ExpressionText.Name = "ExpressionText";
            this.ExpressionText.Size = new System.Drawing.Size(464, 206);
            this.ExpressionText.TabIndex = 6;
            this.ExpressionText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExpressionText_KeyDown);
            this.ExpressionText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExpressionText_KeyUp);
            // 
            // FormExpression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 271);
            this.Controls.Add(this.ExpressionText);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "FormExpression";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Expression Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripDropDownButton btnProperties;
        private System.Windows.Forms.ToolStripDropDownButton btnFunctions;
        private System.Windows.Forms.TextBox ExpressionText;
        private System.Windows.Forms.ToolStripDropDownButton btnFilter;
        private System.Windows.Forms.ToolStripMenuItem btnCondition;
        private System.Windows.Forms.ToolStripMenuItem btnSpatial;
        private System.Windows.Forms.ToolStripMenuItem btnDistance;
        private System.Windows.Forms.ToolTip _autoCompleteTooltip;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox ColumnName;
        private System.Windows.Forms.ToolStripButton LookupValues;
        private System.Windows.Forms.ToolStripComboBox ColumnValue;
    }
}