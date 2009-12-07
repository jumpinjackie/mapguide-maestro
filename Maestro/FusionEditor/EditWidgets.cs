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
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.ListView WidgetList;
        private System.ComponentModel.IContainer components;

		private ApplicationDefinitionType m_appDef = null;
		private System.Windows.Forms.TabControl WidgetControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private FusionEditor.GenericWidgetExtensions GenericWidgetExtensions;
		private System.Windows.Forms.Panel CustomEditors;
		private FusionEditor.WidgetEditorUI widgetEditorUI;
		private FusionEditor.CustomizedEditors.BufferPanel BufferPanel;
		private FusionEditor.CustomizedEditors.Buffer Buffer;
		private FusionEditor.CustomizedEditors.ActivityIndicator ActivityIndicator;
		private FusionEditor.CustomizedEditors.AboutURL About;
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
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton AddWidgetButton;
        private ToolStripButton RemoveWidgetButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton MoveWidgetUpButton;
        private ToolStripButton MoveWidgetDownButton;
        private ToolStripMenuItem AddUIWidget;
        private ToolStripMenuItem AddWidget;
		
		private OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetTypeCollection m_defaultWidgets;

		public EditWidgets()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditWidgets));
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.WidgetList = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddWidgetButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddUIWidget = new System.Windows.Forms.ToolStripMenuItem();
            this.AddWidget = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveWidgetButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveWidgetUpButton = new System.Windows.Forms.ToolStripButton();
            this.MoveWidgetDownButton = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.WidgetControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.CustomEditors = new System.Windows.Forms.Panel();
            this.ZoomToSelection = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ZoomToSelection();
            this.ZoomOnClick = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ZoomOnClick();
            this.Zoom = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Zoom();
            this.ViewSize = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ViewSize();
            this.ViewOptions = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ViewOptions();
            this.TaskPane = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.TaskPane();
            this.SelectWithin = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.SelectWithin();
            this.SelectRadius = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.SelectRadius();
            this.SelectPolygon = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.SelectPolygon();
            this.Select = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Select();
            this.SaveMap = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.SaveMap();
            this.Print = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Print();
            this.OverviewMap = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.OverviewMap();
            this.Measure = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Measure();
            this.MapMenu = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.MapMenu();
            this.Legend = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Legend();
            this.InvokeScript = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.InvokeScript();
            this.Help = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Help();
            this.ExtentHistory = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ExtentHistory();
            this.EditableScale = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.EditableScale();
            this.CursorPosition = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.CursorPosition();
            this.BufferPanel = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.BufferPanel();
            this.Buffer = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.Buffer();
            this.ActivityIndicator = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.ActivityIndicator();
            this.About = new OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors.AboutURL();
            this.widgetEditorUI = new OSGeo.MapGuide.Maestro.FusionEditor.WidgetEditorUI();
            this.widgetEditor = new OSGeo.MapGuide.Maestro.FusionEditor.WidgetEditor();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GenericWidgetExtensions = new OSGeo.MapGuide.Maestro.FusionEditor.GenericWidgetExtensions();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
            this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImages.Images.SetKeyName(0, "");
            this.toolbarImages.Images.SetKeyName(1, "");
            this.toolbarImages.Images.SetKeyName(2, "");
            this.toolbarImages.Images.SetKeyName(3, "");
            this.toolbarImages.Images.SetKeyName(4, "");
            this.toolbarImages.Images.SetKeyName(5, "");
            this.toolbarImages.Images.SetKeyName(6, "");
            this.toolbarImages.Images.SetKeyName(7, "");
            this.toolbarImages.Images.SetKeyName(8, "");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.WidgetList);
            this.panel1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // WidgetList
            // 
            resources.ApplyResources(this.WidgetList, "WidgetList");
            this.WidgetList.MultiSelect = false;
            this.WidgetList.Name = "WidgetList";
            this.WidgetList.SmallImageList = this.toolbarImages;
            this.WidgetList.UseCompatibleStateImageBehavior = false;
            this.WidgetList.View = System.Windows.Forms.View.List;
            this.WidgetList.SelectedIndexChanged += new System.EventHandler(this.WidgetList_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddWidgetButton,
            this.RemoveWidgetButton,
            this.toolStripSeparator1,
            this.MoveWidgetUpButton,
            this.MoveWidgetDownButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddWidgetButton
            // 
            this.AddWidgetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddWidgetButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddUIWidget,
            this.AddWidget});
            resources.ApplyResources(this.AddWidgetButton, "AddWidgetButton");
            this.AddWidgetButton.Name = "AddWidgetButton";
            // 
            // AddUIWidget
            // 
            this.AddUIWidget.Name = "AddUIWidget";
            resources.ApplyResources(this.AddUIWidget, "AddUIWidget");
            this.AddUIWidget.Click += new System.EventHandler(this.withUserInterfaceToolStripMenuItem_Click);
            // 
            // AddWidget
            // 
            this.AddWidget.Name = "AddWidget";
            resources.ApplyResources(this.AddWidget, "AddWidget");
            this.AddWidget.Click += new System.EventHandler(this.withoutUserInterfaceToolStripMenuItem_Click);
            // 
            // RemoveWidgetButton
            // 
            this.RemoveWidgetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveWidgetButton, "RemoveWidgetButton");
            this.RemoveWidgetButton.Name = "RemoveWidgetButton";
            this.RemoveWidgetButton.Click += new System.EventHandler(this.RemoveWidgetButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // MoveWidgetUpButton
            // 
            this.MoveWidgetUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MoveWidgetUpButton, "MoveWidgetUpButton");
            this.MoveWidgetUpButton.Name = "MoveWidgetUpButton";
            this.MoveWidgetUpButton.Click += new System.EventHandler(this.MoveWidgetUpButton_Click);
            // 
            // MoveWidgetDownButton
            // 
            this.MoveWidgetDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MoveWidgetDownButton, "MoveWidgetDownButton");
            this.MoveWidgetDownButton.Name = "MoveWidgetDownButton";
            this.MoveWidgetDownButton.Click += new System.EventHandler(this.MoveWidgetDownButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.WidgetControl);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // WidgetControl
            // 
            this.WidgetControl.Controls.Add(this.tabPage1);
            this.WidgetControl.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.WidgetControl, "WidgetControl");
            this.WidgetControl.Name = "WidgetControl";
            this.WidgetControl.SelectedIndex = 0;
            this.WidgetControl.SelectedIndexChanged += new System.EventHandler(this.WidgetControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CustomEditors);
            this.tabPage1.Controls.Add(this.widgetEditorUI);
            this.tabPage1.Controls.Add(this.widgetEditor);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
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
            resources.ApplyResources(this.CustomEditors, "CustomEditors");
            this.CustomEditors.Name = "CustomEditors";
            // 
            // ZoomToSelection
            // 
            this.ZoomToSelection.DefaultWidgets = null;
            resources.ApplyResources(this.ZoomToSelection, "ZoomToSelection");
            this.ZoomToSelection.Name = "ZoomToSelection";
            // 
            // ZoomOnClick
            // 
            this.ZoomOnClick.DefaultWidgets = null;
            resources.ApplyResources(this.ZoomOnClick, "ZoomOnClick");
            this.ZoomOnClick.Name = "ZoomOnClick";
            // 
            // Zoom
            // 
            this.Zoom.DefaultWidgets = null;
            resources.ApplyResources(this.Zoom, "Zoom");
            this.Zoom.Name = "Zoom";
            // 
            // ViewSize
            // 
            this.ViewSize.DefaultWidgets = null;
            resources.ApplyResources(this.ViewSize, "ViewSize");
            this.ViewSize.Name = "ViewSize";
            // 
            // ViewOptions
            // 
            this.ViewOptions.DefaultWidgets = null;
            resources.ApplyResources(this.ViewOptions, "ViewOptions");
            this.ViewOptions.Name = "ViewOptions";
            // 
            // TaskPane
            // 
            this.TaskPane.DefaultWidgets = null;
            resources.ApplyResources(this.TaskPane, "TaskPane");
            this.TaskPane.Name = "TaskPane";
            // 
            // SelectWithin
            // 
            this.SelectWithin.DefaultWidgets = null;
            resources.ApplyResources(this.SelectWithin, "SelectWithin");
            this.SelectWithin.Name = "SelectWithin";
            // 
            // SelectRadius
            // 
            this.SelectRadius.DefaultWidgets = null;
            resources.ApplyResources(this.SelectRadius, "SelectRadius");
            this.SelectRadius.Name = "SelectRadius";
            // 
            // SelectPolygon
            // 
            this.SelectPolygon.DefaultWidgets = null;
            resources.ApplyResources(this.SelectPolygon, "SelectPolygon");
            this.SelectPolygon.Name = "SelectPolygon";
            // 
            // Select
            // 
            this.Select.DefaultWidgets = null;
            resources.ApplyResources(this.Select, "Select");
            this.Select.Name = "Select";
            // 
            // SaveMap
            // 
            this.SaveMap.DefaultWidgets = null;
            resources.ApplyResources(this.SaveMap, "SaveMap");
            this.SaveMap.Name = "SaveMap";
            // 
            // Print
            // 
            this.Print.DefaultWidgets = null;
            resources.ApplyResources(this.Print, "Print");
            this.Print.Name = "Print";
            // 
            // OverviewMap
            // 
            this.OverviewMap.DefaultWidgets = null;
            resources.ApplyResources(this.OverviewMap, "OverviewMap");
            this.OverviewMap.Name = "OverviewMap";
            // 
            // Measure
            // 
            this.Measure.DefaultWidgets = null;
            resources.ApplyResources(this.Measure, "Measure");
            this.Measure.Name = "Measure";
            // 
            // MapMenu
            // 
            this.MapMenu.DefaultWidgets = null;
            resources.ApplyResources(this.MapMenu, "MapMenu");
            this.MapMenu.Name = "MapMenu";
            // 
            // Legend
            // 
            this.Legend.DefaultWidgets = null;
            resources.ApplyResources(this.Legend, "Legend");
            this.Legend.Name = "Legend";
            // 
            // InvokeScript
            // 
            this.InvokeScript.DefaultWidgets = null;
            resources.ApplyResources(this.InvokeScript, "InvokeScript");
            this.InvokeScript.Name = "InvokeScript";
            // 
            // Help
            // 
            this.Help.DefaultWidgets = null;
            resources.ApplyResources(this.Help, "Help");
            this.Help.Name = "Help";
            // 
            // ExtentHistory
            // 
            this.ExtentHistory.DefaultWidgets = null;
            resources.ApplyResources(this.ExtentHistory, "ExtentHistory");
            this.ExtentHistory.Name = "ExtentHistory";
            // 
            // EditableScale
            // 
            this.EditableScale.DefaultWidgets = null;
            resources.ApplyResources(this.EditableScale, "EditableScale");
            this.EditableScale.Name = "EditableScale";
            // 
            // CursorPosition
            // 
            this.CursorPosition.DefaultWidgets = null;
            resources.ApplyResources(this.CursorPosition, "CursorPosition");
            this.CursorPosition.Name = "CursorPosition";
            // 
            // BufferPanel
            // 
            this.BufferPanel.DefaultWidgets = null;
            resources.ApplyResources(this.BufferPanel, "BufferPanel");
            this.BufferPanel.Name = "BufferPanel";
            // 
            // Buffer
            // 
            this.Buffer.DefaultWidgets = null;
            resources.ApplyResources(this.Buffer, "Buffer");
            this.Buffer.Name = "Buffer";
            // 
            // ActivityIndicator
            // 
            this.ActivityIndicator.DefaultWidgets = null;
            resources.ApplyResources(this.ActivityIndicator, "ActivityIndicator");
            this.ActivityIndicator.Name = "ActivityIndicator";
            // 
            // About
            // 
            this.About.DefaultWidgets = null;
            resources.ApplyResources(this.About, "About");
            this.About.Name = "About";
            // 
            // widgetEditorUI
            // 
            this.widgetEditorUI.DefaultWidgets = null;
            resources.ApplyResources(this.widgetEditorUI, "widgetEditorUI");
            this.widgetEditorUI.Name = "widgetEditorUI";
            // 
            // widgetEditor
            // 
            this.widgetEditor.DefaultWidgets = null;
            resources.ApplyResources(this.widgetEditor, "widgetEditor");
            this.widgetEditor.Name = "widgetEditor";
            this.widgetEditor.ValueChanged += new OSGeo.MapGuide.Maestro.FusionEditor.ValueChangedDelegate(this.widgetEditor_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GenericWidgetExtensions);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // GenericWidgetExtensions
            // 
            this.GenericWidgetExtensions.DataMember = "";
            resources.ApplyResources(this.GenericWidgetExtensions, "GenericWidgetExtensions");
            this.GenericWidgetExtensions.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.GenericWidgetExtensions.Name = "GenericWidgetExtensions";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel1);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CancelBtn);
            this.panel4.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // EditWidgets
            // 
            this.AcceptButton = this.OKBtn;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "EditWidgets";
            this.Load += new System.EventHandler(this.EditWidgets_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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


		private void AddUIWidget_Click(object sender, System.EventArgs e)
		{
			ListViewItem lvi = new ListViewItem(Strings.EditWidgets.NewWidgetName);
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
			ListViewItem lvi = new ListViewItem(Strings.EditWidgets.NewWidgetName);
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

        private void RemoveWidgetButton_Click(object sender, EventArgs e)
        {
            if (WidgetList.SelectedItems.Count != 0)
                WidgetList.Items.Remove(WidgetList.SelectedItems[0]);
        }

        private void MoveWidgetUpButton_Click(object sender, EventArgs e)
        {
            if (WidgetList.SelectedItems.Count == 0 || WidgetList.SelectedItems[0].Index == 0)
                return;

            ListViewItem lvi = WidgetList.SelectedItems[0];
            int index = lvi.Index;
            WidgetList.Items.Remove(lvi);
            WidgetList.Items.Insert(index - 1, lvi);
            lvi.Selected = true;
        }

        private void MoveWidgetDownButton_Click(object sender, EventArgs e)
        {
            if (WidgetList.SelectedItems.Count == 0 || WidgetList.SelectedItems[0].Index == WidgetList.Items.Count - 1)
                return;

            ListViewItem lvi = WidgetList.SelectedItems[0];
            int index = lvi.Index;
            WidgetList.Items.Remove(lvi);
            WidgetList.Items.Insert(index + 1, lvi);
            lvi.Selected = true;
        }

        private void withUserInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(GetNextName());
            UiWidgetType w = new UiWidgetType();

            w.Disabled = false.ToString();
            w.ImageClass = "";
            w.ImageUrl = "";
            w.Label = "";
            w.Location = "";
            w.Name = lvi.Text;
            w.StatusText = "";
            w.Tooltip = "";
            w.Type = "About";

            lvi.Tag = w;
            lvi.ImageIndex = 6;
            WidgetList.Items.Add(lvi);
            lvi.EnsureVisible();
            lvi.Selected = true;
        }

        private void withoutUserInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(GetNextName());
            WidgetType w = new WidgetType();

            w.Location = "";
            w.Name = lvi.Text;
            w.Type = "About";

            lvi.Tag = w;
            lvi.ImageIndex = 6;
            WidgetList.Items.Add(lvi);
            lvi.EnsureVisible();
            lvi.Selected = true;
        }

        private string GetNextName()
        {
            string basename = Strings.EditWidgets.NewWidgetName;
            string currentname = basename;
            int i = 1;
            bool exists = true;

            while (exists)
            {
                exists = false;
                foreach (ListViewItem lvi in WidgetList.Items)
                    if (lvi.Text == currentname)
                    {
                        exists = true;
                        break;
                    }

                if (exists)
                    currentname = basename + " " + (i++).ToString();
            }

            return currentname;

        }

        private void widgetEditor_ValueChanged(object sender, object item)
        {
            WidgetList_SelectedIndexChanged(sender, null);
            if (WidgetList.SelectedItems.Count == 1 && item as WidgetType != null)
                WidgetList.SelectedItems[0].Text = ((WidgetType)item).Name;
        }
	}
}
