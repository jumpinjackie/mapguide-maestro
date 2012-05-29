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
            // IronPythonPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkShowOnStartup);
            this.Name = "IronPythonPreferences";
            this.Size = new System.Drawing.Size(193, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowOnStartup;
    }
}
