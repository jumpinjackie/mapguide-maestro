namespace Maestro.Packaging
{
    partial class PackageUploadOptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageUploadOptionDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.rdTransactional = new System.Windows.Forms.RadioButton();
            this.rdNonTransactional = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rdTransactional
            // 
            resources.ApplyResources(this.rdTransactional, "rdTransactional");
            this.rdTransactional.Checked = true;
            this.rdTransactional.Name = "rdTransactional";
            this.rdTransactional.TabStop = true;
            this.rdTransactional.UseVisualStyleBackColor = true;
            // 
            // rdNonTransactional
            // 
            resources.ApplyResources(this.rdNonTransactional, "rdNonTransactional");
            this.rdNonTransactional.Name = "rdNonTransactional";
            this.rdNonTransactional.UseVisualStyleBackColor = true;
            // 
            // PackageUploadOptionDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.rdNonTransactional);
            this.Controls.Add(this.rdTransactional);
            this.Controls.Add(this.btnOK);
            this.Name = "PackageUploadOptionDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rdTransactional;
        private System.Windows.Forms.RadioButton rdNonTransactional;
    }
}