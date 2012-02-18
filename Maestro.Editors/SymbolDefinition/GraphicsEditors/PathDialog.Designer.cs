namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    partial class PathDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.txtGeometry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TAB_GENERAL = new System.Windows.Forms.TabPage();
            this.TAB_ADVANCED = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.symScaleX = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symScaleY = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symFillColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineWeight = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineWeightScalable = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineCap = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineJoin = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineMiterLimit = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.tabControl1.SuspendLayout();
            this.TAB_GENERAL.SuspendLayout();
            this.TAB_ADVANCED.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtGeometry
            // 
            resources.ApplyResources(this.txtGeometry, "txtGeometry");
            this.txtGeometry.Name = "txtGeometry";
            this.txtGeometry.TextChanged += new System.EventHandler(this.txtGeometry_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
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
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.TAB_GENERAL);
            this.tabControl1.Controls.Add(this.TAB_ADVANCED);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // TAB_GENERAL
            // 
            this.TAB_GENERAL.Controls.Add(this.symLineMiterLimit);
            this.TAB_GENERAL.Controls.Add(this.symLineJoin);
            this.TAB_GENERAL.Controls.Add(this.symLineCap);
            this.TAB_GENERAL.Controls.Add(this.symLineWeightScalable);
            this.TAB_GENERAL.Controls.Add(this.symLineWeight);
            this.TAB_GENERAL.Controls.Add(this.symLineColor);
            this.TAB_GENERAL.Controls.Add(this.symFillColor);
            this.TAB_GENERAL.Controls.Add(this.label2);
            this.TAB_GENERAL.Controls.Add(this.label3);
            this.TAB_GENERAL.Controls.Add(this.label4);
            this.TAB_GENERAL.Controls.Add(this.label5);
            this.TAB_GENERAL.Controls.Add(this.label6);
            this.TAB_GENERAL.Controls.Add(this.label7);
            this.TAB_GENERAL.Controls.Add(this.label8);
            resources.ApplyResources(this.TAB_GENERAL, "TAB_GENERAL");
            this.TAB_GENERAL.Name = "TAB_GENERAL";
            this.TAB_GENERAL.UseVisualStyleBackColor = true;
            // 
            // TAB_ADVANCED
            // 
            this.TAB_ADVANCED.Controls.Add(this.label13);
            this.TAB_ADVANCED.Controls.Add(this.label14);
            this.TAB_ADVANCED.Controls.Add(this.symScaleX);
            this.TAB_ADVANCED.Controls.Add(this.symScaleY);
            resources.ApplyResources(this.TAB_ADVANCED, "TAB_ADVANCED");
            this.TAB_ADVANCED.Name = "TAB_ADVANCED";
            this.TAB_ADVANCED.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // symScaleX
            // 
            resources.ApplyResources(this.symScaleX, "symScaleX");
            this.symScaleX.Name = "symScaleX";
            this.symScaleX.SupportedEnhancedDataTypes = null;
            // 
            // symScaleY
            // 
            resources.ApplyResources(this.symScaleY, "symScaleY");
            this.symScaleY.Name = "symScaleY";
            this.symScaleY.SupportedEnhancedDataTypes = null;
            // 
            // symFillColor
            // 
            resources.ApplyResources(this.symFillColor, "symFillColor");
            this.symFillColor.Name = "symFillColor";
            this.symFillColor.SupportedEnhancedDataTypes = null;
            // 
            // symLineColor
            // 
            resources.ApplyResources(this.symLineColor, "symLineColor");
            this.symLineColor.Name = "symLineColor";
            this.symLineColor.SupportedEnhancedDataTypes = null;
            // 
            // symLineWeight
            // 
            resources.ApplyResources(this.symLineWeight, "symLineWeight");
            this.symLineWeight.Name = "symLineWeight";
            this.symLineWeight.SupportedEnhancedDataTypes = null;
            // 
            // symLineWeightScalable
            // 
            resources.ApplyResources(this.symLineWeightScalable, "symLineWeightScalable");
            this.symLineWeightScalable.Name = "symLineWeightScalable";
            this.symLineWeightScalable.SupportedEnhancedDataTypes = null;
            // 
            // symLineCap
            // 
            resources.ApplyResources(this.symLineCap, "symLineCap");
            this.symLineCap.Name = "symLineCap";
            this.symLineCap.SupportedEnhancedDataTypes = null;
            // 
            // symLineJoin
            // 
            resources.ApplyResources(this.symLineJoin, "symLineJoin");
            this.symLineJoin.Name = "symLineJoin";
            this.symLineJoin.SupportedEnhancedDataTypes = null;
            // 
            // symLineMiterLimit
            // 
            resources.ApplyResources(this.symLineMiterLimit, "symLineMiterLimit");
            this.symLineMiterLimit.Name = "symLineMiterLimit";
            this.symLineMiterLimit.SupportedEnhancedDataTypes = null;
            // 
            // PathDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtGeometry);
            this.Controls.Add(this.label1);
            this.Name = "PathDialog";
            this.tabControl1.ResumeLayout(false);
            this.TAB_GENERAL.ResumeLayout(false);
            this.TAB_GENERAL.PerformLayout();
            this.TAB_ADVANCED.ResumeLayout(false);
            this.TAB_ADVANCED.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGeometry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TAB_GENERAL;
        private System.Windows.Forms.TabPage TAB_ADVANCED;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private SymbolField symScaleX;
        private SymbolField symScaleY;
        private SymbolField symLineMiterLimit;
        private SymbolField symLineJoin;
        private SymbolField symLineCap;
        private SymbolField symLineWeightScalable;
        private SymbolField symLineWeight;
        private SymbolField symLineColor;
        private SymbolField symFillColor;
    }
}