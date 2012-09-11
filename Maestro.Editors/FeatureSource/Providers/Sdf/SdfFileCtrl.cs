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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.FeatureSource.Providers.Sdf
{
    [ToolboxItem(false)]
    internal partial class SdfFileCtrl : FileBasedCtrl
    {
        public SdfFileCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            base.Bind(service);
            _service = service;
            _fs = _service.GetEditedResource() as IFeatureSource;
            Debug.Assert(_fs != null);

            MarkSelected();

            //HACK: Set ReadOnly property if not specified otherwise this will be an invalid feature source
            if (string.IsNullOrEmpty(_fs.GetConnectionProperty("ReadOnly"))) //NOXLATE
                _fs.SetConnectionProperty("ReadOnly", "FALSE"); //NOXLATE

            chkReadOnly.Checked = _fs.GetConnectionProperty("ReadOnly").ToUpper().Equals(true.ToString().ToUpper()); //NOXLATE
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

        protected override string[] GetUnmanagedFileExtensions()
        {
            return new string[] { "sdf" }; //NOXLATE
        }

        protected override void OnResourceChanged()
        {
            base.OnResourceChanged();
            MarkSelected();
        }

        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            var newValue = chkReadOnly.Checked.ToString().ToUpper();
            var currValue = _fs.GetConnectionProperty("ReadOnly"); //NOXLATE
            if (!newValue.Equals(currValue))
                _fs.SetConnectionProperty("ReadOnly", newValue); //NOXLATE
        }

        protected override void OnResourceMarked(string dataName)
        {
            string fileProp = StringConstants.MgDataFilePath + dataName;
            string currFileProp = _fs.GetConnectionProperty(this.FileFdoProperty);
            if (!currFileProp.Equals(fileProp))
                _fs.SetConnectionProperty(this.FileFdoProperty, fileProp);
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
