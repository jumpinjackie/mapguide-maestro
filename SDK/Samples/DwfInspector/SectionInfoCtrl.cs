#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.IO;

namespace DwfInspector
{
    /// <summary>
    /// This control shows more information about the specified DWF section
    /// </summary>
    public partial class SectionInfoCtrl : UserControl
    {
        private SectionInfoCtrl()
        {
            InitializeComponent();
        }

        private IDrawingService _drawSvc;
        private string _drawingSourceId;
        private OSGeo.MapGuide.ObjectModels.Common.DrawingSectionListSection _section;

        public SectionInfoCtrl(IDrawingService drawSvc, string drawingSourceId, OSGeo.MapGuide.ObjectModels.Common.DrawingSectionListSection section)
            : this()
        {
            _drawSvc = drawSvc;
            _drawingSourceId = drawingSourceId;
            _section = section;
        }

        protected override void OnLoad(EventArgs e)
        {
            //Call the EnumerateDrawingSectionResources API of Drawing Service and bind the result to the grid view
            var list = _drawSvc.EnumerateDrawingSectionResources(_drawingSourceId, _section.Name);
            grdResources.DataSource = list.SectionResource;
        }

        private void grdResources_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = grdResources.Rows[e.RowIndex];

            //Because we bound a strongly-typed list to the grid view, each DataGridViewRow has a DataBoundItem property
            //which is the indivdual object from the list we bound to
            var res = row.DataBoundItem as OSGeo.MapGuide.ObjectModels.Common.DrawingSectionResourceListSectionResource;
            pictureBox1.Image = null;
            txtXml.Text = string.Empty;

            //Depending on role, show a preview of the item
            if (res != null)
            {
                //These are image streams. We can convert these to System.Drawing.Image objects for display
                //in a System.Windows.Forms.PictureBox
                if (res.Role == "thumbnail" || res.Role == "preview")
                {
                    using (var stream = _drawSvc.GetSectionResource(_drawingSourceId, res.Href))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                }
                else if (res.Role == "descriptor" || res.Role == "AutoCAD Viewport Data") 
                {
                    //These are plain-text content, which we can house in a System.IO.StreamReader that can 
                    //get the text content.
                    using (var stream = _drawSvc.GetSectionResource(_drawingSourceId, res.Href))
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            txtXml.Text = sr.ReadToEnd();
                        }
                    }
                }
                else //We have no idea how to preview these at the moment.
                {
                    txtXml.Text = "This resource cannot be previewed";
                }
            }
        }
    }
}
