#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace OSGeo.MapGuide.ObjectModels.ApplicationDefinition
{
    /// <summary>
    /// Describes all available widgets
    /// </summary>
    public interface IApplicationDefinitionWidgetInfoSet
    {
        /// <summary>
        /// Gets the widgets
        /// </summary>
        IEnumerable<IWidgetInfo> WidgetInfo { get; }
    }

    /// <summary>
    /// Describes all available containers
    /// </summary>
    public interface IApplicationDefinitionContainerInfoSet
    {
        /// <summary>
        /// Gets the containers
        /// </summary>
        IEnumerable<IApplicationDefinitionContainerInfo> ContainerInfo { get; }
    }

    /// <summary>
    /// Describes all available templates
    /// </summary>
    public interface IApplicationDefinitionTemplateInfoSet
    {
        /// <summary>
        /// Gets the templates
        /// </summary>
        IEnumerable<IApplicationDefinitionTemplateInfo> TemplateInfo { get; }
    }

    /// <summary>
    /// Describes a fusion widget
    /// </summary>
    public interface IWidgetInfo
    {
        /// <summary>
        /// Gets the type of widget
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the localized type of the widget
        /// </summary>
        string LocalizedType { get; }

        /// <summary>
        /// Gets the description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the location
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Gets the label
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the tooltip
        /// </summary>
        string Tooltip { get; }

        /// <summary>
        /// Gets the text to display in the status bar
        /// </summary>
        string StatusText { get; }

        /// <summary>
        /// Gets the image url
        /// </summary>
        string ImageUrl { get; }

        /// <summary>
        /// Gets the image css class
        /// </summary>
        string ImageClass { get; }

        /// <summary>
        /// Indicates if this is a UI widget
        /// </summary>
        bool StandardUi { get; }

        /// <summary>
        /// Indicates which containers this widget is containable by
        /// </summary>
        string[] ContainableBy { get; }

        /// <summary>
        /// Gets the parameters for this widget
        /// </summary>
        IWidgetParameter[] Parameters { get; }
    }

    /// <summary>
    /// Describes a parameter of a fusion widget
    /// </summary>
    public interface IWidgetParameter
    {
        /// <summary>
        /// Gets the name of the parameter
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the parameter
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the type of the parameter
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the label for this parameter
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the minimum allowed value of this parameter
        /// </summary>
        string Min { get; }

        /// <summary>
        /// Gets the maximum allowed value of this parameter
        /// </summary>
        string Max { get; }

        /// <summary>
        /// Gets the list of allowed value of this parameter
        /// </summary>
        IAllowedValue[] AllowedValue { get; }

        /// <summary>
        /// Gets the default value of this parameter
        /// </summary>
        string DefaultValue { get; }

        /// <summary>
        /// Gets whether this parameter is mandatory
        /// </summary>
        bool IsMandatory { get; }
    }

    /// <summary>
    /// Describes an allowed widget parameter value
    /// </summary>
    public interface IAllowedValue
    {
        /// <summary>
        /// Gets the name of this value
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the label of this value
        /// </summary>
        string Label { get; }
    }

    /// <summary>
    /// Describes a fusion container
    /// </summary>
    public interface IApplicationDefinitionContainerInfo
    {
        /// <summary>
        /// Gets the type of container
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the localized type of container
        /// </summary>
        string LocalizedType { get; }

        /// <summary>
        /// Gets the description of this container
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the preview image url of this container
        /// </summary>
        string PreviewImageUrl { get; }
    }

    /// <summary>
    /// Describes a fusion template
    /// </summary>
    public interface IApplicationDefinitionTemplateInfo
    {
        /// <summary>
        /// Gets the name of this template
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the url location of this template
        /// </summary>
        string LocationUrl { get; }

        /// <summary>
        /// Gets the template description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the preview image url of this template
        /// </summary>
        string PreviewImageUrl { get; }

        /// <summary>
        /// Gets the panels applicable for this template
        /// </summary>
        IEnumerable<IApplicationDefinitionPanel> Panels { get; }
    }

    /// <summary>
    /// Describes a fusion template panel
    /// </summary>
    public interface IApplicationDefinitionPanel
    {
        /// <summary>
        /// Gets the name of this panel
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the label of this panel
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the description of this panel
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Factory method signature for creating fusion widgets
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public delegate IWidget WidgetFactoryMethod(IWidgetInfo info);

    /// <summary>
    /// Factory method signature for creating fusion widget containers
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public delegate IWidgetContainer ContainerFactoryMethod(IApplicationDefinitionContainerInfo container);

    /// <summary>
    /// Defines the stock fusion template names that come with a standard MapGuide installation
    /// </summary>
    public static class FusionTemplateNames
    {
        /// <summary>
        /// The preview template. Used for previewing other resources
        /// </summary>
        public const string Preview = "Preview"; //NOXLATE

        /// <summary>
        /// The Aqua template
        /// </summary>
        public const string Aqua = "Aqua"; //NOXLATE

        /// <summary>
        /// The Maroon template
        /// </summary>
        public const string Maroon = "Maroon"; //NOXLATE

        /// <summary>
        /// The Slate template
        /// </summary>
        public const string Slate = "Slate"; //NOXLATE

        /// <summary>
        /// The LimeGold template
        /// </summary>
        public const string LimeGold = "LimeGold"; //NOXLATE

        /// <summary>
        /// The TurquoiseYellow template
        /// </summary>
        public const string TurquoiseYellow = "TurquoiseYellow"; //NOXLATE
    }

    /// <summary>
    /// A set of known widgets that come with a fusion installation
    /// </summary>
    public static class KnownWidgetNames
    {
        /// <summary>
        /// About
        /// </summary>
        public const string About = nameof(About);

        /// <summary>
        /// ActivityIndicator
        /// </summary>
        public const string ActivityIndicator = nameof(ActivityIndicator);

        /// <summary>
        /// BasemapSwitcher
        /// </summary>
        public const string BasemapSwitcher = nameof(BasemapSwitcher);

        /// <summary>
        /// Buffer
        /// </summary>
        public const string Buffer = nameof(Buffer);

        /// <summary>
        /// BufferPanel
        /// </summary>
        public const string BufferPanel = nameof(BufferPanel);

        /// <summary>
        /// CenterSelection
        /// </summary>
        public const string CenterSelection = nameof(CenterSelection);

        /// <summary>
        /// ClearSelection
        /// </summary>
        public const string ClearSelection = nameof(ClearSelection);

        /// <summary>
        /// ColorPicker
        /// </summary>
        public const string ColorPicker = nameof(ColorPicker);

        /// <summary>
        /// CTRLClick
        /// </summary>
        public const string CTRLClick = nameof(CTRLClick);

        /// <summary>
        /// CursorPosition
        /// </summary>
        public const string CursorPosition = nameof(CursorPosition);

        /// <summary>
        /// EditableScale
        /// </summary>
        public const string EditableScale = nameof(EditableScale);

        /// <summary>
        /// ExtentHistory
        /// </summary>
        public const string ExtentHistory = nameof(ExtentHistory);

        /// <summary>
        /// FeatureInfo
        /// </summary>
        public const string FeatureInfo = nameof(FeatureInfo);

        /// <summary>
        /// Help
        /// </summary>
        public const string Help = nameof(Help);

        /// <summary>
        /// InitialMapView
        /// </summary>
        public const string InitialMapView = nameof(InitialMapView);

        /// <summary>
        /// InvokeScript
        /// </summary>
        public const string InvokeScript = nameof(InvokeScript);

        /// <summary>
        /// InvokeURL
        /// </summary>
        public const string InvokeURL = nameof(InvokeURL);

        /// <summary>
        /// LayerManager
        /// </summary>
        public const string LayerManager = nameof(LayerManager);

        /// <summary>
        /// Legend
        /// </summary>
        public const string Legend = nameof(Legend);

        /// <summary>
        /// LinkToView
        /// </summary>
        public const string LinkToView = nameof(LinkToView);

        /// <summary>
        /// MapMenu
        /// </summary>
        public const string MapMenu = nameof(MapMenu);

        /// <summary>
        /// Maptip
        /// </summary>
        public const string Maptip = nameof(Maptip);

        /// <summary>
        /// Measure
        /// </summary>
        public const string Measure = nameof(Measure);

        /// <summary>
        /// Navigator
        /// </summary>
        public const string Navigator = nameof(Navigator);

        /// <summary>
        /// OverviewMap
        /// </summary>
        public const string OverviewMap = nameof(OverviewMap);

        /// <summary>
        /// Pan
        /// </summary>
        public const string Pan = nameof(Pan);

        /// <summary>
        /// PanOnClick
        /// </summary>
        public const string PanOnClick = nameof(PanOnClick);

        /// <summary>
        /// PanQuery
        /// </summary>
        public const string PanQuery = nameof(PanQuery);

        /// <summary>
        /// Print
        /// </summary>
        public const string Print = nameof(Print);

        /// <summary>
        /// Query
        /// </summary>
        public const string Query = nameof(Query);

        /// <summary>
        /// QuickPlot
        /// </summary>
        public const string QuickPlot = nameof(QuickPlot);

        /// <summary>
        /// Redline
        /// </summary>
        public const string Redline = nameof(Redline);

        /// <summary>
        /// RefreshMap
        /// </summary>
        public const string RefreshMap = nameof(RefreshMap);

        /// <summary>
        /// SaveMap
        /// </summary>
        public const string SaveMap = nameof(SaveMap);

        /// <summary>
        /// Scalebar
        /// </summary>
        public const string Scalebar = nameof(Scalebar);

        /// <summary>
        /// ScalebarDual
        /// </summary>
        public const string ScalebarDual = nameof(ScalebarDual);

        /// <summary>
        /// Search
        /// </summary>
        public const string Search = nameof(Search);

        /// <summary>
        /// Select
        /// </summary>
        public const string Select = nameof(Select);

        /// <summary>
        /// SelectionInfo
        /// </summary>
        public const string SelectionInfo = nameof(SelectionInfo);

        /// <summary>
        /// SelectPolygon
        /// </summary>
        public const string SelectPolygon = nameof(SelectPolygon);

        /// <summary>
        /// SelectRadius
        /// </summary>
        public const string SelectRadius = nameof(SelectRadius);

        /// <summary>
        /// SelectRadiusValue
        /// </summary>
        public const string SelectRadiusValue = nameof(SelectRadiusValue);

        /// <summary>
        /// SelectWithin
        /// </summary>
        public const string SelectWithin = nameof(SelectWithin);

        /// <summary>
        /// TaskPane
        /// </summary>
        public const string TaskPane = nameof(TaskPane);

        /// <summary>
        /// Theme
        /// </summary>
        public const string Theme = nameof(Theme);

        /// <summary>
        /// ViewOptions
        /// </summary>
        public const string ViewOptions = nameof(ViewOptions);

        /// <summary>
        /// ViewSize
        /// </summary>
        public const string ViewSize = nameof(ViewSize);

        /// <summary>
        /// Zoom
        /// </summary>
        public const string Zoom = nameof(Zoom);

        /// <summary>
        /// ZoomOnClick
        /// </summary>
        public const string ZoomOnClick = nameof(ZoomOnClick);

        /// <summary>
        /// ZoomToSelection
        /// </summary>
        public const string ZoomToSelection = nameof(ZoomToSelection);
    }

    /// <summary>
    /// Represents a fusion flexible layout (aka. An Application Definition document)
    /// </summary>
    public interface IApplicationDefinition : IResource, IExtensibleElement
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the template url
        /// </summary>
        string TemplateUrl { get; set; }

        /// <summary>
        /// Gets the map set
        /// </summary>
        IMapSet MapSet { get; }

        /// <summary>
        /// Gets the widget sets
        /// </summary>
        IEnumerable<IWidgetSet> WidgetSets { get; }

        /// <summary>
        /// Creates a widget set
        /// </summary>
        /// <param name="mapWidget"></param>
        /// <returns></returns>
        IWidgetSet CreateWidgetSet(IMapWidget mapWidget);

        /// <summary>
        /// Adds the specified widget set
        /// </summary>
        /// <param name="set"></param>
        void AddWidgetSet(IWidgetSet set);

        /// <summary>
        /// Removes the specified widget set
        /// </summary>
        /// <param name="set"></param>
        void RemoveWidgetSet(IWidgetSet set);

        /// <summary>
        /// Creates a widget reference
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IWidgetItem CreateWidgetReference(string name);

        /// <summary>
        /// Creates a separator
        /// </summary>
        /// <returns></returns>
        ISeparator CreateSeparator();

        /// <summary>
        /// Create a flyout menu
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        IFlyoutItem CreateFlyout(string label);

        /// <summary>
        /// Adds the new map group to the current map set with a default MapGuide child map
        /// </summary>
        /// <param name="id"></param>
        /// <param name="singleTile"></param>
        /// <param name="mapDefinitionId"></param>
        /// <returns></returns>
        IMapGroup AddMapGroup(string id, bool singleTile, string mapDefinitionId);

        /// <summary>
        /// Adds a new map group to the specified map set with a default MapGuide child map
        /// </summary>
        /// <param name="id"></param>
        /// <param name="singleTile"></param>
        /// <param name="mapDefinitionId"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        IMapGroup AddMapGroup(string id, bool singleTile, string mapDefinitionId, double centerX, double centerY, double scale);

        /// <summary>
        /// Create a widget from the specified widget information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="widgetInfo"></param>
        /// <returns></returns>
        IWidget CreateWidget(string name, IWidgetInfo widgetInfo);

        /// <summary>
        /// Creates a widget reference UI container
        /// </summary>
        /// <param name="name"></param>
        /// <param name="containerInfo"></param>
        /// <returns></returns>
        IUIItemContainer CreateContainer(string name, IApplicationDefinitionContainerInfo containerInfo);

        /// <summary>
        /// Creates a map widget
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="contextMenuId"></param>
        /// <returns></returns>
        IMapWidget CreateMapWidget(string mapId, string contextMenuId);
    }

    /// <summary>
    /// Represents an element that is extensible with arbitrary XML content
    /// </summary>
    public interface IExtensibleElement
    {
        /// <summary>
        /// Gets the extension
        /// </summary>
        IExtension Extension { get; }
    }

    /// <summary>
    /// Represents a region of arbitrary XML content
    /// </summary>
    public interface IExtension
    {
        /// <summary>
        /// Gets or sets the XML content. It is best to use the extension methods to get and set the appropriate values
        /// rather than accessing this array directly
        /// </summary>
        XmlElement[] Content { get; set; }
    }

    /// <summary>
    /// A reusable <see cref="XmlDocument"/> providing <see cref="XmlElement"/> creation functionality
    /// for those that require it
    /// </summary>
    internal static class AppDefDocument
    {
        private static XmlDocument _doc;

        internal static XmlDocument Instance
        {
            get
            {
                if (_doc == null)
                    _doc = new XmlDocument();

                return _doc;
            }
        }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the name of the fusion template
        /// </summary>
        /// <param name="appDef"></param>
        /// <returns></returns>
        public static string GetTemplateName(this IApplicationDefinition appDef)
        {
            string[] tokens = appDef.TemplateUrl.Split('/');
            // fusion/templates/mapguide/{templateName}/index.html
            return tokens[tokens.Length - 2];
        }

        /// <summary>
        /// Gets whether a widget with the specified name already exists
        /// </summary>
        /// <param name="appDef"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool WidgetNameExists(this IApplicationDefinition appDef, string name)
        {
            Check.ArgumentNotNull(appDef, nameof(appDef));
            Check.ArgumentNotEmpty(name, nameof(name));

            return appDef.FindWidget(name) != null;
        }

        /// <summary>
        /// Gets the widget of the specified name
        /// </summary>
        /// <param name="appDef"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IWidget FindWidget(this IApplicationDefinition appDef, string name)
        {
            Check.ArgumentNotNull(appDef, nameof(appDef));
            Check.ArgumentNotEmpty(name, nameof(name));

            foreach (var set in appDef.WidgetSets)
            {
                foreach (var wgt in set.Widgets)
                {
                    if (wgt.Name == name)
                        return wgt;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the specified widget from this
        /// </summary>
        /// <param name="appDef">The app definition.</param>
        /// <param name="widgetName">Name of the widget to remove.</param>
        /// <param name="deleteReferences">if set to <c>true</c> [deletes any references to the widget to be deleted].</param>
        public static void RemoveWidget(this IApplicationDefinition appDef, string widgetName, bool deleteReferences)
        {
            Check.ArgumentNotNull(appDef, nameof(appDef));
            Check.ArgumentNotEmpty(widgetName, nameof(widgetName));

            IWidgetSet matchingSet = null;
            IWidget matchingWidget = null;
            foreach (var set in appDef.WidgetSets)
            {
                if (matchingSet == null)
                {
                    foreach (var wgt in set.Widgets)
                    {
                        if (wgt.Name == widgetName)
                        {
                            matchingSet = set;
                            matchingWidget = wgt;
                            break;
                        }
                    }
                }
            }

            int removed = 0;

            if (matchingSet != null && matchingWidget != null)
            {
                matchingSet.RemoveWidget(matchingWidget);

                if (deleteReferences)
                {
                    foreach (var set in appDef.WidgetSets)
                    {
                        foreach (var cnt in set.Containers)
                        {
                            var uicnt = cnt as IUIItemContainer;
                            if (uicnt != null)
                            {
                                List<IWidgetItem> removeMe = new List<IWidgetItem>();
                                foreach (var uiitem in uicnt.Items)
                                {
                                    IWidgetItem witem = uiitem as IWidgetItem;
                                    if (witem != null && witem.Widget == widgetName)
                                    {
                                        removeMe.Add(witem);
                                        System.Diagnostics.Trace.TraceInformation("Found widget reference in container: " + uicnt.Name); //NOXLATE
                                    }
                                }

                                if (removeMe.Count > 0)
                                {
                                    foreach (var rm in removeMe)
                                    {
                                        uicnt.RemoveItem(rm);
                                        removed++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (deleteReferences)
                System.Diagnostics.Trace.TraceInformation(removed + " widget references removed");   //NOXLATE
        }

        /// <summary>
        /// Gets a specific container info by type
        /// </summary>
        /// <param name="set"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IApplicationDefinitionContainerInfo FindContainer(this IApplicationDefinitionContainerInfoSet set, string name)
        {
            Check.ArgumentNotNull(set, nameof(set));
            Check.ArgumentNotEmpty(name, nameof(name));

            foreach (var cnt in set.ContainerInfo)
            {
                if (name.Equals(cnt.Type))
                    return cnt;
            }
            return null;
        }

        /// <summary>
        /// Gets a specific Widget Info by name
        /// </summary>
        /// <param name="set"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IWidgetInfo FindWidget(this IApplicationDefinitionWidgetInfoSet set, string name)
        {
            Check.ArgumentNotNull(set, nameof(set));
            Check.ArgumentNotEmpty(name, nameof(name));

            foreach (var wgt in set.WidgetInfo)
            {
                if (name.Equals(wgt.Type))
                    return wgt;
            }
            return null;
        }

        /// <summary>
        /// Gets information for the named template
        /// </summary>
        /// <param name="set"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IApplicationDefinitionTemplateInfo FindTemplate(this IApplicationDefinitionTemplateInfoSet set, string name)
        {
            Check.ArgumentNotNull(set, nameof(set));
            Check.ArgumentNotEmpty(name, nameof(name));

            foreach (var tpl in set.TemplateInfo)
            {
                if (name.Equals(tpl.Name))
                    return tpl;
            }
            return null;
        }

        /// <summary>
        /// Gets the specified map group by its id
        /// </summary>
        /// <param name="set"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IMapGroup GetGroupById(this IMapSet set, string id)
        {
            Check.ArgumentNotNull(set, nameof(set));
            foreach (var group in set.MapGroups)
            {
                if (group.id.Equals(id))
                    return group;
            }
            return null;
        }

        /// <summary>
        /// Gets the names of all properties of this extensible element
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string[] GetNames(this IExtensibleElement ext)
        {
            Check.ArgumentNotNull(ext, nameof(ext));

            List<string> names = new List<string>();

            foreach (var el in ext.Extension.Content)
            {
                names.Add(el.Name);
            }

            return names.ToArray();
        }

        /// <summary>
        /// Gets all the properties in this extensible element
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static NameValueCollection GetAllValues(this IExtensibleElement ext)
        {
            Check.ArgumentNotNull(ext, nameof(ext));
            NameValueCollection values = new NameValueCollection();
            foreach (var el in ext.Extension.Content)
            {
                values.Add(el.Name, el.InnerText);
            }
            return values;
        }

        /// <summary>
        /// Replace the values of all properties in this extensible element with the values provided
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="values"></param>
        public static void SetAllValues(this IExtensibleElement ext, NameValueCollection values)
        {
            Check.ArgumentNotNull(ext, nameof(ext));
            Check.ArgumentNotNull(values, nameof(values));

            var elements = new List<XmlElement>();
            foreach (string name in values.Keys)
            {
                var value = values[name];
                var rid = AppDefDocument.Instance.CreateElement(name);
                rid.InnerText = value;
                elements.Add(rid);
            }
            ext.Extension.Content = elements.ToArray();
        }

        /// <summary>
        /// Sets the value of a property in this extensible element.
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetValue(this IExtensibleElement ext, string name, string value)
        {
            Check.ArgumentNotNull(ext, nameof(ext));
            Check.ArgumentNotEmpty(name, nameof(name));

            if (ext.Extension.Content != null)
            {
                var el = ext.Extension.Content.FindElementByName(name);
                if (el != null)
                {
                    el.InnerText = value;
                }
                else
                {
                    var values = new List<XmlElement>(ext.Extension.Content);
                    var rid = AppDefDocument.Instance.CreateElement(name);
                    rid.InnerText = value;
                    values.Add(rid);
                    ext.Extension.Content = values.ToArray();
                }
            }
            else
            {
                var rid = AppDefDocument.Instance.CreateElement(name);
                rid.InnerText = value;
                ext.Extension.Content = new XmlElement[] { rid };
            }

            Trace.TraceInformation("Extensible element property {0} set to: {1}", name, value); //NOXLATE
        }

        public static XmlElement CreateExtensionElement(this IExtensibleElement ext, string name) => AppDefDocument.Instance.CreateElement(name);

        /// <summary>
        /// Gets the value of a property in this extensible element. If none exists, an empty string is returned
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(this IExtensibleElement ext, string name)
        {
            Check.ArgumentNotNull(ext, nameof(ext));
            Check.ArgumentNotEmpty(name, nameof(name));

            if (ext.Extension.Content != null)
            {
                var el = ext.Extension.Content.FindElementByName(name);
                if (el != null)
                {
                    return el.InnerText;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="System.Xml.XmlElement"/> whose name matches the specified name
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlElement FindElementByName(this XmlElement[] elements, string name)
        {
            Check.ArgumentNotNull(elements, nameof(elements));
            foreach (var el in elements)
            {
                if (el.Name == name)
                    return el;
            }
            return null;
        }

        /// <summary>
        /// Set the map definition id
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string GetMapDefinition(this IMap map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            return map.GetValue("ResourceId"); //NOXLATE
        }

        /// <summary>
        /// Gets the map definition id
        /// </summary>
        /// <param name="map"></param>
        /// <param name="mapDefId"></param>
        public static void SetMapDefinition(this IMap map, string mapDefId)
        {
            Check.ArgumentNotNull(map, nameof(map));
            map.SetValue("ResourceId", mapDefId); //NOXLATE
        }

        /// <summary>
        /// Gets the first widget set of this application definition.
        /// </summary>
        /// <param name="appDef"></param>
        /// <returns></returns>
        public static IWidgetSet GetFirstWidgetSet(this IApplicationDefinition appDef)
        {
            Check.ArgumentNotNull(appDef, nameof(appDef));
            IWidgetSet set = null;
            foreach (var wgt in appDef.WidgetSets)
            {
                if (set == null)
                {
                    set = wgt;
                    break;
                }
            }
            return set;
        }
    }

    /*
public abstract class WidgetValue
{
    protected WidgetValue(string name, bool required)
    {
        this.Name = name;
        this.Nullable = !required;
    }

    public bool Nullable { get; protected set; }

    public string Name { get; set; }

    public abstract object Value { get; set; }

    protected virtual string ValueToString()
    {
        if (this.Value == null)
            return string.Empty;
        else
            return this.Value.ToString();
    }

    public virtual string ToXml()
    {
        if (this.Nullable && this.Value == null)
            return string.Empty;

        return "<" + this.Name + ">" + ValueToString() + "</" + this.Name + ">"; //NOXLATE
    }
}

public class RangedWidgetValue : WidgetValue
{
    public RangedWidgetValue(string name, bool required, IComparable minValue, IComparable maxValue) : base(name, required)
    {
        Check.ArgumentNotNull(minValue, "minValue"); //NOXLATE
        Check.ArgumentNotNull(maxValue, "maxValue"); //NOXLATE

        if (minValue.CompareTo(maxValue) <= 0)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
        else
        {
            this.MinValue = maxValue;
            this.MaxValue = MinValue;
        }
    }

    public IComparable MinValue { get; private set; }

    public IComparable MaxValue { get; private set; }

    private object _value;

    public override object Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value == null)
            {
                if (!this.Nullable)
                    throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Properties.ErrorNullNotAccepted);

                _value = value;
            }
            else
            {
                var cmp = value as IComparable;
                if (cmp == null)
                    throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Properties.ErrroValueNotComparable);

                if (cmp.CompareTo(this.MinValue) >= 0 && cmp.CompareTo(this.MaxValue) <= 0)
                    _value = value;
                else
                    throw new InvalidOperationException(string.Format(OSGeo.MapGuide.MaestroAPI.Properties.ErrorValueOutOfRange, this.MinValue, this.MaxValue));
            }
        }
    }
}

public class RestrictedWidgetValue : WidgetValue
{
    public RestrictedWidgetValue(string name, bool required) : base(name, required) { }

    private object _value;

    public override object Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value == null)
            {
                if (!this.Nullable)
                    throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Properties.ErrorNullNotAccepted);

                _value = value;
            }
            else
            {
                _value = value;
            }
        }
    }
}

public class ArbitraryWidgetValue : WidgetValue
{
    public ArbitraryWidgetValue(string name, bool required) : base(name, required) { }

    private object _value;

    public override object Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value == null)
            {
                if (!this.Nullable)
                    throw new InvalidOperationException(OSGeo.MapGuide.MaestroAPI.Properties.ErrorNullNotAccepted);

                _value = value;
            }
            else
            {
                _value = value;
            }
        }
    }
}
    */

    /// <summary>
    /// Describes the map set of the Flexible Layout
    /// </summary>
    public interface IMapSet
    {
        /// <summary>
        /// The map groups
        /// </summary>
        IEnumerable<IMapGroup> MapGroups { get; }

        /// <summary>
        /// The number of map groups
        /// </summary>
        int MapGroupCount { get; }

        /// <summary>
        /// Gets the map group at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IMapGroup GetGroupAt(int index);

        /// <summary>
        /// Adds a map group
        /// </summary>
        /// <param name="group"></param>
        void AddGroup(IMapGroup group);

        /// <summary>
        /// Removes the given map group
        /// </summary>
        /// <param name="group"></param>
        void RemoveGroup(IMapGroup group);
    }

    /// <summary>
    /// Describes a Map Group in a Flexible Layout. A MapGroup consists of a
    /// MapGuide Map Definition and zero or more external base layers
    /// </summary>
    public interface IMapGroup : IExtensibleElement, INotifyPropertyChanged
    {
        /// <summary>
        /// The map group id
        /// </summary>
        string id { get; set; }

        /// <summary>
        /// The initial map view
        /// </summary>
        IMapView InitialView { get; set; }

        /// <summary>
        /// The list of map entries
        /// </summary>
        IEnumerable<IMap> Map { get; }

        /// <summary>
        /// Adds a map entr
        /// </summary>
        /// <param name="map"></param>
        void AddMap(IMap map);

        /// <summary>
        /// Removes the given map entr
        /// </summary>
        /// <param name="map"></param>
        void RemoveMap(IMap map);

        /// <summary>
        /// Gets the count of map entries
        /// </summary>
        int MapCount { get; }

        /// <summary>
        /// Gets the map entry at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IMap GetMapAt(int index);

        /// <summary>
        /// Creates an initial map view
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        IMapView CreateInitialView(double x, double y, double scale);

        /// <summary>
        /// Creates a MapGuide map entry
        /// </summary>
        /// <param name="mapDefinition"></param>
        /// <returns></returns>
        IMap CreateMapGuideEntry(string mapDefinition);

        /// <summary>
        /// Creates a external base layer map entry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="singleTile"></param>
        /// <param name="name"></param>
        /// <param name="olType"></param>
        /// <returns></returns>
        IMap CreateCmsMapEntry(string type, bool singleTile, string name, string olType);

        /// <summary>
        /// Creates a generic map entry
        /// </summary>
        /// <returns></returns>
        IMap CreateGenericEntry();

        /// <summary>
        /// Moves the given map entry up
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        bool MoveUp(IMap map);

        /// <summary>
        /// Moves the given map entry down
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        bool MoveDown(IMap map);
    }

    /// <summary>
    /// Describes a map entry in a map group
    /// </summary>
    public interface IMap : INotifyPropertyChanged, IExtensibleElement
    {
        /// <summary>
        /// The type of map
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Indicates if it is single tile
        /// </summary>
        bool SingleTile { get; set; }

        /// <summary>
        /// MapGuide overlay options
        /// </summary>
        IMapGuideOverlayOptions OverlayOptions { get; set; }

        /// <summary>
        /// Creates MapGuide overlay options
        /// </summary>
        /// <param name="isBaseLayer"></param>
        /// <param name="useOverlay"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        IMapGuideOverlayOptions CreateOverlayOptions(bool isBaseLayer, bool useOverlay, string projection);

        /// <summary>
        /// External base layer options
        /// </summary>
        ICmsMapOptions CmsMapOptions { get; set; }
        
        /// <summary>
        /// Creates external base layer options
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ICmsMapOptions CreateOptions(string name, string type);

        /// <summary>
        /// Outputs to XML
        /// </summary>
        /// <returns></returns>
        string AsXml();

        /// <summary>
        /// Initialize from XML
        /// </summary>
        /// <param name="xml"></param>
        void FromXml(string xml);
    }

    /// <summary>
    /// Describes MapGuide overlay options
    /// </summary>
    public interface IMapGuideOverlayOptions
    {
        /// <summary>
        /// Indicates if this is a base layer
        /// </summary>
        bool IsBaseLayer { get; set; }

        /// <summary>
        /// Indicates if selection overlay should be used
        /// </summary>
        bool UseOverlay { get; set; }

        /// <summary>
        /// Indicates the projection
        /// </summary>
        string Projection { get; set; }
    }

    /// <summary>
    /// Describes external base layer options
    /// </summary>
    public interface ICmsMapOptions
    {
        /// <summary>
        /// The name of the external base layer
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type of the external base layer
        /// </summary>
        string Type { get; set; }
    }

    /// <summary>
    /// Represents a flexible layout's widget set. This is analogous to a Command Set in a Web Layout
    /// </summary>
    public interface IWidgetSet
    {
        /// <summary>
        /// The list of widget containers
        /// </summary>
        IEnumerable<IWidgetContainer> Containers { get; }

        /// <summary>
        /// The number of widget containers
        /// </summary>
        int ContainerCount { get; }

        /// <summary>
        /// Adds a widget container
        /// </summary>
        /// <param name="container"></param>
        void AddContainer(IWidgetContainer container);

        /// <summary>
        /// Removes the given widget container
        /// </summary>
        /// <param name="container"></param>
        void RemoveContainer(IWidgetContainer container);

        /// <summary>
        /// Gets the designated map widget
        /// </summary>
        IMapWidget MapWidget { get; }

        /// <summary>
        /// Gets the total number of widgets in this widget set
        /// </summary>
        int WidgetCount { get; }

        /// <summary>
        /// Gets the list of widgets
        /// </summary>
        IEnumerable<IWidget> Widgets { get; }

        /// <summary>
        /// Adds a widget
        /// </summary>
        /// <param name="widget"></param>
        void AddWidget(IWidget widget);
        
        /// <summary>
        /// Removes the specified widget
        /// </summary>
        /// <param name="widget"></param>
        void RemoveWidget(IWidget widget);
    }

    /// <summary>
    /// Represents a container component in a flexible layout
    /// </summary>
    public interface IWidgetContainer : INotifyPropertyChanged, IExtensibleElement
    {
        /// <summary>
        /// The name of the widget container
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type of the widget container
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The position of the widget container
        /// </summary>
        string Position { get; set; }
    }

    /// <summary>
    /// Represents a UI item container component
    /// </summary>
    public interface IUIItemContainer : IWidgetContainer, IMenu
    {
    }

    /// <summary>
    /// A interface for widgets and components with menu-like characteristics
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        int ItemCount { get; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        IEnumerable<IUIItem> Items { get; }

        /// <summary>
        /// Moves the specified item up.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool MoveUp(IUIItem item);

        /// <summary>
        /// Moves the specified item down.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool MoveDown(IUIItem item);

        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        int GetIndex(IUIItem item);

        /// <summary>
        /// Inserts the specified item at the specified index.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        void Insert(IUIItem item, int index);

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        void AddItem(IUIItem item);

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        void RemoveItem(IUIItem item);
    }

    /// <summary>
    /// Defines the type of UI items
    /// </summary>
    [System.SerializableAttribute]
    public enum UiItemFunctionType
    {
        /// <remarks/>
        Separator,

        /// <remarks/>
        Widget,

        /// <remarks/>
        Flyout,
    }

    /// <summary>
    /// Represents a UI toolbar/menu item
    /// </summary>
    public interface IUIItem : INotifyPropertyChanged
    {
        /// <summary>
        /// The parent menu
        /// </summary>
        IMenu Parent { get; }

        /// <summary>
        /// The item function
        /// </summary>
        UiItemFunctionType Function { get; }
    }

    /// <summary>
    /// Represents a toolbar/menu flyout item
    /// </summary>
    public interface IFlyoutItem : IUIItem, IMenu, INotifyPropertyChanged
    {
        /// <summary>
        /// The label
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// The tooltip
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// The image url
        /// </summary>
        string ImageUrl { get; set; }

        /// <summary>
        /// The image CSS class
        /// </summary>
        string ImageClass { get; set; }
    }

    /// <summary>
    /// Represents a menu/toolbar separator
    /// </summary>
    public interface ISeparator : IUIItem
    {
    }

    /// <summary>
    /// Represents a widget reference. This is analogous to a command item in a Web Layouts
    /// </summary>
    public interface IWidgetItem : IUIItem
    {
        /// <summary>
        /// The widget name
        /// </summary>
        string Widget { get; set; }
    }

    /// <summary>
    /// Represents a fusion application widget
    /// </summary>
    public interface IWidget : INotifyPropertyChanged, IExtensibleElement
    {
        /// <summary>
        /// Gets or sets the name of the widget
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// Gets the raw XML content of this widget
        /// </summary>
        /// <returns></returns>
        string ToXml();
    }

    /// <summary>
    /// Represents a map widget
    /// </summary>
    public interface IMapWidget : IWidget
    {
        /// <summary>
        /// The map group id
        /// </summary>
        string MapId { get; set; }
    }

    /// <summary>
    /// Represents a widget definition that is accessible from a menu or toolbar
    /// </summary>
    public interface IUIWidget : IWidget
    {
        /// <summary>
        /// The image icon URL
        /// </summary>
        string ImageUrl { get; set; }

        /// <summary>
        /// The image icon CSS class
        /// </summary>
        string ImageClass { get; set; }

        /// <summary>
        /// The label
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// The tooltip
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// The status bar text
        /// </summary>
        string StatusText { get; set; }

        /// <summary>
        /// Indicates if it is disabled
        /// </summary>
        string Disabled { get; set; }

        /// <summary>
        /// Clones this instance
        /// </summary>
        /// <returns></returns>
        IUIWidget Clone();
    }

    /// <summary>
    /// The initial view of the map
    /// </summary>
    public interface IMapView : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the center X.
        /// </summary>
        /// <value>The center X.</value>
        double CenterX { get; set; }

        /// <summary>
        /// Gets or sets the center Y.
        /// </summary>
        /// <value>The center Y.</value>
        double CenterY { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        double Scale { get; set; }
    }
}