namespace OSGeo.MapGuide.Maestro
{
    partial class ResourceProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceProperties));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SecurityTab = new System.Windows.Forms.TabPage();
            this.UseInherited = new System.Windows.Forms.CheckBox();
            this.UsersAndGroups = new System.Windows.Forms.ListView();
            this.UserAndGroupImages = new System.Windows.Forms.ImageList(this.components);
            this.WMSTab = new System.Windows.Forms.TabPage();
            this.WMSClearHeaderButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.WMSBounds = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.WMSOpaque = new System.Windows.Forms.CheckBox();
            this.WMSQueryable = new System.Windows.Forms.CheckBox();
            this.WMSAvalible = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.WMSMetadata = new System.Windows.Forms.TextBox();
            this.WMSAbstract = new System.Windows.Forms.TextBox();
            this.WMSKeyWords = new System.Windows.Forms.TextBox();
            this.WMSTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WFSTab = new System.Windows.Forms.TabPage();
            this.CustomTab = new System.Windows.Forms.TabPage();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.WFSMetadata = new System.Windows.Forms.TextBox();
            this.WFSAbstract = new System.Windows.Forms.TextBox();
            this.WFSKeywords = new System.Windows.Forms.TextBox();
            this.WFSTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.WFSBounds = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.WFSAvalible = new System.Windows.Forms.CheckBox();
            this.WFSPrimarySRS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.WFSOtherSRS = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.WFSClearHeaderButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ClearHeaderButton = new System.Windows.Forms.Button();
            this.ItemKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SecurityTab.SuspendLayout();
            this.WMSTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.WFSTab.SuspendLayout();
            this.CustomTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 389);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 40);
            this.panel1.TabIndex = 0;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.Location = new System.Drawing.Point(183, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKBtn.Location = new System.Drawing.Point(95, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SecurityTab);
            this.tabControl1.Controls.Add(this.WMSTab);
            this.tabControl1.Controls.Add(this.WFSTab);
            this.tabControl1.Controls.Add(this.CustomTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(358, 389);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // SecurityTab
            // 
            this.SecurityTab.Controls.Add(this.UseInherited);
            this.SecurityTab.Controls.Add(this.UsersAndGroups);
            this.SecurityTab.Location = new System.Drawing.Point(4, 22);
            this.SecurityTab.Name = "SecurityTab";
            this.SecurityTab.Padding = new System.Windows.Forms.Padding(3);
            this.SecurityTab.Size = new System.Drawing.Size(350, 363);
            this.SecurityTab.TabIndex = 0;
            this.SecurityTab.Text = "Security";
            this.SecurityTab.UseVisualStyleBackColor = true;
            // 
            // UseInherited
            // 
            this.UseInherited.AutoSize = true;
            this.UseInherited.Location = new System.Drawing.Point(16, 16);
            this.UseInherited.Name = "UseInherited";
            this.UseInherited.Size = new System.Drawing.Size(166, 17);
            this.UseInherited.TabIndex = 1;
            this.UseInherited.Text = "Use inherited security settings";
            this.UseInherited.UseVisualStyleBackColor = true;
            this.UseInherited.CheckedChanged += new System.EventHandler(this.UseInherited_CheckedChanged);
            // 
            // UsersAndGroups
            // 
            this.UsersAndGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.UsersAndGroups.Enabled = false;
            this.UsersAndGroups.Location = new System.Drawing.Point(16, 40);
            this.UsersAndGroups.Name = "UsersAndGroups";
            this.UsersAndGroups.Size = new System.Drawing.Size(320, 240);
            this.UsersAndGroups.SmallImageList = this.UserAndGroupImages;
            this.UsersAndGroups.TabIndex = 0;
            this.UsersAndGroups.UseCompatibleStateImageBehavior = false;
            this.UsersAndGroups.View = System.Windows.Forms.View.List;
            // 
            // UserAndGroupImages
            // 
            this.UserAndGroupImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("UserAndGroupImages.ImageStream")));
            this.UserAndGroupImages.TransparentColor = System.Drawing.Color.Transparent;
            this.UserAndGroupImages.Images.SetKeyName(0, "WriteUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(1, "ReadOnlyUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(2, "DenyUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(3, "WriteGroup.ico");
            this.UserAndGroupImages.Images.SetKeyName(4, "ReadOnlyGroup.ico");
            this.UserAndGroupImages.Images.SetKeyName(5, "DenyGroup.ico");
            // 
            // WMSTab
            // 
            this.WMSTab.Controls.Add(this.WMSClearHeaderButton);
            this.WMSTab.Controls.Add(this.groupBox2);
            this.WMSTab.Controls.Add(this.groupBox1);
            this.WMSTab.Location = new System.Drawing.Point(4, 22);
            this.WMSTab.Name = "WMSTab";
            this.WMSTab.Padding = new System.Windows.Forms.Padding(3);
            this.WMSTab.Size = new System.Drawing.Size(350, 363);
            this.WMSTab.TabIndex = 1;
            this.WMSTab.Text = "WMS";
            this.WMSTab.UseVisualStyleBackColor = true;
            // 
            // WMSClearHeaderButton
            // 
            this.WMSClearHeaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WMSClearHeaderButton.Location = new System.Drawing.Point(224, 329);
            this.WMSClearHeaderButton.Name = "WMSClearHeaderButton";
            this.WMSClearHeaderButton.Size = new System.Drawing.Size(119, 23);
            this.WMSClearHeaderButton.TabIndex = 10;
            this.WMSClearHeaderButton.Text = "Clear all WMS data";
            this.WMSClearHeaderButton.UseVisualStyleBackColor = true;
            this.WMSClearHeaderButton.Click += new System.EventHandler(this.ClearHeaderButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.WMSBounds);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.WMSOpaque);
            this.groupBox2.Controls.Add(this.WMSQueryable);
            this.groupBox2.Controls.Add(this.WMSAvalible);
            this.groupBox2.Location = new System.Drawing.Point(8, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 144);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Functionality";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(304, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 20);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // WMSBounds
            // 
            this.WMSBounds.Location = new System.Drawing.Point(88, 96);
            this.WMSBounds.Multiline = true;
            this.WMSBounds.Name = "WMSBounds";
            this.WMSBounds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WMSBounds.Size = new System.Drawing.Size(216, 40);
            this.WMSBounds.TabIndex = 7;
            this.toolTip.SetToolTip(this.WMSBounds, "This value is the Xml element bounds, which is inserted directly into the xml out" +
                    "put. Use the edit button if you are uncomfortable editing the value manually");
            this.WMSBounds.TextChanged += new System.EventHandler(this.WMSBounds_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Bounds";
            // 
            // WMSOpaque
            // 
            this.WMSOpaque.AutoSize = true;
            this.WMSOpaque.Location = new System.Drawing.Point(8, 72);
            this.WMSOpaque.Name = "WMSOpaque";
            this.WMSOpaque.Size = new System.Drawing.Size(64, 17);
            this.WMSOpaque.TabIndex = 6;
            this.WMSOpaque.Text = "Opaque";
            this.toolTip.SetToolTip(this.WMSOpaque, "This checkbox determines if the layer should be rendered opaque");
            this.WMSOpaque.UseVisualStyleBackColor = true;
            this.WMSOpaque.CheckedChanged += new System.EventHandler(this.WMSOpaque_CheckedChanged);
            // 
            // WMSQueryable
            // 
            this.WMSQueryable.AutoSize = true;
            this.WMSQueryable.Location = new System.Drawing.Point(8, 48);
            this.WMSQueryable.Name = "WMSQueryable";
            this.WMSQueryable.Size = new System.Drawing.Size(74, 17);
            this.WMSQueryable.TabIndex = 3;
            this.WMSQueryable.Text = "Queryable";
            this.toolTip.SetToolTip(this.WMSQueryable, "This checkbox determines if the layer can be queried for tooltips and links");
            this.WMSQueryable.UseVisualStyleBackColor = true;
            this.WMSQueryable.CheckedChanged += new System.EventHandler(this.WMSQueryable_CheckedChanged);
            // 
            // WMSAvalible
            // 
            this.WMSAvalible.AutoSize = true;
            this.WMSAvalible.Location = new System.Drawing.Point(8, 24);
            this.WMSAvalible.Name = "WMSAvalible";
            this.WMSAvalible.Size = new System.Drawing.Size(63, 17);
            this.WMSAvalible.TabIndex = 4;
            this.WMSAvalible.Text = "Avalible";
            this.toolTip.SetToolTip(this.WMSAvalible, "This checkbox controls the avalibility of the layer");
            this.WMSAvalible.UseVisualStyleBackColor = true;
            this.WMSAvalible.CheckedChanged += new System.EventHandler(this.WMSAvalible_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.WMSMetadata);
            this.groupBox1.Controls.Add(this.WMSAbstract);
            this.groupBox1.Controls.Add(this.WMSKeyWords);
            this.groupBox1.Controls.Add(this.WMSTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 160);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // WMSMetadata
            // 
            this.WMSMetadata.Location = new System.Drawing.Point(80, 112);
            this.WMSMetadata.Multiline = true;
            this.WMSMetadata.Name = "WMSMetadata";
            this.WMSMetadata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WMSMetadata.Size = new System.Drawing.Size(248, 40);
            this.WMSMetadata.TabIndex = 11;
            this.toolTip.SetToolTip(this.WMSMetadata, "This field contains human readable metadata");
            this.WMSMetadata.TextChanged += new System.EventHandler(this.WMSMetadata_TextChanged);
            // 
            // WMSAbstract
            // 
            this.WMSAbstract.Location = new System.Drawing.Point(80, 64);
            this.WMSAbstract.Multiline = true;
            this.WMSAbstract.Name = "WMSAbstract";
            this.WMSAbstract.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WMSAbstract.Size = new System.Drawing.Size(248, 40);
            this.WMSAbstract.TabIndex = 10;
            this.toolTip.SetToolTip(this.WMSAbstract, "This field contains an abstract that describes the layer");
            this.WMSAbstract.TextChanged += new System.EventHandler(this.WMSAbstract_TextChanged);
            // 
            // WMSKeyWords
            // 
            this.WMSKeyWords.Location = new System.Drawing.Point(80, 40);
            this.WMSKeyWords.Name = "WMSKeyWords";
            this.WMSKeyWords.Size = new System.Drawing.Size(248, 20);
            this.WMSKeyWords.TabIndex = 9;
            this.toolTip.SetToolTip(this.WMSKeyWords, "This field contains searchable keywords, seperated with spaces");
            this.WMSKeyWords.TextChanged += new System.EventHandler(this.WMSKeyWords_TextChanged);
            // 
            // WMSTitle
            // 
            this.WMSTitle.Location = new System.Drawing.Point(80, 16);
            this.WMSTitle.Name = "WMSTitle";
            this.WMSTitle.Size = new System.Drawing.Size(248, 20);
            this.WMSTitle.TabIndex = 8;
            this.toolTip.SetToolTip(this.WMSTitle, "This field contains the layer title");
            this.WMSTitle.TextChanged += new System.EventHandler(this.WMSTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Abstract";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Metadata";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Keywords";
            // 
            // WFSTab
            // 
            this.WFSTab.Controls.Add(this.WFSClearHeaderButton);
            this.WFSTab.Controls.Add(this.groupBox4);
            this.WFSTab.Controls.Add(this.groupBox3);
            this.WFSTab.Location = new System.Drawing.Point(4, 22);
            this.WFSTab.Name = "WFSTab";
            this.WFSTab.Padding = new System.Windows.Forms.Padding(3);
            this.WFSTab.Size = new System.Drawing.Size(350, 363);
            this.WFSTab.TabIndex = 2;
            this.WFSTab.Text = "WFS";
            this.WFSTab.UseVisualStyleBackColor = true;
            // 
            // CustomTab
            // 
            this.CustomTab.Controls.Add(this.ClearHeaderButton);
            this.CustomTab.Controls.Add(this.dataGridView1);
            this.CustomTab.Location = new System.Drawing.Point(4, 22);
            this.CustomTab.Name = "CustomTab";
            this.CustomTab.Size = new System.Drawing.Size(350, 363);
            this.CustomTab.TabIndex = 3;
            this.CustomTab.Text = "Custom Metadata";
            this.CustomTab.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.WFSMetadata);
            this.groupBox3.Controls.Add(this.WFSAbstract);
            this.groupBox3.Controls.Add(this.WFSKeywords);
            this.groupBox3.Controls.Add(this.WFSTitle);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(7, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(336, 160);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Description";
            // 
            // WFSMetadata
            // 
            this.WFSMetadata.Location = new System.Drawing.Point(80, 112);
            this.WFSMetadata.Multiline = true;
            this.WFSMetadata.Name = "WFSMetadata";
            this.WFSMetadata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WFSMetadata.Size = new System.Drawing.Size(248, 40);
            this.WFSMetadata.TabIndex = 11;
            this.toolTip.SetToolTip(this.WFSMetadata, "This field contains human readable metadata");
            this.WFSMetadata.TextChanged += new System.EventHandler(this.WFSMetadata_TextChanged);
            // 
            // WFSAbstract
            // 
            this.WFSAbstract.Location = new System.Drawing.Point(80, 64);
            this.WFSAbstract.Multiline = true;
            this.WFSAbstract.Name = "WFSAbstract";
            this.WFSAbstract.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WFSAbstract.Size = new System.Drawing.Size(248, 40);
            this.WFSAbstract.TabIndex = 10;
            this.toolTip.SetToolTip(this.WFSAbstract, "This field contains an abstract that describes the layer");
            this.WFSAbstract.TextChanged += new System.EventHandler(this.WFSAbstract_TextChanged);
            // 
            // WFSKeywords
            // 
            this.WFSKeywords.Location = new System.Drawing.Point(80, 40);
            this.WFSKeywords.Name = "WFSKeywords";
            this.WFSKeywords.Size = new System.Drawing.Size(248, 20);
            this.WFSKeywords.TabIndex = 9;
            this.toolTip.SetToolTip(this.WFSKeywords, "This field contains searchable keywords, seperated with spaces");
            this.WFSKeywords.TextChanged += new System.EventHandler(this.WFSKeywords_TextChanged);
            // 
            // WFSTitle
            // 
            this.WFSTitle.Location = new System.Drawing.Point(80, 16);
            this.WFSTitle.Name = "WFSTitle";
            this.WFSTitle.Size = new System.Drawing.Size(248, 20);
            this.WFSTitle.TabIndex = 8;
            this.toolTip.SetToolTip(this.WFSTitle, "This field contains the layer title");
            this.WFSTitle.TextChanged += new System.EventHandler(this.WFSTitle_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Title";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Abstract";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Metadata";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Keywords";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.WFSOtherSRS);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.WFSPrimarySRS);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.WFSBounds);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.WFSAvalible);
            this.groupBox4.Location = new System.Drawing.Point(7, 176);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(336, 144);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Functionality";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(304, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 20);
            this.button2.TabIndex = 8;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // WFSBounds
            // 
            this.WFSBounds.Location = new System.Drawing.Point(88, 96);
            this.WFSBounds.Multiline = true;
            this.WFSBounds.Name = "WFSBounds";
            this.WFSBounds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WFSBounds.Size = new System.Drawing.Size(216, 40);
            this.WFSBounds.TabIndex = 7;
            this.toolTip.SetToolTip(this.WFSBounds, "This value is the Xml element bounds, which is inserted directly into the xml out" +
                    "put. Use the edit button if you are uncomfortable editing the value manually");
            this.WFSBounds.TextChanged += new System.EventHandler(this.WFSBounds_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Bounds";
            // 
            // WFSAvalible
            // 
            this.WFSAvalible.AutoSize = true;
            this.WFSAvalible.Location = new System.Drawing.Point(8, 24);
            this.WFSAvalible.Name = "WFSAvalible";
            this.WFSAvalible.Size = new System.Drawing.Size(63, 17);
            this.WFSAvalible.TabIndex = 4;
            this.WFSAvalible.Text = "Avalible";
            this.toolTip.SetToolTip(this.WFSAvalible, "This checkbox controls the avalibility of the layer");
            this.WFSAvalible.UseVisualStyleBackColor = true;
            this.WFSAvalible.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // WFSPrimarySRS
            // 
            this.WFSPrimarySRS.Location = new System.Drawing.Point(88, 48);
            this.WFSPrimarySRS.Name = "WFSPrimarySRS";
            this.WFSPrimarySRS.Size = new System.Drawing.Size(240, 20);
            this.WFSPrimarySRS.TabIndex = 10;
            this.toolTip.SetToolTip(this.WFSPrimarySRS, "This field contains the layer title");
            this.WFSPrimarySRS.TextChanged += new System.EventHandler(this.WFSPrimarySRS_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Primary SRS";
            // 
            // WFSOtherSRS
            // 
            this.WFSOtherSRS.Location = new System.Drawing.Point(88, 72);
            this.WFSOtherSRS.Name = "WFSOtherSRS";
            this.WFSOtherSRS.Size = new System.Drawing.Size(240, 20);
            this.WFSOtherSRS.TabIndex = 12;
            this.toolTip.SetToolTip(this.WFSOtherSRS, "This field contains the layer title");
            this.WFSOtherSRS.TextChanged += new System.EventHandler(this.WFSOtherSRS_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Other SRS";
            // 
            // WFSClearHeaderButton
            // 
            this.WFSClearHeaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WFSClearHeaderButton.Location = new System.Drawing.Point(224, 328);
            this.WFSClearHeaderButton.Name = "WFSClearHeaderButton";
            this.WFSClearHeaderButton.Size = new System.Drawing.Size(119, 23);
            this.WFSClearHeaderButton.TabIndex = 11;
            this.WFSClearHeaderButton.Text = "Clear all WFS data";
            this.WFSClearHeaderButton.UseVisualStyleBackColor = true;
            this.WFSClearHeaderButton.Click += new System.EventHandler(this.WFSClearHeaderButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemKey,
            this.ItemValue});
            this.dataGridView1.Location = new System.Drawing.Point(8, 8);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(336, 312);
            this.dataGridView1.TabIndex = 0;
            // 
            // ClearHeaderButton
            // 
            this.ClearHeaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearHeaderButton.Location = new System.Drawing.Point(224, 328);
            this.ClearHeaderButton.Name = "ClearHeaderButton";
            this.ClearHeaderButton.Size = new System.Drawing.Size(119, 23);
            this.ClearHeaderButton.TabIndex = 11;
            this.ClearHeaderButton.Text = "Clear all metadata";
            this.ClearHeaderButton.UseVisualStyleBackColor = true;
            this.ClearHeaderButton.Click += new System.EventHandler(this.ClearHeaderButton_Click_1);
            // 
            // ItemKey
            // 
            this.ItemKey.HeaderText = "Key";
            this.ItemKey.Name = "ItemKey";
            // 
            // ItemValue
            // 
            this.ItemValue.HeaderText = "Value";
            this.ItemValue.Name = "ItemValue";
            // 
            // ResourceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 429);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "ResourceProperties";
            this.Text = "ResourceProperties";
            this.Load += new System.EventHandler(this.ResourceProperties_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.SecurityTab.ResumeLayout(false);
            this.SecurityTab.PerformLayout();
            this.WMSTab.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.WFSTab.ResumeLayout(false);
            this.CustomTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SecurityTab;
        private System.Windows.Forms.TabPage WMSTab;
        private System.Windows.Forms.TabPage WFSTab;
        private System.Windows.Forms.TabPage CustomTab;
        private System.Windows.Forms.CheckBox UseInherited;
        private System.Windows.Forms.ListView UsersAndGroups;
        private System.Windows.Forms.ImageList UserAndGroupImages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox WMSOpaque;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox WMSAvalible;
        private System.Windows.Forms.CheckBox WMSQueryable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox WMSBounds;
        private System.Windows.Forms.TextBox WMSTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox WMSMetadata;
        private System.Windows.Forms.TextBox WMSAbstract;
        private System.Windows.Forms.TextBox WMSKeyWords;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button WMSClearHeaderButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox WFSBounds;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox WFSAvalible;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox WFSMetadata;
        private System.Windows.Forms.TextBox WFSAbstract;
        private System.Windows.Forms.TextBox WFSKeywords;
        private System.Windows.Forms.TextBox WFSTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox WFSOtherSRS;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox WFSPrimarySRS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button WFSClearHeaderButton;
        private System.Windows.Forms.Button ClearHeaderButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemValue;
    }
}