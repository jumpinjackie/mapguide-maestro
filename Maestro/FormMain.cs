#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for FormMain.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
        private Hashtable m_userControls = new Hashtable();
		public System.Windows.Forms.TabControl tabItems;
		private System.Windows.Forms.ContextMenuStrip TreeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem PropertiesMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveXmlAsMenu;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TreeView ResourceTree;
        private System.Windows.Forms.ImageList toolbarImagesSmall;

		private Hashtable m_templateMenuIndex = null;

        private TreeNode m_clipboardBuffer = null;
		private bool m_clipboardCut = false;

		private SortedList m_Folders = null;
        private System.Windows.Forms.ToolStripSeparator menuItem1;
        private System.Windows.Forms.ToolStripSeparator menuItem7;
        private System.Windows.Forms.ToolStripMenuItem EditAsXmlMenu;
        private System.Windows.Forms.ToolStripMenuItem LoadFromXmlMenu;
        private System.Windows.Forms.ToolStripMenuItem CutMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyMenu;
        private System.Windows.Forms.ToolStripMenuItem PasteMenu;
		private System.Windows.Forms.Timer KeepAliveTimer;
        private SortedList m_Documents = null;
		private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItem3;
        private System.Windows.Forms.ToolStripSeparator menuItem11;
        private System.Windows.Forms.ToolStripSeparator menuItem17;
        private System.Windows.Forms.ToolStripSeparator menuItem20;
        private System.Windows.Forms.ToolStripSeparator menuItem22;
        private System.Windows.Forms.ToolStripMenuItem MainMenuNew;
        private System.Windows.Forms.ToolStripMenuItem MainMenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MainMenuClose;
        private System.Windows.Forms.ToolStripMenuItem MainMenuSave;
        private System.Windows.Forms.ToolStripMenuItem MainMenuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem MainMenuSaveAll;
        private System.Windows.Forms.ToolStripMenuItem MainMenuSaveAsXml;
        private System.Windows.Forms.ToolStripMenuItem MainMenuLoadFromXml;
        private System.Windows.Forms.ToolStripMenuItem MainMenuChangeServer;
        private System.Windows.Forms.ToolStripMenuItem MainMenuExit;
        private System.Windows.Forms.ToolStripMenuItem MainMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem MainMenuPaste;
        private System.Windows.Forms.ToolStripMenuItem MainMenuAbout;
        private System.Windows.Forms.ToolStripMenuItem MainMenuEditAsXml;
        private System.Windows.Forms.ToolStripMenuItem MainMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem MainMenuCut;
        private System.Windows.Forms.ToolStripSeparator menuItem4;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenu;
        private System.Windows.Forms.ToolStripMenuItem NewMenu;

		private ResourceEditorMap m_editors;
        private System.Windows.Forms.ToolStripMenuItem OpenSiteAdmin;
		private  Globalizator.Globalizator m_globalizor = null;
        private ToolStrip ResourceTreeToolbar;
        private ToolStripDropDownButton AddResourceButton;
        private ToolStripButton DeleteResourceButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ResourceTreeCopy;
        private ToolStripButton ResourceTreeCut;
        private ToolStripButton ResourceTreePaste;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ResourceTreeRefreshButton;
        private SplitContainer splitContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton SaveResourceButton;
        private ToolStripButton SaveResourceAsButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton PreviewButton;
        private ToolStripButton EditAsXmlButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton ClosePageButton;
        private ToolStripMenuItem MainMenuChangePreferences;
        private ToolStripMenuItem packagesToolStripMenuItem;
        private ToolStripMenuItem createPackageToolStripMenuItem;
        private ToolStripMenuItem modifyPackageToolStripMenuItem;
        private ToolStripMenuItem restorePackageToolStripMenuItem;
		private string m_lastSelectedNode = null;
        private ToolStripMenuItem viewLastExceptionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton ProfileButton;
        private ToolStripButton ValidateButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem CopyResourceIdMenu;
        private ContextMenuStrip TabPageContextMenu;
        private ToolStripMenuItem TabClosePageMenu;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem TabSaveMenu;
        private ToolStripMenuItem TabSaveAsMenu;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem TabCopyIdMenu;

        private Exception m_lastException;
        private ToolTip ResourceInfoTip;
        private Timer TooltipUpdateTimer;
        private ToolTip TabPageTooltip;
        private string m_lastTooltip;

        public FormMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//Re-assign to fix mono bug with button sizes
			ImageList tmp = ResourceTreeToolbar.ImageList;
			ResourceTreeToolbar.ImageList = null;
			ResourceTreeToolbar.ImageList = tmp;
			
			m_globalizor = new  Globalizator.Globalizator(this);
            this.Icon = FormMain.MaestroIcon;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ResourceTree = new System.Windows.Forms.TreeView();
            this.TreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyResourceIdMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.EditAsXmlMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFromXmlMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveXmlAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ResourceTreeToolbar = new System.Windows.Forms.ToolStrip();
            this.AddResourceButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.DeleteResourceButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ResourceTreeCopy = new System.Windows.Forms.ToolStripButton();
            this.ResourceTreeCut = new System.Windows.Forms.ToolStripButton();
            this.ResourceTreePaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ResourceTreeRefreshButton = new System.Windows.Forms.ToolStripButton();
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.tabItems = new System.Windows.Forms.TabControl();
            this.toolbarImagesSmall = new System.Windows.Forms.ImageList(this.components);
            this.KeepAliveTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuEditAsXml = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSaveAsXml = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuLoadFromXml = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem20 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuChangePreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuChangeServer = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenSiteAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem22 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.packagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restorePackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLastExceptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SaveResourceButton = new System.Windows.Forms.ToolStripButton();
            this.SaveResourceAsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.PreviewButton = new System.Windows.Forms.ToolStripButton();
            this.EditAsXmlButton = new System.Windows.Forms.ToolStripButton();
            this.ProfileButton = new System.Windows.Forms.ToolStripButton();
            this.ValidateButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ClosePageButton = new System.Windows.Forms.ToolStripButton();
            this.TabPageContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TabClosePageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.TabSaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TabSaveAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.TabCopyIdMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ResourceInfoTip = new System.Windows.Forms.ToolTip(this.components);
            this.TooltipUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.TabPageTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.TreeContextMenu.SuspendLayout();
            this.ResourceTreeToolbar.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.TabPageContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResourceTree
            // 
            this.ResourceTree.AllowDrop = true;
            this.ResourceTree.ContextMenuStrip = this.TreeContextMenu;
            this.ResourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceTree.LabelEdit = true;
            this.ResourceTree.Location = new System.Drawing.Point(0, 39);
            this.ResourceTree.Name = "ResourceTree";
            this.ResourceTree.Size = new System.Drawing.Size(278, 391);
            this.ResourceTree.TabIndex = 0;
            this.ResourceTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_AfterLabelEdit);
            this.ResourceTree.DoubleClick += new System.EventHandler(this.ResourceTree_DoubleClick);
            this.ResourceTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragDrop);
            this.ResourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterSelect);
            this.ResourceTree.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ResourceTree_MouseMove);
            this.ResourceTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResourceTree_MouseDown);
            this.ResourceTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ResourceTree_KeyUp);
            this.ResourceTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_BeforeLabelEdit);
            this.ResourceTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourceTree_ItemDrag);
            this.ResourceTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragOver);
            // 
            // TreeContextMenu
            // 
            this.TreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PropertiesMenu,
            this.toolStripSeparator6,
            this.CopyResourceIdMenu,
            this.menuItem7,
            this.EditAsXmlMenu,
            this.LoadFromXmlMenu,
            this.SaveXmlAsMenu,
            this.menuItem1,
            this.CutMenu,
            this.CopyMenu,
            this.PasteMenu,
            this.menuItem4,
            this.DeleteMenu,
            this.NewMenu});
            this.TreeContextMenu.Name = "TreeContextMenu";
            this.TreeContextMenu.Size = new System.Drawing.Size(181, 248);
            this.TreeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TreeContextMenu_Popup);
            // 
            // PropertiesMenu
            // 
            this.PropertiesMenu.Name = "PropertiesMenu";
            this.PropertiesMenu.Size = new System.Drawing.Size(180, 22);
            this.PropertiesMenu.Text = "Properties";
            this.PropertiesMenu.Click += new System.EventHandler(this.PropertiesMenu_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(177, 6);
            // 
            // CopyResourceIdMenu
            // 
            this.CopyResourceIdMenu.Name = "CopyResourceIdMenu";
            this.CopyResourceIdMenu.Size = new System.Drawing.Size(180, 22);
            this.CopyResourceIdMenu.Text = "Copy id to clipboard";
            this.CopyResourceIdMenu.ToolTipText = "Copies the currently selected resource id to the clipboard";
            this.CopyResourceIdMenu.Click += new System.EventHandler(this.CopyResourceIdMenu_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Size = new System.Drawing.Size(177, 6);
            // 
            // EditAsXmlMenu
            // 
            this.EditAsXmlMenu.Name = "EditAsXmlMenu";
            this.EditAsXmlMenu.Size = new System.Drawing.Size(180, 22);
            this.EditAsXmlMenu.Text = "Edit as xml";
            this.EditAsXmlMenu.Click += new System.EventHandler(this.EditAsXmlMenu_Click);
            // 
            // LoadFromXmlMenu
            // 
            this.LoadFromXmlMenu.Name = "LoadFromXmlMenu";
            this.LoadFromXmlMenu.Size = new System.Drawing.Size(180, 22);
            this.LoadFromXmlMenu.Text = "Load from Xml...";
            this.LoadFromXmlMenu.Click += new System.EventHandler(this.LoadFromXmlMenu_Click);
            // 
            // SaveXmlAsMenu
            // 
            this.SaveXmlAsMenu.Name = "SaveXmlAsMenu";
            this.SaveXmlAsMenu.Size = new System.Drawing.Size(180, 22);
            this.SaveXmlAsMenu.Text = "Save Xml As...";
            this.SaveXmlAsMenu.Click += new System.EventHandler(this.SaveXmlAsMenu_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // CutMenu
            // 
            this.CutMenu.Name = "CutMenu";
            this.CutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.CutMenu.Size = new System.Drawing.Size(180, 22);
            this.CutMenu.Text = "Cut";
            this.CutMenu.Click += new System.EventHandler(this.CutMenu_Click);
            // 
            // CopyMenu
            // 
            this.CopyMenu.Name = "CopyMenu";
            this.CopyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyMenu.Size = new System.Drawing.Size(180, 22);
            this.CopyMenu.Text = "Copy";
            this.CopyMenu.Click += new System.EventHandler(this.CopyMenu_Click);
            // 
            // PasteMenu
            // 
            this.PasteMenu.Name = "PasteMenu";
            this.PasteMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.PasteMenu.Size = new System.Drawing.Size(180, 22);
            this.PasteMenu.Text = "Paste";
            this.PasteMenu.Click += new System.EventHandler(this.PasteMenu_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Name = "menuItem4";
            this.menuItem4.Size = new System.Drawing.Size(177, 6);
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Name = "DeleteMenu";
            this.DeleteMenu.Size = new System.Drawing.Size(180, 22);
            this.DeleteMenu.Text = "Delete";
            this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
            // 
            // NewMenu
            // 
            this.NewMenu.Name = "NewMenu";
            this.NewMenu.Size = new System.Drawing.Size(180, 22);
            this.NewMenu.Text = "New";
            // 
            // ResourceTreeToolbar
            // 
            this.ResourceTreeToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ResourceTreeToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddResourceButton,
            this.DeleteResourceButton,
            this.toolStripSeparator1,
            this.ResourceTreeCopy,
            this.ResourceTreeCut,
            this.ResourceTreePaste,
            this.toolStripSeparator2,
            this.ResourceTreeRefreshButton});
            this.ResourceTreeToolbar.Location = new System.Drawing.Point(0, 0);
            this.ResourceTreeToolbar.Name = "ResourceTreeToolbar";
            this.ResourceTreeToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ResourceTreeToolbar.Size = new System.Drawing.Size(278, 39);
            this.ResourceTreeToolbar.TabIndex = 2;
            // 
            // AddResourceButton
            // 
            this.AddResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddResourceButton.Image = ((System.Drawing.Image)(resources.GetObject("AddResourceButton.Image")));
            this.AddResourceButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AddResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddResourceButton.Name = "AddResourceButton";
            this.AddResourceButton.Size = new System.Drawing.Size(45, 36);
            this.AddResourceButton.ToolTipText = "Creates a new resource";
            // 
            // DeleteResourceButton
            // 
            this.DeleteResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteResourceButton.Enabled = false;
            this.DeleteResourceButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteResourceButton.Image")));
            this.DeleteResourceButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DeleteResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteResourceButton.Name = "DeleteResourceButton";
            this.DeleteResourceButton.Size = new System.Drawing.Size(36, 36);
            this.DeleteResourceButton.ToolTipText = "Deletes the selected resource or folder";
            this.DeleteResourceButton.Click += new System.EventHandler(this.DeleteResourceButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // ResourceTreeCopy
            // 
            this.ResourceTreeCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResourceTreeCopy.Enabled = false;
            this.ResourceTreeCopy.Image = ((System.Drawing.Image)(resources.GetObject("ResourceTreeCopy.Image")));
            this.ResourceTreeCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ResourceTreeCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResourceTreeCopy.Name = "ResourceTreeCopy";
            this.ResourceTreeCopy.Size = new System.Drawing.Size(36, 36);
            this.ResourceTreeCopy.ToolTipText = "Copies the current resource or folder to the clipboard";
            this.ResourceTreeCopy.Click += new System.EventHandler(this.ResourceTreeCopy_Click);
            // 
            // ResourceTreeCut
            // 
            this.ResourceTreeCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResourceTreeCut.Enabled = false;
            this.ResourceTreeCut.Image = ((System.Drawing.Image)(resources.GetObject("ResourceTreeCut.Image")));
            this.ResourceTreeCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ResourceTreeCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResourceTreeCut.Name = "ResourceTreeCut";
            this.ResourceTreeCut.Size = new System.Drawing.Size(36, 36);
            this.ResourceTreeCut.ToolTipText = "Cuts the current resource or folder to the clipboard";
            this.ResourceTreeCut.Click += new System.EventHandler(this.ResourceTreeCut_Click);
            // 
            // ResourceTreePaste
            // 
            this.ResourceTreePaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResourceTreePaste.Enabled = false;
            this.ResourceTreePaste.Image = ((System.Drawing.Image)(resources.GetObject("ResourceTreePaste.Image")));
            this.ResourceTreePaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ResourceTreePaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResourceTreePaste.Name = "ResourceTreePaste";
            this.ResourceTreePaste.Size = new System.Drawing.Size(36, 36);
            this.ResourceTreePaste.ToolTipText = "Pastes the current content of the clipboard";
            this.ResourceTreePaste.Click += new System.EventHandler(this.ResourceTreePaste_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // ResourceTreeRefreshButton
            // 
            this.ResourceTreeRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResourceTreeRefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("ResourceTreeRefreshButton.Image")));
            this.ResourceTreeRefreshButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ResourceTreeRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResourceTreeRefreshButton.Name = "ResourceTreeRefreshButton";
            this.ResourceTreeRefreshButton.Size = new System.Drawing.Size(36, 36);
            this.ResourceTreeRefreshButton.ToolTipText = "Refreshes the tree to match the current server state";
            this.ResourceTreeRefreshButton.Click += new System.EventHandler(this.ResourceTreeRefreshButton_Click);
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
            this.toolbarImages.Images.SetKeyName(12, "");
            // 
            // tabItems
            // 
            this.tabItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabItems.ItemSize = new System.Drawing.Size(0, 18);
            this.tabItems.Location = new System.Drawing.Point(0, 39);
            this.tabItems.Name = "tabItems";
            this.tabItems.SelectedIndex = 0;
            this.tabItems.Size = new System.Drawing.Size(418, 391);
            this.tabItems.TabIndex = 1;
            this.tabItems.MouseLeave += new System.EventHandler(this.tabItems_MouseLeave);
            this.tabItems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tabItems_MouseMove);
            this.tabItems.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabItems_ControlAdded);
            this.tabItems.TabIndexChanged += new System.EventHandler(this.tabItems_SelectedIndexChanged);
            this.tabItems.SelectedIndexChanged += new System.EventHandler(this.tabItems_SelectedIndexChanged);
            // 
            // toolbarImagesSmall
            // 
            this.toolbarImagesSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImagesSmall.ImageStream")));
            this.toolbarImagesSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImagesSmall.Images.SetKeyName(0, "");
            this.toolbarImagesSmall.Images.SetKeyName(1, "");
            this.toolbarImagesSmall.Images.SetKeyName(2, "");
            this.toolbarImagesSmall.Images.SetKeyName(3, "");
            this.toolbarImagesSmall.Images.SetKeyName(4, "");
            this.toolbarImagesSmall.Images.SetKeyName(5, "");
            this.toolbarImagesSmall.Images.SetKeyName(6, "");
            this.toolbarImagesSmall.Images.SetKeyName(7, "");
            this.toolbarImagesSmall.Images.SetKeyName(8, "");
            // 
            // KeepAliveTimer
            // 
            this.KeepAliveTimer.Interval = 300000;
            this.KeepAliveTimer.Tick += new System.EventHandler(this.KeepAliveTimer_Tick);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem2,
            this.MainMenuEdit,
            this.packagesToolStripMenuItem,
            this.menuItem3});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenu.Size = new System.Drawing.Size(704, 24);
            this.MainMenu.TabIndex = 3;
            // 
            // menuItem2
            // 
            this.menuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuNew,
            this.MainMenuOpen,
            this.MainMenuClose,
            this.menuItem17,
            this.MainMenuSave,
            this.MainMenuSaveAs,
            this.MainMenuSaveAll,
            this.menuItem11,
            this.MainMenuEditAsXml,
            this.MainMenuSaveAsXml,
            this.MainMenuLoadFromXml,
            this.menuItem20,
            this.MainMenuChangePreferences,
            this.MainMenuChangeServer,
            this.OpenSiteAdmin,
            this.menuItem22,
            this.MainMenuExit});
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(35, 20);
            this.menuItem2.Text = "File";
            // 
            // MainMenuNew
            // 
            this.MainMenuNew.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuNew.Image")));
            this.MainMenuNew.Name = "MainMenuNew";
            this.MainMenuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.MainMenuNew.Size = new System.Drawing.Size(211, 22);
            this.MainMenuNew.Text = "New";
            this.MainMenuNew.Click += new System.EventHandler(this.MainMenuNew_Click);
            // 
            // MainMenuOpen
            // 
            this.MainMenuOpen.Name = "MainMenuOpen";
            this.MainMenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MainMenuOpen.Size = new System.Drawing.Size(211, 22);
            this.MainMenuOpen.Text = "Open";
            this.MainMenuOpen.Click += new System.EventHandler(this.MainMenuOpen_Click);
            // 
            // MainMenuClose
            // 
            this.MainMenuClose.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuClose.Image")));
            this.MainMenuClose.Name = "MainMenuClose";
            this.MainMenuClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.MainMenuClose.Size = new System.Drawing.Size(211, 22);
            this.MainMenuClose.Text = "Close";
            this.MainMenuClose.Click += new System.EventHandler(this.MainMenuClose_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Name = "menuItem17";
            this.menuItem17.Size = new System.Drawing.Size(208, 6);
            // 
            // MainMenuSave
            // 
            this.MainMenuSave.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuSave.Image")));
            this.MainMenuSave.Name = "MainMenuSave";
            this.MainMenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MainMenuSave.Size = new System.Drawing.Size(211, 22);
            this.MainMenuSave.Text = "Save";
            this.MainMenuSave.Click += new System.EventHandler(this.MainMenuSave_Click);
            // 
            // MainMenuSaveAs
            // 
            this.MainMenuSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuSaveAs.Image")));
            this.MainMenuSaveAs.Name = "MainMenuSaveAs";
            this.MainMenuSaveAs.Size = new System.Drawing.Size(211, 22);
            this.MainMenuSaveAs.Text = "Save as...";
            this.MainMenuSaveAs.Click += new System.EventHandler(this.MainMenuSaveAs_Click);
            // 
            // MainMenuSaveAll
            // 
            this.MainMenuSaveAll.Name = "MainMenuSaveAll";
            this.MainMenuSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.MainMenuSaveAll.Size = new System.Drawing.Size(211, 22);
            this.MainMenuSaveAll.Text = "Save all";
            this.MainMenuSaveAll.Click += new System.EventHandler(this.MainMenuSaveAll_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            this.menuItem11.Size = new System.Drawing.Size(208, 6);
            // 
            // MainMenuEditAsXml
            // 
            this.MainMenuEditAsXml.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuEditAsXml.Image")));
            this.MainMenuEditAsXml.Name = "MainMenuEditAsXml";
            this.MainMenuEditAsXml.Size = new System.Drawing.Size(211, 22);
            this.MainMenuEditAsXml.Text = "Edit as Xml";
            this.MainMenuEditAsXml.Click += new System.EventHandler(this.MainMenuEditAsXml_Click);
            // 
            // MainMenuSaveAsXml
            // 
            this.MainMenuSaveAsXml.Name = "MainMenuSaveAsXml";
            this.MainMenuSaveAsXml.Size = new System.Drawing.Size(211, 22);
            this.MainMenuSaveAsXml.Text = "Save as Xml...";
            this.MainMenuSaveAsXml.Click += new System.EventHandler(this.MainMenuSaveAsXml_Click);
            // 
            // MainMenuLoadFromXml
            // 
            this.MainMenuLoadFromXml.Name = "MainMenuLoadFromXml";
            this.MainMenuLoadFromXml.Size = new System.Drawing.Size(211, 22);
            this.MainMenuLoadFromXml.Text = "Load from Xml...";
            this.MainMenuLoadFromXml.Click += new System.EventHandler(this.MainMenuLoadFromXml_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Name = "menuItem20";
            this.menuItem20.Size = new System.Drawing.Size(208, 6);
            // 
            // MainMenuChangePreferences
            // 
            this.MainMenuChangePreferences.Name = "MainMenuChangePreferences";
            this.MainMenuChangePreferences.Size = new System.Drawing.Size(211, 22);
            this.MainMenuChangePreferences.Text = "Preferences...";
            this.MainMenuChangePreferences.Click += new System.EventHandler(this.MainMenuChangePreferences_Click);
            // 
            // MainMenuChangeServer
            // 
            this.MainMenuChangeServer.Name = "MainMenuChangeServer";
            this.MainMenuChangeServer.Size = new System.Drawing.Size(211, 22);
            this.MainMenuChangeServer.Text = "Change server...";
            this.MainMenuChangeServer.Click += new System.EventHandler(this.MainMenuChangeServer_Click);
            // 
            // OpenSiteAdmin
            // 
            this.OpenSiteAdmin.Name = "OpenSiteAdmin";
            this.OpenSiteAdmin.Size = new System.Drawing.Size(211, 22);
            this.OpenSiteAdmin.Text = "Open Site Administrator...";
            this.OpenSiteAdmin.Click += new System.EventHandler(this.OpenSiteAdmin_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Name = "menuItem22";
            this.menuItem22.Size = new System.Drawing.Size(208, 6);
            // 
            // MainMenuExit
            // 
            this.MainMenuExit.Name = "MainMenuExit";
            this.MainMenuExit.Size = new System.Drawing.Size(211, 22);
            this.MainMenuExit.Text = "Exit";
            this.MainMenuExit.Click += new System.EventHandler(this.MainMenuExit_Click);
            // 
            // MainMenuEdit
            // 
            this.MainMenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuCut,
            this.MainMenuCopy,
            this.MainMenuPaste});
            this.MainMenuEdit.Name = "MainMenuEdit";
            this.MainMenuEdit.Size = new System.Drawing.Size(37, 20);
            this.MainMenuEdit.Text = "Edit";
            // 
            // MainMenuCut
            // 
            this.MainMenuCut.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuCut.Image")));
            this.MainMenuCut.Name = "MainMenuCut";
            this.MainMenuCut.Size = new System.Drawing.Size(112, 22);
            this.MainMenuCut.Text = "Cut";
            this.MainMenuCut.Click += new System.EventHandler(this.MainMenuCut_Click);
            // 
            // MainMenuCopy
            // 
            this.MainMenuCopy.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuCopy.Image")));
            this.MainMenuCopy.Name = "MainMenuCopy";
            this.MainMenuCopy.Size = new System.Drawing.Size(112, 22);
            this.MainMenuCopy.Text = "Copy";
            this.MainMenuCopy.Click += new System.EventHandler(this.MainMenuCopy_Click);
            // 
            // MainMenuPaste
            // 
            this.MainMenuPaste.Image = ((System.Drawing.Image)(resources.GetObject("MainMenuPaste.Image")));
            this.MainMenuPaste.Name = "MainMenuPaste";
            this.MainMenuPaste.Size = new System.Drawing.Size(112, 22);
            this.MainMenuPaste.Text = "Paste";
            this.MainMenuPaste.Click += new System.EventHandler(this.MainMenuPaste_Click);
            // 
            // packagesToolStripMenuItem
            // 
            this.packagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPackageToolStripMenuItem,
            this.modifyPackageToolStripMenuItem,
            this.restorePackageToolStripMenuItem});
            this.packagesToolStripMenuItem.Name = "packagesToolStripMenuItem";
            this.packagesToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.packagesToolStripMenuItem.Text = "Packages";
            // 
            // createPackageToolStripMenuItem
            // 
            this.createPackageToolStripMenuItem.Name = "createPackageToolStripMenuItem";
            this.createPackageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.createPackageToolStripMenuItem.Text = "Create package...";
            this.createPackageToolStripMenuItem.Click += new System.EventHandler(this.createPackageToolStripMenuItem_Click);
            // 
            // modifyPackageToolStripMenuItem
            // 
            this.modifyPackageToolStripMenuItem.Name = "modifyPackageToolStripMenuItem";
            this.modifyPackageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.modifyPackageToolStripMenuItem.Text = "Modify package...";
            this.modifyPackageToolStripMenuItem.Click += new System.EventHandler(this.modifyPackageToolStripMenuItem_Click);
            // 
            // restorePackageToolStripMenuItem
            // 
            this.restorePackageToolStripMenuItem.Name = "restorePackageToolStripMenuItem";
            this.restorePackageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.restorePackageToolStripMenuItem.Text = "Restore package...";
            this.restorePackageToolStripMenuItem.Click += new System.EventHandler(this.restorePackageToolStripMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLastExceptionToolStripMenuItem,
            this.toolStripSeparator5,
            this.MainMenuAbout});
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(40, 20);
            this.menuItem3.Text = "Help";
            // 
            // viewLastExceptionToolStripMenuItem
            // 
            this.viewLastExceptionToolStripMenuItem.Name = "viewLastExceptionToolStripMenuItem";
            this.viewLastExceptionToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.viewLastExceptionToolStripMenuItem.Text = "Last exception...";
            this.viewLastExceptionToolStripMenuItem.Click += new System.EventHandler(this.viewLastExceptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(164, 6);
            // 
            // MainMenuAbout
            // 
            this.MainMenuAbout.Name = "MainMenuAbout";
            this.MainMenuAbout.Size = new System.Drawing.Size(167, 22);
            this.MainMenuAbout.Text = "About...";
            this.MainMenuAbout.Click += new System.EventHandler(this.MainMenuAbout_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ResourceTree);
            this.splitContainer1.Panel1.Controls.Add(this.ResourceTreeToolbar);
            this.splitContainer1.Panel1MinSize = 242;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabItems);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(704, 430);
            this.splitContainer1.SplitterDistance = 278;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveResourceButton,
            this.SaveResourceAsButton,
            this.toolStripSeparator3,
            this.PreviewButton,
            this.EditAsXmlButton,
            this.ProfileButton,
            this.ValidateButton,
            this.toolStripSeparator4,
            this.ClosePageButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(418, 39);
            this.toolStrip1.TabIndex = 2;
            // 
            // SaveResourceButton
            // 
            this.SaveResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveResourceButton.Enabled = false;
            this.SaveResourceButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveResourceButton.Image")));
            this.SaveResourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveResourceButton.Name = "SaveResourceButton";
            this.SaveResourceButton.Size = new System.Drawing.Size(36, 36);
            this.SaveResourceButton.ToolTipText = "Saves the current resource";
            this.SaveResourceButton.Click += new System.EventHandler(this.SaveResourceButton_Click);
            // 
            // SaveResourceAsButton
            // 
            this.SaveResourceAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveResourceAsButton.Enabled = false;
            this.SaveResourceAsButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveResourceAsButton.Image")));
            this.SaveResourceAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveResourceAsButton.Name = "SaveResourceAsButton";
            this.SaveResourceAsButton.Size = new System.Drawing.Size(36, 36);
            this.SaveResourceAsButton.ToolTipText = "Saves the current resource under a different name";
            this.SaveResourceAsButton.Click += new System.EventHandler(this.SaveResourceAsButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // PreviewButton
            // 
            this.PreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PreviewButton.Enabled = false;
            this.PreviewButton.Image = ((System.Drawing.Image)(resources.GetObject("PreviewButton.Image")));
            this.PreviewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(36, 36);
            this.PreviewButton.ToolTipText = "Preview the item";
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // EditAsXmlButton
            // 
            this.EditAsXmlButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditAsXmlButton.Enabled = false;
            this.EditAsXmlButton.Image = ((System.Drawing.Image)(resources.GetObject("EditAsXmlButton.Image")));
            this.EditAsXmlButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditAsXmlButton.Name = "EditAsXmlButton";
            this.EditAsXmlButton.Size = new System.Drawing.Size(36, 36);
            this.EditAsXmlButton.ToolTipText = "Edits the current resource in an xml editor";
            this.EditAsXmlButton.Click += new System.EventHandler(this.EditAsXmlButton_Click);
            // 
            // ProfileButton
            // 
            this.ProfileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ProfileButton.Enabled = false;
            this.ProfileButton.Image = ((System.Drawing.Image)(resources.GetObject("ProfileButton.Image")));
            this.ProfileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ProfileButton.Name = "ProfileButton";
            this.ProfileButton.Size = new System.Drawing.Size(36, 36);
            this.ProfileButton.ToolTipText = "Provides timing details on the current resource";
            this.ProfileButton.Click += new System.EventHandler(this.ProfileButton_Click);
            // 
            // ValidateButton
            // 
            this.ValidateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ValidateButton.Enabled = false;
            this.ValidateButton.Image = ((System.Drawing.Image)(resources.GetObject("ValidateButton.Image")));
            this.ValidateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(36, 36);
            this.ValidateButton.ToolTipText = "Validates the current resource against common errors";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 39);
            // 
            // ClosePageButton
            // 
            this.ClosePageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClosePageButton.Enabled = false;
            this.ClosePageButton.Image = ((System.Drawing.Image)(resources.GetObject("ClosePageButton.Image")));
            this.ClosePageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClosePageButton.Name = "ClosePageButton";
            this.ClosePageButton.Size = new System.Drawing.Size(36, 36);
            this.ClosePageButton.ToolTipText = "Close the current page";
            this.ClosePageButton.Click += new System.EventHandler(this.ClosePageButton_Click);
            // 
            // TabPageContextMenu
            // 
            this.TabPageContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TabClosePageMenu,
            this.toolStripSeparator7,
            this.TabSaveMenu,
            this.TabSaveAsMenu,
            this.toolStripSeparator8,
            this.TabCopyIdMenu});
            this.TabPageContextMenu.Name = "TabPageContextMenu";
            this.TabPageContextMenu.Size = new System.Drawing.Size(181, 104);
            this.TabPageContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TabPageContextMenu_Opening);
            // 
            // TabClosePageMenu
            // 
            this.TabClosePageMenu.Name = "TabClosePageMenu";
            this.TabClosePageMenu.Size = new System.Drawing.Size(180, 22);
            this.TabClosePageMenu.Text = "Close";
            this.TabClosePageMenu.Click += new System.EventHandler(this.TabClosePageMenu_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(177, 6);
            // 
            // TabSaveMenu
            // 
            this.TabSaveMenu.Name = "TabSaveMenu";
            this.TabSaveMenu.Size = new System.Drawing.Size(180, 22);
            this.TabSaveMenu.Text = "Save";
            this.TabSaveMenu.Click += new System.EventHandler(this.TabSaveMenu_Click);
            // 
            // TabSaveAsMenu
            // 
            this.TabSaveAsMenu.Name = "TabSaveAsMenu";
            this.TabSaveAsMenu.Size = new System.Drawing.Size(180, 22);
            this.TabSaveAsMenu.Text = "Save as...";
            this.TabSaveAsMenu.Click += new System.EventHandler(this.TabSaveAsMenu_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(177, 6);
            // 
            // TabCopyIdMenu
            // 
            this.TabCopyIdMenu.Name = "TabCopyIdMenu";
            this.TabCopyIdMenu.Size = new System.Drawing.Size(180, 22);
            this.TabCopyIdMenu.Text = "Copy id to clipboard";
            this.TabCopyIdMenu.Click += new System.EventHandler(this.TabCopyIdMenu_Click);
            // 
            // ResourceInfoTip
            // 
            this.ResourceInfoTip.AutomaticDelay = 1000;
            this.ResourceInfoTip.Popup += new System.Windows.Forms.PopupEventHandler(this.ResourceInfoTip_Popup);
            // 
            // TooltipUpdateTimer
            // 
            this.TooltipUpdateTimer.Interval = 1000;
            this.TooltipUpdateTimer.Tick += new System.EventHandler(this.TooltipUpdateTimer_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 454);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "FormMain";
            this.Text = "MapGuide Maestro";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormMain_Closing);
            this.TreeContextMenu.ResumeLayout(false);
            this.ResourceTreeToolbar.ResumeLayout(false);
            this.ResourceTreeToolbar.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TabPageContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void FormMain_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (Program.ApplicationSettings.MaximizedWindow)
                    this.WindowState = FormWindowState.Maximized;
                else
                {
                    if (Program.ApplicationSettings.WindowWidth >= 100 && Program.ApplicationSettings.WindowHeight >= 100)
                    {
                        Screen s = Screen.FromControl(this);
                        if (s != null)
                        {
                            Rectangle r = new Rectangle(Program.ApplicationSettings.WindowLeft, Program.ApplicationSettings.WindowTop, Program.ApplicationSettings.WindowWidth, Program.ApplicationSettings.WindowHeight);
                            if (s.WorkingArea.Contains(r))
                            {
                                this.Width = Program.ApplicationSettings.WindowWidth;
                                this.Height = Program.ApplicationSettings.WindowHeight;
                                this.Left = Program.ApplicationSettings.WindowLeft;
                                this.Top = Program.ApplicationSettings.WindowTop;
                            }
                            else
                            {
                                r = new Rectangle(0, 0, Program.ApplicationSettings.WindowWidth, Program.ApplicationSettings.WindowHeight);
                                if (s.WorkingArea.Contains(r))
                                {
                                    this.Width = Program.ApplicationSettings.WindowWidth;
                                    this.Height = Program.ApplicationSettings.WindowHeight;
                                }

                                if (s.WorkingArea.Contains(Program.ApplicationSettings.WindowLeft, Program.ApplicationSettings.WindowTop))
                                {
                                    this.Left = Program.ApplicationSettings.WindowLeft;
                                    this.Top = Program.ApplicationSettings.WindowTop;
                                }
                            }
                        }
                    }
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch
            {
            }



			this.Show();

            //Register these after any pre-load stuff
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.Move += new System.EventHandler(this.FormMain_Move);

			FormLogin frm = new FormLogin();
            frm.StartPosition = FormStartPosition.CenterParent;
			frm.UseAutoConnect = true;

			if (frm.ShowDialog(this) == DialogResult.OK)
				m_connection = frm.Connection;
			else
			{
				Application.Exit();
				return;
			}

            KeepAliveTimer.Enabled = true;
			string editorMap = System.IO.Path.Combine(Application.StartupPath, "EditorMap.xml");
			if (!System.IO.File.Exists(editorMap))
			{
				MessageBox.Show(string.Format(m_globalizor.Translate("The editor setup file could not be located.\nIt should be placed in: {0}"), editorMap), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}

			try
			{
                m_editors = new ResourceEditorMap(System.IO.Path.Combine(Application.StartupPath, "EditorMap.xml"));
			}
			catch(Exception ex)
			{
				MessageBox.Show(string.Format(m_globalizor.Translate("Failed to load editor setup: {0}") , ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}

			this.ResourceTree.ImageList = m_editors.SmallImageList;
			this.tabItems.ImageList = m_editors.SmallImageList;

            //TODO: This should be case insensitive for linux
			string templatepath = System.IO.Path.Combine(Application.StartupPath, "Templates");
			m_templateMenuIndex = new Hashtable();

			MainMenuNew.DropDown.Items.Clear();
            MainMenuNew.DropDown.Items.Add(new ToolStripMenuItem(m_globalizor.Translate("Folder"), m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            MainMenuNew.DropDown.Items.Add(new ToolStripSeparator());

            NewMenu.DropDown.Items.Clear();
            NewMenu.DropDown.Items.Add(new ToolStripMenuItem(m_globalizor.Translate("Folder"), m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            NewMenu.DropDown.Items.Add(new ToolStripSeparator());

            AddResourceButton.DropDown.Items.Clear();
            AddResourceButton.DropDown.Items.Add(new ToolStripMenuItem(m_globalizor.Translate("Folder"), m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            AddResourceButton.DropDown.Items.Add(new ToolStripSeparator());

			if (System.IO.Directory.Exists(templatepath))
			{
				foreach(string file in System.IO.Directory.GetFiles(templatepath))
				{
                    string name = System.IO.Path.GetFileNameWithoutExtension(file);
                    ToolStripMenuItem menu = new ToolStripMenuItem(m_globalizor.Translate(name), m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					AddResourceButton.DropDown.Items.Add(menu);

                    menu = new ToolStripMenuItem(m_globalizor.Translate(name), m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					MainMenuNew.DropDown.Items.Add(menu);

                    menu = new ToolStripMenuItem(m_globalizor.Translate(name), m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					NewMenu.DropDown.Items.Add(menu);
				}
			}

			this.Refresh();
			RebuildDocumentTree();
		}

		private TreeNodeCollection FindParent(string resourceID)
		{
			string [] parts = m_editors.SplitResourceID(resourceID);
			TreeNodeCollection current = ResourceTree.Nodes[0].Nodes;
			for(int i = 0; i < parts.Length - 1; i++)
			{
				bool found = false;
				foreach(TreeNode n in current)
					if (n.Text == parts[i])
					{
						current = n.Nodes;
						found = true;
						break;
					}

				if (!found)
					throw new Exception(string.Format(m_globalizor.Translate("Failed to find node with name {0}, while looking for: {1}"), parts[i], resourceID));
			}

			return current;
		}

		private TreeNode FindItem(string resourceID)
		{
			TreeNodeCollection parent = FindParent(resourceID);
			string[] parts = m_editors.SplitResourceID(resourceID);
			foreach(TreeNode n in parent)
				if (n.Text == parts[parts.Length-1])
					return n;
			throw new Exception(string.Format(m_globalizor.Translate("Item not found: {0}"), resourceID));
		}

		public string SelectedPath
		{
			get 
			{
				string path = "Library://";
				if (ResourceTree.SelectedNode != null)
					path += ResourceTree.SelectedNode.FullPath;
				return path;
			}
		}

		private void FindOpenNodes(TreeNodeCollection nodes, ArrayList opennodes)
		{
			foreach(TreeNode n in nodes)
				if (n.IsExpanded)
				{
					opennodes.Add(n.FullPath);
					FindOpenNodes(n.Nodes, opennodes);
				}
		}

		public void RebuildDocumentTree()
		{
			using(new WaitCursor(this))
			{
				TreeNode parentnode = ResourceTree.SelectedNode;
				string parentnodepath = null;
				string actualnodepath = null;
				int parentnodeindex = -1;

				if (parentnode != null)
					parentnodeindex = parentnode.Index;
				if (parentnode != null && parentnode.Parent != null)
					parentnodepath = parentnode.Parent.FullPath;

				if (ResourceTree.SelectedNode != null)
					actualnodepath = ResourceTree.SelectedNode.FullPath;
			

				ArrayList opennodes = new ArrayList();
				FindOpenNodes(ResourceTree.Nodes, opennodes);

				OSGeo.MapGuide.MaestroAPI.ResourceList lst = m_connection.GetRepositoryResources();
				ResourceTree.Nodes.Clear();

				TreeNode rootnode = new TreeNode(m_connection.DisplayName, m_editors.ServerIcon, m_editors.ServerIcon);
				ResourceTree.Nodes.Add(rootnode);

				m_Folders = new SortedList();
				m_Documents = new SortedList();
			
				//Sort items
				foreach(object o in lst.Items)
					if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
						m_Folders.Add(((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)o).ResourceId, o);
					else if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
					{
						OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)o;
						m_Documents.Add((m_editors.GetImageIndexFromResourceID(document.ResourceId)).ToString() + "-" + document.ResourceId, document);
					}

				//Build tree with folders first, so all placeholders are ready
				foreach(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder folder in m_Folders.Values)
				{
					//Skip the root folder
					if (folder.ResourceId == "Library://")
						continue;

					TreeNode n = new TreeNode();
					n.Text = m_editors.GetResourceNameFromResourceID(folder.ResourceId);
					n.Tag = folder;
					n.ImageIndex = n.SelectedImageIndex = m_editors.FolderIcon;
					FindParent(folder.ResourceId).Add(n);
				}


				//Pouplate with documents
				foreach(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document in m_Documents.Values)
				{					
					TreeNode n = new TreeNode();
					n.Text = m_editors.GetResourceNameFromResourceID(document.ResourceId);
					n.Tag = document;
					n.ImageIndex = n.SelectedImageIndex = m_editors.GetImageIndexFromResourceID(document.ResourceId);
					FindParent(document.ResourceId).Add(n);
				}

				rootnode.Expand();

				//TODO: Remove any open resources that do not exist anymore
			
				foreach(string s in opennodes)
				{
					TreeNode n = TreeViewUtil.FindItemExact(ResourceTree, s);
					if (n != null)
						n.Expand();
				}

				if (actualnodepath != null)
				{
					TreeNode n = TreeViewUtil.FindItemExact(ResourceTree, actualnodepath);
					if (n != null)
					{
						ResourceTree.SelectedNode = n;
					}
					else
					{
						if (parentnodepath == null && ResourceTree.Nodes.Count > 0)
							ResourceTree.SelectedNode = ResourceTree.Nodes[Math.Min(ResourceTree.Nodes.Count - 1, parentnodeindex)];
						else
						{
							n = TreeViewUtil.FindItemExact(ResourceTree, parentnodepath);
							if (n != null)
								if (n.Nodes.Count > 0)
									ResourceTree.SelectedNode = n.Nodes[Math.Min(n.Nodes.Count - 1, parentnodeindex)];
								else
									ResourceTree.SelectedNode = n;
						}

					}
				}
			}
				
		}


		public OSGeo.MapGuide.MaestroAPI.ServerConnectionI CurrentConnection
		{
			get { return m_connection; }
		}

		public void CreateResource(object item, string itemType)
		{
			string resourceID = "Library://" + Guid.NewGuid().ToString() + "." + itemType;
			System.Type ClassDef = m_editors.GetResourceEditorTypeFromResourceType(itemType);

			if (ClassDef != null)
				try 
				{ 
					EditorInterface edi = AddEditTab(ClassDef, resourceID, false);
					if (item != null)
						((ResourceEditor)edi.Page.Controls[0]).Resource = item;
					tabItems.SelectedTab = edi.Page;
					edi.HasChanged();
				}
				catch(Exception ex)
				{
                    if (ex as System.Reflection.TargetInvocationException != null)
                        ex = ex.InnerException;
 
					//TODO: Handle cancel with a special exeption type
                    if (ex is CancelException)
                        return;

					MessageBox.Show(this, string.Format(m_globalizor.Translate("Unable to create the resource {0}\nError message: {1}"), resourceID, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			else
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Unable to determine the resource type for: {0}"), resourceID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}


		public void OpenResource(string resourceID)
		{
			OpenResource(resourceID, null);
		}

		public void OpenResource(string resourceID, System.Type type)
		{
			if (!m_userControls.ContainsKey(resourceID))
			{
				System.Type ClassDef = type;
				
				if (ClassDef == null)
					ClassDef = m_editors.GetResourceEditorTypeFromResourceID(resourceID);
				if (ClassDef == null)
					ClassDef = typeof(ResourceEditors.XmlEditorControl);

				if (ClassDef != null)
					try { AddEditTab(ClassDef, resourceID, true); }
					catch (System.Reflection.TargetInvocationException tex) { MessageBox.Show(this, string.Format(m_globalizor.Translate("Unable to open the resource {0}\nError message: {1}"), resourceID, tex.InnerException.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
					catch (Exception ex) { MessageBox.Show(this, string.Format(m_globalizor.Translate("Unable to open the resource {0}\nError message: {1}"), resourceID, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
				else
					MessageBox.Show(this, string.Format(m_globalizor.Translate("Unable to determine the resource type for: {0}"), resourceID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				
			}
			
			if (m_userControls.ContainsKey(resourceID))
				tabItems.SelectedTab = ((EditorInterface)m_userControls[resourceID]).Page;

		}


		private void ResourceTree_DoubleClick(object sender, System.EventArgs e)
		{
			using (new WaitCursor(this))
			{
				if (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null)
					if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
					{
						OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
						OpenResource(document.ResourceId);
					}
			}
		}

		private EditorInterface AddEditTab(System.Type controlType, string resourceID, bool existing)
		{
			TabPage tp;
			if (existing)
				tp = new TabPage(m_editors.GetResourceNameFromResourceID(resourceID));
			else
				tp = new TabPage(m_globalizor.Translate("New resource"));

			tp.ImageIndex = m_editors.GetImageIndexFromResourceID(resourceID);
			EditorInterface edi = new EditorInterface(this, tp, resourceID, existing);

			object[] args = null;
			if (!existing)
				args = new object[] {edi};
			else
				args = new object[] {edi, resourceID};

			UserControl uc = (UserControl)Activator.CreateInstance(controlType, args );

            /*tp.BackgroundImage = new System.Drawing.Bitmap("test.png");
            tp.BackgroundImageLayout = ImageLayout.Stretch;
            uc.BackColor = Color.Transparent;*/

			tp.Controls.Add(uc);
			uc.Top = 16;
			uc.Left = 16;
			uc.Width = tp.Width - 32;
			uc.Height = tp.Height - 32;
			uc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			m_userControls.Add(edi.ResourceID, edi);
			tabItems.TabPages.Add(tp);
			return edi;
		}

        //Mono does not like icons from windows forms resources
        //because of an extra field introduced in .Net 2.0 SP1 called "dateTimeOffsetPattern"
        private static Icon m_maestroIcon;
        public static Icon MaestroIcon
        {
            get
            {
                if (m_maestroIcon == null)
                {
                    try
                    {
                        m_maestroIcon = new System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(FormMain), "Icons.MapGuide Maestro.ico"));
                    }
                    catch
                    {
                    }
                }

                return m_maestroIcon;
            }
        }

		private void TreeContextMenu_Popup(object sender, CancelEventArgs e)
		{
			if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
			{
				foreach(ToolStripItem m in TreeContextMenu.Items)
                    if (m as ToolStripMenuItem != null)
					    m.Enabled = false;
			}
			else
			{
                foreach (ToolStripItem m in TreeContextMenu.Items)
                    if (m as ToolStripMenuItem != null)
                        m.Enabled = true;
            }

            PropertiesMenu.Enabled =
            CopyResourceIdMenu.Enabled = ResourceTree.SelectedNode != null;
			PasteMenu.Enabled =  (ResourceTree.SelectedNode != null && m_clipboardBuffer != null);
			NewMenu.Enabled = true;

		}

		private void SaveXmlAsMenu_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = m_globalizor.Translate("Xml Files (*.xml)") + "|*.xml|" + m_globalizor.Translate("All files (*.*)")  + "|*.*";
				dlg.OverwritePrompt = true;
				dlg.Title = m_globalizor.Translate("Choose filename to save to");
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
					byte[] data = m_connection.GetResourceXmlData(document.ResourceId);
					using(System.IO.FileStream fs = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
						fs.Write(data, 0, data.Length);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to save resource data: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}


		private void NewFolderMenuItem_Click(object sender, System.EventArgs e)
		{
            AddFolder();
		}

		private void NewResourceMenu_Clicked(object sender, System.EventArgs e)
		{
			string filename = (string)m_templateMenuIndex[sender];
			if (filename == null)
				return;

			object o = null;

			try
			{
				using(System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
					o = m_connection.DeserializeObject(m_editors.GetResourceInstanceTypeFromResourceID(filename), fs);

			}
			catch(Exception ex)
			{
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to load template: {0}"), ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (o != null)
				CreateResource(o, m_editors.GetResourceTypeNameFromResourceID(filename));
		}

		private void ResourceTree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			ResourceTree.DoDragDrop(e.Item, DragDropEffects.All);
		}

		private void DeleteResource()
		{
			if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
				return;

			object item = ResourceTree.SelectedNode.Tag;
			if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
			{
				//We do not enumerate here, because it is SLOW
				if (ResourceTree.SelectedNode.Nodes.Count == 0 && MessageBox.Show(this, m_globalizor.Translate("Delete the selected folder?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
					return;
				else if (MessageBox.Show(this, m_globalizor.Translate("If you delete the folder, any resource that references an item in the selected folder will become unusable.\n\nDelete folder and all contents?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return;

				try
				{
					m_connection.DeleteFolder(((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)item).ResourceId);
				}
				catch(Exception ex)
				{
					MessageBox.Show(this, string.Format(m_globalizor.Translate("An error occured while deleting the folder: {0}"), ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				RebuildDocumentTree();
			}
			else if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
			{
				//We do not enumerate here, because it is SLOW
				if (MessageBox.Show(this, m_globalizor.Translate("If you delete the resource, any resource that reference the resource will become unusable.\n\nDelete the resource?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return;

				try
				{
					m_connection.DeleteResource(((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)item).ResourceId);
				}
				catch(Exception ex)
				{
					MessageBox.Show(this, string.Format(m_globalizor.Translate("An error occured while deleting the resource: {0}"), ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				RebuildDocumentTree();
			}
		}

		private void AddFolder()
		{
			string start;
			if (ResourceTree.SelectedNode == null)
				return;
			else if (ResourceTree.SelectedNode.Tag == null)
				start = "Library://";
			else if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				start = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)ResourceTree.SelectedNode.Tag).ResourceId;
			else if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
			{
				start = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag).ResourceId;
				start = start.Substring(0, start.LastIndexOf("/") + 1);
			}
			else
				return;
		
			string foldername = GetNewFolderName(start);
			m_connection.CreateFolder(foldername);
			string targetpath = ResourceTree.SelectedNode.FullPath + ResourceTree.PathSeparator + m_globalizor.Translate("New folder");
			RebuildDocumentTree();
			TreeNode n = TreeViewUtil.FindItemExact(ResourceTree, targetpath);
			if (n != null)
			{
				ResourceTree.SelectedNode = n;
				n.BeginEdit();
			}
		}

		private void ClipboardCopy()
		{
			if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
				return;

			m_clipboardBuffer = ResourceTree.SelectedNode;
			m_clipboardCut = false;
			ResourceTreePaste.Enabled = true;
		}

		private void ClipboardCut()
		{
			if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
				return;

			m_clipboardBuffer = ResourceTree.SelectedNode;
			m_clipboardCut = true;
			ResourceTreePaste.Enabled = true;
		}

		private void ClipboardPaste()
		{
			if (MoveOrCopyResource(m_clipboardBuffer, ResourceTree.SelectedNode, m_clipboardCut, true) && m_clipboardCut)
			{
				ResourceTreePaste.Enabled = false;
				m_clipboardBuffer = null;
				m_clipboardCut = false;
			}
		}

		private bool MoveOrCopyResource(TreeNode source, TreeNode target, bool move, bool refreshTree)
		{
			using (new WaitCursor(this))
			{
				if (source == null || source.Tag == null)
					return false;

				if (target == null)
					return false;

				string targetpath;
				if (target.Tag == null)
					targetpath = "Library://";
				else if (target.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				{
					targetpath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)target.Tag).ResourceId;
				}
				else if (target.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
				{
					targetpath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)target.Tag).ResourceId; 
					targetpath = targetpath.Substring(0, targetpath.LastIndexOf("/") + 1);
				}
				else
					return false;


				string sourcepath;

				if (source.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				{
					sourcepath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)source.Tag).ResourceId;
					targetpath = targetpath + sourcepath.Substring(sourcepath.Substring(0, sourcepath.Length - 1).LastIndexOf("/") + 1);
				}
				else if (source.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
				{
					sourcepath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)source.Tag).ResourceId; 
					targetpath = targetpath + sourcepath.Substring(sourcepath.LastIndexOf("/") + 1);
				}
				else
					return false;

				return MoveOrCopyResource(sourcepath, targetpath, move, refreshTree);
			}

		}

		private bool MoveOrCopyResource(string sourcepath, string targetpath, bool move, bool refreshTree)
		{
			using (new WaitCursor(this))
			{
				bool sourceisFolder = sourcepath.EndsWith("/");

				if (sourcepath == targetpath)
					if (move)
					{
						MessageBox.Show(this, m_globalizor.Translate("Cannot move a resource onto itself."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						return false;
					}
					else
						targetpath = GetUniqueResourceName(sourcepath);

				if (m_connection.ResourceExists(targetpath) && MessageBox.Show(this, sourceisFolder ? m_globalizor.Translate("There already exists a folder at the destination.\nDo you want to overwrite?") : m_globalizor.Translate("There already exists a resource at the destination.\nDo you want to overwrite?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return false;


				LengthyOperation lwdlg = new LengthyOperation();

				if (sourceisFolder && move) 
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.MoveFolder);
				else if (sourceisFolder)
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.CopyFolder);
				else if (move)
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.MoveResource);
				else
				{
					m_connection.CopyResource(sourcepath, targetpath, true);
					if (refreshTree)
						RebuildDocumentTree();
					return true;
				}

				bool rs = lwdlg.ShowDialog(this) == DialogResult.OK;

				//TODO: Close/update any open resource editors, if the resource was affected
				if (refreshTree)
					RebuildDocumentTree();
				return rs;
			}
		}

		private string GetNewFolderName(string path)
		{
			OSGeo.MapGuide.MaestroAPI.ResourceList lst = m_connection.GetRepositoryResources(path, 1);
			Hashtable res = new Hashtable();
			foreach(object o in lst.Items)
				if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				{
					string foldername = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)o).ResourceId;
					foldername = foldername.Substring(foldername.Substring(0, foldername.Length - 1).LastIndexOf("/") + 1);
					res.Add(foldername, null);
				}

			string name = m_globalizor.Translate("New folder") + "/";
			int i = 1;
			while (res.ContainsKey(name) && i < 100)
				name = string.Format(m_globalizor.Translate("New folder {0}"), i) + "/";

			if (i >= 100)
				throw new Exception(m_globalizor.Translate("Internal error, more than 100 new folders is usually an indication of an error"));

			return path + name;
		}

		private string GetUniqueResourceName(string resource)
		{
			string folderpath = resource.Substring(0, resource.Substring(0, resource.Length - 1).LastIndexOf("/") + 1);
			OSGeo.MapGuide.MaestroAPI.ResourceList lst = m_connection.GetRepositoryResources(folderpath, 1);
			Hashtable res = new Hashtable();
			foreach(object o in lst.Items)
				if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
					res.Add(((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)o).ResourceId, null);
				else if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
					res.Add(((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)o).ResourceId, null);

			if (res.ContainsKey(resource))
			{
				string resourcename = resource.Substring(folderpath.Length);
				resource = folderpath + string.Format(m_globalizor.Translate("Copy of {0}"), resourcename);
				int i = 1;
				while (res.ContainsKey(resource) && i < 100)
					resource = folderpath + string.Format(m_globalizor.Translate("Copy {0} of {1}"),  (i++), resourcename);
		
				if (i == 100)
					throw new Exception(m_globalizor.Translate("Internal error, more than 100 copies is usually an indication of an error"));

			}
			return resource;
		}

		private void SaveResource()
		{
			if (tabItems.SelectedTab == null || tabItems.SelectedTab.Controls.Count != 1)
				return;

			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page == tabItems.SelectedTab)
				{
					edi.Save();
					return;
				}
		}

		private void SaveResourceAs()
		{
			if (tabItems.SelectedTab == null || tabItems.SelectedTab.Controls.Count != 1)
				return;

			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page == tabItems.SelectedTab)
				{
					edi.SaveAs();
					return;
				}
		}

		private void EditAsXml()
		{
			if (tabItems.SelectedTab == null || tabItems.SelectedTab.Controls.Count != 1)
				return;

			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page == tabItems.SelectedTab)
				{
					object resource = ((ResourceEditor)edi.Page.Controls[0]).Resource;
					XmlEditor dlg = new XmlEditor(resource, this.CurrentConnection);
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						object o = dlg.SerializedObject;
						System.Reflection.PropertyInfo pi = o.GetType().GetProperty("ResourceId");
						if (pi != null)
							pi.SetValue(o, pi.GetValue(resource, null), null);

						pi = o.GetType().GetProperty("CurrentConnection");
						if (pi != null)
							pi.SetValue(o, pi.GetValue(resource, null), null);

						((ResourceEditor)edi.Page.Controls[0]).Resource = dlg.SerializedObject;
						edi.HasChanged();
					}
				}
		}

		private void ClosePage()
		{
			if (tabItems.SelectedTab == null || tabItems.SelectedTab.Controls.Count != 1)
				return;

			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page == tabItems.SelectedTab)
				{
					edi.Close(true);
					break;
				}
		}

		private void OpenPreview()
		{
			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page == tabItems.SelectedTab)
				{
                    try
                    {
                        if (!((ResourceEditor)edi.Page.Controls[0]).Preview())
                            MessageBox.Show(this, m_globalizor.Translate("The selected editor could not preview the resource. Most likely, the preview feature is not implemented for the given resource."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed while previewing resource: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
					break;
				}
		}

		private void ResourceTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			bool itemSelected = ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null;
			DeleteResourceButton.Enabled = ResourceTreeCopy.Enabled = ResourceTreeCut.Enabled = itemSelected;
			ResourceTreePaste.Enabled = m_clipboardBuffer != null && ResourceTree.SelectedNode != null;
			if (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null)
			{
				object item = ResourceTree.SelectedNode.Tag;
				if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				{
					m_lastSelectedNode = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)item).ResourceId;
				}
				else if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
				{
					m_lastSelectedNode = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)item).ResourceId;
				}
			}
		}


		private void ResourceTree_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			TreeNode x = e.Data.GetData(typeof(TreeNode)) as TreeNode;
			if (x == null || x.Tag == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			TreeNode n = ResourceTree.GetNodeAt(ResourceTree.PointToClient(new Point(e.X, e.Y)));
			if (n == null)
				e.Effect = DragDropEffects.None;
			else if (n.Tag == null || n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
			{
				//Hmm 8 means CTRL, where is the constant?
				if ((e.KeyState & 8) != 0)
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.Move;
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void ResourceTree_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			TreeNode x = e.Data.GetData(typeof(TreeNode)) as TreeNode;
			if (x == null || x.Tag == null)
				return;

			TreeNode n = ResourceTree.GetNodeAt(ResourceTree.PointToClient(new Point(e.X, e.Y)));
			if (n == null)
				return;
			else if (n.Tag == null || n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
			{
				//Hmm 8 means CTRL, where is the constant?
				if ((e.KeyState & 8) != 0)
					MoveOrCopyResource(x, n, false, true);
				else
					MoveOrCopyResource(x, n, true, true);
			}
			else
				return;
			
		}

		private void ResourceTree_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!ResourceTree.Focused)
				return;

			if (e.KeyCode == Keys.Enter)
			{
				ResourceTree_DoubleClick(sender, e);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Delete)
			{
                DeleteResourceButton_Click(sender, e);
				e.Handled = true;
			}
			else if ((e.KeyCode == Keys.R && e.Control) || e.KeyCode == Keys.F5)
			{
                ResourceTreeRefreshButton_Click(sender, e);
				e.Handled = true;
			}
			else if ((e.KeyCode == Keys.F2))
			{
				if (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null)
					ResourceTree.SelectedNode.BeginEdit();
				e.Handled = true;
			}
		}

		private void ResourceTree_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Label == null)
				return;

			//Something goes wrong if the dialog is shown from within the eventhandler
			//This annoying workaround prevents the rename crash, but has an annoying delay
			m_lastLabelEdit = e;
			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(MoveOrCopyThread));
			t.IsBackground = true;
			t.Start();
		}
		
		private System.Windows.Forms.NodeLabelEditEventArgs m_lastLabelEdit;
		//private delegate bool MoveOrCopyResourceDelegate(string sourcepath, string targetpath, bool move, bool refresh);
		private void MoveOrCopyThread()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new System.Threading.ThreadStart(MoveOrCopyThread));
				return;
			}

			System.Windows.Forms.NodeLabelEditEventArgs e = m_lastLabelEdit;
			m_lastLabelEdit = null;

			if (e == null || e.Label == null)
				return;


			string sourcepath;
			if (e.Node.Tag == null)
				return;
			else if (e.Node.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
				sourcepath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)e.Node.Tag).ResourceId;
			else if (e.Node.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
				sourcepath = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)e.Node.Tag).ResourceId;
			else
				return;

			string targetpath = sourcepath.Substring(0, sourcepath.Substring(0, sourcepath.Length - 1).LastIndexOf("/") + 1);
			if (sourcepath.EndsWith("/"))
				targetpath += e.Label + "/";
			else
				targetpath += e.Label + sourcepath.Substring(sourcepath.LastIndexOf("."));

			if (sourcepath == targetpath)
				return;

			if (m_connection.ResourceExists(targetpath))
			{
				MessageBox.Show(this, m_globalizor.Translate("Another resource with that name already exists."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.CancelEdit = true;
				return;
			}

			if (MoveOrCopyResource(sourcepath, targetpath, true, false))
				e.Node.Text = e.Label;

			RebuildDocumentTree();
			//this.Invoke(new MoveOrCopyResourceDelegate(MoveOrCopyResource), new object[] {});
		}


		private void ResourceTree_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if (e.Node == null || e.Node.Tag == null)
				e.CancelEdit = true;
		}

		private void FormMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach(EditorInterface edi in m_userControls.Values)
				if (edi.Page.Text.EndsWith(" *"))
				{
					switch(MessageBox.Show(this, m_globalizor.Translate("One or more resources has unsaved changes.\nClosing this application will loose the changes.\nDo you want to save the resources before closing?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
						case DialogResult.Yes:
						{
							ArrayList tosave = new ArrayList();
							foreach(EditorInterface edi2 in m_userControls.Values)
								if (edi2.Page.Text.EndsWith(" *"))
									tosave.Add(edi2);

							foreach(EditorInterface edi2 in tosave)
								if (!edi2.Save())
								{
									e.Cancel = true;
									return;
								}			
						}
							break;

						case DialogResult.Cancel:
							e.Cancel = true;
							return;
					}
					break;
				}
		
		}

		private void CutMenu_Click(object sender, System.EventArgs e)
		{
            ClipboardCut();
		}

		private void CopyMenu_Click(object sender, System.EventArgs e)
		{
            ClipboardCopy();
		}

		private void PasteMenu_Click(object sender, System.EventArgs e)
		{
            ClipboardPaste();
		}

		private void EditAsXmlMenu_Click(object sender, System.EventArgs e)
		{
			using(new WaitCursor(this))
			{
				try
				{
					if (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null)
						if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
						{
							OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
							if (m_userControls.ContainsKey(document.ResourceId))
							{
								EditorInterface edi = (EditorInterface)m_userControls[document.ResourceId];
								if (!edi.Close(true))
									return;
							}

							OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl));
						}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, string.Format(m_globalizor.Translate("An error occured while opening the xml editor: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void LoadFromXmlMenu_Click(object sender, System.EventArgs e)
		{
			using(new WaitCursor(this))
			{
				try
				{
					OpenFileDialog dlg = new OpenFileDialog();
					dlg.CheckFileExists = true;
					dlg.CheckPathExists = true;
					dlg.DereferenceLinks = true;
					dlg.Filter = m_globalizor.Translate("Xml files (*.xml)") + "|*.xml|" + m_globalizor.Translate("All files (*.*)") + "|*.*";
					dlg.Multiselect = false;
					dlg.ShowReadOnly = false;
					dlg.ValidateNames = true;
					if (dlg.ShowDialog() != DialogResult.OK)
						return;

					if (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Tag != null)
						if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
						{
							OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
							if (m_userControls.ContainsKey(document.ResourceId))
							{
								EditorInterface edi = (EditorInterface)m_userControls[document.ResourceId];
								if (!edi.Close(true))
									return;
							}

							object item = null;
							using(System.IO.FileStream fs = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
								item = m_connection.DeserializeObject(m_editors.GetResourceInstanceTypeFromResourceID(document.ResourceId), fs);
							EditorInterface edir = new EditorInterface(this, new TabPage(m_editors.GetResourceNameFromResourceID(document.ResourceId)), document.ResourceId, true);
							OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl c = new OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl(edir);
							c.Resource = item;
							c.ResourceId = document.ResourceId;
							edir.Page.Controls.Add(c);
							c.Dock = DockStyle.Fill;
							tabItems.TabPages.Add(edir.Page);
							tabItems.SelectedTab = edir.Page;
							edir.HasChanged();
							m_userControls.Add(document.ResourceId, edir);

						}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, string.Format(m_globalizor.Translate("An error occured while opening the xml editor: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void KeepAliveTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				m_connection.GetProviderCapabilities("OSGeo.SDF");
			}
			catch
			{
			}
		}

		private void MainMenuOpen_Click(object sender, System.EventArgs e)
		{
			try
			{
				BrowseResource dlg = new BrowseResource(m_connection, this, this.ResourceEditorMap.SmallImageList, true, null);
				if (dlg.ShowDialog(this) == DialogResult.OK)
					this.OpenResource(dlg.SelectedResource);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to open selected resource: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MainMenuNew_Click(object sender, System.EventArgs e)
		{
		
		}

		private void MainMenuClose_Click(object sender, System.EventArgs e)
		{
			ClosePage();
		}

		private void MainMenuSave_Click(object sender, System.EventArgs e)
		{
			SaveResource();
		}

		private void MainMenuSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveResourceAs();
		}

		private void MainMenuSaveAll_Click(object sender, System.EventArgs e)
		{
			SaveAllResources();
		}

		private void SaveAllResources()
		{
			try
			{
				foreach(OSGeo.MapGuide.Maestro.EditorInterface edi in this.OpenResourceEditors.Values)
					if (edi.Page.Text.EndsWith(" *"))
						edi.Save();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to save resources: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void MainMenuSaveAsXml_Click(object sender, System.EventArgs e)
		{
			SaveXmlAsMenu_Click(sender, e);
		}

		private void MainMenuLoadFromXml_Click(object sender, System.EventArgs e)
		{
			LoadFromXmlMenu_Click(sender, e);
		}

		private void MainMenuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void MainMenuChangeServer_Click(object sender, System.EventArgs e)
		{
			FormLogin lg = new FormLogin();
			if (lg.ShowDialog(this) == DialogResult.OK)
			{
				System.ComponentModel.CancelEventArgs cea = new CancelEventArgs(false);
				cea.Cancel = false;
				FormMain_Closing(this, cea);
				if (!cea.Cancel)
				{
                    //The collection changes, so we trick it!
                    while(this.OpenResourceEditors.Values.Count > 0)
                        foreach (OSGeo.MapGuide.Maestro.EditorInterface edi in this.OpenResourceEditors.Values)
                        {
                            edi.Close(false);
                            break;
                        }
					ResourceTree.Nodes.Clear();
					m_connection = lg.Connection;
					m_Folders = m_Documents = null;
					RebuildDocumentTree();
				}
			}
		}

		private void MainMenuEditAsXml_Click(object sender, System.EventArgs e)
		{
			EditAsXml();
		}

		private void MainMenuCut_Click(object sender, System.EventArgs e)
		{
			ClipboardCut();
		}

		private void MainMenuCopy_Click(object sender, System.EventArgs e)
		{
			ClipboardCopy();
		}

		private void MainMenuPaste_Click(object sender, System.EventArgs e)
		{
			ClipboardPaste();
		}

		private void MainMenuAbout_Click(object sender, System.EventArgs e)
		{
			FormAbout dlg = new FormAbout();
			dlg.ShowDialog(this);
		}

		private void DeleteMenu_Click(object sender, System.EventArgs e)
		{
			DeleteResource();
		}

		private void ResourceTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				ResourceTree.SelectedNode = ResourceTree.GetNodeAt(e.X, e.Y);
		}

		private void OpenSiteAdmin_Click(object sender, System.EventArgs e)
		{

            string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_connection).BaseURL + "mapadmin/login.php";
            Program.OpenUrl(url);
		
		}


		//TODO: Is sucks having two places where we keep track of the pages
		/// <summary>
		/// Gets the list of open resouces
		/// </summary>
		public Hashtable OpenResourceEditors
		{
			get { return m_userControls; }
		}

		public SortedList ResourceFolders { get { return m_Folders; } }
		public SortedList ResourceDocuments { get { return m_Documents; } }
		public ResourceEditorMap ResourceEditorMap { get { return m_editors; } }
		public string LastSelectedNode { get { return m_lastSelectedNode; } }

        private void AddFolderButton_Click(object sender, EventArgs e)
        {
            AddFolder();
        }

        private void DeleteResourceButton_Click(object sender, EventArgs e)
        {
            DeleteResource();
        }

        private void ResourceTreeCopy_Click(object sender, EventArgs e)
        {
            ClipboardCopy();
        }

        private void ResourceTreeCut_Click(object sender, EventArgs e)
        {
            ClipboardCut();
        }

        private void ResourceTreePaste_Click(object sender, EventArgs e)
        {
            ClipboardPaste();
        }

        private void ResourceTreeRefreshButton_Click(object sender, EventArgs e)
        {
            RebuildDocumentTree();
        }

        private void SaveResourceButton_Click(object sender, EventArgs e)
        {
            SaveResource();
        }

        private void SaveResourceAsButton_Click(object sender, EventArgs e)
        {
            SaveResourceAs();
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            OpenPreview();
        }

        private void EditAsXmlButton_Click(object sender, EventArgs e)
        {
            EditAsXml();
        }

        private void ClosePageButton_Click(object sender, EventArgs e)
        {
            ClosePage();
        }

        private void MainMenuChangePreferences_Click(object sender, EventArgs e)
        {
            ApplicationSettings dlg = new ApplicationSettings();
            dlg.ShowDialog(this);
        }

        private void createPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackageManager.CreatePackage dlg = new OSGeo.MapGuide.Maestro.PackageManager.CreatePackage();
            dlg.Setup(new EditorInterface(this, null, null, false), this.LastSelectedNode);
            dlg.ShowDialog(this);
        }

        private void restorePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PackageManager.PackageUploader.UploadPackage(this, m_connection) == DialogResult.OK)
                RebuildDocumentTree();
        }

        private void modifyPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackageManager.PackageEditor.EditPackage(m_connection, this);
        }

        private void PropertiesMenu_Click(object sender, EventArgs e)
        {
            if (ResourceTree.SelectedNode == null)
                return;

            string resid = null;
            if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument).ResourceId;
            else if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder).ResourceId;
            else if (ResourceTree.SelectedNode.Parent == null)
                resid = "Library://";

            if (resid == null)
                return;

            ResourceProperties dlg = new ResourceProperties(m_connection, resid);
            dlg.ShowDialog(this);
        }

        public Exception LastException
        {
            get { return m_lastException; }
            set { m_lastException = value; }
        }

        private void viewLastExceptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lastException == null)
                MessageBox.Show(this, "There is no exception data to view", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                ExceptionViewer dlg = new ExceptionViewer();
                dlg.ExceptionText.Text = m_lastException.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
                dlg.ShowDialog(this);
            }
        }

        private void tabItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabItems.SelectedTab == null || tabItems.SelectedTab.Controls.Count != 1 || tabItems.TabCount == 0)
            {
                foreach (ToolStripItem b in toolStrip1.Items)
                    b.Enabled = false;
                TabPageContextMenu.Enabled = false;
            }
            else
            {
                EditorInterface edi = null;
                foreach (EditorInterface n in m_userControls.Values)
                    if (n.Page == tabItems.SelectedTab)
                    {
                        edi = n;
                        break;
                    }

                if (edi == null || edi.Page == null || edi.Page.Controls.Count < 1 || edi.Page.Controls[0] as ResourceEditor == null)
                {
                    foreach (ToolStripItem b in toolStrip1.Items)
                        b.Enabled = false;
                    TabPageContextMenu.Enabled = false;
                }
                else
                {
                    SaveResourceButton.Enabled =
                    SaveResourceAsButton.Enabled =
                    EditAsXmlButton.Enabled =
                    ClosePageButton.Enabled = true;

                    ResourceEditor ei = edi.Page.Controls[0] as ResourceEditor;
                    PreviewButton.Enabled = ei.SupportsPreview;
                    ProfileButton.Enabled = ei.SupportsProfiling;
                    ValidateButton.Enabled = ei.SupportsValidate;
                    TabPageContextMenu.Enabled = true;
                }
            }
        }

        private void tabItems_ControlAdded(object sender, ControlEventArgs e)
        {
            tabItems_SelectedIndexChanged(null, null);
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            EditorInterface edi = null;
            foreach (EditorInterface n in m_userControls.Values)
                if (n.Page == tabItems.SelectedTab)
                {
                    edi = n;
                    break;
                }

            if (edi == null || edi.Page == null || edi.Page.Controls == null || edi.Page.Controls.Count < 1)
                return;

            ResourceEditor ei = edi.Page.Controls[0] as ResourceEditor;
            if (ei == null || ei.Resource == null)
                return;

            Profiling dlg = new Profiling(ei.Resource, m_connection);
            dlg.ShowDialog(this);
        }

        private void CopyResourceIdMenu_Click(object sender, EventArgs e)
        {
            if (ResourceTree.SelectedNode == null)
                return;

            string resid = null;
            if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument).ResourceId;
            else if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder).ResourceId;

            if (resid == null)
                return;

            Clipboard.SetText(resid);

        }

        private void TabClosePageMenu_Click(object sender, EventArgs e)
        {
            ClosePage();
        }

        private void TabSaveMenu_Click(object sender, EventArgs e)
        {
            SaveResource();
        }

        private void TabSaveAsMenu_Click(object sender, EventArgs e)
        {
            SaveResourceAs();
        }

        private void TabCopyIdMenu_Click(object sender, EventArgs e)
        {
            if (tabItems.SelectedTab != null)
                Clipboard.SetText(tabItems.SelectedTab.ToolTipText);
        }

        private void TabPageContextMenu_Opening(object sender, CancelEventArgs e)
        {
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Program.ApplicationSettings.WindowLeft = this.Left;
                Program.ApplicationSettings.WindowTop = this.Top;
                Program.ApplicationSettings.WindowWidth = this.Width;
                Program.ApplicationSettings.WindowHeight = this.Height;
                Program.ApplicationSettings.MaximizedWindow = this.WindowState == FormWindowState.Maximized;
                //Trigger a save
                Program.ApplicationSettings = Program.ApplicationSettings;

            }
            catch
            {
            }
        }

        private void FormMain_Move(object sender, EventArgs e)
        {
            FormMain_SizeChanged(sender, e);
        }

        private void ResourceInfoTip_Popup(object sender, PopupEventArgs e)
        {
            
        }

        private void ResourceTree_MouseMove(object sender, MouseEventArgs e)
        {
            TooltipUpdateTimer.Enabled = true;
        }

        private void TooltipUpdateTimer_Tick(object sender, EventArgs e)
        {
            TooltipUpdateTimer.Enabled = false;

            Point p = ResourceTree.PointToClient(Cursor.Position);
            TreeNode n = ResourceTree.GetNodeAt(p);
            if (n == null || n.Tag == null)
            {
                ResourceInfoTip.RemoveAll();
                m_lastTooltip = "";
            }
            else if (n.Tag is MaestroAPI.ResourceListResourceDocument)
            {
                MaestroAPI.ResourceListResourceDocument d = n.Tag as MaestroAPI.ResourceListResourceDocument;
                string tooltip = string.Format(m_globalizor.Translate("Resource name: {0}\r\nResource type: {1}\r\nCreated: {2}\r\nLast modified: {3}"), new MaestroAPI.ResourceIdentifier(d.ResourceId).Name, new MaestroAPI.ResourceIdentifier(d.ResourceId).Extension, d.CreatedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture), d.ModifiedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture));

                if (new MaestroAPI.ResourceIdentifier(d.ResourceId).Extension == "LayerDefinition" || new MaestroAPI.ResourceIdentifier(d.ResourceId).Extension == "FeatureSource")
                {
                    bool published = false;
                    string serviceType = new MaestroAPI.ResourceIdentifier(d.ResourceId).Extension == "LayerDefinition" ? "WMS" : "WFS";
                    if (d.ResourceDocumentHeader != null && d.ResourceDocumentHeader.Metadata != null && d.ResourceDocumentHeader.Metadata.Simple != null && d.ResourceDocumentHeader.Metadata.Simple.Property["_IsPublished"] == "1")
                        published = true;

                    tooltip += "\r\n" + string.Format(m_globalizor.Translate("{0} published: {1}"), serviceType, published);
                }

                //if (tooltip != m_lastTooltip)
                    ResourceInfoTip.SetToolTip(ResourceTree, tooltip);
                m_lastTooltip = tooltip;
            }
            else if (n.Tag is MaestroAPI.ResourceListResourceFolder)
            {
                MaestroAPI.ResourceListResourceFolder d = n.Tag as MaestroAPI.ResourceListResourceFolder;

                string tooltip = string.Format(m_globalizor.Translate("Resource name: {0}\r\nResource type: {1}\r\nCreated: {2}\r\nLast modified: {3}"), new MaestroAPI.ResourceIdentifier(d.ResourceId.Substring(0, d.ResourceId.Length - 1) + ".Folder").Name, m_globalizor.Translate("Folder"), d.CreatedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture), d.ModifiedDate.ToString(System.Globalization.CultureInfo.CurrentUICulture));
                //if (tooltip != m_lastTooltip)
                    ResourceInfoTip.SetToolTip(ResourceTree, tooltip);
                m_lastTooltip = tooltip;
            }
            else
            {
                ResourceInfoTip.RemoveAll();
                m_lastTooltip = "";
            }

        }

        private void tabItems_MouseMove(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < tabItems.TabPages.Count; i++)
                if (tabItems.GetTabRect(i).Contains(e.Location))
                {
                    TabPageTooltip.SetToolTip(tabItems, tabItems.TabPages[i].ToolTipText);
                    return;
                }

            TabPageTooltip.SetToolTip(tabItems, null);
        }

        private void tabItems_MouseLeave(object sender, EventArgs e)
        {
            TabPageTooltip.SetToolTip(tabItems, null);
        }
	}
}
