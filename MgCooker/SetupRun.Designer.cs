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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.UseOfficialMethod = new System.Windows.Forms.CheckBox();
            this.MetersPerUnit = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.MaxColLimit = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseNativeAPI = new System.Windows.Forms.CheckBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.Username = new System.Windows.Forms.TextBox();
            this.MapAgent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MaxRowLimit = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LimitTileset = new System.Windows.Forms.CheckBox();
            this.TilesetLimitPanel = new System.Windows.Forms.Panel();
            this.OfficialMethodPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.RandomTileOrder = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetersPerUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxColLimit)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRowLimit)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.TilesetLimitPanel.SuspendLayout();
            this.OfficialMethodPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 417);
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
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(293, 417);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(293, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(272, 417);
            this.panel2.TabIndex = 2;
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
            // MaxColLimit
            // 
            this.MaxColLimit.Location = new System.Drawing.Point(104, 24);
            this.MaxColLimit.Name = "MaxColLimit";
            this.MaxColLimit.Size = new System.Drawing.Size(104, 20);
            this.MaxColLimit.TabIndex = 7;
            this.toolTip1.SetToolTip(this.MaxColLimit, "The maximum number of cols to generate tiles for");
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
            // MaxRowLimit
            // 
            this.MaxRowLimit.Location = new System.Drawing.Point(104, 0);
            this.MaxRowLimit.Name = "MaxRowLimit";
            this.MaxRowLimit.Size = new System.Drawing.Size(104, 20);
            this.MaxRowLimit.TabIndex = 6;
            this.toolTip1.SetToolTip(this.MaxRowLimit, "The maximum number of rows to generate tiles for");
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Max cols";
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
            // OfficialMethodPanel
            // 
            this.OfficialMethodPanel.Controls.Add(this.MetersPerUnit);
            this.OfficialMethodPanel.Controls.Add(this.label8);
            this.OfficialMethodPanel.Location = new System.Drawing.Point(24, 128);
            this.OfficialMethodPanel.Name = "OfficialMethodPanel";
            this.OfficialMethodPanel.Size = new System.Drawing.Size(216, 24);
            this.OfficialMethodPanel.TabIndex = 13;
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
            // SetupRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 458);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(449, 492);
            this.Name = "SetupRun";
            this.Text = "Setup a tile build";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MetersPerUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxColLimit)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRowLimit)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.TilesetLimitPanel.ResumeLayout(false);
            this.TilesetLimitPanel.PerformLayout();
            this.OfficialMethodPanel.ResumeLayout(false);
            this.OfficialMethodPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView1;
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
    }
}