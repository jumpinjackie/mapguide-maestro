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
            this.editScalesButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveScaleButton = new System.Windows.Forms.ToolStripButton();
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
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lstScales);
            this.groupBox1.Controls.Add(this.statusStrip1);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstScales
            // 
            this.lstScales.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.lstScales, "lstScales");
            this.lstScales.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstScales.Name = "lstScales";
            this.lstScales.UseCompatibleStateImageBehavior = false;
            this.lstScales.View = System.Windows.Forms.View.Details;
            this.lstScales.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstScales_AfterLabelEdit);
            this.lstScales.SelectedIndexChanged += new System.EventHandler(this.lstScales_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listviewCount});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // listviewCount
            // 
            this.listviewCount.Name = "listviewCount";
            resources.ApplyResources(this.listviewCount, "listviewCount");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editScalesButton,
            this.RemoveScaleButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // editScalesButton
            // 
            this.editScalesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.editScalesButton, "editScalesButton");
            this.editScalesButton.Name = "editScalesButton";
            this.editScalesButton.Click += new System.EventHandler(this.editScalesButton_Click);
            // 
            // RemoveScaleButton
            // 
            this.RemoveScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveScaleButton, "RemoveScaleButton");
            this.RemoveScaleButton.Name = "RemoveScaleButton";
            this.RemoveScaleButton.Click += new System.EventHandler(this.RemoveScaleButton_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
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
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LogarithmicGeneration);
            this.panel2.Controls.Add(this.linearGeneration);
            this.panel2.Controls.Add(this.ExponentialGeneration);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // LogarithmicGeneration
            // 
            resources.ApplyResources(this.LogarithmicGeneration, "LogarithmicGeneration");
            this.LogarithmicGeneration.Name = "LogarithmicGeneration";
            this.LogarithmicGeneration.UseVisualStyleBackColor = true;
            // 
            // linearGeneration
            // 
            resources.ApplyResources(this.linearGeneration, "linearGeneration");
            this.linearGeneration.Name = "linearGeneration";
            this.linearGeneration.UseVisualStyleBackColor = true;
            this.linearGeneration.CheckedChanged += new System.EventHandler(this.linearGeneration_CheckedChanged);
            // 
            // ExponentialGeneration
            // 
            resources.ApplyResources(this.ExponentialGeneration, "ExponentialGeneration");
            this.ExponentialGeneration.Checked = true;
            this.ExponentialGeneration.Name = "ExponentialGeneration";
            this.ExponentialGeneration.TabStop = true;
            this.ExponentialGeneration.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RoundingOff);
            this.panel1.Controls.Add(this.PrettyRounding);
            this.panel1.Controls.Add(this.RegularRounding);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // RoundingOff
            // 
            resources.ApplyResources(this.RoundingOff, "RoundingOff");
            this.RoundingOff.Name = "RoundingOff";
            this.RoundingOff.UseVisualStyleBackColor = true;
            // 
            // PrettyRounding
            // 
            resources.ApplyResources(this.PrettyRounding, "PrettyRounding");
            this.PrettyRounding.Checked = true;
            this.PrettyRounding.Name = "PrettyRounding";
            this.PrettyRounding.TabStop = true;
            this.PrettyRounding.UseVisualStyleBackColor = true;
            // 
            // RegularRounding
            // 
            resources.ApplyResources(this.RegularRounding, "RegularRounding");
            this.RegularRounding.Name = "RegularRounding";
            this.RegularRounding.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // GenerateButton
            // 
            resources.ApplyResources(this.GenerateButton, "GenerateButton");
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // scaleCount
            // 
            resources.ApplyResources(this.scaleCount, "scaleCount");
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
            this.scaleCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.scaleCount.ValueChanged += new System.EventHandler(this.scaleCount_ValueChanged);
            // 
            // maxScale
            // 
            resources.ApplyResources(this.maxScale, "maxScale");
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
            this.maxScale.Value = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            // 
            // minScale
            // 
            resources.ApplyResources(this.minScale, "minScale");
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
            this.minScale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FiniteDisplayScales
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiniteDisplayScales";
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
        private System.Windows.Forms.ToolStripButton editScalesButton;
    }
}
