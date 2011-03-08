namespace Maestro.Editors.Fusion
{
    partial class MapSettingsCtrl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSettingsCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstMaps = new System.Windows.Forms.ListView();
            this.mapImgList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddMap = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveMap = new System.Windows.Forms.ToolStripButton();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.propertiesPanel);
            this.contentPanel.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lstMaps);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstMaps
            // 
            this.lstMaps.AllowDrop = true;
            resources.ApplyResources(this.lstMaps, "lstMaps");
            this.lstMaps.Name = "lstMaps";
            this.lstMaps.SmallImageList = this.mapImgList;
            this.lstMaps.UseCompatibleStateImageBehavior = false;
            this.lstMaps.View = System.Windows.Forms.View.List;
            this.lstMaps.SelectedIndexChanged += new System.EventHandler(this.lstMaps_SelectedIndexChanged);
            this.lstMaps.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstMaps_DragDrop);
            this.lstMaps.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstMaps_DragEnter);
            this.lstMaps.DragOver += new System.Windows.Forms.DragEventHandler(this.lstMaps_DragOver);
            // 
            // mapImgList
            // 
            this.mapImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mapImgList.ImageStream")));
            this.mapImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.mapImgList.Images.SetKeyName(0, "map.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddMap,
            this.btnRemoveMap});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddMap
            // 
            this.btnAddMap.Image = global::Maestro.Editors.Properties.Resources.map__plus;
            resources.ApplyResources(this.btnAddMap, "btnAddMap");
            this.btnAddMap.Name = "btnAddMap";
            this.btnAddMap.Click += new System.EventHandler(this.btnAddMap_Click);
            // 
            // btnRemoveMap
            // 
            resources.ApplyResources(this.btnRemoveMap, "btnRemoveMap");
            this.btnRemoveMap.Image = global::Maestro.Editors.Properties.Resources.map__minus;
            this.btnRemoveMap.Name = "btnRemoveMap";
            this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
            // 
            // propertiesPanel
            // 
            resources.ApplyResources(this.propertiesPanel, "propertiesPanel");
            this.propertiesPanel.Name = "propertiesPanel";
            // 
            // MapSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Maps";
            this.Name = "MapSettingsCtrl";
            resources.ApplyResources(this, "$this");
            this.contentPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstMaps;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddMap;
        private System.Windows.Forms.ToolStripButton btnRemoveMap;
        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.ImageList mapImgList;
    }
}
