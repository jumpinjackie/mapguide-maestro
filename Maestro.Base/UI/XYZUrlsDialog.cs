#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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


using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    public partial class XYZUrlsDialog : Form
    {
        class XYZUrl
        {
            public string Name { get; set; }

            public string UrlTemplate { get; set; }
        }

        public XYZUrlsDialog(Dictionary<string, string> urls)
            : this()
        {
            var dataSource = urls.Select(kvp => new XYZUrl { Name = kvp.Key, UrlTemplate = kvp.Value }).ToList();
            dgUrls.DataSource = dataSource;
        }

        private XYZUrlsDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
