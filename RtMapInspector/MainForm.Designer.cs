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
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.trvLayersAndGroups = new System.Windows.Forms.TreeView();
            this.TAB_LAYERS = new System.Windows.Forms.TabPage();
            this.TAB_SELECTION = new System.Windows.Forms.TabPage();
            this.trvSelection = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TAB_LAYERS.SuspendLayout();
            this.TAB_SELECTION.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMapName);
            this.groupBox1.Controls.Add(this.txtSessionId);
            this.groupBox1.Controls.Add(this.txtResourceId);
            this.groupBox1.Controls.Add(this.rdResourceId);
            this.groupBox1.Controls.Add(this.rdMapName);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(497, 97);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load Map";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Session ID";
            // 
            // txtMapName
            // 
            this.txtMapName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMapName.Location = new System.Drawing.Point(133, 45);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(439, 20);
            this.txtMapName.TabIndex = 4;
            // 
            // txtSessionId
            // 
            this.txtSessionId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSessionId.Location = new System.Drawing.Point(133, 19);
            this.txtSessionId.Name = "txtSessionId";
            this.txtSessionId.Size = new System.Drawing.Size(439, 20);
            this.txtSessionId.TabIndex = 3;
            // 
            // txtResourceId
            // 
            this.txtResourceId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResourceId.Location = new System.Drawing.Point(133, 71);
            this.txtResourceId.Name = "txtResourceId";
            this.txtResourceId.Size = new System.Drawing.Size(439, 20);
            this.txtResourceId.TabIndex = 2;
            // 
            // rdResourceId
            // 
            this.rdResourceId.AutoSize = true;
            this.rdResourceId.Location = new System.Drawing.Point(20, 72);
            this.rdResourceId.Name = "rdResourceId";
            this.rdResourceId.Size = new System.Drawing.Size(85, 17);
            this.rdResourceId.TabIndex = 1;
            this.rdResourceId.TabStop = true;
            this.rdResourceId.Text = "Resource ID";
            this.rdResourceId.UseVisualStyleBackColor = true;
            // 
            // rdMapName
            // 
            this.rdMapName.AutoSize = true;
            this.rdMapName.Checked = true;
            this.rdMapName.Location = new System.Drawing.Point(20, 45);
            this.rdMapName.Name = "rdMapName";
            this.rdMapName.Size = new System.Drawing.Size(77, 17);
            this.rdMapName.TabIndex = 0;
            this.rdMapName.TabStop = true;
            this.rdMapName.Text = "Map Name";
            this.rdMapName.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(13, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 328);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Runtime Map";
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
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.properties);
            this.groupBox3.Location = new System.Drawing.Point(340, 155);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 328);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Properties";
            // 
            // properties
            // 
            this.properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.properties.Location = new System.Drawing.Point(3, 16);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(264, 309);
            this.properties.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TAB_LAYERS);
            this.tabControl1.Controls.Add(this.TAB_SELECTION);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(315, 309);
            this.tabControl1.TabIndex = 1;
            // 
            // trvLayersAndGroups
            // 
            this.trvLayersAndGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvLayersAndGroups.ImageIndex = 0;
            this.trvLayersAndGroups.ImageList = this.imgList;
            this.trvLayersAndGroups.Location = new System.Drawing.Point(3, 3);
            this.trvLayersAndGroups.Name = "trvLayersAndGroups";
            this.trvLayersAndGroups.SelectedImageIndex = 0;
            this.trvLayersAndGroups.Size = new System.Drawing.Size(301, 277);
            this.trvLayersAndGroups.TabIndex = 0;
            this.trvLayersAndGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvLayersAndGroups_AfterSelect);
            // 
            // TAB_LAYERS
            // 
            this.TAB_LAYERS.Controls.Add(this.trvLayersAndGroups);
            this.TAB_LAYERS.Location = new System.Drawing.Point(4, 22);
            this.TAB_LAYERS.Name = "TAB_LAYERS";
            this.TAB_LAYERS.Padding = new System.Windows.Forms.Padding(3);
            this.TAB_LAYERS.Size = new System.Drawing.Size(307, 283);
            this.TAB_LAYERS.TabIndex = 0;
            this.TAB_LAYERS.Text = "Layers and Groups";
            this.TAB_LAYERS.UseVisualStyleBackColor = true;
            // 
            // TAB_SELECTION
            // 
            this.TAB_SELECTION.Controls.Add(this.trvSelection);
            this.TAB_SELECTION.Location = new System.Drawing.Point(4, 22);
            this.TAB_SELECTION.Name = "TAB_SELECTION";
            this.TAB_SELECTION.Size = new System.Drawing.Size(307, 283);
            this.TAB_SELECTION.TabIndex = 1;
            this.TAB_SELECTION.Text = "Current Selection";
            this.TAB_SELECTION.UseVisualStyleBackColor = true;
            // 
            // trvSelection
            // 
            this.trvSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvSelection.ImageIndex = 0;
            this.trvSelection.ImageList = this.imgList;
            this.trvSelection.Location = new System.Drawing.Point(0, 0);
            this.trvSelection.Name = "trvSelection";
            this.trvSelection.SelectedImageIndex = 0;
            this.trvSelection.Size = new System.Drawing.Size(307, 283);
            this.trvSelection.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 495);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Runtime Map Inspector";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TAB_LAYERS.ResumeLayout(false);
            this.TAB_SELECTION.ResumeLayout(false);
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
    }
}

