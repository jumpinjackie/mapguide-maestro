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
            this.chkPoint = new System.Windows.Forms.CheckBox();
            this.chkLine = new System.Windows.Forms.CheckBox();
            this.chkArea = new System.Windows.Forms.CheckBox();
            this.chkComposite = new System.Windows.Forms.CheckBox();
            this.compList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.areaList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.lineList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.pointList = new Maestro.Editors.LayerDefinition.Vector.Scales.ConditionListButtons();
            this.SuspendLayout();
            // 
            // chkPoint
            // 
            this.chkPoint.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPoint.Location = new System.Drawing.Point(0, 0);
            this.chkPoint.Name = "chkPoint";
            this.chkPoint.Size = new System.Drawing.Size(598, 24);
            this.chkPoint.TabIndex = 0;
            this.chkPoint.Text = "Display Points (Note: Symbol previews only show the symbol. It doesn\'t consider c" +
                "olor overrides or size/rotation settings)";
            this.chkPoint.UseVisualStyleBackColor = true;
            this.chkPoint.CheckedChanged += new System.EventHandler(this.chkPoint_CheckedChanged);
            // 
            // chkLine
            // 
            this.chkLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkLine.Location = new System.Drawing.Point(0, 121);
            this.chkLine.Name = "chkLine";
            this.chkLine.Size = new System.Drawing.Size(598, 24);
            this.chkLine.TabIndex = 2;
            this.chkLine.Text = "Display Lines";
            this.chkLine.UseVisualStyleBackColor = true;
            this.chkLine.CheckedChanged += new System.EventHandler(this.chkLine_CheckedChanged);
            // 
            // chkArea
            // 
            this.chkArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkArea.Location = new System.Drawing.Point(0, 242);
            this.chkArea.Name = "chkArea";
            this.chkArea.Size = new System.Drawing.Size(598, 24);
            this.chkArea.TabIndex = 4;
            this.chkArea.Text = "Display Areas";
            this.chkArea.UseVisualStyleBackColor = true;
            this.chkArea.CheckedChanged += new System.EventHandler(this.chkArea_CheckedChanged);
            // 
            // chkComposite
            // 
            this.chkComposite.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkComposite.Location = new System.Drawing.Point(0, 363);
            this.chkComposite.Name = "chkComposite";
            this.chkComposite.Size = new System.Drawing.Size(598, 24);
            this.chkComposite.TabIndex = 6;
            this.chkComposite.Text = "Display Composite";
            this.chkComposite.UseVisualStyleBackColor = true;
            this.chkComposite.CheckedChanged += new System.EventHandler(this.chkComposite_CheckedChanged);
            // 
            // compList
            // 
            this.compList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compList.Location = new System.Drawing.Point(0, 387);
            this.compList.Name = "compList";
            this.compList.Size = new System.Drawing.Size(598, 108);
            this.compList.TabIndex = 7;
            this.compList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // areaList
            // 
            this.areaList.Dock = System.Windows.Forms.DockStyle.Top;
            this.areaList.Location = new System.Drawing.Point(0, 266);
            this.areaList.Name = "areaList";
            this.areaList.Size = new System.Drawing.Size(598, 97);
            this.areaList.TabIndex = 5;
            this.areaList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // lineList
            // 
            this.lineList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineList.Location = new System.Drawing.Point(0, 145);
            this.lineList.Name = "lineList";
            this.lineList.Size = new System.Drawing.Size(598, 97);
            this.lineList.TabIndex = 3;
            this.lineList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // pointList
            // 
            this.pointList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pointList.Location = new System.Drawing.Point(0, 24);
            this.pointList.Name = "pointList";
            this.pointList.Size = new System.Drawing.Size(598, 97);
            this.pointList.TabIndex = 1;
            this.pointList.ItemChanged += new System.EventHandler(this.OnItemChanged);
            // 
            // VectorScaleRangeCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.compList);
            this.Controls.Add(this.chkComposite);
            this.Controls.Add(this.areaList);
            this.Controls.Add(this.chkArea);
            this.Controls.Add(this.lineList);
            this.Controls.Add(this.chkLine);
            this.Controls.Add(this.pointList);
            this.Controls.Add(this.chkPoint);
            this.Name = "VectorScaleRangeCtrl";
            this.Size = new System.Drawing.Size(598, 495);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPoint;
        private ConditionListButtons pointList;
        private System.Windows.Forms.CheckBox chkLine;
        private ConditionListButtons lineList;
        private System.Windows.Forms.CheckBox chkArea;
        private ConditionListButtons areaList;
        private System.Windows.Forms.CheckBox chkComposite;
        private ConditionListButtons compList;


    }
}
