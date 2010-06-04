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
using System.Collections.Generic;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for FormMain.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
        private Dictionary<string, EditorInterface> m_userControls = new Dictionary<string, EditorInterface>();
		public System.Windows.Forms.TabControl tabItems;
		private System.Windows.Forms.ContextMenuStrip TreeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem PropertiesMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveXmlAsMenu;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.ComponentModel.IContainer components;
        private ResourceBrowser.ResourceTree ResourceTree;
        private System.Windows.Forms.ImageList toolbarImagesSmall;

		private Hashtable m_templateMenuIndex = null;

        private TreeNode m_clipboardBuffer = null;
		private bool m_clipboardCut = false;

        private System.Windows.Forms.ToolStripSeparator menuItem1;
        private System.Windows.Forms.ToolStripSeparator menuItem7;
        private System.Windows.Forms.ToolStripMenuItem EditAsXmlMenu;
        private System.Windows.Forms.ToolStripMenuItem LoadFromXmlMenu;
        private System.Windows.Forms.ToolStripMenuItem CutMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyMenu;
        private System.Windows.Forms.ToolStripMenuItem PasteMenu;
		private System.Windows.Forms.Timer KeepAliveTimer;
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
        private ToolTip TabPageTooltip;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem validateResourcesToolStripMenuItem;
        private ToolStripMenuItem RenameMenu;
		
		private ToolStripMenuItem FindReplaceMenu;
		
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem DuplicateMenu;
        private SaveFileDialog SaveAsXmlDialog;
        private OpenFileDialog OpenXmlFileDialog;
        private ToolStripMenuItem MainMenuCloseAll;
        private ToolStripMenuItem OpenAllChildrenMenu;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem FindReplaceChildrenMenu;
        private string m_lastTabPageTooltip;

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
            this.ResourceTree = new OSGeo.MapGuide.Maestro.ResourceBrowser.ResourceTree();
            this.TreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyResourceIdMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.validateResourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenAllChildrenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.EditAsXmlMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFromXmlMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveXmlAsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.FindReplaceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FindReplaceChildrenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MainMenuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
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
            this.TabPageTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SaveAsXmlDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenXmlFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            this.ResourceTree.Cache = null;
            this.ResourceTree.ContextMenuStrip = this.TreeContextMenu;
            resources.ApplyResources(this.ResourceTree, "ResourceTree");
            this.ResourceTree.LabelEdit = true;
            this.ResourceTree.Name = "ResourceTree";
            this.ResourceTree.ShowNodeToolTips = true;
            this.ResourceTree.Sorted = true;
            this.ResourceTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_AfterLabelEdit);
            this.ResourceTree.DoubleClick += new System.EventHandler(this.ResourceTree_DoubleClick);
            this.ResourceTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragDrop);
            this.ResourceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterSelect);
            this.ResourceTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResourceTree_MouseDown);
            this.ResourceTree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResourceTree_KeyPress);
            this.ResourceTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ResourceTree_KeyUp);
            this.ResourceTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ResourceTree_BeforeLabelEdit);
            this.ResourceTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.ResourceTree_AfterExpand);
            this.ResourceTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ResourceTree_ItemDrag);
            this.ResourceTree.DragOver += new System.Windows.Forms.DragEventHandler(this.ResourceTree_DragOver);
            // 
            // TreeContextMenu
            // 
            this.TreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PropertiesMenu,
            this.toolStripSeparator6,
            this.CopyResourceIdMenu,
            this.toolStripSeparator9,
            this.validateResourcesToolStripMenuItem,
            this.menuItem7,
            this.OpenAllChildrenMenu,
            this.EditAsXmlMenu,
            this.LoadFromXmlMenu,
            this.SaveXmlAsMenu,
            this.toolStripSeparator10,
            this.FindReplaceMenu,
            this.FindReplaceChildrenMenu,
            this.menuItem1,
            this.CutMenu,
            this.CopyMenu,
            this.PasteMenu,
            this.DuplicateMenu,
            this.menuItem4,
            this.DeleteMenu,
            this.NewMenu,
            this.RenameMenu,
            this.openToolStripMenuItem});
            this.TreeContextMenu.Name = "TreeContextMenu";
            resources.ApplyResources(this.TreeContextMenu, "TreeContextMenu");
            this.TreeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TreeContextMenu_Popup);
            // 
            // PropertiesMenu
            // 
            this.PropertiesMenu.Name = "PropertiesMenu";
            resources.ApplyResources(this.PropertiesMenu, "PropertiesMenu");
            this.PropertiesMenu.Click += new System.EventHandler(this.PropertiesMenu_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // CopyResourceIdMenu
            // 
            this.CopyResourceIdMenu.Name = "CopyResourceIdMenu";
            resources.ApplyResources(this.CopyResourceIdMenu, "CopyResourceIdMenu");
            this.CopyResourceIdMenu.Click += new System.EventHandler(this.CopyResourceIdMenu_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // validateResourcesToolStripMenuItem
            // 
            this.validateResourcesToolStripMenuItem.Name = "validateResourcesToolStripMenuItem";
            resources.ApplyResources(this.validateResourcesToolStripMenuItem, "validateResourcesToolStripMenuItem");
            this.validateResourcesToolStripMenuItem.Click += new System.EventHandler(this.validateResourcesToolStripMenuItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Name = "menuItem7";
            resources.ApplyResources(this.menuItem7, "menuItem7");
            // 
            // OpenAllChildrenMenu
            // 
            this.OpenAllChildrenMenu.Name = "OpenAllChildrenMenu";
            resources.ApplyResources(this.OpenAllChildrenMenu, "OpenAllChildrenMenu");
            this.OpenAllChildrenMenu.Click += new System.EventHandler(this.EditAsXmlMenu_Click);
            // 
            // EditAsXmlMenu
            // 
            this.EditAsXmlMenu.Name = "EditAsXmlMenu";
            resources.ApplyResources(this.EditAsXmlMenu, "EditAsXmlMenu");
            this.EditAsXmlMenu.Click += new System.EventHandler(this.EditAsXmlMenu_Click);
            // 
            // LoadFromXmlMenu
            // 
            this.LoadFromXmlMenu.Name = "LoadFromXmlMenu";
            resources.ApplyResources(this.LoadFromXmlMenu, "LoadFromXmlMenu");
            this.LoadFromXmlMenu.Click += new System.EventHandler(this.LoadFromXmlMenu_Click);
            // 
            // SaveXmlAsMenu
            // 
            this.SaveXmlAsMenu.Name = "SaveXmlAsMenu";
            resources.ApplyResources(this.SaveXmlAsMenu, "SaveXmlAsMenu");
            this.SaveXmlAsMenu.Click += new System.EventHandler(this.SaveXmlAsMenu_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // FindReplaceMenu
            // 
            this.FindReplaceMenu.Name = "FindReplaceMenu";
            resources.ApplyResources(this.FindReplaceMenu, "FindReplaceMenu");
            // 
            // FindReplaceChildrenMenu
            // 
            this.FindReplaceChildrenMenu.Name = "FindReplaceChildrenMenu";
            resources.ApplyResources(this.FindReplaceChildrenMenu, "FindReplaceChildrenMenu");
            // 
            // menuItem1
            // 
            this.menuItem1.Name = "menuItem1";
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // CutMenu
            // 
            this.CutMenu.Name = "CutMenu";
            resources.ApplyResources(this.CutMenu, "CutMenu");
            this.CutMenu.Click += new System.EventHandler(this.CutMenu_Click);
            // 
            // CopyMenu
            // 
            this.CopyMenu.Name = "CopyMenu";
            resources.ApplyResources(this.CopyMenu, "CopyMenu");
            this.CopyMenu.Click += new System.EventHandler(this.CopyMenu_Click);
            // 
            // PasteMenu
            // 
            this.PasteMenu.Name = "PasteMenu";
            resources.ApplyResources(this.PasteMenu, "PasteMenu");
            this.PasteMenu.Click += new System.EventHandler(this.PasteMenu_Click);
            // 
            // DuplicateMenu
            // 
            this.DuplicateMenu.Name = "DuplicateMenu";
            resources.ApplyResources(this.DuplicateMenu, "DuplicateMenu");
            this.DuplicateMenu.Click += new System.EventHandler(this.DuplicateMenu_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Name = "menuItem4";
            resources.ApplyResources(this.menuItem4, "menuItem4");
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Name = "DeleteMenu";
            resources.ApplyResources(this.DeleteMenu, "DeleteMenu");
            this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
            // 
            // NewMenu
            // 
            this.NewMenu.Name = "NewMenu";
            resources.ApplyResources(this.NewMenu, "NewMenu");
            // 
            // RenameMenu
            // 
            this.RenameMenu.Name = "RenameMenu";
            resources.ApplyResources(this.RenameMenu, "RenameMenu");
            this.RenameMenu.Click += new System.EventHandler(this.RenameMenu_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
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
            resources.ApplyResources(this.ResourceTreeToolbar, "ResourceTreeToolbar");
            this.ResourceTreeToolbar.Name = "ResourceTreeToolbar";
            this.ResourceTreeToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddResourceButton
            // 
            this.AddResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddResourceButton, "AddResourceButton");
            this.AddResourceButton.Name = "AddResourceButton";
            // 
            // DeleteResourceButton
            // 
            this.DeleteResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DeleteResourceButton, "DeleteResourceButton");
            this.DeleteResourceButton.Name = "DeleteResourceButton";
            this.DeleteResourceButton.Click += new System.EventHandler(this.DeleteResourceButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ResourceTreeCopy
            // 
            this.ResourceTreeCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ResourceTreeCopy, "ResourceTreeCopy");
            this.ResourceTreeCopy.Name = "ResourceTreeCopy";
            this.ResourceTreeCopy.Click += new System.EventHandler(this.ResourceTreeCopy_Click);
            // 
            // ResourceTreeCut
            // 
            this.ResourceTreeCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ResourceTreeCut, "ResourceTreeCut");
            this.ResourceTreeCut.Name = "ResourceTreeCut";
            this.ResourceTreeCut.Click += new System.EventHandler(this.ResourceTreeCut_Click);
            // 
            // ResourceTreePaste
            // 
            this.ResourceTreePaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ResourceTreePaste, "ResourceTreePaste");
            this.ResourceTreePaste.Name = "ResourceTreePaste";
            this.ResourceTreePaste.Click += new System.EventHandler(this.ResourceTreePaste_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ResourceTreeRefreshButton
            // 
            this.ResourceTreeRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ResourceTreeRefreshButton, "ResourceTreeRefreshButton");
            this.ResourceTreeRefreshButton.Name = "ResourceTreeRefreshButton";
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
            resources.ApplyResources(this.tabItems, "tabItems");
            this.tabItems.Name = "tabItems";
            this.tabItems.SelectedIndex = 0;
            this.tabItems.MouseLeave += new System.EventHandler(this.tabItems_MouseLeave);
            this.tabItems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tabItems_MouseMove);
            this.tabItems.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabItems_ControlAdded);
            this.tabItems.Click += new System.EventHandler(this.tabItems_Click);
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
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // menuItem2
            // 
            this.menuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuNew,
            this.MainMenuOpen,
            this.MainMenuClose,
            this.MainMenuCloseAll,
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
            resources.ApplyResources(this.menuItem2, "menuItem2");
            // 
            // MainMenuNew
            // 
            resources.ApplyResources(this.MainMenuNew, "MainMenuNew");
            this.MainMenuNew.Name = "MainMenuNew";
            this.MainMenuNew.Click += new System.EventHandler(this.MainMenuNew_Click);
            // 
            // MainMenuOpen
            // 
            this.MainMenuOpen.Name = "MainMenuOpen";
            resources.ApplyResources(this.MainMenuOpen, "MainMenuOpen");
            this.MainMenuOpen.Click += new System.EventHandler(this.MainMenuOpen_Click);
            // 
            // MainMenuClose
            // 
            resources.ApplyResources(this.MainMenuClose, "MainMenuClose");
            this.MainMenuClose.Name = "MainMenuClose";
            this.MainMenuClose.Click += new System.EventHandler(this.MainMenuClose_Click);
            // 
            // MainMenuCloseAll
            // 
            resources.ApplyResources(this.MainMenuCloseAll, "MainMenuCloseAll");
            this.MainMenuCloseAll.Name = "MainMenuCloseAll";
            this.MainMenuCloseAll.Click += new System.EventHandler(this.MainMenuCloseAll_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Name = "menuItem17";
            resources.ApplyResources(this.menuItem17, "menuItem17");
            // 
            // MainMenuSave
            // 
            resources.ApplyResources(this.MainMenuSave, "MainMenuSave");
            this.MainMenuSave.Name = "MainMenuSave";
            this.MainMenuSave.Click += new System.EventHandler(this.MainMenuSave_Click);
            // 
            // MainMenuSaveAs
            // 
            resources.ApplyResources(this.MainMenuSaveAs, "MainMenuSaveAs");
            this.MainMenuSaveAs.Name = "MainMenuSaveAs";
            this.MainMenuSaveAs.Click += new System.EventHandler(this.MainMenuSaveAs_Click);
            // 
            // MainMenuSaveAll
            // 
            resources.ApplyResources(this.MainMenuSaveAll, "MainMenuSaveAll");
            this.MainMenuSaveAll.Name = "MainMenuSaveAll";
            this.MainMenuSaveAll.Click += new System.EventHandler(this.MainMenuSaveAll_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            resources.ApplyResources(this.menuItem11, "menuItem11");
            // 
            // MainMenuEditAsXml
            // 
            resources.ApplyResources(this.MainMenuEditAsXml, "MainMenuEditAsXml");
            this.MainMenuEditAsXml.Name = "MainMenuEditAsXml";
            this.MainMenuEditAsXml.Click += new System.EventHandler(this.MainMenuEditAsXml_Click);
            // 
            // MainMenuSaveAsXml
            // 
            this.MainMenuSaveAsXml.Name = "MainMenuSaveAsXml";
            resources.ApplyResources(this.MainMenuSaveAsXml, "MainMenuSaveAsXml");
            this.MainMenuSaveAsXml.Click += new System.EventHandler(this.MainMenuSaveAsXml_Click);
            // 
            // MainMenuLoadFromXml
            // 
            this.MainMenuLoadFromXml.Name = "MainMenuLoadFromXml";
            resources.ApplyResources(this.MainMenuLoadFromXml, "MainMenuLoadFromXml");
            this.MainMenuLoadFromXml.Click += new System.EventHandler(this.MainMenuLoadFromXml_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Name = "menuItem20";
            resources.ApplyResources(this.menuItem20, "menuItem20");
            // 
            // MainMenuChangePreferences
            // 
            this.MainMenuChangePreferences.Name = "MainMenuChangePreferences";
            resources.ApplyResources(this.MainMenuChangePreferences, "MainMenuChangePreferences");
            this.MainMenuChangePreferences.Click += new System.EventHandler(this.MainMenuChangePreferences_Click);
            // 
            // MainMenuChangeServer
            // 
            this.MainMenuChangeServer.Name = "MainMenuChangeServer";
            resources.ApplyResources(this.MainMenuChangeServer, "MainMenuChangeServer");
            this.MainMenuChangeServer.Click += new System.EventHandler(this.MainMenuChangeServer_Click);
            // 
            // OpenSiteAdmin
            // 
            this.OpenSiteAdmin.Name = "OpenSiteAdmin";
            resources.ApplyResources(this.OpenSiteAdmin, "OpenSiteAdmin");
            this.OpenSiteAdmin.Click += new System.EventHandler(this.OpenSiteAdmin_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Name = "menuItem22";
            resources.ApplyResources(this.menuItem22, "menuItem22");
            // 
            // MainMenuExit
            // 
            this.MainMenuExit.Name = "MainMenuExit";
            resources.ApplyResources(this.MainMenuExit, "MainMenuExit");
            this.MainMenuExit.Click += new System.EventHandler(this.MainMenuExit_Click);
            // 
            // MainMenuEdit
            // 
            this.MainMenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenuCut,
            this.MainMenuCopy,
            this.MainMenuPaste});
            this.MainMenuEdit.Name = "MainMenuEdit";
            resources.ApplyResources(this.MainMenuEdit, "MainMenuEdit");
            // 
            // MainMenuCut
            // 
            resources.ApplyResources(this.MainMenuCut, "MainMenuCut");
            this.MainMenuCut.Name = "MainMenuCut";
            this.MainMenuCut.Click += new System.EventHandler(this.MainMenuCut_Click);
            // 
            // MainMenuCopy
            // 
            resources.ApplyResources(this.MainMenuCopy, "MainMenuCopy");
            this.MainMenuCopy.Name = "MainMenuCopy";
            this.MainMenuCopy.Click += new System.EventHandler(this.MainMenuCopy_Click);
            // 
            // MainMenuPaste
            // 
            resources.ApplyResources(this.MainMenuPaste, "MainMenuPaste");
            this.MainMenuPaste.Name = "MainMenuPaste";
            this.MainMenuPaste.Click += new System.EventHandler(this.MainMenuPaste_Click);
            // 
            // packagesToolStripMenuItem
            // 
            this.packagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPackageToolStripMenuItem,
            this.modifyPackageToolStripMenuItem,
            this.restorePackageToolStripMenuItem});
            this.packagesToolStripMenuItem.Name = "packagesToolStripMenuItem";
            resources.ApplyResources(this.packagesToolStripMenuItem, "packagesToolStripMenuItem");
            // 
            // createPackageToolStripMenuItem
            // 
            this.createPackageToolStripMenuItem.Name = "createPackageToolStripMenuItem";
            resources.ApplyResources(this.createPackageToolStripMenuItem, "createPackageToolStripMenuItem");
            this.createPackageToolStripMenuItem.Click += new System.EventHandler(this.createPackageToolStripMenuItem_Click);
            // 
            // modifyPackageToolStripMenuItem
            // 
            this.modifyPackageToolStripMenuItem.Name = "modifyPackageToolStripMenuItem";
            resources.ApplyResources(this.modifyPackageToolStripMenuItem, "modifyPackageToolStripMenuItem");
            this.modifyPackageToolStripMenuItem.Click += new System.EventHandler(this.modifyPackageToolStripMenuItem_Click);
            // 
            // restorePackageToolStripMenuItem
            // 
            this.restorePackageToolStripMenuItem.Name = "restorePackageToolStripMenuItem";
            resources.ApplyResources(this.restorePackageToolStripMenuItem, "restorePackageToolStripMenuItem");
            this.restorePackageToolStripMenuItem.Click += new System.EventHandler(this.restorePackageToolStripMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLastExceptionToolStripMenuItem,
            this.toolStripSeparator5,
            this.MainMenuAbout});
            this.menuItem3.Name = "menuItem3";
            resources.ApplyResources(this.menuItem3, "menuItem3");
            // 
            // viewLastExceptionToolStripMenuItem
            // 
            this.viewLastExceptionToolStripMenuItem.Name = "viewLastExceptionToolStripMenuItem";
            resources.ApplyResources(this.viewLastExceptionToolStripMenuItem, "viewLastExceptionToolStripMenuItem");
            this.viewLastExceptionToolStripMenuItem.Click += new System.EventHandler(this.viewLastExceptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // MainMenuAbout
            // 
            this.MainMenuAbout.Name = "MainMenuAbout";
            resources.ApplyResources(this.MainMenuAbout, "MainMenuAbout");
            this.MainMenuAbout.Click += new System.EventHandler(this.MainMenuAbout_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ResourceTree);
            this.splitContainer1.Panel1.Controls.Add(this.ResourceTreeToolbar);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabItems);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
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
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // SaveResourceButton
            // 
            this.SaveResourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.SaveResourceButton, "SaveResourceButton");
            this.SaveResourceButton.Name = "SaveResourceButton";
            this.SaveResourceButton.Click += new System.EventHandler(this.SaveResourceButton_Click);
            // 
            // SaveResourceAsButton
            // 
            this.SaveResourceAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.SaveResourceAsButton, "SaveResourceAsButton");
            this.SaveResourceAsButton.Name = "SaveResourceAsButton";
            this.SaveResourceAsButton.Click += new System.EventHandler(this.SaveResourceAsButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // PreviewButton
            // 
            this.PreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.PreviewButton, "PreviewButton");
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // EditAsXmlButton
            // 
            this.EditAsXmlButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.EditAsXmlButton, "EditAsXmlButton");
            this.EditAsXmlButton.Name = "EditAsXmlButton";
            this.EditAsXmlButton.Click += new System.EventHandler(this.EditAsXmlButton_Click);
            // 
            // ProfileButton
            // 
            this.ProfileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ProfileButton, "ProfileButton");
            this.ProfileButton.Name = "ProfileButton";
            this.ProfileButton.Click += new System.EventHandler(this.ProfileButton_Click);
            // 
            // ValidateButton
            // 
            this.ValidateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ValidateButton, "ValidateButton");
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // ClosePageButton
            // 
            this.ClosePageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ClosePageButton, "ClosePageButton");
            this.ClosePageButton.Name = "ClosePageButton";
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
            resources.ApplyResources(this.TabPageContextMenu, "TabPageContextMenu");
            this.TabPageContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TabPageContextMenu_Opening);
            // 
            // TabClosePageMenu
            // 
            this.TabClosePageMenu.Name = "TabClosePageMenu";
            resources.ApplyResources(this.TabClosePageMenu, "TabClosePageMenu");
            this.TabClosePageMenu.Click += new System.EventHandler(this.TabClosePageMenu_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // TabSaveMenu
            // 
            this.TabSaveMenu.Name = "TabSaveMenu";
            resources.ApplyResources(this.TabSaveMenu, "TabSaveMenu");
            this.TabSaveMenu.Click += new System.EventHandler(this.TabSaveMenu_Click);
            // 
            // TabSaveAsMenu
            // 
            this.TabSaveAsMenu.Name = "TabSaveAsMenu";
            resources.ApplyResources(this.TabSaveAsMenu, "TabSaveAsMenu");
            this.TabSaveAsMenu.Click += new System.EventHandler(this.TabSaveAsMenu_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // TabCopyIdMenu
            // 
            this.TabCopyIdMenu.Name = "TabCopyIdMenu";
            resources.ApplyResources(this.TabCopyIdMenu, "TabCopyIdMenu");
            this.TabCopyIdMenu.Click += new System.EventHandler(this.TabCopyIdMenu_Click);
            // 
            // SaveAsXmlDialog
            // 
            resources.ApplyResources(this.SaveAsXmlDialog, "SaveAsXmlDialog");
            // 
            // OpenXmlFileDialog
            // 
            resources.ApplyResources(this.OpenXmlFileDialog, "OpenXmlFileDialog");
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "FormMain";
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
            frm.Icon = FormMain.MaestroIcon;

            frm.StartPosition = FormStartPosition.CenterParent;
			//frm.UseAutoConnect = true;

            if (frm.ShowDialog(this) == DialogResult.OK)
                m_connection = frm.Connection;
            else
            {
                Application.Exit();
                return;
            }

            if (m_connection is MaestroAPI.HttpServerConnection)
            {
                this.Text = string.Format(Strings.FormMain.WindowTitleTemplate, Application.ProductName, ((MaestroAPI.HttpServerConnection)m_connection).BaseURL);
                ((MaestroAPI.HttpServerConnection)m_connection).UserAgent = "MapGuide Maestro v" + Application.ProductVersion;
            }
            else
                this.Text = string.Format(Strings.FormMain.WindowTitleTemplate, Application.ProductName, Strings.FormMain.NativeConnectionName);


            //Reset
            Program.ApplicationSettings = PreferedSiteList.Load();

            //TODO: Allow thirdparty validators as well
            ResourceValidators.ResourceValidatorLoader.LoadStockValidators();

            KeepAliveTimer.Enabled = true;
			string editorMap = System.IO.Path.Combine(Application.StartupPath, "EditorMap.xml");
			if (!System.IO.File.Exists(editorMap))
			{
				MessageBox.Show(string.Format(Strings.FormMain.MissingEditorSetupError, editorMap), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}

			try
			{
                m_editors = new ResourceEditorMap(System.IO.Path.Combine(Application.StartupPath, "EditorMap.xml"));
			}
			catch(Exception ex)
			{
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
				MessageBox.Show(string.Format(Strings.FormMain.EditorLoadError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}

			this.ResourceTree.ImageList = m_editors.SmallImageList;
			this.tabItems.ImageList = m_editors.SmallImageList;

            //TODO: This should be case insensitive for linux
			string templatepath = System.IO.Path.Combine(Application.StartupPath, "Templates");
			m_templateMenuIndex = new Hashtable();

			MainMenuNew.DropDown.Items.Clear();
            MainMenuNew.DropDown.Items.Add(new ToolStripMenuItem(Strings.FormMain.ResourceNameFolder, m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            MainMenuNew.DropDown.Items.Add(new ToolStripSeparator());

            NewMenu.DropDown.Items.Clear();
            NewMenu.DropDown.Items.Add(new ToolStripMenuItem(Strings.FormMain.ResourceNameFolder, m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            NewMenu.DropDown.Items.Add(new ToolStripSeparator());

            AddResourceButton.DropDown.Items.Clear();
            AddResourceButton.DropDown.Items.Add(new ToolStripMenuItem(Strings.FormMain.ResourceNameFolder, m_editors.SmallImageList.Images[m_editors.FolderIcon], new System.EventHandler(NewFolderMenuItem_Click)));
            AddResourceButton.DropDown.Items.Add(new ToolStripSeparator());

			if (System.IO.Directory.Exists(templatepath))
			{
				foreach(string file in System.IO.Directory.GetFiles(templatepath))
				{
                    string name = System.IO.Path.GetFileNameWithoutExtension(file);
                    string localizedname = Strings.TemplateNames.ResourceManager.GetString(name.Replace(' ', '_')) ?? name;
                    ToolStripMenuItem menu = new ToolStripMenuItem(localizedname, m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					AddResourceButton.DropDown.Items.Add(menu);

                    menu = new ToolStripMenuItem(localizedname, m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					MainMenuNew.DropDown.Items.Add(menu);

                    menu = new ToolStripMenuItem(localizedname, m_editors.SmallImageList.Images[m_editors.GetImageIndexFromResourceID(file)], new System.EventHandler(NewResourceMenu_Clicked));
					m_templateMenuIndex.Add(menu, file);
					NewMenu.DropDown.Items.Add(menu);
				}
			}

			this.Refresh();

            try
            {
                InitFindReplaceContextMenu();
            }
            catch { }

            //Auto reloads tree
            ResourceTree.Cache = new OSGeo.MapGuide.Maestro.ResourceBrowser.RepositoryCache(m_connection, m_editors);
            ResourceTree.Cache.CacheResetEvent += new EventHandler(Cache_CacheResetEvent);
		}

        private void InitFindReplaceContextMenu()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("FindReplaceStrings.xml");

            XmlNodeList strings = doc.SelectNodes("//Strings/String");
            foreach (XmlNode str in strings)
            {
                try
                {
                    string find = str["Find"].InnerText;
                    string repl = str["Replace"].InnerText;

                    string tooltip = string.Format(Strings.FormMain.FindReplaceTooltip, find, repl);

                    EventHandler handler = (sender, e) => { FindReplaceMenu_Click(find, repl); };

                    var item = FindReplaceMenu.DropDown.Items.Add(repl, null, handler);
                    item.ToolTipText = tooltip;

                    //Can't reuse item references for different menus
                    var item2 = FindReplaceChildrenMenu.DropDown.Items.Add(repl, null, handler);
                    item2.ToolTipText = tooltip;
                }
                catch { }
            }
        }

        void Cache_CacheResetEvent(object sender, EventArgs e)
        {
            using (new WaitCursor(this))
                ResourceTree.RefreshTreeNodes();
        }

		private TreeNodeCollection FindParent(string resourceID, bool allowNotFound)
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
                    if (allowNotFound)
                        return null;
                    else
					    throw new Exception(string.Format(Strings.FormMain.LocateNodeInternalError, parts[i], resourceID));
			}

			return current;
		}

		private TreeNode FindItem(string resourceID, bool allowNotFound)
		{
			TreeNodeCollection parent = FindParent(resourceID, allowNotFound);

            if (parent == null)
                return null;

			string item = new MaestroAPI.ResourceIdentifier(resourceID).Name;
			foreach(TreeNode n in parent)
                if (n.Tag is MaestroAPI.ResourceListResourceDocument && (n.Tag as MaestroAPI.ResourceListResourceDocument).ResourceId == resourceID)
					return n;

            if (allowNotFound)
                return null;
            else
			    throw new Exception(string.Format(Strings.FormMain.ItemNotFoundInternalError, resourceID));
		}

        private bool FindAndSelectNode(string resourceId)
        {
            if (resourceId == null)
                return false;

            TreeNode tn = FindItem(resourceId, true);
            if (tn != null)
                ResourceTree.SelectedNode = tn;
            return tn != null;
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


		public void RebuildDocumentTree()
		{
            ResourceTree.Cache.Reset();
		}

		public OSGeo.MapGuide.MaestroAPI.ServerConnectionI CurrentConnection
		{
			get { return m_connection; }
		}

		public void OpenResource(string resourceID)
		{
			OpenResource(resourceID, null);
		}

		public void OpenResource(string resourceID, System.Type ControlType)
		{
			OpenResource( resourceID, ControlType, null, null);
		}
		public void OpenResource(string resourceID, System.Type ControlType, String szFind, String szReplace)
		{
			if (!m_userControls.ContainsKey(resourceID))
			{
				System.Type ClassDef = ControlType;
				
				if (ClassDef == null)
					ClassDef = m_editors.GetResourceEditorTypeFromResourceID(resourceID);
				if (ClassDef == null)
					ClassDef = typeof(ResourceEditors.XmlEditorControl);

				
				if (!String.IsNullOrEmpty( szFind))
					ClassDef = typeof(ResourceEditors.XmlEditorControl);
				

                if (ClassDef != null)
                {
                    try
                    {
						if (! String.IsNullOrEmpty( szFind))
							AddEditTab(ClassDef, resourceID, resourceID.StartsWith("Library://"), szFind, szReplace);
						else
						    AddEditTab(ClassDef, resourceID, resourceID.StartsWith("Library://"));
					}
                    catch (Exception ex)
                    {
                        LastException = ex;
                        Exception iex = ex;
                        while (iex is System.Reflection.TargetInvocationException && iex.InnerException != null)
                            iex = iex.InnerException;
                        if (!(iex is CancelException))
                        {
                            LastException = ex;
                            MessageBox.Show(this, string.Format(Strings.FormMain.OpenFailedError, resourceID, iex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    MessageBox.Show(this, string.Format(Strings.FormMain.UnknownResourceTypeError, resourceID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			return AddEditTab(controlType, resourceID, existing, null, null);
		}

		private EditorInterface AddEditTab(System.Type controlType, string resourceID, bool existing, String szFind, String szReplace)
		{
			TabPage tp;
			if (existing)
				tp = new TabPage(m_editors.GetResourceNameFromResourceID(resourceID));
			else
				tp = new TabPage(Strings.FormMain.NewResourceName + " *");

			tp.ImageIndex = m_editors.GetImageIndexFromResourceID(resourceID);
			
			// EditorInterface edi = new EditorInterface(this, tp, resourceID, existing);
			EditorInterface edi;
			if (String.IsNullOrEmpty( szFind))
				edi = new EditorInterface(this, tp, resourceID, existing);
			else
				edi = new EditorInterface(this, tp, resourceID, existing, szFind, szReplace);
			
			UserControl uc = (UserControl)Activator.CreateInstance(controlType,  new object[] {edi, edi.TempResourceId} );

            /*tp.BackgroundImage = new System.Drawing.Bitmap("test.png");
            tp.BackgroundImageLayout = ImageLayout.Stretch;
            uc.BackColor = Color.Transparent;*/

			tp.Controls.Add(uc);
			uc.Top = 16;
			uc.Left = 16;
			uc.Width = tp.Width - 32;
			uc.Height = tp.Height - 32;
			uc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			m_userControls.Add(edi.ResourceId, edi);
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
            var selNode = ResourceTree.SelectedNode;

            PropertiesMenu.Enabled =
            CopyResourceIdMenu.Enabled = selNode != null;
            PasteMenu.Enabled = (selNode != null && m_clipboardBuffer != null);
			NewMenu.Enabled = true;

            bool isRoot = (selNode != null && selNode.Level == 0);
            bool isFolder = isRoot || (selNode != null && selNode.Tag.GetType() == typeof(MaestroAPI.ResourceListResourceFolder));

            EditAsXmlMenu.Enabled = EditAsXmlButton.Enabled = !isFolder;

            SaveXmlAsMenu.Enabled = !isFolder;

            OpenAllChildrenMenu.Enabled = isFolder;
            OpenAllChildrenMenu.Enabled = isFolder;

            openToolStripMenuItem.Enabled = !isFolder;

            FindReplaceMenu.Enabled = !isFolder;
            FindReplaceChildrenMenu.Enabled = isFolder;

            DeleteMenu.Enabled = !isRoot; //Can't delete the root of the repo
            RenameMenu.Enabled = !isRoot; //Can't rename either
		}

		private void SaveXmlAsMenu_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (SaveAsXmlDialog.ShowDialog() == DialogResult.OK)
				{
					OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
					byte[] data = m_connection.GetResourceXmlData(document.ResourceId);
					using(System.IO.FileStream fs = System.IO.File.Open(SaveAsXmlDialog.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
						fs.Write(data, 0, data.Length);
				}
			}
			catch (Exception ex)
			{
                LastException = ex;
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
				MessageBox.Show(this, string.Format(Strings.FormMain.SaveResourceError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}


		private void NewFolderMenuItem_Click(object sender, System.EventArgs e)
		{
            AddFolder();
		}

		private void NewResourceMenu_Clicked(object sender, System.EventArgs e)
		{
			CreateItem((string)m_templateMenuIndex[sender]);
    	}

        public void CreateItem(string templatefilename)
        {
            if (templatefilename == null)
                return;

            try
            {
                string tempname = "Session:" + m_connection.SessionID + "//" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(templatefilename);
                using (System.IO.FileStream fs = new System.IO.FileStream(templatefilename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    m_connection.SetResourceXmlData(tempname, fs);

                OpenResource(tempname);
            }
            catch (Exception ex)
            {
                LastException = ex;
                MessageBox.Show(this, string.Format(Strings.FormMain.TemplateLoadError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                string resId = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder)item).ResourceId;

                //Find open resources in the folder
                List<string> toClose = new List<string>();
                foreach (string s in m_userControls.Keys)
                    if (s.StartsWith(resId))
                        toClose.Add(s);

                //Close them all
                foreach (string s in toClose)
                    if (!m_userControls[s].Close(true))
                        return;

				//We do not enumerate here, because it is SLOW
                if (ResourceTree.SelectedNode.Nodes.Count == 0)
                {
                    if (MessageBox.Show(this, Strings.FormMain.DeleteFolderConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                }
                else if (MessageBox.Show(this, Strings.FormMain.DeleteFolderAndResourcesConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                    return;

				try
				{
					m_connection.DeleteFolder(resId);
				}
				catch(Exception ex)
				{
                    LastException = ex;
					MessageBox.Show(this, string.Format(Strings.FormMain.DeleteFolderError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				RebuildDocumentTree();
			}
			else if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
			{
                string resId = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)item).ResourceId;
                if (m_userControls.ContainsKey(resId))
                    if (!m_userControls[resId].Close(true))
                        return;

				//We do not enumerate here, because it is SLOW
				if (MessageBox.Show(this, Strings.FormMain.DeleteResourceConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return;

				try
				{
					m_connection.DeleteResource(resId);
				}
				catch(Exception ex)
				{
                    LastException = ex;
					MessageBox.Show(this, string.Format(Strings.FormMain.DeleteResourceError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			string targetpath = ResourceTree.SelectedNode.FullPath + ResourceTree.PathSeparator + Strings.FormMain.NewFolderName;
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
            string actualTargetPath;
            if (MoveOrCopyResource(m_clipboardBuffer, ResourceTree.SelectedNode, m_clipboardCut, true, out actualTargetPath))
            {
                if (m_clipboardCut)
                {
                    ResourceTreePaste.Enabled = false;
                    m_clipboardBuffer = null;
                    m_clipboardCut = false;
                }

                FindAndSelectNode(actualTargetPath);
            }
		}

        private bool MoveOrCopyResource(TreeNode source, TreeNode target, bool move, bool refreshTree, out string actualTargetPath)
		{
            actualTargetPath = null;

			using (new WaitCursor(this))
			{
				if (source == null || source.Tag == null)
					return false;

				if (target == null)
					return false;

                if (source == target)
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

				return MoveOrCopyResource(sourcepath, targetpath, move, refreshTree, out actualTargetPath);
			}

		}

		private bool MoveOrCopyResource(string sourcepath, string targetpath, bool move, bool refreshTree, out string actualTargetPath)
		{
            actualTargetPath = null;

			using (new WaitCursor(this))
			{
				bool sourceisFolder = sourcepath.EndsWith("/");

				if (sourcepath == targetpath)
					if (move)
					{
						MessageBox.Show(this, Strings.FormMain.MoveIntoItselfError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						return false;
					}
					else
						targetpath = GetUniqueResourceName(sourcepath);

                actualTargetPath = targetpath;

				if (m_connection.ResourceExists(targetpath) && MessageBox.Show(this, sourceisFolder ? Strings.FormMain.OverwriteFolderConfirmation : Strings.FormMain.OverwriteResourceConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
					return false;


                if (move)
                    if (sourceisFolder)
                    {
                        List<string> toClose = new List<string>();
                        foreach (string s in m_userControls.Keys)
                            if (s.StartsWith(sourcepath))
                                toClose.Add(s);

                        foreach (string s in toClose)
                            if (!m_userControls[s].Close(true))
                                return false;

                        //Empty folders do not require updating
                        TreeNode item = FindItem(targetpath, true);

                        if (item != null && item.Nodes.Count == 1)
                            ResourceTree.Cache.BuildNode(item, false);

                        if (item != null && item.Nodes.Count == 0)
                        {
                            m_connection.MoveFolder(sourcepath, targetpath, false);

                            if (refreshTree)
                                RebuildDocumentTree();
                            return true;
                        }
                    }
                    else
                        if (m_userControls.ContainsKey(sourcepath))
                            if (!m_userControls[sourcepath].Close(true))
                                return false;

                if (move || sourceisFolder) //No update for copies
                {
                    switch (MessageBox.Show(this, Strings.FormMain.UpdateRelatedItemsConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
                    {
                        case DialogResult.Cancel:
                            return false;
                        case DialogResult.No:
                            if (move && sourceisFolder)
                                m_connection.MoveFolder(sourcepath, targetpath, true);
                            else if (sourceisFolder)
                                m_connection.CopyFolder(sourcepath, targetpath, true);
                            else if (move)
                                m_connection.MoveResource(sourcepath, targetpath, true);
                            else
                                m_connection.CopyResource(sourcepath, targetpath, true);

                            if (refreshTree)
                                RebuildDocumentTree();
                            return true;
                    }
                }
                else
                {
                    //No update for copies, just copy and return
                    m_connection.CopyResource(sourcepath, targetpath, true);
                    if (refreshTree)
                        RebuildDocumentTree();
                    return true;
                }

				LengthyOperation lwdlg = new LengthyOperation();

				if (sourceisFolder && move) 
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.MoveFolder, true);
				else if (sourceisFolder)
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.CopyFolder, true);
				else if (move)
					lwdlg.InitializeDialog(m_connection, sourcepath, targetpath, LengthyOperation.OperationType.MoveResource, true);
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

			string name = Strings.FormMain.NewFolderName + "/";
			int i = 1;
			while (res.ContainsKey(name) && i < 100)
				name = string.Format(Strings.FormMain.NewFolderName + " {0}", i) + "/";

			if (i >= 100)
				throw new Exception(Strings.FormMain.TooManyFoldersInternalError);

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
				resource = folderpath + string.Format(Strings.FormMain.FirstCopyOfName, resourcename);
				int i = 1;
				while (res.ContainsKey(resource) && i < 100)
					resource = folderpath + string.Format(Strings.FormMain.CopyOfName, (i++), resourcename);
		
				if (i == 100)
					throw new Exception(Strings.FormMain.TooManyCopiesInternalError);

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
					UpdateResourceTreeStatus();
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
                    if (edi.SaveAs())
                    {
                        //If we save under a different name, reload the parent folder
                        try
                        {
                            TreeNode c = FindItem(edi.ResourceId, true);
                            if (c == null || c.Parent == null)
                                RebuildDocumentTree(); //TODO: If the node is not open, skip the refresh
                            else
                                ReloadNode(c.Parent);
                        }
                        catch
                        {
                            RebuildDocumentTree();
                        }
                    }
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
					object resource = ((IResourceEditorControl)edi.Page.Controls[0]).Resource;
                    ResourceEditors.XmlEditor dlg = new ResourceEditors.XmlEditor(resource, edi.ResourceId, edi);
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						object o = dlg.SerializedObject;
						System.Reflection.PropertyInfo pi = o.GetType().GetProperty("ResourceId");
						if (pi != null)
							pi.SetValue(o, pi.GetValue(resource, null), null);

						pi = o.GetType().GetProperty("CurrentConnection");
						if (pi != null)
							pi.SetValue(o, pi.GetValue(resource, null), null);

						((IResourceEditorControl)edi.Page.Controls[0]).Resource = dlg.SerializedObject;
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
                        if (!((IResourceEditorControl)edi.Page.Controls[0]).Preview())
                            MessageBox.Show(this, Strings.FormMain.PreviewMissingError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        LastException = ex;
                        MessageBox.Show(this, string.Format(Strings.FormMain.PreviewFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		private void ResourceTree_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			UpdateResourceTreeStatus();
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
                string actualTargetPath;
                bool success;
				//Hmm 8 means CTRL, where is the constant?
				if ((e.KeyState & 8) != 0)
					success = MoveOrCopyResource(x, n, false, true, out actualTargetPath);
				else
					success = MoveOrCopyResource(x, n, true, true, out actualTargetPath);
                
                if (success)
                    FindAndSelectNode(actualTargetPath);
			}
			else
				return;
			
		}

		private void ResourceTree_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!ResourceTree.Focused)
				return;

            if (e.KeyCode == Keys.Delete)
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
				MessageBox.Show(this, Strings.FormMain.MultipleResourceError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.CancelEdit = true;
				return;
			}

            string actualTargetPath;
            if (MoveOrCopyResource(sourcepath, targetpath, true, false, out actualTargetPath))
            {
                e.Node.Text = e.Label;
                FindAndSelectNode(actualTargetPath);
            }

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
					switch(MessageBox.Show(this, Strings.FormMain.SaveResourcesConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
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
			EditAsXmlMenuClick(null, null);
		}
		private void EditAsXmlMenuClick(String szFind)
		{
			EditAsXmlMenuClick(szFind, null);
		}
		private void EditAsXmlMenuClick(String szFind, String szReplace)
		{
			using(new WaitCursor(this))
			{
				try
				{
                    bool isRoot = (ResourceTree.SelectedNode != null && ResourceTree.SelectedNode.Level == 0);
					if (ResourceTree.SelectedNode != null && (ResourceTree.SelectedNode.Tag != null || isRoot))
					{
                        if (!isRoot)
                        {
                            if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
                            {
                                OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ResourceTree.SelectedNode.Tag;
                                if (m_userControls.ContainsKey(document.ResourceId))
                                {
                                    EditorInterface edi = (EditorInterface)m_userControls[document.ResourceId];
                                    if (!edi.Close(true))
                                        return;
                                }

                                if (!String.IsNullOrEmpty(szFind))
                                    OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl), szFind, szReplace);
                                else
                                    OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl));
                            }
                            else
                            {
                                if (ResourceTree.SelectedNode.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceFolder))
                                {
                                    if (!ResourceTree.SelectedNode.IsExpanded)
                                        ResourceTree.SelectedNode.Expand();

                                    int iCount = GetDocumentNodeCount(ResourceTree.SelectedNode);
                                    string msg = string.Format(Strings.FormMain.ConfirmMultipleOpen, iCount);
                                    if (DialogResult.Yes != MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                                        return;

                                    foreach (TreeNode tnThis in ResourceTree.SelectedNode.Nodes)
                                    {
                                        // Document nodes are tagged with the ResourceListResourceDocument class
                                        if (tnThis.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
                                        {
                                            OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)tnThis.Tag;
                                            if (m_userControls.ContainsKey(document.ResourceId))
                                            {
                                                EditorInterface edi = (EditorInterface)m_userControls[document.ResourceId];
                                                if (!edi.Close(true))
                                                    return;
                                            }

                                            if (!String.IsNullOrEmpty(szFind))
                                                OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl), szFind, szReplace);
                                            else
                                                OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int iCount = GetDocumentNodeCount(ResourceTree.SelectedNode);
                            string msg = string.Format(Strings.FormMain.ConfirmMultipleOpen, iCount);
                            if (DialogResult.Yes != MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                                return;

                            foreach (TreeNode tnThis in ResourceTree.SelectedNode.Nodes)
                            {
                                // only deal with documents
                                if (tnThis.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
                                {
                                    OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document = (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)tnThis.Tag;
                                    if (m_userControls.ContainsKey(document.ResourceId))
                                    {
                                        EditorInterface edi = (EditorInterface)m_userControls[document.ResourceId];
                                        if (!edi.Close(true))
                                            return;
                                    }

                                    if (!String.IsNullOrEmpty(szFind))
                                        OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl), szFind, szReplace);
                                    else
                                        OpenResource(document.ResourceId, typeof(OSGeo.MapGuide.Maestro.ResourceEditors.XmlEditorControl));
                                }
                            }
                        }
					}
				}
				catch (Exception ex)
				{
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    LastException = ex;
					MessageBox.Show(this, string.Format(Strings.FormMain.XmlEditorError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

        private static int GetDocumentNodeCount(TreeNode treeNode)
        {
            int i = 0;
            foreach (TreeNode tnThis in treeNode.Nodes)
            {
                // Document nodes are tagged with the ResourceListResourceDocument class
                if (tnThis.Tag != null && tnThis.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument))
                {
                    i++;
                }
            }
            return i;
        }

		private void LoadFromXmlMenu_Click(object sender, System.EventArgs e)
		{
			using(new WaitCursor(this))
			{
				try
				{
					if (OpenXmlFileDialog.ShowDialog() != DialogResult.OK)
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

                            string tmpid = "Session:" + m_connection.SessionID + "//" + Guid.NewGuid().ToString() + "." + new MaestroAPI.ResourceIdentifier(document.ResourceId).Extension;

                            using (System.IO.FileStream fs = System.IO.File.Open(OpenXmlFileDialog.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                                m_connection.SetResourceXmlData(tmpid, fs);

                            OpenResource(tmpid, typeof(ResourceEditors.XmlEditorControl));
						}
				}
				catch (Exception ex)
				{
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    LastException = ex;
					MessageBox.Show(this, string.Format(Strings.FormMain.XmlEditorError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ResourceBrowser.BrowseResource dlg = new ResourceBrowser.BrowseResource(this.RepositoryCache, this, true, true, null);
                if (!string.IsNullOrEmpty(m_lastSelectedNode))
                    dlg.SelectedResources = new string[] { m_lastSelectedNode };

                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.SelectedResources != null)
                {
                    if (dlg.SelectedResources.Length > 0)
                        m_lastSelectedNode = dlg.SelectedResources[0];

                    foreach (string s in dlg.SelectedResources)
                        this.OpenResource(s);
               }
			}
			catch(Exception ex)
			{
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                LastException = ex;
				MessageBox.Show(this, string.Format(Strings.FormMain.OpenResourceError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                foreach (OSGeo.MapGuide.Maestro.EditorInterface edi in this.OpenResourceEditors.Values)
                    if (edi.Page.Text.EndsWith(" *"))
                        edi.Save();
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                LastException = ex;
                MessageBox.Show(this, string.Format(Strings.FormMain.SaveResourceError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateResourceTreeStatus();
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
                    ResourceTree.Cache.Connection = lg.Connection;
					m_connection = lg.Connection;
					RebuildDocumentTree();

                    if (m_connection is OSGeo.MapGuide.MaestroAPI.HttpServerConnection)
                    {
                        this.Text = string.Format(Strings.FormMain.WindowTitleTemplate, Application.ProductName, ((MaestroAPI.HttpServerConnection)m_connection).BaseURL);
                        ((MaestroAPI.HttpServerConnection)m_connection).UserAgent = "MapGuide Maestro v" + Application.ProductVersion;
                    }
                    else
                        this.Text = string.Format(Strings.FormMain.WindowTitleTemplate, Application.ProductName, Strings.FormMain.NativeConnectionName);
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
			FormAbout dlg = new FormAbout(m_connection);
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
		public Dictionary<string, EditorInterface> OpenResourceEditors
		{
			get { return m_userControls; }
		}

        public ResourceBrowser.RepositoryCache RepositoryCache { get { return ResourceTree.Cache; } }
		public ResourceEditorMap ResourceEditorMap { get { return m_editors; } }
		public string LastSelectedNode 
        { 
            get { return m_lastSelectedNode; }
            set { m_lastSelectedNode = value; }
        }

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
            try
            {
                if (MaestroAPI.PackageBuilder.PackageProgress.UploadPackage(this, m_connection) == DialogResult.OK)
                    RebuildDocumentTree();
            }
            catch (Exception ex)
            {
                LastException = ex;

                if (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                    ex = ex.InnerException;
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.FormMain.PackageRestoreError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            ResourceProperties dlg = new ResourceProperties(m_editors, m_connection, resid);
            dlg.ShowDialog(this);
            if (!string.IsNullOrEmpty(dlg.OpenResource))
                OpenResource(dlg.OpenResource);
        }

        public Exception LastException
        {
            get { return m_lastException; }
            set { m_lastException = value; }
        }

        private void viewLastExceptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lastException == null)
                MessageBox.Show(this, Strings.FormMain.NoExceptionData, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                MainMenuClose.Enabled =
                MainMenuCloseAll.Enabled =
                MainMenuSave.Enabled =
                MainMenuSaveAs.Enabled =
                MainMenuSaveAll.Enabled =
                    false;

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

                if (edi == null || edi.Page == null || edi.Page.Controls.Count < 1 || edi.Page.Controls[0] as IResourceEditorControl == null)
                {
                    foreach (ToolStripItem b in toolStrip1.Items)
                        b.Enabled = false;
                    TabPageContextMenu.Enabled = false;
                }
                else
                {
                    SaveResourceButton.Enabled =
                    SaveResourceAsButton.Enabled =
                    MainMenuClose.Enabled =
                    MainMenuCloseAll.Enabled =
                    MainMenuSave.Enabled = 
                    MainMenuSaveAs.Enabled =
                    MainMenuSaveAll.Enabled =
                    ClosePageButton.Enabled = true;

                    IResourceEditorControl ei = edi.Page.Controls[0] as IResourceEditorControl;
                    PreviewButton.Enabled = ei.SupportsPreview;
                    ProfileButton.Enabled = ei.SupportsProfiling;
                    ValidateButton.Enabled = ei.SupportsValidate;
                    EditAsXmlButton.Enabled = !(ei is ResourceEditors.XmlEditorControl);
                    TabPageContextMenu.Enabled = true;
				}
            }

			UpdateResourceTreeStatus();
		}

        /// <summary>
        /// Resets all node background colors to transparent
        /// </summary>
        /// <param name="treeView">The tree to reset the colors for</param>
		private void TreeViewResetStatus(TreeView treeView)
		{
            try
            {
                treeView.BeginUpdate();
                Queue<TreeNodeCollection> nodes = new Queue<TreeNodeCollection>();
                nodes.Enqueue(treeView.Nodes);
                while (nodes.Count > 0)
                {
                    TreeNodeCollection n = nodes.Dequeue();
                    foreach (TreeNode tn in n)
                    {
                        if (tn.Tag is MaestroAPI.ResourceListResourceDocument)
                            tn.BackColor = Color.Transparent;

                        if (tn.Nodes.Count > 0)
                            nodes.Enqueue(tn.Nodes);
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
		}

        public void UpdateResourceTreeStatus()
        {
            UpdateResourceTreeStatus(m_userControls.Values);
        }

		private void UpdateResourceTreeStatus(IEnumerable<EditorInterface> editors)
		{
            try
            {
                ResourceTree.BeginUpdate();

                // start looping through open editors
                foreach (EditorInterface ediThis in editors)
                {
                    TreeNode item = FindItem(ediThis.ResourceId, true);
                    if (ediThis.IsClosing)
                    {
                        item.BackColor = Color.Transparent;
                        continue;
                    }

                    if (item != null && item.Tag is MaestroAPI.ResourceListResourceDocument)
                    {
                        // highlight if current
                        if (ediThis.Page != tabItems.SelectedTab)
                            item.BackColor = ediThis.IsModified ? Color.FromArgb(255, 222, 233) : Color.FromArgb(222, 255, 233);
                        else
                        {
                            item.BackColor = ediThis.IsModified ? Color.Pink : Color.FromArgb(182, 255, 193);

                            //See if all parents are expanded
                            TreeNode pa = item.Parent;
                            while (pa != null && pa.IsExpanded)
                                pa = pa.Parent;

                            //We reached the root and all were expanded
                            if (pa == null)
                                ResourceTree.SelectedNode = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
            }
            finally
            {
                ResourceTree.EndUpdate();
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

            IResourceEditorControl ei = edi.Page.Controls[0] as IResourceEditorControl;
            if (ei == null || ei.Resource == null)
                return;

            Profiling dlg = new Profiling(ei.Resource, edi.ResourceId, m_connection);
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

        private void tabItems_MouseMove(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < tabItems.TabPages.Count; i++)
                if (tabItems.GetTabRect(i).Contains(e.Location))
                {
                    if (m_lastTabPageTooltip != tabItems.TabPages[i].ToolTipText)
                    {
                        m_lastTabPageTooltip = tabItems.TabPages[i].ToolTipText;
                        TabPageTooltip.SetToolTip(tabItems, tabItems.TabPages[i].ToolTipText);
                    }

                    return;
                }

            if (m_lastTabPageTooltip != null)
            {
                m_lastTabPageTooltip = null;
                TabPageTooltip.SetToolTip(tabItems, null);
            }
        }

        private void tabItems_MouseLeave(object sender, EventArgs e)
        {
            TabPageTooltip.SetToolTip(tabItems, null);
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            try
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

                IResourceEditorControl ei = edi.Page.Controls[0] as IResourceEditorControl;
                if (ei == null || ei.Resource == null)
                    return;

                if (ei.ValidateResource(true))
                {
                    System.Reflection.PropertyInfo pi = ei.Resource.GetType().GetProperty("CurrentConnection");
                    if (pi != null && pi.CanWrite)
                        pi.SetValue(ei.Resource, this.CurrentConnection, null);

                    try
                    {
                        pi = ei.Resource.GetType().GetProperty("ResourceId");
                        if (pi != null && pi.CanWrite)
                            pi.SetValue(ei.Resource, edi.ResourceId, null);

                        ResourceEditors.WaitForOperation wdlg = new ResourceEditors.WaitForOperation();
                        wdlg.CancelAbortsThread = true;

                        ResourceValidators.ValidationIssue[] issues = (ResourceValidators.ValidationIssue[])wdlg.RunOperationAsync(this, new ResourceEditors.WaitForOperation.DoBackgroundWork(ValidateBackgroundRunner), ei.Resource);
                        if (issues.Length == 0)
                            MessageBox.Show(this, Strings.FormMain.NoValidationProblems, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            ValidationResults dlg = new ValidationResults(ei.ResourceId, issues);
                            dlg.ShowDialog(this);
                        }
                    }
                    finally
                    {
                        pi = ei.Resource.GetType().GetProperty("ResourceId");
                        if (pi != null && pi.CanWrite)
                            pi.SetValue(ei.Resource, edi.TempResourceId, null);
                    }
                }

            }
            catch (CancelException)
            { }
            catch (Exception ex)
            {
                this.LastException = ex;
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.FormMain.ValidationError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private object ValidateBackgroundRunner(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            if (args[0] is string)
            {
                //The user requested a resource to be validated, but it is not open, and may be a folder
                worker.ReportProgress(-1, Strings.FormMain.ValidationBuildingList);

                string folder = (string)args[0];

                List<string> documents = new List<string>();

                if (folder.EndsWith("/"))
                {
                    foreach (object o in CurrentConnection.GetRepositoryResources((string)args[0]).Items)
                        if (o is MaestroAPI.ResourceListResourceDocument)
                            documents.Add((o as MaestroAPI.ResourceListResourceDocument).ResourceId);
                }
                else
                    documents.Add(folder);
                        

                worker.ReportProgress(0);

                List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>> issues = new List<KeyValuePair<string, OSGeo.MapGuide.Maestro.ResourceValidators.ValidationIssue[]>>();
                int i = 0;
                foreach (string s in documents)
                {
                    worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
                    try
                    {
                        //TODO: This will validate resources multiple times, if they are referenced by
                        //resources inside the folder
                        object item = this.CurrentConnection.GetResource(s);
                        issues.Add(new KeyValuePair<string, ResourceValidators.ValidationIssue[]>(s, ResourceValidators.Validation.Validate(item, true)));
                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        issues.Add(new KeyValuePair<string, ResourceValidators.ValidationIssue[]>(s, new ResourceValidators.ValidationIssue[] { new OSGeo.MapGuide.Maestro.ResourceValidators.ValidationIssue(s, OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Error, string.Format(Strings.FormMain.ValidationResourceLoadFailed, msg)) }));
                    }
                    i++;
                    worker.ReportProgress((int)((i / (double)documents.Count) * 100), s);
                }

                return issues;
            }
            else
            {
                worker.ReportProgress(-1, Strings.FormMain.ValidationValidating);
                return ResourceValidators.Validation.Validate(args[0], true);
            }
        }

        private void validateResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ResourceTree.SelectedNode == null)
                return;

            string resid = null;
            if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceDocument).ResourceId;
            else if (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder != null)
                resid = (ResourceTree.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder).ResourceId;
            else if (ResourceTree.SelectedNode.Level == 0)
                resid = "Library://";

            if (resid == null)
                return;

            try
            {
                ResourceEditors.WaitForOperation wdlg = new ResourceEditors.WaitForOperation();
                wdlg.CancelAbortsThread = true;

                List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>> issues = 
                    (List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>>)
                    wdlg.RunOperationAsync(this, new ResourceEditors.WaitForOperation.DoBackgroundWork(ValidateBackgroundRunner), resid);

                int issuecount = 0;
                foreach (KeyValuePair<string, ResourceValidators.ValidationIssue[]> p in issues)
                    issuecount += p.Value.Length;

                if (issuecount == 0)
                    MessageBox.Show(this, Strings.FormMain.NoValidationProblems, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    ValidationResults dlg = new ValidationResults(issues);
                    dlg.ShowDialog(this);
                }
            }
            catch (CancelException)
            { }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                this.LastException = ex;
                MessageBox.Show(this, string.Format(Strings.FormMain.ValidationError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameMenu_Click(object sender, EventArgs e)
        {
            TreeNode node = ResourceTree.SelectedNode;
            node.BeginEdit();
        }

		private void FindReplaceMenu_Click(String szFind, String szReplace)
		{
			TreeNode nodeSel = ResourceTree.SelectedNode;
			if (null == nodeSel)
				return;

			string resid = null;
            if (nodeSel.Tag as MaestroAPI.ResourceListResourceDocument != null)
                resid = (nodeSel.Tag as MaestroAPI.ResourceListResourceDocument).ResourceId;
            else if (nodeSel.Tag as MaestroAPI.ResourceListResourceFolder != null)
                resid = (nodeSel.Tag as MaestroAPI.ResourceListResourceFolder).ResourceId;
            else if (nodeSel.Level == 0)
                resid = "Library://";

			if (null == resid)
				return;
	
			// open for review
			EditAsXmlMenuClick( szFind, szReplace);
		}

        private void ReloadNode(TreeNode n)
        {
            if (n.Tag is MaestroAPI.ResourceListResourceFolder)
                using (new WaitCursor(this))
                    ResourceTree.RebuildNode(n);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceTree_DoubleClick(sender, e);
        }

        private void tabItems_Click(object sender, EventArgs e)
        {
            MouseEventArgs eventArg = e as MouseEventArgs;

            if (eventArg != null && eventArg.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < tabItems.TabCount; i++)
                {
                    if (tabItems.GetTabRect(i).Contains(eventArg.Location))
                    {
                        tabItems.SelectedIndex = i;
                        ClosePage();
                        break;
                    }
                }
            }

        }

        private void ResourceTree_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!ResourceTree.Focused)
                return;

            if (e.KeyChar == '\n' || e.KeyChar == '\r')
            {
                ResourceTree_DoubleClick(sender, e);
                e.Handled = true;
            }
        }

        private void DuplicateMenu_Click(object sender, EventArgs e)
        {
            if (ResourceTree.SelectedNode == null || ResourceTree.SelectedNode.Tag == null)
                return;

            TreeNode source = ResourceTree.SelectedNode;
            if (source.Parent == null)
                return; //Can't do on Library://

            string actualTargetPath;
            if (MoveOrCopyResource(source, source.Parent, false, true, out actualTargetPath))
            {
                if (FindAndSelectNode(actualTargetPath))
                    ResourceTree.SelectedNode.BeginEdit();
            }
        }

        private void MainMenuCloseAll_Click(object sender, EventArgs e)
        {
            var editors = new List<EditorInterface>(m_userControls.Values);
            foreach (EditorInterface edi in editors)
            {
                if (!edi.Close(true))
                    break;
            }
            UpdateResourceTreeStatus(editors);
        }
	}
}
