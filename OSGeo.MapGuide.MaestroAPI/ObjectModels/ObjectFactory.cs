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
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Services;
using Res = OSGeo.MapGuide.MaestroAPI.Properties.Resources;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition_1_0_0;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Factory method signature for creating layer definitions
    /// </summary>
    public delegate ILayerDefinition LayerCreatorFunc(LayerType type);
    /// <summary>
    /// Factory method signature for creating load procedures
    /// </summary>
    public delegate ILoadProcedure LoadProcCreatorFunc();
    /// <summary>
    /// Factory method signature for creating web layouts
    /// </summary>
    public delegate IWebLayout WebLayoutCreatorFunc(string mapDefinitionId);

    /// <summary>
    /// Factory class to create MapGuide resource objects with either pre-defined or
    /// sensible default values. This is recommended over creating the objects directly
    /// as this ensures that there are no null child properties where the XML schema forbids
    /// it.
    /// 
    /// By default this class will only create v1.0.0 of any specified top-level resource. In order to be able to create
    /// newer versions, you need to register the appropriate methods that can create resources of this version:
    /// 
    ///  - <see cref="RegisterLayerFactoryMethod"/> for Layer Definitions
    ///  
    /// This registration needs to be done as part of your application's startup/initialization phase.
    /// 
    /// In the context of Maestro, this registration is automatically done as part of the addin's startup process
    /// </summary>
    public static class ObjectFactory
    {
        private static Dictionary<Version, LayerCreatorFunc> _layerFactories;
        private static Dictionary<LoadType, LoadProcCreatorFunc> _loadProcFactories;
        private static Dictionary<Version, WebLayoutCreatorFunc> _wlFactories;
        
        static ObjectFactory()
        {
            _layerFactories = new Dictionary<Version,LayerCreatorFunc>();
            _wlFactories = new Dictionary<Version, WebLayoutCreatorFunc>();
            _loadProcFactories = new Dictionary<LoadType, LoadProcCreatorFunc>();

            _layerFactories.Add(
                new Version(1, 0, 0),
                new LayerCreatorFunc(OSGeo.MapGuide.ObjectModels.LayerDefinition_1_0_0.LdfEntryPoint.CreateDefault));
            
            _loadProcFactories.Add(
                LoadType.Sdf,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcEntryPoint.CreateDefaultSdf));
            _loadProcFactories.Add(
                LoadType.Shp,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcEntryPoint.CreateDefaultShp));
            _loadProcFactories.Add(
                LoadType.Dwf,
                new LoadProcCreatorFunc(OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcEntryPoint.CreateDefaultDwf));

            _wlFactories.Add(
                new Version(1, 0, 0),
                new WebLayoutCreatorFunc(OSGeo.MapGuide.ObjectModels.WebLayout_1_0_0.WebLayoutEntryPoint.CreateDefault));
        }

        /// <summary>
        /// Registers the layer factory method.
        /// </summary>
        /// <param name="ver">The ver.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLayerFactoryMethod(Version ver, LayerCreatorFunc method)
        {
            if (_layerFactories.ContainsKey(ver))
                throw new ArgumentException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.FactoryMethodAlreadyRegistered + ver);

            _layerFactories[ver] = method;
        }

        /// <summary>
        /// Registers the load procedure factory method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public static void RegisterLoadProcedureFactoryMethod(LoadType type, LoadProcCreatorFunc method)
        {
            if (_loadProcFactories.ContainsKey(type))
                throw new ArgumentException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.LoadProcFactoryMethodAlreadyRegistered + type);

            _loadProcFactories[type] = method;
        }

        /// <summary>
        /// Registers the web layout factory method.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="method">The method.</param>
        public static void RegisterWebLayoutFactoryMethod(Version version, WebLayoutCreatorFunc method)
        {
            if (_wlFactories.ContainsKey(version))
                throw new ArgumentException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.FactoryMethodAlreadyRegistered + version);

            _wlFactories[version] = method;
        }

        /// <summary>
        /// Creates the web layout.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="version">The version.</param>
        /// <param name="mapDefinitionId">The map definition id.</param>
        /// <returns></returns>
        public static IWebLayout CreateWebLayout(IServerConnection owner, Version version, string mapDefinitionId)
        {
            Check.NotNull(owner, "owner");

            if (!_wlFactories.ContainsKey(version))
                throw new ArgumentException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.UnknownWebLayoutVersion + version.ToString());

            var wl = _wlFactories[version](mapDefinitionId);
            wl.CurrentConnection = owner;

            return wl;
        }

        /// <summary>
        /// Creates the default layer.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static ILayerDefinition CreateDefaultLayer(IServerConnection owner, LayerType type, Version version)
        {
            Check.NotNull(owner, "owner");

            if (!_layerFactories.ContainsKey(version))
                throw new ArgumentException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.UnknownLayerVersion + version.ToString());

            var layer = _layerFactories[version](type);
            layer.CurrentConnection = owner;
            
            return layer;
        }

        /// <summary>
        /// Creates the drawing source.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static IDrawingSource CreateDrawingSource(IServerConnection owner)
        {
            Check.NotNull(owner, "owner");

            return new OSGeo.MapGuide.ObjectModels.DrawingSource_1_0_0.DrawingSource() 
            { 
                CurrentConnection = owner,
                SourceName = string.Empty,
                CoordinateSpace = string.Empty,
                Sheet = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.DrawingSource_1_0_0.DrawingSourceSheet>()
            };
        }

        /// <summary>
        /// Creates the feature source.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static IFeatureSource CreateFeatureSource(IServerConnection owner, string provider)
        {
            Check.NotNull(owner, "owner");

            return new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.FeatureSourceType()
            {
                CurrentConnection = owner,
                Provider = provider,
                Parameter = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.NameValuePairType>()
            };
        }

        /// <summary>
        /// Creates the feature source.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="values">The connection properties.</param>
        /// <returns></returns>
        public static IFeatureSource CreateFeatureSource(IServerConnection owner, string provider, NameValueCollection values)
        {
            var fs = CreateFeatureSource(owner, provider);
            fs.ApplyConnectionProperties(values);

            return fs;
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(IServerConnection owner, string name)
        {
            Check.NotNull(owner, "owner");

            return new OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.MapDefinition() { 
                CurrentConnection = owner,
                CoordinateSystem = string.Empty,
                Extents = new OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.Box2DType() { MaxX = 0.0, MaxY = 0.0, MinX = 0.0, MinY = 0.0 },
                Name = name,
                BackgroundColor = Color.White,
                MapLayer = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.MapLayerType>(),
                MapLayerGroup = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0.MapLayerGroupType>(),
            };
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="coordinateSystemWkt">The coordinate system WKT.</param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(IServerConnection owner, string name, string coordinateSystemWkt)
        {
            var map = CreateMapDefinition(owner, name);
            map.CoordinateSystem = coordinateSystemWkt;

            return map;
        }

        /// <summary>
        /// Creates the map definition.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="coordinateSystemWkt">The coordinate system WKT.</param>
        /// <param name="env">The env.</param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(IServerConnection owner, string name, string coordinateSystemWkt, IEnvelope env)
        {
            var map = CreateMapDefinition(owner, name, coordinateSystemWkt);
            map.Extents = env;

            return map;
        }

        /// <summary>
        /// Creates the simple symbol.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static SimpleSymbolDefinition CreateSimpleSymbol(IServerConnection owner)
        {
            Check.NotNull(owner, "owner");

            return new SimpleSymbolDefinition() { CurrentConnection = owner };
        }

        /// <summary>
        /// Creates the compound symbol.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static CompoundSymbolDefinition CreateCompoundSymbol(IServerConnection owner)
        {
            Check.NotNull(owner, "owner");

            return new CompoundSymbolDefinition() { CurrentConnection = owner };
        }

        static readonly string[] parameterizedWidgets = 
        {
            KnownWidgetNames.CTRLClick,
            KnownWidgetNames.ZoomOnClick,
            KnownWidgetNames.ExtentHistory,
            KnownWidgetNames.Buffer,
            KnownWidgetNames.Measure,
            KnownWidgetNames.InvokeScript,
            KnownWidgetNames.InvokeURL,
            KnownWidgetNames.Search,
            KnownWidgetNames.CursorPosition,
            KnownWidgetNames.SelectionInfo,
            KnownWidgetNames.ViewSize
        };

        private static IUIWidget CreateVerticalWidget(IUIWidget widget)
        {
            var vert = widget.Clone();
            vert.Name = "vert" + widget.Name;
            vert.Label = string.Empty;
            return vert;
        }

        /// <summary>
        /// Creates a fusion flexible layout
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="templateName">The name of the template. See <see cref="FusionTemplateNames"/> for the common pre-defined names</param>
        /// <returns></returns>
        public static IApplicationDefinition CreateFlexibleLayout(IServerConnection owner, string templateName)
        {
            Check.NotNull(owner, "owner");

            /*
            Check.Precondition(Array.IndexOf(owner.Capabilities.SupportedServices, (int)ServiceType.Fusion) >= 0, "Required Fusion service not supported on this connection");

            var fusionSvc = (IFusionService)owner.GetService((int)ServiceType.Fusion);
            var templates = fusionSvc.GetApplicationTemplates();
            
            var appDef = DeserializeEmbeddedFlexLayout();
            //Find matching template.
            var tpl = templates.FindTemplate(templateName);
            if (tpl != null)
            {
                appDef.TemplateUrl = tpl.LocationUrl;
                appDef.Title = tpl.Name;
            }
            appDef.CurrentConnection = owner;
            return appDef;
            */ 
            
            Check.Precondition(Array.IndexOf(owner.Capabilities.SupportedServices, (int)ServiceType.Fusion) >= 0, "Required Fusion service not supported on this connection");
            
            IApplicationDefinition appDef = new ApplicationDefinitionType()
            {
                CurrentConnection = owner,
                MapSet = new System.ComponentModel.BindingList<MapGroupType>(),
                WidgetSet = new System.ComponentModel.BindingList<WidgetSetType>()
            };

            var fusionSvc = (IFusionService)owner.GetService((int)ServiceType.Fusion);
            var templates = fusionSvc.GetApplicationTemplates();
            var widgets = fusionSvc.GetApplicationWidgets();
            var containers = fusionSvc.GetApplicationContainers();

            //Find matching template. If it's a known template we should be able to
            //build it programatically, otherwise return a deserialized copy from our
            //embedded resource
            var tpl = templates.FindTemplate(templateName);
            if (tpl != null)
            {
                appDef.TemplateUrl = tpl.LocationUrl;
                appDef.Title = tpl.Name;
            }
            else
            {
                //NOTE: Depending on MapGuide Server version, this document may be 
                //invalid (eg. References to widgets not available in that version)
                return DeserializeEmbeddedFlexLayout(); 
            }

            //Toolbars, every template has them
            var toolbar = appDef.CreateContainer("Toolbar", containers.FindContainer("Toolbar"));
            var secToolbar = appDef.CreateContainer("ToolbarSecondary", containers.FindContainer("Toolbar"));
            var vertToolbar = appDef.CreateContainer("ToolbarVertical", containers.FindContainer("Toolbar"));

            //Context menus, every template has them
            var mapContextMenu = appDef.CreateContainer("MapContextMenu", containers.FindContainer("ContextMenu"));
            var taskPaneMenu = appDef.CreateContainer("TaskMenu", containers.FindContainer("ContextMenu"));

            //Menu
            var menu = appDef.CreateContainer("FileMenu", containers.FindContainer("Toolbar"));

            //Status bar
            var statusbar = appDef.CreateContainer("Statusbar", containers.FindContainer("Splitterbar"));

            string mapId = "MainMap";
            //Set default map group
            appDef.AddMapGroup(mapId, true, string.Empty);

            //Create default widget set
            var widgetSet = appDef.CreateWidgetSet(appDef.CreateMapWidget(mapId, mapContextMenu.Name));
            appDef.AddWidgetSet(widgetSet);

            //Add all known non-parameterized widgets to this widget set
            foreach (var wgt in widgets.WidgetInfo)
            {
                if (Array.IndexOf(parameterizedWidgets, wgt.Type) < 0)
                {
                    var widget = appDef.CreateWidget(wgt.Type, wgt);
                    widgetSet.AddWidget(widget);
                }
            }

            //Add some parameterized ones

            //Zoom In
            var zoomIn = (IUIWidget)appDef.CreateWidget("ZoomIn", widgets.FindWidget(KnownWidgetNames.ZoomOnClick));
            zoomIn.SetValue("Factor", "2");
            zoomIn.StatusText = zoomIn.Tooltip = Res.ADF_Widget_ZoomIn_Desc;
            zoomIn.Label = Res.ADF_Widget_ZoomIn_Label;
            zoomIn.ImageUrl = "images/icons.png";
            zoomIn.ImageClass = "zoom-in-fixed";
            var vZoomIn = CreateVerticalWidget(zoomIn);

            //Zoom Out
            var zoomOut = (IUIWidget)appDef.CreateWidget("ZoomOut", widgets.FindWidget(KnownWidgetNames.ZoomOnClick));
            zoomOut.SetValue("Factor", "0.5");
            zoomOut.StatusText = zoomOut.Tooltip = Res.ADF_Widget_ZoomOut_Desc;
            zoomOut.Label = Res.ADF_Widget_ZoomOut_Label;
            zoomOut.ImageUrl = "images/icons.png";
            zoomOut.ImageClass = "zoom-out-fixed";
            var vZoomOut = CreateVerticalWidget(zoomOut);

            //Previous View
            var prevView = (IUIWidget)appDef.CreateWidget("PreviousView", widgets.FindWidget(KnownWidgetNames.ExtentHistory));
            prevView.SetValue("Direction", "previous");
            prevView.StatusText = prevView.Tooltip = Res.ADF_Widget_PreviousView_Desc;
            prevView.Label = Res.ADF_Widget_PreviousView_Label;
            prevView.ImageUrl = "images/icons.png";
            prevView.ImageClass = "view-back";
            var vPrevView = CreateVerticalWidget(prevView);

            //Next View
            var nextView = (IUIWidget)appDef.CreateWidget("NextView", widgets.FindWidget(KnownWidgetNames.ExtentHistory));
            nextView.SetValue("Direction", "next");
            nextView.StatusText = nextView.Tooltip = Res.ADF_Widget_NextView_Desc;
            nextView.Label = Res.ADF_Widget_NextView_Label;
            nextView.ImageUrl = "images/icons.png";
            nextView.ImageClass = "view-forward";
            var vNextView = CreateVerticalWidget(nextView);

            //Buffer
            var buffer = (IUIWidget)appDef.CreateWidget("tbBuffer", widgets.FindWidget(KnownWidgetNames.BufferPanel));
            //buffer.SetValue("Target", "TaskPane");
            buffer.StatusText = buffer.Tooltip = Res.ADF_Widget_Buffer_Desc;
            buffer.Tooltip = Res.ADF_Widget_Buffer_Label;

            //Measure
            var measure = (IUIWidget)appDef.CreateWidget("Measure", widgets.FindWidget(KnownWidgetNames.Measure));
            var measureParams = new NameValueCollection();
            measureParams["Type"] = "both";
            measureParams["MeasureTooltipContainer"] = "MeasureResult";
            measureParams["MeasureTooltipType"] = "dynamic";
            measureParams["DistancePrecision"] = "0";
            measureParams["AreaPrecision"] = "0";
            measureParams["Units"] = "meters";
            measureParams["Target"] = "TaskPane";
            measure.SetAllValues(measureParams);
            measure.StatusText = buffer.Tooltip = Res.ADF_Widget_Measure_Desc;
            measure.Tooltip = Res.ADF_Widget_Measure_Label;

            //Show Overview
            var showOverview = (IUIWidget)appDef.CreateWidget("showOverview", widgets.FindWidget(KnownWidgetNames.InvokeScript));
            showOverview.SetValue("Script", "showOverviewMap()");

            //Show Task Pane
            var showTaskPane = (IUIWidget)appDef.CreateWidget("showTaskPane", widgets.FindWidget(KnownWidgetNames.InvokeScript));
            showTaskPane.SetValue("Script", "showTaskPane()");

            //Show Legend
            var showLegend = (IUIWidget)appDef.CreateWidget("showLegend", widgets.FindWidget(KnownWidgetNames.InvokeScript));
            showLegend.SetValue("Script", "showLegend()");

            //Show Selection Panel
            var showSelectionPanel = (IUIWidget)appDef.CreateWidget("showSelectionPanel", widgets.FindWidget(KnownWidgetNames.InvokeScript));
            showSelectionPanel.SetValue("Script", "showSelectionPanel()");

            //Coordinate Tracker
            var coordTracker = appDef.CreateWidget("statusCoordinates", widgets.FindWidget(KnownWidgetNames.CursorPosition));
            coordTracker.SetValue("Template", "X: {x} {units}, Y: {y} {units}");
            coordTracker.SetValue("Precision", "4");
            coordTracker.SetValue("EmptyText", "&amp;nbsp;");

            //Selection Info
            var selInfo = appDef.CreateWidget("statusSelection", widgets.FindWidget(KnownWidgetNames.SelectionInfo));
            selInfo.SetValue("EmptyText", "No selection");

            //View Size
            var viewSize = appDef.CreateWidget("statusViewSize", widgets.FindWidget(KnownWidgetNames.ViewSize));
            viewSize.SetValue("Template", "{w} x {h} ({units})");
            viewSize.SetValue("Precision", "2");

            widgetSet.AddWidget(zoomIn);
            widgetSet.AddWidget(zoomOut);
            widgetSet.AddWidget(prevView);
            widgetSet.AddWidget(nextView);
            widgetSet.AddWidget(buffer);
            widgetSet.AddWidget(measure);
            widgetSet.AddWidget(showOverview);
            widgetSet.AddWidget(showTaskPane);
            widgetSet.AddWidget(showLegend);
            widgetSet.AddWidget(showSelectionPanel);
            widgetSet.AddWidget(coordTracker);
            widgetSet.AddWidget(selInfo);
            widgetSet.AddWidget(viewSize);

            widgetSet.AddWidget(vZoomIn);
            widgetSet.AddWidget(vZoomOut);
            widgetSet.AddWidget(vPrevView);
            widgetSet.AddWidget(vNextView);

            //Now here's where things may diverge completely between templates
            //So let's try for something that is somewhat consistent

            //Init primary toolbar
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Print));

            //2.2 specific stuff
            if (owner.SiteVersion >= new Version(2, 2))
            {
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.QuickPlot));
            }

            toolbar.AddItem(appDef.CreateSeparator());
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.RefreshMap));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.SelectRadius));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.SelectPolygon));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ClearSelection));
            
            toolbar.AddItem(appDef.CreateWidgetReference(buffer.Name));
            toolbar.AddItem(appDef.CreateWidgetReference(measure.Name));
            
            //2.2 specific stuff
            if (owner.SiteVersion >= new Version(2, 2))
            {
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ViewOptions));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.About));
            toolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Help));
            
            //Init secondary toolbar
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Select));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Pan));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Zoom));
            secToolbar.AddItem(appDef.CreateWidgetReference(zoomIn.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(zoomOut.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.InitialMapView));
            secToolbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ZoomToSelection));
            secToolbar.AddItem(appDef.CreateWidgetReference(prevView.Name));
            secToolbar.AddItem(appDef.CreateWidgetReference(nextView.Name));

            //Init vertical toolbar
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Select, widgets.FindWidget(KnownWidgetNames.Select))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Pan, widgets.FindWidget(KnownWidgetNames.Pan))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.Zoom, widgets.FindWidget(KnownWidgetNames.Zoom))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.InitialMapView, widgets.FindWidget(KnownWidgetNames.InitialMapView))));
            widgetSet.AddWidget(CreateVerticalWidget((IUIWidget)appDef.CreateWidget(KnownWidgetNames.ZoomToSelection, widgets.FindWidget(KnownWidgetNames.ZoomToSelection))));

            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Select));
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Pan));
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.Zoom));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vZoomIn.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vZoomOut.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.InitialMapView));
            vertToolbar.AddItem(appDef.CreateWidgetReference("vert" + KnownWidgetNames.ZoomToSelection));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vPrevView.Name));
            vertToolbar.AddItem(appDef.CreateWidgetReference(vNextView.Name));

            //Main menu
            menu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.MapMenu));

            //2.2 specific stuff
            if (owner.SiteVersion >= new Version(2, 2))
            {
                menu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.BasemapSwitcher));
            }
            var viewMenu = appDef.CreateFlyout(Res.ADF_Flyout_View);
            viewMenu.AddItem(appDef.CreateWidgetReference(showOverview.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showTaskPane.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showLegend.Name));
            viewMenu.AddItem(appDef.CreateWidgetReference(showSelectionPanel.Name));
            menu.AddItem(viewMenu);

            //status bar
            statusbar.AddItem(appDef.CreateWidgetReference(coordTracker.Name));
            statusbar.AddItem(appDef.CreateWidgetReference(selInfo.Name));
            statusbar.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.EditableScale));
            statusbar.AddItem(appDef.CreateWidgetReference(viewSize.Name));

            //Map Context Menu
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.RefreshMap));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Pan));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Zoom));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(zoomIn.Name));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(zoomOut.Name));
            mapContextMenu.AddItem(appDef.CreateSeparator());
            var zoomMenu = appDef.CreateFlyout(Res.ADF_Flyout_Zoom);

            mapContextMenu.AddItem(zoomMenu);
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Select));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ClearSelection));
            var selectMoreMenu = appDef.CreateFlyout(Res.ADF_Flyout_SelectMore);

            mapContextMenu.AddItem(selectMoreMenu);
            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(buffer.Name));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(measure.Name));

            if (owner.SiteVersion >= new Version(2, 2))
            {
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            mapContextMenu.AddItem(appDef.CreateSeparator());
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.ViewOptions));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Help));
            mapContextMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.About));

            //Tasks Context Menu
            taskPaneMenu.AddItem(appDef.CreateWidgetReference(measure.Name));
            taskPaneMenu.AddItem(appDef.CreateWidgetReference(buffer.Name));

            if (owner.SiteVersion >= new Version(2, 2))
            {
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.FeatureInfo));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Query));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Theme));
                taskPaneMenu.AddItem(appDef.CreateWidgetReference(KnownWidgetNames.Redline));
            }

            //Now add them all to the main widget set
            widgetSet.AddContainer(toolbar);
            widgetSet.AddContainer(secToolbar);
            widgetSet.AddContainer(vertToolbar);
            widgetSet.AddContainer(menu);
            widgetSet.AddContainer(statusbar);
            widgetSet.AddContainer(mapContextMenu);
            widgetSet.AddContainer(taskPaneMenu);

            //Set positioning
            toolbar.Position = "top";
            secToolbar.Position = "top";
            menu.Position = "top";
            statusbar.Position = "bottom";
            mapContextMenu.Position = "top";
            taskPaneMenu.Position = "top";
            vertToolbar.Position = "left";

            return appDef;
        }

        internal static IApplicationDefinition DeserializeEmbeddedFlexLayout()
        {
            return (IApplicationDefinition)ResourceTypeRegistry.Deserialize(OSGeo.MapGuide.MaestroAPI.Properties.Resources.BaseTemplate_ApplicationDefinition);
        }

        /// <summary>
        /// Creates the preview flex layout.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static IApplicationDefinition CreatePreviewFlexLayout(IServerConnection owner)
        {
            Check.NotNull(owner, "owner");
            var appDef = new ApplicationDefinitionType()
            {
                CurrentConnection = owner,
                Title = "Preview",
                MapSet = new System.ComponentModel.BindingList<MapGroupType>(),
                WidgetSet = new System.ComponentModel.BindingList<WidgetSetType>()
            };
            appDef.TemplateUrl = "fusion/templates/mapguide/preview/index.html";
            return appDef;
        }

        /// <summary>
        /// Creates the print layout.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static IPrintLayout CreatePrintLayout(IServerConnection owner)
        {
            Check.NotNull(owner, "owner");

            return new OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayout()
            {
                CurrentConnection = owner,
                CustomLogos = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayoutLogo>(),
                CustomText = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayoutText>(),
                LayoutProperties = new OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayoutLayoutProperties(),
                PageProperties = new OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayoutPageProperties()
                {
                    BackgroundColor = new OSGeo.MapGuide.ObjectModels.PrintLayout_1_0_0.PrintLayoutPagePropertiesBackgroundColor()
                    {
                    }
                },
            };
        }

        /// <summary>
        /// Creates the load procedure.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        /// <param name="fileNames">The file names.</param>
        /// <returns></returns>
        public static ILoadProcedure CreateLoadProcedure(IServerConnection owner, LoadType type, IEnumerable<string> fileNames)
        {
            var proc = CreateLoadProcedure(owner, type);
            var fproc = proc.SubType as IBaseLoadProcedure;
            if (fproc != null)
            {
                if (fileNames != null)
                {
                    foreach (var f in fileNames)
                    {
                        fproc.SourceFile.Add(f);
                    }
                }
            }
            return proc;
        }

        /// <summary>
        /// Creates the load procedure.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILoadProcedure CreateLoadProcedure(IServerConnection owner, LoadType type)
        {
            if (type == LoadType.Dwg || type == LoadType.Raster)
                throw new NotSupportedException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.UnsupportedLoadProcedureType);

            if (_loadProcFactories.ContainsKey(type))
            {
                var proc = _loadProcFactories[type]();
                proc.CurrentConnection = owner;
                return proc;
            }

            throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Properties.Resources.CannotCreateLoadProcedureSubType + type);
        }

        /// <summary>
        /// Creates an empty resource document header metadata object
        /// </summary>
        /// <returns></returns>
        public static ResourceDocumentHeaderTypeMetadata CreateMetadata()
        {
            return new ResourceDocumentHeaderTypeMetadata()
            {
                Simple = new ResourceDocumentHeaderTypeMetadataSimple()
                {
                    Property = new System.ComponentModel.BindingList<ResourceDocumentHeaderTypeMetadataSimpleProperty>()
                }
            };
        }

        /// <summary>
        /// Creates an envelope (bounding box)
        /// </summary>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        /// <returns></returns>
        public static IEnvelope CreateEnvelope(double minx, double miny, double maxx, double maxy)
        {
            if (minx > maxx)
                throw new ArgumentException("minx > maxx", "minx");

            if (miny > maxy)
                throw new ArgumentException("miny > maxy", "miny");

            return new Envelope()
            {
                LowerLeftCoordinate = new EnvelopeLowerLeftCoordinate()
                {
                    X = minx,
                    Y = miny
                },
                UpperRightCoordinate = new EnvelopeUpperRightCoordinate()
                {
                    X = maxx,
                    Y = maxy
                }
            };
        }

        /// <summary>
        /// Creates an empty group security object
        /// </summary>
        /// <returns></returns>
        public static ResourceSecurityTypeGroups CreateSecurityGroup()
        {
            return new ResourceSecurityTypeGroups()
            {
                Group = new System.ComponentModel.BindingList<ResourceSecurityTypeGroupsGroup>()
            };
        }

        /// <summary>
        /// Creates an empty user security object
        /// </summary>
        /// <returns></returns>
        public static ResourceSecurityTypeUsers CreateSecurityUser()
        {
            return new ResourceSecurityTypeUsers()
            {
                User = new System.ComponentModel.BindingList<ResourceSecurityTypeUsersUser>()
            };
        }

        /// <summary>
        /// Creates the feature source extension.
        /// </summary>
        /// <returns></returns>
        public static IFeatureSourceExtension CreateFeatureSourceExtension()
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.FeatureSourceTypeExtension()
            {
                CalculatedProperty = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.CalculatedPropertyType>(),
                AttributeRelate = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.AttributeRelateType>()
            };
        }

        /// <summary>
        /// Creates the calculated property.
        /// </summary>
        /// <returns></returns>
        public static ICalculatedProperty CreateCalculatedProperty()
        {
            return new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.CalculatedPropertyType();
        }

        /// <summary>
        /// Creates the attribute relation.
        /// </summary>
        /// <returns></returns>
        public static IAttributeRelation CreateAttributeRelation()
        {
            IAttributeRelation rel = new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.AttributeRelateType() 
            { 
                RelateProperty = new System.ComponentModel.BindingList<OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.RelatePropertyType>(),
            };
            rel.RelateType = RelateTypeEnum.LeftOuter;
            rel.ForceOneToOne = false;
            return rel;
        }

        /// <summary>
        /// Creates a 2d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IPoint2D CreatePoint2D(double x, double y)
        {
            return new Point2DImpl() { X = x, Y = y };
        }

        /// <summary>
        /// Creates a 3d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static IPoint3D CreatePoint3D(double x, double y, double z)
        {
            return new Point3DImpl() { X = x, Y = y, Z = z };
        }
    }
}
