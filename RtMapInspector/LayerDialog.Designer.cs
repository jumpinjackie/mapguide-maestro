namespace RtMapInspector
{
    partial class LayerDialog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TAB_DATA = new System.Windows.Forms.TabPage();
            this.localFsPreviewCtrl = new Maestro.Editors.FeatureSource.Preview.LocalFeatureSourcePreviewCtrl();
            this.TAB_XML = new System.Windows.Forms.TabPage();
            this.txtXml = new ICSharpCode.TextEditor.TextEditorControl();
            this.tabControl1.SuspendLayout();
            this.TAB_DATA.SuspendLayout();
            this.TAB_XML.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TAB_DATA);
            this.tabControl1.Controls.Add(this.TAB_XML);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // TAB_DATA
            // 
            this.TAB_DATA.Controls.Add(this.localFsPreviewCtrl);
            this.TAB_DATA.Location = new System.Drawing.Point(4, 22);
            this.TAB_DATA.Name = "TAB_DATA";
            this.TAB_DATA.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_DATA.Size = new System.Drawing.Size(792, 424);
            this.TAB_DATA.TabIndex = 0;
            this.TAB_DATA.Text = "Data";
            this.TAB_DATA.UseVisualStyleBackColor = true;
            // 
            // localFsPreviewCtrl
            // 
            this.localFsPreviewCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localFsPreviewCtrl.Location = new System.Drawing.Point(3, 3);
            this.localFsPreviewCtrl.Name = "localFsPreviewCtrl";
            this.localFsPreviewCtrl.Size = new System.Drawing.Size(786, 418);
            this.localFsPreviewCtrl.SupportsSQL = false;
            this.localFsPreviewCtrl.TabIndex = 0;
            // 
            // TAB_XML
            // 
            this.TAB_XML.Controls.Add(this.txtXml);
            this.TAB_XML.Location = new System.Drawing.Point(4, 22);
            this.TAB_XML.Name = "TAB_XML";
            this.TAB_XML.Size = new System.Drawing.Size(792, 424);
            this.TAB_XML.TabIndex = 2;
            this.TAB_XML.Text = "XML (read-only)";
            this.TAB_XML.UseVisualStyleBackColor = true;
            // 
            // txtXml
            // 
            this.txtXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXml.IsReadOnly = false;
            this.txtXml.Location = new System.Drawing.Point(0, 0);
            this.txtXml.Name = "txtXml";
            this.txtXml.Size = new System.Drawing.Size(792, 424);
            this.txtXml.TabIndex = 0;
            // 
            // LayerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "LayerDialog";
            this.Text = "LayerDialog";
            this.tabControl1.ResumeLayout(false);
            this.TAB_DATA.ResumeLayout(false);
            this.TAB_XML.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TAB_DATA;
        private System.Windows.Forms.TabPage TAB_XML;
        private Maestro.Editors.FeatureSource.Preview.LocalFeatureSourcePreviewCtrl localFsPreviewCtrl;
        private ICSharpCode.TextEditor.TextEditorControl txtXml;
    }
}