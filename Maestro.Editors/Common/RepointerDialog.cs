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

#endregion Disclaimer / License

using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A dialog that prompts the user to re-point the dependent resources of a given resource
    /// to a new resource of the user's choosing.
    ///
    /// This dialog does not perform the re-pointer logic. This dialog simply captures the parameters
    /// required for a re-pointer operation.
    /// </summary>
    public partial class RepointerDialog : Form
    {
        private RepointerDialog()
        {
            InitializeComponent();
        }

        private readonly IServerConnection _conn;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resId">The given resource, whose dependencies we want to re-point</param>
        /// <param name="conn">The server connection</param>
        public RepointerDialog(ResourceIdentifier resId, IServerConnection conn)
            : this()
        {
            _conn = conn;
            txtSource.Text = resId.ToString();
            this.ResourceType = resId.ResourceType;

            var dependents = conn.ResourceService.EnumerateResourceReferences(resId.ToString());

            lstAffectedResources.DataSource = dependents.ResourceId;
        }

        /// <summary>
        /// Returns a list of resource ids which are the dependent resources of the specified resource
        /// </summary>
        public ICollection<string> Dependents => (ICollection<string>)lstAffectedResources.DataSource;

        /// <summary>
        /// Gets the specified resource id
        /// </summary>
        public string Source => txtSource.Text;

        /// <summary>
        /// Gets the target resource id we want to re-point
        /// </summary>
        public string Target => txtTarget.Text;

        internal string ResourceType { get; }

        private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn, this.ResourceType, ResourcePickerMode.OpenResource))
            {
                if (string.IsNullOrEmpty(LastSelectedFolder.FolderId))
                    picker.SetStartingPoint(ResourceIdentifier.GetParentFolder(this.Source));
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtTarget.Text = picker.ResourceID;
                    btnOK.Enabled = !string.IsNullOrEmpty(txtSource.Text) && !string.IsNullOrEmpty(txtTarget.Text) &&
                                    lstAffectedResources.Items.Count > 0 && txtSource.Text != txtTarget.Text;
                }
            }
        }
    }
}