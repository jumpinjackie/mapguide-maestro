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

namespace Maestro.AddIn.Local.UI
{
    public partial class MapPreviewWindow : Form
    {
        private MgdMap _map;
        private MgRenderingService _renderSvc;
        private MgCoordinateSystem _mapCs;
        private MgdSelection _selection;
        private MgRenderingOptions _layerRenderOpts;
        private MgMeasure _mapMeasure;
        private MgWktReaderWriter _wktRW;
        private MgGeometryFactory _geomFact;

        private Color _mapBgColor;
        
        public MapPreviewWindow(MgdMap map)
        {
            InitializeComponent();
            _map = map;
            var fact = new MgCoordinateSystemFactory();
            _mapCs = fact.Create(_map.GetMapSRS());
            _mapMeasure = _mapCs.GetMeasure();
            _wktRW = new MgWktReaderWriter();
            _geomFact = new MgGeometryFactory();
            this.Disposed += new EventHandler(OnDisposed);

            Init(_map);
        }

        void OnDisposed(object sender, EventArgs e)
        {
            if (_renderSvc != null)
            {
                _renderSvc.Dispose();
                _renderSvc = null;
            }

            if (_selection != null)
            {
                _selection.Dispose();
                _selection = null;
            }

            if (_mapMeasure != null)
            {
                _mapMeasure.Dispose();
                _mapMeasure = null;
            }

            if (_mapCs != null)
            {
                _mapCs.Dispose();
                _mapCs = null;
            }

            if (_wktRW != null)
            {
                _wktRW.Dispose();
                _wktRW = null;
            }

            if (_geomFact != null)
            {
                _geomFact.Dispose();
                _geomFact = null;
            }
        }

        private double _orgX1;
        private double _orgX2;
        private double _orgY1;
        private double _orgY2;

        private double _extX1;
        private double _extX2;
        private double _extY1;
        private double _extY2;

        private bool _init = false;

        public void Init(MgdMap map)
        {
            numScale.Maximum = int.MaxValue;
            _init = false;
            _renderSvc = MgServiceFactory.CreateRenderingService();
            _layerRenderOpts = new MgRenderingOptions(MgImageFormats.Png, 7, new MgColor(0, 0, 255));

            _map = map;
            var bgColor = _map.GetBackgroundColor();
            if (bgColor.Length == 8 || bgColor.Length == 6)
            {
                _mapBgColor = ColorTranslator.FromHtml("#" + bgColor);
                mapImage.BackColor = _mapBgColor;
            }
            _map.SetDisplaySize(mapImage.Width, mapImage.Height);
            _selection = new MgdSelection(_map);

            var env = _map.GetMapExtent();
            var ll = env.LowerLeftCoordinate;
            var ur = env.UpperRightCoordinate;

            _extX1 = _orgX1 = ll.X;
            _extY2 = _orgY2 = ll.Y;
            _extX2 = _orgX2 = ur.X;
            _extY1 = _orgY1 = ur.Y;

            if ((_orgX1 - _orgX2) == 0 || (_orgY1 - _orgY2) == 0)
            {
                _extX1 = _orgX1 = -.1;
                _extY2 = _orgX2 = .1;
                _extX2 = _orgY1 = -.1;
                _extY1 = _orgY2 = .1;
            }

            double scale = CalculateScale(Math.Abs(_orgX2 - _orgX1), Math.Abs(_orgY2 - _orgY1), this.Width, this.Height);
            numScale.Value = Convert.ToDecimal(scale);
            _map.SetViewCenterXY(_extX1 + (_extX2 - _extX1) / 2, _extY2 + (_extY1 - _extY2) / 2);
            _map.SetViewScale(scale);

            _init = true;
        }

        private double CalculateScale(double mcsW, double mcsH, int devW, int devH)
        {
            var mpu = _map.GetMetersPerUnit();
            var mpp = 0.0254 / _map.DisplayDpi;
            if (devH * mcsW > devW * mcsH)
                return mcsW * mpu / (devW * mpp); //width-limited
            else
                return mcsH * mpu / (devH * mpp); //height-limited
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ZoomExtents();
        }

        protected override void OnResize(EventArgs e)
        {
            if (!_init)
                return;

            base.OnResize(e);
            _map.SetDisplaySize(mapImage.Width, mapImage.Height);
            RefreshMap();
        }

        class RenderWorkArgs
        {
            public MgRenderingOptions RenderingOptions { get; set; }
        }

        class RenderResult
        {
            public Image Image { get; set; }

            public Image Overlay { get; set; }
        }

        public void RefreshMap()
        {
            if (renderWorker.IsBusy)
                return;

            if (mapImage.Image != null)
            {
                mapImage.Image.Dispose();
                mapImage.Image = null;
            }

            renderWorker.RunWorkerAsync(new RenderWorkArgs() { RenderingOptions = _layerRenderOpts });
        }

        public void Pan(double x, double y, bool refresh)
        {
            ZoomToView(x, y, _map.ViewScale, refresh);
        }

        public void ZoomToView(double x, double y, double scale, bool refresh)
        {
            _map.SetViewCenterXY(x, y);
#if DEBUG
            //var mapExt = _map.MapExtent;
            //var dataExt = _map.DataExtent;
            //Trace.TraceInformation("Map Extent is ({0},{1} {2},{3})", mapExt.LowerLeftCoordinate.X, mapExt.LowerLeftCoordinate.Y, mapExt.UpperRightCoordinate.X, mapExt.UpperRightCoordinate.Y);
            //Trace.TraceInformation("Data Extent is ({0},{1} {2},{3})", dataExt.LowerLeftCoordinate.X, dataExt.LowerLeftCoordinate.Y, dataExt.UpperRightCoordinate.X, dataExt.UpperRightCoordinate.Y);

            Trace.TraceInformation("Center is (" + x + ", " + y + ")");
#endif
            _map.SetViewScale(scale);

            //Update current extents
            double mpu = _map.GetMetersPerUnit();
            double mpp = 0.0254 / _map.DisplayDpi;

            var mcsWidth = _map.DisplayWidth * mpp * scale / mpu;
            var mcsHeight = _map.DisplayHeight * mpp * scale / mpu;

            _extX1 = x - mcsWidth / 2;
            _extY1 = y + mcsHeight / 2;
            _extX2 = x + mcsWidth / 2;
            _extY2 = y - mcsHeight / 2;

#if DEBUG
            Trace.TraceInformation("Current extents is ({0},{1} {2},{3})", _extX1, _extY1, _extX2, _extY2);
#endif

            //Then refresh
            if (refresh)
                RefreshMap();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            PanLeft(true);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            PanUp(true);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            PanRight(true);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            PanDown(true);
        }

        private void PanLeft(bool refresh)
        {
            Pan(_extX1 + (_extX2 - _extX1) / 3, _extY2 + (_extY1 - _extY2) / 2, refresh);
        }

        private void PanUp(bool refresh)
        {
            Pan(_extX1 + (_extX2 - _extX1) / 2, _extY1 - (_extY1 - _extY2) / 3, refresh);
        }

        private void PanRight(bool refresh)
        {
            Pan(_extX2 - (_extX2 - _extX1) / 3, _extY2 + (_extY1 - _extY2) / 2, refresh);
        }

        private void PanDown(bool refresh)
        {
            Pan(_extX1 + (_extX2 - _extX1) / 2, _extY2 + (_extY1 - _extY2) / 3, refresh);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _map.SetViewScale(_map.ViewScale * 0.8);
            RefreshMap();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _map.SetViewScale(_map.ViewScale * 1.2);
            RefreshMap();
        }

        private void btnZoomExtents_Click(object sender, EventArgs e)
        {
            ZoomExtents();
        }

        public void ZoomExtents()
        {
            var scale = CalculateScale((_orgX2 - _orgX1), (_orgY1 - _orgY2), mapImage.Width, mapImage.Height);
            ZoomToView(_orgX1 + ((_orgX2 - _orgX1) / 2), _orgY2 + ((_orgY1 - _orgY2) / 2), scale, true);
        }

        private void renderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (RenderWorkArgs)e.Argument;

            var res = new RenderResult();
            var br = _renderSvc.RenderDynamicOverlay(_map, _selection, args.RenderingOptions);
            byte[] b = new byte[br.GetLength()];
            br.Read(b, b.Length);
            
            using (var ms = new MemoryStream(b))
            {
                res.Image = Image.FromStream(ms);
            }

            e.Result = res;
        }

        private void renderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void renderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error");
            }
            else
            {
                var res = (RenderResult)e.Result;
                //set the image
                mapImage.Image = res.Image;
                numScale.Value = Convert.ToDecimal(_map.ViewScale);
            }
        }

        private void btnClearSelect_Click(object sender, EventArgs e)
        {
            _selection.Clear();
            RefreshMap();
        }

        private void mapImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (!chkSelectFeatures.Checked)
                return;

            var mapPt1 = ScreenToMapUnits(e.X-2, e.Y-2);
            var mapPt2 = ScreenToMapUnits(e.X+2, e.Y+2);

            var coord1 = _geomFact.CreateCoordinateXY(mapPt1.X, mapPt1.Y);
            var coord2 = _geomFact.CreateCoordinateXY(mapPt2.X, mapPt2.Y);

            var dist = _mapMeasure.GetDistance(coord1, coord2);

            MgGeometry geom = _wktRW.Read(MakeWktPolygon(mapPt1.X, mapPt1.Y, mapPt2.X, mapPt2.Y));

            SelectByGeometry(geom);
        }

        private static string MakeWktPolygon(double x1, double y1, double x2, double y2)
        {
            return "POLYGON((" + x1 + " " + y1 + ", " + x2 + " " + y1 + ", " + x2 + " " + y2 + ", " + x1 + " " + y2 + ", " + x1 + " " + y1 + "))";
        }

        private Dictionary<string, string> _layerGeomProps = new Dictionary<string, string>();

        private void SelectByGeometry(MgGeometry geom)
        {   
            var layers = _map.GetLayers();

            //Cache geometry properties
            for (int i = 0; i < layers.GetCount(); i++)
            {
                var layer = layers.GetItem(i);
                if (!layer.Selectable && !layer.IsVisible())
                    continue;

                var objId = layer.GetObjectId();
                if (_layerGeomProps.ContainsKey(objId))
                    continue;

                var cls = layer.GetClassDefinition();
                var geomName = cls.DefaultGeometryPropertyName;
                if (!string.IsNullOrEmpty(geomName))
                {
                    if (!_layerGeomProps.ContainsKey(objId))
                        _layerGeomProps[objId] = geomName;
                }
            }

            _selection.Clear();
            
            for (int i = 0; i < layers.GetCount(); i++)
            {
                var layer = layers.GetItem(i);
                if (!layer.Selectable && !layer.IsVisible())
                    continue;

                var objId = layer.GetObjectId();
                MgFeatureQueryOptions query = new MgFeatureQueryOptions();
                query.SetSpatialFilter(_layerGeomProps[objId], geom, MgFeatureSpatialOperations.Intersects);

                MgFeatureReader reader = layer.SelectFeatures(query);
                _selection.AddFeatures(layer, reader, 0);
            }

            int total = 0;
            for (int i = 0; i < layers.GetCount(); i++)
            {
                var layer = layers.GetItem(i);
                total += _selection.GetSelectedFeaturesCount(layer, layer.FeatureClassName);
            }
            lblFeaturesSelected.Text = string.Format("{0} features selected", total);

            string xml = _selection.ToXml();
            if (!string.IsNullOrEmpty(xml))
                RefreshMap();
        }

        public PointF ScreenToMapUnits(double x, double y)
        {
            return ScreenToMapUnits(x, y, false);
        }

        private PointF ScreenToMapUnits(double x, double y, bool allowOutsideWindow)
        {
            if (!allowOutsideWindow)
            {
                if (x > mapImage.Width - 1) x = mapImage.Width - 1;
                else if (x < 0) x = 0;

                if (y > mapImage.Height - 1) y = mapImage.Height - 1;
                else if (y < 0) y = 0;
            }

            x = _extX1 + (_extX2 - _extX1) * (x / mapImage.Width);
            y = _extY1 - (_extY1 - _extY2) * (y / mapImage.Height);
            return new PointF((float)x, (float)y);
        }

        private void btnUpperLeft_Click(object sender, EventArgs e)
        {
            PanUp(false);
            PanLeft(true);
        }

        private void btnUpperRight_Click(object sender, EventArgs e)
        {
            PanUp(false);
            PanRight(true);
        }

        private void btnLowerLeft_Click(object sender, EventArgs e)
        {
            PanDown(false);
            PanLeft(true);
        }

        private void btnLowerRight_Click(object sender, EventArgs e)
        {
            PanDown(false);
            PanRight(true);
        }

        private void btnZoomScale_Click(object sender, EventArgs e)
        {
            _map.SetViewScale(Convert.ToDouble(numScale.Value));
            RefreshMap();
        }

        private void mapImage_MouseMove(object sender, MouseEventArgs e)
        {
            var mapPt = ScreenToMapUnits(e.X, e.Y);
            lblCoordinates.Text = string.Format("X: {0:0.0000000}, Y: {1:0.0000000} ({2})", mapPt.X, mapPt.Y, _mapCs.Units);
        }
    }
}
