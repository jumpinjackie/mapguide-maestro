namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class ScaleRangeConditions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleRangeConditions));
            this.DisplayLines = new System.Windows.Forms.CheckBox();
            this.lineConditionList = new ConditionListButtons();
            this.DisplayPoints = new System.Windows.Forms.CheckBox();
            this.pointConditionList = new ConditionListButtons();
            this.DisplayAreas = new System.Windows.Forms.CheckBox();
            this.areaConditionList = new ConditionListButtons();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.SuspendLayout();
            // 
            // DisplayLines
            // 
            resources.ApplyResources(this.DisplayLines, "DisplayLines");
            this.DisplayLines.Checked = true;
            this.DisplayLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayLines.Name = "DisplayLines";
            this.DisplayLines.UseVisualStyleBackColor = true;
            this.DisplayLines.CheckedChanged += new System.EventHandler(this.DisplayLines_CheckedChanged);
            // 
            // lineConditionList
            // 
            resources.ApplyResources(this.lineConditionList, "lineConditionList");
            this.lineConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineConditionList.Name = "lineConditionList";
            this.lineConditionList.Owner = null;
            this.lineConditionList.ItemChanged += new System.EventHandler(this.lineConditionList_ItemChanged);
            // 
            // DisplayPoints
            // 
            resources.ApplyResources(this.DisplayPoints, "DisplayPoints");
            this.DisplayPoints.Checked = true;
            this.DisplayPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayPoints.Name = "DisplayPoints";
            this.DisplayPoints.UseVisualStyleBackColor = true;
            this.DisplayPoints.CheckedChanged += new System.EventHandler(this.DisplayPoints_CheckedChanged);
            // 
            // pointConditionList
            // 
            resources.ApplyResources(this.pointConditionList, "pointConditionList");
            this.pointConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pointConditionList.Name = "pointConditionList";
            this.pointConditionList.Owner = null;
            this.pointConditionList.ItemChanged += new System.EventHandler(this.pointConditionList_ItemChanged);
            // 
            // DisplayAreas
            // 
            resources.ApplyResources(this.DisplayAreas, "DisplayAreas");
            this.DisplayAreas.Checked = true;
            this.DisplayAreas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayAreas.Name = "DisplayAreas";
            this.DisplayAreas.UseVisualStyleBackColor = true;
            this.DisplayAreas.CheckedChanged += new System.EventHandler(this.DisplayAreas_CheckedChanged);
            // 
            // areaConditionList
            // 
            resources.ApplyResources(this.areaConditionList, "areaConditionList");
            this.areaConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.areaConditionList.Name = "areaConditionList";
            this.areaConditionList.Owner = null;
            this.areaConditionList.ItemChanged += new System.EventHandler(this.areaConditionList_ItemChanged);
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // ScaleRangeConditions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.areaConditionList);
            this.Controls.Add(this.DisplayAreas);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.lineConditionList);
            this.Controls.Add(this.DisplayLines);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pointConditionList);
            this.Controls.Add(this.DisplayPoints);
            this.Name = "ScaleRangeConditions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DisplayLines;
        private ConditionListButtons lineConditionList;
        private ConditionListButtons pointConditionList;
        private System.Windows.Forms.CheckBox DisplayPoints;
        private ConditionListButtons areaConditionList;
        private System.Windows.Forms.CheckBox DisplayAreas;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
    }
}
