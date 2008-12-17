namespace OSGeo.MapGuide.MgCooker
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.finishEstimate = new System.Windows.Forms.Label();
            this.tileCounter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoEllipsis = true;
            this.label1.Location = new System.Drawing.Point(72, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rendering begins...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Group";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Total";
            // 
            // tilePG
            // 
            this.tilePG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tilePG.Location = new System.Drawing.Point(72, 64);
            this.tilePG.Maximum = 10000;
            this.tilePG.Name = "tilePG";
            this.tilePG.Size = new System.Drawing.Size(432, 16);
            this.tilePG.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tilePG.TabIndex = 6;
            // 
            // totalPG
            // 
            this.totalPG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.totalPG.Location = new System.Drawing.Point(72, 88);
            this.totalPG.Maximum = 10000;
            this.totalPG.Name = "totalPG";
            this.totalPG.Size = new System.Drawing.Size(432, 16);
            this.totalPG.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.totalPG.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(216, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 24);
            this.button1.TabIndex = 10;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Finish";
            // 
            // finishEstimate
            // 
            this.finishEstimate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.finishEstimate.AutoEllipsis = true;
            this.finishEstimate.Location = new System.Drawing.Point(72, 112);
            this.finishEstimate.Name = "finishEstimate";
            this.finishEstimate.Size = new System.Drawing.Size(432, 16);
            this.finishEstimate.TabIndex = 11;
            this.finishEstimate.Text = "Not estimated";
            // 
            // tileCounter
            // 
            this.tileCounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tileCounter.AutoEllipsis = true;
            this.tileCounter.Location = new System.Drawing.Point(72, 40);
            this.tileCounter.Name = "tileCounter";
            this.tileCounter.Size = new System.Drawing.Size(432, 16);
            this.tileCounter.TabIndex = 13;
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 184);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Progress";
            this.Text = "Progress";
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
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label finishEstimate;
        private System.Windows.Forms.Label tileCounter;
    }
}