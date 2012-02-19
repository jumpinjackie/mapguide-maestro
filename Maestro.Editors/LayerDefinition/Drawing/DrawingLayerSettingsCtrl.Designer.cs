namespace Maestro.Editors.LayerDefinition.Drawing
{
    partial class DrawingLayerSettingsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingLayerSettingsCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDrawingSource = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkCheckAll = new System.Windows.Forms.LinkLabel();
            this.chkListDwfLayers = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaxScale = new System.Windows.Forms.TextBox();
            this.txtMinScale = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSheet = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGoToDrawingSource = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnGoToDrawingSource);
            this.contentPanel.Controls.Add(this.groupBox1);
            this.contentPanel.Controls.Add(this.btnBrowse);
            this.contentPanel.Controls.Add(this.txtDrawingSource);
            this.contentPanel.Controls.Add(this.label1);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtDrawingSource
            // 
            resources.ApplyResources(this.txtDrawingSource, "txtDrawingSource");
            this.txtDrawingSource.Name = "txtDrawingSource";
            this.txtDrawingSource.ReadOnly = true;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lnkCheckAll);
            this.groupBox1.Controls.Add(this.chkListDwfLayers);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtMaxScale);
            this.groupBox1.Controls.Add(this.txtMinScale);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbSheet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lnkCheckAll
            // 
            resources.ApplyResources(this.lnkCheckAll, "lnkCheckAll");
            this.lnkCheckAll.Name = "lnkCheckAll";
            this.lnkCheckAll.TabStop = true;
            this.lnkCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCheckAll_LinkClicked);
            // 
            // chkListDwfLayers
            // 
            resources.ApplyResources(this.chkListDwfLayers, "chkListDwfLayers");
            this.chkListDwfLayers.FormattingEnabled = true;
            this.chkListDwfLayers.Name = "chkListDwfLayers";
            this.chkListDwfLayers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListDwfLayers_ItemCheck);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtMaxScale
            // 
            resources.ApplyResources(this.txtMaxScale, "txtMaxScale");
            this.txtMaxScale.Name = "txtMaxScale";
            // 
            // txtMinScale
            // 
            resources.ApplyResources(this.txtMinScale, "txtMinScale");
            this.txtMinScale.Name = "txtMinScale";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbSheet
            // 
            resources.ApplyResources(this.cmbSheet, "cmbSheet");
            this.cmbSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSheet.FormattingEnabled = true;
            this.cmbSheet.Name = "cmbSheet";
            this.cmbSheet.SelectedIndexChanged += new System.EventHandler(this.cmbSheet_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnGoToDrawingSource
            // 
            resources.ApplyResources(this.btnGoToDrawingSource, "btnGoToDrawingSource");
            this.btnGoToDrawingSource.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.btnGoToDrawingSource.Name = "btnGoToDrawingSource";
            this.btnGoToDrawingSource.UseVisualStyleBackColor = true;
            this.btnGoToDrawingSource.Click += new System.EventHandler(this.btnGoToDrawingSource_Click);
            // 
            // DrawingLayerSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "DrawingLayerSettingsCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtDrawingSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSheet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaxScale;
        private System.Windows.Forms.TextBox txtMinScale;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chkListDwfLayers;
        private System.Windows.Forms.LinkLabel lnkCheckAll;
        private System.Windows.Forms.Button btnGoToDrawingSource;
    }
}
