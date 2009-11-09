namespace OSGeo.MapGuide.MgCooker
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.groupBox1.SuspendLayout();
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 537);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(565, 41);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button3.Location = new System.Drawing.Point(227, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Save as script...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(355, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(99, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Build tiles now";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MapTree
            // 
            this.MapTree.CheckBoxes = true;
            this.MapTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapTree.ImageIndex = 0;
            this.MapTree.ImageList = this.imageList1;
            this.MapTree.Location = new System.Drawing.Point(0, 0);
            this.MapTree.Name = "MapTree";
            this.MapTree.SelectedImageIndex = 0;
            this.MapTree.Size = new System.Drawing.Size(293, 537);
            this.MapTree.TabIndex = 1;
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
            this.saveFileDialog1.Filter = "Batch Files (*.bat)|*.bat|All files (*.*)|*.*";
            this.saveFileDialog1.Title = "Select file to save script to";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BoundsOverride);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(293, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(272, 537);
            this.panel2.TabIndex = 2;
            // 
            // BoundsOverride
            // 
            this.BoundsOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.BoundsOverride.Enabled = false;
            this.BoundsOverride.Location = new System.Drawing.Point(12, 424);
            this.BoundsOverride.Name = "BoundsOverride";
            this.BoundsOverride.Size = new System.Drawing.Size(248, 120);
            this.BoundsOverride.TabIndex = 12;
            this.BoundsOverride.TabStop = false;
            this.BoundsOverride.Text = "Override bounds";
            // 
            // ModfiedOverrideWarning
            // 
            this.ModfiedOverrideWarning.AutoSize = true;
            this.ModfiedOverrideWarning.Location = new System.Drawing.Point(8, 96);
            this.ModfiedOverrideWarning.Name = "ModfiedOverrideWarning";
            this.ModfiedOverrideWarning.Size = new System.Drawing.Size(105, 13);
            this.ModfiedOverrideWarning.TabIndex = 19;
            this.ModfiedOverrideWarning.Text = "Coordinates modified";
            this.ModfiedOverrideWarning.Visible = false;
            // 
            // ResetBounds
            // 
            this.ResetBounds.Location = new System.Drawing.Point(136, 88);
            this.ResetBounds.Name = "ResetBounds";
            this.ResetBounds.Size = new System.Drawing.Size(88, 24);
            this.ResetBounds.TabIndex = 18;
            this.ResetBounds.Text = "Reset";
            this.ResetBounds.UseVisualStyleBackColor = true;
            this.ResetBounds.Click += new System.EventHandler(this.ResetBounds_Click);
            // 
            // txtUpperY
            // 
            this.txtUpperY.Location = new System.Drawing.Point(144, 56);
            this.txtUpperY.Name = "txtUpperY";
            this.txtUpperY.Size = new System.Drawing.Size(80, 20);
            this.txtUpperY.TabIndex = 17;
            this.txtUpperY.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(120, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Y";
            // 
            // txtUpperX
            // 
            this.txtUpperX.Location = new System.Drawing.Point(32, 56);
            this.txtUpperX.Name = "txtUpperX";
            this.txtUpperX.Size = new System.Drawing.Size(72, 20);
            this.txtUpperX.TabIndex = 15;
            this.txtUpperX.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(8, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "X";
            // 
            // txtLowerY
            // 
            this.txtLowerY.Location = new System.Drawing.Point(144, 24);
            this.txtLowerY.Name = "txtLowerY";
            this.txtLowerY.Size = new System.Drawing.Size(80, 20);
            this.txtLowerY.TabIndex = 13;
            this.txtLowerY.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Location = new System.Drawing.Point(120, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Y";
            // 
            // txtLowerX
            // 
            this.txtLowerX.Location = new System.Drawing.Point(32, 24);
            this.txtLowerX.Name = "txtLowerX";
            this.txtLowerX.Size = new System.Drawing.Size(72, 20);
            this.txtLowerX.TabIndex = 11;
            this.txtLowerX.TextChanged += new System.EventHandler(this.CoordinateItem_TextChanged);
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label11.Location = new System.Drawing.Point(8, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 16);
            this.label11.TabIndex = 10;
            this.label11.Text = "X";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.UseNativeAPI);
            this.groupBox1.Controls.Add(this.Password);
            this.groupBox1.Controls.Add(this.Username);
            this.groupBox1.Controls.Add(this.MapAgent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 128);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MapAgent";
            // 
            // UseNativeAPI
            // 
            this.UseNativeAPI.AutoSize = true;
            this.UseNativeAPI.Location = new System.Drawing.Point(88, 104);
            this.UseNativeAPI.Name = "UseNativeAPI";
            this.UseNativeAPI.Size = new System.Drawing.Size(146, 17);
            this.UseNativeAPI.TabIndex = 6;
            this.UseNativeAPI.Text = "Connect using native API";
            this.toolTip1.SetToolTip(this.UseNativeAPI, "Using the Native API is potentially faster, but requires binary files, open ports" +
                    " and a webconfig.ini file");
            this.UseNativeAPI.UseVisualStyleBackColor = true;
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(88, 72);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(144, 20);
            this.Password.TabIndex = 5;
            this.toolTip1.SetToolTip(this.Password, "Password used to connect to the server");
            this.Password.UseSystemPasswordChar = true;
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(88, 48);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(144, 20);
            this.Username.TabIndex = 4;
            this.Username.Text = "Anonymous";
            this.toolTip1.SetToolTip(this.Username, "Username used to connect to the server");
            // 
            // MapAgent
            // 
            this.MapAgent.Location = new System.Drawing.Point(88, 24);
            this.MapAgent.Name = "MapAgent";
            this.MapAgent.Size = new System.Drawing.Size(144, 20);
            this.MapAgent.TabIndex = 3;
            this.MapAgent.Text = "http://localhost/mapguide/mapagent/mapagent.fcgi";
            this.toolTip1.SetToolTip(this.MapAgent, "Enter the URL for the MapAgent");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MapAgent";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.RandomTileOrder);
            this.groupBox3.Controls.Add(this.ThreadCount);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 80);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Threading";
            // 
            // RandomTileOrder
            // 
            this.RandomTileOrder.AutoSize = true;
            this.RandomTileOrder.Location = new System.Drawing.Point(24, 48);
            this.RandomTileOrder.Name = "RandomTileOrder";
            this.RandomTileOrder.Size = new System.Drawing.Size(159, 17);
            this.RandomTileOrder.TabIndex = 11;
            this.RandomTileOrder.Text = "Randomize generation order";
            this.toolTip1.SetToolTip(this.RandomTileOrder, "Select tiles at random, rather than sequentially");
            this.RandomTileOrder.UseVisualStyleBackColor = true;
            // 
            // ThreadCount
            // 
            this.ThreadCount.Location = new System.Drawing.Point(128, 24);
            this.ThreadCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.ThreadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadCount.Name = "ThreadCount";
            this.ThreadCount.Size = new System.Drawing.Size(104, 20);
            this.ThreadCount.TabIndex = 5;
            this.toolTip1.SetToolTip(this.ThreadCount, "Number of concurrent request to server");
            this.ThreadCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Concurrent requests";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.OfficialMethodPanel);
            this.groupBox2.Controls.Add(this.LimitTileset);
            this.groupBox2.Controls.Add(this.UseOfficialMethod);
            this.groupBox2.Controls.Add(this.TilesetLimitPanel);
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 168);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tilesettings";
            // 
            // OfficialMethodPanel
            // 
            this.OfficialMethodPanel.Controls.Add(this.MetersPerUnit);
            this.OfficialMethodPanel.Controls.Add(this.label8);
            this.OfficialMethodPanel.Enabled = false;
            this.OfficialMethodPanel.Location = new System.Drawing.Point(24, 128);
            this.OfficialMethodPanel.Name = "OfficialMethodPanel";
            this.OfficialMethodPanel.Size = new System.Drawing.Size(216, 24);
            this.OfficialMethodPanel.TabIndex = 13;
            // 
            // MetersPerUnit
            // 
            this.MetersPerUnit.DecimalPlaces = 4;
            this.MetersPerUnit.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.MetersPerUnit.Location = new System.Drawing.Point(104, 0);
            this.MetersPerUnit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MetersPerUnit.Name = "MetersPerUnit";
            this.MetersPerUnit.Size = new System.Drawing.Size(104, 20);
            this.MetersPerUnit.TabIndex = 9;
            this.toolTip1.SetToolTip(this.MetersPerUnit, "The number of meters pr. map unit");
            this.MetersPerUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MetersPerUnit.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Meters pr. unit";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // LimitTileset
            // 
            this.LimitTileset.AutoSize = true;
            this.LimitTileset.Location = new System.Drawing.Point(24, 24);
            this.LimitTileset.Name = "LimitTileset";
            this.LimitTileset.Size = new System.Drawing.Size(118, 17);
            this.LimitTileset.TabIndex = 11;
            this.LimitTileset.Text = "Limit number of tiles";
            this.toolTip1.SetToolTip(this.LimitTileset, "Set a limit on the number of tiles generated, note that this may prevent all tile" +
                    "s from being created");
            this.LimitTileset.UseVisualStyleBackColor = true;
            this.LimitTileset.CheckedChanged += new System.EventHandler(this.LimitTileset_CheckedChanged);
            // 
            // UseOfficialMethod
            // 
            this.UseOfficialMethod.AutoSize = true;
            this.UseOfficialMethod.Location = new System.Drawing.Point(24, 104);
            this.UseOfficialMethod.Name = "UseOfficialMethod";
            this.UseOfficialMethod.Size = new System.Drawing.Size(116, 17);
            this.UseOfficialMethod.TabIndex = 10;
            this.UseOfficialMethod.Text = "Use official method";
            this.toolTip1.SetToolTip(this.UseOfficialMethod, "The official method is the most accurate, but requires that the meters per map un" +
                    "it is entered");
            this.UseOfficialMethod.UseVisualStyleBackColor = true;
            this.UseOfficialMethod.CheckedChanged += new System.EventHandler(this.UseOfficialMethod_CheckedChanged);
            // 
            // TilesetLimitPanel
            // 
            this.TilesetLimitPanel.Controls.Add(this.MaxRowLimit);
            this.TilesetLimitPanel.Controls.Add(this.label6);
            this.TilesetLimitPanel.Controls.Add(this.MaxColLimit);
            this.TilesetLimitPanel.Controls.Add(this.label7);
            this.TilesetLimitPanel.Enabled = false;
            this.TilesetLimitPanel.Location = new System.Drawing.Point(24, 48);
            this.TilesetLimitPanel.Name = "TilesetLimitPanel";
            this.TilesetLimitPanel.Size = new System.Drawing.Size(216, 48);
            this.TilesetLimitPanel.TabIndex = 12;
            // 
            // MaxRowLimit
            // 
            this.MaxRowLimit.Location = new System.Drawing.Point(104, 0);
            this.MaxRowLimit.Name = "MaxRowLimit";
            this.MaxRowLimit.Size = new System.Drawing.Size(104, 20);
            this.MaxRowLimit.TabIndex = 6;
            this.toolTip1.SetToolTip(this.MaxRowLimit, "The maximum number of rows to generate tiles for");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Max rows";
            // 
            // MaxColLimit
            // 
            this.MaxColLimit.Location = new System.Drawing.Point(104, 24);
            this.MaxColLimit.Name = "MaxColLimit";
            this.MaxColLimit.Size = new System.Drawing.Size(104, 20);
            this.MaxColLimit.TabIndex = 7;
            this.toolTip1.SetToolTip(this.MaxColLimit, "The maximum number of cols to generate tiles for");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Max cols";
            // 
            // SetupRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 578);
            this.Controls.Add(this.MapTree);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(581, 598);
            this.Name = "SetupRun";
            this.Text = "Setup a tile build";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.BoundsOverride.ResumeLayout(false);
            this.BoundsOverride.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox UseNativeAPI;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox Username;
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
    }
}