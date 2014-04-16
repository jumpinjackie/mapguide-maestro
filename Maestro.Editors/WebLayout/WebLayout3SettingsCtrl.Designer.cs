namespace Maestro.Editors.WebLayout
{
    partial class WebLayout3SettingsCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelectionColor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPointBuffer = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMapImageFormat = new System.Windows.Forms.ComboBox();
            this.cmbSelectionImageFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStartupScript = new ICSharpCode.TextEditor.TextEditorControl();
            this.btnSelectionColor = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPointBuffer)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnSelectionColor);
            this.contentPanel.Controls.Add(this.txtStartupScript);
            this.contentPanel.Controls.Add(this.label5);
            this.contentPanel.Controls.Add(this.cmbSelectionImageFormat);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.cmbMapImageFormat);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.numPointBuffer);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.txtSelectionColor);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Size = new System.Drawing.Size(449, 222);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selection Color";
            // 
            // txtSelectionColor
            // 
            this.txtSelectionColor.Location = new System.Drawing.Point(17, 31);
            this.txtSelectionColor.Name = "txtSelectionColor";
            this.txtSelectionColor.Size = new System.Drawing.Size(92, 20);
            this.txtSelectionColor.TabIndex = 1;
            this.txtSelectionColor.TextChanged += new System.EventHandler(this.txtSelectionColor_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Point Selection Buffer (pixels)";
            // 
            // numPointBuffer
            // 
            this.numPointBuffer.Location = new System.Drawing.Point(17, 77);
            this.numPointBuffer.Name = "numPointBuffer";
            this.numPointBuffer.Size = new System.Drawing.Size(120, 20);
            this.numPointBuffer.TabIndex = 3;
            this.numPointBuffer.ValueChanged += new System.EventHandler(this.numPointBuffer_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Map Image Format";
            // 
            // cmbMapImageFormat
            // 
            this.cmbMapImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapImageFormat.FormattingEnabled = true;
            this.cmbMapImageFormat.Items.AddRange(new object[] {
            "PNG",
            "PNG8",
            "JPG",
            "GIF"});
            this.cmbMapImageFormat.Location = new System.Drawing.Point(16, 125);
            this.cmbMapImageFormat.Name = "cmbMapImageFormat";
            this.cmbMapImageFormat.Size = new System.Drawing.Size(121, 21);
            this.cmbMapImageFormat.TabIndex = 5;
            this.cmbMapImageFormat.SelectedIndexChanged += new System.EventHandler(this.cmbMapImageFormat_SelectedIndexChanged);
            // 
            // cmbSelectionImageFormat
            // 
            this.cmbSelectionImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectionImageFormat.FormattingEnabled = true;
            this.cmbSelectionImageFormat.Items.AddRange(new object[] {
            "PNG",
            "PNG8",
            "JPG",
            "GIF"});
            this.cmbSelectionImageFormat.Location = new System.Drawing.Point(16, 174);
            this.cmbSelectionImageFormat.Name = "cmbSelectionImageFormat";
            this.cmbSelectionImageFormat.Size = new System.Drawing.Size(121, 21);
            this.cmbSelectionImageFormat.TabIndex = 7;
            this.cmbSelectionImageFormat.SelectedIndexChanged += new System.EventHandler(this.cmbSelectionImageFormat_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Selection Image Format";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(170, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Startup Script";
            // 
            // txtStartupScript
            // 
            this.txtStartupScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartupScript.IsReadOnly = false;
            this.txtStartupScript.Location = new System.Drawing.Point(173, 31);
            this.txtStartupScript.Name = "txtStartupScript";
            this.txtStartupScript.Size = new System.Drawing.Size(259, 164);
            this.txtStartupScript.TabIndex = 9;
            this.txtStartupScript.TextChanged += new System.EventHandler(this.txtStartupScript_TextChanged);
            // 
            // btnSelectionColor
            // 
            this.btnSelectionColor.Location = new System.Drawing.Point(111, 29);
            this.btnSelectionColor.Name = "btnSelectionColor";
            this.btnSelectionColor.Size = new System.Drawing.Size(26, 23);
            this.btnSelectionColor.TabIndex = 10;
            this.btnSelectionColor.Text = "...";
            this.btnSelectionColor.UseVisualStyleBackColor = true;
            this.btnSelectionColor.Click += new System.EventHandler(this.btnSelectionColor_Click);
            // 
            // colorDialog
            // 
            this.colorDialog.FullOpen = true;
            // 
            // WebLayout3SettingsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "Advanced Settings";
            this.Name = "WebLayout3SettingsCtrl";
            this.Size = new System.Drawing.Size(449, 249);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPointBuffer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl txtStartupScript;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbSelectionImageFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMapImageFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPointBuffer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSelectionColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectionColor;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
