
namespace Maestro.Base.UI
{
    partial class XYZUrlsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XYZUrlsDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.dgUrls = new System.Windows.Forms.DataGridView();
            this.COL_GROUP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COL_URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgUrls)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(499, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following groups are accessible from clients like OpenLayers/Leaflet/etc thro" +
    "ugh the following URLs";
            // 
            // dgUrls
            // 
            this.dgUrls.AllowUserToAddRows = false;
            this.dgUrls.AllowUserToDeleteRows = false;
            this.dgUrls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgUrls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUrls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_GROUP,
            this.COL_URL});
            this.dgUrls.Location = new System.Drawing.Point(16, 119);
            this.dgUrls.Name = "dgUrls";
            this.dgUrls.ReadOnly = true;
            this.dgUrls.Size = new System.Drawing.Size(772, 230);
            this.dgUrls.TabIndex = 1;
            // 
            // COL_GROUP
            // 
            this.COL_GROUP.DataPropertyName = "Name";
            this.COL_GROUP.HeaderText = "Base Layer Group";
            this.COL_GROUP.Name = "COL_GROUP";
            this.COL_GROUP.ReadOnly = true;
            this.COL_GROUP.Width = 150;
            // 
            // COL_URL
            // 
            this.COL_URL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.COL_URL.DataPropertyName = "UrlTemplate";
            this.COL_URL.HeaderText = "URL Template";
            this.COL_URL.Name = "COL_URL";
            this.COL_URL.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(775, 50);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(713, 355);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // XYZUrlsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgUrls);
            this.Controls.Add(this.label1);
            this.Name = "XYZUrlsDialog";
            this.Text = "XYZ Tileset URLs";
            ((System.ComponentModel.ISupportInitialize)(this.dgUrls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgUrls;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_GROUP;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_URL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
    }
}