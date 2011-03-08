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
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System.ComponentModel;
using System.IO;

#pragma warning disable 1591, 0114, 0108

#if WL110
namespace OSGeo.MapGuide.ObjectModels.WebLayout_1_1_0
#else
namespace OSGeo.MapGuide.ObjectModels.WebLayout_1_0_0
#endif
{
    public static class WebLayoutEntryPoint
    {
        public static IWebLayout CreateDefault(string mapDefinitionId)
        {
            IWebLayout wl = WebLayoutType.CreateDefault(mapDefinitionId);
            return wl;
        }

        public static IResource Deserialize(string xml)
        {
            return WebLayoutType.Deserialize(xml);
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }
    }

    [Serializable]
    public class WebLayoutCustomCommandList
    {
        [XmlArrayItem("CustomCommands")]
        public CommandType[] Commands { get; set; }
    }

    partial class WebLayoutType : IWebLayout, ICommandSet
    {
        internal WebLayoutType() { }

#if WL110
        private static readonly Version RES_VERSION = new Version(1, 1, 0);
#else
        private static readonly Version RES_VERSION = new Version(1, 0, 0);
#endif

        [XmlIgnore]
        public OSGeo.MapGuide.MaestroAPI.IServerConnection CurrentConnection
        {
            get;
            set;
        }

        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.WebLayout.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.WebLayout;
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema
        {
#if WL110
            get { return "WebLayout-1.1.0.xsd"; }
#else
            get { return "WebLayout-1.0.0.xsd"; }
#endif
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        public static IWebLayout CreateDefault(string mapDefinitionId)
        {
            //TODO: Localize these strings. Once localized we can have *translatable*
            //web layouts!

            IWebLayout wl = new WebLayoutType()
            {
                CommandSet = new System.ComponentModel.BindingList<CommandType>(),
                ContextMenu = new ContextMenuType()
                {
                    Visible = true,
                    MenuItem = new System.ComponentModel.BindingList<UIItemType>()
                },
                InformationPane = new InformationPaneType()
                {
                    LegendVisible = true,
                    PropertiesVisible = true,
                    Visible = true,
                    Width = 200
                },
                Map = new MapType()
                {
                    HyperlinkTarget = TargetType.TaskPane,
                    ResourceId = mapDefinitionId,
                    //null InitialView = Use map's initial view
                },
                StatusBar = new StatusBarType()
                {
                    Visible = true
                },
                TaskPane = new TaskPaneType()
                {
                    TaskBar = new TaskBarType()
                    {
                        Back = new TaskButtonType()
                        {
                            Name = "Back",
                            Tooltip = "Return to previous task page",
                            Description = "Return to previous task page",
                            ImageURL = "../stdicons/icon_back.gif",
                            DisabledImageURL = "../stdicons/icon_back_disabled.gif"
                        },
                        Forward = new TaskButtonType()
                        {
                            Name = "Forward",
                            Tooltip = "Forward to next task page",
                            Description = "Forward to next task page",
                            ImageURL = "../stdicons/icon_forward.gif",
                            DisabledImageURL = "../stdicons/icon_forward_disabled.gif"
                        },
                        Home = new TaskButtonType()
                        {
                            Name = "Home",
                            Tooltip = "Return to home task page",
                            Description = "Return to home task page",
                            ImageURL = "../stdicons/icon_home.gif",
                            DisabledImageURL = "../stdicons/icon_home_disabled.gif"
                        },
                        //Task Pane menu buttons
                        MenuButton = new System.ComponentModel.BindingList<UIItemType>(),
                        Tasks = new TaskButtonType()
                        {
                            Name = "Tasks",
                            Tooltip = "Task list",
                            Description = "View a list of available tasks",
                            ImageURL = "../stdicons/icon_tasks.gif",
                            DisabledImageURL = "../stdicons/icon_tasks_disabled.gif"
                        },
                        Visible = true,
                    },
                    Visible = true,
                    Width = 250,
                },
                Title = string.Empty,
                ToolBar = new ToolBarType()
                {
                    Visible = true,
                    Button = new System.ComponentModel.BindingList<UIItemType>()
                },
                ZoomControl = new ZoomControlType()
                {
                    Visible = true
                },
            };

            CreateDefaultCommandSet(wl);
            CreateDefaultContextMenu(wl);
            CreateDefaultToolbar(wl);

            return wl;
        }

        private static void CreateDefaultToolbar(IWebLayout wl)
        {
            wl.ToolBar.AddItems(
            wl.CreateCommandItem(BuiltInCommandType.Print.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.GetPrintablePage.ToString()),
            wl.CreateSeparator(),
            wl.CreateCommandItem(BuiltInCommandType.Measure.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.Buffer.ToString()),
            wl.CreateSeparator(),
            wl.CreateFlyout("Zoom", null, null, null, null,
                wl.CreateCommandItem(BuiltInCommandType.PreviousView.ToString()),
                wl.CreateCommandItem(BuiltInCommandType.NextView.ToString()),
                wl.CreateCommandItem(BuiltInCommandType.RestoreView.ToString())
            ),
            wl.CreateSeparator(),
            wl.CreateCommandItem(BuiltInCommandType.ZoomRectangle.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.ZoomIn.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.ZoomOut.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.Zoom.ToString()),
            wl.CreateSeparator(),
            wl.CreateCommandItem(BuiltInCommandType.Select.ToString()),
            wl.CreateCommandItem(BuiltInCommandType.Pan.ToString())
            );
        }
        private static void CreateDefaultContextMenu(IWebLayout wl)
        {
            wl.ContextMenu.AddItems(
                            wl.CreateCommandItem(BuiltInCommandType.Select.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.ClearSelection.ToString()),
                                wl.CreateFlyout("Select More", null, null, null, null,
                                    wl.CreateCommandItem(BuiltInCommandType.SelectRadius.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.SelectPolygon.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.SelectWithin.ToString())
                                ),
                                wl.CreateCommandItem(BuiltInCommandType.Pan.ToString()),
                                wl.CreateSeparator(),
                                wl.CreateCommandItem(BuiltInCommandType.ZoomRectangle.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.ZoomIn.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.ZoomOut.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.Zoom.ToString()),
                                wl.CreateFlyout("Zoom", null, null, null, null,
                                    wl.CreateCommandItem(BuiltInCommandType.PreviousView.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.NextView.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.FitToWindow.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.RestoreView.ToString()),
                                    wl.CreateCommandItem(BuiltInCommandType.ZoomToSelection.ToString())
                                ),
                                wl.CreateSeparator(),
                                wl.CreateCommandItem(BuiltInCommandType.Measure.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.Buffer.ToString()),
                                wl.CreateSeparator(),
                                wl.CreateCommandItem(BuiltInCommandType.Refresh.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.CopyMap.ToString()),
                                wl.CreateSeparator(),
                                wl.CreateCommandItem(BuiltInCommandType.Print.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.GetPrintablePage.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.ViewOptions.ToString()),
                                wl.CreateSeparator(),
                                wl.CreateCommandItem(BuiltInCommandType.Help.ToString()),
                                wl.CreateCommandItem(BuiltInCommandType.About.ToString())
                                );
        }

        private static void CreateDefaultCommandSet(IWebLayout wl)
        {
            wl.CommandSet.AddCommand(
                            wl.CreateBasicCommand("Pan",
                                               "Pan Mode",
                                               "Drag the map to view areas out of range",
                                               "icon_pan",
                                               TargetViewerType.All,
                                               BasicCommandActionType.Pan));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Pan Up",
                               "Pan Up",
                               "Pan up by a preset increment",
                               "icon_panup",
                               TargetViewerType.All,
                               BasicCommandActionType.PanUp));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Pan Down",
                               "Pan Down",
                               "Pan down by a preset increment",
                               "icon_pandown",
                               TargetViewerType.All,
                               BasicCommandActionType.PanDown));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Pan Right",
                               "Pan Right",
                               "Pan right by a preset increment",
                               "icon_panright",
                               TargetViewerType.All,
                               BasicCommandActionType.PanRight));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Pan Left",
                               "Pan Left",
                               "Pan left by a preset increment",
                               "icon_panleft",
                               TargetViewerType.All,
                               BasicCommandActionType.PanLeft));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Zoom",
                               "Zoom Dynamic",
                               "Zoom dynamically by clicking and dragging",
                               "icon_zoom",
                               TargetViewerType.Dwf,
                               BasicCommandActionType.Zoom));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Zoom In",
                               "Zoom In",
                               "Zoom in by a preset increment",
                               "icon_zoomin",
                               TargetViewerType.All,
                               BasicCommandActionType.ZoomIn));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Zoom Out",
                               "Zoom Out",
                               "Zoom out by a preset increment",
                               "icon_zoomout",
                               TargetViewerType.All,
                               BasicCommandActionType.ZoomOut));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Zoom Rectangle",
                               "Zoom Rectangle",
                               "Zoom in on an area",
                               "icon_zoomrect",
                               TargetViewerType.All,
                               BasicCommandActionType.ZoomRectangle));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Zoom Selection",
                               "Zoom to selection",
                               "Zoom to extents of selected features",
                               "icon_zoomselect",
                               TargetViewerType.All,
                               BasicCommandActionType.ZoomToSelection));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Initial Map View",
                               "Initial Map View",
                               "Fit the extents of the map to the window",
                               "icon_fitwindow",
                               TargetViewerType.All,
                               BasicCommandActionType.FitToWindow));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Previous View",
                               "Previous View",
                               "Go to previous view",
                               "icon_zoomprev",
                               TargetViewerType.All,
                               BasicCommandActionType.PreviousView));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Next View",
                               "Next View",
                               "Go to next view",
                               "icon_zoomnext",
                               TargetViewerType.All,
                               BasicCommandActionType.NextView));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Initial Center and Scale",
                               "Initial Center and Scale",
                               "Zoom to initial map center and scale",
                               "icon_restorecenter.gif",
                               TargetViewerType.All,
                               BasicCommandActionType.RestoreView));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Select",
                               "Select Mode",
                               "Select features by clicking and dragging",
                               "icon_select",
                               TargetViewerType.All,
                               BasicCommandActionType.Select));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Select Radius",
                               "Select Radius",
                               "Click and drag to select all features inside the circle",
                               "icon_selectradius",
                               TargetViewerType.All,
                               BasicCommandActionType.SelectRadius));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Select Polygon",
                               "Select Polygon",
                               "Create a polygon to select all features that fall within",
                               "icon_selectpolygon",
                               TargetViewerType.All,
                               BasicCommandActionType.SelectPolygon));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Clear Selection",
                               "Clear Selection",
                               "Clear the current selections",
                               "icon_clearselect",
                               TargetViewerType.All,
                               BasicCommandActionType.ClearSelection));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Refresh Map",
                               "Refresh Map",
                               "Refresh map to reload all layers keeping current center and scale",
                               "icon_refreshmap",
                               TargetViewerType.All,
                               BasicCommandActionType.Refresh));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("Copy",
                               "Copy",
                               "Copy current view of map to clipboard",
                               "icon_copy",
                               TargetViewerType.Dwf,
                               BasicCommandActionType.CopyMap));
            wl.CommandSet.AddCommand(
            wl.CreateBasicCommand("About",
                               "About",
                               "Display information about this application",
                               null,
                               TargetViewerType.All,
                               BasicCommandActionType.About));
            wl.CommandSet.AddCommand(
            wl.CreateTargetedCommand<BufferCommandType>(BuiltInCommandType.Buffer.ToString(),
                                                     "Buffer",
                                                     "Buffer",
                                                     "Create buffers around the selected features",
                                                     "icon_buffer",
                                                     TargetViewerType.All,
                                                     TargetType.TaskPane,
                                                     null));
            wl.CommandSet.AddCommand(
            wl.CreateTargetedCommand<SelectWithinCommandType>(BuiltInCommandType.SelectWithin.ToString(),
                                                     "Select Within",
                                                     "Select Within",
                                                     "Select features that fall within currently selected areas",
                                                     "icon_selectwithin",
                                                     TargetViewerType.All,
                                                     TargetType.TaskPane,
                                                     null));
            wl.CommandSet.AddCommand(
            wl.CreateTargetedCommand<MeasureCommandType>(BuiltInCommandType.Measure.ToString(),
                                                      "Measure",
                                                      "Measure",
                                                      "Measure distances on the map",
                                                      "icon_measure",
                                                      TargetViewerType.All,
                                                      TargetType.TaskPane,
                                                      null));
            wl.CommandSet.AddCommand(
            new PrintCommandType()
            {
                Name = "Print",
                Label = "Print",
                Tooltip = "Print",
                Description = "Print with optional layout",
                ImageURL = "../stdicons/icon_print.gif",
                DisabledImageURL = "../stdicons/icon_print_disabled.gif",
                TargetViewer = TargetViewerType.Dwf
            });
            wl.CommandSet.AddCommand(
            wl.CreateTargetedCommand<ViewOptionsCommandType>(BuiltInCommandType.ViewOptions.ToString(),
                                                          "View Options",
                                                          "View Options",
                                                          "View Options",
                                                          "icon_viewoptions",
                                                          TargetViewerType.All,
                                                          TargetType.TaskPane,
                                                          null));
            wl.CommandSet.AddCommand(
            wl.CreateTargetedCommand<GetPrintablePageCommandType>(BuiltInCommandType.GetPrintablePage.ToString(),
                                                          "Get Printable Page",
                                                          "Get Printable Page",
                                                          "Get Printer-friendly page",
                                                          "icon_printablepage",
                                                          TargetViewerType.Ajax,
                                                          TargetType.NewWindow,
                                                          null));
            wl.CommandSet.AddCommand(
            new HelpCommandType()
            {
                Name = BuiltInCommandType.Help.ToString(),
                Label = "Help",
                Tooltip = "Help",
                Description = "Launch help for this application",
                ImageURL = "../stdicons/icon_help.gif",
                DisabledImageURL = "../stdicons/icon_help_disabled.gif",
                TargetViewer = TargetViewerType.All,
                Target = TargetType.TaskPane
            });
        }

        public IMapView CreateDefaultView()
        {
            return new MapViewType();
        }

        /// <summary>
        /// Indicates whether a given command is referenced in the user interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool IsCommandReferenced(string name, out WebLayoutRegion[] region)
        {
            region = new WebLayoutRegion[0];
            List<WebLayoutRegion> regions = new List<WebLayoutRegion>();
            if (FindCommand(name, this.ContextMenu))
            {
                regions.Add(WebLayoutRegion.ContextMenu);
            }

            if (FindCommand(name, this.ToolBar))
            {
                regions.Add(WebLayoutRegion.Toolbar);
            }

            if (FindCommand(name, this.TaskPane.TaskBar))
            {
                regions.Add(WebLayoutRegion.TaskBar);
            }

            region = regions.ToArray();
            return region.Length > 0;
        }

        /// <summary>
        /// Removes all references of a given command
        /// </summary>
        /// <param name="cmdName"></param>
        /// <returns>The number of references removed</returns>
        public int RemoveAllReferences(string cmdName)
        {
            int removed = 0;
            removed += RemoveInternal(cmdName, contextMenuField.MenuItem);
            removed += RemoveInternal(cmdName, toolBarField.Button);
            removed += RemoveInternal(cmdName, taskPaneField.TaskBar.MenuButton);
            return removed;
        }

        private int RemoveInternal(string cmdName, IList<UIItemType> items)
        {
            int foundCount = 0;
            List<IList<UIItemType>> subItemCheck = new List<IList<UIItemType>>();
            List<UIItemType> found = new List<UIItemType>();
            foreach (var item in items)
            {
                if (item.Function == UIItemFunctionType.Command)
                {
                    if (((CommandItemType)item).Command == cmdName)
                        found.Add(item);
                }
                else if (item.Function == UIItemFunctionType.Flyout)
                {
                    subItemCheck.Add(((FlyoutItemType)item).SubItem);
                }
            }
            foundCount += found.Count;
            //Purge any found
            foreach (var item in found)
            {
                items.Remove(item);
            }
            //Check these sub lists
            foreach (var list in subItemCheck)
            {
                foundCount += RemoveInternal(cmdName, list);
            }

            return foundCount;
        }

        public ICommand GetCommandByName(string cmdName)
        {
            foreach (var cmd in this.CommandSet)
            {
                if (cmd.Name == cmdName)
                    return cmd;
            }

            return null;
        }

        public IEnumerable<ICommand> GetCustomCommands()
        {
            foreach (var cmd in this.CommandSet)
            {
                var type = cmd.GetType();
                if (typeof(InvokeURLCommandType).IsAssignableFrom(type) ||
                    typeof(InvokeScriptCommandType).IsAssignableFrom(type) ||
                    typeof(SearchCommandType).IsAssignableFrom(type))
                {
                    yield return cmd;
                }
            }
        }

        public bool FindCommand(string name, IMenu menu)
        {
            foreach (var item in menu.Items)
            {
                if (item.Function == UIItemFunctionType.Command)
                {
                    if (((CommandItemType)item).Command == name)
                        return true;
                }
                else if (item.Function == UIItemFunctionType.Flyout)
                {
                    return FindCommand(name, ((IFlyoutItem)item));
                }
            }
            return false;
        }

        public IBasicCommand CreateBasicCommand(string label, string tooltip, string description, string iconName, TargetViewerType targets, BasicCommandActionType action)
        {
            var cmd = new BasicCommandType()
            {
                Name = action.ToString(),
                Label = label,
                Tooltip = tooltip,
                Description = description,

                TargetViewer = targets,
                Action = action
            };

            if (!string.IsNullOrEmpty(iconName))
            {
                cmd.ImageURL = "../stdicons/" + iconName + ".gif";
                cmd.DisabledImageURL = "../stdicons/" + iconName + "_disabled.gif";
            }

            return cmd;
        }

        public IInvokeUrlCommand CreateInvokeUrlCommand()
        {
            return new InvokeURLCommandType()
            {
                Target = TargetType.TaskPane,
                DisableIfSelectionEmpty = false,
                ImageURL = "../stdicons/icon_invokeurl.gif",
                DisabledImageURL = "../stdicons/icon_invokeurl_disabled.gif",
                TargetViewer = TargetViewerType.All,
                AdditionalParameter = new BindingList<ParameterPairType>(),
                LayerSet = new BindingList<string>()
            };
        }

        public ISearchCommand CreateSearchCommand()
        {
            return new SearchCommandType()
            {
                ResultColumns = new System.ComponentModel.BindingList<ResultColumnType>(),
                Target = TargetType.TaskPane,
                TargetViewer = TargetViewerType.All,
                DisabledImageURL = "../stdicons/icon_search_disabled.gif",
                ImageURL = "../stdicons/icon_search.gif",
                Filter = string.Empty,
                MatchLimit = "100",
                Prompt = string.Empty
            };
        }

        public IInvokeScriptCommand CreateInvokeScriptCommand()
        {
            return new InvokeScriptCommandType()
            {
                DisabledImageURL = "../stdicons/icon_invokescript_disabled.gif",
                ImageURL = "../stdicons/icon_invokescript.gif",
                TargetViewer = TargetViewerType.All
            };
        }

        public T CreateTargetedCommand<T>(string name, string label, string tooltip, string description, string iconName, TargetViewerType targets, TargetType target, string targetFrame) where T : ITargetedCommand, new()
        {
            var cmd = new T()
            {
                Name = name,
                Label = label,
                Tooltip = tooltip,
                Description = description,
                TargetViewer = targets,
                Target = target,
            };

            if (!string.IsNullOrEmpty(targetFrame) && target == TargetType.SpecifiedFrame)
            {
                cmd.TargetFrame = targetFrame;
            }

            if (!string.IsNullOrEmpty(iconName))
            {
                cmd.ImageURL = "../stdicons/" + iconName + ".gif";
                cmd.DisabledImageURL = "../stdicons/" + iconName + "_disabled.gif";
            }

            return cmd;
        }

        public IFlyoutItem CreateFlyout(
            string label,
            string tooltip,
            string description,
            string imageUrl,
            string disabledImageUrl,
            params IUIItem[] subItems)
        {
            IFlyoutItem flyout = new FlyoutItemType()
            {
                Function = UIItemFunctionType.Flyout,
                Description = description,
                DisabledImageURL = disabledImageUrl,
                ImageURL = imageUrl,
                Label = label,
                SubItem = new System.ComponentModel.BindingList<UIItemType>(),
                Tooltip = tooltip
            };
            flyout.AddItems(subItems);
            return flyout;
        }

        public ICommandItem CreateCommandItem(string cmdName) 
        {
            return new CommandItemType() { Function = UIItemFunctionType.Command, Command = cmdName };
        }

        public ISeparatorItem CreateSeparator()
        {
            return new SeparatorItemType() { Function = UIItemFunctionType.Separator };
        }

        public void ExportCustomCommands(string file, string[] cmdNames)
        {
            WebLayoutCustomCommandList list = new WebLayoutCustomCommandList();

            List<CommandType> commands = new List<CommandType>();

            foreach (var name in cmdNames)
            {
                var cmd = (CommandType)GetCommandByName(name);
                if (cmd != null)
                    commands.Add(cmd);
            }

            list.Commands = commands.ToArray();

            using (var fs = File.OpenWrite(file))
            {
                new XmlSerializer(typeof(WebLayoutCustomCommandList)).Serialize(fs, list);
            }
        }

        public ImportedCommandResult[] ImportCustomCommands(string file)
        {
            List<ImportedCommandResult> clashes = new List<ImportedCommandResult>();

            using (var fs = File.OpenRead(file))
            {
                var list = (WebLayoutCustomCommandList)(new XmlSerializer(typeof(WebLayoutCustomCommandList)).Deserialize(fs));
                foreach (var importCmd in list.Commands)
                {
                    int counter = 0;
                    string oldName = importCmd.Name;
                    string newName = oldName;

                    ICommand cmd = this.GetCommandByName(newName);
                    while (cmd != null)
                    {
                        counter++;
                        newName = oldName + counter;
                        cmd = this.GetCommandByName(newName);
                    }

                    this.commandSetField.Add(importCmd);
                    
                    clashes.Add(new ImportedCommandResult() { OriginalName = oldName, ImportedName = newName });
                }
            }

            return clashes.ToArray();
        }

        [XmlIgnore]
        ICommandSet IWebLayout.CommandSet
        {
            get { return this; }
        }


        void ICommandSet.Clear()
        {
            this.CommandSet.Clear();
        }

        [XmlIgnore]
        int ICommandSet.CommandCount
        {
            get { return this.CommandSet.Count; }
        }

        [XmlIgnore]
        IEnumerable<ICommand> ICommandSet.Commands
        {
            get
            {
                foreach (var cmd in this.CommandSet)
                {
                    yield return cmd;
                }
            }
        }

        public event CommandEventHandler CustomCommandAdded;

        public event CommandEventHandler CustomCommandRemoved;

        void ICommandSet.AddCommand(ICommand cmd)
        {
            var c = cmd as CommandType;
            if (c != null)
            {
                this.CommandSet.Add(c);
                OnPropertyChanged("CommandSet");
                if (cmd is IInvokeUrlCommand || cmd is IInvokeScriptCommand || cmd is ISearchCommand)
                {
                    var handler = this.CustomCommandAdded;
                    if (handler != null)
                        handler(cmd);
                }
            }
        }

        void ICommandSet.RemoveCommand(ICommand cmd)
        {
            var c = cmd as CommandType;
            if (c != null)
            {
                this.CommandSet.Remove(c);
                OnPropertyChanged("CommandSet");
                if (cmd is IInvokeUrlCommand || cmd is IInvokeScriptCommand || cmd is ISearchCommand)
                {
                    var handler = this.CustomCommandRemoved;
                    if (handler != null)
                        handler(cmd);
                }
            }
        }

        [XmlIgnore]
        string IWebLayout.Title
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        [XmlIgnore]
        IMap IWebLayout.Map
        {
            get { return this.Map; }
        }

        [XmlIgnore]
        ITaskPane IWebLayout.TaskPane
        {
            get { return this.TaskPane; }
        }

        [XmlIgnore]
        IToolbar IWebLayout.ToolBar
        {
            get { return this.ToolBar; }
        }

        [XmlIgnore]
        IInformationPane IWebLayout.InformationPane
        {
            get { return this.InformationPane; }
        }

        [XmlIgnore]
        IContextMenu IWebLayout.ContextMenu
        {
            get { return this.ContextMenu; }
        }

        [XmlIgnore]
        IStatusBar IWebLayout.StatusBar
        {
            get { return this.StatusBar; }
        }

        [XmlIgnore]
        IZoomControl IWebLayout.ZoomControl
        {
            get { return this.ZoomControl; }
        }
    }

    partial class TaskPaneType : ITaskPane
    {
        [XmlIgnore]
        ITaskBar ITaskPane.TaskBar
        {
            get
            {
                return this.TaskBar;
            }
        }
    }

    partial class MapType : IMap
    {
        [XmlIgnore]
        IMapView IMap.InitialView
        {
            get
            {
                return this.InitialView;
            }
            set
            {
                this.InitialView = (MapViewType)value;
            }
        }
    }

    partial class MapViewType : IMapView
    {

    }

    partial class ToolBarType : IToolbar
    {
        [XmlIgnore]
        public int ItemCount
        {
            get { return this.Button.Count; }
        }

        [XmlIgnore]
        public IEnumerable<IUIItem> Items
        {
            get 
            {
                foreach (var item in this.Button)
                {
                    yield return item;
                }
            }
        }

        public void AddItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.Button.Add(it);
                it.Parent = this;
                OnPropertyChanged("Button");
            }
        }

        public void RemoveItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.Button.Remove(it);
                it.Parent = null;
                OnPropertyChanged("Button");
            }
        }


        public bool MoveUp(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.Button.IndexOf(it);
                if (isrc > 0)
                {
                    var idst = isrc - 1;
                    var src = this.Button[isrc];
                    var dst = this.Button[idst];

                    this.Button[isrc] = dst;
                    this.Button[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }

            return false;
        }

        public bool MoveDown(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.Button.IndexOf(it);
                if (isrc < this.Button.Count - 1)
                {
                    var idst = isrc + 1;
                    var src = this.Button[isrc];
                    var dst = this.Button[idst];

                    this.Button[isrc] = dst;
                    this.Button[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                return this.Button.IndexOf(it);
            }
            return -1;
        }

        public void Insert(IUIItem item, int index)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                it.Parent = this;
                this.Button.Insert(index, it);
            }
        }
    }

    partial class InformationPaneType : IInformationPane
    {

    }

    partial class ContextMenuType : IContextMenu
    {
        [XmlIgnore]
        public int ItemCount
        {
            get { return this.MenuItem.Count; }
        }

        [XmlIgnore]
        public IEnumerable<IUIItem> Items
        {
            get
            {
                foreach (var item in this.MenuItem)
                {
                    yield return item;
                }
            }
        }

        public void AddItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.MenuItem.Add(it);
                it.Parent = this;
                OnPropertyChanged("MenuItem");
            }
        }

        public void RemoveItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.MenuItem.Remove(it);
                it.Parent = null;
                OnPropertyChanged("MenuItem");
            }
        }

        public bool MoveUp(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.MenuItem.IndexOf(it);
                if (isrc > 0)
                {
                    var idst = isrc - 1;
                    var src = this.MenuItem[isrc];
                    var dst = this.MenuItem[idst];

                    this.MenuItem[isrc] = dst;
                    this.MenuItem[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public bool MoveDown(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.MenuItem.IndexOf(it);
                if (isrc < this.MenuItem.Count - 1)
                {
                    var idst = isrc + 1;
                    var src = this.MenuItem[isrc];
                    var dst = this.MenuItem[idst];

                    this.MenuItem[isrc] = dst;
                    this.MenuItem[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                return this.MenuItem.IndexOf(it);
            }
            return -1;
        }

        public void Insert(IUIItem item, int index)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                it.Parent = this;
                this.MenuItem.Insert(index, it);
            }
        }
    }

    partial class TaskBarType : ITaskBar
    {
        [XmlIgnore]
        public int ItemCount
        {
            get { return this.MenuButton.Count; }
        }

        [XmlIgnore]
        public IEnumerable<IUIItem> Items
        {
            get
            {
                foreach (var item in this.MenuButton)
                {
                    yield return item;
                }
            }
        }

        public void AddItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.MenuButton.Add(it);
                it.Parent = this;
                OnPropertyChanged("MenuButton");
            }
        }

        public void RemoveItem(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                this.MenuButton.Remove(it);
                it.Parent = null;
                OnPropertyChanged("MenuButton");
            }
        }

        public bool MoveUp(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.MenuButton.IndexOf(it);
                if (isrc > 0)
                {
                    var idst = isrc - 1;
                    var src = this.MenuButton[isrc];
                    var dst = this.MenuButton[idst];

                    this.MenuButton[isrc] = dst;
                    this.MenuButton[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public bool MoveDown(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.MenuButton.IndexOf(it);
                if (isrc < this.MenuButton.Count - 1)
                {
                    var idst = isrc + 1;
                    var src = this.MenuButton[isrc];
                    var dst = this.MenuButton[idst];

                    this.MenuButton[isrc] = dst;
                    this.MenuButton[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                return this.MenuButton.IndexOf(it);
            }
            return -1;
        }

        public void Insert(IUIItem item, int index)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                it.Parent = this;
                this.MenuButton.Insert(index, it);
            }
        }

        [XmlIgnore]
        ITaskButton ITaskBar.Home
        {
            get { return this.Home; }
        }

        [XmlIgnore]
        ITaskButton ITaskBar.Forward
        {
            get { return this.Forward; }
        }

        [XmlIgnore]
        ITaskButton ITaskBar.Back
        {
            get { return this.Back; }
        }

        [XmlIgnore]
        ITaskButton ITaskBar.Tasks
        {
            get { return this.Tasks; }
        }
    }

    partial class TaskButtonType : ITaskButton
    {
        
    }

    partial class StatusBarType : IStatusBar
    {
    }

    partial class ZoomControlType : IZoomControl
    {
    }

    partial class CommandType : ICommand
    {
    }

    partial class ResourceReferenceType : IResourceReference
    {
    }

    partial class PrintCommandType : IPrintCommand
    {
        public void Clear()
        {
            this.PrintLayout.Clear();
        }

        [XmlIgnore]
        IEnumerable<IResourceReference> IPrintCommand.PrintLayout
        {
            get 
            {
                foreach (var refer in this.PrintLayout)
                {
                    yield return refer;
                }
            }
        }

        public IResourceReference CreatePrintLayout(string resourceId)
        {
            return new ResourceReferenceType() { ResourceId = resourceId };
        }

        public void AddPrintLayout(IResourceReference reference)
        {
            var r = reference as ResourceReferenceType;
            if (r != null)
            {
                this.PrintLayout.Add(r);
                OnPropertyChanged("PrintLayout");
            }
        }

        public void RemovePrintLayout(IResourceReference reference)
        {
            var r = reference as ResourceReferenceType;
            if (r != null)
            {
                this.PrintLayout.Remove(r);
                OnPropertyChanged("PrintLayout");
            }
        }
    }

    partial class SeparatorItemType : ISeparatorItem
    {
    }

    partial class BasicCommandType : IBasicCommand
    {
    }

    partial class TargetedCommandType : ITargetedCommand
    {
    }

    partial class FlyoutItemType : IFlyoutItem
    {
        [XmlIgnore]
        public int ItemCount
        {
            get { return this.SubItem.Count; }
        }

        [XmlIgnore]
        public IEnumerable<IUIItem> Items
        {
            get 
            {
                foreach (var item in this.SubItem)
                {
                    yield return item;
                }
            }
        }

        public void AddItem(IUIItem item)
        {
            var i = item as UIItemType;
            if (i != null)
            {
                this.SubItem.Add(i);
                i.Parent = this;
                OnPropertyChanged("SubItem");
            }
        }

        public void RemoveItem(IUIItem item)
        {
            var i = item as UIItemType;
            if (i != null)
            {
                this.SubItem.Remove(i);
                i.Parent = null;
                OnPropertyChanged("SubItem");
            };
        }

        public bool MoveUp(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.SubItem.IndexOf(it);
                if (isrc > 0)
                {
                    var idst = isrc - 1;
                    var src = this.SubItem[isrc];
                    var dst = this.SubItem[idst];

                    this.SubItem[isrc] = dst;
                    this.SubItem[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public bool MoveDown(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                var isrc = this.SubItem.IndexOf(it);
                if (isrc < this.SubItem.Count - 1)
                {
                    var idst = isrc + 1;
                    var src = this.SubItem[isrc];
                    var dst = this.SubItem[idst];

                    this.SubItem[isrc] = dst;
                    this.SubItem[idst] = src;

                    OnPropertyChanged("Button");
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(IUIItem item)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                return this.SubItem.IndexOf(it);
            }
            return -1;
        }

        public void Insert(IUIItem item, int index)
        {
            var it = item as UIItemType;
            if (it != null)
            {
                it.Parent = this;
                this.SubItem.Insert(index, it);
            }
        }
    }

    partial class UIItemType : IUIItem
    {
        [XmlIgnore]
        public IMenu Parent
        {
            get;
            set;
        }
    }

    partial class CommandItemType : ICommandItem
    {
        
    }

    partial class InvokeURLCommandType : IInvokeUrlCommand, ILayerSet
    {
        [XmlIgnore]
        ILayerSet IInvokeUrlCommand.LayerSet
        {
            get
            {
                return this;
            }
        }

        [XmlIgnore]
        IEnumerable<IParameterPair> IInvokeUrlCommand.AdditionalParameter
        {
            get 
            {
                foreach (var pair in this.AdditionalParameter)
                {
                    yield return pair;
                }
            }
        }

        public IParameterPair CreateParameter(string name, string value)
        {
            return new ParameterPairType() { Key = name, Value = value };
        }

        public void AddParameter(IParameterPair param)
        {
            var p = param as ParameterPairType;
            if (p != null)
            {
                this.AdditionalParameter.Add(p);
                OnPropertyChanged("AdditionalParameter");
            }
        }

        public void RemoveParameter(IParameterPair param)
        {
            var p = param as ParameterPairType;
            if (p != null)
            {
                this.AdditionalParameter.Remove(p);
                OnPropertyChanged("AdditionalParameter");
            }
        }

        [XmlIgnore]
        public BindingList<string> Layer
        {
            get 
            {
                return this.LayerSet;
            }
        }
    }

    partial class ParameterPairType : IParameterPair
    {
    }

    partial class SearchCommandType : ISearchCommand, IResultColumnSet
    {
        IResultColumn IResultColumnSet.CreateColumn(string name, string property)
        {
            return new ResultColumnType() { Name = name, Property = property };
        }

        [XmlIgnore]
        IResultColumnSet ISearchCommand.ResultColumns
        {
            get 
            {
                return this;
            }
        }

        [XmlIgnore]
        int ISearchCommand.MatchLimit
        {
            get
            {
                int i;
                if (!int.TryParse(this.MatchLimit, out i))
                {
                    i = 100;
                    this.MatchLimit = i.ToString();
                }
                return i;
            }
            set
            {
                this.MatchLimit = value.ToString();
            }
        }

        public void Clear()
        {
            this.ResultColumns.Clear();
        }

        [XmlIgnore]
        public IEnumerable<IResultColumn> Column
        {
            get
            {
                foreach (var col in this.ResultColumns)
                {
                    yield return col;
                }
            }
        }

        public void AddResultColumn(IResultColumn col)
        {
            var c = col as ResultColumnType;
            if (c != null)
            {
                this.ResultColumns.Add(c);
                OnPropertyChanged("ResultColumns");
            }
        }

        public void RemoveResultColumn(IResultColumn col)
        {
            var c = col as ResultColumnType;
            if (c != null)
            {
                this.ResultColumns.Remove(c);
                OnPropertyChanged("ResultColumns");
            }
        }
    }

    partial class ResultColumnType : IResultColumn
    {

    }

    partial class InvokeScriptCommandType : IInvokeScriptCommand
    {
        
    }
}
