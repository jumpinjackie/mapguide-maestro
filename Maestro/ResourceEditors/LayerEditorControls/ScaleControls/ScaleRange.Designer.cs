namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    partial class ScaleRange
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
            this.MinScale = new System.Windows.Forms.ComboBox();
            this.MaxScale = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scaleRangeConditions = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeConditions();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // MinScale
            // 
            this.MinScale.FormattingEnabled = true;
            this.MinScale.Items.AddRange(new object[] {
            "infinite"});
            this.MinScale.Location = new System.Drawing.Point(0, 0);
            this.MinScale.Name = "MinScale";
            this.MinScale.Size = new System.Drawing.Size(80, 21);
            this.MinScale.TabIndex = 1;
            this.MinScale.SelectedIndexChanged += new System.EventHandler(this.MinScale_SelectedIndexChanged);
            this.MinScale.TextChanged += new System.EventHandler(this.MinScale_TextChanged);
            // 
            // MaxScale
            // 
            this.MaxScale.FormattingEnabled = true;
            this.MaxScale.Items.AddRange(new object[] {
            "infinite"});
            this.MaxScale.Location = new System.Drawing.Point(80, 0);
            this.MaxScale.Name = "MaxScale";
            this.MaxScale.Size = new System.Drawing.Size(80, 21);
            this.MaxScale.TabIndex = 2;
            this.MaxScale.SelectedIndexChanged += new System.EventHandler(this.MaxScale_SelectedIndexChanged);
            this.MaxScale.TextChanged += new System.EventHandler(this.MaxScale_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MinScale);
            this.panel1.Controls.Add(this.MaxScale);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(160, 330);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 330);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 8);
            this.panel2.TabIndex = 4;
            // 
            // scaleRangeConditions
            // 
            this.scaleRangeConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scaleRangeConditions.Location = new System.Drawing.Point(160, 0);
            this.scaleRangeConditions.Name = "scaleRangeConditions";
            this.scaleRangeConditions.Owner = null;
            this.scaleRangeConditions.Size = new System.Drawing.Size(642, 330);
            this.scaleRangeConditions.TabIndex = 0;
            this.scaleRangeConditions.ItemChanged += new System.EventHandler(this.scaleRangeConditions_ItemChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ScaleRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scaleRangeConditions);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ScaleRange";
            this.Size = new System.Drawing.Size(802, 338);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ScaleRangeConditions scaleRangeConditions;
        private System.Windows.Forms.ComboBox MinScale;
        private System.Windows.Forms.ComboBox MaxScale;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
