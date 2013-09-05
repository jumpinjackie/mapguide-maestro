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
            this.components = new System.ComponentModel.Container();
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
            this.lblCursorPos = new System.Windows.Forms.ToolStripLabel();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.btnFindAndReplace = new System.Windows.Forms.ToolStripButton();
            this.btnReRead = new System.Windows.Forms.ToolStripButton();
            this.resDataCtrl = new Maestro.Editors.Generic.ResourceDataPanel();
            this.nodeNumericUpDown1 = new Aga.Controls.Tree.NodeControls.NodeNumericUpDown();
            this.txtXmlContent = new Maestro.Editors.Generic.XmlTextEditorControl();
            this.ctxXmlEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.ctxXmlEditor.SuspendLayout();
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
            this.lblCursorPos,
            this.btnFind,
            this.btnFindAndReplace,
            this.btnReRead});
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
            this.btnValidate.Image = global::Maestro.Editors.Properties.Resources.document_task;
            resources.ApplyResources(this.btnValidate, "btnValidate");
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // lblCursorPos
            // 
            this.lblCursorPos.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblCursorPos.Name = "lblCursorPos";
            resources.ApplyResources(this.lblCursorPos, "lblCursorPos");
            // 
            // btnFind
            // 
            this.btnFind.Image = global::Maestro.Editors.Properties.Resources.magnifier_left;
            resources.ApplyResources(this.btnFind, "btnFind");
            this.btnFind.Name = "btnFind";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnFindAndReplace
            // 
            this.btnFindAndReplace.Image = global::Maestro.Editors.Properties.Resources.magnifier_left;
            resources.ApplyResources(this.btnFindAndReplace, "btnFindAndReplace");
            this.btnFindAndReplace.Name = "btnFindAndReplace";
            this.btnFindAndReplace.Click += new System.EventHandler(this.btnFindAndReplace_Click);
            // 
            // btnReRead
            // 
            this.btnReRead.Image = global::Maestro.Editors.Properties.Resources.arrow_circle_135;
            resources.ApplyResources(this.btnReRead, "btnReRead");
            this.btnReRead.Name = "btnReRead";
            this.btnReRead.Click += new System.EventHandler(this.btnReRead_Click);
            // 
            // resDataCtrl
            // 
            this.resDataCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.resDataCtrl, "resDataCtrl");
            this.resDataCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.resDataCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resDataCtrl.Name = "resDataCtrl";
            this.resDataCtrl.DataListChanged += new System.EventHandler(this.resDataCtrl_DataListChanged);
            // 
            // nodeNumericUpDown1
            // 
            this.nodeNumericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nodeNumericUpDown1.IncrementalSearchEnabled = true;
            this.nodeNumericUpDown1.LeftMargin = 3;
            this.nodeNumericUpDown1.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nodeNumericUpDown1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nodeNumericUpDown1.ParentColumn = null;
            // 
            // txtXmlContent
            // 
            this.txtXmlContent.ContextMenuStrip = this.ctxXmlEditor;
            resources.ApplyResources(this.txtXmlContent, "txtXmlContent");
            this.txtXmlContent.IsReadOnly = false;
            this.txtXmlContent.Name = "txtXmlContent";
            this.txtXmlContent.TextChanged += new System.EventHandler(this.txtXmlContent_TextChanged);
            // 
            // ctxXmlEditor
            // 
            this.ctxXmlEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.findToolStripMenuItem,
            this.findReplaceToolStripMenuItem});
            this.ctxXmlEditor.Name = "ctxXmlEditor";
            resources.ApplyResources(this.ctxXmlEditor, "ctxXmlEditor");
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // findReplaceToolStripMenuItem
            // 
            this.findReplaceToolStripMenuItem.Name = "findReplaceToolStripMenuItem";
            resources.ApplyResources(this.findReplaceToolStripMenuItem, "findReplaceToolStripMenuItem");
            this.findReplaceToolStripMenuItem.Click += new System.EventHandler(this.findReplaceToolStripMenuItem_Click);
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
            this.ctxXmlEditor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Aga.Controls.Tree.NodeControls.NodeNumericUpDown nodeNumericUpDown1;

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private ResourceDataPanel resDataCtrl;
        private XmlTextEditorControl txtXmlContent;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnValidate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lblCursorPos;
        private System.Windows.Forms.ToolStripButton btnFormat;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripButton btnFindAndReplace;
        private System.Windows.Forms.ContextMenuStrip ctxXmlEditor;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnReRead;
    }
}
