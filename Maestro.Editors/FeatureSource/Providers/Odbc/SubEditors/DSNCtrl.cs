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
using OSGeo.MapGuide.MaestroAPI;
using System.Collections.Specialized;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.Common;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    [ToolboxItem(false)]
    internal partial class DSNCtrl : EditorBase, IOdbcSubEditor
    {
        public DSNCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;

        string[] _dsnNames;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _fs = (IFeatureSource)service.GetEditedResource();
            _dsnNames = service.FeatureService.GetConnectionPropertyValues("OSGeo.ODBC", "DataSourceName", string.Empty); //NOXLATE
        }

        void OnConnectionChanged()
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
                if (!string.IsNullOrEmpty(txtDSN.Text))
                    values["DataSourceName"] = txtDSN.Text; //NOXLATE
                return values;
            }
            set
            {
                txtDSN.Text = value["DataSourceName"]; //NOXLATE
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

        private void btnBrowseDsn_Click(object sender, EventArgs e)
        {
            string dsn = GenericItemSelectionDialog.SelectItem(null, null, _dsnNames);
            if (dsn != null)
            {
                if (dsn != txtDSN.Text)
                {
                    bool reset = MessageBox.Show(Strings.PromptResetOdbcConfigDocument, Strings.TitleQuestion, MessageBoxButtons.YesNo) == DialogResult.Yes;
                    if (reset)
                    {
                        txtDSN.Text = dsn;
                        OnRequestDocumentReset();
                        OnConnectionChanged();
                    }
                }
            }
        }

        void OnRequestDocumentReset()
        {
            var handler = this.RequestDocumentReset;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler RequestDocumentReset;
    }
}
