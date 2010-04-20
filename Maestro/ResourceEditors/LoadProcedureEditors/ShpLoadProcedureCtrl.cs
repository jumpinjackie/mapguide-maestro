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

    public partial class ShpLoadProcedureCtrl : BaseFileLoadProcedureCtrl
    {
        public ShpLoadProcedureCtrl()
        {
            InitializeComponent();
        }

        public ShpLoadProcedureCtrl(EditorInterface ed)
            : base(ed)
        {
            InitializeComponent();
        }

        public ShpLoadProcedureCtrl(EditorInterface ed, string resourceID)
            : base(ed, resourceID)
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            loadSettingsCtrl1.Editor = _ed;
            sourceFilesCtrl1.FileFilter = "SHP Files (*.shp)|*.shp"; //TODO: Localize

            loadSettingsCtrl1.Modified += (sender, evt) => { if (!_isUpdating) base.RaiseModified(); };
            sourceFilesCtrl1.Modified += (sender, evt) => { if (!_isUpdating) base.RaiseModified(); };
        }

        private bool _isUpdating;

        public override void UpdateDisplay()
        {
            LoadProcedure lp = this.Resource as LoadProcedure;
            if (lp != null)
            {
                ShpLoadProcedureType shpl = lp.Item as ShpLoadProcedureType;
                if (shpl != null)
                {
                    _isUpdating = true;
                    sourceFilesCtrl1.SourceFiles = (shpl.SourceFile != null) ? shpl.SourceFile : new string[0];

                    txtDefaultCs.Text = shpl.CoordinateSystem;
                    numGeneralize.Value = (decimal)shpl.Generalization;
                    chkConvertSdf.Checked = shpl.ConvertToSdf;

                    loadSettingsCtrl1.LoadFeatureSources = shpl.GenerateSpatialDataSources;
                    loadSettingsCtrl1.LoadLayers = shpl.GenerateLayers;
                    loadSettingsCtrl1.FeatureSourceFolderName = shpl.SpatialDataSourcesFolder;
                    loadSettingsCtrl1.FeatureSourceRootPath = shpl.SpatialDataSourcesPath;
                    loadSettingsCtrl1.LayerFolderName = shpl.LayersFolder;
                    loadSettingsCtrl1.LayerRootPath = shpl.LayersPath;
                    loadSettingsCtrl1.ResourceRootPath = shpl.RootPath;
                    _isUpdating = false;
                }
            }
        }

        public override bool Save(string savename)
        {
            LoadProcedure lp = this.Resource as LoadProcedure;
            if (lp != null)
            {
                ShpLoadProcedureType shpl = lp.Item as ShpLoadProcedureType;
                if (shpl != null)
                {
                    shpl.SourceFile = sourceFilesCtrl1.SourceFiles;

                    shpl.CoordinateSystem = txtDefaultCs.Text;
                    shpl.Generalization = (double)numGeneralize.Value;
                    shpl.ConvertToSdf = chkConvertSdf.Checked;

                    shpl.GenerateSpatialDataSources = loadSettingsCtrl1.LoadFeatureSources ;
                    shpl.GenerateLayers = loadSettingsCtrl1.LoadLayers;
                    shpl.SpatialDataSourcesFolder = loadSettingsCtrl1.FeatureSourceFolderName;
                    shpl.SpatialDataSourcesPath = loadSettingsCtrl1.FeatureSourceRootPath;
                    shpl.LayersFolder = loadSettingsCtrl1.LayerFolderName;
                    shpl.LayersPath = loadSettingsCtrl1.LayerRootPath;
                    shpl.RootPath = loadSettingsCtrl1.ResourceRootPath;
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
            LoadProcedure lp = this.Resource as LoadProcedure;
            if (lp != null)
            {
                ShpLoadProcedureType shpl = lp.Item as ShpLoadProcedureType;
                if (shpl != null && shpl.ResourceId != null)
                {
                    affected.AddRange(shpl.ResourceId);
                }
            }

            if (affected.Count == 0)
            {
                foreach (string f in sourceFilesCtrl1.SourceFiles)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(f);

                    if (loadSettingsCtrl1.LoadFeatureSources)
                        affected.Add(loadSettingsCtrl1.FeatureSourceRootPath + loadSettingsCtrl1.FeatureSourceFolderName + "/" + name + ".FeatureSource");
                    if (loadSettingsCtrl1.LoadLayers)
                        affected.Add(loadSettingsCtrl1.LayerRootPath + loadSettingsCtrl1.LayerFolderName + "/" + name + ".LayerDefinition");
                }
            }
            return affected.ToArray();
        }
    }
}
