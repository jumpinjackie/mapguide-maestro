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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors.FeatureSource.Providers.Odbc;

namespace Maestro.Editors.FeatureSource.Providers.Rdbms
{
    [ToolboxItem(false)]
    internal partial class RdbmsBaseCtrl : EditorBindableCollapsiblePanel
    {
        public RdbmsBaseCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.HeaderText = this.Title;
        }

        protected override void UnsubscribeEventHandlers()
        {
            _service.BeforeSave -= OnBeforeSave;
            _service.BeforePreview -= OnBeforePreview;
        }

        private IEditorService _service;
        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.BeforeSave += OnBeforeSave;
            _service.BeforePreview += OnBeforePreview;
            _service.RegisterCustomNotifier(this);
            _fs = _service.GetEditedResource() as IFeatureSource;

            //Set the field values
            txtService.Text = _fs.GetConnectionProperty("Service");

            //We're gonna follow MG Studio behaviour here which is: Never load the password
            //and auto trigger dirty state.
            if (!_service.IsNew)
            {
                txtUsername.Text = _fs.GetEncryptedUsername() ?? _fs.GetConnectionProperty("Username");
                //txtPassword.Text = _fs.GetConnectionProperty("Password");
                OnResourceChanged();
            }

            //Set initial value of data store if possible
            var dstore = _fs.GetConnectionProperty("DataStore");
            txtDataStore.Text = dstore;

            //As our connection properties are not CLR properties, 
            //"manually" bind these fields
            txtService.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("Service", txtService.Text);
            };

            txtUsername.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                    _fs.SetConnectionProperty("Username", null);
                else
                    _fs.SetConnectionProperty("Username", txtUsername.Text);
            };
            
            txtPassword.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                    _fs.SetConnectionProperty("Password", null);
                else
                    _fs.SetConnectionProperty("Password", txtPassword.Text);
            };

            txtDataStore.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("DataStore", txtDataStore.Text);
            };

        }

        void OnBeforePreview(object sender, EventArgs e)
        {
            WriteEncryptedCredentials();
        }

        void OnBeforeSave(object sender, CancelEventArgs e)
        {
            WriteEncryptedCredentials();
        }

        private void WriteEncryptedCredentials()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (username != "%MG_USERNAME%" && password != "%MG_PASSWORD%")
                {
                    _fs.SetConnectionProperty("Username", "%MG_USERNAME%");
                    _fs.SetConnectionProperty("Password", "%MG_PASSWORD%");
                    _fs.SetEncryptedCredentials(username, password);
                    _service.SyncSessionCopy();
                }
            }
            else if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                _fs.SetConnectionProperty("Username", null);
                _fs.SetConnectionProperty("Password", null);
                try
                {
                    _fs.DeleteResourceData("MG_USER_CREDENTIALS");
                }
                catch { }
                _service.SyncSessionCopy();
            }
        }

        public virtual string Title
        {
            get { return Properties.Resources.RdbmsFeatureSource; }
        }

        //MUST OVERRIDE
        public virtual string Provider
        {
            get { throw new NotImplementedException(); }
        }

        private string GetPartialConnectionStringForDataStoreEnumeration()
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Service"] = _fs.GetConnectionProperty("Service");
            builder["Username"] = txtUsername.Text; //_fs.GetConnectionProperty("Username");
            builder["Password"] = txtPassword.Text; //_fs.GetConnectionProperty("Password");
            return builder.ToString();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            WriteEncryptedCredentials();
        }

        private static string[] ConvertToArray(OSGeo.MapGuide.ObjectModels.Common.DataStoreList dstore)
        {
            List<string> values = new List<string>();
            foreach (var ds in dstore.DataStore)
            {
                values.Add(ds.Name);
            }
            return values.ToArray();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtStatus.Text = string.Empty;
            using (new WaitCursor(this))
            {
                WriteEncryptedCredentials();
                txtStatus.Text = string.Format(Properties.Resources.FdoConnectionStatus, _fs.TestConnection());
            }
        }

        private void btnBrowseDataStore_Click(object sender, EventArgs e)
        {
            try
            {
                var dstore = _service.FeatureService.EnumerateDataStores(this.Provider, GetPartialConnectionStringForDataStoreEnumeration());
                var values = ConvertToArray(dstore);
                string item = GenericItemSelectionDialog.SelectItem(Properties.Resources.TextSelectDataStore, Properties.Resources.TextSelectDataStore, values);
                if (item != null)
                {
                    txtDataStore.Text = item;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Properties.Resources.FailEnumDataStores, ex.Message));
            }
        }
    }
}
