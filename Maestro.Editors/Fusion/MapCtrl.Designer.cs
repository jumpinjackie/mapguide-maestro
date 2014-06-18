namespace Maestro.Editors.Fusion
{
    partial class MapCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMapId = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstMaps = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNewMap = new System.Windows.Forms.ToolStripSplitButton();
            this.btnRemoveMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMapUp = new System.Windows.Forms.ToolStripButton();
            this.btnMapDown = new System.Windows.Forms.ToolStripButton();
            this.grpChildMap = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMapId
            // 
            resources.ApplyResources(this.txtMapId, "txtMapId");
            this.txtMapId.Name = "txtMapId";
            this.txtMapId.TextChanged += new System.EventHandler(this.txtMapId_TextChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.txtMapId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
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
            resources.ApplyResources(this.lstMaps, "lstMaps");
            this.lstMaps.FormattingEnabled = true;
            this.lstMaps.Name = "lstMaps";
            this.lstMaps.SelectedIndexChanged += new System.EventHandler(this.lstMaps_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewMap,
            this.btnRemoveMap,
            this.toolStripSeparator1,
            this.btnMapUp,
            this.btnMapDown});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnNewMap
            // 
            this.btnNewMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewMap.Image = global::Maestro.Editors.Properties.Resources.map__plus;
            resources.ApplyResources(this.btnNewMap, "btnNewMap");
            this.btnNewMap.Name = "btnNewMap";
            // 
            // btnRemoveMap
            // 
            this.btnRemoveMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnRemoveMap, "btnRemoveMap");
            this.btnRemoveMap.Image = global::Maestro.Editors.Properties.Resources.map__minus;
            this.btnRemoveMap.Name = "btnRemoveMap";
            this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnMapUp
            // 
            this.btnMapUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMapUp, "btnMapUp");
            this.btnMapUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnMapUp.Name = "btnMapUp";
            this.btnMapUp.Click += new System.EventHandler(this.btnMapUp_Click);
            // 
            // btnMapDown
            // 
            this.btnMapDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMapDown, "btnMapDown");
            this.btnMapDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnMapDown.Name = "btnMapDown";
            this.btnMapDown.Click += new System.EventHandler(this.btnMapDown_Click);
            // 
            // grpChildMap
            // 
            resources.ApplyResources(this.grpChildMap, "grpChildMap");
            this.grpChildMap.Name = "grpChildMap";
            this.grpChildMap.TabStop = false;
            // 
            // MapCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grpChildMap);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "MapCtrl";
            resources.ApplyResources(this, "$this");
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstMaps;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton btnNewMap;
        private System.Windows.Forms.ToolStripButton btnRemoveMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnMapUp;
        private System.Windows.Forms.ToolStripButton btnMapDown;
        private System.Windows.Forms.GroupBox grpChildMap;
    }
}
