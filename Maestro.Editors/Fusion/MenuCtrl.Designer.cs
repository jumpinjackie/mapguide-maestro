namespace Maestro.Editors.Fusion
{
    partial class MenuCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMenuLabel = new System.Windows.Forms.TextBox();
            this.txtTooltip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImageClass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMenuLabel
            // 
            resources.ApplyResources(this.txtMenuLabel, "txtMenuLabel");
            this.txtMenuLabel.Name = "txtMenuLabel";
            this.txtMenuLabel.TextChanged += new System.EventHandler(this.txtMenuLabel_TextChanged);
            // 
            // txtTooltip
            // 
            resources.ApplyResources(this.txtTooltip, "txtTooltip");
            this.txtTooltip.Name = "txtTooltip";
            this.txtTooltip.TextChanged += new System.EventHandler(this.txtTooltip_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtImageUrl
            // 
            resources.ApplyResources(this.txtImageUrl, "txtImageUrl");
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.TextChanged += new System.EventHandler(this.txtImageUrl_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtImageClass
            // 
            resources.ApplyResources(this.txtImageClass, "txtImageClass");
            this.txtImageClass.Name = "txtImageClass";
            this.txtImageClass.TextChanged += new System.EventHandler(this.txtImageClass_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // MenuCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtImageClass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtImageUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTooltip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMenuLabel);
            this.Controls.Add(this.label1);
            this.Name = "MenuCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMenuLabel;
        private System.Windows.Forms.TextBox txtTooltip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtImageClass;
        private System.Windows.Forms.Label label4;
    }
}
