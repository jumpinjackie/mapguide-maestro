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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide;
using System.IO;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.Viewer;
using ICSharpCode.Core;
using OSGeo.MapGuide.Viewer.Desktop;

namespace Maestro.AddIn.Local.UI
{
    public partial class MapPreviewWindow : Form, IMapStatusBar
    {
        private IServerConnection _conn;

        public MapPreviewWindow(IServerConnection conn)
        {
            InitializeComponent();
            _conn = conn;

            new MapViewerController(viewer, legend, this, propertyPane, toolbar);
            this.Disposed += OnDisposed;
        }

        private MgdMap _map;

        public void Init(MgResourceIdentifier mapResId)
        {
            _map = new MgdMap(mapResId);
            var groups = _map.GetLayerGroups();
            if (groups != null && groups.GetCount() > 0)
            {
                for (int i = 0; i < groups.GetCount(); i++)
                {
                    var grp = groups.GetItem(i);
                    if (grp.LayerGroupType == MgLayerGroupType.BaseMap)
                    {
                        MessageBox.Show(Strings.TiledLayerSupportWarning);
                        break;
                    }
                }
            }
            var fact = new MgdServiceFactory();
            viewer.Init(new MgDesktopMapViewerProvider(_map));
            viewer.RefreshMap();
        }

        void OnDisposed(object sender, EventArgs e)
        {
            if (_map != null)
            {
                _map.Dispose();
                _map = null;
            }
        }

        public void SetCursorPositionMessage(string message)
        {
            lblCoordinates.Text = message;
        }

        public void SetFeatureSelectedMessage(string message)
        {
            lblFeaturesSelected.Text = message;
        }

        public void SetMapScaleMessage(string message)
        {
            lblScale.Text = message;
        }

        public void SetMapSizeMessage(string message)
        {
            lblMapSize.Text = message;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var diag = new ZoomScaleDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                viewer.ZoomToScale(diag.Value);
            }
        }
    }
}
