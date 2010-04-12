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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    //NOTE: Except for Default CS, all transformation settings are disabled because
    //backend support for those features does not exist for this load procedure yet.

    public partial class SdfLoadProcedureCtrl : BaseFileLoadProcedureCtrl
    {
        public SdfLoadProcedureCtrl()
        {
            InitializeComponent();
            cmbSdfDuplicateStrategy.DataSource = Enum.GetValues(typeof(SdfKeyTreatmentType));
        }

        public SdfLoadProcedureCtrl(EditorInterface ed)
            : base(ed)
        {
            InitializeComponent();
            cmbSdfDuplicateStrategy.DataSource = Enum.GetValues(typeof(SdfKeyTreatmentType));
        }

        public SdfLoadProcedureCtrl(EditorInterface ed, string resourceID)
            : base(ed, resourceID)
        {
            InitializeComponent();
            cmbSdfDuplicateStrategy.DataSource = Enum.GetValues(typeof(SdfKeyTreatmentType));
        }

        protected override void OnLoad(EventArgs e)
        {
            loadSettingsCtrl1.Editor = _ed;
            sourceFilesCtrl1.FileFilter = "SDF Files (*.sdf)|*.sdf"; //TODO: Localize

            loadSettingsCtrl1.Modified += (sender, evt) => { if (!_isUpdating) base.RaiseModified(); };
            sourceFilesCtrl1.Modified += (sender, evt) => { if (!_isUpdating) base.RaiseModified(); };
        }

        private bool _isUpdating;

        public override void UpdateDisplay()
        {
            LoadProcedure lp = this.Resource as LoadProcedure;
            if (lp != null)
            {
                SdfLoadProcedureType sdfl = lp.Item as SdfLoadProcedureType;
                if (sdfl != null)
                {
                    _isUpdating = true;
                    sourceFilesCtrl1.SourceFiles = (sdfl.SourceFile != null) ? sdfl.SourceFile : new string[0];

                    txtDefaultCs.Text = sdfl.CoordinateSystem;
                    numGeneralize.Value = (decimal)sdfl.Generalization;
                    cmbSdfDuplicateStrategy.SelectedItem = sdfl.SdfKeyTreatment;

                    loadSettingsCtrl1.LoadFeatureSources = sdfl.GenerateSpatialDataSources;
                    loadSettingsCtrl1.LoadLayers = sdfl.GenerateLayers;
                    loadSettingsCtrl1.FeatureSourceFolderName = sdfl.SpatialDataSourcesFolder;
                    loadSettingsCtrl1.FeatureSourceRootPath = sdfl.SpatialDataSourcesPath;
                    loadSettingsCtrl1.LayerFolderName = sdfl.LayersFolder;
                    loadSettingsCtrl1.LayerRootPath = sdfl.LayersPath;
                    loadSettingsCtrl1.ResourceRootPath = sdfl.RootPath;
                    _isUpdating = false;
                }
            }
        }

        public override bool Save(string savename)
        {
            LoadProcedure lp = this.Resource as LoadProcedure;
            if (lp != null)
            {
                SdfLoadProcedureType sdfl = lp.Item as SdfLoadProcedureType;
                if (sdfl != null)
                {
                    sdfl.SourceFile = sourceFilesCtrl1.SourceFiles;

                    sdfl.CoordinateSystem = txtDefaultCs.Text;
                    sdfl.Generalization = (double)numGeneralize.Value;
                    sdfl.SdfKeyTreatment = (SdfKeyTreatmentType)cmbSdfDuplicateStrategy.SelectedItem;

                    sdfl.GenerateSpatialDataSources = loadSettingsCtrl1.LoadFeatureSources;
                    sdfl.GenerateLayers = loadSettingsCtrl1.LoadLayers;
                    sdfl.SpatialDataSourcesFolder = loadSettingsCtrl1.FeatureSourceFolderName;
                    sdfl.SpatialDataSourcesPath = loadSettingsCtrl1.FeatureSourceRootPath;
                    sdfl.LayersFolder = loadSettingsCtrl1.LayerFolderName;
                    sdfl.LayersPath = loadSettingsCtrl1.LayerRootPath;
                    sdfl.RootPath = loadSettingsCtrl1.ResourceRootPath;
                }
            }
            return false;
        }

        private void btnBrowseCS_Click(object sender, EventArgs e)
        {
            SelectCoordinateSystem dlg = new SelectCoordinateSystem(_ed.CurrentConnection);
            dlg.SetWKT(txtDefaultCs.Text);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtDefaultCs.Text = dlg.SelectedCoordSys.Projection;
        }

        public override string[] GetAffectedResourceIds()
        {
            List<string> affected = new List<string>();
            foreach (string f in sourceFilesCtrl1.SourceFiles)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(f);

                if (loadSettingsCtrl1.LoadFeatureSources)
                    affected.Add(loadSettingsCtrl1.FeatureSourceRootPath + loadSettingsCtrl1.FeatureSourceFolderName + "/" + name + ".FeatureSource");
                if (loadSettingsCtrl1.LoadLayers)
                    affected.Add(loadSettingsCtrl1.LayerRootPath + loadSettingsCtrl1.LayerFolderName + "/" + name + ".LayerDefinition");
            }
            return affected.ToArray();
        }
    }
}
