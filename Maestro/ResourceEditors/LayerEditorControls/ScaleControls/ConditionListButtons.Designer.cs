namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionListButtons));
            this.conditionList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ConditionList();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AddRuleButton = new System.Windows.Forms.Button();
            this.CopyRuleButton = new System.Windows.Forms.Button();
            this.CreateThemeButton = new System.Windows.Forms.Button();
            this.MoveRuleDownButton = new System.Windows.Forms.Button();
            this.MoveRuleUpButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // conditionList
            // 
            this.conditionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.conditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.conditionList.Location = new System.Drawing.Point(0, 24);
            this.conditionList.Name = "conditionList";
            this.conditionList.SelectedItem = null;
            this.conditionList.Size = new System.Drawing.Size(592, 72);
            this.conditionList.TabIndex = 2;
            this.conditionList.ItemChanged += new System.EventHandler(this.conditionList_ItemChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rule";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Legendlabel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Featurestyle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(440, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Labelstyle";
            // 
            // AddRuleButton
            // 
            this.AddRuleButton.Image = ((System.Drawing.Image)(resources.GetObject("AddRuleButton.Image")));
            this.AddRuleButton.Location = new System.Drawing.Point(0, 0);
            this.AddRuleButton.Name = "AddRuleButton";
            this.AddRuleButton.Size = new System.Drawing.Size(32, 24);
            this.AddRuleButton.TabIndex = 4;
            this.AddRuleButton.UseVisualStyleBackColor = true;
            this.AddRuleButton.Click += new System.EventHandler(this.AddRuleButton_Click);
            // 
            // CopyRuleButton
            // 
            this.CopyRuleButton.Enabled = false;
            this.CopyRuleButton.Image = ((System.Drawing.Image)(resources.GetObject("CopyRuleButton.Image")));
            this.CopyRuleButton.Location = new System.Drawing.Point(136, 0);
            this.CopyRuleButton.Name = "CopyRuleButton";
            this.CopyRuleButton.Size = new System.Drawing.Size(32, 24);
            this.CopyRuleButton.TabIndex = 5;
            this.CopyRuleButton.UseVisualStyleBackColor = true;
            this.CopyRuleButton.Click += new System.EventHandler(this.CopyRuleButton_Click);
            // 
            // CreateThemeButton
            // 
            this.CreateThemeButton.Image = ((System.Drawing.Image)(resources.GetObject("CreateThemeButton.Image")));
            this.CreateThemeButton.Location = new System.Drawing.Point(104, 0);
            this.CreateThemeButton.Name = "CreateThemeButton";
            this.CreateThemeButton.Size = new System.Drawing.Size(32, 24);
            this.CreateThemeButton.TabIndex = 6;
            this.CreateThemeButton.UseVisualStyleBackColor = true;
            this.CreateThemeButton.Click += new System.EventHandler(this.CreateThemeButton_Click);
            // 
            // MoveRuleDownButton
            // 
            this.MoveRuleDownButton.Enabled = false;
            this.MoveRuleDownButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveRuleDownButton.Image")));
            this.MoveRuleDownButton.Location = new System.Drawing.Point(544, 0);
            this.MoveRuleDownButton.Name = "MoveRuleDownButton";
            this.MoveRuleDownButton.Size = new System.Drawing.Size(32, 24);
            this.MoveRuleDownButton.TabIndex = 7;
            this.MoveRuleDownButton.UseVisualStyleBackColor = true;
            this.MoveRuleDownButton.Click += new System.EventHandler(this.MoveRuleDownButton_Click);
            // 
            // MoveRuleUpButton
            // 
            this.MoveRuleUpButton.Enabled = false;
            this.MoveRuleUpButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveRuleUpButton.Image")));
            this.MoveRuleUpButton.Location = new System.Drawing.Point(512, 0);
            this.MoveRuleUpButton.Name = "MoveRuleUpButton";
            this.MoveRuleUpButton.Size = new System.Drawing.Size(32, 24);
            this.MoveRuleUpButton.TabIndex = 8;
            this.MoveRuleUpButton.UseVisualStyleBackColor = true;
            this.MoveRuleUpButton.Click += new System.EventHandler(this.MoveRuleUpButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MoveRuleUpButton);
            this.panel1.Controls.Add(this.MoveRuleDownButton);
            this.panel1.Controls.Add(this.CreateThemeButton);
            this.panel1.Controls.Add(this.CopyRuleButton);
            this.panel1.Controls.Add(this.AddRuleButton);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 24);
            this.panel1.TabIndex = 1;
            // 
            // ConditionListButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.conditionList);
            this.Controls.Add(this.panel1);
            this.Name = "ConditionListButtons";
            this.Size = new System.Drawing.Size(593, 97);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ConditionList conditionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AddRuleButton;
        private System.Windows.Forms.Button CopyRuleButton;
        private System.Windows.Forms.Button CreateThemeButton;
        private System.Windows.Forms.Button MoveRuleDownButton;
        private System.Windows.Forms.Button MoveRuleUpButton;
        private System.Windows.Forms.Panel panel1;
    }
}
