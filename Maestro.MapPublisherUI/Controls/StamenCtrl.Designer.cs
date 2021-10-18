
namespace Maestro.MapPublisherUI.Controls
{
    partial class StamenCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStamenType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type";
            // 
            // cmbStamenType
            // 
            this.cmbStamenType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStamenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStamenType.FormattingEnabled = true;
            this.cmbStamenType.Location = new System.Drawing.Point(47, 3);
            this.cmbStamenType.Name = "cmbStamenType";
            this.cmbStamenType.Size = new System.Drawing.Size(334, 21);
            this.cmbStamenType.TabIndex = 4;
            this.cmbStamenType.SelectedIndexChanged += new System.EventHandler(this.cmbStamenType_SelectedIndexChanged);
            // 
            // StamenCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbStamenType);
            this.Controls.Add(this.label1);
            this.Name = "StamenCtrl";
            this.Size = new System.Drawing.Size(384, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStamenType;
    }
}
