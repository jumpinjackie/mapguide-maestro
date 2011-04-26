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
    [ToolboxItem(false)]
    internal partial class LoadTargetCtrl : EditorBindableCollapsiblePanel
    {
        public LoadTargetCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _service;
        private ILoadProcedure _loadProc;
        private IBaseLoadProcedure _fProc;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);

            _service = service;
            _loadProc = _service.GetEditedResource() as ILoadProcedure;
            _fProc = _loadProc.SubType;

            TextBoxBinder.BindText(txtTargetRoot, _fProc, "RootPath");
            CheckBoxBinder.BindChecked(chkCreateFeatureSources, _fProc, "GenerateSpatialDataSources");
            CheckBoxBinder.BindChecked(chkCreateLayers, _fProc, "GenerateLayers");
            TextBoxBinder.BindText(txtFeatureSourceRoot, _fProc, "SpatialDataSourcesPath");
            TextBoxBinder.BindText(txtFeatureFolderName, _fProc, "SpatialDataSourcesFolder");
            TextBoxBinder.BindText(txtLayerRoot, _fProc, "LayersPath");
            TextBoxBinder.BindText(txtLayerFolderName, _fProc, "LayersFolder");

            _fProc.PropertyChanged += OnLoadProcedurePropertyChanged;
        }

        protected override void UnsubscribeEventHandlers()
        {
            if (_fProc != null)
            {
                _fProc.PropertyChanged -= OnLoadProcedurePropertyChanged;
            }
            base.UnsubscribeEventHandlers();
        }

        void OnLoadProcedurePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnResourceChanged();
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
