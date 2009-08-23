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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC
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
		public event FeatureSourceEditorODBC.ConnectionStringUpdatedDelegate ConnectionStringUpdated;

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

				NameValueCollection nv = ConnectionStringManager.SplitConnectionString(m_item.Parameter["ConnectionString"]);
				if (nv["Dbq"] != null)
					FilepathText.Text = nv["Dbq"];
				else
					FilepathText.Text = "";
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BrowseFileButton = new System.Windows.Forms.Button();
			this.FilepathText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BrowseFileButton
			// 
			this.BrowseFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BrowseFileButton.Location = new System.Drawing.Point(200, 0);
			this.BrowseFileButton.Name = "BrowseFileButton";
			this.BrowseFileButton.Size = new System.Drawing.Size(24, 20);
			this.BrowseFileButton.TabIndex = 5;
			this.BrowseFileButton.Text = "...";
			this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
			// 
			// FilepathText
			// 
			this.FilepathText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FilepathText.Location = new System.Drawing.Point(120, 0);
			this.FilepathText.Name = "FilepathText";
			this.FilepathText.Size = new System.Drawing.Size(80, 20);
			this.FilepathText.TabIndex = 4;
			this.FilepathText.Text = "textBox1";
			this.FilepathText.TextChanged += new System.EventHandler(this.FilepathText_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Folder or file path";
			// 
			// Unmanaged
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(224, 24);
			this.Controls.Add(this.BrowseFileButton);
			this.Controls.Add(this.FilepathText);
			this.Controls.Add(this.label1);
			this.Name = "Unmanaged";
			this.Size = new System.Drawing.Size(224, 24);
			this.ResumeLayout(false);

		}
		#endregion

		private void BrowseFileButton_Click(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".mdb", "Access database (*.mdb)");
			nv.Add(".asc", "ASCII file (*.asc)");
			nv.Add(".csv", "Comma separated values (*.csv)");
			nv.Add(".tab", "Tabulator seperated values (*.tab)");
			nv.Add(".txt", "Text file (*.txt)");
			nv.Add(".sqlite", "SQLite database (*.sqlite)");
			nv.Add(".db", "SQLite database (*.db)");
			nv.Add(".fdb", "Firebird database (*.fdb)");
			nv.Add(".xls", "Excel spreadsheet (*.xls)");
			nv.Add(".dbf", "dBase database (*.dbf)");
			nv.Add("", "All files (*.*)");
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

			m_item.Parameter["ConnectionString"] = ConnectionStringManager.BuildConnectionString(m_item, FilepathText.Text);
			m_item.Parameter["DataSourceName"] = "";
			if (ConnectionStringUpdated != null)
				ConnectionStringUpdated(m_item.Parameter["ConnectionString"]);
			m_editor.HasChanged();
		}
	}
}
