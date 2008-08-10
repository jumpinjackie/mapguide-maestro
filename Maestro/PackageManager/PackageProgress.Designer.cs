namespace OSGeo.MapGuide.Maestro.PackageManager
{
    partial class PackageProgress
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
            this.CurrentProgress = new System.Windows.Forms.ProgressBar();
            this.TotalProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OperationLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentProgress
            // 
            this.CurrentProgress.Location = new System.Drawing.Point(88, 40);
            this.CurrentProgress.Name = "CurrentProgress";
            this.CurrentProgress.Size = new System.Drawing.Size(384, 23);
            this.CurrentProgress.TabIndex = 0;
            // 
            // TotalProgress
            // 
            this.TotalProgress.Location = new System.Drawing.Point(88, 72);
            this.TotalProgress.Name = "TotalProgress";
            this.TotalProgress.Size = new System.Drawing.Size(384, 23);
            this.TotalProgress.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File";
            // 
            // TotalLabel
            // 
            this.TotalLabel.AutoSize = true;
            this.TotalLabel.Location = new System.Drawing.Point(16, 72);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(31, 13);
            this.TotalLabel.TabIndex = 3;
            this.TotalLabel.Text = "Total";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Operation";
            // 
            // OperationLabel
            // 
            this.OperationLabel.AutoSize = true;
            this.OperationLabel.Location = new System.Drawing.Point(88, 8);
            this.OperationLabel.Name = "OperationLabel";
            this.OperationLabel.Size = new System.Drawing.Size(79, 13);
            this.OperationLabel.TabIndex = 5;
            this.OperationLabel.Text = "OperationLabel";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(184, 112);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(112, 24);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // PackageProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(489, 146);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OperationLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TotalLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TotalProgress);
            this.Controls.Add(this.CurrentProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackageProgress";
            this.Text = "Building package";
            this.Load += new System.EventHandler(this.PackageProgress_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PackageProgress_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar TotalProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TotalLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label OperationLabel;
        private System.Windows.Forms.Button CancelBtn;
        public System.Windows.Forms.ProgressBar CurrentProgress;
    }
}