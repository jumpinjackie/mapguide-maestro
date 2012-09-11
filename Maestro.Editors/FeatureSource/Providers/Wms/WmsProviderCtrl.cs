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
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Shared.UI;

namespace Maestro.Editors.FeatureSource.Providers.Wms
{
    [ToolboxItem(false)]
    internal partial class WmsProviderCtrl : EditorBindableCollapsiblePanel
    {
        public WmsProviderCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private IFeatureSource _fs;
        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            _init = true;
            try
            {
                _service = service;
                _service.RegisterCustomNotifier(this);
                _fs = (IFeatureSource)_service.GetEditedResource();

                txtFeatureServer.Text = _fs.GetConnectionProperty("FeatureServer"); //NOXLATE
                txtUsername.Text = _fs.GetConnectionProperty("Username"); //NOXLATE
                txtPassword.Text = _fs.GetConnectionProperty("Password"); //NOXLATE
            }
            finally
            {
                _init = false;
            }
        }

        private void txtFeatureServer_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            _fs.SetConnectionProperty("FeatureServer", txtFeatureServer.Text); //NOXLATE
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            if (string.IsNullOrEmpty(txtUsername.Text))
                _fs.SetConnectionProperty("Username", null); //NOXLATE
            else
                _fs.SetConnectionProperty("Username", txtUsername.Text); //NOXLATE
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;
            if (string.IsNullOrEmpty(txtPassword.Text))
                _fs.SetConnectionProperty("Password", null); //NOXLATE
            else
                _fs.SetConnectionProperty("Password", txtPassword.Text); //NOXLATE
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtStatus.Text = string.Empty;
            using (new WaitCursor(this))
            {
                WriteEncryptedCredentials();
                txtStatus.Text = string.Format(Strings.FdoConnectionStatus, _fs.TestConnection());
            }
        }

        private void WriteEncryptedCredentials()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (username != StringConstants.MgUsernamePlaceholder && password != StringConstants.MgPasswordPlaceholder)
                {
                    _fs.SetConnectionProperty("Username", StringConstants.MgUsernamePlaceholder); //NOXLATE
                    _fs.SetConnectionProperty("Password", StringConstants.MgPasswordPlaceholder); //NOXLATE
                    _fs.SetEncryptedCredentials(username, password);
                    _service.SyncSessionCopy();
                }
            }
            else
            {
                _fs.SetConnectionProperty("Username", null); //NOXLATE
                _fs.SetConnectionProperty("Password", null); //NOXLATE
                try
                {
                    _fs.DeleteResourceData(StringConstants.MgUserCredentialsResourceData);
                }
                catch { }
                _service.SyncSessionCopy();
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            WriteEncryptedCredentials();
            var diag = new WmsAdvancedConfigurationDialog(_service);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                _fs.SetConfigurationContent(diag.Document.ToXml());
                OnResourceChanged();
            }
        }
    }
}
