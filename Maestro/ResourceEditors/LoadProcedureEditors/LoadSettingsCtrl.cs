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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LoadProcedureEditors
{
    public partial class LoadSettingsCtrl : UserControl
    {
        public event EventHandler Modified;

        public LoadSettingsCtrl()
        {
            InitializeComponent();
            this.LoadFeatureSources = false;
            this.LoadLayers = false;
        }

        public EditorInterface Editor
        {
            get;
            set;
        }

        private void RaiseModified()
        {
            var handler = this.Modified;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public string ResourceRootPath
        {
            get { return txtRootPath.Text; }
            set 
            {
                string oldValue = txtRootPath.Text;
                txtRootPath.Text = value; 
                if (this.FeatureSourceRootPath == oldValue || string.IsNullOrEmpty(this.FeatureSourceRootPath))
                {
                    this.FeatureSourceRootPath = value;
                }
                if (this.LayerRootPath == oldValue || string.IsNullOrEmpty(this.LayerRootPath))
                {
                    this.LayerRootPath = value;
                }

                RaiseModified();
            }
        }

        public string LayerRootPath
        {
            get { return txtLayerRoot.Text; }
            set 
            { 
                txtLayerRoot.Text = value;
                RaiseModified();
            }
        }

        public string FeatureSourceRootPath
        {
            get { return txtFeatureSourceRoot.Text; }
            set 
            {
                txtFeatureSourceRoot.Text = value;
                RaiseModified();
            }
        }

        public string LayerFolderName
        {
            get { return txtLayerFolderName.Text; }
            set 
            {
                txtLayerFolderName.Text = value;
                RaiseModified();
            }
        }

        public string FeatureSourceFolderName
        {
            get { return txtFeatureSourceFolderName.Text; }
            set 
            { 
                txtFeatureSourceFolderName.Text = value;
                RaiseModified();
            }
        }

        public bool LoadFeatureSources
        {
            get { return chkLoadFeatureSources.Checked; }
            set 
            { 
                chkLoadFeatureSources.Checked = value;
                RaiseModified();
            }
        }

        public bool LoadLayers
        {
            get { return chkLoadLayers.Checked; }
            set 
            { 
                chkLoadLayers.Checked = value;
                RaiseModified();
            }
        }

        private void chkLoadFeatureSources_CheckedChanged(object sender, EventArgs e)
        {
            txtFeatureSourceFolderName.Enabled = txtFeatureSourceRoot.Enabled = btnBrowseFsRoot.Enabled = chkLoadFeatureSources.Checked;
            RaiseModified();
        }

        private void chkLoadLayers_CheckedChanged(object sender, EventArgs e)
        {
            txtLayerFolderName.Enabled = txtLayerRoot.Enabled = btnBrowseLayerRoot.Enabled = chkLoadLayers.Checked;
            RaiseModified();
        }

        private string GetFolderPath()
        {
            if (this.Editor != null)
            {
                string value = this.Editor.BrowseResource("Folder");
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }

        private void btnBrowseResourceRoot_Click(object sender, EventArgs e)
        {
            string value = GetFolderPath();
            if (value != null)
            {
                this.ResourceRootPath = value;
            }
        }

        private void btnBrowseFolderRoot_Click(object sender, EventArgs e)
        {
            string value = GetFolderPath();
            if (value != null)
            {
                this.FeatureSourceRootPath = value;
            }
        }

        private void btnBrowseLayerRoot_Click(object sender, EventArgs e)
        {
            string value = GetFolderPath();
            if (value != null)
            {
                this.LayerRootPath = value;
            }
        }
    }
}
