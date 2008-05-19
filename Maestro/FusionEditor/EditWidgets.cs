#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for EditWidgets.
	/// </summary>
	public class EditWidgets : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ImageList toolbarImages;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private FusionEditor.WidgetEditor widgetEditor;
		private System.Windows.Forms.ToolBarButton AddWidgetButton;
		private System.Windows.Forms.ToolBarButton RemoveWidgetButton;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.ListView WidgetList;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton MoveWidgetUpButton;
		private System.Windows.Forms.ToolBarButton MoveWidgetDownButton;
		private System.Windows.Forms.ContextMenu AddWidgetMenu;
		private System.Windows.Forms.MenuItem AddUIWidget;
		private System.Windows.Forms.MenuItem AddWidget;
		private System.Windows.Forms.ToolBar WidgetToolBar;

		private ApplicationDefinitionType m_appDef = null;
		private System.Windows.Forms.TabControl WidgetControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private FusionEditor.GenericWidgetExtensions GenericWidgetExtensions;
		private System.Windows.Forms.Panel CustomEditors;
		private FusionEditor.WidgetEditorUI widgetEditorUI;
		private Globalizator.Globalizator m_globalizor = null;
		private FusionEditor.CustomizedEditors.BufferPanel BufferPanel;
		private FusionEditor.CustomizedEditors.Buffer Buffer;
		private FusionEditor.CustomizedEditors.ActivityIndicator ActivityIndicator;
		private FusionEditor.CustomizedEditors.About About;
		private FusionEditor.CustomizedEditors.CursorPosition CursorPosition;
		private FusionEditor.CustomizedEditors.EditableScale EditableScale;
		private FusionEditor.CustomizedEditors.ExtentHistory ExtentHistory;
		private FusionEditor.CustomizedEditors.Help Help;
		private FusionEditor.CustomizedEditors.InvokeScript InvokeScript;
		private FusionEditor.CustomizedEditors.Legend Legend;
		private FusionEditor.CustomizedEditors.MapMenu MapMenu;
		private FusionEditor.CustomizedEditors.Measure Measure;
		private FusionEditor.CustomizedEditors.OverviewMap OverviewMap;
		private FusionEditor.CustomizedEditors.Print Print;
		private FusionEditor.CustomizedEditors.SaveMap SaveMap;
		private new FusionEditor.CustomizedEditors.Select Select;
		private FusionEditor.CustomizedEditors.SelectPolygon SelectPolygon;
		private FusionEditor.CustomizedEditors.SelectRadius SelectRadius;
		private FusionEditor.CustomizedEditors.SelectWithin SelectWithin;
		private FusionEditor.CustomizedEditors.TaskPane TaskPane;
		private FusionEditor.CustomizedEditors.ViewOptions ViewOptions;
		private FusionEditor.CustomizedEditors.ViewSize ViewSize;
		private FusionEditor.CustomizedEditors.Zoom Zoom;
		private FusionEditor.CustomizedEditors.ZoomOnClick ZoomOnClick;
		private FusionEditor.CustomizedEditors.ZoomToSelection ZoomToSelection;
		
		private OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetTypeCollection m_defaultWidgets;

		public EditWidgets()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
			System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType));
			m_defaultWidgets = ((OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType)sr.Deserialize(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "Defaults.xml"))).WidgetSet[0].Widget;
			widgetEditor.DefaultWidgets = m_defaultWidgets;
			widgetEditorUI.DefaultWidgets = m_defaultWidgets;

			foreach(BasisWidgetEditor c in CustomEditors.Controls)
			{
				c.DefaultWidgets = m_defaultWidgets;
				c.ValueChanged += new ValueChangedDelegate(c_ValueChanged);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EditWidgets));
			this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.WidgetList = new System.Windows.Forms.ListView();
			this.WidgetToolBar = new System.Windows.Forms.ToolBar();
			this.AddWidgetButton = new System.Windows.Forms.ToolBarButton();
			this.RemoveWidgetButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.MoveWidgetUpButton = new System.Windows.Forms.ToolBarButton();
			this.MoveWidgetDownButton = new System.Windows.Forms.ToolBarButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.WidgetControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.CustomEditors = new System.Windows.Forms.Panel();
			this.ZoomToSelection = new FusionEditor.CustomizedEditors.ZoomToSelection();
			this.ZoomOnClick = new FusionEditor.CustomizedEditors.ZoomOnClick();
			this.Zoom = new FusionEditor.CustomizedEditors.Zoom();
			this.ViewSize = new FusionEditor.CustomizedEditors.ViewSize();
			this.ViewOptions = new FusionEditor.CustomizedEditors.ViewOptions();
			this.TaskPane = new FusionEditor.CustomizedEditors.TaskPane();
			this.SelectWithin = new FusionEditor.CustomizedEditors.SelectWithin();
			this.SelectRadius = new FusionEditor.CustomizedEditors.SelectRadius();
			this.SelectPolygon = new FusionEditor.CustomizedEditors.SelectPolygon();
			this.Select = new FusionEditor.CustomizedEditors.Select();
			this.SaveMap = new FusionEditor.CustomizedEditors.SaveMap();
			this.Print = new FusionEditor.CustomizedEditors.Print();
			this.OverviewMap = new FusionEditor.CustomizedEditors.OverviewMap();
			this.Measure = new FusionEditor.CustomizedEditors.Measure();
			this.MapMenu = new FusionEditor.CustomizedEditors.MapMenu();
			this.Legend = new FusionEditor.CustomizedEditors.Legend();
			this.InvokeScript = new FusionEditor.CustomizedEditors.InvokeScript();
			this.Help = new FusionEditor.CustomizedEditors.Help();
			this.ExtentHistory = new FusionEditor.CustomizedEditors.ExtentHistory();
			this.EditableScale = new FusionEditor.CustomizedEditors.EditableScale();
			this.CursorPosition = new FusionEditor.CustomizedEditors.CursorPosition();
			this.BufferPanel = new FusionEditor.CustomizedEditors.BufferPanel();
			this.Buffer = new FusionEditor.CustomizedEditors.Buffer();
			this.ActivityIndicator = new FusionEditor.CustomizedEditors.ActivityIndicator();
			this.About = new FusionEditor.CustomizedEditors.About();
			this.widgetEditorUI = new FusionEditor.WidgetEditorUI();
			this.widgetEditor = new FusionEditor.WidgetEditor();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.GenericWidgetExtensions = new FusionEditor.GenericWidgetExtensions();
			this.panel3 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel4 = new System.Windows.Forms.Panel();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.OKBtn = new System.Windows.Forms.Button();
			this.AddWidgetMenu = new System.Windows.Forms.ContextMenu();
			this.AddUIWidget = new System.Windows.Forms.MenuItem();
			this.AddWidget = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.WidgetControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.CustomEditors.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GenericWidgetExtensions)).BeginInit();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbarImages
			// 
			this.toolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
			this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.WidgetList);
			this.panel1.Controls.Add(this.WidgetToolBar);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(184, 445);
			this.panel1.TabIndex = 0;
			// 
			// WidgetList
			// 
			this.WidgetList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WidgetList.Location = new System.Drawing.Point(0, 28);
			this.WidgetList.MultiSelect = false;
			this.WidgetList.Name = "WidgetList";
			this.WidgetList.Size = new System.Drawing.Size(184, 417);
			this.WidgetList.SmallImageList = this.toolbarImages;
			this.WidgetList.TabIndex = 1;
			this.WidgetList.View = System.Windows.Forms.View.List;
			this.WidgetList.SelectedIndexChanged += new System.EventHandler(this.WidgetList_SelectedIndexChanged);
			// 
			// WidgetToolBar
			// 
			this.WidgetToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.AddWidgetButton,
																							 this.RemoveWidgetButton,
																							 this.toolBarButton1,
																							 this.MoveWidgetUpButton,
																							 this.MoveWidgetDownButton});
			this.WidgetToolBar.DropDownArrows = true;
			this.WidgetToolBar.ImageList = this.toolbarImages;
			this.WidgetToolBar.Location = new System.Drawing.Point(0, 0);
			this.WidgetToolBar.Name = "WidgetToolBar";
			this.WidgetToolBar.ShowToolTips = true;
			this.WidgetToolBar.Size = new System.Drawing.Size(184, 28);
			this.WidgetToolBar.TabIndex = 0;
			this.WidgetToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.WidgetToolBar_ButtonClick);
			// 
			// AddWidgetButton
			// 
			this.AddWidgetButton.ImageIndex = 7;
			this.AddWidgetButton.ToolTipText = "Add a new widget to the list";
			// 
			// RemoveWidgetButton
			// 
			this.RemoveWidgetButton.Enabled = false;
			this.RemoveWidgetButton.ImageIndex = 8;
			this.RemoveWidgetButton.ToolTipText = "Remove the selected widget from the list";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// MoveWidgetUpButton
			// 
			this.MoveWidgetUpButton.Enabled = false;
			this.MoveWidgetUpButton.ImageIndex = 2;
			this.MoveWidgetUpButton.ToolTipText = "Move the selected widget up";
			// 
			// MoveWidgetDownButton
			// 
			this.MoveWidgetDownButton.Enabled = false;
			this.MoveWidgetDownButton.ImageIndex = 3;
			this.MoveWidgetDownButton.ToolTipText = "Move the selected widget down";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.WidgetControl);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(187, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(453, 445);
			this.panel2.TabIndex = 2;
			// 
			// WidgetControl
			// 
			this.WidgetControl.Controls.Add(this.tabPage1);
			this.WidgetControl.Controls.Add(this.tabPage2);
			this.WidgetControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WidgetControl.Location = new System.Drawing.Point(0, 0);
			this.WidgetControl.Name = "WidgetControl";
			this.WidgetControl.SelectedIndex = 0;
			this.WidgetControl.Size = new System.Drawing.Size(453, 445);
			this.WidgetControl.TabIndex = 3;
			this.WidgetControl.Visible = false;
			this.WidgetControl.SelectedIndexChanged += new System.EventHandler(this.WidgetControl_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.CustomEditors);
			this.tabPage1.Controls.Add(this.widgetEditorUI);
			this.tabPage1.Controls.Add(this.widgetEditor);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(445, 419);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Basic properties";
			// 
			// CustomEditors
			// 
			this.CustomEditors.Controls.Add(this.ZoomToSelection);
			this.CustomEditors.Controls.Add(this.ZoomOnClick);
			this.CustomEditors.Controls.Add(this.Zoom);
			this.CustomEditors.Controls.Add(this.ViewSize);
			this.CustomEditors.Controls.Add(this.ViewOptions);
			this.CustomEditors.Controls.Add(this.TaskPane);
			this.CustomEditors.Controls.Add(this.SelectWithin);
			this.CustomEditors.Controls.Add(this.SelectRadius);
			this.CustomEditors.Controls.Add(this.SelectPolygon);
			this.CustomEditors.Controls.Add(this.Select);
			this.CustomEditors.Controls.Add(this.SaveMap);
			this.CustomEditors.Controls.Add(this.Print);
			this.CustomEditors.Controls.Add(this.OverviewMap);
			this.CustomEditors.Controls.Add(this.Measure);
			this.CustomEditors.Controls.Add(this.MapMenu);
			this.CustomEditors.Controls.Add(this.Legend);
			this.CustomEditors.Controls.Add(this.InvokeScript);
			this.CustomEditors.Controls.Add(this.Help);
			this.CustomEditors.Controls.Add(this.ExtentHistory);
			this.CustomEditors.Controls.Add(this.EditableScale);
			this.CustomEditors.Controls.Add(this.CursorPosition);
			this.CustomEditors.Controls.Add(this.BufferPanel);
			this.CustomEditors.Controls.Add(this.Buffer);
			this.CustomEditors.Controls.Add(this.ActivityIndicator);
			this.CustomEditors.Controls.Add(this.About);
			this.CustomEditors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CustomEditors.Location = new System.Drawing.Point(0, 232);
			this.CustomEditors.Name = "CustomEditors";
			this.CustomEditors.Size = new System.Drawing.Size(445, 187);
			this.CustomEditors.TabIndex = 3;
			// 
			// ZoomToSelection
			// 
			this.ZoomToSelection.DefaultWidgets = null;
			this.ZoomToSelection.Location = new System.Drawing.Point(224, 88);
			this.ZoomToSelection.Name = "ZoomToSelection";
			this.ZoomToSelection.Size = new System.Drawing.Size(64, 24);
			this.ZoomToSelection.TabIndex = 24;
			// 
			// ZoomOnClick
			// 
			this.ZoomOnClick.DefaultWidgets = null;
			this.ZoomOnClick.Location = new System.Drawing.Point(224, 56);
			this.ZoomOnClick.Name = "ZoomOnClick";
			this.ZoomOnClick.Size = new System.Drawing.Size(64, 24);
			this.ZoomOnClick.TabIndex = 23;
			// 
			// Zoom
			// 
			this.Zoom.DefaultWidgets = null;
			this.Zoom.Location = new System.Drawing.Point(224, 32);
			this.Zoom.Name = "Zoom";
			this.Zoom.Size = new System.Drawing.Size(56, 24);
			this.Zoom.TabIndex = 22;
			// 
			// ViewSize
			// 
			this.ViewSize.DefaultWidgets = null;
			this.ViewSize.Location = new System.Drawing.Point(216, 8);
			this.ViewSize.Name = "ViewSize";
			this.ViewSize.Size = new System.Drawing.Size(64, 24);
			this.ViewSize.TabIndex = 21;
			// 
			// ViewOptions
			// 
			this.ViewOptions.DefaultWidgets = null;
			this.ViewOptions.Location = new System.Drawing.Point(176, 152);
			this.ViewOptions.Name = "ViewOptions";
			this.ViewOptions.Size = new System.Drawing.Size(40, 24);
			this.ViewOptions.TabIndex = 20;
			// 
			// TaskPane
			// 
			this.TaskPane.DefaultWidgets = null;
			this.TaskPane.Location = new System.Drawing.Point(168, 128);
			this.TaskPane.Name = "TaskPane";
			this.TaskPane.Size = new System.Drawing.Size(48, 24);
			this.TaskPane.TabIndex = 19;
			// 
			// SelectWithin
			// 
			this.SelectWithin.DefaultWidgets = null;
			this.SelectWithin.Location = new System.Drawing.Point(168, 104);
			this.SelectWithin.Name = "SelectWithin";
			this.SelectWithin.Size = new System.Drawing.Size(48, 24);
			this.SelectWithin.TabIndex = 18;
			// 
			// SelectRadius
			// 
			this.SelectRadius.DefaultWidgets = null;
			this.SelectRadius.Location = new System.Drawing.Point(168, 80);
			this.SelectRadius.Name = "SelectRadius";
			this.SelectRadius.Size = new System.Drawing.Size(40, 24);
			this.SelectRadius.TabIndex = 17;
			// 
			// SelectPolygon
			// 
			this.SelectPolygon.DefaultWidgets = null;
			this.SelectPolygon.Location = new System.Drawing.Point(168, 56);
			this.SelectPolygon.Name = "SelectPolygon";
			this.SelectPolygon.Size = new System.Drawing.Size(40, 24);
			this.SelectPolygon.TabIndex = 16;
			// 
			// Select
			// 
			this.Select.DefaultWidgets = null;
			this.Select.Location = new System.Drawing.Point(168, 32);
			this.Select.Name = "Select";
			this.Select.Size = new System.Drawing.Size(48, 24);
			this.Select.TabIndex = 15;
			// 
			// SaveMap
			// 
			this.SaveMap.DefaultWidgets = null;
			this.SaveMap.Location = new System.Drawing.Point(160, 8);
			this.SaveMap.Name = "SaveMap";
			this.SaveMap.Size = new System.Drawing.Size(56, 24);
			this.SaveMap.TabIndex = 14;
			// 
			// Print
			// 
			this.Print.DefaultWidgets = null;
			this.Print.Location = new System.Drawing.Point(104, 152);
			this.Print.Name = "Print";
			this.Print.Size = new System.Drawing.Size(48, 24);
			this.Print.TabIndex = 13;
			// 
			// OverviewMap
			// 
			this.OverviewMap.DefaultWidgets = null;
			this.OverviewMap.Location = new System.Drawing.Point(104, 128);
			this.OverviewMap.Name = "OverviewMap";
			this.OverviewMap.Size = new System.Drawing.Size(48, 24);
			this.OverviewMap.TabIndex = 12;
			// 
			// Measure
			// 
			this.Measure.DefaultWidgets = null;
			this.Measure.Location = new System.Drawing.Point(96, 104);
			this.Measure.Name = "Measure";
			this.Measure.Size = new System.Drawing.Size(56, 24);
			this.Measure.TabIndex = 11;
			// 
			// MapMenu
			// 
			this.MapMenu.DefaultWidgets = null;
			this.MapMenu.Location = new System.Drawing.Point(96, 80);
			this.MapMenu.Name = "MapMenu";
			this.MapMenu.Size = new System.Drawing.Size(64, 24);
			this.MapMenu.TabIndex = 10;
			// 
			// Legend
			// 
			this.Legend.DefaultWidgets = null;
			this.Legend.Location = new System.Drawing.Point(88, 56);
			this.Legend.Name = "Legend";
			this.Legend.Size = new System.Drawing.Size(64, 24);
			this.Legend.TabIndex = 9;
			// 
			// InvokeScript
			// 
			this.InvokeScript.DefaultWidgets = null;
			this.InvokeScript.Location = new System.Drawing.Point(96, 32);
			this.InvokeScript.Name = "InvokeScript";
			this.InvokeScript.Size = new System.Drawing.Size(64, 24);
			this.InvokeScript.TabIndex = 8;
			// 
			// Help
			// 
			this.Help.DefaultWidgets = null;
			this.Help.Location = new System.Drawing.Point(96, 8);
			this.Help.Name = "Help";
			this.Help.Size = new System.Drawing.Size(64, 24);
			this.Help.TabIndex = 7;
			// 
			// ExtentHistory
			// 
			this.ExtentHistory.DefaultWidgets = null;
			this.ExtentHistory.Location = new System.Drawing.Point(8, 152);
			this.ExtentHistory.Name = "ExtentHistory";
			this.ExtentHistory.Size = new System.Drawing.Size(96, 24);
			this.ExtentHistory.TabIndex = 6;
			// 
			// EditableScale
			// 
			this.EditableScale.DefaultWidgets = null;
			this.EditableScale.Location = new System.Drawing.Point(8, 128);
			this.EditableScale.Name = "EditableScale";
			this.EditableScale.Size = new System.Drawing.Size(80, 24);
			this.EditableScale.TabIndex = 5;
			// 
			// CursorPosition
			// 
			this.CursorPosition.DefaultWidgets = null;
			this.CursorPosition.Location = new System.Drawing.Point(8, 104);
			this.CursorPosition.Name = "CursorPosition";
			this.CursorPosition.Size = new System.Drawing.Size(80, 24);
			this.CursorPosition.TabIndex = 4;
			// 
			// BufferPanel
			// 
			this.BufferPanel.DefaultWidgets = null;
			this.BufferPanel.Location = new System.Drawing.Point(8, 80);
			this.BufferPanel.Name = "BufferPanel";
			this.BufferPanel.Size = new System.Drawing.Size(80, 24);
			this.BufferPanel.TabIndex = 3;
			// 
			// Buffer
			// 
			this.Buffer.DefaultWidgets = null;
			this.Buffer.Location = new System.Drawing.Point(8, 56);
			this.Buffer.Name = "Buffer";
			this.Buffer.Size = new System.Drawing.Size(72, 24);
			this.Buffer.TabIndex = 2;
			// 
			// ActivityIndicator
			// 
			this.ActivityIndicator.DefaultWidgets = null;
			this.ActivityIndicator.Location = new System.Drawing.Point(8, 32);
			this.ActivityIndicator.Name = "ActivityIndicator";
			this.ActivityIndicator.Size = new System.Drawing.Size(88, 24);
			this.ActivityIndicator.TabIndex = 1;
			// 
			// About
			// 
			this.About.DefaultWidgets = null;
			this.About.Location = new System.Drawing.Point(8, 8);
			this.About.Name = "About";
			this.About.Size = new System.Drawing.Size(88, 24);
			this.About.TabIndex = 0;
			// 
			// widgetEditorUI
			// 
			this.widgetEditorUI.DefaultWidgets = null;
			this.widgetEditorUI.Dock = System.Windows.Forms.DockStyle.Top;
			this.widgetEditorUI.Location = new System.Drawing.Point(0, 80);
			this.widgetEditorUI.Name = "widgetEditorUI";
			this.widgetEditorUI.Size = new System.Drawing.Size(445, 152);
			this.widgetEditorUI.TabIndex = 4;
			// 
			// widgetEditor
			// 
			this.widgetEditor.DefaultWidgets = null;
			this.widgetEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.widgetEditor.Location = new System.Drawing.Point(0, 0);
			this.widgetEditor.Name = "widgetEditor";
			this.widgetEditor.Size = new System.Drawing.Size(445, 80);
			this.widgetEditor.TabIndex = 2;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.GenericWidgetExtensions);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(445, 419);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Extended properties";
			// 
			// GenericWidgetExtensions
			// 
			this.GenericWidgetExtensions.DataMember = "";
			this.GenericWidgetExtensions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenericWidgetExtensions.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.GenericWidgetExtensions.Location = new System.Drawing.Point(0, 0);
			this.GenericWidgetExtensions.Name = "GenericWidgetExtensions";
			this.GenericWidgetExtensions.Size = new System.Drawing.Size(445, 419);
			this.GenericWidgetExtensions.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel2);
			this.panel3.Controls.Add(this.splitter1);
			this.panel3.Controls.Add(this.panel1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(640, 445);
			this.panel3.TabIndex = 3;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(184, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 445);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.CancelBtn);
			this.panel4.Controls.Add(this.OKBtn);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 445);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(640, 48);
			this.panel4.TabIndex = 4;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelBtn.Location = new System.Drawing.Point(328, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(96, 32);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "Cancel";
			// 
			// OKBtn
			// 
			this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKBtn.Location = new System.Drawing.Point(216, 8);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(96, 32);
			this.OKBtn.TabIndex = 0;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// AddWidgetMenu
			// 
			this.AddWidgetMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.AddUIWidget,
																						  this.AddWidget});
			// 
			// AddUIWidget
			// 
			this.AddUIWidget.Index = 0;
			this.AddUIWidget.Text = "With user interface";
			this.AddUIWidget.Click += new System.EventHandler(this.AddUIWidget_Click);
			// 
			// AddWidget
			// 
			this.AddWidget.Index = 1;
			this.AddWidget.Text = "Without user interface";
			this.AddWidget.Click += new System.EventHandler(this.AddWidget_Click);
			// 
			// EditWidgets
			// 
			this.AcceptButton = this.OKBtn;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(640, 493);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimizeBox = false;
			this.Name = "EditWidgets";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EditWidgets";
			this.Load += new System.EventHandler(this.EditWidgets_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.WidgetControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.CustomEditors.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.GenericWidgetExtensions)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			m_appDef.WidgetSet[0].Widget.Clear();
			foreach(ListViewItem lvi in WidgetList.Items)
				m_appDef.WidgetSet[0].Widget.Add((WidgetType)lvi.Tag);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void EditWidgets_Load(object sender, System.EventArgs e)
		{
			if (WidgetList.Items.Count > 0 && WidgetList.SelectedItems.Count == 0)
				WidgetList.Items[0].Selected = true;
		}

		public void SetupDialog(OSGeo.MapGuide.MaestroAPI.ApplicationDefinitionWidgetInfoSet wi, ApplicationDefinitionType appDef)
		{
			m_appDef = appDef;
			widgetEditor.SetupCombos(wi);
			WidgetList.Items.Clear();

			foreach(WidgetSetType wst in m_appDef.WidgetSet)
				foreach(WidgetType w in wst.Widget)
				{
					ListViewItem lvi = new ListViewItem(w.Name);
					lvi.Tag = w;
					lvi.ImageIndex = 6;
					WidgetList.Items.Add(lvi);
				}

		}

		private void WidgetList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (WidgetList.SelectedItems.Count == 0 || WidgetList.SelectedItems[0].Tag == null)
			{
				WidgetControl.Visible = false;
				MoveWidgetUpButton.Enabled = MoveWidgetDownButton.Enabled = RemoveWidgetButton.Enabled = false;
			}
			else
			{
				RemoveWidgetButton.Enabled = true;
				MoveWidgetUpButton.Enabled = WidgetList.SelectedItems[0].Index != 0;
				MoveWidgetDownButton.Enabled = WidgetList.SelectedItems[0].Index != WidgetList.Items.Count - 1;

				WidgetControl.Visible = true;
				widgetEditor.SetItem((WidgetType)WidgetList.SelectedItems[0].Tag);
				GenericWidgetExtensions.SetItem((WidgetType)WidgetList.SelectedItems[0].Tag);
				widgetEditorUI.SetItem((WidgetType)WidgetList.SelectedItems[0].Tag);
				WidgetType wt = (WidgetType)WidgetList.SelectedItems[0].Tag;
				CustomEditors.Visible = wt != null;
				if (wt == null)
					return;

				foreach(BasisWidgetEditor c in CustomEditors.Controls)
				{
					if (c.Name == wt.Type)
					{
						c.Dock = DockStyle.Fill;
						c.Visible = true;
						c.SetItem(wt);
					}
					else
					{
						c.Visible = false;
					}
				}

			}
		}

		private void WidgetToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == AddWidgetButton)
			{
				AddWidgetMenu.Show(e.Button.Parent, new Point(e.Button.Rectangle.Left, e.Button.Rectangle.Bottom));
			}
			else if (e.Button == RemoveWidgetButton)
			{
				if (WidgetList.SelectedItems.Count != 0)
					WidgetList.Items.Remove(WidgetList.SelectedItems[0]);
			}
			else if (e.Button == MoveWidgetUpButton)
			{
				if (WidgetList.SelectedItems.Count == 0 || WidgetList.SelectedItems[0].Index == 0)
					return;
				
				ListViewItem lvi = WidgetList.SelectedItems[0];
				int index = lvi.Index;
				WidgetList.Items.Remove(lvi);
				WidgetList.Items.Insert(index - 1, lvi);
				lvi.Selected = true;
			}
			else if (e.Button == MoveWidgetDownButton)
			{
				if (WidgetList.SelectedItems.Count == 0 || WidgetList.SelectedItems[0].Index == WidgetList.Items.Count - 1)
					return;
				
				ListViewItem lvi = WidgetList.SelectedItems[0];
				int index = lvi.Index;
				WidgetList.Items.Remove(lvi);
				WidgetList.Items.Insert(index + 1, lvi);
				lvi.Selected = true;
			}
		}

		private void AddUIWidget_Click(object sender, System.EventArgs e)
		{
			ListViewItem lvi = new ListViewItem(m_globalizor.Translate("New widget"));
			UiWidgetType w = new UiWidgetType();

			w.Disabled = false.ToString();
			w.ImageClass = "";
			w.ImageUrl = "";
			w.Label = "";
			w.Location = "";
			w.Name = "";
			w.StatusText = "";
			w.Tooltip = "";
			w.Type = "About";

			lvi.Tag = w;
			lvi.ImageIndex = 6;
			WidgetList.Items.Add(lvi);
			lvi.EnsureVisible();
			lvi.Selected = true;
		}

		private void AddWidget_Click(object sender, System.EventArgs e)
		{
			ListViewItem lvi = new ListViewItem(m_globalizor.Translate("New widget"));
			WidgetType w = new WidgetType();

			w.Location = "";
			w.Name = "";
			w.Type = "About";

			lvi.Tag = w;
			lvi.ImageIndex = 6;
			WidgetList.Items.Add(lvi);
			lvi.EnsureVisible();
			lvi.Selected = true;
		}

		private void c_ValueChanged(object sender, object item)
		{

		}

		private void WidgetControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			WidgetList_SelectedIndexChanged(sender, e);
		}
	}
}
