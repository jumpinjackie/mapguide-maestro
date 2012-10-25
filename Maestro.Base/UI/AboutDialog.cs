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
    public partial class AboutDialog : System.Windows.Forms.Form
    {
        private IServerConnection m_connection;

        private AboutDialog()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.Icon = Properties.Resources.MapGuide_Maestro;
        }

        internal AboutDialog(IServerConnection connection)
            : this()
        {
            m_connection = connection;
        }

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

            Credits.Text = Strings.Contributors;
            Version.Text = string.Format(Strings.About_VersionLabel, Application.ProductVersion);
            Localization.Text = string.Format(Strings.About_LanguageLabel, System.Threading.Thread.CurrentThread.CurrentUICulture, tmp.CurrentUICulture);

            string version = Strings.VersionUnknownOrNotConnected;
            string match = Strings.VersionUnknown;
            if (m_connection != null)
            {
                version = m_connection.SiteVersion.ToString();
                for (int i = 0; i < SiteVersions.SiteVersionNumbers.Length; i++)
                    if (m_connection.SiteVersion == SiteVersions.SiteVersionNumbers[i])
                        match = ((KnownSiteVersions)i).ToString();
            }

            ServerVersion.Text = string.Format(Strings.About_ServerVersionLabel, version, match);
        }

        private void PayPalImage_Click(object sender, System.EventArgs e)
        {
            _launcher.OpenUrl("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=jumpinjackie%40gmail%2ecom&lc=AU&item_name=MapGuide%20Maestro%20Project&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted"); //NOXLATE
        }

        private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl(linkLabel.Text);
        }

        private void tfnetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://code.google.com/p/nettopologysuite/"); //NOXLATE
        }

        private void ziplibLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://sharpdevelop.net/OpenSource/SharpZipLib/Default.aspx"); //NOXLATE
        }

        private void lnkColorBrewer_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://colorbrewer.org/"); //NOXLATE
        }

        private void lnkSharpDevelop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://sharpdevelop.net/OpenSource/SD/Default.aspx"); //NOXLATE
        }

        private void lnkIcons_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://p.yusukekamiyamane.com/"); //NOXLATE
        }

        private void lnkAdvTreeView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("http://sourceforge.net/projects/treeviewadv/"); //NOXLATE
        }

        private void lnkDockPanel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _launcher.OpenUrl("https://github.com/lextm/sharpsnmplib/tree/master/WinFormsUI"); //NOXLATE
        }
    }
}
