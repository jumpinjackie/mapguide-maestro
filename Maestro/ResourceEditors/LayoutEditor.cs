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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for LayoutEditor.
	/// </summary>
	public class LayoutEditor : System.Windows.Forms.UserControl, ResourceEditor
	{
		private static byte[] SharedComboDataSet = null;
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
		private Globalizator.Globalizator m_globalizor = null;
		private System.Windows.Forms.TextBox browserURL;
        private ToolStrip ToolbarToolstrip;
        private ToolStripSplitButton ToolbarAddButton;
        private ToolStripButton ToolbarDeleteButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolbarUpButton;
        private ToolStripButton ToolbarDownButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStrip ContextToolstrip;
        private ToolStripSplitButton ContextAddButton;
        private ToolStripButton ContextDeleteButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ContextUpButton;
        private ToolStripButton ContextDownButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStrip TaskToolstrip;
        private ToolStripSplitButton TaskAddButton;
        private ToolStripButton TaskDeleteButton;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton TaskUpButton;
        private ToolStripButton TaskDownButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSplitButton ToolbarCreateButton;
        private ToolStripSplitButton ContextCreateButton;
        private ToolStripSplitButton TaskCreateButton;
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

		public Globalizator.Globalizator Globalizor { get { return m_globalizor; } }


		public LayoutEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_layout = new OSGeo.MapGuide.MaestroAPI.WebLayout();
			m_tempResource = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, true);
			UpdateDisplay();
		}

		public LayoutEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			m_layout = editor.CurrentConnection.GetWebLayout(resourceID);
			m_tempResource = m_editor.CurrentConnection.GetResourceIdentifier(Guid.NewGuid().ToString(), OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, true);
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
					TreeNode n = new TreeNode(m_globalizor.Translate("- seperator -"));
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

			m_globalizor = new  Globalizator.Globalizator(this);

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
            this.ToolbarAddButton = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolbarDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolbarUpButton = new System.Windows.Forms.ToolStripButton();
            this.ToolbarDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolbarCreateButton = new System.Windows.Forms.ToolStripSplitButton();
            this.ContextMenuTab = new System.Windows.Forms.TabPage();
            this.ContextTree = new System.Windows.Forms.TreeView();
            this.ContextToolstrip = new System.Windows.Forms.ToolStrip();
            this.ContextAddButton = new System.Windows.Forms.ToolStripSplitButton();
            this.ContextDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextUpButton = new System.Windows.Forms.ToolStripButton();
            this.ContextDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextCreateButton = new System.Windows.Forms.ToolStripSplitButton();
            this.TaskFrameTab = new System.Windows.Forms.TabPage();
            this.TaskTree = new System.Windows.Forms.TreeView();
            this.TaskToolstrip = new System.Windows.Forms.ToolStrip();
            this.TaskAddButton = new System.Windows.Forms.ToolStripSplitButton();
            this.TaskDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskUpButton = new System.Windows.Forms.ToolStripButton();
            this.TaskDownButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskCreateButton = new System.Windows.Forms.ToolStripSplitButton();
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
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text for browser title bar";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(0, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Map resource";
            // 
            // overriddenMapExtents
            // 
            this.overriddenMapExtents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.overriddenMapExtents.Controls.Add(this.label6);
            this.overriddenMapExtents.Controls.Add(this.overrideScale);
            this.overriddenMapExtents.Controls.Add(this.overrideY);
            this.overriddenMapExtents.Controls.Add(this.overrideX);
            this.overriddenMapExtents.Controls.Add(this.label5);
            this.overriddenMapExtents.Controls.Add(this.label4);
            this.overriddenMapExtents.Controls.Add(this.label3);
            this.overriddenMapExtents.Enabled = false;
            this.overriddenMapExtents.Location = new System.Drawing.Point(0, 56);
            this.overriddenMapExtents.Name = "overriddenMapExtents";
            this.overriddenMapExtents.Size = new System.Drawing.Size(544, 56);
            this.overriddenMapExtents.TabIndex = 2;
            this.overriddenMapExtents.TabStop = false;
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
            this.overrideScale.Text = "textBox3";
            this.overrideScale.TextChanged += new System.EventHandler(this.overrideScale_TextChanged);
            // 
            // overrideY
            // 
            this.overrideY.Location = new System.Drawing.Point(184, 24);
            this.overrideY.Name = "overrideY";
            this.overrideY.Size = new System.Drawing.Size(104, 20);
            this.overrideY.TabIndex = 4;
            this.overrideY.Text = "textBox2";
            this.overrideY.TextChanged += new System.EventHandler(this.overrideY_TextChanged);
            // 
            // overrideX
            // 
            this.overrideX.Location = new System.Drawing.Point(40, 24);
            this.overrideX.Name = "overrideX";
            this.overrideX.Size = new System.Drawing.Size(104, 20);
            this.overrideX.TabIndex = 3;
            this.overrideX.Text = "textBox1";
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
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(16, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "X";
            // 
            // OverrideDisplayExtents
            // 
            this.OverrideDisplayExtents.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OverrideDisplayExtents.Location = new System.Drawing.Point(8, 56);
            this.OverrideDisplayExtents.Name = "OverrideDisplayExtents";
            this.OverrideDisplayExtents.Size = new System.Drawing.Size(216, 16);
            this.OverrideDisplayExtents.TabIndex = 3;
            this.OverrideDisplayExtents.Text = "Override the maps initial display extents";
            this.OverrideDisplayExtents.CheckedChanged += new System.EventHandler(this.OverrideDisplayExtents_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(0, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 184);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Items visible in viewer";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LeftPaneWidth);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ItemPropertiesCheck);
            this.groupBox3.Controls.Add(this.LayerControlCheck);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(16, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 152);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Left side";
            // 
            // LeftPaneWidth
            // 
            this.LeftPaneWidth.Location = new System.Drawing.Point(8, 120);
            this.LeftPaneWidth.Name = "LeftPaneWidth";
            this.LeftPaneWidth.Size = new System.Drawing.Size(144, 20);
            this.LeftPaneWidth.TabIndex = 3;
            this.LeftPaneWidth.Text = "textBox4";
            this.LeftPaneWidth.TextChanged += new System.EventHandler(this.LeftPaneWidth_TextChanged);
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label7.Location = new System.Drawing.Point(8, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Width (in pixels)";
            // 
            // ItemPropertiesCheck
            // 
            this.ItemPropertiesCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ItemPropertiesCheck.Location = new System.Drawing.Point(16, 48);
            this.ItemPropertiesCheck.Name = "ItemPropertiesCheck";
            this.ItemPropertiesCheck.Size = new System.Drawing.Size(128, 16);
            this.ItemPropertiesCheck.TabIndex = 1;
            this.ItemPropertiesCheck.Text = "Item properties";
            this.ItemPropertiesCheck.CheckedChanged += new System.EventHandler(this.ItemPropertiesCheck_CheckedChanged);
            // 
            // LayerControlCheck
            // 
            this.LayerControlCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.LayerControlCheck.Location = new System.Drawing.Point(16, 24);
            this.LayerControlCheck.Name = "LayerControlCheck";
            this.LayerControlCheck.Size = new System.Drawing.Size(128, 16);
            this.LayerControlCheck.TabIndex = 0;
            this.LayerControlCheck.Text = "Layer control";
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
            this.groupBox4.Location = new System.Drawing.Point(184, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(168, 152);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Map / middle";
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label9.Location = new System.Drawing.Point(24, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "(AJAX viewer only)";
            // 
            // ZoomControlCheck
            // 
            this.ZoomControlCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ZoomControlCheck.Location = new System.Drawing.Point(8, 96);
            this.ZoomControlCheck.Name = "ZoomControlCheck";
            this.ZoomControlCheck.Size = new System.Drawing.Size(144, 16);
            this.ZoomControlCheck.TabIndex = 3;
            this.ZoomControlCheck.Text = "Zoom control";
            this.ZoomControlCheck.CheckedChanged += new System.EventHandler(this.ZoomControlCheck_CheckedChanged);
            // 
            // StatusBarCheck
            // 
            this.StatusBarCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StatusBarCheck.Location = new System.Drawing.Point(8, 72);
            this.StatusBarCheck.Name = "StatusBarCheck";
            this.StatusBarCheck.Size = new System.Drawing.Size(144, 16);
            this.StatusBarCheck.TabIndex = 2;
            this.StatusBarCheck.Text = "Status bar";
            this.StatusBarCheck.CheckedChanged += new System.EventHandler(this.StatusBarCheck_CheckedChanged);
            // 
            // ContextMenuCheck
            // 
            this.ContextMenuCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ContextMenuCheck.Location = new System.Drawing.Point(8, 48);
            this.ContextMenuCheck.Name = "ContextMenuCheck";
            this.ContextMenuCheck.Size = new System.Drawing.Size(136, 16);
            this.ContextMenuCheck.TabIndex = 1;
            this.ContextMenuCheck.Text = "Context menu";
            this.ContextMenuCheck.CheckedChanged += new System.EventHandler(this.ContextMenuCheck_CheckedChanged);
            // 
            // ToolbarCheck
            // 
            this.ToolbarCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ToolbarCheck.Location = new System.Drawing.Point(8, 24);
            this.ToolbarCheck.Name = "ToolbarCheck";
            this.ToolbarCheck.Size = new System.Drawing.Size(136, 16);
            this.ToolbarCheck.TabIndex = 0;
            this.ToolbarCheck.Text = "Toolbar";
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
            this.groupBox5.Location = new System.Drawing.Point(368, 16);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(168, 152);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Right side";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label8.Location = new System.Drawing.Point(8, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "Width (in pixels)";
            // 
            // EditTaskBarBtn
            // 
            this.EditTaskBarBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditTaskBarBtn.Location = new System.Drawing.Point(8, 72);
            this.EditTaskBarBtn.Name = "EditTaskBarBtn";
            this.EditTaskBarBtn.Size = new System.Drawing.Size(152, 24);
            this.EditTaskBarBtn.TabIndex = 6;
            this.EditTaskBarBtn.Text = "Edit task bar";
            this.EditTaskBarBtn.Click += new System.EventHandler(this.EditTaskBarBtn_Click);
            // 
            // RightPaneWidth
            // 
            this.RightPaneWidth.Location = new System.Drawing.Point(8, 120);
            this.RightPaneWidth.Name = "RightPaneWidth";
            this.RightPaneWidth.Size = new System.Drawing.Size(144, 20);
            this.RightPaneWidth.TabIndex = 5;
            this.RightPaneWidth.Text = "textBox5";
            this.RightPaneWidth.TextChanged += new System.EventHandler(this.RightPaneWidth_TextChanged);
            // 
            // TaskBarCheck
            // 
            this.TaskBarCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TaskBarCheck.Location = new System.Drawing.Point(8, 48);
            this.TaskBarCheck.Name = "TaskBarCheck";
            this.TaskBarCheck.Size = new System.Drawing.Size(152, 16);
            this.TaskBarCheck.TabIndex = 2;
            this.TaskBarCheck.Text = "Task bar";
            this.TaskBarCheck.CheckedChanged += new System.EventHandler(this.TaskBarCheck_CheckedChanged);
            // 
            // TaskPaneCheck
            // 
            this.TaskPaneCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TaskPaneCheck.Location = new System.Drawing.Point(8, 24);
            this.TaskPaneCheck.Name = "TaskPaneCheck";
            this.TaskPaneCheck.Size = new System.Drawing.Size(144, 16);
            this.TaskPaneCheck.TabIndex = 1;
            this.TaskPaneCheck.Text = "Task pane";
            this.TaskPaneCheck.CheckedChanged += new System.EventHandler(this.TaskPaneCheck_CheckedChanged);
            // 
            // TitleText
            // 
            this.TitleText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleText.Location = new System.Drawing.Point(160, 0);
            this.TitleText.Name = "TitleText";
            this.TitleText.Size = new System.Drawing.Size(376, 20);
            this.TitleText.TabIndex = 4;
            this.TitleText.Text = "textBox6";
            this.TitleText.TextChanged += new System.EventHandler(this.TitleText_TextChanged);
            // 
            // MapResource
            // 
            this.MapResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapResource.Location = new System.Drawing.Point(160, 24);
            this.MapResource.Name = "MapResource";
            this.MapResource.ReadOnly = true;
            this.MapResource.Size = new System.Drawing.Size(352, 20);
            this.MapResource.TabIndex = 5;
            this.MapResource.Text = "textBox7";
            // 
            // SelectMapButton
            // 
            this.SelectMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectMapButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectMapButton.Location = new System.Drawing.Point(520, 24);
            this.SelectMapButton.Name = "SelectMapButton";
            this.SelectMapButton.Size = new System.Drawing.Size(24, 20);
            this.SelectMapButton.TabIndex = 7;
            this.SelectMapButton.Text = "...";
            this.SelectMapButton.Click += new System.EventHandler(this.SelectMapButton_Click);
            // 
            // FeatureLinkTarget
            // 
            this.FeatureLinkTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FeatureLinkTarget.Enabled = false;
            this.FeatureLinkTarget.Location = new System.Drawing.Point(280, 336);
            this.FeatureLinkTarget.Name = "FeatureLinkTarget";
            this.FeatureLinkTarget.Size = new System.Drawing.Size(264, 20);
            this.FeatureLinkTarget.TabIndex = 11;
            this.FeatureLinkTarget.Text = "textBox8";
            this.FeatureLinkTarget.TextChanged += new System.EventHandler(this.FeatureLinkTarget_TextChanged);
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Location = new System.Drawing.Point(0, 336);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 16);
            this.label10.TabIndex = 9;
            this.label10.Text = "Feature hyperlink target";
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label11.Location = new System.Drawing.Point(0, 312);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 16);
            this.label11.TabIndex = 8;
            this.label11.Text = "Intial task in taskpane";
            // 
            // HomePageURL
            // 
            this.HomePageURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HomePageURL.Location = new System.Drawing.Point(160, 312);
            this.HomePageURL.Name = "HomePageURL";
            this.HomePageURL.Size = new System.Drawing.Size(384, 20);
            this.HomePageURL.TabIndex = 10;
            this.HomePageURL.Text = "textBox9";
            this.HomePageURL.TextChanged += new System.EventHandler(this.HomePageURL_TextChanged);
            // 
            // FeatureLinkTargetType
            // 
            this.FeatureLinkTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FeatureLinkTargetType.Items.AddRange(new object[] {
            "Task pane",
            "New window",
            "Named frame"});
            this.FeatureLinkTargetType.Location = new System.Drawing.Point(160, 336);
            this.FeatureLinkTargetType.Name = "FeatureLinkTargetType";
            this.FeatureLinkTargetType.Size = new System.Drawing.Size(112, 21);
            this.FeatureLinkTargetType.TabIndex = 12;
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
            this.MenuBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuBox.Controls.Add(this.commandEditor);
            this.MenuBox.Controls.Add(this.splitter2);
            this.MenuBox.Controls.Add(this.MenuTabs);
            this.MenuBox.Location = new System.Drawing.Point(0, 384);
            this.MenuBox.Name = "MenuBox";
            this.MenuBox.Size = new System.Drawing.Size(552, 288);
            this.MenuBox.TabIndex = 14;
            this.MenuBox.TabStop = false;
            this.MenuBox.Text = "Menus";
            // 
            // commandEditor
            // 
            this.commandEditor.AutoScroll = true;
            this.commandEditor.AutoScrollMinSize = new System.Drawing.Size(220, 269);
            this.commandEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandEditor.Location = new System.Drawing.Point(267, 16);
            this.commandEditor.Name = "commandEditor";
            this.commandEditor.Size = new System.Drawing.Size(282, 269);
            this.commandEditor.TabIndex = 2;
            this.commandEditor.Visible = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(259, 16);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(8, 269);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // MenuTabs
            // 
            this.MenuTabs.Controls.Add(this.ToolbarTab);
            this.MenuTabs.Controls.Add(this.ContextMenuTab);
            this.MenuTabs.Controls.Add(this.TaskFrameTab);
            this.MenuTabs.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuTabs.Location = new System.Drawing.Point(3, 16);
            this.MenuTabs.Name = "MenuTabs";
            this.MenuTabs.SelectedIndex = 0;
            this.MenuTabs.Size = new System.Drawing.Size(256, 269);
            this.MenuTabs.TabIndex = 0;
            this.MenuTabs.SelectedIndexChanged += new System.EventHandler(this.MenuTabs_SelectedIndexChanged);
            // 
            // ToolbarTab
            // 
            this.ToolbarTab.Controls.Add(this.ToolbarTree);
            this.ToolbarTab.Controls.Add(this.ToolbarToolstrip);
            this.ToolbarTab.Location = new System.Drawing.Point(4, 22);
            this.ToolbarTab.Name = "ToolbarTab";
            this.ToolbarTab.Size = new System.Drawing.Size(248, 243);
            this.ToolbarTab.TabIndex = 0;
            this.ToolbarTab.Text = "Toolbar";
            // 
            // ToolbarTree
            // 
            this.ToolbarTree.AllowDrop = true;
            this.ToolbarTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolbarTree.HideSelection = false;
            this.ToolbarTree.Location = new System.Drawing.Point(0, 25);
            this.ToolbarTree.Name = "ToolbarTree";
            this.ToolbarTree.Size = new System.Drawing.Size(248, 218);
            this.ToolbarTree.TabIndex = 1;
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
            this.ToolbarToolstrip.Location = new System.Drawing.Point(0, 0);
            this.ToolbarToolstrip.Name = "ToolbarToolstrip";
            this.ToolbarToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolbarToolstrip.Size = new System.Drawing.Size(248, 25);
            this.ToolbarToolstrip.TabIndex = 2;
            this.ToolbarToolstrip.Text = "toolStrip1";
            // 
            // ToolbarAddButton
            // 
            this.ToolbarAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolbarAddButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolbarAddButton.Image")));
            this.ToolbarAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolbarAddButton.Name = "ToolbarAddButton";
            this.ToolbarAddButton.Size = new System.Drawing.Size(32, 22);
            this.ToolbarAddButton.Text = "toolStripSplitButton1";
            this.ToolbarAddButton.ToolTipText = "Click to add a new item";
            this.ToolbarAddButton.ButtonClick += new System.EventHandler(this.AddButton_Click);
            // 
            // ToolbarDeleteButton
            // 
            this.ToolbarDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolbarDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolbarDeleteButton.Image")));
            this.ToolbarDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolbarDeleteButton.Name = "ToolbarDeleteButton";
            this.ToolbarDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.ToolbarDeleteButton.Text = "toolStripButton1";
            this.ToolbarDeleteButton.ToolTipText = "Click to delete the selected item";
            this.ToolbarDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolbarUpButton
            // 
            this.ToolbarUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolbarUpButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolbarUpButton.Image")));
            this.ToolbarUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolbarUpButton.Name = "ToolbarUpButton";
            this.ToolbarUpButton.Size = new System.Drawing.Size(23, 22);
            this.ToolbarUpButton.Text = "toolStripButton2";
            this.ToolbarUpButton.ToolTipText = "Click to move the selected item up";
            this.ToolbarUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ToolbarDownButton
            // 
            this.ToolbarDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolbarDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolbarDownButton.Image")));
            this.ToolbarDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolbarDownButton.Name = "ToolbarDownButton";
            this.ToolbarDownButton.Size = new System.Drawing.Size(23, 22);
            this.ToolbarDownButton.Text = "toolStripButton3";
            this.ToolbarDownButton.ToolTipText = "Click to move the selected item down";
            this.ToolbarDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolbarCreateButton
            // 
            this.ToolbarCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolbarCreateButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolbarCreateButton.Image")));
            this.ToolbarCreateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolbarCreateButton.Name = "ToolbarCreateButton";
            this.ToolbarCreateButton.Size = new System.Drawing.Size(32, 22);
            this.ToolbarCreateButton.Text = "toolStripSplitButton1";
            this.ToolbarCreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // ContextMenuTab
            // 
            this.ContextMenuTab.Controls.Add(this.ContextTree);
            this.ContextMenuTab.Controls.Add(this.ContextToolstrip);
            this.ContextMenuTab.Location = new System.Drawing.Point(4, 22);
            this.ContextMenuTab.Name = "ContextMenuTab";
            this.ContextMenuTab.Size = new System.Drawing.Size(248, 243);
            this.ContextMenuTab.TabIndex = 1;
            this.ContextMenuTab.Text = "Context menu";
            // 
            // ContextTree
            // 
            this.ContextTree.AllowDrop = true;
            this.ContextTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextTree.HideSelection = false;
            this.ContextTree.Location = new System.Drawing.Point(0, 25);
            this.ContextTree.Name = "ContextTree";
            this.ContextTree.Size = new System.Drawing.Size(248, 218);
            this.ContextTree.TabIndex = 3;
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
            this.ContextToolstrip.Location = new System.Drawing.Point(0, 0);
            this.ContextToolstrip.Name = "ContextToolstrip";
            this.ContextToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ContextToolstrip.Size = new System.Drawing.Size(248, 25);
            this.ContextToolstrip.TabIndex = 4;
            this.ContextToolstrip.Text = "toolStrip2";
            // 
            // ContextAddButton
            // 
            this.ContextAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContextAddButton.Image = ((System.Drawing.Image)(resources.GetObject("ContextAddButton.Image")));
            this.ContextAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContextAddButton.Name = "ContextAddButton";
            this.ContextAddButton.Size = new System.Drawing.Size(32, 22);
            this.ContextAddButton.Text = "toolStripSplitButton1";
            this.ContextAddButton.ToolTipText = "Click to add a new item";
            this.ContextAddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // ContextDeleteButton
            // 
            this.ContextDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContextDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("ContextDeleteButton.Image")));
            this.ContextDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContextDeleteButton.Name = "ContextDeleteButton";
            this.ContextDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.ContextDeleteButton.Text = "toolStripButton1";
            this.ContextDeleteButton.ToolTipText = "Click to delete the selected item";
            this.ContextDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ContextUpButton
            // 
            this.ContextUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContextUpButton.Image = ((System.Drawing.Image)(resources.GetObject("ContextUpButton.Image")));
            this.ContextUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContextUpButton.Name = "ContextUpButton";
            this.ContextUpButton.Size = new System.Drawing.Size(23, 22);
            this.ContextUpButton.Text = "toolStripButton2";
            this.ContextUpButton.ToolTipText = "Click to move the selected item up";
            this.ContextUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ContextDownButton
            // 
            this.ContextDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContextDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ContextDownButton.Image")));
            this.ContextDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContextDownButton.Name = "ContextDownButton";
            this.ContextDownButton.Size = new System.Drawing.Size(23, 22);
            this.ContextDownButton.Text = "toolStripButton3";
            this.ContextDownButton.ToolTipText = "Click to move the selected item down";
            this.ContextDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ContextCreateButton
            // 
            this.ContextCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContextCreateButton.Image = ((System.Drawing.Image)(resources.GetObject("ContextCreateButton.Image")));
            this.ContextCreateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContextCreateButton.Name = "ContextCreateButton";
            this.ContextCreateButton.Size = new System.Drawing.Size(32, 22);
            this.ContextCreateButton.Text = "toolStripSplitButton1";
            this.ContextCreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // TaskFrameTab
            // 
            this.TaskFrameTab.Controls.Add(this.TaskTree);
            this.TaskFrameTab.Controls.Add(this.TaskToolstrip);
            this.TaskFrameTab.Location = new System.Drawing.Point(4, 22);
            this.TaskFrameTab.Name = "TaskFrameTab";
            this.TaskFrameTab.Size = new System.Drawing.Size(248, 243);
            this.TaskFrameTab.TabIndex = 2;
            this.TaskFrameTab.Text = "Task frame menu";
            // 
            // TaskTree
            // 
            this.TaskTree.AllowDrop = true;
            this.TaskTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskTree.HideSelection = false;
            this.TaskTree.Location = new System.Drawing.Point(0, 25);
            this.TaskTree.Name = "TaskTree";
            this.TaskTree.Size = new System.Drawing.Size(248, 218);
            this.TaskTree.TabIndex = 4;
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
            this.TaskToolstrip.Location = new System.Drawing.Point(0, 0);
            this.TaskToolstrip.Name = "TaskToolstrip";
            this.TaskToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.TaskToolstrip.Size = new System.Drawing.Size(248, 25);
            this.TaskToolstrip.TabIndex = 5;
            this.TaskToolstrip.Text = "toolStrip3";
            // 
            // TaskAddButton
            // 
            this.TaskAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TaskAddButton.Image = ((System.Drawing.Image)(resources.GetObject("TaskAddButton.Image")));
            this.TaskAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TaskAddButton.Name = "TaskAddButton";
            this.TaskAddButton.Size = new System.Drawing.Size(32, 22);
            this.TaskAddButton.Text = "toolStripSplitButton1";
            this.TaskAddButton.ToolTipText = "Click to add a new item";
            this.TaskAddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // TaskDeleteButton
            // 
            this.TaskDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TaskDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("TaskDeleteButton.Image")));
            this.TaskDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TaskDeleteButton.Name = "TaskDeleteButton";
            this.TaskDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.TaskDeleteButton.Text = "toolStripButton1";
            this.TaskDeleteButton.ToolTipText = "Click to delete the selected item";
            this.TaskDeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // TaskUpButton
            // 
            this.TaskUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TaskUpButton.Image = ((System.Drawing.Image)(resources.GetObject("TaskUpButton.Image")));
            this.TaskUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TaskUpButton.Name = "TaskUpButton";
            this.TaskUpButton.Size = new System.Drawing.Size(23, 22);
            this.TaskUpButton.Text = "toolStripButton2";
            this.TaskUpButton.ToolTipText = "Click to move the selected item up";
            this.TaskUpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // TaskDownButton
            // 
            this.TaskDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TaskDownButton.Image = ((System.Drawing.Image)(resources.GetObject("TaskDownButton.Image")));
            this.TaskDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TaskDownButton.Name = "TaskDownButton";
            this.TaskDownButton.Size = new System.Drawing.Size(23, 22);
            this.TaskDownButton.Text = "toolStripButton3";
            this.TaskDownButton.ToolTipText = "Click to move the selected item down";
            this.TaskDownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // TaskCreateButton
            // 
            this.TaskCreateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TaskCreateButton.Image = ((System.Drawing.Image)(resources.GetObject("TaskCreateButton.Image")));
            this.TaskCreateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TaskCreateButton.Name = "TaskCreateButton";
            this.TaskCreateButton.Size = new System.Drawing.Size(32, 22);
            this.TaskCreateButton.Text = "toolStripSplitButton1";
            this.TaskCreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // browserURL
            // 
            this.browserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.browserURL.Location = new System.Drawing.Point(160, 360);
            this.browserURL.Name = "browserURL";
            this.browserURL.ReadOnly = true;
            this.browserURL.Size = new System.Drawing.Size(240, 20);
            this.browserURL.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label12.Location = new System.Drawing.Point(0, 360);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 16);
            this.label12.TabIndex = 15;
            this.label12.Text = "View in browser";
            // 
            // ShowInBrowser
            // 
            this.ShowInBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowInBrowser.Location = new System.Drawing.Point(408, 360);
            this.ShowInBrowser.Name = "ShowInBrowser";
            this.ShowInBrowser.Size = new System.Drawing.Size(136, 20);
            this.ShowInBrowser.TabIndex = 17;
            this.ShowInBrowser.Text = "Show in browser";
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
            this.AddItemMenu.Size = new System.Drawing.Size(159, 92);
            // 
            // AddBuiltInFunctionMenu
            // 
            this.AddBuiltInFunctionMenu.Name = "AddBuiltInFunctionMenu";
            this.AddBuiltInFunctionMenu.Size = new System.Drawing.Size(158, 22);
            this.AddBuiltInFunctionMenu.Text = "Built in function";
            // 
            // AddCustomItemMenu
            // 
            this.AddCustomItemMenu.Name = "AddCustomItemMenu";
            this.AddCustomItemMenu.Size = new System.Drawing.Size(158, 22);
            this.AddCustomItemMenu.Text = "Custom item";
            // 
            // AddSubMenuItem
            // 
            this.AddSubMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("AddSubMenuItem.Image")));
            this.AddSubMenuItem.Name = "AddSubMenuItem";
            this.AddSubMenuItem.Size = new System.Drawing.Size(158, 22);
            this.AddSubMenuItem.Text = "Submenu";
            this.AddSubMenuItem.Click += new System.EventHandler(this.AddSubMenuItem_Click);
            // 
            // AddSeperatorItem
            // 
            this.AddSeperatorItem.Image = ((System.Drawing.Image)(resources.GetObject("AddSeperatorItem.Image")));
            this.AddSeperatorItem.Name = "AddSeperatorItem";
            this.AddSeperatorItem.Size = new System.Drawing.Size(158, 22);
            this.AddSeperatorItem.Text = "Seperator";
            this.AddSeperatorItem.Click += new System.EventHandler(this.AddSeperatorItem_Click);
            // 
            // CreateCommandMenu
            // 
            this.CreateCommandMenu.Name = "CreateCommandMenu";
            this.CreateCommandMenu.Size = new System.Drawing.Size(61, 4);
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
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(552, 672);
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
            this.Size = new System.Drawing.Size(552, 672);
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
			m_layout.InformationPane.Visible = m_layout.InformationPane.LegendVisible && m_layout.InformationPane.PropertiesVisible;
			m_editor.HasChanged();
		}

		private void ItemPropertiesCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating)
				return;
			m_layout.InformationPane.PropertiesVisible = ItemPropertiesCheck.Checked;
			m_layout.InformationPane.Visible = m_layout.InformationPane.LegendVisible && m_layout.InformationPane.PropertiesVisible;
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
			MessageBox.Show(this, m_globalizor.Translate("This function is not yet implemented"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			if (SharedComboDataSet == null)
			{
				System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "CommandTypesDataset.xml");
				byte[] buf = new byte[s.Length];
				if (s.Read(buf, 0, (int)s.Length) != s.Length)
					throw new Exception(m_globalizor.Translate("Failed while reading data from assembly"));
				SharedComboDataSet = buf;
			}

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

			using(System.IO.MemoryStream ms = new System.IO.MemoryStream(SharedComboDataSet))
				CommandTypesDataset.ReadXml(ms);

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
				throw new Exception(m_globalizor.Translate("Failed to get avalible command name"));

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
				MessageBox.Show(this, m_globalizor.Translate("The Taskbar does not support sub menus"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			OSGeo.MapGuide.MaestroAPI.FlyoutItemType fly = new OSGeo.MapGuide.MaestroAPI.FlyoutItemType();
			fly.Function = OSGeo.MapGuide.MaestroAPI.UIItemFunctionType.Flyout;

            fly.Label = m_globalizor.Translate("New submenu");
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

        private void AddButton_Click(object sender, EventArgs e)
        {
            (sender as ToolStripSplitButton).ShowDropDown();
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

        private void CreateButton_Click(object sender, EventArgs e)
        {
            (sender as ToolStripSplitButton).ShowDropDown();
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
	}
}
