namespace Maestro.Editors.SymbolDefinition
{
    partial class CompoundSymbolDefinitionEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompoundSymbolDefinitionEditorCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstSymbols = new System.Windows.Forms.ListView();
            this.symList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.symbolReferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstSymbols);
            this.groupBox1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstSymbols
            // 
            resources.ApplyResources(this.lstSymbols, "lstSymbols");
            this.lstSymbols.LargeImageList = this.symList;
            this.lstSymbols.Name = "lstSymbols";
            this.lstSymbols.SmallImageList = this.symList;
            this.lstSymbols.UseCompatibleStateImageBehavior = false;
            this.lstSymbols.View = System.Windows.Forms.View.List;
            this.lstSymbols.SelectedIndexChanged += new System.EventHandler(this.lstSymbols_SelectedIndexChanged);
            // 
            // symList
            // 
            this.symList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("symList.ImageStream")));
            this.symList.TransparentColor = System.Drawing.Color.Transparent;
            this.symList.Images.SetKeyName(0, "marker.png");
            this.symList.Images.SetKeyName(1, "arrow.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.symbolReferenceToolStripMenuItem,
            this.simpleSymbolToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // symbolReferenceToolStripMenuItem
            // 
            this.symbolReferenceToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.symbolReferenceToolStripMenuItem.Name = "symbolReferenceToolStripMenuItem";
            resources.ApplyResources(this.symbolReferenceToolStripMenuItem, "symbolReferenceToolStripMenuItem");
            this.symbolReferenceToolStripMenuItem.Click += new System.EventHandler(this.symbolReferenceToolStripMenuItem_Click);
            // 
            // simpleSymbolToolStripMenuItem
            // 
            this.simpleSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.simpleSymbolToolStripMenuItem.Name = "simpleSymbolToolStripMenuItem";
            resources.ApplyResources(this.simpleSymbolToolStripMenuItem, "simpleSymbolToolStripMenuItem");
            this.simpleSymbolToolStripMenuItem.Click += new System.EventHandler(this.simpleSymbolToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            // 
            // CompoundSymbolDefinitionEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompoundSymbolDefinitionEditorCtrl";
            resources.ApplyResources(this, "$this");
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstSymbols;
        private System.Windows.Forms.ImageList symList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem symbolReferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleSymbolToolStripMenuItem;

    }
}
