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

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for FormMain.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
		private Hashtable m_userControls = new Hashtable();
		private System.Windows.Forms.Panel panLeftTree;
		private System.Windows.Forms.Panel panInnerRight;
		private System.Windows.Forms.Panel panOuter;
		private System.Windows.Forms.Splitter splitter1;
		public System.Windows.Forms.TabControl tabItems;
		private System.Windows.Forms.ContextMenu NewResourceMenu;
		private System.Windows.Forms.ContextMenu TreeContextMenu;
		private System.Windows.Forms.MenuItem PropertiesMenu;
		private System.Windows.Forms.MenuItem SaveXmlAsMenu;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TreeView ResourceTree;
		private System.Windows.Forms.ToolBar ResourceTreeToolbar;
		private System.Windows.Forms.ToolBarButton ResourceTreeCopy;
		private System.Windows.Forms.ToolBarButton ResourceTreeCut;
		private System.Windows.Forms.ToolBarButton ResourceTreePaste;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ImageList toolbarImagesSmall;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBar toolBar2;
		private System.Windows.Forms.ToolBarButton SaveResourceButton;
		private System.Windows.Forms.ToolBarButton SaveResourceAsButton;
		private System.Windows.Forms.ToolBarButton EditAsXmlButton;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton AddResourceButton;
		private System.Windows.Forms.ToolBarButton DeleteResourceButton;
		private System.Windows.Forms.ToolBarButton ResourceTreeRefreshButton;

		private Hashtable m_templateMenuIndex = null;

		private TreeNode m_clipboardBuffer = null;
		private System.Windows.Forms.ToolBarButton AddFolderButton;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton ClosePageButton;
		private bool m_clipboardCut = false;

		private SortedList m_Folders = null;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem EditAsXmlMenu;
		private System.Windows.Forms.MenuItem LoadFromXmlMenu;
		private System.Windows.Forms.MenuItem CutMenu;
		private System.Windows.Forms.MenuItem CopyMenu;
		private System.Windows.Forms.MenuItem PasteMenu;
		private System.Windows.Forms.Timer KeepAliveTimer;
		private SortedList m_Documents = null;
		private System.Windows.Forms.ToolBarButton PreviewButton;
		private System.Windows.Forms.MainMenu MainMenu;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem MainMenuNew;
		private System.Windows.Forms.MenuItem MainMenuOpen;
		private System.Windows.Forms.MenuItem MainMenuClose;
		private System.Windows.Forms.MenuItem MainMenuSave;
		private System.Windows.Forms.MenuItem MainMenuSaveAs;
		private System.Windows.Forms.MenuItem MainMenuSaveAll;
		private System.Windows.Forms.MenuItem MainMenuSaveAsXml;
		private System.Windows.Forms.MenuItem MainMenuLoadFromXml;
		private System.Windows.Forms.MenuItem MainMenuChangeServer;
		private System.Windows.Forms.MenuItem MainMenuExit;
		private System.Windows.Forms.MenuItem MainMenuCopy;
		private System.Windows.Forms.MenuItem MainMenuPaste;
		private System.Windows.Forms.MenuItem MainMenuAbout;
		private System.Windows.Forms.MenuItem MainMenuEditAsXml;
		private System.Windows.Forms.MenuItem MainMenuEdit;
		private System.Windows.Forms.MenuItem MainMenuCut;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem DeleteMenu;
		private System.Windows.Forms.MenuItem NewMenu;

		private ResourceEditorMap m_editors;
		private System.Windows.Forms.MenuItem OpenSiteAdmin;
		private  Globalizator.Globalizator m_globalizor = null;
		private string m_lastSelectedNode = null;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormMain));
			this.ResourceTree = new System.Windows.Forms.TreeView();
			this.TreeContextMenu = new System.Windows.Forms.ContextMenu();
			this.PropertiesMenu = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.EditAsXmlMenu = new System.Windows.Forms.MenuItem();
			this.LoadFromXmlMenu = new System.Windows.Forms.MenuItem();
			this.SaveXmlAsMenu = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.CutMenu = new System.Windows.Forms.MenuItem();
			this.CopyMenu = new System.Windows.Forms.MenuItem();
			this.PasteMenu = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.DeleteMenu = new System.Windows.Forms.MenuItem();
			this.NewMenu = new System.Windows.Forms.MenuItem();
			this.panLeftTree = new System.Windows.Forms.Panel();
			this.ResourceTreeToolbar = new System.Windows.Forms.ToolBar();
			this.AddResourceButton = new System.Windows.Forms.ToolBarButton();
			this.AddFolderButton = new System.Windows.Forms.ToolBarButton();
			this.DeleteResourceButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.ResourceTreeCopy = new System.Windows.Forms.ToolBarButton();
			this.ResourceTreeCut = new System.Windows.Forms.ToolBarButton();
			this.ResourceTreePaste = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.ResourceTreeRefreshButton = new System.Windows.Forms.ToolBarButton();
			this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.panInnerRight = new System.Windows.Forms.Panel();
			this.toolBar2 = new System.Windows.Forms.ToolBar();
			this.SaveResourceButton = new System.Windows.Forms.ToolBarButton();
			this.SaveResourceAsButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.PreviewButton = new System.Windows.Forms.ToolBarButton();
			this.EditAsXmlButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.ClosePageButton = new System.Windows.Forms.ToolBarButton();
			this.tabItems = new System.Windows.Forms.TabControl();
			this.panOuter = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.NewResourceMenu = new System.Windows.Forms.ContextMenu();
			this.toolbarImagesSmall = new System.Windows.Forms.ImageList(this.components);
			this.KeepAliveTimer = new System.Windows.Forms.Timer(this.components);
			this.MainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.MainMenuNew = new System.Windows.Forms.MenuItem();
			this.MainMenuOpen = new System.Windows.Forms.MenuItem();
			this.MainMenuClose = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.MainMenuSave = new System.Windows.Forms.MenuItem();
			this.MainMenuSaveAs = new System.Windows.Forms.MenuItem();
			this.MainMenuSaveAll = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.MainMenuEditAsXml = new System.Windows.Forms.MenuItem();
			this.MainMenuSaveAsXml = new System.Windows.Forms.MenuItem();
			this.MainMenuLoadFromXml = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.MainMenuChangeServer = new System.Windows.Forms.MenuItem();
			this.OpenSiteAdmin = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.MainMenuExit = new System.Windows.Forms.MenuItem();
			this.MainMenuEdit = new System.Windows.Forms.MenuItem();
			this.MainMenuCut = new System.Windows.Forms.MenuItem();
			this.MainMenuCopy = new System.Windows.Forms.MenuItem();
			this.MainMenuPaste = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.MainMenuAbout = new System.Windows.Forms.MenuItem();
			this.panLeftTree.SuspendLayout();
			this.panInnerRight.SuspendLayout();
			this.panOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// ResourceTree
			// 
			this.ResourceTree.AllowDrop = true;
			this.ResourceTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ResourceTree.ContextMenu = this.TreeContextMenu;
			this.ResourceTree.ImageIndex = -1;
			this.ResourceTree.LabelEdit = true;
			this.ResourceTree.Location = new System.Drawing.Point(8, 48);
			this.ResourceTree.Name = "ResourceTree";
			this.ResourceTree.SelectedImageIndex = -1;
			this.ResourceTree.Size = new System.Drawing.Size(288, 396);
			this.ResourceTree.TabIndex = 0;
			this.ResourceTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResourceTree_MouseDown);
			this.ResourceTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragOver);
			this.ResourceTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ResourceTree_KeyUp);
			this.ResourceTree.DoubleClick += new System.EventHandler(this.ResourceTree_DoubleClick);
			this.ResourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterSelect);
			this.ResourceTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_AfterLabelEdit);
			this.ResourceTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourceTree_ItemDrag);
			this.ResourceTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_BeforeLabelEdit);
			this.ResourceTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragDrop);
			// 
			// TreeContextMenu
			// 
			this.TreeContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.PropertiesMenu,
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
			this.TreeContextMenu.Popup += new System.EventHandler(this.TreeContextMenu_Popup);
			// 
			// PropertiesMenu
			// 
			this.PropertiesMenu.Index = 0;
			this.PropertiesMenu.Text = "Properties";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 1;
			this.menuItem7.Text = "-";
			// 
			// EditAsXmlMenu
			// 
			this.EditAsXmlMenu.Index = 2;
			this.EditAsXmlMenu.Text = "Edit as xml";
			this.EditAsXmlMenu.Click += new System.EventHandler(this.EditAsXmlMenu_Click);
			// 
			// LoadFromXmlMenu
			// 
			this.LoadFromXmlMenu.Index = 3;
			this.LoadFromXmlMenu.Text = "Load from Xml...";
			this.LoadFromXmlMenu.Click += new System.EventHandler(this.LoadFromXmlMenu_Click);
			// 
			// SaveXmlAsMenu
			// 
			this.SaveXmlAsMenu.Index = 4;
			this.SaveXmlAsMenu.Text = "Save Xml As...";
			this.SaveXmlAsMenu.Click += new System.EventHandler(this.SaveXmlAsMenu_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 5;
			this.menuItem1.Text = "-";
			// 
			// CutMenu
			// 
			this.CutMenu.Index = 6;
			this.CutMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.CutMenu.Text = "Cut";
			this.CutMenu.Click += new System.EventHandler(this.CutMenu_Click);
			// 
			// CopyMenu
			// 
			this.CopyMenu.Index = 7;
			this.CopyMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.CopyMenu.Text = "Copy";
			this.CopyMenu.Click += new System.EventHandler(this.CopyMenu_Click);
			// 
			// PasteMenu
			// 
			this.PasteMenu.Index = 8;
			this.PasteMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.PasteMenu.Text = "Paste";
			this.PasteMenu.Click += new System.EventHandler(this.PasteMenu_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 9;
			this.menuItem4.Text = "-";
			// 
			// DeleteMenu
			// 
			this.DeleteMenu.Index = 10;
			this.DeleteMenu.Text = "Delete";
			this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
			// 
			// NewMenu
			// 
			this.NewMenu.Index = 11;
			this.NewMenu.Text = "New";
			// 
			// panLeftTree
			// 
			this.panLeftTree.Controls.Add(this.ResourceTreeToolbar);
			this.panLeftTree.Controls.Add(this.ResourceTree);
			this.panLeftTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.panLeftTree.Location = new System.Drawing.Point(0, 0);
			this.panLeftTree.Name = "panLeftTree";
			this.panLeftTree.Size = new System.Drawing.Size(296, 454);
			this.panLeftTree.TabIndex = 2;
			// 
			// ResourceTreeToolbar
			// 
			this.ResourceTreeToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.ResourceTreeToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																								   this.AddResourceButton,
																								   this.AddFolderButton,
																								   this.DeleteResourceButton,
																								   this.toolBarButton2,
																								   this.ResourceTreeCopy,
																								   this.ResourceTreeCut,
																								   this.ResourceTreePaste,
																								   this.toolBarButton1,
																								   this.ResourceTreeRefreshButton});
			this.ResourceTreeToolbar.DropDownArrows = true;
			this.ResourceTreeToolbar.ImageList = this.toolbarImages;
			this.ResourceTreeToolbar.Location = new System.Drawing.Point(0, 0);
			this.ResourceTreeToolbar.Name = "ResourceTreeToolbar";
			this.ResourceTreeToolbar.ShowToolTips = true;
			this.ResourceTreeToolbar.Size = new System.Drawing.Size(296, 44);
			this.ResourceTreeToolbar.TabIndex = 1;
			this.ResourceTreeToolbar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.ResourceTreeToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ResourceTreeToolbar_ButtonClick);
			// 
			// AddResourceButton
			// 
			this.AddResourceButton.ImageIndex = 4;
			this.AddResourceButton.ToolTipText = "Creates a new resource";
			// 
			// AddFolderButton
			// 
			this.AddFolderButton.ImageIndex = 10;
			this.AddFolderButton.ToolTipText = "Creates a new folder";
			// 
			// DeleteResourceButton
			// 
			this.DeleteResourceButton.Enabled = false;
			this.DeleteResourceButton.ImageIndex = 5;
			this.DeleteResourceButton.ToolTipText = "Deletes the selected resource or folder";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ResourceTreeCopy
			// 
			this.ResourceTreeCopy.Enabled = false;
			this.ResourceTreeCopy.ImageIndex = 0;
			this.ResourceTreeCopy.ToolTipText = "Copies the current resource or folder to the clipboard";
			// 
			// ResourceTreeCut
			// 
			this.ResourceTreeCut.Enabled = false;
			this.ResourceTreeCut.ImageIndex = 1;
			this.ResourceTreeCut.ToolTipText = "Cuts the current resource or folder to the clipboard";
			// 
			// ResourceTreePaste
			// 
			this.ResourceTreePaste.Enabled = false;
			this.ResourceTreePaste.ImageIndex = 2;
			this.ResourceTreePaste.ToolTipText = "Pastes the current content of the clipboard";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ResourceTreeRefreshButton
			// 
			this.ResourceTreeRefreshButton.ImageIndex = 3;
			this.ResourceTreeRefreshButton.ToolTipText = "Refreshes the tree to match the current server state";
			// 
			// toolbarImages
			// 
			this.toolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImages.ImageSize = new System.Drawing.Size(32, 32);
			this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
			this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panInnerRight
			// 
			this.panInnerRight.Controls.Add(this.toolBar2);
			this.panInnerRight.Controls.Add(this.tabItems);
			this.panInnerRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panInnerRight.Location = new System.Drawing.Point(304, 0);
			this.panInnerRight.Name = "panInnerRight";
			this.panInnerRight.Size = new System.Drawing.Size(400, 454);
			this.panInnerRight.TabIndex = 3;
			// 
			// toolBar2
			// 
			this.toolBar2.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar2.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.SaveResourceButton,
																						this.SaveResourceAsButton,
																						this.toolBarButton4,
																						this.PreviewButton,
																						this.EditAsXmlButton,
																						this.toolBarButton3,
																						this.ClosePageButton});
			this.toolBar2.DropDownArrows = true;
			this.toolBar2.ImageList = this.toolbarImages;
			this.toolBar2.Location = new System.Drawing.Point(0, 0);
			this.toolBar2.Name = "toolBar2";
			this.toolBar2.ShowToolTips = true;
			this.toolBar2.Size = new System.Drawing.Size(400, 44);
			this.toolBar2.TabIndex = 2;
			this.toolBar2.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBar2.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar2_ButtonClick);
			// 
			// SaveResourceButton
			// 
			this.SaveResourceButton.ImageIndex = 6;
			this.SaveResourceButton.ToolTipText = "Saves the current resource";
			// 
			// SaveResourceAsButton
			// 
			this.SaveResourceAsButton.ImageIndex = 7;
			this.SaveResourceAsButton.ToolTipText = "Saves the current resource under a different name";
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// PreviewButton
			// 
			this.PreviewButton.ImageIndex = 12;
			this.PreviewButton.ToolTipText = "Preview the item";
			// 
			// EditAsXmlButton
			// 
			this.EditAsXmlButton.ImageIndex = 8;
			this.EditAsXmlButton.ToolTipText = "Edits the current resource in an xml editor";
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ClosePageButton
			// 
			this.ClosePageButton.ImageIndex = 11;
			this.ClosePageButton.ToolTipText = "Close the current page";
			// 
			// tabItems
			// 
			this.tabItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabItems.ItemSize = new System.Drawing.Size(0, 18);
			this.tabItems.Location = new System.Drawing.Point(0, 48);
			this.tabItems.Name = "tabItems";
			this.tabItems.SelectedIndex = 0;
			this.tabItems.Size = new System.Drawing.Size(392, 396);
			this.tabItems.TabIndex = 1;
			// 
			// panOuter
			// 
			this.panOuter.Controls.Add(this.panInnerRight);
			this.panOuter.Controls.Add(this.splitter1);
			this.panOuter.Controls.Add(this.panLeftTree);
			this.panOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panOuter.Location = new System.Drawing.Point(0, 0);
			this.panOuter.Name = "panOuter";
			this.panOuter.Size = new System.Drawing.Size(704, 454);
			this.panOuter.TabIndex = 5;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(296, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 454);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// toolbarImagesSmall
			// 
			this.toolbarImagesSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImagesSmall.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarImagesSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImagesSmall.ImageStream")));
			this.toolbarImagesSmall.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// KeepAliveTimer
			// 
			this.KeepAliveTimer.Interval = 300000;
			this.KeepAliveTimer.Tick += new System.EventHandler(this.KeepAliveTimer_Tick);
			// 
			// MainMenu
			// 
			this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem2,
																					 this.MainMenuEdit,
																					 this.menuItem3});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
																					  this.MainMenuChangeServer,
																					  this.OpenSiteAdmin,
																					  this.menuItem22,
																					  this.MainMenuExit});
			this.menuItem2.Text = "File";
			// 
			// MainMenuNew
			// 
			this.MainMenuNew.Index = 0;
			this.MainMenuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.MainMenuNew.Text = "New";
			this.MainMenuNew.Click += new System.EventHandler(this.MainMenuNew_Click);
			// 
			// MainMenuOpen
			// 
			this.MainMenuOpen.Index = 1;
			this.MainMenuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.MainMenuOpen.Text = "Open";
			this.MainMenuOpen.Click += new System.EventHandler(this.MainMenuOpen_Click);
			// 
			// MainMenuClose
			// 
			this.MainMenuClose.Index = 2;
			this.MainMenuClose.Shortcut = System.Windows.Forms.Shortcut.CtrlF4;
			this.MainMenuClose.Text = "Close";
			this.MainMenuClose.Click += new System.EventHandler(this.MainMenuClose_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 3;
			this.menuItem17.Text = "-";
			// 
			// MainMenuSave
			// 
			this.MainMenuSave.Index = 4;
			this.MainMenuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.MainMenuSave.Text = "Save";
			this.MainMenuSave.Click += new System.EventHandler(this.MainMenuSave_Click);
			// 
			// MainMenuSaveAs
			// 
			this.MainMenuSaveAs.Index = 5;
			this.MainMenuSaveAs.Text = "Save as...";
			this.MainMenuSaveAs.Click += new System.EventHandler(this.MainMenuSaveAs_Click);
			// 
			// MainMenuSaveAll
			// 
			this.MainMenuSaveAll.Index = 6;
			this.MainMenuSaveAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.MainMenuSaveAll.Text = "Save all";
			this.MainMenuSaveAll.Click += new System.EventHandler(this.MainMenuSaveAll_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 7;
			this.menuItem11.Text = "-";
			// 
			// MainMenuEditAsXml
			// 
			this.MainMenuEditAsXml.Index = 8;
			this.MainMenuEditAsXml.Text = "Edit as Xml";
			this.MainMenuEditAsXml.Click += new System.EventHandler(this.MainMenuEditAsXml_Click);
			// 
			// MainMenuSaveAsXml
			// 
			this.MainMenuSaveAsXml.Index = 9;
			this.MainMenuSaveAsXml.Text = "Save as Xml...";
			this.MainMenuSaveAsXml.Click += new System.EventHandler(this.MainMenuSaveAsXml_Click);
			// 
			// MainMenuLoadFromXml
			// 
			this.MainMenuLoadFromXml.Index = 10;
			this.MainMenuLoadFromXml.Text = "Load from Xml...";
			this.MainMenuLoadFromXml.Click += new System.EventHandler(this.MainMenuLoadFromXml_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 11;
			this.menuItem20.Text = "-";
			// 
			// MainMenuChangeServer
			// 
			this.MainMenuChangeServer.Index = 12;
			this.MainMenuChangeServer.Text = "Change server...";
			this.MainMenuChangeServer.Click += new System.EventHandler(this.MainMenuChangeServer_Click);
			// 
			// OpenSiteAdmin
			// 
			this.OpenSiteAdmin.Index = 13;
			this.OpenSiteAdmin.Text = "Open Site Administrator...";
			this.OpenSiteAdmin.Click += new System.EventHandler(this.OpenSiteAdmin_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 14;
			this.menuItem22.Text = "-";
			// 
			// MainMenuExit
			// 
			this.MainMenuExit.Index = 15;
			this.MainMenuExit.Text = "Exit";
			this.MainMenuExit.Click += new System.EventHandler(this.MainMenuExit_Click);
			// 
			// MainMenuEdit
			// 
			this.MainMenuEdit.Index = 1;
			this.MainMenuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.MainMenuCut,
																						 this.MainMenuCopy,
																						 this.MainMenuPaste});
			this.MainMenuEdit.Text = "Edit";
			// 
			// MainMenuCut
			// 
			this.MainMenuCut.Index = 0;
			this.MainMenuCut.Text = "Cut";
			this.MainMenuCut.Click += new System.EventHandler(this.MainMenuCut_Click);
			// 
			// MainMenuCopy
			// 
			this.MainMenuCopy.Index = 1;
			this.MainMenuCopy.Text = "Copy";
			this.MainMenuCopy.Click += new System.EventHandler(this.MainMenuCopy_Click);
			// 
			// MainMenuPaste
			// 
			this.MainMenuPaste.Index = 2;
			this.MainMenuPaste.Text = "Paste";
			this.MainMenuPaste.Click += new System.EventHandler(this.MainMenuPaste_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.MainMenuAbout});
			this.menuItem3.Text = "Help";
			// 
			// MainMenuAbout
			// 
			this.MainMenuAbout.Index = 0;
			this.MainMenuAbout.Text = "About...";
			this.MainMenuAbout.Click += new System.EventHandler(this.MainMenuAbout_Click);
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 454);
			this.Controls.Add(this.panOuter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.MainMenu;
			this.Name = "FormMain";
			this.Text = "MapGuide Maestro";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormMain_Closing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.panLeftTree.ResumeLayout(false);
			this.panInnerRight.ResumeLayout(false);
			this.panOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMain_Load(object sender, System.EventArgs e)
		{
			this.Show();
		
			FormLogin frm = new FormLogin();
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

			string templatepath = System.IO.Path.Combine(Application.StartupPath, "Templates");
			m_templateMenuIndex = new Hashtable();
			NewResourceMenu.MenuItems.Clear();

			WallpaperChanger.ExtendedMenuItem folderMenu = new WallpaperChanger.ExtendedMenuItem(m_globalizor.Translate("Folder") , new System.EventHandler(NewFolderMenuItem_Click));
			folderMenu.Image = m_editors.SmallImageList.Images[m_editors.FolderIcon];

			WallpaperChanger.ExtendedMenuItem folderMenu2 = new WallpaperChanger.ExtendedMenuItem(m_globalizor.Translate("Folder") , new System.EventHandler(NewFolderMenuItem_Click));
			folderMenu2.Image = m_editors.SmallImageList.Images[m_editors.FolderIcon];

			NewResourceMenu.MenuItems.Add(folderMenu);
			NewResourceMenu.MenuItems.Add(new MenuItem("-"));

			MainMenuNew.MenuItems.Clear();
			MainMenuNew.MenuItems.Add(folderMenu2);
			MainMenuNew.MenuItems.Add(new MenuItem("-"));

			NewMenu.MenuItems.Clear();

			if (System.IO.Directory.Exists(templatepath))
			{
				foreach(string file in System.IO.Directory.GetFiles(templatepath))
				{
					WallpaperChanger.ExtendedMenuItem menu = new WallpaperChanger.ExtendedMenuItem(System.IO.Path.GetFileNameWithoutExtension(file), new System.EventHandler(NewResourceMenu_Clicked));
					menu.Image = m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)];
					m_templateMenuIndex.Add(menu, file);
					NewResourceMenu.MenuItems.Add(menu);

					menu = new WallpaperChanger.ExtendedMenuItem(System.IO.Path.GetFileNameWithoutExtension(file), new System.EventHandler(NewResourceMenu_Clicked));
					menu.Image = m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)];
					m_templateMenuIndex.Add(menu, file);
					MainMenuNew.MenuItems.Add(menu);

					menu = new WallpaperChanger.ExtendedMenuItem(System.IO.Path.GetFileNameWithoutExtension(file), new System.EventHandler(NewResourceMenu_Clicked));
					menu.Image = m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)];
					m_templateMenuIndex.Add(menu, file);
					NewMenu.MenuItems.Add(menu);
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
					//TODO: Handle cancel a little more gracefully
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

		private void TreeContextMenu_Popup(object sender, System.EventArgs e)
		{
			if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
			{
				foreach(MenuItem m in TreeContextMenu.MenuItems)
					m.Enabled = false;
			}
			else
			{
				foreach(MenuItem m in TreeContextMenu.MenuItems)
					m.Enabled = true;
			}

			//TODO: Implement the properties dialog
			PropertiesMenu.Enabled = false;
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

		private void ResourceTreeToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == DeleteResourceButton)
				DeleteResource();
			else if (e.Button == AddResourceButton)
				NewResourceMenu.Show(ResourceTreeToolbar, new Point(e.Button.Rectangle.Left, e.Button.Rectangle.Bottom));
			else if (e.Button == AddFolderButton)
				AddFolder();
			else if (e.Button == ResourceTreeCopy)
				ClipboardCopy();
			else if (e.Button == ResourceTreeCut)
				ClipboardCut();
			else if (e.Button == ResourceTreePaste)
				ClipboardPaste();
			else if (e.Button == ResourceTreeRefreshButton)
			{
				RebuildDocumentTree();
				//TODO: Remove any open resources that do not exist any more
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
					((ResourceEditor)edi.Page.Controls[0]).Preview();
					break;
				}
		}

		private void toolBar2_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == SaveResourceButton)
				SaveResource();
			else if (e.Button == SaveResourceAsButton)
				SaveResourceAs();
			else if (e.Button == EditAsXmlButton)
				EditAsXml();
			else if (e.Button == ClosePageButton)
				ClosePage();
			else if (e.Button == PreviewButton)
				OpenPreview();		
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
				ResourceTreeToolbar_ButtonClick(null, new ToolBarButtonClickEventArgs(DeleteResourceButton));
				e.Handled = true;
			}
			else if ((e.KeyCode == Keys.R && e.Control) || e.KeyCode == Keys.F5)
			{
				ResourceTreeToolbar_ButtonClick(null, new ToolBarButtonClickEventArgs(ResourceTreeRefreshButton));
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
			ResourceTreeToolbar_ButtonClick(null, new ToolBarButtonClickEventArgs(ResourceTreeCut));
		}

		private void CopyMenu_Click(object sender, System.EventArgs e)
		{
			ResourceTreeToolbar_ButtonClick(null, new ToolBarButtonClickEventArgs(ResourceTreeCopy));
		}

		private void PasteMenu_Click(object sender, System.EventArgs e)
		{
			ResourceTreeToolbar_ButtonClick(null, new ToolBarButtonClickEventArgs(ResourceTreePaste));
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
					foreach(OSGeo.MapGuide.Maestro.EditorInterface edi in this.OpenResourceEditors.Values)
						edi.Close(false);
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
			try
			{
				string url = ((OSGeo.MapGuide.MaestroAPI.HttpServerConnection)m_connection).BaseURL + "mapadmin/login.php";
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
	}
}
