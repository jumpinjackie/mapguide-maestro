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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors
{
	/// <summary>
	/// Summary description for ManagedFileControl.
	/// </summary>
	public class ManagedFileControl : System.Windows.Forms.UserControl
	{
		public delegate bool IsDefaultItemDelegate(string filename);
        public delegate void NewDefaultSelectedDelegate(string filename);
        private System.Windows.Forms.ListView Filelist;
		private System.ComponentModel.IContainer components;

		private ResourceEditors.EditorInterface m_editor = null;
		private string m_itemID = null;
		private bool m_isUpdating = false;

        private IsDefaultItemDelegate m_isDefaultCallback = null;
        public ToolStrip toolStrip;
        private ToolStripButton AddButton;
        private ToolStripButton RemoveButton;
        private ToolStripButton DataItemButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton DownloadButton;
		public event NewDefaultSelectedDelegate NewDefaultSelected = null;
		private NameValueCollection m_filetypes = null;

		public ManagedFileControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Filelist.SmallImageList = ShellIcons.ImageList;
		}

		public NameValueCollection FileTypes
		{
			get { return m_filetypes; }
			set { m_filetypes = value; }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagedFileControl));
            this.Filelist = new System.Windows.Forms.ListView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveButton = new System.Windows.Forms.ToolStripButton();
            this.DataItemButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.DownloadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Filelist
            // 
            resources.ApplyResources(this.Filelist, "Filelist");
            this.Filelist.Name = "Filelist";
            this.Filelist.UseCompatibleStateImageBehavior = false;
            this.Filelist.View = System.Windows.Forms.View.List;
            this.Filelist.SelectedIndexChanged += new System.EventHandler(this.Filelist_SelectedIndexChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.RemoveButton,
            this.DataItemButton,
            this.toolStripSeparator1,
            this.DownloadButton});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddButton
            // 
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.Name = "AddButton";
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveButton, "RemoveButton");
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // DataItemButton
            // 
            this.DataItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DataItemButton, "DataItemButton");
            this.DataItemButton.Name = "DataItemButton";
            this.DataItemButton.Click += new System.EventHandler(this.DataItemButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // DownloadButton
            // 
            this.DownloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.DownloadButton, "DownloadButton");
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // ManagedFileControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Filelist);
            this.Controls.Add(this.toolStrip);
            this.Name = "ManagedFileControl";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		public void SetItem(ResourceEditors.EditorInterface editor, string resourceId, IsDefaultItemDelegate isDefaultCallback)
		{
			try
			{
				m_isUpdating = true;
				m_editor = editor;
				m_itemID = resourceId;
				m_isDefaultCallback = isDefaultCallback;

				UpdateDisplay();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public void UpdateDisplay()
		{
			Filelist.Items.Clear();

			OSGeo.MapGuide.MaestroAPI.ResourceDataList lst = m_editor.CurrentConnection.EnumerateResourceData(m_itemID);
			foreach(OSGeo.MapGuide.MaestroAPI.ResourceDataListResourceData r in lst.ResourceData)
				if (r.Type == OSGeo.MapGuide.MaestroAPI.ResourceDataType.File)
				{
					ListViewItem lvi = new ListViewItem(r.Name);
					lvi.ImageIndex = ShellIcons.GetShellIcon(r.Name);

					if (m_isDefaultCallback != null && m_isDefaultCallback(r.Name))
						lvi.Font = new Font(lvi.Font, FontStyle.Bold);

					Filelist.Items.Add(lvi);
				}
		}


		private void Filelist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DownloadButton.Enabled = RemoveButton.Enabled = Filelist.SelectedItems.Count >= 1;
			DataItemButton.Enabled = Filelist.SelectedItems.Count == 1;
		}

		private void SelectDataResource()
		{
			bool found = false;
			foreach(ListViewItem lvi in Filelist.Items)
				found |= lvi.Font.Bold;

			if (!found && Filelist.Items.Count >= 1)
			{
				ListViewItem lvi = Filelist.Items[0];
				lvi.Font = new Font(lvi.Font, FontStyle.Bold);
				if (NewDefaultSelected != null)
					NewDefaultSelected(Filelist.Items[0].Text);
				m_editor.HasChanged();
			}
		}

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (ResourceDataEditor.AddFilesToResource(this, m_editor.CurrentConnection, m_itemID, m_filetypes))
            {
                UpdateDisplay();
                SelectDataResource();
                m_editor.HasChanged();
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (ResourceDataEditor.DeleteFilesFromResource(this, m_editor.CurrentConnection, m_itemID, Filelist))
            {
                UpdateDisplay();
                SelectDataResource();
                m_editor.HasChanged();
                Filelist_SelectedIndexChanged(sender, null);
            }
        }

        private void DataItemButton_Click(object sender, EventArgs e)
        {
            if (Filelist.SelectedItems.Count == 1)
            {
                if (NewDefaultSelected != null)
                    NewDefaultSelected(Filelist.SelectedItems[0].Text);
                UpdateDisplay();
                m_editor.HasChanged();
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            ResourceDataEditor.DownloadResourceFiles(this, m_editor.CurrentConnection, m_itemID, Filelist);
        }
	}
}
