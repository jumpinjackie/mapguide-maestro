namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class LoadProcedureCtrl
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
            this.childPanel = new System.Windows.Forms.Panel();
            this.btnLoadResources = new System.Windows.Forms.Button();
            this.btnListAffected = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // childPanel
            // 
            this.childPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.childPanel.AutoScroll = true;
            this.childPanel.Location = new System.Drawing.Point(4, 4);
            this.childPanel.Name = "childPanel";
            this.childPanel.Size = new System.Drawing.Size(573, 424);
            this.childPanel.TabIndex = 0;
            // 
            // btnLoadResources
            // 
            this.btnLoadResources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadResources.Location = new System.Drawing.Point(16, 443);
            this.btnLoadResources.Name = "btnLoadResources";
            this.btnLoadResources.Size = new System.Drawing.Size(138, 23);
            this.btnLoadResources.TabIndex = 1;
            this.btnLoadResources.Text = "Load Resources";
            this.btnLoadResources.UseVisualStyleBackColor = true;
            this.btnLoadResources.Click += new System.EventHandler(this.btnLoadResources_Click);
            // 
            // btnListAffected
            // 
            this.btnListAffected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnListAffected.Location = new System.Drawing.Point(160, 443);
            this.btnListAffected.Name = "btnListAffected";
            this.btnListAffected.Size = new System.Drawing.Size(138, 23);
            this.btnListAffected.TabIndex = 2;
            this.btnListAffected.Text = "List Affected Resources";
            this.btnListAffected.UseVisualStyleBackColor = true;
            this.btnListAffected.Click += new System.EventHandler(this.btnListAffected_Click);
            // 
            // LoadProcedureCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnListAffected);
            this.Controls.Add(this.btnLoadResources);
            this.Controls.Add(this.childPanel);
            this.Name = "LoadProcedureCtrl";
            this.Size = new System.Drawing.Size(580, 481);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel childPanel;
        private System.Windows.Forms.Button btnLoadResources;
        private System.Windows.Forms.Button btnListAffected;
    }
}
