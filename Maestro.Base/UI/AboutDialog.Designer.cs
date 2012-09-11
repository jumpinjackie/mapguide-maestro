using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Base.UI
{
    partial class AboutDialog
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.PayPalImage = new System.Windows.Forms.PictureBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Localization = new System.Windows.Forms.Label();
            this.ServerVersion = new System.Windows.Forms.Label();
            this.thirdPartyTab = new System.Windows.Forms.TabPage();
            this.lnkDockPanel = new System.Windows.Forms.LinkLabel();
            this.lnkAdvTreeView = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lnkSharpDevelop = new System.Windows.Forms.LinkLabel();
            this.colorBrewerlinkLabel = new System.Windows.Forms.LinkLabel();
            this.ziplibLinkLabel = new System.Windows.Forms.LinkLabel();
            this.tfnetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.creditsTab = new System.Windows.Forms.TabPage();
            this.Credits = new System.Windows.Forms.TextBox();
            this.licenseTab = new System.Windows.Forms.TabPage();
            this.License = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayPalImage)).BeginInit();
            this.thirdPartyTab.SuspendLayout();
            this.creditsTab.SuspendLayout();
            this.licenseTab.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel
            // 
            resources.ApplyResources(this.linkLabel, "linkLabel");
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.TabStop = true;
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Version
            // 
            resources.ApplyResources(this.Version, "Version");
            this.Version.Name = "Version";
            // 
            // PayPalImage
            // 
            this.PayPalImage.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.PayPalImage, "PayPalImage");
            this.PayPalImage.Name = "PayPalImage";
            this.PayPalImage.TabStop = false;
            this.ToolTip.SetToolTip(this.PayPalImage, resources.GetString("PayPalImage.ToolTip"));
            this.PayPalImage.Click += new System.EventHandler(this.PayPalImage_Click);
            // 
            // Localization
            // 
            resources.ApplyResources(this.Localization, "Localization");
            this.Localization.Name = "Localization";
            // 
            // ServerVersion
            // 
            resources.ApplyResources(this.ServerVersion, "ServerVersion");
            this.ServerVersion.Name = "ServerVersion";
            // 
            // thirdPartyTab
            // 
            resources.ApplyResources(this.thirdPartyTab, "thirdPartyTab");
            this.thirdPartyTab.Controls.Add(this.lnkDockPanel);
            this.thirdPartyTab.Controls.Add(this.lnkAdvTreeView);
            this.thirdPartyTab.Controls.Add(this.linkLabel1);
            this.thirdPartyTab.Controls.Add(this.lnkSharpDevelop);
            this.thirdPartyTab.Controls.Add(this.colorBrewerlinkLabel);
            this.thirdPartyTab.Controls.Add(this.ziplibLinkLabel);
            this.thirdPartyTab.Controls.Add(this.tfnetLinkLabel);
            this.thirdPartyTab.Controls.Add(this.label2);
            this.thirdPartyTab.Name = "thirdPartyTab";
            this.thirdPartyTab.UseVisualStyleBackColor = true;
            // 
            // lnkDockPanel
            // 
            resources.ApplyResources(this.lnkDockPanel, "lnkDockPanel");
            this.lnkDockPanel.Name = "lnkDockPanel";
            this.lnkDockPanel.TabStop = true;
            this.lnkDockPanel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDockPanel_LinkClicked);
            // 
            // lnkAdvTreeView
            // 
            resources.ApplyResources(this.lnkAdvTreeView, "lnkAdvTreeView");
            this.lnkAdvTreeView.Name = "lnkAdvTreeView";
            this.lnkAdvTreeView.TabStop = true;
            this.lnkAdvTreeView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvTreeView_LinkClicked);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIcons_Clicked);
            // 
            // lnkSharpDevelop
            // 
            resources.ApplyResources(this.lnkSharpDevelop, "lnkSharpDevelop");
            this.lnkSharpDevelop.Name = "lnkSharpDevelop";
            this.lnkSharpDevelop.TabStop = true;
            this.lnkSharpDevelop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSharpDevelop_LinkClicked);
            // 
            // colorBrewerlinkLabel
            // 
            resources.ApplyResources(this.colorBrewerlinkLabel, "colorBrewerlinkLabel");
            this.colorBrewerlinkLabel.Name = "colorBrewerlinkLabel";
            this.colorBrewerlinkLabel.TabStop = true;
            this.colorBrewerlinkLabel.UseCompatibleTextRendering = true;
            this.colorBrewerlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkColorBrewer_Clicked);
            // 
            // ziplibLinkLabel
            // 
            resources.ApplyResources(this.ziplibLinkLabel, "ziplibLinkLabel");
            this.ziplibLinkLabel.Name = "ziplibLinkLabel";
            this.ziplibLinkLabel.TabStop = true;
            this.ziplibLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ziplibLinkLabel_LinkClicked);
            // 
            // tfnetLinkLabel
            // 
            resources.ApplyResources(this.tfnetLinkLabel, "tfnetLinkLabel");
            this.tfnetLinkLabel.Name = "tfnetLinkLabel";
            this.tfnetLinkLabel.TabStop = true;
            this.tfnetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.tfnetLinkLabel_LinkClicked);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // creditsTab
            // 
            this.creditsTab.Controls.Add(this.Credits);
            resources.ApplyResources(this.creditsTab, "creditsTab");
            this.creditsTab.Name = "creditsTab";
            this.creditsTab.UseVisualStyleBackColor = true;
            // 
            // Credits
            // 
            resources.ApplyResources(this.Credits, "Credits");
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            // 
            // licenseTab
            // 
            this.licenseTab.Controls.Add(this.License);
            resources.ApplyResources(this.licenseTab, "licenseTab");
            this.licenseTab.Name = "licenseTab";
            this.licenseTab.UseVisualStyleBackColor = true;
            // 
            // License
            // 
            resources.ApplyResources(this.License, "License");
            this.License.Name = "License";
            this.License.ReadOnly = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.licenseTab);
            this.tabControl.Controls.Add(this.creditsTab);
            this.tabControl.Controls.Add(this.thirdPartyTab);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // AboutDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ServerVersion);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.Localization);
            this.Controls.Add(this.PayPalImage);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayPalImage)).EndInit();
            this.thirdPartyTab.ResumeLayout(false);
            this.thirdPartyTab.PerformLayout();
            this.creditsTab.ResumeLayout(false);
            this.creditsTab.PerformLayout();
            this.licenseTab.ResumeLayout(false);
            this.licenseTab.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.PictureBox PayPalImage;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label Localization;
        private System.Windows.Forms.Label ServerVersion;
        private System.Windows.Forms.TabPage thirdPartyTab;
        private System.Windows.Forms.LinkLabel lnkAdvTreeView;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel lnkSharpDevelop;
        private System.Windows.Forms.LinkLabel colorBrewerlinkLabel;
        private System.Windows.Forms.LinkLabel ziplibLinkLabel;
        private System.Windows.Forms.LinkLabel tfnetLinkLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage creditsTab;
        private System.Windows.Forms.TextBox Credits;
        private System.Windows.Forms.TabPage licenseTab;
        private System.Windows.Forms.TextBox License;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.LinkLabel lnkDockPanel;
    }
}
