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
            grpBingMapsKey = new System.Windows.Forms.GroupBox();
            txtBingMapsApiKey = new System.Windows.Forms.TextBox();
            btnSetBingMapsApiKey = new System.Windows.Forms.Button();
            grpBingMapsKey.SuspendLayout();
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
            // grpBingMapsKey
            // 
            grpBingMapsKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpBingMapsKey.Controls.Add(txtBingMapsApiKey);
            grpBingMapsKey.Controls.Add(btnSetBingMapsApiKey);
            grpBingMapsKey.Location = new System.Drawing.Point(14, 114);
            grpBingMapsKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpBingMapsKey.Name = "grpBingMapsKey";
            grpBingMapsKey.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpBingMapsKey.Size = new System.Drawing.Size(448, 104);
            grpBingMapsKey.TabIndex = 11;
            grpBingMapsKey.TabStop = false;
            grpBingMapsKey.Text = "Bing Maps API key";
            // 
            // txtBingMapsApiKey
            // 
            txtBingMapsApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtBingMapsApiKey.Location = new System.Drawing.Point(7, 21);
            txtBingMapsApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBingMapsApiKey.Multiline = true;
            txtBingMapsApiKey.Name = "txtBingMapsApiKey";
            txtBingMapsApiKey.Size = new System.Drawing.Size(433, 39);
            txtBingMapsApiKey.TabIndex = 8;
            txtBingMapsApiKey.TextChanged += txtBingMapsApiKey_TextChanged;
            // 
            // btnSetBingMapsApiKey
            // 
            btnSetBingMapsApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSetBingMapsApiKey.Enabled = false;
            btnSetBingMapsApiKey.Location = new System.Drawing.Point(354, 67);
            btnSetBingMapsApiKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSetBingMapsApiKey.Name = "btnSetBingMapsApiKey";
            btnSetBingMapsApiKey.Size = new System.Drawing.Size(88, 27);
            btnSetBingMapsApiKey.TabIndex = 9;
            btnSetBingMapsApiKey.Text = "Set API Key";
            btnSetBingMapsApiKey.UseVisualStyleBackColor = true;
            btnSetBingMapsApiKey.Click += btnSetBingMapsApiKey_Click;
            // 
            // CommercialMapEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpBingMapsKey);
            Controls.Add(txtSubType);
            Controls.Add(txtType);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CommercialMapEditor";
            Size = new System.Drawing.Size(486, 252);
            grpBingMapsKey.ResumeLayout(false);
            grpBingMapsKey.PerformLayout();
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
        private System.Windows.Forms.GroupBox grpBingMapsKey;
        private System.Windows.Forms.TextBox txtBingMapsApiKey;
        private System.Windows.Forms.Button btnSetBingMapsApiKey;
    }
}
