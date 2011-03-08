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

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolPicker"/> class.
        /// </summary>
        /// <param name="symbolLibrary">The symbol library.</param>
        /// <param name="conn">The conn.</param>
        public SymbolPicker(string symbolLibrary, IServerConnection conn)
            : this()
        {
            if (ResourceIdentifier.GetResourceType(symbolLibrary) != OSGeo.MapGuide.MaestroAPI.ResourceTypes.SymbolLibrary)
                throw new ArgumentException("Not a valid symbol library resource identifier: " + symbolLibrary); //LOCALIZE

            _conn = conn;
            txtSymbolLibrary.Text = symbolLibrary;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            //Extract the symbols.dwf resource data and copy to a session based drawing source
            var dwf = _conn.ResourceService.GetResourceData(txtSymbolLibrary.Text, "symbols.dwf");
            var ds = OSGeo.MapGuide.ObjectModels.ObjectFactory.CreateDrawingSource(_conn);
            ds.SourceName = "symbols.dwf";
            ds.ResourceID = "Session:" + _conn.SessionID + "//" + Guid.NewGuid() + ".DrawingSource";
            _conn.ResourceService.SaveResource(ds);
            _conn.ResourceService.SetResourceData(ds.ResourceID, "symbols.dwf", OSGeo.MapGuide.ObjectModels.Common.ResourceDataType.File, dwf);

            //Now we should be able to query it via Drawing Service APIs
            var drawSvc = (IDrawingService)_conn.GetService((int)ServiceType.Drawing);

            //Each section in the symbols.dwf represents a symbol
            var sectionList = drawSvc.EnumerateDrawingSections(ds.ResourceID);

            int idx = 0;
            var imgList = new ImageList();
            imgList.ImageSize = new Size(32, 32);
            var symbols = new List<ListViewItem>();

            foreach (var sect in sectionList.Section)
            {
                var sectResources = drawSvc.EnumerateDrawingSectionResources(ds.ResourceID, sect.Name);

                foreach (var res in sectResources.SectionResource)
                {
                    if (res.Role.ToUpper() == "THUMBNAIL")
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
    }
}
