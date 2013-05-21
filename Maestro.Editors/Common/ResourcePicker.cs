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
using Aga.Controls.Tree;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Security.AccessControl;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Editors.Common;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// A generic dialog for selecting folders or resource documents
    /// </summary>
    public partial class ResourcePicker : Form
    {
        private ResourceTypes[] _resTypes;

        private ResourcePicker()
        {
            InitializeComponent();
            _resTypes = new ResourceTypes[] 
            {
                ResourceTypes.ApplicationDefinition,
                ResourceTypes.DrawingSource,
                ResourceTypes.FeatureSource,
                ResourceTypes.Folder,
                ResourceTypes.LayerDefinition,
                ResourceTypes.LoadProcedure,
                ResourceTypes.MapDefinition,
                ResourceTypes.PrintLayout,
                ResourceTypes.SymbolDefinition,
                ResourceTypes.SymbolLibrary,
                ResourceTypes.WebLayout,
                ResourceTypes.WatermarkDefinition
            };
            cmbResourceFilter.DataSource = _resTypes;
            RepositoryIcons.PopulateImageList(resImageList);
            RepositoryIcons.PopulateImageList(folderImageList);
        }

        private IResourceService _resSvc;

        private bool _resourceMode = false;

        /// <summary>
        /// Constructs a new instance. Use this overload to select any resource type. If only
        /// folder selection is desired, set <see cref="SelectFoldersOnly"/> to true before
        /// showing the dialog
        /// </summary>
        /// <param name="resSvc">The res SVC.</param>
        /// <param name="mode">The mode.</param>
        public ResourcePicker(IResourceService resSvc, ResourcePickerMode mode)
            : this()
        {
            _resSvc = resSvc;
            repoView.Init(resSvc, true, false);
            repoView.ItemSelected += OnFolderSelected;
            this.UseFilter = true;
            this.Mode = mode;
            SetStartingPoint(LastSelectedFolder.FolderId);
        }

        void OnFolderSelected(object sender, EventArgs e)
        {
            UpdateDocumentList();
        }

        /// <summary>
        /// Sets the starting point.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <remarks>If the specified folder does not exist, it will fallback to Library://</remarks>
        public void SetStartingPoint(string folderId)
        {
            if (string.IsNullOrEmpty(folderId))
                return;
            if (!ResourceIdentifier.IsFolderResource(folderId))
                throw new ArgumentException(string.Format(Strings.NotAFolder, folderId));
            
            // Library:// will *always* exist, so fallback to this if given folder doesn't check out
            if (!_resSvc.ResourceExists(folderId))
                folderId = StringConstants.RootIdentifier;

            this.ActiveControl = repoView;
            repoView.NavigateTo(folderId);
            this.SelectedFolder = folderId;

            //HACK: Navigating to the specified folder takes away the focus to the 
            //name field
            this.ActiveControl = txtName;
        }

        /// <summary>
        /// Gets the selected folder.
        /// </summary>
        public string SelectedFolder { get; private set; }

        private ResourcePickerMode _mode = ResourcePickerMode.OpenResource;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public ResourcePickerMode Mode
        {
            get { return _mode; }
            private set
            {
                _mode = value;
                switch (_mode)
                {
                    case ResourcePickerMode.OpenFolder:
                        {
                            this.Text = Strings.SelectFolder;
                            this.SelectFoldersOnly = true;
                        } 
                        break;
                    case ResourcePickerMode.SaveResource:
                        {
                            this.Text = Strings.SaveResource;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Constructs a new instance. Use this overload to select only resources of a specific type.
        /// You cannot select folders in this mode. Attempting to set <see cref="SelectFoldersOnly"/> to
        /// true will throw an <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="resSvc">The res SVC.</param>
        /// <param name="resFilter">The res filter.</param>
        /// <param name="mode">The mode.</param>
        public ResourcePicker(IResourceService resSvc, ResourceTypes resFilter, ResourcePickerMode mode)
            : this(resSvc, mode)
        {
            if (mode == ResourcePickerMode.OpenFolder)
                throw new InvalidOperationException(string.Format(Strings.ModeNotAllowed, mode));

            this.Filter = resFilter;
            this.UseFilter = true;

            _resourceMode = true;
            cmbResourceFilter.Enabled = false;
        }

        /// <summary>
        /// Gets or sets the resource filter. If a filter value is specified, browsing
        /// is locked to that particular resource type, otherwise all resource type can be
        /// selected
        /// </summary>
        public ResourceTypes Filter
        {
            get { return (ResourceTypes)cmbResourceFilter.SelectedItem; }
            set
            {
                if (Array.IndexOf<ResourceTypes>(_resTypes, value) < 0)
                    throw new InvalidOperationException("Cannot use specified resource type as filter: " + value); //LOCALIZE

                cmbResourceFilter.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to use a resource filter. If set to false, when selecting a folder
        /// all resource types are returned, otherwise only children of the specified type are returned
        /// </summary>
        internal bool UseFilter
        {
            get { return cmbResourceFilter.Visible; }
            set 
            {
                if (value && this.SelectFoldersOnly)
                    throw new InvalidOperationException("Cannot specify a filter when SelectFoldersOnly is true"); //LOCALIZE
                cmbResourceFilter.Visible = value; lblFilter.Visible = value; 
            }
        }
        
        /// <summary>
        /// Gets or sets whether to select folders only. If true, the document view is disabled and 
        /// <see cref="UseFilter"/> is set to false
        /// </summary>
        private bool SelectFoldersOnly
        {
            get { return splitContainer1.Panel2Collapsed; }
            set 
            {
                if (_resourceMode && value)
                    throw new InvalidOperationException("Cannot specify to select folders when dialog is initialized with a resource filter"); //LOCALIZE

                if (value)
                    this.UseFilter = false;

                splitContainer1.Panel2Collapsed = value;
                resIdComponentPanel.Visible = !value;
                if (value)
                    txtResourceId.Text = string.Empty;
            }
        }

        /// <summary>
        /// Gets the resource id of the selected item
        /// </summary>
        public string ResourceID
        {
            get { return txtResourceId.Text; }
        }

        private void UpdateResourceId()
        {
            btnOK.Enabled = false;
            this.SelectedFolder = txtFolder.Text;
            if (this.SelectFoldersOnly)
            {
                txtResourceId.Text = txtFolder.Text;
                if (!string.IsNullOrEmpty(txtFolder.Text) && ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                {
                    btnOK.Enabled = true;
                }
            }
            else
            {
                txtResourceId.Text = txtFolder.Text + txtName.Text + "." + cmbResourceFilter.SelectedItem.ToString();
                if (!ResourceIdentifier.IsFolderResource(txtResourceId.Text) && !string.IsNullOrEmpty(txtName.Text))
                {
                    btnOK.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_mode == ResourcePickerMode.OpenResource)
            {
                if (!_resSvc.ResourceExists(txtResourceId.Text))
                {
                    MessageBox.Show(Strings.ResourceDoesntExist);
                    return;
                }
            }
            else if (_mode == ResourcePickerMode.SaveResource)
            {
                if (ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                {
                    MessageBox.Show(Strings.InvalidResourceIdFolder);
                    return;
                }
                else
                {
                    if (!ResourceIdentifier.Validate(txtResourceId.Text))
                    {
                        MessageBox.Show(Strings.InvalidResourceId);
                        return;
                    }
                    else
                    {
                        if (ResourceIdentifier.GetResourceType(txtResourceId.Text) != (ResourceTypes)cmbResourceFilter.SelectedItem)
                        {
                            MessageBox.Show(Strings.InvalidResourceIdNotSpecifiedType);
                            return;
                        }
                    }

                    if (_resSvc.ResourceExists(txtResourceId.Text))
                    {
                        if (MessageBox.Show(Strings.OverwriteResource, Strings.SaveResource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;
                    }
                }
            }
            if (ResourceIdentifier.IsFolderResource(txtResourceId.Text))
                LastSelectedFolder.FolderId = txtResourceId.Text;
            else
                LastSelectedFolder.FolderId = (txtResourceId.Text != StringConstants.RootIdentifier) ? ResourceIdentifier.GetParentFolder(txtResourceId.Text) : StringConstants.RootIdentifier;
            this.DialogResult = DialogResult.OK;
        }

        private void UpdateDocumentList()
        {
            IRepositoryItem folder = repoView.SelectedItem;
            if (folder != null)
            {
                txtFolder.Text = folder.ResourceId;

                if (!this.SelectFoldersOnly)
                {
                    ResourceList list = null;
                    if (!this.UseFilter)
                        list = _resSvc.GetRepositoryResources(folder.ResourceId, 1);
                    else
                        list = _resSvc.GetRepositoryResources(folder.ResourceId, this.Filter.ToString(), 1);

                    PopulateDocumentList(list);
                }
            }
        }

        private void PopulateDocumentList(ResourceList list)
        {
            lstResources.Clear();
            SortedList<string, ResourceListResourceDocument> items = new SortedList<string, ResourceListResourceDocument>();
            foreach (var item in list.Items)
            {
                var doc = item as ResourceListResourceDocument;
                if (doc != null)
                {
                    string sortKey = doc.Name + "." + doc.ResourceType;
                    items.Add(sortKey, doc);
                }
            }
            foreach (var doc in items.Values)
            {
                var li = new ListViewItem(doc.Name);
                li.Tag = doc;

                try
                {
                    var rt = ResourceIdentifier.GetResourceType(doc.ResourceId);
                    li.ImageIndex = RepositoryIcons.GetImageIndexForResourceType(rt);
                }
                catch
                {
                    li.ImageIndex = RepositoryIcons.RES_UNKNOWN;
                }

                lstResources.Items.Add(li);
            }
        }

        private void lstResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResources.SelectedItems.Count == 1)
            {
                var item = lstResources.SelectedItems[0];
                var doc = item.Tag as ResourceListResourceDocument;
                if (doc != null)
                {
                    txtName.Text = ResourceIdentifier.GetName(doc.ResourceId);
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
        }

        private void cmbResourceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResourceId();
            UpdateDocumentList();
        }

        private void lstResources_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = lstResources.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                var doc = item.Tag as ResourceListResourceDocument;
                if (doc != null)
                {
                    txtName.Text = ResourceIdentifier.GetName(doc.ResourceId);
                    btnOK.PerformClick();
                }
            }
        }
    }

    /// <summary>
    /// Defines the various modes this resource picker can be in
    /// </summary>
    public enum ResourcePickerMode
    {
        /// <summary>
        /// 
        /// </summary>
        OpenResource,
        /// <summary>
        /// 
        /// </summary>
        SaveResource,
        /// <summary>
        /// 
        /// </summary>
        OpenFolder
    }
}
