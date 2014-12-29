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

#endregion Disclaimer / License

using Maestro.Editors.WatermarkDefinition;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System.Windows.Forms;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// Editor control for Map Definitions
    /// </summary>
    public partial class MapDefinitionEditorCtrl : EditorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDefinitionEditorCtrl"/> class.
        /// </summary>
        public MapDefinitionEditorCtrl()
        {
            InitializeComponent();
        }

        private IMapDefinition _map;
        private IEditorService _edSvc;

        class LayerExtentCalculator : ILayerExtentCalculator
        {
            private IServerConnection _conn;

            public LayerExtentCalculator(IServerConnection conn)
            {
                _conn = conn;
            }

            public LayerExtent GetLayerExtent(string resourceID, string mapCoordSys)
            {
                var ldf = (ILayerDefinition)_conn.ResourceService.GetResource(resourceID);
                string csWkt;
                var env = ldf.GetSpatialExtent(_conn, true, out csWkt);
                if (env != null)
                {
                    if (!string.IsNullOrEmpty(mapCoordSys) && csWkt != mapCoordSys)
                        env = Utility.TransformEnvelope(env, csWkt, mapCoordSys);

                    return new LayerExtent()
                    {
                        LayerCoordinateSystem = csWkt,
                        Extent = env
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _map = _edSvc.GetEditedResource() as IMapDefinition;
            _map.SetExtentsFromFirstAddedLayer = true;

            mapSettingsCtrl.Bind(service);
            mapLayersCtrl.Bind(service);

            _map.ExtentCalculator = new LayerExtentCalculator(_edSvc.CurrentConnection);

            var mp2 = _map as IMapDefinition2;
            if (mp2 != null)
            {
                this.Controls.Remove(mapSettingsCtrl);
                this.Controls.Remove(mapLayersCtrl);

                var wm = new WatermarkCollectionEditorCtrl(service, mp2);
                wm.Dock = DockStyle.Fill;

                this.Controls.Add(wm);
                this.Controls.Add(mapLayersCtrl);
                this.Controls.Add(mapSettingsCtrl);
            }

            mapLayersCtrl.RequestLayerOpen += new OpenLayerEventHandler(OnRequestLayerOpen);
        }

        private void OnRequestLayerOpen(object sender, string layerResourceId)
        {
            _edSvc.OpenResource(layerResourceId);
        }
    }
}