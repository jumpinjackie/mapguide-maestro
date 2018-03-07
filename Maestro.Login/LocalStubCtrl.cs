#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using System;
using System.Windows.Forms;

namespace Maestro.Login
{
    internal partial class LocalStubCtrl : UserControl, ILoginCtrl
    {
        public LocalStubCtrl()
        {
            InitializeComponent();
        }

        public void SetLastIniPath(string path)
        {
            lblIniPath.Text = path;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public void UpdateLoginStatus()
        {
            EnableOk?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler EnableOk;

        public event EventHandler DisabledOk;

        public event EventHandler CheckSavedPassword;
    }
}