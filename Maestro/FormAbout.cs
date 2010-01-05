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

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for FormAbout.
	/// </summary>
	public class FormAbout : System.Windows.Forms.Form
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
        private MaestroAPI.ServerConnectionI m_connection;

		private FormAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.Icon = FormMain.MaestroIcon;
        }

        public FormAbout(MaestroAPI.ServerConnectionI connection)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
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
            this.colorBrewerlinkLabel = new System.Windows.Forms.LinkLabel();
            this.ziplibLinkLabel = new System.Windows.Forms.LinkLabel();
            this.tfnetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerVersion = new System.Windows.Forms.Label();
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
            this.thirdPartyTab.Controls.Add(this.colorBrewerlinkLabel);
            this.thirdPartyTab.Controls.Add(this.ziplibLinkLabel);
            this.thirdPartyTab.Controls.Add(this.tfnetLinkLabel);
            this.thirdPartyTab.Controls.Add(this.label2);
            resources.ApplyResources(this.thirdPartyTab, "thirdPartyTab");
            this.thirdPartyTab.Name = "thirdPartyTab";
            this.thirdPartyTab.UseVisualStyleBackColor = true;
            // 
            // colorBrewerlinkLabel
            // 
            resources.ApplyResources(this.colorBrewerlinkLabel, "colorBrewerlinkLabel");
            this.colorBrewerlinkLabel.Name = "colorBrewerlinkLabel";
            this.colorBrewerlinkLabel.TabStop = true;
            this.colorBrewerlinkLabel.UseCompatibleTextRendering = true;
            this.colorBrewerlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
            // FormAbout
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
            this.Name = "FormAbout";
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

		private void FormAbout_Load(object sender, System.EventArgs e)
		{
            System.Threading.Thread tmp = new System.Threading.Thread(new System.Threading.ThreadStart(dummy_function));

			Version.Text = string.Format(Strings.FormAbout.VersionLabel, Application.ProductVersion);
			Localization.Text = string.Format(Strings.FormAbout.LanguageLabel,  System.Threading.Thread.CurrentThread.CurrentUICulture, tmp.CurrentUICulture);

            string match = "unknown version";
            for(int i = 0; i < MaestroAPI.SiteVersions.SiteVersionNumbers.Length; i++)
                if (m_connection.SiteVersion == MaestroAPI.SiteVersions.SiteVersionNumbers[i])
                    match = ((MaestroAPI.KnownSiteVersions)i).ToString();

            ServerVersion.Text = string.Format(Strings.FormAbout.ServerVersionLabel, m_connection.SiteVersion, match);
		}

		private void PayPalImage_Click(object sender, System.EventArgs e)
		{
			Program.OpenUrl("https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=paypal%40hexad%2edk&item_name=Maestro&no_shipping=2&no_note=1&tax=0&currency_code=EUR&lc=DK&bn=PP%2dDonationsBF&charset=UTF%2d8");
		}

		private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            Program.OpenUrl(linkLabel.Text);
		}

        private void tfnetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenUrl("http://code.google.com/p/tf-net/");
        }

        private void ziplibLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenUrl("http://sharpdevelop.net/OpenSource/SharpZipLib/Default.aspx");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenUrl("http://colorbrewer.org/");
        }
	}
}
