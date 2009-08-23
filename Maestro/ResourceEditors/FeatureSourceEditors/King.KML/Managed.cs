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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.KingKML
{
	/// <summary>
	/// Summary description for Managed.
	/// </summary>
	public class Managed : System.Windows.Forms.UserControl
	{
        private ResourceEditors.FeatureSourceEditors.ManagedFileControl managedFileControl;
		private System.ComponentModel.IContainer components;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
        private EditorInterface m_editor;
		private System.Windows.Forms.ImageList ToolbarImages;

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				m_isUpdating = true;
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
			if (m_item.Parameter == null || m_item.Parameter["File"] == null || m_item.Parameter["File"] == "%MG_DATA_FILE_PATH%")
				return false;

			return m_item.Parameter["File"].ToLower().IndexOf(filename.ToLower()) >= 0;
		}

		public Managed()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			managedFileControl.NewDefaultSelected += new ResourceEditors.FeatureSourceEditors.ManagedFileControl.NewDefaultSelectedDelegate(managedFileControl_NewDefaultSelected);

			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".kml", "KML file (*.kml)");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Managed));
            this.managedFileControl = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ManagedFileControl();
            this.ToolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // managedFileControl
            // 
            this.managedFileControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.managedFileControl.AutoScroll = true;
            this.managedFileControl.AutoScrollMinSize = new System.Drawing.Size(136, 56);
            this.managedFileControl.FileTypes = null;
            this.managedFileControl.Location = new System.Drawing.Point(8, 8);
            this.managedFileControl.Name = "managedFileControl";
            this.managedFileControl.Size = new System.Drawing.Size(240, 120);
            this.managedFileControl.TabIndex = 0;
            // 
            // ToolbarImages
            // 
            this.ToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolbarImages.ImageStream")));
            this.ToolbarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ToolbarImages.Images.SetKeyName(0, "FavoriteFolder.ico");
            // 
            // Managed
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(256, 136);
            this.Controls.Add(this.managedFileControl);
            this.Name = "Managed";
            this.Size = new System.Drawing.Size(256, 136);
            this.ResumeLayout(false);

		}
		#endregion


		private void FileToolbar_ButtonClick(object sender, EventArgs e)
		{
			m_item.Parameter["File"] = "%MG_DATA_FILE_PATH%";
			m_editor.HasChanged();
			managedFileControl.UpdateDisplay();
		}

		private void managedFileControl_NewDefaultSelected(string filename)
		{
			m_item.Parameter["File"] = "%MG_DATA_FILE_PATH%" + filename;
		}
	}
}
