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
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
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
            // ImageDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
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
    }
}