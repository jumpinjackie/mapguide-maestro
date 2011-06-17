namespace Maestro.Editors.WatermarkDefinition
{
    partial class TilePositionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilePositionEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numTileWidth = new System.Windows.Forms.NumericUpDown();
            this.numTileHeight = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numHorizontalOffset = new System.Windows.Forms.NumericUpDown();
            this.cmbHorizontalAlignment = new System.Windows.Forms.ComboBox();
            this.cmbHorizontalUnits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numVerticalOffset = new System.Windows.Forms.NumericUpDown();
            this.cmbVerticalAlignment = new System.Windows.Forms.ComboBox();
            this.cmbVerticalUnits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numTileWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTileHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHorizontalOffset)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVerticalOffset)).BeginInit();
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
            // numTileWidth
            // 
            resources.ApplyResources(this.numTileWidth, "numTileWidth");
            this.numTileWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numTileWidth.Name = "numTileWidth";
            this.numTileWidth.ValueChanged += new System.EventHandler(this.numTileWidth_ValueChanged);
            // 
            // numTileHeight
            // 
            resources.ApplyResources(this.numTileHeight, "numTileHeight");
            this.numTileHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numTileHeight.Name = "numTileHeight";
            this.numTileHeight.ValueChanged += new System.EventHandler(this.numTileHeight_ValueChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.numHorizontalOffset);
            this.groupBox1.Controls.Add(this.cmbHorizontalAlignment);
            this.groupBox1.Controls.Add(this.cmbHorizontalUnits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // numHorizontalOffset
            // 
            resources.ApplyResources(this.numHorizontalOffset, "numHorizontalOffset");
            this.numHorizontalOffset.Name = "numHorizontalOffset";
            this.numHorizontalOffset.ValueChanged += new System.EventHandler(this.numHorizontalOffset_ValueChanged);
            // 
            // cmbHorizontalAlignment
            // 
            resources.ApplyResources(this.cmbHorizontalAlignment, "cmbHorizontalAlignment");
            this.cmbHorizontalAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHorizontalAlignment.FormattingEnabled = true;
            this.cmbHorizontalAlignment.Name = "cmbHorizontalAlignment";
            this.cmbHorizontalAlignment.SelectedIndexChanged += new System.EventHandler(this.cmbHorizontalAlignment_SelectedIndexChanged);
            // 
            // cmbHorizontalUnits
            // 
            resources.ApplyResources(this.cmbHorizontalUnits, "cmbHorizontalUnits");
            this.cmbHorizontalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHorizontalUnits.FormattingEnabled = true;
            this.cmbHorizontalUnits.Name = "cmbHorizontalUnits";
            this.cmbHorizontalUnits.SelectedIndexChanged += new System.EventHandler(this.cmbHorizontalUnits_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.numVerticalOffset);
            this.groupBox2.Controls.Add(this.cmbVerticalAlignment);
            this.groupBox2.Controls.Add(this.cmbVerticalUnits);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // numVerticalOffset
            // 
            resources.ApplyResources(this.numVerticalOffset, "numVerticalOffset");
            this.numVerticalOffset.Name = "numVerticalOffset";
            this.numVerticalOffset.ValueChanged += new System.EventHandler(this.numVerticalOffset_ValueChanged);
            // 
            // cmbVerticalAlignment
            // 
            resources.ApplyResources(this.cmbVerticalAlignment, "cmbVerticalAlignment");
            this.cmbVerticalAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerticalAlignment.FormattingEnabled = true;
            this.cmbVerticalAlignment.Name = "cmbVerticalAlignment";
            this.cmbVerticalAlignment.SelectedIndexChanged += new System.EventHandler(this.cmbVerticalAlignment_SelectedIndexChanged);
            // 
            // cmbVerticalUnits
            // 
            resources.ApplyResources(this.cmbVerticalUnits, "cmbVerticalUnits");
            this.cmbVerticalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerticalUnits.FormattingEnabled = true;
            this.cmbVerticalUnits.Name = "cmbVerticalUnits";
            this.cmbVerticalUnits.SelectedIndexChanged += new System.EventHandler(this.cmbVerticalUnits_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // TilePositionEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numTileHeight);
            this.Controls.Add(this.numTileWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TilePositionEditor";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.numTileWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTileHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHorizontalOffset)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVerticalOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTileWidth;
        private System.Windows.Forms.NumericUpDown numTileHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbHorizontalAlignment;
        private System.Windows.Forms.ComboBox cmbHorizontalUnits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numHorizontalOffset;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numVerticalOffset;
        private System.Windows.Forms.ComboBox cmbVerticalAlignment;
        private System.Windows.Forms.ComboBox cmbVerticalUnits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}
