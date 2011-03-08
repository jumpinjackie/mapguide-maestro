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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using Maestro.Shared.UI;

namespace Maestro.Base.UI.Packaging
{
    public partial class CreatePackageDialog : Form
    {
        public CreatePackageDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        public CreatePackageDialog(IServerConnection conn)
            : this()
        {
            _conn = conn;
        }

        protected override void OnLoad(EventArgs e)
        {
            var caps = _conn.Capabilities;
            foreach (var rt in SiteResourceType.All())
            {
                if (caps.IsSupportedResourceType(rt))
                {
                    chkAllowedTypes.Items.Add(rt, true);
                }
            }
            CheckAll(true);
            chkRestorePath_CheckedChanged(this, EventArgs.Empty);
            CheckSubmitState();
        }

        public string FolderToPackage
        {
            get { return txtResourcePath.Text; }
            set 
            { 
                txtResourcePath.Text = value;
                CheckSubmitState();
            }
        }

        public string OutputFileName
        {
            get { return txtPackageFilename.Text; }
            set 
            { 
                txtPackageFilename.Text = value; 
                CheckSubmitState(); 
            }
        }

        public string RestorePath
        {
            get { return chkRestorePath.Checked ? GetRestorePath() : string.Empty; }
            set
            {
                if (chkRestorePath.Checked)
                {
                    txtRestorePath.Text = value; 
                    CheckSubmitState();
                }
            }
        }

        private string GetRestorePath()
        {
            var path = txtRestorePath.Text.Trim();
            return path;
        }

        public bool RemoveTargetFolderOnRestore
        {
            get { return chkRemoveTargetFolderOnRestore.Checked; }
            set 
            { 
                chkRemoveTargetFolderOnRestore.Checked = value;
                CheckSubmitState();
            }
        }

        public ResourceTypes[] SelectedTypes
        {
            get
            {
                List<ResourceTypes> rts = new List<ResourceTypes>();
                foreach (var obj in chkAllowedTypes.CheckedItems)
                {
                    rts.Add((ResourceTypes)obj);
                }
                return rts.ToArray();
            }
            set
            {
                CheckAll(false);
                foreach (var rt in value)
                {
                    var idx = chkAllowedTypes.Items.IndexOf(rt);
                    if (idx >= 0)
                        chkAllowedTypes.SetItemChecked(0, true);
                }
                CheckSubmitState();
            }
        }

        private void BrowseResourcePath_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.FolderToPackage = picker.ResourceID;
                }
            }
        }

        private void BrowseTargetFilename_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.SaveFile())
            {
                dlg.Filter = Properties.Resources.Filter_Mgp_Files;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.OutputFileName = dlg.FileName;
                }
            }
        }

        private void lnkAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckAll(true);
        }

        private void CheckAll(bool chk)
        {
            for (int i = 0; i < chkAllowedTypes.Items.Count; i++)
            {
                chkAllowedTypes.SetItemChecked(i, chk);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void chkRestorePath_CheckedChanged(object sender, EventArgs e)
        {
            txtRestorePath.Enabled = btnBrowseRestorePath.Enabled = chkRestorePath.Checked;
        }

        void CheckSubmitState()
        {
            btnOK.Enabled =
                txtResourcePath.Text.Length > 0 &&
                txtPackageFilename.Text.Length > 0 &&
                SelectedTypes.Length > 0;
        }

        private void btnBrowseRestorePath_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.FolderToPackage = picker.ResourceID;
                }
            }
        }
    }
}
