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
            this.symFillColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineColor = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineWeight = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineWeightScalable = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineCap = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineJoin = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symLineMiterLimit = new Maestro.Editors.SymbolDefinition.SymbolField();
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
            // symFillColor
            // 
            resources.ApplyResources(this.symFillColor, "symFillColor");
            this.symFillColor.Name = "symFillColor";
            this.symFillColor.SupportedEnhancedDataTypes = null;
            this.symFillColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symFillColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineColor
            // 
            resources.ApplyResources(this.symLineColor, "symLineColor");
            this.symLineColor.Name = "symLineColor";
            this.symLineColor.SupportedEnhancedDataTypes = null;
            this.symLineColor.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineColor.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineWeight
            // 
            resources.ApplyResources(this.symLineWeight, "symLineWeight");
            this.symLineWeight.Name = "symLineWeight";
            this.symLineWeight.SupportedEnhancedDataTypes = null;
            this.symLineWeight.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineWeight.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineWeightScalable
            // 
            resources.ApplyResources(this.symLineWeightScalable, "symLineWeightScalable");
            this.symLineWeightScalable.Name = "symLineWeightScalable";
            this.symLineWeightScalable.SupportedEnhancedDataTypes = null;
            this.symLineWeightScalable.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineWeightScalable.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineCap
            // 
            resources.ApplyResources(this.symLineCap, "symLineCap");
            this.symLineCap.Name = "symLineCap";
            this.symLineCap.SupportedEnhancedDataTypes = null;
            this.symLineCap.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineCap.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineJoin
            // 
            resources.ApplyResources(this.symLineJoin, "symLineJoin");
            this.symLineJoin.Name = "symLineJoin";
            this.symLineJoin.SupportedEnhancedDataTypes = null;
            this.symLineJoin.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineJoin.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // symLineMiterLimit
            // 
            resources.ApplyResources(this.symLineMiterLimit, "symLineMiterLimit");
            this.symLineMiterLimit.Name = "symLineMiterLimit";
            this.symLineMiterLimit.SupportedEnhancedDataTypes = null;
            this.symLineMiterLimit.ContentChanged += new System.EventHandler(this.OnContentChanged);
            this.symLineMiterLimit.RequestBrowse += new Maestro.Editors.SymbolDefinition.SymbolField.BrowseEventHandler(this.OnRequestBrowse);
            // 
            // PathDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.symLineMiterLimit);
            this.Controls.Add(this.symLineJoin);
            this.Controls.Add(this.symLineCap);
            this.Controls.Add(this.symLineWeightScalable);
            this.Controls.Add(this.symLineWeight);
            this.Controls.Add(this.symLineColor);
            this.Controls.Add(this.symFillColor);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGeometry);
            this.Controls.Add(this.label1);
            this.Name = "PathDialog";
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
        private SymbolField symFillColor;
        private SymbolField symLineColor;
        private SymbolField symLineWeight;
        private SymbolField symLineWeightScalable;
        private SymbolField symLineCap;
        private SymbolField symLineJoin;
        private SymbolField symLineMiterLimit;
    }
}