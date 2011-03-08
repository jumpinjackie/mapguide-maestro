namespace MgCooker
{
    partial class Progress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tilePG = new System.Windows.Forms.ProgressBar();
            this.totalPG = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.finishEstimate = new System.Windows.Forms.Label();
            this.tileCounter = new System.Windows.Forms.Label();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.AutoEllipsis = true;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tilePG
            // 
            resources.ApplyResources(this.tilePG, "tilePG");
            this.tilePG.Maximum = 10000;
            this.tilePG.Name = "tilePG";
            this.tilePG.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // totalPG
            // 
            resources.ApplyResources(this.totalPG, "totalPG");
            this.totalPG.Maximum = 10000;
            this.totalPG.Name = "totalPG";
            this.totalPG.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // finishEstimate
            // 
            resources.ApplyResources(this.finishEstimate, "finishEstimate");
            this.finishEstimate.AutoEllipsis = true;
            this.finishEstimate.Name = "finishEstimate";
            // 
            // tileCounter
            // 
            resources.ApplyResources(this.tileCounter, "tileCounter");
            this.tileCounter.AutoEllipsis = true;
            this.tileCounter.Name = "tileCounter";
            // 
            // PauseBtn
            // 
            resources.ApplyResources(this.PauseBtn, "PauseBtn");
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // Progress
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.tileCounter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.finishEstimate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.totalPG);
            this.Controls.Add(this.tilePG);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Progress";
            this.Load += new System.EventHandler(this.Progress_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Progress_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar tilePG;
        private System.Windows.Forms.ProgressBar totalPG;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label finishEstimate;
        private System.Windows.Forms.Label tileCounter;
        private System.Windows.Forms.Button PauseBtn;
    }
}