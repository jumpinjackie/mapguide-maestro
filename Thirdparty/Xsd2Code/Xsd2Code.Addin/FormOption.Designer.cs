namespace Xsd2Code.Addin
{
    partial class FormOption
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSetAsDefault = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.linkToCodePlex = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSetAsDefault);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnGenerate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 570);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 32);
            this.panel1.TabIndex = 2;
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.Location = new System.Drawing.Point(15, 6);
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(83, 23);
            this.btnSetAsDefault.TabIndex = 4;
            this.btnSetAsDefault.Text = "Set as default";
            this.btnSetAsDefault.UseVisualStyleBackColor = true;
            this.btnSetAsDefault.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(359, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(282, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(73, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Location = new System.Drawing.Point(12, 12);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(414, 535);
            this.propertyGrid.TabIndex = 19;
            // 
            // linkToCodePlex
            // 
            this.linkToCodePlex.AutoSize = true;
            this.linkToCodePlex.Location = new System.Drawing.Point(12, 550);
            this.linkToCodePlex.Name = "linkToCodePlex";
            this.linkToCodePlex.Size = new System.Drawing.Size(185, 13);
            this.linkToCodePlex.TabIndex = 20;
            this.linkToCodePlex.TabStop = true;
            this.linkToCodePlex.Text = "http://www.codeplex.com/Xsd2Code";
            this.linkToCodePlex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkToCodePlex_LinkClicked);
            // 
            // FormOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(438, 602);
            this.Controls.Add(this.linkToCodePlex);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormOption";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xsd2Code class generator";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormOption_KeyPress);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button btnSetAsDefault;
        private System.Windows.Forms.LinkLabel linkToCodePlex;

    }
}