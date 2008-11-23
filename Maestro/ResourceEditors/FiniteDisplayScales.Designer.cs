namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    partial class FiniteDisplayScales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiniteDisplayScales));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstScales = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.listviewCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddScaleButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveScaleButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshScalesButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LogarithmicGeneration = new System.Windows.Forms.RadioButton();
            this.linearGeneration = new System.Windows.Forms.RadioButton();
            this.ExponentialGeneration = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RoundingOff = new System.Windows.Forms.RadioButton();
            this.PrettyRounding = new System.Windows.Forms.RadioButton();
            this.RegularRounding = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.scaleCount = new System.Windows.Forms.NumericUpDown();
            this.maxScale = new System.Windows.Forms.NumericUpDown();
            this.minScale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstScales);
            this.groupBox1.Controls.Add(this.statusStrip1);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Location = new System.Drawing.Point(8, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display scales";
            // 
            // lstScales
            // 
            this.lstScales.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstScales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScales.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstScales.LabelEdit = true;
            this.lstScales.Location = new System.Drawing.Point(3, 41);
            this.lstScales.Name = "lstScales";
            this.lstScales.Size = new System.Drawing.Size(367, 83);
            this.lstScales.TabIndex = 1;
            this.lstScales.UseCompatibleStateImageBehavior = false;
            this.lstScales.View = System.Windows.Forms.View.Details;
            this.lstScales.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstScales_AfterLabelEdit);
            this.lstScales.SelectedIndexChanged += new System.EventHandler(this.lstScales_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Scale";
            this.columnHeader1.Width = 300;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listviewCount});
            this.statusStrip1.Location = new System.Drawing.Point(3, 124);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(367, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // listviewCount
            // 
            this.listviewCount.Name = "listviewCount";
            this.listviewCount.Size = new System.Drawing.Size(57, 17);
            this.listviewCount.Text = "500 scales";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddScaleButton,
            this.RemoveScaleButton,
            this.toolStripSeparator1,
            this.RefreshScalesButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(367, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddScaleButton
            // 
            this.AddScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddScaleButton.Image = ((System.Drawing.Image)(resources.GetObject("AddScaleButton.Image")));
            this.AddScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddScaleButton.Name = "AddScaleButton";
            this.AddScaleButton.Size = new System.Drawing.Size(23, 22);
            this.AddScaleButton.Text = "toolStripButton1";
            this.AddScaleButton.Click += new System.EventHandler(this.AddScaleButton_Click);
            // 
            // RemoveScaleButton
            // 
            this.RemoveScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveScaleButton.Enabled = false;
            this.RemoveScaleButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveScaleButton.Image")));
            this.RemoveScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveScaleButton.Name = "RemoveScaleButton";
            this.RemoveScaleButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveScaleButton.Text = "toolStripButton2";
            this.RemoveScaleButton.Click += new System.EventHandler(this.RemoveScaleButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // RefreshScalesButton
            // 
            this.RefreshScalesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshScalesButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshScalesButton.Image")));
            this.RefreshScalesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshScalesButton.Name = "RefreshScalesButton";
            this.RefreshScalesButton.Size = new System.Drawing.Size(23, 22);
            this.RefreshScalesButton.ToolTipText = "Re-sort the scales and remove duplicates";
            this.RefreshScalesButton.Click += new System.EventHandler(this.RefreshScalesButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.GenerateButton);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.scaleCount);
            this.groupBox2.Controls.Add(this.maxScale);
            this.groupBox2.Controls.Add(this.minScale);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 136);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generate scales";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LogarithmicGeneration);
            this.panel2.Controls.Add(this.linearGeneration);
            this.panel2.Controls.Add(this.ExponentialGeneration);
            this.panel2.Location = new System.Drawing.Point(112, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 16);
            this.panel2.TabIndex = 17;
            // 
            // LogarithmicGeneration
            // 
            this.LogarithmicGeneration.AutoSize = true;
            this.LogarithmicGeneration.Location = new System.Drawing.Point(152, 0);
            this.LogarithmicGeneration.Name = "LogarithmicGeneration";
            this.LogarithmicGeneration.Size = new System.Drawing.Size(79, 17);
            this.LogarithmicGeneration.TabIndex = 11;
            this.LogarithmicGeneration.Text = "Logarithmic";
            this.LogarithmicGeneration.UseVisualStyleBackColor = true;
            this.LogarithmicGeneration.Visible = false;
            // 
            // linearGeneration
            // 
            this.linearGeneration.AutoSize = true;
            this.linearGeneration.Location = new System.Drawing.Point(0, 0);
            this.linearGeneration.Name = "linearGeneration";
            this.linearGeneration.Size = new System.Drawing.Size(54, 17);
            this.linearGeneration.TabIndex = 7;
            this.linearGeneration.Text = "Linear";
            this.linearGeneration.UseVisualStyleBackColor = true;
            this.linearGeneration.CheckedChanged += new System.EventHandler(this.linearGeneration_CheckedChanged);
            // 
            // ExponentialGeneration
            // 
            this.ExponentialGeneration.AutoSize = true;
            this.ExponentialGeneration.Checked = true;
            this.ExponentialGeneration.Location = new System.Drawing.Point(64, 0);
            this.ExponentialGeneration.Name = "ExponentialGeneration";
            this.ExponentialGeneration.Size = new System.Drawing.Size(80, 17);
            this.ExponentialGeneration.TabIndex = 8;
            this.ExponentialGeneration.TabStop = true;
            this.ExponentialGeneration.Text = "Exponential";
            this.ExponentialGeneration.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RoundingOff);
            this.panel1.Controls.Add(this.PrettyRounding);
            this.panel1.Controls.Add(this.RegularRounding);
            this.panel1.Location = new System.Drawing.Point(112, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 16);
            this.panel1.TabIndex = 16;
            // 
            // RoundingOff
            // 
            this.RoundingOff.AutoSize = true;
            this.RoundingOff.Location = new System.Drawing.Point(0, 0);
            this.RoundingOff.Name = "RoundingOff";
            this.RoundingOff.Size = new System.Drawing.Size(51, 17);
            this.RoundingOff.TabIndex = 13;
            this.RoundingOff.Text = "None";
            this.RoundingOff.UseVisualStyleBackColor = true;
            // 
            // PrettyRounding
            // 
            this.PrettyRounding.AutoSize = true;
            this.PrettyRounding.Checked = true;
            this.PrettyRounding.Location = new System.Drawing.Point(152, 0);
            this.PrettyRounding.Name = "PrettyRounding";
            this.PrettyRounding.Size = new System.Drawing.Size(52, 17);
            this.PrettyRounding.TabIndex = 15;
            this.PrettyRounding.TabStop = true;
            this.PrettyRounding.Text = "Pretty";
            this.PrettyRounding.UseVisualStyleBackColor = true;
            // 
            // RegularRounding
            // 
            this.RegularRounding.AutoSize = true;
            this.RegularRounding.Location = new System.Drawing.Point(64, 0);
            this.RegularRounding.Name = "RegularRounding";
            this.RegularRounding.Size = new System.Drawing.Size(62, 17);
            this.RegularRounding.TabIndex = 14;
            this.RegularRounding.Text = "Regular";
            this.RegularRounding.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Rounding";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateButton.Location = new System.Drawing.Point(272, 104);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(96, 24);
            this.GenerateButton.TabIndex = 10;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Method";
            // 
            // scaleCount
            // 
            this.scaleCount.Location = new System.Drawing.Point(112, 104);
            this.scaleCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.scaleCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleCount.Name = "scaleCount";
            this.scaleCount.Size = new System.Drawing.Size(64, 20);
            this.scaleCount.TabIndex = 5;
            this.scaleCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.scaleCount.ValueChanged += new System.EventHandler(this.scaleCount_ValueChanged);
            // 
            // maxScale
            // 
            this.maxScale.Location = new System.Drawing.Point(280, 24);
            this.maxScale.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.maxScale.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.maxScale.Name = "maxScale";
            this.maxScale.Size = new System.Drawing.Size(88, 20);
            this.maxScale.TabIndex = 4;
            this.maxScale.ThousandsSeparator = true;
            this.maxScale.Value = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            // 
            // minScale
            // 
            this.minScale.Location = new System.Drawing.Point(112, 24);
            this.minScale.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.minScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minScale.Name = "minScale";
            this.minScale.Size = new System.Drawing.Size(88, 20);
            this.minScale.TabIndex = 3;
            this.minScale.ThousandsSeparator = true;
            this.minScale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Number of scales";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max scale";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min scale";
            // 
            // FiniteDisplayScales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(390, 300);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiniteDisplayScales";
            this.Size = new System.Drawing.Size(390, 300);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddScaleButton;
        private System.Windows.Forms.ToolStripButton RemoveScaleButton;
        private System.Windows.Forms.ListView lstScales;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown scaleCount;
        private System.Windows.Forms.NumericUpDown maxScale;
        private System.Windows.Forms.NumericUpDown minScale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.RadioButton ExponentialGeneration;
        private System.Windows.Forms.RadioButton linearGeneration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton LogarithmicGeneration;
        private System.Windows.Forms.RadioButton PrettyRounding;
        private System.Windows.Forms.RadioButton RegularRounding;
        private System.Windows.Forms.RadioButton RoundingOff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel listviewCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton RefreshScalesButton;
    }
}
