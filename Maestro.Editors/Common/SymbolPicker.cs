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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Obj = OSGeo.MapGuide.ObjectModels.Common;
using Maestro.Editors.Generic;
using System.IO;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A dialog to pick symbols from a DWF symbol library
    /// </summary>
    public partial class SymbolPicker : Form
    {
        private SymbolPicker()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;
        private Image _symbolImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolPicker"/> class.
        /// </summary>
        /// <param name="conn"></param>
        public SymbolPicker(IServerConnection conn)
            : this()
        {
            _conn = conn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolPicker"/> class.
        /// </summary>
        /// <param name="symbolLibrary">The symbol library.</param>
        /// <param name="conn">The conn.</param>
        public SymbolPicker(string symbolLibrary, IServerConnection conn)
            : this(conn)
        {
            if (ResourceIdentifier.GetResourceType(symbolLibrary) != OSGeo.MapGuide.MaestroAPI.ResourceTypes.SymbolLibrary)
                throw new ArgumentException(string.Format(Strings.ErrorInvalidSymbolLibraryResourceId, symbolLibrary));

            txtSymbolLibrary.Text = symbolLibrary;
        }

        /// <summary>
        /// Gets the symbol library resource id
        /// </summary>
        public string SymbolLibrary { get { return txtSymbolLibrary.Text; } }

        /// <summary>
        /// Gets the preview image for this symbol
        /// </summary>
        public Image SymbolImage { get { return _symbolImage; } }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSymbolLibrary.Text))
                LoadSymbols(txtSymbolLibrary.Text);
        }

        /// <summary>
        /// Fetches the thumbnail of a symbol in a symbol library
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="symbolLibId"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        internal static Image GetSymbol(IServerConnection conn, string symbolLibId, string symbolName)
        {
            //NOTE: This could be nasty performance-wise if invoked at lot of times in succession
            //But these types of symbols are deprecated anyway, so we can live with it, because people
            //shouldn't be using these anymore (and thus this method by extension)

            var ds = ImageSymbolConverter.PrepareSymbolDrawingSource(conn, symbolLibId);

            //Now we should be able to query it via Drawing Service APIs
            var drawSvc = (IDrawingService)conn.GetService((int)ServiceType.Drawing);

            //Each section in the symbols.dwf represents a symbol
            var sectionList = drawSvc.EnumerateDrawingSections(ds.ResourceID);

            foreach (var sect in sectionList.Section)
            {
                if (sect.Title == symbolName)
                {
                    var sectResources = drawSvc.EnumerateDrawingSectionResources(ds.ResourceID, sect.Name);

                    foreach (var res in sectResources.SectionResource)
                    {
                        if (res.Role.ToUpper() == StringConstants.Thumbnail.ToUpper())
                        {
                            using (var rs = drawSvc.GetSectionResource(ds.ResourceID, res.Href))
                            {
                                return Image.FromStream(rs);
                            }
                        }
                    }
                }
            }
            return null;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void lstSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (lstSymbols.SelectedItems.Count == 1);

            if (btnOK.Enabled)
            {
                var item = lstSymbols.SelectedItems[0];

                //Dispose of old image before setting new one
                if (_symbolImage != null)
                {
                    _symbolImage.Dispose();
                    _symbolImage = null;
                }
                _symbolImage = (Image)lstSymbols.SmallImageList.Images[item.ImageIndex].Clone();
            }
        }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        /// <value>The name of the symbol.</value>
        public string SymbolName
        {
            get
            {
                if (lstSymbols.SelectedItems.Count == 1)
                {
                    return lstSymbols.SelectedItems[0].Text;
                }
                return string.Empty;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn.ResourceService, ResourceTypes.SymbolLibrary, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    LoadSymbols(picker.ResourceID);
                    txtSymbolLibrary.Text = picker.ResourceID;
                }
            }
        }
    }
}
