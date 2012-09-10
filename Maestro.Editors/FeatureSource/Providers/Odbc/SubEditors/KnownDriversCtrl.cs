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
using System.Collections.Specialized;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    [ToolboxItem(false)]
    internal partial class KnownDriversCtrl : EditorBase, IOdbcSubEditor
    {
        public KnownDriversCtrl()
        {
            InitializeComponent();
        }

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            lstDriver.DataSource = OdbcDriverMap.EnumerateDrivers();
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
                if (this.SelectedDriver != null)
                    values["ConnectionString"] = this.SelectedDriver.OdbcConnectionString; //NOXLATE
                return values;
            }
            set
            {
                if (this.SelectedDriver != null && value["ConnectionString"] != null) //NOXLATE
                    this.SelectedDriver.OdbcConnectionString = value["ConnectionString"]; //NOXLATE
            }
        }

        public NameValueCollection Get64BitConnectionProperties()
        {
            return this.ConnectionProperties;
        }

        public event EventHandler ConnectionChanged;

        public Control Content
        {
            get { return this; }
        }

        private OdbcDriverInfo _SelectedDriver;

        public OdbcDriverInfo SelectedDriver
        {
            get { return _SelectedDriver; }
            set 
            { 
                _SelectedDriver = value;
                propGrid.SelectedObject = value;
            }
        }

        private void lstDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDriver.SelectedItem != null)
            {
                this.SelectedDriver = OdbcDriverMap.GetDriver(lstDriver.SelectedItem.ToString());
            }
        }


        public event EventHandler RequestDocumentReset;
    }
}
