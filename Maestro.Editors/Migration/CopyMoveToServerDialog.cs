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

namespace Maestro.Editors.Migration
{
    /// <summary>
    /// A dialog to specify options for copying/moving resources to another connection
    /// </summary>
    public partial class CopyMoveToServerDialog : Form
    {
        private CopyMoveToServerDialog()
        {
            InitializeComponent();
            cmbAction.DataSource = Enum.GetValues(typeof(MigrationAction));
        }

        private IServerConnection _source;
        private IServerConnection _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyMoveToServerDialog"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public CopyMoveToServerDialog(IServerConnection source, IServerConnection target)
            : this()
        {
            _source = source;
            _target = target;

            cmbAction.SelectedItem = _lastAction;
            chkOverwrite.Checked = _overwrite;
        }

        private void lstResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateCommandState();
        }

        //Used to persist state between dialog invocations
        static string _lastSourceFolder;
        static string _lastTargetFolder;
        static MigrationAction _lastAction;
        static bool _overwrite = false;

        private void btnAddResource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_source.ResourceService, ResourcePickerMode.OpenResource))
            {
                if (!string.IsNullOrEmpty(_lastSourceFolder))
                    picker.SetStartingPoint(_lastSourceFolder);

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    if (!lstResources.Items.Contains(picker.ResourceID))
                        lstResources.Items.Add(picker.ResourceID);

                    _lastSourceFolder = picker.SelectedFolder;

                    EvaluateCommandState();
                }
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_source.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (!string.IsNullOrEmpty(_lastSourceFolder))
                    picker.SetStartingPoint(_lastSourceFolder);

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var folderId = picker.ResourceID;
                    var list = _source.ResourceService.GetRepositoryResources(folderId);

                    foreach (var item in list.Children)
                    {
                        if (!item.IsFolder && !lstResources.Items.Contains(item.ResourceId))
                            lstResources.Items.Add(item.ResourceId);
                    }

                    _lastSourceFolder = picker.SelectedFolder;

                    EvaluateCommandState();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var items = lstResources.SelectedItems;
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    lstResources.Items.Remove(items[i]);
                    EvaluateCommandState();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Gets or sets the target folder.
        /// </summary>
        /// <value>The target folder.</value>
        public string TargetFolder
        {
            get { return txtTargetFolder.Text; }
            set { txtTargetFolder.Text = value; }
        }

        /// <summary>
        /// Gets or sets the source resource ids.
        /// </summary>
        /// <value>The source resource ids.</value>
        public string[] SourceResourceIds
        {
            get
            {
                List<string> resIds = new List<string>();
                foreach (var item in lstResources.Items)
                {
                    resIds.Add(item.ToString());
                }
                return resIds.ToArray();
            }
            set
            {
                lstResources.Items.Clear();
                foreach (var resId in value)
                {
                    lstResources.Items.Add(resId);
                }
                EvaluateCommandState();
            }
        }

        /// <summary>
        /// Gets or sets the selected action.
        /// </summary>
        /// <value>The selected action.</value>
        public MigrationAction SelectedAction
        {
            get { return (MigrationAction)cmbAction.SelectedItem; }
            set { cmbAction.SelectedItem = value ; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [overwrite resources].
        /// </summary>
        /// <value><c>true</c> if [overwrite resources]; otherwise, <c>false</c>.</value>
        public bool OverwriteResources
        {
            get { return chkOverwrite.Checked; }
            set { chkOverwrite.Checked = value; }
        }

        private void btnBrowseTarget_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_target.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (!string.IsNullOrEmpty(_lastTargetFolder))
                    picker.SetStartingPoint(_lastTargetFolder);

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    _lastTargetFolder = picker.SelectedFolder;
                    txtTargetFolder.Text = picker.ResourceID;
                    EvaluateCommandState();
                }
            }
        }

        private void EvaluateCommandState()
        {
            btnRemove.Enabled = (lstResources.SelectedItem != null) || (lstResources.SelectedItems != null && lstResources.SelectedItems.Count > 0);
            btnOK.Enabled = (lstResources.Items.Count > 0) && !string.IsNullOrEmpty(txtTargetFolder.Text);
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lastAction = (MigrationAction)cmbAction.SelectedItem;
        }

        private void chkOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            _overwrite = chkOverwrite.Checked;
        }
    }

    /// <summary>
    /// Defines the valid migration actions
    /// </summary>
    public enum MigrationAction
    {
        /// <summary>
        /// 
        /// </summary>
        Copy,
        /// <summary>
        /// 
        /// </summary>
        Move
    }
}
