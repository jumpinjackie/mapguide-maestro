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
using OSGeo.MapGuide.MaestroAPI.Services;
using Aga.Controls.Tree;
using OSGeo.MapGuide.ObjectModels.Common;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A dialog to browse and select unmanaged resources
    /// </summary>
    public partial class UnmanagedFileBrowser : Form
    {
        abstract class ModelBase<T>
        {
            public ModelBase(T item) { this.Tag = item; }

            public T Tag { get; private set; }
        }

        class FolderModel : ModelBase<UnmanagedDataListUnmanagedDataFolder>
        {
            public FolderModel(UnmanagedDataListUnmanagedDataFolder folder) 
                : base(folder)
            {
                this.Name = folder.FolderName;
                this.HasChildren = (folder.NumberOfFolders > 0);
            }

            public string Name { get; private set; }

            public bool HasChildren { get; private set; }

            public Image Icon { get { return Properties.Resources.folder_horizontal; } }
        }

        class FolderTreeModel : ITreeModel
        {
            private IResourceService _resSvc;

            public FolderTreeModel(IResourceService resSvc)
            {
                _resSvc = resSvc;
            }

            public System.Collections.IEnumerable GetChildren(TreePath treePath)
            {
                if (treePath.IsEmpty())
                {
                    var list = _resSvc.EnumerateUnmanagedData(null, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Folders);
                    foreach (var item in list.Items)
                    {
                        if (typeof(UnmanagedDataListUnmanagedDataFolder).IsAssignableFrom(item.GetType()))
                        {
                            var folder = (UnmanagedDataListUnmanagedDataFolder)item;

                            yield return new FolderModel(folder);
                        }
                    }
                }
                else 
                {
                    var mdl = treePath.LastNode as FolderModel;
                    if (mdl != null)
                    {
                        var folder = mdl.Tag;
                        if (folder.NumberOfFolders > 0)
                        {
                            var list = _resSvc.EnumerateUnmanagedData(folder.UnmanagedDataId, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Folders);
                            foreach (var item in list.Items)
                            {
                                if (typeof(UnmanagedDataListUnmanagedDataFolder).IsAssignableFrom(item.GetType()))
                                {
                                    var fl = (UnmanagedDataListUnmanagedDataFolder)item;

                                    yield return new FolderModel(fl);
                                }
                            }
                        }
                    }
                }
            }

            public bool IsLeaf(TreePath treePath)
            {
                var mdl = treePath.LastNode as FolderModel;
                if (mdl != null)
                    return mdl.Tag.NumberOfFolders == 0;
                else
                    return true;
            }

            public event EventHandler<TreeModelEventArgs> NodesChanged;

            public event EventHandler<TreeModelEventArgs> NodesInserted;

            public event EventHandler<TreeModelEventArgs> NodesRemoved;

            public event EventHandler<TreePathEventArgs> StructureChanged;
        }

        private bool _selectFoldersOnly;

        /// <summary>
        /// Gets or sets a value indicating whether [select folders only].
        /// </summary>
        /// <value><c>true</c> if [select folders only]; otherwise, <c>false</c>.</value>
        public bool SelectFoldersOnly
        {
            get { return _selectFoldersOnly; }
            set
            {
                _selectFoldersOnly = value;
                splitContainer1.Panel2Collapsed = value;
            }
        }

        private UnmanagedFileBrowser()
        {
            InitializeComponent();
        }

        private IResourceService _resSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnmanagedFileBrowser"/> class.
        /// </summary>
        /// <param name="resSvc">The res SVC.</param>
        public UnmanagedFileBrowser(IResourceService resSvc)
            : this()
        {
            _resSvc = resSvc;
            trvFolders.Model = new FolderTreeModel(_resSvc);
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public string SelectedItem
        {
            get 
            {
                var path = txtItem.Text;
                var leftpart = path.Substring(0, path.IndexOf("]"));
                var rightpart = path.Substring(path.IndexOf("]") + 1);
                return "%MG_DATA_PATH_ALIAS" + leftpart + "]%" + rightpart;
            }
        }

        private void trvFolders_SelectionChanged(object sender, EventArgs e)
        {
            if (trvFolders.SelectedNode != null)
            {
                var mdl = trvFolders.SelectedNode.Tag as FolderModel;
                if (mdl != null)
                {
                    if (this.SelectFoldersOnly)
                    {
                        txtItem.Text = mdl.Name;
                        return;
                    }
                    else
                    {
                        //TODO: file filter
                        var list = _resSvc.EnumerateUnmanagedData(mdl.Tag.UnmanagedDataId, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Files);
                        PopulateFileList(list);
                    }
                }
            }
        }

        private void PopulateFileList(UnmanagedDataList list)
        {
            lstResources.Items.Clear();
            foreach (var item in list.Items)
            {
                if (typeof(UnmanagedDataListUnmanagedDataFile).IsAssignableFrom(item.GetType()))
                {
                    var file = (UnmanagedDataListUnmanagedDataFile)item;
                    var li = new ListViewItem();
                    li.Name = file.UnmanagedDataId;
                    li.Text = file.FileName;
                    li.ImageIndex = GetImageIndex(li.Text);

                    lstResources.Items.Add(li);
                }
            }
        }

        private int GetImageIndex(string fileName)
        {
            string ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
            switch (ext.ToUpper())
            {
                case "EXE":
                    return IDX_FILE_EXE;
                case "DOC":
                case "DOCX":
                    return IDX_FILE_DOC;
                case "MDB":
                case "ACCDB":
                    return IDX_FILE_MDB;
                case "XLS":
                case "XLSX":
                    return IDX_FILE_XLS;
                case "CSV":
                    return IDX_FILE_CSV;
                case "MOV":
                case "MPG":
                case "AVI":
                case "MP4":
                case "WMV":
                case "ASF":
                case "FLV":
                    return IDX_FILE_MOVIE;
                case "HTM":
                case "HTML":
                    return IDX_FILE_HTML;
                case "PNG":
                case "JPG":
                case "GIF":
                case "ICO":
                case "BMP":
                case "TGA":
                    return IDX_FILE_IMAGE;
                case "PDF":
                    return IDX_FILE_PDF;
                case "PHP":
                    return IDX_FILE_PHP;
                case "PPT":
                case "PPTX":
                    return IDX_FILE_PPT;
                case "TXT":
                    return IDX_FILE_TXT;
                case "ZIP":
                case "RAR":
                case "ACE":
                case "7Z":
                case "GZ":
                case "TAR":
                case "BZ2":
                    return IDX_FILE_ARCHIVE;
                case "XML":
                    return IDX_FILE_XML;
                default:
                    return IDX_FILE_UNKNOWN;
            }
        }

        const int IDX_FILE_UNKNOWN = 0;
        const int IDX_FILE_EXE = 1;
        const int IDX_FILE_DOC = 2;
        const int IDX_FILE_MDB = 3;
        const int IDX_FILE_XLS = 4;
        const int IDX_FILE_CSV = 5;
        const int IDX_FILE_MOVIE = 6;
        const int IDX_FILE_HTML = 7;
        const int IDX_FILE_IMAGE = 8;
        const int IDX_FILE_PDF = 9;
        const int IDX_FILE_PHP = 10;
        const int IDX_FILE_PPT = 11;
        const int IDX_FILE_TXT = 12;
        const int IDX_FILE_ARCHIVE = 13;
        const int IDX_FILE_XML = 14;

        private void lstResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResources.SelectedItems.Count == 1)
            {
                txtItem.Text = lstResources.SelectedItems[0].Name;
            }
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtItem.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
