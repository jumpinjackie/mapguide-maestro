namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class ConditionListButtons
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionListButtons));
            this.conditionList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionList();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AddRuleButton = new System.Windows.Forms.Button();
            this.CopyRuleButton = new System.Windows.Forms.Button();
            this.CreateThemeButton = new System.Windows.Forms.Button();
            this.MoveRuleDownButton = new System.Windows.Forms.Button();
            this.MoveRuleUpButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExplodeTheme = new System.Windows.Forms.Button();
            this.ShowInLegend = new System.Windows.Forms.CheckBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.conditionList.Name = "conditionList";
            this.conditionList.ItemChanged += new System.EventHandler(this.conditionList_ItemChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // AddRuleButton
            // 
            this.AddRuleButton.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.AddRuleButton, "AddRuleButton");
            this.AddRuleButton.Name = "AddRuleButton";
            this.toolTips.SetToolTip(this.AddRuleButton, resources.GetString("AddRuleButton.ToolTip"));
            this.AddRuleButton.UseVisualStyleBackColor = true;
            this.AddRuleButton.Click += new System.EventHandler(this.AddRuleButton_Click);
            // 
            // CopyRuleButton
            // 
            resources.ApplyResources(this.CopyRuleButton, "CopyRuleButton");
            this.CopyRuleButton.Image = global::Maestro.Editors.Properties.Resources.document_copy;
            this.CopyRuleButton.Name = "CopyRuleButton";
            this.toolTips.SetToolTip(this.CopyRuleButton, resources.GetString("CopyRuleButton.ToolTip"));
            this.CopyRuleButton.UseVisualStyleBackColor = true;
            this.CopyRuleButton.Click += new System.EventHandler(this.CopyRuleButton_Click);
            // 
            // CreateThemeButton
            // 
            resources.ApplyResources(this.CreateThemeButton, "CreateThemeButton");
            this.CreateThemeButton.Name = "CreateThemeButton";
            this.toolTips.SetToolTip(this.CreateThemeButton, resources.GetString("CreateThemeButton.ToolTip"));
            this.CreateThemeButton.UseVisualStyleBackColor = true;
            this.CreateThemeButton.Click += new System.EventHandler(this.CreateThemeButton_Click);
            // 
            // MoveRuleDownButton
            // 
            resources.ApplyResources(this.MoveRuleDownButton, "MoveRuleDownButton");
            this.MoveRuleDownButton.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.MoveRuleDownButton.Name = "MoveRuleDownButton";
            this.toolTips.SetToolTip(this.MoveRuleDownButton, resources.GetString("MoveRuleDownButton.ToolTip"));
            this.MoveRuleDownButton.UseVisualStyleBackColor = true;
            this.MoveRuleDownButton.Click += new System.EventHandler(this.MoveRuleDownButton_Click);
            // 
            // MoveRuleUpButton
            // 
            resources.ApplyResources(this.MoveRuleUpButton, "MoveRuleUpButton");
            this.MoveRuleUpButton.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.MoveRuleUpButton.Name = "MoveRuleUpButton";
            this.toolTips.SetToolTip(this.MoveRuleUpButton, resources.GetString("MoveRuleUpButton.ToolTip"));
            this.MoveRuleUpButton.UseVisualStyleBackColor = true;
            this.MoveRuleUpButton.Click += new System.EventHandler(this.MoveRuleUpButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExplodeTheme);
            this.panel1.Controls.Add(this.ShowInLegend);
            this.panel1.Controls.Add(this.MoveRuleUpButton);
            this.panel1.Controls.Add(this.MoveRuleDownButton);
            this.panel1.Controls.Add(this.CreateThemeButton);
            this.panel1.Controls.Add(this.CopyRuleButton);
            this.panel1.Controls.Add(this.AddRuleButton);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnExplodeTheme
            // 
            this.btnExplodeTheme.Image = global::Maestro.Editors.Properties.Resources.arrow_split;
            resources.ApplyResources(this.btnExplodeTheme, "btnExplodeTheme");
            this.btnExplodeTheme.Name = "btnExplodeTheme";
            this.toolTips.SetToolTip(this.btnExplodeTheme, resources.GetString("btnExplodeTheme.ToolTip"));
            this.btnExplodeTheme.UseVisualStyleBackColor = true;
            this.btnExplodeTheme.Click += new System.EventHandler(this.btnExplodeTheme_Click);
            // 
            // ShowInLegend
            // 
            resources.ApplyResources(this.ShowInLegend, "ShowInLegend");
            this.ShowInLegend.Checked = true;
            this.ShowInLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowInLegend.Name = "ShowInLegend";
            this.toolTips.SetToolTip(this.ShowInLegend, resources.GetString("ShowInLegend.ToolTip"));
            this.ShowInLegend.UseVisualStyleBackColor = true;
            this.ShowInLegend.CheckedChanged += new System.EventHandler(this.ShowInLegend_CheckedChanged);
            // 
            // ConditionListButtons
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.conditionList);
            this.Controls.Add(this.panel1);
            this.Name = "ConditionListButtons";
            resources.ApplyResources(this, "$this");
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ConditionList conditionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AddRuleButton;
        private System.Windows.Forms.Button CopyRuleButton;
        private System.Windows.Forms.Button CreateThemeButton;
        private System.Windows.Forms.Button MoveRuleDownButton;
        private System.Windows.Forms.Button MoveRuleUpButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ShowInLegend;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Button btnExplodeTheme;
    }
}
