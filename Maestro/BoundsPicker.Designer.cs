namespace OSGeo.MapGuide.Maestro
{
    partial class BoundsPicker
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MinX = new System.Windows.Forms.TextBox();
            this.MaxX = new System.Windows.Forms.TextBox();
            this.MinY = new System.Windows.Forms.TextBox();
            this.MaxY = new System.Windows.Forms.TextBox();
            this.SRSLabel = new System.Windows.Forms.Label();
            this.SRSCombo = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Min Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Max Y";
            // 
            // MinX
            // 
            this.MinX.Location = new System.Drawing.Point(64, 8);
            this.MinX.Name = "MinX";
            this.MinX.Size = new System.Drawing.Size(160, 20);
            this.MinX.TabIndex = 4;
            // 
            // MaxX
            // 
            this.MaxX.Location = new System.Drawing.Point(64, 32);
            this.MaxX.Name = "MaxX";
            this.MaxX.Size = new System.Drawing.Size(160, 20);
            this.MaxX.TabIndex = 5;
            // 
            // MinY
            // 
            this.MinY.Location = new System.Drawing.Point(64, 56);
            this.MinY.Name = "MinY";
            this.MinY.Size = new System.Drawing.Size(160, 20);
            this.MinY.TabIndex = 6;
            // 
            // MaxY
            // 
            this.MaxY.Location = new System.Drawing.Point(64, 80);
            this.MaxY.Name = "MaxY";
            this.MaxY.Size = new System.Drawing.Size(160, 20);
            this.MaxY.TabIndex = 7;
            // 
            // SRSLabel
            // 
            this.SRSLabel.AutoSize = true;
            this.SRSLabel.Location = new System.Drawing.Point(8, 112);
            this.SRSLabel.Name = "SRSLabel";
            this.SRSLabel.Size = new System.Drawing.Size(29, 13);
            this.SRSLabel.TabIndex = 8;
            this.SRSLabel.Text = "SRS";
            // 
            // SRSCombo
            // 
            this.SRSCombo.FormattingEnabled = true;
            this.SRSCombo.Location = new System.Drawing.Point(64, 112);
            this.SRSCombo.Name = "SRSCombo";
            this.SRSCombo.Size = new System.Drawing.Size(160, 21);
            this.SRSCombo.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 142);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 40);
            this.panel1.TabIndex = 10;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(121, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.Location = new System.Drawing.Point(33, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // BoundsPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(235, 182);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SRSCombo);
            this.Controls.Add(this.SRSLabel);
            this.Controls.Add(this.MaxY);
            this.Controls.Add(this.MinY);
            this.Controls.Add(this.MaxX);
            this.Controls.Add(this.MinX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BoundsPicker";
            this.Text = "Enter the data bounds";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MinX;
        private System.Windows.Forms.TextBox MaxX;
        private System.Windows.Forms.TextBox MinY;
        private System.Windows.Forms.TextBox MaxY;
        private System.Windows.Forms.Label SRSLabel;
        private System.Windows.Forms.ComboBox SRSCombo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
    }
}