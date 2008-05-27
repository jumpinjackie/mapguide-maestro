namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
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
            this.LabelStyle = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ItemStyle();
            this.FeatureStyle = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ItemStyle();
            this.SuspendLayout();
            // 
            // RuleCondition
            // 
            this.RuleCondition.Location = new System.Drawing.Point(32, 0);
            this.RuleCondition.Name = "RuleCondition";
            this.RuleCondition.Size = new System.Drawing.Size(110, 20);
            this.RuleCondition.TabIndex = 3;
            this.RuleCondition.TextChanged += new System.EventHandler(this.RuleCondition_TextChanged);
            // 
            // LegendLabel
            // 
            this.LegendLabel.Location = new System.Drawing.Point(168, 0);
            this.LegendLabel.Name = "LegendLabel";
            this.LegendLabel.Size = new System.Drawing.Size(134, 20);
            this.LegendLabel.TabIndex = 5;
            this.LegendLabel.TextChanged += new System.EventHandler(this.LegendLabel_TextChanged);
            // 
            // EditFilter
            // 
            this.EditFilter.Location = new System.Drawing.Point(142, 0);
            this.EditFilter.Name = "EditFilter";
            this.EditFilter.Size = new System.Drawing.Size(24, 20);
            this.EditFilter.TabIndex = 6;
            this.EditFilter.Text = "...";
            this.EditFilter.UseVisualStyleBackColor = true;
            this.EditFilter.Click += new System.EventHandler(this.EditFilter_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.Location = new System.Drawing.Point(0, 0);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(32, 20);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // LabelStyle
            // 
            this.LabelStyle.Location = new System.Drawing.Point(440, 0);
            this.LabelStyle.Name = "LabelStyle";
            this.LabelStyle.Size = new System.Drawing.Size(134, 20);
            this.LabelStyle.TabIndex = 1;
            this.LabelStyle.ItemChanged += new System.EventHandler(this.LabelStyle_ItemChanged);
            // 
            // FeatureStyle
            // 
            this.FeatureStyle.Location = new System.Drawing.Point(304, 0);
            this.FeatureStyle.Name = "FeatureStyle";
            this.FeatureStyle.Size = new System.Drawing.Size(134, 20);
            this.FeatureStyle.TabIndex = 0;
            this.FeatureStyle.ItemChanged += new System.EventHandler(this.FeatureStyle_ItemChanged);
            // 
            // Condition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditFilter);
            this.Controls.Add(this.LegendLabel);
            this.Controls.Add(this.RuleCondition);
            this.Controls.Add(this.LabelStyle);
            this.Controls.Add(this.FeatureStyle);
            this.MaximumSize = new System.Drawing.Size(0, 20);
            this.MinimumSize = new System.Drawing.Size(574, 20);
            this.Name = "Condition";
            this.Size = new System.Drawing.Size(574, 20);
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
