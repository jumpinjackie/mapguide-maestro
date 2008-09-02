namespace OSGeo.MapGuide.Maestro
{
    partial class ExceptionViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExceptionText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ExceptionText
            // 
            this.ExceptionText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExceptionText.Location = new System.Drawing.Point(0, 0);
            this.ExceptionText.Multiline = true;
            this.ExceptionText.Name = "ExceptionText";
            this.ExceptionText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ExceptionText.Size = new System.Drawing.Size(543, 331);
            this.ExceptionText.TabIndex = 0;
            // 
            // ExceptionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 331);
            this.Controls.Add(this.ExceptionText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "ExceptionViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View the last exception detail";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExceptionViewer_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox ExceptionText;

    }
}