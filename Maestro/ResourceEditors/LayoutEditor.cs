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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for LayoutEditor.
	/// </summary>
	public class LayoutEditor : System.Windows.Forms.UserControl, IResourceEditorControl
	{
		private static Hashtable LoadedImages = null; 
		public static ImageList LoadedImageList = null;
		
		public Hashtable BuiltInCommands = null;
		private Hashtable m_advancedTypes = null;

        private const int BLANK_IMAGE = 0;
        private const int FOLDER_IMAGE = 1;
        private const int SEPARATOR_IMAGE = 2;

		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;

		private bool m_isUpdating = false;
		private System.Windows.Forms.GroupBox overriddenMapExtents;
		private System.Windows.Forms.TextBox overrideScale;
		private System.Windows.Forms.TextBox overrideY;
		private System.Windows.Forms.TextBox overrideX;
		private System.Windows.Forms.CheckBox OverrideDisplayExtents;
		private System.Windows.Forms.TextBox LeftPaneWidth;
		private System.Windows.Forms.CheckBox ItemPropertiesCheck;
		private System.Windows.Forms.CheckBox LayerControlCheck;
		private System.Windows.Forms.CheckBox ZoomControlCheck;
		private System.Windows.Forms.CheckBox StatusBarCheck;
		private System.Windows.Forms.CheckBox ContextMenuCheck;
		private System.Windows.Forms.CheckBox ToolbarCheck;
		private System.Windows.Forms.Button EditTaskBarBtn;
		private System.Windows.Forms.TextBox RightPaneWidth;
		private System.Windows.Forms.CheckBox TaskBarCheck;
		private System.Windows.Forms.CheckBox TaskPaneCheck;
		private System.Windows.Forms.TextBox TitleText;
		private System.Windows.Forms.TextBox MapResource;
		private System.Windows.Forms.TextBox HomePageURL;
		private System.Windows.Forms.TextBox FeatureLinkTarget;
		private System.Windows.Forms.ComboBox FeatureLinkTargetType;
        private System.Windows.Forms.Button SelectMapButton;
		private System.Data.DataSet CommandTypesDataset;
		private System.Data.DataTable CommandTable;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Data.DataColumn dataColumn5;
		private System.Data.DataColumn dataColumn6;
		private System.Data.DataColumn dataColumn7;
		private System.Data.DataColumn dataColumn8;
		private System.Data.DataColumn dataColumn9;
		private System.Windows.Forms.GroupBox MenuBox;
		private System.Windows.Forms.TabPage ToolbarTab;
		private System.Windows.Forms.TabPage ContextMenuTab;
        private System.Windows.Forms.TabPage TaskFrameTab;
		private System.Windows.Forms.TreeView ToolbarTree;
        private System.Windows.Forms.TreeView ContextTree;
		private System.Windows.Forms.TreeView TaskTree;
		private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TabControl MenuTabs;
		private System.ComponentModel.IContainer components;
		private ResourceEditors.LayoutControls.CommandEditor commandEditor;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button ShowInBrowser;
		private System.Windows.Forms.TextBox browserURL;
        private ToolStrip ToolbarToolstrip;
        private ToolStripDropDownButton ToolbarAddButton;
        private ToolStripButton ToolbarDeleteButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolbarUpButton;
        private ToolStripButton ToolbarDownButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStrip ContextToolstrip;
        private ToolStripDropDownButton ContextAddButton;
        private ToolStripButton ContextDeleteButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ContextUpButton;
        private ToolStripButton ContextDownButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStrip TaskToolstrip;
        private ToolStripDropDownButton TaskAddButton;
        private ToolStripButton TaskDeleteButton;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton TaskUpButton;
        private ToolStripButton TaskDownButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripDropDownButton ToolbarCreateButton;
        private ToolStripDropDownButton ContextCreateButton;
        private ToolStripDropDownButton TaskCreateButton;
        private ContextMenuStrip AddItemMenu;
        private ToolStripMenuItem AddBuiltInFunctionMenu;
        private ToolStripMenuItem AddCustomItemMenu;
        private ToolStripMenuItem AddSubMenuItem;
        private ToolStripMenuItem AddSeperatorItem;
        private ContextMenuStrip CreateCommandMenu;
        private ImageList FixedImages;
		private string m_tempResource;

		private enum ListViewColumns : int
		{
			Command,
			Label,
			Tooltip,
			Action,
			Viewers,
			Type
		};


		public LayoutEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_layout = new OSGeo.MapGuide.MaestroAPI.WebLayout();
            m_tempResource = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, m_editor.CurrentConnection.SessionID);
			UpdateDisplay();
		}

		public LayoutEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_layout = editor.CurrentConnection.GetWebLayout(resourceID);
            m_tempResource = new MaestroAPI.ResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, m_editor.CurrentConnection.SessionID);
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (m_isUpdating)
				return;

			try
			{
				m_isUpdating = true;

				if (m_layout.Map == null)
					m_layout.Map = new OSGeo.MapGuide.MaestroAPI.MapType();

				if (m_layout.InformationPane == null)
					m_layout.InformationPane = new OSGeo.MapGuide.MaestroAPI.InformationPaneType();

				if (m_layout.ContextMenu == null)
					m_layout.ContextMenu = new OSGeo.MapGuide.MaestroAPI.ContextMenuType();

				if (m_layout.ToolBar == null)
					m_layout.ToolBar = new OSGeo.MapGuide.MaestroAPI.ToolBarType();

				if (m_layout.StatusBar == null)
					m_layout.StatusBar = new OSGeo.MapGuide.MaestroAPI.StatusBarType();

				if (m_layout.ZoomControl == null)
					m_layout.ZoomControl = new OSGeo.MapGuide.MaestroAPI.ZoomControlType();

				if (m_layout.TaskPane == null)
					m_layout.TaskPane = new OSGeo.MapGuide.MaestroAPI.TaskPaneType();

				if (m_layout.TaskPane.TaskBar == null)
					m_layout.TaskPane.TaskBar = new OSGeo.MapGuide.MaestroAPI.TaskBarType();

				TitleText.Text = m_layout.Title;

				MapResource.Text = m_layout.Map.ResourceId;
				OverrideDisplayExtents.Checked = m_layout.Map.InitialView != null;
				if (m_layout.Map.InitialView != null)
				{
					overrideX.Text = m_layout.Map.InitialView.CenterX.ToString();
					overrideY.Text = m_layout.Map.InitialView.CenterY.ToString();
					overrideScale.Text = m_layout.Map.InitialView.Scale.ToString();
				}
				else
				{
					overrideX.Text = overrideY.Text = overrideScale.Text = "";
				}

				LayerControlCheck.Checked = m_layout.InformationPane.LegendVisible && m_layout.InformationPane.Visible;
				ItemPropertiesCheck.Checked = m_layout.InformationPane.PropertiesVisible && m_layout.InformationPane.Visible;
				LeftPaneWidth.Text = m_layout.InformationPane.Width.ToString();
				LeftPaneWidth.Enabled = m_layout.InformationPane.Visible;

				ToolbarCheck.Checked = m_layout.ToolBar.Visible;
				ContextMenuCheck.Checked = m_layout.ContextMenu.Visible;
				StatusBarCheck.Checked = m_layout.StatusBar.Visible;
				ZoomControlCheck.Checked = m_layout.ZoomControl.Visible;

				TaskPaneCheck.Checked = m_layout.TaskPane.Visible;
				TaskBarCheck.Checked = m_layout.TaskPane.TaskBar.Visible && m_layout.TaskPane.Visible;
				EditTaskBarBtn.Enabled = RightPaneWidth.Enabled = m_layout.TaskPane.Visible;
				RightPaneWidth.Text = m_layout.TaskPane.Width.ToString();

				HomePageURL.Text = m_layout.TaskPane.InitialTask;
				FeatureLinkTargetType.SelectedItem = m_layout.Map.HyperlinkTarget;
				FeatureLinkTarget.Text = m_layout.Map.HyperlinkTargetFrame;

				ToolbarTree.Nodes.Clear();
				ContextTree.Nodes.Clear();
				TaskTree.Nodes.Clear();

				BuildTree(ToolbarTree, m_layout.ToolBar.Button);
				BuildTree(ContextTree, m_layout.ContextMenu.MenuItem);
				BuildTree(TaskTree, m_layout.TaskPane.TaskBar.MenuButton);

				RebuildCommandSetMenus();

				ToolbarAddButton.DropDown =
                    ContextAddButton.DropDown =
                    TaskAddButton.DropDown = AddItemMenu;

				if (m_editor.Existing)
					browserURL.Text = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL + "mapviewerajax/?WEBLAYOUT=" + System.Web.HttpUtility.UrlEncode(m_layout.ResourceId);
				else
					browserURL.Text = "";

			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void RebuildCommandSetMenus()
		{
			CreateCommandMenu.Items.Clear();
			AddBuiltInFunctionMenu.DropDown.Items.Clear();
            AddCustomItemMenu.DropDown.Items.Clear();

			if (m_layout.CommandSet != null)
			{
				foreach(OSGeo.MapGuide.MaestroAPI.CommandType i in m_layout.CommandSet)
					if (!BuiltInCommands.ContainsKey(i.Name) && i.Name != "Invoke Script" && i.Name != "Invoke URL" && i.Name != "Search")
						AddCustomItemMenu.DropDown.Items.Add(new ToolStripMenuItem(i.Name, LoadedImageList.Images[FindImageIndex(i.ImageURL)], new System.EventHandler(AddCustomItem_Click)));
			}

			SortedList sl = new SortedList(BuiltInCommands);

			foreach(string key in sl.Keys)
			{
				DataRow dr = (DataRow)BuiltInCommands[key];
                CreateCommandMenu.Items.Add(new ToolStripMenuItem(dr["Command"].ToString(), null, new System.EventHandler(CreateCommand_Click)));

				if (key != "Invoke Script" && key != "Invoke URL" && key != "Search")
                    AddBuiltInFunctionMenu.DropDown.Items.Add(new ToolStripMenuItem(dr["Command"].ToString(), LoadedImageList.Images[FindImageIndex(dr["EnabledIcon"].ToString())], new System.EventHandler(AddBuiltInItem_Click)));
			}

            ToolbarCreateButton.DropDown = CreateCommandMenu;
            ContextCreateButton.DropDown = CreateCommandMenu;
            TaskCreateButton.DropDown = CreateCommandMenu;

			//Re-register to avoid annyoing .Net context menu problem
            //TODO: Figure out if this is a problem in .Net 2.0
			int pos = AddItemMenu.Items.IndexOf(AddBuiltInFunctionMenu);
            AddItemMenu.Items.RemoveAt(pos);
            AddItemMenu.Items.Insert(pos, AddBuiltInFunctionMenu);

            pos = AddItemMenu.Items.IndexOf(AddCustomItemMenu);
            AddItemMenu.Items.RemoveAt(pos);
            AddItemMenu.Items.Insert(pos, AddCustomItemMenu);
		}

		private void BuildTree(TreeView tree, OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection items)
		{
			try
			{
				string prevpath = null;
				int previndex = -1;

                tree.ImageList = LoadedImageList;

				if (tree.SelectedNode != null)
				{
					prevpath = tree.SelectedNode.FullPath;
					if (tree.SelectedNode.Parent == null)
						previndex = tree.Nodes.IndexOf(tree.SelectedNode);
					else
						previndex = tree.SelectedNode.Parent.Nodes.IndexOf(tree.SelectedNode);
				}

				tree.BeginUpdate();
				tree.Nodes.Clear();
				if (items != null)
					BuildTree(tree.Nodes, items);

				//Reselect the node closest to the node
				if (prevpath != null)
				{
					string[] parts = prevpath.Split(tree.PathSeparator[0]);
					int i = 0;

					TreeNodeCollection col = tree.Nodes;
					TreeNode p = null;

					for(i = 0; i < parts.Length; i++)
					{
						bool found = false;
						foreach(TreeNode n in col)
							if (n.Text == parts[i])
							{
								col = n.Nodes;
								p = n;
								found = true;
								break;
							}

						if (!found)
							break;
					}

					if (i == parts.Length - 1)
					{
						if (col.Count > 0)
							tree.SelectedNode = col[Math.Min(Math.Max(0, previndex), col.Count - 1)];
						else if (p != null)
							tree.SelectedNode = p;
					}
					else if (p != null)
						tree.SelectedNode = p;
						
				}
			}
			finally
			{
				tree.EndUpdate();
			}
		}

		private void BuildTree(TreeNodeCollection parent, OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection items)
		{
            if (items == null)
                return;

			foreach(OSGeo.MapGuide.MaestroAPI.UIItemType button in items)
			{
				if (button as OSGeo.MapGuide.MaestroAPI.CommandItemType != null)
				{
					OSGeo.MapGuide.MaestroAPI.CommandItemType cmd = (OSGeo.MapGuide.MaestroAPI.CommandItemType)button;
					TreeNode n = new TreeNode(cmd.Command);
					n.Tag = button;
                    n.ImageIndex = n.SelectedImageIndex = BLANK_IMAGE;

                    foreach(OSGeo.MapGuide.MaestroAPI.CommandType ct in m_layout.CommandSet)
                        if (ct.Name == cmd.Command)
                        {
                            n.ImageIndex = n.SelectedImageIndex = FindImageIndex(ct.ImageURL);
                            break;
                        }

					parent.Add(n);
				}
				else if (button as OSGeo.MapGuide.MaestroAPI.SeparatorItemType != null)
				{
					OSGeo.MapGuide.MaestroAPI.SeparatorItemType cmd = (OSGeo.MapGuide.MaestroAPI.SeparatorItemType)button;
					TreeNode n = new TreeNode(Strings.LayoutEditor.SeparatorName);
					n.Tag = button;
                    n.ImageIndex = n.SelectedImageIndex = SEPARATOR_IMAGE;
					parent.Add(n);
				}
				else if (button as OSGeo.MapGuide.MaestroAPI.FlyoutItemType!= null)
				{
					OSGeo.MapGuide.MaestroAPI.FlyoutItemType cmd = (OSGeo.MapGuide.MaestroAPI.FlyoutItemType)button;
					TreeNode n = new TreeNode(cmd.Label);
					n.Tag = button;
					parent.Add(n);
                    n.ImageIndex = n.SelectedImageIndex = FOLDER_IMAGE;
					BuildTree(n.Nodes, cmd.SubItem);
				}
			}
		}

		protected LayoutEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			FeatureLinkTargetType.Items.Clear();
			foreach(object o in Enum.GetValues(typeof(OSGeo.MapGuide.MaestroAPI.TargetType)))
				FeatureLinkTargetType.Items.Add(o);
			LoadMenuItems();

			m_advancedTypes = new Hashtable();
			m_advancedTypes.Add("Get Printable Page", typeof(OSGeo.MapGuide.MaestroAPI.GetPrintablePageCommandType));
			m_advancedTypes.Add("View Options", typeof(OSGeo.MapGuide.MaestroAPI.ViewOptionsCommandType));
			m_advancedTypes.Add("Help", typeof(OSGeo.MapGuide.MaestroAPI.HelpCommandType));
			m_advancedTypes.Add("Print", typeof(OSGeo.MapGuide.MaestroAPI.PrintCommandType));
			m_advancedTypes.Add("Buffer", typeof(OSGeo.MapGuide.MaestroAPI.BufferCommandType));
			m_advancedTypes.Add("Measure", typeof(OSGeo.MapGuide.MaestroAPI.MeasureCommandType));
			m_advancedTypes.Add("Select Within", typeof(OSGeo.MapGuide.MaestroAPI.SelectWithinCommandType));
			m_advancedTypes.Add("Invoke Script", typeof(OSGeo.MapGuide.MaestroAPI.InvokeScriptCommandType));
			m_advancedTypes.Add("Invoke URL", typeof(OSGeo.MapGuide.MaestroAPI.InvokeURLCommandType));
			m_advancedTypes.Add("Search", typeof(OSGeo.MapGuide.MaestroAPI.SearchCommandType));
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.overriddenMapExtents = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.overrideScale = new System.Windows.Forms.TextBox();
            this.overrideY = new System.Windows.Forms.TextBox();
            this.overrideX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OverrideDisplayExtents = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LeftPaneWidth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ItemPropertiesCheck = new System.Windows.Forms.CheckBox();
            this.LayerControlCheck = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ZoomControlCheck = new System.Windows.Forms.CheckBox();
            this.StatusBarCheck = new System.Windows.Forms.CheckBox();
            this.ContextMenuCheck = new System.Windows.Forms.CheckBox();
            this.ToolbarCheck = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.EditTaskBarBtn = new System.Windows.Forms.Button();
            this.RightPaneWidth = new System.Windows.Forms.TextBox();
            this.TaskBarCheck = new System.Windows.Forms.CheckBox();
            this.TaskPaneCheck = new System.Windows.Forms.CheckBox();
            this.TitleText = new System.Windows.Forms.TextBox();
            this.MapResource = new System.Windows.Forms.TextBox();
            this.SelectMapButton = new System.Windows.Forms.Button();
            this.FeatureLinkTarget = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.HomePageURL = new System.Windows.Forms.TextBox();
            this.FeatureLinkTargetType = new System.Windows.Forms.ComboBox();
            this.CommandTypesDataset = new System.Data.DataSet();
            this.CommandTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.MenuBox = new System.Windows.Forms.GroupBox();
            this.commandEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls.CommandEditor();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.MenuTabs = new System.Windows.Forms.TabControl();
            this.ToolbarTab = new System.Windows.Forms.TabPage();
            this.ToolbarTree = new System.Windows.Forms.TreeView();
            this.ToolbarToolstrip = new System.Windows.Forms.ToolStrip();
            this.ToolbarAddButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolbarDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolbarUpButton = new System.Windows.Forms.ToolStripButton();
            this.ToolbarDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolbarCreateButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ContextMenuTab = new System.Windows.Forms.TabPage();
            this.ContextTree = new System.Windows.Forms.TreeView();
            this.ContextToolstrip = new System.Windows.Forms.ToolStrip();
            this.ContextAddButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ContextDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextUpButton = new System.Windows.Forms.ToolStripButton();
            this.ContextDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextCreateButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.TaskFrameTab = new System.Windows.Forms.TabPage();
            this.TaskTree = new System.Windows.Forms.TreeView();
            this.TaskToolstrip = new System.Windows.Forms.ToolStrip();
            this.TaskAddButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.TaskDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskUpButton = new System.Windows.Forms.ToolStripButton();
            this.TaskDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskCreateButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.browserURL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ShowInBrowser = new System.Windows.Forms.Button();
            this.AddItemMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddBuiltInFunctionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddCustomItemMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddSubMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddSeperatorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateCommandMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FixedImages = new System.Windows.Forms.ImageList(this.components);
            this.overriddenMapExtents.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandTypesDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommandTable)).BeginInit();
            this.MenuBox.SuspendLayout();
            this.MenuTabs.SuspendLayout();
            this.ToolbarTab.SuspendLayout();
            this.ToolbarToolstrip.SuspendLayout();
            this.ContextMenuTab.SuspendLayout();
            this.ContextToolstrip.SuspendLayout();
            this.TaskFrameTab.SuspendLayout();
            this.TaskToolstrip.SuspendLayout();
            this.AddItemMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // overriddenMapExtents
            // 
            resources.ApplyResources(this.overriddenMapExtents, "overriddenMapExtents");
            this.overriddenMapExtents.Controls.Add(this.label6);
            this.overriddenMapExtents.Controls.Add(this.overrideScale);
            this.overriddenMapExtents.Controls.Add(this.overrideY);
            this.overriddenMapExtents.Controls.Add(this.overrideX);
            this.overriddenMapExtents.Controls.Add(this.label5);
            this.overriddenMapExtents.Controls.Add(this.label4);
            this.overriddenMapExtents.Controls.Add(this.label3);
            this.overriddenMapExtents.Name = "overriddenMapExtents";
            this.overriddenMapExtents.TabStop = false;
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
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // OverrideDisplayExtents
            // 
            resources.ApplyResources(this.OverrideDisplayExtents, "OverrideDisplayExtents");
            this.OverrideDisplayExtents.Name = "OverrideDisplayExtents";
            this.OverrideDisplayExtents.CheckedChanged += new System.EventHandler(this.OverrideDisplayExtents_CheckedChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LeftPaneWidth);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ItemPropertiesCheck);
            this.groupBox3.Controls.Add(this.LayerControlCheck);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // LeftPaneWidth
            // 
            resources.ApplyResources(this.LeftPaneWidth, "LeftPaneWidth");
            this.LeftPaneWidth.Name = "LeftPaneWidth";
            this.LeftPaneWidth.TextChanged += new System.EventHandler(this.LeftPaneWidth_TextChanged);
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // ItemPropertiesCheck
            // 
            resources.ApplyResources(this.ItemPropertiesCheck, "ItemPropertiesCheck");
            this.ItemPropertiesCheck.Name = "ItemPropertiesCheck";
            this.ItemPropertiesCheck.CheckedChanged += new System.EventHandler(this.ItemPropertiesCheck_CheckedChanged);
            // 
            // LayerControlCheck
            // 
            resources.ApplyResources(this.LayerControlCheck, "LayerControlCheck");
            this.LayerControlCheck.Name = "LayerControlCheck";
            this.LayerControlCheck.CheckedChanged += new System.EventHandler(this.LayerControlCheck_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.ZoomControlCheck);
            this.groupBox4.Controls.Add(this.StatusBarCheck);
            this.groupBox4.Controls.Add(this.ContextMenuCheck);
            this.groupBox4.Controls.Add(this.ToolbarCheck);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // ZoomControlCheck
            // 
            resources.ApplyResources(this.ZoomControlCheck, "ZoomControlCheck");
            this.ZoomControlCheck.Name = "ZoomControlCheck";
            this.ZoomControlCheck.CheckedChanged += new System.EventHandler(this.ZoomControlCheck_CheckedChanged);
            // 
            // StatusBarCheck
            // 
            resources.ApplyResources(this.StatusBarCheck, "StatusBarCheck");
            this.StatusBarCheck.Name = "StatusBarCheck";
            this.StatusBarCheck.CheckedChanged += new System.EventHandler(this.StatusBarCheck_CheckedChanged);
            // 
            // ContextMenuCheck
            // 
            resources.ApplyResources(this.ContextMenuCheck, "ContextMenuCheck");
            this.ContextMenuCheck.Name = "ContextMenuCheck";
            this.ContextMenuCheck.CheckedChanged += new System.EventHandler(this.ContextMenuCheck_CheckedChanged);
            // 
            // ToolbarCheck
            // 
            resources.ApplyResources(this.ToolbarCheck, "ToolbarCheck");
            this.ToolbarCheck.Name = "ToolbarCheck";
            this.ToolbarCheck.CheckedChanged += new System.EventHandler(this.ToolbarCheck_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.EditTaskBarBtn);
            this.groupBox5.Controls.Add(this.RightPaneWidth);
            this.groupBox5.Controls.Add(this.TaskBarCheck);
            this.groupBox5.Controls.Add(this.TaskPaneCheck);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // EditTaskBarBtn
            // 
            resources.ApplyResources(this.EditTaskBarBtn, "EditTaskBarBtn");
            this.EditTaskBarBtn.Name = "EditTaskBarBtn";
            this.EditTaskBarBtn.Click += new System.EventHandler(this.EditTaskBarBtn_Click);
            // 
            // RightPaneWidth
            // 
            resources.ApplyResources(this.RightPaneWidth, "RightPaneWidth");
            this.RightPaneWidth.Name = "RightPaneWidth";
            this.RightPaneWidth.TextChanged += new System.EventHandler(this.RightPaneWidth_TextChanged);
            // 
            // TaskBarCheck
            // 
            resources.ApplyResources(this.TaskBarCheck, "TaskBarCheck");
            this.TaskBarCheck.Name = "TaskBarCheck";
            this.TaskBarCheck.CheckedChanged += new System.EventHandler(this.TaskBarCheck_CheckedChanged);
            // 
            // TaskPaneCheck
            // 
            resources.ApplyResources(this.TaskPaneCheck, "TaskPaneCheck");
            this.TaskPaneCheck.Name = "TaskPaneCheck";
            this.TaskPaneCheck.CheckedChanged += new System.EventHandler(this.TaskPaneCheck_CheckedChanged);
            // 
            // TitleText
            // 
            resources.ApplyResources(this.TitleText, "TitleText");
            this.TitleText.Name = "TitleText";
            this.TitleText.TextChanged += new System.EventHandler(this.TitleText_TextChanged);
            // 
            // MapResource
            // 
            resources.ApplyResources(this.MapResource, "MapResource");
            this.MapResource.Name = "MapResource";
            this.MapResource.ReadOnly = true;
            // 
            // SelectMapButton
            // 
            resources.ApplyResources(this.SelectMapButton, "SelectMapButton");
            this.SelectMapButton.Name = "SelectMapButton";
            this.SelectMapButton.Click += new System.EventHandler(this.SelectMapButton_Click);
            // 
            // FeatureLinkTarget
            // 
            resources.ApplyResources(this.FeatureLinkTarget, "FeatureLinkTarget");
            this.FeatureLinkTarget.Name = "FeatureLinkTarget";
            this.FeatureLinkTarget.TextChanged += new System.EventHandler(this.FeatureLinkTarget_TextChanged);
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // HomePageURL
            // 
            resources.ApplyResources(this.HomePageURL, "HomePageURL");
            this.HomePageURL.Name = "HomePageURL";
            this.HomePageURL.TextChanged += new System.EventHandler(this.HomePageURL_TextChanged);
            // 
            // FeatureLinkTargetType
            // 
            this.FeatureLinkTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FeatureLinkTargetType.Items.AddRange(new object[] {
            resources.GetString("FeatureLinkTargetType.Items"),
            resources.GetString("FeatureLinkTargetType.Items1"),
            resources.GetString("FeatureLinkTargetType.Items2")});
            resources.ApplyResources(this.FeatureLinkTargetType, "FeatureLinkTargetType");
            this.FeatureLinkTargetType.Name = "FeatureLinkTargetType";
            this.FeatureLinkTargetType.SelectedIndexChanged += new System.EventHandler(this.FeatureLinkTargetType_SelectedIndexChanged);
            // 
            // CommandTypesDataset
            // 
            this.CommandTypesDataset.DataSetName = "NewDataSet";
            this.CommandTypesDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.CommandTypesDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.CommandTable});
            // 
            // CommandTable
            // 
            this.CommandTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9});
            this.CommandTable.TableName = "Command";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "Command";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "Label";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "ToolTip";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "Action";
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "Description";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "Viewers";
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "Type";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "EnabledIcon";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "DisabledIcon";
            // 
            // MenuBox
            // 
            resources.ApplyResources(this.MenuBox, "MenuBox");
            this.MenuBox.Controls.Add(this.commandEditor);
            this.MenuBox.Controls.Add(this.splitter2);
            this.MenuBox.Controls.Add(this.MenuTabs);
            this.MenuBox.Name = "MenuBox";
            this.MenuBox.TabStop = false;
            // 
            // commandEditor
            // 
            resources.ApplyResources(this.commandEditor, "commandEditor");
            this.commandEditor.Name = "commandEditor";
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // MenuTabs
            // 
            this.MenuTabs.Controls.Add(this.ToolbarTab);
            this.MenuTabs.Controls.Add(this.ContextMenuTab);
            this.MenuTabs.Controls.Add(this.TaskFrameTab);
            resources.ApplyResources(this.MenuTabs, "MenuTabs");
            this.MenuTabs.Name = "MenuTabs";
            this.MenuTabs.SelectedIndex = 0;
            this.MenuTabs.SelectedIndexChanged += new System.EventHandler(this.MenuTabs_SelectedIndexChanged);
            // 
            // ToolbarTab
            // 
            this.ToolbarTab.Controls.Add(this.ToolbarTree);
            this.ToolbarTab.Controls.Add(this.ToolbarToolstrip);
            resources.ApplyResources(this.ToolbarTab, "ToolbarTab");
            this.ToolbarTab.Name = "ToolbarTab";
            // 
            // ToolbarTree
            // 
            this.ToolbarTree.AllowDrop = true;
            resources.ApplyResources(this.ToolbarTree, "ToolbarTree");
            this.ToolbarTree.HideSelection = false;
            this.ToolbarTree.Name = "ToolbarTree";
            this.ToolbarTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragDrop);
            this.ToolbarTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemTree_AfterSelect);
            this.ToolbarTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragEnter);
            this.ToolbarTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ItemTree_ItemDrag);
            this.ToolbarTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragOver);
            // 
            // ToolbarToolstrip
            // 
            this.ToolbarToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolbarToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolbarAddButton,
            this.ToolbarDeleteButton,
            this.toolStripSeparator1,
            this.ToolbarUpButton,
            this.ToolbarDownButton,
            this.toolStripSeparator2,
            this.ToolbarCreateButton});
            resources.ApplyResources(this.ToolbarToolstrip, "ToolbarToolstrip");
            this.ToolbarToolstrip.Name = "ToolbarToolstrip";
            this.ToolbarToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // ToolbarAddButton
            // 
            this.ToolbarAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ToolbarAddButton, "ToolbarAddButton");
            this.ToolbarAddButton.Name = "ToolbarAddButton";
            // 
            // ToolbarDeleteButton
            // 
            this.ToolbarDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ToolbarDeleteButton, "ToolbarDeleteButton");
            this.ToolbarDeleteButton.Name = "ToolbarDeleteButton";
            this.ToolbarDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ToolbarUpButton
            // 
            this.ToolbarUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ToolbarUpButton, "ToolbarUpButton");
            this.ToolbarUpButton.Name = "ToolbarUpButton";
            this.ToolbarUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ToolbarDownButton
            // 
            this.ToolbarDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ToolbarDownButton, "ToolbarDownButton");
            this.ToolbarDownButton.Name = "ToolbarDownButton";
            this.ToolbarDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ToolbarCreateButton
            // 
            this.ToolbarCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ToolbarCreateButton, "ToolbarCreateButton");
            this.ToolbarCreateButton.Name = "ToolbarCreateButton";
            // 
            // ContextMenuTab
            // 
            this.ContextMenuTab.Controls.Add(this.ContextTree);
            this.ContextMenuTab.Controls.Add(this.ContextToolstrip);
            resources.ApplyResources(this.ContextMenuTab, "ContextMenuTab");
            this.ContextMenuTab.Name = "ContextMenuTab";
            // 
            // ContextTree
            // 
            this.ContextTree.AllowDrop = true;
            resources.ApplyResources(this.ContextTree, "ContextTree");
            this.ContextTree.HideSelection = false;
            this.ContextTree.Name = "ContextTree";
            this.ContextTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragDrop);
            this.ContextTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemTree_AfterSelect);
            this.ContextTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragEnter);
            this.ContextTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ItemTree_ItemDrag);
            this.ContextTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragOver);
            // 
            // ContextToolstrip
            // 
            this.ContextToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ContextToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextAddButton,
            this.ContextDeleteButton,
            this.toolStripSeparator3,
            this.ContextUpButton,
            this.ContextDownButton,
            this.toolStripSeparator4,
            this.ContextCreateButton});
            resources.ApplyResources(this.ContextToolstrip, "ContextToolstrip");
            this.ContextToolstrip.Name = "ContextToolstrip";
            this.ContextToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // ContextAddButton
            // 
            this.ContextAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ContextAddButton, "ContextAddButton");
            this.ContextAddButton.Name = "ContextAddButton";
            // 
            // ContextDeleteButton
            // 
            this.ContextDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ContextDeleteButton, "ContextDeleteButton");
            this.ContextDeleteButton.Name = "ContextDeleteButton";
            this.ContextDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // ContextUpButton
            // 
            this.ContextUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ContextUpButton, "ContextUpButton");
            this.ContextUpButton.Name = "ContextUpButton";
            this.ContextUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ContextDownButton
            // 
            this.ContextDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ContextDownButton, "ContextDownButton");
            this.ContextDownButton.Name = "ContextDownButton";
            this.ContextDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // ContextCreateButton
            // 
            this.ContextCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ContextCreateButton, "ContextCreateButton");
            this.ContextCreateButton.Name = "ContextCreateButton";
            // 
            // TaskFrameTab
            // 
            this.TaskFrameTab.Controls.Add(this.TaskTree);
            this.TaskFrameTab.Controls.Add(this.TaskToolstrip);
            resources.ApplyResources(this.TaskFrameTab, "TaskFrameTab");
            this.TaskFrameTab.Name = "TaskFrameTab";
            // 
            // TaskTree
            // 
            this.TaskTree.AllowDrop = true;
            resources.ApplyResources(this.TaskTree, "TaskTree");
            this.TaskTree.HideSelection = false;
            this.TaskTree.Name = "TaskTree";
            this.TaskTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragDrop);
            this.TaskTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemTree_AfterSelect);
            this.TaskTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragEnter);
            this.TaskTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ItemTree_ItemDrag);
            this.TaskTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ItemTree_DragOver);
            // 
            // TaskToolstrip
            // 
            this.TaskToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.TaskToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskAddButton,
            this.TaskDeleteButton,
            this.toolStripSeparator5,
            this.TaskUpButton,
            this.TaskDownButton,
            this.toolStripSeparator6,
            this.TaskCreateButton});
            resources.ApplyResources(this.TaskToolstrip, "TaskToolstrip");
            this.TaskToolstrip.Name = "TaskToolstrip";
            this.TaskToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // TaskAddButton
            // 
            this.TaskAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TaskAddButton, "TaskAddButton");
            this.TaskAddButton.Name = "TaskAddButton";
            // 
            // TaskDeleteButton
            // 
            this.TaskDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TaskDeleteButton, "TaskDeleteButton");
            this.TaskDeleteButton.Name = "TaskDeleteButton";
            this.TaskDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // TaskUpButton
            // 
            this.TaskUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TaskUpButton, "TaskUpButton");
            this.TaskUpButton.Name = "TaskUpButton";
            this.TaskUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // TaskDownButton
            // 
            this.TaskDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TaskDownButton, "TaskDownButton");
            this.TaskDownButton.Name = "TaskDownButton";
            this.TaskDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // TaskCreateButton
            // 
            this.TaskCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TaskCreateButton, "TaskCreateButton");
            this.TaskCreateButton.Name = "TaskCreateButton";
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
            // ShowInBrowser
            // 
            resources.ApplyResources(this.ShowInBrowser, "ShowInBrowser");
            this.ShowInBrowser.Name = "ShowInBrowser";
            this.ShowInBrowser.Click += new System.EventHandler(this.ShowInBrowser_Click);
            // 
            // AddItemMenu
            // 
            this.AddItemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBuiltInFunctionMenu,
            this.AddCustomItemMenu,
            this.AddSubMenuItem,
            this.AddSeperatorItem});
            this.AddItemMenu.Name = "AddItemMenu_1";
            resources.ApplyResources(this.AddItemMenu, "AddItemMenu");
            // 
            // AddBuiltInFunctionMenu
            // 
            this.AddBuiltInFunctionMenu.Name = "AddBuiltInFunctionMenu";
            resources.ApplyResources(this.AddBuiltInFunctionMenu, "AddBuiltInFunctionMenu");
            // 
            // AddCustomItemMenu
            // 
            this.AddCustomItemMenu.Name = "AddCustomItemMenu";
            resources.ApplyResources(this.AddCustomItemMenu, "AddCustomItemMenu");
            // 
            // AddSubMenuItem
            // 
            resources.ApplyResources(this.AddSubMenuItem, "AddSubMenuItem");
            this.AddSubMenuItem.Name = "AddSubMenuItem";
            this.AddSubMenuItem.Click += new System.EventHandler(this.AddSubMenuItem_Click);
            // 
            // AddSeperatorItem
            // 
            resources.ApplyResources(this.AddSeperatorItem, "AddSeperatorItem");
            this.AddSeperatorItem.Name = "AddSeperatorItem";
            this.AddSeperatorItem.Click += new System.EventHandler(this.AddSeperatorItem_Click);
            // 
            // CreateCommandMenu
            // 
            this.CreateCommandMenu.Name = "CreateCommandMenu";
            resources.ApplyResources(this.CreateCommandMenu, "CreateCommandMenu");
            // 
            // FixedImages
            // 
            this.FixedImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FixedImages.ImageStream")));
            this.FixedImages.TransparentColor = System.Drawing.Color.Transparent;
            this.FixedImages.Images.SetKeyName(0, "FolderOpen.ico");
            this.FixedImages.Images.SetKeyName(1, "Seperator.ico");
            // 
            // LayoutEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ShowInBrowser);
            this.Controls.Add(this.browserURL);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.MenuBox);
            this.Controls.Add(this.FeatureLinkTargetType);
            this.Controls.Add(this.FeatureLinkTarget);
            this.Controls.Add(this.HomePageURL);
            this.Controls.Add(this.SelectMapButton);
            this.Controls.Add(this.MapResource);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.OverrideDisplayExtents);
            this.Controls.Add(this.overriddenMapExtents);
            this.Controls.Add(this.TitleText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Name = "LayoutEditor";
            this.overriddenMapExtents.ResumeLayout(false);
            this.overriddenMapExtents.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandTypesDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommandTable)).EndInit();
            this.MenuBox.ResumeLayout(false);
            this.MenuTabs.ResumeLayout(false);
            this.ToolbarTab.ResumeLayout(false);
            this.ToolbarTab.PerformLayout();
            this.ToolbarToolstrip.ResumeLayout(false);
            this.ToolbarToolstrip.PerformLayout();
            this.ContextMenuTab.ResumeLayout(false);
            this.ContextMenuTab.PerformLayout();
            this.ContextToolstrip.ResumeLayout(false);
            this.ContextToolstrip.PerformLayout();
            this.TaskFrameTab.ResumeLayout(false);
            this.TaskFrameTab.PerformLayout();
            this.TaskToolstrip.ResumeLayout(false);
            this.TaskToolstrip.PerformLayout();
            this.AddItemMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void TitleText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			m_layout.Title = TitleText.Text;
			m_editor.HasChanged();
		}

		private void SelectMapButton_Click(object sender, System.EventArgs e)
		{
			string r = m_editor.BrowseResource("MapDefinition");
			if (r != null)
			{
				m_layout.Map.ResourceId = r;
				MapResource.Text = r;
				m_editor.HasChanged();
			}
		}

		private void OverrideDisplayExtents_CheckedChanged(object sender, System.EventArgs e)
		{
			overriddenMapExtents.Enabled = OverrideDisplayExtents.Checked;
			if (m_isUpdating)
				return;

			if (OverrideDisplayExtents.Checked)
			{
				m_layout.Map.InitialView = new OSGeo.MapGuide.MaestroAPI.MapViewType();
				double x = 0, y = 0, s = 0;
				double.TryParse(overrideX.Text, System.Globalization.NumberStyles.Float, null, out x);
				double.TryParse(overrideY.Text, System.Globalization.NumberStyles.Float, null, out y);
				double.TryParse(overrideScale.Text, System.Globalization.NumberStyles.Float, null, out s);

				m_layout.Map.InitialView.CenterX = x;
				m_layout.Map.InitialView.CenterY = y;
				m_layout.Map.InitialView.Scale = s;
			}
			else
				m_layout.Map.InitialView = null;

			m_editor.HasChanged();
		}

		private void overrideX_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			double x = 0;
			double.TryParse(overrideX.Text, System.Globalization.NumberStyles.Float, null, out x);
			m_layout.Map.InitialView.CenterX = x;
			m_editor.HasChanged();
		}

		private void overrideY_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			double y = 0;
			double.TryParse(overrideY.Text, System.Globalization.NumberStyles.Float, null, out y);
			m_layout.Map.InitialView.CenterY = y;
			m_editor.HasChanged();
		}

		private void overrideScale_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			double s = 0;
			double.TryParse(overrideScale.Text, System.Globalization.NumberStyles.Float, null, out s);
			m_layout.Map.InitialView.Scale = s;
			m_editor.HasChanged();
		}

		private void LayerControlCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			m_layout.InformationPane.LegendVisible = LayerControlCheck.Checked;
			m_layout.InformationPane.Visible = m_layout.InformationPane.LegendVisible || m_layout.InformationPane.PropertiesVisible;

            LeftPaneWidth.Enabled = m_layout.InformationPane.LegendVisible || m_layout.InformationPane.PropertiesVisible;
			m_editor.HasChanged();
		}

		private void ItemPropertiesCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			m_layout.InformationPane.PropertiesVisible = ItemPropertiesCheck.Checked;
			m_layout.InformationPane.Visible = m_layout.InformationPane.LegendVisible || m_layout.InformationPane.PropertiesVisible;
        
            LeftPaneWidth.Enabled = m_layout.InformationPane.LegendVisible || m_layout.InformationPane.PropertiesVisible;
            m_editor.HasChanged();
        }

		private void LeftPaneWidth_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			try
			{
				m_layout.InformationPane.Width = int.Parse(LeftPaneWidth.Text);
				m_editor.HasChanged();
			}
			catch
			{
				//TODO: Notify user...
			}
		}

		private void ToolbarCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.ToolBar.Visible = ToolbarCheck.Checked;
			m_editor.HasChanged();
		}

		private void ContextMenuCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.ContextMenu.Visible = ContextMenuCheck.Checked;
			m_editor.HasChanged();
		}

		private void StatusBarCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.StatusBar.Visible = StatusBarCheck.Checked;
			m_editor.HasChanged();
		
		}

		private void ZoomControlCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.ZoomControl.Visible = ZoomControlCheck.Checked;
			m_editor.HasChanged();		
		}

		private void TaskPaneCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.TaskPane.Visible = TaskPaneCheck.Checked;
			m_editor.HasChanged();
		}

		private void TaskBarCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.TaskPane.TaskBar.Visible = TaskBarCheck.Checked;
			m_editor.HasChanged();
		}

		private void EditTaskBarBtn_Click(object sender, System.EventArgs e)
		{
			//TODO: Fix this
			MessageBox.Show(this, Strings.LayoutEditor.FunctionNotSupportedError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void RightPaneWidth_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			try
			{
				m_layout.TaskPane.Width = int.Parse(RightPaneWidth.Text);
				m_editor.HasChanged();
			}
			catch
			{
				//TODO: Notify user
			}
		}

		private void HomePageURL_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;

			m_layout.TaskPane.InitialTask = HomePageURL.Text;
			m_editor.HasChanged();
		
		}

		private void FeatureLinkTargetType_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			FeatureLinkTarget.Enabled = (OSGeo.MapGuide.MaestroAPI.TargetType)FeatureLinkTargetType.SelectedItem == OSGeo.MapGuide.MaestroAPI.TargetType.SpecifiedFrame;
			if (m_isUpdating)
				return;
			m_layout.Map.HyperlinkTarget = (OSGeo.MapGuide.MaestroAPI.TargetType)FeatureLinkTargetType.SelectedItem;
			m_editor.HasChanged();
		}

		private void FeatureLinkTarget_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			m_layout.Map.HyperlinkTargetFrame = FeatureLinkTarget.Text;
			m_editor.HasChanged();
		}

		public object Resource
		{
			get { return m_layout; }
			set 
			{
				m_layout = (OSGeo.MapGuide.MaestroAPI.WebLayout)value;
				UpdateDisplay();
			}
		}

		private void LoadMenuItems()
		{
			if (LoadedImages == null)
			{
				LoadedImages = new Hashtable();
				LoadedImageList = new ImageList();

				LoadedImageList.ColorDepth = ColorDepth.Depth32Bit;
				LoadedImageList.ImageSize = new Size(16, 16);

				LoadedImageList.Images.Add(new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "blank_icon.gif")));

                LoadedImageList.Images.Add(FixedImages.Images[0]);
                LoadedImageList.Images.Add(FixedImages.Images[1]);

				string path = System.IO.Path.Combine(Application.StartupPath, "stdicons");
				if (System.IO.Directory.Exists(path))
					foreach(string s in System.IO.Directory.GetFiles(path, "*.gif"))
					{
						LoadedImageList.Images.Add(Image.FromFile(s));
						LoadedImages.Add("../stdicons/" + System.IO.Path.GetFileName(s), LoadedImageList.Images.Count - 1);
					}
			}

            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.CommandTypesDataset))
                CommandTypesDataset.ReadXml(sr);

			BuiltInCommands = new Hashtable();
			foreach(System.Data.DataRow dr in CommandTypesDataset.Tables[0].Rows)
				BuiltInCommands.Add(dr["Command"].ToString(), dr);

		}

		private void FlattenTree(TreeNodeCollection col, ArrayList items)
		{
			foreach(TreeNode n in col)
			{
				items.Add(n);
				if (n.Nodes.Count > 0)
					FlattenTree(n.Nodes, items);
			}
		}

		private ArrayList GetAllNodes()
		{
			ArrayList items = new ArrayList();
			FlattenTree(ToolbarTree.Nodes, items);
			FlattenTree(ContextTree.Nodes, items);
			FlattenTree(TaskTree.Nodes, items);
			return items;
		}

		public void NameHasChanged(string newname)
		{
			TreeView tree = GetActiveTree();
			if (tree != null && tree.SelectedNode != null)
			{
				if (tree.SelectedNode.Tag as OSGeo.MapGuide.MaestroAPI.CommandItemType != null)
				{
					string oldname = tree.SelectedNode.Text;
					foreach(TreeNode n in GetAllNodes())
						if (n.Tag as OSGeo.MapGuide.MaestroAPI.CommandItemType != null && n.Text == oldname)
						{
							(n.Tag as OSGeo.MapGuide.MaestroAPI.CommandItemType).Command = newname;
							n.Text = newname;
						}
				}
				else
					tree.SelectedNode.Text = newname;
			}
		}

		public int FindImageIndex(string path)
		{
			if (path == null)
				return 0;

			if (LoadedImages.ContainsKey(path))
				return (int)LoadedImages[path];
			else
				return 0;
		}


		private OSGeo.MapGuide.MaestroAPI.CommandType CreateCommand(string commandname)
		{
			foreach(DataRow dr in CommandTypesDataset.Tables[0].Rows)
				if ((string)dr["Command"] == commandname)
				{
					if (!m_advancedTypes.ContainsKey(commandname))
					{
						OSGeo.MapGuide.MaestroAPI.BasicCommandType m = new OSGeo.MapGuide.MaestroAPI.BasicCommandType();
						string lookupval = ((string)dr["Action"]).Replace(" ", "");
					
						m.Action = (OSGeo.MapGuide.MaestroAPI.BasicCommandActionType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.BasicCommandActionType), lookupval, true);
						m.Description = (string)dr["Description"];
						m.DisabledImageURL = (string)dr["DisabledIcon"];
						m.ImageURL = (string)dr["EnabledIcon"];
						m.Label = (string)dr["Label"];
						m.Name = GetAvalibleCommandName((string)dr["Command"]);
						m.TargetViewer = (OSGeo.MapGuide.MaestroAPI.TargetViewerType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.TargetViewerType), (string)dr["Viewers"], true);
						m.Tooltip = (string)dr["Tooltip"];
						m_layout.CommandSet.Add(m);
						return m;
					}
					else
					{
						Type t = (System.Type)m_advancedTypes[commandname];
						OSGeo.MapGuide.MaestroAPI.CommandType v = (OSGeo.MapGuide.MaestroAPI.CommandType)Activator.CreateInstance(t);
						v.Description = (string)dr["Description"];
						v.DisabledImageURL = (string)dr["DisabledIcon"];
						v.ImageURL = (string)dr["EnabledIcon"];
						v.Label = (string)dr["Label"];
						v.Name = GetAvalibleCommandName((string)dr["Command"]);
						v.TargetViewer = (OSGeo.MapGuide.MaestroAPI.TargetViewerType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.TargetViewerType), (string)dr["Viewers"], true);
						v.Tooltip = (string)dr["Tooltip"];
						m_layout.CommandSet.Add(v);
						return v;
					}
				}

			return null;
		}

		private void CreateCommand_Click(object sender, EventArgs e)
		{
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (menu == null)
				return;

			CreateCommand(menu.Text);
			BuildTree(GetActiveTree(), GetActiveUIParent());
			RebuildCommandSetMenus();
			m_editor.HasChanged();
		}

		private string GetAvalibleCommandName(string name)
		{
			string namebase = name;
			bool taken;
			int i = 0;
			do
			{

				taken = false;
				i++;
				name = namebase + " " + i.ToString();

				foreach(OSGeo.MapGuide.MaestroAPI.CommandType cmd in m_layout.CommandSet)
					if (cmd.Name == name)
					{
						taken = true;
						break;
					}

			} while(taken && i < 100);

			if (i >= 100)
				throw new Exception(Strings.LayoutEditor.CommandNameLookupError);

			return name;
		}

		public string ResourceId
		{
			get { return m_layout.ResourceId; }
			set { m_layout.ResourceId = value; }
		}

		public bool Preview()
		{
			ShowInBrowser_Click(null, null);
			return true;
		}

		private void ItemTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeView cur = GetActiveTree();
			ToolStrip tb = GetActiveToolbar();

			int delIndex = ToolbarToolstrip.Items.IndexOf(ToolbarDeleteButton);
            int upIndex = ToolbarToolstrip.Items.IndexOf(ToolbarUpButton);
            int downIndex = ToolbarToolstrip.Items.IndexOf(ToolbarDownButton);

			if (cur == null || cur.SelectedNode == null || cur.SelectedNode.Tag == null)
			{
				tb.Items[delIndex].Enabled = false;
                tb.Items[upIndex].Enabled = false;
                tb.Items[downIndex].Enabled = false;
				commandEditor.Visible = false;
				return;
			}

            tb.Items[delIndex].Enabled = true;

			TreeNodeCollection parentCol = cur.SelectedNode.Parent == null ? cur.Nodes : cur.SelectedNode.Parent.Nodes;
            tb.Items[upIndex].Enabled = parentCol.IndexOf(cur.SelectedNode) > 0;
            tb.Items[downIndex].Enabled = parentCol.IndexOf(cur.SelectedNode) < parentCol.Count - 1;

			RefreshItemDisplay(cur.SelectedNode.Tag as OSGeo.MapGuide.MaestroAPI.UIItemType);
		}

		private void RefreshItemDisplay(OSGeo.MapGuide.MaestroAPI.UIItemType item)
		{
			try
			{
				m_isUpdating = true;
				if (item as OSGeo.MapGuide.MaestroAPI.CommandItemType != null)
				{
					OSGeo.MapGuide.MaestroAPI.CommandItemType command = (OSGeo.MapGuide.MaestroAPI.CommandItemType)item;
					commandEditor.Visible = true;
					commandEditor.SetItem(item, m_layout, m_editor, this);
				}
				else if (item as OSGeo.MapGuide.MaestroAPI.SeparatorItemType != null)
				{
					commandEditor.Visible = false;
					return;
				}
				else if (item as OSGeo.MapGuide.MaestroAPI.FlyoutItemType != null)
				{
					commandEditor.Visible = true;

					OSGeo.MapGuide.MaestroAPI.FlyoutItemType cmd = (OSGeo.MapGuide.MaestroAPI.FlyoutItemType)item;
					commandEditor.SetItem(cmd, m_layout, m_editor, this);
				}

			} 
			finally
			{
				m_isUpdating = false;
			}

		}

		public string GetActionByType(System.Type t)
		{
			foreach(DictionaryEntry de in m_advancedTypes)
				if (de.Value == t)
					return (string)de.Key;
			return "";
		}

		private string GetActionByType(OSGeo.MapGuide.MaestroAPI.CommandType cmd)
		{
			if (cmd as OSGeo.MapGuide.MaestroAPI.BasicCommandType != null)
				return (cmd as OSGeo.MapGuide.MaestroAPI.BasicCommandType).Action.ToString();
			else
				return GetActionByType(cmd.GetType());
		}

		private void MoveItemUp()
		{
            TreeView tree = GetActiveTree();
            if (tree == null || tree.SelectedNode == null || tree.SelectedNode.Tag == null)
                return;

            OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUISubCollection();

			int idx = col.IndexOf((OSGeo.MapGuide.MaestroAPI.UIItemType)tree.SelectedNode.Tag);
			if (idx > 0 && idx < col.Count)
			{
				OSGeo.MapGuide.MaestroAPI.UIItemType item = col[idx];
				col.RemoveAt(idx);
				col.Insert(idx-1, item);
			}

			BuildTree(GetActiveTree(), GetActiveUIParent());
            m_editor.HasChanged();
		}

		private void MoveItemDown()
		{
            TreeView tree = GetActiveTree();
            if (tree == null || tree.SelectedNode == null || tree.SelectedNode.Tag == null)
                return;

            OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUISubCollection();

            int idx = col.IndexOf((OSGeo.MapGuide.MaestroAPI.UIItemType)tree.SelectedNode.Tag);
            if (idx >= 0 && idx < col.Count - 1)
			{
				OSGeo.MapGuide.MaestroAPI.UIItemType item = col[idx];
				col.RemoveAt(idx);
				col.Insert(idx + 1, item);
			}

			BuildTree(GetActiveTree(), GetActiveUIParent());
            m_editor.HasChanged();
		}

		private void DeleteSelectedItem()
		{
			if (GetActiveTree().SelectedNode == null)
				return;

			OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUICollection();
			int idx = col.IndexOf((OSGeo.MapGuide.MaestroAPI.UIItemType)GetActiveTree().SelectedNode.Tag);
			if (idx >= 0 && idx < col.Count)
				col.RemoveAt(idx);

			BuildTree(GetActiveTree(), GetActiveUIParent());
            m_editor.HasChanged();
		}

		private void MenuTabs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ItemTree_AfterSelect(sender, null);
		}

		private ToolStrip GetActiveToolbar()
		{
			if (MenuTabs.SelectedTab == ToolbarTab)
				return ToolbarToolstrip;
			else if (MenuTabs.SelectedTab == ContextMenuTab)
				return ContextToolstrip;
			else if (MenuTabs.SelectedTab == TaskFrameTab)
				return TaskToolstrip;
			else
				return null;
		}

		private TreeView GetActiveTree()
		{
			if (MenuTabs.SelectedTab == ToolbarTab)
				return ToolbarTree;
			else if (MenuTabs.SelectedTab == ContextMenuTab)
				return ContextTree;
			else if (MenuTabs.SelectedTab == TaskFrameTab)
				return TaskTree;
			else
				return null;
		}

		private OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection GetActiveUIParent()
		{
			if (MenuTabs.SelectedTab == ToolbarTab)
				return m_layout.ToolBar.Button;
			else if (MenuTabs.SelectedTab == ContextMenuTab)
				return m_layout.ContextMenu.MenuItem;
			else if (MenuTabs.SelectedTab == TaskFrameTab)
				return m_layout.TaskPane.TaskBar.MenuButton;
			else
				return null;
		}

		private OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection GetActiveUICollection()
		{
			TreeView tree = GetActiveTree();

			OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUIParent();

			if (tree.SelectedNode != null && tree.SelectedNode.Parent != null)
				col = ((OSGeo.MapGuide.MaestroAPI.FlyoutItemType)tree.SelectedNode.Parent.Tag).SubItem;

			return col;
		}

        public OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection GetActiveUISubCollection()
        {
            return GetActiveUISubCollection(GetActiveTree().SelectedNode);
        }

        public OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection GetActiveUISubCollection(TreeNode sourceNode)
        {
            TreeView tree = GetActiveTree();
            OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUICollection();

            if (sourceNode != null)
            {
                if (sourceNode.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType != null)
                {
                    col = (sourceNode.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType).SubItem;
                }
                else if (sourceNode.Parent != null && sourceNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType != null)
                {
                    col = (sourceNode.Parent.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType).SubItem;
                }
            }

            return col;
        }

		private void InsertItem(OSGeo.MapGuide.MaestroAPI.UIItemType item)
		{
			TreeView tree = GetActiveTree();
			OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUISubCollection();

			int idx = -1;
            if (tree.SelectedNode != null)
            {
                idx = col.IndexOf((OSGeo.MapGuide.MaestroAPI.UIItemType)tree.SelectedNode.Tag);
            }

			if (idx < 0)
				idx = col.Count - 1;

			col.Insert(idx + 1, item);
		}

		private void AddSubMenuItem_Click(object sender, System.EventArgs e)
		{
			if (GetActiveTree() == TaskTree)
			{
				MessageBox.Show(this, Strings.LayoutEditor.SubMenuInTaskBarError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			OSGeo.MapGuide.MaestroAPI.FlyoutItemType fly = new OSGeo.MapGuide.MaestroAPI.FlyoutItemType();
			fly.Function = OSGeo.MapGuide.MaestroAPI.UIItemFunctionType.Flyout;

            fly.Label = Strings.LayoutEditor.NewSubmenuName;
            fly.SubItem = new OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection();

			InsertItem(fly);

			BuildTree(GetActiveTree(), GetActiveUIParent());
            m_editor.HasChanged();
		}

		private void AddSeperatorItem_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.SeparatorItemType sep = new OSGeo.MapGuide.MaestroAPI.SeparatorItemType();
			sep.Function = OSGeo.MapGuide.MaestroAPI.UIItemFunctionType.Separator;

			InsertItem(sep);

			BuildTree(GetActiveTree(), GetActiveUIParent());
            m_editor.HasChanged();
		}

		private void AddBuiltInItem_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.CommandItemType cmd = new OSGeo.MapGuide.MaestroAPI.CommandItemType();
			//TODO: Don't use menu text, use a lookup table, so we may translate the UI
			cmd.Command = ((ToolStripMenuItem)sender).Text;
			cmd.Function = OSGeo.MapGuide.MaestroAPI.UIItemFunctionType.Command;

			bool found = false;
			foreach(OSGeo.MapGuide.MaestroAPI.CommandType ct in m_layout.CommandSet)
				if (cmd.Command == ct.Name)
				{
					found = true;
					break;
				}

			if (!found)
			{
				OSGeo.MapGuide.MaestroAPI.CommandType ct = CreateCommand(cmd.Command);
				ct.Name = cmd.Command;
			}

			InsertItem(cmd);
			BuildTree(GetActiveTree(), GetActiveUIParent());
			ArrayList nodes = new ArrayList();
			FlattenTree(GetActiveTree().Nodes, nodes);
			foreach(TreeNode n in nodes)
				if (n.Tag == cmd)
				{
					GetActiveTree().SelectedNode = n;
					n.EnsureVisible();
					break;
				}

            m_editor.HasChanged();

		}

		private void AddCustomItem_Click(object sender, System.EventArgs e)
		{
			OSGeo.MapGuide.MaestroAPI.CommandItemType cmd = new OSGeo.MapGuide.MaestroAPI.CommandItemType();
			//TODO: Don't use menu text, use a lookup table, so we may translate the UI
			cmd.Command = ((ToolStripMenuItem)sender).Text;
			cmd.Function = OSGeo.MapGuide.MaestroAPI.UIItemFunctionType.Command;

			InsertItem(cmd);

			BuildTree(GetActiveTree(), GetActiveUIParent());

			ArrayList nodes = new ArrayList();
			FlattenTree(GetActiveTree().Nodes, nodes);
			foreach(TreeNode n in nodes)
				if (n.Tag == cmd)
				{
					GetActiveTree().SelectedNode = n;
					n.EnsureVisible();
					break;
				}

            m_editor.HasChanged();
		}


		public bool Save(string savename)
		{
			return false;
		}

		private void CreateCommandMenu_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void AddItemMenu_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void ShowInBrowser_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_editor.CurrentConnection.SaveResourceAs(m_layout, m_tempResource);
				string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_editor.CurrentConnection).BaseURL + "mapviewerajax/?WEBLAYOUT=" + System.Web.HttpUtility.UrlEncode(m_tempResource) + "&SESSION=" + System.Web.HttpUtility.UrlEncode(m_editor.CurrentConnection.SessionID);

                m_editor.OpenUrl(url);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, String.Format(Strings.LayoutEditor.BrowserLaunchError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedItem();            
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            MoveItemUp();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            MoveItemDown();
        }

        private void ItemTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView tree = sender as TreeView;
            if (tree == null || e.Item as TreeNode == null || (e.Item as TreeNode).TreeView != GetActiveTree())
                return;
            tree.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ItemTree_DragEnter(object sender, DragEventArgs e)
        {
            TreeView tree = sender as TreeView;
            TreeNode sourceNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;;
            if (sourceNode != null && sourceNode.TreeView == tree)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
            
            
        }

        private void ItemTree_DragOver(object sender, DragEventArgs e)
        {
            TreeView tree = sender as TreeView;
            TreeNode sourceNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;;
            if (sourceNode != null && sourceNode.TreeView == tree)
            {
                TreeNode targetNode = tree.GetNodeAt(tree.PointToClient(new Point(e.X, e.Y)));
                if (targetNode != sourceNode)
                {
                    tree.SelectedNode = targetNode;
                    e.Effect = DragDropEffects.Move;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;

        }

        private void ItemTree_DragDrop(object sender, DragEventArgs e)
        {
            TreeView tree = sender as TreeView;
            TreeNode sourceNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;;
            if (sourceNode != null && sourceNode.TreeView == tree)
            {
                TreeNode targetNode = tree.GetNodeAt(tree.PointToClient(new Point(e.X, e.Y)));
                if (targetNode != sourceNode)
                {
                    OSGeo.MapGuide.MaestroAPI.UIItemTypeCollection col = GetActiveUISubCollection(sourceNode);

                    for (int i = 0; i < col.Count; i++)
                        if (col[i] == sourceNode.Tag)
                        {
                            col.RemoveAt(i);
                            break;
                        }

                    col = GetActiveUISubCollection(targetNode);
                    int index = col.Count;
                    if (targetNode != null && targetNode.Tag != null)
                        index = col.IndexOf(targetNode.Tag as OSGeo.MapGuide.MaestroAPI.UIItemType);

                    if (index < 0)
                        index = col.Count;

                    col.Insert(index, sourceNode.Tag as OSGeo.MapGuide.MaestroAPI.UIItemType);
                    try
                    {
                        tree.BeginUpdate();
                        sourceNode.Remove();
                        if (targetNode != null && targetNode.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType != null && (targetNode.Tag as OSGeo.MapGuide.MaestroAPI.FlyoutItemType).SubItem == col)
                        {
                            targetNode.Nodes.Insert(index, sourceNode);
                            tree.SelectedNode = sourceNode;
                        }
                        else if (col == GetActiveUICollection())
                        {
                            tree.Nodes.Insert(index, sourceNode);
                            tree.SelectedNode = sourceNode;
                        }
                    }
                    finally
                    {
                        tree.EndUpdate();
                    }

                    //This should never happen, but in case I forgot something, this will pick it up :)
                    if (sourceNode.TreeView == null)
                        BuildTree(GetActiveTree(), GetActiveUICollection());

                    
                    m_editor.HasChanged();
                }
            }

        }
    
        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return true; } }
        public bool SupportsValidate { get { return true; } }
        public bool SupportsProfiling { get { return false; } }
    }
}
