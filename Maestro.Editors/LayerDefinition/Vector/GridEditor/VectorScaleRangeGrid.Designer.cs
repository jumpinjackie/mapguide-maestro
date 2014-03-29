namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    partial class VectorScaleRangeGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorScaleRangeGrid));
            this.geomStyleIcons = new System.Windows.Forms.ImageList(this.components);
            this.TAB_LINES = new System.Windows.Forms.TabPage();
            this.TAB_AREAS = new System.Windows.Forms.TabPage();
            this.TAB_POINTS = new System.Windows.Forms.TabPage();
            this.TAB_COMPOSITE = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstStyles = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.tabGeomStyles = new System.Windows.Forms.TabControl();
            this.label1 = new System.Windows.Forms.Label();
            this.chkComposite = new System.Windows.Forms.CheckBox();
            this.chkArea = new System.Windows.Forms.CheckBox();
            this.chkLine = new System.Windows.Forms.CheckBox();
            this.chkPoints = new System.Windows.Forms.CheckBox();
            this.pointRuleGrid = new Maestro.Editors.LayerDefinition.Vector.GridEditor.RuleGridView();
            this.lineRuleGrid = new Maestro.Editors.LayerDefinition.Vector.GridEditor.RuleGridView();
            this.areaRuleGrid = new Maestro.Editors.LayerDefinition.Vector.GridEditor.RuleGridView();
            this.TAB_LINES.SuspendLayout();
            this.TAB_AREAS.SuspendLayout();
            this.TAB_POINTS.SuspendLayout();
            this.TAB_COMPOSITE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabGeomStyles.SuspendLayout();
            this.SuspendLayout();
            // 
            // geomStyleIcons
            // 
            this.geomStyleIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("geomStyleIcons.ImageStream")));
            this.geomStyleIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.geomStyleIcons.Images.SetKeyName(0, "layer-small.png");
            this.geomStyleIcons.Images.SetKeyName(1, "layer-shape-line.png");
            this.geomStyleIcons.Images.SetKeyName(2, "layer-shape-polygon.png");
            this.geomStyleIcons.Images.SetKeyName(3, "layer-vector.png");
            // 
            // TAB_LINES
            // 
            this.TAB_LINES.AutoScroll = true;
            this.TAB_LINES.Controls.Add(this.lineRuleGrid);
            this.TAB_LINES.ImageIndex = 1;
            this.TAB_LINES.Location = new System.Drawing.Point(4, 23);
            this.TAB_LINES.Name = "TAB_LINES";
            this.TAB_LINES.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_LINES.Size = new System.Drawing.Size(679, 390);
            this.TAB_LINES.TabIndex = 1;
            this.TAB_LINES.Text = "Lines";
            this.TAB_LINES.UseVisualStyleBackColor = true;
            // 
            // TAB_AREAS
            // 
            this.TAB_AREAS.AutoScroll = true;
            this.TAB_AREAS.Controls.Add(this.areaRuleGrid);
            this.TAB_AREAS.ImageIndex = 2;
            this.TAB_AREAS.Location = new System.Drawing.Point(4, 23);
            this.TAB_AREAS.Name = "TAB_AREAS";
            this.TAB_AREAS.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_AREAS.Size = new System.Drawing.Size(679, 390);
            this.TAB_AREAS.TabIndex = 2;
            this.TAB_AREAS.Text = "Areas";
            this.TAB_AREAS.UseVisualStyleBackColor = true;
            // 
            // TAB_POINTS
            // 
            this.TAB_POINTS.AutoScroll = true;
            this.TAB_POINTS.Controls.Add(this.pointRuleGrid);
            this.TAB_POINTS.ImageIndex = 0;
            this.TAB_POINTS.Location = new System.Drawing.Point(4, 23);
            this.TAB_POINTS.Name = "TAB_POINTS";
            this.TAB_POINTS.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_POINTS.Size = new System.Drawing.Size(679, 390);
            this.TAB_POINTS.TabIndex = 0;
            this.TAB_POINTS.Text = "Points";
            this.TAB_POINTS.UseVisualStyleBackColor = true;
            // 
            // TAB_COMPOSITE
            // 
            this.TAB_COMPOSITE.AutoScroll = true;
            this.TAB_COMPOSITE.Controls.Add(this.splitContainer1);
            this.TAB_COMPOSITE.ImageIndex = 3;
            this.TAB_COMPOSITE.Location = new System.Drawing.Point(4, 23);
            this.TAB_COMPOSITE.Name = "TAB_COMPOSITE";
            this.TAB_COMPOSITE.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_COMPOSITE.Size = new System.Drawing.Size(679, 390);
            this.TAB_COMPOSITE.TabIndex = 3;
            this.TAB_COMPOSITE.Text = "Composite";
            this.TAB_COMPOSITE.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstStyles);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(673, 384);
            this.splitContainer1.SplitterDistance = 161;
            this.splitContainer1.TabIndex = 0;
            // 
            // lstStyles
            // 
            this.lstStyles.DisplayMember = "DisplayString";
            this.lstStyles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstStyles.FormattingEnabled = true;
            this.lstStyles.Location = new System.Drawing.Point(0, 25);
            this.lstStyles.Name = "lstStyles";
            this.lstStyles.Size = new System.Drawing.Size(161, 359);
            this.lstStyles.TabIndex = 3;
            this.lstStyles.SelectedIndexChanged += new System.EventHandler(this.lstStyles_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(161, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(49, 22);
            this.btnAdd.Text = "Add";
            this.btnAdd.ToolTipText = "Add a Composite Style";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete this Composite Style";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tabGeomStyles
            // 
            this.tabGeomStyles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabGeomStyles.Controls.Add(this.TAB_POINTS);
            this.tabGeomStyles.Controls.Add(this.TAB_LINES);
            this.tabGeomStyles.Controls.Add(this.TAB_AREAS);
            this.tabGeomStyles.Controls.Add(this.TAB_COMPOSITE);
            this.tabGeomStyles.ImageList = this.geomStyleIcons;
            this.tabGeomStyles.Location = new System.Drawing.Point(0, 27);
            this.tabGeomStyles.Name = "tabGeomStyles";
            this.tabGeomStyles.SelectedIndex = 0;
            this.tabGeomStyles.Size = new System.Drawing.Size(687, 417);
            this.tabGeomStyles.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Enable these styles";
            // 
            // chkComposite
            // 
            this.chkComposite.AutoSize = true;
            this.chkComposite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkComposite.Location = new System.Drawing.Point(294, 4);
            this.chkComposite.Name = "chkComposite";
            this.chkComposite.Size = new System.Drawing.Size(183, 17);
            this.chkComposite.TabIndex = 13;
            this.chkComposite.Text = "Composite (Advanced Stylization)";
            this.chkComposite.UseVisualStyleBackColor = true;
            this.chkComposite.CheckedChanged += new System.EventHandler(this.chkComposite_CheckedChanged);
            // 
            // chkArea
            // 
            this.chkArea.AutoSize = true;
            this.chkArea.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkArea.Location = new System.Drawing.Point(235, 4);
            this.chkArea.Name = "chkArea";
            this.chkArea.Size = new System.Drawing.Size(53, 17);
            this.chkArea.TabIndex = 12;
            this.chkArea.Text = "Areas";
            this.chkArea.UseVisualStyleBackColor = true;
            this.chkArea.CheckedChanged += new System.EventHandler(this.chkArea_CheckedChanged);
            // 
            // chkLine
            // 
            this.chkLine.AutoSize = true;
            this.chkLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkLine.Location = new System.Drawing.Point(178, 4);
            this.chkLine.Name = "chkLine";
            this.chkLine.Size = new System.Drawing.Size(51, 17);
            this.chkLine.TabIndex = 11;
            this.chkLine.Text = "Lines";
            this.chkLine.UseVisualStyleBackColor = true;
            this.chkLine.CheckedChanged += new System.EventHandler(this.chkLine_CheckedChanged);
            // 
            // chkPoints
            // 
            this.chkPoints.AutoSize = true;
            this.chkPoints.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkPoints.Location = new System.Drawing.Point(117, 4);
            this.chkPoints.Name = "chkPoints";
            this.chkPoints.Size = new System.Drawing.Size(55, 17);
            this.chkPoints.TabIndex = 14;
            this.chkPoints.Text = "Points";
            this.chkPoints.UseVisualStyleBackColor = true;
            this.chkPoints.CheckedChanged += new System.EventHandler(this.chkPoints_CheckedChanged);
            // 
            // pointRuleGrid
            // 
            this.pointRuleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pointRuleGrid.Location = new System.Drawing.Point(3, 3);
            this.pointRuleGrid.Name = "pointRuleGrid";
            this.pointRuleGrid.Size = new System.Drawing.Size(673, 384);
            this.pointRuleGrid.TabIndex = 0;
            this.pointRuleGrid.ThemeIndexOffest = 0;
            // 
            // lineRuleGrid
            // 
            this.lineRuleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineRuleGrid.Location = new System.Drawing.Point(3, 3);
            this.lineRuleGrid.Name = "lineRuleGrid";
            this.lineRuleGrid.Size = new System.Drawing.Size(673, 384);
            this.lineRuleGrid.TabIndex = 0;
            this.lineRuleGrid.ThemeIndexOffest = 0;
            // 
            // areaRuleGrid
            // 
            this.areaRuleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.areaRuleGrid.Location = new System.Drawing.Point(3, 3);
            this.areaRuleGrid.Name = "areaRuleGrid";
            this.areaRuleGrid.Size = new System.Drawing.Size(673, 384);
            this.areaRuleGrid.TabIndex = 0;
            this.areaRuleGrid.ThemeIndexOffest = 0;
            // 
            // VectorScaleRangeGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabGeomStyles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkComposite);
            this.Controls.Add(this.chkArea);
            this.Controls.Add(this.chkLine);
            this.Controls.Add(this.chkPoints);
            this.Name = "VectorScaleRangeGrid";
            this.Size = new System.Drawing.Size(687, 444);
            this.TAB_LINES.ResumeLayout(false);
            this.TAB_AREAS.ResumeLayout(false);
            this.TAB_POINTS.ResumeLayout(false);
            this.TAB_COMPOSITE.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabGeomStyles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList geomStyleIcons;
        private System.Windows.Forms.TabPage TAB_LINES;
        private System.Windows.Forms.TabPage TAB_AREAS;
        private System.Windows.Forms.TabPage TAB_POINTS;
        private System.Windows.Forms.TabPage TAB_COMPOSITE;
        private System.Windows.Forms.TabControl tabGeomStyles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkComposite;
        private System.Windows.Forms.CheckBox chkArea;
        private System.Windows.Forms.CheckBox chkLine;
        private System.Windows.Forms.CheckBox chkPoints;
        private RuleGridView lineRuleGrid;
        private RuleGridView areaRuleGrid;
        private RuleGridView pointRuleGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstStyles;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
    }
}
