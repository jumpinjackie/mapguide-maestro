namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class VectorScaleRangeCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorScaleRangeCtrl));
            this.chkLine = new System.Windows.Forms.CheckBox();
            this.chkArea = new System.Windows.Forms.CheckBox();
            this.chkComposite = new System.Windows.Forms.CheckBox();
            this.compList = new Maestro.Editors.LayerDefinition.Vector.Scales.CompositeStyleListCtrl();
            this.areaList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.lineList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.pointList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.chkPoints = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabGeomStyles = new System.Windows.Forms.TabControl();
            this.TAB_POINTS = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAllowOverpost = new System.Windows.Forms.CheckBox();
            this.chkDisplayAsText = new System.Windows.Forms.CheckBox();
            this.TAB_LINES = new System.Windows.Forms.TabPage();
            this.TAB_AREAS = new System.Windows.Forms.TabPage();
            this.TAB_COMPOSITE = new System.Windows.Forms.TabPage();
            this.geomStyleIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabGeomStyles.SuspendLayout();
            this.TAB_POINTS.SuspendLayout();
            this.panel1.SuspendLayout();
            this.TAB_LINES.SuspendLayout();
            this.TAB_AREAS.SuspendLayout();
            this.TAB_COMPOSITE.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkLine
            // 
            resources.ApplyResources(this.chkLine, "chkLine");
            this.chkLine.Name = "chkLine";
            this.chkLine.UseVisualStyleBackColor = true;
            this.chkLine.CheckedChanged += new System.EventHandler(this.chkLine_CheckedChanged);
            // 
            // chkArea
            // 
            resources.ApplyResources(this.chkArea, "chkArea");
            this.chkArea.Name = "chkArea";
            this.chkArea.UseVisualStyleBackColor = true;
            this.chkArea.CheckedChanged += new System.EventHandler(this.chkArea_CheckedChanged);
            // 
            // chkComposite
            // 
            resources.ApplyResources(this.chkComposite, "chkComposite");
            this.chkComposite.Name = "chkComposite";
            this.chkComposite.UseVisualStyleBackColor = true;
            this.chkComposite.CheckedChanged += new System.EventHandler(this.chkComposite_CheckedChanged);
            // 
            // compList
            // 
            resources.ApplyResources(this.compList, "compList");
            this.compList.Name = "compList";
            // 
            // areaList
            // 
            resources.ApplyResources(this.areaList, "areaList");
            this.areaList.Name = "areaList";
            this.areaList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // lineList
            // 
            resources.ApplyResources(this.lineList, "lineList");
            this.lineList.Name = "lineList";
            this.lineList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // pointList
            // 
            resources.ApplyResources(this.pointList, "pointList");
            this.pointList.Name = "pointList";
            this.pointList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // chkPoints
            // 
            resources.ApplyResources(this.chkPoints, "chkPoints");
            this.chkPoints.Name = "chkPoints";
            this.chkPoints.UseVisualStyleBackColor = true;
            this.chkPoints.CheckedChanged += new System.EventHandler(this.chkPoints_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabGeomStyles
            // 
            resources.ApplyResources(this.tabGeomStyles, "tabGeomStyles");
            this.tabGeomStyles.Controls.Add(this.TAB_POINTS);
            this.tabGeomStyles.Controls.Add(this.TAB_LINES);
            this.tabGeomStyles.Controls.Add(this.TAB_AREAS);
            this.tabGeomStyles.Controls.Add(this.TAB_COMPOSITE);
            this.tabGeomStyles.ImageList = this.geomStyleIcons;
            this.tabGeomStyles.Name = "tabGeomStyles";
            this.tabGeomStyles.SelectedIndex = 0;
            // 
            // TAB_POINTS
            // 
            resources.ApplyResources(this.TAB_POINTS, "TAB_POINTS");
            this.TAB_POINTS.Controls.Add(this.pointList);
            this.TAB_POINTS.Controls.Add(this.panel1);
            this.TAB_POINTS.Name = "TAB_POINTS";
            this.TAB_POINTS.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAllowOverpost);
            this.panel1.Controls.Add(this.chkDisplayAsText);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // chkAllowOverpost
            // 
            resources.ApplyResources(this.chkAllowOverpost, "chkAllowOverpost");
            this.chkAllowOverpost.Name = "chkAllowOverpost";
            this.chkAllowOverpost.UseVisualStyleBackColor = true;
            this.chkAllowOverpost.CheckedChanged += new System.EventHandler(this.chkAllowOverpost_CheckedChanged);
            // 
            // chkDisplayAsText
            // 
            resources.ApplyResources(this.chkDisplayAsText, "chkDisplayAsText");
            this.chkDisplayAsText.Name = "chkDisplayAsText";
            this.chkDisplayAsText.UseVisualStyleBackColor = true;
            this.chkDisplayAsText.CheckedChanged += new System.EventHandler(this.chkDisplayAsText_CheckedChanged);
            // 
            // TAB_LINES
            // 
            resources.ApplyResources(this.TAB_LINES, "TAB_LINES");
            this.TAB_LINES.Controls.Add(this.lineList);
            this.TAB_LINES.Name = "TAB_LINES";
            this.TAB_LINES.UseVisualStyleBackColor = true;
            // 
            // TAB_AREAS
            // 
            resources.ApplyResources(this.TAB_AREAS, "TAB_AREAS");
            this.TAB_AREAS.Controls.Add(this.areaList);
            this.TAB_AREAS.Name = "TAB_AREAS";
            this.TAB_AREAS.UseVisualStyleBackColor = true;
            // 
            // TAB_COMPOSITE
            // 
            resources.ApplyResources(this.TAB_COMPOSITE, "TAB_COMPOSITE");
            this.TAB_COMPOSITE.Controls.Add(this.compList);
            this.TAB_COMPOSITE.Name = "TAB_COMPOSITE";
            this.TAB_COMPOSITE.UseVisualStyleBackColor = true;
            // 
            // geomStyleIcons
            // 
            this.geomStyleIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("geomStyleIcons.ImageStream")));
            this.geomStyleIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.geomStyleIcons.Images.SetKeyName(0, "layer-small.png");
            this.geomStyleIcons.Images.SetKeyName(1, "layer-shape-line.png");
            this.geomStyleIcons.Images.SetKeyName(2, "layer-shape-polygon.png");
            this.geomStyleIcons.Images.SetKeyName(3, "images-stack.png");
            // 
            // VectorScaleRangeCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tabGeomStyles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkPoints);
            this.Controls.Add(this.chkComposite);
            this.Controls.Add(this.chkArea);
            this.Controls.Add(this.chkLine);
            this.Name = "VectorScaleRangeCtrl";
            resources.ApplyResources(this, "$this");
            this.tabGeomStyles.ResumeLayout(false);
            this.TAB_POINTS.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.TAB_LINES.ResumeLayout(false);
            this.TAB_AREAS.ResumeLayout(false);
            this.TAB_COMPOSITE.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConditionListButtons pointList;
        private System.Windows.Forms.CheckBox chkLine;
        private ConditionListButtons lineList;
        private System.Windows.Forms.CheckBox chkArea;
        private ConditionListButtons areaList;
        private System.Windows.Forms.CheckBox chkComposite;
        private CompositeStyleListCtrl compList;
        private System.Windows.Forms.CheckBox chkPoints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabGeomStyles;
        private System.Windows.Forms.TabPage TAB_POINTS;
        private System.Windows.Forms.TabPage TAB_LINES;
        private System.Windows.Forms.TabPage TAB_AREAS;
        private System.Windows.Forms.TabPage TAB_COMPOSITE;
        private System.Windows.Forms.ImageList geomStyleIcons;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkAllowOverpost;
        private System.Windows.Forms.CheckBox chkDisplayAsText;


    }
}
