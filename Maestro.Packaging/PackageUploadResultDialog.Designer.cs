namespace Maestro.Packaging
{
    partial class PackageUploadResultDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageUploadResultDialog));
            this.lblSucceeded = new System.Windows.Forms.Label();
            this.lblFailed = new System.Windows.Forms.Label();
            this.lblSkipped = new System.Windows.Forms.Label();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdFailed = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdFailed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSucceeded
            // 
            resources.ApplyResources(this.lblSucceeded, "lblSucceeded");
            this.lblSucceeded.Name = "lblSucceeded";
            // 
            // lblFailed
            // 
            resources.ApplyResources(this.lblFailed, "lblFailed");
            this.lblFailed.Name = "lblFailed";
            // 
            // lblSkipped
            // 
            resources.ApplyResources(this.lblSkipped, "lblSkipped");
            this.lblSkipped.Name = "lblSkipped";
            // 
            // btnRetry
            // 
            resources.ApplyResources(this.btnRetry, "btnRetry");
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdFailed
            // 
            this.grdFailed.AllowUserToAddRows = false;
            this.grdFailed.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grdFailed, "grdFailed");
            this.grdFailed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFailed.Name = "grdFailed";
            this.grdFailed.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // PackageUploadResultDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.grdFailed);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.lblSkipped);
            this.Controls.Add(this.lblFailed);
            this.Controls.Add(this.lblSucceeded);
            this.Name = "PackageUploadResultDialog";
            ((System.ComponentModel.ISupportInitialize)(this.grdFailed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSucceeded;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.Label lblSkipped;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView grdFailed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}