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
	/// Summary description for PostGIS.
	/// </summary>
	public class PostGIS : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox Database;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Port;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Server;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Options;
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

				if (!connectionstring.StartsWith("PG:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("PG:".Length);

				while(connectionstring.IndexOf("  ") >= 0)
					connectionstring = connectionstring.Replace("  ", " ");
                				
				NameValueCollection items = FeatureSourceEditors.ODBC.ConnectionStringManager.SplitConnectionString(connectionstring, ' ');
				Server.Text = GetDefaultValue(items["host"], "localhost");
				Port.Text = GetDefaultValue(items["port"], "5432");
				m_username = GetDefaultValue(items["user"], "");
				m_password = GetDefaultValue(items["password"], "");
				Database.Text = GetDefaultValue(items["dbname"], "");
				Options.Text = GetDefaultValue(items["options"], "");
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
	
			if (Server.Text.Trim().Length > 0)
			{
				sb.Append(" host=");
				sb.Append(Server.Text);
			}

			if (Database.Text.Trim().Length > 0)
			{
				sb.Append(" dbname=");
				sb.Append(Database.Text);
			}

			if (m_username != null && m_username.Trim().Length > 0)
			{
				sb.Append(" user=");
				sb.Append(m_username);
			}

			if (m_password != null && m_password.Trim().Length > 0)
			{
				sb.Append(" password=");
				sb.Append(m_password);
			}

			if (Port.Text.Trim().Length > 0)
			{
				sb.Append(" port=");
				sb.Append(Port.Text);
			}

			if (Options.Text.Trim().Length > 0)
			{
				sb.Append(" options=");
				sb.Append(Options.Text);
			}

			m_item.Parameter["DataSource"] = "PG:" + sb.ToString().Trim();
		}

		private string GetDefaultValue(string item, string defaultValue)
		{
			if (item != null && item.Trim().Length > 0)
				return item;
			else
				return defaultValue;
		}

		public PostGIS()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostGIS));
            this.Database = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Options = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.SuspendLayout();
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
            // Port
            // 
            resources.ApplyResources(this.Port, "Port");
            this.Port.Name = "Port";
            this.Port.TextChanged += new System.EventHandler(this.SomeProperty_Change);
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
            // Options
            // 
            resources.ApplyResources(this.Options, "Options");
            this.Options.Name = "Options";
            this.Options.TextChanged += new System.EventHandler(this.SomeProperty_Change);
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
            // PostGIS
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.credentials);
            this.Controls.Add(this.Options);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.label1);
            this.Name = "PostGIS";
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
