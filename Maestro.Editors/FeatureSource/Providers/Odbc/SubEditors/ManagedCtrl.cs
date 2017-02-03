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
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    [ToolboxItem(false)]
    internal partial class ManagedCtrl : EditorBase, IOdbcSubEditor
    {
        public ManagedCtrl()
        {
            InitializeComponent();
            resDataCtrl.ResourceDataMarked += new Maestro.Editors.Common.ResourceDataSelectionEventHandler(OnResourceDataMarked);
            resDataCtrl.ResourceDataUploaded += new Maestro.Editors.Common.ResourceDataCtrl.ResourceUploadEventHandler(OnResourceDataAdded);
        }

        private void OnResourceDataAdded(string dataName, string origPath)
        {
            OnResourceChanged();
        }

        private void OnResourceDataMarked(object sender, string dataName)
        {
            OnConnectionChanged();
        }

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            resDataCtrl.Init(service);
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
                return GetConnectionPropertiesInternal(false);
            }
            set
            {
                var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
                if (value["ConnectionString"] == null) //NOXLATE
                    throw new InvalidOperationException(string.Format(Strings.FdoConnectionStringComponentNotFound, "ConnectionString")); //NOXLATE

                inner.ConnectionString = value["ConnectionString"]; //NOXLATE
                if (inner["Dbq"] == null) //NOXLATE
                    throw new InvalidOperationException(string.Format(Strings.OdbcConnectionStringComponentNotFound, "Dbq")); //NOXLATE

                string path = inner["Dbq"].ToString(); //NOXLATE
                if (path.Contains(StringConstants.MgDataFilePath))
                    resDataCtrl.MarkedFile = path.Substring(StringConstants.MgDataFilePath.Length);
            }
        }

        public NameValueCollection Get64BitConnectionProperties()
        {
            return GetConnectionPropertiesInternal(true);
        }

        private NameValueCollection GetConnectionPropertiesInternal(bool use64Bit)
        {
            var values = new NameValueCollection();

            if (string.IsNullOrEmpty(resDataCtrl.MarkedFile))
                return values;
            //throw new InvalidOperationException(Properties.Resources.OdbcNoMarkedFile);

            string drv = InferDriver(resDataCtrl.MarkedFile, use64Bit);
            if (drv == null)
                return values;
            //throw new InvalidOperationException(string.Format(Properties.Resources.OdbcCannotInferDriver, resDataCtrl.MarkedFile));

            var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
            inner["Driver"] = drv; //NOXLATE
            inner["Dbq"] = StringConstants.MgDataFilePath + resDataCtrl.MarkedFile;
            values["ConnectionString"] = inner.ToString(); //NOXLATE

            return values;
        }

        public event EventHandler ConnectionChanged;

        private static string InferDriver(string fileName, bool use64Bit)
        {
            string ext = System.IO.Path.GetExtension(fileName).ToUpper();
            switch (ext)
            {
                case ".ACCDB": //NOXLATE
                    return OdbcDriverNames.OdbcDriverAccess64;

                case ".MDB": //NOXLATE
                    return use64Bit ? OdbcDriverNames.OdbcDriverAccess64 : OdbcDriverNames.OdbcDriverAccess;

                case ".XLS": //NOXLATE
                    return use64Bit ? OdbcDriverNames.OdbcDriverExcel64 : OdbcDriverNames.OdbcDriverExcel;

                case ".XLSX": //NOXLATE
                case ".XLSM": //NOXLATE
                case ".XLSB": //NOXLATE
                    return OdbcDriverNames.OdbcDriverExcel64;
            }
            return null;
        }

        public Control Content
        {
            get { return this; }
        }

        public event EventHandler RequestDocumentReset;
    }
}