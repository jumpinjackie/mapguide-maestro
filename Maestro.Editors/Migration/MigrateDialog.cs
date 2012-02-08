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

namespace Maestro.Editors.Migration
{
    /// <summary>
    /// A dialog to specify resource migration settings
    /// </summary>
    public partial class MigrateDialog : Form
    {
        private MigrateDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _source;
        private IServerConnection _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrateDialog"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public MigrateDialog(IServerConnection source, IServerConnection target)
            : this()
        {
            _source = source;
            _target = target;
        }

        /// <summary>
        /// Gets or sets the resource ID.
        /// </summary>
        /// <value>The resource ID.</value>
        public string ResourceID
        {
            get { return txtResourceId.Text; }
            set { txtResourceId.Text = value; }
        }

        /// <summary>
        /// Gets or sets the dependent resources.
        /// </summary>
        /// <value>The dependent resources.</value>
        public string[] DependentResources
        {
            get
            {
                List<string> resIds = new List<string>();
                foreach (var item in chkDependencies.CheckedItems)
                {
                    resIds.Add(item.ToString());
                }
                return resIds.ToArray();
            }
            set
            {
                CheckNone();

                foreach (var item in value)
                {
                    var idx = chkDependencies.Items.IndexOf(item);
                    if (idx >= 0)
                        chkDependencies.SetItemChecked(idx, true);
                }
            }
        }

        private void CheckNone()
        {
            for (int i = 0; i < chkDependencies.Items.Count; i++)
            {
                chkDependencies.SetItemChecked(i, false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [overwrite existing].
        /// </summary>
        /// <value><c>true</c> if [overwrite existing]; otherwise, <c>false</c>.</value>
        public bool OverwriteExisting
        {
            get { return chkOverwrite.Checked; }
            set { chkOverwrite.Checked = value; }
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
            using (var picker = new ResourcePicker(_source.ResourceService, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }

        private void txtResourceId_TextChanged(object sender, EventArgs e)
        {
            chkDependencies.Items.Clear();
            using (new WaitCursor(this))
            {
                Dictionary<string, string> resIds = new Dictionary<string, string>();
                List<string> dependentIds = GetReverseReferences(txtResourceId.Text);
                BuildFullDependencyList(resIds, dependentIds);

                foreach (var resId in resIds.Keys)
                {
                    chkDependencies.Items.Add(resId, false);
                }

                CheckAll();
            }
        }

        private void BuildFullDependencyList(Dictionary<string, string> resIds, IEnumerable<string> resourceIds)
        {
            foreach (var id in resourceIds)
            {
                resIds[id] = id;

                List<string> dependentIds = GetReverseReferences(id);

                BuildFullDependencyList(resIds, dependentIds);
            }
        }

        private List<string> GetReverseReferences(string id)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            using (var ms = _source.ResourceService.GetResourceXmlData(id))
            {
                doc.Load(ms);
            }

            List<KeyValuePair<System.Xml.XmlNode, string>> refs = Utility.GetResourceIdPointers(doc);
            List<string> dependentIds = new List<string>();
            foreach (KeyValuePair<System.Xml.XmlNode, string> s in refs)
                if (!dependentIds.Contains(s.Value))
                    dependentIds.Add(s.Value);
            return dependentIds;
        }

        private void CheckNonExistentItems()
        {
            //Now check those that do not exist on the target connection
            for (int i = 0; i < chkDependencies.Items.Count; i++)
            {
                string resId = (string)chkDependencies.Items[i];
                if (!_target.ResourceService.ResourceExists(resId))
                {
                    chkDependencies.SetItemChecked(i, true);
                }
                else
                {
                    chkDependencies.SetItemChecked(i, false);
                }
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            CheckAll();
        }

        private void CheckAll()
        {
            for (int i = 0; i < chkDependencies.Items.Count; i++)
            {
                chkDependencies.SetItemChecked(i, true);
            }
        }

        private void btnCheckNone_Click(object sender, EventArgs e)
        {
            CheckNone();
        }

        private void btnCheckRequired_Click(object sender, EventArgs e)
        {
            using (new WaitCursor(this))
            {
                CheckNonExistentItems();
            }
        }
    }
}
