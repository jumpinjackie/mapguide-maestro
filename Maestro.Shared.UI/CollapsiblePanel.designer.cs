namespace Maestro.Shared.UI
{
    partial class CollapsiblePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollapsiblePanel));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.headerPanel.Controls.Add(this.btnExpand);
            this.headerPanel.Controls.Add(this.btnCollapse);
            this.headerPanel.Controls.Add(this.lblHeaderText);
            resources.ApplyResources(this.headerPanel, "headerPanel");
            this.headerPanel.Name = "headerPanel";
            // 
            // btnExpand
            // 
            resources.ApplyResources(this.btnExpand, "btnExpand");
            this.btnExpand.BackgroundImage = global::Maestro.Shared.UI.Properties.Resources.plus_white;
            this.btnExpand.FlatAppearance.BorderSize = 0;
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnCollapse
            // 
            resources.ApplyResources(this.btnCollapse, "btnCollapse");
            this.btnCollapse.BackgroundImage = global::Maestro.Shared.UI.Properties.Resources.minus_white;
            this.btnCollapse.FlatAppearance.BorderSize = 0;
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.UseVisualStyleBackColor = true;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblHeaderText, "lblHeaderText");
            this.lblHeaderText.Name = "lblHeaderText";
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.contentPanel, "contentPanel");
            this.contentPanel.Name = "contentPanel";
            // 
            // CollapsiblePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "CollapsiblePanel";
            resources.ApplyResources(this, "$this");
            this.headerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnCollapse;
        public System.Windows.Forms.Panel contentPanel;
    }
}
