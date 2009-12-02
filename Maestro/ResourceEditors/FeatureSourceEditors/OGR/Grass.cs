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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grass));
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
            resources.ApplyResources(this.Path, "Path");
            this.Path.Name = "Path";
            this.Path.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // Mapset
            // 
            resources.ApplyResources(this.Mapset, "Mapset");
            this.Mapset.Name = "Mapset";
            this.Mapset.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Database
            // 
            resources.ApplyResources(this.Database, "Database");
            this.Database.Name = "Database";
            this.Database.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // LocationText
            // 
            resources.ApplyResources(this.LocationText, "LocationText");
            this.LocationText.Name = "LocationText";
            this.LocationText.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // Grass
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.LocationText);
            this.Controls.Add(this.Path);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Mapset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.label1);
            this.Name = "Grass";
            this.ResumeLayout(false);
            this.PerformLayout();

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
