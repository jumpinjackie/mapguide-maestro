#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace MgCooker
{
    public partial class MapExtentsDialog : Form
    {
        private MapExtentsDialog()
        {
            InitializeComponent();
        }

        private RuntimeMap _rtMap;

        public MapExtentsDialog(RuntimeMap rtMap)
            : this()
        {
            _rtMap = rtMap;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            mapViewer.LoadMap(_rtMap);
        }

        private double _minx;
        private double _miny;
        private double _maxx;
        private double _maxy;

        public IEnvelope GetEnvelope() => ObjectFactory.CreateEnvelope(_minx, _miny, _maxx, _maxy);

        private void mapViewer_MapRefreshed(object sender, EventArgs e)
        {
            mapViewer.GetViewExtent(out _minx, out _miny, out _maxx, out _maxy);

            txtMaxX.Text = _maxx.ToString(CultureInfo.InvariantCulture);
            txtMaxY.Text = _maxy.ToString(CultureInfo.InvariantCulture);
            txtMinX.Text = _minx.ToString(CultureInfo.InvariantCulture);
            txtMinY.Text = _miny.ToString(CultureInfo.InvariantCulture);

            btnAccept.Enabled = true;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
