#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition
{
    /// <summary>
    /// A dialog that allows a user to select a list of symbols to extract from a
    /// SymbolLibrary into image-based Symbol Definition resources
    /// </summary>
    public partial class ExtractSymbolLibraryDialog : Form
    {
        private ExtractSymbolLibraryDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        /// <summary>
        /// Creates a new instance of ExtractSymbolLibraryDialog
        /// </summary>
        /// <param name="conn">The connection</param>
        public ExtractSymbolLibraryDialog(IServerConnection conn)
            : this()
        {
            _conn = conn;
        }

        /// <summary>
        /// Creates a new instance of ExtractSymbolLibraryDialog
        /// </summary>
        /// <param name="conn">The connection</param>
        /// <param name="symbolLibId">The symbol library resource id</param>
        public ExtractSymbolLibraryDialog(IServerConnection conn, string symbolLibId)
            : this(conn)
        {
            txtSymbolLibrary.Text = symbolLibId;
            LoadSymbols(symbolLibId);
            EvaluateCommands();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn, ResourceTypes.SymbolLibrary.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    LoadSymbols(picker.ResourceID);
                    txtSymbolLibrary.Text = picker.ResourceID;
                    EvaluateCommands();
                }
            }
        }

        private void LoadSymbols(string symResId)
        {
            var ds = ImageSymbolConverter.PrepareSymbolDrawingSource(_conn, symResId);
            //Now we should be able to query it via Drawing Service APIs
            var drawSvc = (IDrawingService)_conn.GetService((int)ServiceType.Drawing);

            //Each section in the symbols.dwf represents a symbol
            var sectionList = drawSvc.EnumerateDrawingSections(ds.ResourceID);

            lstSymbols.Items.Clear();

            int idx = 0;
            var imgList = new ImageList();
            imgList.ImageSize = new Size(32, 32);
            var symbols = new List<ListViewItem>();

            foreach (var sect in sectionList.Section)
            {
                var sectResources = drawSvc.EnumerateDrawingSectionResources(ds.ResourceID, sect.Name);

                foreach (var res in sectResources.SectionResource)
                {
                    if (res.Role.ToUpper() == StringConstants.Thumbnail.ToUpper())
                    {
                        using (var rs = drawSvc.GetSectionResource(ds.ResourceID, res.Href))
                        {
                            Image img = Image.FromStream(rs);
                            imgList.Images.Add(img);

                            var item = new ListViewItem(sect.Title);
                            item.ImageIndex = idx;
                            symbols.Add(item);

                            idx++;
                        }
                    }
                }
            }

            lstSymbols.SmallImageList = imgList;
            lstSymbols.LargeImageList = imgList;
            foreach (var sym in symbols)
            {
                lstSymbols.Items.Add(sym);
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtTargetFolder.Text = picker.SelectedFolder;
                }
            }
        }

        private void EvaluateCommands()
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtSymbolLibrary.Text) &&
                            !string.IsNullOrEmpty(txtTargetFolder.Text) &&
                            lstSymbols.SelectedItems.Count > 0 &&
                            _conn.ResourceService.ResourceExists(txtSymbolLibrary.Text);
        }

        /// <summary>
        /// Gets the selected symbol library
        /// </summary>
        public string SymbolLibrary { get { return txtSymbolLibrary.Text; } }

        /// <summary>
        /// Gets the selected target folder
        /// </summary>
        public string TargetFolder { get { return txtTargetFolder.Text; } }

        /// <summary>
        /// Gets the list of symbols to extract
        /// </summary>
        public IEnumerable<string> SelectedSymbols
        {
            get
            {
                foreach (ListViewItem item in lstSymbols.SelectedItems)
                {
                    yield return item.Text;
                }
            }
        }

        private void lstSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateCommands();
        }
    }
}
