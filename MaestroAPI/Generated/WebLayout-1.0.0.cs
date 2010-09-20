#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
namespace OSGeo.MapGuide.MaestroAPI 
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("WebLayout", Namespace="", IsNullable=false)]
    public class WebLayout {
        [System.Xml.Serialization.XmlIgnore]
		public static readonly string SchemaName = "WebLayout-1.0.0.xsd";

        [System.Xml.Serialization.XmlIgnore]
        public static readonly string SchemaName1_1 = "WebLayout-1.1.0.xsd";
        
        [System.Xml.Serialization.XmlIgnore]
        public static string [] ValidSchemaNames 
        { 
            get 
            { 
                return new string[] { SchemaName, SchemaName1_1 }; 
            }
        }

        private string _xsdSchema = SchemaName;

		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
            get { return _xsdSchema; }
			set 
            { 
                if (Array.IndexOf(ValidSchemaNames, value) < 0) 
                    throw new System.Exception("Cannot set the schema name");

                _xsdSchema = value;
            }
		}

		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; } 
		}

        private ServerConnectionI m_serverConnection;

        /// <summary>
        /// Gets or sets the connection used in various operations performed on this object
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public ServerConnectionI CurrentConnection
        {
            get { return m_serverConnection; }
            set { m_serverConnection = value; }
        }

        private string m_title;
        private MapType m_map;
        private string m_ping;
        private ToolBarType m_toolBar;
        private InformationPaneType m_informationPane;
        private ContextMenuType m_contextMenu;
        private TaskPaneType m_taskPane;
        private StatusBarType m_statusBar;
        private ZoomControlType m_zoomControl;
        private CommandTypeCollection m_commandSet;
        
        /// <remarks/>
        public string Title {
            get {
                return this.m_title;
            }
            set {
                this.m_title = value;
            }
        }
        
        /// <remarks/>
        public MapType Map {
            get {
                return this.m_map;
            }
            set {
                this.m_map = value;
            }
        }

        [System.Xml.Serialization.XmlElement()]
        public string EnablePingServer
        {
            get 
            {
                if (this.XsdSchema == SchemaName1_1)
                {
                    if (string.IsNullOrEmpty(m_ping))
                        m_ping = true.ToString();

                    return m_ping.ToLower();
                }
                return null; 
            }
            set 
            {
                if (value != null)
                    m_ping = value.ToLower();
                else
                    m_ping = value;
            }
        }

        
        /// <remarks/>
        public ToolBarType ToolBar {
            get {
                return this.m_toolBar;
            }
            set {
                this.m_toolBar = value;
            }
        }
        
        /// <remarks/>
        public InformationPaneType InformationPane {
            get {
                return this.m_informationPane;
            }
            set {
                this.m_informationPane = value;
            }
        }
        
        /// <remarks/>
        public ContextMenuType ContextMenu {
            get {
                return this.m_contextMenu;
            }
            set {
                this.m_contextMenu = value;
            }
        }
        
        /// <remarks/>
        public TaskPaneType TaskPane {
            get {
                return this.m_taskPane;
            }
            set {
                this.m_taskPane = value;
            }
        }
        
        /// <remarks/>
        public StatusBarType StatusBar {
            get {
                return this.m_statusBar;
            }
            set {
                this.m_statusBar = value;
            }
        }
        
        /// <remarks/>
        public ZoomControlType ZoomControl {
            get {
                return this.m_zoomControl;
            }
            set {
                this.m_zoomControl = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Command", IsNullable=false)]
        public CommandTypeCollection CommandSet {
            get {
                return this.m_commandSet;
            }
            set {
                this.m_commandSet = value;
            }
        }

        public void ConvertToVersion(System.Version version)
        {
            string xsd = "WebLayout-" + version.ToString() + ".xsd";
            this.XsdSchema = xsd;

            //Remove the EnablePingServer element if 1.0.0
            if (version == new Version(1, 0, 0))
            {
                this.EnablePingServer = null;
            }
        }

        public string GetVersion()
        {
            //x.y.z = 5 characters
            return this.XsdSchema.Substring("WebLayout-".Length, 5);
        }
    }
    
    /// <remarks/>
    public class MapType : ResourceReferenceType {
        
        private MapViewType m_initialView;
        
        private TargetType m_hyperlinkTarget;
        
        private string m_hyperlinkTargetFrame;
        
        /// <remarks/>
        public MapViewType InitialView {
            get {
                return this.m_initialView;
            }
            set {
                this.m_initialView = value;
            }
        }
        
        /// <remarks/>
        public TargetType HyperlinkTarget {
            get {
                return this.m_hyperlinkTarget;
            }
            set {
                this.m_hyperlinkTarget = value;
            }
        }
        
        /// <remarks/>
        public string HyperlinkTargetFrame {
            get {
                return this.m_hyperlinkTargetFrame;
            }
            set {
                this.m_hyperlinkTargetFrame = value;
            }
        }
    }
    
    /// <remarks/>
    public class MapViewType {
        
        private System.Double m_centerX;
        
        private System.Double m_centerY;
        
        private System.Double m_scale;
        
        /// <remarks/>
        public System.Double CenterX {
            get {
                return this.m_centerX;
            }
            set {
                this.m_centerX = value;
            }
        }
        
        /// <remarks/>
        public System.Double CenterY {
            get {
                return this.m_centerY;
            }
            set {
                this.m_centerY = value;
            }
        }
        
        /// <remarks/>
        public System.Double Scale {
            get {
                return this.m_scale;
            }
            set {
                this.m_scale = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResultColumnType {
        
        private string m_name;
        
        private string m_property;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Property {
            get {
                return this.m_property;
            }
            set {
                this.m_property = value;
            }
        }
    }
    
    /// <remarks/>
    public class ParameterPairType {
        
        private string m_key;
        
        private string m_value;
        
        /// <remarks/>
        public string Key {
            get {
                return this.m_key;
            }
            set {
                this.m_key = value;
            }
        }
        
        /// <remarks/>
        public string Value {
            get {
                return this.m_value;
            }
            set {
                this.m_value = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrintCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetedCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetPrintablePageCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InvokeURLCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SelectWithinCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BufferCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ViewOptionsCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HelpCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InvokeScriptCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BasicCommandType))]
    public abstract class CommandType {
        
        private string m_name;
        
        private string m_label;
        
        private string m_tooltip;
        
        private string m_description;
        
        private string m_imageURL;
        
        private string m_disabledImageURL;
        
        private TargetViewerType m_targetViewer;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                this.m_tooltip = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string ImageURL {
            get {
                return this.m_imageURL;
            }
            set {
                this.m_imageURL = value;
            }
        }
        
        /// <remarks/>
        public string DisabledImageURL {
            get {
                return this.m_disabledImageURL;
            }
            set {
                this.m_disabledImageURL = value;
            }
        }
        
        /// <remarks/>
        public TargetViewerType TargetViewer {
            get {
                return this.m_targetViewer;
            }
            set {
                this.m_targetViewer = value;
            }
        }
    }
    
    /// <remarks/>
    public enum TargetViewerType {
        
        /// <remarks/>
        Dwf,
        
        /// <remarks/>
        Ajax,
        
        /// <remarks/>
        All,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrintCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetedCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetPrintablePageCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InvokeURLCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SelectWithinCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BufferCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ViewOptionsCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HelpCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InvokeScriptCommandType))]
    public abstract class CustomCommandType : CommandType {
    }
    
    /// <remarks/>
    public class PrintCommandType : CustomCommandType {
        
        private ResourceReferenceTypeCollection m_printLayout;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PrintLayout")]
        public ResourceReferenceTypeCollection PrintLayout {
            get {
                return this.m_printLayout;
            }
            set {
                this.m_printLayout = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MapType))]
    public class ResourceReferenceType {
        
        private string m_resourceId;
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetPrintablePageCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InvokeURLCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SelectWithinCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BufferCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ViewOptionsCommandType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HelpCommandType))]
    public abstract class TargetedCommandType : CustomCommandType {
        
        private TargetType m_target;
        
        private string m_targetFrame;
        
        /// <remarks/>
        public TargetType Target {
            get {
                return this.m_target;
            }
            set {
                this.m_target = value;
            }
        }
        
        /// <remarks/>
        public string TargetFrame {
            get {
                return this.m_targetFrame;
            }
            set {
                this.m_targetFrame = value;
            }
        }
    }
    
    /// <remarks/>
    public enum TargetType {
        
        /// <remarks/>
        TaskPane,
        
        /// <remarks/>
        NewWindow,
        
        /// <remarks/>
        SpecifiedFrame,
    }
    
    /// <remarks/>
    public class SearchCommandType : TargetedCommandType {
        
        private string m_layer;
        
        private string m_prompt;
        
        private ResultColumnTypeCollection m_resultColumns;
        
        private string m_filter;
        
        private int m_matchLimit;
        
        /// <remarks/>
        public string Layer {
            get {
                return this.m_layer;
            }
            set {
                this.m_layer = value;
            }
        }
        
        /// <remarks/>
        public string Prompt {
            get {
                return this.m_prompt;
            }
            set {
                this.m_prompt = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Column", IsNullable=false)]
        public ResultColumnTypeCollection ResultColumns {
            get {
                return this.m_resultColumns;
            }
            set {
                this.m_resultColumns = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
       public int MatchLimit {
            get {
                return this.m_matchLimit;
            }
            set {
                this.m_matchLimit = value;
            }
        }
    }
    
    /// <remarks/>
    public class MeasureCommandType : TargetedCommandType {
    }
    
    /// <remarks/>
    public class GetPrintablePageCommandType : TargetedCommandType {
    }
    
    /// <remarks/>
    public class InvokeURLCommandType : TargetedCommandType {
        
        private string m_uRL;

        private System.Collections.Specialized.StringCollection m_layerSet;
        
        private ParameterPairTypeCollection m_additionalParameter;
        
        private bool m_disableIfSelectionEmpty;
        
        /// <remarks/>
        public string URL {
            get {
                return this.m_uRL;
            }
            set {
                this.m_uRL = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Layer", IsNullable=false)]
        public System.Collections.Specialized.StringCollection LayerSet {
            get {
                return this.m_layerSet;
            }
            set {
                this.m_layerSet = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AdditionalParameter")]
        public ParameterPairTypeCollection AdditionalParameter {
            get {
                return this.m_additionalParameter;
            }
            set {
                this.m_additionalParameter = value;
            }
        }
        
        /// <remarks/>
        public bool DisableIfSelectionEmpty {
            get {
                return this.m_disableIfSelectionEmpty;
            }
            set {
                this.m_disableIfSelectionEmpty = value;
            }
        }
    }
    
    /// <remarks/>
    public class SelectWithinCommandType : TargetedCommandType {
    }
    
    /// <remarks/>
    public class BufferCommandType : TargetedCommandType {
    }
    
    /// <remarks/>
    public class ViewOptionsCommandType : TargetedCommandType {
    }
    
    /// <remarks/>
    public class HelpCommandType : TargetedCommandType {
        
        private string m_uRL;
        
        /// <remarks/>
        public string URL {
            get {
                return this.m_uRL;
            }
            set {
                this.m_uRL = value;
            }
        }
    }
    
    /// <remarks/>
    public class InvokeScriptCommandType : CustomCommandType {
        
        private string m_script;
        
        /// <remarks/>
        public string Script {
            get {
                return this.m_script;
            }
            set {
                this.m_script = value;
            }
        }
    }
    
    /// <remarks/>
    public class BasicCommandType : CommandType {
        
        private BasicCommandActionType m_action;
        
        /// <remarks/>
        public BasicCommandActionType Action {
            get {
                return this.m_action;
            }
            set {
                this.m_action = value;
            }
        }
    }
    
    /// <remarks/>
    public enum BasicCommandActionType {
        
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

	}
    
    /// <remarks/>
    public class TaskButtonType {
        
        private string m_name;
        
        private string m_tooltip;
        
        private string m_description;
        
        private string m_imageURL;
        
        private string m_disabledImageURL;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                this.m_tooltip = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string ImageURL {
            get {
                return this.m_imageURL;
            }
            set {
                this.m_imageURL = value;
            }
        }
        
        /// <remarks/>
        public string DisabledImageURL {
            get {
                return this.m_disabledImageURL;
            }
            set {
                this.m_disabledImageURL = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CommandItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SeparatorItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FlyoutItemType))]
    public class UIItemType {
        
        private UIItemFunctionType m_function;
        
        /// <remarks/>
        public UIItemFunctionType Function {
            get {
                return this.m_function;
            }
            set {
                this.m_function = value;
            }
        }
    }
    
    /// <remarks/>
    public enum UIItemFunctionType {
        
        /// <remarks/>
        Separator,
        
        /// <remarks/>
        Command,
        
        /// <remarks/>
        Flyout,
    }
    
    /// <remarks/>
    public class CommandItemType : UIItemType {
        
        private string m_command;
        
        /// <remarks/>
        public string Command {
            get {
                return this.m_command;
            }
            set {
                this.m_command = value;
            }
        }
    }
    
    /// <remarks/>
    public class SeparatorItemType : UIItemType {
    }
    
    /// <remarks/>
    public class FlyoutItemType : UIItemType {
        
        private string m_label;
        
        private string m_tooltip;
        
        private string m_description;
        
        private string m_imageURL;
        
        private string m_disabledImageURL;
        
        private UIItemTypeCollection m_subItem;
        
        /// <remarks/>
        public string Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                this.m_tooltip = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        public string ImageURL {
            get {
                return this.m_imageURL;
            }
            set {
                this.m_imageURL = value;
            }
        }
        
        /// <remarks/>
        public string DisabledImageURL {
            get {
                return this.m_disabledImageURL;
            }
            set {
                this.m_disabledImageURL = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SubItem")]
        public UIItemTypeCollection SubItem {
            get {
                return this.m_subItem;
            }
            set {
                this.m_subItem = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StatusBarType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskBarType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WebLayoutResizableControlType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InformationPaneType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskPaneType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ZoomControlType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ToolBarType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContextMenuType))]
    public class WebLayoutControlType {
        
        private bool m_visible;
        
        /// <remarks/>
        public bool Visible {
            get {
                return this.m_visible;
            }
            set {
                this.m_visible = value;
            }
        }
    }
    
    /// <remarks/>
    public class StatusBarType : WebLayoutControlType {
    }
    
    /// <remarks/>
    public class TaskBarType : WebLayoutControlType {
        
        private TaskButtonType m_home;
        
        private TaskButtonType m_forward;
        
        private TaskButtonType m_back;
        
        private TaskButtonType m_tasks;
        
        private UIItemTypeCollection m_menuButton;
        
        /// <remarks/>
        public TaskButtonType Home {
            get {
                return this.m_home;
            }
            set {
                this.m_home = value;
            }
        }
        
        /// <remarks/>
        public TaskButtonType Forward {
            get {
                return this.m_forward;
            }
            set {
                this.m_forward = value;
            }
        }
        
        /// <remarks/>
        public TaskButtonType Back {
            get {
                return this.m_back;
            }
            set {
                this.m_back = value;
            }
        }
        
        /// <remarks/>
        public TaskButtonType Tasks {
            get {
                return this.m_tasks;
            }
            set {
                this.m_tasks = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MenuButton")]
        public UIItemTypeCollection MenuButton {
            get {
                return this.m_menuButton;
            }
            set {
                this.m_menuButton = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InformationPaneType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskPaneType))]
    public class WebLayoutResizableControlType : WebLayoutControlType {
        
        private int m_width;
        
        /// <remarks/>
        public int Width {
            get {
                return this.m_width;
            }
            set {
                this.m_width = value;
            }
        }
    }
    
    /// <remarks/>
    public class InformationPaneType : WebLayoutResizableControlType {
        
        private bool m_legendVisible;
        
        private bool m_propertiesVisible;
        
        /// <remarks/>
        public bool LegendVisible {
            get {
                return this.m_legendVisible;
            }
            set {
                this.m_legendVisible = value;
            }
        }
        
        /// <remarks/>
        public bool PropertiesVisible {
            get {
                return this.m_propertiesVisible;
            }
            set {
                this.m_propertiesVisible = value;
            }
        }
    }
    
    /// <remarks/>
    public class TaskPaneType : WebLayoutResizableControlType {
        
        private TaskBarType m_taskBar;
        
        private string m_initialTask;
        
        /// <remarks/>
        public TaskBarType TaskBar {
            get {
                return this.m_taskBar;
            }
            set {
                this.m_taskBar = value;
            }
        }
        
        /// <remarks/>
        public string InitialTask {
            get {
                return this.m_initialTask;
            }
            set {
                this.m_initialTask = value;
            }
        }
    }
    
    /// <remarks/>
    public class ZoomControlType : WebLayoutControlType {
    }
    
    /// <remarks/>
    public class ToolBarType : WebLayoutControlType {
        
        private UIItemTypeCollection m_button;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Button")]
        public UIItemTypeCollection Button {
            get {
                return this.m_button;
            }
            set {
                this.m_button = value;
            }
        }
    }
    
    /// <remarks/>
    public class ContextMenuType : WebLayoutControlType {
        
        private UIItemTypeCollection m_menuItem;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MenuItem")]
        public UIItemTypeCollection MenuItem {
            get {
                return this.m_menuItem;
            }
            set {
                this.m_menuItem = value;
            }
        }
    }
    
    public class CommandTypeCollection : System.Collections.CollectionBase {
        
        public CommandType this[int idx] {
            get {
                return ((CommandType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(CommandType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ResourceReferenceTypeCollection : System.Collections.CollectionBase {
        
        public ResourceReferenceType this[int idx] {
            get {
                return ((ResourceReferenceType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResourceReferenceType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ResultColumnTypeCollection : System.Collections.CollectionBase {
        
        public ResultColumnType this[int idx] {
            get {
                return ((ResultColumnType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResultColumnType value) {
            return base.InnerList.Add(value);
        }
    }

	public class UIItemTypeCollection : System.Collections.CollectionBase 
	{
        
		public UIItemType this[int idx] 
		{
			get 
			{
				return ((UIItemType)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(UIItemType value) 
		{
			return base.InnerList.Add(value);
		}

		public int IndexOf(UIItemType value)
		{
			return base.InnerList.IndexOf(value);
		}

		public void Insert(int index, UIItemType value)
		{
			base.InnerList.Insert(index, value);
		}
	}

	public class ParameterPairTypeCollection : System.Collections.CollectionBase 
	{
        
		public ParameterPairType this[int idx] 
		{
			get 
			{
				return ((ParameterPairType)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(ParameterPairType value) 
		{
			return base.InnerList.Add(value);
		}
	}

}
