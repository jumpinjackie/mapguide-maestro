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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

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
		private EditorInterface m_editor;
        private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
        private ToolStrip toolStrip;
        private ToolStripButton DeleteButton;
        private ToolStripButton RefreshButton;
        private ToolStripDropDownButton AddButton;
        private ToolStripMenuItem browseAliasToolStripMenuItem;
        private ToolStripMenuItem browseFilesToolStripMenuItem;
        private ToolStripMenuItem browseFolderToolStripMenuItem;
        private ToolStripMenuItem typeFilelistToolStripMenuItem;
        private ToolStripMenuItem typeFolderToolStripMenuItem;
        private OpenFileDialog OpenFileDialog;
        private FolderBrowserDialog FolderBrowserDialog;
		private ConfigUpdater m_updater;

		public Composite()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
		{
			m_editor = editor;
			m_feature = feature;
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
            this.AddButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.browseAliasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeFilelistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            resources.ApplyResources(this.listView, "listView");
            this.listView.Name = "listView";
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.RebuildButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // RebuildButton
            // 
            resources.ApplyResources(this.RebuildButton, "RebuildButton");
            this.RebuildButton.Name = "RebuildButton";
            this.RebuildButton.Click += new System.EventHandler(this.RebuildButton_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.DeleteButton,
            this.RefreshButton});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
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
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.Name = "AddButton";
            // 
            // browseAliasToolStripMenuItem
            // 
            this.browseAliasToolStripMenuItem.Name = "browseAliasToolStripMenuItem";
            resources.ApplyResources(this.browseAliasToolStripMenuItem, "browseAliasToolStripMenuItem");
            this.browseAliasToolStripMenuItem.Click += new System.EventHandler(this.AddAliasMenu_Click);
            // 
            // browseFilesToolStripMenuItem
            // 
            this.browseFilesToolStripMenuItem.Name = "browseFilesToolStripMenuItem";
            resources.ApplyResources(this.browseFilesToolStripMenuItem, "browseFilesToolStripMenuItem");
            this.browseFilesToolStripMenuItem.Click += new System.EventHandler(this.AddFilesMenu_Click);
            // 
            // browseFolderToolStripMenuItem
            // 
            this.browseFolderToolStripMenuItem.Name = "browseFolderToolStripMenuItem";
            resources.ApplyResources(this.browseFolderToolStripMenuItem, "browseFolderToolStripMenuItem");
            this.browseFolderToolStripMenuItem.Click += new System.EventHandler(this.AddFolderMenu_Click);
            // 
            // typeFilelistToolStripMenuItem
            // 
            this.typeFilelistToolStripMenuItem.Name = "typeFilelistToolStripMenuItem";
            resources.ApplyResources(this.typeFilelistToolStripMenuItem, "typeFilelistToolStripMenuItem");
            this.typeFilelistToolStripMenuItem.Click += new System.EventHandler(this.AddManualMenu_Click);
            // 
            // typeFolderToolStripMenuItem
            // 
            this.typeFolderToolStripMenuItem.Name = "typeFolderToolStripMenuItem";
            resources.ApplyResources(this.typeFolderToolStripMenuItem, "typeFolderToolStripMenuItem");
            this.typeFolderToolStripMenuItem.Click += new System.EventHandler(this.AddFolderManual_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DeleteButton, "DeleteButton");
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RefreshButton, "RefreshButton");
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // OpenFileDialog
            // 
            resources.ApplyResources(this.OpenFileDialog, "OpenFileDialog");
            this.OpenFileDialog.Multiselect = true;
            // 
            // FolderBrowserDialog
            // 
            resources.ApplyResources(this.FolderBrowserDialog, "FolderBrowserDialog");
            // 
            // Composite
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.listView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.panel1);
            this.Name = "Composite";
            this.panel1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void AddFilesMenu_Click(object sender, System.EventArgs e)
		{
			if (OpenFileDialog.ShowDialog(this) == DialogResult.OK)
			{
                m_updater.UpdateItems(OpenFileDialog.FileNames, new string[] { });
				UpdateDisplay();
			}
		}

		private void AddFolderMenu_Click(object sender, System.EventArgs e)
		{
			ArrayList files = new ArrayList();
            if (FolderBrowserDialog.ShowDialog(this) != DialogResult.OK)
				return;

			//I usually use the recursive model, but this time I'll try a queue based
			Queue folders = new Queue();
            folders.Enqueue(FolderBrowserDialog.SelectedPath);

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
			nv.Add("", OSGeo.MapGuide.Maestro.ResourceEditors.Strings.Common.AllFiles);
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
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.Composite.FileLoadError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
	}
}
