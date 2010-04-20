namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    partial class SdfLoadProcedureCtrl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sourceFilesCtrl1 = new OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors.SourceFilesCtrl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.cmbSdfDuplicateStrategy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numGeneralize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseCS = new System.Windows.Forms.Button();
            this.txtDefaultCs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.loadSettingsCtrl1 = new OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors.LoadSettingsCtrl();
            this.lblNote2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralize)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.sourceFilesCtrl1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Files";
            // 
            // sourceFilesCtrl1
            // 
            this.sourceFilesCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceFilesCtrl1.FileFilter = global::OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.Strings.BasicCommand.DuplicateRenameError;
            this.sourceFilesCtrl1.Location = new System.Drawing.Point(3, 16);
            this.sourceFilesCtrl1.Name = "sourceFilesCtrl1";
            this.sourceFilesCtrl1.Size = new System.Drawing.Size(586, 111);
            this.sourceFilesCtrl1.SourceFiles = new string[0];
            this.sourceFilesCtrl1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblNote2);
            this.groupBox2.Controls.Add(this.lblNote);
            this.groupBox2.Controls.Add(this.cmbSdfDuplicateStrategy);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numGeneralize);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnBrowseCS);
            this.groupBox2.Controls.Add(this.txtDefaultCs);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(592, 142);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transformation";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(6, 98);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(311, 13);
            this.lblNote.TabIndex = 27;
            this.lblNote.Text = "Note: Disabled features are not supported by Maestro";
            // 
            // cmbSdfDuplicateStrategy
            // 
            this.cmbSdfDuplicateStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSdfDuplicateStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSdfDuplicateStrategy.Enabled = false;
            this.cmbSdfDuplicateStrategy.FormattingEnabled = true;
            this.cmbSdfDuplicateStrategy.Location = new System.Drawing.Point(275, 74);
            this.cmbSdfDuplicateStrategy.Name = "cmbSdfDuplicateStrategy";
            this.cmbSdfDuplicateStrategy.Size = new System.Drawing.Size(311, 21);
            this.cmbSdfDuplicateStrategy.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "For SDF files that contain records with the same key";
            // 
            // numGeneralize
            // 
            this.numGeneralize.Enabled = false;
            this.numGeneralize.Location = new System.Drawing.Point(275, 48);
            this.numGeneralize.Name = "numGeneralize";
            this.numGeneralize.Size = new System.Drawing.Size(58, 20);
            this.numGeneralize.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Generalize data by this percentage";
            // 
            // btnBrowseCS
            // 
            this.btnBrowseCS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCS.Location = new System.Drawing.Point(560, 20);
            this.btnBrowseCS.Name = "btnBrowseCS";
            this.btnBrowseCS.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseCS.TabIndex = 22;
            this.btnBrowseCS.Text = "...";
            this.btnBrowseCS.UseVisualStyleBackColor = true;
            this.btnBrowseCS.Click += new System.EventHandler(this.btnBrowseCS_Click);
            // 
            // txtDefaultCs
            // 
            this.txtDefaultCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultCs.Location = new System.Drawing.Point(275, 22);
            this.txtDefaultCs.Name = "txtDefaultCs";
            this.txtDefaultCs.ReadOnly = true;
            this.txtDefaultCs.Size = new System.Drawing.Size(281, 20);
            this.txtDefaultCs.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use the following Coordinate System (if none found)";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.loadSettingsCtrl1);
            this.groupBox3.Location = new System.Drawing.Point(3, 287);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 121);
            this.groupBox3.TabIndex = 2;
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
            this.loadSettingsCtrl1.Size = new System.Drawing.Size(586, 102);
            this.loadSettingsCtrl1.TabIndex = 0;
            // 
            // lblNote2
            // 
            this.lblNote2.AutoSize = true;
            this.lblNote2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote2.Location = new System.Drawing.Point(6, 117);
            this.lblNote2.Name = "lblNote2";
            this.lblNote2.Size = new System.Drawing.Size(497, 13);
            this.lblNote2.TabIndex = 28;
            this.lblNote2.Text = "Note: Do not use SDF2 files. Maestro does not support conversion from SDF2 to SDF" +
                "3";
            // 
            // SdfLoadProcedureCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SdfLoadProcedureCtrl";
            this.Size = new System.Drawing.Size(598, 411);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGeneralize)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private SourceFilesCtrl sourceFilesCtrl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private LoadSettingsCtrl loadSettingsCtrl1;
        private System.Windows.Forms.TextBox txtDefaultCs;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button btnBrowseCS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numGeneralize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSdfDuplicateStrategy;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblNote2;
    }
}
