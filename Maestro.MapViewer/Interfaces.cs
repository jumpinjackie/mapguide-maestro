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
using System.Linq;
using System.Text;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Maestro.MapViewer
{
    /// <summary>
    /// A map viewer component
    /// </summary>
    public interface IMapViewer : INotifyPropertyChanged
    {
        /// <summary>
        /// Clears the current selection
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Starts the digitization process for a circle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizeCircle(CircleDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a line
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizeLine(LineDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a point
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizePoint(PointDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a polygon
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizePolygon(PolygonDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizeLineString(LineStringDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="segmentDigitized">The callback to be invoked when a new segment of the current line string is digitized</param>
        void DigitizeLineString(LineStringDigitizationCallback callback, LineDigitizationCallback segmentDigitized);

        /// <summary>
        /// Starts the digitization process for a rectangle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        void DigitizeRectangle(RectangleDigitizationCallback callback);

        /// <summary>
        /// Starts the digitization process for a circle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizeCircle(CircleDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a line
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizeLine(LineDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a point
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizePoint(PointDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a polygon
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizePolygon(PolygonDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizeLineString(LineStringDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a line string (polyline)
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="segmentDigitized">The callback to be invoked when a new segment of the current line string is digitized</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizeLineString(LineStringDigitizationCallback callback, LineDigitizationCallback segmentDigitized, string customPrompt);

        /// <summary>
        /// Starts the digitization process for a rectangle
        /// </summary>
        /// <param name="callback">The callback to be invoked when the digitization process completes</param>
        /// <param name="customPrompt">The custom prompt to use for the tracking tooltip</param>
        void DigitizeRectangle(RectangleDigitizationCallback callback, string customPrompt);

        /// <summary>
        /// Gets the current runtime map
        /// </summary>
        /// <returns></returns>
        RuntimeMap GetMap();

        /// <summary>
        /// Gets or sets the color used to render selected features
        /// </summary>
        Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets the active tool
        /// </summary>
        MapActiveTool ActiveTool { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed zoom scale for this viewer
        /// </summary>
        int MinScale { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed zoom scale for this viewer
        /// </summary>
        int MaxScale { get; set; }

        /// <summary>
        /// The amount of time (in ms) to wait to re-render after a mouse wheel scroll
        /// </summary>
        int MouseWheelDelayRenderInterval { get; set; }

        /// <summary>
        /// Gets or sets the factor by which to multiply the scale to zoom in
        /// </summary>
        double ZoomInFactor { get; set; }

        /// <summary>
        /// Gets or sets the factor by which to multiply the scale to zoom out
        /// </summary>
        double ZoomOutFactor { get; set; }

        /// <summary>
        /// Gets or sets whether feature tooltips are enabled. If set to true, tooltip queries are
        /// executed at the current mouse position if the active tool is Pan or Select
        /// </summary>
        bool FeatureTooltipsEnabled { get; set; }

        /// <summary>
        /// Gets whether the viewer has any active rendering operations
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets the type of object being currently digitized. If the digitization type is None, then
        /// the viewer is not currently digitizing
        /// </summary>
        MapDigitizationType DigitizingType { get; }

        /// <summary>
        /// Gets the currently rendered image
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image GetCurrentImage();

        /// <summary>
        /// Copies the image of the current map to the clipboard
        /// </summary>
        void CopyMap();

        /// <summary>
        /// Refreshes the current map view. Any changes to the runtime map state will be saved first before rendering begins
        /// </summary>
        void RefreshMap();

        /// <summary>
        /// Raised when the viewer has started refreshing the map. This is to allow
        /// any actions dependent on map state to update themselves asynchronously 
        /// without needing to wait for the updated map to be rendered.
        /// </summary>
        event EventHandler MapRefreshing;

        /// <summary>
        /// Raised when the map has been refreshed and the updated map image has been rendered
        /// </summary>
        event EventHandler MapRefreshed;

        /// <summary>
        /// Pans the view left by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        void PanLeft(bool refresh);

        /// <summary>
        /// Pans the view up by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        void PanUp(bool refresh);

        /// <summary>
        /// Pans the view right by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        void PanRight(bool refresh);

        /// <summary>
        /// Pans the view down by a pre-defined distance
        /// </summary>
        /// <param name="refresh"></param>
        void PanDown(bool refresh);

        /// <summary>
        /// Updates the rendered selection. Call this method if you have manipulated the selection
        /// set outside of the viewer
        /// </summary>
        void UpdateSelection();

        /// <summary>
        /// Updates the rendered selection. Call this method if you have manipulated the selection
        /// set outside of the viewer
        /// </summary>
        /// <param name="raise">Indicates if the <see cref="SelectionChanged"/> event should be raised as well</param>
        void UpdateSelection(bool raise);

        /// <summary>
        /// Selects features from all selectable layers that intersects the given geometry in WKT format
        /// </summary>
        /// <param name="wkt">The geometry wkt</param>
        /// <param name="maxFeatures">The maximum number of features to select. Specify -1 to select all features</param>
        void SelectByWkt(string wkt, int maxFeatures);

        /// <summary>
        /// Zooms to the initial map view
        /// </summary>
        void InitialMapView();

        /// <summary>
        /// Zooms to the specified map view
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        /// <param name="refresh"></param>
        void ZoomToView(double x, double y, double scale, bool refresh);

        /// <summary>
        /// Raised when the scale of the current runtime map has changed
        /// </summary>
        event EventHandler MapScaleChanged;

        /// <summary>
        /// Raised when the selection has changed. Note that programmatic selection modifications
        /// will not raise this event.
        /// </summary>
        event EventHandler SelectionChanged;

        /// <summary>
        /// Raised when the viewer has been initialized
        /// </summary>
        event EventHandler MapLoaded;

        /// <summary>
        /// Raised when the map cursor position has changed
        /// </summary>
        event EventHandler<MapPointEventArgs> MouseMapPositionChanged;

        /// <summary>
        /// Zooms to the view defined by the specified extent
        /// </summary>
        /// <param name="llx"></param>
        /// <param name="lly"></param>
        /// <param name="urx"></param>
        /// <param name="ury"></param>
        void ZoomToExtents(double llx, double lly, double urx, double ury);

        /// <summary>
        /// Gets or sets whether to show vertex coordinates when digitizing
        /// </summary>
        bool ShowVertexCoordinatesWhenDigitizing { get; set; }

        /// <summary>
        /// Gets or sets whether to convert tiled layers to non-tiled layers. This is a workaround
        /// setting for tiled maps to be displayed as viewer support for tiled layers is still not
        /// implemented
        /// </summary>
        bool ConvertTiledGroupsToNonTiled { get; set; }


        /// <summary>
        /// Gets whether to use the RenderMap API instead of RenderDynamicOverlay if the map has tiled
        /// layers. RenderMap includes tiled layers as part of the output image, but will not take advantage
        /// of any tile caching mechanisms. Setting this property to true nullifies any effect of the 
        /// <see cref="P:Maestro.MapViewer.IMapViewer.ConvertTiledGroupsToNonTiled"/> property
        /// </summary>
        bool UseRenderMapIfTiledLayersExist { get; set; }

        /// <summary>
        /// Gets whether to respect the list of finite display scales in a map being viewed if there are any defined.
        /// If true, all zooms will "snap" to the nearest finite display scale
        /// </summary>
        bool RespectFiniteDisplayScales { get; set; }

        /// <summary>
        /// Gets whether this viewer has a map loaded into it
        /// </summary>
        bool HasLoadedMap { get; }

        /// <summary>
        /// Gets or sets the amount of pixels to buffer out by when doing point-based selections with the Select tool
        /// </summary>
        int PointPixelBuffer { get; set; }

        /// <summary>
        /// Navigates to the previous view in the history stack
        /// </summary>
        void PreviousView();

        /// <summary>
        /// Navigates to the next view in the history stack
        /// </summary>
        void NextView();

        /// <summary>
        /// Gets the current index in the view history stack
        /// </summary>
        int ViewHistoryIndex { get; }

        /// <summary>
        /// Gets the view history stack. The first item being the earliest and the last item being the most recent.
        /// </summary>
        ReadOnlyCollection<MapViewHistoryEntry> ViewHistory { get; }

        /// <summary>
        /// Gets the current view extent in the map's coordinates
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        void GetViewExtent(out double minX, out double minY, out double maxX, out double maxY);
    }

    /// <summary>
    /// A toolbar that contains a default set of viewer commands
    /// </summary>
    public interface IDefaultToolbar
    {
        /// <summary>
        /// Gets or sets the viewer this toolbar is associated with
        /// </summary>
        IMapViewer Viewer { get; set; }
    }
}
