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

namespace Maestro.Editors.FeatureSource.Providers.SQLite
{
    internal partial class SQLiteFileCtrl : FileBasedCtrl
    {
        public SQLiteFileCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;

        public override void Bind(IEditorService service)
        {
            base.Bind(service);
            _fs = service.GetEditedResource() as IFeatureSource;
            Debug.Assert(_fs != null);

            MarkSelected();

            //HACK: Set UseFdoMetadata property if not specified otherwise this will be an invalid feature source
            if (string.IsNullOrEmpty(_fs.GetConnectionProperty("UseFdoMetadata")))
                _fs.SetConnectionProperty("UseFdoMetadata", "FALSE");

            chkUseFdoMetadata.Checked = _fs.GetConnectionProperty("UseFdoMetadata").ToUpper().Equals(true.ToString().ToUpper());
        }

        private void MarkSelected()
        {
            var file = _fs.GetConnectionProperty("File");
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
                    rdUnmanaged.Checked = true;
                    txtAlias.Text = file;
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
            string fileProp = "%MG_DATA_FILE_PATH%" + dataName;
            string currFileProp = _fs.GetConnectionProperty("File");
            if (!currFileProp.Equals(fileProp))
                _fs.SetConnectionProperty("File", fileProp);
        }

        private void chkUseFdoMetadata_CheckedChanged(object sender, EventArgs e)
        {
            var newValue = chkUseFdoMetadata.Checked.ToString().ToUpper();
            var currValue = _fs.GetConnectionProperty("UseFdoMetadata");
            if (!newValue.Equals(currValue))
                _fs.SetConnectionProperty("UseFdoMetadata", newValue);
        }
    }
}
