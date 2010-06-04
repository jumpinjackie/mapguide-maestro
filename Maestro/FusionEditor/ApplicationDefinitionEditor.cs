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
	public class ApplicationDefinitionEditor : System.Windows.Forms.UserControl, OSGeo.MapGuide.Maestro.IResourceEditorControl
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
        private System.Windows.Forms.ImageList toolbarImages;
		private System.Windows.Forms.Panel MapPropertiesPanel;
		private System.Windows.Forms.ComboBox MapTypeCombo;
		private System.Windows.Forms.CheckBox MapSingleTileCheck;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox MapID;
		private FusionEditor.FlyoutEditor flyoutEntry;
		private System.Windows.Forms.Button ShowInBrowser;
		private System.Windows.Forms.TextBox browserURL;
		private System.Windows.Forms.Label label12;
		private string m_tempResource;
        private ToolStrip toolStrip1;
        private ToolStripButton AddMapButton;
        private ToolStripButton RemoveMapButton;
        private ToolStrip toolStrip2;
        private ToolStripDropDownButton AddWidgetButton;
        private ToolStripButton AddContainerButton;
        private ToolStripButton RemoveWidgetButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton MoveWidgetUpButton;
        private ToolStripButton MoveWidgetDownButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ConfigureWidgetsButton;
        private ToolStripMenuItem AddWidgetSeperatorMenu;
        private ToolStripMenuItem AddWidgetSubmenuMenu;
        private ToolStripMenuItem AddWidgetEntryMenu;


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
		}

		public ApplicationDefinitionEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_appDef = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.ApplicationDefinitionType();
            m_tempResource = new ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, m_editor.CurrentConnection.SessionID);
			UpdateDisplay();
		}

		public ApplicationDefinitionEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_appDef = m_editor.CurrentConnection.GetApplicationDefinition(resourceID);
            m_tempResource = new ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, m_editor.CurrentConnection.SessionID);
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
					TreeNode tn = new TreeNode(Strings.ApplicationDefinitionEditor.SeperatorMarker);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationDefinitionEditor));
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddMapButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveMapButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flyoutEntry = new OSGeo.MapGuide.Maestro.FusionEditor.FlyoutEditor();
            this.widgetEntry = new OSGeo.MapGuide.Maestro.FusionEditor.WidgetEntry();
            this.containerEditor = new OSGeo.MapGuide.Maestro.FusionEditor.ContainerEditor();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.WidgetTree = new System.Windows.Forms.TreeView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.AddWidgetButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddWidgetSeperatorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddWidgetSubmenuMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddWidgetEntryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddContainerButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveWidgetButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveWidgetUpButton = new System.Windows.Forms.ToolStripButton();
            this.MoveWidgetDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ConfigureWidgetsButton = new System.Windows.Forms.ToolStripButton();
            this.SelectTemplateBtn = new System.Windows.Forms.Button();
            this.ShowInBrowser = new System.Windows.Forms.Button();
            this.browserURL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.MapGroup.SuspendLayout();
            this.MapPropertiesPanel.SuspendLayout();
            this.overriddenMapExtents.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // MapTitle
            // 
            resources.ApplyResources(this.MapTitle, "MapTitle");
            this.MapTitle.Name = "MapTitle";
            this.MapTitle.TextChanged += new System.EventHandler(this.MapTitle_TextChanged);
            // 
            // TemplateURL
            // 
            resources.ApplyResources(this.TemplateURL, "TemplateURL");
            this.TemplateURL.Name = "TemplateURL";
            this.TemplateURL.TextChanged += new System.EventHandler(this.TemplateURL_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // MapGroup
            // 
            resources.ApplyResources(this.MapGroup, "MapGroup");
            this.MapGroup.Controls.Add(this.MapPropertiesPanel);
            this.MapGroup.Controls.Add(this.splitter1);
            this.MapGroup.Controls.Add(this.panel1);
            this.MapGroup.Name = "MapGroup";
            this.MapGroup.TabStop = false;
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
            resources.ApplyResources(this.MapPropertiesPanel, "MapPropertiesPanel");
            this.MapPropertiesPanel.Name = "MapPropertiesPanel";
            // 
            // MapID
            // 
            resources.ApplyResources(this.MapID, "MapID");
            this.MapID.Name = "MapID";
            this.MapID.TextChanged += new System.EventHandler(this.MapID_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // MapSingleTileCheck
            // 
            resources.ApplyResources(this.MapSingleTileCheck, "MapSingleTileCheck");
            this.MapSingleTileCheck.Name = "MapSingleTileCheck";
            this.MapSingleTileCheck.CheckedChanged += new System.EventHandler(this.MapSingleTileCheck_CheckedChanged);
            // 
            // MapTypeCombo
            // 
            resources.ApplyResources(this.MapTypeCombo, "MapTypeCombo");
            this.MapTypeCombo.Name = "MapTypeCombo";
            this.MapTypeCombo.SelectedIndexChanged += new System.EventHandler(this.MapTypeCombo_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // OverrideDisplayExtents
            // 
            resources.ApplyResources(this.OverrideDisplayExtents, "OverrideDisplayExtents");
            this.OverrideDisplayExtents.Name = "OverrideDisplayExtents";
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
            resources.ApplyResources(this.overriddenMapExtents, "overriddenMapExtents");
            this.overriddenMapExtents.Name = "overriddenMapExtents";
            this.overriddenMapExtents.TabStop = false;
            this.overriddenMapExtents.Enter += new System.EventHandler(this.overriddenMapExtents_Enter);
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // overrideScale
            // 
            resources.ApplyResources(this.overrideScale, "overrideScale");
            this.overrideScale.Name = "overrideScale";
            this.overrideScale.TextChanged += new System.EventHandler(this.overrideScale_TextChanged);
            // 
            // overrideY
            // 
            resources.ApplyResources(this.overrideY, "overrideY");
            this.overrideY.Name = "overrideY";
            this.overrideY.TextChanged += new System.EventHandler(this.overrideY_TextChanged);
            // 
            // overrideX
            // 
            resources.ApplyResources(this.overrideX, "overrideX");
            this.overrideX.Name = "overrideX";
            this.overrideX.TextChanged += new System.EventHandler(this.overrideX_TextChanged);
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // BrowseMapButton
            // 
            resources.ApplyResources(this.BrowseMapButton, "BrowseMapButton");
            this.BrowseMapButton.Name = "BrowseMapButton";
            this.BrowseMapButton.Click += new System.EventHandler(this.BrowseMapButton_Click);
            // 
            // MapResourceID
            // 
            resources.ApplyResources(this.MapResourceID, "MapResourceID");
            this.MapResourceID.Name = "MapResourceID";
            this.MapResourceID.ReadOnly = true;
            this.MapResourceID.TextChanged += new System.EventHandler(this.MapResourceID_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MapList);
            this.panel1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // MapList
            // 
            resources.ApplyResources(this.MapList, "MapList");
            this.MapList.MultiSelect = false;
            this.MapList.Name = "MapList";
            this.MapList.SmallImageList = this.toolbarImages;
            this.MapList.UseCompatibleStateImageBehavior = false;
            this.MapList.View = System.Windows.Forms.View.List;
            this.MapList.SelectedIndexChanged += new System.EventHandler(this.MapList_SelectedIndexChanged);
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
            this.toolbarImages.Images.SetKeyName(9, "");
            this.toolbarImages.Images.SetKeyName(10, "");
            this.toolbarImages.Images.SetKeyName(11, "");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddMapButton,
            this.RemoveMapButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddMapButton
            // 
            this.AddMapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddMapButton, "AddMapButton");
            this.AddMapButton.Name = "AddMapButton";
            this.AddMapButton.Click += new System.EventHandler(this.AddMapButton_Click);
            // 
            // RemoveMapButton
            // 
            this.RemoveMapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveMapButton, "RemoveMapButton");
            this.RemoveMapButton.Name = "RemoveMapButton";
            this.RemoveMapButton.Click += new System.EventHandler(this.RemoveMapButton_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.splitter2);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.flyoutEntry);
            this.panel3.Controls.Add(this.widgetEntry);
            this.panel3.Controls.Add(this.containerEditor);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // flyoutEntry
            // 
            resources.ApplyResources(this.flyoutEntry, "flyoutEntry");
            this.flyoutEntry.Name = "flyoutEntry";
            // 
            // widgetEntry
            // 
            resources.ApplyResources(this.widgetEntry, "widgetEntry");
            this.widgetEntry.Name = "widgetEntry";
            // 
            // containerEditor
            // 
            resources.ApplyResources(this.containerEditor, "containerEditor");
            this.containerEditor.Name = "containerEditor";
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.WidgetTree);
            this.panel4.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // WidgetTree
            // 
            resources.ApplyResources(this.WidgetTree, "WidgetTree");
            this.WidgetTree.ImageList = this.toolbarImages;
            this.WidgetTree.Name = "WidgetTree";
            this.WidgetTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.WidgetTree_AfterSelect);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddWidgetButton,
            this.AddContainerButton,
            this.RemoveWidgetButton,
            this.toolStripSeparator1,
            this.MoveWidgetUpButton,
            this.MoveWidgetDownButton,
            this.toolStripSeparator2,
            this.ConfigureWidgetsButton});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddWidgetButton
            // 
            this.AddWidgetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddWidgetButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddWidgetSeperatorMenu,
            this.AddWidgetSubmenuMenu,
            this.AddWidgetEntryMenu});
            resources.ApplyResources(this.AddWidgetButton, "AddWidgetButton");
            this.AddWidgetButton.Name = "AddWidgetButton";
            // 
            // AddWidgetSeperatorMenu
            // 
            this.AddWidgetSeperatorMenu.Name = "AddWidgetSeperatorMenu";
            resources.ApplyResources(this.AddWidgetSeperatorMenu, "AddWidgetSeperatorMenu");
            this.AddWidgetSeperatorMenu.Click += new System.EventHandler(this.seperatorToolStripMenuItem_Click);
            // 
            // AddWidgetSubmenuMenu
            // 
            this.AddWidgetSubmenuMenu.Name = "AddWidgetSubmenuMenu";
            resources.ApplyResources(this.AddWidgetSubmenuMenu, "AddWidgetSubmenuMenu");
            this.AddWidgetSubmenuMenu.Click += new System.EventHandler(this.submenuToolStripMenuItem_Click);
            // 
            // AddWidgetEntryMenu
            // 
            this.AddWidgetEntryMenu.Name = "AddWidgetEntryMenu";
            resources.ApplyResources(this.AddWidgetEntryMenu, "AddWidgetEntryMenu");
            this.AddWidgetEntryMenu.Click += new System.EventHandler(this.widgetToolStripMenuItem_Click);
            // 
            // AddContainerButton
            // 
            this.AddContainerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddContainerButton, "AddContainerButton");
            this.AddContainerButton.Name = "AddContainerButton";
            this.AddContainerButton.Click += new System.EventHandler(this.AddContainerButton_Click);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ConfigureWidgetsButton
            // 
            this.ConfigureWidgetsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ConfigureWidgetsButton, "ConfigureWidgetsButton");
            this.ConfigureWidgetsButton.Name = "ConfigureWidgetsButton";
            this.ConfigureWidgetsButton.Click += new System.EventHandler(this.ConfigureWidgetsButton_Click);
            // 
            // SelectTemplateBtn
            // 
            resources.ApplyResources(this.SelectTemplateBtn, "SelectTemplateBtn");
            this.SelectTemplateBtn.Name = "SelectTemplateBtn";
            this.SelectTemplateBtn.Click += new System.EventHandler(this.SelectTemplateBtn_Click);
            // 
            // ShowInBrowser
            // 
            resources.ApplyResources(this.ShowInBrowser, "ShowInBrowser");
            this.ShowInBrowser.Name = "ShowInBrowser";
            this.ShowInBrowser.Click += new System.EventHandler(this.ShowInBrowser_Click);
            // 
            // browserURL
            // 
            resources.ApplyResources(this.browserURL, "browserURL");
            this.browserURL.Name = "browserURL";
            this.browserURL.ReadOnly = true;
            // 
            // label12
            // 
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // ApplicationDefinitionEditor
            // 
            resources.ApplyResources(this, "$this");
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
            this.Load += new System.EventHandler(this.ApplicationDefinitionEditor_Load);
            this.MapGroup.ResumeLayout(false);
            this.MapPropertiesPanel.ResumeLayout(false);
            this.MapPropertiesPanel.PerformLayout();
            this.overriddenMapExtents.ResumeLayout(false);
            this.overriddenMapExtents.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
					throw new Exception (Strings.ApplicationDefinitionEditor.MalformedURLError);

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
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
				MessageBox.Show(this, String.Format(Strings.ApplicationDefinitionEditor.BrowserLaunchError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		
		}

        private void AddMapButton_Click(object sender, EventArgs e)
        {
            MapGroupType mgr = new MapGroupType();
            mgr.id = Strings.ApplicationDefinitionEditor.NewMapName;
            mgr.InitialView = null;
            mgr.Map = new MapTypeCollection();
            OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType mt = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType();
            mt.SingleTile = "true";
            mt.Type = "MapGuide";
            mt.Extension = new CustomContentType();
            mt.Extension.Any = new System.Xml.XmlElement[1];
            mt.Extension.Any[0] = m_appDef.ApplicationDocument.CreateElement("ResourceId");
            mgr.Map.Add(mt);
            m_appDef.MapSet.Add(mgr);
            m_editor.HasChanged();
            UpdateDisplay();
            MapList.Items[MapList.Items.Count - 1].Selected = true;
        }

        private void RemoveMapButton_Click(object sender, EventArgs e)
        {
            if (MapList.SelectedItems.Count == 0 || MapList.SelectedItems[0].Tag as MapGroupType == null)
                return;

            for (int i = 0; i < m_appDef.MapSet.Count; i++)
                if (m_appDef.MapSet[i] == MapList.SelectedItems[0].Tag)
                {
                    m_appDef.MapSet.RemoveAt(i);
                    m_editor.HasChanged();
                    UpdateDisplay();
                    MapList_SelectedIndexChanged(sender, new EventArgs());
                    break;
                }
        }

        private void AddContainerButton_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType c = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType();
            c.Name = Strings.ApplicationDefinitionEditor.NewContainerName;
            c.Item = new UiItemTypeCollection();
            m_appDef.WidgetSet[0].Container.Add(c);
            UpdateDisplay();
            WidgetTree.SelectedNode = WidgetTree.Nodes[WidgetTree.Nodes.Count - 1];
            WidgetTree.SelectedNode.EnsureVisible();
        }

        private void RemoveWidgetButton_Click(object sender, EventArgs e)
        {
            if (WidgetTree.SelectedNode == null)
                return;

            if (WidgetTree.SelectedNode.Parent == null)
            {
                foreach (WidgetSetType wst in m_appDef.WidgetSet)
                    for (int i = 0; i < wst.Container.Count; i++)
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
                    for (int i = 0; i < col.Count; i++)
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

        private void MoveWidgetUpButton_Click(object sender, EventArgs e)
        {
            MoveWidgetUpOrDown(true);
        }

        private void MoveWidgetDownButton_Click(object sender, EventArgs e)
        {
            MoveWidgetUpOrDown(false);
        }

        private void ConfigureWidgetsButton_Click(object sender, EventArgs e)
        {
            EditWidgets dlg = new EditWidgets();
            dlg.SetupDialog(m_editor.CurrentConnection.GetApplicationWidgets(), m_appDef);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                UpdateDisplay();
                m_editor.HasChanged();
            }
        }

        private void MoveWidgetUpOrDown(bool moveUp)
        {
            if (WidgetTree.SelectedNode == null)
                return;

            if (WidgetTree.SelectedNode.Parent == null)
            {
                ContainerTypeCollection col = null;
                foreach (WidgetSetType wst in m_appDef.WidgetSet)
                    if (wst.Container.Contains((OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType)WidgetTree.SelectedNode.Tag))
                    {
                        col = wst.Container;
                        break;
                    }

                if (col != null)
                {
                    OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType item = WidgetTree.SelectedNode.Tag as OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.UiItemContainerType;
                    int index = col.IndexOf(item);

                    if (moveUp)
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
                        catch { }
                    }

                    foreach (TreeNode n in WidgetTree.Nodes)
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
                    if (moveUp)
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
                        catch { }
                    }
                    foreach (TreeNode n in b.Nodes)
                        if (n.Tag == item)
                        {
                            WidgetTree.SelectedNode = n;
                            break;
                        }
                }
            }
        }

        private void seperatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.SeparatorItemType sep = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.SeparatorItemType();
            sep.Function = UiItemFunctionType.Separator;
            AddItemToWidgetTree(sep);
        }

        private void submenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType flv = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.FlyoutItemType();
            flv.Item = new UiItemTypeCollection();
            flv.Label = Strings.ApplicationDefinitionEditor.NewSubMenuName;
            flv.Function = UiItemFunctionType.Flyout;
            AddItemToWidgetTree(flv);
        }

        private void widgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetItemType w = new OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.WidgetItemType();
            w.Function = UiItemFunctionType.Widget;
            w.Widget = "About";
            AddItemToWidgetTree(w);
        }

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return m_editor.CurrentConnection.SupportsResourcePreviews; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
