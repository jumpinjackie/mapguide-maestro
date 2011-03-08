namespace Maestro.Editors.DrawingSource
{
    partial class SourceSectionCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceSectionCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourceCs = new System.Windows.Forms.TextBox();
            this.btnBrowseCs = new System.Windows.Forms.Button();
            this.resDataCtrl = new Maestro.Editors.Common.ResourceDataCtrl();
            this.label2 = new System.Windows.Forms.Label();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.resDataCtrl);
            this.contentPanel.Controls.Add(this.btnBrowseCs);
            this.contentPanel.Controls.Add(this.txtSourceCs);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtSourceCs
            // 
            resources.ApplyResources(this.txtSourceCs, "txtSourceCs");
            this.txtSourceCs.Name = "txtSourceCs";
            // 
            // btnBrowseCs
            // 
            resources.ApplyResources(this.btnBrowseCs, "btnBrowseCs");
            this.btnBrowseCs.Name = "btnBrowseCs";
            this.btnBrowseCs.UseVisualStyleBackColor = true;
            this.btnBrowseCs.Click += new System.EventHandler(this.btnBrowseCs_Click);
            // 
            // resDataCtrl
            // 
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.MarkedFile = "";
            this.resDataCtrl.MarkEnabled = true;
            this.resDataCtrl.Name = "resDataCtrl";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // SourceSectionCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "DWF Drawing Source";
            this.Name = "SourceSectionCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceCs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseCs;
        private Maestro.Editors.Common.ResourceDataCtrl resDataCtrl;
        private System.Windows.Forms.Label label2;

    }
}
