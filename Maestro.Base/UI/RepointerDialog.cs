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
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Base.UI
{
    public partial class RepointerDialog : Form
    {
        private RepointerDialog()
        {
            InitializeComponent();
        }

        private IResourceService _resSvc;

        public RepointerDialog(ResourceIdentifier resId, IResourceService resSvc)
            : this()
        {
            _resSvc = resSvc;
            txtSource.Text = resId.ToString();
            this.ResourceType = resId.ResourceType;

            var dependents = resSvc.EnumerateResourceReferences(resId.ToString());

            lstAffectedResources.DataSource = dependents.ResourceId;
        }

        public ICollection<string> Dependents { get { return (ICollection<string>)lstAffectedResources.DataSource; } }

        public string Source { get { return txtSource.Text; } }

        public string Target { get { return txtTarget.Text; } }

        public ResourceTypes ResourceType
        {
            get;
            private set;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc, this.ResourceType, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtTarget.Text = picker.ResourceID;
                    btnOK.Enabled = !string.IsNullOrEmpty(txtSource.Text) && !string.IsNullOrEmpty(txtTarget.Text);
                }
            }
        }
    }
}
