#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Login
{
    internal partial class HttpLoginCtrl : UserControl, ILoginCtrl
    {
        private bool _loading = true;

        public HttpLoginCtrl()
        {
            InitializeComponent();
            cmbServerUrl.Text = DefaultValues.Server;
            txtUsername.Text = DefaultValues.Username;
            txtStartingpoint.Text = DefaultValues.StartingPoint;
            txtPassword.Text = DefaultValues.Password;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            List<CultureInfo> supported = new List<CultureInfo>();
            supported.Add(CultureInfo.GetCultureInfo("en-US")); //NOXLATE

            //Probe for language bundles and add them as well
            System.Text.RegularExpressions.Regex cix = new System.Text.RegularExpressions.Regex("[A-z][A-z](\\-[A-z][A-z])?"); //NOXLATE
            foreach (string f in System.IO.Directory.GetDirectories(Application.StartupPath))
            {
                if (cix.Match(System.IO.Path.GetFileName(f)).Length == System.IO.Path.GetFileName(f).Length)
                {
                    try
                    {
                        supported.Add(System.Globalization.CultureInfo.GetCultureInfo(System.IO.Path.GetFileName(f)));
                    }
                    catch { }
                }
            }

            cmbLanguage.DisplayMember = "DisplayName"; //NOXLATE
            cmbLanguage.ValueMember = "Name"; //NOXLATE

            //Set default language based on current thread culture
            int selected = -1;
            foreach (var ci in supported)
            {
                int index = cmbLanguage.Items.Add(ci);
                if (string.Compare(ci.Name, System.Threading.Thread.CurrentThread.CurrentUICulture.Name, true) == 0)
                {
                    selected = index;
                }
            }

            if (selected >= 0)
                cmbLanguage.SelectedIndex = selected;

            _loading = false;
            UpdateLoginStatus();
        }

        public PreferedSite[] GetSites()
        {
            List<PreferedSite> items = new List<PreferedSite>();
            foreach (PreferedSite s in cmbServerUrl.Items)
            {
                items.Add(s);
            }
            return items.ToArray();
        }

        public void AddSites(PreferedSite[] sites)
        {
            cmbServerUrl.Items.AddRange(sites);
        }

        public void SetPreferredSite(int index)
        {
            cmbServerUrl.SelectedIndex = index;
        }

        #region ILoginCtrl Members

        public string Username
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        #endregion

        public string Server
        {
            get { return cmbServerUrl.Text; }
            set { cmbServerUrl.Text = value; }
        }

        public string StartingPoint
        {
            get { return txtStartingpoint.Text; }
            set { txtStartingpoint.Text = value; }
        }

        public string Language
        {
            get 
            { 
                var ci = cmbLanguage.SelectedItem as CultureInfo;
                if (ci != null)
                {
                    return ci.TwoLetterISOLanguageName;
                }
                return null;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginStatus();
        }

        public PreferredSiteList SiteList
        {
            get;
            set;
        }

        public event EventHandler EnableOk = delegate { };

        public event EventHandler DisabledOk = delegate { };

        public event EventHandler CheckSavedPassword = delegate { };

        public CultureInfo SelectedCulture
        {
            get { return cmbLanguage.SelectedItem as CultureInfo; }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                if (cmbLanguage.SelectedIndex >= 0)
                {
                    //System.Threading.Thread.CurrentThread.CurrentCulture = m_supportedLanguages[cmbLanguage.SelectedIndex];
                    //System.Threading.Thread.CurrentThread.CurrentUICulture = m_supportedLanguages[cmbLanguage.SelectedIndex];

                    if (this.Visible)
                    {
                        try
                        {
                            var ci = cmbLanguage.SelectedItem as CultureInfo;
                            if (ci != null)
                            {
                                this.SiteList.GUILanguage = ci.Name;
                                this.SiteList.Save();
                            }
                        }
                        catch { }

                        MessageBox.Show(this, Strings.RestartForLanguageChange, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void cmbServerUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreferedSite site = cmbServerUrl.SelectedItem as PreferedSite;
            if (site == null)
                return;

            txtStartingpoint.Text = site.StartingPoint;
            txtUsername.Text = site.Username;
            if (site.SavePassword)
                txtPassword.Text = site.UnscrambledPassword;
            else
                txtPassword.Text = string.Empty;

            CheckSavedPassword(this, EventArgs.Empty);
        }

        public void UpdateLoginStatus()
        {
            if (this.Username.Trim().Length > 0 && this.Server.Trim().Length > 0)
                EnableOk(this, EventArgs.Empty);
            else
                DisabledOk(this, EventArgs.Empty);
        }
    }

    static class DefaultValues
    {
        public const string Server = "http://localhost/mapguide/mapagent/mapagent.fcgi"; //NOXLATE
        public const string StartingPoint = StringConstants.RootIdentifier;
        public const string Username = "Administrator"; //NOXLATE
        public const string Password = "admin"; //NOXLATE
    }
}
