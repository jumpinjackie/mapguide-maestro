#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    [ToolboxItem(false)]
    internal partial class ConnectionStringCtrl : EditorBase, IOdbcSubEditor
    {
        public ConnectionStringCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
        }

        private void OnConnectionChanged()
        {
            var handler = this.ConnectionChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public NameValueCollection ConnectionProperties
        {
            get
            {
                var values = new NameValueCollection();
                values["ConnectionString"] = txtConnStr.Text; //NOXLATE
                return values;
            }
            set
            {
                txtConnStr.Text = value["ConnectionString"]; //NOXLATE
            }
        }

        public event EventHandler ConnectionChanged;

        public Control Content
        {
            get { return this; }
        }

        public NameValueCollection Get64BitConnectionProperties()
        {
            return this.ConnectionProperties;
        }

        public event EventHandler RequestDocumentReset;

        private void lnkApplyCredentials_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string connStr = txtConnStr.Text;
            if (connStr.Contains(StringConstants.MgUsernamePlaceholder) &&
                connStr.Contains(StringConstants.MgPasswordPlaceholder))
            {
                using (var diag = new SetCredentialsDialog(StringConstants.MgUsernamePlaceholder, StringConstants.MgPasswordPlaceholder))
                {
                    if (diag.ShowDialog() == DialogResult.OK)
                    {
                        var fs = (IFeatureSource)_service.GetEditedResource();
                        fs.SetEncryptedCredentials(_service.CurrentConnection, diag.Username, diag.Password);
                        //Bit of a hack as parent does a 64-bit driver check to determine
                        //which NameValueCollection to return from the child, but we can
                        //get away with this because the same NameValueCollection is returned
                        //for both
                        fs.ApplyConnectionProperties(this.ConnectionProperties);
                        _service.SyncSessionCopy();
                        MessageBox.Show("Credentials applied");
                    }
                }
            }
            else
            {
                MessageBox.Show(Strings.OdbcConnStrMissingMgPlaceholders);
            }
        }
    }
}