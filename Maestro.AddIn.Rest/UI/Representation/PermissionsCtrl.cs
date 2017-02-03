#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using Maestro.AddIn.Rest.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.AddIn.Rest.UI.Representation
{
    internal partial class PermissionsCtrl : UserControl
    {
        public PermissionsCtrl()
        {
            InitializeComponent();
        }

        public void Init(RestSourceContext context) => lstAllowGroups.DataSource = context.GetGroups();

        public void UpdateConfiguration(dynamic methodConfig)
        {
            if (chkAllowAnonymous.Checked)
                methodConfig.AllowAnonymous = true;

            if (chkAllowGroups.Checked)
                methodConfig.AllowGroups = new List<string>(lstAllowGroups.SelectedItems.Cast<object>().Select(o => o.ToString()));

            if (chkAllowRoles.Checked)
                methodConfig.AllowRoles = new List<string>(txtRoles.Lines);
        }
    }
}
