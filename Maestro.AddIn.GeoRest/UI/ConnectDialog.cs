#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.IO;

namespace Maestro.AddIn.GeoRest.UI
{
    public partial class ConnectDialog : Form
    {
        public ConnectDialog()
        {
            InitializeComponent();
        }

        public string ConfigurationRoot { get { return txtConfigRoot.Text; } }

        public string GeoRestUrl { get { return txtGeoRestUrl.Text; } }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.ShowNewFolderButton = true;
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    txtConfigRoot.Text = browser.SelectedPath;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtConfigRoot.Text))
                MessageBox.Show(Properties.Resources.ErrConfigRootDoesNotExist);
            if (string.IsNullOrEmpty(txtGeoRestUrl.Text))
                MessageBox.Show(Properties.Resources.ErrGeoRestUrlNotSpecified);

            this.DialogResult = DialogResult.OK;
        }
    }
}
