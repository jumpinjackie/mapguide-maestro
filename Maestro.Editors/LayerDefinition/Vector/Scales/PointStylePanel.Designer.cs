namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class PointStylePanel
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
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.chkDisplayAsText = new System.Windows.Forms.CheckBox();
            this.chkAllowOverpost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Location = new System.Drawing.Point(3, 3);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(92, 17);
            this.chkEnable.TabIndex = 0;
            this.chkEnable.Text = "Display Points";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // chkDisplayAsText
            // 
            this.chkDisplayAsText.AutoSize = true;
            this.chkDisplayAsText.Location = new System.Drawing.Point(114, 3);
            this.chkDisplayAsText.Name = "chkDisplayAsText";
            this.chkDisplayAsText.Size = new System.Drawing.Size(99, 17);
            this.chkDisplayAsText.TabIndex = 1;
            this.chkDisplayAsText.Text = "Display As Text";
            this.chkDisplayAsText.UseVisualStyleBackColor = true;
            this.chkDisplayAsText.CheckedChanged += new System.EventHandler(this.chkDisplayAsText_CheckedChanged);
            // 
            // chkAllowOverpost
            // 
            this.chkAllowOverpost.AutoSize = true;
            this.chkAllowOverpost.Location = new System.Drawing.Point(230, 3);
            this.chkAllowOverpost.Name = "chkAllowOverpost";
            this.chkAllowOverpost.Size = new System.Drawing.Size(97, 17);
            this.chkAllowOverpost.TabIndex = 2;
            this.chkAllowOverpost.Text = "Allow Overpost";
            this.chkAllowOverpost.UseVisualStyleBackColor = true;
            this.chkAllowOverpost.CheckedChanged += new System.EventHandler(this.chkAllowOverpost_CheckedChanged);
            // 
            // PointStylePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkAllowOverpost);
            this.Controls.Add(this.chkDisplayAsText);
            this.Controls.Add(this.chkEnable);
            this.Name = "PointStylePanel";
            this.Size = new System.Drawing.Size(603, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.CheckBox chkDisplayAsText;
        private System.Windows.Forms.CheckBox chkAllowOverpost;
    }
}
