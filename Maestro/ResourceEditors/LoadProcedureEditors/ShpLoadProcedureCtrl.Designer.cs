namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class ShpLoadProcedureCtrl
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.loadSettingsCtrl1 = new OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors.LoadSettingsCtrl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkConvertSdf = new System.Windows.Forms.CheckBox();
            this.btnBrowseCS = new System.Windows.Forms.Button();
            this.numGeneralize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefaultCs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sourceFilesCtrl1 = new OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors.SourceFilesCtrl();
            this.lblNote = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.loadSettingsCtrl1);
            this.groupBox3.Location = new System.Drawing.Point(3, 275);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(527, 121);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Load Settings";
            // 
            // loadSettingsCtrl1
            // 
            this.loadSettingsCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadSettingsCtrl1.Editor = null;
            this.loadSettingsCtrl1.FeatureSourceFolderName = "";
            this.loadSettingsCtrl1.FeatureSourceRootPath = "";
            this.loadSettingsCtrl1.LayerFolderName = "";
            this.loadSettingsCtrl1.LayerRootPath = "";
            this.loadSettingsCtrl1.LoadFeatureSources = false;
            this.loadSettingsCtrl1.LoadLayers = false;
            this.loadSettingsCtrl1.Location = new System.Drawing.Point(3, 16);
            this.loadSettingsCtrl1.Name = "loadSettingsCtrl1";
            this.loadSettingsCtrl1.ResourceRootPath = "";
            this.loadSettingsCtrl1.Size = new System.Drawing.Size(521, 102);
            this.loadSettingsCtrl1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblNote);
            this.groupBox2.Controls.Add(this.chkConvertSdf);
            this.groupBox2.Controls.Add(this.btnBrowseCS);
            this.groupBox2.Controls.Add(this.numGeneralize);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtDefaultCs);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(527, 130);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transformation";
            // 
            // chkConvertSdf
            // 
            this.chkConvertSdf.AutoSize = true;
            this.chkConvertSdf.Enabled = false;
            this.chkConvertSdf.Location = new System.Drawing.Point(340, 41);
            this.chkConvertSdf.Name = "chkConvertSdf";
            this.chkConvertSdf.Size = new System.Drawing.Size(99, 17);
            this.chkConvertSdf.TabIndex = 30;
            this.chkConvertSdf.Text = "Convert to SDF";
            this.chkConvertSdf.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCS
            // 
            this.btnBrowseCS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCS.Location = new System.Drawing.Point(495, 11);
            this.btnBrowseCS.Name = "btnBrowseCS";
            this.btnBrowseCS.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseCS.TabIndex = 29;
            this.btnBrowseCS.Text = "...";
            this.btnBrowseCS.UseVisualStyleBackColor = true;
            this.btnBrowseCS.Click += new System.EventHandler(this.btnBrowseCS_Click);
            // 
            // numGeneralize
            // 
            this.numGeneralize.Enabled = false;
            this.numGeneralize.Location = new System.Drawing.Point(275, 39);
            this.numGeneralize.Name = "numGeneralize";
            this.numGeneralize.Size = new System.Drawing.Size(58, 20);
            this.numGeneralize.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Generalize data by this percentage";
            // 
            // txtDefaultCs
            // 
            this.txtDefaultCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultCs.Location = new System.Drawing.Point(275, 13);
            this.txtDefaultCs.Name = "txtDefaultCs";
            this.txtDefaultCs.ReadOnly = true;
            this.txtDefaultCs.Size = new System.Drawing.Size(215, 20);
            this.txtDefaultCs.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Use the following Coordinate System (if none found)";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.sourceFilesCtrl1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(527, 130);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Files";
            // 
            // sourceFilesCtrl1
            // 
            this.sourceFilesCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceFilesCtrl1.FileFilter = global::OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.Strings.BasicCommand.DuplicateRenameError;
            this.sourceFilesCtrl1.Location = new System.Drawing.Point(3, 16);
            this.sourceFilesCtrl1.Name = "sourceFilesCtrl1";
            this.sourceFilesCtrl1.Size = new System.Drawing.Size(521, 111);
            this.sourceFilesCtrl1.SourceFiles = new string[0];
            this.sourceFilesCtrl1.TabIndex = 0;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(6, 72);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(311, 13);
            this.lblNote.TabIndex = 31;
            this.lblNote.Text = "Note: Disabled features are not supported by Maestro";
            // 
            // ShpLoadProcedureCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ShpLoadProcedureCtrl";
            this.Size = new System.Drawing.Size(533, 399);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private LoadSettingsCtrl loadSettingsCtrl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private SourceFilesCtrl sourceFilesCtrl1;
        private System.Windows.Forms.NumericUpDown numGeneralize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDefaultCs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkConvertSdf;
        protected System.Windows.Forms.Button btnBrowseCS;
        private System.Windows.Forms.Label lblNote;
    }
}
