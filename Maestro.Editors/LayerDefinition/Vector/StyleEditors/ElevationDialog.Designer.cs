namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class ElevationDialog
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtZOffset = new System.Windows.Forms.TextBox();
            this.txtZExtrusion = new System.Windows.Forms.TextBox();
            this.cmbZOffsetType = new System.Windows.Forms.ComboBox();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.btnZOffset = new System.Windows.Forms.Button();
            this.btnZExtrusion = new System.Windows.Forms.Button();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(25, 247);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(106, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(12, 12);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(106, 17);
            this.chkEnabled.TabIndex = 2;
            this.chkEnabled.Text = "Settings Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // grpSettings
            // 
            this.grpSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSettings.Controls.Add(this.btnZExtrusion);
            this.grpSettings.Controls.Add(this.btnZOffset);
            this.grpSettings.Controls.Add(this.cmbUnits);
            this.grpSettings.Controls.Add(this.cmbZOffsetType);
            this.grpSettings.Controls.Add(this.txtZExtrusion);
            this.grpSettings.Controls.Add(this.txtZOffset);
            this.grpSettings.Controls.Add(this.label4);
            this.grpSettings.Controls.Add(this.label3);
            this.grpSettings.Controls.Add(this.label2);
            this.grpSettings.Controls.Add(this.label1);
            this.grpSettings.Location = new System.Drawing.Point(12, 36);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(169, 205);
            this.grpSettings.TabIndex = 3;
            this.grpSettings.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Z Offset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Z Offset Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z Extrusion";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Units";
            // 
            // txtZOffset
            // 
            this.txtZOffset.Location = new System.Drawing.Point(10, 37);
            this.txtZOffset.Name = "txtZOffset";
            this.txtZOffset.Size = new System.Drawing.Size(114, 20);
            this.txtZOffset.TabIndex = 4;
            this.txtZOffset.TextChanged += new System.EventHandler(this.txtZOffset_TextChanged);
            // 
            // txtZExtrusion
            // 
            this.txtZExtrusion.Location = new System.Drawing.Point(10, 78);
            this.txtZExtrusion.Name = "txtZExtrusion";
            this.txtZExtrusion.Size = new System.Drawing.Size(114, 20);
            this.txtZExtrusion.TabIndex = 5;
            this.txtZExtrusion.TextChanged += new System.EventHandler(this.txtZExtrusion_TextChanged);
            // 
            // cmbZOffsetType
            // 
            this.cmbZOffsetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZOffsetType.FormattingEnabled = true;
            this.cmbZOffsetType.Location = new System.Drawing.Point(10, 120);
            this.cmbZOffsetType.Name = "cmbZOffsetType";
            this.cmbZOffsetType.Size = new System.Drawing.Size(144, 21);
            this.cmbZOffsetType.TabIndex = 6;
            this.cmbZOffsetType.SelectedIndexChanged += new System.EventHandler(this.cmbZOffsetType_SelectedIndexChanged);
            // 
            // cmbUnits
            // 
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Location = new System.Drawing.Point(10, 167);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Size = new System.Drawing.Size(144, 21);
            this.cmbUnits.TabIndex = 7;
            this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
            // 
            // btnZOffset
            // 
            this.btnZOffset.Location = new System.Drawing.Point(130, 35);
            this.btnZOffset.Name = "btnZOffset";
            this.btnZOffset.Size = new System.Drawing.Size(24, 23);
            this.btnZOffset.TabIndex = 8;
            this.btnZOffset.Text = "...";
            this.btnZOffset.UseVisualStyleBackColor = true;
            this.btnZOffset.Click += new System.EventHandler(this.btnZOffset_Click);
            // 
            // btnZExtrusion
            // 
            this.btnZExtrusion.Location = new System.Drawing.Point(130, 76);
            this.btnZExtrusion.Name = "btnZExtrusion";
            this.btnZExtrusion.Size = new System.Drawing.Size(24, 23);
            this.btnZExtrusion.TabIndex = 9;
            this.btnZExtrusion.Text = "...";
            this.btnZExtrusion.UseVisualStyleBackColor = true;
            this.btnZExtrusion.Click += new System.EventHandler(this.btnZExtrusion_Click);
            // 
            // ElevationDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(193, 282);
            this.ControlBox = false;
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "ElevationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KML Elevation/Extrusion Settings";
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Button btnZOffset;
        private System.Windows.Forms.ComboBox cmbUnits;
        private System.Windows.Forms.ComboBox cmbZOffsetType;
        private System.Windows.Forms.TextBox txtZExtrusion;
        private System.Windows.Forms.TextBox txtZOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnZExtrusion;
    }
}