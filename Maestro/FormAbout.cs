#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
		private  Globalizator.Globalizator m_globalizor = null;

		public FormAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new  Globalizator.Globalizator(this);
            this.Icon = FormMain.MaestroIcon;
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
            this.tfnetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.ziplibLinkLabel = new System.Windows.Forms.LinkLabel();
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
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(368, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel
            // 
            this.linkLabel.Location = new System.Drawing.Point(16, 240);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(328, 16);
            this.linkLabel.TabIndex = 1;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "http://trac.osgeo.org/mapguide/wiki/maestro";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "MapGuide Maestro";
            // 
            // License
            // 
            this.License.Dock = System.Windows.Forms.DockStyle.Fill;
            this.License.Location = new System.Drawing.Point(0, 0);
            this.License.Multiline = true;
            this.License.Name = "License";
            this.License.ReadOnly = true;
            this.License.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.License.Size = new System.Drawing.Size(320, 142);
            this.License.TabIndex = 3;
            this.License.Text = resources.GetString("License.Text");
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(16, 32);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(328, 16);
            this.Version.TabIndex = 4;
            this.Version.Text = "label2";
            // 
            // PayPalImage
            // 
            this.PayPalImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PayPalImage.Image = ((System.Drawing.Image)(resources.GetObject("PayPalImage.Image")));
            this.PayPalImage.Location = new System.Drawing.Point(352, 8);
            this.PayPalImage.Name = "PayPalImage";
            this.PayPalImage.Size = new System.Drawing.Size(72, 24);
            this.PayPalImage.TabIndex = 5;
            this.PayPalImage.TabStop = false;
            this.ToolTip.SetToolTip(this.PayPalImage, "Click here to open the donation page");
            this.PayPalImage.Click += new System.EventHandler(this.PayPalImage_Click);
            // 
            // Localization
            // 
            this.Localization.Location = new System.Drawing.Point(16, 48);
            this.Localization.Name = "Localization";
            this.Localization.Size = new System.Drawing.Size(328, 16);
            this.Localization.TabIndex = 6;
            this.Localization.Text = "label2";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.licenseTab);
            this.tabControl.Controls.Add(this.creditsTab);
            this.tabControl.Controls.Add(this.thirdPartyTab);
            this.tabControl.Location = new System.Drawing.Point(16, 64);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(328, 168);
            this.tabControl.TabIndex = 7;
            // 
            // licenseTab
            // 
            this.licenseTab.Controls.Add(this.License);
            this.licenseTab.Location = new System.Drawing.Point(4, 22);
            this.licenseTab.Name = "licenseTab";
            this.licenseTab.Size = new System.Drawing.Size(320, 142);
            this.licenseTab.TabIndex = 0;
            this.licenseTab.Text = "License";
            this.licenseTab.UseVisualStyleBackColor = true;
            // 
            // creditsTab
            // 
            this.creditsTab.Controls.Add(this.Credits);
            this.creditsTab.Location = new System.Drawing.Point(4, 22);
            this.creditsTab.Name = "creditsTab";
            this.creditsTab.Size = new System.Drawing.Size(320, 142);
            this.creditsTab.TabIndex = 1;
            this.creditsTab.Text = "Credits";
            this.creditsTab.UseVisualStyleBackColor = true;
            // 
            // Credits
            // 
            this.Credits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Credits.Location = new System.Drawing.Point(0, 0);
            this.Credits.Multiline = true;
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            this.Credits.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Credits.Size = new System.Drawing.Size(320, 142);
            this.Credits.TabIndex = 4;
            this.Credits.Text = resources.GetString("Credits.Text");
            // 
            // thirdPartyTab
            // 
            this.thirdPartyTab.Controls.Add(this.ziplibLinkLabel);
            this.thirdPartyTab.Controls.Add(this.tfnetLinkLabel);
            this.thirdPartyTab.Controls.Add(this.label2);
            this.thirdPartyTab.Location = new System.Drawing.Point(4, 22);
            this.thirdPartyTab.Name = "thirdPartyTab";
            this.thirdPartyTab.Size = new System.Drawing.Size(320, 142);
            this.thirdPartyTab.TabIndex = 2;
            this.thirdPartyTab.Text = "Thirdparty";
            this.thirdPartyTab.UseVisualStyleBackColor = true;
            // 
            // tfnetLinkLabel
            // 
            this.tfnetLinkLabel.AutoSize = true;
            this.tfnetLinkLabel.Location = new System.Drawing.Point(8, 32);
            this.tfnetLinkLabel.Name = "tfnetLinkLabel";
            this.tfnetLinkLabel.Size = new System.Drawing.Size(181, 13);
            this.tfnetLinkLabel.TabIndex = 1;
            this.tfnetLinkLabel.TabStop = true;
            this.tfnetLinkLabel.Text = "Topology Framework .NET (TF.NET)";
            this.tfnetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.tfnetLinkLabel_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "This program uses the following thirdparty libraries:";
            // 
            // ziplibLinkLabel
            // 
            this.ziplibLinkLabel.AutoSize = true;
            this.ziplibLinkLabel.Location = new System.Drawing.Point(8, 56);
            this.ziplibLinkLabel.Name = "ziplibLinkLabel";
            this.ziplibLinkLabel.Size = new System.Drawing.Size(80, 13);
            this.ziplibLinkLabel.TabIndex = 2;
            this.ziplibLinkLabel.TabStop = true;
            this.ziplibLinkLabel.Text = "ICSharp Zip Lib";
            this.ziplibLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ziplibLinkLabel_LinkClicked);
            // 
            // FormAbout
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 269);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About...";
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

		private void FormAbout_Load(object sender, System.EventArgs e)
		{
			Version.Text = string.Format(m_globalizor.Translate("Version: {0}"), Application.ProductVersion);
			Localization.Text = string.Format(m_globalizor.Translate("Selected language: {0}, OS Language: {1}"),  Globalizator.Globalizator.CurrentCulture.Name, System.Globalization.CultureInfo.CurrentUICulture.Name);
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
	}
}
