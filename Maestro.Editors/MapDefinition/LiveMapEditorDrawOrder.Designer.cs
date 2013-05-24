
namespace Maestro.Editors.MapDefinition
{
    partial class LiveMapEditorDrawOrder
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.lstDrawOrder = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUp,
            this.btnDown,
            this.toolStripSeparator1,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(241, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Enabled = false;
            this.btnUp.Image = global::Maestro.Editors.Properties.Resources.arrow_090;
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "Move Up";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDown.Enabled = false;
            this.btnDown.Image = global::Maestro.Editors.Properties.Resources.arrow_270;
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Text = "Move Down";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::Maestro.Editors.Properties.Resources.cross_script;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lstDrawOrder
            // 
            this.lstDrawOrder.AllowDrop = true;
            this.lstDrawOrder.DisplayMember = "DisplayString";
            this.lstDrawOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDrawOrder.FormattingEnabled = true;
            this.lstDrawOrder.Location = new System.Drawing.Point(0, 25);
            this.lstDrawOrder.Name = "lstDrawOrder";
            this.lstDrawOrder.Size = new System.Drawing.Size(241, 380);
            this.lstDrawOrder.TabIndex = 1;
            this.lstDrawOrder.SelectedIndexChanged += new System.EventHandler(this.lstDrawOrder_SelectedIndexChanged);
            this.lstDrawOrder.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstDrawOrder_DragDrop);
            this.lstDrawOrder.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstDrawOrder_DragEnter);
            this.lstDrawOrder.DragOver += new System.Windows.Forms.DragEventHandler(this.lstDrawOrder_DragOver);
            this.lstDrawOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstDrawOrder_MouseDown);
            // 
            // LiveMapEditorDrawOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstDrawOrder);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LiveMapEditorDrawOrder";
            this.Size = new System.Drawing.Size(241, 405);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnDown;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ListBox lstDrawOrder;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}
