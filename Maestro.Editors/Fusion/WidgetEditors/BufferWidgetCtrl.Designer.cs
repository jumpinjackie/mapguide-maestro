namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class BufferWidgetCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BufferWidgetCtrl));
            this.baseEditor = new Maestro.Editors.Fusion.WidgetEditors.WidgetEditorBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseBufferUnits = new System.Windows.Forms.Button();
            this.txtBufferUnits = new System.Windows.Forms.TextBox();
            this.txtFillColorInput = new System.Windows.Forms.TextBox();
            this.txtBorderColorInput = new System.Windows.Forms.TextBox();
            this.txtBufferUnitsInput = new System.Windows.Forms.TextBox();
            this.txtBufferDistanceInput = new System.Windows.Forms.TextBox();
            this.txtBufferDistance = new System.Windows.Forms.TextBox();
            this.txtLayerNameInput = new System.Windows.Forms.TextBox();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.cmbBorderColor = new Maestro.Editors.Common.ColorComboBox();
            this.cmbFillColor = new Maestro.Editors.Common.ColorComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseEditor
            // 
            resources.ApplyResources(this.baseEditor, "baseEditor");
            this.baseEditor.Name = "baseEditor";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnBrowseBufferUnits);
            this.groupBox1.Controls.Add(this.txtBufferUnits);
            this.groupBox1.Controls.Add(this.txtFillColorInput);
            this.groupBox1.Controls.Add(this.txtBorderColorInput);
            this.groupBox1.Controls.Add(this.txtBufferUnitsInput);
            this.groupBox1.Controls.Add(this.txtBufferDistanceInput);
            this.groupBox1.Controls.Add(this.txtBufferDistance);
            this.groupBox1.Controls.Add(this.txtLayerNameInput);
            this.groupBox1.Controls.Add(this.txtLayerName);
            this.groupBox1.Controls.Add(this.cmbBorderColor);
            this.groupBox1.Controls.Add(this.cmbFillColor);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnBrowseBufferUnits
            // 
            resources.ApplyResources(this.btnBrowseBufferUnits, "btnBrowseBufferUnits");
            this.btnBrowseBufferUnits.Name = "btnBrowseBufferUnits";
            this.btnBrowseBufferUnits.UseVisualStyleBackColor = true;
            this.btnBrowseBufferUnits.Click += new System.EventHandler(this.btnBrowseBufferUnits_Click);
            // 
            // txtBufferUnits
            // 
            resources.ApplyResources(this.txtBufferUnits, "txtBufferUnits");
            this.txtBufferUnits.Name = "txtBufferUnits";
            this.txtBufferUnits.ReadOnly = true;
            // 
            // txtFillColorInput
            // 
            resources.ApplyResources(this.txtFillColorInput, "txtFillColorInput");
            this.txtFillColorInput.Name = "txtFillColorInput";
            // 
            // txtBorderColorInput
            // 
            resources.ApplyResources(this.txtBorderColorInput, "txtBorderColorInput");
            this.txtBorderColorInput.Name = "txtBorderColorInput";
            // 
            // txtBufferUnitsInput
            // 
            resources.ApplyResources(this.txtBufferUnitsInput, "txtBufferUnitsInput");
            this.txtBufferUnitsInput.Name = "txtBufferUnitsInput";
            // 
            // txtBufferDistanceInput
            // 
            resources.ApplyResources(this.txtBufferDistanceInput, "txtBufferDistanceInput");
            this.txtBufferDistanceInput.Name = "txtBufferDistanceInput";
            // 
            // txtBufferDistance
            // 
            resources.ApplyResources(this.txtBufferDistance, "txtBufferDistance");
            this.txtBufferDistance.Name = "txtBufferDistance";
            // 
            // txtLayerNameInput
            // 
            resources.ApplyResources(this.txtLayerNameInput, "txtLayerNameInput");
            this.txtLayerNameInput.Name = "txtLayerNameInput";
            // 
            // txtLayerName
            // 
            resources.ApplyResources(this.txtLayerName, "txtLayerName");
            this.txtLayerName.Name = "txtLayerName";
            // 
            // cmbBorderColor
            // 
            this.cmbBorderColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBorderColor, "cmbBorderColor");
            this.cmbBorderColor.Name = "cmbBorderColor";
            this.cmbBorderColor.SelectedIndexChanged += new System.EventHandler(this.cmbBorderColor_SelectedIndexChanged);
            // 
            // cmbFillColor
            // 
            this.cmbFillColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFillColor, "cmbFillColor");
            this.cmbFillColor.Name = "cmbFillColor";
            this.cmbFillColor.SelectedIndexChanged += new System.EventHandler(this.cmbFillColor_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            // BufferWidgetCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.baseEditor);
            this.Name = "BufferWidgetCtrl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WidgetEditorBase baseEditor;
        private System.Windows.Forms.GroupBox groupBox1;
        private Maestro.Editors.Common.ColorComboBox cmbFillColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBufferUnitsInput;
        private System.Windows.Forms.TextBox txtBufferDistanceInput;
        private System.Windows.Forms.TextBox txtBufferDistance;
        private System.Windows.Forms.TextBox txtLayerNameInput;
        private System.Windows.Forms.TextBox txtLayerName;
        private Maestro.Editors.Common.ColorComboBox cmbBorderColor;
        private System.Windows.Forms.TextBox txtFillColorInput;
        private System.Windows.Forms.TextBox txtBorderColorInput;
        private System.Windows.Forms.TextBox txtBufferUnits;
        private System.Windows.Forms.Button btnBrowseBufferUnits;
    }
}
