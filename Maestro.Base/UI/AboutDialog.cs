#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Services;
using System.Diagnostics;

namespace Maestro.Base.UI
{
	/// <summary>
	/// Summary description for FormAbout.
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel linkLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox License;
		private System.Windows.Forms.Label Version;
		private System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.PictureBox PayPalImage;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label Localization;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage licenseTab;
		private System.Windows.Forms.TabPage creditsTab;
		private System.Windows.Forms.TextBox Credits;
        private TabPage thirdPartyTab;
        private LinkLabel tfnetLinkLabel;
        private Label label2;
        private LinkLabel ziplibLinkLabel;
        private Label ServerVersion;
        private LinkLabel colorBrewerlinkLabel;
        private LinkLabel lnkSharpDevelop;
        private LinkLabel linkLabel1;
        private LinkLabel lnkAdvTreeView;
        private IServerConnection m_connection;

		private AboutDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.Icon = Properties.Resources.MapGuide_Maestro;
        }

        public AboutDialog(IServerConnection connection)
            : this()
        {
            m_connection = connection;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
            this.License = new System.Windows.Forms.TextBox();
            this.Version = new System.Windows.Forms.Label();
            this.PayPalImage = new System.Windows.Forms.PictureBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Localization = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.licenseTab = new System.Windows.Forms.TabPage();
            this.creditsTab = new System.Windows.Forms.TabPage();
            this.Credits = new System.Windows.Forms.TextBox();
            this.thirdPartyTab = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lnkSharpDevelop = new System.Windows.Forms.LinkLabel();
            this.colorBrewerlinkLabel = new System.Windows.Forms.LinkLabel();
            this.ziplibLinkLabel = new System.Windows.Forms.LinkLabel();
            this.tfnetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerVersion = new System.Windows.Forms.Label();
            this.lnkAdvTreeView = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayPalImage)).BeginInit();
            this.tabControl.SuspendLayout();
            this.licenseTab.SuspendLayout();
            this.creditsTab.SuspendLayout();
            this.thirdPartyTab.SuspendLayout();
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
            // License
            // 
            resources.ApplyResources(this.License, "License");
            this.License.Name = "License";
            this.License.ReadOnly = true;
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.licenseTab);
            this.tabControl.Controls.Add(this.creditsTab);
            this.tabControl.Controls.Add(this.thirdPartyTab);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // licenseTab
            // 
            this.licenseTab.Controls.Add(this.License);
            resources.ApplyResources(this.licenseTab, "licenseTab");
            this.licenseTab.Name = "licenseTab";
            this.licenseTab.UseVisualStyleBackColor = true;
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
            // thirdPartyTab
            // 
            resources.ApplyResources(this.thirdPartyTab, "thirdPartyTab");
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
            // ServerVersion
            // 
            resources.ApplyResources(this.ServerVersion, "ServerVersion");
            this.ServerVersion.Name = "ServerVersion";
            // 
            // lnkAdvTreeView
            // 
            resources.ApplyResources(this.lnkAdvTreeView, "lnkAdvTreeView");
            this.lnkAdvTreeView.Name = "lnkAdvTreeView";
            this.lnkAdvTreeView.TabStop = true;
            this.lnkAdvTreeView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvTreeView_LinkClicked);
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
            this.tabControl.ResumeLayout(false);
            this.licenseTab.ResumeLayout(false);
            this.licenseTab.PerformLayout();
            this.creditsTab.ResumeLayout(false);
            this.creditsTab.PerformLayout();
            this.thirdPartyTab.ResumeLayout(false);
            this.thirdPartyTab.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// Dummy function used to create a thread to read the default locale from
        /// </summary>
        private void dummy_function() { }

        private UrlLauncherService _launcher;

		private void FormAbout_Load(object sender, System.EventArgs e)
		{
            System.Threading.Thread tmp = new System.Threading.Thread(new System.Threading.ThreadStart(dummy_function));

            _launcher = ServiceRegistry.GetService<UrlLauncherService>();
            Debug.Assert(_launcher != null);

			Version.Text = string.Format(Properties.Resources.About_VersionLabel, Application.ProductVersion);
            Localization.Text = string.Format(Properties.Resources.About_LanguageLabel, System.Threading.Thread.CurrentThread.CurrentUICulture, tmp.CurrentUICulture);

            string version = "Unknown or Not Connected";
            string match = "unknown version";
            if (m_connection != null)
            {
                version = m_connection.SiteVersion.ToString();
                for (int i = 0; i < SiteVersions.SiteVersionNumbers.Length; i++)
                    if (m_connection.SiteVersion == SiteVersions.SiteVersionNumbers[i])
                        match = ((KnownSiteVersions)i).ToString();
            }

            ServerVersion.Text = string.Format(Properties.Resources.About_ServerVersionLabel, version, match);
		}

		private void PayPalImage_Click(object sender, System.EventArgs e)
		{
            _launcher.OpenUrl("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=jumpinjackie%40gmail%2ecom&lc=AU&item_name=MapGuide%20Maestro%20Project&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
		}

		private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            _launcher.OpenUrl(linkLabel.Text);
		}

        private void tfnetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://code.google.com/p/tf-net/");
        }

        private void ziplibLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://sharpdevelop.net/OpenSource/SharpZipLib/Default.aspx");
        }

        private void lnkColorBrewer_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://colorbrewer.org/");
        }

        private void lnkSharpDevelop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://sharpdevelop.net/OpenSource/SD/Default.aspx");
        }

        private void lnkIcons_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://p.yusukekamiyamane.com/");
        }

        private void lnkAdvTreeView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://www.codeproject.com/KB/tree/treeviewadv.aspx");
        }
	}
}
