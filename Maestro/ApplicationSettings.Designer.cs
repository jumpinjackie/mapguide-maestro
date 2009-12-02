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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.BrowserCommand = new System.Windows.Forms.TextBox();
            this.BrowseForBrowser = new System.Windows.Forms.Button();
            this.Connections = new System.Windows.Forms.ListBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FusionViewer = new System.Windows.Forms.RadioButton();
            this.RegularViewer = new System.Windows.Forms.RadioButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SelectBrowser = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // BrowserCommand
            // 
            resources.ApplyResources(this.BrowserCommand, "BrowserCommand");
            this.BrowserCommand.Name = "BrowserCommand";
            this.toolTip.SetToolTip(this.BrowserCommand, resources.GetString("BrowserCommand.ToolTip"));
            this.BrowserCommand.TextChanged += new System.EventHandler(this.BrowserCommand_TextChanged);
            // 
            // BrowseForBrowser
            // 
            resources.ApplyResources(this.BrowseForBrowser, "BrowseForBrowser");
            this.BrowseForBrowser.Name = "BrowseForBrowser";
            this.BrowseForBrowser.UseVisualStyleBackColor = true;
            this.BrowseForBrowser.Click += new System.EventHandler(this.BrowseForBrowser_Click);
            // 
            // Connections
            // 
            this.Connections.ContextMenuStrip = this.contextMenuStrip;
            this.Connections.FormattingEnabled = true;
            resources.ApplyResources(this.Connections, "Connections");
            this.Connections.Name = "Connections";
            this.toolTip.SetToolTip(this.Connections, resources.GetString("Connections.ToolTip"));
            this.Connections.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Connections_KeyUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            resources.ApplyResources(this.removeToolStripMenuItem, "removeToolStripMenuItem");
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FusionViewer);
            this.groupBox1.Controls.Add(this.RegularViewer);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // FusionViewer
            // 
            resources.ApplyResources(this.FusionViewer, "FusionViewer");
            this.FusionViewer.Name = "FusionViewer";
            this.FusionViewer.TabStop = true;
            this.FusionViewer.UseVisualStyleBackColor = true;
            this.FusionViewer.CheckedChanged += new System.EventHandler(this.FusionViewer_CheckedChanged);
            // 
            // RegularViewer
            // 
            resources.ApplyResources(this.RegularViewer, "RegularViewer");
            this.RegularViewer.Name = "RegularViewer";
            this.RegularViewer.TabStop = true;
            this.RegularViewer.UseVisualStyleBackColor = true;
            this.RegularViewer.CheckedChanged += new System.EventHandler(this.RegularViewer_CheckedChanged);
            // 
            // SelectBrowser
            // 
            this.SelectBrowser.FileName = "SelectBrowser";
            resources.ApplyResources(this.SelectBrowser, "SelectBrowser");
            // 
            // ApplicationSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
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
        private System.Windows.Forms.OpenFileDialog SelectBrowser;
    }
}