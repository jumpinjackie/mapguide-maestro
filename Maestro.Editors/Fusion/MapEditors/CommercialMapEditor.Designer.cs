namespace Maestro.Editors.Fusion.MapEditors
{
    partial class CommercialMapEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.txtSubType = new System.Windows.Forms.TextBox();
            this.txtGoogleMapsApiKey = new System.Windows.Forms.TextBox();
            this.btnSetApiKey = new System.Windows.Forms.Button();
            this.grpGoogleApiKey = new System.Windows.Forms.GroupBox();
            this.grpGoogleApiKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(85, 40);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(312, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sub-Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type";
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.Location = new System.Drawing.Point(85, 12);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(176, 20);
            this.txtType.TabIndex = 5;
            // 
            // txtSubType
            // 
            this.txtSubType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubType.Location = new System.Drawing.Point(85, 66);
            this.txtSubType.Name = "txtSubType";
            this.txtSubType.ReadOnly = true;
            this.txtSubType.Size = new System.Drawing.Size(312, 20);
            this.txtSubType.TabIndex = 6;
            // 
            // txtGoogleMapsApiKey
            // 
            this.txtGoogleMapsApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGoogleMapsApiKey.Location = new System.Drawing.Point(6, 18);
            this.txtGoogleMapsApiKey.Multiline = true;
            this.txtGoogleMapsApiKey.Name = "txtGoogleMapsApiKey";
            this.txtGoogleMapsApiKey.Size = new System.Drawing.Size(372, 34);
            this.txtGoogleMapsApiKey.TabIndex = 8;
            this.txtGoogleMapsApiKey.TextChanged += new System.EventHandler(this.txtGoogleMapsApiKey_TextChanged);
            // 
            // btnSetApiKey
            // 
            this.btnSetApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetApiKey.Enabled = false;
            this.btnSetApiKey.Location = new System.Drawing.Point(303, 58);
            this.btnSetApiKey.Name = "btnSetApiKey";
            this.btnSetApiKey.Size = new System.Drawing.Size(75, 23);
            this.btnSetApiKey.TabIndex = 9;
            this.btnSetApiKey.Text = "Set API Key";
            this.btnSetApiKey.UseVisualStyleBackColor = true;
            this.btnSetApiKey.Click += new System.EventHandler(this.btnSetApiKey_Click);
            // 
            // grpGoogleApiKey
            // 
            this.grpGoogleApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGoogleApiKey.Controls.Add(this.txtGoogleMapsApiKey);
            this.grpGoogleApiKey.Controls.Add(this.btnSetApiKey);
            this.grpGoogleApiKey.Location = new System.Drawing.Point(13, 104);
            this.grpGoogleApiKey.Name = "grpGoogleApiKey";
            this.grpGoogleApiKey.Size = new System.Drawing.Size(384, 90);
            this.grpGoogleApiKey.TabIndex = 10;
            this.grpGoogleApiKey.TabStop = false;
            this.grpGoogleApiKey.Text = "Google Maps API key";
            // 
            // CommercialMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpGoogleApiKey);
            this.Controls.Add(this.txtSubType);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "CommercialMapEditor";
            this.Size = new System.Drawing.Size(417, 280);
            this.grpGoogleApiKey.ResumeLayout(false);
            this.grpGoogleApiKey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtSubType;
        private System.Windows.Forms.TextBox txtGoogleMapsApiKey;
        private System.Windows.Forms.Button btnSetApiKey;
        private System.Windows.Forms.GroupBox grpGoogleApiKey;
    }
}
