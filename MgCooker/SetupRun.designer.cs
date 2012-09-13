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
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.MapTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
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
            this.grpDifferentConnection = new System.Windows.Forms.GroupBox();
            this.chkUseDifferentConnection = new System.Windows.Forms.CheckBox();
            this.UseNativeAPI = new System.Windows.Forms.CheckBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.Username = new System.Windows.Forms.TextBox();
            this.MapAgent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RandomTileOrder = new System.Windows.Forms.CheckBox();
            this.ThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OfficialMethodPanel = new System.Windows.Forms.Panel();
            this.MetersPerUnit = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.LimitTileset = new System.Windows.Forms.CheckBox();
            this.UseOfficialMethod = new System.Windows.Forms.CheckBox();
            this.TilesetLimitPanel = new System.Windows.Forms.Panel();
            this.MaxRowLimit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.MaxColLimit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.BoundsOverride.SuspendLayout();
            this.grpDifferentConnection.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.OfficialMethodPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetersPerUnit)).BeginInit();
            this.TilesetLimitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRowLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxColLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MapTree
            // 
            this.MapTree.CheckBoxes = true;
            resources.ApplyResources(this.MapTree, "MapTree");
            this.MapTree.ImageList = this.imageList1;
            this.MapTree.Name = "MapTree";
            this.MapTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.MapTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MapTree_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Map.ico");
            this.imageList1.Images.SetKeyName(1, "Layer.ico");
            this.imageList1.Images.SetKeyName(2, "Range.ico");
            this.imageList1.Images.SetKeyName(3, "Scale.ico");
            // 
            // saveFileDialog1
            // 
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BoundsOverride);
            this.panel2.Controls.Add(this.grpDifferentConnection);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
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
            // grpDifferentConnection
            // 
            resources.ApplyResources(this.grpDifferentConnection, "grpDifferentConnection");
            this.grpDifferentConnection.Controls.Add(this.chkUseDifferentConnection);
            this.grpDifferentConnection.Controls.Add(this.UseNativeAPI);
            this.grpDifferentConnection.Controls.Add(this.Password);
            this.grpDifferentConnection.Controls.Add(this.Username);
            this.grpDifferentConnection.Controls.Add(this.MapAgent);
            this.grpDifferentConnection.Controls.Add(this.label3);
            this.grpDifferentConnection.Controls.Add(this.label2);
            this.grpDifferentConnection.Controls.Add(this.label1);
            this.grpDifferentConnection.Name = "grpDifferentConnection";
            this.grpDifferentConnection.TabStop = false;
            // 
            // chkUseDifferentConnection
            // 
            resources.ApplyResources(this.chkUseDifferentConnection, "chkUseDifferentConnection");
            this.chkUseDifferentConnection.Name = "chkUseDifferentConnection";
            this.chkUseDifferentConnection.UseVisualStyleBackColor = true;
            // 
            // UseNativeAPI
            // 
            resources.ApplyResources(this.UseNativeAPI, "UseNativeAPI");
            this.UseNativeAPI.Name = "UseNativeAPI";
            this.toolTip1.SetToolTip(this.UseNativeAPI, resources.GetString("UseNativeAPI.ToolTip"));
            this.UseNativeAPI.UseVisualStyleBackColor = true;
            // 
            // Password
            // 
            resources.ApplyResources(this.Password, "Password");
            this.Password.Name = "Password";
            this.toolTip1.SetToolTip(this.Password, resources.GetString("Password.ToolTip"));
            this.Password.UseSystemPasswordChar = true;
            // 
            // Username
            // 
            resources.ApplyResources(this.Username, "Username");
            this.Username.Name = "Username";
            this.toolTip1.SetToolTip(this.Username, resources.GetString("Username.ToolTip"));
            // 
            // MapAgent
            // 
            resources.ApplyResources(this.MapAgent, "MapAgent");
            this.MapAgent.Name = "MapAgent";
            this.toolTip1.SetToolTip(this.MapAgent, resources.GetString("MapAgent.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.RandomTileOrder);
            this.groupBox3.Controls.Add(this.ThreadCount);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // RandomTileOrder
            // 
            resources.ApplyResources(this.RandomTileOrder, "RandomTileOrder");
            this.RandomTileOrder.Name = "RandomTileOrder";
            this.toolTip1.SetToolTip(this.RandomTileOrder, resources.GetString("RandomTileOrder.ToolTip"));
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
            this.toolTip1.SetToolTip(this.ThreadCount, resources.GetString("ThreadCount.ToolTip"));
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
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.OfficialMethodPanel);
            this.groupBox2.Controls.Add(this.LimitTileset);
            this.groupBox2.Controls.Add(this.UseOfficialMethod);
            this.groupBox2.Controls.Add(this.TilesetLimitPanel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
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
            this.toolTip1.SetToolTip(this.MetersPerUnit, resources.GetString("MetersPerUnit.ToolTip"));
            this.MetersPerUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MetersPerUnit.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // LimitTileset
            // 
            resources.ApplyResources(this.LimitTileset, "LimitTileset");
            this.LimitTileset.Name = "LimitTileset";
            this.toolTip1.SetToolTip(this.LimitTileset, resources.GetString("LimitTileset.ToolTip"));
            this.LimitTileset.UseVisualStyleBackColor = true;
            this.LimitTileset.CheckedChanged += new System.EventHandler(this.LimitTileset_CheckedChanged);
            // 
            // UseOfficialMethod
            // 
            resources.ApplyResources(this.UseOfficialMethod, "UseOfficialMethod");
            this.UseOfficialMethod.Name = "UseOfficialMethod";
            this.toolTip1.SetToolTip(this.UseOfficialMethod, resources.GetString("UseOfficialMethod.ToolTip"));
            this.UseOfficialMethod.UseVisualStyleBackColor = true;
            this.UseOfficialMethod.CheckedChanged += new System.EventHandler(this.UseOfficialMethod_CheckedChanged);
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
            this.toolTip1.SetToolTip(this.MaxRowLimit, resources.GetString("MaxRowLimit.ToolTip"));
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
            this.toolTip1.SetToolTip(this.MaxColLimit, resources.GetString("MaxColLimit.ToolTip"));
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
            this.grpDifferentConnection.ResumeLayout(false);
            this.grpDifferentConnection.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView MapTree;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpDifferentConnection;
        private System.Windows.Forms.CheckBox UseNativeAPI;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox MapAgent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown ThreadCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox LimitTileset;
        private System.Windows.Forms.CheckBox UseOfficialMethod;
        private System.Windows.Forms.NumericUpDown MetersPerUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown MaxColLimit;
        private System.Windows.Forms.NumericUpDown MaxRowLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel OfficialMethodPanel;
        private System.Windows.Forms.Panel TilesetLimitPanel;
        private System.Windows.Forms.ToolTip toolTip1;
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
        private System.Windows.Forms.CheckBox chkUseDifferentConnection;
        private System.Windows.Forms.TextBox Username;
    }
}