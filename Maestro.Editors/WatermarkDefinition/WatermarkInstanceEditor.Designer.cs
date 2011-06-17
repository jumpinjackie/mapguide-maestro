namespace Maestro.Editors.WatermarkDefinition
{
    partial class WatermarkInstanceEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatermarkInstanceEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResourceId = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbUsage = new System.Windows.Forms.ComboBox();
            this.grpAppearance = new System.Windows.Forms.GroupBox();
            this.numOvRotation = new System.Windows.Forms.NumericUpDown();
            this.numOvTransparency = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkOverrideAppearance = new System.Windows.Forms.CheckBox();
            this.grpPosition = new System.Windows.Forms.GroupBox();
            this.ovPosPanel = new System.Windows.Forms.Panel();
            this.rdOvPosXY = new System.Windows.Forms.RadioButton();
            this.rdOvTilePos = new System.Windows.Forms.RadioButton();
            this.chkOverridePosition = new System.Windows.Forms.CheckBox();
            this.grpAppearance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOvRotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOvTransparency)).BeginInit();
            this.grpPosition.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
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
            this.txtResourceId.ReadOnly = true;
            this.txtResourceId.TextChanged += new System.EventHandler(this.txtResourceId_TextChanged);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbUsage
            // 
            resources.ApplyResources(this.cmbUsage, "cmbUsage");
            this.cmbUsage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsage.FormattingEnabled = true;
            this.cmbUsage.Name = "cmbUsage";
            this.cmbUsage.SelectedIndexChanged += new System.EventHandler(this.cmbUsage_SelectedIndexChanged);
            // 
            // grpAppearance
            // 
            resources.ApplyResources(this.grpAppearance, "grpAppearance");
            this.grpAppearance.Controls.Add(this.numOvRotation);
            this.grpAppearance.Controls.Add(this.numOvTransparency);
            this.grpAppearance.Controls.Add(this.label5);
            this.grpAppearance.Controls.Add(this.label4);
            this.grpAppearance.Controls.Add(this.chkOverrideAppearance);
            this.grpAppearance.Name = "grpAppearance";
            this.grpAppearance.TabStop = false;
            // 
            // numOvRotation
            // 
            resources.ApplyResources(this.numOvRotation, "numOvRotation");
            this.numOvRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numOvRotation.Name = "numOvRotation";
            this.numOvRotation.ValueChanged += new System.EventHandler(this.numOvRotation_ValueChanged);
            // 
            // numOvTransparency
            // 
            resources.ApplyResources(this.numOvTransparency, "numOvTransparency");
            this.numOvTransparency.Name = "numOvTransparency";
            this.numOvTransparency.ValueChanged += new System.EventHandler(this.numOvTransparency_ValueChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkOverrideAppearance
            // 
            resources.ApplyResources(this.chkOverrideAppearance, "chkOverrideAppearance");
            this.chkOverrideAppearance.Name = "chkOverrideAppearance";
            this.chkOverrideAppearance.UseVisualStyleBackColor = true;
            this.chkOverrideAppearance.CheckedChanged += new System.EventHandler(this.chkOverrideAppearance_CheckedChanged);
            // 
            // grpPosition
            // 
            resources.ApplyResources(this.grpPosition, "grpPosition");
            this.grpPosition.Controls.Add(this.ovPosPanel);
            this.grpPosition.Controls.Add(this.rdOvPosXY);
            this.grpPosition.Controls.Add(this.rdOvTilePos);
            this.grpPosition.Name = "grpPosition";
            this.grpPosition.TabStop = false;
            // 
            // ovPosPanel
            // 
            resources.ApplyResources(this.ovPosPanel, "ovPosPanel");
            this.ovPosPanel.Name = "ovPosPanel";
            // 
            // rdOvPosXY
            // 
            resources.ApplyResources(this.rdOvPosXY, "rdOvPosXY");
            this.rdOvPosXY.Name = "rdOvPosXY";
            this.rdOvPosXY.UseVisualStyleBackColor = true;
            this.rdOvPosXY.CheckedChanged += new System.EventHandler(this.TilePos_CheckedChanged);
            // 
            // rdOvTilePos
            // 
            resources.ApplyResources(this.rdOvTilePos, "rdOvTilePos");
            this.rdOvTilePos.Checked = true;
            this.rdOvTilePos.Name = "rdOvTilePos";
            this.rdOvTilePos.TabStop = true;
            this.rdOvTilePos.UseVisualStyleBackColor = true;
            this.rdOvTilePos.CheckedChanged += new System.EventHandler(this.TilePos_CheckedChanged);
            // 
            // chkOverridePosition
            // 
            resources.ApplyResources(this.chkOverridePosition, "chkOverridePosition");
            this.chkOverridePosition.Name = "chkOverridePosition";
            this.chkOverridePosition.UseVisualStyleBackColor = true;
            this.chkOverridePosition.CheckedChanged += new System.EventHandler(this.chkOverridePosition_CheckedChanged);
            // 
            // WatermarkInstanceEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.chkOverridePosition);
            this.Controls.Add(this.grpPosition);
            this.Controls.Add(this.grpAppearance);
            this.Controls.Add(this.cmbUsage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtResourceId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "WatermarkInstanceEditor";
            resources.ApplyResources(this, "$this");
            this.grpAppearance.ResumeLayout(false);
            this.grpAppearance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOvRotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOvTransparency)).EndInit();
            this.grpPosition.ResumeLayout(false);
            this.grpPosition.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResourceId;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbUsage;
        private System.Windows.Forms.GroupBox grpAppearance;
        private System.Windows.Forms.CheckBox chkOverrideAppearance;
        private System.Windows.Forms.GroupBox grpPosition;
        private System.Windows.Forms.CheckBox chkOverridePosition;
        private System.Windows.Forms.NumericUpDown numOvRotation;
        private System.Windows.Forms.NumericUpDown numOvTransparency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdOvPosXY;
        private System.Windows.Forms.RadioButton rdOvTilePos;
        private System.Windows.Forms.Panel ovPosPanel;
    }
}
