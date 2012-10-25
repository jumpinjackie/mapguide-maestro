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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Services;
using System.Collections;
using Maestro.Editors.Generic;

namespace Maestro.Base.UI
{
    internal partial class OpenResourceIdDialog : Form
    {
        private OpenResourceIdDialog()
        {
            InitializeComponent();
        }

        public OpenResourceIdDialog(ServerConnectionManager connMgr)
            : this()
        {
            var items = new List<KeyValuePair<string, IServerConnection>>();
            foreach (var name in connMgr.GetConnectionNames())
            {
                items.Add(new KeyValuePair<string, IServerConnection>(name, connMgr.GetConnection(name)));
            }
            cmbConnection.DisplayMember = "Key"; //NOXLATE
            cmbConnection.DataSource = items;
            cmbConnection.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.ActiveControl = txtResourceId;
        }

        public IServerConnection SelectedConnection
        {
            get
            {
                return ((KeyValuePair<string, IServerConnection>)cmbConnection.SelectedItem).Value;
            }
        }

        public string SelectedResourceId
        {
            get { return txtResourceId.Text; }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            if (ResourceIdentifier.Validate(txtResourceId.Text) && !ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                lblMessage.Text = Strings.InvalidResourceId;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(SelectedConnection.ResourceService, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }
    }
}
