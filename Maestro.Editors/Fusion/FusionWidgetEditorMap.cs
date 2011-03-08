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
using System.Text;
using Maestro.Editors.Fusion.WidgetEditors;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.Fusion
{
    public static class FusionWidgetEditorMap
    {
        static Dictionary<string, Type> _edTypes;

        static FusionWidgetEditorMap()
        {
            _edTypes = new Dictionary<string, Type>();

            _edTypes[KnownWidgetNames.About] = typeof(AboutWidgetCtrl);
            _edTypes[KnownWidgetNames.ActivityIndicator] = typeof(ActivityIndicatorWidgetCtrl);
            _edTypes[KnownWidgetNames.Buffer] = typeof(BufferWidgetCtrl);
            _edTypes[KnownWidgetNames.BufferPanel] = typeof(BufferPanelWidgetCtrl);
            _edTypes[KnownWidgetNames.CenterSelection] = typeof(CenterSelectionWidgetCtrl);
            _edTypes[KnownWidgetNames.ClearSelection] = typeof(ClearSelectionCtrl);
            _edTypes[KnownWidgetNames.ColorPicker] = typeof(ColorPickerWidgetCtrl);
            //_edTypes[KnownWidgetNames.CTRLClick]
            _edTypes[KnownWidgetNames.CursorPosition] = typeof(CursorPositionWidgetCtrl);
            _edTypes[KnownWidgetNames.EditableScale] = typeof(EditableScaleWidgetCtrl);
            _edTypes[KnownWidgetNames.ExtentHistory] = typeof(ExtentHistoryWidgetCtrl);
            _edTypes[KnownWidgetNames.FeatureInfo] = typeof(FeatureInfoWidgetCtrl);
            _edTypes[KnownWidgetNames.Help] = typeof(HelpWidgetCtrl);
            _edTypes[KnownWidgetNames.InitialMapView] = typeof(InitialMapViewWidgetCtrl);
            _edTypes[KnownWidgetNames.InvokeScript] = typeof(InvokeScriptWidgetCtrl);
            //_edTypes[KnownWidgetNames.InvokeURL]
            //_edTypes[KnownWidgetNames.LayerManager]
            _edTypes[KnownWidgetNames.Legend] = typeof(LegendWidgetCtrl);
            _edTypes[KnownWidgetNames.LinkToView] = typeof(LinkToViewWidgetCtrl);
            _edTypes[KnownWidgetNames.MapMenu] = typeof(MapMenuWidgetCtrl);
            _edTypes[KnownWidgetNames.Maptip] = typeof(MapTipWidgetCtrl);
            _edTypes[KnownWidgetNames.Measure] = typeof(MeasureWidgetCtrl);
            _edTypes[KnownWidgetNames.Navigator] = typeof(NavigatorWidgetCtrl);
            //_edTypes[KnownWidgetNames.OverviewMap]
            _edTypes[KnownWidgetNames.Pan] = typeof(PanWidgetCtrl);
            //_edTypes[KnownWidgetNames.PanOnClick] 
            //_edTypes[KnownWidgetNames.PanQuery]
            //_edTypes[KnownWidgetNames.Print]
            //_edTypes[KnownWidgetNames.Query]
            //_edTypes[KnownWidgetNames.QuickPlot]
            //_edTypes[KnownWidgetNames.Redline]
            _edTypes[KnownWidgetNames.RefreshMap] = typeof(RefreshMapWidgetCtrl);
            //_edTypes[KnownWidgetNames.SaveMap]
            //_edTypes[KnownWidgetNames.Scalebar]
            //_edTypes[KnownWidgetNames.Search]
            //_edTypes[KnownWidgetNames.Select]
            //_edTypes[KnownWidgetNames.SelectionInfo]
            //_edTypes[KnownWidgetNames.SelectPolygon]
            //_edTypes[KnownWidgetNames.SelectRadius]
            //_edTypes[KnownWidgetNames.SelectRadiusValue]
            //_edTypes[KnownWidgetNames.SelectWithin]
            //_edTypes[KnownWidgetNames.TaskPane]
            //_edTypes[KnownWidgetNames.Theme]
            //_edTypes[KnownWidgetNames.ViewOptions]
            //_edTypes[KnownWidgetNames.ViewSize]
            //_edTypes[KnownWidgetNames.Zoom]
            //_edTypes[KnownWidgetNames.ZoomOnClick]
            //_edTypes[KnownWidgetNames.ZoomToSelection]
        }

        public static IWidgetEditor GetEditorForWidget(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            Check.NotNull(widget, "widget");
            Check.NotNull(context, "context");
            Check.NotNull(edsvc, "edsvc");

            IWidgetEditor ed = null;
            if (_edTypes.ContainsKey(widget.Name))
            {
                try
                {
                    ed = (IWidgetEditor)Activator.CreateInstance(_edTypes[widget.Name]);
                }
                catch (Exception ex)
                {
                    ed = null;
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                }
            }

            if (ed == null)
                ed = new GenericWidgetCtrl();

            ed.Setup(widget, context, edsvc);
            return ed;
        }
    }
}
