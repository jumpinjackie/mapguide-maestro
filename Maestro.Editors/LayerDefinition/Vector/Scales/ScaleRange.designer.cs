namespace Maestro.Editors.LayerDefinition.Vector.Scales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleRange));
            this.MinScale = new System.Windows.Forms.ComboBox();
            this.MaxScale = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scaleRangeConditions = new ScaleRangeConditions();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // MinScale
            // 
            this.MinScale.FormattingEnabled = true;
            this.MinScale.Items.AddRange(new object[] {
            resources.GetString("MinScale.Items")});
            resources.ApplyResources(this.MinScale, "MinScale");
            this.MinScale.Name = "MinScale";
            this.MinScale.SelectedIndexChanged += new System.EventHandler(this.MinScale_SelectedIndexChanged);
            this.MinScale.TextChanged += new System.EventHandler(this.MinScale_TextChanged);
            // 
            // MaxScale
            // 
            this.MaxScale.FormattingEnabled = true;
            this.MaxScale.Items.AddRange(new object[] {
            resources.GetString("MaxScale.Items")});
            resources.ApplyResources(this.MaxScale, "MaxScale");
            this.MaxScale.Name = "MaxScale";
            this.MaxScale.SelectedIndexChanged += new System.EventHandler(this.MaxScale_SelectedIndexChanged);
            this.MaxScale.TextChanged += new System.EventHandler(this.MaxScale_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MinScale);
            this.panel1.Controls.Add(this.MaxScale);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // scaleRangeConditions
            // 
            resources.ApplyResources(this.scaleRangeConditions, "scaleRangeConditions");
            this.scaleRangeConditions.Name = "scaleRangeConditions";
            this.scaleRangeConditions.Owner = null;
            this.scaleRangeConditions.ItemChanged += new System.EventHandler(this.scaleRangeConditions_ItemChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ScaleRange
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.scaleRangeConditions);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ScaleRange";
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
