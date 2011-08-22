#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using Maestro.Shared.UI;
using System.IO;

namespace Maestro.Login
{
    public partial class LocalLoginCtrl : UserControl, ILoginCtrl
    {
        public LocalLoginCtrl()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                open.RestoreDirectory = true; 
                open.Filter = "*.ini|*.ini";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtPlatformConfig.Text = open.FileName;
                    UpdateLoginStatus();
                }
            }
        }

        public string PlatformConfigPath
        {
            get { return txtPlatformConfig.Text; }
        }

        public string Username
        {
            get { return string.Empty; }
        }

        public string Password
        {
            get { return string.Empty; }
        }

        public void UpdateLoginStatus()
        {
            if (this.PlatformConfigPath.Trim().Length > 0 && File.Exists(this.PlatformConfigPath))
                EnableOk(this, EventArgs.Empty);
            else
                DisabledOk(this, EventArgs.Empty);
        }

        public event EventHandler EnableOk;

        public event EventHandler DisabledOk;

        public event EventHandler CheckSavedPassword;
    }
}
