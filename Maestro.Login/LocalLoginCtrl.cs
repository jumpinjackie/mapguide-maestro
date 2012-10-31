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
    /// <summary>
    /// A user control for entering connection information for a local connection
    /// </summary>
    public partial class LocalLoginCtrl : UserControl, ILoginCtrl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalLoginCtrl"/> class.
        /// </summary>
        public LocalLoginCtrl()
        {
            InitializeComponent();
        }

        internal static string LastIniPath { get; set; }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                open.InitialDirectory = Application.StartupPath;
                open.RestoreDirectory = true;
                open.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickIni, "ini"); //NOXLATE
                if (open.ShowDialog() == DialogResult.OK)
                {
                    LastIniPath = txtPlatformConfig.Text = open.FileName;
                    UpdateLoginStatus();
                }
            }
        }

        /// <summary>
        /// Gets the platform config path.
        /// </summary>
        public string PlatformConfigPath
        {
            get { return txtPlatformConfig.Text; }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Updates the login status.
        /// </summary>
        public void UpdateLoginStatus()
        {
            if (this.PlatformConfigPath.Trim().Length > 0 && File.Exists(this.PlatformConfigPath))
                EnableOk(this, EventArgs.Empty);
            else
                DisabledOk(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when [enable ok].
        /// </summary>
        public event EventHandler EnableOk;

        /// <summary>
        /// Occurs when [disabled ok].
        /// </summary>
        public event EventHandler DisabledOk;

        /// <summary>
        /// Occurs when [check saved password].
        /// </summary>
        public event EventHandler CheckSavedPassword;
    }
}
