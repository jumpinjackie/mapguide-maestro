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
	/// Summary description for ArcSDE.
	/// </summary>
	public class ArcSDE : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.TextBox Instance;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Layer;
		private System.Windows.Forms.Label label6;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private EditorInterface m_editor;

		private string m_username;
		private string m_password;

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

				if (!connectionstring.StartsWith("SDE:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("SDE:".Length);

				string[] items = connectionstring.Split(',');
				Server.Text = GetDefaultValue(items, 0, "localhost");
				Instance.Text = GetDefaultValue(items, 1, "");
				Database.Text = GetDefaultValue(items, 2, "");
				m_username = GetDefaultValue(items, 3, "");
				m_password = GetDefaultValue(items, 4, "");
				Layer.Text = GetDefaultValue(items, 5, "");
				credentials.SetCredentials(m_username, m_password);
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
			sb.Append("SDE:");
			sb.Append(Server.Text);
			sb.Append(",");
			sb.Append(Instance.Text);
			sb.Append(",");
			sb.Append(Database.Text);
			sb.Append(",");
			sb.Append(m_username);
			sb.Append(",");
			sb.Append(m_password);
			if (Layer.Text.Length > 0)
			{
				sb.Append(",");
				sb.Append(Instance.Text);
			}
			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string[] items, int index, string defaultValue)
		{
			if (items.Length > index && items[index].Trim().Length > 0)
				return items[index];
			else
				return defaultValue;
		}



		public ArcSDE()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArcSDE));
            this.label1 = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.TextBox();
            this.Instance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Database = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Layer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Server
            // 
            resources.ApplyResources(this.Server, "Server");
            this.Server.Name = "Server";
            this.Server.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // Instance
            // 
            resources.ApplyResources(this.Instance, "Instance");
            this.Instance.Name = "Instance";
            this.Instance.TextChanged += new System.EventHandler(this.SomeProperty_Change);
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Layer
            // 
            resources.ApplyResources(this.Layer, "Layer");
            this.Layer.Name = "Layer";
            this.Layer.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // credentials
            // 
            resources.ApplyResources(this.credentials, "credentials");
            this.credentials.Name = "credentials";
            this.credentials.CredentialsChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
            // 
            // ArcSDE
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.credentials);
            this.Controls.Add(this.Layer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Instance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.label1);
            this.Name = "ArcSDE";
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

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_isUpdating || m_item == null)
				return;

			m_username = username;
			m_password = password;

			SomeProperty_Change(null, null);
		}
	}
}
