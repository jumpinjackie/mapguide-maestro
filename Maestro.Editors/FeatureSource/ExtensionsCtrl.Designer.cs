namespace Maestro.Editors.FeatureSource
{
    partial class ExtensionsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionsCtrl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trvExtensions = new System.Windows.Forms.TreeView();
            this.trvImages = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNewExtension = new System.Windows.Forms.ToolStripButton();
            this.btnNewCalculation = new System.Windows.Forms.ToolStripButton();
            this.btnNewJoin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.contentPanel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.splitContainer1);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvExtensions);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // trvExtensions
            // 
            resources.ApplyResources(this.trvExtensions, "trvExtensions");
            this.trvExtensions.ImageList = this.trvImages;
            this.trvExtensions.Name = "trvExtensions";
            this.trvExtensions.ShowNodeToolTips = true;
            this.trvExtensions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvExtensions_AfterSelect);
            // 
            // trvImages
            // 
            this.trvImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("trvImages.ImageStream")));
            this.trvImages.TransparentColor = System.Drawing.Color.Transparent;
            this.trvImages.Images.SetKeyName(0, "table--plus.png");
            this.trvImages.Images.SetKeyName(1, "function.png");
            this.trvImages.Images.SetKeyName(2, "sql-join.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewExtension,
            this.btnNewCalculation,
            this.btnNewJoin,
            this.toolStripSeparator1,
            this.btnDelete});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnNewExtension
            // 
            this.btnNewExtension.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewExtension.Image = global::Maestro.Editors.Properties.Resources.database__plus;
            resources.ApplyResources(this.btnNewExtension, "btnNewExtension");
            this.btnNewExtension.Name = "btnNewExtension";
            this.btnNewExtension.Click += new System.EventHandler(this.btnNewExtension_Click);
            // 
            // btnNewCalculation
            // 
            this.btnNewCalculation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnNewCalculation, "btnNewCalculation");
            this.btnNewCalculation.Image = global::Maestro.Editors.Properties.Resources.function;
            this.btnNewCalculation.Name = "btnNewCalculation";
            this.btnNewCalculation.Click += new System.EventHandler(this.btnNewCalculation_Click);
            // 
            // btnNewJoin
            // 
            this.btnNewJoin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnNewJoin, "btnNewJoin");
            this.btnNewJoin.Image = global::Maestro.Editors.Properties.Resources.sql_join;
            this.btnNewJoin.Name = "btnNewJoin";
            this.btnNewJoin.Click += new System.EventHandler(this.btnNewJoin_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ExtensionsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.HeaderText = "Extensions and Joins";
            this.Name = "ExtensionsCtrl";
            this.contentPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView trvExtensions;
        private System.Windows.Forms.ImageList trvImages;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNewExtension;
        private System.Windows.Forms.ToolStripButton btnNewCalculation;
        private System.Windows.Forms.ToolStripButton btnNewJoin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDelete;
    }
}
