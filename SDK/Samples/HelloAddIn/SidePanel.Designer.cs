namespace HelloAddIn
{
    partial class SidePanel
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
            this.btnOpenEditor = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 117);
            this.label1.TabIndex = 0;
            this.label1.Text = "If you can see this sidebar (and the \"Hello AddIn\" menu), it means this AddIn was" +
    " successfully loaded into Maestro :)";
            // 
            // btnOpenEditor
            // 
            this.btnOpenEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenEditor.Location = new System.Drawing.Point(26, 245);
            this.btnOpenEditor.Name = "btnOpenEditor";
            this.btnOpenEditor.Size = new System.Drawing.Size(199, 23);
            this.btnOpenEditor.TabIndex = 1;
            this.btnOpenEditor.Text = "Edit Selected Item";
            this.btnOpenEditor.UseVisualStyleBackColor = true;
            this.btnOpenEditor.Click += new System.EventHandler(this.btnOpenEditor_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.Location = new System.Drawing.Point(26, 274);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(199, 23);
            this.btnValidate.TabIndex = 2;
            this.btnValidate.Text = "Validate Selected Item";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(26, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 55);
            this.label2.TabIndex = 3;
            this.label2.Text = "An assortment of various examples demonstrating host application interaction";
            // 
            // SidePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnOpenEditor);
            this.Controls.Add(this.label1);
            this.Name = "SidePanel";
            this.Size = new System.Drawing.Size(251, 564);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenEditor;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Label label2;
    }
}
