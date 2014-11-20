#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Maestro.Login
{
    public partial class RestLoginCtrl : UserControl, ILoginCtrl
    {
        private bool _loading = true;

        public RestLoginCtrl()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return txtUsername.Text; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
        }

        public string Endpoint
        {
            get { return txtEndpoint.Text; }
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

        public CultureInfo SelectedCulture
        {
            get { return cmbLanguage.SelectedItem as CultureInfo; }
        }

        public void UpdateLoginStatus()
        {
            if (this.Username.Trim().Length > 0 && this.Endpoint.Trim().Length > 0)
                EnableOk(this, EventArgs.Empty);
            else
                DisabledOk(this, EventArgs.Empty);
        }

        public event EventHandler EnableOk;

        public event EventHandler DisabledOk;

        public event EventHandler CheckSavedPassword;

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginStatus();
        }

        private void txtEndpoint_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginStatus();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginStatus();
        }
    }
}
