namespace Maestro.Editors.TileSetDefinition
{
    partial class LayerStructureCtrl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trvBaseLayers = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNewBaseLayerGroup = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveBaseLayerGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddBaseLayer = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveBaseLayer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveBaseLayerUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveBaseLayerDown = new System.Windows.Forms.ToolStripButton();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.splitContainer1);
            this.contentPanel.Size = new System.Drawing.Size(610, 259);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvBaseLayers);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertiesPanel);
            this.splitContainer1.Size = new System.Drawing.Size(610, 259);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 0;
            // 
            // trvBaseLayers
            // 
            this.trvBaseLayers.AllowDrop = true;
            this.trvBaseLayers.BackColor = System.Drawing.SystemColors.Window;
            this.trvBaseLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvBaseLayers.DefaultToolTipProvider = null;
            this.trvBaseLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvBaseLayers.DragDropMarkColor = System.Drawing.Color.Black;
            this.trvBaseLayers.LineColor = System.Drawing.SystemColors.ControlDark;
            this.trvBaseLayers.Location = new System.Drawing.Point(0, 25);
            this.trvBaseLayers.Model = null;
            this.trvBaseLayers.Name = "trvBaseLayers";
            this.trvBaseLayers.NodeControls.Add(this.nodeIcon1);
            this.trvBaseLayers.NodeControls.Add(this.nodeTextBox1);
            this.trvBaseLayers.SelectedNode = null;
            this.trvBaseLayers.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.trvBaseLayers.Size = new System.Drawing.Size(291, 234);
            this.trvBaseLayers.TabIndex = 1;
            this.trvBaseLayers.Text = "trvBaseLayers";
            this.trvBaseLayers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvBaseLayers_ItemDrag);
            this.trvBaseLayers.SelectionChanged += new System.EventHandler(this.trvBaseLayers_SelectionChanged);
            this.trvBaseLayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragDrop);
            this.trvBaseLayers.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragEnter);
            this.trvBaseLayers.DragOver += new System.Windows.Forms.DragEventHandler(this.trvBaseLayers_DragOver);
            this.trvBaseLayers.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trvBaseLayers_KeyUp);
            this.trvBaseLayers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvBaseLayers_MouseDoubleClick);
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
            this.nodeTextBox1.DataPropertyName = "Text";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewBaseLayerGroup,
            this.btnRemoveBaseLayerGroup,
            this.toolStripSeparator2,
            this.btnAddBaseLayer,
            this.btnRemoveBaseLayer,
            this.toolStripSeparator4,
            this.btnMoveBaseLayerUp,
            this.btnMoveBaseLayerDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(291, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNewBaseLayerGroup
            // 
            this.btnNewBaseLayerGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewBaseLayerGroup.Image = global::Maestro.Editors.Properties.Resources.folder__plus;
            this.btnNewBaseLayerGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewBaseLayerGroup.Name = "btnNewBaseLayerGroup";
            this.btnNewBaseLayerGroup.Size = new System.Drawing.Size(23, 22);
            this.btnNewBaseLayerGroup.ToolTipText = "Add a new group";
            this.btnNewBaseLayerGroup.Click += new System.EventHandler(this.btnNewBaseLayerGroup_Click);
            // 
            // btnRemoveBaseLayerGroup
            // 
            this.btnRemoveBaseLayerGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveBaseLayerGroup.Enabled = false;
            this.btnRemoveBaseLayerGroup.Image = global::Maestro.Editors.Properties.Resources.folder__minus;
            this.btnRemoveBaseLayerGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveBaseLayerGroup.Name = "btnRemoveBaseLayerGroup";
            this.btnRemoveBaseLayerGroup.Size = new System.Drawing.Size(23, 22);
            this.btnRemoveBaseLayerGroup.ToolTipText = "Remove selected group";
            this.btnRemoveBaseLayerGroup.Click += new System.EventHandler(this.btnRemoveBaseLayerGroup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddBaseLayer
            // 
            this.btnAddBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddBaseLayer.Enabled = false;
            this.btnAddBaseLayer.Image = global::Maestro.Editors.Properties.Resources.layer__plus;
            this.btnAddBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddBaseLayer.Name = "btnAddBaseLayer";
            this.btnAddBaseLayer.Size = new System.Drawing.Size(23, 22);
            this.btnAddBaseLayer.ToolTipText = "Add a new layer";
            this.btnAddBaseLayer.Click += new System.EventHandler(this.btnAddBaseLayer_Click);
            // 
            // btnRemoveBaseLayer
            // 
            this.btnRemoveBaseLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveBaseLayer.Enabled = false;
            this.btnRemoveBaseLayer.Image = global::Maestro.Editors.Properties.Resources.layer__minus;
            this.btnRemoveBaseLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveBaseLayer.Name = "btnRemoveBaseLayer";
            this.btnRemoveBaseLayer.Size = new System.Drawing.Size(23, 22);
            this.btnRemoveBaseLayer.ToolTipText = "Remove selected layer";
            this.btnRemoveBaseLayer.Click += new System.EventHandler(this.btnRemoveBaseLayer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveBaseLayerUp
            // 
            this.btnMoveBaseLayerUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveBaseLayerUp.Enabled = false;
            this.btnMoveBaseLayerUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMoveBaseLayerUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveBaseLayerUp.Name = "btnMoveBaseLayerUp";
            this.btnMoveBaseLayerUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveBaseLayerUp.ToolTipText = "Move selected layer up";
            this.btnMoveBaseLayerUp.Click += new System.EventHandler(this.btnMoveBaseLayerUp_Click);
            // 
            // btnMoveBaseLayerDown
            // 
            this.btnMoveBaseLayerDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveBaseLayerDown.Enabled = false;
            this.btnMoveBaseLayerDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMoveBaseLayerDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveBaseLayerDown.Name = "btnMoveBaseLayerDown";
            this.btnMoveBaseLayerDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveBaseLayerDown.ToolTipText = "Move selected layer down";
            this.btnMoveBaseLayerDown.Click += new System.EventHandler(this.btnMoveBaseLayerDown_Click);
            // 
            // propertiesPanel
            // 
            this.propertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesPanel.Location = new System.Drawing.Point(0, 0);
            this.propertiesPanel.Name = "propertiesPanel";
            this.propertiesPanel.Size = new System.Drawing.Size(315, 259);
            this.propertiesPanel.TabIndex = 0;
            // 
            // LayerStructureCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "Layer Structure";
            this.Name = "LayerStructureCtrl";
            this.Size = new System.Drawing.Size(610, 286);
            this.contentPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Aga.Controls.Tree.TreeViewAdv trvBaseLayers;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNewBaseLayerGroup;
        private System.Windows.Forms.ToolStripButton btnRemoveBaseLayerGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAddBaseLayer;
        private System.Windows.Forms.ToolStripButton btnRemoveBaseLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnMoveBaseLayerUp;
        private System.Windows.Forms.ToolStripButton btnMoveBaseLayerDown;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private System.Windows.Forms.Panel propertiesPanel;
    }
}
