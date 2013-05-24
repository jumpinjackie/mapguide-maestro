namespace RtMapInspector
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.txtSessionId = new System.Windows.Forms.TextBox();
            this.txtResourceId = new System.Windows.Forms.TextBox();
            this.rdResourceId = new System.Windows.Forms.RadioButton();
            this.rdMapName = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TAB_LAYERS = new System.Windows.Forms.TabPage();
            this.trvLayersAndGroups = new System.Windows.Forms.TreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.TAB_SELECTION = new System.Windows.Forms.TabPage();
            this.trvSelection = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.PropertyGrid();
            this.TAB_DRAW_ORDER = new System.Windows.Forms.TabPage();
            this.lstDrawOrder = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TAB_LAYERS.SuspendLayout();
            this.TAB_SELECTION.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.TAB_DRAW_ORDER.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMapName);
            this.groupBox1.Controls.Add(this.txtSessionId);
            this.groupBox1.Controls.Add(this.txtResourceId);
            this.groupBox1.Controls.Add(this.rdResourceId);
            this.groupBox1.Controls.Add(this.rdMapName);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMapName
            // 
            resources.ApplyResources(this.txtMapName, "txtMapName");
            this.txtMapName.Name = "txtMapName";
            // 
            // txtSessionId
            // 
            resources.ApplyResources(this.txtSessionId, "txtSessionId");
            this.txtSessionId.Name = "txtSessionId";
            // 
            // txtResourceId
            // 
            resources.ApplyResources(this.txtResourceId, "txtResourceId");
            this.txtResourceId.Name = "txtResourceId";
            // 
            // rdResourceId
            // 
            resources.ApplyResources(this.rdResourceId, "rdResourceId");
            this.rdResourceId.Name = "rdResourceId";
            this.rdResourceId.TabStop = true;
            this.rdResourceId.UseVisualStyleBackColor = true;
            // 
            // rdMapName
            // 
            resources.ApplyResources(this.rdMapName, "rdMapName");
            this.rdMapName.Checked = true;
            this.rdMapName.Name = "rdMapName";
            this.rdMapName.TabStop = true;
            this.rdMapName.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TAB_LAYERS);
            this.tabControl1.Controls.Add(this.TAB_DRAW_ORDER);
            this.tabControl1.Controls.Add(this.TAB_SELECTION);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // TAB_LAYERS
            // 
            this.TAB_LAYERS.Controls.Add(this.trvLayersAndGroups);
            resources.ApplyResources(this.TAB_LAYERS, "TAB_LAYERS");
            this.TAB_LAYERS.Name = "TAB_LAYERS";
            this.TAB_LAYERS.UseVisualStyleBackColor = true;
            // 
            // trvLayersAndGroups
            // 
            resources.ApplyResources(this.trvLayersAndGroups, "trvLayersAndGroups");
            this.trvLayersAndGroups.ImageList = this.imgList;
            this.trvLayersAndGroups.Name = "trvLayersAndGroups";
            this.trvLayersAndGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvLayersAndGroups_AfterSelect);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "folder.png");
            this.imgList.Images.SetKeyName(1, "folder--minus.png");
            this.imgList.Images.SetKeyName(2, "layer.png");
            this.imgList.Images.SetKeyName(3, "layer--minus.png");
            this.imgList.Images.SetKeyName(4, "map.png");
            // 
            // TAB_SELECTION
            // 
            this.TAB_SELECTION.Controls.Add(this.trvSelection);
            resources.ApplyResources(this.TAB_SELECTION, "TAB_SELECTION");
            this.TAB_SELECTION.Name = "TAB_SELECTION";
            this.TAB_SELECTION.UseVisualStyleBackColor = true;
            // 
            // trvSelection
            // 
            resources.ApplyResources(this.trvSelection, "trvSelection");
            this.trvSelection.ImageList = this.imgList;
            this.trvSelection.Name = "trvSelection";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.properties);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // properties
            // 
            resources.ApplyResources(this.properties, "properties");
            this.properties.Name = "properties";
            // 
            // TAB_DRAW_ORDER
            // 
            this.TAB_DRAW_ORDER.Controls.Add(this.lstDrawOrder);
            resources.ApplyResources(this.TAB_DRAW_ORDER, "TAB_DRAW_ORDER");
            this.TAB_DRAW_ORDER.Name = "TAB_DRAW_ORDER";
            this.TAB_DRAW_ORDER.UseVisualStyleBackColor = true;
            // 
            // lstDrawOrder
            // 
            resources.ApplyResources(this.lstDrawOrder, "lstDrawOrder");
            this.lstDrawOrder.FormattingEnabled = true;
            this.lstDrawOrder.Name = "lstDrawOrder";
            this.lstDrawOrder.SelectedIndexChanged += new System.EventHandler(this.lstDrawOrder_SelectedIndexChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TAB_LAYERS.ResumeLayout(false);
            this.TAB_SELECTION.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.TAB_DRAW_ORDER.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.TextBox txtSessionId;
        private System.Windows.Forms.TextBox txtResourceId;
        private System.Windows.Forms.RadioButton rdResourceId;
        private System.Windows.Forms.RadioButton rdMapName;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PropertyGrid properties;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TAB_LAYERS;
        private System.Windows.Forms.TreeView trvLayersAndGroups;
        private System.Windows.Forms.TabPage TAB_SELECTION;
        private System.Windows.Forms.TreeView trvSelection;
        private System.Windows.Forms.TabPage TAB_DRAW_ORDER;
        private System.Windows.Forms.ListBox lstDrawOrder;
    }
}

