namespace Maestro.Editors.WebLayout.Commands
{
    partial class InvokeURLCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvokeURLCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBrowseLayers = new System.Windows.Forms.Button();
            this.txtFrame = new System.Windows.Forms.TextBox();
            this.cmbTargetFrame = new System.Windows.Forms.ComboBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grdParameters = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.chkDisableIfEmpty = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnBrowseLayers);
            this.panel1.Controls.Add(this.txtFrame);
            this.panel1.Controls.Add(this.cmbTargetFrame);
            this.panel1.Controls.Add(this.txtUrl);
            this.panel1.Controls.Add(this.lstLayers);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.grdParameters);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.chkDisableIfEmpty);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // btnBrowseLayers
            // 
            resources.ApplyResources(this.btnBrowseLayers, "btnBrowseLayers");
            this.btnBrowseLayers.Name = "btnBrowseLayers";
            this.btnBrowseLayers.UseVisualStyleBackColor = true;
            this.btnBrowseLayers.Click += new System.EventHandler(this.btnBrowseLayers_Click);
            // 
            // txtFrame
            // 
            resources.ApplyResources(this.txtFrame, "txtFrame");
            this.txtFrame.Name = "txtFrame";
            // 
            // cmbTargetFrame
            // 
            resources.ApplyResources(this.cmbTargetFrame, "cmbTargetFrame");
            this.cmbTargetFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetFrame.FormattingEnabled = true;
            this.cmbTargetFrame.Name = "cmbTargetFrame";
            // 
            // txtUrl
            // 
            resources.ApplyResources(this.txtUrl, "txtUrl");
            this.txtUrl.Name = "txtUrl";
            // 
            // lstLayers
            // 
            resources.ApplyResources(this.lstLayers, "lstLayers");
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Name = "lstLayers";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // grdParameters
            // 
            resources.ApplyResources(this.grdParameters, "grdParameters");
            this.grdParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdParameters.Name = "grdParameters";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkDisableIfEmpty
            // 
            resources.ApplyResources(this.chkDisableIfEmpty, "chkDisableIfEmpty");
            this.chkDisableIfEmpty.Name = "chkDisableIfEmpty";
            this.chkDisableIfEmpty.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // InvokeURLCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "InvokeURLCtrl";
            resources.ApplyResources(this, "$this");
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdParameters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView grdParameters;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkDisableIfEmpty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFrame;
        private System.Windows.Forms.ComboBox cmbTargetFrame;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnBrowseLayers;
    }
}
