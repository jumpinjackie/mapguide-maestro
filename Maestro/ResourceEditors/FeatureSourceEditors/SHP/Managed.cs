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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.SHP
{
	/// <summary>
	/// Summary description for Managed.
	/// </summary>
	public class Managed : System.Windows.Forms.UserControl
	{
		private ResourceEditors.FeatureSourceEditors.ManagedFileControl managedFileControl;
		private System.Windows.Forms.Button BrowseTempPath;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.IContainer components;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private EditorInterface m_editor;

		private System.Windows.Forms.TextBox TempPath;
		private System.Windows.Forms.ImageList ToolbarImages;

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				m_isUpdating = true;
				TempPath.Text = m_item.Parameter["TemporaryFileLocation"];
				managedFileControl.UpdateDisplay(); 
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			m_item = item;
			m_editor = editor;
			managedFileControl.SetItem(editor, item.ResourceId, new ManagedFileControl.IsDefaultItemDelegate(IsDefaultItem));
			UpdateDisplay();
		}

		private bool IsDefaultItem(string filename)
		{
			if (m_item.Parameter == null || m_item.Parameter["DefaultFileLocation"] == null || m_item.Parameter["DefaultFileLocation"] == "%MG_DATA_FILE_PATH%")
				return false;

			return m_item.Parameter["DefaultFileLocation"].ToLower().IndexOf(filename.ToLower()) >= 0;
		}

		public Managed()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            managedFileControl.toolStrip.Items.Insert(3, new ToolStripButton("", ToolbarImages.Images[0], new EventHandler(FileToolbar_ButtonClick)));
            managedFileControl.toolStrip.Items[3].ToolTipText = "Use all files in the feature source as data source";

			managedFileControl.NewDefaultSelected += new ResourceEditors.FeatureSourceEditors.ManagedFileControl.NewDefaultSelectedDelegate(managedFileControl_NewDefaultSelected);

			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".shp", "Shape file (*.shp)");
			nv.Add("", "All files (*.*)");
			managedFileControl.FileTypes = nv;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Managed));
			this.managedFileControl = new ResourceEditors.FeatureSourceEditors.ManagedFileControl();
			this.BrowseTempPath = new System.Windows.Forms.Button();
			this.TempPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// managedFileControl
			// 
			this.managedFileControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.managedFileControl.FileTypes = null;
			this.managedFileControl.Location = new System.Drawing.Point(8, 8);
			this.managedFileControl.Name = "managedFileControl";
			this.managedFileControl.Size = new System.Drawing.Size(240, 88);
			this.managedFileControl.TabIndex = 0;
			// 
			// BrowseTempPath
			// 
			this.BrowseTempPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseTempPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BrowseTempPath.Location = new System.Drawing.Point(224, 104);
			this.BrowseTempPath.Name = "BrowseTempPath";
			this.BrowseTempPath.Size = new System.Drawing.Size(24, 20);
			this.BrowseTempPath.TabIndex = 51;
			this.BrowseTempPath.Text = "...";
			this.BrowseTempPath.Click += new System.EventHandler(this.BrowseTempPath_Click);
			// 
			// TempPath
			// 
			this.TempPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TempPath.Location = new System.Drawing.Point(152, 104);
			this.TempPath.Name = "TempPath";
			this.TempPath.Size = new System.Drawing.Size(72, 20);
			this.TempPath.TabIndex = 50;
			this.TempPath.Text = "";
			this.TempPath.TextChanged += new System.EventHandler(this.TempPath_TextChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(8, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 49;
			this.label2.Text = "Temp path";
			// 
			// ToolbarImages
			// 
			this.ToolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ToolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.ToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolbarImages.ImageStream")));
			this.ToolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Managed
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(256, 136);
			this.Controls.Add(this.BrowseTempPath);
			this.Controls.Add(this.TempPath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.managedFileControl);
			this.Name = "Managed";
			this.Size = new System.Drawing.Size(256, 136);
			this.ResumeLayout(false);

		}
		#endregion

		private void TempPath_TextChanged(object sender, System.EventArgs e)
		{
			if (m_item == null || m_isUpdating)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_item.Parameter["TemporaryFileLocation"] = TempPath.Text;
			m_editor.HasChanged();
		}

		private void BrowseTempPath_Click(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add("", "All files (*.*)");
			string f = m_editor.BrowseUnmanagedData(null, nv);
			if (f != null)
				TempPath.Text = f;
		}

		private void FileToolbar_ButtonClick(object sender, EventArgs e)
		{
			m_item.Parameter["DefaultFileLocation"] = "%MG_DATA_FILE_PATH%";
			m_editor.HasChanged();
			managedFileControl.UpdateDisplay();
		}

		private void managedFileControl_NewDefaultSelected(string filename)
		{
			m_item.Parameter["DefaultFileLocation"] = "%MG_DATA_FILE_PATH%" + filename;
		}
	}
}
