namespace Maestro.Base.UI.Preferences
{
    partial class EditorPreferencesCtrl
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkAddDebugWatermark = new System.Windows.Forms.CheckBox();
            this.chkUseLocalPreview = new System.Windows.Forms.CheckBox();
            this.chkValidateOnSave = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseXsdPath = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtXsdPath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPreviewLocale = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkUseGridBasedStyleEditor = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReactBaseUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkAddDebugWatermark);
            this.groupBox4.Controls.Add(this.chkUseLocalPreview);
            this.groupBox4.Controls.Add(this.chkValidateOnSave);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(529, 78);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General";
            // 
            // chkAddDebugWatermark
            // 
            this.chkAddDebugWatermark.AutoSize = true;
            this.chkAddDebugWatermark.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAddDebugWatermark.Location = new System.Drawing.Point(19, 42);
            this.chkAddDebugWatermark.Name = "chkAddDebugWatermark";
            this.chkAddDebugWatermark.Size = new System.Drawing.Size(290, 17);
            this.chkAddDebugWatermark.TabIndex = 4;
            this.chkAddDebugWatermark.Text = "Add Debug Watermark for generated resource previews";
            this.chkAddDebugWatermark.UseVisualStyleBackColor = true;
            // 
            // chkUseLocalPreview
            // 
            this.chkUseLocalPreview.AutoSize = true;
            this.chkUseLocalPreview.Location = new System.Drawing.Point(134, 19);
            this.chkUseLocalPreview.Name = "chkUseLocalPreview";
            this.chkUseLocalPreview.Size = new System.Drawing.Size(257, 17);
            this.chkUseLocalPreview.TabIndex = 3;
            this.chkUseLocalPreview.Text = "Preview with local map viewer (where applicable)";
            this.chkUseLocalPreview.UseVisualStyleBackColor = true;
            // 
            // chkValidateOnSave
            // 
            this.chkValidateOnSave.AutoSize = true;
            this.chkValidateOnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkValidateOnSave.Location = new System.Drawing.Point(19, 19);
            this.chkValidateOnSave.Name = "chkValidateOnSave";
            this.chkValidateOnSave.Size = new System.Drawing.Size(109, 17);
            this.chkValidateOnSave.TabIndex = 2;
            this.chkValidateOnSave.Text = "Validate On Save";
            this.chkValidateOnSave.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBrowseXsdPath);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtXsdPath);
            this.groupBox1.Location = new System.Drawing.Point(3, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 66);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "XML Editor";
            // 
            // btnBrowseXsdPath
            // 
            this.btnBrowseXsdPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseXsdPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowseXsdPath.Location = new System.Drawing.Point(497, 22);
            this.btnBrowseXsdPath.Name = "btnBrowseXsdPath";
            this.btnBrowseXsdPath.Size = new System.Drawing.Size(25, 23);
            this.btnBrowseXsdPath.TabIndex = 14;
            this.btnBrowseXsdPath.Text = "...";
            this.btnBrowseXsdPath.UseVisualStyleBackColor = true;
            this.btnBrowseXsdPath.Click += new System.EventHandler(this.btnBrowseXsdPath_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(15, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Xml Schema Path";
            // 
            // txtXsdPath
            // 
            this.txtXsdPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXsdPath.Location = new System.Drawing.Point(112, 24);
            this.txtXsdPath.Name = "txtXsdPath";
            this.txtXsdPath.ReadOnly = true;
            this.txtXsdPath.Size = new System.Drawing.Size(379, 20);
            this.txtXsdPath.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtPreviewLocale);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 159);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(528, 60);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Localization";
            // 
            // txtPreviewLocale
            // 
            this.txtPreviewLocale.Location = new System.Drawing.Point(187, 22);
            this.txtPreviewLocale.Name = "txtPreviewLocale";
            this.txtPreviewLocale.Size = new System.Drawing.Size(47, 20);
            this.txtPreviewLocale.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Preview using the following locale";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.chkUseGridBasedStyleEditor);
            this.groupBox3.Location = new System.Drawing.Point(3, 223);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(528, 51);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Layer Editor";
            // 
            // chkUseGridBasedStyleEditor
            // 
            this.chkUseGridBasedStyleEditor.AutoSize = true;
            this.chkUseGridBasedStyleEditor.Location = new System.Drawing.Point(18, 19);
            this.chkUseGridBasedStyleEditor.Name = "chkUseGridBasedStyleEditor";
            this.chkUseGridBasedStyleEditor.Size = new System.Drawing.Size(152, 17);
            this.chkUseGridBasedStyleEditor.TabIndex = 0;
            this.chkUseGridBasedStyleEditor.Text = "Use Grid-based style editor";
            this.chkUseGridBasedStyleEditor.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.txtReactBaseUrl);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(4, 281);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(527, 85);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Viewer Previews";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "mapguide-react-layout base URL";
            // 
            // txtReactBaseUrl
            // 
            this.txtReactBaseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReactBaseUrl.Location = new System.Drawing.Point(182, 22);
            this.txtReactBaseUrl.Name = "txtReactBaseUrl";
            this.txtReactBaseUrl.Size = new System.Drawing.Size(339, 20);
            this.txtReactBaseUrl.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(376, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Setting this value will unlock extra preview URLs for Web and Flexible Layouts";
            // 
            // EditorPreferencesCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Name = "EditorPreferencesCtrl";
            this.Size = new System.Drawing.Size(535, 402);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkValidateOnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseXsdPath;
        private System.Windows.Forms.TextBox txtXsdPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPreviewLocale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseLocalPreview;
        private System.Windows.Forms.CheckBox chkAddDebugWatermark;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkUseGridBasedStyleEditor;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtReactBaseUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
