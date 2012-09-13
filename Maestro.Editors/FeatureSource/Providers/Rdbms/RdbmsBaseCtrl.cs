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

        private bool _bChangedUsername = false;
        private bool _bChangedPassword = false;

        public override void Bind(IEditorService service)
        {
            _bChangedUsername = false;
            _bChangedPassword = false;
            _service = service;
            _service.BeforeSave += OnBeforeSave;
            _service.BeforePreview += OnBeforePreview;
            _service.RegisterCustomNotifier(this);
            _fs = _service.GetEditedResource() as IFeatureSource;

            //Set the field values
            txtService.Text = _fs.GetConnectionProperty("Service"); //NOXLATE

            //We're gonna follow MG Studio behaviour here which is: Never load the password
            //and auto trigger dirty state.
            if (!_service.IsNew)
            {
                txtUsername.Text = _fs.GetEncryptedUsername() ?? _fs.GetConnectionProperty("Username"); //NOXLATE
                txtPassword.Text = GenerateRandomFakeString();
            }

            //Set initial value of data store if possible
            var dstore = _fs.GetConnectionProperty("DataStore"); //NOXLATE
            txtDataStore.Text = dstore;

            //As our connection properties are not CLR properties, 
            //"manually" bind these fields
            txtService.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("Service", txtService.Text); //NOXLATE
            };

            txtUsername.TextChanged += (s, e) =>
            {
                _bChangedUsername = true;
                if (string.IsNullOrEmpty(txtUsername.Text))
                    _fs.SetConnectionProperty("Username", null); //NOXLATE
                else
                    _fs.SetConnectionProperty("Username", txtUsername.Text); //NOXLATE
            };
            
            txtPassword.TextChanged += (s, e) =>
            {
                _bChangedPassword = true;
                if (string.IsNullOrEmpty(txtPassword.Text))
                    _fs.SetConnectionProperty("Password", null); //NOXLATE
                else
                    _fs.SetConnectionProperty("Password", txtPassword.Text); //NOXLATE
            };

            txtDataStore.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("DataStore", txtDataStore.Text); //NOXLATE
            };

        }

        private string GenerateRandomFakeString()
        {
            Random rng = new Random();
            char[] letters = new char[rng.Next(6, 12)];
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = GenerateChar(rng);
            }
            return new string(letters);
        }

        private static char GenerateChar(Random rng)
        {
            // 'Z' + 1 because the range is exclusive
            return (char)(rng.Next('A', 'Z' + 1));
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
                if (username != StringConstants.MgUsernamePlaceholder && password != StringConstants.MgPasswordPlaceholder)
                {
                    if (_bChangedUsername || _bChangedPassword)
                    {
                        _fs.SetConnectionProperty("Username", StringConstants.MgUsernamePlaceholder); //NOXLATE
                        _fs.SetConnectionProperty("Password", StringConstants.MgPasswordPlaceholder); //NOXLATE
                        _fs.SetEncryptedCredentials(username, password);
                        _service.SyncSessionCopy();
                    }
                }
            }
            else if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
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

        public virtual string Title
        {
            get { return Strings.RdbmsFeatureSource; }
        }

        //MUST OVERRIDE
        public virtual string Provider
        {
            get { throw new NotImplementedException(); }
        }

        private string GetPartialConnectionStringForDataStoreEnumeration()
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Service"] = _fs.GetConnectionProperty("Service"); //NOXLATE
            builder["Username"] = txtUsername.Text; //_fs.GetConnectionProperty("Username"); //NOXLATE
            builder["Password"] = txtPassword.Text; //_fs.GetConnectionProperty("Password"); //NOXLATE
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
                txtStatus.Text = string.Format(Strings.FdoConnectionStatus, _fs.TestConnection());
            }
        }

        private void btnBrowseDataStore_Click(object sender, EventArgs e)
        {
            try
            {
                var dstore = _service.FeatureService.EnumerateDataStores(this.Provider, GetPartialConnectionStringForDataStoreEnumeration());
                var values = ConvertToArray(dstore);
                string item = GenericItemSelectionDialog.SelectItem(Strings.TextSelectDataStore, Strings.TextSelectDataStore, values);
                if (item != null)
                {
                    txtDataStore.Text = item;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Strings.FailEnumDataStores, ex.Message));
            }
        }
    }
}
