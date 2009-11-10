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

namespace OSGeo.MapGuide.Maestro.ResourceBrowser
{
	/// <summary>
	/// Summary description for BrowseResource.
	/// </summary>
	public class BrowseResource : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button OKButton;
		private new System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel5;
		private ResourceTree FolderView;
		private System.Windows.Forms.ListView ItemView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ResourceName;
		private System.Windows.Forms.ComboBox ResourceType;
		private System.Windows.Forms.Button RefreshButton;
		private Globalizator.Globalizator m_globalizor = null;

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;

		private bool m_openMode;
		private string[] m_selectedResources;
		private string[] m_validTypes;
		private Form m_ownerform;
        private ResourceEditors.ResourceEditorMap m_editorMap;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BrowseResource(RepositoryCache cache, Form ownerform, bool openMode, bool allowMulti, string[] avalibleTypes)
			: this()
		{
			m_connection = cache.Connection;
			m_openMode = openMode;
			m_ownerform = ownerform;
            m_editorMap = cache.EditorMap;

            FolderView.Cache = cache;

			m_validTypes = avalibleTypes == null ? cache.EditorMap.AvalibleResourceTypes : avalibleTypes;
			ResourceType.Items.Clear();

            if (avalibleTypes == null)
                ResourceType.Items.Add(m_globalizor.Translate("All resource types"));

			foreach(string i in m_validTypes)
				ResourceType.Items.Add(cache.EditorMap.GetResourceDisplayNameFromResourceType(i));

            if (avalibleTypes == null)
            {
                string[] tmp = new string[m_validTypes.Length + 1];
                Array.Copy(m_validTypes, 0, tmp, 1, m_validTypes.Length);
                tmp[0] = "";
                m_validTypes = tmp;
            }

			ResourceType.Enabled = ResourceType.Items.Count > 1;
			ItemView.SmallImageList = cache.EditorMap.SmallImageList;
            ItemView.MultiSelect = allowMulti;

			if (!m_openMode)
				this.Text = m_globalizor.Translate("Save resource");

            this.Icon = m_ownerform.Icon;
		}

		protected BrowseResource()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ItemView = new System.Windows.Forms.ListView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ResourceType = new System.Windows.Forms.ComboBox();
            this.ResourceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.FolderView = new OSGeo.MapGuide.Maestro.ResourceBrowser.ResourceTree();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 463);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(640, 391);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ItemView);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(240, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(400, 391);
            this.panel5.TabIndex = 2;
            // 
            // ItemView
            // 
            this.ItemView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemView.Location = new System.Drawing.Point(0, 8);
            this.ItemView.Name = "ItemView";
            this.ItemView.Size = new System.Drawing.Size(392, 375);
            this.ItemView.TabIndex = 0;
            this.ItemView.UseCompatibleStateImageBehavior = false;
            this.ItemView.View = System.Windows.Forms.View.List;
            this.ItemView.SelectedIndexChanged += new System.EventHandler(this.ItemView_SelectedIndexChanged);
            this.ItemView.DoubleClick += new System.EventHandler(this.ItemView_DoubleClick);
            this.ItemView.Click += new System.EventHandler(this.ItemView_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(232, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 391);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.RefreshButton);
            this.panel4.Controls.Add(this.FolderView);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(232, 391);
            this.panel4.TabIndex = 0;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.RefreshButton.Location = new System.Drawing.Point(8, 359);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(224, 24);
            this.RefreshButton.TabIndex = 1;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ResourceType);
            this.panel2.Controls.Add(this.ResourceName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 391);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 72);
            this.panel2.TabIndex = 0;
            // 
            // ResourceType
            // 
            this.ResourceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResourceType.Location = new System.Drawing.Point(112, 40);
            this.ResourceType.Name = "ResourceType";
            this.ResourceType.Size = new System.Drawing.Size(520, 21);
            this.ResourceType.TabIndex = 3;
            this.ResourceType.SelectedIndexChanged += new System.EventHandler(this.ResourceType_SelectedIndexChanged);
            // 
            // ResourceName
            // 
            this.ResourceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceName.Location = new System.Drawing.Point(112, 8);
            this.ResourceName.Name = "ResourceName";
            this.ResourceName.Size = new System.Drawing.Size(520, 20);
            this.ResourceName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Resource type";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resource name";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OKButton.Location = new System.Drawing.Point(216, 471);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(80, 24);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CancelButton.Location = new System.Drawing.Point(320, 471);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(80, 24);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FolderView
            // 
            this.FolderView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderView.Cache = null;
            this.FolderView.HideDocuments = true;
            this.FolderView.HideSelection = false;
            this.FolderView.Location = new System.Drawing.Point(8, 8);
            this.FolderView.Name = "FolderView";
            this.FolderView.Size = new System.Drawing.Size(224, 351);
            this.FolderView.Sorted = true;
            this.FolderView.TabIndex = 0;
            this.FolderView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FolderView_AfterSelect);
            // 
            // BrowseResource
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 509);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.panel1);
            this.Name = "BrowseResource";
            this.Text = "Open resource";
            this.Load += new System.EventHandler(this.BrowseResource_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void BrowseResource_Load(object sender, System.EventArgs e)
		{
			LoadTreeList();
			ResourceType.SelectedIndex = 0;

			if (m_selectedResources != null && m_selectedResources.Length == 1 && m_selectedResources[0] != null)
			{
				TreeNodeCollection basis = FolderView.Nodes[0].Nodes;
				TreeNode best = null;
				string path = m_selectedResources[0];
				if (path.StartsWith("Library://"))
					path = path.Substring("Library://".Length);
				string[] items = path.Split('/');
				foreach(string s in items)
				{
					bool found = false;
					foreach(TreeNode n in basis)
						if (n.Text == s)
						{
							best = n;
							n.Expand();
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
			m_selectedResources = null;

		}

		private void LoadTreeList()
		{
			string selectedNode = "";
			if (FolderView.SelectedNode != null)
				selectedNode = FolderView.SelectedNode.FullPath;

            FolderView.RefreshTreeNodes();
            FolderView.SelectClosestParent(selectedNode);
		}

		private void UpdateDocumentList()
		{
			try
			{
				ItemView.BeginUpdate();
				ItemView.Items.Clear();
                foreach (OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument document in FolderView.GetDocuments())
				{
                    if (string.IsNullOrEmpty(m_validTypes[ResourceType.SelectedIndex]) || m_editorMap.GetResourceTypeNameFromResourceID(document.ResourceId) == m_validTypes[ResourceType.SelectedIndex])
                    {
                        ListViewItem l = new ListViewItem();
                        l.Text = m_editorMap.GetResourceNameFromResourceID(document.ResourceId);
                        l.Tag = document;
                        l.ImageIndex = m_editorMap.GetImageIndexFromResourceID(document.ResourceId);
                        ItemView.Items.Add(l);
                    }
				}
			}
			finally
			{
				ItemView.EndUpdate();
			}
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			string fullpath = ResourceName.Text;

            if (ItemView.SelectedItems.Count > 1)
                fullpath = ((MaestroAPI.ResourceListResourceDocument)ItemView.SelectedItems[0].Tag).ResourceId;

            int imageindex = m_editorMap.GetImageIndexFromResourceID(fullpath);
            string itemType = m_editorMap.GetResourceTypeNameFromResourceID(fullpath);

            if (imageindex == m_editorMap.BlankIcon || imageindex == m_editorMap.FolderIcon)
			{
				itemType = m_validTypes[ResourceType.SelectedIndex];
                if (itemType.ToLower() != "folder")
				    fullpath += "." + itemType;
			}

			bool valid = false;
			foreach(string icon in m_validTypes)
				if (itemType == icon)
				{
					valid = true;
					break;
				}

			if (!valid)
			{
				MessageBox.Show(this, m_globalizor.Translate("The resource entered does not have a valid type"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}


			if (!fullpath.ToLower().StartsWith("library://"))
			{
				TreeNode node = FolderView.SelectedNode;
				string startPath = "Library://";
				if (node != null && node.Parent != null)
					startPath += node.FullPath.Substring(m_connection.DisplayName.Length + FolderView.PathSeparator.Length).Replace(FolderView.PathSeparator, "/");

				if (!startPath.EndsWith("/"))
					startPath += "/";

				if (fullpath.StartsWith("/"))
					fullpath = "Library:/" + fullpath;
				else
					fullpath = startPath + fullpath;
			}

            
            if (FolderView.Cache.ResourceExists(fullpath))
			{
				if (!m_openMode)
					if (MessageBox.Show(this, m_globalizor.Translate("Overwrite existing resource?"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
						return;

                if (ItemView.MultiSelect && ItemView.SelectedItems.Count > 1)
                {
                    List<string> items = new List<string>();
                    foreach (ListViewItem lvi in ItemView.SelectedItems)
                        items.Add(((MaestroAPI.ResourceListResourceDocument)lvi.Tag).ResourceId);

                    m_selectedResources = items.ToArray();
                }
                else
				    m_selectedResources = new string[] { fullpath };

				this.DialogResult = DialogResult.OK;
				this.Close();
				return;
			}

            if (itemType.ToLower().Equals("folder"))
            {
                try
                {
                    MaestroAPI.ResourceIdentifier.Validate(fullpath, OSGeo.MapGuide.MaestroAPI.ResourceTypes.Folder);
                    m_selectedResources = new string[] { fullpath };
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                catch
                {
                }
            }

			if (m_openMode)
				MessageBox.Show(this, m_globalizor.Translate("The resource entered does not exist."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
			{
				//Make sure the folder exists
				if (FolderView.Cache.FolderExists(new MaestroAPI.ResourceIdentifier(fullpath).ParentFolder))
				{
					m_selectedResources = new string[] { fullpath };
					this.DialogResult = DialogResult.OK;
					this.Close();
					return;
				}
			
				MessageBox.Show(this, m_globalizor.Translate("The resource cannot be saved because the folder entered does not exist"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void FolderView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
            if (FolderView.SelectedNode != null && m_validTypes != null && m_validTypes.Length == 1 && m_validTypes[0].ToLower().Equals("folder"))
            {
                if (FolderView.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder == null)
                    ResourceName.Text = "Library://";
                else
                    ResourceName.Text = (FolderView.SelectedNode.Tag as MaestroAPI.ResourceListResourceFolder).ResourceId;
            }

			UpdateDocumentList();
		}

		private void CancelButton_Click(object sender, System.EventArgs e)
		{
			m_selectedResources = null;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void ItemView_Click(object sender, System.EventArgs e)
		{
            if (ItemView.SelectedItems.Count == 1)
                ResourceName.Text = ((OSGeo.MapGuide.MaestroAPI.ResourceListResourceDocument)ItemView.SelectedItems[0].Tag).ResourceId;
            else if (ItemView.SelectedItems.Count > 1)
                ResourceName.Text = "";
		}

		private void ItemView_DoubleClick(object sender, System.EventArgs e)
		{
			OKButton.PerformClick();
		}

		private void ResourceType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateDocumentList();
		}

		public string[] SelectedResources
		{
			get { return m_selectedResources; }
			set { m_selectedResources = value; }
		}

        private void ItemView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            FolderView.Cache.Reset();
            FolderView.RefreshTreeNodes();
        }
	}
}
