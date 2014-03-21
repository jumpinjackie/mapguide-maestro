using Maestro.Editors.Common;
namespace Maestro.Editors.LayerDefinition.Vector.Thematics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeCreator));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AggregateLabel = new System.Windows.Forms.Label();
            this.OverwriteRules = new System.Windows.Forms.RadioButton();
            this.AppendRules = new System.Windows.Forms.RadioButton();
            this.ColumnCombo = new System.Windows.Forms.ComboBox();
            this.RuleCount = new System.Windows.Forms.NumericUpDown();
            this.AggregateCombo = new System.Windows.Forms.ComboBox();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.rdValuesFromLookup = new System.Windows.Forms.RadioButton();
            this.rdValuesFromClass = new System.Windows.Forms.RadioButton();
            this.grpValuesFromLookup = new System.Windows.Forms.GroupBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUpdateThemeParameters = new System.Windows.Forms.Button();
            this.btnBrowseFeatureSource = new System.Windows.Forms.Button();
            this.cmbValueProperty = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbKeyProperty = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFeatureClass = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFeatureSource = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpValuesFromClass = new System.Windows.Forms.GroupBox();
            this.GroupPanel = new System.Windows.Forms.Panel();
            this.chkUseFirstRuleAsTemplate = new System.Windows.Forms.CheckBox();
            this.RuleCountPanel = new System.Windows.Forms.Panel();
            this.DisplayGroup = new System.Windows.Forms.GroupBox();
            this.btnFlipColorBrewer = new System.Windows.Forms.Button();
            this.ColorBrewerPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ColorBrewerDataType = new System.Windows.Forms.ComboBox();
            this.ColorBrewerColorSet = new Maestro.Editors.Common.CustomCombo();
            this.ColorBrewerLabel = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GradientToColor = new Maestro.Editors.Common.ColorComboBox();
            this.GradientFromColor = new Maestro.Editors.Common.ColorComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ColorBrewerColors = new System.Windows.Forms.RadioButton();
            this.GradientColors = new System.Windows.Forms.RadioButton();
            this.PreviewGroup = new System.Windows.Forms.GroupBox();
            this.PreviewPicture = new System.Windows.Forms.PictureBox();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.grpThemeGeneration = new System.Windows.Forms.GroupBox();
            this.colorComboBox1 = new Maestro.Editors.Common.ColorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.RuleCount)).BeginInit();
            this.DataGroup.SuspendLayout();
            this.grpValuesFromLookup.SuspendLayout();
            this.grpValuesFromClass.SuspendLayout();
            this.GroupPanel.SuspendLayout();
            this.RuleCountPanel.SuspendLayout();
            this.DisplayGroup.SuspendLayout();
            this.ColorBrewerPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PreviewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
            this.ButtonPanel.SuspendLayout();
            this.grpThemeGeneration.SuspendLayout();
            this.SuspendLayout();
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
            // AggregateLabel
            // 
            resources.ApplyResources(this.AggregateLabel, "AggregateLabel");
            this.AggregateLabel.Name = "AggregateLabel";
            // 
            // OverwriteRules
            // 
            resources.ApplyResources(this.OverwriteRules, "OverwriteRules");
            this.OverwriteRules.Checked = true;
            this.OverwriteRules.Name = "OverwriteRules";
            this.OverwriteRules.TabStop = true;
            this.OverwriteRules.UseVisualStyleBackColor = true;
            // 
            // AppendRules
            // 
            resources.ApplyResources(this.AppendRules, "AppendRules");
            this.AppendRules.Name = "AppendRules";
            this.AppendRules.UseVisualStyleBackColor = true;
            // 
            // ColumnCombo
            // 
            resources.ApplyResources(this.ColumnCombo, "ColumnCombo");
            this.ColumnCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnCombo.FormattingEnabled = true;
            this.ColumnCombo.Name = "ColumnCombo";
            this.ColumnCombo.SelectedIndexChanged += new System.EventHandler(this.ColumnCombo_SelectedIndexChanged);
            // 
            // RuleCount
            // 
            resources.ApplyResources(this.RuleCount, "RuleCount");
            this.RuleCount.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.RuleCount.Name = "RuleCount";
            this.RuleCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.RuleCount.ValueChanged += new System.EventHandler(this.RuleCount_ValueChanged);
            // 
            // AggregateCombo
            // 
            resources.ApplyResources(this.AggregateCombo, "AggregateCombo");
            this.AggregateCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AggregateCombo.FormattingEnabled = true;
            this.AggregateCombo.Items.AddRange(new object[] {
            resources.GetString("AggregateCombo.Items"),
            resources.GetString("AggregateCombo.Items1"),
            resources.GetString("AggregateCombo.Items2"),
            resources.GetString("AggregateCombo.Items3")});
            this.AggregateCombo.Name = "AggregateCombo";
            this.AggregateCombo.SelectedIndexChanged += new System.EventHandler(this.AggregateCombo_SelectedIndexChanged);
            // 
            // DataGroup
            // 
            resources.ApplyResources(this.DataGroup, "DataGroup");
            this.DataGroup.Controls.Add(this.rdValuesFromLookup);
            this.DataGroup.Controls.Add(this.rdValuesFromClass);
            this.DataGroup.Controls.Add(this.ColumnCombo);
            this.DataGroup.Controls.Add(this.label1);
            this.DataGroup.Controls.Add(this.grpValuesFromLookup);
            this.DataGroup.Controls.Add(this.grpValuesFromClass);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.TabStop = false;
            // 
            // rdValuesFromLookup
            // 
            resources.ApplyResources(this.rdValuesFromLookup, "rdValuesFromLookup");
            this.rdValuesFromLookup.Name = "rdValuesFromLookup";
            this.rdValuesFromLookup.UseVisualStyleBackColor = true;
            this.rdValuesFromLookup.CheckedChanged += new System.EventHandler(this.rdValuesFromLookup_CheckedChanged);
            // 
            // rdValuesFromClass
            // 
            resources.ApplyResources(this.rdValuesFromClass, "rdValuesFromClass");
            this.rdValuesFromClass.Checked = true;
            this.rdValuesFromClass.Name = "rdValuesFromClass";
            this.rdValuesFromClass.TabStop = true;
            this.rdValuesFromClass.UseVisualStyleBackColor = true;
            this.rdValuesFromClass.CheckedChanged += new System.EventHandler(this.rdValuesFromClass_CheckedChanged);
            // 
            // grpValuesFromLookup
            // 
            resources.ApplyResources(this.grpValuesFromLookup, "grpValuesFromLookup");
            this.grpValuesFromLookup.Controls.Add(this.btnFilter);
            this.grpValuesFromLookup.Controls.Add(this.txtFilter);
            this.grpValuesFromLookup.Controls.Add(this.label9);
            this.grpValuesFromLookup.Controls.Add(this.btnUpdateThemeParameters);
            this.grpValuesFromLookup.Controls.Add(this.btnBrowseFeatureSource);
            this.grpValuesFromLookup.Controls.Add(this.cmbValueProperty);
            this.grpValuesFromLookup.Controls.Add(this.label8);
            this.grpValuesFromLookup.Controls.Add(this.cmbKeyProperty);
            this.grpValuesFromLookup.Controls.Add(this.label7);
            this.grpValuesFromLookup.Controls.Add(this.cmbFeatureClass);
            this.grpValuesFromLookup.Controls.Add(this.label6);
            this.grpValuesFromLookup.Controls.Add(this.txtFeatureSource);
            this.grpValuesFromLookup.Controls.Add(this.label5);
            this.grpValuesFromLookup.Name = "grpValuesFromLookup";
            this.grpValuesFromLookup.TabStop = false;
            // 
            // btnFilter
            // 
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.ReadOnly = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btnUpdateThemeParameters
            // 
            resources.ApplyResources(this.btnUpdateThemeParameters, "btnUpdateThemeParameters");
            this.btnUpdateThemeParameters.Name = "btnUpdateThemeParameters";
            this.btnUpdateThemeParameters.UseVisualStyleBackColor = true;
            this.btnUpdateThemeParameters.Click += new System.EventHandler(this.btnUpdateThemeParameters_Click);
            // 
            // btnBrowseFeatureSource
            // 
            resources.ApplyResources(this.btnBrowseFeatureSource, "btnBrowseFeatureSource");
            this.btnBrowseFeatureSource.Name = "btnBrowseFeatureSource";
            this.btnBrowseFeatureSource.UseVisualStyleBackColor = true;
            this.btnBrowseFeatureSource.Click += new System.EventHandler(this.btnBrowseFeatureSource_Click);
            // 
            // cmbValueProperty
            // 
            resources.ApplyResources(this.cmbValueProperty, "cmbValueProperty");
            this.cmbValueProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValueProperty.FormattingEnabled = true;
            this.cmbValueProperty.Name = "cmbValueProperty";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbKeyProperty
            // 
            resources.ApplyResources(this.cmbKeyProperty, "cmbKeyProperty");
            this.cmbKeyProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyProperty.FormattingEnabled = true;
            this.cmbKeyProperty.Name = "cmbKeyProperty";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbFeatureClass
            // 
            resources.ApplyResources(this.cmbFeatureClass, "cmbFeatureClass");
            this.cmbFeatureClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFeatureClass.FormattingEnabled = true;
            this.cmbFeatureClass.Name = "cmbFeatureClass";
            this.cmbFeatureClass.SelectedIndexChanged += new System.EventHandler(this.cmbFeatureClass_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtFeatureSource
            // 
            resources.ApplyResources(this.txtFeatureSource, "txtFeatureSource");
            this.txtFeatureSource.Name = "txtFeatureSource";
            this.txtFeatureSource.ReadOnly = true;
            this.txtFeatureSource.TextChanged += new System.EventHandler(this.txtFeatureSource_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // grpValuesFromClass
            // 
            resources.ApplyResources(this.grpValuesFromClass, "grpValuesFromClass");
            this.grpValuesFromClass.Controls.Add(this.GroupPanel);
            this.grpValuesFromClass.Name = "grpValuesFromClass";
            this.grpValuesFromClass.TabStop = false;
            // 
            // GroupPanel
            // 
            resources.ApplyResources(this.GroupPanel, "GroupPanel");
            this.GroupPanel.Controls.Add(this.AggregateLabel);
            this.GroupPanel.Controls.Add(this.AggregateCombo);
            this.GroupPanel.Name = "GroupPanel";
            // 
            // chkUseFirstRuleAsTemplate
            // 
            resources.ApplyResources(this.chkUseFirstRuleAsTemplate, "chkUseFirstRuleAsTemplate");
            this.chkUseFirstRuleAsTemplate.Checked = true;
            this.chkUseFirstRuleAsTemplate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseFirstRuleAsTemplate.Name = "chkUseFirstRuleAsTemplate";
            this.chkUseFirstRuleAsTemplate.UseVisualStyleBackColor = true;
            // 
            // RuleCountPanel
            // 
            resources.ApplyResources(this.RuleCountPanel, "RuleCountPanel");
            this.RuleCountPanel.Controls.Add(this.RuleCount);
            this.RuleCountPanel.Controls.Add(this.label2);
            this.RuleCountPanel.Name = "RuleCountPanel";
            // 
            // DisplayGroup
            // 
            resources.ApplyResources(this.DisplayGroup, "DisplayGroup");
            this.DisplayGroup.Controls.Add(this.btnFlipColorBrewer);
            this.DisplayGroup.Controls.Add(this.ColorBrewerPanel);
            this.DisplayGroup.Controls.Add(this.ColorBrewerLabel);
            this.DisplayGroup.Controls.Add(this.panel2);
            this.DisplayGroup.Controls.Add(this.ColorBrewerColors);
            this.DisplayGroup.Controls.Add(this.GradientColors);
            this.DisplayGroup.Name = "DisplayGroup";
            this.DisplayGroup.TabStop = false;
            // 
            // btnFlipColorBrewer
            // 
            resources.ApplyResources(this.btnFlipColorBrewer, "btnFlipColorBrewer");
            this.btnFlipColorBrewer.Name = "btnFlipColorBrewer";
            this.btnFlipColorBrewer.UseVisualStyleBackColor = true;
            this.btnFlipColorBrewer.Click += new System.EventHandler(this.btnFlipColorBrewer_Click);
            // 
            // ColorBrewerPanel
            // 
            this.ColorBrewerPanel.Controls.Add(this.label3);
            this.ColorBrewerPanel.Controls.Add(this.ColorBrewerDataType);
            this.ColorBrewerPanel.Controls.Add(this.ColorBrewerColorSet);
            resources.ApplyResources(this.ColorBrewerPanel, "ColorBrewerPanel");
            this.ColorBrewerPanel.Name = "ColorBrewerPanel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ColorBrewerDataType
            // 
            this.ColorBrewerDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorBrewerDataType.FormattingEnabled = true;
            resources.ApplyResources(this.ColorBrewerDataType, "ColorBrewerDataType");
            this.ColorBrewerDataType.Name = "ColorBrewerDataType";
            this.ColorBrewerDataType.SelectedIndexChanged += new System.EventHandler(this.ColorBrewerDataType_SelectedIndexChanged);
            // 
            // ColorBrewerColorSet
            // 
            this.ColorBrewerColorSet.DropDownWidth = 150;
            this.ColorBrewerColorSet.FormattingEnabled = true;
            resources.ApplyResources(this.ColorBrewerColorSet, "ColorBrewerColorSet");
            this.ColorBrewerColorSet.Name = "ColorBrewerColorSet";
            this.ColorBrewerColorSet.SelectedIndexChanged += new System.EventHandler(this.ColorBrewerColorSet_SelectedIndexChanged);
            // 
            // ColorBrewerLabel
            // 
            resources.ApplyResources(this.ColorBrewerLabel, "ColorBrewerLabel");
            this.ColorBrewerLabel.Name = "ColorBrewerLabel";
            this.ColorBrewerLabel.TabStop = true;
            this.ColorBrewerLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ColorBrewerLabel_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GradientToColor);
            this.panel2.Controls.Add(this.GradientFromColor);
            this.panel2.Controls.Add(this.label4);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // GradientToColor
            // 
            this.GradientToColor.FormattingEnabled = true;
            resources.ApplyResources(this.GradientToColor, "GradientToColor");
            this.GradientToColor.Name = "GradientToColor";
            this.GradientToColor.SelectedIndexChanged += new System.EventHandler(this.GradientToColor_SelectedIndexChanged);
            // 
            // GradientFromColor
            // 
            this.GradientFromColor.FormattingEnabled = true;
            resources.ApplyResources(this.GradientFromColor, "GradientFromColor");
            this.GradientFromColor.Name = "GradientFromColor";
            this.GradientFromColor.SelectedIndexChanged += new System.EventHandler(this.GradientFromColor_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ColorBrewerColors
            // 
            resources.ApplyResources(this.ColorBrewerColors, "ColorBrewerColors");
            this.ColorBrewerColors.Name = "ColorBrewerColors";
            this.ColorBrewerColors.TabStop = true;
            this.ColorBrewerColors.UseVisualStyleBackColor = true;
            this.ColorBrewerColors.CheckedChanged += new System.EventHandler(this.ColorBrewerColors_CheckedChanged);
            // 
            // GradientColors
            // 
            resources.ApplyResources(this.GradientColors, "GradientColors");
            this.GradientColors.Name = "GradientColors";
            this.GradientColors.TabStop = true;
            this.GradientColors.UseVisualStyleBackColor = true;
            this.GradientColors.CheckedChanged += new System.EventHandler(this.GradientColors_CheckedChanged);
            // 
            // PreviewGroup
            // 
            resources.ApplyResources(this.PreviewGroup, "PreviewGroup");
            this.PreviewGroup.Controls.Add(this.PreviewPicture);
            this.PreviewGroup.Name = "PreviewGroup";
            this.PreviewGroup.TabStop = false;
            // 
            // PreviewPicture
            // 
            resources.ApplyResources(this.PreviewPicture, "PreviewPicture");
            this.PreviewPicture.Name = "PreviewPicture";
            this.PreviewPicture.TabStop = false;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.CancelBtn);
            this.ButtonPanel.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.ButtonPanel, "ButtonPanel");
            this.ButtonPanel.Name = "ButtonPanel";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // grpThemeGeneration
            // 
            resources.ApplyResources(this.grpThemeGeneration, "grpThemeGeneration");
            this.grpThemeGeneration.Controls.Add(this.RuleCountPanel);
            this.grpThemeGeneration.Controls.Add(this.OverwriteRules);
            this.grpThemeGeneration.Controls.Add(this.AppendRules);
            this.grpThemeGeneration.Controls.Add(this.chkUseFirstRuleAsTemplate);
            this.grpThemeGeneration.Name = "grpThemeGeneration";
            this.grpThemeGeneration.TabStop = false;
            // 
            // colorComboBox1
            // 
            resources.ApplyResources(this.colorComboBox1, "colorComboBox1");
            this.colorComboBox1.Name = "colorComboBox1";
            // 
            // ThemeCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.grpThemeGeneration);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.PreviewGroup);
            this.Controls.Add(this.DisplayGroup);
            this.Controls.Add(this.DataGroup);
            this.Name = "ThemeCreator";
            this.Load += new System.EventHandler(this.ThemeCreator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RuleCount)).EndInit();
            this.DataGroup.ResumeLayout(false);
            this.DataGroup.PerformLayout();
            this.grpValuesFromLookup.ResumeLayout(false);
            this.grpValuesFromLookup.PerformLayout();
            this.grpValuesFromClass.ResumeLayout(false);
            this.GroupPanel.ResumeLayout(false);
            this.GroupPanel.PerformLayout();
            this.RuleCountPanel.ResumeLayout(false);
            this.RuleCountPanel.PerformLayout();
            this.DisplayGroup.ResumeLayout(false);
            this.DisplayGroup.PerformLayout();
            this.ColorBrewerPanel.ResumeLayout(false);
            this.ColorBrewerPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PreviewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
            this.ButtonPanel.ResumeLayout(false);
            this.grpThemeGeneration.ResumeLayout(false);
            this.grpThemeGeneration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label AggregateLabel;
        private System.Windows.Forms.RadioButton OverwriteRules;
        private System.Windows.Forms.RadioButton AppendRules;
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
        private System.Windows.Forms.RadioButton ColorBrewerColors;
        private System.Windows.Forms.RadioButton GradientColors;
        private CustomCombo ColorBrewerColorSet;
        private System.Windows.Forms.Label label4;
        private ColorComboBox colorComboBox1;
        private System.Windows.Forms.Panel RuleCountPanel;
        private ColorComboBox GradientToColor;
        private ColorComboBox GradientFromColor;
        private System.Windows.Forms.PictureBox PreviewPicture;
        private System.Windows.Forms.Panel GroupPanel;
        private System.Windows.Forms.Panel ColorBrewerPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ColorBrewerDataType;
        private System.Windows.Forms.LinkLabel ColorBrewerLabel;
        private System.Windows.Forms.CheckBox chkUseFirstRuleAsTemplate;
        private System.Windows.Forms.Button btnFlipColorBrewer;
        private System.Windows.Forms.GroupBox grpValuesFromLookup;
        private System.Windows.Forms.GroupBox grpValuesFromClass;
        private System.Windows.Forms.Button btnBrowseFeatureSource;
        private System.Windows.Forms.ComboBox cmbValueProperty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbKeyProperty;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbFeatureClass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFeatureSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdValuesFromLookup;
        private System.Windows.Forms.RadioButton rdValuesFromClass;
        private System.Windows.Forms.GroupBox grpThemeGeneration;
        private System.Windows.Forms.Button btnUpdateThemeParameters;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label9;
    }
}