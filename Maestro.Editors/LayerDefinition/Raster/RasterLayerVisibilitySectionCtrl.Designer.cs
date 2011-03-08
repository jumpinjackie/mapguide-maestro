namespace Maestro.Editors.LayerDefinition.Raster
{
    partial class RasterLayerVisibilitySectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterLayerVisibilitySectionCtrl));
            this.txtRebuildFactor = new System.Windows.Forms.TextBox();
            this.RebuildThreshold = new System.Windows.Forms.Label();
            this.cmbVisibleTo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVisibleFrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBackgroundColor = new Maestro.Editors.Common.ColorComboBox();
            this.cmbForegroundColor = new Maestro.Editors.Common.ColorComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.groupBox1);
            this.contentPanel.Controls.Add(this.txtRebuildFactor);
            this.contentPanel.Controls.Add(this.RebuildThreshold);
            this.contentPanel.Controls.Add(this.cmbVisibleTo);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.txtVisibleFrom);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // txtRebuildFactor
            // 
            resources.ApplyResources(this.txtRebuildFactor, "txtRebuildFactor");
            this.txtRebuildFactor.Name = "txtRebuildFactor";
            this.txtRebuildFactor.TextChanged += new System.EventHandler(this.txtRebuildFactor_TextChanged);
            // 
            // RebuildThreshold
            // 
            resources.ApplyResources(this.RebuildThreshold, "RebuildThreshold");
            this.RebuildThreshold.Name = "RebuildThreshold";
            // 
            // cmbVisibleTo
            // 
            resources.ApplyResources(this.cmbVisibleTo, "cmbVisibleTo");
            this.cmbVisibleTo.Items.AddRange(new object[] {
            resources.GetString("cmbVisibleTo.Items")});
            this.cmbVisibleTo.Name = "cmbVisibleTo";
            this.cmbVisibleTo.SelectedIndexChanged += new System.EventHandler(this.cmbVisibleTo_SelectedIndexChanged);
            this.cmbVisibleTo.TextChanged += new System.EventHandler(this.cmbVisibleTo_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtVisibleFrom
            // 
            resources.ApplyResources(this.txtVisibleFrom, "txtVisibleFrom");
            this.txtVisibleFrom.Name = "txtVisibleFrom";
            this.txtVisibleFrom.TextChanged += new System.EventHandler(this.txtVisibleFrom_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cmbBackgroundColor);
            this.groupBox1.Controls.Add(this.cmbForegroundColor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbBackgroundColor
            // 
            this.cmbBackgroundColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBackgroundColor, "cmbBackgroundColor");
            this.cmbBackgroundColor.Name = "cmbBackgroundColor";
            this.cmbBackgroundColor.SelectedIndexChanged += new System.EventHandler(this.cmbBackgroundColor_SelectedIndexChanged);
            // 
            // cmbForegroundColor
            // 
            this.cmbForegroundColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbForegroundColor, "cmbForegroundColor");
            this.cmbForegroundColor.Name = "cmbForegroundColor";
            this.cmbForegroundColor.SelectedIndexChanged += new System.EventHandler(this.cmbForegroundColor_SelectedIndexChanged);
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
            // RasterLayerVisibilitySectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Layer Visbility";
            this.Name = "RasterLayerVisibilitySectionCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRebuildFactor;
        private System.Windows.Forms.Label RebuildThreshold;
        private System.Windows.Forms.ComboBox cmbVisibleTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVisibleFrom;
        private System.Windows.Forms.Label label1;
        private Maestro.Editors.Common.ColorComboBox cmbBackgroundColor;
        private Maestro.Editors.Common.ColorComboBox cmbForegroundColor;
    }
}
