namespace Maestro.Editors.Generic
{
    partial class XmlEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlEditorCtrl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFormat = new System.Windows.Forms.ToolStripButton();
            this.btnValidate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtFind = new System.Windows.Forms.ToolStripTextBox();
            this.btnFindNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtReplace = new System.Windows.Forms.ToolStripTextBox();
            this.btnReplaceAll = new System.Windows.Forms.ToolStripButton();
            this.lblCursorPos = new System.Windows.Forms.ToolStripLabel();
            this.txtXmlContent = new System.Windows.Forms.TextBox();
            this.resDataCtrl = new Maestro.Editors.Generic.ResourceDataPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.toolStripSeparator1,
            this.btnCopy,
            this.btnCut,
            this.btnPaste,
            this.toolStripSeparator2,
            this.btnFormat,
            this.btnValidate,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.txtFind,
            this.btnFindNext,
            this.toolStripSeparator4,
            this.toolStripLabel3,
            this.txtReplace,
            this.btnReplaceAll,
            this.lblCursorPos});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnUndo
            // 
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUndo.Image = global::Maestro.Editors.Properties.Resources.arrow_curve_180_left;
            resources.ApplyResources(this.btnUndo, "btnUndo");
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = global::Maestro.Editors.Properties.Resources.document_copy;
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCut.Image = global::Maestro.Editors.Properties.Resources.scissors_blue;
            resources.ApplyResources(this.btnCut, "btnCut");
            this.btnCut.Name = "btnCut";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPaste.Image = global::Maestro.Editors.Properties.Resources.clipboard_paste;
            resources.ApplyResources(this.btnPaste, "btnPaste");
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnFormat
            // 
            this.btnFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFormat.Image = global::Maestro.Editors.Properties.Resources.edit_indent;
            resources.ApplyResources(this.btnFormat, "btnFormat");
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnValidate.Image = global::Maestro.Editors.Properties.Resources.tick;
            resources.ApplyResources(this.btnValidate, "btnValidate");
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // txtFind
            // 
            this.txtFind.Name = "txtFind";
            resources.ApplyResources(this.txtFind, "txtFind");
            // 
            // btnFindNext
            // 
            resources.ApplyResources(this.btnFindNext, "btnFindNext");
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // txtReplace
            // 
            this.txtReplace.Name = "txtReplace";
            resources.ApplyResources(this.txtReplace, "txtReplace");
            // 
            // btnReplaceAll
            // 
            resources.ApplyResources(this.btnReplaceAll, "btnReplaceAll");
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // lblCursorPos
            // 
            this.lblCursorPos.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblCursorPos.Name = "lblCursorPos";
            resources.ApplyResources(this.lblCursorPos, "lblCursorPos");
            // 
            // txtXmlContent
            // 
            this.txtXmlContent.AcceptsReturn = true;
            this.txtXmlContent.AcceptsTab = true;
            resources.ApplyResources(this.txtXmlContent, "txtXmlContent");
            this.txtXmlContent.Name = "txtXmlContent";
            this.txtXmlContent.TextChanged += new System.EventHandler(this.txtXmlContent_TextChanged);
            this.txtXmlContent.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtXmlContent_KeyUp);
            this.txtXmlContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtXmlContent_MouseClick);
            // 
            // resDataCtrl
            // 
            this.resDataCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.resDataCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resDataCtrl.HeaderText = "Resource Data Files";
            this.resDataCtrl.Name = "resDataCtrl";
            this.resDataCtrl.DataListChanged += new System.EventHandler(this.resDataCtrl_DataListChanged);
            // 
            // XmlEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtXmlContent);
            this.Controls.Add(this.resDataCtrl);
            this.Controls.Add(this.toolStrip1);
            this.Name = "XmlEditorCtrl";
            resources.ApplyResources(this, "$this");
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private ResourceDataPanel resDataCtrl;
        private System.Windows.Forms.TextBox txtXmlContent;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnValidate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtFind;
        private System.Windows.Forms.ToolStripButton btnFindNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnReplaceAll;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox txtReplace;
        private System.Windows.Forms.ToolStripLabel lblCursorPos;
        private System.Windows.Forms.ToolStripButton btnFormat;
    }
}
