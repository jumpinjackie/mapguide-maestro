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

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    public partial class ManagedCtrl : EditorBase, IOdbcSubEditor
    {
        public ManagedCtrl()
        {
            InitializeComponent();
            resDataCtrl.ResourceDataMarked += new Maestro.Editors.Common.ResourceDataSelectionEventHandler(OnResourceDataMarked);
            resDataCtrl.ResourceDataUploaded += new Maestro.Editors.Common.ResourceDataCtrl.ResourceUploadEventHandler(OnResourceDataAdded);
        }

        void OnResourceDataAdded(string dataName, string origPath)
        {
            OnResourceChanged();
        }

        void OnResourceDataMarked(object sender, string dataName)
        {
            OnConnectionChanged();
        }

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            resDataCtrl.Init(service);
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

                if (string.IsNullOrEmpty(resDataCtrl.MarkedFile))
                    return values;
                    //throw new InvalidOperationException(Properties.Resources.OdbcNoMarkedFile);

                string drv = InferDriver(resDataCtrl.MarkedFile);
                if (drv == null)
                    return values;
                    //throw new InvalidOperationException(string.Format(Properties.Resources.OdbcCannotInferDriver, resDataCtrl.MarkedFile));

                var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
                inner["Driver"] = drv;
                inner["Dbq"] = "%MG_DATA_FILE_PATH%" + resDataCtrl.MarkedFile;
                values["ConnectionString"] = inner.ToString();

                return values;
            }
            set
            {
                var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
                if (value["ConnectionString"] == null)
                    throw new InvalidOperationException(string.Format(Properties.Resources.FdoConnectionStringComponentNotFound, "ConnectionString"));

                inner.ConnectionString = value["ConnectionString"];
                if (inner["Dbq"] == null)
                    throw new InvalidOperationException(string.Format(Properties.Resources.OdbcConnectionStringComponentNotFound, "Dbq"));

                string path = inner["Dbq"].ToString();
                if (path.Contains("%MG_DATA_FILE_PATH%"))
                    resDataCtrl.MarkedFile = path.Substring("%MG_DATA_FILE_PATH%".Length);
            }
        }

        public event EventHandler ConnectionChanged;

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

        public Control Content
        {
            get { return this; }
        }

        public event EventHandler ResourceChanged;
    }
}
