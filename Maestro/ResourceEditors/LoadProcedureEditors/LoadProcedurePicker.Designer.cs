namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class LoadProcedurePicker
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
            this.label1 = new System.Windows.Forms.Label();
            this.rdSdf = new System.Windows.Forms.RadioButton();
            this.rdShp = new System.Windows.Forms.RadioButton();
            this.rdSqlite = new System.Windows.Forms.RadioButton();
            this.rdOther = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose the type of Load Procedure to create";
            // 
            // rdSdf
            // 
            this.rdSdf.AutoSize = true;
            this.rdSdf.Checked = true;
            this.rdSdf.Location = new System.Drawing.Point(43, 51);
            this.rdSdf.Name = "rdSdf";
            this.rdSdf.Size = new System.Drawing.Size(46, 17);
            this.rdSdf.TabIndex = 1;
            this.rdSdf.TabStop = true;
            this.rdSdf.Text = "SDF";
            this.rdSdf.UseVisualStyleBackColor = true;
            // 
            // rdShp
            // 
            this.rdShp.AutoSize = true;
            this.rdShp.Location = new System.Drawing.Point(43, 74);
            this.rdShp.Name = "rdShp";
            this.rdShp.Size = new System.Drawing.Size(47, 17);
            this.rdShp.TabIndex = 2;
            this.rdShp.Text = "SHP";
            this.rdShp.UseVisualStyleBackColor = true;
            // 
            // rdSqlite
            // 
            this.rdSqlite.AutoSize = true;
            this.rdSqlite.Location = new System.Drawing.Point(43, 97);
            this.rdSqlite.Name = "rdSqlite";
            this.rdSqlite.Size = new System.Drawing.Size(57, 17);
            this.rdSqlite.TabIndex = 3;
            this.rdSqlite.Text = "SQLite";
            this.rdSqlite.UseVisualStyleBackColor = true;
            // 
            // rdOther
            // 
            this.rdOther.AutoSize = true;
            this.rdOther.Location = new System.Drawing.Point(43, 120);
            this.rdOther.Name = "rdOther";
            this.rdOther.Size = new System.Drawing.Size(233, 17);
            this.rdOther.TabIndex = 4;
            this.rdOther.Text = "Other (will use XML Editor, may not execute)";
            this.rdOther.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(109, 165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // LoadProcedurePicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 200);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rdOther);
            this.Controls.Add(this.rdSqlite);
            this.Controls.Add(this.rdShp);
            this.Controls.Add(this.rdSdf);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadProcedurePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Procedure Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdSdf;
        private System.Windows.Forms.RadioButton rdShp;
        private System.Windows.Forms.RadioButton rdSqlite;
        private System.Windows.Forms.RadioButton rdOther;
        private System.Windows.Forms.Button btnOK;
    }
}
