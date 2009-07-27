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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.Filebased
{
	/// <summary>
	/// Summary description for Unmanaged.
	/// </summary>
	public class Unmanaged : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button BrowseFileButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox FilepathText;

		private ResourceEditors.EditorInterface m_editor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item = null;
		private bool m_isUpdating = false;

        private string m_resourceName;
        private System.Collections.Specialized.NameValueCollection m_fileTypes;

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
			this.label1 = new System.Windows.Forms.Label();
			this.FilepathText = new System.Windows.Forms.TextBox();
			this.BrowseFileButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Folder or file path";
			// 
			// FilepathText
			// 
			this.FilepathText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FilepathText.Location = new System.Drawing.Point(120, 0);
			this.FilepathText.Name = "FilepathText";
			this.FilepathText.Size = new System.Drawing.Size(104, 20);
			this.FilepathText.TabIndex = 1;
			this.FilepathText.Text = "textBox1";
			this.FilepathText.TextChanged += new System.EventHandler(this.FilepathText_TextChanged);
			// 
			// BrowseFileButton
			// 
			this.BrowseFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BrowseFileButton.Location = new System.Drawing.Point(224, 0);
			this.BrowseFileButton.Name = "BrowseFileButton";
			this.BrowseFileButton.Size = new System.Drawing.Size(24, 20);
			this.BrowseFileButton.TabIndex = 2;
			this.BrowseFileButton.Text = "...";
			this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
			// 
			// Unmanaged
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(248, 24);
			this.Controls.Add(this.BrowseFileButton);
			this.Controls.Add(this.FilepathText);
			this.Controls.Add(this.label1);
			this.Name = "Unmanaged";
			this.Size = new System.Drawing.Size(248, 24);
			this.ResumeLayout(false);

		}
		#endregion

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item, string resourceName, System.Collections.Specialized.NameValueCollection fileTypes)
		{
			try
			{
				m_isUpdating = true;
				m_editor = editor;
				m_item = item;

                m_resourceName = resourceName;
                m_fileTypes = fileTypes;

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

				FilepathText.Text = m_item.Parameter[m_resourceName] == null ? "" : m_item.Parameter[m_resourceName];
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void BrowseFileButton_Click(object sender, System.EventArgs e)
		{
			/*System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".sdf", "SDF Files (*.sdf)");
			nv.Add("", "All files (*.*)");*/
			string f = m_editor.BrowseUnmanagedData(null, m_fileTypes);
			if (f != null)
				FilepathText.Text = f;
		}

		private void FilepathText_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_item.Parameter[m_resourceName] = FilepathText.Text;
			m_editor.HasChanged();

		}
	}
}
