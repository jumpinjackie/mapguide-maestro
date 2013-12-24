namespace Maestro.Editors.MapDefinition
{
    partial class CommonPropertyCtrl
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
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.lnkCount = new System.Windows.Forms.LinkLabel();
            this.grpProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpProperties
            // 
            this.grpProperties.Controls.Add(this.lnkCount);
            this.grpProperties.Controls.Add(this.propGrid);
            this.grpProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProperties.Location = new System.Drawing.Point(0, 0);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.Size = new System.Drawing.Size(245, 261);
            this.grpProperties.TabIndex = 0;
            this.grpProperties.TabStop = false;
            this.grpProperties.Text = "Common Properties";
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(3, 16);
            this.propGrid.Name = "propGrid";
            this.propGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propGrid.Size = new System.Drawing.Size(239, 242);
            this.propGrid.TabIndex = 0;
            this.propGrid.ToolbarVisible = false;
            // 
            // lnkCount
            // 
            this.lnkCount.AutoSize = true;
            this.lnkCount.Location = new System.Drawing.Point(106, 0);
            this.lnkCount.Name = "lnkCount";
            this.lnkCount.Size = new System.Drawing.Size(0, 13);
            this.lnkCount.TabIndex = 1;
            this.lnkCount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCount_LinkClicked);
            // 
            // CommonPropertyCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpProperties);
            this.Name = "CommonPropertyCtrl";
            this.Size = new System.Drawing.Size(245, 261);
            this.grpProperties.ResumeLayout(false);
            this.grpProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpProperties;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.LinkLabel lnkCount;
    }
}
