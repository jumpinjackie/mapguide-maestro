namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
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
            this.DisplayLines = new System.Windows.Forms.CheckBox();
            this.lineConditionList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ConditionListButtons();
            this.DisplayPoints = new System.Windows.Forms.CheckBox();
            this.pointConditionList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ConditionListButtons();
            this.DisplayAreas = new System.Windows.Forms.CheckBox();
            this.areaConditionList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ConditionListButtons();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.SuspendLayout();
            // 
            // DisplayLines
            // 
            this.DisplayLines.AutoSize = true;
            this.DisplayLines.Checked = true;
            this.DisplayLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayLines.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisplayLines.Location = new System.Drawing.Point(0, 113);
            this.DisplayLines.Name = "DisplayLines";
            this.DisplayLines.Size = new System.Drawing.Size(597, 17);
            this.DisplayLines.TabIndex = 7;
            this.DisplayLines.Text = "Display lines";
            this.DisplayLines.UseVisualStyleBackColor = true;
            this.DisplayLines.CheckedChanged += new System.EventHandler(this.DisplayLines_CheckedChanged);
            // 
            // lineConditionList
            // 
            this.lineConditionList.AutoScroll = true;
            this.lineConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineConditionList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineConditionList.Location = new System.Drawing.Point(0, 130);
            this.lineConditionList.Name = "lineConditionList";
            this.lineConditionList.Size = new System.Drawing.Size(597, 88);
            this.lineConditionList.TabIndex = 10;
            this.lineConditionList.ItemChanged += new System.EventHandler(this.lineConditionList_ItemChanged);
            // 
            // DisplayPoints
            // 
            this.DisplayPoints.AutoSize = true;
            this.DisplayPoints.Checked = true;
            this.DisplayPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayPoints.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisplayPoints.Location = new System.Drawing.Point(0, 0);
            this.DisplayPoints.Name = "DisplayPoints";
            this.DisplayPoints.Size = new System.Drawing.Size(597, 17);
            this.DisplayPoints.TabIndex = 7;
            this.DisplayPoints.Text = "Display points";
            this.DisplayPoints.UseVisualStyleBackColor = true;
            this.DisplayPoints.CheckedChanged += new System.EventHandler(this.DisplayPoints_CheckedChanged);
            // 
            // pointConditionList
            // 
            this.pointConditionList.AutoScroll = true;
            this.pointConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pointConditionList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pointConditionList.Location = new System.Drawing.Point(0, 17);
            this.pointConditionList.Name = "pointConditionList";
            this.pointConditionList.Size = new System.Drawing.Size(597, 88);
            this.pointConditionList.TabIndex = 10;
            this.pointConditionList.ItemChanged += new System.EventHandler(this.pointConditionList_ItemChanged);
            // 
            // DisplayAreas
            // 
            this.DisplayAreas.AutoSize = true;
            this.DisplayAreas.Checked = true;
            this.DisplayAreas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayAreas.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisplayAreas.Location = new System.Drawing.Point(0, 226);
            this.DisplayAreas.Name = "DisplayAreas";
            this.DisplayAreas.Size = new System.Drawing.Size(597, 17);
            this.DisplayAreas.TabIndex = 7;
            this.DisplayAreas.Text = "Display areas";
            this.DisplayAreas.UseVisualStyleBackColor = true;
            this.DisplayAreas.CheckedChanged += new System.EventHandler(this.DisplayAreas_CheckedChanged);
            // 
            // areaConditionList
            // 
            this.areaConditionList.AutoScroll = true;
            this.areaConditionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.areaConditionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.areaConditionList.Location = new System.Drawing.Point(0, 243);
            this.areaConditionList.Name = "areaConditionList";
            this.areaConditionList.Size = new System.Drawing.Size(597, 91);
            this.areaConditionList.TabIndex = 10;
            this.areaConditionList.ItemChanged += new System.EventHandler(this.areaConditionList_ItemChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 105);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(597, 8);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 218);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(597, 8);
            this.splitter2.TabIndex = 12;
            this.splitter2.TabStop = false;
            // 
            // ScaleRangeConditions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.areaConditionList);
            this.Controls.Add(this.DisplayAreas);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.lineConditionList);
            this.Controls.Add(this.DisplayLines);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pointConditionList);
            this.Controls.Add(this.DisplayPoints);
            this.Name = "ScaleRangeConditions";
            this.Size = new System.Drawing.Size(597, 334);
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
