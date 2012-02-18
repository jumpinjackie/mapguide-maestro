namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    partial class ImageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDialog));
            this.rdResourceRef = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResourceId = new System.Windows.Forms.TextBox();
            this.txtResData = new System.Windows.Forms.TextBox();
            this.rdInline = new System.Windows.Forms.RadioButton();
            this.txtImageBase64 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lnkPreview = new System.Windows.Forms.LinkLabel();
            this.lnkLoadImage = new System.Windows.Forms.LinkLabel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnResData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.symSizeX = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.symPositionX = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symPositionY = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.symSizeY = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.symAngle = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label7 = new System.Windows.Forms.Label();
            this.symSizeScalable = new Maestro.Editors.SymbolDefinition.SymbolField();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdResourceRef
            // 
            resources.ApplyResources(this.rdResourceRef, "rdResourceRef");
            this.rdResourceRef.Name = "rdResourceRef";
            this.rdResourceRef.TabStop = true;
            this.rdResourceRef.UseVisualStyleBackColor = true;
            this.rdResourceRef.CheckedChanged += new System.EventHandler(this.imageType_CheckedChanged);
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
            // txtResourceId
            // 
            resources.ApplyResources(this.txtResourceId, "txtResourceId");
            this.txtResourceId.Name = "txtResourceId";
            this.txtResourceId.TextChanged += new System.EventHandler(this.txtResourceId_TextChanged);
            // 
            // txtResData
            // 
            resources.ApplyResources(this.txtResData, "txtResData");
            this.txtResData.Name = "txtResData";
            this.txtResData.TextChanged += new System.EventHandler(this.txtResData_TextChanged);
            // 
            // rdInline
            // 
            resources.ApplyResources(this.rdInline, "rdInline");
            this.rdInline.Name = "rdInline";
            this.rdInline.TabStop = true;
            this.rdInline.UseVisualStyleBackColor = true;
            this.rdInline.CheckedChanged += new System.EventHandler(this.imageType_CheckedChanged);
            // 
            // txtImageBase64
            // 
            resources.ApplyResources(this.txtImageBase64, "txtImageBase64");
            this.txtImageBase64.Name = "txtImageBase64";
            this.txtImageBase64.ReadOnly = true;
            this.txtImageBase64.TextChanged += new System.EventHandler(this.txtImageBase64_TextChanged);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lnkPreview
            // 
            resources.ApplyResources(this.lnkPreview, "lnkPreview");
            this.lnkPreview.Name = "lnkPreview";
            this.lnkPreview.TabStop = true;
            this.lnkPreview.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPreview_LinkClicked);
            // 
            // lnkLoadImage
            // 
            resources.ApplyResources(this.lnkLoadImage, "lnkLoadImage");
            this.lnkLoadImage.Name = "lnkLoadImage";
            this.lnkLoadImage.TabStop = true;
            this.lnkLoadImage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLoadImage_LinkClicked);
            // 
            // picPreview
            // 
            resources.ApplyResources(this.picPreview, "picPreview");
            this.picPreview.Name = "picPreview";
            this.picPreview.TabStop = false;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnResData
            // 
            resources.ApplyResources(this.btnResData, "btnResData");
            this.btnResData.Name = "btnResData";
            this.btnResData.UseVisualStyleBackColor = true;
            this.btnResData.Click += new System.EventHandler(this.btnResData_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.symAngle);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.symSizeScalable);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.symPositionY);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.symSizeY);
            this.groupBox1.Controls.Add(this.symPositionX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.symSizeX);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // symSizeX
            // 
            resources.ApplyResources(this.symSizeX, "symSizeX");
            this.symSizeX.Name = "symSizeX";
            this.symSizeX.SupportedEnhancedDataTypes = null;
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
            // symPositionX
            // 
            resources.ApplyResources(this.symPositionX, "symPositionX");
            this.symPositionX.Name = "symPositionX";
            this.symPositionX.SupportedEnhancedDataTypes = null;
            // 
            // symPositionY
            // 
            resources.ApplyResources(this.symPositionY, "symPositionY");
            this.symPositionY.Name = "symPositionY";
            this.symPositionY.SupportedEnhancedDataTypes = null;
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
            // symSizeY
            // 
            resources.ApplyResources(this.symSizeY, "symSizeY");
            this.symSizeY.Name = "symSizeY";
            this.symSizeY.SupportedEnhancedDataTypes = null;
            // 
            // symAngle
            // 
            resources.ApplyResources(this.symAngle, "symAngle");
            this.symAngle.Name = "symAngle";
            this.symAngle.SupportedEnhancedDataTypes = null;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // symSizeScalable
            // 
            resources.ApplyResources(this.symSizeScalable, "symSizeScalable");
            this.symSizeScalable.Name = "symSizeScalable";
            this.symSizeScalable.SupportedEnhancedDataTypes = null;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // ImageDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnResData);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.lnkLoadImage);
            this.Controls.Add(this.lnkPreview);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtImageBase64);
            this.Controls.Add(this.rdInline);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtResData);
            this.Controls.Add(this.txtResourceId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdResourceRef);
            this.Name = "ImageDialog";
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdResourceRef;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResourceId;
        private System.Windows.Forms.TextBox txtResData;
        private System.Windows.Forms.RadioButton rdInline;
        private System.Windows.Forms.TextBox txtImageBase64;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lnkPreview;
        private System.Windows.Forms.LinkLabel lnkLoadImage;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnResData;
        private System.Windows.Forms.GroupBox groupBox1;
        private SymbolField symSizeX;
        private SymbolField symAngle;
        private System.Windows.Forms.Label label7;
        private SymbolField symSizeScalable;
        private System.Windows.Forms.Label label8;
        private SymbolField symPositionY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private SymbolField symSizeY;
        private SymbolField symPositionX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}