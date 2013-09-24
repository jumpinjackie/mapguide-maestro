namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    partial class SymbolInstancesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolInstancesDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstInstances = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.referenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inlineSimpleSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inlineCompoundSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.symPreview = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnEditInstanceProperties = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.symPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstInstances);
            this.groupBox1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lstInstances
            // 
            resources.ApplyResources(this.lstInstances, "lstInstances");
            this.lstInstances.LargeImageList = this.imageList1;
            this.lstInstances.Name = "lstInstances";
            this.lstInstances.SmallImageList = this.imageList1;
            this.lstInstances.UseCompatibleStateImageBehavior = false;
            this.lstInstances.View = System.Windows.Forms.View.List;
            this.lstInstances.SelectedIndexChanged += new System.EventHandler(this.lstInstances_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow.png");
            this.imageList1.Images.SetKeyName(1, "marker.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.referenceToolStripMenuItem,
            this.inlineSimpleSymbolToolStripMenuItem,
            this.inlineCompoundSymbolToolStripMenuItem});
            this.btnAdd.Image = global::Maestro.Editors.Properties.Resources.plus_circle;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // referenceToolStripMenuItem
            // 
            this.referenceToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.arrow;
            this.referenceToolStripMenuItem.Name = "referenceToolStripMenuItem";
            resources.ApplyResources(this.referenceToolStripMenuItem, "referenceToolStripMenuItem");
            this.referenceToolStripMenuItem.Click += new System.EventHandler(this.referenceToolStripMenuItem_Click);
            // 
            // inlineSimpleSymbolToolStripMenuItem
            // 
            this.inlineSimpleSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.inlineSimpleSymbolToolStripMenuItem.Name = "inlineSimpleSymbolToolStripMenuItem";
            resources.ApplyResources(this.inlineSimpleSymbolToolStripMenuItem, "inlineSimpleSymbolToolStripMenuItem");
            this.inlineSimpleSymbolToolStripMenuItem.Click += new System.EventHandler(this.inlineSimpleSymbolToolStripMenuItem_Click);
            // 
            // inlineCompoundSymbolToolStripMenuItem
            // 
            this.inlineCompoundSymbolToolStripMenuItem.Image = global::Maestro.Editors.Properties.Resources.marker;
            this.inlineCompoundSymbolToolStripMenuItem.Name = "inlineCompoundSymbolToolStripMenuItem";
            resources.ApplyResources(this.inlineCompoundSymbolToolStripMenuItem, "inlineCompoundSymbolToolStripMenuItem");
            this.inlineCompoundSymbolToolStripMenuItem.Click += new System.EventHandler(this.inlineCompoundSymbolToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.symPreview);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // symPreview
            // 
            resources.ApplyResources(this.symPreview, "symPreview");
            this.symPreview.Name = "symPreview";
            this.symPreview.TabStop = false;
            this.symPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnEditInstanceProperties
            // 
            resources.ApplyResources(this.btnEditInstanceProperties, "btnEditInstanceProperties");
            this.btnEditInstanceProperties.Name = "btnEditInstanceProperties";
            this.btnEditInstanceProperties.UseVisualStyleBackColor = true;
            this.btnEditInstanceProperties.Click += new System.EventHandler(this.btnEditInstanceProperties_Click);
            // 
            // SymbolInstancesDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnEditInstanceProperties);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnClose);
            this.Name = "SymbolInstancesDialog";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.symPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstInstances;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem referenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inlineSimpleSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inlineCompoundSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox symPreview;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnEditInstanceProperties;
    }
}