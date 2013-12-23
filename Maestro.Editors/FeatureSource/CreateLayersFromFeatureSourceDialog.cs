#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource
{
    /// <summary>
    /// A dialog that allows for the easy creation of Layer Definitions with default styles from one or more selected
    /// feature classes in the given feature source
    /// </summary>
    public partial class CreateLayersFromFeatureSourceDialog : Form
    {
        private CreateLayersFromFeatureSourceDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        /// <summary>
        /// Initializes a new instance of the CreateLayersFromFeatureSourceDialog class
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="featureSource"></param>
        public CreateLayersFromFeatureSourceDialog(IServerConnection conn, string featureSource)
            : this()
        {
            _conn = conn;
            txtFeatureSource.Text = featureSource;
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Form.Load event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            LoadFeatureClassesAsync();
        }

        private void LoadFeatureClassesAsync()
        {
            btnCreate.Enabled = false;

            string fsId = txtFeatureSource.Text;
            BusyWaitDialog.Run(null, () =>
            {
                return _conn.FeatureService.GetClassNames(fsId, null);
            }, (result, ex) =>
            {
                if (ex != null)
                {
                    ErrorDialog.Show(ex);
                }
                else
                {
                    lstFeatureClasses.DataSource = result;
                    EvalButtonState();
                }
            });
        }

        private void EvalButtonState()
        {
            btnCreate.Enabled = !string.IsNullOrEmpty(txtFeatureSource.Text) &&
                                !string.IsNullOrEmpty(txtCreateTargetFolder.Text) &&
                                lstFeatureClasses.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Gets the selected Feature Source
        /// </summary>
        public string FeatureSource
        {
            get { return txtFeatureSource.Text; }
        }

        /// <summary>
        /// Gets the selected target folder where default Layer Definitions will be created
        /// </summary>
        public string TargetFolder
        {
            get { return txtCreateTargetFolder.Text; }
        }
        
        /// <summary>
        /// Gets the selected feature classes to create default Layer Definition documents for
        /// </summary>
        public string[] FeatureClasses
        {
            get
            {
                var items = new List<string>();
                var selected = lstFeatureClasses.SelectedItems;
                for (int i = 0; i < selected.Count; i++)
                {
                    items.Add(selected[i].ToString());
                }
                return items.ToArray();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnFeatureSource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.FeatureSource, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFeatureSource.Text = picker.ResourceID;
                    LoadFeatureClassesAsync();
                }
            }
        }

        private void btnCreateTarget_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtCreateTargetFolder.Text = picker.ResourceID;
                    EvalButtonState();
                }
            }
        }

        private void lstFeatureClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvalButtonState();
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < lstFeatureClasses.Items.Count; i++)
            {
                lstFeatureClasses.SetSelected(i, true);
            }
            EvalButtonState();
        }

        private void lnkClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < lstFeatureClasses.Items.Count; i++)
            {
                lstFeatureClasses.SetSelected(i, false);
            }
            EvalButtonState();
        }
    }
}
