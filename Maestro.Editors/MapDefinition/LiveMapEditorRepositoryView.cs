#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.Common;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// A Live Map Editor component that provides a view into the currently edited map's resource repository
    /// </summary>
    public partial class LiveMapEditorRepositoryView : UserControl
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public LiveMapEditorRepositoryView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes this view
        /// </summary>
        /// <param name="resSvc"></param>
        /// <param name="filteredTypes"></param>
        public void Init(IResourceService resSvc, ResourceTypes[] filteredTypes)
        {
            repoView.Init(resSvc, false);
            repoView.ClearResourceTypeFilters();
            if (filteredTypes != null)
            {
                foreach (var rt in filteredTypes)
                {
                    repoView.AddResourceTypeFilter(rt);
                }
            }
        }

        private void btnAddToMap_Click(object sender, EventArgs e)
        {
            var h = this.RequestAddToMap;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var h = this.RequestEdit;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the currently selected item is requested to be added to the currently edited map
        /// </summary>
        public event EventHandler RequestAddToMap;

        /// <summary>
        /// Raised when the currently selected item is requested to be edited
        /// </summary>
        public event EventHandler RequestEdit;

        /// <summary>
        /// Gets the selected item in the repository
        /// </summary>
        public IRepositoryItem SelectedItem
        {
            get { return repoView.SelectedItem; }
        }

        private void repoView_ItemSelected(object sender, EventArgs e)
        {
            var item = repoView.SelectedItem;
            var condition = (item != null && !item.IsFolder && (item.ResourceType == ResourceTypes.LayerDefinition));
            btnAddToMap.Enabled = btnEdit.Enabled = condition;
            btnRefresh.Enabled = !condition;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var item = repoView.SelectedItem;
            if (item.IsFolder)
            {
                repoView.RefreshModel(item.ResourceId);
            }
            else 
            {
                var parent = ResourceIdentifier.GetParentFolder(item.ResourceId);
                repoView.RefreshModel(parent);
            }
        }
    }
}
