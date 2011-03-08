namespace Maestro.Editors.Diagnostics
{
    partial class FdoCacheViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FdoCacheViewer));
            this.btnClose = new System.Windows.Forms.Button();
            this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
            this.tmRefresh = new System.Windows.Forms.Timer(this.components);
            this.txtXml = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkAutoRefresh
            // 
            resources.ApplyResources(this.chkAutoRefresh, "chkAutoRefresh");
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.UseVisualStyleBackColor = true;
            this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.chkAutoRefresh_CheckedChanged);
            // 
            // tmRefresh
            // 
            this.tmRefresh.Interval = 5000;
            this.tmRefresh.Tick += new System.EventHandler(this.tmRefresh_Tick);
            // 
            // txtXml
            // 
            resources.ApplyResources(this.txtXml, "txtXml");
            this.txtXml.Name = "txtXml";
            this.txtXml.ReadOnly = true;
            // 
            // FdoCacheViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.txtXml);
            this.Controls.Add(this.chkAutoRefresh);
            this.Controls.Add(this.btnClose);
            this.Name = "FdoCacheViewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkAutoRefresh;
        private System.Windows.Forms.Timer tmRefresh;
        private System.Windows.Forms.TextBox txtXml;
    }
}