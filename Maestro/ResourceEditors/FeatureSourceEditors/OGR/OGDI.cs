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
	/// Summary description for OGDI.
	/// </summary>
	public class OGDI : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Layername;
		private System.Windows.Forms.TextBox Dataset;
		private System.Windows.Forms.TextBox Family;
		private System.Windows.Forms.TextBox Driver;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
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

				if (!connectionstring.StartsWith("gltp:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("gltp:".Length);

				if (connectionstring.IndexOf(":") > 0)
				{
					string items = connectionstring.Substring(0, connectionstring.IndexOf(":"));
					connectionstring = connectionstring.Substring(items.Length + 1);
					if (items.StartsWith("//"))
						items = items.Substring(2);
					string[] parts = items.Split('/');
					Server.Text = GetDefaultValue(parts, 0, "localhost");
					Driver.Text = GetDefaultValue(parts, 1, "");
					Dataset.Text = GetDefaultValue(parts, 2, "");
						
				}
				else
				{
					Server.Text = "";
					Driver.Text = "";
					Dataset.Text = "";
				}

				string[] parts2 = connectionstring.Split(':');
				Layername.Text = GetDefaultValue(parts2, 0, "");
				Family.Text = GetDefaultValue(parts2, 1, "");
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
			sb.Append("gltp:");
			if (Server.Text.Trim().Length > 0)
			{
				sb.Append("//");
				sb.Append(Server.Text);
			}
			sb.Append("/");
			sb.Append(Driver.Text);
			sb.Append("/");
			sb.Append(Dataset.Text);
			sb.Append(":");
			sb.Append(Layername.Text);
			sb.Append(":");
			sb.Append(Family.Text);

			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string[] items, int index, string defaultValue)
		{
			if (items.Length > index && items[index].Trim().Length > 0)
				return items[index];
			else
				return defaultValue;
		}


		public OGDI()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OGDI));
            this.Layername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Dataset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Family = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Driver = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Layername
            // 
            resources.ApplyResources(this.Layername, "Layername");
            this.Layername.Name = "Layername";
            this.Layername.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Dataset
            // 
            resources.ApplyResources(this.Dataset, "Dataset");
            this.Dataset.Name = "Dataset";
            this.Dataset.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // Family
            // 
            resources.ApplyResources(this.Family, "Family");
            this.Family.Name = "Family";
            this.Family.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Driver
            // 
            resources.ApplyResources(this.Driver, "Driver");
            this.Driver.Name = "Driver";
            this.Driver.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Server
            // 
            resources.ApplyResources(this.Server, "Server");
            this.Server.Name = "Server";
            this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // OGDI
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Layername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Dataset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Family);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Driver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.label1);
            this.Name = "OGDI";
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
