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
using Maestro.Shared.UI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

namespace Maestro.Editors.LoadProcedure
{
    internal partial class LoadTargetCtrl : EditorBindableCollapsiblePanel
    {
        public LoadTargetCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);

            _service = service;
            var lp = _service.GetEditedResource() as ILoadProcedure;
            var fproc = lp.SubType;
            
            TextBoxBinder.BindText(txtTargetRoot, fproc, "RootPath");
            CheckBoxBinder.BindChecked(chkCreateFeatureSources, fproc, "GenerateSpatialDataSources");
            CheckBoxBinder.BindChecked(chkCreateLayers, fproc, "GenerateLayers");
            TextBoxBinder.BindText(txtFeatureSourceRoot, fproc, "SpatialDataSourcesPath");
            TextBoxBinder.BindText(txtFeatureFolderName, fproc, "SpatialDataSourcesFolder");
            TextBoxBinder.BindText(txtLayerRoot, fproc, "LayersPath");
            TextBoxBinder.BindText(txtLayerFolderName, fproc, "LayersFolder");
        }

        private void btnBrowseRoot_Click(object sender, EventArgs e)
        {
            txtTargetRoot.Text = _service.SelectFolder();

            if (string.IsNullOrEmpty(txtFeatureSourceRoot.Text))
                txtFeatureSourceRoot.Text = txtTargetRoot.Text;

            if (string.IsNullOrEmpty(txtLayerRoot.Text))
                txtLayerRoot.Text = txtTargetRoot.Text;
        }

        private void btnBrowseFeatureRoot_Click(object sender, EventArgs e)
        {
            txtFeatureSourceRoot.Text = _service.SelectFolder();
        }

        private void btnBrowseLayerRoot_Click(object sender, EventArgs e)
        {
            txtLayerRoot.Text = _service.SelectFolder();
        }
    }
}
