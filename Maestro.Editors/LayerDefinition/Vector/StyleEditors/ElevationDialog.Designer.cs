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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElevationDialog));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnZExtrusion = new System.Windows.Forms.Button();
            this.btnZOffset = new System.Windows.Forms.Button();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.cmbZOffsetType = new System.Windows.Forms.ComboBox();
            this.txtZExtrusion = new System.Windows.Forms.TextBox();
            this.txtZOffset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // grpSettings
            // 
            resources.ApplyResources(this.grpSettings, "grpSettings");
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
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.TabStop = false;
            // 
            // btnZExtrusion
            // 
            resources.ApplyResources(this.btnZExtrusion, "btnZExtrusion");
            this.btnZExtrusion.Name = "btnZExtrusion";
            this.btnZExtrusion.UseVisualStyleBackColor = true;
            this.btnZExtrusion.Click += new System.EventHandler(this.btnZExtrusion_Click);
            // 
            // btnZOffset
            // 
            resources.ApplyResources(this.btnZOffset, "btnZOffset");
            this.btnZOffset.Name = "btnZOffset";
            this.btnZOffset.UseVisualStyleBackColor = true;
            this.btnZOffset.Click += new System.EventHandler(this.btnZOffset_Click);
            // 
            // cmbUnits
            // 
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            resources.ApplyResources(this.cmbUnits, "cmbUnits");
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
            // 
            // cmbZOffsetType
            // 
            this.cmbZOffsetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZOffsetType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbZOffsetType, "cmbZOffsetType");
            this.cmbZOffsetType.Name = "cmbZOffsetType";
            this.cmbZOffsetType.SelectedIndexChanged += new System.EventHandler(this.cmbZOffsetType_SelectedIndexChanged);
            // 
            // txtZExtrusion
            // 
            resources.ApplyResources(this.txtZExtrusion, "txtZExtrusion");
            this.txtZExtrusion.Name = "txtZExtrusion";
            this.txtZExtrusion.TextChanged += new System.EventHandler(this.txtZExtrusion_TextChanged);
            // 
            // txtZOffset
            // 
            resources.ApplyResources(this.txtZOffset, "txtZOffset");
            this.txtZOffset.Name = "txtZOffset";
            this.txtZOffset.TextChanged += new System.EventHandler(this.txtZOffset_TextChanged);
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
            // ElevationDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "ElevationDialog";
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