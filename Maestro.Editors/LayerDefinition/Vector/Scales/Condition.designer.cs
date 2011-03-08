namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class Condition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Condition));
            this.RuleCondition = new System.Windows.Forms.TextBox();
            this.LegendLabel = new System.Windows.Forms.TextBox();
            this.EditFilter = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.LabelStyle = new ItemStyle();
            this.FeatureStyle = new ItemStyle();
            this.SuspendLayout();
            // 
            // RuleCondition
            // 
            resources.ApplyResources(this.RuleCondition, "RuleCondition");
            this.RuleCondition.Name = "RuleCondition";
            this.RuleCondition.TextChanged += new System.EventHandler(this.RuleCondition_TextChanged);
            // 
            // LegendLabel
            // 
            resources.ApplyResources(this.LegendLabel, "LegendLabel");
            this.LegendLabel.Name = "LegendLabel";
            this.LegendLabel.TextChanged += new System.EventHandler(this.LegendLabel_TextChanged);
            // 
            // EditFilter
            // 
            resources.ApplyResources(this.EditFilter, "EditFilter");
            this.EditFilter.Name = "EditFilter";
            this.EditFilter.UseVisualStyleBackColor = true;
            this.EditFilter.Click += new System.EventHandler(this.EditFilter_Click);
            // 
            // DeleteButton
            // 
            resources.ApplyResources(this.DeleteButton, "DeleteButton");
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // LabelStyle
            // 
            resources.ApplyResources(this.LabelStyle, "LabelStyle");
            this.LabelStyle.Name = "LabelStyle";
            this.LabelStyle.Owner = null;
            this.LabelStyle.DoubleClick += new System.EventHandler(this.FeatureStyle_Click);
            this.LabelStyle.Click += new System.EventHandler(this.FeatureStyle_Click);
            this.LabelStyle.ItemChanged += new System.EventHandler(this.LabelStyle_ItemChanged);
            // 
            // FeatureStyle
            // 
            resources.ApplyResources(this.FeatureStyle, "FeatureStyle");
            this.FeatureStyle.Name = "FeatureStyle";
            this.FeatureStyle.Owner = null;
            this.FeatureStyle.DoubleClick += new System.EventHandler(this.FeatureStyle_Click);
            this.FeatureStyle.Click += new System.EventHandler(this.FeatureStyle_Click);
            this.FeatureStyle.ItemChanged += new System.EventHandler(this.FeatureStyle_ItemChanged);
            // 
            // Condition
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditFilter);
            this.Controls.Add(this.LegendLabel);
            this.Controls.Add(this.RuleCondition);
            this.Controls.Add(this.LabelStyle);
            this.Controls.Add(this.FeatureStyle);
            this.MaximumSize = new System.Drawing.Size(0, 20);
            this.MinimumSize = new System.Drawing.Size(577, 23);
            this.Name = "Condition";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ItemStyle FeatureStyle;
        private ItemStyle LabelStyle;
        private System.Windows.Forms.TextBox RuleCondition;
        private System.Windows.Forms.TextBox LegendLabel;
        private System.Windows.Forms.Button EditFilter;
        private System.Windows.Forms.Button DeleteButton;
    }
}
