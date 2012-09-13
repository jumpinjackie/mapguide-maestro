namespace Maestro.Packaging
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageProgress));
            this.CurrentProgress = new System.Windows.Forms.ProgressBar();
            this.TotalProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OperationLabel = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // CurrentProgress
            // 
            resources.ApplyResources(this.CurrentProgress, "CurrentProgress");
            this.CurrentProgress.Name = "CurrentProgress";
            // 
            // TotalProgress
            // 
            resources.ApplyResources(this.TotalProgress, "TotalProgress");
            this.TotalProgress.Name = "TotalProgress";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // TotalLabel
            // 
            resources.ApplyResources(this.TotalLabel, "TotalLabel");
            this.TotalLabel.Name = "TotalLabel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // OperationLabel
            // 
            resources.ApplyResources(this.OperationLabel, "OperationLabel");
            this.OperationLabel.AutoEllipsis = true;
            this.OperationLabel.Name = "OperationLabel";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // PackageProgress
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
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
        private System.Windows.Forms.ProgressBar CurrentProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}