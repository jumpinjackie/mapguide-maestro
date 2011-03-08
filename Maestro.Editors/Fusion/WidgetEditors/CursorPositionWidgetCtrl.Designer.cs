namespace Maestro.Editors.Fusion.WidgetEditors
{
    partial class CursorPositionWidgetCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CursorPositionWidgetCtrl));
            this.baseEditor = new Maestro.Editors.Fusion.WidgetEditors.WidgetEditorBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUnits = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numPrecision = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecision)).BeginInit();
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
            this.groupBox1.Controls.Add(this.txtUnits);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numPrecision);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTemplate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtUnits
            // 
            resources.ApplyResources(this.txtUnits, "txtUnits");
            this.txtUnits.Name = "txtUnits";
            this.txtUnits.TextChanged += new System.EventHandler(this.txtUnits_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numPrecision
            // 
            resources.ApplyResources(this.numPrecision, "numPrecision");
            this.numPrecision.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numPrecision.Name = "numPrecision";
            this.numPrecision.ValueChanged += new System.EventHandler(this.numPrecision_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtTemplate
            // 
            resources.ApplyResources(this.txtTemplate, "txtTemplate");
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.TextChanged += new System.EventHandler(this.txtTemplate_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CursorPositionWidgetCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.baseEditor);
            this.Name = "CursorPositionWidgetCtrl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WidgetEditorBase baseEditor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUnits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPrecision;
    }
}
