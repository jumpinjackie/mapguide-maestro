namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    partial class ThemeCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AggregateLabel = new System.Windows.Forms.Label();
            this.OverwriteRules = new System.Windows.Forms.RadioButton();
            this.AppendRules = new System.Windows.Forms.RadioButton();
            this.CreateDefaultRule = new System.Windows.Forms.CheckBox();
            this.ColumnCombo = new System.Windows.Forms.ComboBox();
            this.RuleCount = new System.Windows.Forms.NumericUpDown();
            this.AggregateCombo = new System.Windows.Forms.ComboBox();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.GroupPanel = new System.Windows.Forms.Panel();
            this.RuleCountPanel = new System.Windows.Forms.Panel();
            this.DisplayGroup = new System.Windows.Forms.GroupBox();
            this.ColorBrewerColorSet = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ChangeBaseStyleBtn = new System.Windows.Forms.Button();
            this.ColorBrewerColors = new System.Windows.Forms.RadioButton();
            this.GradientColors = new System.Windows.Forms.RadioButton();
            this.PreviewGroup = new System.Windows.Forms.GroupBox();
            this.PreviewPicture = new System.Windows.Forms.PictureBox();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.GradientToColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.GradientFromColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.colorComboBox1 = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.RuleCount)).BeginInit();
            this.DataGroup.SuspendLayout();
            this.GroupPanel.SuspendLayout();
            this.RuleCountPanel.SuspendLayout();
            this.DisplayGroup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PreviewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Column";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rulecount";
            // 
            // AggregateLabel
            // 
            this.AggregateLabel.AutoSize = true;
            this.AggregateLabel.Location = new System.Drawing.Point(0, 0);
            this.AggregateLabel.Name = "AggregateLabel";
            this.AggregateLabel.Size = new System.Drawing.Size(74, 13);
            this.AggregateLabel.TabIndex = 2;
            this.AggregateLabel.Text = "Group method";
            // 
            // OverwriteRules
            // 
            this.OverwriteRules.AutoSize = true;
            this.OverwriteRules.Checked = true;
            this.OverwriteRules.Location = new System.Drawing.Point(8, 88);
            this.OverwriteRules.Name = "OverwriteRules";
            this.OverwriteRules.Size = new System.Drawing.Size(133, 17);
            this.OverwriteRules.TabIndex = 3;
            this.OverwriteRules.TabStop = true;
            this.OverwriteRules.Text = "Overwrite existing rules";
            this.OverwriteRules.UseVisualStyleBackColor = true;
            // 
            // AppendRules
            // 
            this.AppendRules.AutoSize = true;
            this.AppendRules.Location = new System.Drawing.Point(8, 112);
            this.AppendRules.Name = "AppendRules";
            this.AppendRules.Size = new System.Drawing.Size(87, 17);
            this.AppendRules.TabIndex = 4;
            this.AppendRules.Text = "Append rules";
            this.AppendRules.UseVisualStyleBackColor = true;
            // 
            // CreateDefaultRule
            // 
            this.CreateDefaultRule.AutoSize = true;
            this.CreateDefaultRule.Checked = true;
            this.CreateDefaultRule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateDefaultRule.Location = new System.Drawing.Point(8, 136);
            this.CreateDefaultRule.Name = "CreateDefaultRule";
            this.CreateDefaultRule.Size = new System.Drawing.Size(121, 17);
            this.CreateDefaultRule.TabIndex = 5;
            this.CreateDefaultRule.Text = "Create a default rule";
            this.CreateDefaultRule.UseVisualStyleBackColor = true;
            // 
            // ColumnCombo
            // 
            this.ColumnCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ColumnCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnCombo.FormattingEnabled = true;
            this.ColumnCombo.Location = new System.Drawing.Point(104, 16);
            this.ColumnCombo.Name = "ColumnCombo";
            this.ColumnCombo.Size = new System.Drawing.Size(208, 21);
            this.ColumnCombo.TabIndex = 6;
            this.ColumnCombo.SelectedIndexChanged += new System.EventHandler(this.ColumnCombo_SelectedIndexChanged);
            // 
            // RuleCount
            // 
            this.RuleCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RuleCount.Location = new System.Drawing.Point(96, 0);
            this.RuleCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.RuleCount.Name = "RuleCount";
            this.RuleCount.Size = new System.Drawing.Size(208, 20);
            this.RuleCount.TabIndex = 7;
            this.RuleCount.ValueChanged += new System.EventHandler(this.RuleCount_ValueChanged);
            // 
            // AggregateCombo
            // 
            this.AggregateCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AggregateCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AggregateCombo.FormattingEnabled = true;
            this.AggregateCombo.Items.AddRange(new object[] {
            "Qualitative",
            "Sequential",
            "Diverging",
            "Individual"});
            this.AggregateCombo.Location = new System.Drawing.Point(96, 0);
            this.AggregateCombo.Name = "AggregateCombo";
            this.AggregateCombo.Size = new System.Drawing.Size(208, 21);
            this.AggregateCombo.TabIndex = 8;
            this.AggregateCombo.SelectedIndexChanged += new System.EventHandler(this.AggregateCombo_SelectedIndexChanged);
            // 
            // DataGroup
            // 
            this.DataGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGroup.Controls.Add(this.GroupPanel);
            this.DataGroup.Controls.Add(this.RuleCountPanel);
            this.DataGroup.Controls.Add(this.ColumnCombo);
            this.DataGroup.Controls.Add(this.CreateDefaultRule);
            this.DataGroup.Controls.Add(this.AppendRules);
            this.DataGroup.Controls.Add(this.OverwriteRules);
            this.DataGroup.Controls.Add(this.label1);
            this.DataGroup.Location = new System.Drawing.Point(8, 8);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Size = new System.Drawing.Size(320, 160);
            this.DataGroup.TabIndex = 9;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "Data setup";
            // 
            // GroupPanel
            // 
            this.GroupPanel.Controls.Add(this.AggregateLabel);
            this.GroupPanel.Controls.Add(this.AggregateCombo);
            this.GroupPanel.Location = new System.Drawing.Point(8, 40);
            this.GroupPanel.Name = "GroupPanel";
            this.GroupPanel.Size = new System.Drawing.Size(304, 24);
            this.GroupPanel.TabIndex = 10;
            // 
            // RuleCountPanel
            // 
            this.RuleCountPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RuleCountPanel.Controls.Add(this.RuleCount);
            this.RuleCountPanel.Controls.Add(this.label2);
            this.RuleCountPanel.Location = new System.Drawing.Point(8, 64);
            this.RuleCountPanel.Name = "RuleCountPanel";
            this.RuleCountPanel.Size = new System.Drawing.Size(304, 24);
            this.RuleCountPanel.TabIndex = 9;
            // 
            // DisplayGroup
            // 
            this.DisplayGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplayGroup.Controls.Add(this.ColorBrewerColorSet);
            this.DisplayGroup.Controls.Add(this.panel2);
            this.DisplayGroup.Controls.Add(this.ChangeBaseStyleBtn);
            this.DisplayGroup.Controls.Add(this.ColorBrewerColors);
            this.DisplayGroup.Controls.Add(this.GradientColors);
            this.DisplayGroup.Location = new System.Drawing.Point(8, 176);
            this.DisplayGroup.Name = "DisplayGroup";
            this.DisplayGroup.Size = new System.Drawing.Size(320, 112);
            this.DisplayGroup.TabIndex = 10;
            this.DisplayGroup.TabStop = false;
            this.DisplayGroup.Text = "Display setup";
            // 
            // ColorBrewerColorSet
            // 
            this.ColorBrewerColorSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorBrewerColorSet.FormattingEnabled = true;
            this.ColorBrewerColorSet.Location = new System.Drawing.Point(112, 48);
            this.ColorBrewerColorSet.Name = "ColorBrewerColorSet";
            this.ColorBrewerColorSet.Size = new System.Drawing.Size(200, 21);
            this.ColorBrewerColorSet.TabIndex = 4;
            this.ColorBrewerColorSet.SelectedIndexChanged += new System.EventHandler(this.ColorBrewerColorSet_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GradientToColor);
            this.panel2.Controls.Add(this.GradientFromColor);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(112, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 24);
            this.panel2.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "->";
            // 
            // ChangeBaseStyleBtn
            // 
            this.ChangeBaseStyleBtn.Location = new System.Drawing.Point(16, 80);
            this.ChangeBaseStyleBtn.Name = "ChangeBaseStyleBtn";
            this.ChangeBaseStyleBtn.Size = new System.Drawing.Size(112, 23);
            this.ChangeBaseStyleBtn.TabIndex = 2;
            this.ChangeBaseStyleBtn.Text = "Change base style";
            this.ChangeBaseStyleBtn.UseVisualStyleBackColor = true;
            // 
            // ColorBrewerColors
            // 
            this.ColorBrewerColors.AutoSize = true;
            this.ColorBrewerColors.Location = new System.Drawing.Point(16, 48);
            this.ColorBrewerColors.Name = "ColorBrewerColors";
            this.ColorBrewerColors.Size = new System.Drawing.Size(82, 17);
            this.ColorBrewerColors.TabIndex = 1;
            this.ColorBrewerColors.TabStop = true;
            this.ColorBrewerColors.Text = "ColorBrewer";
            this.ColorBrewerColors.UseVisualStyleBackColor = true;
            this.ColorBrewerColors.CheckedChanged += new System.EventHandler(this.ColorBrewerColors_CheckedChanged);
            // 
            // GradientColors
            // 
            this.GradientColors.AutoSize = true;
            this.GradientColors.Location = new System.Drawing.Point(16, 24);
            this.GradientColors.Name = "GradientColors";
            this.GradientColors.Size = new System.Drawing.Size(65, 17);
            this.GradientColors.TabIndex = 0;
            this.GradientColors.TabStop = true;
            this.GradientColors.Text = "Gradient";
            this.GradientColors.UseVisualStyleBackColor = true;
            this.GradientColors.CheckedChanged += new System.EventHandler(this.GradientColors_CheckedChanged);
            // 
            // PreviewGroup
            // 
            this.PreviewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviewGroup.Controls.Add(this.PreviewPicture);
            this.PreviewGroup.Location = new System.Drawing.Point(8, 296);
            this.PreviewGroup.Name = "PreviewGroup";
            this.PreviewGroup.Size = new System.Drawing.Size(320, 48);
            this.PreviewGroup.TabIndex = 11;
            this.PreviewGroup.TabStop = false;
            this.PreviewGroup.Text = "Preview";
            // 
            // PreviewPicture
            // 
            this.PreviewPicture.Location = new System.Drawing.Point(8, 16);
            this.PreviewPicture.Name = "PreviewPicture";
            this.PreviewPicture.Size = new System.Drawing.Size(304, 24);
            this.PreviewPicture.TabIndex = 0;
            this.PreviewPicture.TabStop = false;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.CancelBtn);
            this.ButtonPanel.Controls.Add(this.OKBtn);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 356);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(333, 36);
            this.ButtonPanel.TabIndex = 12;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CancelBtn.Location = new System.Drawing.Point(176, 7);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OKBtn.Location = new System.Drawing.Point(88, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // GradientToColor
            // 
            this.GradientToColor.FormattingEnabled = true;
            this.GradientToColor.Location = new System.Drawing.Point(112, 0);
            this.GradientToColor.Name = "GradientToColor";
            this.GradientToColor.Size = new System.Drawing.Size(88, 21);
            this.GradientToColor.TabIndex = 8;
            // 
            // GradientFromColor
            // 
            this.GradientFromColor.FormattingEnabled = true;
            this.GradientFromColor.Location = new System.Drawing.Point(0, 0);
            this.GradientFromColor.Name = "GradientFromColor";
            this.GradientFromColor.Size = new System.Drawing.Size(88, 21);
            this.GradientFromColor.TabIndex = 5;
            // 
            // colorComboBox1
            // 
            this.colorComboBox1.Location = new System.Drawing.Point(0, 0);
            this.colorComboBox1.Name = "colorComboBox1";
            this.colorComboBox1.Size = new System.Drawing.Size(121, 21);
            this.colorComboBox1.TabIndex = 0;
            // 
            // ThemeCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 392);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.PreviewGroup);
            this.Controls.Add(this.DisplayGroup);
            this.Controls.Add(this.DataGroup);
            this.Name = "ThemeCreator";
            this.Text = "Theme Creator";
            this.Load += new System.EventHandler(this.ThemeCreator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RuleCount)).EndInit();
            this.DataGroup.ResumeLayout(false);
            this.DataGroup.PerformLayout();
            this.GroupPanel.ResumeLayout(false);
            this.GroupPanel.PerformLayout();
            this.RuleCountPanel.ResumeLayout(false);
            this.RuleCountPanel.PerformLayout();
            this.DisplayGroup.ResumeLayout(false);
            this.DisplayGroup.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PreviewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AggregateLabel;
        private System.Windows.Forms.RadioButton OverwriteRules;
        private System.Windows.Forms.RadioButton AppendRules;
        private System.Windows.Forms.CheckBox CreateDefaultRule;
        private System.Windows.Forms.ComboBox ColumnCombo;
        private System.Windows.Forms.NumericUpDown RuleCount;
        private System.Windows.Forms.ComboBox AggregateCombo;
        private System.Windows.Forms.GroupBox DataGroup;
        private System.Windows.Forms.GroupBox DisplayGroup;
        private System.Windows.Forms.GroupBox PreviewGroup;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ChangeBaseStyleBtn;
        private System.Windows.Forms.RadioButton ColorBrewerColors;
        private System.Windows.Forms.RadioButton GradientColors;
        private System.Windows.Forms.ComboBox ColorBrewerColorSet;
        private System.Windows.Forms.Label label4;
        private OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox colorComboBox1;
        private System.Windows.Forms.Panel RuleCountPanel;
        private OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox GradientToColor;
        private OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox GradientFromColor;
        private System.Windows.Forms.PictureBox PreviewPicture;
        private System.Windows.Forms.Panel GroupPanel;
    }
}