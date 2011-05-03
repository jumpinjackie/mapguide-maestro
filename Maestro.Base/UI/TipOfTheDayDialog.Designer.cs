namespace Maestro.Base.UI
{
    partial class TipOfTheDayDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipOfTheDayDialog));
            this.txtTip = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkShowTip = new System.Windows.Forms.CheckBox();
            this.btnNextTip = new System.Windows.Forms.Button();
            this.btnRandomTip = new System.Windows.Forms.Button();
            this.btnPrevTip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTip
            // 
            resources.ApplyResources(this.txtTip, "txtTip");
            this.txtTip.Name = "txtTip";
            this.txtTip.ReadOnly = true;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkShowTip
            // 
            resources.ApplyResources(this.chkShowTip, "chkShowTip");
            this.chkShowTip.Name = "chkShowTip";
            this.chkShowTip.UseVisualStyleBackColor = true;
            // 
            // btnNextTip
            // 
            resources.ApplyResources(this.btnNextTip, "btnNextTip");
            this.btnNextTip.Name = "btnNextTip";
            this.btnNextTip.UseVisualStyleBackColor = true;
            this.btnNextTip.Click += new System.EventHandler(this.btnNextTip_Click);
            // 
            // btnRandomTip
            // 
            resources.ApplyResources(this.btnRandomTip, "btnRandomTip");
            this.btnRandomTip.Name = "btnRandomTip";
            this.btnRandomTip.UseVisualStyleBackColor = true;
            this.btnRandomTip.Click += new System.EventHandler(this.btnRandomTip_Click);
            // 
            // btnPrevTip
            // 
            resources.ApplyResources(this.btnPrevTip, "btnPrevTip");
            this.btnPrevTip.Name = "btnPrevTip";
            this.btnPrevTip.UseVisualStyleBackColor = true;
            this.btnPrevTip.Click += new System.EventHandler(this.btnPrevTip_Click);
            // 
            // TipOfTheDayDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnPrevTip);
            this.Controls.Add(this.btnRandomTip);
            this.Controls.Add(this.btnNextTip);
            this.Controls.Add(this.chkShowTip);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtTip);
            this.Name = "TipOfTheDayDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtTip;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkShowTip;
        private System.Windows.Forms.Button btnNextTip;
        private System.Windows.Forms.Button btnRandomTip;
        private System.Windows.Forms.Button btnPrevTip;
    }
}