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
using Maestro.Editors.FeatureSource.Providers.Odbc;

namespace Maestro.Editors.FeatureSource.Providers.Rdbms
{
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

        private IEditorService _edsvc;
        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);
            _fs = _edsvc.GetEditedResource() as IFeatureSource;

            //Set the field values
            txtService.Text = _fs.GetConnectionProperty("Service");
            txtUsername.Text = _fs.GetConnectionProperty("Username");
            txtPassword.Text = _fs.GetConnectionProperty("Password");

            UpdateDataStoreValues(true);

            //Set initial value of data store if possible
            var dstore = _fs.GetConnectionProperty("DataStore");
            if (!string.IsNullOrEmpty(dstore) && cmbDataStore.Items.Count > 0)
            {
                var idx = cmbDataStore.Items.IndexOf(dstore);
                if (idx >= 0)
                    cmbDataStore.SelectedIndex = idx;
            }

            //As our connection properties are not CLR properties, 
            //"manually" bind these fields
            txtService.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("Service", txtService.Text);
            };

            txtUsername.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("Username", txtUsername.Text);
            };
            
            txtPassword.TextChanged += (s, e) =>
            {
                _fs.SetConnectionProperty("Password", txtPassword.Text);
            };

            cmbDataStore.SelectedIndexChanged += (s, e) =>
            {
                if (cmbDataStore.SelectedItem != null)
                    _fs.SetConnectionProperty("DataStore", cmbDataStore.SelectedItem.ToString());
            };
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

        private string GetPartialConnectionString()
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Service"] = _fs.GetConnectionProperty("Service");
            builder["Username"] = _fs.GetConnectionProperty("Username");
            builder["Password"] = _fs.GetConnectionProperty("Password");
            return builder.ToString();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            UpdateDataStoreValues(false);
        }

        private void UpdateDataStoreValues(bool silent)
        {
            using (new WaitCursor(this))
            {
                string[] values = null;
                string reason = string.Empty;
                try
                {
                    var dstore = _edsvc.FeatureService.EnumerateDataStores(this.Provider, GetPartialConnectionString());
                    values = ConvertToArray(dstore);
                }
                catch (Exception ex) { reason = ex.ToString(); }
                if (values != null && values.Length > 0)
                {
                    cmbDataStore.DataSource = values;
                }
                else
                {
                    if (!silent)
                        MessageBox.Show(string.Format(Properties.Resources.FailEnumDataStores, reason));

                    cmbDataStore.DataSource = null;
                }
            }
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
    }
}
