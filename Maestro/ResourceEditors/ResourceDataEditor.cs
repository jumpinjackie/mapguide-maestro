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
using System.Collections.Specialized;
using System.Collections.Generic;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for ResourceDataEditor.
	/// </summary>
	public class ResourceDataEditor : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.ImageList toolbarImages;
        private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView ResourceDataFiles;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem ChangeResourceTypeMenu;
		private System.ComponentModel.IContainer components;
		private bool m_resourceExists;
        private ToolStrip ResourceDataFilesToolbar;
        private ToolStripButton AddFileButton;
        private ToolStripButton DeleteFileButton;
        private ToolStripButton DownloadFileButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToggleDocumentsButton;
        private MenuItem EditResourceXmlMenu;

		public ResourceDataEditor(EditorInterface editor, string resourceid)
			: this()
		{
            m_editor = editor;
			m_resource = resourceid;
			this.Enabled = true;
		}

		public ResourceDataEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			foreach(string s in Enum.GetNames(typeof(OSGeo.MapGuide.MaestroAPI.ResourceDataType)))
				ChangeResourceTypeMenu.MenuItems.Add(s, new EventHandler(ChangeType_Clicked));
			this.Enabled = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceDataEditor));
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.ResourceDataFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.ChangeResourceTypeMenu = new System.Windows.Forms.MenuItem();
            this.EditResourceXmlMenu = new System.Windows.Forms.MenuItem();
            this.ResourceDataFilesToolbar = new System.Windows.Forms.ToolStrip();
            this.AddFileButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteFileButton = new System.Windows.Forms.ToolStripButton();
            this.DownloadFileButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleDocumentsButton = new System.Windows.Forms.ToolStripButton();
            this.ResourceDataFilesToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarImages
            // 
            this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
            this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImages.Images.SetKeyName(0, "");
            this.toolbarImages.Images.SetKeyName(1, "");
            this.toolbarImages.Images.SetKeyName(2, "");
            this.toolbarImages.Images.SetKeyName(3, "");
            // 
            // ResourceDataFiles
            // 
            this.ResourceDataFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.ResourceDataFiles.ContextMenu = this.contextMenu;
            resources.ApplyResources(this.ResourceDataFiles, "ResourceDataFiles");
            this.ResourceDataFiles.Name = "ResourceDataFiles";
            this.ResourceDataFiles.UseCompatibleStateImageBehavior = false;
            this.ResourceDataFiles.View = System.Windows.Forms.View.Details;
            this.ResourceDataFiles.SelectedIndexChanged += new System.EventHandler(this.ResourceDataFiles_SelectedIndexChanged);
            this.ResourceDataFiles.SizeChanged += new System.EventHandler(this.ResourceDataFiles_SizeChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ChangeResourceTypeMenu,
            this.EditResourceXmlMenu});
            this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
            // 
            // ChangeResourceTypeMenu
            // 
            this.ChangeResourceTypeMenu.Index = 0;
            resources.ApplyResources(this.ChangeResourceTypeMenu, "ChangeResourceTypeMenu");
            // 
            // EditResourceXmlMenu
            // 
            this.EditResourceXmlMenu.Index = 1;
            resources.ApplyResources(this.EditResourceXmlMenu, "EditResourceXmlMenu");
            this.EditResourceXmlMenu.Click += new System.EventHandler(this.EditResourceXmlMenu_Click);
            // 
            // ResourceDataFilesToolbar
            // 
            this.ResourceDataFilesToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ResourceDataFilesToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddFileButton,
            this.DeleteFileButton,
            this.DownloadFileButton,
            this.toolStripSeparator1,
            this.ToggleDocumentsButton});
            resources.ApplyResources(this.ResourceDataFilesToolbar, "ResourceDataFilesToolbar");
            this.ResourceDataFilesToolbar.Name = "ResourceDataFilesToolbar";
            this.ResourceDataFilesToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddFileButton
            // 
            resources.ApplyResources(this.AddFileButton, "AddFileButton");
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // DeleteFileButton
            // 
            resources.ApplyResources(this.DeleteFileButton, "DeleteFileButton");
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // DownloadFileButton
            // 
            resources.ApplyResources(this.DownloadFileButton, "DownloadFileButton");
            this.DownloadFileButton.Name = "DownloadFileButton";
            this.DownloadFileButton.Click += new System.EventHandler(this.DownloadFileButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ToggleDocumentsButton
            // 
            resources.ApplyResources(this.ToggleDocumentsButton, "ToggleDocumentsButton");
            this.ToggleDocumentsButton.Name = "ToggleDocumentsButton";
            this.ToggleDocumentsButton.Click += new System.EventHandler(this.ToggleDocumentsButton_Click);
            // 
            // ResourceDataEditor
            // 
            this.Controls.Add(this.ResourceDataFiles);
            this.Controls.Add(this.ResourceDataFilesToolbar);
            this.Name = "ResourceDataEditor";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.ResourceDataEditor_Load);
            this.ResourceDataFilesToolbar.ResumeLayout(false);
            this.ResourceDataFilesToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OSGeo.MapGuide.MaestroAPI.ResourceDataList m_resourceFiles;
		private string m_resource;
		private EditorInterface m_editor;

		private void ResourceDataFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DeleteFileButton.Enabled = DownloadFileButton.Enabled = (ResourceDataFiles.SelectedItems.Count > 0);
		}

		private void RefreshFileList()
		{
			if (m_resourceExists && m_resource != null)
				m_resourceFiles = m_editor.CurrentConnection.EnumerateResourceData(m_resource);
		}

		private void UpdateDisplay()
		{
			try
			{
				ResourceDataFiles.BeginUpdate();
				ResourceDataFiles.Items.Clear();

				if (m_resourceExists && m_resource != null)
				{
					if (m_resourceFiles == null)
						RefreshFileList();

					if (ResourceDataFiles.SmallImageList == null)
						ResourceDataFiles.SmallImageList = ShellIcons.ImageList;

					foreach(OSGeo.MapGuide.MaestroAPI.ResourceDataListResourceData d in m_resourceFiles.ResourceData)
					{
						if (d.Type == OSGeo.MapGuide.MaestroAPI.ResourceDataType.File || ToggleDocumentsButton.Checked)
						{
							ListViewItem lvi = new ListViewItem(new string[] {d.Name, d.Type.ToString()}, ShellIcons.GetShellIcon(d.Name));
							if (d.Type != OSGeo.MapGuide.MaestroAPI.ResourceDataType.File)
								lvi.Font = new Font(ResourceDataFiles.Font, FontStyle.Italic);
                            lvi.Tag = d;
							ResourceDataFiles.Items.Add(lvi);
						}
					}
				}
			}
			finally
			{
				ResourceDataFiles.EndUpdate();
			}
			this.Enabled = m_resourceExists;
			
		}

		public static bool AddFilesToResource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, NameValueCollection filetypes)
		{
			return AddFilesToResource(owner, connection, resourceId, null, filetypes);
		}

		public static bool AddFilesToReource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId)
		{
			return AddFilesToResource(owner, connection, resourceId, null, null);
		}

		public static bool AddFilesToResource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, OSGeo.MapGuide.MaestroAPI.ResourceDataList rld)
		{
			return AddFilesToResource(owner, connection, resourceId, rld, null);
		}

		public static bool AddFilesToResource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, OSGeo.MapGuide.MaestroAPI.ResourceDataList rld, NameValueCollection filetypes)
		{
			bool res = false;
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = Strings.ResourceDataEditor.UploadFilesDialogTitle;
			dlg.Multiselect = true;
			if (filetypes != null)
			{
				string ft = "";
				foreach(string s in filetypes.Keys)
					ft += (ft.Length == 0 ? "" : "|") + filetypes[s] + "|*" + s;
				dlg.Filter = ft;
			}

			if (dlg.ShowDialog(owner) == DialogResult.OK)
			{
                //Find files with same name, but other extensions
                List<string> basenames = new List<string>();
                List<string> extraFiles = new List<string>();

                foreach (string s in dlg.FileNames)
                {
                    string basename = System.IO.Path.GetFileNameWithoutExtension(s);
                    bool caseSensitiveFS = System.Environment.OSVersion.Platform == PlatformID.MacOSX || System.Environment.OSVersion.Platform == PlatformID.Unix;

                    if (!caseSensitiveFS)
                        basename = basename.ToLower();

                    //Do not add duplicates
                    if (basenames.Contains(basename))
                        continue;

                    basenames.Add(basename);

                    foreach (string s1 in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(s), basename + ".*"))
                        if (s1 != s && (string.Compare(System.IO.Path.GetFileNameWithoutExtension(s1), basename, !caseSensitiveFS) == 0))
                            extraFiles.Add(s1);
                }


                if (extraFiles.Count > 0)
                {
                    switch(MessageBox.Show(owner, String.Format(Strings.ResourceDataEditor.IncludeSimilarFilesConfirmation, extraFiles.Count), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Cancel:
                            return false;
                        case DialogResult.No:
                            extraFiles.Clear();
                            break;
                    }
                }

				if (rld == null)
					rld = connection.EnumerateResourceData(resourceId);

                List<string> actualFiles = new List<string>();
                actualFiles.AddRange(dlg.FileNames);
                actualFiles.AddRange(extraFiles);

                WaitForOperation wdlg = new WaitForOperation();
                wdlg.CancelAbortsThread = false;
                Form ownerForm = owner == null ? null : owner.TopLevelControl as Form;

                try
                {
                    res = (bool)wdlg.RunOperationAsync(ownerForm, new WaitForOperation.DoBackgroundWork(Background_Upload), actualFiles, rld, owner, connection, resourceId);
                }
                catch (CancelException)
                {
                    return true;
                }

			}
			return res;
		}

        private static object Background_Upload(BackgroundWorker worker, DoWorkEventArgs args, params object[] target)
        {
            bool res = false;

            List<string> actualFiles = (List<string>)target[0];
            MaestroAPI.ResourceDataList rld = (MaestroAPI.ResourceDataList)target[1];
            Control owner = (Control)target[2];
            MaestroAPI.HttpServerConnection connection = (MaestroAPI.HttpServerConnection)target[3];
            string resourceId = (string)target[4];

            if (owner != null && owner.InvokeRequired)
                owner = null;

            int i = 0;

            foreach (string s in actualFiles)
            {
                bool retry = true;
                while (retry)
                    try
                    {
                        string filename = System.IO.Path.GetFileName(s);
                        worker.ReportProgress((int)((i / (double)actualFiles.Count) * 100), filename);

                        if (worker.CancellationPending)
                        {
                            args.Cancel = true;
                            return res;
                        }

                        bool removeFirst = false;
                        retry = false;
                        bool upload = true;
                        foreach (OSGeo.MapGuide.MaestroAPI.ResourceDataListResourceData rd in rld.ResourceData)
                            if (rd.Name == filename)
                            {
                                removeFirst = true;

                                switch (MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.OverwriteResourceFileConfirmation, filename), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                                {
                                    case DialogResult.No:
                                        upload = false;
                                        break;
                                    case DialogResult.Cancel:
                                        return res;
                                }
                                break;
                            }

                        if (!upload)
                            continue;

                        res = true;

                        if (worker.CancellationPending)
                        {
                            args.Cancel = true;
                            return res;
                        }

                        if (removeFirst)
                            connection.DeleteResourceData(resourceId, filename);

                        using (System.IO.FileStream fs = System.IO.File.Open(s, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            connection.SetResourceData(resourceId, filename, OSGeo.MapGuide.MaestroAPI.ResourceDataType.File, fs);
                    }
                    catch (Exception ex)
                    {
                        if (worker.CancellationPending)
                        {
                            args.Cancel = true;
                            return res;
                        }

                        switch (MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.UploadFailedQuestion, s, ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
                        {
                            case DialogResult.Abort:
                                return res;
                            case DialogResult.Retry:
                                retry = true;
                                continue;
                        }
                    }
                i++;
                worker.ReportProgress((int)((i / (double)actualFiles.Count) * 100));
            }

            return res;
        }

		public static bool DeleteFilesFromResource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, ListView lv)
		{
			if (lv.SelectedItems.Count == 0)
				return false;
			
			string[] items = new string[lv.SelectedItems.Count];
			for(int i = 0; i < lv.SelectedItems.Count; i++)
				items[i] = lv.SelectedItems[i].Text;

			return DeleteFilesFromResource(owner, connection, resourceId, items);
		}

		public static bool DeleteFilesFromResource(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, string[] resourcenames)
		{
			bool res = false;
			if (MessageBox.Show(owner, Strings.ResourceDataEditor.DeleteFileConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				foreach(string resourcename in resourcenames)
				{
					bool retry = true;
					while(retry)
						try
						{
							retry = false;
							res = true;
							connection.DeleteResourceData(resourceId, resourcename);
						}
						catch (Exception ex)
						{
							switch(MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.DeleteFailedQuestion, resourcename, ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
							{
								case DialogResult.Abort:
									return res;
								case DialogResult.Retry:
									retry = true;
									continue;
							}
						}
				}
			}

			return res;
		}

		public static bool DownloadResourceFiles(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, ListView lv)
		{
			if (lv.SelectedItems.Count == 0)
				return false;
			
			string[] items = new string[lv.SelectedItems.Count];
			for(int i = 0; i < lv.SelectedItems.Count; i++)
				items[i] = lv.SelectedItems[i].Text;

            return DownloadResourceFiles(owner, connection, resourceId, items);
		}

		public static bool DownloadResourceFiles(Control owner, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string resourceId, string[] items)
		{
			bool res = false;

			if (items == null || items.Length == 0)
				return res;
			else if (items.Length == 1)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Title = Strings.ResourceDataEditor.DownloadFileDialogTitle;
				dlg.ValidateNames = true;
				dlg.OverwritePrompt = true;
				dlg.AddExtension = true;
				dlg.CheckPathExists = true;
				dlg.CreatePrompt = false;
				dlg.DefaultExt = System.IO.Path.GetExtension(items[0]);
				dlg.DereferenceLinks = true;
				dlg.FileName = items[0];
				dlg.Filter = Strings.Common.AllFiles + "|*.*";
				if (dlg.ShowDialog(owner) == DialogResult.OK)
				{
					bool retry = true;
					while(retry)
						try
						{
							retry = false;
							res = true;
							using(System.IO.MemoryStream ms = connection.GetResourceData(resourceId, items[0]))
							using(System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
								ms.WriteTo(fs);
						}
						catch(Exception ex)
						{
							switch(MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.DownloadFailedQuestion, items[0], ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
							{
								case DialogResult.Abort:
									return res;
								case DialogResult.Retry:
									retry = true;
									continue;
							}
						}
				}
			}
			else
			{
				FolderBrowserDialog dlg = new FolderBrowserDialog();
				dlg.Description = Strings.ResourceDataEditor.DownloadFilesDialogTitle;
				dlg.ShowNewFolderButton = true;
				if (dlg.ShowDialog(owner) == DialogResult.OK)
				{
					foreach(string item in items)
					{
						bool retry = true;
						while(retry)
							try
							{
								retry = false;
								bool download = true;
								string targetpath = System.IO.Path.Combine(dlg.SelectedPath, item);
								if (System.IO.File.Exists(targetpath))
									switch(MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.OverwriteLocalFileConfirmation, item), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
									{
										case DialogResult.No:
											download = false;
											break;
										case DialogResult.Cancel:
											return res; 
									}

								if (!download)
									continue;
								
								res = true;
								using(System.IO.MemoryStream ms = connection.GetResourceData(resourceId, item))
								using(System.IO.FileStream fs = new System.IO.FileStream(targetpath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
									ms.WriteTo(fs);
							}
							catch(Exception ex)
							{
								switch(MessageBox.Show(owner, string.Format(Strings.ResourceDataEditor.DownloadFailedQuestion, item, ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
								{
									case DialogResult.Abort:
										return res;
									case DialogResult.Retry:
										retry = true;
										continue;
								}
							}
					}
				}
			}

			return res;
		}

		private void ResourceDataFiles_SizeChanged(object sender, System.EventArgs e)
		{
			ResourceDataFiles.Columns[0].Width = Math.Max(60, ResourceDataFiles.Width - ResourceDataFiles.Columns[1].Width - 20);
		}

		private void ChangeType_Clicked(object sender, System.EventArgs e)
		{
            MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;

			OSGeo.MapGuide.MaestroAPI.ResourceDataType targetType = (OSGeo.MapGuide.MaestroAPI.ResourceDataType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.ResourceDataType), menu.Text);
			foreach(ListViewItem i in ResourceDataFiles.SelectedItems)
			{
				bool retry = true;
				while(retry)
					try
					{
						retry = false;
                        using (System.IO.Stream s = m_editor.CurrentConnection.GetResourceData(m_resource, i.Text))
						{
                            m_editor.CurrentConnection.DeleteResourceData(m_resource, i.Text);
                            m_editor.HasChanged();

                            try
                            {
                                m_editor.CurrentConnection.SetResourceData(m_resource, i.Text, targetType, s);
                            }
                            catch
                            {
                                try 
                                {
                                    //Attempt to recover the file
                                    s.Position = 0;
                                    m_editor.CurrentConnection.SetResourceData(m_resource, i.Text, ((MaestroAPI.ResourceDataListResourceData)i.Tag).Type, s); 
                                }
                                catch { }
                                throw;
                            }
						}
					}
					catch(Exception ex)
					{
						switch(MessageBox.Show(this, string.Format(Strings.ResourceDataEditor.DownloadFailedQuestion, i.Text, ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
						{
							case DialogResult.Abort:
								RefreshFileList();
								UpdateDisplay();
								return;
							case DialogResult.Retry:
								retry = true;
								continue;
						}
					}
			}
			RefreshFileList();
			UpdateDisplay();
		}

		private void ResourceDataEditor_Load(object sender, System.EventArgs e)
		{
			UpdateDisplay();
		}

		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
		
		}

		public bool ResourceExists
		{
			get { return m_resourceExists; }
			set 
			{
				m_resourceExists = value;
				this.Enabled = value;
				UpdateDisplay();
			}
		}

		public string ResourceID
		{
			get { return m_resource; }
			set 
			{
				m_resource = value;
				RefreshFileList();
				UpdateDisplay();
			}
		}

		public OSGeo.MapGuide.MaestroAPI.ServerConnectionI Connection
		{
			get { return m_editor.CurrentConnection; }
		}

        public EditorInterface Editor
        {
            get { return m_editor; }
            set { m_editor = value; }
        }

        private void AddFileButton_Click(object sender, EventArgs e)
        {
            if (AddFilesToResource(this, m_editor.CurrentConnection, m_resource, m_resourceFiles))
            {
                m_editor.HasChanged();
                
                RefreshFileList();
                UpdateDisplay();
            }
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            if (DeleteFilesFromResource(this, m_editor.CurrentConnection, m_resource, ResourceDataFiles))
            {
                m_editor.HasChanged();

                RefreshFileList();
                UpdateDisplay();
            }
        }

        private void DownloadFileButton_Click(object sender, EventArgs e)
        {
            if (DownloadResourceFiles(this, m_editor.CurrentConnection, m_resource, ResourceDataFiles))
            {
                RefreshFileList();
                UpdateDisplay();
            }
        }

        private void ToggleDocumentsButton_Click(object sender, EventArgs e)
        {
            ToggleDocumentsButton.Checked = !ToggleDocumentsButton.Checked;
            UpdateDisplay();
        }

        private void EditResourceXmlMenu_Click(object sender, EventArgs e)
        {
            try
            {
			    if (ResourceDataFiles.SelectedItems.Count != 1)
				    return;

                XmlEditor dlg;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(m_editor.CurrentConnection.GetResourceData(m_resource, ResourceDataFiles.SelectedItems[0].Text), System.Text.Encoding.UTF8, true))
                    dlg = new XmlEditor(sr.ReadToEnd(), m_editor.CurrentConnection);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    using(System.IO.MemoryStream ms = new System.IO.MemoryStream(new System.Text.UTF8Encoding(false).GetBytes(dlg.EditorText)))
                        m_editor.CurrentConnection.SetResourceData(m_resource, ResourceDataFiles.SelectedItems[0].Text, ((MaestroAPI.ResourceDataListResourceData)(ResourceDataFiles.SelectedItems[0].Tag)).Type, ms);
                    
                    m_editor.HasChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Strings.ResourceDataEditor.UpdateXmlDataError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

	}
}
