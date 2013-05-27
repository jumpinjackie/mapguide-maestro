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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System.Globalization;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Editors.Common;
using Maestro.Editors.MapDefinition;
using Maestro.MapViewer;

namespace Maestro.LiveMapEditor
{
    public partial class MapSettingsDialog : Form
    {
        private IMapDefinition _mdf;
        private IServerConnection _conn;
        private IMapViewer _viewer;

        public MapSettingsDialog(IServerConnection conn, IMapDefinition mdf, IMapViewer viewer)
        {
            InitializeComponent();
            _conn = conn;
            _viewer = viewer;
            txtCoordinateSystem.Text = mdf.CoordinateSystem;
            var ext = mdf.Extents;
            txtLowerX.Text = ext.MinX.ToString(CultureInfo.InvariantCulture);
            txtLowerY.Text = ext.MinY.ToString(CultureInfo.InvariantCulture);
            txtUpperX.Text = ext.MaxX.ToString(CultureInfo.InvariantCulture);
            txtUpperY.Text = ext.MaxY.ToString(CultureInfo.InvariantCulture);
            cmbBackgroundColor.ResetColors();
            cmbBackgroundColor.CurrentColor = mdf.BackgroundColor;
            _mdf = mdf;
            btnUseCurrentView.Visible = (_viewer != null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
                return;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool ValidateSettings()
        {
            /*
            if (string.IsNullOrEmpty(txtCoordinateSystem.Text))
            {
                MessageBox.Show(Strings.ErrCoordSysRequired);
                return false;
            }
            else
            {
                _mdf.CoordinateSystem = txtCoordinateSystem.Text;
            }*/

            _mdf.CoordinateSystem = txtCoordinateSystem.Text;

            double llx;
            double lly;
            double urx;
            double ury;
            if (!double.TryParse(txtLowerX.Text, out llx) ||
                !double.TryParse(txtLowerY.Text, out lly) ||
                !double.TryParse(txtUpperX.Text, out urx) ||
                !double.TryParse(txtUpperY.Text, out ury))
            {
                MessageBox.Show(Strings.ErrInvalidExtents);
                return false;
            }
            else 
            {
                _mdf.Extents = ObjectFactory.CreateEnvelope(llx, lly, urx, ury);
            }

            _mdf.BackgroundColor = cmbBackgroundColor.CurrentColor;

            return true;
        }

        private void btnPickCs_Click(object sender, EventArgs e)
        {
            using (var picker = new CoordinateSystemPicker(_conn.CoordinateSystemCatalog))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var cs = picker.SelectedCoordSys;
                    txtCoordinateSystem.Text = cs.WKT;
                }
            }
        }

        private void btnSetZoom_Click(object sender, EventArgs e)
        {
            var diag = new ExtentCalculationDialog(_mdf);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var env = diag.Extents;
                if (env != null)
                {
                    _mdf.SetExtents(env.MinX, env.MinY, env.MaxX, env.MaxY);
                    txtLowerX.Text = env.MinX.ToString(CultureInfo.InvariantCulture);
                    txtLowerY.Text = env.MinY.ToString(CultureInfo.InvariantCulture);
                    txtUpperX.Text = env.MaxX.ToString(CultureInfo.InvariantCulture);
                    txtUpperY.Text = env.MaxY.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show(Strings.ErrorMapExtentCalculationFailed, Strings.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUseCurrentView_Click(object sender, EventArgs e)
        {
            if (_viewer != null)
            {
                double minX;
                double minY;
                double maxX;
                double maxY;
                _viewer.GetViewExtent(out minX, out minY, out maxX, out maxY);
                var ext = ObjectFactory.CreateEnvelope(minX, minY, maxX, maxY);
                _mdf.Extents = ext;
                txtLowerX.Text = ext.MinX.ToString(CultureInfo.InvariantCulture);
                txtLowerY.Text = ext.MinY.ToString(CultureInfo.InvariantCulture);
                txtUpperX.Text = ext.MaxX.ToString(CultureInfo.InvariantCulture);
                txtUpperY.Text = ext.MaxY.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
