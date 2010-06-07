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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro
{
    public partial class FormLogin : Form
    {
        private int _selectedIndex;
        private ILoginCtrl[] _controls;
        private ServerConnectionI _conn;

        private PreferedSiteList _siteList;

        private HttpLoginCtrl _http;
        private LocalNativeLoginCtrl _local;

        public FormLogin()
        {
            InitializeComponent();
            _http = new HttpLoginCtrl() { Dock = DockStyle.Fill };
            _local = new LocalNativeLoginCtrl() { Dock = DockStyle.Fill };
            _controls = new ILoginCtrl[] 
            {
                _http,
                _local
            };
            _controls[0].EnableOk += OnEnableOk;
            _controls[1].EnableOk += OnEnableOk;
            _controls[0].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[1].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[0].DisabledOk += OnDisableOk;
            _controls[1].DisabledOk += OnDisableOk;
        }

        public string Username
        {
            get { return _controls[_selectedIndex].Username; }
        }

        public string Password
        {
            get { return _controls[_selectedIndex].Password; }
        }

        protected override void OnLoad(EventArgs e)
        {
            _siteList = PreferedSiteList.Load();

            if (_siteList.Sites.Length == 0)
            {
                _http.Server = "http://localhost/mapguide/mapagent/mapagent.fcgi";
                _http.StartingPoint = "Library://";
                _http.Username = "Administrator";
                _http.Password = "admin";
                chkSavePassword.Checked = true;
                //chkAutoConnect.Checked = false;
            }
            else
            {
                _http.AddSites(_siteList.Sites);
                //In case the site was removed...
                try { _http.SetPreferredSite(_siteList.PreferedSite); }
                catch { }
                //chkAutoConnect.Checked = _siteList.AutoConnect;
            }

            _http.SiteList = _siteList;

            base.OnLoad(e);
            rdHttp.Checked = true;

            //Mono = No LocalNativeConnection for you (for now...)
            if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
            {
                rdTcpIp.Enabled = false;
            }
        }

        private void OnEnableOk(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

        private void OnDisableOk(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
        }

        public ServerConnectionI Connection
        {
            get { return _conn; }
        }

        public void SetLoginControl(Control c)
        {
            loginPanel.Controls.Clear();
            loginPanel.Controls.Add(c);
        }

        private void UpdateLoginControl()
        {
            if (rdHttp.Checked)
                _selectedIndex = 0;
            else
                _selectedIndex = 1;
            
            SetLoginControl((Control)_controls[_selectedIndex]);
            _controls[_selectedIndex].UpdateLoginStatus();
        }

        private void rdHttp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLoginControl();
        }

        private void rdTcpIp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLoginControl();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            using (new WaitCursor(this))
            {
                try
				{
                    PreferedSite ps = null;

                    if (_selectedIndex == 0) //HTTP
                    {
                        _conn = ConnectionFactory.CreateHttpConnection(new Uri(_http.Server), _http.Username, _http.Password, _http.Locale, true);
                        (_conn as HttpServerConnection).UserAgent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                        //Update preferred site entry if it exists
                        int index = 0;
                        foreach (PreferedSite s in _http.GetSites())
                        {
                            if (s.SiteURL == _http.Server)
                            {
                                ps = s;
                                break;
                            }
                            else
                                index++;
                        }

                        if (ps == null)
                            ps = new PreferedSite();

                        if (ps.ApprovedVersion == null)
                            ps.ApprovedVersion = new Version(0, 0, 0, 0);

                        if (_conn.SiteVersion > _conn.MaxTestedVersion && _conn.SiteVersion > ps.ApprovedVersion)
                        {
                            if (MessageBox.Show(this, Strings.FormLogin.UntestedServerVersion, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                                return;
                        }

                        try
                        {
                            ps.SiteURL = _http.Server;
                            ps.StartingPoint = _http.StartingPoint;
                            ps.Username = _http.Username;
                            ps.SavePassword = chkSavePassword.Checked;
                            ps.ApprovedVersion = ps.ApprovedVersion > _conn.SiteVersion ? ps.ApprovedVersion : _conn.SiteVersion;
                            if (ps.SavePassword)
                                ps.UnscrambledPassword = _http.Password;
                            else
                                ps.ScrambledPassword = "";

                            if (index >= _siteList.Sites.Length)
                                _siteList.AddSite(ps);

                            //_siteList.AutoConnect = chkAutoConnect.Checked;
                            _siteList.PreferedSite = index;
                            _siteList.GUILanguage = _http.Language;

                            _siteList.Save();
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }
                    }
                    else //Native
                    {
                        _conn = ConnectionFactory.CreateLocalNativeConnection(_local.WebConfigPath, _local.Username, _local.Password, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    }

                    _conn.AutoRestartSession = true;

					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				catch (Exception ex)
				{
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Strings.FormLogin.ConnectionFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
            }
        }
    }
}
