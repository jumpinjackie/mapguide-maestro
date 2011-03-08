namespace Maestro.Base.UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoundsPicker));
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
            resources.ApplyResources(this.label1, "label1");
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // MinX
            // 
            resources.ApplyResources(this.MinX, "MinX");
            this.MinX.Name = "MinX";
            // 
            // MaxX
            // 
            resources.ApplyResources(this.MaxX, "MaxX");
            this.MaxX.Name = "MaxX";
            // 
            // MinY
            // 
            resources.ApplyResources(this.MinY, "MinY");
            this.MinY.Name = "MinY";
            // 
            // MaxY
            // 
            resources.ApplyResources(this.MaxY, "MaxY");
            this.MaxY.Name = "MaxY";
            // 
            // SRSLabel
            // 
            resources.ApplyResources(this.SRSLabel, "SRSLabel");
            this.SRSLabel.Name = "SRSLabel";
            // 
            // SRSCombo
            // 
            this.SRSCombo.FormattingEnabled = true;
            resources.ApplyResources(this.SRSCombo, "SRSCombo");
            this.SRSCombo.Name = "SRSCombo";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // BoundsPicker
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.CancelBtn;
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