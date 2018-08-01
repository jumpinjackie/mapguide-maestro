#region Disclaimer / License

// Copyright (C) 2018, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Mapping;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RtMapInspector
{
    public partial class MapImageDialog : Form
    {
        readonly RuntimeMap _map;

        public MapImageDialog(RuntimeMap map)
        {
            _map = map;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.Size = new Size(_map.DisplayWidth, _map.DisplayHeight);
                var img = Image.FromStream(_map.RenderDynamicOverlay(_map.Selection, "PNG", Color.Blue, 1 | 2 | 4 | 8)); ;
                mapImage.Image = img;
            }
            catch { }
            base.OnLoad(e);
        }
    }
}
