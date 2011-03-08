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
using System.Collections.Specialized;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    public partial class UnmanagedCtrl : EditorBase, IOdbcSubEditor
    {
        public UnmanagedCtrl()
        {
            InitializeComponent();
        }

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
        }

        public Control Content
        {
            get { return this; }
        }

        void OnConnectionChanged()
        {
            var handler = this.ConnectionChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private static string InferDriver(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName).ToUpper();
            switch (ext)
            {
                case ".ACCDB":
                    return Properties.Resources.OdbcDriverAccess64;
                case ".MDB":
                    return Properties.Resources.OdbcDriverAccess;
                case ".XLS":
                    return Properties.Resources.OdbcDriverExcel;
                case ".XLSX":
                case ".XLSM":
                case ".XLSB":
                    return Properties.Resources.OdbcDriverExcel64;
            }
            return null;
        }

        public NameValueCollection ConnectionProperties
        {
            get
            {
                var values = new NameValueCollection();
                string path = txtFilePath.Text;
                if (string.IsNullOrEmpty(path))
                    return values;
                    //throw new InvalidOperationException(Properties.Resources.OdbcNoMarkedFile);

                string drv = InferDriver(path);
                if (drv == null)
                    return values;
                    //throw new InvalidOperationException(string.Format(Properties.Resources.OdbcCannotInferDriver, path));

                var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
                inner["Driver"] = drv;
                inner["Dbq"] = path;
                values["ConnectionString"] = inner.ToString();

                return values;
            }
            set
            {
                var cstr = value["ConnectionString"];
                var builder = new System.Data.Common.DbConnectionStringBuilder();
                builder.ConnectionString = cstr;
                if (builder["Dbq"] != null)
                    txtFilePath.Text = builder["Dbq"].ToString();
            }
        }

        public event EventHandler ConnectionChanged;
    }
}
