namespace MgCooker
{
    partial class SetupRun
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupRun));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveScript = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.MapTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BoundsOverride = new System.Windows.Forms.GroupBox();
            this.ModfiedOverrideWarning = new System.Windows.Forms.Label();
            this.ResetBounds = new System.Windows.Forms.Button();
            this.txtUpperY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUpperX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLowerY = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLowerX = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.grpGlobalSettings = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.grpThreading = new System.Windows.Forms.GroupBox();
            this.RandomTileOrder = new System.Windows.Forms.CheckBox();
            this.ThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpTileSettings = new System.Windows.Forms.GroupBox();
            this.lnkCalcMpu = new System.Windows.Forms.LinkLabel();
            this.OfficialMethodPanel = new System.Windows.Forms.Panel();
            this.MetersPerUnit = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.LimitTileset = new System.Windows.Forms.CheckBox();
            this.TilesetLimitPanel = new System.Windows.Forms.Panel();
            this.MaxRowLimit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.MaxColLimit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.BoundsOverride.SuspendLayout();
            this.grpGlobalSettings.SuspendLayout();
            this.grpThreading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).BeginInit();
            this.grpTileSettings.SuspendLayout();
            this.OfficialMethodPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetersPerUnit)).BeginInit();
            this.TilesetLimitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRowLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxColLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveScript);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnBuild);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnSaveScript
            // 
            resources.ApplyResources(this.btnSaveScript, "btnSaveScript");
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.UseVisualStyleBackColor = true;
            this.btnSaveScript.Click += new System.EventHandler(this.btnSaveScript_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnBuild
            // 
            resources.ApplyResources(this.btnBuild, "btnBuild");
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // MapTree
            // 
            this.MapTree.CheckBoxes = true;
            resources.ApplyResources(this.MapTree, "MapTree");
            this.MapTree.ImageList = this.imageList;
            this.MapTree.Name = "MapTree";
            this.MapTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.MapTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MapTree_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Map.ico");
            this.imageList.Images.SetKeyName(1, "Layer.ico");
            this.imageList.Images.SetKeyName(2, "Range.ico");
            this.imageList.Images.SetKeyName(3, "Scale.ico");
            // 
            // saveFileDialog
            // 
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BoundsOverride);
            this.panel2.Controls.Add(this.grpGlobalSettings);
            this.panel2.Controls.Add(this.grpTileSettings);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // BoundsOverride
            // 
            resources.ApplyResources(this.BoundsOverride, "BoundsOverride");
            this.BoundsOverride.Controls.Add(this.ModfiedOverrideWarning);
            this.BoundsOverride.Controls.Add(this.ResetBounds);
            this.BoundsOverride.Controls.Add(this.txtUpperY);
            this.BoundsOverride.Controls.Add(this.label4);
            this.BoundsOverride.Controls.Add(this.txtUpperX);
            this.BoundsOverride.Controls.Add(this.label5);
            this.BoundsOverride.Controls.Add(this.txtLowerY);
            this.BoundsOverride.Controls.Add(this.label10);
            this.BoundsOverride.Controls.Add(this.txtLowerX);
            this.BoundsOverride.Controls.Add(this.label11);
            this.BoundsOverride.Name = "BoundsOverride";
            this.BoundsOverride.TabStop = false;
            // 
            // ModfiedOverrideWarning
            // 
            resources.ApplyResources(this.ModfiedOverrideWarning, "ModfiedOverrideWarning");
            this.ModfiedOverrideWarning.ForeColor = System.Drawing.Color.DarkRed;
            this.ModfiedOverrideWarning.Name = "ModfiedOverrideWarning";
            // 
            // ResetBounds
            // 
            resources.ApplyResources(this.ResetBounds, "ResetBounds");
            this.ResetBounds.Name = "ResetBounds";
            this.ResetBounds.UseVisualStyleBackColor = true;
            this.ResetBounds.Click += new System.EventHandler(this.ResetBounds_Click);
            // 
            // txtUpperY
            // 
            resources.ApplyResources(this.txtUpperY, "txtUpperY");
            this.txtUpperY.Name = "txtUpperY";
            this.txtUpperY.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtUpperX
            // 
            resources.ApplyResources(this.txtUpperX, "txtUpperX");
            this.txtUpperX.Name = "txtUpperX";
            this.txtUpperX.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtLowerY
            // 
            resources.ApplyResources(this.txtLowerY, "txtLowerY");
            this.txtLowerY.Name = "txtLowerY";
            this.txtLowerY.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txtLowerX
            // 
            resources.ApplyResources(this.txtLowerX, "txtLowerX");
            this.txtLowerX.Name = "txtLowerX";
            this.txtLowerX.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // grpGlobalSettings
            // 
            resources.ApplyResources(this.grpGlobalSettings, "grpGlobalSettings");
            this.grpGlobalSettings.Controls.Add(this.txtConnectionString);
            this.grpGlobalSettings.Controls.Add(this.txtProvider);
            this.grpGlobalSettings.Controls.Add(this.grpThreading);
            this.grpGlobalSettings.Controls.Add(this.label2);
            this.grpGlobalSettings.Controls.Add(this.label1);
            this.grpGlobalSettings.Name = "grpGlobalSettings";
            this.grpGlobalSettings.TabStop = false;
            this.toolTip.SetToolTip(this.grpGlobalSettings, resources.GetString("grpGlobalSettings.ToolTip"));
            // 
            // txtConnectionString
            // 
            resources.ApplyResources(this.txtConnectionString, "txtConnectionString");
            this.txtConnectionString.Name = "txtConnectionString";
            // 
            // txtProvider
            // 
            resources.ApplyResources(this.txtProvider, "txtProvider");
            this.txtProvider.Name = "txtProvider";
            // 
            // grpThreading
            // 
            resources.ApplyResources(this.grpThreading, "grpThreading");
            this.grpThreading.Controls.Add(this.RandomTileOrder);
            this.grpThreading.Controls.Add(this.ThreadCount);
            this.grpThreading.Controls.Add(this.label9);
            this.grpThreading.Name = "grpThreading";
            this.grpThreading.TabStop = false;
            // 
            // RandomTileOrder
            // 
            resources.ApplyResources(this.RandomTileOrder, "RandomTileOrder");
            this.RandomTileOrder.Name = "RandomTileOrder";
            this.toolTip.SetToolTip(this.RandomTileOrder, resources.GetString("RandomTileOrder.ToolTip"));
            this.RandomTileOrder.UseVisualStyleBackColor = true;
            // 
            // ThreadCount
            // 
            resources.ApplyResources(this.ThreadCount, "ThreadCount");
            this.ThreadCount.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.ThreadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadCount.Name = "ThreadCount";
            this.toolTip.SetToolTip(this.ThreadCount, resources.GetString("ThreadCount.ToolTip"));
            this.ThreadCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // grpTileSettings
            // 
            resources.ApplyResources(this.grpTileSettings, "grpTileSettings");
            this.grpTileSettings.Controls.Add(this.lnkCalcMpu);
            this.grpTileSettings.Controls.Add(this.OfficialMethodPanel);
            this.grpTileSettings.Controls.Add(this.LimitTileset);
            this.grpTileSettings.Controls.Add(this.TilesetLimitPanel);
            this.grpTileSettings.Name = "grpTileSettings";
            this.grpTileSettings.TabStop = false;
            // 
            // lnkCalcMpu
            // 
            resources.ApplyResources(this.lnkCalcMpu, "lnkCalcMpu");
            this.lnkCalcMpu.Name = "lnkCalcMpu";
            this.lnkCalcMpu.TabStop = true;
            this.lnkCalcMpu.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCalcMpu_LinkClicked);
            // 
            // OfficialMethodPanel
            // 
            this.OfficialMethodPanel.Controls.Add(this.MetersPerUnit);
            this.OfficialMethodPanel.Controls.Add(this.label8);
            resources.ApplyResources(this.OfficialMethodPanel, "OfficialMethodPanel");
            this.OfficialMethodPanel.Name = "OfficialMethodPanel";
            // 
            // MetersPerUnit
            // 
            this.MetersPerUnit.DecimalPlaces = 4;
            this.MetersPerUnit.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.MetersPerUnit, "MetersPerUnit");
            this.MetersPerUnit.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.MetersPerUnit.Name = "MetersPerUnit";
            this.toolTip.SetToolTip(this.MetersPerUnit, resources.GetString("MetersPerUnit.ToolTip"));
            this.MetersPerUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // LimitTileset
            // 
            resources.ApplyResources(this.LimitTileset, "LimitTileset");
            this.LimitTileset.Name = "LimitTileset";
            this.toolTip.SetToolTip(this.LimitTileset, resources.GetString("LimitTileset.ToolTip"));
            this.LimitTileset.UseVisualStyleBackColor = true;
            this.LimitTileset.CheckedChanged += new System.EventHandler(this.LimitTileset_CheckedChanged);
            // 
            // TilesetLimitPanel
            // 
            this.TilesetLimitPanel.Controls.Add(this.MaxRowLimit);
            this.TilesetLimitPanel.Controls.Add(this.label6);
            this.TilesetLimitPanel.Controls.Add(this.MaxColLimit);
            this.TilesetLimitPanel.Controls.Add(this.label7);
            resources.ApplyResources(this.TilesetLimitPanel, "TilesetLimitPanel");
            this.TilesetLimitPanel.Name = "TilesetLimitPanel";
            // 
            // MaxRowLimit
            // 
            resources.ApplyResources(this.MaxRowLimit, "MaxRowLimit");
            this.MaxRowLimit.Name = "MaxRowLimit";
            this.toolTip.SetToolTip(this.MaxRowLimit, resources.GetString("MaxRowLimit.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // MaxColLimit
            // 
            resources.ApplyResources(this.MaxColLimit, "MaxColLimit");
            this.MaxColLimit.Name = "MaxColLimit";
            this.toolTip.SetToolTip(this.MaxColLimit, resources.GetString("MaxColLimit.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // SetupRun
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.MapTree);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SetupRun";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.BoundsOverride.ResumeLayout(false);
            this.BoundsOverride.PerformLayout();
            this.grpGlobalSettings.ResumeLayout(false);
            this.grpGlobalSettings.PerformLayout();
            this.grpThreading.ResumeLayout(false);
            this.grpThreading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).EndInit();
            this.grpTileSettings.ResumeLayout(false);
            this.grpTileSettings.PerformLayout();
            this.OfficialMethodPanel.ResumeLayout(false);
            this.OfficialMethodPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetersPerUnit)).EndInit();
            this.TilesetLimitPanel.ResumeLayout(false);
            this.TilesetLimitPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRowLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxColLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.TreeView MapTree;
        private System.Windows.Forms.Button btnSaveScript;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpGlobalSettings;
        private System.Windows.Forms.GroupBox grpThreading;
        private System.Windows.Forms.NumericUpDown ThreadCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grpTileSettings;
        private System.Windows.Forms.CheckBox LimitTileset;
        private System.Windows.Forms.NumericUpDown MetersPerUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown MaxColLimit;
        private System.Windows.Forms.NumericUpDown MaxRowLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel OfficialMethodPanel;
        private System.Windows.Forms.Panel TilesetLimitPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox RandomTileOrder;
        private System.Windows.Forms.GroupBox BoundsOverride;
        private System.Windows.Forms.TextBox txtUpperY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUpperX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLowerY;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLowerX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button ResetBounds;
        private System.Windows.Forms.Label ModfiedOverrideWarning;
        private System.Windows.Forms.LinkLabel lnkCalcMpu;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}