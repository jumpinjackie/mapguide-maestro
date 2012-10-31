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
using System.IO;
using Maestro.Shared.UI;

namespace Maestro.Login
{
    internal partial class LocalNativeLoginCtrl : UserControl, ILoginCtrl
    {
        public LocalNativeLoginCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DisabledOk(this, EventArgs.Empty);
        }

        internal static string LastIniPath { get; set; }

        #region ILoginCtrl Members

        public string Username
        {
            get { return txtUsername.Text; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
        }

        #endregion

        public string WebConfigPath
        {
            get { return txtWebConfig.Text; }
        }

        public event EventHandler EnableOk = delegate { };

        public event EventHandler DisabledOk = delegate { };

        public event EventHandler CheckSavedPassword = delegate { };

        private void txtWebConfig_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginStatus();
        }

        public void UpdateLoginStatus()
        {
            if (this.WebConfigPath.Trim().Length > 0 && File.Exists(this.WebConfigPath))
                EnableOk(this, EventArgs.Empty);
            else
                DisabledOk(this, EventArgs.Empty);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.InitialDirectory = Application.StartupPath;
                //WTF does this default to false??? Does MS not realize that changing directories 
                //via this dialog absolutely screws up file/assembly loading from relative paths?
                dlg.RestoreDirectory = true;
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickIni, "ini"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LastIniPath = txtWebConfig.Text = dlg.FileName;
                }
            }
        }
    }
}
