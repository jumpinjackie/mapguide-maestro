namespace Maestro.Base.UI
{
    partial class ResourcePropertiesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourcePropertiesDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SecurityTab = new System.Windows.Forms.TabPage();
            this.UseInherited = new System.Windows.Forms.CheckBox();
            this.UsersAndGroups = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.securityContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.readWriteAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readOnlyAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.denyAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inheritedAccessRightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WMSTab = new System.Windows.Forms.TabPage();
            this.WMSClearHeaderButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AutoGenerateWMSBounds = new System.Windows.Forms.Button();
            this.EditWMSBounds = new System.Windows.Forms.Button();
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
            this.WFSClearHeaderButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnOtherSRS = new System.Windows.Forms.Button();
            this.btnPrimarySRS = new System.Windows.Forms.Button();
            this.txtOtherSRS = new System.Windows.Forms.TextBox();
            this.txtPrimarySRS = new System.Windows.Forms.TextBox();
            this.AutoGenerateWFSBounds = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.EditWFSBounds = new System.Windows.Forms.Button();
            this.WFSBounds = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.WFSAvailable = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.WFSMetadata = new System.Windows.Forms.TextBox();
            this.WFSAbstract = new System.Windows.Forms.TextBox();
            this.WFSKeywords = new System.Windows.Forms.TextBox();
            this.WFSTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.CustomTab = new System.Windows.Forms.TabPage();
            this.ClearHeaderButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ItemKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferenceTab = new System.Windows.Forms.TabPage();
            this.LoadingReferences = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OutReferences = new System.Windows.Forms.GroupBox();
            this.OutReferenceList = new System.Windows.Forms.ListView();
            this.ctxReferences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyResourceIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InReferences = new System.Windows.Forms.GroupBox();
            this.InReferenceList = new System.Windows.Forms.ListView();
            this.ctxReferenced = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyResourceIDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ResourceID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.ReferenceWorker = new System.ComponentModel.BackgroundWorker();
            this.UserAndGroupImages = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnReferencesOpenSelected = new System.Windows.Forms.ToolStripButton();
            this.btnReferencedByOpenSelected = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.SecurityTab.SuspendLayout();
            this.securityContextMenu.SuspendLayout();
            this.WMSTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.WFSTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CustomTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.ReferenceTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.OutReferences.SuspendLayout();
            this.ctxReferences.SuspendLayout();
            this.InReferences.SuspendLayout();
            this.ctxReferenced.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SecurityTab);
            this.tabControl1.Controls.Add(this.WMSTab);
            this.tabControl1.Controls.Add(this.WFSTab);
            this.tabControl1.Controls.Add(this.CustomTab);
            this.tabControl1.Controls.Add(this.ReferenceTab);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // SecurityTab
            // 
            this.SecurityTab.Controls.Add(this.UseInherited);
            this.SecurityTab.Controls.Add(this.UsersAndGroups);
            resources.ApplyResources(this.SecurityTab, "SecurityTab");
            this.SecurityTab.Name = "SecurityTab";
            this.SecurityTab.UseVisualStyleBackColor = true;
            // 
            // UseInherited
            // 
            resources.ApplyResources(this.UseInherited, "UseInherited");
            this.UseInherited.Name = "UseInherited";
            this.UseInherited.UseVisualStyleBackColor = true;
            this.UseInherited.CheckedChanged += new System.EventHandler(this.UseInherited_CheckedChanged);
            // 
            // UsersAndGroups
            // 
            resources.ApplyResources(this.UsersAndGroups, "UsersAndGroups");
            this.UsersAndGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.UsersAndGroups.ContextMenuStrip = this.securityContextMenu;
            this.UsersAndGroups.FullRowSelect = true;
            this.UsersAndGroups.Name = "UsersAndGroups";
            this.UsersAndGroups.UseCompatibleStateImageBehavior = false;
            this.UsersAndGroups.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // securityContextMenu
            // 
            this.securityContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readWriteAccessToolStripMenuItem,
            this.readOnlyAccessToolStripMenuItem,
            this.denyAccessToolStripMenuItem,
            this.inheritedAccessRightsToolStripMenuItem});
            this.securityContextMenu.Name = "securityContextMenu";
            resources.ApplyResources(this.securityContextMenu, "securityContextMenu");
            this.securityContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.securityContextMenu_Opening);
            // 
            // readWriteAccessToolStripMenuItem
            // 
            this.readWriteAccessToolStripMenuItem.Name = "readWriteAccessToolStripMenuItem";
            resources.ApplyResources(this.readWriteAccessToolStripMenuItem, "readWriteAccessToolStripMenuItem");
            this.readWriteAccessToolStripMenuItem.Click += new System.EventHandler(this.readWriteAccessToolStripMenuItem_Click);
            // 
            // readOnlyAccessToolStripMenuItem
            // 
            this.readOnlyAccessToolStripMenuItem.Name = "readOnlyAccessToolStripMenuItem";
            resources.ApplyResources(this.readOnlyAccessToolStripMenuItem, "readOnlyAccessToolStripMenuItem");
            this.readOnlyAccessToolStripMenuItem.Click += new System.EventHandler(this.readOnlyAccessToolStripMenuItem_Click);
            // 
            // denyAccessToolStripMenuItem
            // 
            this.denyAccessToolStripMenuItem.Name = "denyAccessToolStripMenuItem";
            resources.ApplyResources(this.denyAccessToolStripMenuItem, "denyAccessToolStripMenuItem");
            this.denyAccessToolStripMenuItem.Click += new System.EventHandler(this.denyAccessToolStripMenuItem_Click);
            // 
            // inheritedAccessRightsToolStripMenuItem
            // 
            this.inheritedAccessRightsToolStripMenuItem.Name = "inheritedAccessRightsToolStripMenuItem";
            resources.ApplyResources(this.inheritedAccessRightsToolStripMenuItem, "inheritedAccessRightsToolStripMenuItem");
            this.inheritedAccessRightsToolStripMenuItem.Click += new System.EventHandler(this.inheritedAccessRightsToolStripMenuItem_Click);
            // 
            // WMSTab
            // 
            this.WMSTab.Controls.Add(this.WMSClearHeaderButton);
            this.WMSTab.Controls.Add(this.groupBox2);
            this.WMSTab.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.WMSTab, "WMSTab");
            this.WMSTab.Name = "WMSTab";
            this.WMSTab.UseVisualStyleBackColor = true;
            // 
            // WMSClearHeaderButton
            // 
            resources.ApplyResources(this.WMSClearHeaderButton, "WMSClearHeaderButton");
            this.WMSClearHeaderButton.Name = "WMSClearHeaderButton";
            this.WMSClearHeaderButton.UseVisualStyleBackColor = true;
            this.WMSClearHeaderButton.Click += new System.EventHandler(this.ClearHeaderButton_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.AutoGenerateWMSBounds);
            this.groupBox2.Controls.Add(this.EditWMSBounds);
            this.groupBox2.Controls.Add(this.WMSBounds);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.WMSOpaque);
            this.groupBox2.Controls.Add(this.WMSQueryable);
            this.groupBox2.Controls.Add(this.WMSAvalible);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // AutoGenerateWMSBounds
            // 
            resources.ApplyResources(this.AutoGenerateWMSBounds, "AutoGenerateWMSBounds");
            this.AutoGenerateWMSBounds.Name = "AutoGenerateWMSBounds";
            this.AutoGenerateWMSBounds.UseVisualStyleBackColor = true;
            this.AutoGenerateWMSBounds.Click += new System.EventHandler(this.AutoGenerateWMSBounds_Click);
            // 
            // EditWMSBounds
            // 
            resources.ApplyResources(this.EditWMSBounds, "EditWMSBounds");
            this.EditWMSBounds.Name = "EditWMSBounds";
            this.EditWMSBounds.UseVisualStyleBackColor = true;
            this.EditWMSBounds.Click += new System.EventHandler(this.EditWMSBounds_Click);
            // 
            // WMSBounds
            // 
            resources.ApplyResources(this.WMSBounds, "WMSBounds");
            this.WMSBounds.Name = "WMSBounds";
            this.WMSBounds.TextChanged += new System.EventHandler(this.WMSBounds_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // WMSOpaque
            // 
            resources.ApplyResources(this.WMSOpaque, "WMSOpaque");
            this.WMSOpaque.Name = "WMSOpaque";
            this.WMSOpaque.UseVisualStyleBackColor = true;
            this.WMSOpaque.CheckedChanged += new System.EventHandler(this.WMSOpaque_CheckedChanged);
            // 
            // WMSQueryable
            // 
            resources.ApplyResources(this.WMSQueryable, "WMSQueryable");
            this.WMSQueryable.Name = "WMSQueryable";
            this.WMSQueryable.UseVisualStyleBackColor = true;
            this.WMSQueryable.CheckedChanged += new System.EventHandler(this.WMSQueryable_CheckedChanged);
            // 
            // WMSAvalible
            // 
            resources.ApplyResources(this.WMSAvalible, "WMSAvalible");
            this.WMSAvalible.Name = "WMSAvalible";
            this.WMSAvalible.UseVisualStyleBackColor = true;
            this.WMSAvalible.CheckedChanged += new System.EventHandler(this.WMSAvalible_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.WMSMetadata);
            this.groupBox1.Controls.Add(this.WMSAbstract);
            this.groupBox1.Controls.Add(this.WMSKeyWords);
            this.groupBox1.Controls.Add(this.WMSTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // WMSMetadata
            // 
            resources.ApplyResources(this.WMSMetadata, "WMSMetadata");
            this.WMSMetadata.Name = "WMSMetadata";
            this.WMSMetadata.TextChanged += new System.EventHandler(this.WMSMetadata_TextChanged);
            // 
            // WMSAbstract
            // 
            resources.ApplyResources(this.WMSAbstract, "WMSAbstract");
            this.WMSAbstract.Name = "WMSAbstract";
            this.WMSAbstract.TextChanged += new System.EventHandler(this.WMSAbstract_TextChanged);
            // 
            // WMSKeyWords
            // 
            resources.ApplyResources(this.WMSKeyWords, "WMSKeyWords");
            this.WMSKeyWords.Name = "WMSKeyWords";
            this.WMSKeyWords.TextChanged += new System.EventHandler(this.WMSKeyWords_TextChanged);
            // 
            // WMSTitle
            // 
            resources.ApplyResources(this.WMSTitle, "WMSTitle");
            this.WMSTitle.Name = "WMSTitle";
            this.WMSTitle.TextChanged += new System.EventHandler(this.WMSTitle_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // WFSTab
            // 
            this.WFSTab.Controls.Add(this.WFSClearHeaderButton);
            this.WFSTab.Controls.Add(this.groupBox4);
            this.WFSTab.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.WFSTab, "WFSTab");
            this.WFSTab.Name = "WFSTab";
            this.WFSTab.UseVisualStyleBackColor = true;
            // 
            // WFSClearHeaderButton
            // 
            resources.ApplyResources(this.WFSClearHeaderButton, "WFSClearHeaderButton");
            this.WFSClearHeaderButton.Name = "WFSClearHeaderButton";
            this.WFSClearHeaderButton.UseVisualStyleBackColor = true;
            this.WFSClearHeaderButton.Click += new System.EventHandler(this.WFSClearHeaderButton_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.btnOtherSRS);
            this.groupBox4.Controls.Add(this.btnPrimarySRS);
            this.groupBox4.Controls.Add(this.txtOtherSRS);
            this.groupBox4.Controls.Add(this.txtPrimarySRS);
            this.groupBox4.Controls.Add(this.AutoGenerateWFSBounds);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.EditWFSBounds);
            this.groupBox4.Controls.Add(this.WFSBounds);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.WFSAvailable);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // btnOtherSRS
            // 
            resources.ApplyResources(this.btnOtherSRS, "btnOtherSRS");
            this.btnOtherSRS.Name = "btnOtherSRS";
            this.btnOtherSRS.UseVisualStyleBackColor = true;
            this.btnOtherSRS.Click += new System.EventHandler(this.btnOtherSRS_Click);
            // 
            // btnPrimarySRS
            // 
            resources.ApplyResources(this.btnPrimarySRS, "btnPrimarySRS");
            this.btnPrimarySRS.Name = "btnPrimarySRS";
            this.btnPrimarySRS.UseVisualStyleBackColor = true;
            this.btnPrimarySRS.Click += new System.EventHandler(this.btnPrimarySRS_Click);
            // 
            // txtOtherSRS
            // 
            resources.ApplyResources(this.txtOtherSRS, "txtOtherSRS");
            this.txtOtherSRS.Name = "txtOtherSRS";
            this.txtOtherSRS.ReadOnly = true;
            this.txtOtherSRS.TextChanged += new System.EventHandler(this.WFSOtherSRS_TextChanged);
            // 
            // txtPrimarySRS
            // 
            resources.ApplyResources(this.txtPrimarySRS, "txtPrimarySRS");
            this.txtPrimarySRS.Name = "txtPrimarySRS";
            this.txtPrimarySRS.ReadOnly = true;
            this.txtPrimarySRS.TextChanged += new System.EventHandler(this.WFSPrimarySRS_TextChanged);
            // 
            // AutoGenerateWFSBounds
            // 
            resources.ApplyResources(this.AutoGenerateWFSBounds, "AutoGenerateWFSBounds");
            this.AutoGenerateWFSBounds.Name = "AutoGenerateWFSBounds";
            this.AutoGenerateWFSBounds.UseVisualStyleBackColor = true;
            this.AutoGenerateWFSBounds.Click += new System.EventHandler(this.AutoGenerateWFSBounds_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // EditWFSBounds
            // 
            resources.ApplyResources(this.EditWFSBounds, "EditWFSBounds");
            this.EditWFSBounds.Name = "EditWFSBounds";
            this.EditWFSBounds.UseVisualStyleBackColor = true;
            this.EditWFSBounds.Click += new System.EventHandler(this.EditWFSBounds_Click);
            // 
            // WFSBounds
            // 
            resources.ApplyResources(this.WFSBounds, "WFSBounds");
            this.WFSBounds.Name = "WFSBounds";
            this.WFSBounds.TextChanged += new System.EventHandler(this.WFSBounds_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // WFSAvailable
            // 
            resources.ApplyResources(this.WFSAvailable, "WFSAvailable");
            this.WFSAvailable.Name = "WFSAvailable";
            this.WFSAvailable.UseVisualStyleBackColor = true;
            this.WFSAvailable.CheckedChanged += new System.EventHandler(this.WFSAvailable_CheckedChanged);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.WFSMetadata);
            this.groupBox3.Controls.Add(this.WFSAbstract);
            this.groupBox3.Controls.Add(this.WFSKeywords);
            this.groupBox3.Controls.Add(this.WFSTitle);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // WFSMetadata
            // 
            resources.ApplyResources(this.WFSMetadata, "WFSMetadata");
            this.WFSMetadata.Name = "WFSMetadata";
            this.WFSMetadata.TextChanged += new System.EventHandler(this.WFSMetadata_TextChanged);
            // 
            // WFSAbstract
            // 
            resources.ApplyResources(this.WFSAbstract, "WFSAbstract");
            this.WFSAbstract.Name = "WFSAbstract";
            this.WFSAbstract.TextChanged += new System.EventHandler(this.WFSAbstract_TextChanged);
            // 
            // WFSKeywords
            // 
            resources.ApplyResources(this.WFSKeywords, "WFSKeywords");
            this.WFSKeywords.Name = "WFSKeywords";
            this.WFSKeywords.TextChanged += new System.EventHandler(this.WFSKeywords_TextChanged);
            // 
            // WFSTitle
            // 
            resources.ApplyResources(this.WFSTitle, "WFSTitle");
            this.WFSTitle.Name = "WFSTitle";
            this.WFSTitle.TextChanged += new System.EventHandler(this.WFSTitle_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // CustomTab
            // 
            this.CustomTab.Controls.Add(this.ClearHeaderButton);
            this.CustomTab.Controls.Add(this.dataGridView1);
            resources.ApplyResources(this.CustomTab, "CustomTab");
            this.CustomTab.Name = "CustomTab";
            this.CustomTab.UseVisualStyleBackColor = true;
            // 
            // ClearHeaderButton
            // 
            resources.ApplyResources(this.ClearHeaderButton, "ClearHeaderButton");
            this.ClearHeaderButton.Name = "ClearHeaderButton";
            this.ClearHeaderButton.UseVisualStyleBackColor = true;
            this.ClearHeaderButton.Click += new System.EventHandler(this.ClearHeaderButton_Click);
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemKey,
            this.ItemValue});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Leave += new System.EventHandler(this.dataGridView1_Leave);
            // 
            // ItemKey
            // 
            resources.ApplyResources(this.ItemKey, "ItemKey");
            this.ItemKey.Name = "ItemKey";
            // 
            // ItemValue
            // 
            resources.ApplyResources(this.ItemValue, "ItemValue");
            this.ItemValue.Name = "ItemValue";
            // 
            // ReferenceTab
            // 
            this.ReferenceTab.Controls.Add(this.LoadingReferences);
            this.ReferenceTab.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.ReferenceTab, "ReferenceTab");
            this.ReferenceTab.Name = "ReferenceTab";
            this.ReferenceTab.UseVisualStyleBackColor = true;
            // 
            // LoadingReferences
            // 
            resources.ApplyResources(this.LoadingReferences, "LoadingReferences");
            this.LoadingReferences.Name = "LoadingReferences";
            this.LoadingReferences.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.OutReferences);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.InReferences);
            // 
            // OutReferences
            // 
            this.OutReferences.Controls.Add(this.OutReferenceList);
            this.OutReferences.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.OutReferences, "OutReferences");
            this.OutReferences.Name = "OutReferences";
            this.OutReferences.TabStop = false;
            // 
            // OutReferenceList
            // 
            this.OutReferenceList.ContextMenuStrip = this.ctxReferences;
            resources.ApplyResources(this.OutReferenceList, "OutReferenceList");
            this.OutReferenceList.FullRowSelect = true;
            this.OutReferenceList.GridLines = true;
            this.OutReferenceList.Name = "OutReferenceList";
            this.OutReferenceList.UseCompatibleStateImageBehavior = false;
            this.OutReferenceList.View = System.Windows.Forms.View.List;
            this.OutReferenceList.SelectedIndexChanged += new System.EventHandler(this.OutReferenceList_SelectedIndexChanged);
            this.OutReferenceList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OutReferenceList_KeyUp);
            // 
            // ctxReferences
            // 
            this.ctxReferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyResourceIDToolStripMenuItem});
            this.ctxReferences.Name = "ctxReferences";
            resources.ApplyResources(this.ctxReferences, "ctxReferences");
            this.ctxReferences.Opening += new System.ComponentModel.CancelEventHandler(this.ctxReferences_Opening);
            // 
            // copyResourceIDToolStripMenuItem
            // 
            this.copyResourceIDToolStripMenuItem.Name = "copyResourceIDToolStripMenuItem";
            resources.ApplyResources(this.copyResourceIDToolStripMenuItem, "copyResourceIDToolStripMenuItem");
            this.copyResourceIDToolStripMenuItem.Click += new System.EventHandler(this.referencesCopyResourceIDToolStripMenuItem_Click);
            // 
            // InReferences
            // 
            this.InReferences.Controls.Add(this.InReferenceList);
            this.InReferences.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.InReferences, "InReferences");
            this.InReferences.Name = "InReferences";
            this.InReferences.TabStop = false;
            // 
            // InReferenceList
            // 
            this.InReferenceList.ContextMenuStrip = this.ctxReferenced;
            resources.ApplyResources(this.InReferenceList, "InReferenceList");
            this.InReferenceList.FullRowSelect = true;
            this.InReferenceList.GridLines = true;
            this.InReferenceList.Name = "InReferenceList";
            this.InReferenceList.UseCompatibleStateImageBehavior = false;
            this.InReferenceList.View = System.Windows.Forms.View.List;
            this.InReferenceList.SelectedIndexChanged += new System.EventHandler(this.InReferenceList_SelectedIndexChanged);
            this.InReferenceList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InReferenceList_KeyUp);
            // 
            // ctxReferenced
            // 
            this.ctxReferenced.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyResourceIDToolStripMenuItem1});
            this.ctxReferenced.Name = "ctxReferenced";
            resources.ApplyResources(this.ctxReferenced, "ctxReferenced");
            this.ctxReferenced.Opening += new System.ComponentModel.CancelEventHandler(this.ctxReferenced_Opening);
            // 
            // copyResourceIDToolStripMenuItem1
            // 
            this.copyResourceIDToolStripMenuItem1.Name = "copyResourceIDToolStripMenuItem1";
            resources.ApplyResources(this.copyResourceIDToolStripMenuItem1, "copyResourceIDToolStripMenuItem1");
            this.copyResourceIDToolStripMenuItem1.Click += new System.EventHandler(this.referencedCopyResourceIDToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ResourceID);
            this.panel2.Controls.Add(this.label13);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // ResourceID
            // 
            resources.ApplyResources(this.ResourceID, "ResourceID");
            this.ResourceID.Name = "ResourceID";
            this.ResourceID.ReadOnly = true;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // ReferenceWorker
            // 
            this.ReferenceWorker.WorkerReportsProgress = true;
            this.ReferenceWorker.WorkerSupportsCancellation = true;
            this.ReferenceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ReferenceWorker_DoWork);
            this.ReferenceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ReferenceWorker_RunWorkerCompleted);
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
            this.UserAndGroupImages.Images.SetKeyName(6, "InheritedUser.ico");
            this.UserAndGroupImages.Images.SetKeyName(7, "InheritedGroup.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReferencesOpenSelected});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReferencedByOpenSelected});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnReferencesOpenSelected
            // 
            resources.ApplyResources(this.btnReferencesOpenSelected, "btnReferencesOpenSelected");
            this.btnReferencesOpenSelected.Image = global::Maestro.Base.Properties.Resources.folder_open_document;
            this.btnReferencesOpenSelected.Name = "btnReferencesOpenSelected";
            this.btnReferencesOpenSelected.Click += new System.EventHandler(this.btnReferencesOpenSelected_Click);
            // 
            // btnReferencedByOpenSelected
            // 
            resources.ApplyResources(this.btnReferencedByOpenSelected, "btnReferencedByOpenSelected");
            this.btnReferencedByOpenSelected.Image = global::Maestro.Base.Properties.Resources.folder_open_document;
            this.btnReferencedByOpenSelected.Name = "btnReferencedByOpenSelected";
            this.btnReferencedByOpenSelected.Click += new System.EventHandler(this.btnReferencedByOpenSelected_Click);
            // 
            // ResourcePropertiesDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ResourcePropertiesDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.tabControl1.ResumeLayout(false);
            this.SecurityTab.ResumeLayout(false);
            this.SecurityTab.PerformLayout();
            this.securityContextMenu.ResumeLayout(false);
            this.WMSTab.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.WFSTab.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.CustomTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ReferenceTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.OutReferences.ResumeLayout(false);
            this.OutReferences.PerformLayout();
            this.ctxReferences.ResumeLayout(false);
            this.InReferences.ResumeLayout(false);
            this.InReferences.PerformLayout();
            this.ctxReferenced.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SecurityTab;
        private System.Windows.Forms.CheckBox UseInherited;
        private System.Windows.Forms.ListView UsersAndGroups;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabPage WMSTab;
        private System.Windows.Forms.Button WMSClearHeaderButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AutoGenerateWMSBounds;
        private System.Windows.Forms.Button EditWMSBounds;
        private System.Windows.Forms.TextBox WMSBounds;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox WMSOpaque;
        private System.Windows.Forms.CheckBox WMSQueryable;
        private System.Windows.Forms.CheckBox WMSAvalible;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox WMSMetadata;
        private System.Windows.Forms.TextBox WMSAbstract;
        private System.Windows.Forms.TextBox WMSKeyWords;
        private System.Windows.Forms.TextBox WMSTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage WFSTab;
        private System.Windows.Forms.Button WFSClearHeaderButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button AutoGenerateWFSBounds;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button EditWFSBounds;
        private System.Windows.Forms.TextBox WFSBounds;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox WFSAvailable;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox WFSMetadata;
        private System.Windows.Forms.TextBox WFSAbstract;
        private System.Windows.Forms.TextBox WFSKeywords;
        private System.Windows.Forms.TextBox WFSTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage CustomTab;
        private System.Windows.Forms.Button ClearHeaderButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemValue;
        private System.Windows.Forms.TabPage ReferenceTab;
        private System.Windows.Forms.ProgressBar LoadingReferences;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox OutReferences;
        private System.Windows.Forms.ListView OutReferenceList;
        private System.Windows.Forms.GroupBox InReferences;
        private System.Windows.Forms.ListView InReferenceList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox ResourceID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.ComponentModel.BackgroundWorker ReferenceWorker;
        private System.Windows.Forms.ImageList UserAndGroupImages;
        private System.Windows.Forms.ContextMenuStrip securityContextMenu;
        private System.Windows.Forms.ToolStripMenuItem readWriteAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readOnlyAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem denyAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inheritedAccessRightsToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnOtherSRS;
        private System.Windows.Forms.Button btnPrimarySRS;
        private System.Windows.Forms.TextBox txtOtherSRS;
        private System.Windows.Forms.TextBox txtPrimarySRS;
        private System.Windows.Forms.ContextMenuStrip ctxReferences;
        private System.Windows.Forms.ToolStripMenuItem copyResourceIDToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxReferenced;
        private System.Windows.Forms.ToolStripMenuItem copyResourceIDToolStripMenuItem1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnReferencesOpenSelected;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnReferencedByOpenSelected;
    }
}