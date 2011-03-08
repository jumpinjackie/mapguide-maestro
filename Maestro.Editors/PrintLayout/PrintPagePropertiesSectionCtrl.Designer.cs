namespace Maestro.Editors.PrintLayout
{
    partial class PrintPagePropertiesSectionCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPagePropertiesSectionCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.bgColorPicker = new System.Windows.Forms.ColorDialog();
            this.grpLayout = new System.Windows.Forms.GroupBox();
            this.chkCustomText = new System.Windows.Forms.CheckBox();
            this.chkNorthArrow = new System.Windows.Forms.CheckBox();
            this.chkCustomLogos = new System.Windows.Forms.CheckBox();
            this.chkScaleBar = new System.Windows.Forms.CheckBox();
            this.chkDateTime = new System.Windows.Forms.CheckBox();
            this.chkLegend = new System.Windows.Forms.CheckBox();
            this.chkURL = new System.Windows.Forms.CheckBox();
            this.chkTitle = new System.Windows.Forms.CheckBox();
            this.cmbBgColor = new Maestro.Editors.Common.ColorComboBox();
            this.contentPanel.SuspendLayout();
            this.grpLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.cmbBgColor);
            this.contentPanel.Controls.Add(this.grpLayout);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // grpLayout
            // 
            this.grpLayout.Controls.Add(this.chkCustomText);
            this.grpLayout.Controls.Add(this.chkNorthArrow);
            this.grpLayout.Controls.Add(this.chkCustomLogos);
            this.grpLayout.Controls.Add(this.chkScaleBar);
            this.grpLayout.Controls.Add(this.chkDateTime);
            this.grpLayout.Controls.Add(this.chkLegend);
            this.grpLayout.Controls.Add(this.chkURL);
            this.grpLayout.Controls.Add(this.chkTitle);
            resources.ApplyResources(this.grpLayout, "grpLayout");
            this.grpLayout.Name = "grpLayout";
            this.grpLayout.TabStop = false;
            // 
            // chkCustomText
            // 
            resources.ApplyResources(this.chkCustomText, "chkCustomText");
            this.chkCustomText.Name = "chkCustomText";
            this.chkCustomText.UseVisualStyleBackColor = true;
            // 
            // chkNorthArrow
            // 
            resources.ApplyResources(this.chkNorthArrow, "chkNorthArrow");
            this.chkNorthArrow.Name = "chkNorthArrow";
            this.chkNorthArrow.UseVisualStyleBackColor = true;
            // 
            // chkCustomLogos
            // 
            resources.ApplyResources(this.chkCustomLogos, "chkCustomLogos");
            this.chkCustomLogos.Name = "chkCustomLogos";
            this.chkCustomLogos.UseVisualStyleBackColor = true;
            // 
            // chkScaleBar
            // 
            resources.ApplyResources(this.chkScaleBar, "chkScaleBar");
            this.chkScaleBar.Name = "chkScaleBar";
            this.chkScaleBar.UseVisualStyleBackColor = true;
            // 
            // chkDateTime
            // 
            resources.ApplyResources(this.chkDateTime, "chkDateTime");
            this.chkDateTime.Name = "chkDateTime";
            this.chkDateTime.UseVisualStyleBackColor = true;
            // 
            // chkLegend
            // 
            resources.ApplyResources(this.chkLegend, "chkLegend");
            this.chkLegend.Name = "chkLegend";
            this.chkLegend.UseVisualStyleBackColor = true;
            // 
            // chkURL
            // 
            resources.ApplyResources(this.chkURL, "chkURL");
            this.chkURL.Name = "chkURL";
            this.chkURL.UseVisualStyleBackColor = true;
            // 
            // chkTitle
            // 
            resources.ApplyResources(this.chkTitle, "chkTitle");
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.UseVisualStyleBackColor = true;
            // 
            // cmbBgColor
            // 
            this.cmbBgColor.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBgColor, "cmbBgColor");
            this.cmbBgColor.Name = "cmbBgColor";
            // 
            // PrintPagePropertiesSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Page and Layout Properties";
            this.Name = "PrintPagePropertiesSectionCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.grpLayout.ResumeLayout(false);
            this.grpLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog bgColorPicker;
        private System.Windows.Forms.GroupBox grpLayout;
        private System.Windows.Forms.CheckBox chkCustomText;
        private System.Windows.Forms.CheckBox chkNorthArrow;
        private System.Windows.Forms.CheckBox chkCustomLogos;
        private System.Windows.Forms.CheckBox chkScaleBar;
        private System.Windows.Forms.CheckBox chkDateTime;
        private System.Windows.Forms.CheckBox chkLegend;
        private System.Windows.Forms.CheckBox chkURL;
        private System.Windows.Forms.CheckBox chkTitle;
        private Maestro.Editors.Common.ColorComboBox cmbBgColor;
    }
}
