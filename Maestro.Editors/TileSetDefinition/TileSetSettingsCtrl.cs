#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.ObjectModels.Common;
using Maestro.Editors.TileSetDefinition.Providers;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using Maestro.Editors.MapDefinition;
using System.Globalization;

namespace Maestro.Editors.TileSetDefinition
{
    [ToolboxItem(false)]
    internal partial class TileSetSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public TileSetSettingsCtrl()
        {
            InitializeComponent();
        }

        private bool _init = false;
        private ITileSetDefinition _tsd;
        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
            try
            {
                _init = true;
                _tsd = (ITileSetDefinition)_service.GetEditedResource();

                txtMinX.Text = _tsd.Extents.MinX.ToString(CultureInfo.InvariantCulture);
                txtMinY.Text = _tsd.Extents.MinY.ToString(CultureInfo.InvariantCulture);
                txtMaxX.Text = _tsd.Extents.MaxX.ToString(CultureInfo.InvariantCulture);
                txtMaxY.Text = _tsd.Extents.MaxY.ToString(CultureInfo.InvariantCulture);

                var cmd = (IGetTileProviders)_service.CurrentConnection.CreateCommand((int)CommandType.GetTileProviders);
                var providers = cmd.Execute();

                var provider = providers.TileProvider.FirstOrDefault(p => p.Name == _tsd.TileStoreParameters.TileProvider);
                if (provider != null)
                {
                    txtProvider.Text = provider.DisplayName;
                    LoadProviderCtrl(provider);
                }
            }
            finally
            {
                _init = false;
            }
        }

        private void LoadProviderCtrl(TileProvider provider)
        {
            var ctrl = MakeProviderCtrl(provider);
            ctrl.Dock = DockStyle.Fill;
            grpSettings.Controls.Clear();
            grpSettings.Controls.Add(ctrl);
        }

        private Control MakeProviderCtrl(TileProvider provider)
        {
            return new GenericProviderCtrl(provider, _tsd, OnResourceChanged);
        }

        private IEnumerable<string> CollectLayerIds()
        {
            HashSet<string> ids = new HashSet<string>();

            foreach (var grp in _tsd.BaseMapLayerGroups)
            {
                foreach (var lyr in grp.BaseMapLayer)
                {
                    ids.Add(lyr.ResourceId);
                }
            }

            return ids;
        }

        private void btnSetZoom_Click(object sender, EventArgs e)
        {
            string coordinateSystem = null;
            if (_tsd.TileStoreParameters.TileProvider == "Default") //NOXLATE
                coordinateSystem = _tsd.GetDefaultCoordinateSystem();
            else
                coordinateSystem = _service.CurrentConnection.CoordinateSystemCatalog.FindCoordSys("LL84").WKT;

            var diag = new ExtentCalculationDialog(_service.CurrentConnection, coordinateSystem, CollectLayerIds);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var env = diag.Extents;
                if (env != null)
                {
                    txtMinX.Text = env.MinX.ToString(CultureInfo.InvariantCulture);
                    txtMinY.Text = env.MinY.ToString(CultureInfo.InvariantCulture);
                    txtMaxX.Text = env.MaxX.ToString(CultureInfo.InvariantCulture);
                    txtMaxY.Text = env.MaxY.ToString(CultureInfo.InvariantCulture);
                    OnResourceChanged();
                }
                else
                {
                    MessageBox.Show(Strings.ErrorMapExtentCalculationFailed, Strings.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtMinX_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtMinX.Text, out d))
                _tsd.Extents.MinX = d;
        }

        private void txtMinY_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtMinY.Text, out d))
                _tsd.Extents.MinY = d;
        }

        private void txtMaxX_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtMaxX.Text, out d))
                _tsd.Extents.MaxX = d;
        }

        private void txtMaxY_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            double d;
            if (double.TryParse(txtMaxY.Text, out d))
                _tsd.Extents.MaxY = d;
        }
    }
}
