using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.ObjectModels.WebLayout
{
    /// <summary>
    /// Defines a region of the web layout where UI items can reside
    /// </summary>
    public enum WebLayoutRegion
    {
        /// <summary>
        /// The context menu
        /// </summary>
        ContextMenu,
        /// <summary>
        /// The toolbar
        /// </summary>
        Toolbar,
        /// <summary>
        /// The task bar
        /// </summary>
        TaskBar
    }

    /// <summary>
    /// Encompasses all built-in commands usable by the web layout (AJAX and DWF)
    /// </summary>
    public enum BuiltInCommandType : int
    {
        /// <summary>
        /// 
        /// </summary>
        Pan = BasicCommandActionType.Pan,
        /// <summary>
        /// 
        /// </summary>
        PanUp = BasicCommandActionType.PanUp,
        /// <summary>
        /// 
        /// </summary>
        PanDown = BasicCommandActionType.PanDown,
        /// <summary>
        /// 
        /// </summary>
        PanRight = BasicCommandActionType.PanRight,
        /// <summary>
        /// 
        /// </summary>
        PanLeft = BasicCommandActionType.PanLeft,
        /// <summary>
        /// 
        /// </summary>
        Zoom = BasicCommandActionType.Zoom,
        /// <summary>
        /// 
        /// </summary>
        ZoomIn = BasicCommandActionType.ZoomIn,
        /// <summary>
        /// 
        /// </summary>
        ZoomOut = BasicCommandActionType.ZoomOut,
        /// <summary>
        /// 
        /// </summary>
        ZoomRectangle = BasicCommandActionType.ZoomRectangle,
        /// <summary>
        /// 
        /// </summary>
        ZoomToSelection = BasicCommandActionType.ZoomToSelection,
        /// <summary>
        /// 
        /// </summary>
        FitToWindow = BasicCommandActionType.FitToWindow,
        /// <summary>
        /// 
        /// </summary>
        PreviousView = BasicCommandActionType.PreviousView,
        /// <summary>
        /// 
        /// </summary>
        NextView = BasicCommandActionType.NextView,
        /// <summary>
        /// 
        /// </summary>
        RestoreView = BasicCommandActionType.RestoreView,
        /// <summary>
        /// 
        /// </summary>
        Select = BasicCommandActionType.Select,
        /// <summary>
        /// 
        /// </summary>
        SelectRadius = BasicCommandActionType.SelectRadius,
        /// <summary>
        /// 
        /// </summary>
        SelectPolygon = BasicCommandActionType.SelectPolygon,
        /// <summary>
        /// 
        /// </summary>
        ClearSelection = BasicCommandActionType.ClearSelection,
        /// <summary>
        /// 
        /// </summary>
        Refresh = BasicCommandActionType.Refresh,
        /// <summary>
        /// 
        /// </summary>
        CopyMap = BasicCommandActionType.CopyMap,
        /// <summary>
        /// 
        /// </summary>
        About = BasicCommandActionType.About,
        /// <summary>
        /// 
        /// </summary>
        Buffer,
        /// <summary>
        /// 
        /// </summary>
        SelectWithin,
        /// <summary>
        /// 
        /// </summary>
        Print,
        /// <summary>
        /// 
        /// </summary>
        GetPrintablePage,
        /// <summary>
        /// 
        /// </summary>
        Measure,
        /// <summary>
        /// 
        /// </summary>
        ViewOptions,
        /// <summary>
        /// 
        /// </summary>
        Help,
    }

    /// <summary>
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    public enum BasicCommandActionType
    {
        /// <remarks/>
        Pan,

        /// <remarks/>
        PanUp,

        /// <remarks/>
        PanDown,

        /// <remarks/>
        PanRight,

        /// <remarks/>
        PanLeft,

        /// <remarks/>
        Zoom,

        /// <remarks/>
        ZoomIn,

        /// <remarks/>
        ZoomOut,

        /// <remarks/>
        ZoomRectangle,

        /// <remarks/>
        ZoomToSelection,

        /// <remarks/>
        FitToWindow,

        /// <remarks/>
        PreviousView,

        /// <remarks/>
        NextView,

        /// <remarks/>
        RestoreView,

        /// <remarks/>
        Select,

        /// <remarks/>
        SelectRadius,

        /// <remarks/>
        SelectPolygon,

        /// <remarks/>
        ClearSelection,

        /// <remarks/>
        Refresh,

        /// <remarks/>
        CopyMap,

        /// <remarks/>
        About,

        /// <remarks/>
        MapTip,
    }

    /// <summary>
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    public enum UIItemFunctionType
    {

        /// <remarks/>
        Separator,

        /// <remarks/>
        Command,

        /// <remarks/>
        Flyout,
    }

    /// <summary>
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    public enum TargetType
    {

        /// <remarks/>
        TaskPane,

        /// <remarks/>
        NewWindow,

        /// <remarks/>
        SpecifiedFrame,
    }

    /// <summary>
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    public enum TargetViewerType
    {

        /// <remarks/>
        Dwf,

        /// <remarks/>
        Ajax,

        /// <remarks/>
        All,
    }

    /// <summary>
    /// Represents a result of a command import
    /// </summary>
    public class ImportedCommandResult
    {
        /// <summary>
        /// Gets or sets the original name
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the imported name
        /// </summary>
        public string ImportedName { get; set; }

        /// <summary>
        /// Gets whether the name was changed when importing
        /// </summary>
        public bool NameChanged { get { return !this.ImportedName.Equals(this.OriginalName); } }

        /// <summary>
        /// Gets the string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} => {1}", this.OriginalName, this.ImportedName); //NOXLATE
        }
    }

    /// <summary>
    /// The Web Layout
    /// </summary>
    public interface IWebLayout : IResource, INotifyPropertyChanged
    {
        /// <summary>
        /// Exports the specified commands to the specified file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cmdNames"></param>
        void ExportCustomCommands(string file, string[] cmdNames);

        /// <summary>
        /// Imports commands from the specified file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        ImportedCommandResult[] ImportCustomCommands(string file);

        /// <summary>
        /// Determines whether the specified command name is referenced in any regions
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="region">The region.</param>
        bool IsCommandReferenced(string name, out WebLayoutRegion[] region);

        /// <summary>
        /// Removes all references to the specified command name
        /// </summary>
        /// <param name="cmdName">Name of the command.</param>
        /// <returns></returns>
        int RemoveAllReferences(string cmdName);

        /// <summary>
        /// Gets a command by its name
        /// </summary>
        /// <param name="cmdName">Name of the command.</param>
        /// <returns></returns>
        ICommand GetCommandByName(string cmdName);

        /// <summary>
        /// Gets the custom commands.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommand> GetCustomCommands();

        /// <summary>
        /// Finds the command by its name in the specified menu.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="menu">The menu.</param>
        /// <returns></returns>
        bool FindCommand(string name, IMenu menu);

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Creates the default view.
        /// </summary>
        /// <returns></returns>
        IMapView CreateDefaultView();

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>The map.</value>
        IMap Map { get; }

        /// <summary>
        /// Gets the task pane.
        /// </summary>
        /// <value>The task pane.</value>
        ITaskPane TaskPane { get; }

        /// <summary>
        /// Gets the tool bar.
        /// </summary>
        /// <value>The tool bar.</value>
        IToolbar ToolBar { get; }

        /// <summary>
        /// Gets the information pane.
        /// </summary>
        /// <value>The information pane.</value>
        IInformationPane InformationPane { get; }

        /// <summary>
        /// Gets the context menu.
        /// </summary>
        /// <value>The context menu.</value>
        IContextMenu ContextMenu { get; }

        /// <summary>
        /// Gets the status bar.
        /// </summary>
        /// <value>The status bar.</value>
        IStatusBar StatusBar { get; }

        /// <summary>
        /// Gets the zoom control.
        /// </summary>
        /// <value>The zoom control.</value>
        IZoomControl ZoomControl { get; }

        /// <summary>
        /// Gets the command set.
        /// </summary>
        /// <value>The command set.</value>
        ICommandSet CommandSet { get; }

        /// <summary>
        /// Creates the basic command.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="description">The description.</param>
        /// <param name="iconName">Name of the icon.</param>
        /// <param name="targets">The targets.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        IBasicCommand CreateBasicCommand(string label, string tooltip, string description, string iconName, TargetViewerType targets, BasicCommandActionType action);

        /// <summary>
        /// Creates the flyout.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="description">The description.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="disabledImageUrl">The disabled image URL.</param>
        /// <param name="subItems">The sub items.</param>
        /// <returns></returns>
        IFlyoutItem CreateFlyout(
            string label,
            string tooltip,
            string description,
            string imageUrl,
            string disabledImageUrl,
            params IUIItem[] subItems);

        /// <summary>
        /// Creates the targeted command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="label">The label.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="description">The description.</param>
        /// <param name="iconName">Name of the icon.</param>
        /// <param name="targets">The targets.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetFrame">The target frame.</param>
        /// <returns></returns>
        T CreateTargetedCommand<T>(string name, string label, string tooltip, string description, string iconName, TargetViewerType targets, TargetType target, string targetFrame) where T : ITargetedCommand, new();

        /// <summary>
        /// Creates the command item.
        /// </summary>
        /// <param name="cmdName">Name of the CMD.</param>
        /// <returns></returns>
        ICommandItem CreateCommandItem(string cmdName);

        /// <summary>
        /// Creates the invoke URL command.
        /// </summary>
        /// <returns></returns>
        IInvokeUrlCommand CreateInvokeUrlCommand();

        /// <summary>
        /// Creates the search command.
        /// </summary>
        /// <returns></returns>
        ISearchCommand CreateSearchCommand();

        /// <summary>
        /// Creates the invoke script command.
        /// </summary>
        /// <returns></returns>
        IInvokeScriptCommand CreateInvokeScriptCommand();

        /// <summary>
        /// Creates the separator.
        /// </summary>
        /// <returns></returns>
        ISeparatorItem CreateSeparator();
    }

    /// <summary>
    /// Web Layout from v1.1.0 schema
    /// </summary>
    public interface IWebLayout2 : IWebLayout
    {
        /// <summary>
        /// Gets or sets a value indicating whether [enable ping server].
        /// </summary>
        /// <value><c>true</c> if [enable ping server]; otherwise, <c>false</c>.</value>
        bool EnablePingServer { get; set; }
    }

    /// <summary>
    /// The map referenced in this web layout
    /// </summary>
    public interface IMap : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the initial view.
        /// </summary>
        /// <value>The initial view.</value>
        IMapView InitialView { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink target for feature selection urls
        /// </summary>
        /// <value>The hyperlink target.</value>
        TargetType HyperlinkTarget { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink target frame for feature selection urls.
        /// </summary>
        /// <value>The hyperlink target frame.</value>
        string HyperlinkTargetFrame { get; set; }
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

    /// <summary>
    /// A web layout element
    /// </summary>
    public interface IWebLayoutControl : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IWebLayoutControl"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }
    }

    /// <summary>
    /// A resizable web layout element
    /// </summary>
    public interface IWebLayoutResizableControl
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int Width { get; set; }
    }

    /// <summary>
    /// A UI element
    /// </summary>
    public interface IUIItem
    {
        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        /// <value>The parent menu.</value>
        IMenu Parent { get; }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>The function.</value>
        UIItemFunctionType Function { get; set; }
    }

    /// <summary>
    /// A UI element with localizable features
    /// </summary>
    public interface ILocalizable : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>The tooltip.</value>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the disabled image URL.
        /// </summary>
        /// <value>The disabled image URL.</value>
        string DisabledImageURL { get; set; }
    }

    /// <summary>
    /// A separator item
    /// </summary>
    public interface ISeparatorItem : IUIItem { }

    /// <summary>
    /// A menu item that invokes a command
    /// </summary>
    public interface ICommandItem : IUIItem
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        string Command { get; set; }
    }

    /// <summary>
    /// A flyout menu item
    /// </summary>
    public interface IFlyoutItem : IUIItem, IMenu, ILocalizable 
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        string Label { get; set; }
    }

    /// <summary>
    /// Represents a UI element that can have any number of child UI elements
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
    /// Extension method class
    /// </summary>
    public static class MenuExtensions
    {
        /// <summary>
        /// Adds items to the menu
        /// </summary>
        /// <param name="mnu"></param>
        /// <param name="items"></param>
        public static void AddItems(this IMenu mnu, params IUIItem[] items)
        {
            Check.NotNull(mnu, "mnu"); //NOXLATE
            Check.NotNull(items, "items"); //NOXLATE
            foreach (var item in items)
            {
                mnu.AddItem(item);
            }
        }
    }
    
    /// <summary>
    /// The viewer toolbar
    /// </summary>
    public interface IToolbar : IMenu, IWebLayoutControl, INotifyPropertyChanged
    { 
        
    }

    /// <summary>
    /// The legend and property pane
    /// </summary>
    public interface IInformationPane : IWebLayoutResizableControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether [legend visible].
        /// </summary>
        /// <value><c>true</c> if [legend visible]; otherwise, <c>false</c>.</value>
        bool LegendVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [properties visible].
        /// </summary>
        /// <value><c>true</c> if [properties visible]; otherwise, <c>false</c>.</value>
        bool PropertiesVisible { get; set; }
    }

    /// <summary>
    /// The viewer context menu
    /// </summary>
    public interface IContextMenu : IMenu, IWebLayoutControl, INotifyPropertyChanged
    {
    }

    /// <summary>
    /// The task pane
    /// </summary>
    public interface ITaskPane : IWebLayoutResizableControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets whether this control is visible
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the url of the initial task.
        /// </summary>
        /// <value>The url of the initial task.</value>
        string InitialTask { get; set; }

        /// <summary>
        /// Gets the task bar.
        /// </summary>
        /// <value>The task bar.</value>
        ITaskBar TaskBar { get; }
    }

    /// <summary>
    /// The task bar
    /// </summary>
    public interface ITaskBar : IWebLayoutControl, IMenu, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the home button.
        /// </summary>
        /// <value>The home.</value>
        ITaskButton Home { get; }

        /// <summary>
        /// Gets the forward button.
        /// </summary>
        /// <value>The forward.</value>
        ITaskButton Forward { get; }

        /// <summary>
        /// Gets the back button.
        /// </summary>
        /// <value>The back.</value>
        ITaskButton Back { get; }

        /// <summary>
        /// Gets the tasks button.
        /// </summary>
        /// <value>The tasks.</value>
        ITaskButton Tasks { get; }
    }

    /// <summary>
    /// A button on the task pane
    /// </summary>
    public interface ITaskButton : ILocalizable
    {

    }

    /// <summary>
    /// The status bar
    /// </summary>
    public interface IStatusBar : IWebLayoutControl, INotifyPropertyChanged
    {
    }

    /// <summary>
    /// The zoom slider
    /// </summary>
    public interface IZoomControl : IWebLayoutControl, INotifyPropertyChanged
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void CommandEventHandler(ICommand cmd);

    /// <summary>
    /// The master list of viewer commands
    /// </summary>
    public interface ICommandSet
    {
        /// <summary>
        /// Gets the command count.
        /// </summary>
        /// <value>The command count.</value>
        int CommandCount { get; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <value>The commands.</value>
        IEnumerable<ICommand> Commands { get; }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        void AddCommand(ICommand cmd);

        /// <summary>
        /// Removes the command.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        void RemoveCommand(ICommand cmd);

        /// <summary>
        /// Occurs when [custom command added].
        /// </summary>
        event CommandEventHandler CustomCommandAdded;

        /// <summary>
        /// Occurs when [custom command removed].
        /// </summary>
        event CommandEventHandler CustomCommandRemoved;
    }

    /// <summary>
    /// Base viewer command
    /// </summary>
    public interface ICommand : ILocalizable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets the target viewer.
        /// </summary>
        /// <value>The target viewer.</value>
        TargetViewerType TargetViewer { get; set; }
    }

    /// <summary>
    /// A built-in basic command
    /// </summary>
    public interface IBasicCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        BasicCommandActionType Action { get; set; }
    }

    /// <summary>
    /// A custom command
    /// </summary>
    public interface ICustomCommand : ICommand
    {

    }

    /// <summary>
    /// A command that operates in a certain viewer frame
    /// </summary>
    public interface ITargetedCommand : ICustomCommand
    {
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        TargetType Target { get; set; }

        /// <summary>
        /// Gets or sets the target frame.
        /// </summary>
        /// <value>The target frame.</value>
        string TargetFrame { get; set; }
    }

    /// <summary>
    /// Search command
    /// </summary>
    public interface ISearchCommand : ITargetedCommand
    {
        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        /// <value>The layer.</value>
        string Layer { get; set; }

        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        /// <value>The prompt.</value>
        string Prompt { get; set; }

        /// <summary>
        /// Gets the result columns.
        /// </summary>
        /// <value>The result columns.</value>
        IResultColumnSet ResultColumns { get; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        string Filter { get; set; }

        /// <summary>
        /// Gets or sets the match limit.
        /// </summary>
        /// <value>The match limit.</value>
        int MatchLimit { get; set; }
    }

    /// <summary>
    /// A search command result specification
    /// </summary>
    public interface IResultColumnSet
    {
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        IEnumerable<IResultColumn> Column { get; }

        /// <summary>
        /// Creates the column.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        IResultColumn CreateColumn(string name, string property);

        /// <summary>
        /// Adds the result column.
        /// </summary>
        /// <param name="col">The col.</param>
        void AddResultColumn(IResultColumn col);

        /// <summary>
        /// Removes the result column.
        /// </summary>
        /// <param name="col">The col.</param>
        void RemoveResultColumn(IResultColumn col);
    }

    /// <summary>
    /// A search command result column
    /// </summary>
    public interface IResultColumn
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The property.</value>
        string Property { get; set; }
    }

    /// <summary>
    /// Invoke URL command
    /// </summary>
    public interface IInvokeUrlCommand : ITargetedCommand
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; set; }

        /// <summary>
        /// Gets the layer set that this layer applies to
        /// </summary>
        /// <value>The layer set.</value>
        ILayerSet LayerSet { get; }

        /// <summary>
        /// Gets the additional parameters.
        /// </summary>
        /// <value>The additional parameters.</value>
        IEnumerable<IParameterPair> AdditionalParameter { get; }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IParameterPair CreateParameter(string name, string value);

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="param">The param.</param>
        void AddParameter(IParameterPair param);

        /// <summary>
        /// Removes the parameter.
        /// </summary>
        /// <param name="param">The param.</param>
        void RemoveParameter(IParameterPair param);

        /// <summary>
        /// Gets or sets a value indicating whether [disable if selection empty].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [disable if selection empty]; otherwise, <c>false</c>.
        /// </value>
        bool DisableIfSelectionEmpty { get; set; }
    }

    /// <summary>
    /// A set of layers
    /// </summary>
    public interface ILayerSet
    {
        /// <summary>
        /// Gets the layers.
        /// </summary>
        /// <value>The layers.</value>
        BindingList<string> Layer { get; }
    }

    /// <summary>
    /// A key value pair
    /// </summary>
    public interface IParameterPair
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; set; }
    }

    /// <summary>
    /// Buffer command
    /// </summary>
    public interface IBufferCommand : ITargetedCommand { }

    /// <summary>
    /// Select within command
    /// </summary>
    public interface ISelectWithinCommand : ITargetedCommand { }

    /// <summary>
    /// A resource reference
    /// </summary>
    public interface IResourceReference : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }
    }

    /// <summary>
    /// Print command
    /// </summary>
    public interface IPrintCommand : ICustomCommand 
    {
        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the print layouts.
        /// </summary>
        /// <value>The print layouts.</value>
        IEnumerable<IResourceReference> PrintLayout { get; }

        /// <summary>
        /// Creates the print layout.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        IResourceReference CreatePrintLayout(string resourceId);

        /// <summary>
        /// Adds the print layout.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void AddPrintLayout(IResourceReference reference);

        /// <summary>
        /// Removes the print layout.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void RemovePrintLayout(IResourceReference reference);
    }

    /// <summary>
    /// Get printable page command
    /// </summary>
    public interface IGetPrintablePageCommand : ITargetedCommand { }

    /// <summary>
    /// Measure command
    /// </summary>
    public interface IMeasureCommand : ITargetedCommand { }

    /// <summary>
    /// View options command
    /// </summary>
    public interface IViewOptionsCommand : ITargetedCommand { }

    /// <summary>
    /// A help command
    /// </summary>
    public interface IHelpCommand : ITargetedCommand
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; set; }
    }

    /// <summary>
    /// An invoke script command
    /// </summary>
    public interface IInvokeScriptCommand : ICustomCommand
    {
        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        /// <value>The script.</value>
        string Script { get; set; }
    }
}
