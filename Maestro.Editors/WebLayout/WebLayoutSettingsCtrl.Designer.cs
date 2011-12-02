namespace Maestro.Editors.WebLayout
{
    partial class WebLayoutSettingsCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebLayoutSettingsCtrl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBrowserTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMapDefinition = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCustomView = new System.Windows.Forms.CheckBox();
            this.numX = new System.Windows.Forms.TextBox();
            this.numY = new System.Windows.Forms.TextBox();
            this.numScale = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numTaskPaneWidth = new System.Windows.Forms.TextBox();
            this.chkTaskBar = new System.Windows.Forms.CheckBox();
            this.chkTaskPane = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkZoomControl = new System.Windows.Forms.CheckBox();
            this.chkStatusBar = new System.Windows.Forms.CheckBox();
            this.chkContextMenu = new System.Windows.Forms.CheckBox();
            this.chkToolbar = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numInfoPaneWidth = new System.Windows.Forms.TextBox();
            this.chkProperties = new System.Windows.Forms.CheckBox();
            this.chkLegend = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtInitialTaskPaneUrl = new System.Windows.Forms.TextBox();
            this.txtHyperlinkFrame = new System.Windows.Forms.TextBox();
            this.cmbHyperlinkTarget = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAjaxViewerUrl = new System.Windows.Forms.TextBox();
            this.btnShowInBrowser = new System.Windows.Forms.Button();
            this.chkPingServer = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.chkPingServer);
            this.contentPanel.Controls.Add(this.btnShowInBrowser);
            this.contentPanel.Controls.Add(this.txtAjaxViewerUrl);
            this.contentPanel.Controls.Add(this.label11);
            this.contentPanel.Controls.Add(this.label10);
            this.contentPanel.Controls.Add(this.cmbHyperlinkTarget);
            this.contentPanel.Controls.Add(this.txtHyperlinkFrame);
            this.contentPanel.Controls.Add(this.txtInitialTaskPaneUrl);
            this.contentPanel.Controls.Add(this.label9);
            this.contentPanel.Controls.Add(this.groupBox2);
            this.contentPanel.Controls.Add(this.groupBox1);
            this.contentPanel.Controls.Add(this.btnBrowse);
            this.contentPanel.Controls.Add(this.txtMapDefinition);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.txtBrowserTitle);
            this.contentPanel.Controls.Add(this.label1);
            resources.ApplyResources(this.contentPanel, "contentPanel");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBrowserTitle
            // 
            resources.ApplyResources(this.txtBrowserTitle, "txtBrowserTitle");
            this.txtBrowserTitle.Name = "txtBrowserTitle";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtMapDefinition
            // 
            resources.ApplyResources(this.txtMapDefinition, "txtMapDefinition");
            this.txtMapDefinition.Name = "txtMapDefinition";
            this.txtMapDefinition.ReadOnly = true;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.chkCustomView);
            this.groupBox1.Controls.Add(this.numX);
            this.groupBox1.Controls.Add(this.numY);
            this.groupBox1.Controls.Add(this.numScale);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkCustomView
            // 
            resources.ApplyResources(this.chkCustomView, "chkCustomView");
            this.chkCustomView.Name = "chkCustomView";
            this.chkCustomView.UseVisualStyleBackColor = true;
            this.chkCustomView.CheckedChanged += new System.EventHandler(this.chkCustomView_CheckedChanged);
            // 
            // numX
            // 
            resources.ApplyResources(this.numX, "numX");
            this.numX.Name = "numX";
            // 
            // numY
            // 
            resources.ApplyResources(this.numY, "numY");
            this.numY.Name = "numY";
            // 
            // numScale
            // 
            resources.ApplyResources(this.numScale, "numScale");
            this.numScale.Name = "numScale";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.numTaskPaneWidth);
            this.groupBox5.Controls.Add(this.chkTaskBar);
            this.groupBox5.Controls.Add(this.chkTaskPane);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // numTaskPaneWidth
            // 
            resources.ApplyResources(this.numTaskPaneWidth, "numTaskPaneWidth");
            this.numTaskPaneWidth.Name = "numTaskPaneWidth";
            // 
            // chkTaskBar
            // 
            resources.ApplyResources(this.chkTaskBar, "chkTaskBar");
            this.chkTaskBar.Name = "chkTaskBar";
            this.chkTaskBar.UseVisualStyleBackColor = true;
            // 
            // chkTaskPane
            // 
            resources.ApplyResources(this.chkTaskPane, "chkTaskPane");
            this.chkTaskPane.Name = "chkTaskPane";
            this.chkTaskPane.UseVisualStyleBackColor = true;
            this.chkTaskPane.CheckedChanged += new System.EventHandler(this.chkTaskPane_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkZoomControl);
            this.groupBox4.Controls.Add(this.chkStatusBar);
            this.groupBox4.Controls.Add(this.chkContextMenu);
            this.groupBox4.Controls.Add(this.chkToolbar);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // chkZoomControl
            // 
            resources.ApplyResources(this.chkZoomControl, "chkZoomControl");
            this.chkZoomControl.Name = "chkZoomControl";
            this.chkZoomControl.UseVisualStyleBackColor = true;
            // 
            // chkStatusBar
            // 
            resources.ApplyResources(this.chkStatusBar, "chkStatusBar");
            this.chkStatusBar.Name = "chkStatusBar";
            this.chkStatusBar.UseVisualStyleBackColor = true;
            // 
            // chkContextMenu
            // 
            resources.ApplyResources(this.chkContextMenu, "chkContextMenu");
            this.chkContextMenu.Name = "chkContextMenu";
            this.chkContextMenu.UseVisualStyleBackColor = true;
            // 
            // chkToolbar
            // 
            resources.ApplyResources(this.chkToolbar, "chkToolbar");
            this.chkToolbar.Name = "chkToolbar";
            this.chkToolbar.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.numInfoPaneWidth);
            this.groupBox3.Controls.Add(this.chkProperties);
            this.groupBox3.Controls.Add(this.chkLegend);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // numInfoPaneWidth
            // 
            resources.ApplyResources(this.numInfoPaneWidth, "numInfoPaneWidth");
            this.numInfoPaneWidth.Name = "numInfoPaneWidth";
            // 
            // chkProperties
            // 
            resources.ApplyResources(this.chkProperties, "chkProperties");
            this.chkProperties.Name = "chkProperties";
            this.chkProperties.UseVisualStyleBackColor = true;
            this.chkProperties.CheckedChanged += new System.EventHandler(this.chkProperties_CheckedChanged);
            // 
            // chkLegend
            // 
            resources.ApplyResources(this.chkLegend, "chkLegend");
            this.chkLegend.Name = "chkLegend";
            this.chkLegend.UseVisualStyleBackColor = true;
            this.chkLegend.CheckedChanged += new System.EventHandler(this.chkLegend_CheckedChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // txtInitialTaskPaneUrl
            // 
            resources.ApplyResources(this.txtInitialTaskPaneUrl, "txtInitialTaskPaneUrl");
            this.txtInitialTaskPaneUrl.Name = "txtInitialTaskPaneUrl";
            // 
            // txtHyperlinkFrame
            // 
            resources.ApplyResources(this.txtHyperlinkFrame, "txtHyperlinkFrame");
            this.txtHyperlinkFrame.Name = "txtHyperlinkFrame";
            // 
            // cmbHyperlinkTarget
            // 
            this.cmbHyperlinkTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHyperlinkTarget.FormattingEnabled = true;
            resources.ApplyResources(this.cmbHyperlinkTarget, "cmbHyperlinkTarget");
            this.cmbHyperlinkTarget.Name = "cmbHyperlinkTarget";
            this.cmbHyperlinkTarget.SelectedIndexChanged += new System.EventHandler(this.cmbHyperlinkTarget_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // txtAjaxViewerUrl
            // 
            resources.ApplyResources(this.txtAjaxViewerUrl, "txtAjaxViewerUrl");
            this.txtAjaxViewerUrl.Name = "txtAjaxViewerUrl";
            this.txtAjaxViewerUrl.ReadOnly = true;
            // 
            // btnShowInBrowser
            // 
            resources.ApplyResources(this.btnShowInBrowser, "btnShowInBrowser");
            this.btnShowInBrowser.Name = "btnShowInBrowser";
            this.btnShowInBrowser.UseVisualStyleBackColor = true;
            this.btnShowInBrowser.Click += new System.EventHandler(this.btnShowInBrowser_Click);
            // 
            // chkPingServer
            // 
            resources.ApplyResources(this.chkPingServer, "chkPingServer");
            this.chkPingServer.Name = "chkPingServer";
            this.chkPingServer.UseVisualStyleBackColor = true;
            // 
            // WebLayoutSettingsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "WebLayoutSettingsCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtMapDefinition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBrowserTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCustomView;
        private System.Windows.Forms.TextBox numX;
        private System.Windows.Forms.TextBox numY;
        private System.Windows.Forms.TextBox numScale;
        private System.Windows.Forms.CheckBox chkZoomControl;
        private System.Windows.Forms.CheckBox chkStatusBar;
        private System.Windows.Forms.CheckBox chkContextMenu;
        private System.Windows.Forms.CheckBox chkToolbar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox numInfoPaneWidth;
        private System.Windows.Forms.CheckBox chkProperties;
        private System.Windows.Forms.CheckBox chkLegend;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox numTaskPaneWidth;
        private System.Windows.Forms.CheckBox chkTaskBar;
        private System.Windows.Forms.CheckBox chkTaskPane;
        private System.Windows.Forms.Button btnShowInBrowser;
        private System.Windows.Forms.TextBox txtAjaxViewerUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbHyperlinkTarget;
        private System.Windows.Forms.TextBox txtHyperlinkFrame;
        private System.Windows.Forms.TextBox txtInitialTaskPaneUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkPingServer;
    }
}
