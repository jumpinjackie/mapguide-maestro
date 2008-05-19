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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for Grass.
	/// </summary>
	public class Grass : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox Path;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox Mapset;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private System.Windows.Forms.TextBox LocationText;
		private EditorInterface m_editor;

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			m_editor = editor;
			m_item = item;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				m_isUpdating = true;
				string connectionstring = m_item.Parameter["DataSource"];
				if (connectionstring == null)
					connectionstring = "";

				if (!connectionstring.StartsWith("GRASS:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("GRASS:".Length);

				string[] items = connectionstring.Split('/');
				Database.Text = GetDefaultValue(items, 0, "$GISDBASE");
				LocationText.Text = GetDefaultValue(items, 1, "$LOCATION_NAME");
				Mapset.Text = GetDefaultValue(items, 2, "$MAPSET");
				if (items.Length < 3) 
					Path.Text = "vector/map";
				else
					Path.Text = string.Join("/", items, 3, items.Length - 3);

			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void UpdateConnectionString()
		{
			if (m_item == null) 
				return;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("GRASS:");
			sb.Append(Database.Text);
			sb.Append("/");
			sb.Append(LocationText.Text);
			sb.Append("/");
			sb.Append(Mapset.Text);
			sb.Append("/");
			sb.Append(Path.Text);

			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string[] items, int index, string defaultValue)
		{
			if (items.Length > index && items[index].Trim().Length > 0)
				return items[index];
			else
				return defaultValue;
		}

		public Grass()
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
			this.Path = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.Mapset = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.Database = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.LocationText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// Path
			// 
			this.Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Path.Location = new System.Drawing.Point(112, 104);
			this.Path.Name = "Path";
			this.Path.Size = new System.Drawing.Size(144, 20);
			this.Path.TabIndex = 19;
			this.Path.Text = "";
			this.Path.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 18;
			this.label4.Text = "Path";
			// 
			// Mapset
			// 
			this.Mapset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Mapset.Location = new System.Drawing.Point(112, 72);
			this.Mapset.Name = "Mapset";
			this.Mapset.Size = new System.Drawing.Size(144, 20);
			this.Mapset.TabIndex = 17;
			this.Mapset.Text = "";
			this.Mapset.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Mapset";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Location";
			// 
			// Database
			// 
			this.Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Database.Location = new System.Drawing.Point(112, 8);
			this.Database.Name = "Database";
			this.Database.Size = new System.Drawing.Size(144, 20);
			this.Database.TabIndex = 13;
			this.Database.Text = "";
			this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 12;
			this.label1.Text = "Database";
			// 
			// LocationText
			// 
			this.LocationText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LocationText.Location = new System.Drawing.Point(112, 40);
			this.LocationText.Name = "LocationText";
			this.LocationText.Size = new System.Drawing.Size(144, 20);
			this.LocationText.TabIndex = 20;
			this.LocationText.Text = "";
			this.LocationText.TextChanged += new System.EventHandler(this.SomeProperty_Change);
			// 
			// Grass
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(264, 136);
			this.Controls.Add(this.LocationText);
			this.Controls.Add(this.Path);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Mapset);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Database);
			this.Controls.Add(this.label1);
			this.Name = "Grass";
			this.Size = new System.Drawing.Size(264, 136);
			this.ResumeLayout(false);

		}
		#endregion

		private void SomeProperty_Change(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			UpdateConnectionString();
			m_editor.HasChanged();
		
		}
	}
}
