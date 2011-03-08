namespace Maestro.Editors.WebLayout
{
    partial class MenuEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuEditorCtrl));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuBuiltin = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.trvMenuItems = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.btnDelete,
            this.toolStripSeparator4,
            this.btnMoveUp,
            this.btnMoveDown});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBuiltin,
            this.mnuCustom,
            this.toolStripSeparator3,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.toolStripDropDownButton1.Image = global::Maestro.Editors.Properties.Resources.application__plus;
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            // 
            // mnuBuiltin
            // 
            this.mnuBuiltin.Name = "mnuBuiltin";
            resources.ApplyResources(this.mnuBuiltin, "mnuBuiltin");
            // 
            // mnuCustom
            // 
            this.mnuCustom.Name = "mnuCustom";
            resources.ApplyResources(this.mnuCustom, "mnuCustom");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::Maestro.Editors.Properties.Resources.ui_splitter_horizontal;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Click += new System.EventHandler(this.addSeparator_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Image = global::Maestro.Editors.Properties.Resources.ui_menu;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Click += new System.EventHandler(this.addFlyout_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // trvMenuItems
            // 
            this.trvMenuItems.AllowDrop = true;
            this.trvMenuItems.BackColor = System.Drawing.SystemColors.Window;
            this.trvMenuItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trvMenuItems.DefaultToolTipProvider = null;
            resources.ApplyResources(this.trvMenuItems, "trvMenuItems");
            this.trvMenuItems.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvMenuItems.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvMenuItems.Model = null;
            this.trvMenuItems.Name = "trvMenuItems";
            this.trvMenuItems.NodeControls.Add(this.nodeIcon1);
            this.trvMenuItems.NodeControls.Add(this.nodeTextBox1);
            this.trvMenuItems.SelectedNode = null;
            this.trvMenuItems.SelectionChanged += new System.EventHandler(this.trvMenuItems_SelectionChanged);
            this.trvMenuItems.DragOver += new System.Windows.Forms.DragEventHandler(this.trvMenuItems_DragOver);
            this.trvMenuItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvMenuItems_DragDrop);
            this.trvMenuItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvMenuItems_ItemDrag);
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Icon";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = null;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Label";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // MenuEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.trvMenuItems);
            this.Controls.Add(this.toolStrip2);
            this.Name = "MenuEditorCtrl";
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem mnuBuiltin;
        private System.Windows.Forms.ToolStripMenuItem mnuCustom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private Aga.Controls.Tree.TreeViewAdv trvMenuItems;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
    }
}
