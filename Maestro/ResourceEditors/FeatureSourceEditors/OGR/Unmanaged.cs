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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for Unmanaged.
	/// </summary>
	public class Unmanaged : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button BrowseFileButton;
		private System.Windows.Forms.TextBox FilepathText;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private bool m_isUpdating = false;

		public Unmanaged()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Unmanaged));
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.FilepathText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BrowseFileButton
            // 
            resources.ApplyResources(this.BrowseFileButton, "BrowseFileButton");
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FilepathText
            // 
            resources.ApplyResources(this.FilepathText, "FilepathText");
            this.FilepathText.Name = "FilepathText";
            this.FilepathText.TextChanged += new System.EventHandler(this.FilepathText_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Unmanaged
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.FilepathText);
            this.Controls.Add(this.label1);
            this.Name = "Unmanaged";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			try
			{
				m_isUpdating = true;
				m_editor = editor;
				m_item = item;

				UpdateDisplay();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_item == null || m_item.Parameter == null)
					return;

				if (m_item.Parameter["DataSource"] != null)
					FilepathText.Text = m_item.Parameter["DataSource"];
				else
					FilepathText.Text = "";
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void BrowseFileButton_Click(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
            nv.Add(".shp", ResourceEditors.Strings.Common.ShapeFiles);
            nv.Add(".mif", ResourceEditors.Strings.Common.MapInfoInterchangeFiles);
            nv.Add(".tab", ResourceEditors.Strings.Common.MapInfoNativeFiles);
            nv.Add(".sqlite", ResourceEditors.Strings.Common.SQLiteFiles);
            nv.Add(".gml", ResourceEditors.Strings.Common.GMLFiles);
            nv.Add(".dgn", ResourceEditors.Strings.Common.DGNFiles);
            nv.Add(".s57", ResourceEditors.Strings.Common.S57Files);
            nv.Add("", ResourceEditors.Strings.Common.AllFiles);
            string f = m_editor.BrowseUnmanagedData(null, nv);
			if (f != null)
				FilepathText.Text = f;
		}

		private void FilepathText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_item.Parameter["DataSource"] =  FilepathText.Text;
			m_editor.HasChanged();
	
		}

	}
}
