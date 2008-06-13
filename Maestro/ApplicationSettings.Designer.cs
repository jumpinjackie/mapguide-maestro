namespace OSGeo.MapGuide.Maestro
{
    partial class ApplicationSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.BrowserCommand = new System.Windows.Forms.TextBox();
            this.BrowseForBrowser = new System.Windows.Forms.Button();
            this.Connections = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RegularViewer = new System.Windows.Forms.RadioButton();
            this.FusionViewer = new System.Windows.Forms.RadioButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Browser program";
            // 
            // BrowserCommand
            // 
            this.BrowserCommand.Location = new System.Drawing.Point(104, 8);
            this.BrowserCommand.Name = "BrowserCommand";
            this.BrowserCommand.Size = new System.Drawing.Size(256, 20);
            this.BrowserCommand.TabIndex = 1;
            this.toolTip.SetToolTip(this.BrowserCommand, "Leave blank for system default on Windows, and firefox for Linux");
            this.BrowserCommand.TextChanged += new System.EventHandler(this.BrowserCommand_TextChanged);
            // 
            // BrowseForBrowser
            // 
            this.BrowseForBrowser.Location = new System.Drawing.Point(360, 8);
            this.BrowseForBrowser.Name = "BrowseForBrowser";
            this.BrowseForBrowser.Size = new System.Drawing.Size(24, 20);
            this.BrowseForBrowser.TabIndex = 2;
            this.BrowseForBrowser.Text = "...";
            this.BrowseForBrowser.UseVisualStyleBackColor = true;
            this.BrowseForBrowser.Click += new System.EventHandler(this.BrowseForBrowser_Click);
            // 
            // Connections
            // 
            this.Connections.ContextMenuStrip = this.contextMenuStrip;
            this.Connections.FormattingEnabled = true;
            this.Connections.Location = new System.Drawing.Point(8, 136);
            this.Connections.Name = "Connections";
            this.Connections.Size = new System.Drawing.Size(376, 108);
            this.Connections.TabIndex = 3;
            this.toolTip.SetToolTip(this.Connections, "Right click an entry to delete it");
            this.Connections.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Connections_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Stored connection settings";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(96, 256);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(88, 24);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(200, 256);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(88, 24);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 26);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FusionViewer);
            this.groupBox1.Controls.Add(this.RegularViewer);
            this.groupBox1.Location = new System.Drawing.Point(8, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 72);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview viewer type";
            // 
            // RegularViewer
            // 
            this.RegularViewer.AutoSize = true;
            this.RegularViewer.Location = new System.Drawing.Point(8, 24);
            this.RegularViewer.Name = "RegularViewer";
            this.RegularViewer.Size = new System.Drawing.Size(158, 17);
            this.RegularViewer.TabIndex = 0;
            this.RegularViewer.TabStop = true;
            this.RegularViewer.Text = "Regular (WebLayout based)";
            this.RegularViewer.UseVisualStyleBackColor = true;
            this.RegularViewer.CheckedChanged += new System.EventHandler(this.RegularViewer_CheckedChanged);
            // 
            // FusionViewer
            // 
            this.FusionViewer.AutoSize = true;
            this.FusionViewer.Location = new System.Drawing.Point(8, 48);
            this.FusionViewer.Name = "FusionViewer";
            this.FusionViewer.Size = new System.Drawing.Size(88, 17);
            this.FusionViewer.TabIndex = 1;
            this.FusionViewer.TabStop = true;
            this.FusionViewer.Text = "Fusion based";
            this.FusionViewer.UseVisualStyleBackColor = true;
            this.FusionViewer.CheckedChanged += new System.EventHandler(this.FusionViewer_CheckedChanged);
            // 
            // ApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(392, 289);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Connections);
            this.Controls.Add(this.BrowseForBrowser);
            this.Controls.Add(this.BrowserCommand);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplicationSettings";
            this.Text = "MapGuide Maestro Settings";
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox BrowserCommand;
        private System.Windows.Forms.Button BrowseForBrowser;
        private System.Windows.Forms.ListBox Connections;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton FusionViewer;
        private System.Windows.Forms.RadioButton RegularViewer;
        private System.Windows.Forms.ToolTip toolTip;
    }
}