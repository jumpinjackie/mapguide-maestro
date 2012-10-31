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
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using System.Reflection;
using System.Collections.Specialized;

namespace Maestro.Login
{
    /// <summary>
    /// A dialog to prompt for <see cref="OSGeo.MapGuide.MaestroAPI.IServerConnection"/> information
    /// </summary>
    public partial class LoginDialog : Form
    {
        private int _selectedIndex;
        private ILoginCtrl[] _controls;
        private IServerConnection _conn;

        private PreferredSiteList _siteList;

        private HttpLoginCtrl _http;
        private LocalNativeLoginCtrl _localNative;
        private LocalLoginCtrl _local;
        private LocalNativeStubCtrl _localNativeStub;
        private LocalStubCtrl _localStub;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDialog"/> class.
        /// </summary>
        public LoginDialog()
        {
            InitializeComponent();
            _http = new HttpLoginCtrl() { Dock = DockStyle.Fill };
            _localNative = new LocalNativeLoginCtrl() { Dock = DockStyle.Fill };
            _local = new LocalLoginCtrl() { Dock = DockStyle.Fill };
            _localNativeStub = new LocalNativeStubCtrl() { Dock = DockStyle.Fill };
            _localStub = new LocalStubCtrl() { Dock = DockStyle.Fill };
            _controls = new ILoginCtrl[] 
            {
                _http,
                _localNative,
                _local,
                _localNativeStub,
                _localStub
            };
            _controls[0].EnableOk += OnEnableOk;
            _controls[1].EnableOk += OnEnableOk;
            _controls[2].EnableOk += OnEnableOk;
            _controls[3].EnableOk += OnEnableOk;
            _controls[4].EnableOk += OnEnableOk;
            _controls[0].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[1].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[2].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[3].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[4].CheckSavedPassword += (sender, e) => { chkSavePassword.Checked = true; };
            _controls[0].DisabledOk += OnDisableOk;
            _controls[1].DisabledOk += OnDisableOk;
            _controls[2].DisabledOk += OnDisableOk;
            _controls[3].DisabledOk += OnDisableOk;
            _controls[4].DisabledOk += OnDisableOk;
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return _controls[_selectedIndex].Username; }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _controls[_selectedIndex].Password; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            _siteList = PreferredSiteList.Load();

            if (_siteList.Sites.Length == 0)
            {
                _http.Server = DefaultValues.Server;
                _http.StartingPoint = DefaultValues.StartingPoint;
                _http.Username = DefaultValues.Username;
                _http.Password = DefaultValues.Password;
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

            var isNativeApiAvailable = false;
            var isLocalApiAvailable = false;
            var providers = ConnectionProviderRegistry.GetProviders();
            foreach (var prv in providers)
            {
                if (prv.Name.ToUpper().Equals("MAESTRO.LOCALNATIVE")) //NOXLATE
                {
                    isNativeApiAvailable = true;
                    break;
                }
                else if (prv.Name.ToUpper().Equals("MAESTRO.LOCAL")) //NOXLATE
                {
                    isLocalApiAvailable = true;
                    break;
                }
            }
            //Mono = No LocalNativeConnection for you (for now...)
            if (Platform.IsRunningOnMono || !isNativeApiAvailable)
            {
                rdTcpIp.Enabled = false;
            }

            if (Platform.IsRunningOnMono || !isLocalApiAvailable)
            {
                rdLocal.Enabled = false;
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

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public IServerConnection Connection
        {
            get { return _conn; }
        }

        /// <summary>
        /// Sets the login control.
        /// </summary>
        /// <param name="c">The c.</param>
        public void SetLoginControl(Control c)
        {
            loginPanel.Controls.Clear();
            loginPanel.Controls.Add(c);
        }

        private void rdHttp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLoginControl();
        }

        private void rdTcpIp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLoginControl();
        }

        private void rdLocal_CheckedChanged(object sender, EventArgs e)
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
                        var builder = new System.Data.Common.DbConnectionStringBuilder();
                        builder["Url"] = _http.Server; //NOXLATE
                        builder["Username"] = _http.Username; //NOXLATE
                        builder["Password"] = _http.Password; //NOXLATE
                        builder["Locale"] = _http.Language; //NOXLATE
                        builder["AllowUntestedVersion"] = true; //NOXLATE

                        string agent = "MapGuide Maestro v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); //NOXLATE

                        _conn = ConnectionProviderRegistry.CreateConnection("Maestro.Http", builder.ToString()); //NOXLATE
                        _conn.SetCustomProperty("UserAgent", agent); //NOXLATE

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
                            if (MessageBox.Show(this, Strings.UntestedServerVersion, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
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
                                ps.ScrambledPassword = string.Empty;

                            if (index >= _siteList.Sites.Length)
                                _siteList.AddSite(ps);

                            //_siteList.AutoConnect = chkAutoConnect.Checked;
                            _siteList.PreferedSite = index;
                            var ci = _http.SelectedCulture;
                            if (ci != null)
                            {
                                _siteList.GUILanguage = ci.Name;
                            }
                            _siteList.Save();
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (_selectedIndex == 1) //Native
                    {
                        System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
                        builder["ConfigFile"] = LocalNativeLoginCtrl.LastIniPath; //NOXLATE
                        builder["Username"] = _localNative.Username; //NOXLATE
                        builder["Password"] = _localNative.Password; //NOXLATE
                        builder["Locale"] = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName; //NOXLATE
                        _conn = ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative", builder.ToString()); //NOXLATE
                    }
                    else //Local
                    {
                        NameValueCollection param = new NameValueCollection();
                        param["ConfigFile"] = LocalLoginCtrl.LastIniPath; //NOXLATE
                        _conn = ConnectionProviderRegistry.CreateConnection("Maestro.Local", param); //NOXLATE
                    }

                    _conn.AutoRestartSession = true;



                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
                catch (TargetInvocationException ex) 
                {
                    //We don't care about the outer exception
                    string msg = ex.InnerException.Message;
                    MessageBox.Show(this, string.Format(Strings.ConnectionFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Strings.ConnectionFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateLoginControl()
        {
            if (rdHttp.Checked)
            {
                _selectedIndex = 0;
                SetLoginControl((Control)_controls[_selectedIndex]);
                _controls[_selectedIndex].UpdateLoginStatus();
            }
            else if (rdTcpIp.Checked)
            {
                if (ConnectionProviderRegistry.GetInvocationCount("Maestro.LocalNative") == 0)
                {
                    _selectedIndex = 1;
                }
                else
                {
                    _selectedIndex = 3;
                    _localNativeStub.SetLastIniPath(LocalNativeLoginCtrl.LastIniPath);
                }
                SetLoginControl((Control)_controls[_selectedIndex]);
                _controls[_selectedIndex].UpdateLoginStatus();
            }
            else
            {
                _selectedIndex = 2;
                if (ConnectionProviderRegistry.GetInvocationCount("Maestro.Local") == 0)
                {
                    _selectedIndex = 2;
                }
                else
                {
                    _selectedIndex = 4;
                    _localStub.SetLastIniPath(LocalLoginCtrl.LastIniPath);
                }
                SetLoginControl((Control)_controls[_selectedIndex]);
                _controls[_selectedIndex].UpdateLoginStatus();
            }
        }
    }
}
