namespace Maestro.Editors.Fusion
{
    partial class ManageCustomProjectionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageCustomProjectionsDialog));
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnAddAll = new System.Windows.Forms.ToolStripButton();
            btnGetDefn = new System.Windows.Forms.ToolStripButton();
            panel1 = new System.Windows.Forms.Panel();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            btnApplyAndClose = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            grdProjections = new System.Windows.Forms.DataGridView();
            COL_EPSG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            COL_DEFN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdProjections).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnAddAll, btnGetDefn });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(855, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnAddAll
            // 
            btnAddAll.Image = Properties.Resources.globe__plus;
            btnAddAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAddAll.Name = "btnAddAll";
            btnAddAll.Size = new System.Drawing.Size(157, 22);
            btnAddAll.Text = "Auto-add All Projections";
            btnAddAll.ToolTipText = "Automatically add all projections from maps referenced in this resource";
            btnAddAll.Click += btnAddAll_Click;
            // 
            // btnGetDefn
            // 
            btnGetDefn.Enabled = false;
            btnGetDefn.Image = Properties.Resources.drive_download;
            btnGetDefn.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnGetDefn.Name = "btnGetDefn";
            btnGetDefn.Size = new System.Drawing.Size(129, 22);
            btnGetDefn.Text = "Get proj4 definition";
            btnGetDefn.Click += btnGetDefn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btnApplyAndClose);
            panel1.Controls.Add(btnCancel);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 399);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(855, 117);
            panel1.TabIndex = 1;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(181, 86);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(85, 15);
            linkLabel1.TabIndex = 5;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://epsg.io";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 86);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(154, 15);
            label1.TabIndex = 4;
            label1.Text = "proj4 definition lookups use";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(12, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(617, 72);
            label2.TabIndex = 3;
            label2.Text = resources.GetString("label2.Text");
            // 
            // btnApplyAndClose
            // 
            btnApplyAndClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnApplyAndClose.Location = new System.Drawing.Point(655, 82);
            btnApplyAndClose.Name = "btnApplyAndClose";
            btnApplyAndClose.Size = new System.Drawing.Size(107, 23);
            btnApplyAndClose.TabIndex = 1;
            btnApplyAndClose.Text = "Apply and Close";
            btnApplyAndClose.UseVisualStyleBackColor = true;
            btnApplyAndClose.Click += btnApplyAndClose_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(768, 82);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // grdProjections
            // 
            grdProjections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdProjections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { COL_EPSG, COL_DEFN });
            grdProjections.Dock = System.Windows.Forms.DockStyle.Fill;
            grdProjections.Location = new System.Drawing.Point(0, 25);
            grdProjections.Name = "grdProjections";
            grdProjections.RowTemplate.Height = 25;
            grdProjections.Size = new System.Drawing.Size(855, 374);
            grdProjections.TabIndex = 2;
            grdProjections.SelectionChanged += grdProjections_SelectionChanged;
            // 
            // COL_EPSG
            // 
            COL_EPSG.DataPropertyName = "epsg";
            COL_EPSG.HeaderText = "EPSG code";
            COL_EPSG.Name = "COL_EPSG";
            // 
            // COL_DEFN
            // 
            COL_DEFN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            COL_DEFN.DataPropertyName = "Value";
            COL_DEFN.HeaderText = "proj4 definition";
            COL_DEFN.Name = "COL_DEFN";
            // 
            // ManageCustomProjectionsDialog
            // 
            AcceptButton = btnApplyAndClose;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(855, 516);
            ControlBox = false;
            Controls.Add(grdProjections);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Name = "ManageCustomProjectionsDialog";
            Text = "Custom Projections";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grdProjections).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grdProjections;
        private System.Windows.Forms.Button btnApplyAndClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_EPSG;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_DEFN;
        private System.Windows.Forms.ToolStripButton btnGetDefn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
    }
}