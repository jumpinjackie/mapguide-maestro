namespace Maestro.Editors.Fusion.MapEditors
{
    partial class XYZEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdUrls = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddTileSet = new System.Windows.Forms.ToolStripButton();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUrls)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(15, 26);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(738, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tile URLs";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grdUrls);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(15, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 242);
            this.panel1.TabIndex = 3;
            // 
            // grdUrls
            // 
            this.grdUrls.AllowUserToDeleteRows = false;
            this.grdUrls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUrls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.URL});
            this.grdUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUrls.Location = new System.Drawing.Point(0, 25);
            this.grdUrls.Name = "grdUrls";
            this.grdUrls.Size = new System.Drawing.Size(738, 217);
            this.grdUrls.TabIndex = 1;
            this.grdUrls.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUrls_CellEndEdit);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddTileSet});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(738, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddTileSet
            // 
            this.btnAddTileSet.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.btnAddTileSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTileSet.Name = "btnAddTileSet";
            this.btnAddTileSet.Size = new System.Drawing.Size(198, 22);
            this.btnAddTileSet.Text = "Add from XYZ Tile Set Definition";
            this.btnAddTileSet.Click += new System.EventHandler(this.btnAddTileSet_Click);
            // 
            // URL
            // 
            this.URL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.URL.DataPropertyName = "URL";
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            // 
            // XYZEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "XYZEditor";
            this.Size = new System.Drawing.Size(765, 329);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUrls)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grdUrls;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddTileSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
    }
}
