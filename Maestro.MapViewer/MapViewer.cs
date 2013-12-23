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
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.MapViewer
{
    /// <summary>
    /// An interactive map viewer control
    /// </summary>
    [ToolboxItem(true)]
    public class MapViewer : Control, IMapViewer
    {
        private BackgroundWorker renderWorker;

        private RuntimeMap _map;
        private MapSelection _selection;
        private ViewerRenderingOptions _overlayRenderOpts;
        private ViewerRenderingOptions _selectionRenderOpts;

        private Color _mapBgColor;

        private double _orgX1;
        private double _orgX2;
        private double _orgY1;
        private double _orgY2;

        private double _extX1;
        private double _extX2;
        private double _extY1;
        private double _extY2;

        private Image _selectionImage;
        private Image _mapImage;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Image Image
        {
            get { return _mapImage; }
            set
            {
                _mapImage = value;
                //Invalidate();
            }
        }

        const double MINIMUM_ZOOM_SCALE = 5.0;

#if VIEWER_DEBUG
        private MgdLayer _debugLayer;

        private void CreateDebugFeatureSource()
        {
            var id = new MgDataPropertyDefinition("ID");
            id.DataType = MgPropertyType.Int32;
            id.Nullable = false;
            id.SetAutoGeneration(true);

            var geom = new MgGeometricPropertyDefinition("Geometry");
            geom.GeometryTypes = MgFeatureGeometricType.Point;
            geom.SpatialContextAssociation = "MapCs";

            var cls = new MgClassDefinition();
            cls.Name = "Debug";
            var props = cls.GetProperties();
            props.Add(id);
            props.Add(geom);

            var idProps = cls.GetIdentityProperties();
            idProps.Add(id);

            cls.DefaultGeometryPropertyName = "Geometry";

            var schema = new MgFeatureSchema("Default", "Default schema");
            var classes = schema.GetClasses();
            classes.Add(cls);

            //We can make anything up here, there's no real concept of sessions
            var sessionId = Guid.NewGuid().ToString();

            var debugFsId = new MgResourceIdentifier("Session:" + sessionId + "//Debug" + Guid.NewGuid().ToString() + ".FeatureSource");
            var createSdf = new MgCreateSdfParams("MapCs", _map.GetMapSRS(), schema);
            var featureSvc = (MgdFeatureService)fact.CreateService(MgServiceType.FeatureService);
            var resSvc = (MgResourceService)fact.CreateService(MgServiceType.ResourceService);
            featureSvc.CreateFeatureSource(debugFsId, createSdf);

            byte[] bytes = Encoding.UTF8.GetBytes(string.Format(Debug.DebugLayer, debugFsId.ToString(), "Default:Debug", "Geometry"));
            var source = new MgByteSource(bytes, bytes.Length);

            var debugLayerId = new MgResourceIdentifier("Session:" + sessionId + "//" + debugFsId.Name + ".LayerDefinition");
            var breader = source.GetReader();
            resSvc.SetResource(debugLayerId, breader, null);
            
            _debugLayer = new MgdLayer(debugLayerId, resSvc);
            _debugLayer.SetLegendLabel("Debug Layer");
            _debugLayer.SetVisible(true);
            _debugLayer.SetDisplayInLegend(true);

            var mapLayers = _map.GetLayers();
            mapLayers.Insert(0, _debugLayer);

            UpdateCenterDebugPoint();
        }

        private MgPropertyCollection _debugCenter;

        private void UpdateCenterDebugPoint()
        {
            if (_debugCenter == null)
                _debugCenter = new MgPropertyCollection();

            var center = _wktRW.Read("POINT (" + _map.ViewCenter.Coordinate.X + " " + _map.ViewCenter.Coordinate.Y + ")");
            var agf = _agfRW.Write(center);
            if (!_debugCenter.Contains("Geometry"))
            {
                MgGeometryProperty geom = new MgGeometryProperty("Geometry", agf);
                _debugCenter.Add(geom);
            }
            else
            {
                MgGeometryProperty geom = (MgGeometryProperty)_debugCenter.GetItem("Geometry");
                geom.SetValue(agf);
            }

            int deleted = _debugLayer.DeleteFeatures("");
            Trace.TraceInformation("Deleted {0} debug points", deleted);
            var reader = _debugLayer.InsertFeatures(_debugCenter);
            int inserted = 0;
            while (reader.ReadNext())
            {
                inserted++;
            }
            reader.Close();
            Trace.TraceInformation("Added {0} debug points", inserted);
            _debugLayer.ForceRefresh();
        }
#endif

        private int _viewHistoryIndex;
        private List<MapViewHistoryEntry> _viewHistory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapViewer"/> class.
        /// </summary>
        public MapViewer()
        {
            _viewHistory = new List<MapViewHistoryEntry>();
            _viewHistoryIndex = -1;
            this.ShowVertexCoordinatesWhenDigitizing = false;
            this.FeatureTooltipsEnabled = false;
            this.TooltipsEnabled = false;
            this.ZoomInFactor = 0.5;
            this.ZoomOutFactor = 2.0;
            this.SelectionColor = Color.Blue;
            this.PointPixelBuffer = 2;

            this.MinScale = 10;
            this.MaxScale = 1000000000;

            this.UseRenderMapIfTiledLayersExist = true;
            this.RespectFiniteDisplayScales = true;

            this.DigitizingFillTransparency = 100;
            this.DigitizingOutline = Brushes.Red;
            this.DigitzingFillColor = Color.White;
            this.TooltipFillColor = Color.LightYellow;
            this.TooltipFillTransparency = 200;
            this.MouseWheelDelayRenderInterval = 800;
            this.TooltipDelayInterval = 1000;

            this.ActiveTool = MapActiveTool.None;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            _mapBgColor = Color.Transparent;

            _defaultDigitizationInstructions = Properties.Resources.GeneralDigitizationInstructions;
            _defaultMultiSegmentDigitizationInstructions = Properties.Resources.MultiSegmentDigitzationInstructions;
            _defaultPointDigitizationPrompt = Properties.Resources.PointDigitizationPrompt;
            _defaultLineDigitizationPrompt = Properties.Resources.LineDigitizationPrompt;
            _defaultCircleDigitizationPrompt = Properties.Resources.CircleDigitizationPrompt;
            _defaultLineStringDigitizationPrompt = Properties.Resources.LineStringDigitizationPrompt;
            _defaultPolygonDigitizationPrompt = Properties.Resources.PolygonDigitizationPrompt;
            _defaultRectangleDigitizationPrompt = Properties.Resources.RectangleDigitizationPrompt;
            
            renderWorker = new BackgroundWorker();

            renderWorker.DoWork += renderWorker_DoWork;
            renderWorker.RunWorkerCompleted += renderWorker_RunWorkerCompleted;

            base.MouseUp += OnMapMouseUp;
            base.MouseMove += OnMapMouseMove;
            base.MouseDown += OnMapMouseDown;
            base.MouseClick += OnMapMouseClick;
            base.MouseDoubleClick += OnMapMouseDoubleClick;
            base.MouseHover += OnMapMouseHover;
            base.MouseEnter += OnMouseEnter;
            base.MouseWheel += OnMapMouseWheel;
            base.MouseLeave += OnMapMouseLeave;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CancelDigitization();
            }
            else if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
            {
                if (this.DigitizingType == MapDigitizationType.LineString ||
                    this.DigitizingType == MapDigitizationType.Polygon)
                {
                    if (dPath.Count > 1) //Slice off the last recorded point
                    {
                        dPath.RemoveAt(dPath.Count - 1);
                        Invalidate();
                    }
                }
            }
        }

        private void CancelDigitization()
        {
            if (this.DigitizingType != MapDigitizationType.None)
            {
                dPath.Clear();
                dPtStart.X = 0;
                dPtStart.Y = 0;
                this.DigitizingType = MapDigitizationType.None;
                Trace.TraceInformation("Digitization cancelled");
                this.Invalidate();
            }
        }

        void OnMouseEnter(object sender, EventArgs e)
        {
            this.Focus();   
        }

        void OnMapMouseHover(object sender, EventArgs e)
        {
            HandleMouseHover(e);
        }

        private void HandleMouseHover(EventArgs e)
        {
            
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"/> and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.MouseUp -= OnMapMouseUp;
                base.MouseMove -= OnMapMouseMove;
                base.MouseDown -= OnMapMouseDown;
                base.MouseClick -= OnMapMouseClick;
                base.MouseDoubleClick -= OnMapMouseDoubleClick;
                base.MouseHover -= OnMapMouseHover;
                base.MouseEnter -= OnMouseEnter;
                base.MouseLeave -= OnMapMouseLeave; 

                if (renderWorker != null)
                {
                    renderWorker.DoWork -= renderWorker_DoWork;
                    renderWorker.RunWorkerCompleted -= renderWorker_RunWorkerCompleted;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the minimum allowed zoom scale for this viewer
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The minimum allowed zoom scale for this viewer")]
        public int MinScale { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed zoom scale for this viewer
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The maximum allowed zoom scale for this viewer")]
        public int MaxScale { get; set; }

        /// <summary>
        /// The amount of time (in ms) to wait to re-render after a mouse wheel scroll
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The amount of time (in ms) to wait to re-render after a mouse wheel scroll")]
        public int MouseWheelDelayRenderInterval { get; set; }

        /// <summary> 
        /// The amount of time (in ms) to wait to re-render after a mouse wheel scroll 
        /// </summary> 
        [Category("MapGuide Viewer")]
        [Description("The amount of time (in ms) to wait to fire off a tooltip request after the mouse pointer becomes stationary")]
        public int TooltipDelayInterval { get; set; } 

        private Color _selColor;

        /// <summary>
        /// Gets or sets the color used to render selected features
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The color to use for active selections")]
        public Color SelectionColor
        {
            get { return _selColor; }
            set 
            { 
                _selColor = value;
                OnPropertyChanged("SelectionColor");
            }
        }

        private Color _tooltipFillColor;

        /// <summary>
        /// Gets or sets the color of the tooltip fill.
        /// </summary>
        /// <value>
        /// The color of the tooltip fill.
        /// </value>
        [Category("MapGuide Viewer")]
        [Description("The color background for feature tooltips")]
        internal Color TooltipFillColor
        {
            get { return _tooltipFillColor; }
            set
            {
                if (!value.Equals(_tooltipFillColor))
                {
                    _tooltipFillColor = value;
                    OnPropertyChanged("TooltipFillColor");
                }
            }
        }

        private int _tooltipFillTransparency;

        [Category("MapGuide Viewer")]
        [Description("The color background transparency for feature tooltips")]
        [DefaultValue(200)]
        internal int TooltipFillTransparency
        {
            get { return _tooltipFillTransparency; }
            set
            {
                if (!value.Equals(_tooltipFillTransparency))
                {
                    _tooltipFillTransparency = value;
                    OnPropertyChanged("TooltipFillTransparency");
                }
            }
        }

        private void UpdateSelectionRenderingOptions()
        {
            var value = this.SelectionColor;
            if (_selectionRenderOpts != null)
            {
                var color = _selectionRenderOpts.Color;
                if (!color.Equals(value))
                {
                    _selectionRenderOpts = null;

                    _selectionRenderOpts = CreateSelectionRenderingOptions(value.R, value.G, value.B);
                    Trace.TraceInformation("Selection color updated to ({0}, {1}, {2})", value.R, value.G, value.B);
                }
            }
            
        }

        private bool _showVertexCoords;

        /// <summary>
        /// Gets or sets a value indicating whether [show vertex coordinates when digitizing].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [show vertex coordinates when digitizing]; otherwise, <c>false</c>.
        /// </value>
        [Category("MapGuide Viewer")]
        [Description("Indicates whether coordinate values are shown when digitizing geometry")]
        [DefaultValue(false)]
        public bool ShowVertexCoordinatesWhenDigitizing
        {
            get { return _showVertexCoords; }
            set
            {
                if (!value.Equals(_showVertexCoords))
                {
                    _showVertexCoords = value;
                    OnPropertyChanged("ShowVertexCoordinatesWhenDigitizing");
                }
            }
        }

        private string _defaultDigitizationInstructions;
        private string _defaultMultiSegmentDigitizationInstructions;

        private string _defaultPointDigitizationPrompt;
        private string _defaultLineDigitizationPrompt;
        private string _defaultCircleDigitizationPrompt;
        private string _defaultLineStringDigitizationPrompt;
        private string _defaultPolygonDigitizationPrompt;
        private string _defaultRectangleDigitizationPrompt;

        private string _pointCustomDigitizationPrompt;
        private string _lineCustomDigitizationPrompt;
        private string _circleCustomDigitizationPrompt;
        private string _lineStringCustomDigitizationPrompt;
        private string _polygonCustomDigitizationPrompt;
        private string _rectangleCustomDigitizationPrompt;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Trace.TraceInformation("OnPaint(e)");

            ApplyPaintTranslateTransform(e);

            if (mouseWheelSx.HasValue && mouseWheelSy.HasValue && mouseWheelSx.Value != 0.0 && mouseWheelSy.Value != 0.0)
            {
                e.Graphics.ScaleTransform(mouseWheelSx.Value, mouseWheelSy.Value);
            }

            if (_mapImage != null)
            {
                Trace.TraceInformation("Render Map");
                e.Graphics.DrawImage(_mapImage, new PointF(0, 0));
            }

            //Thread.Sleep(100);
            if (_selectionImage != null)
            {
                Trace.TraceInformation("Render Selection");
                e.Graphics.DrawImage(_selectionImage, new PointF(0, 0));
            }

            //TODO: We could add support here for map-space persistent digitizations 

            if (isDragging && (this.ActiveTool == MapActiveTool.Select || this.ActiveTool == MapActiveTool.ZoomIn))
            {
                DrawDragRectangle(e);
            }
            else
            {
                if (this.DigitizingType != MapDigitizationType.None)
                {
                    if (this.DigitizingType == MapDigitizationType.Point)
                    {
                        string str = (_pointCustomDigitizationPrompt ?? _defaultPointDigitizationPrompt) + Environment.NewLine + _defaultDigitizationInstructions;
                        DrawTrackingTooltip(e, str);
                    }
                    else
                    {
                        if (!dPtStart.IsEmpty)
                        {
                            switch (this.DigitizingType)
                            {
                                case MapDigitizationType.Circle:
                                    DrawTracingCircle(e);
                                    break;
                                case MapDigitizationType.Line:
                                    DrawTracingLine(e);
                                    break;
                                case MapDigitizationType.Rectangle:
                                    DrawTracingRectangle(e);
                                    break;
                            }
                        }
                        else if (dPath.Count > 0)
                        {
                            switch (this.DigitizingType)
                            {
                                case MapDigitizationType.LineString:
                                    DrawTracingLineString(e);
                                    break;
                                case MapDigitizationType.Polygon:
                                    DrawTracingPolygon(e);
                                    break;
                            }
                        }
                    }
                }
                else //None
                {
                    if (this.ActiveTool != MapActiveTool.None)
                    {
                        if (!string.IsNullOrEmpty(_activeTooltipText))
                            DrawTrackingTooltip(e, _activeTooltipText);
                    }
                }
            }
        }

        private void ApplyPaintTranslateTransform(PaintEventArgs e)
        {
            if (!translate.IsEmpty)
            {
                if (mouseWheelTx.HasValue && mouseWheelTy.HasValue)
                    e.Graphics.TranslateTransform(translate.X + mouseWheelTx.Value, translate.Y + mouseWheelTy.Value);
                else
                    e.Graphics.TranslateTransform(translate.X, translate.Y);
            }
            else
            {
                if (mouseWheelTx.HasValue && mouseWheelTy.HasValue)
                    e.Graphics.TranslateTransform(mouseWheelTx.Value, mouseWheelTy.Value);
            }
        }

        private Brush _digitizingOutline;

        [Category("MapGuide Viewer")]
        [Description("The outline color for geometries being digitized")]
        internal Brush DigitizingOutline
        {
            get { return _digitizingOutline; }
            set
            {
                _digitizingOutline = value;
                OnPropertyChanged("DigitizingOutline");
            }
        }

        private int _digitizingFillTransparency;

        [Category("MapGuide Viewer")]
        [Description("The fill color transparency for geometries being digitized")]
        [DefaultValue(100)]
        internal int DigitizingFillTransparency
        {
            get { return _digitizingFillTransparency; }
            set
            {
                if (!value.Equals(_digitizingFillTransparency))
                {
                    _digitizingFillTransparency = value;
                    OnPropertyChanged("DigitizingFillTransparency");
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the amount of pixels to buffer out by when doing point-based selections with the select tool
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The amount of pixels to buffer out by when doing point-based selections with the Select tool")]
        public int PointPixelBuffer { get; set; }

        private Color _digitizingFillColor;

        [Category("MapGuide Viewer")]
        [Description("The fill color for geometries being digitized")]
        internal Color DigitzingFillColor
        {
            get { return _digitizingFillColor; }
            set
            {
                _digitizingFillColor = value;
                OnPropertyChanged("DigitzingFillColor");
            }
        }

        private Pen CreateOutlinePen()
        {
            return new Pen(this.DigitizingOutline, 2.0f);
        }

        private Brush CreateFillBrush()
        {
            return new SolidBrush(Color.FromArgb(this.DigitizingFillTransparency, this.DigitzingFillColor));
        }

        private static double GetDistanceBetween(PointF a, PointF b)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(a.X - b.X), 2) + Math.Pow(Math.Abs(a.Y - b.Y), 2)));
        }

        private void DrawVertexCoordinates(PaintEventArgs e, double devX, double devY, bool mapSpace)
        {
            if (!this.ShowVertexCoordinatesWhenDigitizing)
                return;

            string text = "";
            if (mapSpace)
            {
                var mapPt = ScreenToMapUnits(devX, devY);
                text = string.Format("X: {0}, Y: {1}", mapPt.X, mapPt.Y);
            }
            else
            {
                text = string.Format("X: {0}, Y: {1}", devX, devY);
            }

            var f = Control.DefaultFont;
            SizeF size = e.Graphics.MeasureString(text, Font);
            var vertex = new PointF((float)devX, (float)devY);

            //Offset so that the "box" for this string is centered on the vertex itself
            vertex.X -= (size.Width / 2);

            //Fill the surrounding box
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.WhiteSmoke)), vertex.X, vertex.Y, size.Width, size.Height);
            e.Graphics.DrawRectangle(Pens.Red, vertex.X, vertex.Y, size.Width, size.Height);

            //Draw the string
            e.Graphics.DrawString(text, f, Brushes.Black, vertex);
        }

        private void DrawTrackingTooltip(PaintEventArgs e, string text)
        {
            if (string.IsNullOrEmpty(text)) //Nothing to draw
                return;

            var f = Control.DefaultFont;
            
            
            int height = 0;
            int width = 0;
            string [] tokens = text.Split(new string[] {"\\n", "\\r\\n", "\n", Environment.NewLine }, StringSplitOptions.None);
            foreach(string t in tokens)
            {
                var size = e.Graphics.MeasureString(t, f);
                height += (int)size.Height;

                width = Math.Max(width, (int)size.Width);
            }

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(this.TooltipFillTransparency, this.TooltipFillColor)), new Rectangle(_mouseX, _mouseY, width + 10, height + 4));
            float y = 2.0f;
            float heightPerLine = height / tokens.Length;
            foreach (string t in tokens)
            {
                e.Graphics.DrawString(t, f, Brushes.Black, new PointF(_mouseX + 5.0f, _mouseY + y));
                y += heightPerLine;
            }
        }

        private void DrawTracingCircle(PaintEventArgs e)
        {
            var pt2 = new Point(dPtStart.X, dPtStart.Y);
            var diameter = (float)GetDistanceBetween(dPtStart, new PointF(_mouseX, _mouseY)) * 2.0f;
            //Trace.TraceInformation("Diameter ({0}, {1} -> {2}, {3}): {4}", dPtStart.X, dPtStart.Y, _mouseX, _mouseY, diameter);
            pt2.Offset((int)-(diameter / 2), (int)-(diameter / 2));
            e.Graphics.DrawEllipse(CreateOutlinePen(), pt2.X, pt2.Y, diameter, diameter);
            e.Graphics.FillEllipse(CreateFillBrush(), pt2.X, pt2.Y, diameter, diameter);

            string str = (_circleCustomDigitizationPrompt ?? _defaultCircleDigitizationPrompt) + Environment.NewLine + _defaultDigitizationInstructions; 
            DrawTrackingTooltip(e, str);
        }

        private void DrawTracingLine(PaintEventArgs e)
        {
            e.Graphics.DrawLine(CreateOutlinePen(), dPtStart, new Point(_mouseX, _mouseY));
            DrawVertexCoordinates(e, dPtStart.X, dPtStart.Y, true);
            DrawVertexCoordinates(e, _mouseX, _mouseY, true);
            string str = (_lineCustomDigitizationPrompt ?? _defaultLineDigitizationPrompt) + Environment.NewLine + _defaultDigitizationInstructions; 
            DrawTrackingTooltip(e, str);
        }

        private void DrawTracingLineString(PaintEventArgs e)
        {
            //Not enough points to constitute a line string or polygon
            if (dPath.Count < 2)
                return;
            
            e.Graphics.DrawLines(CreateOutlinePen(), dPath.ToArray());
            foreach (var pt in dPath)
            {
                DrawVertexCoordinates(e, pt.X, pt.Y, true);
            }

            string str = (_lineStringCustomDigitizationPrompt ?? _defaultLineStringDigitizationPrompt) + Environment.NewLine + _defaultMultiSegmentDigitizationInstructions; 
            DrawTrackingTooltip(e, str);
        }

        private void DrawTracingPolygon(PaintEventArgs e)
        {
            //Not enough points to constitute a line string or polygon
            if (dPath.Count < 2)
                return;

            e.Graphics.DrawPolygon(CreateOutlinePen(), dPath.ToArray());
            e.Graphics.FillPolygon(CreateFillBrush(), dPath.ToArray());
            foreach (var pt in dPath)
            {
                DrawVertexCoordinates(e, pt.X, pt.Y, true);
            }
            string str = (_polygonCustomDigitizationPrompt ?? _defaultPolygonDigitizationPrompt) + Environment.NewLine + _defaultMultiSegmentDigitizationInstructions; 
            DrawTrackingTooltip(e, str);
        }

        private void DrawTracingRectangle(PaintEventArgs e)
        {
            var rect = GetRectangle(dragStart, new Point(_mouseX, _mouseY));
            if (rect.HasValue)
            {
                var r = rect.Value;
                Trace.TraceInformation("Draw rangle ({0} {1}, {2} {3})", r.Left, r.Top, r.Right, r.Bottom);
                e.Graphics.DrawRectangle(CreateOutlinePen(), r);
                Trace.TraceInformation("Fill rangle ({0} {1}, {2} {3})", r.Left, r.Top, r.Right, r.Bottom);
                e.Graphics.FillRectangle(CreateFillBrush(), r);

                DrawVertexCoordinates(e, r.Left, r.Top, true);
                DrawVertexCoordinates(e, r.Left, r.Bottom, true);
                DrawVertexCoordinates(e, r.Right, r.Top, true);
                DrawVertexCoordinates(e, r.Right, r.Bottom, true);
                string str = (_rectangleCustomDigitizationPrompt ?? _defaultRectangleDigitizationPrompt) + Environment.NewLine + _defaultDigitizationInstructions; 
                DrawTrackingTooltip(e, str);
            }
        }

        private void DrawDragRectangle(PaintEventArgs e)
        {
            var rect = GetRectangle(dragStart, new Point(_mouseX, _mouseY));
            if (rect.HasValue)
            {
                var r = rect.Value;
                Trace.TraceInformation("Draw rangle ({0} {1}, {2} {3})", r.Left, r.Top, r.Right, r.Bottom);
                e.Graphics.DrawRectangle(CreateOutlinePen(), r);
                Trace.TraceInformation("Fill rangle ({0} {1}, {2} {3})", r.Left, r.Top, r.Right, r.Bottom);
                e.Graphics.FillRectangle(CreateFillBrush(), r);
            }
        }

        private bool _featTooltipsEnabled;

        /// <summary>
        /// Gets or sets whether feature tooltips are enabled. If set to true, tooltip queries are
        /// executed at the current mouse position if the active tool is Pan or Select
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FeatureTooltipsEnabled
        {
            get { return _featTooltipsEnabled; }
            set
            {
                if (value.Equals(_featTooltipsEnabled))
                    return;

                _featTooltipsEnabled = value;
                if (!value)
                {
                    _activeTooltipText = null;
                    Invalidate();
                }

                OnPropertyChanged("FeatureTooltipsEnabled");
            }
        }

        /// <summary>
        /// Internally determines whether a tooltip query can be executed. Must be true along
        /// with <see cref="FeatureTooltipsEnabled"/> in order for a tooltip query to be executed
        /// </summary>
        internal bool TooltipsEnabled
        {
            get;
            set;
        }

        #region Digitization

        /*
         * Digitization behaviour with respect to mouse and paint events
         * 
         * Point:
         *  MouseClick -> Invoke Callback
         * 
         * Rectangle:
         *  MouseClick -> set start, temp end
         *  MouseMove -> update temp end
         *  OnPaint -> Draw rectangle from start/temp end
         *  MouseClick -> set end -> Invoke Callback
         * 
         * Line:
         *  MouseClick -> set start, temp end
         *  MouseMove -> update temp end
         *  OnPaint -> Draw line from start/temp end
         *  MouseClick -> set end -> Invoke Callback
         * 
         * LineString:
         *  MouseClick -> append point to path
         *  MouseMove -> update temp end
         *  OnPaint -> Draw line with points in path + temp end
         *  MouseDoubleClick -> append point to path -> Invoke Callback
         * 
         * Polygon:
         *  MouseClick -> append point to path
         *  MouseMove -> update temp end
         *  OnPaint -> Draw polygon fill with points in path + temp end
         *  MouseDoubleClick -> append point to path -> Invoke Callback
         * 
         * Circle:
         *  MouseClick -> set start, temp end
         *  MouseMove -> update temp end
         *  OnPaint -> Draw circle from start with radius = (dist from start to temp end)
         *  MouseClick -> set end -> Invoke Callback
         */

        private Point dPtStart; //Rectangle, Line, Circle
        private Point dPtEnd; //Rectangle, Line, Circle
        private List<Point> dPath = new List<Point>(); //LineString, Polygon

        private Delegate _digitzationCallback;

        private bool _digitizationYetToStart = true;

        /// <summary>
        /// Starts the digitization process for a circle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizeCircle(CircleDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.Circle;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _circleCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts the digitization process for a line
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizeLine(LineDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.Line;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _lineCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts the digitization process for a point
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizePoint(PointDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.Point;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _pointCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts the digitization process for a polygon
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizePolygon(PolygonDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.Polygon;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _polygonCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizeLineString(LineStringDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.LineString;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _lineStringCustomDigitizationPrompt = customPrompt;
        }

        private LineDigitizationCallback _segmentCallback;

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="segmentCallback">The callback to be invoked when a new segment of the current line string is digitized</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizeLineString(LineStringDigitizationCallback callback, LineDigitizationCallback segmentCallback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.LineString;
            _digitzationCallback = callback;
            _segmentCallback = segmentCallback;
            _digitizationYetToStart = true;
            _lineStringCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts the digitization process for a rectangle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        public void DigitizeRectangle(RectangleDigitizationCallback callback, string customPrompt)
        {
            this.DigitizingType = MapDigitizationType.Rectangle;
            _digitzationCallback = callback;
            _digitizationYetToStart = true;
            _rectangleCustomDigitizationPrompt = customPrompt;
        }

        /// <summary>
        /// Starts circle digitization
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizeCircle(CircleDigitizationCallback callback)
        {
            DigitizeCircle(callback, null);
        }

        /// <summary>
        /// Starts digitize line
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizeLine(LineDigitizationCallback callback)
        {
            DigitizeLine(callback, null);
        }

        /// <summary>
        /// Starts point digitization
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizePoint(PointDigitizationCallback callback)
        {
            DigitizePoint(callback, null);
        }

        /// <summary>
        /// Starts polygon digitization
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizePolygon(PolygonDigitizationCallback callback)
        {
            DigitizePolygon(callback, null);
        }

        /// <summary>
        /// Starts line string digitization
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizeLineString(LineStringDigitizationCallback callback)
        {
            DigitizeLineString(callback, (string)null);
        }

        /// <summary>
        /// Starts line string digitization
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="segmentDigitized"></param>
        public void DigitizeLineString(LineStringDigitizationCallback callback, LineDigitizationCallback segmentDigitized)
        {
            DigitizeLineString(callback, segmentDigitized, null);
        }

        /// <summary>
        /// Starts rectangle digitization
        /// </summary>
        /// <param name="callback"></param>
        public void DigitizeRectangle(RectangleDigitizationCallback callback)
        {
            DigitizeRectangle(callback, null);
        }

        private void ResetDigitzationState()
        {
            _digitzationCallback = null;
            _segmentCallback = null;
            dPath.Clear();
            dPtEnd.X = dPtStart.Y = 0;
            dPtStart.X = dPtStart.Y = 0;
            this.DigitizingType = MapDigitizationType.None;
            _circleCustomDigitizationPrompt = null;
            _lineCustomDigitizationPrompt = null;
            _lineStringCustomDigitizationPrompt = null;
            _polygonCustomDigitizationPrompt = null;
            _pointCustomDigitizationPrompt = null;
            _rectangleCustomDigitizationPrompt = null;
            Invalidate();
        }

        private void OnCircleDigitized(Point ptStart, Point ptEnd)
        {
            var mapPt = ScreenToMapUnits(ptStart.X, ptStart.Y);
            var mapEnd = ScreenToMapUnits(ptEnd.X, ptEnd.Y);

            var radius = Math.Sqrt(Math.Pow(mapEnd.X - mapPt.X, 2) + Math.Pow(mapEnd.Y - mapPt.Y, 2));

            var cb = (CircleDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(mapPt.X, mapPt.Y, radius);
        }

        private void OnPolygonDigitized(List<Point> path)
        {
            double[,] coords = new double[path.Count, 2];
            for (int i = 0; i < path.Count; i++)
            {
                var pt = ScreenToMapUnits(path[i].X, path[i].Y);
                coords[i, 0] = pt.X;
                coords[i, 1] = pt.Y;
            }

            var cb = (PolygonDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(coords);
        }

        private void OnLineStringSegmentDigitized(Point p1, Point p2)
        {
            if (_segmentCallback != null)
            {
                var ptx1 = ScreenToMapUnits(p1.X, p1.Y);
                var ptx2 = ScreenToMapUnits(p2.X, p2.Y);
                _segmentCallback.Invoke(ptx1.X, ptx1.Y, ptx2.X, ptx2.Y);
            }
        }

        private void OnLineStringDigitized(List<Point> path)
        {
            double[,] coords = new double[path.Count, 2];
            for (int i = 0; i < path.Count; i++)
            {
                var pt = ScreenToMapUnits(path[i].X, path[i].Y);
                coords[i, 0] = pt.X;
                coords[i, 1] = pt.Y;
            }

            var cb = (LineStringDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(coords);
        }

        private void OnLineDigitized(Point start, Point end)
        {
            var mapStart = ScreenToMapUnits(start.X, start.Y);
            var mapEnd = ScreenToMapUnits(end.X, end.Y);

            var cb = (LineDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(mapStart.X, mapStart.Y, mapEnd.X, mapEnd.Y);
        }

        private void OnRectangleDigitized(Rectangle rect)
        {
            var mapLL = ScreenToMapUnits(rect.Left, rect.Bottom);
            var mapUR = ScreenToMapUnits(rect.Right, rect.Top);

            var cb = (RectangleDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(mapLL.X, mapLL.Y, mapUR.X, mapUR.Y);
        }

        private void OnPointDigitizationCompleted(Point p)
        {
            var mapPt = ScreenToMapUnits(p.X, p.Y);
            var cb = (PointDigitizationCallback)_digitzationCallback;
            ResetDigitzationState();
            cb(mapPt.X, mapPt.Y);
        }

        #endregion

        static ViewerRenderingOptions CreateMapRenderingOptions(short red, short green, short blue)
        {
            return new ViewerRenderingOptions("PNG", 2, Color.FromArgb(red, green, blue));
        }

        static ViewerRenderingOptions CreateSelectionRenderingOptions(short red, short green, short blue)
        {
            return new ViewerRenderingOptions("PNG", (1 | 4), Color.FromArgb(red, green, blue));
        }

        /// <summary>
        /// Load the viewer with the given runtime map
        /// </summary>
        /// <param name="map"></param>
        public void LoadMap(RuntimeMap map)
        {
            LoadMap(map, null);
        }

        /// <summary>
        /// Load the viewer with the given runtime map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="initialScale"></param>
        public void LoadMap(RuntimeMap map, double? initialScale)
        {
            _map = map;
            _map.StrictSelection = false;
            InitViewerFromMap(initialScale);
        }

        private void InitViewerFromMap(double? initialScale)
        {
            this.BackColor = _map.BackgroundColor;
            _map.DisplayWidth = this.Width;
            _map.DisplayHeight = this.Height;
            _selection = _map.Selection;
            _overlayRenderOpts = CreateMapRenderingOptions(0, 0, 255);
            _selectionRenderOpts = CreateSelectionRenderingOptions(0, 0, 255);

            var env = _map.MapExtent;

            _extX1 = _orgX1 = env.MinX;
            _extY2 = _orgY2 = env.MinY;
            _extX2 = _orgX2 = env.MaxX;
            _extY1 = _orgY1 = env.MaxY;

            if ((_orgX1 - _orgX2) == 0 || (_orgY1 - _orgY2) == 0)
            {
                _extX1 = _orgX1 = -.1;
                _extY2 = _orgX2 = .1;
                _extX2 = _orgY1 = -.1;
                _extY1 = _orgY2 = .1;
            }

            if (this.ConvertTiledGroupsToNonTiled)
            {
                var groups = _map.Groups;
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    group.Type = RuntimeMapGroup.kNormal;

                    var layers = _map.GetLayersOfGroup(group.Name);
                    for (int j = 0; j < layers.Length; j++)
                    {
                        layers[j].Type = RuntimeMapLayer.kDynamic;
                    }
                }
            }

#if VIEWER_DEBUG
            CreateDebugFeatureSource();
#endif
            this.Focus();

            //Reset history stack
            _viewHistory.Clear();
            OnPropertyChanged("ViewHistory");
            _viewHistoryIndex = -1;
            OnPropertyChanged("ViewHistoryIndex");

            var handler = this.MapLoaded;
            if (handler != null)
                handler(this, EventArgs.Empty);

            if (initialScale.HasValue)
                ZoomToScale(initialScale.Value);
            else
                InitialMapView();
        }

        /// <summary>
        /// Utility method to calculate the zoom scale for the give map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="mcsW"></param>
        /// <param name="mcsH"></param>
        /// <param name="devW"></param>
        /// <param name="devH"></param>
        /// <returns></returns>
        public static double CalculateScale(RuntimeMap map, double mcsW, double mcsH, int devW, int devH)
        {
            var mpu = map.MetersPerUnit;
            var mpp = GetMetersPerPixel(map.DisplayDpi);
            if (devH * mcsW > devW * mcsH)
                return mcsW * mpu / (devW * mpp); //width-limited
            else
                return mcsH * mpu / (devH * mpp); //height-limited
        }

        /// <summary>
        /// Gets or sets a value indicating whether tiled groups are converted to normal groups. Must be set before
        /// a map is loaded
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("If true, the map being viewed will have all its tiled groups converted to non-tiled groups. Tiled groups are not supported by this viewer and are not rendered")]
        [DefaultValue(false)]
        public bool ConvertTiledGroupsToNonTiled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether to use the RenderMap API instead of RenderDynamicOverlay if the map has tiled
        /// layers. RenderMap includes tiled layers as part of the output image, but will not take advantage
        /// of any tile caching mechanisms. Setting this property to true nullifies any effect of the 
        /// <see cref="P:Maestro.MapViewer.MapViewer.ConvertTiledGroupsToNonTiled"/> property
        /// </summary>
        [Category("MapGuide Viewer")] //NOXLATE
        [Description("If true, the viewer will use the RenderMap API instead of RenderDynamicOverlay allowing tiled layers to be rendered to the final image. Setting this property to true nullifies the ConvertTiledGroupsToNonTiled property")] //NOXLATE
        [DefaultValue(true)]
        public bool UseRenderMapIfTiledLayersExist { get; set; }

        /// <summary>
        /// Gets whether to respect the list of finite display scales in a map being viewed if there are any defined.
        /// If true, all zooms will "snap" to the nearest finite display scale. Otherwise, the viewer will disregard
        /// this list when zooming in or out.
        /// </summary>
        [Category("MapGuide Viewer")] //NOXLATE
        [Description("If true, all zooms will snap to the nearest finite display scale defined in the map being viewed")] //NOXLATE
        [DefaultValue(true)]
        public bool RespectFiniteDisplayScales { get; set; }

        /// <summary>
        /// Raised when the viewer has been initialized
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when the viewer has been initialized with a runtime map")]
        public event EventHandler MapLoaded;

        private System.Timers.Timer _delayedResizeTimer;

        void OnDelayResizeTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var action = new MethodInvoker(() =>
            {
                if (_map != null)
                {
                    Trace.TraceInformation("Performing delayed resize to (" + this.Width + ", " + this.Height + ")");
                    _map.DisplayWidth = this.Width;
                    _map.DisplayHeight = this.Height;
                    UpdateExtents();
                    RefreshMap(false);
                }
                _delayedResizeTimer.Stop();
                Trace.TraceInformation("Delayed resize timer stopped");
            });
            if (this.InvokeRequired)
                this.Invoke(action);
            else
                action();
        }

        void OnControlResized(object sender, EventArgs e)
        {
            if (_delayedResizeTimer == null)
            {
                _delayedResizeTimer = new System.Timers.Timer();
                _delayedResizeTimer.Elapsed += OnDelayResizeTimerElapsed;
                Trace.TraceInformation("Delay resize timer initialized");
            }
            if (_delayedResizeTimer.Enabled)
            {
                Trace.TraceInformation("Stopped delayed resize");
                _delayedResizeTimer.Stop();
            }

            _delayedResizeTimer.Interval = 500;
            _delayedResizeTimer.Start();
            Trace.TraceInformation("Delayed resize re-scheduled");
        }

        /// <summary>
        /// Clears the current selection
        /// </summary>
        public void ClearSelection()
        {
            //_provider.ClearSelection(_selection);
            _selection.LoadXml(string.Empty);
            _map.Save();

            if (_selectionImage != null)
            {
                _selectionImage.Dispose();
                _selectionImage = null;
            }

            var handler = this.SelectionChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);

            this.Refresh();
        }

        /// <summary>
        /// Gets the current runtime map
        /// </summary>
        /// <returns></returns>
        public RuntimeMap GetMap()
        {
            return _map;
        }

        private bool HasSelection()
        {
            return _selection.Count > 0;
        }

        private static int GetSelectionTotal(MapSelection sel)
        {
            int total = 0;
            for (int i = 0; i < sel.Count; i++)
            {
                total += sel[i].Count;
            }
            return total;
        }

        private MapDigitizationType _digitizingType = MapDigitizationType.None;

        /// <summary>
        /// Gets the type of object being currently digitized. If the digitization type is None, then
        /// the viewer is not currently digitizing
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapDigitizationType DigitizingType
        {
            get { return _digitizingType; }
            private set
            {
                if (_digitizingType.Equals(value))
                    return;

                if (value != MapDigitizationType.None)
                {
                    this.ActiveTool = MapActiveTool.None;
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }

                _digitizingType = value;

                OnPropertyChanged("DigitizingType");
            }
        }

        class RenderWorkArgs
        {
            public RenderWorkArgs() { this.UseRenderMap = false; }

            public bool UseRenderMap { get; set; }

            public ViewerRenderingOptions SelectionRenderingOptions { get; set; }

            public ViewerRenderingOptions MapRenderingOptions { get; set; }

            public bool RaiseEvents { get; set; }

            public bool InvalidateRegardless { get; set; }
        }

        class RenderResult
        {
            public Image Image { get; set; }

            public Image SelectionImage { get; set; }

            public bool RaiseEvents { get; set; }

            public bool InvalidateRegardless { get; set; }
        }

        /// <summary>
        /// Refreshes the current map view
        /// </summary>
        public void RefreshMap()
        {
            RefreshMap(true);
        }

        /// <summary>
        /// Updates the rendered selection. Call this method if you have manipulated the selection
        /// set outside of the viewer. This does not raise the <see cref="SelectionChanged"/> event
        /// </summary>
        public void UpdateSelection()
        {
            UpdateSelection(false);
        }

        /// <summary>
        /// Updates the rendered selection. Call this method if you have manipulated the selection
        /// set outside of the viewer
        /// </summary>
        /// <param name="raise">Indicates if the <see cref="SelectionChanged"/> event should be raised as well</param>
        public void UpdateSelection(bool raise)
        {
            RenderSelection();
            if (raise)
            {
                var handler = this.SelectionChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        internal void RenderSelection()
        {
            RenderSelection(false);
        }

        internal void RenderSelection(bool invalidateRegardless)
        {
            //This is our refresh action
            RefreshAction action = new RefreshAction(() => 
            {
                if (HasSelection())
                {
                    this.IsBusy = true;
                    UpdateSelectionRenderingOptions();
                    renderWorker.RunWorkerAsync(new RenderWorkArgs()
                    {
                        SelectionRenderingOptions = _selectionRenderOpts,
                        RaiseEvents = false,
                        InvalidateRegardless = invalidateRegardless
                    });
                }
                else
                {
                    if (invalidateRegardless)
                        this.Invalidate();
                }
            });

            //If an existing rendering operation is in progress queue it if 
            //there isn't one queued. Because there is no point in doing the
            //same thing more than once
            if (this.IsBusy)
            {
                if (_queuedRefresh == null) //No refresh operations currently queued
                    _queuedRefresh = action;
            }
            else //Otherwise execute it immediately
            {
                action();
            }
        }

        delegate void RefreshAction();

        RefreshAction _queuedRefresh = null;

        internal void RefreshMap(bool raiseEvents)
        {
            var h = this.MapRefreshing;
            if (h != null)
                h(this, EventArgs.Empty);

            //This is our refresh action
            RefreshAction action = new RefreshAction(() => 
            {
                var args = new RenderWorkArgs()
                {
                    UseRenderMap = this.UseRenderMapIfTiledLayersExist && this.HasTiledLayers,
                    MapRenderingOptions = _overlayRenderOpts,
                    RaiseEvents = raiseEvents
                };
                if (HasSelection())
                {
                    UpdateSelectionRenderingOptions();
                    args.SelectionRenderingOptions = _selectionRenderOpts;
                }
                this.IsBusy = true;
                renderWorker.RunWorkerAsync(args);
            });

            //If an existing rendering operation is in progress queue it if 
            //there isn't one queued. Because there is no point in doing the
            //same thing more than once
            if (this.IsBusy)
            {
                if (_queuedRefresh == null) //No refresh operations currently queued
                    _queuedRefresh = action;
            }
            else //Otherwise execute it immediately
            {
                action();   
            }
        }

        /// <summary>
        /// Raised when the viewer has started refreshing the map. This is to allow
        /// any actions dependent on map state to update themselves asynchronously 
        /// without needing to wait for the updated map to be rendered.
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when the viewer has started refreshing the map")]
        public event EventHandler MapRefreshing;

        /// <summary>
        /// Raised when the map has been refreshed and the updated map image has been rendered
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised after the viewer has refreshed")]
        public event EventHandler MapRefreshed;

        private bool _busy = false;

#if TRACE
        private Stopwatch _renderSw = new Stopwatch();
#endif

        /// <summary>
        /// Indicates whether a rendering operation is in progress
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBusy
        {
            get { return _busy; }
            private set
            {
                if (_busy.Equals(value))
                    return;

                _busy = value;
#if TRACE
                Trace.TraceInformation("IsBusy = " + _busy);
                if (value)
                {
                    _renderSw.Reset();
                    _renderSw.Start();
                }
                else
                {
                    _renderSw.Stop();
                    Trace.TraceInformation("Rendering operation took {0}ms", _renderSw.ElapsedMilliseconds);
                }
#endif
                OnPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Pans the view left by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        public void PanLeft(bool refresh)
        {
            PanTo(_extX1 + (_extX2 - _extX1) / 3, _extY2 + (_extY1 - _extY2) / 2, refresh);
        }

        /// <summary>
        /// Pans the view up by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        public void PanUp(bool refresh)
        {
            PanTo(_extX1 + (_extX2 - _extX1) / 2, _extY1 - (_extY1 - _extY2) / 3, refresh);
        }

        /// <summary>
        /// Pans the view right by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        public void PanRight(bool refresh)
        {
            PanTo(_extX2 - (_extX2 - _extX1) / 3, _extY2 + (_extY1 - _extY2) / 2, refresh);
        }

        /// <summary>
        /// Pans the view down by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        public void PanDown(bool refresh)
        {
            PanTo(_extX1 + (_extX2 - _extX1) / 2, _extY2 + (_extY1 - _extY2) / 3, refresh);
        }

        /// <summary>
        /// Zooms the extents.
        /// </summary>
        public void ZoomExtents()
        {
            var scale = CalculateScale(_map, (_orgX2 - _orgX1), (_orgY1 - _orgY2), this.Width, this.Height);
            ZoomToView(_orgX1 + ((_orgX2 - _orgX1) / 2), _orgY2 + ((_orgY1 - _orgY2) / 2), scale, true);
        }

        /// <summary>
        /// Zooms to the view defined by the specified extent
        /// </summary>
        /// <param name="llx"></param>
        /// <param name="lly"></param>
        /// <param name="urx"></param>
        /// <param name="ury"></param>
        public void ZoomToExtents(double llx, double lly, double urx, double ury)
        {
            var scale = CalculateScale(_map, (urx - llx), (ury - lly), this.Width, this.Height);
            ZoomToView(llx + ((urx - llx) / 2), ury + ((lly - ury) / 2), scale, true);
        }

        /// <summary>
        /// Zooms to scale.
        /// </summary>
        /// <param name="scale">The scale.</param>
        public void ZoomToScale(double scale)
        {
            ZoomToView(_extX1 + (_extX2 - _extX1) / 2, _extY2 + (_extY1 - _extY2) / 2, scale, true);
        }

        /// <summary>
        /// Zooms to the specified map view
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        /// <param name="refresh"></param>
        public void ZoomToView(double x, double y, double scale, bool refresh)
        {
            ZoomToView(x, y, scale, refresh, true);
        }

        internal void PanTo(double x, double y, bool refresh)
        {
            ZoomToView(x, y, _map.ViewScale, refresh);
        }

        private void UpdateExtents()
        {
            //Update current extents
            double mpu = _map.MetersPerUnit;
            double scale = _map.ViewScale;
            double mpp = GetMetersPerPixel(_map.DisplayDpi);
            var coord = _map.ViewCenter;

            var mcsWidth = _map.DisplayWidth * mpp * scale / mpu;
            var mcsHeight = _map.DisplayHeight * mpp * scale / mpu;

            _extX1 = coord.X - mcsWidth / 2;
            _extY1 = coord.Y + mcsHeight / 2;
            _extX2 = coord.X + mcsWidth / 2;
            _extY2 = coord.Y - mcsHeight / 2;
        }
        
        /// <summary>
        /// Gets the current view extent
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public void GetViewExtent(out double minX, out double minY, out double maxX, out double maxY)
        {
            //NOTE: Something strange about the AJAX viewer code we grafted this from. Y2 is not the max Y, Y1 is the max Y.
            minX = _extX1;
            minY = _extY2;
            maxX = _extX2;
            maxY = _extY1;
        }

        private bool PruneHistoryEntriesFromCurrentView()
        {
            if (_viewHistoryIndex < _viewHistory.Count - 1)
            {
                int removed = 0;
                for (int i = _viewHistory.Count - 1; i > _viewHistoryIndex; i--)
                {
                    _viewHistory.RemoveAt(i);
                    removed++;
                }
                return removed > 0;
            }
            return false;
        }

        internal void ZoomToView(double x, double y, double scale, bool refresh, bool raiseEvents)
        {
            ZoomToView(x, y, scale, refresh, raiseEvents, true);
        }

        /// <summary>
        /// Navigates to the previous view in the history stack
        /// </summary>
        public void PreviousView()
        {
            var newIndex = _viewHistoryIndex - 1;
            if (newIndex < 0)
                return;

            var view = _viewHistory[newIndex];
            ZoomToView(view.X, view.Y, view.Scale, true, true, false);
            _viewHistoryIndex = newIndex;
            OnPropertyChanged("ViewHistoryIndex");
        }

        /// <summary>
        /// Navigates to the next view in the history stack
        /// </summary>
        public void NextView()
        {
            //Cannot advance from current view
            if (_viewHistoryIndex == _viewHistory.Count - 1)
                return;

            var newIndex = _viewHistoryIndex + 1;
            if (newIndex > _viewHistory.Count - 1)
                return;

            var view = _viewHistory[newIndex];
            ZoomToView(view.X, view.Y, view.Scale, true, true, false);
            _viewHistoryIndex = newIndex;
            OnPropertyChanged("ViewHistoryIndex");
        }

        /// <summary>
        /// Gets the current index in the view history stack
        /// </summary>
        public int ViewHistoryIndex
        {
            get { return _viewHistoryIndex; }
        }

        /// <summary>
        /// Gets the view history stack. The first item being the earliest and the last item being the most recent.
        /// </summary>
        public ReadOnlyCollection<MapViewHistoryEntry> ViewHistory
        {
            get { return _viewHistory.AsReadOnly(); }
        }

        internal void ZoomToView(double x, double y, double scale, bool refresh, bool raiseEvents, bool addToHistoryStack)
        {
            var newScale = NormalizeScale(scale);
            if (_map.FiniteDisplayScaleCount > 0 && this.RespectFiniteDisplayScales)
                newScale = GetNearestFiniteScale(scale);
            if (addToHistoryStack)
            {
                //If not current view, then any entries from the current view index are no longer needed
                if (ViewHistoryIndex < _viewHistory.Count - 1)
                    PruneHistoryEntriesFromCurrentView();

                _viewHistory.Add(new MapViewHistoryEntry(x, y, newScale));
                OnPropertyChanged("ViewHistory");
                _viewHistoryIndex = _viewHistory.Count - 1;
                OnPropertyChanged("ViewHistoryIndex");
            }

            _map.SetViewCenter(x, y);
#if VIEWER_DEBUG
            UpdateCenterDebugPoint();
            //var mapExt = _map.MapExtent;
            //var dataExt = _map.DataExtent;
            Trace.TraceInformation("Map Extent is ({0},{1} {2},{3})", mapExt.LowerLeftCoordinate.X, mapExt.LowerLeftCoordinate.Y, mapExt.UpperRightCoordinate.X, mapExt.UpperRightCoordinate.Y);
            Trace.TraceInformation("Data Extent is ({0},{1} {2},{3})", dataExt.LowerLeftCoordinate.X, dataExt.LowerLeftCoordinate.Y, dataExt.UpperRightCoordinate.X, dataExt.UpperRightCoordinate.Y);

            Trace.TraceInformation("Center is (" + x + ", " + y + ")");
#endif
            var oldScale = _map.ViewScale;
            _map.ViewScale = newScale;

            if (oldScale != _map.ViewScale)
            {
                var handler = this.MapScaleChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

            UpdateExtents();

#if VIEWER_DEBUG
            Trace.TraceInformation("Current extents is ({0},{1} {2},{3})", _extX1, _extY1, _extX2, _extY2);
#endif

            //Then refresh
            if (refresh)
                RefreshMap(raiseEvents);
        }

        private double GetNearestFiniteScale(double scale)
        {
            return _map.GetFiniteDisplayScaleAt(GetFiniteScaleIndex(scale));
        }

        private int GetFiniteScaleIndex(double reqScale)
        {
            var index = 0;
            var scaleCount = _map.FiniteDisplayScaleCount;
            if (scaleCount > 0)
            {
                var bestDiff = Math.Abs(_map.GetFiniteDisplayScaleAt(0) - reqScale);
                for (var i = 1; i < scaleCount; i++)
                {
                    var scaleDiff = Math.Abs(_map.GetFiniteDisplayScaleAt(i) - reqScale);
                    if (scaleDiff < bestDiff)
                    {
                        index = i;
                        bestDiff = scaleDiff;
                        if (bestDiff == 0)
                        {
                            //perfect match
                            break;
                        }
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Raised when the scale of the current runtime map has changed
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when the zoom scale of the map has changed")]
        public event EventHandler MapScaleChanged;

        /// <summary>
        /// Raised when the selection has changed. Note that programmatic selection modifications
        /// will not raise this event.
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when active viewer selection has changed")]
        public event EventHandler SelectionChanged;

        private void renderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _map.Save(); //We must sync up runtime map state before rendering

            var args = (RenderWorkArgs)e.Argument;
            var res = new RenderResult() { RaiseEvents = args.RaiseEvents, InvalidateRegardless = args.InvalidateRegardless };
            if (args.MapRenderingOptions != null)
            {
                if (args.UseRenderMap)
                    res.Image = Image.FromStream(_map.Render(args.MapRenderingOptions.Format));
                else
                    res.Image = Image.FromStream(_map.RenderDynamicOverlay(null, args.MapRenderingOptions.Format, args.MapRenderingOptions.Color, args.MapRenderingOptions.Behavior));
            }
            if (args.SelectionRenderingOptions != null)
            {
                //HACK: HTTP provider is stateless, so passing the selection is not only redundant, but will probably break on large selections.
                var sel = (_map.CurrentConnection.ProviderName.ToUpper().Equals("MAESTRO.HTTP")) ? null : _map.Selection;
                res.SelectionImage = Image.FromStream(_map.RenderDynamicOverlay(sel, args.SelectionRenderingOptions.Format, args.SelectionRenderingOptions.Color, args.SelectionRenderingOptions.Behavior));
            }

            e.Result = res;
        }

        private void renderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.IsBusy = AreWorkersBusy();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error");
            }
            else
            {
                var res = (RenderResult)e.Result;
                //reset translation
                translate = new System.Drawing.Point();

                bool bInvalidate = false;
                //set the image
                if (res.Image != null)
                {
                    if (this.Image != null)
                    {
                        this.Image.Dispose();
                        this.Image = null;
                    }
                    Trace.TraceInformation("Set map image");
                    this.Image = res.Image;
                    bInvalidate = true;
                }
                if (res.SelectionImage != null)
                {
                    if (_selectionImage != null)
                    {
                        _selectionImage.Dispose();
                        _selectionImage = null;
                    }
                    Trace.TraceInformation("Set selection image");
                    _selectionImage = res.SelectionImage;
                    bInvalidate = true;
                }

                //If there is a queued refresh action, execute it now
                if (_queuedRefresh != null)
                {
                    Trace.TraceInformation("Executing queued rendering operation");
                    _queuedRefresh();
                    _queuedRefresh = null;
                }
                else 
                {
                    if (bInvalidate || res.InvalidateRegardless)
                        Invalidate(true);

                    /*
                    var center = _map.ViewCenter;
                    var ext = _map.DataExtent;
                    System.Diagnostics.Trace.TraceInformation(
                        "**POST-RENDER**{2}Map Center: {0}, {1}{2}Lower left: {3}, {4}{2}Upper Right: {5}, {6}",
                        center.Coordinate.X,
                        center.Coordinate.Y,
                        Environment.NewLine,
                        ext.LowerLeftCoordinate.X,
                        ext.LowerLeftCoordinate.Y,
                        ext.UpperRightCoordinate.X,
                        ext.UpperRightCoordinate.Y);
                    */
                    if (res.RaiseEvents)
                    {
                        var handler = this.MapRefreshed;
                        if (handler != null)
                            handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool AreWorkersBusy()
        {
            return renderWorker.IsBusy;
        }

        /// <summary>
        /// Zooms to the initial map view
        /// </summary>
        public void InitialMapView()
        {
            InitialMapView(true);
        }

        private void InitialMapView(bool refreshMap)
        {
            var scale = CalculateScale(_map, (_orgX2 - _orgX1), (_orgY1 - _orgY2), this.Width, this.Height);
            ZoomToView(_orgX1 + ((_orgX2 - _orgX1) / 2), _orgY2 + ((_orgY1 - _orgY2) / 2), scale, refreshMap);
        }

        private static Rectangle? GetRectangle(Point dPtStart, Point dPtEnd)
        {
            int? left = null;
            int? right = null;
            int? top = null;
            int? bottom = null;

            if (dPtEnd.X < dPtStart.X)
            {
                if (dPtEnd.Y < dPtStart.Y)
                {
                    left = dPtEnd.X;
                    bottom = dPtStart.Y;
                    top = dPtEnd.Y;
                    right = dPtStart.X;
                }
                else if (dPtEnd.Y > dPtStart.Y)
                {
                    left = dPtEnd.X;
                    bottom = dPtEnd.Y;
                    top = dPtStart.Y;
                    right = dPtStart.X;
                }
                else
                {
                    //Equal
                }
            }
            else
            {
                if (dPtEnd.X > dPtStart.X)
                {
                    if (dPtEnd.Y < dPtStart.Y)
                    {
                        left = dPtStart.X;
                        bottom = dPtStart.Y;
                        top = dPtEnd.Y;
                        right = dPtEnd.X;
                    }
                    else if (dPtEnd.Y > dPtStart.Y)
                    {
                        left = dPtStart.X;
                        bottom = dPtEnd.Y;
                        top = dPtStart.Y;
                        right = dPtEnd.X;
                    }
                    else
                    {
                        //Equal
                    }
                }
                //else equal
            }
            if (left.HasValue && right.HasValue && top.HasValue && bottom.HasValue)
            {
                return new Rectangle(left.Value, top.Value, (right.Value - left.Value), (bottom.Value - top.Value));
            }
            return null;
        }

        private double _zoomInFactor;
        private double _zoomOutFactor;

        /// <summary>
        /// Gets or sets the factor by which to multiply the scale to zoom in
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The zoom in factor")]
        public double ZoomInFactor
        {
            get { return _zoomInFactor; }
            set
            {
                if (value.Equals(_zoomInFactor))
                    return;
                _zoomInFactor = value;
                OnPropertyChanged("ZoomInFactor");
            }
        }

        /// <summary>
        /// Gets or sets the factor by which to multiply the scale to zoom out
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The zoom out factor")]
        public double ZoomOutFactor
        {
            get { return _zoomOutFactor; }
            set
            {
                if (value.Equals(_zoomOutFactor))
                    return;
                _zoomOutFactor = value;
                OnPropertyChanged("ZoomOutFactor");
            }
        }

        private static string MakeWktPolygon(double x1, double y1, double x2, double y2)
        {
            return "POLYGON((" + x1 + " " + y1 + ", " + x2 + " " + y1 + ", " + x2 + " " + y2 + ", " + x1 + " " + y2 + ", " + x1 + " " + y1 + "))";
        }

        private int? _lastTooltipX;
        private int? _lastTooltipY;

        private string QueryFirstVisibleTooltip(int x, int y)
        {
            //No intialized map
            if (_map == null)
                return "";

            //No change in position
            if (_lastTooltipX == x && _lastTooltipY == y && !string.IsNullOrEmpty(_activeTooltipText))
                return _activeTooltipText;

            if (_lastTooltipX.HasValue && _lastTooltipY.HasValue)
            {
                //Not considered a significant change
                if (Math.Abs(x - _lastTooltipX.Value) < MOUSE_TOOLTIP_MOVE_TOLERANCE ||
                    Math.Abs(y - _lastTooltipY.Value) < MOUSE_TOOLTIP_MOVE_TOLERANCE)
                    return _activeTooltipText;
            }

            _lastTooltipX = x;
            _lastTooltipY = y;

            var pt1 = ScreenToMapUnits(x - this.PointPixelBuffer, x - this.PointPixelBuffer);
            var pt2 = ScreenToMapUnits(x + this.PointPixelBuffer, y + this.PointPixelBuffer);
            //Unlike mg-desktop, this is actually easy API-wise
            var res = _map.QueryMapFeatures(
                MakeWktPolygon(pt1.X, pt1.Y, pt2.X, pt2.Y),
                1,
                false,
                "INTERSECTS",
                new QueryMapOptions()
                {
                    LayerNames = GetAllLayerNames(_map),
                    LayerAttributeFilter = QueryMapFeaturesLayerAttributes.VisibleWithToolTips
                });
            try
            {
                _tooltipDoc.LoadXml(res);
                return _tooltipDoc.DocumentElement["Tooltip"].InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        private XmlDocument _tooltipDoc = new XmlDocument();

        private static string[] GetAllLayerNames(RuntimeMap map)
        {
            var names = new List<string>();
            for (int i = 0; i < map.Layers.Count; i++)
            {
                names.Add(map.Layers[i].Name);
            }
            return names.ToArray();
        }

        private static bool IsRasterClass(ClassDefinition cls)
        {
            foreach (var prop in cls.Properties)
            {
                if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Raster)
                    return true;
            }
            return false;
        }

        private static bool IsRasterLayer(RuntimeMapLayer layer)
        {
            var cls = layer.GetClassDefinition();

            return IsRasterClass(cls);
        }

        /// <summary>
        /// Gets the current buffered image
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image GetCurrentImage()
        {
            var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, this.ClientRectangle);
            return bmp;
        }

        /// <summary>
        /// Copies the image of the current map to the clipboard
        /// </summary>
        public void CopyMap()
        {
            Clipboard.SetImage(this.GetCurrentImage());
        }

        /// <summary>
        /// Selects features from all selectable layers that intersects the given geometry
        /// </summary>
        /// <param name="wkt">The geometry wkt</param>
        /// <param name="maxFeatures">The maximum number of features to select. Specify -1 to select all features.</param>
        public void SelectByWkt(string wkt, int maxFeatures)
        {
            //Don't select if dragging. This is the cause of the failure to render
            //multiple selections, which required a manual refresh afterwards
            if (isDragging)
                return;

#if TRACE
            var sw = new Stopwatch();
            sw.Start();
#endif
            _map.QueryMapFeatures(wkt, maxFeatures, true, "INTERSECTS", CreateQueryOptionsForSelection());
#if TRACE
            sw.Stop();
            Trace.TraceInformation("Selection processing completed in {0}ms", sw.ElapsedMilliseconds);
#endif

            RenderSelection(true); //This is either async or queued up. Either way do this before firing off selection changed
            var handler = this.SelectionChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private QueryMapOptions CreateQueryOptionsForSelection()
        {
            return new QueryMapOptions()
            {
                LayerAttributeFilter = QueryMapFeaturesLayerAttributes.OnlySelectable | QueryMapFeaturesLayerAttributes.OnlyVisible
            };
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Resize event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            OnControlResized(this, e);
            base.OnResize(e);
        }

        #region Mouse handlers

        private void OnMapMouseLeave(object sender, EventArgs e)
        {
            //Need to do this otherwise a tooltip query is made at the viewer boundary 
 		    if (delayTooltipTimer != null && delayTooltipTimer.Enabled) 
 		        delayTooltipTimer.Stop(); 
        }

        private void OnMapMouseDown(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            HandleMouseDown(e);
        }

        private void OnMapMouseMove(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            HandleMouseMove(e);
        }
        
        private void OnMapMouseUp(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            HandleMouseUp(e);
        }

        private void OnMapMouseWheel(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            this.Focus();
            HandleMouseWheel(e);
        }

        private void OnMapMouseClick(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            this.Focus();
            HandleMouseClick(e);
        }

        private void OnMapMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (IsBusy) return;
            this.Focus();
            HandleMouseDoubleClick(e);
        }

        private void HandleMouseDoubleClick(MouseEventArgs e)
        {
            //Not enough points to constitute a line string or polygon
            if (dPath.Count < 2)
                return;

            if (this.DigitizingType == MapDigitizationType.LineString)
            {
                //Fix the last one, can't edit last one because points are value types
                dPath.RemoveAt(dPath.Count - 1);
                dPath.Add(new Point(e.X, e.Y));
                OnLineStringDigitized(dPath);
            }
            else if (this.DigitizingType == MapDigitizationType.Polygon)
            {
                //Fix the last one, can't edit last one because points are value types
                dPath.RemoveAt(dPath.Count - 1);
                dPath.Add(new Point(e.X, e.Y));
                OnPolygonDigitized(dPath);
            }
        }

        private double? delayRenderScale;
        private PointF? delayRenderViewCenter;
        private float? mouseWheelSx = null;
        private float? mouseWheelSy = null;
        private float? mouseWheelTx = null;
        private float? mouseWheelTy = null;
        private int? mouseWheelDelta = null;
        private System.Timers.Timer delayRenderTimer = null;

        private void HandleMouseWheel(MouseEventArgs e)
        {
            if (delayRenderTimer == null)
            {
                delayRenderTimer = new System.Timers.Timer();
                delayRenderTimer.Enabled = false;
                delayRenderTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnDelayRender);
                delayRenderTimer.Interval = this.MouseWheelDelayRenderInterval;
            }

            delayRenderTimer.Stop();
            delayRenderTimer.Start();
            Trace.TraceInformation("Postponed delay render");
            Trace.TraceInformation("Mouse delta: " + e.Delta + " (" + (e.Delta > 0 ? "Zoom in" : "Zoom out") + ")");
            //Negative delta = zoom out, Positive delta = zoom in
            //deltas are in units of 120, so treat each multiple of 120 as a "zoom unit"

            if (!mouseWheelSx.HasValue && !mouseWheelSy.HasValue)
            {
                mouseWheelSx = 1.0f;
                mouseWheelSy = 1.0f;
            }

            if (!mouseWheelDelta.HasValue)
                mouseWheelDelta = 0;

            if (e.Delta > 0) //Zoom In
            {
                mouseWheelDelta++;
                mouseWheelSx -= 0.1f;
                mouseWheelSy -= 0.1f;
                Invalidate();
            }
            else if (e.Delta < 0) //Zoom Out
            {
                mouseWheelDelta--;
                mouseWheelSx += 0.1f;
                mouseWheelSy += 0.1f;
                Invalidate();
            }

            Trace.TraceInformation("Delta units is: " + mouseWheelDelta);

            //Completely ripped the number crunching here from the AJAX viewer with no sense of shame whatsoever :)
            delayRenderScale = GetNewScale(_map.ViewScale, mouseWheelDelta.Value);
            double zoomChange = _map.ViewScale / delayRenderScale.Value;

            //Determine the center of the new, zoomed map, in current screen device coords
            double screenZoomCenterX = e.X - (e.X - this.Width / 2) / zoomChange;
            double screenZoomCenterY = e.Y - (e.Y - this.Height / 2) / zoomChange;
            delayRenderViewCenter = ScreenToMapUnits(screenZoomCenterX, screenZoomCenterY);

            var mpu = _map.MetersPerUnit;
            var mpp = GetMetersPerPixel(_map.DisplayDpi);
            var w = (_extX2 - _extX1) * _map.MetersPerUnit / (delayRenderScale * mpp);
            if (w > 20000)
            {
                w = 20000;
            }
            var h = w * ((double)this.Height / (double)this.Width);
            var xClickOffset = screenZoomCenterX - this.Width / 2;
            var yClickOffset = screenZoomCenterY - this.Height / 2;

            //Set the paint transforms. Will be reset once the delayed render is fired away
            mouseWheelTx = (float)((double)this.Width / 2 - w / 2 - xClickOffset * zoomChange);
            mouseWheelTy = (float)((double)this.Height / 2 - h / 2 - yClickOffset * zoomChange);
            mouseWheelSx = (float)(w / (double)this.Width);
            mouseWheelSy = (float)(h / (double)this.Height);

            Trace.TraceInformation("Paint transform (tx: " + mouseWheelTx + ", ty: " + mouseWheelTy + ", sx: " + mouseWheelSx + ", sy: " + mouseWheelSy + ")");
        }

        static double GetMetersPerPixel(int dpi)
        {
            return 0.0254 / dpi;
        }

        double GetNewScale(double currentScale, int wheelZoomDelta)
        {
            var newScale = currentScale;
            /*
            //handle finite zoom scales for tiled map
            if (finscale)
            {
                var newScaleIndex = sci - wheelDelta;
                if (newScaleIndex < 0)
                {
                    newScaleIndex = 0;
                }
                if (newScaleIndex > scales.length - 1)
                {
                    newScaleIndex = scales.length - 1;
                }
                newScale = scales[newScaleIndex];
            }
            //no finite zoom scales (untiled map)
            else */
            {
                var zoomChange = Math.Pow(1.5, wheelZoomDelta);
                newScale = zoomChange > 0 ? currentScale / zoomChange : this.MaxScale;
                newScale = NormalizeScale(newScale);
            }
            return newScale;
        }

        double NormalizeScale(double scale)
        {
            if (scale < this.MinScale)
                return this.MinScale;
            if (scale > this.MaxScale)
                return this.MaxScale;
            return scale;
        }

        void OnDelayRender(object sender, System.Timers.ElapsedEventArgs e)
        {
            Trace.TraceInformation("Delay rendering");
            Trace.TraceInformation("Set new map coordinates to (" + delayRenderViewCenter.Value.X + ", " + delayRenderViewCenter.Value.Y + " at " + delayRenderScale.Value + ")");
            ResetMouseWheelPaintTransforms();
            MethodInvoker action = () => { ZoomToView(delayRenderViewCenter.Value.X, delayRenderViewCenter.Value.Y, delayRenderScale.Value, true); };
            if (this.InvokeRequired)
                this.Invoke(action);
            else
                action();
        }

        private void ResetMouseWheelPaintTransforms()
        {
            if (delayRenderTimer != null)
                delayRenderTimer.Stop();
            mouseWheelSx = null;
            mouseWheelSy = null;
            mouseWheelTx = null;
            mouseWheelTy = null;
            mouseWheelDelta = 0;
            Trace.TraceInformation("Mouse wheel paint transform reset");
        }

        private void HandleMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            if (this.DigitizingType != MapDigitizationType.None)
            {
                //Points are easy, one click and you're done
                if (this.DigitizingType == MapDigitizationType.Point)
                {
                    OnPointDigitizationCompleted(new Point(e.X, e.Y));
                }
                else
                {
                    //Check first click in digitization
                    if (_digitizationYetToStart)
                    {
                        if (this.DigitizingType == MapDigitizationType.LineString ||
                            this.DigitizingType == MapDigitizationType.Polygon)
                        {
                            dPath.Add(new Point(e.X, e.Y));
                            dPath.Add(new Point(e.X, e.Y)); //This is a transient one
                        }
                        else
                        {
                            dPtStart.X = e.X;
                            dPtStart.Y = e.Y;
                        }
                        _digitizationYetToStart = false;
                    }
                    else
                    {
                        if (this.DigitizingType == MapDigitizationType.LineString ||
                            this.DigitizingType == MapDigitizationType.Polygon)
                        {
                            var pt = dPath[dPath.Count - 1];
                            pt.X = e.X;
                            pt.Y = e.Y;
                            dPath.Add(new Point(e.X, e.Y)); //This is a transient one
                            OnLineStringSegmentDigitized(dPath[dPath.Count - 3], dPath[dPath.Count - 2]);
                        }
                        else
                        {
                            //Fortunately, these are all 2-click affairs meaning this is
                            //the second click
                            switch (this.DigitizingType)
                            {
                                case MapDigitizationType.Circle:
                                    {
                                        dPtEnd.X = e.X;
                                        dPtEnd.Y = e.Y;
                                        OnCircleDigitized(dPtStart, dPtEnd);
                                    }
                                    break;
                                case MapDigitizationType.Line:
                                    {
                                        dPtEnd.X = e.X;
                                        dPtEnd.Y = e.Y;
                                        OnLineDigitized(dPtStart, dPtEnd);
                                    }
                                    break;
                                case MapDigitizationType.Rectangle:
                                    {
                                        dPtEnd.X = e.X;
                                        dPtEnd.Y = e.Y;
                                        var rect = GetRectangle(dPtStart, dPtEnd);
                                        if (rect.HasValue)
                                            OnRectangleDigitized(rect.Value);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.ActiveTool == MapActiveTool.Select)
                {
                    var mapPt1 = ScreenToMapUnits(e.X - this.PointPixelBuffer, e.Y - this.PointPixelBuffer);
                    var mapPt2 = ScreenToMapUnits(e.X + this.PointPixelBuffer, e.Y + this.PointPixelBuffer);

                    var env = ObjectFactory.CreateEnvelope(
                        Math.Min(mapPt1.X, mapPt2.X),
                        Math.Min(mapPt1.Y, mapPt2.Y),
                        Math.Max(mapPt1.X, mapPt2.X),
                        Math.Max(mapPt1.Y, mapPt2.Y));

                    SelectByWkt(MakeWktPolygon(env.MinX, env.MinY, env.MaxX, env.MaxY), 1);
                }
                else if (this.ActiveTool == MapActiveTool.ZoomIn)
                {
                    if (!isDragging)
                    {
                        var mapPt = ScreenToMapUnits(e.X, e.Y);
                        var scale = _map.ViewScale;
                        ZoomToView(mapPt.X, mapPt.Y, scale * ZoomInFactor, true);
                    }
                }
                else if (this.ActiveTool == MapActiveTool.ZoomOut)
                {
                    if (!isDragging)
                    {
                        var mapPt = ScreenToMapUnits(e.X, e.Y);
                        var scale = _map.ViewScale;
                        ZoomToView(mapPt.X, mapPt.Y, scale * ZoomOutFactor, true);
                    }
                }
            }
        }

        private void HandleMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragStart = e.Location;
                Trace.TraceInformation("Drag started at (" + dragStart.X + ", " + dragStart.Y + ")");

                switch (this.ActiveTool)
                {
                    case MapActiveTool.Pan:
                        Trace.TraceInformation("START PANNING");
                        break;
                    case MapActiveTool.Select:
                        Trace.TraceInformation("START SELECT");
                        break;
                    case MapActiveTool.ZoomIn:
                        Trace.TraceInformation("START ZOOM");
                        break;
                }
            }
        }

        private System.Drawing.Point translate;

        private System.Drawing.Point dragStart;
        bool isDragging = false;
        
        private int _mouseX;
        private int _mouseY;
        
        private string _activeTooltipText;

        private int _mouseDx;
        private int _mouseDy;

        /// <summary>
        /// A mouse is considered to have moved if the differerence in either X or Y directions is greater
        /// than this number
        /// </summary>
        const int MOUSE_TOOLTIP_MOVE_TOLERANCE = 10;

        private void HandleMouseMove(MouseEventArgs e)
        {
            if (_mouseX == e.X &&
                _mouseY == e.Y)
            {
                return;
            }

            //Record displacement
            _mouseDx = e.X - _mouseX;
            _mouseDy = e.Y - _mouseY;

            _mouseX = e.X;
            _mouseY = e.Y;

            var mapPt = ScreenToMapUnits(e.X, e.Y);
            OnMouseMapPositionChanged(mapPt.X, mapPt.Y);

            if (this.ActiveTool == MapActiveTool.Pan || this.ActiveTool == MapActiveTool.Select || this.ActiveTool == MapActiveTool.ZoomIn)
            {
                if (e.Location != dragStart && !isDragging && e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                }

                if (this.ActiveTool == MapActiveTool.Pan)
                {
                    if (isDragging)
                    {
                        translate = new System.Drawing.Point(e.X - dragStart.X, e.Y - dragStart.Y);
                    }
                }

                // FIXME: 
                //
                // We really need a JS setTimeout() equivalent for C# because that's what we want
                // to do here, set a delayed call to QueryFirstVisibleTooltip() that is aborted if
                // the mouse pointer has moved significantly since the last time.
                //
                // A timer based approach could probably work, but I haven't figured out the best 
                // way yet.

                this.TooltipsEnabled = !isDragging && this.FeatureTooltipsEnabled;

                //Only query for tooltips if not digitizing
                if (this.DigitizingType == MapDigitizationType.None &&
                   (this.ActiveTool == MapActiveTool.Select || this.ActiveTool == MapActiveTool.Pan) &&
                    this.TooltipsEnabled)
                {
                    if (delayTooltipTimer == null) 
 		            { 
 		                delayTooltipTimer = new System.Timers.Timer(); 
 		                delayTooltipTimer.Enabled = false; 
 		                delayTooltipTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnDelayTooltipTimerElapsed); 
 		                delayTooltipTimer.Interval = this.TooltipDelayInterval; 
 		            } 
 		 
 		            _delayTooltipQueryPoint = new Point(e.X, e.Y); 
 		            delayTooltipTimer.Start(); 
 		 
 		            if (Math.Abs(e.X - _lastTooltipQueryX) > 2 || Math.Abs(e.Y - _lastTooltipQueryY) > 2) 
 		            { 
 		                _activeTooltipText = null; 
 		                Invalidate(); 
 		            } 
                }
                else
                {
                    _activeTooltipText = null;
                }

                if (e.Button == MouseButtons.Left || !string.IsNullOrEmpty(_activeTooltipText))
                    Invalidate();
            }
            else if (this.DigitizingType != MapDigitizationType.None)
            {
                if (dPath.Count >= 2)
                {
                    //Fix the last one, can't edit last one because points are value types
                    dPath.RemoveAt(dPath.Count - 1);
                    dPath.Add(new Point(e.X, e.Y));
                    Trace.TraceInformation("Updating last point of a {0} point path", dPath.Count);
                }
                Invalidate();
            }
        }

        void OnDelayTooltipTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) 
 		{ 
 		    delayTooltipTimer.Stop(); 
 		    if (_delayTooltipQueryPoint.HasValue) 
 		    { 
 		        _activeTooltipText = QueryFirstVisibleTooltip(_delayTooltipQueryPoint.Value.X, _delayTooltipQueryPoint.Value.Y); 
 		        _lastTooltipQueryX = _delayTooltipQueryPoint.Value.X; 
 		        _lastTooltipQueryY = _delayTooltipQueryPoint.Value.Y; 
 		        _delayTooltipQueryPoint = null; 
 		        Invalidate(); 
 		    } 
 		} 
 		 
 		private int _lastTooltipQueryX; 
 		private int _lastTooltipQueryY; 
 		private Point? _delayTooltipQueryPoint = null; 
 		private System.Timers.Timer delayTooltipTimer = null; 

        private void HandleMouseUp(MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;

                if (this.ActiveTool == MapActiveTool.Pan)
                {
                    //FIXME: This is not perfect. The view will be slightly off of where you released the mouse button

                    //System.Diagnostics.Trace.TraceInformation("Dragged screen distance (" + translate.X + ", " + translate.Y + ")");

                    int dx = e.X - dragStart.X;
                    int dy = e.Y - dragStart.Y;

                    var centerScreen = new Point(this.Location.X + (this.Width / 2), this.Location.Y + (this.Height / 2));

                    centerScreen.X -= translate.X;
                    centerScreen.Y -= translate.Y;

                    var pt = _map.ViewCenter;
                    var coord = ScreenToMapUnits(centerScreen.X, centerScreen.Y);

                    double mdx = coord.X - pt.X;
                    double mdy = coord.Y - pt.Y;

                    ZoomToView(coord.X, coord.Y, _map.ViewScale, true);
                    Trace.TraceInformation("END PANNING");
                }
                else if (this.ActiveTool == MapActiveTool.Select)
                {
                    var mapPt = ScreenToMapUnits(e.X, e.Y);
                    var mapDragPt = ScreenToMapUnits(dragStart.X, dragStart.Y);
                    var env = ObjectFactory.CreateEnvelope(
                        Math.Min(mapPt.X, mapDragPt.X),
                        Math.Min(mapPt.Y, mapDragPt.Y),
                        Math.Max(mapPt.X, mapDragPt.X),
                        Math.Max(mapPt.Y, mapDragPt.Y));
                    SelectByWkt(MakeWktPolygon(env.MinX, env.MinY, env.MaxX, env.MaxY), -1);
                }
                else if (this.ActiveTool == MapActiveTool.ZoomIn)
                {
                    var mapPt = ScreenToMapUnits(e.X, e.Y);
                    var mapDragPt = ScreenToMapUnits(dragStart.X, dragStart.Y);

                    PointF ll;
                    PointF ur;

                    if (mapPt.X <= mapDragPt.X && mapPt.Y <= mapDragPt.Y)
                    {
                        ll = mapPt;
                        ur = mapDragPt;
                    }
                    else
                    {
                        ll = mapDragPt;
                        ur = mapPt;
                    }

                    ZoomToExtents(ll.X, ll.Y, ur.X, ur.Y);
                }
            }
        }

        private void OnMouseMapPositionChanged(double x, double y)
        {
            var handler = this.MouseMapPositionChanged;
            if (handler != null)
                handler(this, new MapPointEventArgs(x, y));
        }

        /// <summary>
        /// Raised when the map cursor position has changed
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when the map position as indicated by the current mouse pointer has changed")]
        public event EventHandler<MapPointEventArgs> MouseMapPositionChanged;

        #endregion

        private MapActiveTool _tool;

        /// <summary>
        /// Gets or sets the active tool
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapActiveTool ActiveTool
        {
            get
            {
                return _tool;
            }
            set
            {
                if (_tool.Equals(value))
                    return;

                _tool = value;
                switch (value)
                {
                    case MapActiveTool.Pan:
                        using (var ms = new MemoryStream(Properties.Resources.grab))
                        {
                            this.Cursor = new Cursor(ms);
                        }
                        break;
                    case MapActiveTool.ZoomIn:
                        using (var ms = new MemoryStream(Properties.Resources.zoomin))
                        {
                            this.Cursor = new Cursor(ms);
                        }
                        break;
                    case MapActiveTool.ZoomOut:
                        using (var ms = new MemoryStream(Properties.Resources.zoomout))
                        {
                            this.Cursor = new Cursor(ms);
                        }
                        break;
                    case MapActiveTool.None:
                    case MapActiveTool.Select:
                        {
                            this.Cursor = Cursors.Default;
                        }
                        break;
                }

                //Clear to prevent stray tooltips from being rendered
                if (value != MapActiveTool.Select &&
                    value != MapActiveTool.Pan)
                {
                    _activeTooltipText = null;
                }

                if (value != MapActiveTool.None)
                    this.DigitizingType = MapDigitizationType.None;

                OnPropertyChanged("ActiveTool");
            }
        }

        /// <summary>
        /// Screens to map units.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public PointF ScreenToMapUnits(double x, double y)
        {
            return ScreenToMapUnits(x, y, false);
        }

        private PointF ScreenToMapUnits(double x, double y, bool allowOutsideWindow)
        {
            if (!allowOutsideWindow)
            {
                if (x > this.Width - 1) x = this.Width - 1;
                else if (x < 0) x = 0;

                if (y > this.Height - 1) y = this.Height - 1;
                else if (y < 0) y = 0;
            }

            x = _extX1 + (_extX2 - _extX1) * (x / this.Width);
            y = _extY1 - (_extY1 - _extY2) * (y / this.Height);
            return new PointF((float)x, (float)y);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("Raised when a public property of this component has changed")]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a public property has changed
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            Action action = () =>
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(name));
            };
            if (this.InvokeRequired)
                this.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Gets whether this viewer has a loaded map
        /// </summary>
        public bool HasLoadedMap { get { return _map != null; } }

        private bool? _hasTiledLayers;

        internal bool HasTiledLayers
        {
            get
            {
                if (!_hasTiledLayers.HasValue)
                {
                    if (_map != null)
                    {
                        var groups = _map.Groups;
                        for (int i = 0; i < groups.Count; i++)
                        {
                            if (groups[i].Type == RuntimeMapGroup.kBaseMap)
                            {
                                _hasTiledLayers = true;
                                break;
                            }
                        }
                        if (!_hasTiledLayers.HasValue)
                            _hasTiledLayers = false;
                    }
                    else
                    {
                        _hasTiledLayers = false;
                    }
                }
                return _hasTiledLayers.Value;
            }
        }
    }
}
