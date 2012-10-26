namespace Maestro.AddIn.Scripting.UI
{
    partial class IronPythonPreferences
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
            this.chkShowOnStartup = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtModulePaths = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkShowOnStartup
            // 
            this.chkShowOnStartup.AutoSize = true;
            this.chkShowOnStartup.Location = new System.Drawing.Point(22, 21);
            this.chkShowOnStartup.Name = "chkShowOnStartup";
            this.chkShowOnStartup.Size = new System.Drawing.Size(105, 17);
            this.chkShowOnStartup.TabIndex = 0;
            this.chkShowOnStartup.Text = "Show on Startup";
            this.chkShowOnStartup.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Python Module Paths";
            // 
            // txtModulePaths
            // 
            this.txtModulePaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModulePaths.Location = new System.Drawing.Point(149, 51);
            this.txtModulePaths.Name = "txtModulePaths";
            this.txtModulePaths.Size = new System.Drawing.Size(273, 20);
            this.txtModulePaths.TabIndex = 2;
            // 
            // IronPythonPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtModulePaths);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkShowOnStartup);
            this.Name = "IronPythonPreferences";
            this.Size = new System.Drawing.Size(448, 103);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowOnStartup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtModulePaths;
    }
}
