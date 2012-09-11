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
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Diagnostics;
using System.IO;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.FeatureSource.Providers.Shp
{
    [ToolboxItem(false)]
    internal partial class ShpFileCtrl : FileBasedCtrl
    {
        public ShpFileCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            base.Bind(service);
            _service = service;
            _fs = service.GetEditedResource() as IFeatureSource;
            Debug.Assert(_fs != null);
            if (_fs.ConnectionPropertyNames.Length == 0)
            {
                _fs.SetConnectionProperty("DefaultFileLocation", StringConstants.MgDataFilePath); //NOXLATE
            }
            MarkSelected();
            resDataCtrl.ResourceDataUploaded += new Maestro.Editors.Common.ResourceDataCtrl.ResourceUploadEventHandler(OnResourceDataUploaded);
        }

        static readonly string[] SHP_RELATED_EXTENSIONS = { ".shx", ".dbf", ".idx", ".prj", ".cpg" }; //NOXLATE

        void OnResourceDataUploaded(string dataName, string origPath)
        {
            //If a SHP file was loaded, we want all of its buddies too
            if (origPath.ToLower().EndsWith(".shp")) //NOXLATE
            {
                var pathPrefix = origPath.Substring(0, origPath.Length - 4);
                foreach (string ext in SHP_RELATED_EXTENSIONS)
                {
                    string path = pathPrefix + ext;
                    //Can't upload something that doesn't exist
                    if (File.Exists(path))
                    {
                        //This can cause this method to be called again,
                        //as UploadFile() raises ResourceDataUploaded
                        //but there isn't a scenario I can think of where
                        //this would reach StackOverflowException proportions
                        resDataCtrl.UploadFile(path);
                    }
                }
            }
        }

        protected override string[] GetUnmanagedFileExtensions()
        {
            return new string[] { "shp" }; //NOXLATE
        }

        protected override string FileFdoProperty
        {
            get
            {
                return "DefaultFileLocation"; //NOXLATE
            }
        }

        private void MarkSelected()
        {
            var file = _fs.GetConnectionProperty(this.FileFdoProperty);
            if (!string.IsNullOrEmpty(file))
            {
                if (_fs.UsesEmbeddedDataFiles)
                {
                    rdManaged.Checked = true;
                    var df = _fs.GetEmbeddedDataName();
                    resDataCtrl.MarkedFile = df;
                }
                else //if (_fs.UsesAliasedDataFiles)
                {
                    txtAlias.Text = file;
                    rdUnmanaged.Checked = true;
                }
            }
        }

        protected override void OnResourceChanged()
        {
            base.OnResourceChanged();
            MarkSelected();
        }

        protected override void OnResourceMarked(string dataName)
        {
            var newValue = StringConstants.MgDataFilePath + dataName;
            var currValue = _fs.GetConnectionProperty(this.FileFdoProperty);
            if (!newValue.Equals(currValue))
                _fs.SetConnectionProperty(this.FileFdoProperty, newValue);
        }

        protected override bool CanSelectFolders()
        {
            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtStatus.Text = string.Empty;
            using (new WaitCursor(this))
            {
                _service.SyncSessionCopy();
                txtStatus.Text = string.Format(Strings.FdoConnectionStatus, _fs.TestConnection());
            }
        }
    }
}
