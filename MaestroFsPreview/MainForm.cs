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

using Maestro.Editors;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Windows.Forms;

namespace MaestroFsPreview
{
    public partial class MainForm : Form
    {
        private MainForm()
        {
            InitializeComponent();
        }

        public MainForm(IEditorService edSvc)
            : this()
        {
            _edSvc = edSvc;
            localFsPreviewCtrl.Init(edSvc);
        }

        private readonly IEditorService _edSvc;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.FeatureSource.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.FeatureSourceID = picker.ResourceID;
                }
            }
        }

        private IFeatureSource _fs;

        public string FeatureSourceID
        {
            get { return txtFeatureSource.Text; }
            set
            {
                txtFeatureSource.Text = value;
                InitPreview();
            }
        }

        private void InitPreview()
        {
            var conn = _edSvc.CurrentConnection;
            _fs = (IFeatureSource)conn.ResourceService.GetResource(this.FeatureSourceID);
            var caps = conn.FeatureService.GetProviderCapabilities(_fs.Provider);
            localFsPreviewCtrl.SupportsSQL = caps.Connection.SupportsSQL;
            localFsPreviewCtrl.ReloadTree(this.FeatureSourceID, caps);
        }
    }
}