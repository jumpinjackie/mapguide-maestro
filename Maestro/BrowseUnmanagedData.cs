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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for BrowseUnmanagedData.
	/// </summary>
	public class BrowseUnmanagedData : System.Windows.Forms.Form
	{
		private new System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.ListView ItemView;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TreeView FolderView;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ComboBox ResourceType;
		private System.Windows.Forms.TextBox ResourceName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button RefreshButton;
		private System.ComponentModel.IContainer components;
		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
		private System.Windows.Forms.ImageList imageList;
		private Hashtable m_lists = null;
		private ArrayList m_extensions = null;
		private const string FOLDER_IMAGE = "_folder_._folder_";
		private const string FOLDER_ALIAS = "_folderalias_._folderalias_";
		private string m_startPath = null;

		protected BrowseUnmanagedData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			FolderView.ImageList = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.ImageList;
			ItemView.SmallImageList = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.ImageList;

			OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.AddIcon(FOLDER_IMAGE, imageList.Images[0]);
			OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.AddIcon(FOLDER_ALIAS, imageList.Images[1]);
            this.Icon = FormMain.MaestroIcon;
        }

		public void SetFileTypes(NameValueCollection filetypes)
		{
			m_extensions = new ArrayList();
			ResourceType.Items.Clear();
			foreach(string key in filetypes)
			{
				ResourceType.Items.Add(filetypes[key]);
				m_extensions.Add(key.ToLower().Split(';'));
			}

			if (ResourceType.Items.Count == 0)
				ResourceType.Enabled = label2.Enabled = false;
			else
				ResourceType.SelectedIndex = 0;
		}

		public BrowseUnmanagedData(OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection)
			: this()
		{
			m_connection = connection;
			RefreshButton_Click(null, null);
		}

		public string SelectedText 
		{ 
			get 
			{
				string path = ResourceName.Text;
				if (path.IndexOf("]") > 0)
				{
					string leftpart = path.Substring(0, path.IndexOf("]"));
					string rightpart = path.Substring(path.IndexOf("]") + 1);
					return "%MG_DATA_PATH_ALIAS" + leftpart + "]%" + rightpart;
				}
				else
					return ResourceName.Text; 
			}
			set
			{
				m_startPath = value;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseUnmanagedData));
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ItemView = new System.Windows.Forms.ListView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.FolderView = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ResourceType = new System.Windows.Forms.ComboBox();
            this.ResourceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            resources.ApplyResources(this.CancelButton, "CancelButton");
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Name = "CancelButton";
            // 
            // OKButton
            // 
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.Name = "OKButton";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Name = "panel1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel4);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ItemView);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // ItemView
            // 
            resources.ApplyResources(this.ItemView, "ItemView");
            this.ItemView.Name = "ItemView";
            this.ItemView.UseCompatibleStateImageBehavior = false;
            this.ItemView.View = System.Windows.Forms.View.List;
            this.ItemView.SelectedIndexChanged += new System.EventHandler(this.ItemView_SelectedIndexChanged);
            this.ItemView.DoubleClick += new System.EventHandler(this.ItemView_DoubleClick);
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.RefreshButton);
            this.panel4.Controls.Add(this.FolderView);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // RefreshButton
            // 
            resources.ApplyResources(this.RefreshButton, "RefreshButton");
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FolderView
            // 
            resources.ApplyResources(this.FolderView, "FolderView");
            this.FolderView.HideSelection = false;
            this.FolderView.Name = "FolderView";
            this.FolderView.PathSeparator = "/";
            this.FolderView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.FolderView_BeforeExpand);
            this.FolderView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FolderView_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ResourceType);
            this.panel2.Controls.Add(this.ResourceName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // ResourceType
            // 
            resources.ApplyResources(this.ResourceType, "ResourceType");
            this.ResourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResourceType.Name = "ResourceType";
            this.ResourceType.SelectedIndexChanged += new System.EventHandler(this.ResourceType_SelectedIndexChanged);
            // 
            // ResourceName
            // 
            resources.ApplyResources(this.ResourceName, "ResourceName");
            this.ResourceName.Name = "ResourceName";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            // 
            // BrowseUnmanagedData
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.panel1);
            this.Name = "BrowseUnmanagedData";
            this.Load += new System.EventHandler(this.BrowseUnmanagedData_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void RefreshButton_Click(object sender, System.EventArgs e)
		{
			m_lists = new Hashtable();
			OSGeo.MapGuide.MaestroAPI.UnmanagedDataList list = m_connection.EnumerateUnmanagedData(null, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Folders);
			FolderView.Nodes.Clear();
			foreach(object o in list.Items)
				if (o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder != null)
				{
					TreeNode n = CreateFolderNode("", o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder);
					n.ImageIndex = n.SelectedImageIndex = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.GetShellIcon(FOLDER_ALIAS);
					FolderView.Nodes.Add(n);
				}

			if (FolderView.Nodes.Count == 0)
			{
				MessageBox.Show(this, Strings.BrowseUnmanagedData.NoAliasFoldersError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				OKButton.Enabled = false;
			}
		}

		private void FolderView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (e == null || e.Node == null || e.Node.Tag == null)
				return;
			
			string path = (string)e.Node.Tag;
			if (!m_lists.ContainsKey(path))
				m_lists[path] = m_connection.EnumerateUnmanagedData(path, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Both);

			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
			{
				e.Node.Nodes.Clear();
				foreach(object o in ((OSGeo.MapGuide.MaestroAPI.UnmanagedDataList)m_lists[path]).Items)
					if (o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder != null)
						e.Node.Nodes.Add(CreateFolderNode(e.Node.FullPath, o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder));
			}
		}

		private TreeNode CreateFolderNode(string parentpath, OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder folder)
		{
			string path = folder.UnmanagedDataId.Substring(parentpath.Length);
			if (path.EndsWith("/"))
				path = path.Substring(0, path.Length - 1);
			TreeNode n = new TreeNode(path);
			n.ImageIndex = n.SelectedImageIndex = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.GetShellIcon(FOLDER_IMAGE);
			n.Nodes.Add("Dummy");
			n.Tag = folder.UnmanagedDataId;
			return n;
		}

		private void FolderView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e == null || e.Node == null || e.Node.Tag == null)
				return;
		
			string path = (string)e.Node.Tag;
			if (!m_lists.ContainsKey(path))
				m_lists[path] = m_connection.EnumerateUnmanagedData(path, null, false, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Both);

			try
			{
				ItemView.BeginUpdate();
				ItemView.Items.Clear();
				string parentpath = e.Node.FullPath;
				foreach(object o in ((OSGeo.MapGuide.MaestroAPI.UnmanagedDataList)m_lists[path]).Items)
					if (o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder != null)
					{
						OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder folder = (OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFolder)o;
						string	itempath = folder.UnmanagedDataId.Substring(parentpath.Length);
						if (itempath.EndsWith("/"))
							itempath = itempath.Substring(0, itempath.Length - 1);
						ListViewItem lvi = new ListViewItem(itempath);
						lvi.Tag = folder.UnmanagedDataId;
						lvi.ImageIndex = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.GetShellIcon(FOLDER_IMAGE);
						ItemView.Items.Add(lvi);
					}

				foreach(object o in ((OSGeo.MapGuide.MaestroAPI.UnmanagedDataList)m_lists[path]).Items)
					if (o as OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFile != null)
					{
						OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFile file = (OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFile)o;
						if (ResourceType.SelectedIndex >= 0 && m_extensions != null && ResourceType.SelectedIndex < m_extensions.Count )
						{
							string[] allowed = (string[])m_extensions[ResourceType.SelectedIndex];
							if (allowed.Length != 1 || allowed[0] != "")
							{
								string ext = System.IO.Path.GetExtension(file.UnmanagedDataId).ToLower();
								if (Array.IndexOf(allowed, ext) < 0)
									continue;
							}
						}

						ListViewItem lvi = new ListViewItem(file.UnmanagedDataId.Substring(parentpath.Length));
						lvi.Tag = file.UnmanagedDataId;
						lvi.ImageIndex = OSGeo.MapGuide.Maestro.ResourceEditors.ShellIcons.GetShellIcon(file.UnmanagedDataId);
						ItemView.Items.Add(lvi);
					}
			} 
			finally
			{
				ItemView.EndUpdate();
			}

			ResourceName.Text = (string)e.Node.Tag;
		}

		private void ItemView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (ItemView.SelectedItems.Count == 1)
				ResourceName.Text = (string)ItemView.SelectedItems[0].Tag;
		}

		private void ItemView_DoubleClick(object sender, System.EventArgs e)
		{
			if (ItemView.SelectedItems.Count == 1)
			{
				if (((string)ItemView.SelectedItems[0].Tag).EndsWith("/"))
				{
					if (FolderView.SelectedNode != null)
					{
						FolderView.SelectedNode.Expand();
						foreach(TreeNode n in FolderView.SelectedNode.Nodes)
							if (n.Text == ItemView.SelectedItems[0].Text)
							{
								FolderView.SelectedNode = n;
								break;
							}
					}
				}
				else
				{
					try { OKButton.PerformClick(); }
					catch { }
				}
			}
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			if (ResourceName.Text.Trim().Length == 0)
			{
				MessageBox.Show(this, Strings.BrowseUnmanagedData.NoResourceSelectedError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void ResourceType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			FolderView_AfterSelect(FolderView, new TreeViewEventArgs(FolderView.SelectedNode));
		}

		private void BrowseUnmanagedData_Load(object sender, System.EventArgs e)
		{
			if (FolderView.Nodes.Count > 0)
				FolderView.SelectedNode = FolderView.Nodes[0];

			if (m_startPath != null)
			{
				string[] items = m_startPath.Replace("%MG_DATA_PATH_ALIAS[", "[").Replace("]%", "]/").Split('/');
				TreeNodeCollection basis = FolderView.Nodes;
				TreeNode best = null;
				foreach(string s in items)
				{
					bool found = false;
					foreach(TreeNode n in basis)
						if (n.Text == s)
						{
							n.Expand();
							best = n;
							basis = n.Nodes;
							found = true;
							break;
						}
					if (!found)
						break;
				}

				if (best != null)
					FolderView.SelectedNode = best;
			}

		}
	}
}
