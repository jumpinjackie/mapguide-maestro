namespace Maestro.Editors.Common
{
    partial class CoordinateSystemPicker
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateSystemPicker));
            this.SelectByList = new System.Windows.Forms.RadioButton();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SelectByEPSGCode = new System.Windows.Forms.RadioButton();
            this.SelectByEPSGCodeGroup = new System.Windows.Forms.GroupBox();
            this.EPSGCodeText = new System.Windows.Forms.ComboBox();
            this.ValidateEPSG = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectByCoordSysCode = new System.Windows.Forms.RadioButton();
            this.SelectByCoordSysCodeGroup = new System.Windows.Forms.GroupBox();
            this.CoordSysCodeText = new System.Windows.Forms.ComboBox();
            this.ValidateCoordSysCode = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectByWKT = new System.Windows.Forms.RadioButton();
            this.SelectByWKTGroup = new System.Windows.Forms.GroupBox();
            this.ValidateWKT = new System.Windows.Forms.Button();
            this.WKTText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectByListGroup = new System.Windows.Forms.GroupBox();
            this.CoordinateSystem = new System.Windows.Forms.ComboBox();
            this.CoordinateCategory = new System.Windows.Forms.ComboBox();
            this.CoordinateSystemLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CoordinateWait = new System.Windows.Forms.Label();
            this.SelectByEPSGCodeGroup.SuspendLayout();
            this.SelectByCoordSysCodeGroup.SuspendLayout();
            this.SelectByWKTGroup.SuspendLayout();
            this.SelectByListGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectByList
            // 
            this.SelectByList.Checked = true;
            resources.ApplyResources(this.SelectByList, "SelectByList");
            this.SelectByList.Name = "SelectByList";
            this.SelectByList.TabStop = true;
            this.SelectByList.CheckedChanged += new System.EventHandler(this.SelectByList_CheckedChanged);
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // SelectByEPSGCode
            // 
            resources.ApplyResources(this.SelectByEPSGCode, "SelectByEPSGCode");
            this.SelectByEPSGCode.Name = "SelectByEPSGCode";
            this.SelectByEPSGCode.CheckedChanged += new System.EventHandler(this.SelectByEPSGCode_CheckedChanged);
            // 
            // SelectByEPSGCodeGroup
            // 
            resources.ApplyResources(this.SelectByEPSGCodeGroup, "SelectByEPSGCodeGroup");
            this.SelectByEPSGCodeGroup.Controls.Add(this.EPSGCodeText);
            this.SelectByEPSGCodeGroup.Controls.Add(this.ValidateEPSG);
            this.SelectByEPSGCodeGroup.Controls.Add(this.label4);
            this.SelectByEPSGCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByEPSGCodeGroup.Name = "SelectByEPSGCodeGroup";
            this.SelectByEPSGCodeGroup.TabStop = false;
            // 
            // EPSGCodeText
            // 
            resources.ApplyResources(this.EPSGCodeText, "EPSGCodeText");
            this.EPSGCodeText.Name = "EPSGCodeText";
            this.EPSGCodeText.TextChanged += new System.EventHandler(this.EPSGCodeText_TextChanged);
            // 
            // ValidateEPSG
            // 
            resources.ApplyResources(this.ValidateEPSG, "ValidateEPSG");
            this.ValidateEPSG.Name = "ValidateEPSG";
            this.ValidateEPSG.Click += new System.EventHandler(this.ValidateEPSG_Click);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // SelectByCoordSysCode
            // 
            resources.ApplyResources(this.SelectByCoordSysCode, "SelectByCoordSysCode");
            this.SelectByCoordSysCode.Name = "SelectByCoordSysCode";
            this.SelectByCoordSysCode.CheckedChanged += new System.EventHandler(this.SelectByCoordSysCode_CheckedChanged);
            // 
            // SelectByCoordSysCodeGroup
            // 
            resources.ApplyResources(this.SelectByCoordSysCodeGroup, "SelectByCoordSysCodeGroup");
            this.SelectByCoordSysCodeGroup.Controls.Add(this.CoordSysCodeText);
            this.SelectByCoordSysCodeGroup.Controls.Add(this.ValidateCoordSysCode);
            this.SelectByCoordSysCodeGroup.Controls.Add(this.label5);
            this.SelectByCoordSysCodeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByCoordSysCodeGroup.Name = "SelectByCoordSysCodeGroup";
            this.SelectByCoordSysCodeGroup.TabStop = false;
            // 
            // CoordSysCodeText
            // 
            resources.ApplyResources(this.CoordSysCodeText, "CoordSysCodeText");
            this.CoordSysCodeText.Name = "CoordSysCodeText";
            this.CoordSysCodeText.TextChanged += new System.EventHandler(this.CoordSysCodeText_TextChanged);
            // 
            // ValidateCoordSysCode
            // 
            resources.ApplyResources(this.ValidateCoordSysCode, "ValidateCoordSysCode");
            this.ValidateCoordSysCode.Name = "ValidateCoordSysCode";
            this.ValidateCoordSysCode.Click += new System.EventHandler(this.ValidateCoordSysCode_Click);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // SelectByWKT
            // 
            resources.ApplyResources(this.SelectByWKT, "SelectByWKT");
            this.SelectByWKT.Name = "SelectByWKT";
            this.SelectByWKT.CheckedChanged += new System.EventHandler(this.SelectByWKT_CheckedChanged);
            // 
            // SelectByWKTGroup
            // 
            resources.ApplyResources(this.SelectByWKTGroup, "SelectByWKTGroup");
            this.SelectByWKTGroup.Controls.Add(this.ValidateWKT);
            this.SelectByWKTGroup.Controls.Add(this.WKTText);
            this.SelectByWKTGroup.Controls.Add(this.label3);
            this.SelectByWKTGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByWKTGroup.Name = "SelectByWKTGroup";
            this.SelectByWKTGroup.TabStop = false;
            // 
            // ValidateWKT
            // 
            resources.ApplyResources(this.ValidateWKT, "ValidateWKT");
            this.ValidateWKT.Name = "ValidateWKT";
            this.ValidateWKT.Click += new System.EventHandler(this.ValidateWKT_Click);
            // 
            // WKTText
            // 
            resources.ApplyResources(this.WKTText, "WKTText");
            this.WKTText.Name = "WKTText";
            this.WKTText.TextChanged += new System.EventHandler(this.WKTText_TextChanged);
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SelectByListGroup
            // 
            resources.ApplyResources(this.SelectByListGroup, "SelectByListGroup");
            this.SelectByListGroup.Controls.Add(this.CoordinateSystem);
            this.SelectByListGroup.Controls.Add(this.CoordinateCategory);
            this.SelectByListGroup.Controls.Add(this.CoordinateSystemLabel);
            this.SelectByListGroup.Controls.Add(this.label1);
            this.SelectByListGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectByListGroup.Name = "SelectByListGroup";
            this.SelectByListGroup.TabStop = false;
            // 
            // CoordinateSystem
            // 
            resources.ApplyResources(this.CoordinateSystem, "CoordinateSystem");
            this.CoordinateSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CoordinateSystem.Name = "CoordinateSystem";
            this.CoordinateSystem.SelectedIndexChanged += new System.EventHandler(this.CoordinateSystem_SelectedIndexChanged);
            // 
            // CoordinateCategory
            // 
            resources.ApplyResources(this.CoordinateCategory, "CoordinateCategory");
            this.CoordinateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CoordinateCategory.Name = "CoordinateCategory";
            this.CoordinateCategory.SelectedIndexChanged += new System.EventHandler(this.CoordinateCategory_SelectedIndexChanged);
            // 
            // CoordinateSystemLabel
            // 
            resources.ApplyResources(this.CoordinateSystemLabel, "CoordinateSystemLabel");
            this.CoordinateSystemLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CoordinateSystemLabel.Name = "CoordinateSystemLabel";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CoordinateWait
            // 
            resources.ApplyResources(this.CoordinateWait, "CoordinateWait");
            this.CoordinateWait.Name = "CoordinateWait";
            // 
            // CoordinateSystemPicker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.SelectByList);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SelectByEPSGCode);
            this.Controls.Add(this.SelectByEPSGCodeGroup);
            this.Controls.Add(this.SelectByCoordSysCode);
            this.Controls.Add(this.SelectByCoordSysCodeGroup);
            this.Controls.Add(this.SelectByWKT);
            this.Controls.Add(this.SelectByWKTGroup);
            this.Controls.Add(this.SelectByListGroup);
            this.Controls.Add(this.CoordinateWait);
            this.Name = "CoordinateSystemPicker";
            this.SelectByEPSGCodeGroup.ResumeLayout(false);
            this.SelectByCoordSysCodeGroup.ResumeLayout(false);
            this.SelectByWKTGroup.ResumeLayout(false);
            this.SelectByWKTGroup.PerformLayout();
            this.SelectByListGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton SelectByList;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.RadioButton SelectByEPSGCode;
        private System.Windows.Forms.GroupBox SelectByEPSGCodeGroup;
        private System.Windows.Forms.ComboBox EPSGCodeText;
        private System.Windows.Forms.Button ValidateEPSG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton SelectByCoordSysCode;
        private System.Windows.Forms.GroupBox SelectByCoordSysCodeGroup;
        private System.Windows.Forms.ComboBox CoordSysCodeText;
        private System.Windows.Forms.Button ValidateCoordSysCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton SelectByWKT;
        private System.Windows.Forms.GroupBox SelectByWKTGroup;
        private System.Windows.Forms.Button ValidateWKT;
        private System.Windows.Forms.TextBox WKTText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox SelectByListGroup;
        private System.Windows.Forms.ComboBox CoordinateSystem;
        private System.Windows.Forms.ComboBox CoordinateCategory;
        private System.Windows.Forms.Label CoordinateSystemLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CoordinateWait;
    }
}