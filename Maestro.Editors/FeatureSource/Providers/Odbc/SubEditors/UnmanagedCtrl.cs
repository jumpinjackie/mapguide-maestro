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
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.SubEditors
{
    [ToolboxItem(false)]
    internal partial class UnmanagedCtrl : EditorBase, IOdbcSubEditor
    {
        public UnmanagedCtrl()
        {
            InitializeComponent();
        }

        private IResourceService _resSvc;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _resSvc = service.ResourceService;
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

        public NameValueCollection ConnectionProperties
        {
            get
            {
                return GetConnectionPropertiesInternal(false);
            }
            set
            {
                var cstr = value["ConnectionString"]; //NOXLATE
                var builder = new System.Data.Common.DbConnectionStringBuilder();
                builder.ConnectionString = cstr;
                if (builder["Dbq"] != null) //NOXLATE
                    txtFilePath.Text = builder["Dbq"].ToString(); //NOXLATE
            }
        }

        private NameValueCollection GetConnectionPropertiesInternal(bool use64Bit)
        {
            var values = new NameValueCollection();
            string path = txtFilePath.Text;
            if (string.IsNullOrEmpty(path))
                return values;
            //throw new InvalidOperationException(Properties.Resources.OdbcNoMarkedFile);

            string drv = InferDriver(path, use64Bit);
            if (drv == null)
                return values;
            //throw new InvalidOperationException(string.Format(Properties.Resources.OdbcCannotInferDriver, path));

            var inner = new System.Data.Odbc.OdbcConnectionStringBuilder();
            inner["Driver"] = drv; //NOXLATE
            inner["Dbq"] = path; //NOXLATE
            values["ConnectionString"] = inner.ToString(); //NOXLATE

            return values;
        }

        public NameValueCollection Get64BitConnectionProperties()
        {
            return GetConnectionPropertiesInternal(true);
        }

        public event EventHandler ConnectionChanged;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var open = new OpenFileDialog())
            {
                open.Filter = OdbcDriverNames.OdbcDriverAccess + "|*.mdb|" + //NOXLATE
                              OdbcDriverNames.OdbcDriverAccess64 + "|*.accdb,*.mdb|" +  //NOXLATE
                              OdbcDriverNames.OdbcDriverExcel + "|*.xls|" + //NOXLATE
                              OdbcDriverNames.OdbcDriverExcel64 + "|*.xlsx,*.xlsm,*.xlsb"; //NOXLATE

                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = open.FileName;
                }
            }
        }

        private void btnBrowseAlias_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_resSvc))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = picker.SelectedItem;
                }
            }
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {
            OnConnectionChanged();
        }

        /// <summary>
        /// Raised when a resource document reset is required
        /// </summary>
        public event EventHandler RequestDocumentReset;
    }

    internal class OdbcDriverNames
    {
        //These aren't localizable
        public const string OdbcDriverAccess = "{Microsoft Access Driver (*.mdb)}";
        public const string OdbcDriverAccess64 = "{Microsoft Access Driver (*.mdb, *.accdb)}";
        public const string OdbcDriverExcel = "{Microsoft Excel Driver (*.xls)}";
        public const string OdbcDriverExcel64 = "{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}";

        internal static bool Uses64BitDriver(string odbcConnStr)
        {
            return odbcConnStr.Contains(OdbcDriverNames.OdbcDriverAccess64) ||
                   odbcConnStr.Contains(OdbcDriverNames.OdbcDriverExcel64);
        }
    }
}
