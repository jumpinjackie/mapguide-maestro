namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    partial class ItemStyle
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
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.EditButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // previewPicture
            // 
            this.previewPicture.BackColor = System.Drawing.Color.White;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPicture.Location = new System.Drawing.Point(0, 0);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(113, 20);
            this.previewPicture.TabIndex = 0;
            this.previewPicture.TabStop = false;
            this.previewPicture.DoubleClick += new System.EventHandler(this.EditButton_Click);
            this.previewPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // EditButton
            // 
            this.EditButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.EditButton.Location = new System.Drawing.Point(113, 0);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(24, 20);
            this.EditButton.TabIndex = 1;
            this.EditButton.Text = "...";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // ItemStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewPicture);
            this.Controls.Add(this.EditButton);
            this.Name = "ItemStyle";
            this.Size = new System.Drawing.Size(137, 20);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.Button EditButton;
    }
}
