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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Gdal
{
	/// <summary>
	/// Summary description for Composite.
	/// </summary>
	public class Composite : System.Windows.Forms.UserControl
    {
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button RebuildButton;
        private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;

		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor;
		private EditorInterface m_editor;
        private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
        private ToolStrip toolStrip;
        private ToolStripButton DeleteButton;
        private ToolStripButton RefreshButton;
        private ToolStripSplitButton AddButton;
        private ToolStripMenuItem browseAliasToolStripMenuItem;
        private ToolStripMenuItem browseFilesToolStripMenuItem;
        private ToolStripMenuItem browseFolderToolStripMenuItem;
        private ToolStripMenuItem typeFilelistToolStripMenuItem;
        private ToolStripMenuItem typeFolderToolStripMenuItem;
		private ConfigUpdater m_updater;

		public Composite()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature,  Globalizator.Globalizator globalizor)
		{
			m_editor = editor;
			m_feature = feature;
			m_globalizor = globalizor;
			m_updater = new ConfigUpdater(editor, feature);
			listView.SmallImageList = ShellIcons.ImageList;
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				Hashtable items = m_updater.GetFilelist();
				listView.SelectedItems.Clear();
				listView.Items.Clear();
				foreach(string s in items.Keys)
				{
					ListViewItem lvi = new ListViewItem(s);
					lvi.ImageIndex = ShellIcons.GetShellIcon(s);
					listView.Items.Add(lvi);
				}
			}
			finally
			{
				m_isUpdating = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Composite));
            this.listView = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.RebuildButton = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddButton = new System.Windows.Forms.ToolStripSplitButton();
            this.browseAliasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeFilelistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Location = new System.Drawing.Point(0, 25);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(376, 119);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.RebuildButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 144);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 40);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Note that all paths are as seen by the MapGuide server, not Maestro";
            // 
            // RebuildButton
            // 
            this.RebuildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RebuildButton.Location = new System.Drawing.Point(240, 8);
            this.RebuildButton.Name = "RebuildButton";
            this.RebuildButton.Size = new System.Drawing.Size(136, 32);
            this.RebuildButton.TabIndex = 0;
            this.RebuildButton.Text = "Rebuild all";
            this.RebuildButton.Click += new System.EventHandler(this.RebuildButton_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.DeleteButton,
            this.RefreshButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(376, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // AddButton
            // 
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseAliasToolStripMenuItem,
            this.browseFilesToolStripMenuItem,
            this.browseFolderToolStripMenuItem,
            this.typeFilelistToolStripMenuItem,
            this.typeFolderToolStripMenuItem});
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(32, 22);
            this.AddButton.Text = "toolStripSplitButton1";
            this.AddButton.ToolTipText = "Add raster files";
            this.AddButton.ButtonClick += new System.EventHandler(this.AddButton_Click);
            // 
            // browseAliasToolStripMenuItem
            // 
            this.browseAliasToolStripMenuItem.Name = "browseAliasToolStripMenuItem";
            this.browseAliasToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.browseAliasToolStripMenuItem.Text = "Browse alias...";
            this.browseAliasToolStripMenuItem.Click += new System.EventHandler(this.AddAliasMenu_Click);
            // 
            // browseFilesToolStripMenuItem
            // 
            this.browseFilesToolStripMenuItem.Name = "browseFilesToolStripMenuItem";
            this.browseFilesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.browseFilesToolStripMenuItem.Text = "Browse files...";
            this.browseFilesToolStripMenuItem.Click += new System.EventHandler(this.AddFilesMenu_Click);
            // 
            // browseFolderToolStripMenuItem
            // 
            this.browseFolderToolStripMenuItem.Name = "browseFolderToolStripMenuItem";
            this.browseFolderToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.browseFolderToolStripMenuItem.Text = "Browse folder...";
            this.browseFolderToolStripMenuItem.Click += new System.EventHandler(this.AddFolderMenu_Click);
            // 
            // typeFilelistToolStripMenuItem
            // 
            this.typeFilelistToolStripMenuItem.Name = "typeFilelistToolStripMenuItem";
            this.typeFilelistToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.typeFilelistToolStripMenuItem.Text = "Type filelist...";
            this.typeFilelistToolStripMenuItem.Click += new System.EventHandler(this.AddManualMenu_Click);
            // 
            // typeFolderToolStripMenuItem
            // 
            this.typeFolderToolStripMenuItem.Name = "typeFolderToolStripMenuItem";
            this.typeFolderToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.typeFolderToolStripMenuItem.Text = "Type folder...";
            this.typeFolderToolStripMenuItem.Click += new System.EventHandler(this.AddFolderManual_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteButton.Text = "toolStripButton2";
            this.DeleteButton.ToolTipText = "Remove the selected files";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Enabled = false;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(23, 22);
            this.RefreshButton.Text = "toolStripButton3";
            this.RefreshButton.ToolTipText = "Refresh the mapping data for the selected items";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // Composite
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(376, 144);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.panel1);
            this.Name = "Composite";
            this.Size = new System.Drawing.Size(376, 184);
            this.panel1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void AddFilesMenu_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = string.Join("|", new string[] {
				"PNG files (*.png)", "*.png",
				"JPEG files (*.jpg; *.jpeg)", "*.jpg;*.jpeg",
				"Tagged Image format (*.tif, *.tiff)", "*.tif;*.tiff",
				"Enhanced Wawelet files (*.ecw)", "*.ecw",
				"MrSID files (*.sid)", "*.sid",
				"GIF files (*.gif)", "*.gif",
				"All files (*.*)", "*.*"
			});

			dlg.Title = "Select files to add";
			dlg.Multiselect = true;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_updater.UpdateItems(dlg.FileNames, new string[]{});
				UpdateDisplay();
			}

		}

		private void AddFolderMenu_Click(object sender, System.EventArgs e)
		{
			ArrayList files = new ArrayList();
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.Description = "Select the folder with files to include";
			if (dlg.ShowDialog(this) != DialogResult.OK)
				return;

			//I usually use the recursive model, but this time I'll try a queue based
			Queue folders = new Queue();
			folders.Enqueue(dlg.SelectedPath);

			Hashtable ext = new Hashtable();
			ext.Add(".png", "");
			ext.Add(".jpg", "");
			ext.Add(".jpeg", "");
			ext.Add(".tif", "");
			ext.Add(".tiff", "");
			ext.Add(".ecw", "");
			ext.Add(".sid", "");
			ext.Add(".gif", "");

			while(folders.Count > 0)
			{
				string path = (string)folders.Dequeue();
				foreach(string s in System.IO.Directory.GetFiles(path))
					if (ext.ContainsKey(System.IO.Path.GetExtension(s).ToLower()))
						files.Add(s);
				foreach(string s in System.IO.Directory.GetDirectories(path))
					folders.Enqueue(s);
			}

			m_updater.UpdateItems((string[])files.ToArray(typeof(string)), new string[]{});
			UpdateDisplay();
		}

		private void AddAliasMenu_Click(object sender, System.EventArgs e)
		{
			ArrayList files = new ArrayList();
			NameValueCollection nv = new NameValueCollection();
			nv.Add("", "All files");
			string s = m_editor.BrowseUnmanagedData("", nv);

			if (s != null)
			{
				Hashtable ext = new Hashtable();
				ext.Add(".png", "");
				ext.Add(".jpg", "");
				ext.Add(".jpeg", "");
				ext.Add(".tif", "");
				ext.Add(".tiff", "");
				ext.Add(".ecw", "");
				ext.Add(".sid", "");
				ext.Add(".gif", "");
				
				s = s.Replace("%MG_DATA_PATH_ALIAS[", "[").Replace("]%", "]");
                if (s.IndexOf("/") > 0 && !s.EndsWith("/"))
                    s = s.Substring(0, s.LastIndexOf("/"));

				OSGeo.MapGuide.MaestroAPI.UnmanagedDataList lst = m_editor.CurrentConnection.EnumerateUnmanagedData(s, null, true, OSGeo.MapGuide.MaestroAPI.UnmanagedDataTypes.Files);
				for(int i = 0; i < lst.Items.Count; i++)
				{
					string path = ((OSGeo.MapGuide.MaestroAPI.UnmanagedDataListUnmanagedDataFile)lst.Items[i]).UnmanagedDataId;
					if (ext.ContainsKey(System.IO.Path.GetExtension(path).ToLower()))
						files.Add(path.Replace("[", "%MG_DATA_PATH_ALIAS[").Replace("]", "]%"));
				}

				m_updater.UpdateItems((string[])files.ToArray(typeof(string)), new string[]{});
				UpdateDisplay();
			}
		}

		private void AddManualMenu_Click(object sender, System.EventArgs e)
		{
			ArrayList files = new ArrayList();
			AddFilenames dlg = new AddFilenames();
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				using(System.IO.StringReader sr = new System.IO.StringReader(dlg.FileList.Text))
					while(sr.Peek() >= 0)
					{
						string s = sr.ReadLine();
						if (s.Trim().Length > 0)
							files.Add(s.Trim());
					}

				if (files.Count == 0)
					return;

				m_updater.UpdateItems((string[])files.ToArray(typeof(string)), new string[]{});
				UpdateDisplay();
			}
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DeleteButton.Enabled = RefreshButton.Enabled = listView.SelectedItems.Count > 0;
		}

		private void RebuildButton_Click(object sender, System.EventArgs e)
		{
			string[] files = new string[listView.Items.Count];
			for(int i = 0; i < files.Length; i++)
				files[i] = listView.Items[i].Text;
		
			string oldconfig = m_feature.ConfigurationDocument;
			m_feature.ConfigurationDocument = null;
			m_updater.UpdateItems(files, new string[]{});

			//This happens after a cancel
			if (m_feature.ConfigurationDocument == null)
				m_feature.ConfigurationDocument = oldconfig;
			UpdateDisplay();
		}

		private void AddFolderManual_Click(object sender, System.EventArgs e)
		{
			ArrayList files = new ArrayList();
			AddFolder dlg = new AddFolder();
			if (dlg.ShowDialog(this) != DialogResult.OK)
				return;

			//I usually use the recursive model, but this time I'll try a queue based
			Queue folders = new Queue();
			folders.Enqueue(dlg.FileList.Text);

			Hashtable ext = new Hashtable();
			ext.Add(".png", "");
			ext.Add(".jpg", "");
			ext.Add(".jpeg", "");
			ext.Add(".tif", "");
			ext.Add(".tiff", "");
			ext.Add(".ecw", "");
			ext.Add(".sid", "");
			ext.Add(".gif", "");

            try
            {
                while (folders.Count > 0)
                {
                    string path = (string)folders.Dequeue();
                    foreach (string s in System.IO.Directory.GetFiles(path))
                        if (ext.ContainsKey(System.IO.Path.GetExtension(s).ToLower()))
                            files.Add(s);
                    foreach (string s in System.IO.Directory.GetDirectories(path))
                        folders.Enqueue(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to load files: {0}"), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			m_updater.UpdateItems((string[])files.ToArray(typeof(string)), new string[]{});
			UpdateDisplay();
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string[] files = new string[listView.SelectedItems.Count];
            for (int i = 0; i < files.Length; i++)
                files[i] = listView.SelectedItems[i].Text;

            m_updater.UpdateItems(new string[] { }, files);
            UpdateDisplay();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            string[] files = new string[listView.SelectedItems.Count];
            for (int i = 0; i < files.Length; i++)
                files[i] = listView.SelectedItems[i].Text;

            m_updater.UpdateItems(files, files);
            UpdateDisplay();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddButton.ShowDropDown();
        }

	}
}
