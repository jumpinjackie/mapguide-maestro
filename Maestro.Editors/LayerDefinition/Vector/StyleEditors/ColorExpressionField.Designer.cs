namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class ColorExpressionField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorExpressionField));
            this.txtColor = new System.Windows.Forms.TextBox();
            this.btnExpr = new System.Windows.Forms.Button();
            this.btnColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtColor
            // 
            resources.ApplyResources(this.txtColor, "txtColor");
            this.txtColor.Name = "txtColor";
            this.txtColor.TextChanged += new System.EventHandler(this.txtColor_TextChanged);
            // 
            // btnExpr
            // 
            resources.ApplyResources(this.btnExpr, "btnExpr");
            this.btnExpr.Image = global::Maestro.Editors.Properties.Resources.sum;
            this.btnExpr.Name = "btnExpr";
            this.btnExpr.UseVisualStyleBackColor = true;
            this.btnExpr.Click += new System.EventHandler(this.btnExpr_Click);
            // 
            // btnColor
            // 
            resources.ApplyResources(this.btnColor, "btnColor");
            this.btnColor.Image = global::Maestro.Editors.Properties.Resources.color;
            this.btnColor.Name = "btnColor";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // ColorExpressionField
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.btnExpr);
            this.Controls.Add(this.txtColor);
            this.Name = "ColorExpressionField";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Button btnExpr;
        private System.Windows.Forms.Button btnColor;

    }
}
