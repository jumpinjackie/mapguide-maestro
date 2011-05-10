namespace DwfInspector
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInspect = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnBrowseDwf = new System.Windows.Forms.Button();
            this.btnBrowseDs = new System.Windows.Forms.Button();
            this.txtDwfPath = new System.Windows.Forms.TextBox();
            this.txtDrawingSource = new System.Windows.Forms.TextBox();
            this.rdDwfFile = new System.Windows.Forms.RadioButton();
            this.rdDrawingSource = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstSections = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.grpSectionDetails = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnInspect);
            this.groupBox1.Controls.Add(this.btnUpload);
            this.groupBox1.Controls.Add(this.btnBrowseDwf);
            this.groupBox1.Controls.Add(this.btnBrowseDs);
            this.groupBox1.Controls.Add(this.txtDwfPath);
            this.groupBox1.Controls.Add(this.txtDrawingSource);
            this.groupBox1.Controls.Add(this.rdDwfFile);
            this.groupBox1.Controls.Add(this.rdDrawingSource);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(874, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DWF File";
            // 
            // btnInspect
            // 
            this.btnInspect.Location = new System.Drawing.Point(446, 26);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(55, 23);
            this.btnInspect.TabIndex = 1;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.UseVisualStyleBackColor = true;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(446, 50);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(55, 23);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnBrowseDwf
            // 
            this.btnBrowseDwf.Location = new System.Drawing.Point(385, 50);
            this.btnBrowseDwf.Name = "btnBrowseDwf";
            this.btnBrowseDwf.Size = new System.Drawing.Size(55, 23);
            this.btnBrowseDwf.TabIndex = 5;
            this.btnBrowseDwf.Text = "Browse";
            this.btnBrowseDwf.UseVisualStyleBackColor = true;
            this.btnBrowseDwf.Click += new System.EventHandler(this.btnBrowseDwf_Click);
            // 
            // btnBrowseDs
            // 
            this.btnBrowseDs.Location = new System.Drawing.Point(385, 26);
            this.btnBrowseDs.Name = "btnBrowseDs";
            this.btnBrowseDs.Size = new System.Drawing.Size(55, 23);
            this.btnBrowseDs.TabIndex = 4;
            this.btnBrowseDs.Text = "Browse";
            this.btnBrowseDs.UseVisualStyleBackColor = true;
            this.btnBrowseDs.Click += new System.EventHandler(this.btnBrowseDs_Click);
            // 
            // txtDwfPath
            // 
            this.txtDwfPath.Location = new System.Drawing.Point(140, 52);
            this.txtDwfPath.Name = "txtDwfPath";
            this.txtDwfPath.ReadOnly = true;
            this.txtDwfPath.Size = new System.Drawing.Size(238, 20);
            this.txtDwfPath.TabIndex = 3;
            // 
            // txtDrawingSource
            // 
            this.txtDrawingSource.Location = new System.Drawing.Point(140, 28);
            this.txtDrawingSource.Name = "txtDrawingSource";
            this.txtDrawingSource.ReadOnly = true;
            this.txtDrawingSource.Size = new System.Drawing.Size(238, 20);
            this.txtDrawingSource.TabIndex = 2;
            // 
            // rdDwfFile
            // 
            this.rdDwfFile.AutoSize = true;
            this.rdDwfFile.Location = new System.Drawing.Point(18, 53);
            this.rdDwfFile.Name = "rdDwfFile";
            this.rdDwfFile.Size = new System.Drawing.Size(106, 17);
            this.rdDwfFile.TabIndex = 1;
            this.rdDwfFile.TabStop = true;
            this.rdDwfFile.Text = "Upload DWF File";
            this.rdDwfFile.UseVisualStyleBackColor = true;
            this.rdDwfFile.CheckedChanged += new System.EventHandler(this.rdDrawingSource_CheckedChanged);
            // 
            // rdDrawingSource
            // 
            this.rdDrawingSource.AutoSize = true;
            this.rdDrawingSource.Location = new System.Drawing.Point(18, 29);
            this.rdDrawingSource.Name = "rdDrawingSource";
            this.rdDrawingSource.Size = new System.Drawing.Size(101, 17);
            this.rdDrawingSource.TabIndex = 0;
            this.rdDrawingSource.TabStop = true;
            this.rdDrawingSource.Text = "Drawing Source";
            this.rdDrawingSource.UseVisualStyleBackColor = true;
            this.rdDrawingSource.CheckedChanged += new System.EventHandler(this.rdDrawingSource_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstSections);
            this.groupBox2.Location = new System.Drawing.Point(13, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 131);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DWF Sections";
            // 
            // lstSections
            // 
            this.lstSections.DisplayMember = "Name";
            this.lstSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSections.FormattingEnabled = true;
            this.lstSections.Location = new System.Drawing.Point(3, 16);
            this.lstSections.Name = "lstSections";
            this.lstSections.Size = new System.Drawing.Size(162, 108);
            this.lstSections.TabIndex = 0;
            this.lstSections.SelectedIndexChanged += new System.EventHandler(this.lstSections_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lstLayers);
            this.groupBox3.Location = new System.Drawing.Point(13, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(168, 346);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DWF Layers";
            // 
            // lstLayers
            // 
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Location = new System.Drawing.Point(3, 16);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(162, 316);
            this.lstLayers.TabIndex = 0;
            // 
            // grpSectionDetails
            // 
            this.grpSectionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSectionDetails.Location = new System.Drawing.Point(188, 108);
            this.grpSectionDetails.Name = "grpSectionDetails";
            this.grpSectionDetails.Size = new System.Drawing.Size(699, 483);
            this.grpSectionDetails.TabIndex = 3;
            this.grpSectionDetails.TabStop = false;
            this.grpSectionDetails.Text = "Section Details";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 603);
            this.Controls.Add(this.grpSectionDetails);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "DwfInspector Example";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnBrowseDwf;
        private System.Windows.Forms.Button btnBrowseDs;
        private System.Windows.Forms.TextBox txtDwfPath;
        private System.Windows.Forms.TextBox txtDrawingSource;
        private System.Windows.Forms.RadioButton rdDwfFile;
        private System.Windows.Forms.RadioButton rdDrawingSource;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstSections;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.GroupBox grpSectionDetails;
    }
}

