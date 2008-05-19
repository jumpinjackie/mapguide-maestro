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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.Maestro.ResourceEditors;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for ApplicationDefinitionEditor.
	/// </summary>
	public class ApplicationDefinitionEditor : System.Windows.Forms.UserControl, OSGeo.MapGuide.Maestro.ResourceEditor
	{
		private System.ComponentModel.IContainer components;

		private ApplicationDefinitionType m_appDef;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox MapTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox MapGroup;
		private System.Windows.Forms.ListView MapList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox MapResourceID;
		private System.Windows.Forms.Button BrowseMapButton;
		private System.Windows.Forms.CheckBox OverrideDisplayExtents;
		private System.Windows.Forms.GroupBox overriddenMapExtents;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox overrideScale;
		private System.Windows.Forms.TextBox overrideY;
		private System.Windows.Forms.TextBox overrideX;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TextBox TemplateURL;
		private EditorInterface m_editor;
		private System.Windows.Forms.TreeView WidgetTree;
		private FusionEditor.ContainerEditor containerEditor;
		private Hashtable m_widgetEditorLookup = null;
		private FusionEditor.WidgetEntry widgetEntry;
		private System.Windows.Forms.Button SelectTemplateBtn;
		private System.Windows.Forms.ToolBar WidgetToolBar;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.Windows.Forms.ToolBarButton RemoveWidgetButton;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton MoveWidgetUpButton;
		private System.Windows.Forms.ToolBarButton MoveWidgetDownButton;
		private System.Windows.Forms.ContextMenu AddWidgetMenu;
		private System.Windows.Forms.MenuItem AddWidgetSeperatorMenu;
		private System.Windows.Forms.MenuItem AddWidgetSubmenuMenu;
		private System.Windows.Forms.MenuItem AddWidgetEntryMenu;
		private System.Windows.Forms.ToolBarButton AddWidgetButton;
		private System.Windows.Forms.Panel MapPropertiesPanel;
		private System.Windows.Forms.ComboBox MapTypeCombo;
		private System.Windows.Forms.CheckBox MapSingleTileCheck;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox MapID;
		private System.Windows.Forms.ToolBar MapToolbar;
		private System.Windows.Forms.ToolBarButton AddMapButton;
		private System.Windows.Forms.ToolBarButton RemoveMapButton;
		private System.Windows.Forms.ToolBarButton AddContainerButton;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton ConfigureWidgetsButton;
		private FusionEditor.FlyoutEditor flyoutEntry;
		private Globalizator.Globalizator m_globalizor = null;
		private System.Windows.Forms.Button ShowInBrowser;
		private System.Windows.Forms.TextBox browserURL;
		private System.Windows.Forms.Label label12;
		private string m_tempResource;


		private bool m_isUpdating = false;

		private ApplicationDefinitionEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			m_widgetEditorLookup = new Hashtable();
			m_widgetEditorLookup.Add(typeof(ContainerType), containerEditor);
			m_widgetEditorLookup.Add(typeof(UiItemContainerType), containerEditor);
			m_widgetEditorLookup.Add(typeof(WidgetItemType), widgetEntry);
			m_widgetEditorLookup.Add(typeof(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType), flyoutEntry); 

			containerEditor.ValueChanged += new ValueChangedDelegate(containerEditor_ValueChanged);
			widgetEntry.ValueChanged += new ValueChangedDelegate(widgetEntry_ValueChanged);
			flyoutEntry.ValueChanged += new ValueChangedDelegate(flyoutEntry_ValueChanged);

			m_globalizor = new  Globalizator.Globalizator(this);
		}

		public ApplicationDefinitionEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_appDef = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType();
			m_tempResource = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, true);
			UpdateDisplay();
		}

		public ApplicationDefinitionEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_appDef = m_editor.CurrentConnection.GetApplicationDefinition(resourceID);
			m_tempResource = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, true);
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				ApplicationDefinitionContainerInfoSet co = m_editor.CurrentConnection.GetApplicationContainers();

				containerEditor.SetupCombos(co);

				MapTitle.Text = m_appDef.Title;
				TemplateURL.Text = m_appDef.TemplateUrl;

				MapList.Items.Clear();
				if (m_appDef.MapSet == null)
					m_appDef.MapSet = new MapGroupTypeCollection();

				foreach(MapGroupType mgr in m_appDef.MapSet)
				{
					ListViewItem lvi = new ListViewItem(mgr.id);
					lvi.Tag = mgr;
					lvi.ImageIndex = 7;
					MapList.Items.Add(lvi);
				}

				if (MapList.Items.Count > 0 && MapList.SelectedItems.Count == 0)
					MapList.Items[0].Selected = true;

				WidgetTree.Nodes.Clear();
				if (m_appDef.WidgetSet == null)
					m_appDef.WidgetSet = new WidgetSetTypeCollection();

				ArrayList wid = new ArrayList();
				foreach(WidgetSetType ws in m_appDef.WidgetSet)
					wid.AddRange(ws.Widget);
				widgetEntry.SetupCombos((WidgetType[])wid.ToArray(typeof(WidgetType)));

				foreach(WidgetSetType wst in m_appDef.WidgetSet)
					foreach(ContainerType c in wst.Container)
					{
						TreeNode n = new TreeNode(c.Name);
						n.Tag = c;
						n.ImageIndex = n.SelectedImageIndex = 8;
						WidgetTree.Nodes.Add(n);

						if (c as UiItemContainerType != null)
							FillNode(((UiItemContainerType)c).Item, n.Nodes);
					}

				if (WidgetTree.Nodes.Count > 0 && WidgetTree.SelectedNode == null)
				{
					WidgetTree.SelectedNode = WidgetTree.Nodes[0];
					WidgetTree.Nodes[0].Expand();
				}

			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void FillNode(UiItemTypeCollection items, TreeNodeCollection parent)
		{
			foreach(UiItemType u in items) 
			{
				if (u as WidgetItemType != null)
				{
					WidgetItemType uiw = u as WidgetItemType;
					TreeNode tn = new TreeNode(uiw.Widget);
					tn.Tag = u;
					tn.ImageIndex = tn.SelectedImageIndex = 6;
					parent.Add(tn);
				}
				else if (u as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.SeparatorItemType  != null)
				{
					TreeNode tn = new TreeNode(m_globalizor.Translate("- Seperator -"));
					tn.Tag = u;
					tn.ImageIndex = tn.SelectedImageIndex = 11;
					parent.Add(tn);
				}
				else if (u as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType  != null)
				{
					OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType flw = u as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType;
					TreeNode tn = new TreeNode(flw.Label);
					tn.Tag = u;
					tn.ImageIndex = tn.SelectedImageIndex = 4;
					parent.Add(tn);
					FillNode(flw.Item, tn.Nodes);
				}
				else
				{
					TreeNode tn = new TreeNode(u.Function.ToString());
					tn.Tag = u;
					tn.ImageIndex = tn.SelectedImageIndex = 6;
					parent.Add(tn);
				}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ApplicationDefinitionEditor));
			this.label1 = new System.Windows.Forms.Label();
			this.MapTitle = new System.Windows.Forms.TextBox();
			this.TemplateURL = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.MapGroup = new System.Windows.Forms.GroupBox();
			this.MapPropertiesPanel = new System.Windows.Forms.Panel();
			this.MapID = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.MapSingleTileCheck = new System.Windows.Forms.CheckBox();
			this.MapTypeCombo = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.OverrideDisplayExtents = new System.Windows.Forms.CheckBox();
			this.overriddenMapExtents = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.overrideScale = new System.Windows.Forms.TextBox();
			this.overrideY = new System.Windows.Forms.TextBox();
			this.overrideX = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.BrowseMapButton = new System.Windows.Forms.Button();
			this.MapResourceID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.MapList = new System.Windows.Forms.ListView();
			this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.MapToolbar = new System.Windows.Forms.ToolBar();
			this.AddMapButton = new System.Windows.Forms.ToolBarButton();
			this.RemoveMapButton = new System.Windows.Forms.ToolBarButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.flyoutEntry = new FusionEditor.FlyoutEditor();
			this.widgetEntry = new FusionEditor.WidgetEntry();
			this.containerEditor = new FusionEditor.ContainerEditor();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel4 = new System.Windows.Forms.Panel();
			this.WidgetTree = new System.Windows.Forms.TreeView();
			this.WidgetToolBar = new System.Windows.Forms.ToolBar();
			this.AddWidgetButton = new System.Windows.Forms.ToolBarButton();
			this.AddContainerButton = new System.Windows.Forms.ToolBarButton();
			this.RemoveWidgetButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.MoveWidgetUpButton = new System.Windows.Forms.ToolBarButton();
			this.MoveWidgetDownButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.ConfigureWidgetsButton = new System.Windows.Forms.ToolBarButton();
			this.SelectTemplateBtn = new System.Windows.Forms.Button();
			this.AddWidgetMenu = new System.Windows.Forms.ContextMenu();
			this.AddWidgetSeperatorMenu = new System.Windows.Forms.MenuItem();
			this.AddWidgetSubmenuMenu = new System.Windows.Forms.MenuItem();
			this.AddWidgetEntryMenu = new System.Windows.Forms.MenuItem();
			this.ShowInBrowser = new System.Windows.Forms.Button();
			this.browserURL = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.MapGroup.SuspendLayout();
			this.MapPropertiesPanel.SuspendLayout();
			this.overriddenMapExtents.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Title";
			// 
			// MapTitle
			// 
			this.MapTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MapTitle.Location = new System.Drawing.Point(128, 0);
			this.MapTitle.Name = "MapTitle";
			this.MapTitle.Size = new System.Drawing.Size(576, 20);
			this.MapTitle.TabIndex = 1;
			this.MapTitle.Text = "textBox1";
			this.MapTitle.TextChanged += new System.EventHandler(this.MapTitle_TextChanged);
			// 
			// TemplateURL
			// 
			this.TemplateURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TemplateURL.Location = new System.Drawing.Point(128, 24);
			this.TemplateURL.Name = "TemplateURL";
			this.TemplateURL.Size = new System.Drawing.Size(544, 20);
			this.TemplateURL.TabIndex = 3;
			this.TemplateURL.Text = "textBox1";
			this.TemplateURL.TextChanged += new System.EventHandler(this.TemplateURL_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Template URL";
			// 
			// MapGroup
			// 
			this.MapGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MapGroup.Controls.Add(this.MapPropertiesPanel);
			this.MapGroup.Controls.Add(this.splitter1);
			this.MapGroup.Controls.Add(this.panel1);
			this.MapGroup.Location = new System.Drawing.Point(0, 56);
			this.MapGroup.Name = "MapGroup";
			this.MapGroup.Size = new System.Drawing.Size(704, 176);
			this.MapGroup.TabIndex = 4;
			this.MapGroup.TabStop = false;
			this.MapGroup.Text = "Maps";
			// 
			// MapPropertiesPanel
			// 
			this.MapPropertiesPanel.Controls.Add(this.MapID);
			this.MapPropertiesPanel.Controls.Add(this.label9);
			this.MapPropertiesPanel.Controls.Add(this.MapSingleTileCheck);
			this.MapPropertiesPanel.Controls.Add(this.MapTypeCombo);
			this.MapPropertiesPanel.Controls.Add(this.label8);
			this.MapPropertiesPanel.Controls.Add(this.OverrideDisplayExtents);
			this.MapPropertiesPanel.Controls.Add(this.overriddenMapExtents);
			this.MapPropertiesPanel.Controls.Add(this.BrowseMapButton);
			this.MapPropertiesPanel.Controls.Add(this.MapResourceID);
			this.MapPropertiesPanel.Controls.Add(this.label3);
			this.MapPropertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MapPropertiesPanel.Location = new System.Drawing.Point(208, 16);
			this.MapPropertiesPanel.Name = "MapPropertiesPanel";
			this.MapPropertiesPanel.Size = new System.Drawing.Size(493, 157);
			this.MapPropertiesPanel.TabIndex = 3;
			this.MapPropertiesPanel.Visible = false;
			// 
			// MapID
			// 
			this.MapID.Location = new System.Drawing.Point(128, 32);
			this.MapID.Name = "MapID";
			this.MapID.Size = new System.Drawing.Size(352, 20);
			this.MapID.TabIndex = 10;
			this.MapID.Text = "textBox1";
			this.MapID.TextChanged += new System.EventHandler(this.MapID_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 32);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 9;
			this.label9.Text = "Map ID";
			// 
			// MapSingleTileCheck
			// 
			this.MapSingleTileCheck.Location = new System.Drawing.Point(280, 128);
			this.MapSingleTileCheck.Name = "MapSingleTileCheck";
			this.MapSingleTileCheck.Size = new System.Drawing.Size(200, 16);
			this.MapSingleTileCheck.TabIndex = 8;
			this.MapSingleTileCheck.Text = "Single tiled";
			this.MapSingleTileCheck.CheckedChanged += new System.EventHandler(this.MapSingleTileCheck_CheckedChanged);
			// 
			// MapTypeCombo
			// 
			this.MapTypeCombo.Location = new System.Drawing.Point(128, 128);
			this.MapTypeCombo.Name = "MapTypeCombo";
			this.MapTypeCombo.Size = new System.Drawing.Size(128, 21);
			this.MapTypeCombo.TabIndex = 7;
			this.MapTypeCombo.Text = "comboBox1";
			this.MapTypeCombo.SelectedIndexChanged += new System.EventHandler(this.MapTypeCombo_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 128);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 16);
			this.label8.TabIndex = 6;
			this.label8.Text = "MapType";
			// 
			// OverrideDisplayExtents
			// 
			this.OverrideDisplayExtents.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OverrideDisplayExtents.Location = new System.Drawing.Point(16, 64);
			this.OverrideDisplayExtents.Name = "OverrideDisplayExtents";
			this.OverrideDisplayExtents.Size = new System.Drawing.Size(216, 16);
			this.OverrideDisplayExtents.TabIndex = 5;
			this.OverrideDisplayExtents.Text = "Override the maps initial display extents";
			this.OverrideDisplayExtents.CheckedChanged += new System.EventHandler(this.OverrideDisplayExtents_CheckedChanged);
			// 
			// overriddenMapExtents
			// 
			this.overriddenMapExtents.Controls.Add(this.label6);
			this.overriddenMapExtents.Controls.Add(this.overrideScale);
			this.overriddenMapExtents.Controls.Add(this.overrideY);
			this.overriddenMapExtents.Controls.Add(this.overrideX);
			this.overriddenMapExtents.Controls.Add(this.label5);
			this.overriddenMapExtents.Controls.Add(this.label4);
			this.overriddenMapExtents.Controls.Add(this.label7);
			this.overriddenMapExtents.Enabled = false;
			this.overriddenMapExtents.Location = new System.Drawing.Point(8, 64);
			this.overriddenMapExtents.Name = "overriddenMapExtents";
			this.overriddenMapExtents.Size = new System.Drawing.Size(472, 56);
			this.overriddenMapExtents.TabIndex = 4;
			this.overriddenMapExtents.TabStop = false;
			this.overriddenMapExtents.Enter += new System.EventHandler(this.overriddenMapExtents_Enter);
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(336, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "1:";
			// 
			// overrideScale
			// 
			this.overrideScale.Location = new System.Drawing.Point(352, 24);
			this.overrideScale.Name = "overrideScale";
			this.overrideScale.Size = new System.Drawing.Size(104, 20);
			this.overrideScale.TabIndex = 5;
			this.overrideScale.Text = "";
			this.overrideScale.TextChanged += new System.EventHandler(this.overrideScale_TextChanged);
			// 
			// overrideY
			// 
			this.overrideY.Location = new System.Drawing.Point(184, 24);
			this.overrideY.Name = "overrideY";
			this.overrideY.Size = new System.Drawing.Size(104, 20);
			this.overrideY.TabIndex = 4;
			this.overrideY.Text = "";
			this.overrideY.TextChanged += new System.EventHandler(this.overrideY_TextChanged);
			// 
			// overrideX
			// 
			this.overrideX.Location = new System.Drawing.Point(40, 24);
			this.overrideX.Name = "overrideX";
			this.overrideX.Size = new System.Drawing.Size(104, 20);
			this.overrideX.TabIndex = 3;
			this.overrideX.Text = "";
			this.overrideX.TextChanged += new System.EventHandler(this.overrideX_TextChanged);
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(296, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Scale";
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(160, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Y";
			// 
			// label7
			// 
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(16, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(24, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "X";
			// 
			// BrowseMapButton
			// 
			this.BrowseMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseMapButton.Location = new System.Drawing.Point(459, 8);
			this.BrowseMapButton.Name = "BrowseMapButton";
			this.BrowseMapButton.Size = new System.Drawing.Size(24, 20);
			this.BrowseMapButton.TabIndex = 2;
			this.BrowseMapButton.Text = "...";
			this.BrowseMapButton.Click += new System.EventHandler(this.BrowseMapButton_Click);
			// 
			// MapResourceID
			// 
			this.MapResourceID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MapResourceID.Location = new System.Drawing.Point(128, 8);
			this.MapResourceID.Name = "MapResourceID";
			this.MapResourceID.ReadOnly = true;
			this.MapResourceID.Size = new System.Drawing.Size(323, 20);
			this.MapResourceID.TabIndex = 1;
			this.MapResourceID.Text = "textBox1";
			this.MapResourceID.TextChanged += new System.EventHandler(this.MapResourceID_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Map Resource";
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 16);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 157);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.MapList);
			this.panel1.Controls.Add(this.MapToolbar);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(3, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(197, 157);
			this.panel1.TabIndex = 1;
			// 
			// MapList
			// 
			this.MapList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MapList.Location = new System.Drawing.Point(0, 28);
			this.MapList.MultiSelect = false;
			this.MapList.Name = "MapList";
			this.MapList.Size = new System.Drawing.Size(197, 129);
			this.MapList.SmallImageList = this.toolbarImages;
			this.MapList.TabIndex = 0;
			this.MapList.View = System.Windows.Forms.View.List;
			this.MapList.SelectedIndexChanged += new System.EventHandler(this.MapList_SelectedIndexChanged);
			// 
			// toolbarImages
			// 
			this.toolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
			this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MapToolbar
			// 
			this.MapToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						  this.AddMapButton,
																						  this.RemoveMapButton});
			this.MapToolbar.DropDownArrows = true;
			this.MapToolbar.ImageList = this.toolbarImages;
			this.MapToolbar.Location = new System.Drawing.Point(0, 0);
			this.MapToolbar.Name = "MapToolbar";
			this.MapToolbar.ShowToolTips = true;
			this.MapToolbar.Size = new System.Drawing.Size(197, 28);
			this.MapToolbar.TabIndex = 1;
			this.MapToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MapToolbar_ButtonClick);
			// 
			// AddMapButton
			// 
			this.AddMapButton.ImageIndex = 0;
			// 
			// RemoveMapButton
			// 
			this.RemoveMapButton.ImageIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel3);
			this.groupBox1.Controls.Add(this.splitter2);
			this.groupBox1.Controls.Add(this.panel4);
			this.groupBox1.Location = new System.Drawing.Point(0, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(704, 400);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Widgets";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.flyoutEntry);
			this.panel3.Controls.Add(this.widgetEntry);
			this.panel3.Controls.Add(this.containerEditor);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(208, 16);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(493, 381);
			this.panel3.TabIndex = 3;
			// 
			// flyoutEntry
			// 
			this.flyoutEntry.Location = new System.Drawing.Point(16, 128);
			this.flyoutEntry.Name = "flyoutEntry";
			this.flyoutEntry.Size = new System.Drawing.Size(392, 104);
			this.flyoutEntry.TabIndex = 3;
			// 
			// widgetEntry
			// 
			this.widgetEntry.Location = new System.Drawing.Point(16, 88);
			this.widgetEntry.Name = "widgetEntry";
			this.widgetEntry.Size = new System.Drawing.Size(392, 40);
			this.widgetEntry.TabIndex = 2;
			this.widgetEntry.Visible = false;
			// 
			// containerEditor
			// 
			this.containerEditor.Location = new System.Drawing.Point(16, 8);
			this.containerEditor.Name = "containerEditor";
			this.containerEditor.Size = new System.Drawing.Size(400, 80);
			this.containerEditor.TabIndex = 0;
			this.containerEditor.Visible = false;
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(200, 16);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(8, 381);
			this.splitter2.TabIndex = 2;
			this.splitter2.TabStop = false;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.WidgetTree);
			this.panel4.Controls.Add(this.WidgetToolBar);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel4.Location = new System.Drawing.Point(3, 16);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(197, 381);
			this.panel4.TabIndex = 1;
			// 
			// WidgetTree
			// 
			this.WidgetTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WidgetTree.ImageList = this.toolbarImages;
			this.WidgetTree.Location = new System.Drawing.Point(0, 28);
			this.WidgetTree.Name = "WidgetTree";
			this.WidgetTree.Size = new System.Drawing.Size(197, 353);
			this.WidgetTree.TabIndex = 2;
			this.WidgetTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.WidgetTree_AfterSelect);
			// 
			// WidgetToolBar
			// 
			this.WidgetToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							 this.AddWidgetButton,
																							 this.AddContainerButton,
																							 this.RemoveWidgetButton,
																							 this.toolBarButton1,
																							 this.MoveWidgetUpButton,
																							 this.MoveWidgetDownButton,
																							 this.toolBarButton2,
																							 this.ConfigureWidgetsButton});
			this.WidgetToolBar.DropDownArrows = true;
			this.WidgetToolBar.ImageList = this.toolbarImages;
			this.WidgetToolBar.Location = new System.Drawing.Point(0, 0);
			this.WidgetToolBar.Name = "WidgetToolBar";
			this.WidgetToolBar.ShowToolTips = true;
			this.WidgetToolBar.Size = new System.Drawing.Size(197, 28);
			this.WidgetToolBar.TabIndex = 1;
			this.WidgetToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.WidgetToolBar_ButtonClick);
			// 
			// AddWidgetButton
			// 
			this.AddWidgetButton.Enabled = false;
			this.AddWidgetButton.ImageIndex = 10;
			this.AddWidgetButton.ToolTipText = "Add a widget";
			// 
			// AddContainerButton
			// 
			this.AddContainerButton.ImageIndex = 9;
			this.AddContainerButton.ToolTipText = "Add a container";
			// 
			// RemoveWidgetButton
			// 
			this.RemoveWidgetButton.Enabled = false;
			this.RemoveWidgetButton.ImageIndex = 1;
			this.RemoveWidgetButton.ToolTipText = "Delete an item";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// MoveWidgetUpButton
			// 
			this.MoveWidgetUpButton.ImageIndex = 2;
			this.MoveWidgetUpButton.ToolTipText = "Move the selected item up";
			// 
			// MoveWidgetDownButton
			// 
			this.MoveWidgetDownButton.ImageIndex = 3;
			this.MoveWidgetDownButton.ToolTipText = "Move the selected item down";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ConfigureWidgetsButton
			// 
			this.ConfigureWidgetsButton.ImageIndex = 6;
			this.ConfigureWidgetsButton.ToolTipText = "Configure avalible widgets";
			// 
			// SelectTemplateBtn
			// 
			this.SelectTemplateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectTemplateBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectTemplateBtn.Location = new System.Drawing.Point(680, 24);
			this.SelectTemplateBtn.Name = "SelectTemplateBtn";
			this.SelectTemplateBtn.Size = new System.Drawing.Size(24, 20);
			this.SelectTemplateBtn.TabIndex = 12;
			this.SelectTemplateBtn.Text = "...";
			this.SelectTemplateBtn.Click += new System.EventHandler(this.SelectTemplateBtn_Click);
			// 
			// AddWidgetMenu
			// 
			this.AddWidgetMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.AddWidgetSeperatorMenu,
																						  this.AddWidgetSubmenuMenu,
																						  this.AddWidgetEntryMenu});
			this.AddWidgetMenu.Popup += new System.EventHandler(this.AddWidgetMenu_Popup);
			// 
			// AddWidgetSeperatorMenu
			// 
			this.AddWidgetSeperatorMenu.Index = 0;
			this.AddWidgetSeperatorMenu.Text = "Seperator";
			this.AddWidgetSeperatorMenu.Click += new System.EventHandler(this.AddWidgetSeperatorMenu_Click);
			// 
			// AddWidgetSubmenuMenu
			// 
			this.AddWidgetSubmenuMenu.Index = 1;
			this.AddWidgetSubmenuMenu.Text = "Submenu";
			this.AddWidgetSubmenuMenu.Click += new System.EventHandler(this.AddWidgetSubmenuMenu_Click);
			// 
			// AddWidgetEntryMenu
			// 
			this.AddWidgetEntryMenu.Index = 2;
			this.AddWidgetEntryMenu.Text = "Widget";
			this.AddWidgetEntryMenu.Click += new System.EventHandler(this.AddWidgetEntryMenu_Click);
			// 
			// ShowInBrowser
			// 
			this.ShowInBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowInBrowser.Location = new System.Drawing.Point(568, 240);
			this.ShowInBrowser.Name = "ShowInBrowser";
			this.ShowInBrowser.Size = new System.Drawing.Size(136, 20);
			this.ShowInBrowser.TabIndex = 20;
			this.ShowInBrowser.Text = "Show in browser";
			this.ShowInBrowser.Click += new System.EventHandler(this.ShowInBrowser_Click);
			// 
			// browserURL
			// 
			this.browserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.browserURL.Location = new System.Drawing.Point(160, 240);
			this.browserURL.Name = "browserURL";
			this.browserURL.ReadOnly = true;
			this.browserURL.Size = new System.Drawing.Size(400, 20);
			this.browserURL.TabIndex = 19;
			this.browserURL.Text = "";
			// 
			// label12
			// 
			this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label12.Location = new System.Drawing.Point(0, 240);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(144, 16);
			this.label12.TabIndex = 18;
			this.label12.Text = "View in browser";
			// 
			// ApplicationDefinitionEditor
			// 
			this.Controls.Add(this.ShowInBrowser);
			this.Controls.Add(this.browserURL);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.SelectTemplateBtn);
			this.Controls.Add(this.MapGroup);
			this.Controls.Add(this.TemplateURL);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.MapTitle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Name = "ApplicationDefinitionEditor";
			this.Size = new System.Drawing.Size(704, 664);
			this.Load += new System.EventHandler(this.ApplicationDefinitionEditor_Load);
			this.MapGroup.ResumeLayout(false);
			this.MapPropertiesPanel.ResumeLayout(false);
			this.overriddenMapExtents.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public bool Preview()
		{
			ShowInBrowser_Click(null, null);
			return true;
		}

		private void ApplicationDefinitionEditor_Load(object sender, System.EventArgs e)
		{
		
		}

		private void WidgetTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			Control ed = null;
			if (WidgetTree.SelectedNode != null && WidgetTree.SelectedNode.Tag != null && m_widgetEditorLookup.ContainsKey(WidgetTree.SelectedNode.Tag.GetType()))
				ed = m_widgetEditorLookup[WidgetTree.SelectedNode.Tag.GetType()] as Control;
				
			foreach(Control c in panel3.Controls)
				c.Visible = c == ed;

			if (ed != null)
			{
				ed.GetType().GetMethod("SetItem").Invoke(ed, new object[] { WidgetTree.SelectedNode.Tag });
				ed.Dock = DockStyle.Fill;

			}

			AddWidgetButton.Enabled = GetParentContainerNode() != null;
			RemoveWidgetButton.Enabled = WidgetTree.SelectedNode != null;

			if (WidgetTree.SelectedNode != null)
			{
				TreeNodeCollection c = WidgetTree.SelectedNode.Parent == null ? WidgetTree.Nodes : WidgetTree.SelectedNode.Parent.Nodes;
				MoveWidgetUpButton.Enabled = c.IndexOf(WidgetTree.SelectedNode) != 0;
				MoveWidgetDownButton.Enabled = c.IndexOf(WidgetTree.SelectedNode) != c.Count - 1;
			}
			else
			{
				MoveWidgetUpButton.Enabled = false;
				MoveWidgetUpButton.Enabled = false;
			}
		}

		private void SelectTemplateBtn_Click(object sender, System.EventArgs e)
		{
			SelectTemplate dlg = new SelectTemplate();
			dlg.BaseURL = (((HttpServerConnection)m_editor.CurrentConnection).BaseURL);
			dlg.SetupCombos(m_editor.CurrentConnection.GetApplicationTemplates());
			if (dlg.ShowDialog(this) == DialogResult.OK)
				TemplateURL.Text = dlg.txtUrl.Text;
		}

		private void WidgetToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == AddWidgetButton)
			{
				AddWidgetMenu.Show(e.Button.Parent, new Point(e.Button.Rectangle.Left, e.Button.Rectangle.Bottom));
			}
			else if (e.Button == AddContainerButton)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType c = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType();
				c.Name = m_globalizor.Translate("New container");
				c.Item = new UiItemTypeCollection();
				m_appDef.WidgetSet[0].Container.Add(c);
				UpdateDisplay();
				WidgetTree.SelectedNode = WidgetTree.Nodes[WidgetTree.Nodes.Count - 1];
				WidgetTree.SelectedNode.EnsureVisible();
			}
			else if (e.Button == RemoveWidgetButton)
			{
				if (WidgetTree.SelectedNode == null)
					return;

				if (WidgetTree.SelectedNode.Parent == null)
				{
					foreach(WidgetSetType wst in m_appDef.WidgetSet)
						for(int i = 0; i < wst.Container.Count; i++)
							if (wst.Container[i] == WidgetTree.SelectedNode.Tag)
							{
								wst.Container.RemoveAt(i);
								UpdateDisplay();
								break;
							}
				}
				else
				{
					UiItemTypeCollection col = null;
					if (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType != null)
						col = (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType).Item;
					else if (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType != null)
						col = (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType).Item;

					if (col != null)
						for(int i = 0; i < col.Count; i++)
							if (col[i] == WidgetTree.SelectedNode.Tag)
							{
								col.RemoveAt(i);
								TreeNode b = WidgetTree.SelectedNode.Parent;
								b.Nodes.Clear();
								FillNode(col, b.Nodes);
								break;
							}

				}
			}
			else if (e.Button == MoveWidgetUpButton || e.Button == MoveWidgetDownButton)
			{
				if (WidgetTree.SelectedNode == null)
					return;

				if (WidgetTree.SelectedNode.Parent == null)
				{
					ContainerTypeCollection col = null;
					foreach(WidgetSetType wst in m_appDef.WidgetSet)
						if (wst.Container.Contains((OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType)WidgetTree.SelectedNode.Tag))
						{
							col = wst.Container;
							break;
						}

					if (col != null)
					{
						OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType item = WidgetTree.SelectedNode.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType;
						int index = col.IndexOf(item);

						if (e.Button == MoveWidgetUpButton)
						{
							if (index == 0)
								return;
							col.RemoveAt(index);
							col.Insert(index - 1, item);
						}
						else
						{
							if (index == col.Count - 1)
								return;
							col.RemoveAt(index);
							col.Insert(index + 1, item);
						}

						try
						{
							WidgetTree.BeginUpdate();
							UpdateDisplay();
						}
						finally
						{
							try { WidgetTree.EndUpdate(); }
							catch {}
						}

						foreach(TreeNode n in WidgetTree.Nodes)
							if (n.Tag == item)
							{
								if (n.Index != 0)
								{
									WidgetTree.Nodes[0].Collapse();
									n.Expand();
								}
								WidgetTree.SelectedNode = n;
								break;
							}
						
					}
				}
				else
				{
					UiItemTypeCollection col = null;
					if (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType != null)
						col = (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType).Item;
					else if (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType != null)
						col = (WidgetTree.SelectedNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType).Item;

					if (col != null)
					{
						OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemType item = WidgetTree.SelectedNode.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemType;
						int index = col.IndexOf(item);
						if (e.Button == MoveWidgetUpButton)
						{
							if (index == 0)
								return;
							col.RemoveAt(index);
							col.Insert(index - 1, item);
						}
						else
						{
							if (index == col.Count - 1)
								return;
							col.RemoveAt(index);
							col.Insert(index + 1, item);
						}

						TreeNode b = WidgetTree.SelectedNode.Parent;
						b.Nodes.Clear();
						try
						{
							WidgetTree.BeginUpdate();
							FillNode(col, b.Nodes);
						}
						finally
						{
							try { WidgetTree.EndUpdate(); }
							catch {}
						}
						foreach(TreeNode n in b.Nodes)
							if (n.Tag == item)
							{
								WidgetTree.SelectedNode = n;
								break;
							}
					}
				}
			}
			else if (e.Button == ConfigureWidgetsButton)
			{
				EditWidgets dlg = new EditWidgets();
				dlg.SetupDialog(m_editor.CurrentConnection.GetApplicationWidgets(), m_appDef);
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					UpdateDisplay();
					m_editor.HasChanged();
				}
			}

		}

		private void AddWidgetMenu_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void MapList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				m_isUpdating = true;
				OverrideDisplayExtents.Tag = null;

				if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				{
					MapPropertiesPanel.Visible = false;
				}
				else
				{
					MapPropertiesPanel.Visible = true;
					MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;
					MapID.Text = mgr.id;
					if (mgr.Map.Count >= 1)
					{
						bool found = false;
						MapTypeCombo.Text = mgr.Map[0].Type;
						MapSingleTileCheck.Checked = bool.Parse(mgr.Map[0].SingleTile); 
						if (mgr.Map[0].Extension != null && mgr.Map[0].Extension.Any != null)
							foreach(System.Xml.XmlNode n in mgr.Map[0].Extension.Any)
								if (n.Name == "ResourceId")
								{
									MapResourceID.Text = n.InnerText;
									found = true;
									break;
								}

						if (!found)
							MapResourceID.Text = "";
					}

					UpdateDisplayOverrideExtents(mgr);
				}
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void UpdateDisplayOverrideExtents(MapGroupType mgr)
		{
			if (mgr.InitialView == null)
			{
				OverrideDisplayExtents.Checked = false;
			}
			else
			{
				OverrideDisplayExtents.Checked = false;
				overrideX.Text = mgr.InitialView.CenterX.ToString();
				overrideY.Text = mgr.InitialView.CenterY.ToString();
				overrideScale.Text = mgr.InitialView.Scale.ToString();
			}
		}

		private void MapResourceID_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;

			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;
			if (mgr.Map.Count == 0)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mt = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType();
				mt.SingleTile = "true";
				mt.Type = "MapGuide";
				mgr.Map.Add(mt);
			}
			
			if (mgr.Map[0].Extension == null)
				mgr.Map[0].Extension = new CustomContentType();

			if (mgr.Map[0].Extension.Any == null || mgr.Map[0].Extension.Any.Length == 0)
			{
				mgr.Map[0].Extension.Any = new System.Xml.XmlElement[1];
				mgr.Map[0].Extension.Any[0] = m_appDef.ApplicationDocument.CreateElement("ResourceId");
				}

			foreach(System.Xml.XmlNode n in mgr.Map[0].Extension.Any)
				if (n.Name == "ResourceId")
				{
					n.InnerText = MapResourceID.Text;
					m_editor.HasChanged();
					break;
				}
		}

		public string ResourceId
		{
			get { return m_appDef.ResourceId; }
			set { m_appDef.ResourceId = value; }
		}

		public object Resource
		{
			get { return m_appDef; }
			set 
			{ 
				m_appDef = (ApplicationDefinitionType)value;
				UpdateDisplay();
			}
		}


		private void MapToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == AddMapButton)
			{
				MapGroupType mgr = new MapGroupType();
				mgr.id = "New map";
				mgr.InitialView = null;
				mgr.Map = new MapTypeCollection();
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mt = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType();
				mt.SingleTile = "true";
				mt.Type = "MapGuide";
				mt.Extension = new CustomContentType();
				mt.Extension.Any = new System.Xml.XmlElement[1];
				mt.Extension.Any[0] =  m_appDef.ApplicationDocument.CreateElement("ResourceId");
				mgr.Map.Add(mt);
				m_appDef.MapSet.Add(mgr);
				m_editor.HasChanged();
				UpdateDisplay();
				MapList.Items[MapList.Items.Count - 1].Selected = true;
			}
			else if (e.Button == RemoveMapButton)
			{
				if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
					return;

				for(int i = 0; i < m_appDef.MapSet.Count; i++)
					if (m_appDef.MapSet[i] == MapList.SelectedItems[0].Tag)
					{
						m_appDef.MapSet.RemoveAt(i);
						m_editor.HasChanged();
						UpdateDisplay();
						MapList_SelectedIndexChanged(sender, new EventArgs());
						break;
					}
			}
		}

		private void MapID_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;
			mgr.id = MapID.Text;
			MapList.SelectedItems[0].Text = MapID.Text;
			m_editor.HasChanged();
		}

		private void BrowseMapButton_Click(object sender, System.EventArgs e)
		{
			string item = m_editor.BrowseResource("MapDefinition");
			if (item != null)
				MapResourceID.Text = item;
		}

		private void MapTypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;
			if (mgr.Map.Count == 0)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mt = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType();
				mt.SingleTile = "true";
				mt.Type = "MapGuide";
				mgr.Map.Add(mt);
			}

			mgr.Map[0].Type = MapTypeCombo.Text;
			m_editor.HasChanged();
		}

		private void MapSingleTileCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;
			if (mgr.Map.Count == 0)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mt = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType();
				mt.SingleTile = "true";
				mt.Type = "MapGuide";
				mgr.Map.Add(mt);
			}

			mgr.Map[0].SingleTile = MapSingleTileCheck.Checked.ToString().ToLower();
			m_editor.HasChanged();
		}

		private void OverrideDisplayExtents_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;

			overriddenMapExtents.Enabled = OverrideDisplayExtents.Checked;
			if (!OverrideDisplayExtents.Checked)
			{
				if (mgr.InitialView != null)
					OverrideDisplayExtents.Tag = mgr.InitialView;
				mgr.InitialView = null;
			}
			else
			{
				if (OverrideDisplayExtents.Tag != null)
					mgr.InitialView = (OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapViewType)OverrideDisplayExtents.Tag;
				OverrideDisplayExtents.Tag = null;

			}

			m_editor.HasChanged();

			UpdateDisplayOverrideExtents(mgr);
		}

		private void overrideX_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;

			if (mgr.InitialView == null)
				return;

			double v;
			if (double.TryParse(overrideX.Text, System.Globalization.NumberStyles.Float, null, out v))
			{
				mgr.InitialView.CenterX = v;
				m_editor.HasChanged();
			}

		}

		private void overriddenMapExtents_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void overrideY_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;

			if (mgr.InitialView == null)
				return;

			double v;
			if (double.TryParse(overrideY.Text, System.Globalization.NumberStyles.Float, null, out v))
			{
				mgr.InitialView.CenterY = v;
				m_editor.HasChanged();
			}
		}

		private void overrideScale_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
				return;
		
			MapGroupType mgr = (MapGroupType)MapList.SelectedItems[0].Tag;

			if (mgr.InitialView == null)
				return;

			double v;
			if (double.TryParse(overrideScale.Text, System.Globalization.NumberStyles.Float, null, out v))
			{
				mgr.InitialView.Scale = v;
				m_editor.HasChanged();
			}
		
		}

		private void TemplateURL_TextChanged(object sender, System.EventArgs e)
		{
			if (m_editor.Existing)
				browserURL.Text = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL + TemplateURL.Text + "?ApplicationDefinition=" + System.Web.HttpUtility.UrlEncode(m_appDef.ResourceId);
			else
				browserURL.Text = "";

			if (m_isUpdating)
				return;

			m_appDef.TemplateUrl = TemplateURL.Text;
			m_editor.HasChanged();

		}

		private void MapTitle_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_appDef.Title = MapTitle.Text;
			m_editor.HasChanged();
		}

		private void AddWidgetSeperatorMenu_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.SeparatorItemType sep = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.SeparatorItemType();
			sep.Function = UiItemFunctionType.Separator;
			AddItemToWidgetTree(sep);
		}

		private void AddWidgetSubmenuMenu_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType flv = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType();
			flv.Item = new UiItemTypeCollection();
			flv.Label = m_globalizor.Translate("New submenu");
			flv.Function = UiItemFunctionType.Flyout;
			AddItemToWidgetTree(flv);
		}

		private TreeNode GetParentContainerNode()
		{
			TreeNode p = WidgetTree.SelectedNode;
			if (p == null)
				return null;

			if (p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType == null
				&& p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType == null)
				p = p.Parent;

			if (p == null)
				return null;

			if (p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType == null
				&& p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType == null)
				return null;

			return p;
		}

		private void AddItemToWidgetTree(UiItemType item)
		{
			TreeNode p = GetParentContainerNode();
			
			OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemTypeCollection col = null;
			if (p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType != null)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType flv = p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType;
				col = flv.Item;
			}
			else if (p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType != null)
			{
				OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType c = p.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType;
				col = c.Item;
			}
			
			if (col != null)
			{
				col.Add(item);
				p.Nodes.Clear();
				FillNode(col, p.Nodes);
				m_editor.HasChanged();
			}
		}

		private void AddWidgetEntryMenu_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetItemType w = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetItemType();
			w.Function = UiItemFunctionType.Widget;
			w.Widget = "About";
			AddItemToWidgetTree(w);
		}

		private void containerEditor_ValueChanged(object sender, object item)
		{
			if (WidgetTree.SelectedNode != null)
				WidgetTree.SelectedNode.Text = ((UiItemContainerType)item).Name;
			m_editor.HasChanged();
		}

		private void widgetEntry_ValueChanged(object sender, object item)
		{
			if (WidgetTree.SelectedNode != null)
				WidgetTree.SelectedNode.Text = ((WidgetItemType)item).Widget;
			m_editor.HasChanged();
		}

		private void flyoutEntry_ValueChanged(object sender, object item)
		{
			if (WidgetTree.SelectedNode != null)
				WidgetTree.SelectedNode.Text = ((OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType)item).Label;
			m_editor.HasChanged();
		}

		public bool Save(string savename)
		{
			return false;
		}

		private void ShowInBrowser_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_editor.CurrentConnection.SaveResourceAs(m_appDef, m_tempResource);
				string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL + TemplateURL.Text + "?ApplicationDefinition=" + System.Web.HttpUtility.UrlEncode(m_tempResource) + "&SESSION=" + System.Web.HttpUtility.UrlEncode(m_editor.CurrentConnection.SessionID);
				if (!url.StartsWith("http://") && !url.StartsWith("https://"))
					throw new Exception ("Malformed URL");

				try
				{
					System.Diagnostics.Process process = new System.Diagnostics.Process();
					process.StartInfo.FileName = url;
					process.StartInfo.UseShellExecute = true;
					process.Start();
				}
				catch
				{
					//The straightforward method gives an error: "The requested lookup key was not found in any active activation context"
					System.Diagnostics.Process process = new System.Diagnostics.Process();
					process.StartInfo.FileName = "rundll32.exe";
					process.StartInfo.Arguments = "url.dll,FileProtocolHandler " + url;
					process.StartInfo.UseShellExecute = true;
					process.Start();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, String.Format(m_globalizor.Translate("Failed to launch browser: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		
		}
	}
}
