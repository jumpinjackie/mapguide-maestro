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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorScaleRangeCtrl));
            this.chkLine = new System.Windows.Forms.CheckBox();
            this.chkArea = new System.Windows.Forms.CheckBox();
            this.chkComposite = new System.Windows.Forms.CheckBox();
            this.compList = new Maestro.Editors.LayerDefinition.Vector.Scales.CompositeStyleListCtrl();
            this.areaList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.lineList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.pointList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.pointStylePanel = new Maestro.Editors.LayerDefinition.Vector.Scales.PointStylePanel();
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
            // pointStylePanel
            // 
            resources.ApplyResources(this.pointStylePanel, "pointStylePanel");
            this.pointStylePanel.Name = "pointStylePanel";
            // 
            // VectorScaleRangeCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.compList);
            this.Controls.Add(this.chkComposite);
            this.Controls.Add(this.areaList);
            this.Controls.Add(this.chkArea);
            this.Controls.Add(this.lineList);
            this.Controls.Add(this.chkLine);
            this.Controls.Add(this.pointList);
            this.Controls.Add(this.pointStylePanel);
            this.Name = "VectorScaleRangeCtrl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private ConditionListButtons pointList;
        private System.Windows.Forms.CheckBox chkLine;
        private ConditionListButtons lineList;
        private System.Windows.Forms.CheckBox chkArea;
        private ConditionListButtons areaList;
        private System.Windows.Forms.CheckBox chkComposite;
        private CompositeStyleListCtrl compList;
        private PointStylePanel pointStylePanel;


    }
}
