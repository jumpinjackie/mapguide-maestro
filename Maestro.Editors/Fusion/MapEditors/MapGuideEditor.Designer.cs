namespace Maestro.Editors.Fusion.MapEditors
{
    partial class MapGuideEditor
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
            this.chkSelectionAsOverlay = new System.Windows.Forms.CheckBox();
            this.cmbSelectionColor = new Maestro.Editors.Common.ColorComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.txtViewScale = new System.Windows.Forms.TextBox();
            this.txtViewY = new System.Windows.Forms.TextBox();
            this.txtViewX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSelColor = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSingleTiled = new System.Windows.Forms.CheckBox();
            this.txtMapDefinition = new System.Windows.Forms.TextBox();
            this.btnBrowseMdf = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSelectionAsOverlay
            // 
            this.chkSelectionAsOverlay.AutoSize = true;
            this.chkSelectionAsOverlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSelectionAsOverlay.Location = new System.Drawing.Point(317, 107);
            this.chkSelectionAsOverlay.Name = "chkSelectionAsOverlay";
            this.chkSelectionAsOverlay.Size = new System.Drawing.Size(124, 17);
            this.chkSelectionAsOverlay.TabIndex = 19;
            this.chkSelectionAsOverlay.Text = "Selection As Overlay";
            this.chkSelectionAsOverlay.UseVisualStyleBackColor = true;
            this.chkSelectionAsOverlay.CheckedChanged += new System.EventHandler(this.chkSelectionAsOverlay_CheckedChanged);
            // 
            // cmbSelectionColor
            // 
            this.cmbSelectionColor.FormattingEnabled = true;
            this.cmbSelectionColor.Location = new System.Drawing.Point(103, 105);
            this.cmbSelectionColor.Name = "cmbSelectionColor";
            this.cmbSelectionColor.Size = new System.Drawing.Size(121, 21);
            this.cmbSelectionColor.TabIndex = 18;
            this.cmbSelectionColor.SelectedIndexChanged += new System.EventHandler(this.cmbSelectionColor_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkOverride);
            this.groupBox1.Controls.Add(this.txtViewScale);
            this.groupBox1.Controls.Add(this.txtViewY);
            this.groupBox1.Controls.Add(this.txtViewX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(13, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 56);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // chkOverride
            // 
            this.chkOverride.AutoSize = true;
            this.chkOverride.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkOverride.Location = new System.Drawing.Point(6, 0);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(119, 17);
            this.chkOverride.TabIndex = 6;
            this.chkOverride.Text = "Override Initial View";
            this.chkOverride.UseVisualStyleBackColor = true;
            this.chkOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
            // 
            // txtViewScale
            // 
            this.txtViewScale.Location = new System.Drawing.Point(321, 23);
            this.txtViewScale.Name = "txtViewScale";
            this.txtViewScale.Size = new System.Drawing.Size(92, 20);
            this.txtViewScale.TabIndex = 5;
            this.txtViewScale.TextChanged += new System.EventHandler(this.txtViewScale_TextChanged);
            // 
            // txtViewY
            // 
            this.txtViewY.Location = new System.Drawing.Point(180, 23);
            this.txtViewY.Name = "txtViewY";
            this.txtViewY.Size = new System.Drawing.Size(80, 20);
            this.txtViewY.TabIndex = 4;
            this.txtViewY.TextChanged += new System.EventHandler(this.txtViewY_TextChanged);
            // 
            // txtViewX
            // 
            this.txtViewX.Location = new System.Drawing.Point(54, 23);
            this.txtViewX.Name = "txtViewX";
            this.txtViewX.Size = new System.Drawing.Size(80, 20);
            this.txtViewX.TabIndex = 3;
            this.txtViewX.TextChanged += new System.EventHandler(this.txtViewX_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(272, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Scale";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(148, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(21, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "X";
            // 
            // lblSelColor
            // 
            this.lblSelColor.AutoSize = true;
            this.lblSelColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSelColor.Location = new System.Drawing.Point(13, 108);
            this.lblSelColor.Name = "lblSelColor";
            this.lblSelColor.Size = new System.Drawing.Size(78, 13);
            this.lblSelColor.TabIndex = 17;
            this.lblSelColor.Text = "Selection Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(10, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Map Definition";
            // 
            // chkSingleTiled
            // 
            this.chkSingleTiled.AutoSize = true;
            this.chkSingleTiled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSingleTiled.Location = new System.Drawing.Point(230, 107);
            this.chkSingleTiled.Name = "chkSingleTiled";
            this.chkSingleTiled.Size = new System.Drawing.Size(75, 17);
            this.chkSingleTiled.TabIndex = 16;
            this.chkSingleTiled.Text = "Single Tile";
            this.chkSingleTiled.UseVisualStyleBackColor = true;
            this.chkSingleTiled.CheckedChanged += new System.EventHandler(this.chkSingleTiled_CheckedChanged);
            // 
            // txtMapDefinition
            // 
            this.txtMapDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMapDefinition.Location = new System.Drawing.Point(103, 16);
            this.txtMapDefinition.Name = "txtMapDefinition";
            this.txtMapDefinition.ReadOnly = true;
            this.txtMapDefinition.Size = new System.Drawing.Size(305, 20);
            this.txtMapDefinition.TabIndex = 14;
            this.txtMapDefinition.TextChanged += new System.EventHandler(this.txtMapDefinition_TextChanged);
            // 
            // btnBrowseMdf
            // 
            this.btnBrowseMdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMdf.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowseMdf.Location = new System.Drawing.Point(414, 14);
            this.btnBrowseMdf.Name = "btnBrowseMdf";
            this.btnBrowseMdf.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseMdf.TabIndex = 15;
            this.btnBrowseMdf.Text = "...";
            this.btnBrowseMdf.UseVisualStyleBackColor = true;
            this.btnBrowseMdf.Click += new System.EventHandler(this.btnBrowseMdf_Click);
            // 
            // MapGuideEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkSelectionAsOverlay);
            this.Controls.Add(this.cmbSelectionColor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblSelColor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkSingleTiled);
            this.Controls.Add(this.txtMapDefinition);
            this.Controls.Add(this.btnBrowseMdf);
            this.Name = "MapGuideEditor";
            this.Size = new System.Drawing.Size(460, 138);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSelectionAsOverlay;
        private Common.ColorComboBox cmbSelectionColor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkOverride;
        private System.Windows.Forms.TextBox txtViewScale;
        private System.Windows.Forms.TextBox txtViewY;
        private System.Windows.Forms.TextBox txtViewX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSelColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkSingleTiled;
        private System.Windows.Forms.TextBox txtMapDefinition;
        private System.Windows.Forms.Button btnBrowseMdf;
    }
}
