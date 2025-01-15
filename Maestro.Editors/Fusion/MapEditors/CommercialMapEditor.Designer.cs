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
            label1 = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtType = new System.Windows.Forms.TextBox();
            txtSubType = new System.Windows.Forms.TextBox();
            grpApiKey = new System.Windows.Forms.GroupBox();
            txtApiKey = new System.Windows.Forms.TextBox();
            btnSetApiKey = new System.Windows.Forms.Button();
            grpApiKey.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 50);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // txtName
            // 
            txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtName.Location = new System.Drawing.Point(99, 46);
            txtName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(363, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(22, 80);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 15);
            label2.TabIndex = 2;
            label2.Text = "Sub-Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(22, 17);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(31, 15);
            label3.TabIndex = 4;
            label3.Text = "Type";
            // 
            // txtType
            // 
            txtType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtType.Location = new System.Drawing.Point(99, 14);
            txtType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtType.Name = "txtType";
            txtType.ReadOnly = true;
            txtType.Size = new System.Drawing.Size(205, 23);
            txtType.TabIndex = 5;
            // 
            // txtSubType
            // 
            txtSubType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSubType.Location = new System.Drawing.Point(99, 76);
            txtSubType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSubType.Name = "txtSubType";
            txtSubType.ReadOnly = true;
            txtSubType.Size = new System.Drawing.Size(363, 23);
            txtSubType.TabIndex = 6;
            // 
            // grpApiKey
            // 
            grpApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpApiKey.Controls.Add(txtApiKey);
            grpApiKey.Controls.Add(btnSetApiKey);
            grpApiKey.Location = new System.Drawing.Point(14, 114);
            grpApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpApiKey.Name = "grpApiKey";
            grpApiKey.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpApiKey.Size = new System.Drawing.Size(448, 104);
            grpApiKey.TabIndex = 11;
            grpApiKey.TabStop = false;
            // 
            // txtApiKey
            // 
            txtApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtApiKey.Location = new System.Drawing.Point(7, 21);
            txtApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtApiKey.Multiline = true;
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new System.Drawing.Size(433, 39);
            txtApiKey.TabIndex = 8;
            txtApiKey.TextChanged += txtApiKey_TextChanged;
            // 
            // btnSetBingMapsApiKey
            // 
            btnSetApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSetApiKey.Enabled = false;
            btnSetApiKey.Location = new System.Drawing.Point(354, 67);
            btnSetApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSetApiKey.Name = "btnSetBingMapsApiKey";
            btnSetApiKey.Size = new System.Drawing.Size(88, 27);
            btnSetApiKey.TabIndex = 9;
            btnSetApiKey.Text = "Set API Key";
            btnSetApiKey.UseVisualStyleBackColor = true;
            btnSetApiKey.Click += btnSetApiKey_Click;
            // 
            // CommercialMapEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpApiKey);
            Controls.Add(txtSubType);
            Controls.Add(txtType);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CommercialMapEditor";
            Size = new System.Drawing.Size(486, 252);
            grpApiKey.ResumeLayout(false);
            grpApiKey.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtSubType;
        private System.Windows.Forms.GroupBox grpApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Button btnSetApiKey;
    }
}
